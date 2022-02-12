using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfeiModel.Entity
{
    public class logsEntity
    {
        public Int64 id { get; set; }
        public string shorts { get; set; }
        public string shortstate { get; set; }
        public string descs { get; set; }
        public string descsstate { get; set; }
        public DateTime crtdt { get; set; }
    }
}
