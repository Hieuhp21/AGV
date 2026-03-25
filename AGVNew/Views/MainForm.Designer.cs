using System.Drawing;
using System.Windows.Forms;

namespace AGVNew.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtLog = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.textBoxalarm = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.pnlSystemStatusCard = new System.Windows.Forms.Panel();
            this.lblErrorDetail = new System.Windows.Forms.Label();
            this.pnlSystemStatus = new System.Windows.Forms.Panel();
            this.lblSystemStatusIcon = new System.Windows.Forms.Label();
            this.lblSystemStatus = new System.Windows.Forms.Label();
            this.lblSystemTitle = new System.Windows.Forms.Label();
            this.lblSystemHeaderIcon = new System.Windows.Forms.Label();
            this.pnlPlcCard = new System.Windows.Forms.Panel();
            this.pnlPlcStatus = new System.Windows.Forms.Panel();
            this.lblPlcIcon = new System.Windows.Forms.Label();
            this.lblPlcStatus = new System.Windows.Forms.Label();
            this.lblPlcTitle = new System.Windows.Forms.Label();
            this.lblPlcHeaderIcon = new System.Windows.Forms.Label();
            this.pnlServerCard = new System.Windows.Forms.Panel();
            this.pnlServerStatus = new System.Windows.Forms.Panel();
            this.lblServerIcon = new System.Windows.Forms.Label();
            this.lblServerStatus = new System.Windows.Forms.Label();
            this.lblServerTitle = new System.Windows.Forms.Label();
            this.lblServerHeaderIcon = new System.Windows.Forms.Label();
            this.pnlModeCard = new System.Windows.Forms.Panel();
            this.pnlAuto = new System.Windows.Forms.Panel();
            this.lblAutoIcon = new System.Windows.Forms.Label();
            this.lblAuto = new System.Windows.Forms.Label();
            this.pnlManual = new System.Windows.Forms.Panel();
            this.lblManualIcon = new System.Windows.Forms.Label();
            this.lblManual = new System.Windows.Forms.Label();
            this.lblModeTitle = new System.Windows.Forms.Label();
            this.lblModeHeaderIcon = new System.Windows.Forms.Label();
            this.pnlAgvInfoCard = new System.Windows.Forms.Panel();
            this.tlpAgvInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lblAgvId = new System.Windows.Forms.Label();
            this.lblAgvIdValue = new System.Windows.Forms.Label();
            this.lblIp = new System.Windows.Forms.Label();
            this.lblIpValue = new System.Windows.Forms.Label();
            this.lblAgvInfoTitle = new System.Windows.Forms.Label();
            this.lblAgvInfoHeaderIcon = new System.Windows.Forms.Label();
            this.pnlStatusCard = new System.Windows.Forms.Panel();
            this.tlpStatus = new System.Windows.Forms.TableLayoutPanel();
            this.lblOpStatus = new System.Windows.Forms.Label();
            this.lblOpStatusValue = new System.Windows.Forms.Label();
            this.lblRfid = new System.Windows.Forms.Label();
            this.lblRfidValue = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblSpeedValue = new System.Windows.Forms.Label();
            this.lblMovement = new System.Windows.Forms.Label();
            this.lblMovementValue = new System.Windows.Forms.Label();
            this.lblDirection = new System.Windows.Forms.Label();
            this.lblDirectionValue = new System.Windows.Forms.Label();
            this.lblLoadStatus = new System.Windows.Forms.Label();
            this.lblLoadStatusValue = new System.Windows.Forms.Label();
            this.lblBattery = new System.Windows.Forms.Label();
            this.pbBattery = new System.Windows.Forms.ProgressBar();
            this.lblBatteryValue = new System.Windows.Forms.Label();
            this.lblStatusTitle = new System.Windows.Forms.Label();
            this.lblStatusHeaderIcon = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlSystemStatusCard.SuspendLayout();
            this.pnlSystemStatus.SuspendLayout();
            this.pnlPlcCard.SuspendLayout();
            this.pnlPlcStatus.SuspendLayout();
            this.pnlServerCard.SuspendLayout();
            this.pnlServerStatus.SuspendLayout();
            this.pnlModeCard.SuspendLayout();
            this.pnlAuto.SuspendLayout();
            this.pnlManual.SuspendLayout();
            this.pnlAgvInfoCard.SuspendLayout();
            this.tlpAgvInfo.SuspendLayout();
            this.pnlStatusCard.SuspendLayout();
            this.tlpStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLog
            // 
            this.txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.txtLog.Font = new System.Drawing.Font("Consolas", 12F);
            this.txtLog.Location = new System.Drawing.Point(12, 772);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(1880, 230);
            this.txtLog.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1904, 100);
            this.panel1.TabIndex = 5;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::AGVNew.Properties.Resources.logo_baoan;
            this.pictureBox2.Location = new System.Drawing.Point(1600, 20);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(280, 60);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AGVNew.Properties.Resources.LG_Electronics_logo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(353, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(758, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(330, 37);
            this.label1.TabIndex = 4;
            this.label1.Text = "AGV Monitoring App";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.pnlSystemStatusCard);
            this.panel2.Controls.Add(this.pnlPlcCard);
            this.panel2.Controls.Add(this.pnlServerCard);
            this.panel2.Controls.Add(this.pnlModeCard);
            this.panel2.Controls.Add(this.pnlAgvInfoCard);
            this.panel2.Controls.Add(this.pnlStatusCard);
            this.panel2.Location = new System.Drawing.Point(3, 100);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1901, 606);
            this.panel2.TabIndex = 6;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.textBoxalarm);
            this.panel3.Controls.Add(this.label16);
            this.panel3.Location = new System.Drawing.Point(1565, 162);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(10);
            this.panel3.Size = new System.Drawing.Size(300, 418);
            this.panel3.TabIndex = 7;
            // 
            // textBoxalarm
            // 
            this.textBoxalarm.Font = new System.Drawing.Font("Consolas", 9F);
            this.textBoxalarm.Location = new System.Drawing.Point(13, 37);
            this.textBoxalarm.Multiline = true;
            this.textBoxalarm.Name = "textBoxalarm";
            this.textBoxalarm.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxalarm.Size = new System.Drawing.Size(286, 380);
            this.textBoxalarm.TabIndex = 2;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label16.ForeColor = System.Drawing.Color.DimGray;
            this.label16.Location = new System.Drawing.Point(13, 10);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 24);
            this.label16.TabIndex = 1;
            this.label16.Text = "Alarm";
            // 
            // pnlSystemStatusCard
            // 
            this.pnlSystemStatusCard.BackColor = System.Drawing.Color.White;
            this.pnlSystemStatusCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSystemStatusCard.Controls.Add(this.lblErrorDetail);
            this.pnlSystemStatusCard.Controls.Add(this.pnlSystemStatus);
            this.pnlSystemStatusCard.Controls.Add(this.lblSystemTitle);
            this.pnlSystemStatusCard.Controls.Add(this.lblSystemHeaderIcon);
            this.pnlSystemStatusCard.Location = new System.Drawing.Point(35, 380);
            this.pnlSystemStatusCard.Name = "pnlSystemStatusCard";
            this.pnlSystemStatusCard.Padding = new System.Windows.Forms.Padding(10);
            this.pnlSystemStatusCard.Size = new System.Drawing.Size(431, 200);
            this.pnlSystemStatusCard.TabIndex = 5;
            // 
            // lblErrorDetail
            // 
            this.lblErrorDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorDetail.ForeColor = System.Drawing.Color.Red;
            this.lblErrorDetail.Location = new System.Drawing.Point(10, 130);
            this.lblErrorDetail.Name = "lblErrorDetail";
            this.lblErrorDetail.Size = new System.Drawing.Size(420, 60);
            this.lblErrorDetail.TabIndex = 3;
            this.lblErrorDetail.Text = "Error details here";
            this.lblErrorDetail.Visible = false;
            // 
            // pnlSystemStatus
            // 
            this.pnlSystemStatus.BackColor = System.Drawing.Color.LimeGreen;
            this.pnlSystemStatus.Controls.Add(this.lblSystemStatusIcon);
            this.pnlSystemStatus.Controls.Add(this.lblSystemStatus);
            this.pnlSystemStatus.Location = new System.Drawing.Point(10, 60);
            this.pnlSystemStatus.Name = "pnlSystemStatus";
            this.pnlSystemStatus.Size = new System.Drawing.Size(406, 60);
            this.pnlSystemStatus.TabIndex = 2;
            // 
            // lblSystemStatusIcon
            // 
            this.lblSystemStatusIcon.AutoSize = true;
            this.lblSystemStatusIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblSystemStatusIcon.ForeColor = System.Drawing.Color.White;
            this.lblSystemStatusIcon.Location = new System.Drawing.Point(370, 15);
            this.lblSystemStatusIcon.Name = "lblSystemStatusIcon";
            this.lblSystemStatusIcon.Size = new System.Drawing.Size(26, 26);
            this.lblSystemStatusIcon.TabIndex = 1;
            this.lblSystemStatusIcon.Text = "✓";
            // 
            // lblSystemStatus
            // 
            this.lblSystemStatus.AutoSize = true;
            this.lblSystemStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSystemStatus.ForeColor = System.Drawing.Color.White;
            this.lblSystemStatus.Location = new System.Drawing.Point(10, 15);
            this.lblSystemStatus.Name = "lblSystemStatus";
            this.lblSystemStatus.Size = new System.Drawing.Size(55, 31);
            this.lblSystemStatus.TabIndex = 0;
            this.lblSystemStatus.Text = "OK";
            // 
            // lblSystemTitle
            // 
            this.lblSystemTitle.AutoSize = true;
            this.lblSystemTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblSystemTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblSystemTitle.Location = new System.Drawing.Point(40, 10);
            this.lblSystemTitle.Name = "lblSystemTitle";
            this.lblSystemTitle.Size = new System.Drawing.Size(105, 24);
            this.lblSystemTitle.TabIndex = 1;
            this.lblSystemTitle.Text = "AGV Status";
            // 
            // lblSystemHeaderIcon
            // 
            this.lblSystemHeaderIcon.AutoSize = true;
            this.lblSystemHeaderIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblSystemHeaderIcon.ForeColor = System.Drawing.Color.DimGray;
            this.lblSystemHeaderIcon.Location = new System.Drawing.Point(10, 10);
            this.lblSystemHeaderIcon.Name = "lblSystemHeaderIcon";
            this.lblSystemHeaderIcon.Size = new System.Drawing.Size(31, 24);
            this.lblSystemHeaderIcon.TabIndex = 0;
            this.lblSystemHeaderIcon.Text = "⚠";
            // 
            // pnlPlcCard
            // 
            this.pnlPlcCard.BackColor = System.Drawing.Color.White;
            this.pnlPlcCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPlcCard.Controls.Add(this.pnlPlcStatus);
            this.pnlPlcCard.Controls.Add(this.lblPlcTitle);
            this.pnlPlcCard.Controls.Add(this.lblPlcHeaderIcon);
            this.pnlPlcCard.Location = new System.Drawing.Point(35, 20);
            this.pnlPlcCard.Name = "pnlPlcCard";
            this.pnlPlcCard.Padding = new System.Windows.Forms.Padding(10);
            this.pnlPlcCard.Size = new System.Drawing.Size(431, 85);
            this.pnlPlcCard.TabIndex = 0;
            // 
            // pnlPlcStatus
            // 
            this.pnlPlcStatus.BackColor = System.Drawing.Color.LimeGreen;
            this.pnlPlcStatus.Controls.Add(this.lblPlcIcon);
            this.pnlPlcStatus.Controls.Add(this.lblPlcStatus);
            this.pnlPlcStatus.Location = new System.Drawing.Point(212, 10);
            this.pnlPlcStatus.Name = "pnlPlcStatus";
            this.pnlPlcStatus.Size = new System.Drawing.Size(204, 56);
            this.pnlPlcStatus.TabIndex = 2;
            // 
            // lblPlcIcon
            // 
            this.lblPlcIcon.AutoSize = true;
            this.lblPlcIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblPlcIcon.ForeColor = System.Drawing.Color.White;
            this.lblPlcIcon.Location = new System.Drawing.Point(175, 15);
            this.lblPlcIcon.Name = "lblPlcIcon";
            this.lblPlcIcon.Size = new System.Drawing.Size(26, 26);
            this.lblPlcIcon.TabIndex = 1;
            this.lblPlcIcon.Text = "✓";
            // 
            // lblPlcStatus
            // 
            this.lblPlcStatus.AutoSize = true;
            this.lblPlcStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblPlcStatus.ForeColor = System.Drawing.Color.White;
            this.lblPlcStatus.Location = new System.Drawing.Point(20, 15);
            this.lblPlcStatus.Name = "lblPlcStatus";
            this.lblPlcStatus.Size = new System.Drawing.Size(126, 26);
            this.lblPlcStatus.TabIndex = 0;
            this.lblPlcStatus.Text = "Connected";
            // 
            // lblPlcTitle
            // 
            this.lblPlcTitle.AutoSize = true;
            this.lblPlcTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblPlcTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblPlcTitle.Location = new System.Drawing.Point(43, 27);
            this.lblPlcTitle.Name = "lblPlcTitle";
            this.lblPlcTitle.Size = new System.Drawing.Size(147, 24);
            this.lblPlcTitle.TabIndex = 1;
            this.lblPlcTitle.Text = "PLC Connection";
            // 
            // lblPlcHeaderIcon
            // 
            this.lblPlcHeaderIcon.AutoSize = true;
            this.lblPlcHeaderIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblPlcHeaderIcon.ForeColor = System.Drawing.Color.DimGray;
            this.lblPlcHeaderIcon.Location = new System.Drawing.Point(13, 25);
            this.lblPlcHeaderIcon.Name = "lblPlcHeaderIcon";
            this.lblPlcHeaderIcon.Size = new System.Drawing.Size(22, 24);
            this.lblPlcHeaderIcon.TabIndex = 0;
            this.lblPlcHeaderIcon.Text = "⎕";
            // 
            // pnlServerCard
            // 
            this.pnlServerCard.BackColor = System.Drawing.Color.White;
            this.pnlServerCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlServerCard.Controls.Add(this.pnlServerStatus);
            this.pnlServerCard.Controls.Add(this.lblServerTitle);
            this.pnlServerCard.Controls.Add(this.lblServerHeaderIcon);
            this.pnlServerCard.Location = new System.Drawing.Point(472, 20);
            this.pnlServerCard.Name = "pnlServerCard";
            this.pnlServerCard.Padding = new System.Windows.Forms.Padding(10);
            this.pnlServerCard.Size = new System.Drawing.Size(435, 85);
            this.pnlServerCard.TabIndex = 1;
            // 
            // pnlServerStatus
            // 
            this.pnlServerStatus.BackColor = System.Drawing.Color.Red;
            this.pnlServerStatus.Controls.Add(this.lblServerIcon);
            this.pnlServerStatus.Controls.Add(this.lblServerStatus);
            this.pnlServerStatus.Location = new System.Drawing.Point(213, 10);
            this.pnlServerStatus.Name = "pnlServerStatus";
            this.pnlServerStatus.Size = new System.Drawing.Size(204, 60);
            this.pnlServerStatus.TabIndex = 2;
            // 
            // lblServerIcon
            // 
            this.lblServerIcon.AutoSize = true;
            this.lblServerIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblServerIcon.ForeColor = System.Drawing.Color.White;
            this.lblServerIcon.Location = new System.Drawing.Point(175, 16);
            this.lblServerIcon.Name = "lblServerIcon";
            this.lblServerIcon.Size = new System.Drawing.Size(26, 26);
            this.lblServerIcon.TabIndex = 1;
            this.lblServerIcon.Text = "×";
            // 
            // lblServerStatus
            // 
            this.lblServerStatus.AutoSize = true;
            this.lblServerStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblServerStatus.ForeColor = System.Drawing.Color.White;
            this.lblServerStatus.Location = new System.Drawing.Point(13, 16);
            this.lblServerStatus.Name = "lblServerStatus";
            this.lblServerStatus.Size = new System.Drawing.Size(156, 26);
            this.lblServerStatus.TabIndex = 0;
            this.lblServerStatus.Text = "Disconnected";
            // 
            // lblServerTitle
            // 
            this.lblServerTitle.AutoSize = true;
            this.lblServerTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblServerTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblServerTitle.Location = new System.Drawing.Point(43, 28);
            this.lblServerTitle.Name = "lblServerTitle";
            this.lblServerTitle.Size = new System.Drawing.Size(167, 24);
            this.lblServerTitle.TabIndex = 1;
            this.lblServerTitle.Text = "Server Connection";
            // 
            // lblServerHeaderIcon
            // 
            this.lblServerHeaderIcon.AutoSize = true;
            this.lblServerHeaderIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblServerHeaderIcon.ForeColor = System.Drawing.Color.DimGray;
            this.lblServerHeaderIcon.Location = new System.Drawing.Point(16, 30);
            this.lblServerHeaderIcon.Name = "lblServerHeaderIcon";
            this.lblServerHeaderIcon.Size = new System.Drawing.Size(21, 24);
            this.lblServerHeaderIcon.TabIndex = 0;
            this.lblServerHeaderIcon.Text = "□";
            // 
            // pnlModeCard
            // 
            this.pnlModeCard.BackColor = System.Drawing.Color.White;
            this.pnlModeCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlModeCard.Controls.Add(this.pnlAuto);
            this.pnlModeCard.Controls.Add(this.pnlManual);
            this.pnlModeCard.Controls.Add(this.lblModeTitle);
            this.pnlModeCard.Controls.Add(this.lblModeHeaderIcon);
            this.pnlModeCard.Location = new System.Drawing.Point(1013, 20);
            this.pnlModeCard.Name = "pnlModeCard";
            this.pnlModeCard.Padding = new System.Windows.Forms.Padding(10);
            this.pnlModeCard.Size = new System.Drawing.Size(852, 85);
            this.pnlModeCard.TabIndex = 2;
            // 
            // pnlAuto
            // 
            this.pnlAuto.BackColor = System.Drawing.Color.LimeGreen;
            this.pnlAuto.Controls.Add(this.lblAutoIcon);
            this.pnlAuto.Controls.Add(this.lblAuto);
            this.pnlAuto.Location = new System.Drawing.Point(268, 10);
            this.pnlAuto.Name = "pnlAuto";
            this.pnlAuto.Size = new System.Drawing.Size(233, 60);
            this.pnlAuto.TabIndex = 3;
            // 
            // lblAutoIcon
            // 
            this.lblAutoIcon.AutoSize = true;
            this.lblAutoIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblAutoIcon.ForeColor = System.Drawing.Color.White;
            this.lblAutoIcon.Location = new System.Drawing.Point(10, 15);
            this.lblAutoIcon.Name = "lblAutoIcon";
            this.lblAutoIcon.Size = new System.Drawing.Size(28, 26);
            this.lblAutoIcon.TabIndex = 1;
            this.lblAutoIcon.Text = "↻";
            // 
            // lblAuto
            // 
            this.lblAuto.AutoSize = true;
            this.lblAuto.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblAuto.ForeColor = System.Drawing.Color.White;
            this.lblAuto.Location = new System.Drawing.Point(50, 15);
            this.lblAuto.Name = "lblAuto";
            this.lblAuto.Size = new System.Drawing.Size(126, 26);
            this.lblAuto.TabIndex = 0;
            this.lblAuto.Text = "Auto Mode";
            // 
            // pnlManual
            // 
            this.pnlManual.BackColor = System.Drawing.Color.Gray;
            this.pnlManual.Controls.Add(this.lblManualIcon);
            this.pnlManual.Controls.Add(this.lblManual);
            this.pnlManual.Location = new System.Drawing.Point(551, 10);
            this.pnlManual.Name = "pnlManual";
            this.pnlManual.Size = new System.Drawing.Size(250, 60);
            this.pnlManual.TabIndex = 4;
            // 
            // lblManualIcon
            // 
            this.lblManualIcon.AutoSize = true;
            this.lblManualIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblManualIcon.ForeColor = System.Drawing.Color.White;
            this.lblManualIcon.Location = new System.Drawing.Point(10, 15);
            this.lblManualIcon.Name = "lblManualIcon";
            this.lblManualIcon.Size = new System.Drawing.Size(38, 26);
            this.lblManualIcon.TabIndex = 1;
            this.lblManualIcon.Text = "👤";
            // 
            // lblManual
            // 
            this.lblManual.AutoSize = true;
            this.lblManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.lblManual.ForeColor = System.Drawing.Color.White;
            this.lblManual.Location = new System.Drawing.Point(50, 15);
            this.lblManual.Name = "lblManual";
            this.lblManual.Size = new System.Drawing.Size(154, 26);
            this.lblManual.TabIndex = 0;
            this.lblManual.Text = "Manual Mode";
            // 
            // lblModeTitle
            // 
            this.lblModeTitle.AutoSize = true;
            this.lblModeTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblModeTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblModeTitle.Location = new System.Drawing.Point(76, 30);
            this.lblModeTitle.Name = "lblModeTitle";
            this.lblModeTitle.Size = new System.Drawing.Size(147, 24);
            this.lblModeTitle.TabIndex = 1;
            this.lblModeTitle.Text = "Operation Mode";
            // 
            // lblModeHeaderIcon
            // 
            this.lblModeHeaderIcon.AutoSize = true;
            this.lblModeHeaderIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblModeHeaderIcon.ForeColor = System.Drawing.Color.DimGray;
            this.lblModeHeaderIcon.Location = new System.Drawing.Point(28, 30);
            this.lblModeHeaderIcon.Name = "lblModeHeaderIcon";
            this.lblModeHeaderIcon.Size = new System.Drawing.Size(31, 24);
            this.lblModeHeaderIcon.TabIndex = 0;
            this.lblModeHeaderIcon.Text = "⚙";
            // 
            // pnlAgvInfoCard
            // 
            this.pnlAgvInfoCard.BackColor = System.Drawing.Color.White;
            this.pnlAgvInfoCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlAgvInfoCard.Controls.Add(this.tlpAgvInfo);
            this.pnlAgvInfoCard.Controls.Add(this.lblAgvInfoTitle);
            this.pnlAgvInfoCard.Controls.Add(this.lblAgvInfoHeaderIcon);
            this.pnlAgvInfoCard.Location = new System.Drawing.Point(35, 162);
            this.pnlAgvInfoCard.Name = "pnlAgvInfoCard";
            this.pnlAgvInfoCard.Padding = new System.Windows.Forms.Padding(10);
            this.pnlAgvInfoCard.Size = new System.Drawing.Size(431, 200);
            this.pnlAgvInfoCard.TabIndex = 3;
            // 
            // tlpAgvInfo
            // 
            this.tlpAgvInfo.ColumnCount = 2;
            this.tlpAgvInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAgvInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAgvInfo.Controls.Add(this.lblAgvId, 0, 0);
            this.tlpAgvInfo.Controls.Add(this.lblAgvIdValue, 1, 0);
            this.tlpAgvInfo.Controls.Add(this.lblIp, 0, 1);
            this.tlpAgvInfo.Controls.Add(this.lblIpValue, 1, 1);
            this.tlpAgvInfo.Location = new System.Drawing.Point(10, 60);
            this.tlpAgvInfo.Name = "tlpAgvInfo";
            this.tlpAgvInfo.RowCount = 2;
            this.tlpAgvInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAgvInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAgvInfo.Size = new System.Drawing.Size(406, 120);
            this.tlpAgvInfo.TabIndex = 2;
            // 
            // lblAgvId
            // 
            this.lblAgvId.AutoSize = true;
            this.lblAgvId.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblAgvId.ForeColor = System.Drawing.Color.DimGray;
            this.lblAgvId.Location = new System.Drawing.Point(3, 0);
            this.lblAgvId.Name = "lblAgvId";
            this.lblAgvId.Size = new System.Drawing.Size(77, 24);
            this.lblAgvId.TabIndex = 0;
            this.lblAgvId.Text = "AGV ID:";
            // 
            // lblAgvIdValue
            // 
            this.lblAgvIdValue.AutoSize = true;
            this.lblAgvIdValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblAgvIdValue.ForeColor = System.Drawing.Color.DimGray;
            this.lblAgvIdValue.Location = new System.Drawing.Point(206, 0);
            this.lblAgvIdValue.Name = "lblAgvIdValue";
            this.lblAgvIdValue.Size = new System.Drawing.Size(93, 24);
            this.lblAgvIdValue.TabIndex = 1;
            this.lblAgvIdValue.Text = "AGV-001";
            // 
            // lblIp
            // 
            this.lblIp.AutoSize = true;
            this.lblIp.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblIp.ForeColor = System.Drawing.Color.DimGray;
            this.lblIp.Location = new System.Drawing.Point(3, 60);
            this.lblIp.Name = "lblIp";
            this.lblIp.Size = new System.Drawing.Size(98, 24);
            this.lblIp.TabIndex = 2;
            this.lblIp.Text = "Current IP:";
            // 
            // lblIpValue
            // 
            this.lblIpValue.AutoSize = true;
            this.lblIpValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblIpValue.ForeColor = System.Drawing.Color.DimGray;
            this.lblIpValue.Location = new System.Drawing.Point(206, 60);
            this.lblIpValue.Name = "lblIpValue";
            this.lblIpValue.Size = new System.Drawing.Size(138, 24);
            this.lblIpValue.TabIndex = 3;
            this.lblIpValue.Text = "192.168.1.100";
            // 
            // lblAgvInfoTitle
            // 
            this.lblAgvInfoTitle.AutoSize = true;
            this.lblAgvInfoTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblAgvInfoTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblAgvInfoTitle.Location = new System.Drawing.Point(40, 10);
            this.lblAgvInfoTitle.Name = "lblAgvInfoTitle";
            this.lblAgvInfoTitle.Size = new System.Drawing.Size(147, 24);
            this.lblAgvInfoTitle.TabIndex = 1;
            this.lblAgvInfoTitle.Text = "AGV Information";
            // 
            // lblAgvInfoHeaderIcon
            // 
            this.lblAgvInfoHeaderIcon.AutoSize = true;
            this.lblAgvInfoHeaderIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblAgvInfoHeaderIcon.ForeColor = System.Drawing.Color.DimGray;
            this.lblAgvInfoHeaderIcon.Location = new System.Drawing.Point(10, 10);
            this.lblAgvInfoHeaderIcon.Name = "lblAgvInfoHeaderIcon";
            this.lblAgvInfoHeaderIcon.Size = new System.Drawing.Size(23, 24);
            this.lblAgvInfoHeaderIcon.TabIndex = 0;
            this.lblAgvInfoHeaderIcon.Text = "ⓘ";
            // 
            // pnlStatusCard
            // 
            this.pnlStatusCard.BackColor = System.Drawing.Color.White;
            this.pnlStatusCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStatusCard.Controls.Add(this.tlpStatus);
            this.pnlStatusCard.Controls.Add(this.lblStatusTitle);
            this.pnlStatusCard.Controls.Add(this.lblStatusHeaderIcon);
            this.pnlStatusCard.Location = new System.Drawing.Point(504, 162);
            this.pnlStatusCard.Name = "pnlStatusCard";
            this.pnlStatusCard.Padding = new System.Windows.Forms.Padding(10);
            this.pnlStatusCard.Size = new System.Drawing.Size(1021, 418);
            this.pnlStatusCard.TabIndex = 4;
            // 
            // tlpStatus
            // 
            this.tlpStatus.ColumnCount = 4;
            this.tlpStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpStatus.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpStatus.Controls.Add(this.lblOpStatus, 0, 0);
            this.tlpStatus.Controls.Add(this.lblOpStatusValue, 1, 0);
            this.tlpStatus.Controls.Add(this.lblRfid, 2, 0);
            this.tlpStatus.Controls.Add(this.lblRfidValue, 3, 0);
            this.tlpStatus.Controls.Add(this.lblSpeed, 0, 1);
            this.tlpStatus.Controls.Add(this.lblSpeedValue, 1, 1);
            this.tlpStatus.Controls.Add(this.lblMovement, 2, 1);
            this.tlpStatus.Controls.Add(this.lblMovementValue, 3, 1);
            this.tlpStatus.Controls.Add(this.lblDirection, 0, 2);
            this.tlpStatus.Controls.Add(this.lblDirectionValue, 1, 2);
            this.tlpStatus.Controls.Add(this.lblLoadStatus, 2, 2);
            this.tlpStatus.Controls.Add(this.lblLoadStatusValue, 3, 2);
            this.tlpStatus.Controls.Add(this.lblBattery, 0, 3);
            this.tlpStatus.Controls.Add(this.pbBattery, 1, 3);
            this.tlpStatus.Controls.Add(this.lblBatteryValue, 2, 3);
            this.tlpStatus.Location = new System.Drawing.Point(30, 86);
            this.tlpStatus.Name = "tlpStatus";
            this.tlpStatus.RowCount = 4;
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpStatus.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpStatus.Size = new System.Drawing.Size(959, 287);
            this.tlpStatus.TabIndex = 2;
            // 
            // lblOpStatus
            // 
            this.lblOpStatus.AutoSize = true;
            this.lblOpStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblOpStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblOpStatus.Location = new System.Drawing.Point(3, 0);
            this.lblOpStatus.Name = "lblOpStatus";
            this.lblOpStatus.Size = new System.Drawing.Size(148, 24);
            this.lblOpStatus.TabIndex = 0;
            this.lblOpStatus.Text = "Operation Status";
            // 
            // lblOpStatusValue
            // 
            this.lblOpStatusValue.AutoSize = true;
            this.lblOpStatusValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblOpStatusValue.ForeColor = System.Drawing.Color.DimGray;
            this.lblOpStatusValue.Location = new System.Drawing.Point(242, 0);
            this.lblOpStatusValue.Name = "lblOpStatusValue";
            this.lblOpStatusValue.Size = new System.Drawing.Size(44, 24);
            this.lblOpStatusValue.TabIndex = 1;
            this.lblOpStatusValue.Text = "Idle";
            // 
            // lblRfid
            // 
            this.lblRfid.AutoSize = true;
            this.lblRfid.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblRfid.ForeColor = System.Drawing.Color.DimGray;
            this.lblRfid.Location = new System.Drawing.Point(481, 0);
            this.lblRfid.Name = "lblRfid";
            this.lblRfid.Size = new System.Drawing.Size(94, 24);
            this.lblRfid.TabIndex = 2;
            this.lblRfid.Text = "RFID Data";
            // 
            // lblRfidValue
            // 
            this.lblRfidValue.AutoSize = true;
            this.lblRfidValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblRfidValue.ForeColor = System.Drawing.Color.DimGray;
            this.lblRfidValue.Location = new System.Drawing.Point(720, 0);
            this.lblRfidValue.Name = "lblRfidValue";
            this.lblRfidValue.Size = new System.Drawing.Size(107, 24);
            this.lblRfidValue.TabIndex = 3;
            this.lblRfidValue.Text = "RFID-1234";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblSpeed.ForeColor = System.Drawing.Color.DimGray;
            this.lblSpeed.Location = new System.Drawing.Point(3, 71);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(66, 24);
            this.lblSpeed.TabIndex = 4;
            this.lblSpeed.Text = "Speed";
            // 
            // lblSpeedValue
            // 
            this.lblSpeedValue.AutoSize = true;
            this.lblSpeedValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblSpeedValue.ForeColor = System.Drawing.Color.DimGray;
            this.lblSpeedValue.Location = new System.Drawing.Point(242, 71);
            this.lblSpeedValue.Name = "lblSpeedValue";
            this.lblSpeedValue.Size = new System.Drawing.Size(77, 24);
            this.lblSpeedValue.TabIndex = 5;
            this.lblSpeedValue.Text = "1.2 m/s";
            // 
            // lblMovement
            // 
            this.lblMovement.AutoSize = true;
            this.lblMovement.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblMovement.ForeColor = System.Drawing.Color.DimGray;
            this.lblMovement.Location = new System.Drawing.Point(481, 71);
            this.lblMovement.Name = "lblMovement";
            this.lblMovement.Size = new System.Drawing.Size(99, 24);
            this.lblMovement.TabIndex = 6;
            this.lblMovement.Text = "Movement";
            // 
            // lblMovementValue
            // 
            this.lblMovementValue.AutoSize = true;
            this.lblMovementValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblMovementValue.ForeColor = System.Drawing.Color.DimGray;
            this.lblMovementValue.Location = new System.Drawing.Point(720, 71);
            this.lblMovementValue.Name = "lblMovementValue";
            this.lblMovementValue.Size = new System.Drawing.Size(52, 24);
            this.lblMovementValue.TabIndex = 7;
            this.lblMovementValue.Text = "Stop";
            // 
            // lblDirection
            // 
            this.lblDirection.AutoSize = true;
            this.lblDirection.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblDirection.ForeColor = System.Drawing.Color.DimGray;
            this.lblDirection.Location = new System.Drawing.Point(3, 142);
            this.lblDirection.Name = "lblDirection";
            this.lblDirection.Size = new System.Drawing.Size(84, 24);
            this.lblDirection.TabIndex = 8;
            this.lblDirection.Text = "Direction";
            // 
            // lblDirectionValue
            // 
            this.lblDirectionValue.AutoSize = true;
            this.lblDirectionValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblDirectionValue.ForeColor = System.Drawing.Color.DimGray;
            this.lblDirectionValue.Location = new System.Drawing.Point(242, 142);
            this.lblDirectionValue.Name = "lblDirectionValue";
            this.lblDirectionValue.Size = new System.Drawing.Size(58, 24);
            this.lblDirectionValue.TabIndex = 9;
            this.lblDirectionValue.Text = "Right";
            // 
            // lblLoadStatus
            // 
            this.lblLoadStatus.AutoSize = true;
            this.lblLoadStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblLoadStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblLoadStatus.Location = new System.Drawing.Point(481, 142);
            this.lblLoadStatus.Name = "lblLoadStatus";
            this.lblLoadStatus.Size = new System.Drawing.Size(107, 24);
            this.lblLoadStatus.TabIndex = 10;
            this.lblLoadStatus.Text = "Load Status";
            // 
            // lblLoadStatusValue
            // 
            this.lblLoadStatusValue.AutoSize = true;
            this.lblLoadStatusValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblLoadStatusValue.ForeColor = System.Drawing.Color.DimGray;
            this.lblLoadStatusValue.Location = new System.Drawing.Point(720, 142);
            this.lblLoadStatusValue.Name = "lblLoadStatusValue";
            this.lblLoadStatusValue.Size = new System.Drawing.Size(105, 24);
            this.lblLoadStatusValue.TabIndex = 11;
            this.lblLoadStatusValue.Text = "Unloading";
            // 
            // lblBattery
            // 
            this.lblBattery.AutoSize = true;
            this.lblBattery.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblBattery.ForeColor = System.Drawing.Color.DimGray;
            this.lblBattery.Location = new System.Drawing.Point(3, 213);
            this.lblBattery.Name = "lblBattery";
            this.lblBattery.Size = new System.Drawing.Size(116, 24);
            this.lblBattery.TabIndex = 12;
            this.lblBattery.Text = "Battery Level";
            // 
            // pbBattery
            // 
            this.pbBattery.Location = new System.Drawing.Point(242, 216);
            this.pbBattery.Name = "pbBattery";
            this.pbBattery.Size = new System.Drawing.Size(200, 30);
            this.pbBattery.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbBattery.TabIndex = 13;
            this.pbBattery.Value = 75;
            // 
            // lblBatteryValue
            // 
            this.lblBatteryValue.AutoSize = true;
            this.lblBatteryValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblBatteryValue.ForeColor = System.Drawing.Color.DimGray;
            this.lblBatteryValue.Location = new System.Drawing.Point(481, 213);
            this.lblBatteryValue.Name = "lblBatteryValue";
            this.lblBatteryValue.Size = new System.Drawing.Size(48, 24);
            this.lblBatteryValue.TabIndex = 14;
            this.lblBatteryValue.Text = "75%";
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.AutoSize = true;
            this.lblStatusTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblStatusTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatusTitle.Location = new System.Drawing.Point(40, 10);
            this.lblStatusTitle.Name = "lblStatusTitle";
            this.lblStatusTitle.Size = new System.Drawing.Size(157, 24);
            this.lblStatusTitle.TabIndex = 1;
            this.lblStatusTitle.Text = "Status Information";
            // 
            // lblStatusHeaderIcon
            // 
            this.lblStatusHeaderIcon.AutoSize = true;
            this.lblStatusHeaderIcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.lblStatusHeaderIcon.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatusHeaderIcon.Location = new System.Drawing.Point(10, 10);
            this.lblStatusHeaderIcon.Name = "lblStatusHeaderIcon";
            this.lblStatusHeaderIcon.Size = new System.Drawing.Size(21, 24);
            this.lblStatusHeaderIcon.TabIndex = 0;
            this.lblStatusHeaderIcon.Text = "∿";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1893, 1025);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtLog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "AGV APP";
            this.TopMost = false;
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.pnlSystemStatusCard.ResumeLayout(false);
            this.pnlSystemStatusCard.PerformLayout();
            this.pnlSystemStatus.ResumeLayout(false);
            this.pnlSystemStatus.PerformLayout();
            this.pnlPlcCard.ResumeLayout(false);
            this.pnlPlcCard.PerformLayout();
            this.pnlPlcStatus.ResumeLayout(false);
            this.pnlPlcStatus.PerformLayout();
            this.pnlServerCard.ResumeLayout(false);
            this.pnlServerCard.PerformLayout();
            this.pnlServerStatus.ResumeLayout(false);
            this.pnlServerStatus.PerformLayout();
            this.pnlModeCard.ResumeLayout(false);
            this.pnlModeCard.PerformLayout();
            this.pnlAuto.ResumeLayout(false);
            this.pnlAuto.PerformLayout();
            this.pnlManual.ResumeLayout(false);
            this.pnlManual.PerformLayout();
            this.pnlAgvInfoCard.ResumeLayout(false);
            this.pnlAgvInfoCard.PerformLayout();
            this.tlpAgvInfo.ResumeLayout(false);
            this.tlpAgvInfo.PerformLayout();
            this.pnlStatusCard.ResumeLayout(false);
            this.pnlStatusCard.PerformLayout();
            this.tlpStatus.ResumeLayout(false);
            this.tlpStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.TextBox txtLog;
        private Panel panel1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Label label1;
        private Panel panel2;
        private Panel pnlPlcCard;
        private Panel pnlServerCard;
        private Panel pnlModeCard;
        private Panel pnlAgvInfoCard;
        private Panel pnlStatusCard;
        private Label lblPlcTitle;
        private Panel pnlPlcStatus;
        private Label lblPlcStatus;
        private Label lblPlcIcon;
        private Label lblServerTitle;
        private Panel pnlServerStatus;
        private Label lblServerStatus;
        private Label lblServerIcon;
        private Label lblModeHeaderIcon;
        private Label lblModeTitle;
        private Panel pnlAuto;
        private Label lblAuto;
        private Label lblAutoIcon;
        private Panel pnlManual;
        private Label lblManual;
        private Label lblManualIcon;
        private Label lblAgvInfoHeaderIcon;
        private Label lblAgvInfoTitle;
        private TableLayoutPanel tlpAgvInfo;
        private Label lblAgvId;
        private Label lblAgvIdValue;
        private Label lblIp;
        private Label lblIpValue;
        private Label lblStatusHeaderIcon;
        private Label lblStatusTitle;
        private TableLayoutPanel tlpStatus;
        private Label lblOpStatus;
        private Label lblOpStatusValue;
        private Label lblRfid;
        private Label lblRfidValue;
        private Label lblSpeed;
        private Label lblSpeedValue;
        private Label lblMovement;
        private Label lblMovementValue;
        private Label lblDirection;
        private Label lblDirectionValue;
        private Label lblLoadStatus;
        private Label lblLoadStatusValue;
        private Label lblBattery;
        private ProgressBar pbBattery;
        private Label lblBatteryValue;
        private Panel pnlSystemStatusCard;
        private Panel pnlSystemStatus;
        private Label lblSystemStatusIcon;
        private Label lblSystemStatus;
        private Label lblSystemTitle;
        private Label lblSystemHeaderIcon;
        private Label lblErrorDetail;
        private Label lblPlcHeaderIcon;
        private Label lblServerHeaderIcon;
        private Panel panel3;
        private TextBox textBoxalarm;
        private Label label16;
    }
}