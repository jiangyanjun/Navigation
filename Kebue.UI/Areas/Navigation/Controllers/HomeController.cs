using CommonLayer;
using CommonLayer.Enum;
using Kebue.UI.Areas.Admin.Controllers;
using Kebue.UI.Models;
using PhysicalLayer;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kebue.UI.Areas.Navigation.Controllers
{
    public class HomeController : Controller
    {
        // GET: Navigation/Home
        public ActionResult Index()
        {
            try
            {
                var result = CacheHelper.GetCache(CacheKeyEnum.TypesScript.ToEnumDiscription()) as string;
                if (result.IsNull())
                {
                    result = HttpHelp.Post<string, string>(ActionEnum.TypesScript, null);
                    CacheHelper.SetCache(CacheKeyEnum.TypesScript.ToEnumDiscription(), result);
                }
                ViewBag.Types = result;
                if (GetStr.GetCurrentMinute % 59 != 0)
                {
                    Task.Factory.StartNew(() => new AJAXRequestAPIController().PerformTask());
                }
                //var keywords = CacheHelper.GetCache(ActionEnum.GetJoinTypeFind.ToString()) as string;
                //if (keywords.IsNull())
                //{
                //    StringBuilder stringBuilder = new StringBuilder();
                //    var p = new { Status = EnumUrlStatus.Normal.GetHashCode() };
                //    var returnInfo = HttpHelp.Post<List<U_Url_List>, dynamic>(ActionEnum.GetJoinTypeFind, p);
                //    returnInfo.ForEach(para =>
                //    {
                //        stringBuilder.AppendFormat("{0},", para.Name);
                //    });
                //    keywords = stringBuilder.ToString();
                //    CacheHelper.SetCache(ActionEnum.GetJoinTypeFind.ToString(), keywords);
                //}
                //ViewBag.keywords = keywords;
            }
            catch (System.Exception ex)
            {
                LogHelper.AddLog(ex);
            }
            return View();
        }

        public ActionResult AddUrl()
        {
            try
            {
                var result = CacheHelper.GetCache(CacheKeyEnum.GetU_Url_TypeList.ToEnumDiscription()) as List<U_Url_Type>;
                if (result.IsNull())
                {
                    result = HttpHelp.Post<List<U_Url_Type>, string>(ActionEnum.GetU_Url_TypeList, null);
                    CacheHelper.SetCache(CacheKeyEnum.GetU_Url_TypeList.ToEnumDiscription(), result);
                }
                ViewBag.Types = result;
            }
            catch (System.Exception ex)
            {
                LogHelper.AddLog(ex);
            }
            return View();
        }
    }
}