using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class StartProcessConfigCategory : ProtoObject
    {
        public static StartProcessConfigCategory Instance;

        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<long, StartProcessConfig> dict = new Dictionary<long, StartProcessConfig>();

        [BsonElement]
        [ProtoMember(1)]
        private List<StartProcessConfig> list = new List<StartProcessConfig>();

        public StartProcessConfigCategory()
        {
            Instance = this;
        }

        public override void EndInit()
        {
            foreach (StartProcessConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }
            this.AfterEndInit();
        }

        public StartProcessConfig Get(long id)
        {
            this.dict.TryGetValue(id, out StartProcessConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof(StartProcessConfig)}，配置id: {id}");
            }

            return item;
        }

        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, StartProcessConfig> GetAll()
        {
            return this.dict;
        }

        public StartProcessConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
    public partial class StartProcessConfig : ProtoObject, IConfig
    {
        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public int MachineId { get; set; }
        [ProtoMember(3)]
        public int InnerPort { get; set; }
        [ProtoMember(4)]
        public string AppName { get; set; }

    }
}