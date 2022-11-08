//----------------------------
//作者:XXX
//修订日期:XXX
//联系方式:XXX
//----------------------------
//修改者:XXX
//修改日期:XXX
//联系方式:XXX
//修改内容:XXX
//----------------------------

using System;
using System.Collections.Generic;
using ET._Component_Public;

namespace ET
{
    /// <summary>
    /// 通知内容类型
    /// </summary>
    public enum NoticeContentType
    {
        Error    =    0,
        /// <summary>
        /// 消息标题
        /// </summary>
        Title,
        /// <summary>
        /// 普通消息内容
        /// </summary>
        CommonMessge,
    }

    /// <summary>
    /// 通知窗口Btn样式类型
    /// </summary>
    public enum NoticeWindowBtnStyleType
    {
        /// <summary>
        /// 无按钮类型点击空位置关闭
        /// </summary>
        NoBtn = 0,
        /// <summary>
        /// 单个确认按钮类型
        /// </summary>
        BtnOk,
        /// <summary>
        /// 确认与取消按钮类型
        /// </summary>
        BtnOkCancel,
    }

    /// <summary>
    /// 通知窗口内容样式类型
    /// </summary>
    public enum NoticeContentStyleType
    {
        /// <summary>
        /// 单消息类型
        /// </summary>
        Message = 0,
        /// <summary>
        /// 单图标List类型
        /// </summary>
        IconList,
        /// <summary>
        /// 消息与图标类型(消息在图标上面)
        /// </summary>
        MessageAndIconList,
    }

    /// <summary>
    /// 通知窗口样式
    /// </summary>
    public struct NoticeWindowStyle
    {
        /// <summary>
        /// 窗口标题Id
        /// </summary>
        public int TitileId;
        /// <summary>
        /// 窗口样式Url
        /// </summary>
        public string WindowStyleUrl;
        /// <summary>
        /// 按钮样式
        /// </summary>
        public NoticeWindowBtnStyleType NoticeWindowBtnStyleType;
        /// <summary>
        /// 内容样式
        /// </summary>
        public NoticeContentStyleType NoticeContentStyleType;
        /// <summary>
        /// 确认按钮标题Id
        /// </summary>
        public int OkBtnTitleId;
        /// <summary>
        /// 点击确认按钮的回调
        /// 无按钮时为mask点击回调
        /// </summary>
        private List<Func<ETTask>> OkCallBack;
        // /// <summary>
        // /// 展示的英雄与点击回调容器
        // /// </summary>
        // public Dictionary<Hero,Func<ETTask>> HeroClickCallBackDict;
        // /// <summary>
        // /// 展示的装备与点击回调容器
        // /// </summary>
        // public Dictionary<Equip,Func<ETTask>> EquipClickCallOackDict;
        /// <summary>
        /// 文本模版容器
        /// </summary>
        public Dictionary<string, string> values;
        /// <summary>
        /// IconList初始化事件 *初始化请使用pool操作List
        /// </summary>
        public Action<NoticeWindow> IconListInitEvent;
        /// <summary>
        /// 添加Ok按钮回调事件
        /// </summary>
        /// <param name="callBack"></param>
        public void AddOkCallBack(Func<ETTask> callBack)
        {
            if (this.OkCallBack == null)
            {
                this.OkCallBack = new List<Func<ETTask>>();
            }
        
            this.OkCallBack.Add(callBack);
        }
        
        /// <summary>
        /// ok按钮回调
        /// </summary>
        public void OnOkCallBack()
        {
            this.OkCallBack?.ForEach(callBack=>{callBack.Invoke().Coroutine();});
            this.OkCallBack?.Clear();
            this.OkCallBack = null;
        }
    }
}