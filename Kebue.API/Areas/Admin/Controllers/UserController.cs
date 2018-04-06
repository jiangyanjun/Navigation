using Business;
using CommonLayer;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;

namespace Kebue.API.Areas.Admin.Controllers
{
    /// <summary>
    /// 用户接口API
    /// </summary>
    public class UserController : ApiController
    {
        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="parameter">用户登录：用户和密码</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage Login([FromBody]KeyValuePair<string, string> Key)
        {
            var result = new S_UserInfoBusiness().Login(Key.Key, Key.Value);
            return JsonConvert.SerializeObject(result).ToHttpResponseMessage();
        }
        #endregion
    }
}