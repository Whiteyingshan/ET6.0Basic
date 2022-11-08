using System;
using System.Collections.Generic;

namespace ET.Module.Numeric
{
    public sealed class NumericExpressionComponent : Entity
    {
        private sealed class NumericExpressionComponentAwake : AwakeSystem<NumericExpressionComponent>
        {
            public override void Awake(NumericExpressionComponent self)
            {
                self.Load();
            }
        }

        private sealed class NumericExpressionComponentLoad : LoadSystem<NumericExpressionComponent>
        {
            public override void Load(NumericExpressionComponent self)
            {
                self.Load();
            }
        }

        private readonly Dictionary<NumericType, INumericExpression> Expressions = new Dictionary<NumericType, INumericExpression>();

        private void Load()
        {
            Expressions.Clear();
            HashSet<Type> types = Game.EventSystem.GetTypes(typeof(NumericExpressionAttribute));
            foreach (Type type in types)
            {
                object[] attrs = type.GetCustomAttributes(typeof(NumericExpressionAttribute), false);
                INumericExpression expression = Activator.CreateInstance(type) as INumericExpression;
                foreach (object attr in attrs)
                {
                    NumericExpressionAttribute attribute = attr as NumericExpressionAttribute;
                    Expressions.Add(attribute.Type, expression);
                }
            }
        }

        internal INumericExpression this[long key] => this[(NumericType)key];

        internal INumericExpression this[NumericType key] => Expressions.ContainsKey(key) ? Expressions[key] : null;
    }
}