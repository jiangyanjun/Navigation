using Business;
using PhysicalLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Reptitle.BusinessLogic
{
    public class ReptitleDownload
    {
        #region 数据搜索
        public static void ReptitleChongbuluoUrl()
        {
            #region 数据搜索
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("学术搜索", "http://scholar.chongbuluo.com/");
            dict.Add("数据搜索", "http://data.chongbuluo.com/");
            dict.Add("图片搜索", "http://image.chongbuluo.com/");
            dict.Add("快搜索", "http://search.chongbuluo.com/");
            var bll = new U_Url_ListBLL();
            U_Url_List entity;
            List<U_Url_List> listEntity = new List<U_Url_List>();
            foreach (var d in dict)
            {
                var result = DownloadData.GetDownloadData(d.Value);
                var filter = " <ul id=\"foo\" class=\"chongbuluo\">";
                result = result.Substring(result.IndexOf(filter) + filter.Length);
                filter = "</ul>";
                result = result.Substring(0, result.IndexOf(filter));
                var list = Regex.Split(result, "</li>");
                foreach (var item in list)
                {
                    var a = item.Trim();
                    if (!a.StartsWith("<li ")) continue;
                    entity = new U_Url_List();
                    var x = Regex.Split(item, ">");
                    if (x.Length < 4) continue;
                    entity.Id = System.Guid.NewGuid().ToString("N");
                    entity.IconImg = Regex.Split(x[1], "\"")[1];
                    if (a.Contains("<ul class=\"more\">"))
                    {
                        entity.Url = Regex.Split(x[7], "\"")[1];
                        entity.Name = Regex.Split(x[8], "<")[0];
                    }
                    else
                    {
                        entity.Url = Regex.Split(x[2], "\"")[1];
                        entity.Name = Regex.Split(x[3], "<")[0];
                    }
                    entity.Source = d.Value + " 爬取";
                    entity.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    entity.Create_Id = "pc";
                    entity.Status = 1;
                    entity.Types = d.Key;
                    listEntity.Add(entity);
                    Console.WriteLine(string.Format("{0} {1} {2}写入成功", d.Key, entity.Name, entity.Url));
                }
                bll.Add(listEntity);
                Console.WriteLine("虫虫部落抓取写入完成……");
            }
            #endregion
        } 
        #endregion
        #region ReptitleH_UIUrl
        public static void ReptitleH_UIUrl()
        {
            string url = "http://www.h-ui.net/site.shtml";
            var result = DownloadData.GetDownloadData(url);
            string filter = "<div class=\"bk_gray mt-10\">";
            if (result.Contains(filter))
                result = result.Substring(result.IndexOf(filter));
            filter = "</article>";
            if (result.Contains(filter))
                result = result.Substring(0, result.IndexOf(filter));
            filter = "<dl class=\"sitelist_1 cl\">";
            foreach (var item in Regex.Split(result, filter).Where(n => n.Trim().StartsWith("<dt class")))
            {
                result = item;
                filter = ">";
                if (result.Contains(filter))
                    result = result.Substring(result.IndexOf(filter) + filter.Length);
                filter = "<";
                if (result.Contains(filter))
                    result = result.Substring(0, result.IndexOf(filter));
                var titie = result;

                filter = "<ul class=\"cl\">";
                if (item.Contains(filter))
                    result = item.Substring(item.IndexOf(filter) + filter.Length);
                filter = "</ul>";
                if (result.Contains(filter))
                    result = result.Substring(0, result.IndexOf(filter)).Trim();
                filter = "<li>";
                var list = Regex.Split(result, filter);
                var bll = new U_Url_ListBLL();
                U_Url_List entity;
                List<U_Url_List> listEntity = new List<U_Url_List>();
                foreach (var key in list)
                {
                    if (string.IsNullOrEmpty(key)) continue;
                    filter = "\"";
                    var k = Regex.Split(key, filter);
                    if (k.Length > 6)
                    {
                        entity = new U_Url_List();
                        if (ReptitleDownload.VerifyURLIsValid(k[5]))
                        {
                            result = k[6];
                            filter = "</a>";
                            if (result.Contains(filter))
                                result = result.Substring(0, result.IndexOf(filter)).Substring(1);
                            else
                                result = k[7];
                            entity.Url = k[5];
                        }
                        else if (ReptitleDownload.VerifyURLIsValid(k[7]))
                        {
                            result = k[8];
                            filter = "</a>";
                            if (result.Contains(filter))
                                result = result.Substring(0, result.IndexOf(filter)).Substring(1);
                            entity.Url = k[7];
                        }
                        entity.Id = System.Guid.NewGuid().ToString("N");
                        entity.Name = result;
                        entity.Source = url + " 爬取";
                        entity.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        entity.Create_Id = "pc";
                        entity.Status = 1;
                        entity.Types = titie;
                        listEntity.Add(entity);
                        Console.WriteLine(string.Format("{0} {1} {2}", titie, k[5], result));
                    }
                    else
                        Console.WriteLine("异常数据：" + key);
                }
                bll.Add(listEntity);
                Console.WriteLine("" + url + "落抓取写入完成……");
            }

        }
        #endregion
        /// <summary>
        /// 正则表达式验证字符串是否Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool RegexUrl(string url)
        {
            string Pattern = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&$%\$#\=~])*$";
            Regex r = new Regex(Pattern);
            Match m = r.Match(url);
            return m.Success;
        }
        /// <summary>
        /// 验证是否合法url
        /// </summary>
        /// <param name="strUri"></param>
        /// <returns></returns>
        //public static bool VerifyURLIsValid(string strUri)
        //{
        //    try
        //    {
        //        if (!RegexUrl(strUri))
        //            System.Net.HttpWebRequest.Create(strUri).GetResponse();
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        public static bool VerifyURLIsValid(string strUri)
        {
            bool status = false;
            try
            {
                if (RegexUrl(strUri))
                {
                    //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(strUri);
                    //myRequest.Method = "GET";               //设置提交方式可以为＂ｇｅｔ＂，＂ｈｅａｄ＂等
                    ////myRequest.Timeout = 10000;              //设置网页响应时间长度
                    //myRequest.AllowAutoRedirect = false;//是否允许自动重定向
                    //HttpWebResponse myResponse = (HttpWebResponse)myRequest.GetResponse();
                    //status = (myResponse.StatusCode == HttpStatusCode.OK);//返回响应的状态
                    //myResponse.Close();
                    //if (!status)
                    //{
                    //    System.Net.HttpWebRequest.Create(strUri).GetResponse();
                        status = true;
                    //}
                }
            }
            catch (Exception ex)
            {
                status = false;
            }
            return status;
        }
    }
}



































































#region MyRegion
//#region 程序人生抓取
///// <summary>
///// 程序人生抓取
///// </summary>
//public static void ReptitleCSRS()
//{
//    var bll = new U_Url_ListBll();
//    U_Url_List entity;
//    List<U_Url_List> listEntity = new List<U_Url_List>();
//    string str = File.ReadAllText(@"E:\工作室\sh\Mmmxa.So\navigation\Mmmxa.So\HtmlPage1.html", Encoding.ASCII);
//    var list = Regex.Split(str, "<div class=\"section mtop\">");
//    foreach (var item in list)
//    {
//        if (string.IsNullOrEmpty(item)) continue;
//        var result = item;
//        string filter = "<span class=\"sub-category\">";
//        if (result.Contains(filter))
//            result = result.Substring(0, result.IndexOf(filter));
//        filter = " <h2 class=\"title\"> <i class=\"icon-\"></i>";
//        if (result.Contains(filter))
//            result = result.Replace(filter, "");
//        string title = result;
//        result = item;
//        var listResult = Regex.Split(result, "<li> <a");
//        foreach (var res in listResult)
//        {
//            entity = new U_Url_List();
//            entity.Id = System.Guid.NewGuid().ToString("N");
//            entity.Types = title;
//            filter = "href=";
//            result = res.Trim();
//            if (!result.StartsWith(filter)) continue;
//            var listResult2 = Regex.Split(result, "\"");
//            entity.Url = listResult2[1];
//            result = listResult2[6];
//            entity.Name = result.Substring(1, result.IndexOf("<") - 1);
//            result = listResult2[listResult2.Length - 1];
//            result = result.Replace(" /></a> ", "<li>");
//            entity.Description = HtmlConvert.RemoveHtml(result);
//            if (ReptitleDownload.VerifyURLIsValid(entity.Url) && !string.IsNullOrEmpty(entity.Name))
//            {
//                entity.Source = "程序人生";
//                entity.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//                entity.Create_Id = "pc";
//                entity.Status = 1;
//                listEntity.Add(entity);
//                Console.WriteLine(string.Format("{0} {1} {2}写入成功", title, entity.Name, entity.Url));
//            }
//            else
//                Console.WriteLine(string.Format("{0} {1} {2}写入数据异常", title, entity.Name, entity.Url));
//        }
//    }
//    Console.WriteLine(string.Format("成功写入{0}条数据", listEntity.Count));
//    bll.Add(listEntity);
//}
//#endregion
//#region HOT福利站
///// <summary>
///// HOT福利站
///// </summary>
//public static int GetUrl()
//{
//    var bll = new U_Url_ListBll();
//    U_Url_List entity;
//    List<U_Url_List> listEntity = new List<U_Url_List>();
//    string result = File.ReadAllText(@"E:\工作室\sh\Mmmxa.So\navigation\Mmmxa.So\HtmlPage1.html");
//    string filter = "<div id=\"tieba\" class=\"wrap mt1 nav\">";
//    if (result.Contains(filter))
//    {
//        var list = Regex.Split(result, filter);
//        foreach (var item in list)
//        {
//            if (string.IsNullOrEmpty(item)) continue;
//            var list2 = Regex.Split(item, "<li><a");
//            string types = item.Substring(item.IndexOf(">") + 1, item.IndexOf("<") - 3);
//            foreach (var l2 in list2)
//            {
//                var list3 = Regex.Split(l2, "\"");
//                entity = new U_Url_List();
//                entity.Id = System.Guid.NewGuid().ToString("N");
//                entity.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//                entity.Create_Id = "pc";
//                entity.Status = 1;
//                entity.Types = types;
//                if (!l2.Trim().StartsWith("href=")) continue;
//                if (ReptitleDownload.VerifyURLIsValid(list3[1]))
//                    entity.Url = list3[1];
//                else if (ReptitleDownload.VerifyURLIsValid(list3[2]))
//                    entity.Url = list3[2];
//                entity.Name = list3[3];
//                entity.Description = list3[list3.Length - 1];
//                filter = "<";
//                if (entity.Description.Contains(filter))
//                    entity.Description = entity.Description.Substring(1, entity.Description.IndexOf(filter) - 1);
//                if (ReptitleDownload.VerifyURLIsValid(entity.Url) && !string.IsNullOrEmpty(entity.Name))
//                {
//                    entity.Source = "HOT福利站";
//                    listEntity.Add(entity);
//                    Console.WriteLine(string.Format("{0} {1} {2}写入成功", entity.Types, entity.Name, entity.Url));
//                }
//                else
//                    Console.WriteLine(string.Format("{0} {1} {2}写入数据异常", entity.Types, entity.Name, entity.Url));
//            }
//        }
//    }
//    Console.WriteLine(string.Format("成功写入{0}条数据", listEntity.Count));
//    bll.Add(listEntity);
//    return listEntity.Count;
//}
//#endregion
//#region GetAcg
//public static int GetAcg()
//{
//    var bll = new U_Url_ListBll();
//    U_Url_List entity;
//    List<U_Url_List> listEntity = new List<U_Url_List>();
//    var result = File.ReadAllText(@"E:\工作室\sh\Mmmxa.So\navigation\Mmmxa.So\Views\Home\Acg.cshtml");
//    string filter = "<div id=\"danmu\" class=\"wrap mt1 nav\">";
//    var list = Regex.Split(result, filter);
//    foreach (var item in list)
//    {
//        if (string.IsNullOrEmpty(item)) continue;
//        string types = item.Substring(item.IndexOf(">") + 1, item.IndexOf("<") - 3);
//        filter = "<li><a";
//        var list2 = Regex.Split(item, filter);
//        foreach (var x in list2)
//        {
//            var liat3 = Regex.Split(x, "\"");
//            entity = new U_Url_List();
//            entity.Id = System.Guid.NewGuid().ToString("N");
//            entity.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//            entity.Create_Id = "pc";
//            entity.Status = 1;
//            entity.Types = types;
//            if (!x.Trim().StartsWith("href=")) continue;
//            if (ReptitleDownload.VerifyURLIsValid(liat3[1]))
//                entity.Url = liat3[1];
//            else if (ReptitleDownload.VerifyURLIsValid(liat3[2]))
//                entity.Url = liat3[2];
//            entity.Name = liat3[3];
//            entity.Description = liat3[liat3.Length - 1];
//            filter = "<";
//            if (entity.Description.Contains(filter))
//                entity.Description = entity.Description.Substring(1, entity.Description.IndexOf(filter) - 1);
//            if (ReptitleDownload.VerifyURLIsValid(entity.Url) && !string.IsNullOrEmpty(entity.Name))
//            {
//                entity.Source = "ACG";
//                listEntity.Add(entity);
//                Console.WriteLine(string.Format("{0} {1} {2}写入成功", entity.Types, entity.Name, entity.Url));
//            }
//            else
//                Console.WriteLine(string.Format("{0} {1} {2}写入数据异常", entity.Types, entity.Name, entity.Url));

//        }
//    }
//    Console.WriteLine(string.Format("成功写入{0}条数据", listEntity.Count));
//    bll.Add(listEntity);
//    return listEntity.Count;
//}
//#endregion
//#region GetJSZ
//public static int GetJSZ()
//{
//    var bll = new U_Url_ListBll();
//    U_Url_List entity;
//    List<U_Url_List> listEntity = new List<U_Url_List>();
//    var result = File.ReadAllText(@"E:\工作室\sh\Mmmxa.So\navigation\Mmmxa.So\Views\Home\JishuZhan.cshtml");
//    string filter = "<div id=\"danmu\" class=\"wrap mt1 nav\">";
//    var list = Regex.Split(result, filter);
//    foreach (var item in list)
//    {
//        if (string.IsNullOrEmpty(item)) continue;
//        filter = "<div class=\"btcont\">";
//        string types = item.Substring(item.IndexOf(filter) + filter.Length, item.IndexOf("<") - 2);
//        filter = "<li><a";
//        var list2 = Regex.Split(item, filter);
//        foreach (var x in list2)
//        {
//            var liat3 = Regex.Split(x, "\"");
//            entity = new U_Url_List();
//            entity.Id = System.Guid.NewGuid().ToString("N");
//            entity.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//            entity.Create_Id = "pc";
//            entity.Status = 1;
//            entity.Types = types;
//            if (!x.Trim().StartsWith("href=")) continue;
//            if (ReptitleDownload.VerifyURLIsValid(liat3[1]))
//                entity.Url = liat3[1];
//            else if (ReptitleDownload.VerifyURLIsValid(liat3[2]))
//                entity.Url = liat3[2];
//            entity.Name = liat3[3];
//            entity.Description = liat3[liat3.Length - 1];
//            filter = "<";
//            if (entity.Description.Contains(filter))
//                entity.Description = entity.Description.Substring(1, entity.Description.IndexOf(filter) - 1);
//            if (ReptitleDownload.VerifyURLIsValid(entity.Url) && !string.IsNullOrEmpty(entity.Name))
//            {
//                entity.Source = "技术站";
//                listEntity.Add(entity);
//                Console.WriteLine(string.Format("{0} {1} {2}写入成功", entity.Types, entity.Name, entity.Url));
//            }
//            else
//                Console.WriteLine(string.Format("{0} {1} {2}写入数据异常", entity.Types, entity.Name, entity.Url));

//        }
//    }
//    Console.WriteLine(string.Format("成功写入{0}条数据", listEntity.Count));
//    bll.Add(listEntity);
//    return listEntity.Count;
//}
//#endregion
//#region GetRYM
//public static int GetRYM()
//{
//    var bll = new U_Url_ListBll();
//    U_Url_List entity;
//    List<U_Url_List> listEntity = new List<U_Url_List>();
//    var result = File.ReadAllText(@"E:\工作室\sh\Mmmxa.So\navigation\Mmmxa.So\Views\Home\RenYiMen.cshtml");
//    string filter = "<div id=\"wangpan\" class=\"wrap mt1 nav\">";
//    var list = Regex.Split(result, filter);
//    foreach (var item in list)
//    {
//        if (string.IsNullOrEmpty(item)) continue;
//        filter = "<div class=\"btcont\">";
//        string types = item.Substring(item.IndexOf(filter) + filter.Length, item.IndexOf("<") - 2);
//        filter = "<li><a";
//        var list2 = Regex.Split(item, filter);
//        foreach (var x in list2)
//        {
//            var liat3 = Regex.Split(x, "\"");
//            entity = new U_Url_List();
//            entity.Id = System.Guid.NewGuid().ToString("N");
//            entity.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//            entity.Create_Id = "pc";
//            entity.Status = 1;
//            entity.Types = types;
//            if (!x.Trim().StartsWith("href=")) continue;
//            if (ReptitleDownload.VerifyURLIsValid(liat3[1]))
//                entity.Url = liat3[1];
//            else if (ReptitleDownload.VerifyURLIsValid(liat3[2]))
//                entity.Url = liat3[2];
//            entity.Name = liat3[3];
//            entity.Description = liat3[liat3.Length - 1];
//            filter = "<";
//            if (entity.Description.Contains(filter))
//                entity.Description = entity.Description.Substring(1, entity.Description.IndexOf(filter) - 1);
//            if (ReptitleDownload.VerifyURLIsValid(entity.Url) && !string.IsNullOrEmpty(entity.Name))
//            {
//                entity.Source = "任意门";
//                listEntity.Add(entity);
//                Console.WriteLine(string.Format("{0} {1} {2}写入成功", entity.Types, entity.Name, entity.Url));
//            }
//            else
//                Console.WriteLine(string.Format("{0} {1} {2}写入数据异常", entity.Types, entity.Name, entity.Url));

//        }
//    }
//    Console.WriteLine(string.Format("成功写入{0}条数据", listEntity.Count));
//    bll.Add(listEntity);
//    return listEntity.Count;
//}
//#endregion
//#region 影视库
//public static int GetYSK()
//{
//    var bll = new U_Url_ListBll();
//    U_Url_List entity;
//    List<U_Url_List> listEntity = new List<U_Url_List>();
//    var result = File.ReadAllText(@"E:\工作室\sh\Mmmxa.So\navigation\Mmmxa.So\Views\Home\YingShiKu.cshtml");
//    string filter = "<div id=\"zonghe\" class=\"wrap mt1 nav\">";
//    var list = Regex.Split(result, filter);
//    foreach (var item in list)
//    {
//        if (string.IsNullOrEmpty(item)) continue;
//        filter = "<div class=\"btcont\">";
//        string types = item.Substring(item.IndexOf(filter) + filter.Length, item.IndexOf("<") - 2);
//        filter = "<li><a";
//        var list2 = Regex.Split(item, filter);
//        foreach (var x in list2)
//        {
//            var liat3 = Regex.Split(x, "\"");
//            entity = new U_Url_List();
//            entity.Id = System.Guid.NewGuid().ToString("N");
//            entity.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
//            entity.Create_Id = "pc";
//            entity.Status = 1;
//            entity.Types = types;
//            if (!x.Trim().StartsWith("href=")) continue;
//            if (ReptitleDownload.VerifyURLIsValid(liat3[1]))
//                entity.Url = liat3[1];
//            else if (ReptitleDownload.VerifyURLIsValid(liat3[2]))
//                entity.Url = liat3[2];
//            entity.Name = liat3[3];
//            entity.Description = liat3[liat3.Length - 1];
//            filter = "<";
//            if (entity.Description.Contains(filter))
//                entity.Description = entity.Description.Substring(1, entity.Description.IndexOf(filter) - 1);
//            if (ReptitleDownload.VerifyURLIsValid(entity.Url) && !string.IsNullOrEmpty(entity.Name))
//            {
//                entity.Source = "影视库";
//                listEntity.Add(entity);
//                Console.WriteLine(string.Format("{0} {1} {2}写入成功", entity.Types, entity.Name, entity.Url));
//            }
//            else
//                Console.WriteLine(string.Format("{0} {1} {2}写入数据异常", entity.Types, entity.Name, entity.Url));

//        }
//    }
//    Console.WriteLine(string.Format("成功写入{0}条数据", listEntity.Count));
//    bll.Add(listEntity);
//    return listEntity.Count;
//}
//#endregion 
#endregion