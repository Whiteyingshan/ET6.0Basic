using ET.Code;
using UnityEngine;

namespace ET
{
    public class GameObjectComponent : Entity
    {
        public GameObject GameObject { get; set; }
        public UnitObject UnitObject { get; set; }

        public override void Dispose()
        {
            base.Dispose();
            GameObject = null;
            UnitObject = null;
        }
    }
}