using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace CreditSysWeChat.Common
{
    public class EnumExtend
    {
        /// <summary>
        /// 将枚举转化成SelectList
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="exception">例外数组</param>
        /// <returns></returns>
        public static SelectList SelectListEnum(Type enumType, object defaultValue,params int[] exception)
        {
            IDictionary<int, string> dic = new Dictionary<int, string>();
            System.Reflection.FieldInfo[] fields = enumType.GetFields();

            foreach (FieldInfo field in fields)
            {
                // 过滤掉一个不是枚举值的，记录的是枚举的源类型 
                if (field.FieldType.IsEnum == false)
                {
                    continue;
                }

                // 通过字段的名字得到枚举的值 
                int key = (int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null);
                string value = field.Name;

                //判断是否例外
                if (exception.Contains(key))
                {
                    continue;
                }

                dic.Add(key, value);
            }
            return new SelectList(dic, "key", "value", defaultValue);
        }

        /// <summary>
        /// 将枚举类型转换成字典，适用于selectList
        /// </summary>
        /// <param name="enumType">枚举类型必须顺序累加出现(如0 1 2 3 ...)</param>
        private static IDictionary<int, string> EnumToIEnumerable(Type enumType)
        {
            IDictionary<int, string> dic = new Dictionary<int, string>();

            int i = 0;
            while (Enum.IsDefined(enumType, i))
            {
                dic.Add(i, Enum.GetName(enumType, i));
                i++;
            }
            return dic;
        }
    }
}
