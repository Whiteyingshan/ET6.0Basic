using System;
using System.ComponentModel;

namespace ET
{
    public static class EnumHelper
    {
        public static int EnumIndex<T>(int value)
        {
            int i = 0;
            foreach (object v in Enum.GetValues(typeof(T)))
            {
                if ((int)v == value)
                {
                    return i;
                }
                ++i;
            }
            return -1;
        }

        public static T FromString<T>(string str)
        {
            if (!Enum.IsDefined(typeof(T), str))
            {
                return default(T);
            }
            return (T)Enum.Parse(typeof(T), str);
        }

        /// <summary>
        /// 获取Enum描述信息
        /// </summary> 
        public static string GetDes(Type eType, int index)
        {
            string name = Enum.GetName(eType, index);

            if (name == null) throw new Exception($"{eType} 未找到！");

            return GetDes(eType, name);
        }
        static string GetDes(Type eType, string name)
        {
            DescriptionAttribute dec = null;
            var fieldInfo = eType.GetField(name);

            if (fieldInfo != null)
                dec = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute), false) as DescriptionAttribute;

            if (dec != null && !string.IsNullOrEmpty(dec.Description))
                return dec.Description;

            throw new Exception($"{eType} 未添加描述标记！");
        }
    }
}