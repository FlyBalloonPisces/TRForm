using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Item
{
    public class TblAvatarItemDescExClass
    {
        //"clientiteminfo\\tblavataritemdescex"
        //fdNum,fdVirtualItemDescNum,fdSupplyItemDescNum,fdHasExpireTime,fdExpireMinute,fdCount

        /// <summary>
        /// 基本信息
        /// </summary>
        public uint fdNum { get; set; } //条目编号
        public uint fdVirtualItemDescNum { get; set; } //虚拟道具编号，type=2，3，4
        public uint fdSupplyItemDescNum { get; set; } //发放道具编号，type=1
        public bool fdHasExpireTime { get; set; } //是否期限制
        public uint fdExpireMinute { get; set; } //发放时长
        public ushort fdCount { get; set; } //发放数量

        public static int startIndex { get; } = 2;//开始读取数据的行

        /// <summary>
        /// 空白构造函数
        /// </summary>
        public TblAvatarItemDescExClass() 
        {
            fdNum = 0;
            fdVirtualItemDescNum = 0;
            fdSupplyItemDescNum = 0;
            fdHasExpireTime = false;
            fdExpireMinute = 0;
            fdCount = 0;
        }

        /// <summary>
        /// 基本构造函数
        /// </summary>
        /// <param name="fdNum"></param>
        /// <param name="fdVirtualItemDescNum"></param>
        /// <param name="fdSupplyItemDescNum"></param>
        /// <param name="fdHasExpireTime"></param>
        /// <param name="fdExpireMinute"></param>
        /// <param name="fdCount"></param>
        public TblAvatarItemDescExClass(uint fdNum, uint fdVirtualItemDescNum, uint fdSupplyItemDescNum, bool fdHasExpireTime, uint fdExpireMinute, ushort fdCount)
        {
            this.fdNum = fdNum;
            this.fdVirtualItemDescNum = fdVirtualItemDescNum;
            this.fdSupplyItemDescNum = fdSupplyItemDescNum;
            this.fdHasExpireTime = fdHasExpireTime;
            this.fdExpireMinute = fdExpireMinute;
            this.fdCount = fdCount;
        }

        /// <summary>
        /// 正式构造函数
        /// </summary>
        /// <param name="text">单行文本</param>
        public TblAvatarItemDescExClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdNum = uint.Parse(texts[0]);
            this.fdVirtualItemDescNum = uint.Parse(texts[1]);
            this.fdSupplyItemDescNum = uint.Parse(texts[2]);
            this.fdHasExpireTime = bool.Parse(texts[3]);
            this.fdExpireMinute = uint.Parse(texts[4]);
            this.fdCount = ushort.Parse(texts[5]);
        }
    }
}
