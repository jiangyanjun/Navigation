using CommonLayer;
using CommonLayer.Enum;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Kebue.UI.Models;

namespace Kebue.UI.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            try
            {
                base.UserCheck();
                if (CurrenUserInfo.IsNotNull())
                {
                    ViewBag.UserName = CurrenUserInfo;
                    var menuLIst = HttpHelp.Post<List<S_Menu>, string>(ActionEnum.GetMenuList, null);
                    var result = HttpHelp.Post<List<UrlType>, string>(ActionEnum.GetHomePageCount, null);
                    ViewBag.CountTool = result;
                    ViewBag.menu = menuLIst;
                    var menuType = menuLIst.Select(n => n.Types).Distinct();
                    ViewBag.menuType = menuType;
                }
                else
                {
                    return Redirect("../Admin/User/Login");
                }

            }
            catch (Exception ex)
            {
                LogHelper.AddLog(ex);
            }
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
        public ActionResult Report()
        {
            base.UserCheck();
            var typeList = HttpHelp.Post<List<U_Url_Type>, string>(ActionEnum.GetU_Url_TypeList, null);
            ViewBag.type = typeList.FindAll(n => n.Status == EnumType.Normal.GetHashCode());
            DateTime now = DateTime.Now;
            ViewBag.StarDay = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMdd);
            ViewBag.EndDay = GetStr.GetCurrentDay;
            ViewBag.StarMonth = DateTime.Now.AddYears(-1).ToString(GetStr.yyyyMMdd);
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
        public ActionResult Url()
        {
            base.UserCheck();
            var typeList = HttpHelp.Post<List<U_Url_Type>, string>(ActionEnum.GetU_Url_TypeList, null);
            ViewBag.type = typeList.FindAll(n => n.Status == EnumType.Normal.GetHashCode());
            return View();
        }
    }
}