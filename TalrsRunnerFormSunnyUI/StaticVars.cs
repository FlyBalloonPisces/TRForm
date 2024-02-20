using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalrsRunnerFormSunnyUI.Data
{
    public static class StaticVars
    {
        #region Keys
        /// <summary>
        /// 第二种解包方式的密钥
        /// </summary>
        internal static readonly string[] keys2 = {
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
            "character",
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
            "acctail"
        };

        #endregion


    }
}
