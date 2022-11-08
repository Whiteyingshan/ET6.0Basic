/** This is an automatically generated class by FairyGUI. Please do not modify it. **/

using FairyGUI;
using FairyGUI.Utils;

namespace ReGengJianCe
{
    public partial class SplashUI : GComponent
    {
        public Controller Panel;
        public GGraph BG;
        public GTextField Res_Text_Download_CheckFileTip;
        public GGroup Res_Group_CheckFile;
        public GTextField Res_Text_Download_DownloadFileTip;
        public GTextField Text_Downloaded;
        public GTextField Text_TotalSize;
        public GProgressBar ProgressBar_LodingBar;
        public GGroup Res_Group_DownloadFile;
        public GTextField Res_Text_Download_DownloadFinishTip;
        public GTextField Text_LoadingTips;
        public GGroup Res_Group_DownloadFinish;
        public const string URL = "ui://cwjgmjz0i96m8";

        public static SplashUI CreateInstance()
        {
            return (SplashUI)UIPackage.CreateObject("热更检测", "SplashUI");
        }

        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);

            Panel = GetControllerAt(0);
            BG = (GGraph)GetChildAt(0);
            Res_Text_Download_CheckFileTip = (GTextField)GetChildAt(1);
            Res_Group_CheckFile = (GGroup)GetChildAt(2);
            Res_Text_Download_DownloadFileTip = (GTextField)GetChildAt(3);
            Text_Downloaded = (GTextField)GetChildAt(4);
            Text_TotalSize = (GTextField)GetChildAt(6);
            ProgressBar_LodingBar = (GProgressBar)GetChildAt(7);
            Res_Group_DownloadFile = (GGroup)GetChildAt(8);
            Res_Text_Download_DownloadFinishTip = (GTextField)GetChildAt(9);
            Text_LoadingTips = (GTextField)GetChildAt(10);
            Res_Group_DownloadFinish = (GGroup)GetChildAt(11);
        }
    }
}