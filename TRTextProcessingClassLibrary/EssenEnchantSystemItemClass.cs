using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Enchant
{
    public class EssenEnchantSystemItemClass
    {
        //"clientiteminfo\\enchantsystem\\essenenchantsystemitem",
        //fdItemNum,fdSeqNum,fdSocketNum,fdCostType,fdCost
        public uint fdItemNum { get; set; }//道具编号
        public ushort fdSeqNum { get; set; }//槽位序号
        public ushort fdSocketNum { get; set; }//槽位类型编号
        public ushort fdCostType { get; set; }//消耗材料类型
        public ushort fdCost { get; set; }//消耗材料数量

        public static int startIndex { get; } = 1;//开始读取数据的行


        public EssenEnchantSystemItemClass()
        {
            fdItemNum = 0;
            fdSeqNum = 0;
            fdSocketNum = 0;
            fdCostType = 0;
            fdCost = 0;
        }

        public EssenEnchantSystemItemClass(uint fdItemNum, ushort fdSeqNum, ushort fdSocketNum, ushort fdCostType, ushort fdCost)
        {
            this.fdItemNum = fdItemNum;
            this.fdSeqNum = fdSeqNum;
            this.fdSocketNum = fdSocketNum;
            this.fdCostType = fdCostType;
            this.fdCost = fdCost;
        }

        public EssenEnchantSystemItemClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdItemNum = uint.Parse(texts[0]);
            this.fdSeqNum = ushort.Parse(texts[1]);
            this.fdSocketNum = ushort.Parse(texts[2]);
            this.fdCostType = ushort.Parse(texts[3]);
            this.fdCost = ushort.Parse(texts[4]);
        }
    }
}
