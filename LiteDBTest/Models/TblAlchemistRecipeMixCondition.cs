using LiteDBTest.Tools;using LiteDBTest.Interfaces;

namespace LiteDBTest.Alchemist.Old
{
    public class TblAlchemistRecipeMixCondition : ITextProcessing
    {
        //"clientiteminfo\\tblalchemist_recipe_mix_condition"
        //fdNum,fdRecipeNum,fdClass,fdProbability,fdAttrType,fdAttrValueMin,fdAttrValueMax,fdAttrClass

        public ushort fdNum { get; set; }//条目序号
        public ushort fdRecipeNum { get; set; }//配方编号
        public ushort fdClass { get; set; }//炼金等级（100为SS）
        public ushort fdProbability { get; set; }//炼金几率
        public ushort fdAttrType { get; set; }//属性序号
        public float fdAttrValueMin { get; set; }//属性最小值
        public float fdAttrValueMax { get; set; }//属性最大值
        public ushort fdAttrClass { get; set; }//是否隐藏属性

        public int startIndex { get; } = 1;//开始读取数据的行

        public string txtName { get; } = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToLower();

        public TblAlchemistRecipeMixCondition() 
        {
            fdNum = 0;
            fdRecipeNum = 0;
            fdClass = 0;
            fdProbability = 0;
            fdAttrType = 0;
            fdAttrValueMin = 0;
            fdAttrValueMax = 0;
            fdAttrClass = 0;
        }

        public TblAlchemistRecipeMixCondition(ushort fdNum, ushort fdRecipeNum, ushort fdClass, ushort fdProbability, ushort fdAttrType, float fdAttrValueMin, float fdAttrValueMax, ushort fdAttrClass)
        {
            this.fdNum = fdNum;
            this.fdRecipeNum = fdRecipeNum;
            this.fdClass = fdClass;
            this.fdProbability = fdProbability;
            this.fdAttrType = fdAttrType;
            this.fdAttrValueMin = fdAttrValueMin;
            this.fdAttrValueMax = fdAttrValueMax;
            this.fdAttrClass = fdAttrClass;
        }

        public TblAlchemistRecipeMixCondition(string text)
        {
            // 分割文本
            string[] texts = StringDivide.DoDivide(text);

            this.fdNum = ushort.Parse(texts[0]);
            this.fdRecipeNum = ushort.Parse(texts[1]);
            this.fdClass = ushort.Parse(texts[2]);
            this.fdProbability = ushort.Parse(texts[3]);
            this.fdAttrType = ushort.Parse(texts[4]);
            this.fdAttrValueMin = float.Parse(texts[5]);
            this.fdAttrValueMax = float.Parse(texts[6]);
            this.fdAttrClass = ushort.Parse(texts[7]);
        }
    }
}
