namespace VEngine
{
    /// <summary>
    ///     版本更新操作，更新操作默认只处理服务器的就行
    /// </summary>
    public sealed class UpdateVersions : LoadManifests
    {
        public string[] manifests;

        public override void Start()
        {
            base.Start();
            foreach (var manifest in manifests)
            {
                assets.Add(ManifestFile.LoadAsync(manifest));
            }
        }
    }
}