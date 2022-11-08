using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;
#if !NOT_UNITY
using UnityEngine;
using LitJson;

#endif

namespace ET
{
    public abstract class Object : ISupportInitialize, IDisposable
    {
#if VIEWGO
        [BsonIgnore]
        [JsonIgnore]
        [IgnoreDataMember]
        public static GameObject Global => GameObject.Find("/Global");

        [BsonIgnore]
        [JsonIgnore]
        [IgnoreDataMember]
        public GameObject ViewGO
        {
            get;
        }

        public string ViewData;
#endif

        public Object()
        {
#if VIEWGO
            if (!this.GetType().IsDefined(typeof (HideInHierarchy), true) && Log.NeedLog)
            {
                this.ViewGO = new GameObject();
                this.ViewGO.name = this.GetType().Name;
                this.ViewGO.layer = LayerMask.NameToLayer("Hidden");
                this.ViewGO.transform.SetParent(Global.transform, false);
                this.ViewGO.AddComponent<ComponentView>().Component = this;
            }
#endif
        }

        public virtual void BeginInit()
        {
        }

        public virtual void EndInit()
        {
        }

        public virtual void Dispose()
        {
#if VIEWGO
            if (this.ViewGO != null)
            {
                UnityEngine.Object.Destroy(this.ViewGO);
            }
#endif
        }

        public override string ToString()
        {
            return JsonHelper.ToJson(this);
        }
    }
}