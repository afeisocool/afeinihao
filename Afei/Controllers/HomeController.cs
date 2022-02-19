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