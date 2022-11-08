namespace VEngine
{
    /// <summary>
    ///     版本初始化操作
    /// </summary>
    public sealed class InitializeVersions : LoadManifests
    {
        public ManifestInfo[] manifests;

        public override void Start()
        {
            foreach (var info in manifests)
            {
                assets.Add(ManifestFile.LoadAsync(info.name, true));
                if (info.autoUpdate)
                {
                    assets.Add(ManifestFile.LoadAsync(info.name));
                }
            }
            base.Start();
        }
    }
}