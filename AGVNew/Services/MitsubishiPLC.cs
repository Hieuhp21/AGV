using System;
using System.Text;
using System.Threading;
using AGVNew.Models;
using ActUtlTypeLib;

namespace AGVNew.Services
{
    public class MitsubishiPLC
    {
        private static MitsubishiPLC _instance;
        private ActUtlType _actUtlType;
        public bool IsConnected { get; private set; }
        private const int MaxRetryAttempts = 3;
        private const int RetryDelayMs = 2000;

        public static MitsubishiPLC Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MitsubishiPLC();
                }
                return _instance;
            }
        }

        private MitsubishiPLC()
        {
            try
            {
                _actUtlType = new ActUtlType();
                ManagerLog.Instance.AddLog("System", "PLC", "ActUtlType instantiated successfully");
            }
            catch (Exception ex)
            {
                ManagerLog.Instance.AddLog("System", "PLC", "Failed to instantiate ActUtlType: " + ex.Message);
            }
        }

        public bool Connect(int logicalStationNumber)
        {
            int attempts = 0;
            while (attempts < MaxRetryAttempts)
            {
                try
                {
                    attempts++;
                    ManagerLog.Instance.AddLog("System", "PLC", $"Attempting to connect to PLC (Logical Station: {logicalStationNumber}, Attempt: {attempts}/{MaxRetryAttempts})");
                    _actUtlType.ActLogicalStationNumber = logicalStationNumber;
                    int result = _actUtlType.Open();
                    if (result == 0)
                    {
                        IsConnected = true;
                        ManagerLog.Instance.AddLog("System", "PLC", $"Connected to Mitsubishi FX5U PLC (Logical Station: {logicalStationNumber})");
                        return true;
                    }
                    else
                    {
                        IsConnected = false;
                        ManagerLog.Instance.AddLog("System", "PLC", $"Connection failed, error code: {result}");
                        if (attempts < MaxRetryAttempts)
                        {
                            ManagerLog.Instance.AddLog("System", "PLC", $"Retrying in {RetryDelayMs}ms...");
                            Thread.Sleep(RetryDelayMs);
                        }
                    }
                }
                catch (Exception ex)
                {
                    IsConnected = false;
                    ManagerLog.Instance.AddLog("System", "PLC", $"Connection error (Attempt {attempts}/{MaxRetryAttempts}): {ex.Message}, StackTrace: {ex.StackTrace}");
                    if (attempts < MaxRetryAttempts)
                    {
                        ManagerLog.Instance.AddLog("System", "PLC", $"Retrying in {RetryDelayMs}ms...");
                        Thread.Sleep(RetryDelayMs);
                    }
                }
            }
            ManagerLog.Instance.AddLog("System", "PLC", $"Failed to connect to PLC after {MaxRetryAttempts} attempts");
            return false;
        }

        public void Disconnect()
        {
            try
            {
                if (IsConnected)
                {
                    ManagerLog.Instance.AddLog("System", "PLC", "Attempting to disconnect from PLC");
                    _actUtlType.Close();
                    IsConnected = false;
                    ManagerLog.Instance.AddLog("System", "PLC", "Disconnected from PLC successfully");
                }
            }
            catch (Exception ex)
            {
                ManagerLog.Instance.AddLog("System", "PLC", "Disconnect error: " + ex.Message);
            }
        }

        private string BufferToString(short[] buffer)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < buffer.Length; i++)
            {
                char low = (char)(buffer[i] & 0xFF);
                char high = (char)((buffer[i] >> 8) & 0xFF);
                if (low != '\0') sb.Append(low);
                if (high != '\0') sb.Append(high);
            }
            return sb.ToString().Trim();
        }

        /// <summary>
        /// Backward-compatible: đọc PLC cho AGV mặc định (AGV1)
        /// </summary>
        public void UpdateStateFromPLC()
        {
            UpdateStateFromPLC(AGVData.Instance);
        }

        /// <summary>
        /// Đọc PLC cho 1 AGV cụ thể, dùng address config từ AGVData.option
        /// </summary>
        public void UpdateStateFromPLC(AGVData targetAgv)
        {
            if (!IsConnected)
            {
                ManagerLog.Instance.AddLog("System", "PLC", $"[{targetAgv.option.AGV_Key}] Cannot update state: Not connected to PLC, attempting to reconnect...");
                bool reconnected = Connect(targetAgv.option.PLC_LogicalStationNumber);
                if (!reconnected)
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{targetAgv.option.AGV_Key}] Reconnection failed, skipping state update");
                    return;
                }
            }

            try
            {
                var opt = targetAgv.option;
                string agvKey = opt.AGV_Key;
                ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Starting PLC state update");
                short result;
                int code;

                // Đọc action_0 (M_Base + 0, BIT) - Go/Stop
                code = _actUtlType.GetDevice2($"M{opt.PLC_M_Base}", out result);
                if (code == 0)
                {
                    targetAgv.state.action_0 = result == 0;  // 0: Go (true), 1: Stop (false)
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Read M{opt.PLC_M_Base} (action_0) error, code: {code}");
                }

                // Đọc action_1 (D_Action1, INT) - S/L/R
                code = _actUtlType.GetDevice2($"D{opt.PLC_D_Action1}", out result);
                if (code == 0)
                {
                    targetAgv.state.action_1 = result;
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Read D{opt.PLC_D_Action1} (action_1) error, code: {code}");
                }

                // Đọc action_2 (D_Action2, INT) - N/U/L
                code = _actUtlType.GetDevice2($"D{opt.PLC_D_Action2}", out result);
                if (code == 0)
                {
                    targetAgv.state.action_2 = result;
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Read D{opt.PLC_D_Action2} (action_2) error, code: {code}");
                }

                // Đọc battery (D_Battery, INT)
                code = _actUtlType.GetDevice2($"D{opt.PLC_D_Battery}", out result);
                if (code == 0)
                {
                    targetAgv.state.battery = result;
                }
                else
                {
                    // silent - không log liên tục
                }

                // Đọc tag_id (D_TagId_Base ~ D_TagId_Base+4, STRING, 5 words)
                short[] buffer = new short[5];
                code = _actUtlType.ReadDeviceBlock2($"D{opt.PLC_D_TagId_Base}", 5, out buffer[0]);
                if (code == 0)
                {
                    targetAgv.state.tag_id = BufferToString(buffer);
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Read D{opt.PLC_D_TagId_Base}-D{opt.PLC_D_TagId_Base + 4} (tag_id) error, code: {code}");
                }

                // Đọc speed (M_Base + 6, BIT)
                code = _actUtlType.GetDevice2($"M{opt.PLC_M_Base + 6}", out result);
                if (code == 0)
                {
                    targetAgv.state.speed = result;
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Read M{opt.PLC_M_Base + 6} (speed): {result} (0=S,1=H,2=M,3=L)");
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Read M{opt.PLC_M_Base + 6} (speed) error, code: {code}");
                }

                // Đọc direction (M_Base + 1, BIT) - F/B
                code = _actUtlType.GetDevice2($"M{opt.PLC_M_Base + 1}", out result);
                if (code == 0)
                {
                    targetAgv.state.direction = result != 0;  // 0:F, 1:B
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Read M{opt.PLC_M_Base + 1} (direction) error, code: {code}");
                }

                // Đọc state_0 (M_Base + 2, BIT)
                code = _actUtlType.GetDevice2($"M{opt.PLC_M_Base + 2}", out result);
                if (code == 0)
                {
                    targetAgv.state.state_0 = result == 0;
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Read M{opt.PLC_M_Base + 2} (state_0) error, code: {code}");
                }

                // Đọc state_1 (M_Base + 3, BIT)
                code = _actUtlType.GetDevice2($"M{opt.PLC_M_Base + 3}", out result);
                if (code == 0)
                {
                    targetAgv.state.state_1 = result != 0;
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Read M{opt.PLC_M_Base + 3} (state_1) error, code: {code}");
                }

                // Đọc error (D_Error, INT)
                code = _actUtlType.GetDevice2($"D{opt.PLC_D_Error}", out result);
                if (code == 0)
                {
                    targetAgv.state.error = result;
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Read D{opt.PLC_D_Error} (error) error, code: {code}");
                }

                // Đọc mode (M_Base + 4, BIT) - auto/manual
                code = _actUtlType.GetDevice2($"M{opt.PLC_M_Base + 4}", out result);
                if (code == 0)
                {
                    targetAgv.state.mode = result != 0;  // 0: auto (false), 1: manual (true)
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Read M{opt.PLC_M_Base + 4} (mode) error, code: {code}");
                }

                ManagerLog.Instance.AddLog("System", "PLC", $"[{agvKey}] Completed PLC state update");
            }
            catch (Exception ex)
            {
                IsConnected = false;
                ManagerLog.Instance.AddLog("System", "PLC", $"[{targetAgv.option.AGV_Key}] Read error: {ex.Message}, StackTrace: {ex.StackTrace}. Attempting to reconnect...");
                bool reconnected = Connect(targetAgv.option.PLC_LogicalStationNumber);
                if (!reconnected)
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"[{targetAgv.option.AGV_Key}] Reconnection failed after read error");
                }
            }
        }
    }
}