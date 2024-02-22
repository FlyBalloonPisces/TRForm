using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Alchemist.Old
{
    public class TblAlchemistRecipeClass
    {
        //"clientiteminfo\\tblalchemist_recipe"
        //fdRecipeNum,fdRecipeCardItemDescNum,fdResultItemDescNum,fdReferenceItemDescNum

        public ushort fdRecipeNum { get; set; }//炼金配方编号
        public uint fdRecipeCardItemDescNum { get; set; }//炼金配方道具编号
        public uint fdResultItemDescNum { get; set; }//炼金结果道具编号
        public uint fdReferenceItemDescNum { get; set; }//炼金展示道具编号

        public static int startIndex { get; } = 1;//开始读取数据的行

        public TblAlchemistRecipeClass() 
        {
            fdRecipeNum = 0;
            fdRecipeCardItemDescNum = 0;
            fdResultItemDescNum = 0;
            fdReferenceItemDescNum = 0;
        }

        public TblAlchemistRecipeClass(ushort fdRecipeNum, uint fdRecipeCardItemDescNum, uint fdResultItemDescNum, uint fdReferenceItemDescNum)
        {
            this.fdRecipeNum = fdRecipeNum;
            this.fdRecipeCardItemDescNum = fdRecipeCardItemDescNum;
            this.fdResultItemDescNum = fdResultItemDescNum;
            this.fdReferenceItemDescNum = fdReferenceItemDescNum;
        }

        public TblAlchemistRecipeClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdRecipeNum = ushort.Parse(texts[0]);
            this.fdRecipeCardItemDescNum = uint.Parse(texts[1]);
            this.fdResultItemDescNum = uint.Parse(texts[2]);
            this.fdReferenceItemDescNum = uint.Parse(texts[3]);
        }
    }
}
