using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kebue.UI.Areas.Chat.Controllers
{
    public class HomeController : Controller
    {
        // GET: Chat/Home
        public ActionResult Index()
        {
            //图灵Key  var apikey = 'c4f3c14c5de44175be9b1b581cc7673c';//c75ba576f50ddaa5fd2a87615d144ecf
            return View();
        }
    }
}