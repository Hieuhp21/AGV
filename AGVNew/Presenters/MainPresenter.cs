using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AGVNew.Models;
using AGVNew.Services;
using AGVNew.Views;
using System.Windows.Forms;
using System.Collections.Generic;

namespace AGVNew.Presenters
{
    public class MainPresenter : IDisposable
    {
        private readonly MainForm _view;
        private readonly ManagerHTTP _httpService;
        private readonly MitsubishiPLC _plcService;
        private readonly MockPLCService _mockService;

        private CancellationTokenSource _cts;
        private Task _uiUpdateTask;
        private Task _plcMonitorTask;

        public MainPresenter(MainForm view)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _httpService = ManagerHTTP.Instance;
            _plcService = MitsubishiPLC.Instance;
            _mockService = MockPLCService.Instance;

            ManagerLog.Instance.View = _view;
            ManagerLog.Instance.AddLog("System", "Presenter", "MainPresenter initialized (Multi-AGV mode)");

            // Cập nhật AGV Info cho tất cả tabs
            foreach (var kvp in _view.AgvControlSets)
            {
                string agvKey = kvp.Key;
                AGVControlSet controls = kvp.Value;
                if (AGVData.All.ContainsKey(agvKey))
                {
                    _view.UpdateAgvInfo(AGVData.All[agvKey], controls);
                }
            }

            _cts = new CancellationTokenSource();
            _uiUpdateTask = Task.Run(() => UiUpdateLoop(_cts.Token));

            // Nếu USE_MOCK = true: chạy mock ngay, không cần PLC
            if (Program.USE_MOCK)
            {
                ManagerLog.Instance.AddLog("System", "Presenter", "USE_MOCK=true → Starting Mock mode");
                _mockService.StartMock();
            }
            else
            {
                // Chạy thật: monitor PLC connection
                _plcMonitorTask = Task.Run(() => PlcMonitorLoop(_cts.Token));
            }
        }

        private async void UiUpdateLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        if (_view.IsDisposed || _view.Disposing) break;

                        // CHỈ cập nhật UI — PLC đọc ở HTTP thread hoặc Mock thread
                        // Không đọc PLC ở đây để tránh double-read

                        if (_view.InvokeRequired)
                        {
                            _view.BeginInvoke(new Action(UpdateAllLabels));
                        }
                        else
                        {
                            UpdateAllLabels();
                        }
                    }
                    catch (Exception ex) when (!(ex is OperationCanceledException))
                    {
                        ManagerLog.Instance.AddLog("System", "UI", "Update error: " + ex.Message);
                    }

                    await Task.Delay(500, token).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException) { /* Graceful exit */ }
        }

        private async void PlcMonitorLoop(CancellationToken token)
        {
            try
            {
                bool wasConnected = false;
                bool hasStartedHttp = false;
                string plcIp = AGVData.Instance.option.PLC_IP ?? "192.168.3.39";
                int pingTimeoutMs = 1000;
                int reconnectDelayMs = 2000;
                const int maxDelayMs = 30000;

                DateTime lastSuccessfulPing = DateTime.Now;

                while (!token.IsCancellationRequested)
                {
                    bool isConnected = _plcService.IsConnected;

                    if (!isConnected && wasConnected)
                    {
                        ManagerLog.Instance.AddLog("System", "PLC", "Connection lost");
                        _httpService.StopThread();
                        hasStartedHttp = false;
                        // Không tự bật mock → chỉ hiển Disconnected
                    }

                    if (isConnected && !wasConnected && !hasStartedHttp)
                    {
                        ManagerLog.Instance.AddLog("System", "PLC", "PLC reconnected → Starting HTTP server");
                        _httpService.Http_Thread_Start();
                        hasStartedHttp = true;
                    }

                    if (isConnected)
                    {
                        if (DateTime.Now - lastSuccessfulPing > TimeSpan.FromSeconds(15))
                        {
                            bool pingOk = await PingHostAsync(plcIp, pingTimeoutMs);
                            if (pingOk)
                            {
                                lastSuccessfulPing = DateTime.Now;
                            }
                            else
                            {
                                ManagerLog.Instance.AddLog("System", "PLC", "Ping failed while IsConnected=true → Force disconnect check");
                                _ = Task.Run(() =>
                                {
                                    foreach (var kvp in AGVData.All)
                                    {
                                        MitsubishiPLC.Instance.UpdateStateFromPLC(kvp.Value);
                                    }
                                });
                            }
                        }

                        await Task.Delay(10000, token).ConfigureAwait(false);
                    }
                    else
                    {
                        bool pingSuccess = await PingHostAsync(plcIp, pingTimeoutMs);

                        if (pingSuccess)
                        {
                            ManagerLog.Instance.AddLog("System", "PLC", $"Ping {plcIp} OK → Reconnecting...");
                            _plcService.Connect(AGVData.Instance.option.PLC_LogicalStationNumber);
                            reconnectDelayMs = 2000;
                        }
                        else
                        {
                            reconnectDelayMs = Math.Min(reconnectDelayMs * 2, maxDelayMs);
                            ManagerLog.Instance.AddLog("System", "PLC", $"Ping failed. Next try in {reconnectDelayMs / 1000}s");
                        }

                        await Task.Delay(reconnectDelayMs, token).ConfigureAwait(false);
                    }

                    wasConnected = isConnected;
                }
            }
            catch (OperationCanceledException) { /* Graceful exit */ }
        }

        private static Task<bool> PingHostAsync(string host, int timeoutMs)
        {
            return Task.Run(() =>
            {
                try
                {
                    using (var ping = new System.Net.NetworkInformation.Ping())
                    {
                        var reply = ping.Send(host, timeoutMs);
                        return reply?.Status == System.Net.NetworkInformation.IPStatus.Success;
                    }
                }
                catch
                {
                    return false;
                }
            });
        }

        /// <summary>
        /// Cập nhật labels cho TẤT CẢ AGV tabs
        /// </summary>
        private void UpdateAllLabels()
        {
            foreach (var kvp in _view.AgvControlSets)
            {
                string agvKey = kvp.Key;
                AGVControlSet controls = kvp.Value;

                AGVData agvData;
                if (AGVData.All.ContainsKey(agvKey))
                {
                    agvData = AGVData.All[agvKey];
                }
                else
                {
                    continue;
                }

                UpdateLabelsForAgv(agvData, controls);
            }
        }

        /// <summary>
        /// Cập nhật labels cho 1 AGV cụ thể
        /// </summary>
        private void UpdateLabelsForAgv(AGVData model, AGVControlSet controls)
        {
            var plcState = model.state;
            if (plcState == null) return;

            try
            {
                controls.LblAgvIdValue.Text = model.option.AGV_ID;
                controls.LblOpStatusValue.Text = plcState.action_0 ? "Working" : "None";
                controls.LblRfidValue.Text = string.IsNullOrEmpty(plcState.tag_id) ? "N/A" : plcState.tag_id;
                controls.LblSpeedValue.Text = model.GetSpeed();
                controls.LblMovementValue.Text = model.GetMovement();
                controls.LblLoadStatusValue.Text = model.GetLoadStatus();
                controls.LblDirectionValue.Text = plcState.direction ? "Backward" : "Forward";

                int battery = Math.Max(0, Math.Min(100, plcState.battery));
                controls.PbBattery.Value = battery;
                controls.LblBatteryValue.Text = $"{battery}%";
                controls.PbBattery.ForeColor = battery > 50 ? Color.Green : battery > 20 ? Color.Yellow : Color.Red;

                // AGV Status (error)
                if (plcState.error == 0)
                {
                    controls.LblSystemStatus.Text = "OK";
                    controls.PnlSystemStatus.BackColor = Color.LimeGreen;
                    controls.LblSystemStatusIcon.Text = "✓";
                    controls.LblErrorDetail.Visible = false;
                }
                else
                {
                    controls.LblSystemStatus.Text = "Error";
                    controls.PnlSystemStatus.BackColor = Color.Red;
                    controls.LblSystemStatusIcon.Text = "✗";
                    controls.LblErrorDetail.Visible = true;
                    string errorMessage;
                    switch (plcState.error)
                    {
                        case 1: errorMessage = "Low Battery"; break;
                        case 2: errorMessage = "Motor Failure"; break;
                        case 3: errorMessage = "Sensor Error"; break;
                        default: errorMessage = $"Unknown Error ({plcState.error})"; break;
                    }
                    controls.LblErrorDetail.Text = errorMessage;
                    _view.AppendErrorLog(errorMessage, controls);
                }

                // PLC Status (chung cho tất cả AGV vì chung 1 PLC)
                controls.PnlPlcStatus.BackColor = _mockService.IsRunning ? Color.Yellow : (_plcService.IsConnected ? Color.LimeGreen : Color.Red);
                controls.LblPlcStatus.Text = _mockService.IsRunning ? "Mock Mode" : (_plcService.IsConnected ? "Connected" : "Disconnected");
                controls.LblPlcIcon.Text = _mockService.IsRunning ? "⚙" : (_plcService.IsConnected ? "✓" : "✗");

                // Server Status
                controls.PnlServerStatus.BackColor = _httpService.IsServerConnected ? Color.LimeGreen : Color.Red;
                controls.LblServerStatus.Text = _httpService.IsServerConnected ? "Connected" : "Disconnected";
                controls.LblServerIcon.Text = _httpService.IsServerConnected ? "✓" : "✗";

                // Mode (auto/manual)
                controls.PnlAuto.BackColor = !plcState.mode ? Color.LimeGreen : Color.Gray;
                controls.LblAutoIcon.Text = !plcState.mode ? "↻" : "○";
                controls.PnlManual.BackColor = plcState.mode ? Color.LimeGreen : Color.Gray;
                controls.LblManualIcon.Text = plcState.mode ? "👤" : "○";
            }
            catch (ObjectDisposedException) { /* Form closing */ }
            catch (Exception ex)
            {
                ManagerLog.Instance.AddLog("System", "UI", $"[{controls.AgvKey}] UpdateLabels error: " + ex.Message);
            }
        }

        // InvalidateControls removed — WinForms tự repaint khi Text/BackColor thay đổi
        // Gọi Invalidate(true) + Refresh() mỗi 500ms gây lag nặng khi chạy 24/7

        public void ConnectPLC()
        {
            ManagerLog.Instance.AddLog("System", "Presenter", "Manual PLC reconnect");
            Task.Run(() =>
            {
                bool success = _plcService.Connect(AGVData.Instance.option.PLC_LogicalStationNumber);
                ManagerLog.Instance.AddLog("System", "Presenter", success ? "PLC reconnected" : "Reconnect failed");
            });
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _httpService.StopThread();
            try { _uiUpdateTask?.Wait(1000); } catch { }
            try { _plcMonitorTask?.Wait(1000); } catch { }

            ManagerLog.Instance.AddLog("System", "Presenter", "Disposed");
        }
    }
}