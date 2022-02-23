using AfeiLib;
using AfeiModel;
using AfeiModel.Dto;
using AfeiModel.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Afei.Controllers
{
    public class HomeController : Controller
    {
        [HttpPost]
        public string Jiemi(string str,string key = "mcitloft")
        {
            try
            {
                var res = str.Md5_Decrypt(key).ToString();
                return res.Tojson();
            }
            catch (Exception e)
            {
                return e.Message.Tojson();
            }
        }

        [HttpPost]
        public string Saveuser(userEntity user)
        {
            var sql = string.Format("update users set usernm ='{0}' where usercd='{1}'",user.usernm,user.usercd);
            ToDB.Select(sql);
            return "改名成功".Tojson();
        }
        [HttpPost]
        public string Savehd()
        {
            var usercd = NetworkHelper.GetUsercd();
            var files = HttpContext.Request.Files;
            var file = files[files.AllKeys[0]];
            var path = HttpContext.Server.MapPath("/Content/up/" + usercd + ".head.png");
            var sql = string.Format("update users set userhd='{0}' where usercd='{1}'", usercd+".head.png",usercd);
            ToDB.Select(sql);
            file.SaveAs(path);
            return "头像保存成功".Tojson();
        }
        [HttpPost]
        public string GetLogin()
        {
            var entity = NetworkHelper.GetLogin();
            return entity.Tojson();
        }
        [HttpPost]
        public string sendmsg(string field,string msg)
        {
            var usercd = NetworkHelper.GetUsercd();
            if (usercd!=null)
            {
                string sql = string.Format("insert into reply(field,usercd,msg)values('{0}','{1}','{2}')", field, usercd, msg);
                ToDB.Select(sql);
                return "发送成功".Tojson();
            }
            else
            {
                return "未登录".Tojson();
            }
        }
        [HttpPost]
        public string register(string usernm, string userpwd) 
        { 
            var entitys = ToDB.Getdblist<userEntity>(string.Format("select * from users where usercd = '{0}'",usernm));
            if (entitys.Count==0)
            {
                var sql = string.Format("insert into users(usercd,userpwd,usernm)values('{0}','{1}','nm-'+'{0}')",usernm,userpwd.Md5_Encrypt());
                var res = ToDB.Select(sql);
                return "注册成功".Tojson();
            }
            else
            {
                return "号已存在".Tojson();
            }
        }
        [HttpPost]
        public string login(string usernm,string userpwd)
        {
            var entitys = ToDB.Getdblist<userEntity>(string.Format("select * from users where usercd = '{0}'",usernm));
            var entity = entitys.Count == 0 ? null : entitys[0];
            if (entity==null)
            {
                return "号不存在".Tojson();
            }
            else if (entity.userpwd!=userpwd.Md5_Encrypt())
            {
                var tp = userpwd.Md5_Encrypt();
                return "密码错误".Tojson();
            }
            else
            {
                var ck = new loginDto() {usercd=entity.usercd,edt=DateTime.Now.AddDays(3) };
                var tokens = ck.Tojson().Md5_Encrypt();
                HttpCookie hk = new HttpCookie("afeitool_tokens");
                hk.Value = tokens;
                hk.Expires.AddDays(3);

                HttpCookie hk2 = new HttpCookie("afeitool_login");
                hk2.Value = "logining";
                hk2.Expires.AddDays(3);

                HttpContext.Response.AppendCookie(hk);
                HttpContext.Response.AppendCookie(hk2);
                return "登录成功".Tojson();
            }
        }
        [HttpPost]
        public string logincheck()
        {
            var ck = HttpContext.Request.Cookies["afeitool_tokens"] == null ? null : HttpContext.Request.Cookies["afeitool_tokens"].Value;
            var logins = ck == null ? null : ck.Md5_Decrypt().ToObject<loginDto>();
            if (logins==null)
            {
                return "notlogin".Tojson();
            }
            else if(DateTime.Now< logins.edt)
            {
                return "logining".Tojson();
            }
            else
            {
                return "outdate".Tojson();
            }
        }
        [HttpPost]
        public string getdismsg(string field,string page)
        {
            var list = ToDB.Getdblist<replyDto>(string.Format("exec get_discuss_page '{0}','{1}'",field,page));
            return list.Tojson();
        }
        [HttpPost]
        public string getdiscnt(string field)
        {
            var cnt = ToDB.Getdblist<replyEntity>("select * from reply where field = '" + field + "'").Count();
            return cnt.ToString();
        }
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string Getmenu()
        {
            var list = ToDB.Getdblist<menuEntity>("select * from menu").OrderBy(n=>n.seq).ToList();
            return list.Tojson();
        }
        [HttpGet]
        public string GetAnotherGetApi(string url)
        {
            //接口参数
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url); ;
            req.Method = "Get";
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                using (StreamReader reader = new StreamReader(stream,Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
        [HttpPost]
        private static string GetAnotherPostApi(string postUrl, string paramData, Encoding dataEncode)
        {
            string responseContent = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData);
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";
                webReq.ContentLength = byteArray.Length;
                using (Stream reqStream = webReq.GetRequestStream())
                {
                    reqStream.Write(byteArray, 0, byteArray.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)webReq.GetResponse())
                {
                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
                    {
                        responseContent = sr.ReadToEnd().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return responseContent;
        }
    }
}