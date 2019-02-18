using System;
using System.Collections.Generic;
using CommonAPI.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using Newtonsoft.Json;

namespace CommonAPI.Controllers
{
    /// <summary>
    /// 省市区三级查询部分
    /// </summary>
    public class RegionsController : ApiController
    {
        private SqlHelper dataContext = new SqlHelper();

        /// <summary>
        /// 获取所有省
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetAllProvince()
        {
            var sql = string.Format("select ProvinceId, ProvinceName from Province");
            var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
            return JsonConvert.SerializeObject(q);
        }

        /// <summary>
        /// 根据省ID获取市，如需要查询全部则不需要传参数
        /// </summary>
        /// <param name="provinceId">省ID</param>
        /// <returns></returns>
        [HttpGet]
        public string GetCityList(int provinceId = 0)
        {
            if (provinceId > 0)
            {
                string sql = string.Format("select ProvinceId, CityId, CityName from City where ProvinceId = {0}", provinceId);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(q);
            }
            else
            {
                string sql = string.Format("select ProvinceId, CityId, CityName from City");
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(q);
            }
        }

        /// <summary>
        /// 根据市ID获取区,如果需查询全部则不需要传参数
        /// </summary>
        /// <param name="cityId">市ID</param>
        /// <returns></returns>
        [HttpGet]
        public string GetAreaList(int cityId = 0)
        {
            if (cityId > 0)
            {
                string sql = string.Format("select CityId, AreaId, AreaName from Area where CityId = {0}", cityId);
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(q);
            }
            else
            {
                string sql = string.Format("select CityId, AreaId, AreaName from Area");
                var q = dataContext.ExecuteDataTable(CommandType.Text, sql);
                return JsonConvert.SerializeObject(q);
            }
        }

        /// <summary>
        /// 获取所有省市区及城市级别
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetRegion()
        {
            //string sql = string.Format("select p.ProvinceId, p.ProvinceName, c.CityId, c.CityName, a.AreaId, a.AreaName, a.AreaLevel from Province as p left join City as c on p.ProvinceId = c.ProvinceId left join Area as a on c.CityId = a.CityId");

            var sql = string.Format("select ProvinceId, ProvinceName from Province");
            var pro = dataContext.ExecuteDataTable(CommandType.Text, sql);
            string province = JsonConvert.SerializeObject(pro);

            var sql2 = string.Format("select ProvinceId, CityId, CityName from City");
            var cit = dataContext.ExecuteDataTable(CommandType.Text, sql2);
            string city = JsonConvert.SerializeObject(cit);

            var sql3 = string.Format("select CityId, AreaId, AreaName, AreaLevel from Area");
            var are = dataContext.ExecuteDataTable(CommandType.Text, sql3);
            string area = JsonConvert.SerializeObject(are);

            return "{\"Province\":" + province + ",\"City\":" + city + ", \"Area\": "+ area +" }";
        }

        /// <summary>
        /// 查询省市区
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        [HttpPost]
        public string FindCity(dynamic requestData)
        {
            string province = requestData.Province;
            string city = requestData.City;
            string area = requestData.Area;

            var sql = string.Format("select ProvinceId, ProvinceName from Province where ProvinceName like '%{0}%'", province);
            var pro = dataContext.ExecuteDataTable(CommandType.Text, sql);
            string provinces = JsonConvert.SerializeObject(pro);

            var sql2 = string.Format("select CityId, CityName from City where CityName like '%{0}%'", city);
            var cit = dataContext.ExecuteDataTable(CommandType.Text, sql2);
            string citys = JsonConvert.SerializeObject(cit);

            var sql3 = string.Format("select AreaId, AreaName from Area where AreaName like '%{0}%'", area);
            var are = dataContext.ExecuteDataTable(CommandType.Text, sql3);
            string areas = JsonConvert.SerializeObject(are);

            if (string.IsNullOrEmpty(provinces) || string.IsNullOrEmpty(citys) || string.IsNullOrEmpty(areas))
            {
                return "none";
            }

            return "{\"Province\":" + provinces + ",\"City\":" + citys + ", \"Area\": " + areas + " }"; ;
        }

    }
}
