using CommonLayer;
using CommonLayer.Enum;
using Kebue.UI.Models;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kebue.UI.Areas.Admin.Controllers
{
    /// <summary>
    /// AJAXRequestAPI
    /// </summary>
    public class AJAXRequestAPIController : BaseController
    {
        public string Report(int limit, int offset, string pare, string starTime, string endTime)
        {
            var p = new { method = Request.QueryString["method"].ToLower(), limit = limit, offset = offset, pare = pare, starTime = starTime, endTime = endTime };
            var result = HttpHelp.Post<dynamic>(ActionEnum.Report, p);
            return result;
        }
        [HttpPost]
        public ActionResult UrlIcomeDonwn()
        {
            Action action = new Action(_urlicomedonwn);
            action();
            Thread.Sleep(1000 * 6);
            return this.Json("执行成功", JsonRequestBehavior.AllowGet);
        }
        public void _urlicomedonwn()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    #region 图标下载
                    try
                    {
                        #region 有图标未下载的
                        var data = HttpHelp.Post<List<U_Url_List>, string>(ActionEnum.Find_Url, null);
                        var list = data.FindAll(n => n.IconImg.IsNotNull());
                        foreach (var item in list)
                        {
                            if (item.IconImg.IsNotNull() && System.IO.File.Exists(string.Format("{0}\\{1}", Images.GenerateIconsPath, item.IconImg)))
                            {
                                continue;
                            }
                            else if (item.IcomStream.IsNotNull())
                            {
                                item.IconImg = Images.StrConvertImage(item.IcomStream, item.Url);
                                HttpHelp.Post<int, U_Url_List>(ActionEnum.Update_Url, item);
                            }
                            else
                            {
                                item.IcomStream = Images.ImageDownloadData(item.IconImg);
                                item.IconImg = Images.StrConvertImage(item.IcomStream, item.Url);
                                HttpHelp.Post<int, U_Url_List>(ActionEnum.Update_Url, item);
                            }
                        }
                        #endregion
                        #region 下载过路径不正确重新生成
                        list = data.FindAll(n => n.IcomStream.IsNotNull());
                        foreach (var item in list)
                        {
                            if (item.IconImg.IsNotNull() && System.IO.File.Exists(string.Format("{0}\\{1}", Images.GenerateIconsPath, item.IconImg)))
                                continue;
                            item.IconImg = Images.StrConvertImage(item.IcomStream, item.Url);
                            HttpHelp.Post<int, U_Url_List>(ActionEnum.Update_Url, item);
                        }
                        #endregion
                    }
                    catch (System.Exception ex)
                    {
                        LogHelper.AddLogMsg(ex, "图标下载发生异常");
                    }
                    #endregion
                }
                catch { }
            });
        }



        [HttpPost]
        public ActionResult WebSiteSecurityCheck()
        {
            var result = HttpHelp.Post<string, dynamic>(ActionEnum.PerformTask, new { method = "web" });
            Thread.Sleep(1000 * 6);
            return this.Json("执行成功", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UrlCheck()
        {
            HttpHelp.Post<string, dynamic>(ActionEnum.PerformTask, new { method = "url" });
            Thread.Sleep(1000 * 6);
            return this.Json("执行成功", JsonRequestBehavior.AllowGet);
        }

        public void PerformTask()
        {
            Task.Factory.StartNew(() => { _urlicomedonwn(); });
            HttpHelp.Post<string, dynamic>(ActionEnum.PerformTask, new { method = "web" });
            HttpHelp.Post<string, dynamic>(ActionEnum.PerformTask, new { method = "url" });
        }
        #region URL信息修改
        [HttpPost]
        public ActionResult UpadateValue(string Id, string Value, int type)
        {
            string result = string.Empty;
            var entity = HttpHelp.Post<U_Url_List, dynamic>(ActionEnum.Find_Url, new { Id = Id });
            if (entity.IsNotNull())
            {
                entity.Id = Id;
                entity.LastUpdate_Id = CurrenUserInfo.UserAccount;
                entity.LastUpdate_Time = GetStr.GetCurrentDate;
                switch (type)
                {
                    case 1: entity.SortDesc = Convert.ToInt32(Value); break;
                    case 2: entity.Name = Value; break;
                    case 3: entity.Url = Value; break;
                    case 4: entity.Source = Value; break;
                    case 5: entity.Title = Value; break;
                }
                result = HttpHelp.Post<string, U_Url_List>(ActionEnum.UpadateValue, entity);
            }
            return this.Json(result, JsonRequestBehavior.AllowGet);
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
                        returnInfo = HttpHelp.Post<string, U_Url_List>(ActionEnum.AddUrl, t);
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
                returnInfo = HttpHelp.Post<string, U_Url_Type>(ActionEnum.UpdateType, t);
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
            int returnInfo = 0;
            if (toId.IsNotNull() && arryList != null && arryList.Length > 0)
            {
                var p = new CommRequestParameter { stringArry = arryList, Id = toId, UserId = CurrenUserInfo.UserAccount };
                returnInfo = HttpHelp.Post<int, CommRequestParameter>(ActionEnum.UrlTypeUpdate, p);
            }
            return returnInfo.ToString();
        }
        #region 修改URL状态
        [HttpPost]
        public ActionResult UpadateUrlStatus(string Id, int Status)
        {
            var entity = HttpHelp.Post<U_Url_List, dynamic>(ActionEnum.Find_Url, new { Id = Id });
            entity.Status = Status;
            entity.LastUpdate_Id = CurrenUserInfo != null ? CurrenUserInfo.UserAccount : "";
            entity.LastUpdate_Time = GetStr.GetCurrentDate;
            var result = HttpHelp.Post<string, U_Url_List>(ActionEnum.UpadateValue, entity);
            return this.Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region myModalModify
        [HttpPost]
        public string myModalModify(FormCollection fm)
        {
            string msg = "修改失败";
            U_Url_List u = HttpHelp.Post<U_Url_List, dynamic>(ActionEnum.Find_Url, new { Id = fm["hdId"] });
            u.Url = fm["txt_Url"];
            u.Name = fm["txt_Name"];
            u.Title = fm["txt_Desc"];
            u.Types = fm["txt_Type"];
            u.IconImg = fm["txt_IconImg"];
            u.LastUpdate_Id = base.UserCheck().UserAccount;
            u.LastUpdate_Time = GetStr.GetCurrentDate;
            var result = HttpHelp.Post<int, U_Url_List>(ActionEnum.UpadateValue, u);
            if (result > 0)
            {
                msg = "修改成功";
            }
            return msg;
        }
        #endregion
        #endregion











    }

}