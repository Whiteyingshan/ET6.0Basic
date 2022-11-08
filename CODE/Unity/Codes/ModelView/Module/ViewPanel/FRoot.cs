using FairyGUI;

namespace ET
{
    public sealed class FRoot : FObject
    {
        public static readonly FRoot inst = new FRoot();

        public GRoot GRoot { get; private set; }

        private FRoot()
        {
            GRoot = GRoot.inst;
            GObject = GRoot.inst;
        }

        public void Add(FObject fObject)
        {
            Add(fObject, true);
        }

        public void SetChildIndex(FObject fObject, int index)
        {
            GRoot.inst.SetChildIndex(fObject.GObject, index);
        }

        public override void Dispose()
        {
            GObject[] gObjects = GRoot.GetChildren();

            foreach (GObject item in gObjects)
            {
                GObjectHelper.GetFObject<FObject>(item)?.Dispose();
            }
        }
    }
}