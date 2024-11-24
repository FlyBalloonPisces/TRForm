using LiteDBTest.Tools;using LiteDBTest.Interfaces;

namespace LiteDBTest.Set
{
    public class TblAvatarItemSetAttr : ITextProcessing
    {
        //"clientiteminfo\\tblavataritemsetattr",
        //fdNum,fdGroupItemDescNum,fdComplexKey,fdAttrType,fdAttrValue,fdApplyTarget
        public uint fdNum { get; set; } //条目序号
        public uint fdGroupItemDescNum { get; set; } //套装编号
        public int fdComplexKey { get; set; } //部分套装属性情况，转二进制
        public uint fdAttrType { get; set; } //属性编号
        public float fdAttrValue { get; set; } //属性数值
        public ushort fdApplyTarget { get; set; } //内外装属性

        public int startIndex { get; } = 1;//开始读取数据的行

        public string txtName { get; } = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToLower();

        /// <summary>
        /// 空白构造函数
        /// </summary>
        public TblAvatarItemSetAttr() 
        {
            fdNum = 0;
            fdGroupItemDescNum = 0;
            fdComplexKey = 0;
            fdAttrType = 0;
            fdAttrValue = 0;
            fdApplyTarget = 0;
        }

        /// <summary>
        /// 基本构造函数
        /// </summary>
        /// <param name="fdNum"></param>
        /// <param name="fdGroupItemDescNum"></param>
        /// <param name="fdComplexKey"></param>
        /// <param name="fdAttrType"></param>
        public TblAvatarItemSetAttr(uint fdNum, uint fdGroupItemDescNum, int fdComplexKey, uint fdAttrType, float fdAttrValue, ushort fdApplyTarget)
        {
            this.fdNum = fdNum;
            this.fdGroupItemDescNum = fdGroupItemDescNum;
            this.fdComplexKey = fdComplexKey;
            this.fdAttrType = fdAttrType;
            this.fdAttrValue = fdAttrValue;
            this.fdApplyTarget = fdApplyTarget;
        }

        /// <summary>
        /// 正式构造函数
        /// </summary>
        /// <param name="text"></param>
        public TblAvatarItemSetAttr(string text)
        {
            // 分割文本
            string[] texts = StringDivide.StringDivide(text);

            this.fdNum = uint.Parse(texts[0]);
            this.fdGroupItemDescNum = uint.Parse(texts[1]);
            this.fdComplexKey = int.Parse(texts[2]);
            this.fdAttrType = uint.Parse(texts[3]);
            this.fdAttrValue = float.Parse(texts[4]);
            this.fdApplyTarget = ushort.Parse(texts[5]);
        }
    }
}
