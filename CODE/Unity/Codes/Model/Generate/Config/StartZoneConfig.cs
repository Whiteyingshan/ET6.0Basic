using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class StartZoneConfigCategory : ProtoObject
    {
        public static StartZoneConfigCategory Instance;

        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<long, StartZoneConfig> dict = new Dictionary<long, StartZoneConfig>();

        [BsonElement]
        [ProtoMember(1)]
        private List<StartZoneConfig> list = new List<StartZoneConfig>();

        public StartZoneConfigCategory()
        {
            Instance = this;
        }

        public override void EndInit()
        {
            foreach (StartZoneConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }
            this.AfterEndInit();
        }

        public StartZoneConfig Get(long id)
        {
            this.dict.TryGetValue(id, out StartZoneConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof(StartZoneConfig)}，配置id: {id}");
            }

            return item;
        }

        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, StartZoneConfig> GetAll()
        {
            return this.dict;
        }

        public StartZoneConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
    public partial class StartZoneConfig : ProtoObject, IConfig
    {
        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public string DBConnection { get; set; }
        [ProtoMember(3)]
        public string DBName { get; set; }

    }
}