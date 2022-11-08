using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class TurnBasedSkillCategoryAttribute : BaseAttribute
    {
        /// <summary>
        /// 种类
        /// </summary>
        public int categoryType { get; }
        /// <summary>
        /// 效果类型
        /// </summary>
        public int effectType { get; }
        public TurnBasedSkillCategoryAttribute(int baseType, int effectType) : base()
        {
            this.categoryType = baseType;
            this.effectType = effectType;
        }
    }

    public class UnitFilterCategoryAttribute : BaseAttribute
    {
        public int FilterType { get; }
        public UnitFilterCategoryAttribute(int type)
        {
            this.FilterType = type;
        }
    }
}
