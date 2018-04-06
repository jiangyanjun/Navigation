using Business;
using CommonLayer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Kebue.API.Areas.Navigation.Controllers
{
    /// <summary>
    /// 导航首页
    /// </summary>
    public class HomeController : ApiController
    {
        #region TypesScript
        /// <summary>
        /// TypesScript
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public Task<HttpResponseMessage> TypesScript()
        {
            return Task.Factory.StartNew(() =>
            {
                return JsonConvert.SerializeObject(UrlTypeBLL.TypesScript).ToHttpResponseMessage();
            });
        }
        #endregion

        #region SO 首页数据加载
        /// <summary>
        /// SO 首页数据加载
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage GeturlDaras([FromBody]JObject obj)
        {
            var p = new RequestNavigateEntity();
            dynamic parameter = obj;
            if (parameter != null)
            {
                int index = parameter.Parameter;
                var types = UrlTypeBLL.Types;
                if (index < types.Count())
                {
                    p.Type = types.ToList()[index];
                    p.UrlList = new U_Url_ListBLL().QuerySql(string.Format(SqlStr.GetHomeUrlList, p.Type.Id));
                }
            }
            return JsonConvert.SerializeObject(p).ToHttpResponseMessage();
        }
        #endregion

        #region Addclickrate
        /// <summary>
        /// Addclickrate
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Addclickrate([FromBody]U_Url_ClickRate rate)
        {
            int res = 0;
            if (!string.IsNullOrEmpty(rate.UserAgents) && !string.IsNullOrEmpty(rate.UrlId))
            {
                res = new U_Url_ClickRateBLL().Add(rate);
            }
            return JsonConvert.SerializeObject(res).ToHttpResponseMessage();
        }
        #endregion

        #region 留言数据加载
        /// <summary>
        /// 留言数据加载
        /// </summary>
        /// <param name="lmsg"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage LeaveAMessage([FromBody]LeaveAMessage lmsg)
        {
            int status = 0;
            string info = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(lmsg.Name) && !string.IsNullOrEmpty(lmsg.Content))
                {
                    new LeaveAMessageBLL().Add(lmsg);
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
                //  log.AddLog(ex, string.Format("留言发生异常:传入参数是【type：{0}，msg：{1}，userAgent：{2}】", type, msg, userAgent));
            }
            return JsonConvert.SerializeObject(new { status = status, info = info }).ToHttpResponseMessage();
        }
        #endregion

    }
}