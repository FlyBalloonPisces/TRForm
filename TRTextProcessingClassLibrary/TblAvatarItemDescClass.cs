using System;
using TRTextProcessingClassLibrary.Tool;

namespace TRTextProcessingClassLibrary.Item
{
    public class TblAvatarItemDescClass
    {
        //"clientiteminfo\\tblavataritemdesc"
        //fdItemNum,fdBillingItemID,fdType,fdCharacter,fdPosition,fdItemKind,fdGameMoney,fdFarmPoint,fdCash,fdGuildPoint,fdContributionPoint,fdItemName,fdItemCategory,fdGotRateKind,fdOnOfftype,fdCanUse,fdNotDeleteWhenExpired,fdCanStack,fdNewMark,fdCoupleItemType,fdShowShop,fdPurchasable,fdShowDetailImage,fdDesc,fdRefundable,fdEtc,fdLevel,fdFixedExpireDate
        /// <summary>
        /// 基本信息
        /// </summary>
        public uint fdItemNum { get; set; } //道具编号
        public ushort fdBillingItemID { get; set; } //盈利编号
        public ushort fdType { get; set; } //道具种类
        public ushort fdCharacter { get; set; } //限定角色
        public uint fdPosition { get; set; } //道具部位
        public uint fdItemKind { get; set; } //道具部位编号
        public uint fdGameMoney { get; set; } //TR
        public uint fdFarmPoint { get; set; } //农场点
        public uint fdCash { get; set; } //点券
        public uint fdGuildPoint { get; set; } //全体公会点数
        public uint fdContributionPoint { get; set; } //个人公会点数
        public string fdItemName { get; set; } //道具名称
        public uint fdItemCategory { get; set; } //道具页签
        public ushort fdGotRateKind { get; set; } //卡片等级
        public ushort fdOnOfftype { get; set; } //开关类型
        public bool fdCanUse { get; set; } //是否启用使用按钮
        public bool fdNotDeleteWhenExpired { get; set; } //是否过期删除
        public bool fdCanStack { get; set; } //是否可累计
        public bool fdNewMark { get; set; } //是否标新
        public ushort fdCoupleItemType { get; set; } //情侣道具种类
        public bool fdShowShop { get; set; } //是否展示商店
        public bool fdPurchasable { get; set; } //是否可购买
        public bool fdShowDetailImage { get; set; } //是否展示细节图片
        public string fdDesc { get; set; } //道具描述
        public bool fdRefundable { get; set; } //是否可回收
        public uint fdEtc { get; set; } //其他数据
        public ushort fdLevel { get; set; } //道具等级，体现在开箱界面
        public string fdFixedExpireDate { get; set; } //过期时间

        public static int startIndex { get; } = 2;//开始读取数据的行

        /// <summary>
        /// 空白构造函数
        /// </summary>
        public TblAvatarItemDescClass() 
        {
            fdItemNum = 0;
            fdItemCategory = 0;
            fdType = 0;
            fdCharacter = 0;
            fdPosition = 0;
            fdItemKind = 0;
            fdGameMoney = 0;
            fdFarmPoint = 0;
            fdCash = 0;
            fdGuildPoint = 0;
            fdContributionPoint = 0;
            fdItemName = string.Empty;
            fdItemCategory = 0;
            fdGotRateKind = 0;
            fdOnOfftype = 0;
            fdCanUse = false;
            fdNotDeleteWhenExpired = false;
            fdCanStack = false;
            fdNewMark = false;
            fdCoupleItemType = 0;
            fdShowShop = false;
            fdPurchasable = false;
            fdShowDetailImage = false;
            fdDesc = string.Empty;
            fdRefundable = false;
            fdEtc = 0;
            fdLevel = 0;
            fdFixedExpireDate = string.Empty;
        }

        /// <summary>
        /// 基本构造函数
        /// </summary>
        /// <param name="fdItemNum"></param>
        /// <param name="fdBillingItemID"></param>
        /// <param name="fdType"></param>
        /// <param name="fdCharacter"></param>
        /// <param name="fdPosition"></param>
        /// <param name="fdItemKind"></param>
        /// <param name="fdGameMoney"></param>
        /// <param name="fdFarmPoint"></param>
        /// <param name="fdCash"></param>
        /// <param name="fdGuildPoint"></param>
        /// <param name="fdContributionPoint"></param>
        /// <param name="fdItemName"></param>
        /// <param name="fdItemCategory"></param>
        /// <param name="fdGotRateKind"></param>
        /// <param name="fdOnOfftype"></param>
        /// <param name="fdCanUse"></param>
        /// <param name="fdNotDeleteWhenExpired"></param>
        /// <param name="fdCanStack"></param>
        /// <param name="fdNewMark"></param>
        /// <param name="fdCoupleItemType"></param>
        /// <param name="fdShowShop"></param>
        /// <param name="fdPurchasable"></param>
        /// <param name="fdShowDetailImage"></param>
        /// <param name="fdDesc"></param>
        /// <param name="fdRefundable"></param>
        /// <param name="fdEtc"></param>
        /// <param name="fdLevel"></param>
        /// <param name="fdFixedExpireDate"></param>
        public TblAvatarItemDescClass(uint fdItemNum, ushort fdBillingItemID, ushort fdType, ushort fdCharacter, uint fdPosition, uint fdItemKind, uint fdGameMoney, uint fdFarmPoint, uint fdCash, uint fdGuildPoint, uint fdContributionPoint, string fdItemName, uint fdItemCategory, ushort fdGotRateKind, ushort fdOnOfftype, bool fdCanUse, bool fdNotDeleteWhenExpired, bool fdCanStack, bool fdNewMark, ushort fdCoupleItemType, bool fdShowShop, bool fdPurchasable, bool fdShowDetailImage, string fdDesc, bool fdRefundable, uint fdEtc, ushort fdLevel, string fdFixedExpireDate)
        {
            this.fdItemNum = fdItemNum;
            this.fdBillingItemID = fdBillingItemID;
            this.fdType = fdType;
            this.fdCharacter = fdCharacter;
            this.fdPosition = fdPosition;
            this.fdItemKind = fdItemKind;
            this.fdGameMoney = fdGameMoney;
            this.fdFarmPoint = fdFarmPoint;
            this.fdCash = fdCash;
            this.fdGuildPoint = fdGuildPoint;
            this.fdContributionPoint = fdContributionPoint;
            this.fdItemName = fdItemName;
            this.fdItemCategory = fdItemCategory;
            this.fdGotRateKind = fdGotRateKind;
            this.fdOnOfftype = fdOnOfftype;
            this.fdCanUse = fdCanUse;
            this.fdNotDeleteWhenExpired = fdNotDeleteWhenExpired;
            this.fdCanStack = fdCanStack;
            this.fdNewMark = fdNewMark;
            this.fdCoupleItemType = fdCoupleItemType;
            this.fdShowShop = fdShowShop;
            this.fdPurchasable = fdPurchasable;
            this.fdShowDetailImage = fdShowDetailImage;
            this.fdDesc = fdDesc;
            this.fdRefundable = fdRefundable;
            this.fdEtc = fdEtc;
            this.fdLevel = fdLevel;
            this.fdFixedExpireDate = fdFixedExpireDate;
        }

        /// <summary>
        /// 正式构造函数
        /// </summary>
        /// <param name="text">单行文本</param>
        public TblAvatarItemDescClass(string text)
        {
            // 分割文本
            string[] texts = StringDivideClass.StringDivide(text);

            // 录入数据
            this.fdItemNum = uint.Parse(texts[0]); //道具编号
            this.fdBillingItemID = ushort.Parse(texts[1]); //盈利编号
            this.fdType = ushort.Parse(texts[2]); //道具种类
            this.fdCharacter = ushort.Parse(texts[3]); //限定角色
            this.fdPosition = uint.Parse(texts[4]); //道具部位
            this.fdItemKind = uint.Parse(texts[5]); //道具部位编号
            this.fdGameMoney = uint.Parse(texts[6]); //TR
            this.fdFarmPoint = uint.Parse(texts[7]); //农场点
            this.fdCash = uint.Parse(texts[8]); //点券
            this.fdGuildPoint = uint.Parse(texts[9]); //全体公会点数
            this.fdContributionPoint = uint.Parse(texts[10]); //个人公会点数
            this.fdItemName = texts[11]; //道具名称
            this.fdItemCategory = uint.Parse(texts[12]); //道具页签
            this.fdGotRateKind = ushort.Parse(texts[13]); //卡片等级
            this.fdOnOfftype = ushort.Parse(texts[14]); //开关类型
            this.fdCanUse = bool.Parse(texts[15]); //是否启用使用按钮
            this.fdNotDeleteWhenExpired = bool.Parse(texts[16]); //是否过期删除
            this.fdCanStack = bool.Parse(texts[17]); //是否可累计
            this.fdNewMark = bool.Parse(texts[18]); //是否标新
            this.fdCoupleItemType = ushort.Parse(texts[19]); //情侣道具种类
            this.fdShowShop = bool.Parse(texts[20]); //是否展示商店
            this.fdPurchasable = bool.Parse(texts[21]); //是否可购买
            this.fdShowDetailImage = bool.Parse(texts[22]); //是否展示细节图片
            this.fdDesc = texts[23]; //道具描述
            this.fdRefundable = bool.Parse(texts[24]); //是否可回收
            this.fdEtc = uint.Parse(texts[25]); //其他数据
            this.fdLevel = ushort.Parse(texts[26]); //道具等级，体现在开箱界面
            this.fdFixedExpireDate = texts[27]; //过期时间
        }
    }
}