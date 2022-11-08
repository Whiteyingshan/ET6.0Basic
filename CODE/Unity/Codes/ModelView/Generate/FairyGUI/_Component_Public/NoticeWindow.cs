/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;

namespace ET._Component_Public
{
    public sealed partial class NoticeWindow : FObject
    {
        public const string URL = "ui://5ardt36yang3d5";
        public const string UIResName = "NoticeWindow";
        public const string UIPackageName = "_Component_Public";

        public override string ResName => UIResName;

        /// <summary>
        /// NoticeWindow的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GLabel self { get; private set; }

        public Controller BtnStyleController;
        public Controller ContentStyleController;
        public GGraph BG;
        public GGraph icon;
        public GTextField title;
        public GGraph n23;
        public GRichTextField RichText_MessageContent;
        public GList List_IconList;
        public NoticeBtn Btn_OK;
        public NoticeBtn Btn_Cancel;

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

        internal static NoticeWindow Create(GObject go)
        {
            var fui = Fetch<NoticeWindow>();
            fui.Init(go);
            return fui;
        }

        public static NoticeWindow CreateInstance()
        {
            return Create(CreateGObject());
        }

        public static async Task<NoticeWindow> CreateInstanceAsync()
        {
            return Create(await CreateGObjectAsync());
        }

        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static NoticeWindow GetFormPool(GObject go)
        {
            var fui = go.GetFObject<NoticeWindow>();
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
                BtnStyleController = com.GetControllerAt(0);
                ContentStyleController = com.GetControllerAt(1);
                BG = (GGraph)com.GetChildAt(0);
                icon = (GGraph)com.GetChildAt(1);
                title = (GTextField)com.GetChildAt(2);
                n23 = (GGraph)com.GetChildAt(3);
                RichText_MessageContent = (GRichTextField)com.GetChildAt(4);
                List_IconList = (GList)com.GetChildAt(5);
                Btn_OK = NoticeBtn.Create(com.GetChildAt(6));
                Btn_Cancel = NoticeBtn.Create(com.GetChildAt(7));
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
            BtnStyleController = null;
            ContentStyleController = null;
            BG = null;
            icon = null;
            title = null;
            n23 = null;
            RichText_MessageContent = null;
            List_IconList = null;
            Btn_OK.Dispose();
            Btn_OK = null;
            Btn_Cancel.Dispose();
            Btn_Cancel = null;

            base.Dispose();
        }
    }
}