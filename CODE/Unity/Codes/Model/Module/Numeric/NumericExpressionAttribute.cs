using System;

namespace ET.Module.Numeric
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class NumericExpressionAttribute : BaseAttribute
    {
        public NumericType Type;

        public NumericExpressionAttribute(NumericType type)
        {
            Type = type;
        }
    }
}