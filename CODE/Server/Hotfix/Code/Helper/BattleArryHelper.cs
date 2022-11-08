using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ET;
namespace ET.Code.Helper
{
    internal static class BattleArryHelper
    {
        public static UnityEngine.Vector3 toV3(this Vector3Info info)
        {
            return new UnityEngine.Vector3(info.X, info.Y, info.Z);
        }
        public static Vector3Info toV3Info(this UnityEngine.Vector3 v3)
        {
            return new Vector3Info() { X = v3.x, Y = v3.y, Z = v3.z };
        }

    }
}
