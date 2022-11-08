using System.Collections.Generic;

namespace ET
{
    [MessageIOC]
    internal class NumericIOC : MessageConvert<NumericComponent, NumericInfo>
    {
        public override void ToObject(NumericComponent entity, NumericInfo message)
        {
            entity.Clear();
            foreach (Long_Long item in message.Numerics)
            {
                entity[item.Key] = item.Value;
            }
        }

        public override void ToMessage(NumericInfo message, NumericComponent entity)
        {
            message.Numerics = new List<Long_Long>();
            foreach (var item in entity.Numeric)
            {
                message.Numerics.Add(new Long_Long() { Key = item.Key, Value = item.Value });
            }
            foreach (var item in entity.NumericDB)
            {
                message.Numerics.Add(new Long_Long() { Key = long.Parse(item.Key), Value = item.Value });
            }
        }
    }
}