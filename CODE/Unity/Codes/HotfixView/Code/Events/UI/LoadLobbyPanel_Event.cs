using ET._Component_Main;
using ET.EventType;
using FairyGUI;
using UnityEngine;

namespace ET.Code.Events
{
    internal sealed class LoadLobbyPanel_Event : AEvent<CreateLobbyPanel>
    {
        protected override async ETTask Run(CreateLobbyPanel args)
        {
            await UIPackageHelper.AddPackageAsync(FUIPackage._Component_Main);
            await UIPackageHelper.AddPackageAsync(FUIPackage._Rescources_Pics);
            await UIPackageHelper.AddPackageAsync(FUIPackage._Resources_Icons);
            await UIPackageHelper.AddPackageAsync(FUIPackage._Resources_Misc);
            await UIPackageHelper.AddPackageAsync(FUIPackage.ZiYuan1000Versions);
            

            await UIPackageHelper.AddPackageAsync(FUIPackage.ZhuJieMian);

            MainBottomComponent bottom = await MainBottomComponent.CreateInstanceAsync();
            FRoot.inst.Add(bottom);
            FRoot.inst.SetChildIndex(bottom, 0);
            bottom.MakeFullScreen();
            bottom.self.AddRelation(GRoot.inst, RelationType.Size);
            await ViewControllerComponent.Instance.InitChildren(bottom);
            MainMiddleComponent middle = await MainMiddleComponent.CreateInstanceAsync();
            FRoot.inst.Add(middle);
            FRoot.inst.SetChildIndex(middle, 1);
            middle.MakeFullScreen();
            middle.self.AddRelation(GRoot.inst, RelationType.Size);
            await ViewControllerComponent.Instance.InitChildren(middle);
            MainTopComponent top = await MainTopComponent.CreateInstanceAsync();
            FRoot.inst.Add(top);
            FRoot.inst.SetChildIndex(top, 2);
            top.MakeFullScreen();
            top.self.AddRelation(GRoot.inst, RelationType.Size);
            await ViewControllerComponent.Instance.InitChildren(top);

            // 播放背景音乐
            args.ZoneScene.GetComponent<AudioComponent>().Play("Main", AudioType.BGM, true, PlayerPrefs.GetFloat("MusicVolume", 50) / 100);
        }
    }
}