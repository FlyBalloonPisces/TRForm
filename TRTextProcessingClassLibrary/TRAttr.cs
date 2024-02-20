using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRTextProcessingClassLibrary
{
    public class TRAttr
    {
        public ushort AttrNum { get; set; }//属性编号
        public float AttrValue { get; set; }//属性数值

        public TRAttr() 
        {
            AttrNum = 0;
            AttrValue = 0;
        }

        public TRAttr(ushort attrNum, float attrValue)
        {
            AttrNum = attrNum;
            AttrValue = attrValue;
        }
    }
}
