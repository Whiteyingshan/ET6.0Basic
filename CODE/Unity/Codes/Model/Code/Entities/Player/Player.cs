using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;


namespace ET
{
    [BsonIgnoreExtraElements]
    public sealed class Player : Entity
    {
#if !SERVER
        public static Player Inst { get; set; }
#endif

        public BasicData BasicData
        {
            get => GetComponent<BasicData>() ?? AddComponent<BasicData>();
        }

        public NumericComponent NumericComponent
        {
            get => GetComponent<NumericComponent>() ?? AddComponent<NumericComponent>();
        }

        public override void Dispose()
        {
            if (this.IsDisposed)
                return;

            base.Dispose();
        }
    }
}