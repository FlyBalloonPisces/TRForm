using LiteDBTest.Tools;using LiteDBTest.Interfaces;

namespace LiteDBTest.LuckyBag
{
    public class EssenItemCubeReward : ITextProcessing
    {
        //"clientiteminfo\\essenitemcubereward",
        //fdResultGroup,fdItemNum,fdItemCount,fdAppearanceRate,fdGoldGuage,fdGoldGuageIfDuplicated

        public ushort fdResultGroup { get; set; }//结果组
        public uint fdItemNum { get; set; }//道具编号
        public ushort fdItemCount { get; set; }//道具数量
        public float fdAppearanceRate { get; set; }//出现几率
        public ushort fdGoldGuage { get; set; }//魔方积分
        public ushort fdGoldGuageIfDuplicated { get; set; }//重复时魔方积分

        public int startIndex { get; } = 1;//开始读取数据的行

        public string txtName { get; } = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToLower();

        public EssenItemCubeReward()
        {
            fdResultGroup = 0;
            fdItemNum = 0;
            fdItemCount = 0;
            fdAppearanceRate = 0;
            fdGoldGuage = 0;
            fdGoldGuageIfDuplicated = 0;
        }

        public EssenItemCubeReward(ushort fdResultGroup, uint fdItemNum, ushort fdItemCount, float fdAppearanceRate, ushort fdGoldGuage, ushort fdGoldGuageIfDuplicated)
        {
            this.fdResultGroup = fdResultGroup;
            this.fdItemNum = fdItemNum;
            this.fdItemCount = fdItemCount;
            this.fdAppearanceRate = fdAppearanceRate;
            this.fdGoldGuage = fdGoldGuage;
            this.fdGoldGuageIfDuplicated = fdGoldGuageIfDuplicated;
        }

        public EssenItemCubeReward(string text)
        {
            // 分割文本
            string[] texts = StringDivide.DoDivide(text);

            this.fdResultGroup = ushort.Parse(texts[0]);
            this.fdItemNum = uint.Parse(texts[1]);
            this.fdItemCount = ushort.Parse(texts[2]);
            this.fdAppearanceRate = float.Parse(texts[3]);
            this.fdGoldGuage = ushort.Parse(texts[4]);
            this.fdGoldGuageIfDuplicated = ushort.Parse(texts[5]);
        }
    }
}
