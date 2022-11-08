using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine;

namespace ET
{
    public sealed class SceneAssetComponent : Entity
    {
        public readonly Dictionary<string, Asset> AssetCache = new Dictionary<string, Asset>();
    }
}