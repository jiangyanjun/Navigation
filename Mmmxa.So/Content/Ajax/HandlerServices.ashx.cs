using Business;
using CommonLayer;
using CommonLayer.Enum;
using DataLayer;
using Newtonsoft.Json;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mmmxa.So.Content.Ajax
{
    /// <summary>
    /// HandlerServices 的摘要说明
    /// </summary>
    public class HandlerServices : IHttpHandler
    {
        U_Url_ClickRateBLL clickRateBll = new U_Url_ClickRateBLL();
        U_Url_TypeDAL bllType = new U_Url_TypeDAL();
        U_Url_ListDAL bllMenuList = new U_Url_ListDAL();
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
            switch (method)
            {
                case "geturlalldata":
                    if (parameter == "undefined") parameter = "1";
                    if (!string.IsNullOrEmpty(parameter) && !string.IsNullOrEmpty(parameter2) && parameter != "undefined")
                        returnInfo = JsonConvert.SerializeObject(GetMenuList(Convert.ToInt16(parameter), parameter2));
                    else if (!string.IsNullOrEmpty(parameter))
                        returnInfo = JsonConvert.SerializeObject(GetMenuList(Convert.ToInt16(parameter), null));
                    break;
                case "getcurrentobj":
                    if (!string.IsNullOrEmpty(parameter))
                        returnInfo = JsonConvert.SerializeObject(GetMeu(parameter));
                    break;
                case "gettypedata":
                    returnInfo = JsonConvert.SerializeObject(UrlTypeBLL.UrlTypeListManager.OrderByDescending(n => n.SortDesc));
                    break;
                case "geturlentity":
                    if (!string.IsNullOrEmpty(parameter))
                        returnInfo = JsonConvert.SerializeObject(bll.FindPrimaryId(string.Format("AND Types='{0}'", parameter)).OrderBy(n => n.SortDesc));
                    break;
                case "addclickrate":
                    U_Url_ClickRate rate = new U_Url_ClickRate();
                    rate.Id = GetStr.GetGuid;
                    rate.UserAgents = context.Request.QueryString["b"];
                    rate.UrlId = context.Request.QueryString["a"];
                    rate.Ip = context.Request.QueryString["c"];
                    rate.Msg = context.Request.QueryString["d"];
                    rate.ClickDate = GetStr.GetCurrentDate;
                    if (!string.IsNullOrEmpty(rate.UserAgents) && !string.IsNullOrEmpty(rate.UrlId))
                    {
                        clickRateBll.Add(rate);
                    }
                    break;
                case "geturlmodify":
                    returnInfo = GeturlModify(parameter);
                    break;
                case "getclickrateday":
                    returnInfo = GetClickRateDay(parameter, parameter2);
                    break;
                case "getclickratemonth":
                    returnInfo = GetClickRateMonth(parameter, parameter2);
                    break;
                case "getcheckresult":
                    returnInfo = GetCheckResult(parameter, parameter2);
                    break;
                case "leaveamessage":
                    returnInfo = LeaveAMessage(parameter, parameter2);
                    break;
                case "log":
                    returnInfo = Log(parameter, parameter2);
                    break;
            }
            context.Response.Write(returnInfo);
        }
        public string Log(string starTime = null, string endTime = null)
        {
            if (string.IsNullOrEmpty(starTime))
            {
                DateTime now = DateTime.Now;
                starTime = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMddmmHHss);
            }
            if (string.IsNullOrEmpty(endTime))
                endTime = GetStr.GetCurrentDate;
            var result = clickRateBll.FinList<S_Log>(string.Format(SqlStr.LogSql, starTime, endTime));
            return JsonConvert.SerializeObject(result);
        }
        public string LeaveAMessage(string starTime = null, string endTime = null)
        {
            if (string.IsNullOrEmpty(starTime))
            {
                DateTime now = DateTime.Now;
                starTime = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMddmmHHss);
            }
            if (string.IsNullOrEmpty(endTime))
                endTime = GetStr.GetCurrentDate;
            var result = clickRateBll.FinList<LeaveAMessage>(string.Format(SqlStr.LeaveAMessage, starTime, endTime));
            return JsonConvert.SerializeObject(result);
        }
        public string GetCheckResult(string starTime = null, string endTime = null)
        {
            if (string.IsNullOrEmpty(starTime))
            {
                DateTime now = DateTime.Now;
                starTime = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMddmmHHss);
            }
            if (string.IsNullOrEmpty(endTime))
                endTime = GetStr.GetCurrentDate;
            var result = clickRateBll.FinList<U_Url_Check>(string.Format(SqlStr.GetCheckResult, starTime, endTime));
            return JsonConvert.SerializeObject(result);
        }
        public string GetClickRateDay(string starTime = null, string endTime = null)
        {
            if (string.IsNullOrEmpty(starTime))
            {
                DateTime now = DateTime.Now;
                starTime = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMddmmHHss);
            }
            if (string.IsNullOrEmpty(endTime))
                endTime = GetStr.GetCurrentDate;
            var result = clickRateBll.FinList<_ClickRate>(string.Format(SqlStr.GetClickRateDay, starTime, endTime));
            return JsonConvert.SerializeObject(result);
        }
        public string GetClickRateMonth(string starTime = null, string endTime = null)
        {
            if (string.IsNullOrEmpty(starTime))
                starTime = DateTime.Now.AddYears(-1).ToString(GetStr.yyyyMMddmmHHss);
            if (string.IsNullOrEmpty(endTime))
                endTime = GetStr.GetCurrentDate;
            var result = clickRateBll.FinList<_ClickRate>(string.Format(SqlStr.GetClickRateMonth, starTime, endTime));
            return JsonConvert.SerializeObject(result);
        }
        public U_Url_List GetMeu(string Id)
        {
            return bll.Find(string.Format("AND A.ID='{0}'", Id));
        }
        U_Url_ListBLL bll = new U_Url_ListBLL();
        public List<UrlList> GetMenuList(int i, string filter)
        {
            List<UrlList> list = new List<UrlList>();
            var result = bll.Find().FindAll(n => n.Status == i);
            if (!string.IsNullOrEmpty(filter))
                result = result.FindAll(n => n.Url.Contains(filter) || n.Id.Contains(filter) || n.Name.Contains(filter) || n.Types.Contains(filter));
            foreach (var item in result)
            {
                UrlList e = new UrlList();
                e.Id = item.Id;
                e.Name = string.Format("<div class=\"input-group\"><input type=\"text\" value=\"{0}\" class=\"form-control\" placeholder=\"名称……\" id=\"txtName_{1}\"><span class=\"input-group-btn\"><button class=\"btn btn-default\" type=\"button\" onclick=\"UrlNameUpdateSave('{1}')\">Go</button></span></div>", item.Name, item.Id);
                e.Url = string.Format("<div class=\"input-group\"><input type=\"text\" value=\"{0}\" class=\"form-control\" placeholder=\"网址……\" id=\"txtUrl_{1}\"><span class=\"input-group-btn\"><button class=\"btn btn-default\" type=\"button\" onclick=\"UrlUpdateSave('{1}')\">Go</button></span></div>", item.Url, item.Id);
                e.Source = string.Format("<div class=\"input-group\"><input type=\"text\" value=\"{0}\" class=\"form-control\" placeholder=\"网址描述……\" id=\"txtSource_{1}\"><span class=\"input-group-btn\"><button class=\"btn btn-default\" type=\"button\" onclick=\"UrlSourceUpdateSave('{1}')\">Go</button></span></div>", item.Source, item.Id);
                //e.Title = string.Format("<div class=\"input-group\"><input type=\"text\" value=\"{0}\" class=\"form-control\" placeholder=\"Title……\" id=\"txtTitle_{1}\"><span class=\"input-group-btn\"><button class=\"btn btn-default\" type=\"button\" onclick=\"UrlTitleUpdateSave('{1}')\">Go</button></span></div>", item.Title, item.Id);
                e.Types = string.Format("<a onclick=\"myModalModify('{0}')\">{1}</a>", item.Id, item.Types);
                e.SortDesc = string.Format("<div class=\"input-group\"><input type=\"text\" value=\"{0}\" class=\"form-control\" placeholder=\"降序排列……\" id=\"txtSort_{1}\"><span class=\"input-group-btn\"><button class=\"btn btn-default\" type=\"button\" onclick=\"btnSortSave('{1}')\">Go</button></span></div>", item.SortDesc.ToString(), item.Id);
                e.Status = string.Format("<button class=\"{2}\" id='btnStatus{0}' onclick=\"myStatusEdit('{0}');\">{1}</button>", item.Id, item.Status.ToEnumDiscription<EnumUrlStatus>(), GetStatusCss(item.Status));
                e.Operation = string.Format("<a class=\"btn btn-info btn-circle\" href=\"{0}\" target=\"_blank\" title=\"{1}\" id=\"txtfwxtest_{2}\" onclick=\"BtnClearClass('{2}')\"> <i class=\"glyphicon glyphicon-ok\"></i></a>", item.Url, item.Title, item.Id);
                list.Add(e);
            }
            return list;
        }
        public static string GetStatusCss(int i)
        {
            string result = string.Empty;
            if (i == 0) result = "btn btn-rounded btn-danger btn-outline";
            else if (i == 1) result = "btn btn-rounded btn-success";
            else if (i == 2) result = "btn btn-rounded btn-warning";
            else if (i == 3) result = "btn btn-rounded btn-danger";
            else if (i == 4) result = "btn btn-rounded btn-primary";
            else result = "btn btn-rounded ";
            return result;
        }
        public static string GeturlModify(string id)
        {
            var list = new U_Url_ListBLL().QuerySql(string.Format(SqlStr.GetmyModalModify, id)).FirstOrDefault();
            return JsonConvert.SerializeObject(list);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}