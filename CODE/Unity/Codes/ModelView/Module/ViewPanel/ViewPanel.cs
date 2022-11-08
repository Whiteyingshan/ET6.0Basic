using FairyGUI;

namespace ET
{
    public abstract class ViewPanel
    {
        public abstract string PanelName { get; }
        public virtual bool HideAll => true;
        public FObject FObject { get; set; }
        public Scene ZoneScene { get; set; }

        public long CallInterval = 233;
        /// <summary>
        /// 是否已经发送请求
        /// </summary>
        private bool isCall;
        public bool IsCall
        {
            get => this.isCall;
            protected set
            {
                isCall = value;
                /*if (value)
                {
                    LodingContrllerComponent.Instance.ShowAwaitNetworkDelay(ETModel.ComponentFactory.Create<ETCancellationTokenSource>()).Coroutine();
                }
                else
                {
                    LodingContrllerComponent.Instance.CloseLodingAwait();
                }*/

                /*if (value)
                {
                    TimerComponent.Instance.WaitAsync(CallInterval).GetAwaiter().OnCompleted(() =>
                    {
                        this.isCall = false;
                    });
                }
                else
                {
                    this.isCall = false;
                }*/

            }
        }

        public abstract void Init();

        public virtual async ETTask InitAsync()
        {
            await ETTask.CompletedTask;
        }

        public virtual void Show()
        {
            FObject.Visible = true;
            GComponent parent = FObject.GObject.parent;
            parent.SetChildIndex(FObject.GObject, parent.numChildren);
        }

        public virtual void Show(string args)
        {
            FObject.Visible = true;
            GComponent parent = FObject.GObject.parent;
            parent.SetChildIndex(FObject.GObject, parent.numChildren);
        }

        public virtual void Hide()
        {
            FObject.Visible = false;
        }

        public virtual void Close()
        {
            Hide();
        }

        public virtual void Dispose()
        {
            FObject?.Dispose();
            FObject = null;
        }
    }

    public abstract class ViewPanel<T> : ViewPanel where T : FObject
    {
        public T SelfUI => FObject as T;
        public override string PanelName => typeof(T).Name;
    }
}