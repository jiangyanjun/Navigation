using Business;
using DataLayer;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Mmmxa.So
{
    public abstract class BaseController : Controller
    {

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public S_UserInfo CurrenUserInfo
        {
            get
            {
                try
                {
                    return Session["S_UserInfo"] == null ? null : Session["S_UserInfo"] as S_UserInfo;
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 用户登录检查
        /// </summary>
        public virtual S_UserInfo UserCheck()
        {
            Func<S_UserInfo> func = FuncUserCheck;
            return func();
        }
        private S_UserInfo FuncUserCheck()
        {
            if (CurrenUserInfo == null)
            {
                JsResponse("../User/AdminLogin");
            }
            return CurrenUserInfo;
        }
        /// <summary>
        /// js 跳转到指定URL
        /// </summary>
        /// <param name="url"></param>
        public void JsResponse(string url)
        {
            System.Web.HttpContext.Current.Response.Write(string.Format("<script>window.location.href='{0}'</script>", url));
            System.Web.HttpContext.Current.Response.End();
        }





    }
}