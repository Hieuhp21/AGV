using System;
using System.Collections.Generic;
using System.Drawing;
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
        private MainPresenter _presenter;
        private TabControl _tabControl;
        private readonly Dictionary<string, AGVControlSet> _agvControls = new Dictionary<string, AGVControlSet>();

        public MainForm()
        {
            InitializeComponent();
            SetupMultiAgvTabs();
        }

        public void SetPresenter(MainPresenter presenter)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            ManagerLog.Instance.AddLog("System", "UI", "MainForm loaded - Multi-AGV mode enabled");
            this.FormClosing += MainForm_FormClosing;
        }

        /// <summary>
        /// Trả về dictionary tất cả AGV control sets
        /// </summary>
        public IReadOnlyDictionary<string, AGVControlSet> AgvControlSets => _agvControls;

        /// <summary>
        /// Tạo TabControl với 1 tab cho mỗi AGV
        /// </summary>
        private void SetupMultiAgvTabs()
        {
            // Tạo TabControl thay thế panel2
            _tabControl = new TabControl();
            _tabControl.Location = panel2.Location;
            _tabControl.Size = panel2.Size;
            _tabControl.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            _tabControl.Dock = DockStyle.None;
            _tabControl.Anchor = AnchorStyles.Top | AnchorStyles.Left;

            // AGV1: dùng controls có sẵn từ Designer
            var tab1 = new TabPage("AGV1");
            tab1.BackColor = panel2.BackColor;
            // Di chuyển tất cả controls từ panel2 vào tab1
            while (panel2.Controls.Count > 0)
            {
                tab1.Controls.Add(panel2.Controls[0]);
            }
            tab1.AutoScroll = true;
            _tabControl.TabPages.Add(tab1);

            // Map AGV1 controls
            var agv1Controls = new AGVControlSet
            {
                AgvKey = "AGV1",
                LblAgvIdValue = lblAgvIdValue,
                LblIpValue = lblIpValue,
                LblOpStatusValue = lblOpStatusValue,
                LblRfidValue = lblRfidValue,
                LblSpeedValue = lblSpeedValue,
                LblMovementValue = lblMovementValue,
                LblDirectionValue = lblDirectionValue,
                LblLoadStatusValue = lblLoadStatusValue,
                PbBattery = pbBattery,
                LblBatteryValue = lblBatteryValue,
                PnlSystemStatus = pnlSystemStatus,
                LblSystemStatus = lblSystemStatus,
                LblSystemStatusIcon = lblSystemStatusIcon,
                LblErrorDetail = lblErrorDetail,
                PnlPlcStatus = pnlPlcStatus,
                LblPlcStatus = lblPlcStatus,
                LblPlcIcon = lblPlcIcon,
                PnlServerStatus = pnlServerStatus,
                LblServerStatus = lblServerStatus,
                LblServerIcon = lblServerIcon,
                PnlAuto = pnlAuto,
                LblAutoIcon = lblAutoIcon,
                PnlManual = pnlManual,
                LblManualIcon = lblManualIcon,
                TextBoxAlarm = textBoxalarm
            };
            _agvControls["AGV1"] = agv1Controls;

            // AGV2: tạo tab mới với clone controls
            var tab2 = new TabPage("AGV2");
            tab2.BackColor = panel2.BackColor;
            tab2.AutoScroll = true;
            var agv2Controls = CreateAgvTabControls(tab2, "AGV2");
            _agvControls["AGV2"] = agv2Controls;
            _tabControl.TabPages.Add(tab2);

            // Thay panel2 bằng tabControl
            this.Controls.Remove(panel2);
            this.Controls.Add(_tabControl);
        }

        /// <summary>
        /// Tạo bộ controls cho 1 AGV tab mới (clone layout từ AGV1)
        /// </summary>
        private AGVControlSet CreateAgvTabControls(TabPage tabPage, string agvKey)
        {
            var controlSet = new AGVControlSet { AgvKey = agvKey };

            // === PLC Connection Card ===
            var pnlPlcCard2 = CreateCard(35, 20, 431, 85);
            var lblPlcTitle2 = CreateHeaderLabel("PLC Connection", 43, 27, "⎕", 13, 25);
            pnlPlcCard2.Controls.Add(lblPlcTitle2.Item1);
            pnlPlcCard2.Controls.Add(lblPlcTitle2.Item2);

            controlSet.PnlPlcStatus = CreateStatusPanel(212, 10, 204, 56, Color.LimeGreen);
            controlSet.LblPlcIcon = CreateIconLabel("✓", 175, 15);
            controlSet.LblPlcStatus = CreateStatusLabel("Connected", 20, 15);
            controlSet.PnlPlcStatus.Controls.Add(controlSet.LblPlcIcon);
            controlSet.PnlPlcStatus.Controls.Add(controlSet.LblPlcStatus);
            pnlPlcCard2.Controls.Add(controlSet.PnlPlcStatus);
            tabPage.Controls.Add(pnlPlcCard2);

            // === Server Connection Card ===
            var pnlServerCard2 = CreateCard(472, 20, 435, 85);
            var lblServerTitle2 = CreateHeaderLabel("Server Connection", 43, 28, "□", 16, 30);
            pnlServerCard2.Controls.Add(lblServerTitle2.Item1);
            pnlServerCard2.Controls.Add(lblServerTitle2.Item2);

            controlSet.PnlServerStatus = CreateStatusPanel(213, 10, 204, 60, Color.Red);
            controlSet.LblServerIcon = CreateIconLabel("×", 175, 16);
            controlSet.LblServerStatus = CreateStatusLabel("Disconnected", 13, 16);
            controlSet.PnlServerStatus.Controls.Add(controlSet.LblServerIcon);
            controlSet.PnlServerStatus.Controls.Add(controlSet.LblServerStatus);
            pnlServerCard2.Controls.Add(controlSet.PnlServerStatus);
            tabPage.Controls.Add(pnlServerCard2);

            // === Mode Card ===
            var pnlModeCard2 = CreateCard(1013, 20, 852, 85);
            var lblModeTitle2 = CreateHeaderLabel("Operation Mode", 76, 30, "⚙", 28, 30);
            pnlModeCard2.Controls.Add(lblModeTitle2.Item1);
            pnlModeCard2.Controls.Add(lblModeTitle2.Item2);

            controlSet.PnlAuto = CreateStatusPanel(268, 10, 233, 60, Color.LimeGreen);
            controlSet.LblAutoIcon = CreateIconLabel("↻", 10, 15);
            var lblAuto2 = CreateStatusLabel("Auto Mode", 50, 15);
            controlSet.PnlAuto.Controls.Add(controlSet.LblAutoIcon);
            controlSet.PnlAuto.Controls.Add(lblAuto2);

            controlSet.PnlManual = CreateStatusPanel(551, 10, 250, 60, Color.Gray);
            controlSet.LblManualIcon = CreateIconLabel("👤", 10, 15);
            var lblManual2 = CreateStatusLabel("Manual Mode", 50, 15);
            controlSet.PnlManual.Controls.Add(controlSet.LblManualIcon);
            controlSet.PnlManual.Controls.Add(lblManual2);

            pnlModeCard2.Controls.Add(controlSet.PnlAuto);
            pnlModeCard2.Controls.Add(controlSet.PnlManual);
            tabPage.Controls.Add(pnlModeCard2);

            // === AGV Info Card ===
            var pnlAgvInfoCard2 = CreateCard(35, 162, 431, 200);
            var lblInfoTitle2 = CreateHeaderLabel("AGV Information", 40, 10, "ⓘ", 10, 10);
            pnlAgvInfoCard2.Controls.Add(lblInfoTitle2.Item1);
            pnlAgvInfoCard2.Controls.Add(lblInfoTitle2.Item2);

            var tlpAgvInfo2 = new TableLayoutPanel();
            tlpAgvInfo2.ColumnCount = 2;
            tlpAgvInfo2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpAgvInfo2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tlpAgvInfo2.RowCount = 2;
            tlpAgvInfo2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpAgvInfo2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tlpAgvInfo2.Location = new Point(10, 60);
            tlpAgvInfo2.Size = new Size(406, 120);

            tlpAgvInfo2.Controls.Add(CreateInfoLabel("AGV ID:"), 0, 0);
            controlSet.LblAgvIdValue = CreateInfoValueLabel(agvKey);
            tlpAgvInfo2.Controls.Add(controlSet.LblAgvIdValue, 1, 0);
            tlpAgvInfo2.Controls.Add(CreateInfoLabel("Current IP:"), 0, 1);
            controlSet.LblIpValue = CreateInfoValueLabel("N/A");
            tlpAgvInfo2.Controls.Add(controlSet.LblIpValue, 1, 1);

            pnlAgvInfoCard2.Controls.Add(tlpAgvInfo2);
            tabPage.Controls.Add(pnlAgvInfoCard2);

            // === Status Card ===
            var pnlStatusCard2 = CreateCard(504, 162, 1021, 418);
            var lblStatusTitle2 = CreateHeaderLabel("Status Information", 40, 10, "∿", 10, 10);
            pnlStatusCard2.Controls.Add(lblStatusTitle2.Item1);
            pnlStatusCard2.Controls.Add(lblStatusTitle2.Item2);

            var tlpStatus2 = new TableLayoutPanel();
            tlpStatus2.ColumnCount = 4;
            for (int i = 0; i < 4; i++) tlpStatus2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tlpStatus2.RowCount = 4;
            for (int i = 0; i < 4; i++) tlpStatus2.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tlpStatus2.Location = new Point(30, 86);
            tlpStatus2.Size = new Size(959, 287);

            tlpStatus2.Controls.Add(CreateInfoLabel("Operation Status"), 0, 0);
            controlSet.LblOpStatusValue = CreateInfoValueLabel("Idle");
            tlpStatus2.Controls.Add(controlSet.LblOpStatusValue, 1, 0);
            tlpStatus2.Controls.Add(CreateInfoLabel("RFID Data"), 2, 0);
            controlSet.LblRfidValue = CreateInfoValueLabel("N/A");
            tlpStatus2.Controls.Add(controlSet.LblRfidValue, 3, 0);

            tlpStatus2.Controls.Add(CreateInfoLabel("Speed"), 0, 1);
            controlSet.LblSpeedValue = CreateInfoValueLabel("Stop");
            tlpStatus2.Controls.Add(controlSet.LblSpeedValue, 1, 1);
            tlpStatus2.Controls.Add(CreateInfoLabel("Movement"), 2, 1);
            controlSet.LblMovementValue = CreateInfoValueLabel("Stop");
            tlpStatus2.Controls.Add(controlSet.LblMovementValue, 3, 1);

            tlpStatus2.Controls.Add(CreateInfoLabel("Direction"), 0, 2);
            controlSet.LblDirectionValue = CreateInfoValueLabel("Forward");
            tlpStatus2.Controls.Add(controlSet.LblDirectionValue, 1, 2);
            tlpStatus2.Controls.Add(CreateInfoLabel("Load Status"), 2, 2);
            controlSet.LblLoadStatusValue = CreateInfoValueLabel("None");
            tlpStatus2.Controls.Add(controlSet.LblLoadStatusValue, 3, 2);

            tlpStatus2.Controls.Add(CreateInfoLabel("Battery Level"), 0, 3);
            controlSet.PbBattery = new ProgressBar();
            controlSet.PbBattery.Size = new Size(200, 30);
            controlSet.PbBattery.Style = ProgressBarStyle.Continuous;
            controlSet.PbBattery.Value = 0;
            tlpStatus2.Controls.Add(controlSet.PbBattery, 1, 3);
            controlSet.LblBatteryValue = CreateInfoValueLabel("0%");
            tlpStatus2.Controls.Add(controlSet.LblBatteryValue, 2, 3);

            pnlStatusCard2.Controls.Add(tlpStatus2);
            tabPage.Controls.Add(pnlStatusCard2);

            // === AGV Status Card ===
            var pnlSystemStatusCard2 = CreateCard(35, 380, 431, 200);
            var lblSystemTitle2 = CreateHeaderLabel("AGV Status", 40, 10, "⚠", 10, 10);
            pnlSystemStatusCard2.Controls.Add(lblSystemTitle2.Item1);
            pnlSystemStatusCard2.Controls.Add(lblSystemTitle2.Item2);

            controlSet.PnlSystemStatus = CreateStatusPanel(10, 60, 406, 60, Color.LimeGreen);
            controlSet.LblSystemStatusIcon = CreateIconLabel("✓", 370, 15);
            controlSet.LblSystemStatus = new Label();
            controlSet.LblSystemStatus.AutoSize = true;
            controlSet.LblSystemStatus.Font = new Font("Microsoft Sans Serif", 20.25F, FontStyle.Bold);
            controlSet.LblSystemStatus.ForeColor = Color.White;
            controlSet.LblSystemStatus.Location = new Point(10, 15);
            controlSet.LblSystemStatus.Text = "OK";
            controlSet.PnlSystemStatus.Controls.Add(controlSet.LblSystemStatusIcon);
            controlSet.PnlSystemStatus.Controls.Add(controlSet.LblSystemStatus);

            controlSet.LblErrorDetail = new Label();
            controlSet.LblErrorDetail.Font = new Font("Microsoft Sans Serif", 12F);
            controlSet.LblErrorDetail.ForeColor = Color.Red;
            controlSet.LblErrorDetail.Location = new Point(10, 130);
            controlSet.LblErrorDetail.Size = new Size(420, 60);
            controlSet.LblErrorDetail.Visible = false;

            pnlSystemStatusCard2.Controls.Add(controlSet.PnlSystemStatus);
            pnlSystemStatusCard2.Controls.Add(controlSet.LblErrorDetail);
            tabPage.Controls.Add(pnlSystemStatusCard2);

            // === Alarm Card ===
            var pnlAlarm2 = CreateCard(1565, 162, 300, 418);
            var lblAlarmTitle2 = new Label();
            lblAlarmTitle2.AutoSize = true;
            lblAlarmTitle2.Font = new Font("Microsoft Sans Serif", 14F);
            lblAlarmTitle2.ForeColor = Color.DimGray;
            lblAlarmTitle2.Location = new Point(13, 10);
            lblAlarmTitle2.Text = "Alarm";
            pnlAlarm2.Controls.Add(lblAlarmTitle2);

            controlSet.TextBoxAlarm = new TextBox();
            controlSet.TextBoxAlarm.Font = new Font("Consolas", 9F);
            controlSet.TextBoxAlarm.Location = new Point(13, 37);
            controlSet.TextBoxAlarm.Multiline = true;
            controlSet.TextBoxAlarm.ScrollBars = ScrollBars.Vertical;
            controlSet.TextBoxAlarm.Size = new Size(286, 380);
            pnlAlarm2.Controls.Add(controlSet.TextBoxAlarm);
            tabPage.Controls.Add(pnlAlarm2);

            return controlSet;
        }

        // === Helper methods để tạo controls ===

        private Panel CreateCard(int x, int y, int w, int h)
        {
            var panel = new Panel();
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.Location = new Point(x, y);
            panel.Size = new Size(w, h);
            panel.Padding = new Padding(10);
            return panel;
        }

        private Panel CreateStatusPanel(int x, int y, int w, int h, Color bgColor)
        {
            var panel = new Panel();
            panel.BackColor = bgColor;
            panel.Location = new Point(x, y);
            panel.Size = new Size(w, h);
            return panel;
        }

        private Label CreateIconLabel(string icon, int x, int y)
        {
            var lbl = new Label();
            lbl.AutoSize = true;
            lbl.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            lbl.ForeColor = Color.White;
            lbl.Location = new Point(x, y);
            lbl.Text = icon;
            return lbl;
        }

        private Label CreateStatusLabel(string text, int x, int y)
        {
            var lbl = new Label();
            lbl.AutoSize = true;
            lbl.Font = new Font("Microsoft Sans Serif", 16F, FontStyle.Bold);
            lbl.ForeColor = Color.White;
            lbl.Location = new Point(x, y);
            lbl.Text = text;
            return lbl;
        }

        private Tuple<Label, Label> CreateHeaderLabel(string title, int titleX, int titleY, string icon, int iconX, int iconY)
        {
            var lblTitle = new Label();
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Microsoft Sans Serif", 14F);
            lblTitle.ForeColor = Color.DimGray;
            lblTitle.Location = new Point(titleX, titleY);
            lblTitle.Text = title;

            var lblIcon = new Label();
            lblIcon.AutoSize = true;
            lblIcon.Font = new Font("Microsoft Sans Serif", 14F);
            lblIcon.ForeColor = Color.DimGray;
            lblIcon.Location = new Point(iconX, iconY);
            lblIcon.Text = icon;

            return Tuple.Create(lblTitle, lblIcon);
        }

        private Label CreateInfoLabel(string text)
        {
            var lbl = new Label();
            lbl.AutoSize = true;
            lbl.Font = new Font("Microsoft Sans Serif", 14F);
            lbl.ForeColor = Color.DimGray;
            lbl.Text = text;
            return lbl;
        }

        private Label CreateInfoValueLabel(string text)
        {
            var lbl = new Label();
            lbl.AutoSize = true;
            lbl.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold);
            lbl.ForeColor = Color.DimGray;
            lbl.Text = text;
            return lbl;
        }

        // === Event Handlers ===

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ManagerLog.Instance.AddLog("System", "UI", "MainForm closing");
            MockPLCService.Instance.StopMock();
            _presenter?.Dispose();
            ManagerLog.Instance.View = null;
        }

        // === Log Methods ===

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

        /// <summary>
        /// Cập nhật AGV Info cho 1 AGVControlSet
        /// </summary>
        public void UpdateAgvInfo(AGVData model, AGVControlSet controls)
        {
            if (IsDisposed) return;

            try
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => UpdateAgvInfoInternal(model, controls)));
                }
                else
                {
                    UpdateAgvInfoInternal(model, controls);
                }
            }
            catch (Exception ex)
            {
                ManagerLog.Instance.AddLog("System", "UI", $"UpdateAgvInfo error: {ex.Message}");
            }
        }

        private void UpdateAgvInfoInternal(AGVData model, AGVControlSet controls)
        {
            if (IsDisposed) return;

            controls.LblAgvIdValue.Text = model?.option?.AGV_ID ?? "N/A";

            try
            {
                string hostName = Dns.GetHostName();
                IPAddress[] addresses = Dns.GetHostAddresses(hostName);
                string ipAddress = addresses
                    .Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    .Select(ip => ip.ToString())
                    .FirstOrDefault() ?? "N/A";
                controls.LblIpValue.Text = ipAddress;
            }
            catch (Exception ex)
            {
                ManagerLog.Instance.AddLog("System", "UI", $"Failed to get local IP: {ex.Message}");
                controls.LblIpValue.Text = "N/A";
            }
        }

        /// <summary>
        /// Backward-compatible: cập nhật cho AGV1
        /// </summary>
        public void UpdateAgvInfo(AGVData model)
        {
            if (_agvControls.ContainsKey("AGV1"))
                UpdateAgvInfo(model, _agvControls["AGV1"]);
        }

        /// <summary>
        /// Append error log vào alarm textbox của 1 AGV
        /// </summary>
        public void AppendErrorLog(string errorLog, AGVControlSet controls)
        {
            if (IsDisposed) return;

            try
            {
                var tb = controls.TextBoxAlarm;
                if (tb == null || tb.IsDisposed) return;

                if (tb.InvokeRequired)
                {
                    tb.BeginInvoke(new Action(() => AppendErrorLogInternal(errorLog, tb)));
                }
                else
                {
                    AppendErrorLogInternal(errorLog, tb);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AppendErrorLog error: {ex.Message}");
            }
        }

        /// <summary>
        /// Backward-compatible
        /// </summary>
        public void AppendErrorLog(string errorLog)
        {
            if (_agvControls.ContainsKey("AGV1"))
                AppendErrorLog(errorLog, _agvControls["AGV1"]);
        }

        private void AppendErrorLogInternal(string errorLog, TextBox textBox)
        {
            if (IsDisposed || textBox.IsDisposed) return;

            const int maxErrorLines = 50;
            if (textBox.Lines.Length >= maxErrorLines)
            {
                var lines = textBox.Lines;
                textBox.Lines = lines.Skip(lines.Length - maxErrorLines + 1).ToArray();
            }

            string logWithTimestamp = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {errorLog}";
            textBox.AppendText(logWithTimestamp + Environment.NewLine);
            textBox.ScrollToCaret();
        }

        // === Backward-compatible public properties cho AGV1 (dùng bởi MainPresenter cũ) ===
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