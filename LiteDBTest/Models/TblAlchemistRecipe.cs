using LiteDBTest.Tools;using LiteDBTest.Interfaces;

namespace LiteDBTest.Alchemist.Old
{
    public class TblAlchemistRecipe : ITextProcessing
    {
        //"clientiteminfo\\tblalchemist_recipe"
        //fdRecipeNum,fdRecipeCardItemDescNum,fdResultItemDescNum,fdReferenceItemDescNum

        public ushort fdRecipeNum { get; set; }//炼金配方编号
        public uint fdRecipeCardItemDescNum { get; set; }//炼金配方道具编号
        public uint fdResultItemDescNum { get; set; }//炼金结果道具编号
        public uint fdReferenceItemDescNum { get; set; }//炼金展示道具编号

        public int startIndex { get; } = 1;//开始读取数据的行

        public string txtName { get; } = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToLower();

        public TblAlchemistRecipe() 
        {
            fdRecipeNum = 0;
            fdRecipeCardItemDescNum = 0;
            fdResultItemDescNum = 0;
            fdReferenceItemDescNum = 0;
        }

        public TblAlchemistRecipe(ushort fdRecipeNum, uint fdRecipeCardItemDescNum, uint fdResultItemDescNum, uint fdReferenceItemDescNum)
        {
            this.fdRecipeNum = fdRecipeNum;
            this.fdRecipeCardItemDescNum = fdRecipeCardItemDescNum;
            this.fdResultItemDescNum = fdResultItemDescNum;
            this.fdReferenceItemDescNum = fdReferenceItemDescNum;
        }

        public TblAlchemistRecipe(string text)
        {
            // 分割文本
            string[] texts = StringDivide.DoDivide(text);

            this.fdRecipeNum = ushort.Parse(texts[0]);
            this.fdRecipeCardItemDescNum = uint.Parse(texts[1]);
            this.fdResultItemDescNum = uint.Parse(texts[2]);
            this.fdReferenceItemDescNum = uint.Parse(texts[3]);
        }
    }
}
