using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AfeiLib
{
    /// <summary>
    /// 网络帮助
    /// </summary>
    public class NetworkHelper
    {
        /// <summary>
        /// 获取ip
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIP()
        {
            IPAddress localIp = null;

            try
            {
                IPAddress[] ipArray;
                ipArray = Dns.GetHostAddresses(Dns.GetHostName());
                localIp = ipArray.First(ip => ip.AddressFamily == AddressFamily.InterNetwork);

            }
            catch (Exception)
            {
            }
            if (localIp == null)
            {
                localIp = IPAddress.Parse("192.168.1.101");
            }
            return localIp.ToString();
        }
    }
}
