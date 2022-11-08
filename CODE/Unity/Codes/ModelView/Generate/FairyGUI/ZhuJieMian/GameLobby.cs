/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;

namespace ET.ZhuJieMian
{
    public sealed partial class GameLobby : FObject
    {
        public const string URL = "ui://frpzwf8asbpgz38r";
        public const string UIResName = "GameLobby";
        public const string UIPackageName = "主界面";

        public override string ResName => UIResName;

        /// <summary>
        /// GameLobby的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self { get; private set; }

        public GGraph BG;
        public GTextField UserName;

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

        internal static GameLobby Create(GObject go)
        {
            var fui = Fetch<GameLobby>();
            fui.Init(go);
            return fui;
        }

        public static GameLobby CreateInstance()
        {
            return Create(CreateGObject());
        }

        public static async Task<GameLobby> CreateInstanceAsync()
        {
            return Create(await CreateGObjectAsync());
        }

        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static GameLobby GetFormPool(GObject go)
        {
            var fui = go.GetFObject<GameLobby>();
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
                BG = (GGraph)com.GetChildAt(0);
                UserName = (GTextField)com.GetChildAt(1);
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
            BG = null;
            UserName = null;

            base.Dispose();
        }
    }
}