/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;

namespace ET._Component_Public
{
    public sealed partial class NoticeBtn : FObject
    {
        public const string URL = "ui://5ardt36yang3d6";
        public const string UIResName = "NoticeBtn";
        public const string UIPackageName = "_Component_Public";

        public override string ResName => UIResName;

        /// <summary>
        /// NoticeBtn的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GButton self { get; private set; }

        public Controller button;
        public GLoader icon;
        public GTextField title;

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

        internal static NoticeBtn Create(GObject go)
        {
            var fui = Fetch<NoticeBtn>();
            fui.Init(go);
            return fui;
        }

        public static NoticeBtn CreateInstance()
        {
            return Create(CreateGObject());
        }

        public static async Task<NoticeBtn> CreateInstanceAsync()
        {
            return Create(await CreateGObjectAsync());
        }

        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static NoticeBtn GetFormPool(GObject go)
        {
            var fui = go.GetFObject<NoticeBtn>();
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

            self = (GButton)go;

            self.AddFObject(this);

            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = UIResName;
            }

            var com = go.asCom;

            if (com != null)
            {
                button = com.GetControllerAt(0);
                icon = (GLoader)com.GetChildAt(0);
                title = (GTextField)com.GetChildAt(1);
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
            button = null;
            icon = null;
            title = null;

            base.Dispose();
        }
    }
}