using System;
using System.Collections.Generic;
using ET.EventType;
using ET.Module.Numeric;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    namespace EventType
    {
        public struct NumbericChange
        {
            public Entity Parent;
            public NumericType NumericType;
            public long Old;
            public long New;
        }
    }

    public sealed class NumericComponent : Entity, ISerializeToEntity
    {
        [BsonIgnore]
        public Dictionary<long, long> Numeric = new Dictionary<long, long>();
        [BsonElement]
        public Dictionary<string, long> NumericDB = new Dictionary<string, long>();

        public long this[long key]
        {
            get => GetByKey(key);
            set => SetByKey(key, value);
        }

        public long this[NumericType nt]
        {
            get => this[(int)nt];
            set => this[(int)nt] = value;
        }

        public void Clear()
        {
            Numeric.Clear();
            NumericDB.Clear();
        }

        public int GetAsInt(int numericType)
        {
            return (int)GetByKey(numericType);
        }

        public long GetAsLong(int numericType)
        {
            return GetByKey(numericType);
        }

        public float GetAsFloat(int numericType)
        {
            return (float)GetByKey(numericType) / 10000;
        }

        public int GetAsInt(NumericType numericType)
        {
            return GetAsInt((int)numericType);
        }

        public long GetAsLong(NumericType numericType)
        {
            return GetAsLong((int)numericType);
        }

        public float GetAsFloat(NumericType numericType)
        {
            return GetAsFloat((int)numericType);
        }

        /// <summary>
		/// 当需要的属性为Ratio时获取Ration,通过属性ID
		/// </summary>
		/// <param name="numericType">属性ID</param>
		/// <returns></returns>
		public float GetAsRatio(int numericType)
        {
            return GetAsLong(numericType) / 10000f;
        }

        public void Set(NumericType nt, int value, int type = 0)
        {
            SetByKey((int)nt, value, type);
        }

        public void Set(NumericType nt, long value, int type = 0)
        {
            SetByKey((int)(nt), value, type);
        }

        public void Set(NumericType nt, float value, int type = 0)
        {
            SetByKey((int)nt, (long)(value * 10000), type);
        }

        private long GetByKey(long key, int type = 0)
        {
            long value;
            type = type != 0 ? type : NumericTypeConfigCategory.Instance.Get(key).Type;
            switch (type)
            {
                case 1:
                    NumericDB.TryGetValue(key.ToString(), out value);
                    return value;
                case 2:
                    Numeric.TryGetValue(key, out value);
                    return value;
                case 3:
                    INumericExpression ex = Game.Scene.GetComponent<NumericExpressionComponent>()[key];
                    return ex != null ? ex.Calculate(this) : 0;
                default:
                    throw new System.Exception($"未知的数值类型{type}, Id为{key}");
            }
        }

        private void SetByKey(long key, long value, int type = 0)
        {

            /*            long old = this[key];
                        type = type != 0 ? type : NumericTypeConfigCategory.Instance.Get(key).Type;
                        switch (type)
                        {
                            case 1:
                                this.NumericDB[key.ToString()] = value;
                                break;
                            default:
                                {*/
            if (key > (long)NumericType.Max && key < 10000000)
            {
                this.NumericDB[key.ToString()] = value;
                return;
            }

            long v = this.GetByKey(key);
            if (v == value)
            {
                return;
            }

            if (value < 0)
            {
                value = 0;
            }

            this.Update(key, value);
            /* }
             break;
     }

     if (value == this[key])
     {
         return;
     }

     Game.EventSystem.Publish(new NumbericChange() { Parent = this.Parent, NumericType = (NumericType)key, Old = old, New = value }).Coroutine();*/
        }
        /// <summary>
		/// 初始化属性值
		/// </summary>
		/// <param name="unitLevel">单位等级</param>
		/// <param name="baseValue">基础数值</param>
		/// <param name="growthValue">成长数值</param>
		/// <returns></returns>
		public long InitBaseValue(int unitLevel, long baseValue, int growthValue)
        {
            return (unitLevel - 1) * growthValue + baseValue;
        }
        /// <summary>
        /// 通过属性ID修改属性值 （当前值 + value）
        /// </summary>
        /// <param name="nt">属性ID</param>
        /// <param name="value">属性ID</param>
        public long SetBySelf(NumericType nt, long value)
        {
            return SetBySelf((long)nt, value);
        }
        public long SetBySelf(long nt, long value)
        {
            return this[nt] += value;
        }

        /// <summary>
		/// 更新数值
		/// 由于可以直接变更的为可计算Id，所以所传入Id必定大于最小属性Id的100倍值也就是Max
		/// 注意：Cur值不会被直接数值组件更新中直接操作
		/// </summary>
		/// <param name="numericType">传入Id</param>
		/// <param name="changeValue"></param>
		public async void Update(long numericType, long changeValue)
        {
            long pastValue = 0;

            if (numericType < (int)NumericType.Max)
            {
                if (!this.NumericDB.ContainsKey(numericType.ToString()))
                {
                    pastValue = 0;
                }
                else
                {
                    pastValue = this.NumericDB[numericType.ToString()];
                }
                this.NumericDB[numericType.ToString()] = changeValue;

                await Game.EventSystem.Publish(new EventType.NumericChangeEvent()
                {
                    changeObj = this.parent,
                    numericType = (NumericType)numericType,
                    finalId = numericType,
                    result = new long[2] { this.GetAsLong((int)numericType), pastValue }
                });
                return;
            }

            int final = (int)(numericType / 100);

            int bas = final * 100 + 1;
            int add = final * 100 + 2;
            int pct = final * 100 + 3;
            int finalAdd = final * 100 + 4;
            int finalPct = final * 100 + 5;

            this.NumericDB[numericType.ToString()] = changeValue;
            //long result = (long)((((this.GetAsValue(bas) + this.GetAsValue(add)) * (1 + this.GetAsRatio(pct))) * (1 + this.GetAsRatio(finalPct))) + this.GetAsValue(finalAdd));
            long result = (long)(((this.GetAsLong(bas) * (1 + this.GetAsRatio(pct))) + this.GetAsLong(add)) * (1 + this.GetAsRatio(finalPct)) + this.GetAsLong(finalAdd));

            pastValue = this.NumericDB[final.ToString()];
            this.NumericDB[final.ToString()] = result;

            await Game.EventSystem.Publish(new EventType.NumericChangeEvent()
            {
                changeObj = this.parent,
                numericType = (NumericType)numericType,
                finalId = final,
                result = new long[2] { result, pastValue }
            });
        }
    }
}