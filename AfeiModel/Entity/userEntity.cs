using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfeiModel.Entity
{
    public class userEntity
    {
        public Int64 id { get; set; }
        public string usercd { get; set; }
        public string userpwd { get; set; }
        public string userhd { get; set; }
        public string usernm { get; set; }
        public string userst { get; set; }
        public DateTime crtdt { get; set; }
    }
}
