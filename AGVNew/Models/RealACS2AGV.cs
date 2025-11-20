using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AGVNew.Models
{
    public class RealACS2AGV
    {
        public string agv_id { get; set; }
        public string action_0 { get; set; }
        public string action_1 { get; set; }
        public string action_2 { get; set; }
        public string action_3 { get; set; }
        public string action_4 { get; set; }
        public string tag_id { get; set; }
        public string speed { get; set; }
        public string front_sensor { get; set; }
        public int agv_mode { get; set; }
        public string acs_command { get; set; }
        public string wait_for { get; set; }
        public string alarm { get; set; }
        public string depot { get; set; }
        public List<string> location { get; set; } = new List<string>();
        public string error { get; set; }
    }
}