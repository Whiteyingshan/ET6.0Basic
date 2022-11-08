﻿using UnityEngine;

namespace VEngine
{
    /// <summary>
    ///     从本地加载的Bundle，默认使用 LoadFromFile 这个最高效的接口。
    /// </summary>
    internal class LocalBundle : Bundle
    {
        private AssetBundleCreateRequest request;

        protected override void OnLoad()
        {
            request = AssetBundle.LoadFromFileAsync(pathOrURL);
        }

        public override void LoadImmediate()
        {
            if (isDone)
            {
                return;
            }

            assetBundle = request.assetBundle;
            if (assetBundle == null)
            {
                Finish("assetBundle == null");
                return;
            }

            Finish();
            request = null;
        }

        private void OnLoaded()
        {
            if (request == null)
            {
                Finish("request == null");
                return;
            }

            assetBundle = request.assetBundle;
            request = null;
            if (assetBundle == null)
            {
                Finish("assetBundle == null");
                return;
            }

            Finish();
        }

        protected override void OnUpdate()
        {
            if (status != LoadableStatus.Loading)
            {
                return;
            }

            if (request == null)
            {
                OnLoad();
                return;
            }

            progress = request.progress;
            if (request.isDone)
            {
                OnLoaded();
            }
        }
    }
}