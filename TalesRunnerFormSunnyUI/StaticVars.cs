using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalesRunnerFormSunnyUI.Data
{
    public static class StaticVars
    {
        #region Keys
        /// <summary>
        /// 第二种解包方式的密钥
        /// </summary>
        internal static readonly string[] keys2 = {
            "default", // 默认无文本加密
            "8913899DAED9410CA8C3ABB2D16102DD" ,
            "D55EA427E57F4914A68FF2466B8366C4",
            "FF4080A5FED649978D94A0031EFFCE3D",
            "A73FF15DBE914FCC880037FB32E4D314",
            "4ADEC4FE0A8B4E28964A508B229F341A",
            "420831A87C5F4A1998E8A6146C16586B",
            "BAEEE46EFC2D4E60A08DF99C7ED8B93A",
            "4BA1ACFBCDCC4EF8A6F237B83201F2BE",
            "5375AED5A9564292A6CD6552E37129EB",
            "37720E5F97AF456fADEF817FE7EA14BB"
        };

        /// <summary>
        /// 第一种解包方式的密钥
        /// </summary>
        internal static readonly byte[] aesKey1 =
        {
            0x0D, 0x68, 0x07, 0x6F, 0x0A, 0x09, 0x07, 0x6C, 0x65, 0x73,
            0x0D, 0x75, 0x6E, 0x0A, 0x65, 0x0D
        };
        /// <summary>
        /// 第一种解包方式的密钥
        /// </summary>
        internal static readonly byte[] xorKey1 =
        {
            0x05, 0x5B, 0xCB, 0x64, 0xFB, 0xC2, 0xCE, 0xB4, 0x77, 0x8B,
            0x1B, 0xB8, 0xE9, 0xB5, 0x9C, 0xC6
        };
        #endregion

        #region 各类变量
        /// <summary>
        /// 道具（非装备）收藏等级
        /// </summary>
        internal static readonly string[] Level =
{
            // 长度8
            // 收藏等级
            "No",
            "普通LV1",
            "普通LV2",
            "稀有",
            "独特",
            "传说",
            "威望",
            "神话"
        };

        /// <summary>
        /// 装备部位名称，index即position值
        /// </summary>
        internal static readonly string[] Position =
        {
            // 长度15
            "character", //0
            "head",
            "topbody",
            "downbody",
            "foot",
            "acchead",
            "accface",
            "acchand",
            "accback",
            "accneck",
            "pet",
            "expansion",
            "accwrist",
            "accbooster",
            "acctail" //14
        };

        internal static readonly string[] PositionName =
{
            // 长度
            "角色", //0
            "发型",
            "上衣",
            "下衣",
            "鞋子",
            "头饰",
            "面饰",
            "手饰",
            "背饰",
            "颈饰",
            "宠物",
            "特殊",
            "手腕",
            "推进器",
            "尾巴",//14
            "套装",
            "变身",
            "合作角色",
            "项链",
            "称号",
            "象征",
            "动作",
            "道具",
            "游戏",
            "活动",
            "材料"
        };

        internal static readonly int[] PositionNum2 =
        {
            1001,
            2001,
            2002,
            2003,
            2004,
            3001,
            3002,
            3004,
            3005,
            3003,
            4001,
            1003,
            3008,
            3009,
            3010,//14
            9999,
            1002,
            1004,
            3007,
            5006,
            5010,
            1005,
            1006,
            5002,
            5009,
            9099
        };

        internal static readonly string[] NecessaryFolder =
        {
            "guiex\\itemimage"
        };

        internal static readonly string[] NecessaryText =
        {
            "clientiteminfo\\tblavataritemdesc",
            "clientiteminfo\\tblavataritemdescex",
            "tblluckybagresultitems_1",
            "clientiteminfo\\tblavataritemdescattr",
            "content\\alchemist\\essenalchemistmixcondition",
            "clientiteminfo\\tblavataritemsetdesc",
            "clientiteminfo\\tblavataritemsetattr",
            "archives\\essenarchives_exchangelist",
            "clientiteminfo\\essenitemcubereward",
            "clientiteminfo\\settingitemcubelist",
            "content\\fishing\\essenfishing_decoy",
            "content\\trading\\tradingitemrate_ver2",
            "collectionrenewal\\essenitem_collectionrenewal_itemlist",
            "collectionrenewal\\essenitem_collectionrenewal_itemrank",
            "collectionrenewal\\essenitem_collectionrenewal_group",
            "content\\statsystem\\essenstatsystemnodeinfo",
            "clientiteminfo\\essenexchangesysteminfo",
            "clientiteminfo\\essenexchangesystemconsumelist",
            "clientiteminfo\\selectivepackage\\essenselectiveboxreward",
            "clientiteminfo\\selectivepackage\\essenselectivepackagebonusreward",
            "clientiteminfo\\selectivepackage\\essenselectivepackageboxitem",
            "clientiteminfo\\essenitemcollectiondesc",
            "clientiteminfo\\tblalchemist_recipe_mix_condition",
            "clientiteminfo\\tblalchemist_recipe",
            "clientiteminfo\\enchantsystem\\essenenchantsystemitem",
            "clientiteminfo\\enchantsystem\\essenenchantsystemstonemount"
        };

        internal static readonly string[] LocalText =
        {
            "itemdata",
            "itemdata2",
            "itemsetdata",
            "itemattr",
            "itemoccupation",
            "itemtranslation"
        };
        #endregion

        #region 属性区域
        internal static readonly string[] CardPack =
        {
            "套装所属",
            "四人同心",
            "亨利城的训练",
            "阿里巴巴",
            "彼得潘",
            "数学王系列",
            "地狱火之路",
            "冰河世纪",
            "甜蜜的饼干树林",
            "魔笛",
            "青蛙王子",
            "被遗忘的天使神殿"
        };

        internal static readonly string[] PetSeries =
        {
            "A型",
            "B型",
            "O型",
            "AB型",
            "GirlsDay",
            "GirlFriend",
            "SEVENTEEN",
            "BTS",
            "Astro",
            "神秘公寓",
            "天使",
            "恶魔",
            "塔罗牌",
            "超级跑跑角色",
            "国际象棋"
        };


        #endregion


    }
}
