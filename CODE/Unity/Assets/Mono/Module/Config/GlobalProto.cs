using ET;
using UnityEngine;

public class GloboProto
{
    public static GloboProto Inst { get; private set; }

    public string ServerAddress;
    public string AssetsAddress;

    public static void Load()
    {
        string text = Resources.Load<TextAsset>("Config\\GlobalProto").text;
        GloboProto globoProto = JsonHelper.FromJson<GloboProto>(text);
        Inst = globoProto;
    }
}