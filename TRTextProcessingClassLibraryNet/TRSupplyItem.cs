using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TRTextProcessingClassLibrary
{
    public class TRSupplyItem : TRItem
    {
        //public ushort Character {  get; set; }//角色
        public uint Position { get; set; } //道具部位
        public uint ItemKind { get; set; } //道具部位编号
        public bool ShowDetailImage { get; set; } //是否展示细节图片
        public string Desc { get; set; } //道具描述

        public TRSupplyItem() : base()
        {
            //Character = 0;
            Position = 0;
            ItemKind = 0;
            ShowDetailImage = false;
            Desc = string.Empty;
        }

        public TRSupplyItem(uint itemNum, string name, string nameCh, byte level, ushort pkgNum, long picOffset, /*ushort character,*/ uint position, uint itemKind, bool showDetailImage, string desc) : base(itemNum, name, nameCh, level, pkgNum, picOffset)
        {
            //Character = character;
            Position = position;
            ItemKind = itemKind;
            ShowDetailImage = showDetailImage;
            Desc = desc;
        }
    }
}
