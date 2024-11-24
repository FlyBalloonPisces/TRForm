using LiteDBTest.Tools;using LiteDBTest.Interfaces;

namespace LiteDBTest.Enchant
{
    public class EssenEnchantSystemStoneMount : ITextProcessing
    {
        //"clientiteminfo\\enchantsystem\\essenenchantsystemstonemount"
        //fdItemNum,fdAttr,fdValue,fdRatio

        public uint fdItemNum { get; set; }//宝石道具编号
        public ushort fdAttr { get; set; }//属性编号
        public float fdValue { get; set; }//属性数值
        public ushort fdRatio { get; set; }//几率

        public int startIndex { get; } = 2;//开始读取数据的行

        public string txtName { get; } = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToLower();

        public EssenEnchantSystemStoneMount()
        {
            fdItemNum = 0;
            fdAttr = 0;
            fdValue = 0;
            fdRatio = 0;
        }

        public EssenEnchantSystemStoneMount(uint fdItemNum, ushort fdAttr, float fdValue, ushort fdRatio)
        {
            this.fdItemNum = fdItemNum;
            this.fdAttr = fdAttr;
            this.fdValue = fdValue;
            this.fdRatio = fdRatio;
        }

        public EssenEnchantSystemStoneMount(string text)
        {
            // 分割文本
            string[] texts = StringDivide.StringDivide(text);

            this.fdItemNum = uint.Parse(texts[0]);
            this.fdAttr = ushort.Parse(texts[1]);
            this.fdValue = float.Parse(texts[3]);
            this.fdRatio = ushort.Parse(texts[3]);
        }
    }
}
