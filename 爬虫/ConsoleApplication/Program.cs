using Business;
using CommonLayer;
using PhysicalLayer;
using Reptitle.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleApplication
{

    public class Loudong
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

    public class Guama
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

    public class Xujia
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

    public class Cuangai
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

    public class Pangzhu
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

    public class Score
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

    public class Violation
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

    public class Google
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

    public class Data
    {
        /// <summary>
        /// Loudong
        /// </summary>
        public Loudong loudong { get; set; }
        /// <summary>
        /// Guama
        /// </summary>
        public Guama guama { get; set; }
        /// <summary>
        /// Xujia
        /// </summary>
        public Xujia xujia { get; set; }
        /// <summary>
        /// Cuangai
        /// </summary>
        public Cuangai cuangai { get; set; }
        /// <summary>
        /// Pangzhu
        /// </summary>
        public Pangzhu pangzhu { get; set; }
        /// <summary>
        /// Score
        /// </summary>
        public Score score { get; set; }
        /// <summary>
        /// Violation
        /// </summary>
        public Violation violation { get; set; }
        /// <summary>
        /// Google
        /// </summary>
        public Google google { get; set; }
    }

    public class Result
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
        public Data data { get; set; }
    }

    public class Root
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
        public Result result { get; set; }
        /// <summary>
        /// Error_code
        /// </summary>
        public int error_code { get; set; }
    }










    class Program
    {
        static void Main(string[] args)
        {
            string url = "https://www.niubide.com/q?s=&wd=.net";
            var contentStr = DownloadData.GetDownloadData(url);
            var s = contentStr;


        }

        /// <summary>
        /// Http (GET/POST)
        /// </summary>
        /// <param name="url">请求URL</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="method">请求方法</param>
        /// <returns>响应内容</returns>
        static string sendPost(string url, IDictionary<string, string> parameters, string method)
        {
            if (method.ToLower() == "post")
            {
                HttpWebRequest req = null;
                HttpWebResponse rsp = null;
                System.IO.Stream reqStream = null;
                try
                {
                    req = (HttpWebRequest)WebRequest.Create(url);
                    req.Method = method;
                    req.KeepAlive = false;
                    req.ProtocolVersion = HttpVersion.Version10;
                    req.Timeout = 5000;
                    req.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    byte[] postData = Encoding.UTF8.GetBytes(BuildQuery(parameters, "utf8"));
                    reqStream = req.GetRequestStream();
                    reqStream.Write(postData, 0, postData.Length);
                    rsp = (HttpWebResponse)req.GetResponse();
                    Encoding encoding = Encoding.GetEncoding(rsp.CharacterSet);
                    return GetResponseAsString(rsp, encoding);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                finally
                {
                    if (reqStream != null) reqStream.Close();
                    if (rsp != null) rsp.Close();
                }
            }
            else
            {
                //创建请求
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "?" + BuildQuery(parameters, "utf8"));

                //GET请求
                request.Method = "GET";
                request.ReadWriteTimeout = 5000;
                request.ContentType = "text/html;charset=UTF-8";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));

                //返回内容
                string retString = myStreamReader.ReadToEnd();
                return retString;
            }
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <returns>URL编码后的请求数据</returns>
        static string BuildQuery(IDictionary<string, string> parameters, string encode)
        {
            StringBuilder postData = new StringBuilder();
            bool hasParam = false;
            IEnumerator<KeyValuePair<string, string>> dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                string name = dem.Current.Key;
                string value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name))//&& !string.IsNullOrEmpty(value)
                {
                    if (hasParam)
                    {
                        postData.Append("&");
                    }
                    postData.Append(name);
                    postData.Append("=");
                    if (encode == "gb2312")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.GetEncoding("gb2312")));
                    }
                    else if (encode == "utf8")
                    {
                        postData.Append(HttpUtility.UrlEncode(value, Encoding.UTF8));
                    }
                    else
                    {
                        postData.Append(value);
                    }
                    hasParam = true;
                }
            }
            return postData.ToString();
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            System.IO.Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                return reader.ReadToEnd();
            }
            finally
            {
                // 释放资源
                if (reader != null) reader.Close();
                if (stream != null) stream.Close();
                if (rsp != null) rsp.Close();
            }
        }


























        //static void Main(string[] args)
        //{
        //    //ReptitleDownload.ReptitleChongbuluoUrl();
        //    //ReptitleDownload.ReptitleH_UIUrl();
        //    // List<U_Url_List> dt = bll.Find();
        //    //ReptitleDownload.ReptitleCSRS();
        //    //var i = ReptitleDownload.GetUrl();
        //    //var ii = ReptitleDownload.GetAcg();
        //    //var iii = ReptitleDownload.GetJSZ();
        //    //var bll = new U_Url_ListBll();
        //    //var list = bll.Find();
        //    //foreach (var item in list)
        //    //{
        //    //    bool isf = ReptitleDownload.VerifyURLIsValid(item.Url);
        //    //    if (!isf)
        //    //    {
        //    //        bll.UpdateStatus(item.Id,EnumUrlStatus.Unreasonable);
        //    //    }
        //    //    Console.WriteLine(string.Format("{0} {1}检测：{2}",item.Name,item.Url,isf));
        //    //}
        //    //  ReptitleDownload.GetYSK();
        //    string url = "http://guanjia.qq.com/online_server/result.html?url=http://www.uusdh.com/";
        //   var result= DownloadData.GetDownloadData(url);
        //    ReceivedResource(url);



        //    Console.WriteLine("已全部执行完成");
        //    Console.ReadKey();
        //}

        //private static void ReceivedResource(string strUri)
        //{
        //    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(strUri);
        //    myRequest.Method = "GET";               //设置提交方式可以为＂ｇｅｔ＂，＂ｈｅａｄ＂等
        //    //myRequest.Timeout = 100000;              //设置网页响应时间长度
        //    myRequest.AllowAutoRedirect = false;//是否允许自动重定向
        //    HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
        //    string text = string.Empty;
        //    using (Stream responseStm = response.GetResponseStream())
        //    {
        //        StreamReader redStm = new StreamReader(responseStm, Encoding.UTF8);
        //        text = redStm.ReadToEnd();
        //    }
        //}
































    }
}
