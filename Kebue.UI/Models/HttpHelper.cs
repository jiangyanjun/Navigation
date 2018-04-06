using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;

namespace Kebue.UI.Models
{
    public class HttpHelper
    {
        /// <summary>
        /// POST方法获取某URL的页面内容
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="parms">参数</param>
        /// <param name="encoding">编码</param>
        /// <returns>页面内容</returns>
        public static string PostResponse(string url, string parms, int timeOut = 20000, int slowResponseTime = 10000, bool keepAlive = true, string siteid = "", string apikey = "")
        {
            string errorMsg; long reqTime; int httpStatusCode;
            var result = string.Empty;
            errorMsg = string.Empty;
            reqTime = 0;
            httpStatusCode = 0;
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                parms = parms.Replace("utf-16", "utf-8");
                System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)768 | (SecurityProtocolType)3072;
                var mRequest = (HttpWebRequest)WebRequest.Create(url);
                //相应请求的参数
                var data = Encoding.UTF8.GetBytes(parms);
                mRequest.Method = "POST";
                mRequest.ContentType = "Text/XML";
                //mRequest.Accept = "application/xml";
                mRequest.ContentLength = data.Length;
                mRequest.Timeout = timeOut;
                //false会导致基础连接已关闭这个错误
                mRequest.KeepAlive = true;
                mRequest.ProtocolVersion = HttpVersion.Version10;
                //这个在Post的时候，一定要加上，如果服务器返回错误，他还会继续再去请求，不会使用之前的错误数据，做返回数据,取消100-continue
                mRequest.ServicePoint.Expect100Continue = false;


                //mRequest.Headers.Add("Accept-Encoding", "gzip");
                if (!string.IsNullOrEmpty(siteid) && !string.IsNullOrEmpty(apikey))
                {
                    mRequest.Headers.Add(string.Format("Authorization:{0}:{1}", siteid, apikey));
                }

                mRequest.Headers["Accept-Encoding"] = "gzip,deflate";
                mRequest.AutomaticDecompression = DecompressionMethods.GZip;

                //请求流
                var requestStream = mRequest.GetRequestStream();
                requestStream.Write(data, 0, data.Length);
                requestStream.Close();

                //响应流
                string response = string.Empty;
                using (HttpWebResponse res = (HttpWebResponse)mRequest.GetResponse())
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(
                                                           res.GetResponseStream()
                                                         , Encoding.UTF8))
                    {
                        response = reader.ReadToEnd();
                    }
                    result = response;
                    if (string.IsNullOrEmpty(result))
                    {
                        errorMsg = "请求成功但是返回为空！" + (res != null ? res.StatusCode.ToString() : "");
                    }
                }
            }
            catch (WebException ex)
            {
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                if (response != null)
                {
                    using (Stream data = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(data))
                        {
                            errorMsg = reader.ReadToEnd();
                            result = errorMsg;
                        }
                    }
                }
                else
                {
                    errorMsg = ex.Message;
                }
            }
            catch (TimeoutException ex)
            {
                errorMsg = ex.Message;

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }

            sw.Stop();
            reqTime = sw.ElapsedMilliseconds;

            //WarningSlowResponse(url, parms, reqTime, slowResponseTime);
            return result;
        }

        /// <summary>
        /// GET方法获取某URL的页面内容
        /// </summary>
        /// <param name="url">网址</param>
        /// <param name="encoding">编码</param>
        /// <returns>页面内容</returns>
        public static string GetResponse(string url, out string errorMsg, out long reqTime, int timeOut = 20000)
        {
            var result = string.Empty;
            errorMsg = string.Empty;
            reqTime = 0;
            Stopwatch sw = Stopwatch.StartNew();
            try
            {
                System.Net.HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                req.Method = "GET";
                req.ContentType = "application/xml";
                req.Accept = "application/xml";
                req.ServicePoint.Expect100Continue = false;
                req.ServicePoint.UseNagleAlgorithm = false;
                req.Timeout = timeOut;
                //加入GZip 黄纪涛 2016-02-26
                //req.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                //req.Headers.Add("Accept-Encoding", "gzip, deflate");
                req.Headers["Accept-Encoding"] = "gzip,deflate";
                req.AutomaticDecompression = DecompressionMethods.GZip;
                string response = string.Empty;
                using (HttpWebResponse res = (HttpWebResponse)req.GetResponse())
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(
                                                           res.GetResponseStream()
                                                         , Encoding.UTF8))
                    {
                        response = reader.ReadToEnd();
                    }
                    result = response;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
            }
            sw.Stop();
            reqTime = sw.ElapsedMilliseconds;
            return result;
        }
    }
}