using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ECActivityAPI.Controllers
{
    /// <summary>
    /// 测试API
    /// </summary>
    public class ValuesController : ApiController
    {
        /// <summary>
        /// 测试接口
        /// </summary>
        /// <returns></returns>
        public string Get(string requestData)
        {

            return "123";
        }
    }
}
