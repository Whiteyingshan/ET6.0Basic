using ET._Component_Public;
using FairyGUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [ViewMethod(NoticeWindow.UIResName)]
    internal class NoticeWindowPanel : ViewPanel<NoticeWindow>
    {
        public static NoticeWindowPanel Instance { get; private set; }

        private Window window;
        private EventCallback0 eventCallback0;

        /// <summary>
        /// 通知窗口标题类型容器
        /// </summary>
        private readonly Dictionary<long, string> noticeTitleDict = new Dictionary<long, string>();
        /// <summary>
        /// 通知窗口普通消息类型容器
        /// </summary>
        private readonly Dictionary<long, string> noticeCommonMessageDict = new Dictionary<long, string>();
        /// <summary>
        /// 通知Tips Label容器
        /// </summary>
        public List<NoticeTipsLabel> NoticeTipsLabels = new List<NoticeTipsLabel>();

        public override bool HideAll => false;

        public override void Init()
        {
            Instance = this;
            window = new Window { contentPane = SelfUI.self, modal = true };
            window.Center();
            window.AddRelation(GRoot.inst, RelationType.Center_Center);

            IConfig[] noticeConfigs = NoticeConfigCategory.Instance.GetAll().Values.ToArray();

            foreach (var iConfig in noticeConfigs)
            {
                NoticeConfig noticeConfig = (NoticeConfig)iConfig;
                if (noticeConfig.MessageType == 0) continue;
                switch (noticeConfig.MessageType)
                {
                    case (int)NoticeContentType.Error:
                        Log.Error(new Exception($"未设置Notice类型{noticeConfig.Id}"));
                        return;
                    case (int)NoticeContentType.Title:
                        this.noticeTitleDict.Add(noticeConfig.Id, noticeConfig.MessageContent);
                        break;
                    case (int)NoticeContentType.CommonMessge:
                        this.noticeCommonMessageDict.Add(noticeConfig.Id, noticeConfig.MessageContent);
                        break;
                    default:
                        Log.Error(new Exception($"{noticeConfig.Id}错误Notice类型{noticeConfig.MessageType}"));
                        return;
                }
            }

            SelfUI.BG.onClick.Set(Hide);
        }

        public void ShowNotice(string content)
        {
            SelfUI.RichText_MessageContent.text = content;
            window.BringToFront();
            Show();
        }

        public void ShowNotice(string content, EventCallback0 eventCallback0)
        {
            eventCallback0 += eventCallback0;
            ShowNotice(content);
        }

        public ETTask ShowNoticeAsync(string content)
        {
            ETTask task = ETTask.Create();
            eventCallback0 += SetReslut;
            ShowNotice(content);
            return task;

            void SetReslut()
            {
                task.SetResult();
            }
        }

        public override void Show()
        {
            window.Show();
            window.BringToFront();
        }

        public override void Show(string args)
        {
            SelfUI.RichText_MessageContent.text = args;
            window.Show();
            window.BringToFront();
        }

        public override void Hide()
        {
            eventCallback0?.Invoke();
            eventCallback0 = null;
            window.Hide();
        }
        /// <summary>
        /// 通过MessageId 与文本模版显示 NoticeTips
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="values"></param>
        public void ShowNoticeTipsByMessageId(long messageId, Dictionary<string, string> values = null)
        {
            var fui = this.GetNoticeTipsLabel();

            if (messageId != default && this.noticeCommonMessageDict.ContainsKey(messageId))
            {
                fui.title.text = this.noticeCommonMessageDict[messageId];

                if (values != null)
                {
                    fui.title.templateVars = values;
                }

                fui.Animation_Show.Play();
            }
            else
            {
                Log.Error(new Exception("Notice系统MessageId不存在"));
                return;
            }
        }
        /// <summary>
        /// 获取NoticeTipsUI对象
        /// </summary>
        /// <returns></returns>
        private NoticeTipsLabel GetNoticeTipsLabel()
        {
            NoticeTipsLabel fui = this.NoticeTipsLabels.Find(label => !label.Visible);

            if (fui == null)
            {
                fui = NoticeTipsLabel.CreateInstance();
                fui.self.touchable = false;

                this.NoticeTipsLabels.Add(fui);
                GRoot.inst.AddChild(fui.GObject);
                fui.self.Center();
            }

            return fui;
        }
        /// <summary>
        /// 使用消息内容Id打开通知窗口
        /// </summary>
        /// <param name="messageId">消息内容Id</param>
        /// <param name="noticeWindowStyle">窗口样式</param>
        /// <exception cref="Exception"></exception>
        /// <returns></returns>
        public async ETTask ShowNoticeWindow(int messageId, NoticeWindowStyle noticeWindowStyle = new NoticeWindowStyle())
        {
            if (messageId != default && this.noticeCommonMessageDict.ContainsKey(messageId))
            {
                //通知标题
                if (noticeWindowStyle.TitileId != default && this.noticeTitleDict.ContainsKey(noticeWindowStyle.TitileId))
                {
                    SelfUI.title.text = this.noticeTitleDict[noticeWindowStyle.TitileId];
                }
                else
                {
                    SelfUI.title.text = this.noticeTitleDict[10000001];
                }

                //窗口样式
                if (noticeWindowStyle.WindowStyleUrl != default)
                {
                    SelfUI.self.icon = noticeWindowStyle.WindowStyleUrl;
                }
                else
                {
                    SelfUI.self.icon = null;
                }

                //文本模版
                if (noticeWindowStyle.values != null && noticeWindowStyle.values.Count > 0)
                {
                    SelfUI.RichText_MessageContent.templateVars = noticeWindowStyle.values;
                    SelfUI.RichText_MessageContent.FlushVars();
                }

                //按钮标题
                if (noticeWindowStyle.OkBtnTitleId != default && this.noticeTitleDict.ContainsKey(noticeWindowStyle.OkBtnTitleId))
                {
                    SelfUI.Btn_OK.title.text = this.noticeTitleDict[noticeWindowStyle.OkBtnTitleId];
                }
                else
                {
                    SelfUI.Btn_OK.title.text = this.noticeTitleDict[10000007];
                }

                //按钮样式
                SelfUI.self.onClick.Clear();

                SelfUI.BtnStyleController.selectedIndex = (int)noticeWindowStyle.NoticeWindowBtnStyleType;

                switch (noticeWindowStyle.NoticeWindowBtnStyleType)
                {
                    case NoticeWindowBtnStyleType.NoBtn:
                        SelfUI.BG.onClick.Set(() =>
                        {
                            noticeWindowStyle.OnOkCallBack();
                            Hide();
                        });
                        break;
                    case NoticeWindowBtnStyleType.BtnOk:
                    case NoticeWindowBtnStyleType.BtnOkCancel:
                        SelfUI.BG.onClick.Clear();

                        //确认按钮
                        SelfUI.Btn_OK.self.onClick.Set(() =>
                        {
                            noticeWindowStyle.OnOkCallBack();
                            Hide();
                        });

                        //取消按钮
                        SelfUI.Btn_Cancel.self.onClick.Set(() =>
                        {
                            Hide();
                        });
                        break;
                }

                //内容样式
                SelfUI.List_IconList.RemoveChildrenToPool();

                SelfUI.ContentStyleController.selectedIndex = (int)noticeWindowStyle.NoticeContentStyleType;

                switch (noticeWindowStyle.NoticeContentStyleType)
                {
                    case NoticeContentStyleType.Message:
                        break;
                    case NoticeContentStyleType.IconList:
                    case NoticeContentStyleType.MessageAndIconList:
                        SelfUI.List_IconList.RemoveChildrenToPool();
                        noticeWindowStyle.IconListInitEvent?.Invoke(SelfUI);
                        break;
                }

                //通知消息
                this.Show(this.noticeCommonMessageDict[messageId]);

                await ETTask.CompletedTask;
            }
            else
            {
                Log.Error(new Exception($"Notice系统MessageId:{messageId}不存在"));
                return;
            }
        }
        /// <summary>
        /// 显示NoticeTips
        /// </summary>
        /// <param name="message"></param>
        public void ShowNoticeTips(string message)
        {
            var fui = this.GetNoticeTipsLabel();

            fui.title.text = message;

            fui.Animation_Show.Play();
        }
    }
}