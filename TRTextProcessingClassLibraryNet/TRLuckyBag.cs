using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary
{
    public class TRLuckyBag : TRSupplyItem
    {
        public List<float> WeightBasic { get; set; } // 基本 存放的是编号，箱子可以开到的道具的权重
        public List<int> MemberBasic { get; set; } // 基本 存放的是编号，箱子可以开到的道具
        public float WeightTotalBasic { get; set; } // 基本 总权重
                                                    //bool Openable { get; set; } //是否可以开启

        public TRLuckyBag()
        {
            WeightBasic = new List<float>();
            MemberBasic = new List<int>();
            WeightTotalBasic = 0;
        }

        public TRLuckyBag(uint itemNum, string name, string nameCh, byte level, ushort pkgNum, long picOffset, uint position, uint itemKind, bool showDetailImage, string desc, List<float> weightBasic, List<int> memberBasic) : base(itemNum, name, nameCh, level, pkgNum, picOffset, position, itemKind, showDetailImage, desc)
        {
            WeightBasic = weightBasic;
            MemberBasic = memberBasic;
            float weightTotalTemp = 0;
            foreach (var item in weightBasic)
            {
                weightTotalTemp = CalculationClass.Plus(weightTotalTemp, item);
            }
            WeightTotalBasic = weightTotalTemp;
        }
    }
}
