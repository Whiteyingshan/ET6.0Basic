using System;
using System.Collections.Generic;

namespace ET
{
    public class DictionaryComponent<Tkey, TValue> : Dictionary<Tkey, TValue>, IDisposable
    {
        public static DictionaryComponent<Tkey, TValue> Create()
        {
            return MonoPool.Instance.Fetch(typeof(DictionaryComponent<Tkey, TValue>)) as DictionaryComponent<Tkey, TValue>;
        }

        public void Dispose()
        {
            this.Clear();
            MonoPool.Instance.Recycle(this);
        }
    }
}