using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ET
{
    public static class TransformEx
    {
        public static void SetChildrenLayers(this Transform self, LayerMask layerMask)
        {
            self.gameObject.layer = layerMask;
            var childrenTrans = self.GetComponentsInChildren<Transform>(true);
            for (int i = 0; i < childrenTrans.Length; i++)
            {
                childrenTrans[i].gameObject.layer = layerMask;
                //var child = childrenTrans[i];
                //if (child == self) continue;
                //childrenTrans[i].SetChildrenLayers(layerMask);
            }
        }
    }
}
