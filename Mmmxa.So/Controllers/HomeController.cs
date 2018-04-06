using Business;
using CommonLayer;
using PhysicalLayer;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mmmxa.So.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Main()
        {
            ViewBag.Types = UrlTypeBLL.TypesScript;
            if (GetStr.GetCurrentMinute % 3 != 0)
                Task.Factory.StartNew(() => WebSiteSecurity.Request());
            return View();
        }

        public ActionResult Nav()
        {
            return View();
        }

        public ActionResult Nav1()
        {
            return View();
        }
        public ActionResult Acg()
        {
            return View();
        }
        public ActionResult Fulizhan()
        {
            return View();
        }
        public ActionResult JishuZhan()
        {
            return View();
        }
        public ActionResult RenYiMen()
        {
            return View();
        }
        public ActionResult YingShiKu()
        {
            return View();
        }
    }
}