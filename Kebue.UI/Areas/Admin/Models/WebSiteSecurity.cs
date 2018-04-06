using Business;
using CommonLayer;
using CommonLayer.Enum;
using Newtonsoft.Json;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kebue.UI
{
    /// <summary>
    /// 网站安全检测
    public class WebSiteSecurity
    {
        static S_LogBLL bll_LogBLL = new S_LogBLL();
        static S_ConfigBLL bll_ConfigBLL = new S_ConfigBLL();
        static U_Url_ListBLL bllU_Url_List = new U_Url_ListBLL();
        static U_Url_CheckBLL bllU_Url_Check = new U_Url_CheckBLL();
        public static void Request(bool IsConfig = false)
        {
            Task.Factory.StartNew(() =>
            {
                #region 后台异步处理事务
                string result = string.Empty;
                var configEntity = bll_ConfigBLL.Find().FindAll(n => n.Keys == "url_check").FirstOrDefault();
                if ((configEntity != null && configEntity.Value != GetStr.GetCurrentDay) || IsConfig)
                {
                    string sql = @"SELECT * FROM U_URL_LIST WHERE ID NOT IN (SELECT URLID FROM U_URL_CHECK) AND STATUS=1";
                    List<U_Url_List> t = bllU_Url_List.QuerySql(sql);
                    if (t != null && t.Count > 0)
                        foreach (var item in t)
                        {
                            #region 处理
                            try
                            {
                                U_Url_Check entity = new U_Url_Check();
                                entity.Url = item.Url;
                                if (entity.Url.Contains("."))
                                    entity.Url = entity.Url.Substring(entity.Url.IndexOf(".") + 1);
                                if (entity.Url.Contains("/"))
                                    entity.Url = entity.Url.Substring(0, entity.Url.IndexOf("/"));
                                if (entity.Url.IsUrlFormat2())
                                {
                                    var parameters1 = new Dictionary<string, string>();
                                    parameters1.Add("domain", entity.Url); //域名如:juhe.cn ,1jing.com
                                    parameters1.Add("dtype", "json"); //返回类型,xml/json/jsonp可选
                                    parameters1.Add("key", "cfdc0e6ed4b1fc85ecf6635066c179d6");//你申请的key
                                    result = RequestMethod.SendRequest("http://apis.juhe.cn/webscan/", parameters1, false);
                                    if (result.IsNotNull() && result.Contains("200") && result.ToLower().Contains("successed"))
                                    {
                                        WebSiteSecurityTest security = JsonConvert.DeserializeObject<WebSiteSecurityTest>(result);
                                        entity.Id = GetStr.GetGuid;
                                        entity.Create_Time = GetStr.GetCurrentDate;
                                        entity.Result = result;
                                        entity.Webstate = security.result.webstate;
                                        entity.Msg = security.result.msg;
                                        entity.UrlId = item.Id;
                                        bllU_Url_Check.Add(new List<U_Url_Check> { entity });
                                    }
                                    else if (result.Contains("\"error_code\":10012"))
                                    {
                                        configEntity.Value = GetStr.GetCurrentDay;
                                        bll_ConfigBLL.Update(configEntity);
                                        bll_LogBLL.AddLog(result, EnumLogType.Info, "调用聚和数据接口", "网站安全检测", "当日请求超过次数限制");
                                        break;
                                    }
                                }
                                System.Threading.Thread.Sleep(1000);
                            }
                            catch (Exception ex) { bll_LogBLL.AddLog(ex, result, "调用聚和数据接口", "网站安全检测", "发生异常"); }
                            #endregion
                        }
                }
                #endregion
            });
        }
        public static void Check()
        {

        }
    }

    public class WebSiteSecurityTest
    {
        /// <summary>
        /// 200
        /// </summary>
        public string resultcode { get; set; }
        /// <summary>
        /// Successed!
        /// </summary>
        public string reason { get; set; }
        /// <summary>
        /// Result
        /// </summary>
        public WebSiteSecurityTestResult result { get; set; }
        /// <summary>
        /// Error_code
        /// </summary>
        public int error_code { get; set; }
    }
    public class WebSiteSecurityTestLoudong
    {
        /// <summary>
        /// High
        /// </summary>
        public string high { get; set; }
        /// <summary>
        /// Mid
        /// </summary>
        public string mid { get; set; }
        /// <summary>
        /// Low
        /// </summary>
        public string low { get; set; }
        /// <summary>
        /// Info
        /// </summary>
        public string info { get; set; }
    }

    public class WebSiteSecurityTestGuama
    {
        /// <summary>
        /// Level
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 没有挂马或恶意内容
        /// </summary>
        public string msg { get; set; }
    }

    public class WebSiteSecurityTestXujia
    {
        /// <summary>
        /// Level
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 不是虚假或欺诈网站
        /// </summary>
        public string msg { get; set; }
    }

    public class WebSiteSecurityTestCuangai
    {
        /// <summary>
        /// Level
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 未篡改
        /// </summary>
        public string msg { get; set; }
    }

    public class WebSiteSecurityTestPangzhu
    {
        /// <summary>
        /// Level
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 没有旁注
        /// </summary>
        public string msg { get; set; }
    }

    public class WebSiteSecurityTestScore
    {
        /// <summary>
        /// 未知
        /// </summary>
        public string score { get; set; }
        /// <summary>
        /// 未知
        /// </summary>
        public string msg { get; set; }
    }

    public class WebSiteSecurityTestViolation
    {
        /// <summary>
        /// Level
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 未存在违规内容
        /// </summary>
        public string msg { get; set; }
    }

    public class WebSiteSecurityTestGoogle
    {
        /// <summary>
        /// Level
        /// </summary>
        public int level { get; set; }
        /// <summary>
        /// 没有google搜索屏蔽
        /// </summary>
        public string msg { get; set; }
    }

    public class WebSiteSecurityTestData
    {
        /// <summary>
        /// Loudong
        /// </summary>
        public WebSiteSecurityTestLoudong loudong { get; set; }
        /// <summary>
        /// Guama
        /// </summary>
        public WebSiteSecurityTestGuama guama { get; set; }
        /// <summary>
        /// Xujia
        /// </summary>
        public WebSiteSecurityTestXujia xujia { get; set; }
        /// <summary>
        /// Cuangai
        /// </summary>
        public WebSiteSecurityTestCuangai cuangai { get; set; }
        /// <summary>
        /// Pangzhu
        /// </summary>
        public WebSiteSecurityTestPangzhu pangzhu { get; set; }
        /// <summary>
        /// Score
        /// </summary>
        public WebSiteSecurityTestScore score { get; set; }
        /// <summary>
        /// Violation
        /// </summary>
        public WebSiteSecurityTestViolation violation { get; set; }
        /// <summary>
        /// Google
        /// </summary>
        public WebSiteSecurityTestGoogle google { get; set; }
    }

    public class WebSiteSecurityTestResult
    {
        /// <summary>
        /// State
        /// </summary>
        public int state { get; set; }
        /// <summary>
        /// Webstate
        /// </summary>
        public int webstate { get; set; }
        /// <summary>
        /// 未知
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// Scantime
        /// </summary>
        public string scantime { get; set; }
        /// <summary>
        /// Data
        /// </summary>
        public WebSiteSecurityTestData data { get; set; }
    }
}
