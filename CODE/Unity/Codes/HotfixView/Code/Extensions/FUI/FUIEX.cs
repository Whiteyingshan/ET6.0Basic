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

using System.Text;
using FairyGUI;
using UnityEngine;

namespace ET.Extension
{
    public static class FUIEX
    {
        /// <summary>
        /// 修改数字文本
        /// </summary>
        /// <param name="self"></param>
        /// <param name="number"></param>
        /// <param name="isAutoUnitConversion"></param>
        public static void SetTextNumber(this GTextField self, long number, bool isAutoUnitConversion = false)
        {
            self.text = isAutoUnitConversion? NumberLanguageConvert.NumUnitConversionToString(number) : number.ToString();
        }
        
        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        public static void SetXY(this GObject self, GObject target)
        {
            Vector2 targetXY;
            if (self.parent == target.parent)
            {
                targetXY = target.xy;
            }
            else
            {
                targetXY = new Vector2(target.LocalToRoot(Vector2.zero, GRoot.inst).x, target.LocalToRoot(Vector2.zero, GRoot.inst).y);
            }
            
            self.SetXY(targetXY.x, targetXY.y);
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <param name="offset">偏移量</param>
        public static void SetXY(this GObject self, GObject target, Vector2 offset)
        {
            Vector2 targetXY;
            if (self.parent == target.parent)
            {
                targetXY = target.xy;
            }
            else
            {
                targetXY = new Vector2(target.LocalToRoot(Vector2.zero, GRoot.inst).x, target.LocalToRoot(Vector2.zero, GRoot.inst).y);
            }
            
            self.SetXY(targetXY.x + offset.x, targetXY.y + offset.y);
        }

        /// <summary>
        /// 获取自定义数据（string）
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string GetData2String(this GObject self)
        {
            return (string) self.data;
        }
    }
}