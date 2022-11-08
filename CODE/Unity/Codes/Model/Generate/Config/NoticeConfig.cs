using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class NoticeConfigCategory : ProtoObject
    {
        public static NoticeConfigCategory Instance;

        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<long, NoticeConfig> dict = new Dictionary<long, NoticeConfig>();

        [BsonElement]
        [ProtoMember(1)]
        private List<NoticeConfig> list = new List<NoticeConfig>();

        public NoticeConfigCategory()
        {
            Instance = this;
        }

        public override void EndInit()
        {
            foreach (NoticeConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }
            this.AfterEndInit();
        }

        public NoticeConfig Get(long id)
        {
            this.dict.TryGetValue(id, out NoticeConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof(NoticeConfig)}，配置id: {id}");
            }

            return item;
        }

        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, NoticeConfig> GetAll()
        {
            return this.dict;
        }

        public NoticeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
    public partial class NoticeConfig : ProtoObject, IConfig
    {
        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public string MessageContent { get; set; }
        [ProtoMember(3)]
        public int MessageType { get; set; }

    }
}