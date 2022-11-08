using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using ReGengJianCe;
using UnityEngine;
using VEngine;

namespace ET
{
    public class AssetsLoader
    {
        private string pkg;
        private List<ManifestInfo> manifests;
        private SplashUI splashUI { get; set; }
        public static AssetsLoader Instance { get; } = new AssetsLoader();

        public async ETTask Update()
        {
            GloboProto.Load();
            ReGengJianCeBinder.BindAll();

            pkg = UIPackage.AddPackage("FairyUI/热更检测/热更检测").id;
            string text = Resources.Load<TextAsset>(@"Config\AssetsManifests").text;
            manifests = JsonHelper.FromJson<List<ManifestInfo>>(text);

            splashUI = SplashUI.CreateInstance();
            GRoot.inst.SetContentScaleFactor(720, 1280);
            GRoot.inst.AddChild(splashUI);
            splashUI.MakeFullScreen();
            splashUI.AddRelation(GRoot.inst, RelationType.Size);

            ETTask task = ETTask.Create(true);
            Timers.inst.StartCoroutine(UpdateAsync(task));
            await task;
        }
        IEnumerator UpdateAsync(ETTask task)
        {
            splashUI.Panel.selectedIndex = 0;

            while (true)
            {
                Versions.DownloadURL = GloboProto.Inst.AssetsAddress;
                InitializeVersions initializeVersions = Versions.InitializeAsync(manifests.ToArray());
                yield return initializeVersions;
                if (initializeVersions.status != OperationStatus.Success)
                {
                    continue;
                }
                UpdateVersions updateVersions = Versions.UpdateAsync();
                yield return updateVersions;
                if (updateVersions.status != OperationStatus.Success)
                {
                    continue;
                }
                GetDownloadSize dSize = Versions.GetDownloadSizeAsync();
                yield return dSize;
                if (dSize.status != OperationStatus.Success)
                {
                    continue;
                }

                if (dSize.result.Count > 0)
                {
                    DownloadVersions dVersions = Versions.DownloadAsync(dSize.result.ToArray());
                    splashUI.ProgressBar_LodingBar.max = 1;
                    splashUI.Panel.selectedIndex = 1;
                    splashUI.Text_TotalSize.text = Utility.FormatBytes(dVersions.totalSize);
                    while (!dVersions.isDone)
                    {
                        splashUI.ProgressBar_LodingBar.value = dVersions.progress;
                        splashUI.Text_Downloaded.text = Utility.FormatBytes(dVersions.downloadedBytes);
                        yield return null;
                    }
                }
                break;
            }

            splashUI.Panel.selectedIndex = 2;
            splashUI.Dispose();
            UIPackage.RemovePackage(pkg);
            UIObjectFactory.Clear();
            task.SetResult();
        }
    }
}