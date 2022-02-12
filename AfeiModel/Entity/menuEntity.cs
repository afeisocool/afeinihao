using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfeiModel.Entity
{
    public class menuEntity
    {
        public Int64 id { get; set; }
        public Int64 seq { get; set; }
        public string name { get; set; }
        public string iconcls { get; set; }
        public string desc { get; set; }
        public string areaurl { get; set; }
        public string state { get; set; }
        public string crtdt { get; set; }
        public string upddt { get; set; }
    }
}
