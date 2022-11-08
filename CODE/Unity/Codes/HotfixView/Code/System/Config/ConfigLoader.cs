using System.Collections.Generic;
using System.IO;
using UnityEngine;
using VEngine;

namespace ET
{
    public class ConfigLoader : IConfigLoader
    {
        public void GetAllConfigBytes(Dictionary<string, byte[]> output)
        {
            Asset[] assets = AssetHelper.GetAssets("Assets/Bundles/Config/", typeof(TextAsset));
            foreach (Asset asset in assets)
            {
                output[asset.asset.name] = (asset.asset as TextAsset).bytes;
                asset.Release();
            }
        }

        public byte[] GetOneConfigBytes(string configName)
        {
            byte[] output = null;
            Asset asset = Asset.Load($"Assets/Bundles/Config/{configName}.bytes", typeof(TextAsset));
            output = (asset.asset as TextAsset).bytes;
            asset.Release();
            return output;
        }
    }
}