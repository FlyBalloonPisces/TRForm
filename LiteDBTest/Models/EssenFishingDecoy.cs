using LiteDBTest.Tools;using LiteDBTest.Interfaces;

namespace LiteDBTest.LuckyBag
{
    public class EssenFishingDecoy:ITextProcessing
    {
        //"content\\fishing\\essenfishing_decoy",
        //fdDecoyNum,fdFishNum
        public uint fdDecoyNum { get; set; } //鱼饵道具编号
        public uint fdFishNum { get; set; } //渔获道具编号

        public int startIndex { get; } = 1;//开始读取数据的行

        public string txtName { get; } = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToLower();

        public EssenFishingDecoy()
        {
            fdDecoyNum = 0;
            fdFishNum = 0;
        }

        public EssenFishingDecoy(uint fdDecoyNum, uint fdFishNum)
        {
            this.fdDecoyNum = fdDecoyNum;
            this.fdFishNum = fdFishNum;
        }

        public EssenFishingDecoy(string text)
        {
            // 分割文本
            string[] texts = StringDivide.StringDivide(text);

            this.fdDecoyNum = uint.Parse(texts[0]);
            this.fdFishNum = uint.Parse(texts[1]);
        }
    }
}
