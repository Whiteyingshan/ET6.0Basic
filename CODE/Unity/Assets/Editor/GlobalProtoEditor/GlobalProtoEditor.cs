using System.Collections.Generic;
using System.IO;
using ET;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor.GlobalProtoEditor
{
    internal class ServerSetting : ScriptableObject
    {
        List<GloboProto> Protos = new List<GloboProto>();
    }

    public class GlobalProtoEditor : EditorWindow
    {
        private const string PATH = @"Assets\Resources\Config\GlobalProto.txt";
        private const string APATH = @"ProjectSettings\ServerSetting.asset";

        private GloboProto globoProto;

        [MenuItem("Tools/全局配置", false, 0)]
        public static void ShowWindow()
        {
            GetWindow<GlobalProtoEditor>();
        }

        private void OnEnable()
        {
            globoProto = File.Exists(PATH) ? JsonHelper.FromJson<GloboProto>(File.ReadAllText(PATH)) : new GloboProto();
            if (globoProto is null)
            {
                globoProto = new GloboProto();
            }
        }

        private void OnGUI()
        {
            globoProto.ServerAddress = EditorGUILayout.TextField("服务器地址:", globoProto.ServerAddress);
            globoProto.AssetsAddress = EditorGUILayout.TextField("热更资源地址:", globoProto.AssetsAddress);

            if (GUILayout.Button("保存", GUILayout.Height(30)))
            {
                File.WriteAllText(PATH, JsonHelper.ToJson(globoProto));
            }
        }
    }
}