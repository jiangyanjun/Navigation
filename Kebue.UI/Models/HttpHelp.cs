using CommonLayer;
using CommonLayer.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Kebue.UI.Models
{
    public class HttpHelp
    {
        public static int staffId { get { return int.Parse(AppSettingsConfig.StaffId); } }
        const string ContentType = "application/json";

        /// <summary>
        /// API请求
        /// </summary>
        /// <typeparam name="TR">返回类型</typeparam>
        /// <typeparam name="Parameter">参数类型</typeparam>
        /// <param name="RequestParameter">请求参数</param>
        /// <param name="RequesrAction">请求Action</param>
        /// <param name=""></param>
        /// <returns></returns>
        public static TR Post<TR, Parameter>(ActionEnum RequesrAction, Parameter RequestParameter)
        {
            var reult = Post<Parameter>(RequesrAction, RequestParameter);
            if (!reult.IsNotNull())
            {
                return default(TR);
            }
            else
            {
                return JsonConvert.DeserializeObject<TR>(reult);
            }
        }
        public static string Post<Parameter>(ActionEnum RequesrAction, Parameter RequestParameter)
        {
            string para = RequestParameter.ToSerialize();
            string strResult = string.Empty;
            #region HttpWebRequest
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(para);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(RequesrAction.GetRequestApiUrlPath());
                string timeStamp = GetTimeStamp();
                string nonce = GetRandom();
                //加入头信息
                request.Headers.Add("staffid", staffId.ToString()); //当前请求用户StaffId
                request.Headers.Add("timestamp", timeStamp); //发起请求时的时间戳（单位：毫秒）
                request.Headers.Add("nonce", nonce); //发起请求时的时间戳（单位：毫秒）
                request.Headers.Add("signature", GetSignature(timeStamp, nonce, staffId, para)); //当前请求内容的数字签名

                //写数据
                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.ContentType = ContentType;
                Stream reqstream = request.GetRequestStream();
                reqstream.Write(bytes, 0, bytes.Length);

                //读数据
                request.Timeout = 300000;
                request.Headers.Set("Pragma", "no-cache");
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                strResult = streamReader.ReadToEnd();

                //关闭流
                reqstream.Close();
                streamReader.Close();
                streamReceive.Close();
                request.Abort();
                response.Close();
            }
            catch (Exception ex)
            {
                strResult = Post_HttpClientHandler(RequesrAction, RequestParameter);
            }
            #endregion
            return strResult;
        }
        public static string Post_HttpClientHandler<Parameter>(ActionEnum RequesrAction, Parameter RequestParameter)
        {
            string reResponse = string.Empty;
            string para = RequestParameter.ToSerialize();
            HttpClientHandler handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            using (var http = new HttpClient(handler))
            {
                HttpRequestHeaders header = http.DefaultRequestHeaders;
                string timeStamp = GetTimeStamp();
                string nonce = GetRandom();
                //加入头信息
                header.Add("staffid", staffId.ToString()); //当前请求用户StaffId
                header.Add("timestamp", timeStamp); //发起请求时的时间戳（单位：毫秒）
                header.Add("nonce", nonce); //发起请求时的时间戳（单位：毫秒）
                header.Add("signature", GetSignature(timeStamp, nonce, staffId, null)); //当前请求内容的数字签名

                var parameter = RequestParameter.GetObjectPropertyValue();
                var content = new FormUrlEncodedContent(parameter);
                var response = http.PostAsync(RequesrAction.GetRequestApiUrlPath(), content).Result;
                if (response.IsSuccessStatusCode)
                {
                    reResponse = response.Content.ReadAsStringAsync().Result;
                }
            }
            return reResponse;
        }
        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="webApi"></param>
        /// <param name="queryStr"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        public static T Get<T>(string webApi, string query, string queryStr, int staffId, bool sign = true)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(webApi + "?" + queryStr);
            string timeStamp = GetTimeStamp();
            string nonce = GetRandom();
            //加入头信息
            request.Headers.Add("staffid", staffId.ToString()); //当前请求用户StaffId
            request.Headers.Add("timestamp", timeStamp); //发起请求时的时间戳（单位：毫秒）
            request.Headers.Add("nonce", nonce); //发起请求时的时间戳（单位：毫秒）

            if (sign)
                request.Headers.Add("signature", GetSignature(timeStamp, nonce, staffId, query)); //当前请求内容的数字签名

            request.Method = "GET";
            request.ContentType = "application/json";
            request.Timeout = 90000;
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
            string strResult = streamReader.ReadToEnd();

            streamReader.Close();
            streamReceive.Close();
            request.Abort();
            response.Close();

            return JsonConvert.DeserializeObject<T>(strResult);
        }



        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="userinfo"></param>
        /// <returns></returns>
        public static TokenResultMsg GetSignToken(int staffId)
        {
            string tokenApi = AppSettingsConfig.GetTokenApi;
            Dictionary<string, string> parames = new Dictionary<string, string>();
            parames.Add("staffId", staffId.ToString());
            Tuple<string, string> parameters = GetQueryString(parames);
            TokenResultMsg token = Get<TokenResultMsg>(tokenApi, parameters.Item1, parameters.Item2, staffId, false);
            return token;
        }

        /// <summary>
        /// 计算签名
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="nonce"></param>
        /// <param name="staffId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string GetSignature(string timeStamp, string nonce, int staffId, string data)
        {
            Token token = null;
            var resultMsg = GetSignToken(staffId);
            if (resultMsg != null)
            {
                if (resultMsg.StatusCode == (int)StatusCodeEnum.Success)
                {
                    token = resultMsg.Result;
                }
                else
                {
                    throw new Exception(resultMsg.Data.ToString());
                }
            }
            else
            {
                throw new Exception("token为null，员工编号为：" + staffId);
            }

            var hash = System.Security.Cryptography.MD5.Create();
            //拼接签名数据
            var signStr = timeStamp + nonce + staffId + token.SignToken.ToString() + data;
            //将字符串中字符按升序排序
            var sortStr = string.Concat(signStr.OrderBy(c => c));
            var bytes = Encoding.UTF8.GetBytes(sortStr);
            //使用MD5加密
            var md5Val = hash.ComputeHash(bytes);
            //把二进制转化为大写的十六进制
            StringBuilder result = new StringBuilder();
            foreach (var c in md5Val)
            {
                result.Append(c.ToString("X2"));
            }
            return result.ToString().ToUpper();
        }

        /// <summary>  
        /// 获取时间戳  
        /// </summary>  
        /// <returns></returns>  
        private static string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }


        /// <summary>  
        /// 获取随机数
        /// </summary>  
        /// <returns></returns>  
        private static string GetRandom()
        {
            Random rd = new Random(DateTime.Now.Millisecond);
            int i = rd.Next(0, int.MaxValue);
            return i.ToString();
        }


        /// <summary>
        /// 拼接get参数
        /// </summary>
        /// <param name="parames"></param>
        /// <returns></returns>
        public static Tuple<string, string> GetQueryString(Dictionary<string, string> parames)
        {
            // 第一步：把字典按Key的字母顺序排序
            IDictionary<string, string> sortedParams = new SortedDictionary<string, string>(parames);
            IEnumerator<KeyValuePair<string, string>> dem = sortedParams.GetEnumerator();

            // 第二步：把所有参数名和参数值串在一起
            StringBuilder query = new StringBuilder("");  //签名字符串
            StringBuilder queryStr = new StringBuilder(""); //url参数
            if (parames == null || parames.Count == 0)
                return new Tuple<string, string>("", "");

            while (dem.MoveNext())
            {
                string key = dem.Current.Key;
                string value = dem.Current.Value;
                if (!string.IsNullOrEmpty(key))
                {
                    query.Append(key).Append(value);
                    queryStr.Append("&").Append(key).Append("=").Append(value);
                }
            }

            return new Tuple<string, string>(query.ToString(), queryStr.ToString().Substring(1, queryStr.Length - 1));
        }
    }
    public class TokenResultMsg : HttpResponseMsg
    {
        public Token Result
        {
            get
            {
                if (StatusCode == (int)StatusCodeEnum.Success)
                {
                    return JsonConvert.DeserializeObject<Token>(Data.ToString());
                }

                return null;
            }
        }
    }
}