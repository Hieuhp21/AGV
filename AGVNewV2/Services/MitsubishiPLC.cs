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

        public void UpdateStateFromPLC()
        {
            if (!IsConnected)
            {
                ManagerLog.Instance.AddLog("System", "PLC", "Cannot update state: Not connected to PLC, attempting to reconnect...");
                bool reconnected = Connect(AGVData.Instance.option.PLC_LogicalStationNumber);
                if (!reconnected)
                {
                    ManagerLog.Instance.AddLog("System", "PLC", "Reconnection failed, skipping state update");
                    return;
                }
            }

            try
            {
                ManagerLog.Instance.AddLog("System", "PLC", "Starting PLC state update");
                short result;
                int code;

                // Bỏ đọc agv_id từ PLC, lấy từ config AGVData.Instance.option.AGV_ID

                // Đọc action_0 (M100, BIT)
                code = _actUtlType.GetDevice2("M5000", out result);
                if (code == 0)
                {
                    AGVData.Instance.state.action_0 = result == 0;  // 0: Go (true), 1: Stop (false)
                    //ManagerLog.Instance.AddLog("System", "PLC", $"Read M100 (action_0): {(AGVData.Instance.state.action_0 ? "Go" : "Stop")}");
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"Read M100 error, code: {code}");
                }

                // Đọc action_1 (D102, INT)
                code = _actUtlType.GetDevice2("D5003", out result);
                if (code == 0)
                {
                    AGVData.Instance.state.action_1 = result;
                    //ManagerLog.Instance.AddLog("System", "PLC", $"Read  (action_1): {result} (0=S,1=L,2=R)");
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"Read D102 error, code: {code}");
                }

                // Đọc action_2 (D103, INT)
                code = _actUtlType.GetDevice2("D5004", out result);
                if (code == 0)
                {
                    AGVData.Instance.state.action_2 = result;
                   // ManagerLog.Instance.AddLog("System", "PLC", $"Read  (action_2): {result} (0=N,1=U,2=L)");
                }
                else
                { 
                    ManagerLog.Instance.AddLog("System", "PLC", $"Read D103 error, code: {code}");
                }

                // Đọc battery (D101, INT)
                code = _actUtlType.GetDevice2("D5005", out result);
                if (code == 0)
                {
                    AGVData.Instance.state.battery = result;
                   // ManagerLog.Instance.AddLog("System", "PLC", $"Read D101 (battery): {result}%");
                }
                else
                {
                    //ManagerLog.Instance.AddLog("System", "PLC", $"Read D101 error, code: {code}");
                }

                // Đọc tag_id (D200-D204, STRING)
                short[] buffer = new short[5];
                code = _actUtlType.ReadDeviceBlock2("D5010", 5, out buffer[0]);
                if (code == 0)
                {
                    AGVData.Instance.state.tag_id = BufferToString(buffer);
                   // ManagerLog.Instance.AddLog("System", "PLC", $"Read D200-D204 (tag_id): {AGVData.Instance.state.tag_id}");
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"Read D5010-D5014 error, code: {code}");
                }

                // Đọc speed (D105, INT)
                code = _actUtlType.GetDevice2("M5006", out result);
                if (code == 0)
                {
                    AGVData.Instance.state.speed = result;
                   // ManagerLog.Instance.AddLog("System", "PLC", $"Read  (speed): {result} (0=S,1=H,2=M,3=L)");
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"Read M5000 error, code: {code}");
                }

                // Đọc direction (M107, BIT)
                code = _actUtlType.GetDevice2("M5001", out result);
                if (code == 0)
                {
                    AGVData.Instance.state.direction = result != 0;  // 0:F, 1:B
                    //ManagerLog.Instance.AddLog("System", "PLC", $"Read  (direction): {(AGVData.Instance.state.direction ? "Backward" : "Forward")}");
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"Read M5001 error, code: {code}");
                }

                // Đọc state_0 (M108, BIT)
                code = _actUtlType.GetDevice2("M5002", out result);
                if (code == 0)
                {
                    AGVData.Instance.state.state_0 = result == 0;  // 0: working (true?), 1: ? (false)
                    //ManagerLog.Instance.AddLog("System", "PLC", $"Read M108 (state_0): {(AGVData.Instance.state.state_0 ? "Working" : "Unknown")}");
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"Read M108 error, code: {code}");
                }

                // Đọc state_1 (M109, BIT)
                code = _actUtlType.GetDevice2("M5003", out result);
                if (code == 0)
                {
                    AGVData.Instance.state.state_1 = result != 0;
                  //  ManagerLog.Instance.AddLog("System", "PLC", $"Read M5350 (state_1): {result != 0}");
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"Read M109 error, code: {code}");
                }

                // Đọc error (D110, INT) - 0: no error; 1: Low Battery; 2: Motor Failure; 3: Sensor Error; other: Unknown Error
                code = _actUtlType.GetDevice2("M5350", out result);
                if (code == 0)
                {
                    AGVData.Instance.state.error = result;
                  //  ManagerLog.Instance.AddLog("System", "PLC", $"Read M5350 (error): {result} (0=no error)");
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"Read M5350 error, code: {code}");
                }

                // Đọc mode (M110, BIT)
                code = _actUtlType.GetDevice2("M5004", out result);
                if (code == 0)
                {
                    AGVData.Instance.state.mode = result != 0;  // 0: auto (false), 1: manual (true)
                    //ManagerLog.Instance.AddLog("System", "PLC", $"Read M5004 (mode): {(AGVData.Instance.state.mode ? "Manual" : "Auto")}");
                }
                else
                {
                    ManagerLog.Instance.AddLog("System", "PLC", $"Read M5004 error, code: {code}");
                }

                ManagerLog.Instance.AddLog("System", "PLC", "Completed PLC state update");
            }
            catch (Exception ex)
            {
                IsConnected = false;
                ManagerLog.Instance.AddLog("System", "PLC", $"Read error: {ex.Message}, StackTrace: {ex.StackTrace}. Attempting to reconnect...");
                bool reconnected = Connect(AGVData.Instance.option.PLC_LogicalStationNumber);
                if (!reconnected)
                {
                    ManagerLog.Instance.AddLog("System", "PLC", "Reconnection failed after read error");
                }
            }
        }
    }
}