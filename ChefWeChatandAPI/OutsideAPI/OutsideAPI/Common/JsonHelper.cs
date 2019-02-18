using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OutsideAPI.Common
{
    public class JsonHelper
    {
        #region 对象的序列化、反序列化

        /// <summary>
        /// 对象序列化成json
        /// </summary>
        /// <param name="toJsonValue">要转化成Json的对象</param>
        /// <returns>返回序列化后的字符串</returns>
        public static string ObjectToJson(object toJsonValue)
        {
            return toJsonValue == null ? "" : JsonConvert.SerializeObject(toJsonValue);
        }

        /// <summary>
        /// json反序列化成对象
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="json">反序列化字符串</param>
        /// <returns>返回反序列化后的实体T</returns>
        public static T JsonToObject<T>(string json)
        {
            var t = JsonConvert.DeserializeObject<T>(json);

            return t;
        }

        /// <summary>
        /// json反序列化成list
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="json">反序列化字符串</param>
        /// <returns>返回反序列化后的实体T</returns>
        public static List<T> JsonToList<T>(string json)
        {
            var t = JsonConvert.DeserializeObject<List<T>>(json);

            return t;
        }

        #endregion
    }
}
