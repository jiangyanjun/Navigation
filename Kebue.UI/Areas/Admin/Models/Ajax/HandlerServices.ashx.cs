using Business;
using CommonLayer;
using CommonLayer.Enum;
using Kebue.UI;
using Kebue.UI.Models;
using Newtonsoft.Json;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kebue.UI.Areas.Admin.Controllers
{
    /// <summary>
    /// HandlerServices 的摘要说明
    /// </summary>
    public class HandlerServices : IHttpHandler
    {
        U_Url_ClickRateBLL clickRateBll = new U_Url_ClickRateBLL();
        U_Url_TypeBLL bllType = new U_Url_TypeBLL();
        U_Url_ListBLL bllMenuList = new U_Url_ListBLL();

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.Charset = "utf-8";
            context.Response.Buffer = true;
            context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);
            context.Response.AddHeader("pragma", "no-cache");
            context.Response.AddHeader("cache-control", "");
            context.Response.CacheControl = "no-cache";
            string returnInfo = string.Empty;
            string method = context.Request.QueryString["method"].ToLower();
            string parameter = context.Request.QueryString["key"];
            string parameter2 = context.Request.QueryString["key2"];
            var p = new{method=method,parameter=parameter,parameter2=parameter2 };
            returnInfo = HttpHelp.Post<string, dynamic>(ActionEnum.ProcessRequest, p);
            context.Response.Write(returnInfo);
        }
    }
}