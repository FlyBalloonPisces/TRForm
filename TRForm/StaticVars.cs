namespace TalesRunnerForm
{
    public static class StaticVars
    {
        #region Keys

        internal static readonly byte[][] aesKeys =
        {
            new byte[] {
                
                0xFD, 0xD7, 0x15, 0xCB, 0xBE, 0xBF, 0xA5, 0xFF, 0xEF, 0x9E,
                0xED, 0x97, 0xCE, 0x96, 0xD3, 0x0F, 0x4C, 0xDC, 0xA0, 0x1D,
                0xAF, 0x5F, 0xCF, 0xA2, 0xD8, 0xB1, 0x58, 0x08, 0xB9, 0xB6,
                0xC1, 0x0A
            },
            new byte[] {
                // 港服、泰服
                0xdb, 0x27, 0x0b, 0xbb, 0x82, 0x88, 0xdf, 0xf3, 0x44, 0xee,
                0xef, 0x93, 0x67, 0xd1, 0xb5, 0xc2, 0xb6, 0xda, 0x17, 0x59,
                0x07, 0x75, 0x06, 0x8f, 0x32, 0x4a, 0x9f, 0x29, 0x49, 0x52,
                0x32, 0xc2
            },
            new byte[] {
                // 韩服
                0xc, 0xf6, 0xaf, 0xd5, 0x0, 0x48, 0xfe, 0x99, 0xe1, 0xab,
                0xf9, 0xb6, 0x70, 0x68, 0xad, 0xcd, 0x28, 0x3, 0x8a, 0x6d,
                0x16, 0x85, 0xe3, 0x7b, 0xeb, 0x9, 0xb, 0x48, 0x4f, 0xb1,
                0x7e, 0x3
            }
        };
        internal static readonly byte[][] xorKeys =
{
            new byte[] {
                0x20, 0x44, 0xB2, 0xA3, 0x63, 0xC7, 0x47, 0x88, 0x4D, 0x1E,
                0x2F, 0x12, 0x90, 0x39, 0x3C, 0x8E
            },
            new byte[] {
                0x1c, 0x67, 0x5b, 0xd4, 0x5b, 0x4a, 0x46, 0x74, 0x31, 0x7,
                0x4b, 0x82, 0xab, 0x3f, 0x55, 0xfd
            },
            new byte[] {
                0x7c, 0x82, 0x37, 0xd5, 0x2c, 0xf8, 0x81, 0x9, 0x4d, 0x76, 
                0x5, 0xf5, 0xe5, 0x47, 0xe8, 0xdf
            }
        };
        internal static readonly byte[] aesKey1 =
        {
            0x0D, 0x68, 0x07, 0x6F, 0x0A, 0x09, 0x07, 0x6C, 0x65, 0x73,
            0x0D, 0x75, 0x6E, 0x0A, 0x65, 0x0D
        };
        internal static readonly byte[] xorKey1 =
        {
            0x05, 0x5B, 0xCB, 0x64, 0xFB, 0xC2, 0xCE, 0xB4, 0x77, 0x8B,
            0x1B, 0xB8, 0xE9, 0xB5, 0x9C, 0xC6
        };
        #endregion

        #region TrData常量数组

        internal const int PerPage = 30; // 开箱页每页16个箱子

        // 数组控制用常量
        // TODO 人物数的自动读取
        internal const int Characters_kr = 30; // 30个常规角色
        internal const int Characters_hk = 29; // 29个常规角色
        internal const int Positions = 14; // 14个常规部位

        internal static readonly string[] CharName =
        {
            "光光",
            "明明",
            "莉娜",
            "大熊",
            "蒂蒂",
            "纳鲁西斯",
            "琪琪",
            "雷",
            "贝尔",
            "凯",
            "雪",
            "深子",
            "亚伯",
            "哈鲁",
            "维拉",
            "孙悟空",
            "隐雷",
            "曦狐",
            "露西",
            "米狐",
            "基纳丽",
            "R",
            "哈朗",
            "拉拉",
            "埃利姆斯",
            "凯恩",
            "缘迕",
            "血腥维拉",
            "修内尔",
            "露露亚"
        };

        internal static readonly int[] CharNum =
        {
            // 角色道具编号
            17, // 光光
            18, // 明明
            34, // 莉娜
            139, // 大熊
            424, // 蒂蒂
            1102, // 纳鲁西斯
            2354, // 琪琪
            4762, // 雷
            7130, // 贝儿
            8351, // 凯
            21205, // 雪
            21453, // 深子
            24097, // 亚伯
            25003, // 哈鲁
            28281, // 维拉
            30018, // 孙悟空
            35914, // 隐雷
            37033, // 曦狐
            46299, // 露西
            49147, // 米狐
            52139, // 基纳丽
            52386, // R
            77385, // 哈朗
            80404, // 拉拉
            83403, // 埃利姆斯
            83404, // 凯恩
            85507, // 缘迕
            91242, // 血腥维拉
            94708, // 修内尔
            96962  // 露露亚
        };

        internal static readonly string[] Level =
        {
            // 长度7
            // 收藏等级
            "No",
            "C",
            "B",
            "A",
            "S",
            "SS",
            "SSS"
        };

        internal static readonly int[] Limit212 =
        {
            // 长度15
            // 212属性限制人数
            15,
            5,
            10,
            5,
            10,
            10,
            10,
            10,
            10,
            10,
            10,
            10,
            10,
            10,
            10
        };

        internal static readonly string[] Character =
        {
            // 长度50
            // 全角色通用
            "all_",
            // 普通角色
            "cw_",
            "mm_",
            "rn_",
            "bb_",
            "dn_",
            "ns_",
            "mk_",
            "rg_",
            "bd_",
            "ka_",
            "yk_",
            "kr_",
            "ab_",
            "hr_",
            "vr_",
            "og_",
            "rg2_",
            "sh_",
            "lc_",
            "mh_",
            "kn_",
            "r_",
            "ha_",
            "la_",
            "el_",
            "ca_",
            "gb_",
            "vrd_",
            "xn_",
            "rl_",
            // 合作角色
            "k1_",
            "k2_",
            "k3_",
            "k4_",
            "k5_",
            "am_",
            "gi_",
            "do_",
            "ke_",
            "ta_",
            "ku_",
            "m1_",
            "m2_",
            "m3_",
            "m4_",
            "bk1_",
            "bk2_",
            "bk3_",
            "bk4_",
            "bk5_",
            "bk6_",
            "bk7_"
        };

        internal static readonly string[] Position =
        {
            // 长度15
            "character_",
            "head_",
            "topbody_",
            "downbody_",
            "foot_",
            "acchead_",
            "accface_",
            "acchand_",
            "accback_",
            "accneck_",
            "pet_",
            "expansion_",
            "accwrist_",
            "accbooster_",
            "acctail_"
        };

        internal static readonly string[] Server =
        {
            "kr",
            "hk",
            "th"
        };

        #endregion

        #region Unpack常量数组
        internal static readonly string[] tr_pkg_kr =
        {
            "9",
            "13",
            "14",
            "15"
        };

        internal static readonly string[] positionNames =
        {
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

        internal static readonly int[] CharacterPkg =
        {
            // 长度31
            // TODO 角色自动读取，下拉框
            0, // 预留位，pkgUnpack的调用语句从1开始
            // 普通角色
            1, // 光光
            2, // 明明
            3, // 丽娜
            4, // 大熊
            5, // Dnd
            6, // 纳鲁西斯
            7, // 琪琪
            8, // 雷
            9, // 贝儿
            10, // 凯
            11, // 雪
            12, // 深子
            13, // 亚伯
            14, // 哈鲁
            15, // 维拉
            16, // 孙悟空
            17, // 隐雷
            18, // 曦狐
            19, // 露西
            20, // 米狐
            21, // 基纳丽
            22, // R
            23, // 哈朗
            24, // 啦啦
            25, // 埃利姆斯
            26, // 凯恩
            27, // 缘迕
            15, // 血腥维拉
            29, // 修内尔
            30  // 露露亚
        };
        #endregion
    }
}
