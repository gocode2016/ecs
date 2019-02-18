using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECActivityAPI.Common
{
   /// <summary>
   /// Json日期转换
   /// </summary>
    public class DateTimeConvent
    {
        /// <summary>
        /// yyyy-MM-dd hh:mm
        /// </summary>
        /// <returns></returns>
        public static IsoDateTimeConverter DateTimehh()
        {
            var iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyy-MM-dd HH:mm";
            return iso;
        }
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        /// <returns></returns>
        public static IsoDateTimeConverter DateTimedd()
        {
            var iso = new IsoDateTimeConverter();
            iso.DateTimeFormat = "yyyy-MM-dd";
            return iso;
        }
             
    }
}