using UnityEngine;

namespace ET.Code
{
    public sealed class UnitObject
    {
        public Transform Transform;
        public Animator Animation;
        public Transform BallPoint;

        public static UnitObject Parse(GameObject gameObject)
        {
            UnitObject obj = new UnitObject
            {
                Transform = gameObject.transform,
                Animation = gameObject.GetComponent<Animator>()
            };

            obj.BallPoint = obj.Transform.Find("BallPoint");
            if (obj.BallPoint == null)
            {
                obj.BallPoint = new GameObject("BallPoint").transform;
                obj.BallPoint.SetParent(obj.Transform);
                obj.BallPoint.localPosition = Vector3.zero;
            }

            return obj;
        }
    }
}