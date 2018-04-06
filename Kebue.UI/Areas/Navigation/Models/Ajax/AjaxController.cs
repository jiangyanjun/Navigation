using CommonLayer;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Linq;
using CommonLayer.Enum;
using Kebue.UI.Models;

namespace Kebue.UI.Areas.Navigation.Controllers
{
    /// <summary>
    /// Ajax
    /// </summary>
    public class AjaxController : BaseController
    {
        public void Addclickrate(string UserAgents, string UrlId, string Ip, string Msg)
        {
            U_Url_ClickRate rate = new U_Url_ClickRate();
            rate.Id = GetStr.GetGuid;
            rate.UserAgents = UserAgents;
            rate.UrlId = UrlId;
            rate.Ip = Ip;
            rate.Msg = Msg;
            rate.ClickDate = GetStr.GetCurrentDate;
            if (!string.IsNullOrEmpty(rate.UserAgents) && !string.IsNullOrEmpty(rate.UrlId))
            {
                var r = HttpHelp.Post<string, U_Url_ClickRate>(ActionEnum.Addclickrate, rate);
            }
        }
        #region 留言数据加载
        [HttpPost]
        public string LeaveAMessage(string type, string msg, string userAgent)
        {
            string info = string.Empty;
            if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(msg))
            {
                LeaveAMessage lmsg = new PhysicalLayer.LeaveAMessage();
                lmsg.Id = GetStr.GetGuid;
                lmsg.Name = type;
                lmsg.Content = msg;
                lmsg.UserAgent = userAgent;
                lmsg.Create_Time = GetStr.GetCurrentDate;
                info = HttpHelp.Post<string, LeaveAMessage>(ActionEnum.LeaveAMessage, lmsg);
            }
            else
            {
                info = "类型和内容不可为空";
            }
            return info;
        }
        #endregion
        [HttpPost]
        public ActionResult GeturlDaras(int index = 0)
        {
            ActionResult res;
            string cacheKey = CacheKeyEnum.GeturlDaras.ToEnumDiscription() + index;
            res = CacheHelper.GetCache(cacheKey) as ActionResult;
            if (res.IsNull())
            {
                Func<int, Tuple<int, string>> func = GeturlDarasResult;
                var result = func(index);
                res = this.Json(new { nex = result.Item1, msg = result.Item2 }, JsonRequestBehavior.AllowGet);
                CacheHelper.SetCache(cacheKey, res);
            }
            return res;
        }
        private Tuple<int, string> GeturlDarasResult(int index = 0)
        {
            int nex;
            string msg = string.Empty;
            RequestNavigateEntity result = HttpHelp.Post<RequestNavigateEntity, dynamic>(ActionEnum.GeturlDaras, new { Parameter = index });
            if (result != null && result.UrlList != null && result.Type != null && result.UrlList.Count > 0)
            {
                #region 组装
                #region 变量定义
                int with = 0;
                int height = 0;
                StringBuilder str = new StringBuilder();
                StringBuilder str2 = new StringBuilder();// g-br13
                List<S_Config> ConfigList = CacheHelper.GetCache(ActionEnum.Find_Config.ToString()) as List<S_Config>;
                {
                    ConfigList = HttpHelp.Post<List<S_Config>, string>(ActionEnum.Find_Config, null);
                    CacheHelper.SetCache(ActionEnum.Find_Config.ToString(), ConfigList);
                }
                if (ConfigList.IsNotNull())
                {
                    ConfigList = ConfigList.FindAll(n => n.Types == "icom");
                    with = ConfigList.Find(n => n.Keys == "icom_with").Value.ToInt();
                    height = ConfigList.Find(n => n.Keys == "icom_height").Value.ToInt();
                }
                with = with <= 0 ? 25 : with;
                height = height <= 0 ? 18 : with;
                var img = "<img src=\"{0}\" alt=\"www.kebue.com\" style=\"max-width:" + with + "px;max-height:" + height + "px;\"/>";
                var header = "<div class=\"row kebue_t g-grid g-bg11 govsite-top\"><div class=\"g-gc g-fc20 g-brwr title kebue_h\" {0}>{1}</div>{2}</div></div>";
                var html = "<div class=\"g-gc\"><a href='{2}' title='{3}' class=\"lba\" target=\"_blank\" onclick=\"clickrate('{0}')\">{4}{1}</a></div>";
                int i = 0;
                bool IsH = true;
                #endregion
                foreach (var item in result.UrlList)
                {
                    string resImg = string.Empty;
                    #region name
                    string name = item.Name;
                    if (name.GetStrLength() > 30)
                        name = name.StrCut(30);
                    #endregion
                    #region title
                    string title = item.Name;
                    if (!string.IsNullOrEmpty(item.Title))
                        title = title + "    " + item.Title;
                    #endregion
                    #region 格式组装
                    string path = string.Format("{0}\\{1}", Images.GenerateIconsPath, item.IconImg);
                    if (item.IconImg.IsNotNull() && System.IO.File.Exists(path))
                    {
                        resImg = string.Format(img + "&nbsp;&nbsp;", Images.GetIcomPath + item.IconImg);
                    }
                    str.AppendFormat(html, item.Id, name, item.Url, title, resImg);
                    i++;
                    if (IsH && i == 11 || (result.UrlList.Count() <= 11 && result.UrlList.Count() == i))
                    {
                        str2.AppendFormat("{0}</div></div>", string.Format(header, string.Format("id=dw_{0}", result.Type.Id),string.Format("<a title=\"{0}\"><span>{0}</span></a>", result.Type.Name), str.ToString()));
                        str.Length = 0;
                        str.Clear();
                    }
                    else if ((i % 11 == 0 && i > 11) || (i == result.UrlList.Count()))
                    {
                        str2.AppendFormat("{0}</div></div>", string.Format(header, "", "", str.ToString()));
                        str.Length = 0;
                        str.Clear();
                    }
                    #endregion
                }
                var htmlContent = str2.ToString();
                if (htmlContent.Length > 10 && !htmlContent.Contains("<hr/>"))
                {
                    htmlContent = htmlContent + "<hr/>";
                }
                nex = index + 1;
                msg = htmlContent;
                #endregion
            }
            else
            {
                nex = 0;
                msg = "";
            }
            return new Tuple<int, string>(nex, msg);
        }

    }

}