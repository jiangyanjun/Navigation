using CommonLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Text;

namespace Kebue.UI.Areas.Chat.Models
{
    public class ChatHelper
    {
        /// <summary>
        ///  POST请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="session">参数</param>
        /// <returns></returns>
        public string HttpPost(string url, string param, UserInfoUnDeteil userinfo)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "text/json; charset=UTF-8";
            var model = new Parameter(param, userinfo);
            string json = JsonConvert.SerializeObject(model);
            using (StreamWriter dataStream = new StreamWriter(request.GetRequestStream()))
            {
                dataStream.Write(json);
                dataStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string encoding = response.ContentEncoding;
            if (encoding == null || encoding.Length < 1)
            {
                encoding = "UTF-8"; //默认编码  
            }
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(encoding));
            return reader.ReadToEnd();
        }

    }

    public class UserInfoUnDeteil
    {
        /// <summary>
        /// True为合法用法，false说明进入了黑名单
        /// </summary>
        public bool Flage { get; set; }
        public string Key { get; set; }
        public UserInfo UserInfo { get; set; }
        public UserInfoDetail UserInfoDetail { get; set; }
        /// <summary>
        /// 前台发送的问题
        /// </summary>
        public string Message { get; set; }
    }
    public class UserInfoDetail
    {
        [Key]
        [DisplayName("主键")]
        public string Id { get; set; }

        [DisplayName("访问IP")]
        public string Ip { get; set; }

        [DisplayName("访问游览器")]
        public string Browser { get; set; }

        [DisplayName("访问操作系统类型")]
        public string OS { get; set; }

        [DisplayName("访问设备类型")]
        public string ModelType { get; set; }

        [DisplayName("备注说明")]
        public string Remarks { get; set; }

        [DisplayName("操作时间")]
        public string OperationTime { get; set; }
    }
    public class UserInfo
    {
        [Key]
        [DisplayName("主键ID")]
        public string Id { get; set; }

        [DisplayName("用户唯一IP地址")]
        [Key]
        public string Ip { get; set; }

        [DisplayName("用户详细地址")]
        public string Isp { get; set; }

        [DisplayName("国家")]
        public string Country { get; set; }

        [DisplayName("省份")]
        public string Province { get; set; }

        [DisplayName("城市")]
        public string City { get; set; }

        [DisplayName("区县")]
        public string District { get; set; }

        [DisplayName("街道")]
        public string Street { get; set; }

        [DisplayName("门牌号")]
        public string Street_Number { get; set; }

        [DisplayName("行政区划代码（身份证前6位）")]
        public string Admin_Area_Code { get; set; }

        [DisplayName("经度")]
        public string Lat { get; set; }

        [DisplayName("纬度")]
        public string Lng { get; set; }

        [DisplayName("百度IP定位结果唯一ID")]
        public string Locid { get; set; }

        [DisplayName("百度IP定位定位结果半径")]
        public string radius { get; set; }

        [DisplayName("百度IP定位定位结果可信度")]
        public string Confidence { get; set; }

        [DisplayName("用户状态1无效 0有效")]
        public int Status { get; set; }

        [DisplayName("最近操作时间")]
        public string OperationTime { get; set; }

        [DisplayName("图像索引")]
        public int Img { get; set; }

        [DisplayName("访问次数")]
        public int InCount { get; set; }

        [DisplayName("备注")]
        public string Remarks { get; set; }
    }
    #region 参数组装
    public class Parameter
    {
        public Parameter(string info, UserInfoUnDeteil userinfo)
        {
            this.info = info;
            if (userinfo != null && userinfo.UserInfo.Lng == null && userinfo.UserInfo.Lat == null)
            {
                var ClientIpAddress = JsonConvert.DeserializeObject<Root>(BaidOrdinaryRequest.BaidApiGetRequestIpAddress(userinfo.UserInfo.Ip));
                if (ClientIpAddress.error == 0 && ClientIpAddress.data != null)
                {
                    userinfo.UserInfo.Lat = ClientIpAddress.data.Lat;
                    userinfo.UserInfo.Lng = ClientIpAddress.data.Lng;
                    if (!userinfo.UserInfo.Isp.Contains(ClientIpAddress.data.Province) || !userinfo.UserInfo.Isp.Contains(ClientIpAddress.data.City))
                    {
                        userinfo.UserInfo.Isp = userinfo.UserInfo.Isp + "[" + ClientIpAddress.data.Province + ClientIpAddress.data.City + ClientIpAddress.data.Country + "]";
                    }
                    //UserInfoBLL userinfobll = new UserInfoBLL();
                    //userinfobll.Update(userinfo.UserInfo);
                    CacheHelper.RemoveAllCache(userinfo.Key);
                    CacheHelper.SetCache(userinfo.Key, userinfo);
                }
                else
                    return;
            }
            int result;
            int.TryParse(userinfo.UserInfo.Ip.Replace(".", ""), out result);
            if (result != 0)
                userid = result;
            else
                userid = 123;
            double reslut_lng;
            Double.TryParse(userinfo.UserInfo.Lng, out reslut_lng);
            if (reslut_lng > 1)
            {
                this.lon = Convert.ToDouble(reslut_lng).ToString("0.000000");
            }
            double reslut_lat;
            Double.TryParse(userinfo.UserInfo.Lat, out reslut_lat);
            if (reslut_lat > 1)
            {
                this.lat = Convert.ToDouble(reslut_lat).ToString("0.000000");
            }
            if (userinfo.UserInfo.Isp.Contains(" "))
            {
                this.loc = userinfo.UserInfo.Isp.Split(' ')[0];
            }
            else
                this.loc = userinfo.UserInfo.Isp;
        }
        public string key { get { return ConstParameter.Turing_key; } }
        public string info { get; set; }
        public string loc { get; set; }
        public string lon { get; set; }
        public string lat { get; set; }
        /// <summary>
        /// 上写文ID
        /// </summary>
        public int userid { get; set; }
    }
    #endregion
    /// <summary>
    /// 地区级联
    /// </summary>
    public class RegionalCascadeAddres
    {
        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 区县
        /// </summary>
        public string District { get; set; }
        /// <summary>
        /// 街道
        /// </summary>
        public string Street { get; set; }
        /// <summary>
        /// 门牌号
        /// </summary>
        public string Street_Number { get; set; }
    }
    public class Root
    {
        /// <summary>
        /// Data
        /// </summary>
        public IpLocation data { get; set; }
        /// <summary>
        /// Error
        /// </summary>
        public int error { get; set; }
        /// <summary>
        /// succeed
        /// </summary>
        public string msg { get; set; }
    }
    /// <summary>
    /// 经纬度
    /// </summary>
    public class IpLocation : RegionalCascadeAddres
    {
        /// <summary>
        /// Ip
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public string Lat { get; set; }
        /// <summary>
        /// 维度
        /// </summary>
        public string Lng { get; set; }
    }
    public class BaidOrdinaryRequest
    {
        /// <summary>
        /// 根据IP 用百度API请求地址
        /// </summary>
        /// <param name="param">ip</param>
        /// <returns></returns>
        public static string BaidApiGetRequestIpAddress(string param)
        {
            string url = "http://apis.baidu.com/chazhao/ipsearch/ipsearch";
            string strURL = url + "?ip=" + param;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
            request.Method = "GET";
            // 添加header
            request.Headers.Add("apikey", "ddc3e074ebbe2a7a01538056daa1c8ee");
            System.Net.HttpWebResponse response;
            response = (System.Net.HttpWebResponse)request.GetResponse();
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue;
        }
    }
    public class ConstParameter
    {
        public const string cacheKey = "CustomizeTuringLoginc";
        #region Turing
        public const string DateTimeformar = "ddHHmm";
        public const string Info = "info";
        public const string Code = "code";
        public const string userid = "userid";
        public const string DateTime = "DateTime";
        public const string address = "address";
        /// <summary>
        /// apikey
        /// </summary>
        public const string Turing_Apikey = "apikey";
        /// <summary>
        /// 求其API地址
        /// </summary>
        public const string Turing_url = "http://www.tuling123.com/openapi/api";
        /// <summary>
        /// 必须参数
        /// </summary>
        public const string Turing_key = "251cfdca8ce34d9c8f19ef1b437691cb";//'c4f3c14c5de44175be9b1b581cc7673c';//c75ba576f50ddaa5fd2a87615d144ecf

        #endregion
    }
    #region 返回实体
    #region 文字类  和其他类别基础类

    public class TuringCustom : turingText
    {
        public List<turingText> CShare { get; set; }
    }

    /// <summary>
    /// 文字类
    /// </summary>
    public class turingText
    {
        /// <summary>
        /// 标示 识别码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 提示语
        /// </summary>
        public string text { get; set; }
    }
    #endregion
    #region  链接类  和  列车类 ，航班类
    /// <summary>
    /// 链接类  和  列车类 ，航班类
    /// </summary>
    public class Links : turingText
    {
        public string url { get; set; }
    }
    #endregion
    #region 新闻类
    /// <summary>
    /// 新闻类
    /// </summary>
    public class News : turingText
    {
        /// <summary>
        /// 新闻列表
        /// </summary>
        public List<articleList> list { get; set; }

    }
    /// <summary>
    /// 文章列表
    /// </summary>
    public class articleList
    {
        /// <summary>
        /// 新闻标题
        /// </summary>
        public string article { get; set; }
        /// <summary>
        /// 新闻来源
        /// </summary>
        public string source { get; set; }
        /// <summary>
        /// 新闻图片
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 新闻链接地址
        /// </summary>
        public string detailurl { get; set; }
    }
    #endregion
    #region 菜谱类
    /// <summary>
    /// 菜谱类
    /// </summary>
    public class menuClass : turingText
    {
        /// <summary>
        /// 菜单列表
        /// </summary>
        public List<menuList> list { get; set; }
    }
    public class menuList
    {
        /// <summary>
        /// 菜名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 菜谱信息
        /// </summary>
        public string info { get; set; }
        /// <summary>
        /// 详情链接
        /// </summary>
        public string detailurl { get; set; }
        /// <summary>
        /// 信息图标
        /// </summary>
        public string icon { get; set; }
    }
    #endregion
    #region 儿歌列表
    /// <summary>
    /// 儿歌类
    /// </summary>
    public class ChildrenIsSongs : turingText
    {
        /// <summary>
        /// 儿歌列表
        /// </summary>
        public List<ChildrenIsSongsList> function { get; set; }
    }
    public class ChildrenIsSongsList
    {
        /// <summary>
        /// 歌曲名
        /// </summary>
        public string song { get; set; }
        /// <summary>
        /// 歌手
        /// </summary>
        public string singer { get; set; }
    }
    #endregion
    #region 诗词类
    /// <summary>
    /// 诗词类
    /// </summary>
    public class Poetry : turingText
    {
        /// <summary>
        /// 诗词列表
        /// </summary>
        public List<PoetryList> function { get; set; }
    }
    public class PoetryList
    {
        /// <summary>
        /// 作者
        /// </summary>
        public string author { get; set; }
        /// <summary>
        /// 诗词名
        /// </summary>
        public string name { get; set; }
        #endregion
        #endregion



    }
}