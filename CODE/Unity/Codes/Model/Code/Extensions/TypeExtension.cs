using System;

namespace ET
{
    public static class TypeExtension
    {
        public static T GetCustomAttribute<T>(this Type type) where T : Attribute
        {
            return (T)type.GetCustomAttributes(true)[0];
        }
    }
}