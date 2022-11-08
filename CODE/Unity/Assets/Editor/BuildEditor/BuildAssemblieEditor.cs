using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace ET
{
    public static class BuildAssemblieEditor
    {
        private const string CodeDir = "Assets/Bundles/Code/";

        [MenuItem("Tools/ReloadLogic _F4")]
        public static void ReloadLogic()
        {
            CodeLoader.Instance?.Reload?.Invoke();
        }

        [MenuItem("Tools/BuildCodeDebug _F5")]
        public static void BuildCodeDebug()
        {
            BuildMuteAssembly("Code", new[]
            {
                "Codes/Model/",
                "Codes/ModelView/",
                "Codes/Hotfix/",
                "Codes/HotfixView/"
            }, Array.Empty<string>(), CodeOptimization.Debug);

            AfterCompiling();

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/BuildCodeRelease _F6")]
        public static void BuildCodeRelease()
        {
            BuildMuteAssembly("Code", new[]
            {
                "Codes/Model/",
                "Codes/ModelView/",
                "Codes/Hotfix/",
                "Codes/HotfixView/"
            }, Array.Empty<string>(), CodeOptimization.Release);

            AfterCompiling();

            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/BuildData _F7")]
        public static void BuildData()
        {
            BuildMuteAssembly("Data", new[]
            {
                "Codes/Model/",
                "Codes/ModelView/",
            }, Array.Empty<string>(), CodeOptimization.Debug);
        }


        [MenuItem("Tools/BuildLogic _F8")]
        public static void BuildLogic()
        {
            string[] logicFiles = Directory.GetFiles(Define.BuildOutputDir, "Logic_*");
            foreach (string file in logicFiles)
            {
                File.Delete(file);
            }

            int random = RandomHelper.RandomNumber(100000000, 999999999);
            string logicFile = $"Logic_{random}";

            BuildMuteAssembly(logicFile, new[]
            {
                "Codes/Hotfix/",
                "Codes/HotfixView/",
            }, new[] { Path.Combine(Define.BuildOutputDir, "Data.dll") }, CodeOptimization.Debug);
        }

        private static void BuildMuteAssembly(string assemblyName, string[] CodeDirectorys, string[] additionalReferences, CodeOptimization codeOptimization)
        {
            List<string> scripts = new List<string>();
            for (int i = 0; i < CodeDirectorys.Length; i++)
            {
                DirectoryInfo dti = new DirectoryInfo(CodeDirectorys[i]);
                FileInfo[] fileInfos = dti.GetFiles("*.cs", System.IO.SearchOption.AllDirectories);
                for (int j = 0; j < fileInfos.Length; j++)
                {
                    scripts.Add(fileInfos[j].FullName);
                }
            }

            string dllPath = Path.Combine(Define.BuildOutputDir, $"{assemblyName}.dll");
            string pdbPath = Path.Combine(Define.BuildOutputDir, $"{assemblyName}.pdb");
            File.Delete(dllPath);
            File.Delete(pdbPath);

            Directory.CreateDirectory(Define.BuildOutputDir);

            AssemblyBuilder assemblyBuilder = new AssemblyBuilder(dllPath, scripts.ToArray());
            BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            assemblyBuilder.additionalDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup).Split(';');
            assemblyBuilder.additionalReferences = additionalReferences;
            assemblyBuilder.buildTarget = EditorUserBuildSettings.activeBuildTarget;
            assemblyBuilder.buildTargetGroup = buildTargetGroup;
            //assemblyBuilder.compilerOptions.AllowUnsafeCode = true;
            assemblyBuilder.compilerOptions.CodeOptimization = codeOptimization;
            // assemblyBuilder.compilerOptions.ApiCompatibilityLevel = ApiCompatibilityLevel.NET_4_6;
            assemblyBuilder.compilerOptions.ApiCompatibilityLevel = PlayerSettings.GetApiCompatibilityLevel(buildTargetGroup);
            //AssemblyBuilderFlags.None                 正常发布
            //AssemblyBuilderFlags.DevelopmentBuild     开发模式打包
            //AssemblyBuilderFlags.EditorAssembly       编辑器状态
            assemblyBuilder.flags = AssemblyBuilderFlags.None;
            assemblyBuilder.referencesOptions = ReferencesOptions.UseEngineModules;

            assemblyBuilder.buildStarted += delegate (string assemblyPath)
            {
                Debug.LogFormat("build start：" + assemblyPath);
            };

            assemblyBuilder.buildFinished += delegate (string assemblyPath, CompilerMessage[] compilerMessages)
            {
                int errorCount = compilerMessages.Count(m => m.type == CompilerMessageType.Error);
                int warningCount = compilerMessages.Count(m => m.type == CompilerMessageType.Warning);

                Debug.LogFormat("Warnings: {0} - Errors: {1}", warningCount, errorCount);

                if (warningCount > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine($"有{warningCount}个Warning!!!");
                    for (int i = 0; i < compilerMessages.Length; i++)
                    {
                        if (compilerMessages[i].type == CompilerMessageType.Warning)
                        {
                            sb.AppendLine(compilerMessages[i].message);
                        }
                    }
                    Debug.Log(sb.ToString());
                }

                if (errorCount > 0)
                {
                    for (int i = 0; i < compilerMessages.Length; i++)
                    {
                        if (compilerMessages[i].type == CompilerMessageType.Error)
                        {
                            Debug.LogError(compilerMessages[i].message);
                        }
                    }
                }
            };

            //开始构建
            if (!assemblyBuilder.Build())
            {
                Debug.LogErrorFormat("build fail：" + assemblyBuilder.assemblyPath);
            }
        }

        private static void AfterCompiling()
        {
            Debug.Log("Compiling wait");
            while (EditorApplication.isCompiling)
            {
                // 主线程sleep并不影响编译线程
                Thread.Sleep(1);
            }
            Debug.Log("Compiling finish");

            Directory.CreateDirectory(CodeDir);
            File.Copy(Path.Combine(Define.BuildOutputDir, "Code.dll"), Path.Combine(CodeDir, "Code.dll.bytes"), true);
            File.Copy(Path.Combine(Define.BuildOutputDir, "Code.pdb"), Path.Combine(CodeDir, "Code.pdb.bytes"), true);
            AssetDatabase.Refresh();
            Debug.Log("copy Code.dll to Bundles/Code success!");
            Debug.Log("build success!");
        }
    }
}