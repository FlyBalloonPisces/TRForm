using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Collection
{
    public class EssenItemCollectionRenewalItemRankClass
    {
        //"collectionrenewal\\essenitem_collectionrenewal_itemrank"
        //fdItemRank,fdName,fdPoint
        public ushort fdItemRank { get; set; } //收藏等级编号
        public string fdName { get; set; } //收藏等级名称
        public ushort fdPoint { get; set; } //收藏分数

        public static int startIndex { get; } = 1;//开始读取数据的行

        /// <summary>
        /// 空构造
        /// </summary>
        public EssenItemCollectionRenewalItemRankClass() 
        {
            fdItemRank = 0;
            fdName = string.Empty;
            fdPoint = 0;
        }

        /// <summary>
        /// 基本构造
        /// </summary>
        /// <param name="fdItemRank"></param>
        /// <param name="fdName"></param>
        /// <param name="fdPoint"></param>
        public EssenItemCollectionRenewalItemRankClass(ushort fdItemRank, string fdName, ushort fdPoint)
        {
            this.fdItemRank = fdItemRank;
            this.fdName = fdName;
            this.fdPoint = fdPoint;
        }

        /// <summary>
        /// 正式构造
        /// </summary>
        /// <param name="text"></param>
        public EssenItemCollectionRenewalItemRankClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdItemRank = ushort.Parse(texts[0]);
            this.fdName = texts[1];
            this.fdPoint = ushort.Parse(texts[2]);
        }
    }
}
