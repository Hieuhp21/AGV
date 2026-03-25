using System;
using System.Collections.Generic;

namespace AGVNew.Models
{
    public class AGVData
    {
        public static readonly object _lock = new object();
        private State _state = new State();

        // Multi-AGV support
        private static readonly Dictionary<string, AGVData> _instances = new Dictionary<string, AGVData>();

        /// <summary>
        /// Backward-compatible: trả về AGV1 mặc định
        /// </summary>
        public static AGVData Instance => GetOrCreate("AGV1");

        /// <summary>
        /// Lấy hoặc tạo AGVData theo key (VD: "AGV1", "AGV2")
        /// </summary>
        public static AGVData GetOrCreate(string agvKey)
        {
            lock (_lock)
            {
                if (!_instances.ContainsKey(agvKey))
                {
                    var data = new AGVData();
                    data.option.AGV_Key = agvKey;
                    _instances[agvKey] = data;
                }
                return _instances[agvKey];
            }
        }

        /// <summary>
        /// Trả về tất cả AGV instances
        /// </summary>
        public static IReadOnlyDictionary<string, AGVData> All => _instances;

        public Option option { get; set; } = new Option();
        public State state
        {
            get
            {
                lock (_lock) { return _state; }
            }
            set
            {
                lock (_lock) { _state = value; }
            }
        }

        public int Tag_ID { get; set; }
        public int Line_Out { get; set; }
        public bool Request_Full_Path_2_ACS { get; set; }
        public bool Request_Carry_Manual_2_ACS { get; set; }
        public string Request_Carry_Manual_StartPoint_str { get; set; }
        public string Request_Carry_Manual_EndPoint_str { get; set; }
        public bool Request_AGV_Auto_Mode_Change_2_ACS { get; set; }
        public Dictionary<string, ACSActionCommand> Action_Command { get; set; } = new Dictionary<string, ACSActionCommand>();
        public RealCommand2ACS Real_Command_2_ACS { get; set; } = new RealCommand2ACS();
        public RealACS2AGV Real_command_2_AGV { get; set; } = new RealACS2AGV();
        public FullPathACS2AGV FullPath { get; set; } = new FullPathACS2AGV();
        public ACS2AGVResult acs_responce { get; set; } = new ACS2AGVResult();
        public int communication_TimeOut_ms { get; set; } = 5000;

        public string GetMovement()
        {
            switch (this.state.action_1)
            {
                case 0: return "Straight";
                case 1: return "Left";
                case 2: return "Right";
                default: return "Unknown";
            }
        }

        public string GetLoadStatus()
        {
            switch (this.state.action_2)
            {
                case 0: return "None";
                case 1: return "Unloading";
                case 2: return "Loading";
                default: return "Unknown";
            }
        }

        public string GetSpeed()
        {
            switch (this.state.speed)
            {
                case 0: return "Stop";
                case 1: return "High";
                case 2: return "Middle";
                case 3: return "Low";
                default: return "Unknown";
            }
        }

        public class Option
        {
            public string AGV_Key { get; set; } = "AGV1";
            public string AGV_ID { get; set; } = "KD130";
            public string IP { get; set; } = "localhost"; // Server API IP
            public int Port { get; set; } = 8000; // Server API Port
            public int Http_TimeOut_ms { get; set; } = 5000;
            public int AGV_Fullpath_Request_Count { get; set; } = 10;
            public double AGV_Battery_MIN { get; set; } = 0;
            public double AGV_Battery_MAX { get; set; } = 100;
            public int AGV_LineCount_MAX_Count { get; set; } = 5;
            public string PLC_IP { get; set; } = "10.224.11.163"; // PLC FX5U IP
            public int PLC_Port { get; set; } = 5001; // PLC Port (MC Protocol)
            public int PLC_LogicalStationNumber { get; set; } = 3; // Logical Station Number in MX Component

            // PLC Address Config per-AGV (dễ thay đổi khi chốt)
            // AGV1: M5000+, D5003+ | AGV2: placeholder M6000+, D6003+
            public int PLC_M_Base { get; set; } = 5000;       // M5000 cho AGV1
            public int PLC_D_Action1 { get; set; } = 5003;    // D5003 cho AGV1
            public int PLC_D_Action2 { get; set; } = 5004;    // D5004 cho AGV1
            public int PLC_D_Battery { get; set; } = 5005;    // D5005 cho AGV1
            public int PLC_D_TagId_Base { get; set; } = 5010; // D5010-D5014 cho AGV1
            public int PLC_D_Error { get; set; } = 5015;      // D5015 cho AGV1
        }

        public class State
        {
            public string agv_id { get; set; } // STRING từ config
            public bool action_0 { get; set; } // BIT: true=Go, false=Stop
            public int action_1 { get; set; } // INT: 0=S, 1=L, 2=R
            public int action_2 { get; set; } // INT: 0=N, 1=U, 2=L
            public int battery { get; set; } // INT: 0-100
            public string tag_id { get; set; } // STRING
            public int speed { get; set; } // INT: 0=S, 1=H, 2=M, 3=L
            public bool direction { get; set; } // BIT: false=F, true=B
            public bool state_0 { get; set; } // BIT: true=working (0), false=unknown
            public bool state_1 { get; set; } // BIT: default false
            public int error { get; set; } // INT: 0=OK, 1=Low Battery, 2=Motor Failure, 3=Sensor Error, etc.
            public bool mode { get; set; } // BIT: false=auto, true=manual
        }
    }

    public class ACSActionCommand
    {
        public string TagID { get; set; }
        public string Loading_Unloading { get; set; }
        public bool Send_Result { get; set; }
    }
}