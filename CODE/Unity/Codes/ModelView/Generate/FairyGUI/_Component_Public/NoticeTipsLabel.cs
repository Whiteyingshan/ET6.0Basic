/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;

namespace ET._Component_Public
{
    public sealed partial class NoticeTipsLabel : FObject
    {
        public const string URL = "ui://5ardt36yang3d4";
        public const string UIResName = "NoticeTipsLabel";
        public const string UIPackageName = "_Component_Public";

        public override string ResName => UIResName;

        /// <summary>
        /// NoticeTipsLabel的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GLabel self { get; private set; }

        public GImage bg;
        public GTextField title;
        public Transition Animation_Show;

        private static GObject CreateGObject()
        {
            return UIPackage.CreateObject(UIPackageName, UIResName);
        }

        private static Task<GObject> CreateGObjectAsync()
        {
            var tcs = new TaskCompletionSource<GObject>();
            UIPackage.CreateObjectAsync(UIPackageName, UIResName, (go) => tcs.SetResult(go));
            return tcs.Task;
        }

        internal static NoticeTipsLabel Create(GObject go)
        {
            var fui = Fetch<NoticeTipsLabel>();
            fui.Init(go);
            return fui;
        }

        public static NoticeTipsLabel CreateInstance()
        {
            return Create(CreateGObject());
        }

        public static async Task<NoticeTipsLabel> CreateInstanceAsync()
        {
            return Create(await CreateGObjectAsync());
        }

        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static NoticeTipsLabel GetFormPool(GObject go)
        {
            var fui = go.GetFObject<NoticeTipsLabel>();
            if (fui == null)
            {
                fui = Create(go);
                fui.IsFromPool = true;
            }
            return fui;
        }

        private void Init(GObject go)
        {
            if (go == null)
            {
                return;
            }

            GObject = go;
            IsDisposed = false;

            self = (GLabel)go;

            self.AddFObject(this);

            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = UIResName;
            }

            var com = go.asCom;

            if (com != null)
            {
                bg = (GImage)com.GetChildAt(0);
                title = (GTextField)com.GetChildAt(1);
                Animation_Show = com.GetTransitionAt(0);
            }
        }

        public override void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            self.RemoveFObject();
            self = null;
            bg = null;
            title = null;
            Animation_Show = null;

            base.Dispose();
        }
    }
}