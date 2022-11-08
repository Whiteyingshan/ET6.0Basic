/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;

namespace ET._Component_Main
{
    public sealed partial class MainMiddleComponent : FObject
    {
        public const string URL = "ui://5job6nn8ny110";
        public const string UIResName = "MainMiddleComponent";
        public const string UIPackageName = "_Component_Main";

        public override string ResName => UIResName;

        /// <summary>
        /// MainMiddleComponent的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self { get; private set; }


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

        internal static MainMiddleComponent Create(GObject go)
        {
            var fui = Fetch<MainMiddleComponent>();
            fui.Init(go);
            return fui;
        }

        public static MainMiddleComponent CreateInstance()
        {
            return Create(CreateGObject());
        }

        public static async Task<MainMiddleComponent> CreateInstanceAsync()
        {
            return Create(await CreateGObjectAsync());
        }

        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static MainMiddleComponent GetFormPool(GObject go)
        {
            var fui = go.GetFObject<MainMiddleComponent>();
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

            base.Dispose();
        }
    }
}