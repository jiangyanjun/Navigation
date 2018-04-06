using Business;
using CommonLayer;
using DataLayer;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mmmxa.So.Controllers
{
    public class UserController : BaseController
    {
        #region 对象初始化
        S_UserInfoBusiness bll_S_UserInfo = new S_UserInfoBusiness();
        #endregion

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
                var result = bll_S_UserInfo.Find();
                if (User.IsNotNull() && Pwd.IsNotNull())
                    result = result.FindAll(n => n.UserAccount.ToUpper() == User.ToUpper() && n.Password == Pwd);
                if (result != null && result.Count > 0)
                {
                    Session["S_UserInfo"] = result[0];
                    ViewBag.UserName = result[0];
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
            return this.Json("成功退出", JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult AddUrl()
        {
            ViewBag.Types = UrlTypeBLL.Types;
            return View();
        }
    }
}