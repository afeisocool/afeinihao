using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfeiModel.Entity
{
    public class replyEntity
    {
        public Int64 id { get; set; }
        public string field { get; set; }
        public string usercd { get; set; }
        public string usernm { get; set; }
        public string msg { get; set; }
        public string state { get; set; }
        public DateTime crtdt { get; set; }
    }
}
