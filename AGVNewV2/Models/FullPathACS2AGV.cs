using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AGVNew.Models
{
    public class FullPathACS2AGV
    {
        public string agv_id { get; set; }
        public string state { get; set; }
        public List<string> full_path { get; set; } = new List<string>();
        public List<string> rf_com { get; set; } = new List<string>();
        public string depot { get; set; }
        public List<string> location { get; set; } = new List<string>();
    }
}
