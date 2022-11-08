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

namespace ET
{
    /// <summary>
    /// 数值计算类型s开头为静态数据不存储、d开头为动态数据云存储
    /// </summary>
    public enum NumericCalcType
    {
        Error = 0,
        /// <summary>
        /// 普通类型，不存储，通过公式、动态数据、静态数据计算得出
        /// </summary>
        sCommon,
        /// <summary>
        /// 单一数值类型
        /// </summary>
        dSimple,
        /// <summary>
        /// 单一数值类型（不保存）
        /// </summary>
        cSinmple,
    }
    
    public static partial class NumericTypeEx
    {
        /// <summary>
        /// 获取数值名称
        /// </summary>
        /// <param name="numericId">数值Id</param>
        /// <returns></returns>
        public static string GetNumericName(int numericId)
        {
            string numericName = $"错误类型{numericId}";

            if (numericNameDict.ContainsKey(numericId))
            {
                numericName = numericNameDict[numericId].Value;
            }

            return numericName;
        }

        /// <summary>
        /// 获取数值计算类型
        /// </summary>
        /// <param name="numericId">数值Id</param>
        /// <returns></returns>
        public static NumericCalcType GetNumericTypeCalcType(long numericId)
        {
            NumericCalcType numericCalcType = NumericCalcType.Error;

            if (numericNameDict.ContainsKey(numericId))
            {
                numericCalcType = (NumericCalcType)numericNameDict[numericId].Key;
            }
            
            return numericCalcType;
        }
    }
}