using Business;
using CommonLayer;
using CommonLayer.Enum;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kebue.API.Areas.Admin.Controllers
{
    //Admin/Backstages
    /// <summary>
    /// 后台接口API
    /// </summary>
    public class BackstagesController : ApiController
    {
        #region URL信息修改
        /// <summary>
        /// URL信息修改
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Value"></param>
        /// <param name="type"></param>
        /// <param name="UserAccount"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpadateValue([FromBody]U_Url_List entity)
        {
            var result = new U_Url_ListBLL().Update(entity);
            return JsonConvert.SerializeObject(result).ToHttpResponseMessage();
        }
        #endregion

        #region 添加URL
        [HttpPost]
        public HttpResponseMessage AddUrl([FromBody]U_Url_List p)
        {
            U_Url_ListBLL bll_UrlList = new U_Url_ListBLL();
            U_Url_TypeBLL bllType = new U_Url_TypeBLL();
            string returnInfo = string.Empty;
            #region 添加网站
            try
            {
                if (!string.IsNullOrEmpty(p.Url) && !string.IsNullOrEmpty(p.Name))
                {
                    if (p.Url.IsUrlFormat())
                    {
                        if (bllType.Find(string.Format("AND ID='{0}'", p.Types)) != null)
                        {
                            if (bll_UrlList.Find(string.Format("AND Url='{0}'", p.Url)) == null)
                            {
                                if (!string.IsNullOrEmpty(p.IconImg))
                                {
                                    if (!p.IconImg.IsUrlFormat())
                                    {
                                        returnInfo = "网址LOG错误，不是Url地址，请检查";
                                    }
                                }
                                if (string.IsNullOrEmpty(returnInfo))
                                {
                                    p.Id = GetStr.GetGuid;
                                    bll_UrlList.Add(new List<U_Url_List> { p });
                                    returnInfo = "添加成功";
                                }
                            }
                            else
                                returnInfo = "网址已存在，请确认";
                        }
                        else
                            returnInfo = "添加的类型不存，请确认";
                    }
                    else
                        returnInfo = "网址地址，错误，请检查";
                }
                else
                    returnInfo = "网址地址或名称为空，请检查";
            }
            catch
            {
                returnInfo = "发生错误";
            }
            #endregion
            return JsonConvert.SerializeObject(returnInfo).ToHttpResponseMessage();
        }
        #endregion

        #region 修改类型
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UrlTypeUpdate([FromBody]JObject obj)
        {
            int result = 0;
            if (obj != null)
            {
                try
                {
                    dynamic p = obj;
                    var arryList = p.stringArry;
                    string toId = p.Id;
                    string UserAccount = p.UserId;
                    if (toId.IsNotNull() && UserAccount.IsNotNull())
                    {
                        var bll = new U_Url_ListBLL();
                        List<U_Url_List> list = new List<U_Url_List>();
                        foreach (string item in arryList)
                        {
                            var entity = bll.Query<U_Url_List>(string.Format("Id='{0}'", item));
                            if (entity.IsNull()) { continue; }
                            entity.Types = toId;
                            entity.LastUpdate_Id = UserAccount;
                            entity.LastUpdate_Time = GetStr.GetCurrentDate;
                            list.Add(entity);
                        }
                        result = bll.Update(list);
                    }
                }
                catch { }
            }
            return JsonConvert.SerializeObject(result).ToHttpResponseMessage();
        }
        public HttpResponseMessage UpdateType([FromBody]U_Url_Type t)
        {
            U_Url_TypeBLL bllType = new U_Url_TypeBLL();
            string returnInfo = string.Empty;
            try
            {
                bllType.Update(t);
                returnInfo = "修改成功";
            }
            catch (Exception ex)
            {
                returnInfo = string.Format("修改发生异常：Exception：{0}，StackTrace：{1}", ex.ToString(), ex.StackTrace);
            }
            return JsonConvert.SerializeObject(returnInfo).ToHttpResponseMessage();
        }
        #endregion

        #region 留言数据加载
        /// <summary>
        /// 留言数据加载
        /// </summary>
        /// <param name="type"></param>
        /// <param name="msg"></param>
        /// <param name="userAgent"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage LeaveAMessageAsync([FromBody]string type, string msg, string userAgent)
        {
            var result = LeaveAMessage(type, msg, userAgent);
            return JsonConvert.SerializeObject(result).ToHttpResponseMessage();
        }
        private HttpResponseMessage LeaveAMessage([FromBody]string type, string msg, string userAgent)
        {
            S_LogBLL log = new S_LogBLL();
            LeaveAMessageBLL bllmsg = new LeaveAMessageBLL();
            int status = 0;
            string info = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(msg))
                {
                    LeaveAMessage lmsg = new PhysicalLayer.LeaveAMessage();
                    lmsg.Id = GetStr.GetGuid;
                    lmsg.Name = type;
                    lmsg.Content = msg;
                    lmsg.UserAgent = userAgent;
                    lmsg.Create_Time = GetStr.GetCurrentDate;
                    bllmsg.Add(lmsg);
                    info = "成功反馈，请耐心等待回复，感谢支付与理解";
                    status = 1;
                }
                else
                {
                    info = "类型和内容不可为空";
                }
            }
            catch (Exception ex)
            {
                info = "添加发生异常，请重试，或者qq联系：434951775";
                log.AddLog(ex, string.Format("留言发生异常:传入参数是【type：{0}，msg：{1}，userAgent：{2}】", type, msg, userAgent));
            }
            return JsonConvert.SerializeObject(new { status = status, info = info }).ToHttpResponseMessage();
        }
        #endregion

        #region 网站后台首页统计数据
        /// <summary>
        /// 网站后台首页统计数据
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpPost]
        public HttpResponseMessage GetWebsiteStatistics()
        {
            List<UrlType> result = new U_Url_ListBLL().FinList<UrlType>(SqlStr.GetHomePageCount);
            return JsonConvert.SerializeObject(result).ToHttpResponseMessage();
        }
        #endregion

        #region Report
        [HttpPost]
        public HttpResponseMessage Report([FromBody]JObject obj)
        {
            dynamic parameter = obj;
            if (parameter != null)
            {
                string method = parameter.method;
                int limit = parameter.limit == null ? 0 : parameter.limit;
                int offset = parameter.offset == null ? 0 : parameter.offset;
                string pare = parameter.pare;
                string starTime = parameter.starTime;
                string endTime = parameter.endTime;
                Func<string, int, int, string, string, string, HttpResponseMessage> func = _reportresult;
                return func(method, limit, offset, pare, starTime, endTime);
            }
            else return null;

        }
        private HttpResponseMessage _reportresult(string method, int limit, int offset, string pare, string starTime, string endTime)
        {

            U_Url_ClickRateBLL clickRateBll = new U_Url_ClickRateBLL();
            object total = 0;
            object rows = 0;
            if (method.IsNotNull())
            {
                method = method.ToLower();
                switch (method)
                {
                    case "clickrate_curr":
                        if (string.IsNullOrEmpty(starTime))
                        {
                            DateTime now = DateTime.Now;
                            starTime = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMddmmHHss);
                        }
                        if (string.IsNullOrEmpty(endTime))
                            endTime = GetStr.GetCurrentDate;
                        var Currentresult = clickRateBll.FinList<U_Url_ClickRate>(string.Format(SqlStr.GetCuurentClickRate, starTime, endTime));
                        if (pare.IsNotNull())
                            Currentresult = Currentresult.FindAll(n => n.Id.IsContains(pare) || n.UrlId.IsContains(pare) || n.ClickDate.IsContains(pare) || n.UserAgents.IsContains(pare));
                        total = Currentresult.Count();
                        Currentresult = Currentresult.OrderByDescending(n => n.ClickDate).ToList();
                        rows = Currentresult.Skip(offset).Take(limit).ToList();
                        break;
                    case "getclickrateday":
                        if (string.IsNullOrEmpty(starTime))
                        {
                            DateTime now = DateTime.Now;
                            starTime = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMddmmHHss);
                        }
                        if (string.IsNullOrEmpty(endTime))
                            endTime = GetStr.GetCurrentDate;
                        var result = clickRateBll.FinList<_ClickRate>(string.Format(SqlStr.GetClickRateDay, starTime, endTime));
                        if (pare.IsNotNull())
                            result = result.FindAll(n => n.Url.IsContains(pare) || n.Name.IsContains(pare) || n.ClickDate.IsContains(pare));
                        total = result.Count();
                        result = result.OrderByDescending(n => n.ClickDate).ToList();
                        rows = result.Skip(offset).Take(limit).ToList();
                        break;
                    case "getclickratemonth":
                        if (string.IsNullOrEmpty(starTime))
                            starTime = DateTime.Now.AddYears(-1).ToString(GetStr.yyyyMMddmmHHss);
                        if (string.IsNullOrEmpty(endTime))
                            endTime = GetStr.GetCurrentDate;
                        var list = clickRateBll.FinList<_ClickRate>(string.Format(SqlStr.GetClickRateMonth, starTime, endTime));
                        if (pare.IsNotNull())
                            list = list.FindAll(n => n.Url.IsContains(pare) || n.Name.IsContains(pare) || n.ClickDate.IsContains(pare));
                        total = list.Count();
                        list = list.OrderByDescending(n => n.ClickDate).ToList();
                        rows = list.Skip(offset).Take(limit).ToList();
                        break;
                    case "check":
                        if (string.IsNullOrEmpty(starTime))
                        {
                            DateTime now = DateTime.Now;
                            starTime = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMddmmHHss);
                        }
                        if (string.IsNullOrEmpty(endTime))
                            endTime = GetStr.GetCurrentDate;
                        var resultCheck = clickRateBll.FinList<U_Url_Check>(string.Format(SqlStr.GetCheckResult, starTime, endTime));
                        if (pare.IsNotNull())
                            resultCheck = resultCheck.FindAll(n => n.Url.IsContains(pare) || n.Id.IsContains(pare) || n.UrlId.IsContains(pare) || n.Webstate.IsContains(pare) || n.Msg.IsContains(pare) || n.Result.IsContains(pare));
                        total = resultCheck.Count();
                        resultCheck = resultCheck.OrderByDescending(n => n.Create_Time).ToList();
                        rows = resultCheck.Skip(offset).Take(limit).ToList();
                        break;
                    case "log":
                        if (string.IsNullOrEmpty(starTime))
                        {
                            DateTime now = DateTime.Now;
                            starTime = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMddmmHHss);
                        }
                        if (string.IsNullOrEmpty(endTime))
                            endTime = GetStr.GetCurrentDate;
                        var Logresult = clickRateBll.FinList<S_Log>(string.Format(SqlStr.LogSql, starTime, endTime));
                        if (pare.IsNotNull())
                            Logresult = Logresult.FindAll(n => n.Category.IsContains(pare) || n.Id.IsContains(pare) || n.Create_Time.IsContains(pare) || n.Module.IsContains(pare) || n.Log_Type.IsContains(pare) || n.Msg.IsContains(pare) || n.SubCategory.IsContains(pare));
                        total = Logresult.Count();
                        Logresult = Logresult.OrderByDescending(n => n.Create_Time).ToList();
                        rows = Logresult.Skip(offset).Take(limit).ToList();
                        break;
                    case "geturlresult":
                        List<UrlList> lstRes = new List<UrlList>();
                        if (endTime.IsNotNull() && endTime == "undefined")
                        {
                            endTime = null;
                        }
                        lstRes = _geturlalldata(starTime, endTime);
                        if (pare.IsNotNull())

                        {
                            lstRes = lstRes.FindAll(n =>
                                        n.Url.IsContains(pare) ||
                                        n.Name.IsContains(pare) ||
                                        n.Id.IsContains(pare) ||
                                        n.Operation.IsContains(pare) ||
                                        n.SortDesc.IsContains(pare) ||
                                        n.Source.IsContains(pare) ||
                                        n.Status.IsContains(pare) ||
                                        n.Title.IsContains(pare) ||
                                        n.Types.IsContains(pare));
                        }
                        total = lstRes.Count();
                        rows = lstRes.Skip(offset).Take(limit).ToList();
                        break;
                }
            }
            return JsonConvert.SerializeObject(new { total = total, rows = rows }).ToHttpResponseMessage();
        }
        #endregion

        #region ProcessRequest
        [HttpPost]
        public HttpResponseMessage ProcessRequest([FromBody]JObject obj)
        {
            dynamic parameter = obj;
            if (parameter != null)
            {
                string p = parameter.method;
                string p1 = parameter.parameter;
                string p2 = parameter.parameter2;
                Func<string, string, string, HttpResponseMessage> func = _processrequest;
                return func(p, p1, p2);
            }
            return null;
        }
        private HttpResponseMessage _processrequest(string method, string parameter, string parameter2)
        {
            string returnInfo = string.Empty;
            if (method.IsNotNull())
            {
                U_Url_ClickRateBLL clickRateBll = new U_Url_ClickRateBLL();
                U_Url_TypeBLL bllType = new U_Url_TypeBLL();
                U_Url_ListBLL bllMenuList = new U_Url_ListBLL();
                U_Url_ListBLL bll = new U_Url_ListBLL();
                switch (method)
                {
                    case "geturlalldata":
                        returnInfo = JsonConvert.SerializeObject(_geturlalldata(parameter, parameter2));
                        break;
                    case "getcurrentobj":
                        if (!string.IsNullOrEmpty(parameter))
                            returnInfo = JsonConvert.SerializeObject(bll.Find(string.Format("AND ID='{0}'", parameter)));
                        break;
                    case "gettypedata":
                        returnInfo = JsonConvert.SerializeObject(UrlTypeBLL.UrlTypeListManager.OrderByDescending(n => n.SortDesc));
                        break;
                    case "geturlentity":
                        if (!string.IsNullOrEmpty(parameter))
                        {
                            returnInfo = JsonConvert.SerializeObject(bll.FindList(string.Format("AND Types='{0}'", parameter)).OrderBy(n => n.SortDesc));
                        }
                        break;
                    case "addclickrate":
                        U_Url_ClickRate rate = new U_Url_ClickRate();
                        //rate.Id = GetStr.GetGuid;
                        //rate.UserAgents = context.Request.QueryString["b"];
                        //rate.UrlId = context.Request.QueryString["a"];
                        //rate.Ip = context.Request.QueryString["c"];
                        //rate.Msg = context.Request.QueryString["d"];
                        //rate.ClickDate = GetStr.GetCurrentDate;
                        //if (!string.IsNullOrEmpty(rate.UserAgents) && !string.IsNullOrEmpty(rate.UrlId))
                        //{
                        //    clickRateBll.Add(rate);
                        //}
                        break;
                    case "geturlmodify":
                        returnInfo = JsonConvert.SerializeObject(new U_Url_ListBLL().QuerySql(string.Format(SqlStr.GetmyModalModify, parameter)).FirstOrDefault());
                        break;
                    case "getclickrateday":
                        if (string.IsNullOrEmpty(parameter))
                        {
                            DateTime now = DateTime.Now;
                            parameter = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMddmmHHss);
                        }
                        if (string.IsNullOrEmpty(parameter2))
                            parameter2 = GetStr.GetCurrentDate;
                        returnInfo = JsonConvert.SerializeObject(clickRateBll.FinList<_ClickRate>(string.Format(SqlStr.GetClickRateDay, parameter, parameter2)));
                        break;
                    case "getclickratemonth":
                        if (string.IsNullOrEmpty(parameter))
                            parameter = DateTime.Now.AddYears(-1).ToString(GetStr.yyyyMMddmmHHss);
                        if (string.IsNullOrEmpty(parameter2))
                            parameter2 = GetStr.GetCurrentDate;
                        returnInfo = JsonConvert.SerializeObject(clickRateBll.FinList<_ClickRate>(string.Format(SqlStr.GetClickRateMonth, parameter, parameter2)));
                        break;
                    case "getcheckresult":
                        if (string.IsNullOrEmpty(parameter))
                        {
                            DateTime now = DateTime.Now;
                            parameter = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMddmmHHss);
                        }
                        if (string.IsNullOrEmpty(parameter2))
                            parameter2 = GetStr.GetCurrentDate;
                        returnInfo = JsonConvert.SerializeObject(clickRateBll.FinList<U_Url_Check>(string.Format(SqlStr.GetCheckResult, parameter, parameter2)));
                        break;
                    case "leaveamessage":
                        if (string.IsNullOrEmpty(parameter))
                        {
                            DateTime now = DateTime.Now;
                            parameter = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMddmmHHss);
                        }
                        if (string.IsNullOrEmpty(parameter2))
                            parameter2 = GetStr.GetCurrentDate;
                        returnInfo = JsonConvert.SerializeObject(clickRateBll.FinList<LeaveAMessage>(string.Format(SqlStr.LeaveAMessage, parameter, parameter2)));
                        break;
                    case "log":
                        if (string.IsNullOrEmpty(parameter))
                        {
                            DateTime now = DateTime.Now;
                            parameter = new DateTime(now.Year, now.Month, 1).ToString(GetStr.yyyyMMddmmHHss);
                        }
                        if (string.IsNullOrEmpty(parameter2))
                            parameter2 = GetStr.GetCurrentDate;
                        returnInfo = JsonConvert.SerializeObject(clickRateBll.FinList<S_Log>(string.Format(SqlStr.LogSql, parameter, parameter2)));
                        break;
                }
            }
            return JsonConvert.SerializeObject(returnInfo).ToHttpResponseMessage();
        }
        [HttpPost]
        public HttpResponseMessage GetJoinTypeFind([FromBody]JObject obj)
        {
            List<U_Url_List> result = new List<U_Url_List>();
            U_Url_ListBLL bll = new U_Url_ListBLL();
            if (obj != null)
            {
                dynamic p = obj;
                int i = p.Status;
                result = bll.JoinTypeFind().FindAll(n => n.Status == i);
            }
            else
            {
                result = bll.JoinTypeFind();
            }
            return JsonConvert.SerializeObject(result).ToHttpResponseMessage();
        }
        private List<UrlList> _geturlalldata(string parameter, string parameter2)
        {
            U_Url_ListBLL bll = new U_Url_ListBLL();
            if (parameter == "undefined")
            {
                parameter = "1";
            }
            if (string.IsNullOrEmpty(parameter))
            {
                parameter2 = null;
            }
            List<UrlList> list = new List<UrlList>();
            var i = 0;
            int.TryParse(parameter, out i);
            var result = bll.JoinTypeFind().FindAll(n => n.Status == i);
            if (!string.IsNullOrEmpty(parameter2))
                result = result.FindAll(n => n.Url.Contains(parameter2) || n.Id.Contains(parameter2) || n.Name.Contains(parameter2) || n.Types.Contains(parameter2));
            foreach (var item in result)
            {
                UrlList e = new UrlList();
                string statusCss = string.Empty;
                if (item.Status == 0) statusCss = "btn btn-rounded btn-danger btn-outline";
                else if (item.Status == 1) statusCss = "btn btn-rounded btn-success";
                else if (item.Status == 2) statusCss = "btn btn-rounded btn-warning";
                else if (item.Status == 3) statusCss = "btn btn-rounded btn-danger";
                else if (item.Status == 4) statusCss = "btn btn-rounded btn-primary";
                else statusCss = "btn btn-rounded ";
                e.Id = item.Id;
                e.Name = string.Format("<div class=\"input-group\"><input type=\"text\" value=\"{0}\" class=\"form-control\" placeholder=\"名称……\" id=\"txtName_{1}\"><span class=\"input-group-btn\"><button class=\"btn btn-default\" type=\"button\" onclick=\"UrlNameUpdateSave('{1}')\">Go</button></span></div>", item.Name, item.Id);
                e.Url = string.Format("<div class=\"input-group\"><input type=\"text\" value=\"{0}\" class=\"form-control\" placeholder=\"网址……\" id=\"txtUrl_{1}\"><span class=\"input-group-btn\"><button class=\"btn btn-default\" type=\"button\" onclick=\"UrlUpdateSave('{1}')\">Go</button></span></div>", item.Url, item.Id);
                e.Source = string.Format("<div class=\"input-group\"><input type=\"text\" value=\"{0}\" class=\"form-control\" placeholder=\"网址描述……\" id=\"txtSource_{1}\"><span class=\"input-group-btn\"><button class=\"btn btn-default\" type=\"button\" onclick=\"UrlSourceUpdateSave('{1}')\">Go</button></span></div>", item.Source, item.Id);
                //e.Title = string.Format("<div class=\"input-group\"><input type=\"text\" value=\"{0}\" class=\"form-control\" placeholder=\"Title……\" id=\"txtTitle_{1}\"><span class=\"input-group-btn\"><button class=\"btn btn-default\" type=\"button\" onclick=\"UrlTitleUpdateSave('{1}')\">Go</button></span></div>", item.Title, item.Id);
                e.Types = string.Format("<a onclick=\"myModalModify('{0}')\">{1}</a>", item.Id, item.Types);
                e.SortDesc = string.Format("<div class=\"input-group\"><input type=\"text\" value=\"{0}\" class=\"form-control\" placeholder=\"降序排列……\" id=\"txtSort_{1}\"><span class=\"input-group-btn\"><button class=\"btn btn-default\" type=\"button\" onclick=\"btnSortSave('{1}')\">Go</button></span></div>", item.SortDesc.ToString(), item.Id);
                e.Status = string.Format("<button class=\"{2}\" id='btnStatus{0}' onclick=\"myStatusEdit('{0}');\">{1}</button>", item.Id, item.Status.ToEnumDiscription<EnumUrlStatus>(), statusCss);
                e.Operation = string.Format("<a class=\"btn btn-info btn-circle\" href=\"{0}\" target=\"_blank\" title=\"{1}\" id=\"txtfwxtest_{2}\" onclick=\"BtnClearClass('{2}')\"> <i class=\"glyphicon glyphicon-ok\"></i></a>", item.Url, item.Title, item.Id);
                list.Add(e);
            }
            return list;
        }
        #endregion

        #region GetU_Url_TypeList
        [HttpPost]
        public HttpResponseMessage GetU_Url_TypeList()
        {
            var result = new U_Url_TypeBLL().Find();
            return JsonConvert.SerializeObject(result).ToHttpResponseMessage();
        }
        #endregion

        #region 任务
        public HttpResponseMessage PerformTask([FromBody]JObject obj)
        {
            dynamic parameter = obj;
            if (parameter != null)
            {
                string method = parameter.method.ToString().ToLower();
                switch (method)
                {
                    case "web":
                        Task.Factory.StartNew(() => WebSiteSecurity.Request(true));
                        break;
                    case "icom":
                        string res = string.Empty;
                        U_Url_ListBLL bll_UrlList = new U_Url_ListBLL();
                        Task.Factory.StartNew(() => _urlcheckresult("SystemAuto"));
                        break;
                }
            }
            return JsonConvert.SerializeObject("执行成功").ToHttpResponseMessage();
        }

        #region U_Url_ListBLL
        public HttpResponseMessage Find_Url([FromBody]JObject obj)
        {
            dynamic p = obj;
            if (p != null && p.Id != null)
            {
                string where = string.Format("Id=='{0}'", p.Id);
                return JsonConvert.SerializeObject(new U_Url_ListBLL().Find(where)).ToHttpResponseMessage();
            }
            else
            {
                return JsonConvert.SerializeObject(new U_Url_ListBLL().Find()).ToHttpResponseMessage();
            }
        }

        public HttpResponseMessage Update_Url(U_Url_List t)
        {
            var result = new U_Url_ListBLL().Update(t);
            return JsonConvert.SerializeObject(result).ToHttpResponseMessage();
        }
        #endregion
        public HttpResponseMessage Add_Log(S_Log t)
        {
            var result = new S_LogBLL().Add(t);
            return JsonConvert.SerializeObject(result).ToHttpResponseMessage();
        }
        public HttpResponseMessage Find_Config()
        {
            var result = new S_ConfigBLL().Find();
            return JsonConvert.SerializeObject(result).ToHttpResponseMessage();
        }


        private void _urlcheckresult(string userId)
        {
            U_Url_ListBLL bll_UrlList = new U_Url_ListBLL();
            var list = bll_UrlList.Find().FindAll(n => n.Status == EnumUrlStatus.Normal.GetHashCode());
            #region foreach
            foreach (var item in list)
            {
                try
                {
                    if (!item.Url.UrlVerification())
                    {
                        bll_UrlList.UpdateStatus(item.Id, EnumUrlStatus.NotDefined.GetHashCode(), userId);
                    }
                    if (item.IconImg.IsNotNull()
                        && !System.IO.File.Exists(string.Format("{0}\\{1}", Images.GenerateIconsPath, item.IconImg))
                        && !item.IcomStream.IsNotNull()
                        && !item.IconImg.UrlVerification())
                    {
                        item.IconImg = "";
                        item.LastUpdate_Id = userId;
                        bll_UrlList.Update(item);
                    }
                }
                catch { }
            }
            #endregion
        }






        #endregion
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}