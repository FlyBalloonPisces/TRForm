using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Enchant
{
    public class EssenEnchantSystemStoneMountClass
    {
        //"clientiteminfo\\enchantsystem\\essenenchantsystemstonemount"
        //fdItemNum,fdAttr,fdValue,fdRatio

        public uint fdItemNum { get; set; }//宝石道具编号
        public ushort fdAttr { get; set; }//属性编号
        public float fdValue { get; set; }//属性数值
        public ushort fdRatio { get; set; }//几率

        public static int startIndex { get; } = 2;//开始读取数据的行

        public EssenEnchantSystemStoneMountClass()
        {
            fdItemNum = 0;
            fdAttr = 0;
            fdValue = 0;
            fdRatio = 0;
        }

        public EssenEnchantSystemStoneMountClass(uint fdItemNum, ushort fdAttr, float fdValue, ushort fdRatio)
        {
            this.fdItemNum = fdItemNum;
            this.fdAttr = fdAttr;
            this.fdValue = fdValue;
            this.fdRatio = fdRatio;
        }

        public EssenEnchantSystemStoneMountClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdItemNum = uint.Parse(texts[0]);
            this.fdAttr = ushort.Parse(texts[1]);
            this.fdValue = float.Parse(texts[3]);
            this.fdRatio = ushort.Parse(texts[3]);
        }
    }
}
