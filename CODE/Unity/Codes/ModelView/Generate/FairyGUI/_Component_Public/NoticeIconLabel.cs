/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using System.Threading.Tasks;
using FairyGUI;

namespace ET._Component_Public
{
    public sealed partial class NoticeIconLabel : FObject
    {
        public const string URL = "ui://5ardt36yang3d7";
        public const string UIResName = "NoticeIconLabel";
        public const string UIPackageName = "_Component_Public";

        public override string ResName => UIResName;

        /// <summary>
        /// NoticeIconLabel的组件类型(GComponent、GButton、GProcessBar等)，它们都是GObject的子类。
        /// </summary>
        public GLabel self { get; private set; }

        public GImage n6;
        public GLoader icon;
        public GLoader Loder_IconBorder;
        public GLoader Loader_PhaseGrade;
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

        internal static NoticeIconLabel Create(GObject go)
        {
            var fui = Fetch<NoticeIconLabel>();
            fui.Init(go);
            return fui;
        }

        public static NoticeIconLabel CreateInstance()
        {
            return Create(CreateGObject());
        }

        public static async Task<NoticeIconLabel> CreateInstanceAsync()
        {
            return Create(await CreateGObjectAsync());
        }

        /// <summary>
        /// 通过此方法获取的FUI，在Dispose时不会释放GObject，需要自行管理（一般在配合FGUI的Pool机制时使用）。
        /// </summary>
        public static NoticeIconLabel GetFormPool(GObject go)
        {
            var fui = go.GetFObject<NoticeIconLabel>();
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
                n6 = (GImage)com.GetChildAt(0);
                icon = (GLoader)com.GetChildAt(1);
                Loder_IconBorder = (GLoader)com.GetChildAt(2);
                Loader_PhaseGrade = (GLoader)com.GetChildAt(3);
                title = (GTextField)com.GetChildAt(4);
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
            n6 = null;
            icon = null;
            Loder_IconBorder = null;
            Loader_PhaseGrade = null;
            title = null;

            base.Dispose();
        }
    }
}