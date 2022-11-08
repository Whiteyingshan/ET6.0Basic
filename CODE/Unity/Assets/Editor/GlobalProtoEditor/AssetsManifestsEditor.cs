using System.Collections.Generic;
using System.IO;
using ET;
using NUnit.Framework;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VEngine;

namespace Assets.Editor.GlobalProtoEditor
{
    public class AssetsManifestsEditor : EditorWindow
    {
        private const string PATH = @"Assets\Resources\Config\AssetsManifests.txt";

        private List<ManifestInfo> manifests;
        private ReorderableList reorderableList;

        [MenuItem("Tools/资源清单配置", false, 1)]
        public static void ShowWindow()
        {
            GetWindow<AssetsManifestsEditor>();
        }

        private void OnEnable()
        {
            manifests = File.Exists(PATH) ? JsonHelper.FromJson<List<ManifestInfo>>(File.ReadAllText(PATH)) : new List<ManifestInfo>();
            reorderableList = new ReorderableList(manifests, null);
            reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * 2 + 10;
            reorderableList.drawHeaderCallback = (Rect rect) =>
            {
                GUI.Label(rect, "清单");
            };
            reorderableList.drawElementCallback += (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                ManifestInfo manifestInfo = manifests[index];
                rect.y += 5;
                rect.height = EditorGUIUtility.singleLineHeight;
                manifestInfo.name = EditorGUI.TextField(rect, manifestInfo.name);
                rect.y += EditorGUIUtility.singleLineHeight;
                manifestInfo.autoUpdate = EditorGUI.ToggleLeft(rect, "自动更新", manifestInfo.autoUpdate);
            };
        }

        private void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 5, position.width - 20, position.height - 15));
            GUILayout.Space(5);

            reorderableList.DoLayoutList();

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("保存", GUILayout.Height(40)))
            {
                File.WriteAllText(PATH, JsonHelper.ToJson(manifests));
                AssetDatabase.Refresh();
            }
            GUILayout.EndArea();
        }
    }
}