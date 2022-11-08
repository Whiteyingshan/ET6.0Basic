using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using ILRuntime.Runtime.Intepreter;
using LitJson;
using ProtoBuf;
using UnityEngine;
using UnityEngine.Events;

namespace ET
{
    public static class ILHelper
    {
        public static List<Type> list = new List<Type>();

        public static void InitILRuntime(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
            //Generic Types
            #region Generic Types
            list.Add(typeof(Dictionary<int, int>));
            list.Add(typeof(Dictionary<int, long>));
            list.Add(typeof(Dictionary<int, object>));
            list.Add(typeof(Dictionary<int, ILTypeInstance>));
            list.Add(typeof(Dictionary<long, int>));
            list.Add(typeof(Dictionary<long, object>));
            list.Add(typeof(Dictionary<long, ILTypeInstance>));
            list.Add(typeof(Dictionary<string, int>));
            list.Add(typeof(Dictionary<string, long>));
            list.Add(typeof(Dictionary<string, object>));
            list.Add(typeof(Dictionary<string, ILTypeInstance>));
            list.Add(typeof(Dictionary<object, object>));
            list.Add(typeof(Dictionary<object, ILTypeInstance>));
            list.Add(typeof(Dictionary<ILTypeInstance, object>));
            list.Add(typeof(Dictionary<ILTypeInstance, ILTypeInstance>));
            list.Add(typeof(ETTask<int>));
            list.Add(typeof(ETTask<long>));
            list.Add(typeof(ETTask<string>));
            list.Add(typeof(ETTask<object>));
            list.Add(typeof(ETTask<AssetBundle>));
            list.Add(typeof(ETTask<UnityEngine.Object[]>));
            list.Add(typeof(ETTask<ILTypeInstance>));
            list.Add(typeof(List<int>));
            list.Add(typeof(List<long>));
            list.Add(typeof(List<string>));
            list.Add(typeof(List<object>));
            list.Add(typeof(List<ILTypeInstance>));
            list.Add(typeof(ListComponent<int>));
            list.Add(typeof(ListComponent<long>));
            list.Add(typeof(ListComponent<string>));
            list.Add(typeof(ListComponent<object>));
            list.Add(typeof(ListComponent<ETTask>));
            list.Add(typeof(ListComponent<Vector3>));
            list.Add(typeof(ListComponent<ILTypeInstance>));
            #endregion Generic Types

            // 注册委托
            #region Custom Delegate
            appdomain.DelegateManager.RegisterMethodDelegate<bool>();
            appdomain.DelegateManager.RegisterMethodDelegate<float>();
            appdomain.DelegateManager.RegisterMethodDelegate<string>();
            appdomain.DelegateManager.RegisterMethodDelegate<object>();
            appdomain.DelegateManager.RegisterMethodDelegate<List<object>>();
            appdomain.DelegateManager.RegisterMethodDelegate<long, int>();
            appdomain.DelegateManager.RegisterMethodDelegate<long, IPEndPoint>();
            appdomain.DelegateManager.RegisterMethodDelegate<long, MemoryStream>();
            appdomain.DelegateManager.RegisterMethodDelegate<AsyncOperation>();
            appdomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();

            appdomain.DelegateManager.RegisterFunctionDelegate<int, bool>();//Linq
            appdomain.DelegateManager.RegisterFunctionDelegate<int, int, int>();//Linq
            appdomain.DelegateManager.RegisterFunctionDelegate<object, ETTask>();
            appdomain.DelegateManager.RegisterFunctionDelegate<List<int>, int>();
            appdomain.DelegateManager.RegisterFunctionDelegate<List<int>, bool>();
            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<int, int>, bool>();
            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<int, List<int>>, bool>();
            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<string, int>, int>();
            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<string, int>, string>();

            appdomain.DelegateManager.RegisterDelegateConvertor<UnityAction>((act) =>
            {
                return new UnityAction(() => ((Action)act)());
            });

            appdomain.DelegateManager.RegisterFunctionDelegate<KeyValuePair<int, int>, KeyValuePair<int, int>, int>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Comparison<KeyValuePair<int, int>>>((act) =>
            {
                return new Comparison<KeyValuePair<int, int>>((x, y) => ((Func<KeyValuePair<int, int>, KeyValuePair<int, int>, int>)act)(x, y));
            });

            appdomain.DelegateManager.RegisterFunctionDelegate<ILTypeInstance, bool>();
            appdomain.DelegateManager.RegisterDelegateConvertor<Predicate<ILTypeInstance>>((act) =>
            {
                return new Predicate<ILTypeInstance>((obj) => ((Func<ILTypeInstance, bool>)act)(obj));
            });
            #endregion

            // FairyGUI Delegate
            #region FairyGUI Delegate
            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.EventCallback0>((act) =>
            {
                return new FairyGUI.EventCallback0(() => ((Action)act)());
            });

            appdomain.DelegateManager.RegisterMethodDelegate<FairyGUI.EventContext>();
            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.EventCallback1>((act) =>
            {
                return new FairyGUI.EventCallback1((context) => ((Action<FairyGUI.EventContext>)act)(context));
            });

            appdomain.DelegateManager.RegisterMethodDelegate<FairyGUI.GObject>();
            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.UIPackage.CreateObjectCallback>((act) =>
            {
                return new FairyGUI.UIPackage.CreateObjectCallback((result) => ((Action<FairyGUI.GObject>)act)(result));
            });

            appdomain.DelegateManager.RegisterMethodDelegate<int, FairyGUI.GObject>();
            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.ListItemRenderer>((act) =>
            {
                return new FairyGUI.ListItemRenderer((index, item) => ((Action<int, FairyGUI.GObject>)act)(index, item));
            });

            appdomain.DelegateManager.RegisterMethodDelegate<string, string, Type, FairyGUI.PackageItem>();
            appdomain.DelegateManager.RegisterDelegateConvertor<FairyGUI.UIPackage.LoadResourceAsync>((act) =>
            {
                return new FairyGUI.UIPackage.LoadResourceAsync((name, extension, type, item) => ((Action<string, string, Type, FairyGUI.PackageItem>)act)(name, extension, type, item));
            });
            #endregion FairyGUI Delegate

            // 注册适配器
            RegisterAdaptor(appdomain);
            // 注册ProtoBuf的CLR
            PType.RegisterILRuntimeCLRRedirection(appdomain);
            // 注册LitJson的CLR
            JsonMapper.RegisterILRuntimeCLRRedirection(appdomain);

            //////////////////////////////////////////////////////////////////
            //// CLR绑定的注册，一定要记得将CLR绑定的注册写在CLR重定向的注册后面，因为同一个方法只能被重定向一次，只有先注册的那个才能生效
            //////////////////////////////////////////////////////////////////
            ILRuntime.Runtime.CLRBinding.CLRBindingUtils.Initialize(appdomain);
        }

        public static void RegisterAdaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
        {
            // 注册自己写的适配器
            appdomain.RegisterCrossBindingAdaptor(new IAsyncStateMachineClassInheritanceAdaptor());
            appdomain.RegisterCrossBindingAdaptor(new IComparer_1_ILTypeInstanceAdapter());
            appdomain.RegisterCrossBindingAdaptor(new ICriticalNotifyCompletionAdapter());
            appdomain.RegisterCrossBindingAdaptor(new IDisposableAdapter());
        }
    }
}