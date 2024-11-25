using LiteDBTest.Tools;using LiteDBTest.Interfaces;

namespace LiteDBTest.Exchange
{
    public class EssenExchangeSystemConsumeList : ITextProcessing
    {
        //"clientiteminfo\\essenexchangesystemconsumelist"        
        //fdExchangeID,fdConsumeType,fdConsumeID,fdConsumeCount,fdOrder

        public uint fdExchangeID { get; set; }//交换序号
        public ushort fdConsumeType { get; set; } //交换类型
        public uint fdConsumeID { get; set; }//材料道具编号
        public ushort fdConsumeCount { get; set; }// 材料需要的数量
        public uint fdOrder { get; set; }//交换显示顺序

        public int startIndex { get; } = 1;//开始读取数据的行

        public string txtName { get; } = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToLower();

        public EssenExchangeSystemConsumeList()
        {
            fdExchangeID = 0;
            fdConsumeType = 0;
            fdConsumeID = 0;
            fdConsumeCount = 0;
            fdOrder = 0;
        }

        public EssenExchangeSystemConsumeList(uint fdExchangeID, ushort fdConsumeType, uint fdConsumeID, ushort fdConsumeCount, uint fdOrder)
        {
            this.fdExchangeID = fdExchangeID;
            this.fdConsumeType = fdConsumeType;
            this.fdConsumeID = fdConsumeID;
            this.fdConsumeCount = fdConsumeCount;
            this.fdOrder = fdOrder;
        }

        public EssenExchangeSystemConsumeList(string text)
        {
            // 分割文本
            string[] texts = StringDivide.DoDivide(text);

            this.fdExchangeID = uint.Parse(texts[0]);
            this.fdConsumeType = ushort.Parse(texts[1]);
            this.fdConsumeID = uint.Parse(texts[2]);
            this.fdConsumeCount = ushort.Parse(texts[3]);
            this.fdOrder = ushort.Parse(texts[4]);


        }
    }
}
