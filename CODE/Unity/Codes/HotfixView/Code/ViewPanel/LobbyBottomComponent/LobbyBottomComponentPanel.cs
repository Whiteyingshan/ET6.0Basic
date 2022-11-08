using System;
using System.Collections.Generic;
using ET.Code.ViewPanel;
using ET.EventType;
using ET.ZhuJieMian;
using FairyGUI;

namespace ET
{
    [ViewMethod(GameLobby.UIResName)]
    public class LobbyBottomComponentPanel : ViewPanel<GameLobby>
    {
        public override void Init()
        {
            //昵称
            this.SelfUI.UserName.SetVar("name", Player.Inst.BasicData.Nickname).FlushVars();
        }
        public override void Show()
        {
            base.Show();
        }
    }
}