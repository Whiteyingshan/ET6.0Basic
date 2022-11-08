using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class cSpineAssetConfigCategory : ProtoObject
    {
        public static cSpineAssetConfigCategory Instance;

        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<long, cSpineAssetConfig> dict = new Dictionary<long, cSpineAssetConfig>();

        [BsonElement]
        [ProtoMember(1)]
        private List<cSpineAssetConfig> list = new List<cSpineAssetConfig>();

        public cSpineAssetConfigCategory()
        {
            Instance = this;
        }

        public override void EndInit()
        {
            foreach (cSpineAssetConfig config in list)
            {
                config.EndInit();
                this.dict.Add(config.Id, config);
            }
            this.AfterEndInit();
        }

        public cSpineAssetConfig Get(long id)
        {
            this.dict.TryGetValue(id, out cSpineAssetConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof(cSpineAssetConfig)}，配置id: {id}");
            }

            return item;
        }

        public bool Contain(long id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<long, cSpineAssetConfig> GetAll()
        {
            return this.dict;
        }

        public cSpineAssetConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
    public partial class cSpineAssetConfig : ProtoObject, IConfig
    {
        [ProtoMember(1)]
        public long Id { get; set; }
        [ProtoMember(2)]
        public string[] AssetPath { get; set; }
        [ProtoMember(3)]
        public double TopFUIOffset { get; set; }

    }
}