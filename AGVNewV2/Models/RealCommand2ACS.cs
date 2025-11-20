using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVNew.Models
{
    public class RealCommand2ACS
    {
        public string agv_id { get; set; }
        public string action_0 { get; set; }
        public string action_1 { get; set; }
        public string action_2 { get; set; }
        public int battery { get; set; }
        public string tag_id { get; set; }
        public string speed { get; set; }
        public string direction { get; set; }
        public int state_0 { get; set; }
        public int state_1 { get; set; }
        public int error { get; set; }
    }
}
