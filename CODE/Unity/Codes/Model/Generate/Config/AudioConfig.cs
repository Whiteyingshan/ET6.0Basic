using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class AudioConfigCategory : ProtoObject
    {
        public static AudioConfigCategory Instance;

        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<long, AudioConfig> dict = new Dictionary<long, AudioConfig>();

        [BsonElement]
        [ProtoMember(1)]
        private List<AudioConfig> list = new List<AudioConfig>();

        public AudioConfigCategory()
        {
            Instance = this;
        }

        public override void EndInit()
        {
            foreach (AudioConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }
            this.AfterEndInit();
        }

        public AudioConfig Get(long id)
        {
            this.dict.TryGetValue(id, out AudioConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof(AudioConfig)}，配置id: {id}");
            }

            return item;
        }

        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, AudioConfig> GetAll()
        {
            return this.dict;
        }

        public AudioConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
    public partial class AudioConfig : ProtoObject, IConfig
    {
        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public int Type { get; set; }
        [ProtoMember(3)]
        public string Name { get; set; }
        [ProtoMember(4)]
        public string Path { get; set; }

    }
}