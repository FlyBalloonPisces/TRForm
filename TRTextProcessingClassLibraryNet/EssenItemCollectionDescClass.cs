using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Collection.Old
{
    public class EssenItemCollectionDescClass
    {
        //"clientiteminfo\\essenitemcollectiondesc"
        //fdItemNum,fdPoint,fdID

        /// <summary>
        /// 基本数据
        /// </summary>
        public uint fdItemNum { get; set; } //道具编号
        public ushort fdPoint { get; set; } //收藏分数
        public uint fdID { get; set; } //收藏编号

        public static int startIndex { get; } = 1;//开始读取数据的行

        /// <summary>
        /// 空白构造函数
        /// </summary>
        public EssenItemCollectionDescClass()
        {
            fdItemNum = 0;
            fdPoint = 0;
            fdID = 0;
        }

        /// <summary>
        /// 基本构造函数
        /// </summary>
        /// <param name="fdItemNum"></param>
        /// <param name="fdPoint"></param>
        /// <param name="fdID"></param>
        public EssenItemCollectionDescClass(uint fdItemNum, ushort fdPoint, uint fdID)
        {
            this.fdItemNum = fdItemNum;
            this.fdPoint = fdPoint;
            this.fdID = fdID;
        }

        /// <summary>
        /// 正式构造函数
        /// </summary>
        /// <param name="text">单行文本</param>
        public EssenItemCollectionDescClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdItemNum = uint.Parse(texts[0]);
            this.fdPoint = ushort.Parse(texts[1]);
            this.fdID = uint.Parse(texts[2]);
        }
    }
}
