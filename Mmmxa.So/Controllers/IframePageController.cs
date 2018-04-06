using System.Web.Mvc;

namespace Mmmxa.So.Controllers
{
    public class IframePageController : Controller
    {
        // 学术搜索
        public ActionResult Scholar()
        {
            return View();
        }
        /// <summary>
        /// 磁力搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult Magnet()
        {
            return View();
        }
        /// <summary>
        /// 图片搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult Image()
        {
            return View();
        }
        /// <summary>
        /// 数据搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult Data()
        {
            return View();
        }
        /// <summary>
        /// 快搜索
        /// </summary>
        /// <returns></returns>
        public ActionResult Search()
        {
            return View();
        }
    }
}