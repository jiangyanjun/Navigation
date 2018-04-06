using Business;
using CommonLayer;
using CommonLayer.Enum;
using DataLayer;
using PhysicalLayer;
using System;
using System.Web.Mvc;

namespace Mmmxa.So.Controllers
{
    public class AdminController : BaseController
    {
        #region 对象初始化
        U_Url_ListDAL bllMenuList = new U_Url_ListDAL();
        #endregion

        public ActionResult Index()
        {
            ViewBag.UserName = base.UserCheck();
            return View();
        }

        /// <summary>
        /// hhhhh
        /// </summary>
        /// <returns></returns>
        public ActionResult Home()
        {
            base.UserCheck();
            ViewBag.CountTool = bllMenuList.QuerySql<UrlType>(SqlStr.GetHomePageCount);
            return View();
        }

        public ActionResult Task()
        {
            base.UserCheck();
            return View();
        }

        public ActionResult UrlTypeManager()
        {
            base.UserCheck();
            return View();
        }
        /// <summary>
        /// 留言管理
        /// </summary>
        /// <returns></returns>
        public ActionResult LeaveAMessage()
        {
            base.UserCheck();
            DateTime now = DateTime.Now;
            ViewBag.StarDay = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMdd);
            ViewBag.EndDay = GetStr.GetCurrentDay;
            return View();
        }
        /// <summary>
        /// 日志管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Log()
        {
            base.UserCheck();
            DateTime now = DateTime.Now;
            ViewBag.StarDay = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMdd);
            ViewBag.EndDay = GetStr.GetCurrentDay;
            return View();
        }

        public ActionResult Url()
        {
            base.UserCheck();
            ViewBag.type = new U_Url_TypeBLL().Find().FindAll(n => n.Status == EnumType.Normal.GetHashCode());
            return View();
        }
        public ActionResult Report()
        {
            base.UserCheck();
            ViewBag.type = new U_Url_TypeBLL().Find().FindAll(n => n.Status == EnumType.Normal.GetHashCode());
            DateTime now = DateTime.Now;
            ViewBag.StarDay = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMdd);
            ViewBag.EndDay = GetStr.GetCurrentDay;
            ViewBag.StarMonth = DateTime.Now.AddYears(-1).ToString(GetStr.yyyyMMdd);
            return View();
        }
    }
}