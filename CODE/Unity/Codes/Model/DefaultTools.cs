//----------------------------
//作者:XXX
//修订日期:XXX
//联系方式:XXX
//----------------------------
//修改者:XXX
//修改日期:XXX
//联系方式:XXX
//修改内容:XXX
//----------------------------

using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System;
using System.Text;

namespace ET
{
    public static class DefaultTools
    {
        public static int IndexOf<T>(this T[] self, T value) where T : IComparable<T>, IComparable
        {
            for (int i = 0; i < self.Length; i++)
            {
                if(self[i].CompareTo(value) == 0) return i;
            }

            return -1;
        }

        public static T Min<T>(params T[] values)
        {
            return values.Min();
        }

        public static T Max<T>(params T[] values)
        {
            return values.Max();
        }

        public static bool SwapAt<T>(this T[] self, int index1, int index2)
        {
            if (index1 == index2 || self.Length <= index1 || self.Length <= index2)
                return false;

            T temp = self[index1];
            self[index1] = self[index2];
            self[index2] = temp;

            return true;
        }

        public static bool SwapAt<T>(this List<T> self, int index1, int index2)
        {
            if (index1 == index2 || self.Count <= index1 || self.Count <= index2)
                return false;

            T temp = self[index1];
            self[index1] = self[index2];
            self[index2] = temp;

            return true;
        }

        public static T FitLast<T>(this T[] self, int index)
        {
            if (index >= self.Length)
                return self[self.Length - 1];
            else
                return self[index];
        }

        public static T FitFirst<T>(this T[] self, int index)
        {
            if (index >= self.Length)
                return self[0];
            else
                return self[index];
        }

        /// <summary>
        /// 转换为小时为最大单位的时间字符串
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ToTimeString_HH_MM_SS(this long self)
        {
            if (self <= 0)
                return "00:00:00";

            var h = self / 3600000;
            var m = (self % 3600000) / 60000;
            var s = (self % 60000) / 1000;
            // 秒数向上取整
            if (self % 1000 != 0)
                s += 1;
            // 秒数对齐
            if (s  == 60)
            {
                m += 1;
                s = 0;
            }
            // 分钟对齐
            if (m == 60)
            {
                h += 1;
                m = 0;
            }
                

            StringBuilder sb = new StringBuilder();

            if (h < 10)
                sb.Append($"0{h}");
            else
                sb.Append(h);

            sb.Append(':');

            if (m < 10)
                sb.Append($"0{m}");
            else
                sb.Append(m);

            sb.Append(':');

            if (s < 10)
                sb.Append($"0{s}");
            else
                sb.Append(s);

            return sb.ToString();
        }

        /// <summary>
        /// 数值转换成百分比值
        /// </summary>
        public static long NumericValue2Percentage(this long self)
        {
            if (self == 0)
                return 0;
            return self / 100;
        }

        /// <summary>
        /// 比较两个自然数数,取得小的那个 
        /// ***自然数的比较 非自然是会无视(小于0的数)
        /// </summary>
        public static int uCompareToMin(this int self, int value)
        {
            if (value < 0)
                return self;
            return self <= value ? self : value; 
        }

        /// <summary>
        /// 将比率转换为折扣
        /// 没有折扣时返回 empty
        /// * 默认为万分比
        /// </summary>
        public static string RatioToDiscount(int ratio)
        {
            if (ratio >= 10000)
                return string.Empty;

            float r = (ratio) / 1000f;

            return r.ToString(".0");
        }
        
        /// <summary>
        /// 数值单位转换
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string NumberUnitConversion(this long self)
        {
            if(self <= 1000)
            {
                return self.ToString();
            }
            else if(self > 1000 && self <= 1000000)
            {
                return $"{self / 1000}.{(self % 1000) / 10}K";
            }
            else
            {
                return $"{self / 1000000}.{(self % 1000000) / 1000}M";
            }
        }

        public static string NumericSign(this long self)
        {
            if (self >= 0)
                return "+";
            else
                return "-";
        }

        /// <summary>
        /// 整数比较
        /// </summary>
        public static bool IntegerCompare(long value1, long value2, string compareMode)
        {
            switch (compareMode)
            {
                case "=":
                case "==":
                    return value1 == value2;
                case ">":
                    return value1 > value2;
                case ">=":
                    return value1 >= value2;
                case "<":
                    return value1 < value2;
                case "<=":
                    return value1 <= value2;
                case "!=":
                    return value1 != value2;
                default:
                    throw new System.Exception($"未定义的比较模式:{compareMode}");
            }
        }

        /// <summary>
        /// 抽取随机索引
        /// </summary>
        /// <param name="total">容器长度</param>
        /// <param name="count">抽取数量</param>
        /// <returns></returns>
        public static int[] GetRandomSequence(int total, int count)
        {
            int[] sequence = new int[total];
            int[] output = new int[count];

            for (int i = 0; i < total; i++)
            {
                sequence[i] = i;
            }
            int end = total - 1;
            for (int i = 0; i < count; i++)
            {
                //随机一个数，每随机一次，随机区间-1
                int num = RandomHelper.RandomNumber(0, end + 1);
                output[i] = sequence[num];
                //将区间最后一个数赋值到取到的数上
                sequence[num] = sequence[end];
                end--;
            }
            return output;
        }

        /// <summary>
        /// 抽取随机索引
        /// </summary>
        /// <param name="total">容器长度</param>
        /// <param name="count">抽取数量</param>
        /// <param name="exclude">排除容器， 容器内数值必须小于total</param>
        /// <returns></returns>
        public static int[] GetRandomSequence(int total, int count, int[] exclude)
        {
            int[] sequence = new int[total - exclude.Length];

            int excludeSign = 0;
            for (int i = 0; i < total; i++)
            {
                if (exclude.Contains(i))
                {
                    excludeSign++;
                    continue;
                }

                sequence[i - excludeSign] = i;
            }

            if (count >= sequence.Length)
            {
                return sequence;
            }

            int[] output = new int[count];

            int end = sequence.Length - 1;

            for (int i = 0; i < count; i++)
            {
                //随机一个数，每随机一次，随机区间-1
                int num = RandomHelper.RandomNumber(0, end + 1);
                output[i] = sequence[num];
                //将区间最后一个数赋值到取到的数上
                sequence[num] = sequence[end];
                end--;
            }
            return output;
        }

        /// <summary>
        /// 随即生成数字与字母组合
        /// </summary>
        /// <returns></returns>
        public static string RandomPassword()
        {
            string chars = "";

            for (int i = 0; i < 10; i++)
            {
                char _char = ' ';
                switch (ET.RandomHelper.RandomNumber(0, 3))
                {
                    case 0: _char = (char)RandomHelper.RandomNumber(48, 58); break;
                    case 1: _char = (char)RandomHelper.RandomNumber(65, 91); break;
                    case 2: _char = (char)RandomHelper.RandomNumber(97, 123); break;
                }

                if (_char == ' ')
                {
                    Log.Debug("密码生成出现错误");
                }
               
                chars += _char;
            }

            return chars;
        }
        
        /// <summary>
        /// 获取本机MAC地址作为初始账户名
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            string physicalAddress = "";
            NetworkInterface[] nice = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adaper in nice)
            {
                if (adaper.Description == "en0")
                {
                    physicalAddress = adaper.GetPhysicalAddress().ToString();
                    break;
                }
                else
                {
                    physicalAddress = adaper.GetPhysicalAddress().ToString();
                    if (physicalAddress != "")
                    {
                        break;
                    };
                }
            }
            return physicalAddress;
        }

        /// <summary>
        /// 万分比转换
        /// </summary>
        /// <param name="value">万分比数值</param>
        /// <returns></returns>
        public static float TenThousandRatioConvert(long value)
        {
            return value / 10000f;
        }
    }
}