using LiteDBTest.Interfaces;
using LiteDBTest.Tools;using LiteDBTest.Interfaces;

namespace LiteDBTest
{
    public class EssenArchivesExchangeList : ITextProcessing
    {
        //"archives\\essenarchives_exchangelist",
        //fdArtifactsNum,fdRewardItemNum,fdCount
        public uint fdArtifactsNum { get; set; }//工艺品道具编号
        public uint fdRewardItemNum { get; set; }//结果道具编号
        public ushort fdCount { get; set; } //结果道具数量

        public int startIndex { get; } = 1;//开始读取数据的行

        public string txtName { get; } = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToLower();

        public EssenArchivesExchangeList()
        {
            fdArtifactsNum = 0;
            fdRewardItemNum = 0;
            fdCount = 0;
        }

        public EssenArchivesExchangeList(uint fdArtifactsNum, uint fdRewardItemNum, ushort fdCount)
        {
            this.fdArtifactsNum = fdArtifactsNum;
            this.fdRewardItemNum = fdRewardItemNum;
            this.fdCount = fdCount;
        }

        public EssenArchivesExchangeList(string text)
        {
            // 分割文本
            string[] texts = StringDivide.DoDivide(text);

            this.fdArtifactsNum = uint.Parse(texts[0]);
            this.fdRewardItemNum = uint.Parse(texts[1]);
            this.fdCount = ushort.Parse(texts[2]);
        }
    }
}
