using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary
{
    public class TRCube : TRLuckyBag
    {
        public List<float> WeightSilver { get; set; } // 魔方银色 存放的是编号，箱子可以开到的道具的权重
        public List<int> MemberSilver { get; set; } // 魔方银色 存放的是编号，箱子可以开到的道具
        public List<float> WeightGold { get; set; } // 魔方金色 存放的是编号，箱子可以开到的道具的权重
        public List<int> MemberGold { get; set; } // 魔方金色 存放的是编号，箱子可以开到的道具

        public float WeightTotalSilver { get; set; } // 魔方银色 总权重
        public float WeightTotalGold { get; set; } // 魔方金色 总权重
        public float SilverRate { get; set; } // 魔方银色概率

        public TRCube() :base()
        {
            WeightSilver = new List<float>();
            MemberSilver = new List<int>();
            WeightGold = new List<float>();
            MemberGold = new List<int>();

            WeightTotalSilver = 0;
            WeightTotalGold = 0;
            SilverRate = 0;
        }

        public TRCube(uint itemNum, string name, string nameCh, byte level, ushort pkgNum, long picOffset, uint position, uint itemKind, bool showDetailImage, string desc, List<float> weightBasic, List<int> memberBasic, List<float> weightSilver, List<int> memberSilver, List<float> weightGold, List<int> memberGold) : base(itemNum, name, nameCh, level, pkgNum, picOffset, position, itemKind, showDetailImage, desc, weightBasic, memberBasic)
        {
            WeightSilver = weightSilver;
            MemberSilver = memberSilver;
            WeightGold = weightGold;
            MemberGold = memberGold;

            float weightTotalTemp = 0;
            foreach (var item in weightSilver)
            {
                weightTotalTemp = CalculationClass.Plus(weightTotalTemp, item);
            }
            WeightTotalSilver = weightTotalTemp;

            weightTotalTemp = 0;
            foreach (var item in weightGold)
            {
                weightTotalTemp = CalculationClass.Plus(weightTotalTemp, item);
            }
            WeightTotalGold = weightTotalTemp;
        }
    }
}
