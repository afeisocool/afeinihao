using AfeiLib;
using AfeiModel;
using AfeiModel.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Afei.Controllers
{
    public class Version_recordController : Controller
    {
        // GET: Version_record
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string getlogs()
        {
            var list = ToDB.Getdblist<logsEntity>("select * from logs").OrderByDescending(n=>n.crtdt).ToList();
            return list.Tojson();
        }
    }
}