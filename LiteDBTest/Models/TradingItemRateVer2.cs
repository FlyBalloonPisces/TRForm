using LiteDBTest.Tools;using LiteDBTest.Interfaces;

namespace LiteDBTest.LuckyBag
{
    public class TradingItemRateVer2 : ITextProcessing
    {
        //"content\\trading\\tradingitemrate_ver2"
        //fdTicketItem,fdSourcePosition,fdSourceLevel,fdTargetItem,fdTargetPosition,fdTargetLevel,fdResult
        public uint fdTicketItem { get; set; }//交换券道具
        public ushort fdSourcePosition { get; set; }//材料部位
        public ushort fdSourceLevel { get; set; }//材料等级
        public uint fdTargetItem { get; set; }//结果道具
        public ushort fdTargetPosition { get; set; }//结果部位
        public ushort fdTargetLevel { get; set; }//结果等级
        public ushort fdResult { get; set; }//结果

        public int startIndex { get; } = 1;//开始读取数据的行

        public string txtName { get; } = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToLower();

        public TradingItemRateVer2()
        {
            fdTicketItem = 0;
            fdSourcePosition = 0;
            fdSourceLevel = 0;
            fdTargetItem = 0;
            fdTargetPosition = 0;
            fdTargetLevel = 0;
            fdResult = 0;
        }

        public TradingItemRateVer2(uint fdTicketItem, ushort fdSourcePosition, ushort fdSourceLevel, uint fdTargetItem, ushort fdTargetPosition, ushort fdTargetLevel, ushort fdResult)
        {
            this.fdTicketItem = fdTicketItem;
            this.fdSourcePosition = fdSourcePosition;
            this.fdSourceLevel = fdSourceLevel;
            this.fdTargetItem = fdTargetItem;
            this.fdTargetPosition = fdTargetPosition;
            this.fdTargetLevel = fdTargetLevel;
            this.fdResult = fdResult;
        }

        public TradingItemRateVer2(string text)
        {
            // 分割文本
            string[] texts = StringDivide.DoDivide(text);

            this.fdTicketItem = uint.Parse(texts[0]);
            this.fdSourcePosition = ushort.Parse(texts[1]);
            this.fdSourceLevel = ushort.Parse(texts[2]);
            this.fdTargetItem = uint.Parse(texts[3]);
            this.fdTargetPosition = ushort.Parse(texts[4]);
            this.fdTargetLevel = ushort.Parse(texts[5]);
            this.fdResult = ushort.Parse(texts[6]);
        }
    }
}
