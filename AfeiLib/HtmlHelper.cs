using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AfeiLib
{
    public static class HtmlHelper
    {
        public static IHtmlString Script(this System.Web.Mvc.HtmlHelper html, string url)
        {
            return html.Raw("<script src=\"" + url + "\" type=\"text/javascript\"></script>");
        }
        public static IHtmlString Style(this System.Web.Mvc.HtmlHelper html, string url)
        {
            return html.Raw("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + url + "\" />");
        }
        public static IHtmlString IncludeScriptAndStyle(this System.Web.Mvc.HtmlHelper html)
        {
            var arr = HttpContext.Current.Request.Url.LocalPath.Split('/');
            string styleurl;
            string scripturl;
            if (arr.Length<=2)
            {
                styleurl = "/Views/Home/index.css";
                scripturl = "/Views/Home/index.js";
            }
            else
            {
                styleurl = string.Format("/Views/{0}/{1}.css", arr[1], arr[2]);
                scripturl = string.Format("/Views/{0}/{1}.js", arr[1], arr[2]);
            }
            return html.Raw(
                "<link rel=\"stylesheet\" type=\"text/css\" href=\"" + styleurl + "\" />\n" +
                "<script src=\"" + scripturl + "\" type=\"text/javascript\"></script>"
            );
        }
    }
}
