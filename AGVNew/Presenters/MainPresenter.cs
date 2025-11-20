using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using AGVNew.Models;
using AGVNew.Services;
using AGVNew.Views;
using System.Windows.Forms; // THÊM DÒNG NÀY ĐỂ DÙNG Control

namespace AGVNew.Presenters
{
    public class MainPresenter : IDisposable
    {
        private readonly AGVData _model;
        private readonly MainForm _view;
        private readonly ManagerHTTP _httpService;
        private readonly MitsubishiPLC _plcService;
        private readonly MockPLCService _mockService;

        private CancellationTokenSource _cts;
        private Task _uiUpdateTask;
        private Task _plcMonitorTask;

        public MainPresenter(AGVData model, MainForm view)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _httpService = ManagerHTTP.Instance;
            _plcService = MitsubishiPLC.Instance;
            _mockService = MockPLCService.Instance;

            ManagerLog.Instance.View = _view;
            ManagerLog.Instance.AddLog("System", "Presenter", "MainPresenter initialized");

            _view.UpdateAgvInfo(_model);

            _cts = new CancellationTokenSource();
            _uiUpdateTask = Task.Run(() => UiUpdateLoop(_cts.Token));
            _plcMonitorTask = Task.Run(() => PlcMonitorLoop(_cts.Token));
        }

        private async void UiUpdateLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (_view.IsDisposed || _view.Disposing) break;

                    // CHỈ ĐỌC PLC KHI KHÔNG MOCK
                    if (!_mockService.IsRunning && _plcService.IsConnected)
                    {
                        MitsubishiPLC.Instance.UpdateStateFromPLC();
                    }

                    if (_view.InvokeRequired)
                    {
                        _view.BeginInvoke(new Action(UpdateLabels));
                    }
                    else
                    {
                        UpdateLabels();
                    }
                }
                catch (Exception ex)
                {
                    ManagerLog.Instance.AddLog("System", "UI", "Update error: " + ex.Message);
                }

                await Task.Delay(500, token).ConfigureAwait(false);
            }
        }

        private async void PlcMonitorLoop(CancellationToken token)
        {
            bool wasConnected = false;
			bool hasStartedHttp = false;
			while (!token.IsCancellationRequested)
            {
                 bool isConnected = _plcService.IsConnected;
               // bool isConnected = true;
                if (!isConnected && wasConnected)
                {
                    ManagerLog.Instance.AddLog("System", "PLC", "Connection lost. Reconnecting...");
					_httpService.StopThread();
					// _mockService.StartMock();
				}
                else if (isConnected && !wasConnected)
                {
                    ManagerLog.Instance.AddLog("System", "PLC", "Reconnected. Starting HTTP...");
                    _httpService.Http_Thread_Start();
                }

                if (!isConnected)
                {
                    // await Task.Run(() => _plcService.Connect(_model.option.PLC_LogicalStationNumber));
                }

                wasConnected = isConnected;
                await Task.Delay(2000, token).ConfigureAwait(false);
            }
        }

        private void UpdateLabels()
        {
            var plcState = _model.state;
            if (plcState == null) return;

            try
            {
                _view.LblAgvIdValue.Text = _model.option.AGV_ID;
                _view.LblOpStatusValue.Text = plcState.action_0 ? "Working" : "None";
                _view.LblRfidValue.Text = string.IsNullOrEmpty(plcState.tag_id) ? "N/A" : plcState.tag_id;
                _view.LblSpeedValue.Text = _model.GetSpeed();
                _view.LblMovementValue.Text = _model.GetMovement();
                _view.LblLoadStatusValue.Text = _model.GetLoadStatus();
                _view.LblDirectionValue.Text = plcState.direction ? "Backward" : "Forward";

                int battery = Math.Max(0, Math.Min(100, plcState.battery));
                _view.PbBattery.Value = battery;
                _view.LblBatteryValue.Text = $"{battery}%";
                _view.PbBattery.ForeColor = battery > 50 ? Color.Green : battery > 20 ? Color.Yellow : Color.Red;

                // Cập nhật AGV Status (error)
                if (plcState.error == 0)
                {
                    _view.LblSystemStatus.Text = "OK";
                    _view.PnlSystemStatus.BackColor = Color.LimeGreen;
                    _view.LblSystemStatusIcon.Text = "✓";
                    _view.LblErrorDetail.Visible = false;
                }
                else
                {
                    _view.LblSystemStatus.Text = "Error";
                    _view.PnlSystemStatus.BackColor = Color.Red;
                    _view.LblSystemStatusIcon.Text = "✗";
                    _view.LblErrorDetail.Visible = true;
                    string errorMessage;
                    switch (plcState.error)
                    {
                        case 1:
                            errorMessage = "Low Battery";
                            break;
                        case 2:
                            errorMessage = "Motor Failure";
                            break;
                        case 3:
                            errorMessage = "Sensor Error";
                            break;
                        default:
                            errorMessage = $"Unknown Error ({plcState.error})";
                            break;
                    }
                    _view.LblErrorDetail.Text = errorMessage;

                    // Ghi lỗi vào textBoxalarm
                    _view.AppendErrorLog(errorMessage);
                }

                // Cập nhật PLC Status
                _view.PnlPlcStatus.BackColor = _mockService.IsRunning ? Color.Yellow : (_plcService.IsConnected ? Color.LimeGreen : Color.Red);
                _view.LblPlcStatus.Text = _mockService.IsRunning ? "Mock Mode" : (_plcService.IsConnected ? "Connected" : "Disconnected");
                _view.LblPlcIcon.Text = _mockService.IsRunning ? "⚙" : (_plcService.IsConnected ? "✓" : "✗");
                _view.PnlPlcStatus.Invalidate();
                _view.LblPlcStatus.Invalidate();
                _view.LblPlcIcon.Invalidate();

                // Cập nhật Server Status
                _view.PnlServerStatus.BackColor = _httpService.IsServerConnected ? Color.LimeGreen : Color.Red;
                _view.LblServerStatus.Text = _httpService.IsServerConnected ? "Connected" : "Disconnected";
                _view.LblServerIcon.Text = _httpService.IsServerConnected ? "✓" : "✗";
                _view.PnlServerStatus.Invalidate();
                _view.LblServerStatus.Invalidate();
                _view.LblServerIcon.Invalidate();

                // Cập nhật Mode (gộp auto/manual)
                _view.PnlAuto.BackColor = !plcState.mode ? Color.LimeGreen : Color.Gray;
                _view.LblAutoIcon.Text = !plcState.mode ? "↻" : "○";
                _view.PnlManual.BackColor = plcState.mode ? Color.LimeGreen : Color.Gray;
                _view.LblManualIcon.Text = plcState.mode ? "👤" : "○";

                InvalidateControls();
            }
            catch (Exception ex)
            {
                ManagerLog.Instance.AddLog("System", "UI", "UpdateLabels error: " + ex.Message);
            }
        }

        // SỬA: DÙNG Control[] RÕ RÀNG
        private void InvalidateControls()
        {
            Control[] controls = new Control[]
            {
                _view.LblOpStatusValue, _view.LblRfidValue, _view.LblSpeedValue,
                _view.LblMovementValue, _view.LblDirectionValue, _view.LblLoadStatusValue,
                _view.PbBattery, _view.LblBatteryValue,
                _view.PnlSystemStatus, _view.LblSystemStatus, _view.LblSystemStatusIcon, _view.LblErrorDetail,
                _view.PnlPlcStatus, _view.LblPlcStatus, _view.LblPlcIcon,
                _view.PnlServerStatus, _view.LblServerStatus, _view.LblServerIcon,
                _view.PnlAuto, _view.LblAutoIcon, _view.PnlManual, _view.LblManualIcon
            };

            foreach (Control ctrl in controls)
            {
                try { ctrl?.Invalidate(); } catch { }
            }

            try
            {
                _view.Invalidate(true);
                _view.Refresh();
            }
            catch { }
        }

        public void ConnectPLC()
        {
            ManagerLog.Instance.AddLog("System", "Presenter", "Manual PLC reconnect");
            Task.Run(() =>
            {
                bool success = _plcService.Connect(_model.option.PLC_LogicalStationNumber);
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