using Business;
using CommonLayer;
using CommonLayer.Enum;
using Mmmxa.So.Content.Ajax;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mmmxa.So.Controllers
{
    /// <summary>
    /// AJAXRequestAPI
    /// </summary>
    public class AJAXRequestAPIController : BaseController
    {
        #region 变量定义
        S_LogBLL log = new S_LogBLL();
        LeaveAMessageBLL bllmsg = new LeaveAMessageBLL();
        U_Url_TypeBLL bllType = new U_Url_TypeBLL();
        U_Url_ListBLL bll_UrlList = new U_Url_ListBLL();
        #endregion

        #region 修改URL状态
        [HttpPost]
        public ActionResult UpadateUrlStatus(string Id, int Status)
        {
            var result = bll_UrlList.UpdateStatus(Id, Status, CurrenUserInfo.UserAccount);
            return this.Json(Status.ToEnumDiscription<EnumUrlStatus>(), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region URL信息修改
        [HttpPost]
        public ActionResult UpadateValue(string Id, string Value, int type)
        {
            var entity = new U_Url_List() { Id = Id, LastUpdate_Id = CurrenUserInfo.UserAccount, LastUpdate_Time = GetStr.GetCurrentDate };
            switch (type)
            {
                case 1: entity.SortDesc = Convert.ToInt32(Value); break;
                case 2: entity.Name = Value; break;
                case 3: entity.Url = Value; break;
                case 4: entity.Source = Value; break;
                case 5: entity.Title = Value; break;
            }
            var result = bll_UrlList.Update(entity);
            return this.Json("1", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 添加URL
        [HttpPost]
        public ActionResult AddUrl(string Url, string Name, string IconImg, string Title, string Types)
        {
            string returnInfo = string.Empty;
            #region 添加网站
            try
            {
                U_Url_List t = new U_Url_List();
                t.Url = Url;
                t.Name = Ext.FilterMark(Name);
                t.IconImg = IconImg;
                t.Title = Ext.FilterMark(Title);
                t.Types = Ext.FilterMark(Types);
                t.Status = EnumUrlStatus.New.GetHashCode();
                t.Source = "用户添加";
                t.Create_Time = GetStr.GetCurrentDate;
                if (!string.IsNullOrEmpty(t.Url) && !string.IsNullOrEmpty(t.Name))
                {
                    if (t.Url.IsUrlFormat())
                    {
                        if (bllType.Find(string.Format("AND ID='{0}'", t.Types)) != null)
                        {
                            if (bll_UrlList.Find(string.Format("AND Url='{0}'", t.Url)) == null)
                            {
                                if (!string.IsNullOrEmpty(t.IconImg))
                                {
                                    if (!t.IconImg.IsUrlFormat())
                                    {
                                        returnInfo = "网址LOG错误，不是Url地址，请检查";
                                    }
                                }
                                if (string.IsNullOrEmpty(returnInfo))
                                {
                                    t.Id = GetStr.GetGuid;
                                    bll_UrlList.Add(new List<U_Url_List> { t });
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
            return this.Json(returnInfo, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 修改类型
        [HttpPost]
        public ActionResult UpdateType(string Id, string Name, string Status, int sort)
        {
            string returnInfo = string.Empty;
            try
            {
                U_Url_Type t = new U_Url_Type();
                t.Id = Id;
                t.Name = Name;
                t.SortDesc = sort;
                t.LastUpdate_Time = GetStr.GetCurrentDate;
                t.LastUpdate_Id = CurrenUserInfo.UserAccount;
                t.Status = Status.ToEnumByDescription<EnumType>();
                bllType.Update(t);
                returnInfo = "修改成功";
            }
            catch (Exception ex)
            {
                returnInfo = string.Format("修改发生异常：Exception：{0}，StackTrace：{1}", ex.ToString(), ex.StackTrace);
            }
            return this.Json(returnInfo, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string UrlTypeUpdate(string[] arryList, string toId)
        {
            if (toId.IsNotNull() && arryList != null && arryList.Length > 0)
                foreach (var item in arryList)
                {
                    if (item.IsNotNull())
                        bll_UrlList.UpdateTypes(toId, item, CurrenUserInfo.UserAccount);
                }
            return "1";
        }
        #endregion

        #region SO 首页数据加载
        [HttpPost]
        public ActionResult GeturlDaras(int index = 0)
        {
            Func<int, Tuple<int, string>> func = GeturlDarasResult;
            var result = func(index);// Json(func(index), JsonRequestBehavior.AllowGet);
            return this.Json(new { nex = result.Item1, msg = result.Item2 }, JsonRequestBehavior.AllowGet);
        }
        private Tuple<int, string> GeturlDarasResult(int index = 0)
        {
            int nex;
            string msg = string.Empty;
            #region 变量定义
            StringBuilder str = new StringBuilder();
            StringBuilder str2 = new StringBuilder();// g-br13
            var img = "<img src=\"../{0}\" alt=\"www.kebue.com\" style=\"max-width:25px;max-height:18px;\"/>";
            var header = "<div class=\"row kebue_t g-grid g-bg11 govsite-top\"><div class=\"g-gc g-fc20 g-brwr title kebue_h\" {0}>{1}</div>{2}</div></div>";
            var html = "<div class=\"g-gc\"><a href='{2}' title='{3}' class=\"lba\" target=\"_blank\" onclick=\"clickrate('{0}')\">{4}{1}</a></div>";
            #endregion
            if (index < UrlTypeBLL.Types.Count())
            {
                var type = UrlTypeBLL.Types.ToList()[index];
                var list = new U_Url_ListBLL().QuerySql(string.Format(SqlStr.GetHomeUrlList, type.Id));
                int i = 0;
                bool IsH = true;
                foreach (var item in list)
                {
                    string resImg = string.Empty;
                    #region name
                    string name = item.Name;
                    if (name.GetStrLength() > 10)
                        name = name.StrCut(10);
                    #endregion
                    #region title
                    string title = item.Name;
                    if (!string.IsNullOrEmpty(item.Title))
                        title = title + "    " + item.Title;
                    #endregion
                    #region 格式组装
                    if (item.IconImg.IsNotNull() && System.IO.File.Exists(string.Format("{0}\\{1}", Images.GenerateIconsPath, item.IconImg)))
                        resImg = string.Format(img + "&nbsp;&nbsp;", Images.GetIcomPath + item.IconImg);
                    str.AppendFormat(html, item.Id, name, item.Url, title, resImg);
                    i++;
                    if (IsH && i == 11 || (list.Count() <= 11 && list.Count() == i))
                    {
                        str2.AppendFormat("{0}</div></div>", string.Format(header, string.Format("id=dw_{0}", type.Id), type.Name, str.ToString()));
                        str.Length = 0;
                        str.Clear();
                    }
                    else if ((i % 11 == 0 && i > 11) || (i == list.Count()))
                    {
                        str2.AppendFormat("{0}</div></div>", string.Format(header, "", "", str.ToString()));
                        str.Length = 0;
                        str.Clear();
                    }
                    #endregion
                }
                var htmlContent = str2.ToString();
                if (htmlContent.Length > 10 && !htmlContent.Contains("<hr/>"))
                    htmlContent = htmlContent + "<hr/>";
                nex = index + 1;
                msg = htmlContent;
            }
            else
            {
                nex = 0;
                msg = "";
            }
            return new Tuple<int, string>(nex, msg);
        }
        #endregion

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
                clickRateBll.Add(rate);
            }
        }

        #region 留言数据加载
        [HttpPost]
        public ActionResult LeaveAMessage(string type, string msg, string userAgent)
        {
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
            return this.Json(new { status = status, info = info }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region myModalModify
        [HttpPost]
        public string myModalModify(FormCollection fm)
        {
            U_Url_List u = new U_Url_List();
            u.Id = fm["hdId"];
            u.Url = fm["txt_Url"];
            u.Name = fm["txt_Name"];
            u.Title = fm["txt_Desc"];
            u.Types = fm["txt_Type"];
            u.IconImg = fm["txt_IconImg"];
            u.LastUpdate_Id = base.UserCheck().UserAccount;
            u.LastUpdate_Time = GetStr.GetCurrentDate;
            bll_UrlList.Update(u);
            return "修改成功";
        }
        #endregion

        #region 网址有效性检测
        [HttpPost]
        public ActionResult UrlCheck()
        {
            Func<ActionResult> func = UrlCheckResult;
            return func();
        }
        [HttpPost]
        public ActionResult UrlIcomeDonwn()
        {
            Thread.Sleep(1000 * 6);
            return this.Json("执行成功", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult WebSiteSecurityCheck()
        {
            Task.Factory.StartNew(() => WebSiteSecurity.Request(true));
            Thread.Sleep(1000 * 6);
            return this.Json("执行成功", JsonRequestBehavior.AllowGet);
        }
        private ActionResult UrlCheckResult()
        {
            string res = string.Empty;
            try
            {

                Task.Factory.StartNew(() => WebSiteSecurity.Request());
                int i = 0, j = 0, h = 0;
                var list = bll_UrlList.Find().FindAll(n => n.Status == EnumUrlStatus.Normal.GetHashCode());
                foreach (var item in list)
                {
                    i++;
                    if (!item.Url.UrlVerification())
                    {
                        j++;
                        bll_UrlList.UpdateStatus(item.Id, EnumUrlStatus.NotDefined.GetHashCode(), this.CurrenUserInfo.UserAccount);
                    }
                    if (item.IconImg.IsNotNull()
                        && !System.IO.File.Exists(string.Format("{0}\\{1}", Images.GenerateIconsPath, item.IconImg))
                        && !item.IcomStream.IsNotNull()
                        && !item.IconImg.UrlVerification())
                    {
                        h++;
                        item.IconImg = "";
                        item.LastUpdate_Id = this.CurrenUserInfo.UserAccount;
                        bll_UrlList.Update(item);
                    }
                }
                res = string.Format("本次检测{0}条数据，其中网址{1}条访问异常，图片网址{2}条访问异常", i, j, h);
            }
            catch (Exception ex)
            {
                res = string.Format("本次检测发生异常：Exception：{0}     StackTrace：{1}", ex.ToString(), ex.StackTrace);
            }
            return this.Json(res, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Boostrap Table
        HandlerServices hd = new HandlerServices();
        U_Url_ClickRateBLL clickRateBll = new U_Url_ClickRateBLL();
        public JsonResult GetUrl(int limit, int offset, string paramss, string statuSearch, string filter)
        {
            Func<int, int, string, string, string, JsonResult> func = GetUrlResult;
            return func(limit, offset, paramss, statuSearch, filter);
        }

        public JsonResult Report(int limit, int offset, string pare, string starTime, string endTime)
        {
            Func<int, int, string, string, string, JsonResult> func = ReportResult;
            return func(limit, offset, pare, starTime, endTime);
        }

        public JsonResult GetUrlResult(int limit, int offset, string paramss, string statuSearch, string filter)
        {
            List<UrlList> lstRes = new List<UrlList>();
            if (filter.IsNotNull() && filter != "undefined")
                lstRes = hd.GetMenuList(Convert.ToInt16(statuSearch), filter);
            else
                lstRes = hd.GetMenuList(Convert.ToInt16(statuSearch), null);
            object total = 0;
            object rows = 0;
            if (paramss.IsNotNull())

            {
                lstRes = lstRes.FindAll(n =>
                            n.Url.IsContains(paramss) ||
                            n.Name.IsContains(paramss) ||
                            n.Id.IsContains(paramss) ||
                            n.Operation.IsContains(paramss) ||
                            n.SortDesc.IsContains(paramss) ||
                            n.Source.IsContains(paramss) ||
                            n.Status.IsContains(paramss) ||
                            n.Title.IsContains(paramss) ||
                            n.Types.IsContains(paramss));
            }
            total = lstRes.Count();
            rows = lstRes.Skip(offset).Take(limit).ToList();
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        private JsonResult ReportResult(int limit, int offset, string pare, string starTime, string endTime)
        {
            object total = 0;
            object rows = 0;
            string method = Request.QueryString["method"].ToLower();
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
            }
            return Json(new { total = total, rows = rows }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}