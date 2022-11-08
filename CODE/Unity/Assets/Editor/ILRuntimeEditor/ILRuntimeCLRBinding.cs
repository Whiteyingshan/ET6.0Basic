using System.Collections.Generic;
using System.IO;
using ILRuntime.Runtime.CLRBinding;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public static class ILRuntimeCLRBinding
    {
        [MenuItem("Tools/ILRuntime/通过自动分析热更DLL生成CLR绑定")]
        private static void GenerateCLRBindingByAnalysis()
        {
            ListComponent<System.Type> delegateTypes = new ListComponent<System.Type>()
            {
                typeof(System.Func<int, bool>),
                typeof(System.Func<int, int, int>),
                typeof(System.Func<object, ETTask >),
                typeof(System.Func<List<int>, int>),
                typeof(System.Func<List<int>, bool>),
                typeof(System.Func<KeyValuePair<int, int>, bool>),
                typeof(System.Func<KeyValuePair<int, List<int>>, bool>),
                typeof(System.Func<KeyValuePair<string, int>, int>),
                typeof(System.Func<KeyValuePair<string, int>, string>),

                typeof(System.Comparison<KeyValuePair<int, int>>),
                typeof(System.Predicate<ILTypeInstance>),

                typeof(FairyGUI.EventCallback0),
                typeof(FairyGUI.EventCallback1),
                typeof(FairyGUI.ListItemRenderer),
                typeof(FairyGUI.UIPackage.CreateObjectCallback),
                typeof(FairyGUI.UIPackage.LoadResourceAsync),
            };

            //用新的分析热更dll调用引用来生成绑定代码
            using (FileStream fs = new FileStream(Path.Combine(Define.BuildOutputDir, "Code.dll"), FileMode.Open, FileAccess.Read))
            {
                AppDomain domain = new AppDomain();
                domain.LoadAssembly(fs);
                ILHelper.RegisterAdaptor(domain);
                BindingCodeGenerator.GenerateBindingCode(domain, "Assets/Mono/ILRuntime/Generate", delegateTypes: delegateTypes);
            }

            AssetDatabase.Refresh();
            Debug.Log("生成CLR绑定文件完成");
        }

        [MenuItem("Tools/ILRuntime/生成跨域继承适配器")]
        private static void GenerateCrossbindAdapter()
        {
            //由于跨域继承特殊性太多，自动生成无法实现完全无副作用生成，所以这里提供的代码自动生成主要是给大家生成个初始模版，简化大家的工作
            //大多数情况直接使用自动生成的模版即可，如果遇到问题可以手动去修改生成后的文件，因此这里需要大家自行处理是否覆盖的问题
            using (StreamWriter sw = new StreamWriter("Assets/Mono/ILRuntime/Adapter/IComparer_1_ILTypeInstanceAdapter.cs"))
            {
                sw.Write(CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode(typeof(System.Collections.Generic.IComparer<ILTypeInstance>), "ET"));
            }
            using (StreamWriter sw = new StreamWriter("Assets/Mono/ILRuntime/Adapter/ICriticalNotifyCompletionAdapter.cs"))
            {
                sw.Write(CrossBindingCodeGenerator.GenerateCrossBindingAdapterCode(typeof(System.Runtime.CompilerServices.ICriticalNotifyCompletion), "ET"));
            }

            AssetDatabase.Refresh();
            Debug.Log("生成适配器完成");
        }
    }
}