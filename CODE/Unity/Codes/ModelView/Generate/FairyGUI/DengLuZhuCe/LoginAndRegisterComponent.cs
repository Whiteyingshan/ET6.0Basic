/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;

namespace ET.DengLuZhuCe
{
    public sealed partial class LoginAndRegisterComponent : FObject
    {
        public const string URL = "ui://fkxb3vu5d0e13m";
        public const string UIResName = "LoginAndRegisterComponent";
        public const string UIPackageName = "登陆注册";

        public override string ResName => UIResName;

        /// <summary>
        /// LoginAndRegisterComponent的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GComponent self { get; private set; }

        public GGraph BG;
        public GTextField PassWordTitle;
        public GTextField AccountTitle;
        public GImage Res_Img_LoginComponent_FiledBG1;
        public GTextInput Res_Text_LoginComponent_AccountTextFiled;
        public GImage Res_Img_LoginComponent_FiledBG2;
        public GTextInput Res_Text_LoginComponent_PasswordTextFiled;
        public GGroup LoginTextFiledGroup;
        public NormalButton Res_Btn_LoginComponent_RegisterBtn;
        public NormalButton Res_Btn_LoginComponent_LoginBtn;

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

        internal static LoginAndRegisterComponent Create(GObject go)
        {
            var fui = Fetch<LoginAndRegisterComponent>();
            fui.Init(go);
            return fui;
        }

        public static LoginAndRegisterComponent CreateInstance()
        {
            return Create(CreateGObject());
        }

        public static async Task<LoginAndRegisterComponent> CreateInstanceAsync()
        {
            return Create(await CreateGObjectAsync());
        }

        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static LoginAndRegisterComponent GetFormPool(GObject go)
        {
            var fui = go.GetFObject<LoginAndRegisterComponent>();
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
                PassWordTitle = (GTextField)com.GetChildAt(1);
                AccountTitle = (GTextField)com.GetChildAt(2);
                Res_Img_LoginComponent_FiledBG1 = (GImage)com.GetChildAt(3);
                Res_Text_LoginComponent_AccountTextFiled = (GTextInput)com.GetChildAt(4);
                Res_Img_LoginComponent_FiledBG2 = (GImage)com.GetChildAt(5);
                Res_Text_LoginComponent_PasswordTextFiled = (GTextInput)com.GetChildAt(6);
                LoginTextFiledGroup = (GGroup)com.GetChildAt(7);
                Res_Btn_LoginComponent_RegisterBtn = NormalButton.Create(com.GetChildAt(8));
                Res_Btn_LoginComponent_LoginBtn = NormalButton.Create(com.GetChildAt(9));
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
            PassWordTitle = null;
            AccountTitle = null;
            Res_Img_LoginComponent_FiledBG1 = null;
            Res_Text_LoginComponent_AccountTextFiled = null;
            Res_Img_LoginComponent_FiledBG2 = null;
            Res_Text_LoginComponent_PasswordTextFiled = null;
            LoginTextFiledGroup = null;
            Res_Btn_LoginComponent_RegisterBtn.Dispose();
            Res_Btn_LoginComponent_RegisterBtn = null;
            Res_Btn_LoginComponent_LoginBtn.Dispose();
            Res_Btn_LoginComponent_LoginBtn = null;

            base.Dispose();
        }
    }
}