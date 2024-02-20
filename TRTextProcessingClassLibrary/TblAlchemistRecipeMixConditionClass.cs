using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Alchemist.Old
{
    public class TblAlchemistRecipeMixConditionClass
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

        public static int startIndex { get; } = 1;//开始读取数据的行

        public TblAlchemistRecipeMixConditionClass() 
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

        public TblAlchemistRecipeMixConditionClass(ushort fdNum, ushort fdRecipeNum, ushort fdClass, ushort fdProbability, ushort fdAttrType, float fdAttrValueMin, float fdAttrValueMax, ushort fdAttrClass)
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

        public TblAlchemistRecipeMixConditionClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

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
