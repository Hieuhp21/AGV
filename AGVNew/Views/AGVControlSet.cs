using System.Windows.Forms;

namespace AGVNew.Views
{
    /// <summary>
    /// Chứa tất cả controls UI cho 1 AGV tab
    /// </summary>
    public class AGVControlSet
    {
        public string AgvKey { get; set; }

        // AGV Info
        public Label LblAgvIdValue { get; set; }
        public Label LblIpValue { get; set; }

        // Status
        public Label LblOpStatusValue { get; set; }
        public Label LblRfidValue { get; set; }
        public Label LblSpeedValue { get; set; }
        public Label LblMovementValue { get; set; }
        public Label LblDirectionValue { get; set; }
        public Label LblLoadStatusValue { get; set; }
        public ProgressBar PbBattery { get; set; }
        public Label LblBatteryValue { get; set; }

        // System Status
        public Panel PnlSystemStatus { get; set; }
        public Label LblSystemStatus { get; set; }
        public Label LblSystemStatusIcon { get; set; }
        public Label LblErrorDetail { get; set; }

        // PLC Status
        public Panel PnlPlcStatus { get; set; }
        public Label LblPlcStatus { get; set; }
        public Label LblPlcIcon { get; set; }

        // Server Status
        public Panel PnlServerStatus { get; set; }
        public Label LblServerStatus { get; set; }
        public Label LblServerIcon { get; set; }

        // Mode
        public Panel PnlAuto { get; set; }
        public Label LblAutoIcon { get; set; }
        public Panel PnlManual { get; set; }
        public Label LblManualIcon { get; set; }

        // Alarm
        public TextBox TextBoxAlarm { get; set; }
    }
}
