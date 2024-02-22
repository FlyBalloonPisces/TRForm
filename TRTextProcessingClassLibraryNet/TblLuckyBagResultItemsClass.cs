using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.LuckyBag
{
    public class TblLuckyBagResultItemsClass
    {
        //"tblluckybagresultitems_1",
        //fdNum,fdItemDescNum,fdLuckyBagNum,fdCount
        public uint fdNum { get; set; }//条目编号
        public uint fdItemDescNum { get; set; }//结果道具编号
        public uint fdLuckyBagNum { get; set; }//箱子道具编号
        public uint fdCount { get; set; }//道具几率

        public static int startIndex { get; } = 1;//开始读取数据的行


        public TblLuckyBagResultItemsClass()
        {
            fdNum = 0;
            fdItemDescNum = 0;
            fdLuckyBagNum = 0;
            fdCount = 0;
        }

        public TblLuckyBagResultItemsClass(uint fdNum, uint fdItemDescNum, uint fdLuckyBagNum, uint fdCount)
        {
            this.fdNum = fdNum;
            this.fdItemDescNum = fdItemDescNum;
            this.fdLuckyBagNum = fdLuckyBagNum;
            this.fdCount = fdCount;
        }

        public TblLuckyBagResultItemsClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdNum = uint.Parse(texts[0]);
            this.fdItemDescNum = uint.Parse(texts[1]);
            this.fdLuckyBagNum = uint.Parse(texts[2]);
            this.fdCount = uint.Parse(texts[3]);
        }
    }
}
