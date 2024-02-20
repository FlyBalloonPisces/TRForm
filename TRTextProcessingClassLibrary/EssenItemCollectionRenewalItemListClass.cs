using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Collection
{
    public class EssenItemCollectionRenewalItemListClass
    {
        //"collectionrenewal\\essenitem_collectionrenewal_itemlist"
        //fdItemNum,fdItemRank,fdBonusPoint,fdID,fdUse,fdType

        public uint fdItemNum { get; set; } //道具编号
        public uint fdItemRank { get; set; } //收藏等级编号
        public ushort fdBonusPoint { get; set; } //奖励点数
        public uint fdID { get; set; } //收藏编号
        public bool fdUse { get; set; } //是否使用
        public ushort fdType { get; set; } //收藏组编号

        public static int startIndex { get; } = 1;//开始读取数据的行

        /// <summary>
        /// 空白构造函数
        /// </summary>
        public EssenItemCollectionRenewalItemListClass() 
        {
            fdItemNum = 0;
            fdItemRank = 0;
            fdBonusPoint = 0;
            fdID = 0;
            fdUse = false;
            fdType = 0;
        }

        /// <summary>
        /// 基本构造函数
        /// </summary>
        /// <param name="fdItemNum"></param>
        /// <param name="fdItemRank"></param>
        /// <param name="fdBonusPoint"></param>
        /// <param name="fdID"></param>
        /// <param name="fdUse"></param>
        /// <param name="fdType"></param>
        public EssenItemCollectionRenewalItemListClass(uint fdItemNum, uint fdItemRank, ushort fdBonusPoint, uint fdID, bool fdUse, ushort fdType)
        {
            this.fdItemNum = fdItemNum;
            this.fdItemRank = fdItemRank;
            this.fdBonusPoint = fdBonusPoint;
            this.fdID = fdID;
            this.fdUse = fdUse;
            this.fdType = fdType;
        }

        /// <summary>
        /// 正式构造函数
        /// </summary>
        /// <param name="text">单行文本</param>
        public EssenItemCollectionRenewalItemListClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdItemNum = uint.Parse(texts[0]);
            this.fdItemRank = uint.Parse(texts[1]);
            this.fdBonusPoint = ushort.Parse(texts[2]);
            this.fdID = uint.Parse(texts[3]);
            this.fdUse = bool.Parse(texts[4]);
            this.fdType = ushort.Parse(texts[5]);
        }
    }
}
