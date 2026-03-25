using AGVNew.Models;
using System;
using System.Windows.Forms;

namespace AGVNew
{
    static class Program
    {
        // ========================================
        // CHẾ ĐỘ TEST: true = dùng Mock PLC (không cần PLC thật)
        // CHẾ ĐỘ THẬT: false = dùng PLC thật
        // ========================================
        public static bool USE_MOCK = true;  // <-- ĐỔI THÀNH false KHI CHẠY THẬT

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // === Khởi tạo 2 AGV ===

            // AGV1
            var agv1 = AGVData.GetOrCreate("AGV1");
            agv1.option.AGV_ID = "KD130";
            agv1.option.PLC_LogicalStationNumber = 3;
            agv1.option.PLC_M_Base = 5000;       // M5000-M5006
            agv1.option.PLC_D_Action1 = 5003;    // D5003
            agv1.option.PLC_D_Action2 = 5004;    // D5004
            agv1.option.PLC_D_Battery = 5005;    // D5005
            agv1.option.PLC_D_TagId_Base = 5010; // D5010-D5014
            agv1.option.PLC_D_Error = 5015;      // D5015

            // AGV2 (placeholder - sẽ cập nhật khi chốt)
            var agv2 = AGVData.GetOrCreate("AGV2");
            agv2.option.AGV_ID = "AGV2";             // placeholder
            agv2.option.PLC_LogicalStationNumber = 3; // chung PLC
            agv2.option.PLC_M_Base = 6000;            // placeholder M6000-M6006
            agv2.option.PLC_D_Action1 = 6003;         // placeholder D6003
            agv2.option.PLC_D_Action2 = 6004;         // placeholder D6004
            agv2.option.PLC_D_Battery = 6005;         // placeholder D6005
            agv2.option.PLC_D_TagId_Base = 6010;      // placeholder D6010-D6014
            agv2.option.PLC_D_Error = 6015;           // placeholder D6015

            // Tạo View & Presenter
            var view = new Views.MainForm();
            var presenter = new Presenters.MainPresenter(view);
            view.SetPresenter(presenter);

            Application.Run(view);
        }
    }
}