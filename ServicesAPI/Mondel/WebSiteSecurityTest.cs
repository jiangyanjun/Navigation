using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAPI.Mondel
{
    /// <summary>
    /// 网站安全检测
    /// </summary>
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
