using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.LuckyBag
{
    public class EssenFishingDecoyClass
    {
        //"content\\fishing\\essenfishing_decoy",
        //fdDecoyNum,fdFishNum
        public uint fdDecoyNum { get; set; } //鱼饵道具编号
        public uint fdFishNum { get; set; } //渔获道具编号

        public static int startIndex { get; } = 1;//开始读取数据的行

        public EssenFishingDecoyClass()
        {
            fdDecoyNum = 0;
            fdFishNum = 0;
        }

        public EssenFishingDecoyClass(uint fdDecoyNum, uint fdFishNum)
        {
            this.fdDecoyNum = fdDecoyNum;
            this.fdFishNum = fdFishNum;
        }

        public EssenFishingDecoyClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdDecoyNum = uint.Parse(texts[0]);
            this.fdFishNum = uint.Parse(texts[1]);
        }
    }
}
