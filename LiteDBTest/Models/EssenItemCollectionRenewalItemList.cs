using LiteDBTest.Tools;using LiteDBTest.Interfaces;

namespace LiteDBTest.Collection
{
    public class EssenItemCollectionRenewalItemList : ITextProcessing
    {
        //"collectionrenewal\\essenitem_collectionrenewal_itemlist"
        //fdItemNum,fdItemRank,fdBonusPoint,fdID,fdUse,fdType

        public uint fdItemNum { get; set; } //道具编号
        public uint fdItemRank { get; set; } //收藏等级编号
        public ushort fdBonusPoint { get; set; } //奖励点数
        public uint fdID { get; set; } //收藏编号
        public bool fdUse { get; set; } //是否使用
        public ushort fdType { get; set; } //收藏组编号

        public int startIndex { get; } = 1;//开始读取数据的行

        public string txtName { get; } = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName.ToLower();

        /// <summary>
        /// 空白构造函数
        /// </summary>
        public EssenItemCollectionRenewalItemList() 
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
        public EssenItemCollectionRenewalItemList(uint fdItemNum, uint fdItemRank, ushort fdBonusPoint, uint fdID, bool fdUse, ushort fdType)
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
        public EssenItemCollectionRenewalItemList(string text)
        {
            // 分割文本
            string[] texts = StringDivide.DoDivide(text);

            this.fdItemNum = uint.Parse(texts[0]);
            this.fdItemRank = uint.Parse(texts[1]);
            this.fdBonusPoint = ushort.Parse(texts[2]);
            this.fdID = uint.Parse(texts[3]);
            this.fdUse = bool.Parse(texts[4]);
            this.fdType = ushort.Parse(texts[5]);
        }
    }
}
