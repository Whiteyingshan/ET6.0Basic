using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class NumericTypeConfigCategory : ProtoObject
    {
        public static NumericTypeConfigCategory Instance;

        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<long, NumericTypeConfig> dict = new Dictionary<long, NumericTypeConfig>();

        [BsonElement]
        [ProtoMember(1)]
        private List<NumericTypeConfig> list = new List<NumericTypeConfig>();

        public NumericTypeConfigCategory()
        {
            Instance = this;
        }

        public override void EndInit()
        {
            foreach (NumericTypeConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }
            this.AfterEndInit();
        }

        public NumericTypeConfig Get(long id)
        {
            this.dict.TryGetValue(id, out NumericTypeConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof(NumericTypeConfig)}，配置id: {id}");
            }

            return item;
        }

        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, NumericTypeConfig> GetAll()
        {
            return this.dict;
        }

        public NumericTypeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
    public partial class NumericTypeConfig : ProtoObject, IConfig
    {
        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public string NumericCode { get; set; }
        [ProtoMember(3)]
        public int Type { get; set; }
        [ProtoMember(4)]
        public string Remarks { get; set; }
        [ProtoMember(5)]
        public string Expression { get; set; }

    }
}