using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRTextProcessingClassLibrary
{
    public class TREquipment : TRSupplyItem
    {
        public ushort Character { get; set; }
        public List<TRAttr> Attrs { get; set; }

        public TREquipment() :base() 
        {
            Character = 0;
            Attrs = new List<TRAttr>();
        }

        public TREquipment(uint itemNum, string name, string nameCh, byte level, ushort pkgNum, long picOffset, uint position, uint itemKind, bool showDetailImage, string desc, ushort character, List<TRAttr> attrs) : base(itemNum, name, nameCh, level, pkgNum, picOffset, position, itemKind, showDetailImage, desc)
        {
            Character = character;
            Attrs = attrs;
        }
    }
}
