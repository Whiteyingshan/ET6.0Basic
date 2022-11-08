using FairyGUI;
using System.Collections.Generic;

namespace ET
{
    public static class GObjectHelper
    {
        private static readonly Dictionary<GObject, FObject> keyValuePairs = new Dictionary<GObject, FObject>(1024);

        internal static void AddFObject(this GObject self, FObject fObject)
        {
            if (self != null && fObject != null)
            {
                keyValuePairs[self] = fObject;
            }
        }

        internal static FObject RemoveFObject(this GObject self)
        {
            if (self != null && keyValuePairs.ContainsKey(self))
            {
                var result = keyValuePairs[self];
                keyValuePairs.Remove(self);
                return result;
            }

            return default;
        }

        public static FObject GetFObject(this GObject self)
        {
            if (self != null && keyValuePairs.ContainsKey(self))
            {
                return keyValuePairs[self];
            }

            return default;
        }

        public static T GetFObject<T>(this GObject self) where T : FObject
        {
            return GetFObject(self) as T;
        }
    }
}