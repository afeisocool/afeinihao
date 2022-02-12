using System.Web;
using System.Web.Optimization;

namespace Afei
{
    public class BundleConfig
    {
        // 有关捆绑的详细信息，请访问 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //必要的js
            bundles.Add(new ScriptBundle("~/Scripts/js")
                .Include(
                "~/Scripts/jquery-3.4.1-min.js",
                "~/Scripts/angular-min.js",
                "~/Scripts/layer-min.js",
                "~/Scripts/afei.js"
                ));
            //必要的css
            bundles.Add(new StyleBundle("~/Content/css")
                .Include(
                "~/Content/layer-min.css",
                "~/Content/afei.css"
                ));
        }
    }
}
