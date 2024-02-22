using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Stat
{
    public class EssenStatSystemNodeInfoClass
    {
        //"content\\statsystem\\essenstatsystemnodeinfo"
        //fdCatType,fdIsMain,fdAttrType,fdAttrLevel,fdAttrValue,fdNeedSP,fdTitle,fdOrder

        public uint fdCatType { get; set; }//属性种类
        public bool fdIsMain { get; set; }//是否主属性
        public ushort fdAttrType { get; set; } //属性编号
        public ushort fdAttrLevel { get; set; } //属性等级
        public float fdAttrValue { get; set; }//属性数值
        public ushort fdNeedSP { get; set; }// 属性需要的点数
        public string fdTitle { get; set; }// 属性标题
        public uint fdOrder { get; set; }//属性顺序

        public static int startIndex { get; } = 1;//开始读取数据的行


        public EssenStatSystemNodeInfoClass()
        {
            fdCatType = 0;
            fdAttrType = 0;
            fdAttrLevel = 0;
            fdAttrValue = 0;
            fdNeedSP = 0;
            fdTitle = string.Empty;
            fdOrder = 0;
        }

        public EssenStatSystemNodeInfoClass(uint fdCatType, bool fdIsMain, ushort fdAttrType, ushort fdAttrLevel, float fdAttrValue, ushort fdNeedSP, string fdTitle, uint fdOrder)
        {
            this.fdCatType = fdCatType;
            this.fdIsMain = fdIsMain;
            this.fdAttrType = fdAttrType;
            this.fdAttrLevel = fdAttrLevel;
            this.fdAttrValue = fdAttrValue;
            this.fdNeedSP = fdNeedSP;
            this.fdTitle = fdTitle;
            this.fdOrder = fdOrder;
        }

        public EssenStatSystemNodeInfoClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdCatType = uint.Parse(texts[0]);
            this.fdIsMain = bool.Parse(texts[1]);
            this.fdAttrType = ushort.Parse(texts[2]);
            this.fdAttrLevel = ushort.Parse(texts[3]);
            this.fdAttrValue = float.Parse(texts[4]);
            this.fdNeedSP = ushort.Parse(texts[5]);
            this.fdTitle = texts[6];
            this.fdOrder = ushort.Parse(texts[7]);
        }
    }
}
