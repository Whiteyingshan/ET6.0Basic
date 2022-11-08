using System;

namespace ET
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class ViewMethodAttribute : BaseAttribute
    {
        public string UIResName { get; set; }

        public ViewMethodAttribute()
        {
        }

        public ViewMethodAttribute(string uIResName)
        {
            UIResName = uIResName;
        }
    }
}