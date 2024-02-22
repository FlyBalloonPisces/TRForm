using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Set
{
    public class TblAvatarItemSetDescClass
    {
        //"clientiteminfo\\tblavataritemsetdesc"
        //groupIdx,memberIdx,active,compCount
        public uint groupIdx { get; set; } //套装编号
        public uint memberIdx { get; set; } //成员道具编号
        public int active { get; set; } //属性激活情况
        public ushort compCount { get; set; } //套装成员个数

        public static int startIndex { get; } = 1;//开始读取数据的行

        /// <summary>
        /// 空白构造函数
        /// </summary>
        public TblAvatarItemSetDescClass() 
        {
            groupIdx = 0;
            memberIdx = 0;
            active = 0;
            compCount = 0;
        }

        /// <summary>
        /// 基本构造函数
        /// </summary>
        /// <param name="groupIdx"></param>
        /// <param name="memberIdx"></param>
        /// <param name="active"></param>
        /// <param name="compCount"></param>
        public TblAvatarItemSetDescClass(uint groupIdx, uint memberIdx, int active, ushort compCount)
        {
            this.groupIdx = groupIdx;
            this.memberIdx = memberIdx;
            this.active = active;
            this.compCount = compCount;
        }

        /// <summary>
        /// 正式构造函数
        /// </summary>
        /// <param name="text">单行文本</param>
        public TblAvatarItemSetDescClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.groupIdx = uint.Parse(texts[0]);
            this.memberIdx = uint.Parse(texts[1]);
            this.active = int.Parse(texts[2]);
            this.compCount = ushort.Parse(texts[3]);
        }
    }
}
