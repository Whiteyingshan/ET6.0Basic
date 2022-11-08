using System.Collections.Generic;

namespace VEngine
{
    public class LoadManifests : Operation
    {
        protected readonly List<ManifestFile> assets = new List<ManifestFile>();

        protected override void Update()
        {
            base.Update();
            switch (status)
            {
                case OperationStatus.Processing:
                    foreach (var asset in assets)
                    {
                        if (!asset.isDone)
                        {
                            return;
                        }
                    }
                    var errors = new List<string>();
                    foreach (var asset in assets)
                    {
                        if (asset.status != LoadableStatus.FailedToLoad)
                        {
                            asset.Release();
                        }
                        else
                        {
                            errors.Add(asset.error);
                        }
                    }
                    assets.Clear();
                    Finish(errors.Count == 0 ? null : string.Join("\n", errors.ToArray()));
                    break;
            }
        }
    }
}