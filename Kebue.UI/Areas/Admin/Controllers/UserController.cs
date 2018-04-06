using CommonLayer;
using CommonLayer.Enum;
using Kebue.UI.Models;
using PhysicalLayer;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Kebue.UI.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        #region 登录
        [AllowAnonymous]
        public ActionResult AdminLogin(string User, string Pwd)
        {
            if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Pwd))
                return View();
            #region 登录验证
            int logStatus = 2;
            if (!CurrenUserInfo.IsNull() && CurrenUserInfo.UserAccount.IsNotNull())
            {
                ViewBag.UserName = CurrenUserInfo;
                logStatus = 1;
            }
            else if (User.IsNotNull() && Pwd.IsNotNull())
            {
                var result = HttpHelp.Post<S_UserInfo, KeyValuePair<string, string>>(ActionEnum.Login, new KeyValuePair<string, string>(User, Pwd));
                if (result != null)
                {
                    Session["S_UserInfo"] = result;
                    ViewBag.UserName = result;
                    logStatus = 1;
                }
            }
            return this.Json(logStatus, JsonRequestBehavior.AllowGet);
            #endregion
        }
        #endregion

        #region 注销
        public ActionResult OutLogin()
        {
            Session["S_UserInfo"] = null;
            base.UserCheck();
            return this.Json("成功退出", JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}