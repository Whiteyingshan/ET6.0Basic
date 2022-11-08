using System;
using System.Collections.Generic;
using FairyGUI;

namespace ET
{
    public class FObject
    {
        public GObject GObject { get; set; }

        public bool IsDisposed { get; protected set; }

        public virtual string ResName { get; }

        private bool isFromPool = false;

        /// <summary>
        /// FUI对象池
        /// </summary>
        private static readonly Dictionary<Type, Queue<FObject>> FObjectPool = new Dictionary<Type, Queue<FObject>>();

        protected bool IsFromPool
        {
            get
            {
                return isFromPool;
            }
            set
            {
                isFromPool = value;

                if (GObject is GComponent gComponent)
                {
                    GObject[] gObjects = gComponent.GetChildren();

                    foreach (GObject gObject in gObjects)
                    {
                        FObject fObject = gObject.GetFObject();

                        if (fObject != null)
                        {
                            fObject.IsFromPool = value;
                        }
                    }
                }
            }
        }

        public string Name
        {
            get
            {
                if (GObject == null)
                {
                    return string.Empty;
                }

                return GObject.name;
            }
            set
            {
                if (GObject == null)
                {
                    return;
                }

                GObject.name = value;
            }
        }

        public bool Visible
        {
            get
            {
                if (GObject == null)
                {
                    return false;
                }

                return GObject.visible;
            }
            set
            {
                if (GObject == null)
                {
                    return;
                }

                GObject.visible = value;
            }
        }

        public bool IsWindow
        {
            get
            {
                return GObject is Window;
            }
        }

        public bool IsComponent
        {
            get
            {
                return GObject is GComponent;
            }
        }

        public bool IsRoot
        {
            get
            {
                return GObject is GRoot;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return GObject == null;
            }
        }

        public FObject Parent
        {
            get
            {
                return GObject?.parent?.GetFObject();
            }
        }

        protected static T Fetch<T>() where T : FObject
        {
            if (FObjectPool.TryGetValue(typeof(T), out Queue<FObject> queue))
            {
                if (queue.Count > 0)
                {
                    return queue.Dequeue() as T;
                }
            }

            return Activator.CreateInstance(typeof(T)) as T;
        }

        protected static void Recycle<T>(T fui) where T : FObject
        {
            if (!FObjectPool.ContainsKey(typeof(T)))
            {
                FObjectPool.Add(typeof(T), new Queue<FObject>());
            }

            FObjectPool[typeof(T)].Enqueue(fui);
        }


        public void MakeFullScreen()
        {
            GObject?.asCom?.MakeFullScreen();
        }

        public void Add(FObject ui, bool asChildGObject = true)
        {
            if (ui == null || ui.IsEmpty)
            {
                throw new Exception($"ui can not be empty");
            }

            if (string.IsNullOrWhiteSpace(ui.Name))
            {
                throw new Exception($"ui.Name can not be empty");
            }

            if (IsComponent && asChildGObject)
            {
                GObject.asCom.AddChild(ui.GObject);
            }
        }

        public void Remove(string name)
        {
            if (IsDisposed || !IsComponent)
            {
                return;
            }

            FObject ui = GObject.asCom.GetChild(name).GetFObject<FObject>();

            if (ui != null)
            {
                GObject.asCom.RemoveChild(ui.GObject, false);
                ui.Dispose();
            }
        }

        /// <summary>
        /// 一般情况不要使用此方法，如需使用，需要自行管理返回值的FUI的释放。
        /// </summary>
        public FObject RemoveNoDispose(string name)
        {
            if (IsDisposed || !IsComponent)
            {
                return null;
            }

            FObject ui = GObject.asCom.GetChild(name).GetFObject<FObject>();

            if (ui != null)
            {
                if (IsComponent)
                {
                    GObject.asCom.RemoveChild(ui.GObject, false);
                }
            }

            return ui;
        }

        public void RemoveChildren()
        {
            if (IsDisposed || !IsComponent)
            {
                return;
            }

            foreach (var item in GObject.asCom.GetChildren())
            {
                item.GetFObject<FObject>().Dispose();
            }
        }

        public FObject Get(string name)
        {
            if (IsDisposed || !IsComponent)
            {
                return null;
            }

            return GObject?.asCom?.GetChild(name)?.GetFObject<FObject>();
        }

        public FObject[] GetAll()
        {
            if (IsDisposed || !IsComponent)
            {
                return Array.Empty<FObject>();
            }

            List<FObject> list = new List<FObject>(GObject.asCom.numChildren);
            foreach (GObject item in GObject.asCom.GetChildren())
            {
                FObject fui = item.GetFObject();

                if (fui != null)
                {
                    list.Add(fui);
                }
            }
            return list.ToArray();
        }

        protected virtual void AfterDispose()
        {
        }

        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            // 删除自己的UI
            if (!isFromPool)
            {
                GObject.Dispose();
            }

            GObject = null;
            IsDisposed = true;
            isFromPool = false;

            AfterDispose();
            Recycle(this);
        }
    }
}