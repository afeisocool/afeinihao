using AfeiModel;
using AfeiModel.Dto;
using AfeiModel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
        public static string GetUsercd()
        {
            try
            {
                var tokens = HttpContext.Current.Request.Cookies["afeitool_tokens"];
                var entity = tokens.Value.ToString().Md5_Decrypt().ToObject<loginDto>();
                if (entity.edt>DateTime.Now)
                {
                    return entity.usercd;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        /// <summary>
        /// 获得用户的个人信息
        /// </summary>
        /// <returns></returns>
        public static userEntity GetLogin()
        {
            try
            {
                var usercd = GetUsercd();
                var entity = ToDB.Getdblist<userEntity>(string.Format("select * from users where usercd = '{0}'", usercd))[0];
                return entity;
            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
