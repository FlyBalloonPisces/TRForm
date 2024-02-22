using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TRTextProcessingClassLibrary
{
    public class TRVirtualItem : TRItem
    {
        public uint SupplyItemNum {  get; set; } // 提供道具编号

        public TRVirtualItem() : base()
        {
            SupplyItemNum = 0;
        }

        public TRVirtualItem(uint itemNum, string name, string nameCh, byte level, ushort pkgNum, long picOffset, uint supplyItemNum) : base(itemNum, name, nameCh, level, pkgNum, picOffset)
        {
            SupplyItemNum = supplyItemNum;
        }

        public TRVirtualItem(TRItem virtualItem, TRItem supplyItem)
        {
            ItemNum = virtualItem.ItemNum;
            Name = virtualItem.Name;
            NameCh = virtualItem.NameCh;
            Level = virtualItem.Level;
            SupplyItemNum = supplyItem.ItemNum;
            PkgNum = supplyItem.PkgNum;
            PicOffset = supplyItem.PicOffset;
        }
    }
}
