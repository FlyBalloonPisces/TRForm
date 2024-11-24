using LiteDBTest.Interfaces;
using LiteDBTest.Tools;using LiteDBTest.Interfaces;

namespace LiteDBTest.Alchemist
{
    public class EssenAlchemistMixCondition : ITextProcessing
    {
        //"content\\alchemist\\essenalchemistmixcondition",
        //fdItemNum,fdClass,fdProbability,fdAttrType,fdAttrValueMin,fdAttrValueMax,fdAttrClass

        public uint fdItemNum { get; set; } //道具编号
        public ushort fdClass { get; set; } //炼金等级（100为SS）
        public ushort fdProbability { get; set; }//炼金几率
        public ushort fdAttrType { get; set; }//属性序号
        public float fdAttrValueMin { get; set; }//属性最小值
        public float fdAttrValueMax { get; set; }//属性最大值
        public ushort fdAttrClass { get; set; }//是否隐藏属性

        public int startIndex { get; } = 1;//开始读取数据的行

        public string txtName { get; } = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToLower();

        public EssenAlchemistMixCondition()
        {
            fdItemNum = 0;
            fdClass = 0;
            fdProbability = 0;
            fdAttrType = 0;
            fdAttrValueMin = 0;
            fdAttrValueMax = 0;
            fdAttrClass = 0;
        }

        public EssenAlchemistMixCondition(uint fdItemNum, ushort fdClass, ushort fdProbability, ushort fdAttrType, float fdAttrValueMin, float fdAttrValueMax, ushort fdAttrClass)
        {
            this.fdItemNum = fdItemNum;
            this.fdClass = fdClass;
            this.fdProbability = fdProbability;
            this.fdAttrType = fdAttrType;
            this.fdAttrValueMin = fdAttrValueMin;
            this.fdAttrValueMax = fdAttrValueMax;
            this.fdAttrClass = fdAttrClass;
        }

        public EssenAlchemistMixCondition(string text)
        {
            // 分割文本
            string[] texts = StringDivide.StringDivide(text);

            this.fdItemNum = uint.Parse(texts[0]);
            this.fdClass = ushort.Parse(texts[1]);
            this.fdProbability = ushort.Parse(texts[2]);
            this.fdAttrType = ushort.Parse(texts[3]);
            this.fdAttrValueMin = float.Parse(texts[4]);
            this.fdAttrValueMax = float.Parse(texts[5]);
            this.fdAttrClass = ushort.Parse(texts[6]);
        }
    }
}
