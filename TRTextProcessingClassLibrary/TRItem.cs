using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRTextProcessingClassLibrary
{
    public class TRItem
    {
        public uint ItemNum { get; set; }
        public string Name { get; set; }
        public string NameCh {  get; set; }

        public byte Level { get; set; } // 抽奖等显示的等级

        public ushort PkgNum { get; set; } // 图片文件所在pkg -1代表无图片
        public long PicOffset { get; set; }

        public TRItem()
        {
            ItemNum = 0;
            Name = string.Empty;
            NameCh = string.Empty;
            Level = 0;
            PkgNum = 0;
            PicOffset = 0;
        }

        public TRItem(uint itemNum, string name, string nameCh, byte level, ushort pkgNum, long picOffset)
        {
            ItemNum = itemNum;
            Name = name;
            NameCh = nameCh;
            Level = level;
            PkgNum = pkgNum;
            PicOffset = picOffset;
        }
    }
}
