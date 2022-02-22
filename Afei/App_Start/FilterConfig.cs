using AfeiLib;
using System.Web;
using System.Web.Mvc;

namespace Afei
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //状态监测
            //filters.Add(new CheckAttribute());
            //日志记录
            //filters.Add(new LogAttribute());
        }
    }
}
