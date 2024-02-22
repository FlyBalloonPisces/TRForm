using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Item
{
    public class TblAvatarItemDescAttrClass
    {
        //"clientiteminfo\\tblavataritemdescattr"
        //fdNum,fdItemNum,fdAttr,fdValue
        /// <summary>
        /// 基本变量
        /// </summary>
        public uint fdNum { get; set; } //条目编号
        public uint fdItemNum { get; set; } //道具编号
        public ushort fdAttr { get; set; } //属性序号
        public float fdValue { get; set; } //属性数值

        public static int startIndex { get; } = 2;//开始读取数据的行

        /// <summary>
        /// 空白构造函数
        /// </summary>
        public TblAvatarItemDescAttrClass()
        {
            fdNum = 0;
            fdItemNum = 0;
            fdAttr = 0;
            fdValue = 0;
        }

        public TblAvatarItemDescAttrClass(uint fdNum, uint fdItemNum, ushort fdAttr, float fdValue)
        {
            this.fdNum = fdNum;
            this.fdItemNum = fdItemNum;
            this.fdAttr = fdAttr;
            this.fdValue = fdValue;
        }

        /// <summary>
        /// 正式构造函数
        /// </summary>
        /// <param name="text">单行文本</param>
        public TblAvatarItemDescAttrClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdNum = uint.Parse(texts[0]);
            this.fdItemNum = uint.Parse(texts[1]);
            this.fdAttr = ushort.Parse(texts[2]);
            this.fdValue = float.Parse(texts[3]);
        }
    }
}
