using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Exchange
{
    public class EssenExchangeSystemInfoClass
    {
        //"clientiteminfo\\essenexchangesystemconsumelist"        
        //fdExchangeID,fdSystemNum,fdMix,fdExchangeType,fdExchangeItem,fdExchangeCount,fdExchangeRate

        public uint fdExchangeID { get; set; }//交换序号
        public ushort fdSystemNum { get; set; } //交换系统编号
        public bool fdMix { get; set; }//是否混合
        public ushort fdExchangeType { get; set; } //交换类型
        public uint fdExchangeItem { get; set; }//交换道具编号
        public ushort fdExchangeCount { get; set; }// 交换获得的数量
        public ushort fdExchangeRate { get; set; }//交换的比率

        public static int startIndex { get; } = 1;//开始读取数据的行

        public EssenExchangeSystemInfoClass()
        {
            fdExchangeID = 0;
            fdSystemNum = 0;
            fdMix = false;
            fdExchangeType = 0;
            fdExchangeItem = 0;
            fdExchangeCount = 0;
            fdExchangeRate = 0;
        }

        public EssenExchangeSystemInfoClass(uint fdExchangeID, ushort fdSystemNum, bool fdMix, ushort fdExchangeType, uint fdExchangeItem, ushort fdExchangeCount, ushort fdExchangeRate)
        {
            this.fdExchangeID = fdExchangeID;
            this.fdSystemNum = fdSystemNum;
            this.fdMix = fdMix;
            this.fdExchangeType = fdExchangeType;
            this.fdExchangeItem = fdExchangeItem;
            this.fdExchangeCount = fdExchangeCount;
            this.fdExchangeRate = fdExchangeRate;
        }

        public EssenExchangeSystemInfoClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdExchangeID = uint.Parse(texts[0]);
            this.fdSystemNum = ushort.Parse(texts[1]);
            this.fdMix = bool.Parse(texts[2]);
            this.fdExchangeType = ushort.Parse(texts[3]);
            this.fdExchangeItem = uint.Parse(texts[4]);
            this.fdExchangeCount = ushort.Parse(texts[5]);
            this.fdExchangeRate = ushort.Parse(texts[6]);
        }


    }
}
