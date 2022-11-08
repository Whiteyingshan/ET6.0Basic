/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;

namespace ET.DengLuZhuCe
{
    public sealed partial class LoginBtn : FObject
    {
        public const string URL = "ui://fkxb3vu5sbpg6c";
        public const string UIResName = "LoginBtn";
        public const string UIPackageName = "登陆注册";

        public override string ResName => UIResName;

        /// <summary>
        /// LoginBtn的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GButton self { get; private set; }

        public Controller button;
        public GGraph n0;
        public GGraph n1;
        public GGraph n2;

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

        internal static LoginBtn Create(GObject go)
        {
            var fui = Fetch<LoginBtn>();
            fui.Init(go);
            return fui;
        }

        public static LoginBtn CreateInstance()
        {
            return Create(CreateGObject());
        }

        public static async Task<LoginBtn> CreateInstanceAsync()
        {
            return Create(await CreateGObjectAsync());
        }

        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static LoginBtn GetFormPool(GObject go)
        {
            var fui = go.GetFObject<LoginBtn>();
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
                n0 = (GGraph)com.GetChildAt(0);
                n1 = (GGraph)com.GetChildAt(1);
                n2 = (GGraph)com.GetChildAt(2);
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
            n0 = null;
            n1 = null;
            n2 = null;

            base.Dispose();
        }
    }
}