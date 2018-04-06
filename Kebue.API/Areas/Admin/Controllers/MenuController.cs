using Business;
using CommonLayer;
using Newtonsoft.Json;
using System.Net.Http;
using System.Web.Http;

namespace Kebue.API.Areas.Admin.Controllers
{
    public class MenuController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage GetMenuList()
        {
            var result = new S_MenuBLL().Find();
            return JsonConvert.SerializeObject(result).ToHttpResponseMessage();
        }
        [HttpPost]
        public HttpResponseMessage GetHomePageCount()
        {
            var result = new S_MenuBLL().GetHomePageCount();
            return JsonConvert.SerializeObject(result).ToHttpResponseMessage();
        }
    }
}