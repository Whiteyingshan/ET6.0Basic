/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;

namespace ET._Component_Main
{
    public sealed partial class MainBottomComponent : FObject
    {
        public const string URL = "ui://5job6nn8mb012";
        public const string UIResName = "MainBottomComponent";
        public const string UIPackageName = "_Component_Main";

        public override string ResName => UIResName;

        /// <summary>
        /// MainBottomComponent的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self { get; private set; }

        public ZhuJieMian.GameLobby n4;

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

        internal static MainBottomComponent Create(GObject go)
        {
            var fui = Fetch<MainBottomComponent>();
            fui.Init(go);
            return fui;
        }

        public static MainBottomComponent CreateInstance()
        {
            return Create(CreateGObject());
        }

        public static async Task<MainBottomComponent> CreateInstanceAsync()
        {
            return Create(await CreateGObjectAsync());
        }

        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static MainBottomComponent GetFormPool(GObject go)
        {
            var fui = go.GetFObject<MainBottomComponent>();
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

            self = (GComponent)go;

            self.AddFObject(this);

            if (string.IsNullOrWhiteSpace(Name))
            {
                Name = UIResName;
            }

            var com = go.asCom;

            if (com != null)
            {
                n4 = ZhuJieMian.GameLobby.Create(com.GetChildAt(0));
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
            n4.Dispose();
            n4 = null;

            base.Dispose();
        }
    }
}