using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Collection
{
    public class EssenItemCollectionRenewalGroupClass
    {
        //"collectionrenewal\\essenitem_collectionrenewal_group"
        //fdGroup,fdType,fdName

        public ushort fdGroup { get; set; } //收藏组编号
        public ushort fdType { get; set; } //收藏组类型
        public string fdName { get; set; } //收藏组名称

        public static int startIndex { get; } = 1;//开始读取数据的行

        public EssenItemCollectionRenewalGroupClass()
        {
            fdGroup = 0;
            fdType = 0;
            fdName = string.Empty;
        }

        /// <summary>
        /// 基本构造
        /// </summary>
        /// <param name="fdGroup"></param>
        /// <param name="fdType"></param>
        /// <param name="fdName"></param>
        public EssenItemCollectionRenewalGroupClass(ushort fdGroup, ushort fdType, string fdName)
        {
            this.fdGroup = fdGroup;
            this.fdType = fdType;
            this.fdName = fdName;
        }

        public EssenItemCollectionRenewalGroupClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            this.fdGroup = ushort.Parse(texts[0]);
            this.fdType = ushort.Parse(texts[1]);
            this.fdName = texts[2];
        }
    }
}
