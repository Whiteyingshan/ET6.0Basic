using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using ET.DengLuZhuCe;
using ET.EventType;
using FairyGUI;
using UnityEngine;
using UnityEngine.Windows;
using LitJson;
using System.Net;
using System;
using System.Text.RegularExpressions;

namespace ET.Code.ViewPanel
{
    /// <summary>
    /// 登录状态类型
    /// </summary>
    public enum LoginStateType
    {
        /// <summary>
        /// 开始登陆
        /// </summary>
        StartLogin = 1,
        /// <summary>
        /// 登录成功
        /// </summary>
        LoginComplete = 2,
    }
    [ViewMethod(LoginForm.UIResName)]
    internal sealed class LoginFormInit : ViewPanel<LoginForm>
    {
        LoginAndRegisterComponent larComponent;
        public override void Init()
        {
            larComponent = SelfUI.Component_LoginAndRegister;

            //--------------------初始化按钮事件
            SelfUI.Btn_GameStart.self.onClick.Set(() =>
            {
                SelfUI.GameStartController.selectedIndex = (int)LoginStateType.StartLogin;
            });

            // 按回车登陆
            larComponent.self.onKeyDown.Set((context) =>
            {
                if (context.inputEvent.character == '\n')
                {
                    Login().Coroutine();
                }
            });

            
            // 登录按钮事件
            larComponent.Res_Btn_LoginComponent_LoginBtn.self.onClick.Add(() => Login().Coroutine());

            // 注册按钮事件
            larComponent.Res_Btn_LoginComponent_RegisterBtn.self.onClick.Add(() => Register().Coroutine());

            larComponent.Res_Text_LoginComponent_AccountTextFiled.text = PlayerPrefs.GetString("Username");
            larComponent.Res_Text_LoginComponent_PasswordTextFiled.text = PlayerPrefs.GetString("Password");
        }

        private async ETTask Login()
        {
            if (this.IsCall) return;
            try
            {
                this.IsCall = true;
                string username = larComponent.Res_Text_LoginComponent_AccountTextFiled.text;
                string password = larComponent.Res_Text_LoginComponent_PasswordTextFiled.text;

                if (string.IsNullOrEmpty(username))
                {
                    NoticeWindowPanel.Instance.ShowNotice("用户名为空!");
                    this.IsCall = false;
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    NoticeWindowPanel.Instance.ShowNotice("密码为空!");
                    this.IsCall = false;
                    return;
                }

                PlayerPrefs.SetString("Username", username);
                PlayerPrefs.SetString("Password", password);

                R2C_Login RealNameResponse = await LoginHelper.Login(ZoneScene, GloboProto.Inst.ServerAddress, username, password) as R2C_Login;
                if (RealNameResponse == null) 
                {
                    NoticeWindowPanel.Instance.ShowNotice("服务器正在维护中");
                    this.IsCall = false;
                    return;
                }
                switch (RealNameResponse.Error)
                {
                    case ErrorCode.ERR_Success:
                        break;
                    case ErrorCode.ERR_AccountNonExistent:
                        NoticeWindowPanel.Instance.ShowNotice("账号不存在!");
                        this.IsCall = false;
                        return;
                    case ErrorCode.ERR_PasswordError:
                        NoticeWindowPanel.Instance.ShowNotice("密码错误!");
                        this.IsCall = false;
                        return;
                    default:
                        NoticeWindowPanel.Instance.ShowNotice("登录账号服务器错误!");
                        this.IsCall = false;
                        return;
                }
                Game.Scene.GetComponent<LoginServerComponent>().Token = RealNameResponse.Message;

                IResponse response = await LoginHelper.GetServers(ZoneScene, GloboProto.Inst.ServerAddress);
                if (response.Error != ErrorCode.ERR_Success)
                {
                    NoticeWindowPanel.Instance.ShowNotice("获取服务器地址错误");
                    this.IsCall = false;
                    return;
                }

                string token = Game.Scene.GetComponent<LoginServerComponent>().Token;
                response = await LoginHelper.LoginGate(ZoneScene, 0, token);
                if (response.Error != 0)
                {
                    NoticeWindowPanel.Instance.ShowNotice("登陆错误!");
                    this.IsCall = false;
                    return;
                }

                if (AccountZone.Inst.Players.Count == 0)
                {
                    response = await LoginHelper.CreatePlayer(ZoneScene);
                    if (response.Error != 0)
                    {
                        Log.Error(response.Error.ToString());
                        NoticeWindowPanel.Instance.ShowNotice("创建角色错误!");
                        this.IsCall = false;
                        return;
                    }
                }
                response = await LoginHelper.LoginPlayer(ZoneScene, AccountZone.Inst.Players.First().Value.First());
                if (response.Error != 0)
                {
                    Log.Error(response.Error.ToString());
                    NoticeWindowPanel.Instance.ShowNotice("获取玩家信息错误!");
                    this.IsCall = false;
                    return;
                }
                AccountZone.Inst.Age = RealNameResponse.Age;
                SelfUI.GameStartController.selectedIndex = (int)LoginStateType.LoginComplete;
                await SwitchToLobby();
            }
            catch (Exception e)
            {
                this.IsCall = false;
                Log.Error(e);
                throw;
            }
        }

        private async ETTask Register()
        {
            if (this.IsCall) return;
            try
            {
                this.IsCall = true;
                string username = larComponent.Res_Text_LoginComponent_AccountTextFiled.text;
                string password = larComponent.Res_Text_LoginComponent_PasswordTextFiled.text;

                if (string.IsNullOrEmpty(username))
                {
                    NoticeWindowPanel.Instance.ShowNotice("用户名为空!");
                    this.IsCall = false;
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    NoticeWindowPanel.Instance.ShowNotice("密码为空!");
                    this.IsCall = false;
                    return;
                }

                IResponse response = await LoginHelper.Register(ZoneScene, GloboProto.Inst.ServerAddress, username, password);
                if (response.Error == ErrorCode.ERR_AccountAlreadyExistent)
                {
                    NoticeWindowPanel.Instance.ShowNotice("账号已存在!");
                    this.IsCall = false;
                    return;
                }
                else if (response.Error != ErrorCode.ERR_Success)
                {
                    NoticeWindowPanel.Instance.ShowNotice("注册失败!");
                    this.IsCall = false;
                    return;
                }

                NoticeWindowPanel.Instance.ShowNotice("注册成功!");
                PlayerPrefs.SetString("Username", username);
                PlayerPrefs.SetString("Password", password);

                larComponent.Res_Text_LoginComponent_AccountTextFiled.text = username;
                larComponent.Res_Text_LoginComponent_PasswordTextFiled.text = password;
                this.IsCall = false;
            }
            catch (Exception e)
            {
                this.IsCall = false;
                Log.Error(e);
                throw;
            }
        }

        private async ETTask SwitchToLobby()
        {
            await Game.EventSystem.Publish(new CreateLobbyPanel() { ZoneScene = ZoneScene });
            await Game.EventSystem.Publish(new RemoveLoginPanel() { });
        }
    }
}