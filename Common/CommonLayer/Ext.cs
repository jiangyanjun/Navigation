using CommonLayer.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace CommonLayer
{
    public static class Ext
    {

        #region 验证URL是否可以访问
        /// <summary>
        /// 验证URL是否可以访问
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private static bool _urlverification(string url)
        {
            #region UrlVerification
            string result = string.Empty;
            bool IsReturn = false;
            if (url.IsNotNull())
            {
                try
                {
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                    myRequest.Method = "HEAD";
                    myRequest.Timeout = 10000;  //超时时间10秒
                    HttpWebResponse res = (HttpWebResponse)myRequest.GetResponse();
                    IsReturn = (res.StatusCode == HttpStatusCode.OK);
                }
                catch
                {
                    try
                    {
                        System.Net.HttpWebRequest.Create(url).GetResponse();
                        IsReturn = true;
                    }
                    catch
                    {
                        try
                        {
                            WebClient MyWebClient = new WebClient();
                            MyWebClient.Credentials = CredentialCache.DefaultCredentials;
                            result = Encoding.UTF8.GetString(MyWebClient.DownloadData(url));
                            IsReturn = true;
                        }
                        catch { }
                    }
                }
            }
            if (IsReturn == false)
            {
                for (int i = 0; i < 5; i++)
                {
                    UrlVerification(url);
                    if (IsReturn) return IsReturn;
                    else continue;
                }
            }
            #endregion
            return IsReturn;
        }
        public static bool UrlVerification(this string url)
        {
            Func<string, bool> func = _urlverification;
            return func(url);
        }
        #endregion

        #region 判断2个字符是否相等 忽略大小写
        /// <summary>
        /// 判断2个字符是否相等 忽略大小写
        /// </summary>
        /// <param name="value"></param>
        /// <param name="toBeCompared"></param>
        /// <returns></returns>
        public static bool IsEqual(this string value, string toBeCompared)
        {
            if (value.IsNull())
            {
                return toBeCompared.IsNull();
            }
            return value.Equals(toBeCompared, StringComparison.OrdinalIgnoreCase);
        }
        #endregion

        #region 是否空对象
        /// <summary>
        /// 是否空对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Pure, ContractVerification(false)]
        public static bool IsNull(this object value)
        {
            return value == null;
        }
        [Pure, ContractVerification(false)]
        public static bool IsNotNull(this object value)
        {
            return value != null;
        }
        /// <summary>
        /// 判断字符串不等于空
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool IsNotNull(this string s)
        {
            if (s != null && !string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                return true;
            else return false;
        }
        public static bool IsContains(this object s, string compStr)
        {
            if (!s.IsNull() && s.ToString().IsNotNull() && compStr.IsNotNull())
            {
                return s.ToString().Contains(compStr);
            }
            else return false;
        }
        #endregion

        #region DataTable生成实体
        /// <summary>
        /// DataTable生成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DataTable dataTable) where T : class, new()
        {
            if (dataTable == null || dataTable.Rows.Count <= 0) return null;
            Func<DataRow, T> func = dataTable.Rows[0].ToExpression<T>();
            List<T> collection = new List<T>(dataTable.Rows.Count);
            foreach (DataRow dr in dataTable.Rows)
            {
                collection.Add(func(dr));
            }
            return collection;
        }
        #region 生成表达式
        /// <summary>
        /// 生成表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataRow"></param>
        /// <returns></returns>
        public static Func<DataRow, T> ToExpression<T>(this DataRow dataRow) where T : class, new()
        {
            if (dataRow == null) throw new ArgumentNullException("dataRow", "当前对象为null 无法转换成实体");
            ParameterExpression paramter = Expression.Parameter(typeof(DataRow), "dr");
            List<MemberBinding> binds = new List<MemberBinding>();
            for (int i = 0; i < dataRow.ItemArray.Length; i++)
            {
                String colName = dataRow.Table.Columns[i].ColumnName;
                PropertyInfo pInfo = typeof(T).GetProperty(colName);
                if (pInfo == null) continue;
                MethodInfo mInfo = typeof(DataRowExtensions).GetMethod("Field", new Type[] { typeof(DataRow), typeof(String) }).MakeGenericMethod(pInfo.PropertyType);
                MethodCallExpression call = Expression.Call(mInfo, paramter, Expression.Constant(colName, typeof(String)));
                MemberAssignment bind = Expression.Bind(pInfo, call);
                binds.Add(bind);
            }
            MemberInitExpression init = Expression.MemberInit(Expression.New(typeof(T)), binds.ToArray());
            return Expression.Lambda<Func<DataRow, T>>(init, paramter).Compile();
        }
        #endregion
        #endregion

        #region 获取中英字符串长度
        /// <summary>
        /// 获取中英字符串长度
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetStrLength(this string str)
        {
            int len = 0;
            byte[] b;
            for (int i = 0; i < str.Length; i++)
            {
                b = Encoding.Default.GetBytes(str.Substring(i, 1));
                if (b.Length > 1)
                    len += 2;
                else
                    len++;
            }
            return len;
        }
        #endregion

        #region 过滤标记(包括HTML，脚本，数据库关键字，特殊字符的源码)
        /// <summary>
        /// 过滤标记
        /// </summary>
        /// <param name="NoHTML">包括HTML，脚本，数据库关键字，特殊字符的源码 </param>
        /// <returns>已经去除标记后的文字</returns>
        public static string FilterMark(this string Htmlstring)
        {
            if (Htmlstring == null)
            {
                return "";
            }
            else
            {
                //删除脚本
                Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                //删除HTML
                Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);

                //删除与数据库相关的词
                Htmlstring = Regex.Replace(Htmlstring, "select", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "insert", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete from", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "count''", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "drop table", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "truncate", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "asc", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "mid", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "char", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "exec master", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net localgroup administrators", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "and", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net user", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "or", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "net", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring,"*", "", RegexOptions.IgnoreCase);
                //Htmlstring = Regex.Replace(Htmlstring,"-", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "delete", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "drop", "", RegexOptions.IgnoreCase);
                Htmlstring = Regex.Replace(Htmlstring, "script", "", RegexOptions.IgnoreCase);

                //特殊的字符
                Htmlstring = Htmlstring.Replace("<", "");
                Htmlstring = Htmlstring.Replace(">", "");
                Htmlstring = Htmlstring.Replace("*", "");
                Htmlstring = Htmlstring.Replace("-", "");
                Htmlstring = Htmlstring.Replace("?", "");
                Htmlstring = Htmlstring.Replace(",", "");
                Htmlstring = Htmlstring.Replace("/", "");
                Htmlstring = Htmlstring.Replace(";", "");
                Htmlstring = Htmlstring.Replace("*/", "");
                Htmlstring = Htmlstring.Replace("\r\n", "");
                Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
                return Htmlstring;
            }

        }
        #endregion

        #region 是否Url格式
        /// <summary>
        /// 是否Url格式
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsUrlFormat(this string url)
        {
            string Pattern = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&$%\$#\=~])*$";
            Regex r = new Regex(Pattern);
            Match m = r.Match(url);
            return m.Success;
        }
        public static bool IsUrlFormat2(this string url)
        {
            string Pattern = @"^[a-zA-Z0-9]+\.[a-zA-Z]{2,3}";
            Regex r = new Regex(Pattern);
            Match m = r.Match(url);
            return m.Success;
        }
        #endregion

        #region  计算文本长度，区分中英文字符，中文算两个长度，英文算一个长度
        public static int GetLength(this string Text)
        {
            int len = 0;
            for (int i = 0; i < Text.Length; i++)
            {
                byte[] byte_len = Encoding.Default.GetBytes(Text.Substring(i, 1));
                if (byte_len.Length > 1)
                    len += 2;  //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1;  //如果长度等于1，是英文，占一个字节，+1
            }
            return len;
        }
        #endregion

        /// <summary>
        /// 截取指定长度字符串(按字节算)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string StrCut(this string str, int length)
        {
            int len = 0;
            byte[] b;
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < str.Length; i++)
            {
                b = Encoding.Default.GetBytes(str.Substring(i, 1));
                if (b.Length > 1)
                    len += 2;
                else
                    len++;

                if (len >= length)
                    break;

                sb.Append(str[i]);
            }

            return sb.ToString();
        }


        public static HttpResponseMessage ToHttpResponseMessage(this Object obj)
        {
            String str;
            if (obj is String || obj is Char)
            {
                str = obj.ToString();
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                str = serializer.Serialize(obj);
            }
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(str, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
        public static TR ToDeserialize<TR>(this string data)
        {
            try
            {
                JavaScriptSerializer Serializer = new JavaScriptSerializer();
                return Serializer.Deserialize<TR>(data);
            }
            catch (Exception)
            {
                return default(TR);
            }
        }
        public static string ToSerialize(this object data)
        {
            JavaScriptSerializer Serializer = new JavaScriptSerializer();
            return Serializer.Serialize(data);
        }
        public static Dictionary<string, string> GetObjectPropertyValue<T>(this T t)
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
        /// <summary>
        /// 获取请求API的URL路径
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string GetRequestApiUrlPath(this ActionEnum action)
        {
            string filter = "Controller";
            var desc = action.ToEnumDiscription();
            if (desc.Contains(filter))
            {
                desc = desc.Replace(filter, "");
            }
            filter = " ";
            if (desc.Contains(filter))
            {
                desc = desc.Replace(filter, "");
            }
            return string.Format("{0}/{1}/{2}", ConfigurationSettings.AppSettings["WebApiUrl"].ToString().Trim(), desc.Trim(), action.ToString().Trim());
        }

        public static EntityAttribute GetEntityAttribute<T>(this T t)
        {
            object[] objs = typeof(T).GetCustomAttributes(typeof(EntityAttribute), true);
            foreach (object obj in objs)
            {
                EntityAttribute attr = obj as EntityAttribute;
                if (attr != null)
                {
                    return attr;
                }
            }
            return null;
        }
        public static int ToInt(this string obj)
        {
            int r = 0;
            int.TryParse(obj, out r);
            return r;
        }
    }
}
