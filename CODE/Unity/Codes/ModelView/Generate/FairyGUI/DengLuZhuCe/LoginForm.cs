/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;

namespace ET.DengLuZhuCe
{
    public sealed partial class LoginForm : FObject
    {
        public const string URL = "ui://fkxb3vu5s1qb7";
        public const string UIResName = "LoginForm";
        public const string UIPackageName = "登陆注册";

        public override string ResName => UIResName;

        /// <summary>
        /// LoginForm的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self { get; private set; }

        public Controller GameStartController;
        public GGraph BG;
        public LoginAndRegisterComponent Component_LoginAndRegister;
        public GImage Res_Img_GameStart_StartBtnBG;
        public GImage Res_Img_GameStart_StartBtnText;
        public LoginBtn Btn_GameStart;
        public GGroup StartBtnGroup;
        public GTextField Text_LoadingTips;

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

        internal static LoginForm Create(GObject go)
        {
            var fui = Fetch<LoginForm>();
            fui.Init(go);
            return fui;
        }

        public static LoginForm CreateInstance()
        {
            return Create(CreateGObject());
        }

        public static async Task<LoginForm> CreateInstanceAsync()
        {
            return Create(await CreateGObjectAsync());
        }

        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static LoginForm GetFormPool(GObject go)
        {
            var fui = go.GetFObject<LoginForm>();
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
                GameStartController = com.GetControllerAt(0);
                BG = (GGraph)com.GetChildAt(0);
                Component_LoginAndRegister = LoginAndRegisterComponent.Create(com.GetChildAt(1));
                Res_Img_GameStart_StartBtnBG = (GImage)com.GetChildAt(2);
                Res_Img_GameStart_StartBtnText = (GImage)com.GetChildAt(3);
                Btn_GameStart = LoginBtn.Create(com.GetChildAt(4));
                StartBtnGroup = (GGroup)com.GetChildAt(5);
                Text_LoadingTips = (GTextField)com.GetChildAt(6);
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
            GameStartController = null;
            BG = null;
            Component_LoginAndRegister.Dispose();
            Component_LoginAndRegister = null;
            Res_Img_GameStart_StartBtnBG = null;
            Res_Img_GameStart_StartBtnText = null;
            Btn_GameStart.Dispose();
            Btn_GameStart = null;
            StartBtnGroup = null;
            Text_LoadingTips = null;

            base.Dispose();
        }
    }
}