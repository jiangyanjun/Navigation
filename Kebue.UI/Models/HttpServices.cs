using CommonLayer;
using CommonLayer.Enum;
using Kebue.UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;

namespace Kebue.UI
{
    public class HttpServices
    {
        public static TR Post<TR, Parameter>(string url, Parameter RequestParameter)
        {
            return Post<Parameter>(url, RequestParameter).ToDeserialize<TR>();
        }
        public static string Post<Parameter>(string url, Parameter RequestParameter)
        {
            string reResponse = string.Empty;
            HttpClientHandler handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip };
            using (var http = new HttpClient(handler))
            {
                HttpRequestHeaders header = http.DefaultRequestHeaders;
                string user = "admin@kebue.com";
                string key = "aDm!〗@，。“；";
                Encoding encoding = Encoding.UTF8;
                http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Base64编码
                var data = Convert.ToBase64String(encoding.GetBytes(user + ":" + key));
                //设置AuthenticationHeaderValue
                header.Authorization = new AuthenticationHeaderValue("Basic", data);
                //通过HttpRequestHeaders.Add
                // header.Add("Authorization", "Basic " + data);

                var parameter = HttpServices.GetObjectPropertyValue<Parameter>(RequestParameter);
                var content = new FormUrlEncodedContent(parameter);
                var response = http.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    reResponse = response.Content.ReadAsStringAsync().Result;
                }
            }
            if (reResponse.IsNotNull())
            {
                reResponse = PostRequest(url, RequestParameter.ToSerialize());
            }
            return reResponse;
        }

        private static Dictionary<string, string> GetObjectPropertyValue<T>(T t)
        {
            Dictionary<string, string> x = new Dictionary<string, string>();
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property != null && t != null)
                {

                    if (t is String || t is Char)
                    {
                        t = (T)Convert.ChangeType(t, typeof(T)); 
                    }

                    object o = property.GetValue(t, null);
                    if (o != null)
                    {
                        x.Add(property.Name, o.ToString());
                    }
                }
            }
            return x;
        }
        public static string PostRequest(string url, string postParameter)
        {
            string responseStr = string.Empty;
            WebRequest request = WebRequest.Create(url);
            request.Method = "Post";
            request.ContentType = "application/json";
            byte[] requestData = System.Text.Encoding.UTF8.GetBytes(postParameter);
            request.ContentLength = requestData.Length;
            Stream newStream = request.GetRequestStream();
            newStream.Write(requestData, 0, requestData.Length);
            newStream.Close();
            var response = request.GetResponse();
            Stream ReceiveStream = response.GetResponseStream();
            using (StreamReader stream = new StreamReader(ReceiveStream, Encoding.UTF8))
            {
                responseStr = stream.ReadToEnd();
            }

            return responseStr;
        }
        public static string GetUrlPath(ActionEnum action)
        {
            string filter = "Controller";
            var desc = action.ToEnumDiscription();
            if (desc.Contains(filter))
            {
                desc = desc.Replace(filter, "");
            }
            return string.Format("{0}/{1}/{2}", AppSettingsConfig.WebApiUrl, desc, action);
        }
    }

   
}