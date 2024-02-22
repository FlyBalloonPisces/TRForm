using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.LuckyBag
{
    public class SettingItemCubeListClass
    {
        //"clientiteminfo\\settingitemcubelist"
        //fdCubeItem,fdBronzeCubeCount,fdBronzeOpenableCount,fdBronzeAcceptableCount,fdBronzeResultGroup,fdSilverCubeCount,fdSilverOpenableCount,fdSilverAcceptableCount,fdSilverResultGroup,fdGoldCubeCount,fdGoldCubeOpenableCount,fdGoldAcceptableCount,fdGoldResultGroup,fdCanMoveStorage,fdSilverRate,fdGoldGuageMax
        public uint fdCubeItem { get; set; }//魔方道具编号
        public ushort fdBronzeCubeCount { get; set; }//魔方展示个数
        public ushort fdBronzeOpenableCount { get; set; }//魔方可开个数
        public ushort fdBronzeAcceptableCount { get; set; }//魔方可领个数
        public ushort fdBronzeResultGroup { get; set; }//魔方结果组编号
        public ushort fdSilverCubeCount { get; set; }//魔方展示个数
        public ushort fdSilverOpenableCount { get; set; }//魔方可开个数
        public ushort fdSilverAcceptableCount { get; set; }//魔方可领个数
        public ushort fdSilverResultGroup { get; set; }//魔方结果组编号
        public ushort fdGoldCubeCount { get; set; }//魔方展示个数
        public ushort fdGoldCubeOpenableCount { get; set; }//魔方可开个数
        public ushort fdGoldAcceptableCount { get; set; }//魔方可领个数
        public ushort fdGoldResultGroup { get; set; }//魔方结果组编号
        public ushort fdCanMoveStorage { get; set; }//魔方是否可以保管
        public float fdSilverRate { get; set; }//银魔方几率
        public ushort fdGoldGuageMax { get; set; }//金魔方积分条件

        public static int startIndex { get; } = 1;//开始读取数据的行

        public SettingItemCubeListClass()
        {
            this.fdCubeItem = 0;
            this.fdBronzeCubeCount = 0;
            this.fdBronzeOpenableCount = 0;
            this.fdBronzeAcceptableCount = 0;
            this.fdBronzeResultGroup = 0;
            this.fdSilverCubeCount = 0;
            this.fdSilverOpenableCount = 0;
            this.fdSilverAcceptableCount = 0;
            this.fdSilverResultGroup = 0;
            this.fdGoldCubeCount = 0;
            this.fdGoldCubeOpenableCount = 0;
            this.fdGoldAcceptableCount = 0;
            this.fdGoldResultGroup = 0;
            this.fdCanMoveStorage = 0;
            this.fdSilverRate = 0;
            this.fdGoldGuageMax = 0;
        }

        public SettingItemCubeListClass(uint fdCubeItem, ushort fdBronzeCubeCount, ushort fdBronzeOpenableCount, ushort fdBronzeAcceptableCount, ushort fdBronzeResultGroup, ushort fdSilverCubeCount, ushort fdSilverOpenableCount, ushort fdSilverAcceptableCount, ushort fdSilverResultGroup, ushort fdGoldCubeCount, ushort fdGoldCubeOpenableCount, ushort fdGoldAcceptableCount, ushort fdGoldResultGroup, ushort fdCanMoveStorage, float fdSilverRate, ushort fdGoldGuageMax)
        {
            this.fdCubeItem = fdCubeItem;
            this.fdBronzeCubeCount = fdBronzeCubeCount;
            this.fdBronzeOpenableCount = fdBronzeOpenableCount;
            this.fdBronzeAcceptableCount = fdBronzeAcceptableCount;
            this.fdBronzeResultGroup = fdBronzeResultGroup;
            this.fdSilverCubeCount = fdSilverCubeCount;
            this.fdSilverOpenableCount = fdSilverOpenableCount;
            this.fdSilverAcceptableCount = fdSilverAcceptableCount;
            this.fdSilverResultGroup = fdSilverResultGroup;
            this.fdGoldCubeCount = fdGoldCubeCount;
            this.fdGoldCubeOpenableCount = fdGoldCubeOpenableCount;
            this.fdGoldAcceptableCount = fdGoldAcceptableCount;
            this.fdGoldResultGroup = fdGoldResultGroup;
            this.fdCanMoveStorage = fdCanMoveStorage;
            this.fdSilverRate = fdSilverRate;
            this.fdGoldGuageMax = fdGoldGuageMax;
        }

        public SettingItemCubeListClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdCubeItem = uint.Parse(texts[0]);
            this.fdBronzeCubeCount = ushort.Parse(texts[1]);
            this.fdBronzeOpenableCount = ushort.Parse(texts[2]);
            this.fdBronzeAcceptableCount = ushort.Parse(texts[3]);
            this.fdBronzeResultGroup = ushort.Parse(texts[4]);
            this.fdSilverCubeCount = ushort.Parse(texts[5]);
            this.fdSilverOpenableCount = ushort.Parse(texts[6]);
            this.fdSilverAcceptableCount = ushort.Parse(texts[7]);
            this.fdSilverResultGroup = ushort.Parse(texts[8]);
            this.fdGoldCubeCount = ushort.Parse(texts[9]);
            this.fdGoldCubeOpenableCount = ushort.Parse(texts[10]);
            this.fdGoldAcceptableCount = ushort.Parse(texts[11]);
            this.fdGoldResultGroup = ushort.Parse(texts[12]);
            this.fdCanMoveStorage = ushort.Parse(texts[13]);
            this.fdSilverRate = float.Parse(texts[14]);
            this.fdGoldGuageMax = ushort.Parse(texts[15]);
        }
    }
}
