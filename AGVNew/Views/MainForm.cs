using System;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using AGVNew.Models;
using AGVNew.Presenters;
using AGVNew.Services;

namespace AGVNew.Views
{
    public partial class MainForm : Form
    {
        private readonly MainPresenter _presenter;

        public MainForm()
        {
            InitializeComponent();
        }

        public MainForm(MainPresenter presenter)
        {
            InitializeComponent();
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            ManagerLog.Instance.AddLog("System", "UI", "MainForm loaded - Auto-start mode enabled");
            this.FormClosing += MainForm_FormClosing;

            // Không gọi UpdateAgvInfo ở đây vì đã gọi trong MainPresenter
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ManagerLog.Instance.AddLog("System", "UI", "MainForm closing");
            MockPLCService.Instance.StopMock();
            _presenter?.Dispose();
           // _presenter.StopTimer();
            ManagerLog.Instance.View = null;
        }

        public void AppendLog(string log)
        {
            if (IsDisposed || txtLog.IsDisposed)
            {
                Console.WriteLine("AppendLog: Form or txtLog is disposed");
                return;
            }

            try
            {
                if (txtLog.InvokeRequired)
                {
                    txtLog.BeginInvoke(new Action(() => AppendLogInternal(log)));
                }
                else
                {
                    AppendLogInternal(log);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AppendLog error: {ex.Message}");
            }
        }

        private void AppendLogInternal(string log)
        {
            if (IsDisposed || txtLog.IsDisposed)
            {
                return;
            }

            const int maxLines = 1000;
            if (txtLog.Lines.Length > maxLines)
            {
                var lines = txtLog.Lines;
                txtLog.Lines = lines.Skip(lines.Length - maxLines).ToArray();
            }

            txtLog.AppendText(log + Environment.NewLine);
            txtLog.ScrollToCaret();
        }

        public void UpdateAgvInfo(AGVData model)
        {
            if (IsDisposed || lblAgvIdValue.IsDisposed || lblIpValue.IsDisposed)
            {
                Console.WriteLine("UpdateAgvInfo: Form or labels are disposed");
                return;
            }

            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => UpdateAgvInfoInternal(model)));
                }
                else
                {
                    UpdateAgvInfoInternal(model);
                }
            }
            catch (Exception ex)
            {
                ManagerLog.Instance.AddLog("System", "UI", $"UpdateAgvInfo error: {ex.Message}");
            }
        }

        private void UpdateAgvInfoInternal(AGVData model)
        {
            if (IsDisposed || lblAgvIdValue.IsDisposed || lblIpValue.IsDisposed)
            {
                return;
            }

            // Cập nhật AGV_ID từ AGVData.Option
            lblAgvIdValue.Text = model?.option?.AGV_ID ?? "N/A";

            // Cập nhật IP của máy hiện tại
            try
            {
                string hostName = Dns.GetHostName();
                IPAddress[] addresses = Dns.GetHostAddresses(hostName);
                string ipAddress = addresses
                    .Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(ip => ip.ToString())
                    .FirstOrDefault() ?? "N/A";
                lblIpValue.Text = ipAddress;
            }
            catch (Exception ex)
            {
                ManagerLog.Instance.AddLog("System", "UI", $"Failed to get local IP: {ex.Message}");
                lblIpValue.Text = "N/A";
            }

            lblAgvIdValue.Invalidate();
            lblAgvIdValue.Update();
            lblIpValue.Invalidate();
            lblIpValue.Update();
        }
        public void AppendErrorLog(string errorLog)
        {
            if (IsDisposed || textBoxalarm.IsDisposed)
            {
                Console.WriteLine("AppendErrorLog: Form or textBoxalarm is disposed");
                return;
            }

            try
            {
                if (textBoxalarm.InvokeRequired)
                {
                    textBoxalarm.BeginInvoke(new Action(() => AppendErrorLogInternal(errorLog)));
                }
                else
                {
                    AppendErrorLogInternal(errorLog);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AppendErrorLog error: {ex.Message}");
            }
        }

        private void AppendErrorLogInternal(string errorLog)
        {
            if (IsDisposed || textBoxalarm.IsDisposed)
            {
                return;
            }

            const int maxErrorLines = 50;
            if (textBoxalarm.Lines.Length >= maxErrorLines)
            {
                var lines = textBoxalarm.Lines;
                textBoxalarm.Lines = lines.Skip(lines.Length - maxErrorLines + 1).ToArray();
            }

            // Thêm bản ghi lỗi với timestamp
            string logWithTimestamp = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {errorLog}";
            textBoxalarm.AppendText(logWithTimestamp + Environment.NewLine);
            textBoxalarm.ScrollToCaret();
        }

        // Public properties để MainPresenter truy cập controls
        public Label LblAgvIdValue => lblAgvIdValue;
        public Label LblIpValue => lblIpValue;
        public Label LblOpStatusValue => lblOpStatusValue;
        public Label LblRfidValue => lblRfidValue;
        public Label LblSpeedValue => lblSpeedValue;
        public Label LblMovementValue => lblMovementValue;
        public Label LblDirectionValue => lblDirectionValue;
        public Label LblLoadStatusValue => lblLoadStatusValue;
        public ProgressBar PbBattery => pbBattery;
        public Label LblBatteryValue => lblBatteryValue;
        public Panel PnlSystemStatus => pnlSystemStatus;
        public Label LblSystemStatus => lblSystemStatus;
        public Label LblSystemStatusIcon => lblSystemStatusIcon;
        public Label LblErrorDetail => lblErrorDetail;
        public Panel PnlPlcStatus => pnlPlcStatus;
        public Label LblPlcStatus => lblPlcStatus;
        public Label LblPlcIcon => lblPlcIcon;
        public Panel PnlServerStatus => pnlServerStatus;
        public Label LblServerStatus => lblServerStatus;
        public Label LblServerIcon => lblServerIcon;
        public Panel PnlAuto => pnlAuto;
        public Label LblAutoIcon => lblAutoIcon;
        public Panel PnlManual => pnlManual;
        public Label LblManualIcon => lblManualIcon;
    }
}