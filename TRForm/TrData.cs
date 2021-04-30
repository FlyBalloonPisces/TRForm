using DevIL;
using DevIL.Unmanaged;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using TalesRunnerForm.Properties;
using Image = DevIL.Image;

namespace TalesRunnerForm
{
    public static class TrData
    {
        #region 数值初始化

        /// <summary>
        /// 对于TrData单元部分数值进行初始化
        /// </summary>
        internal static void InitData()
        {
            for (int i = 0; i < StaticVars.Positions; i++)
            {
                P1[i] = -1;
                P2[i] = -1;
                if (i < P3.Length)
                {
                    P3[i] = -1;
                }
            }

            P3[0] = Item.FindIndex(item => item.ItemNum.Equals(StaticVars.CharNum[0]));
            InitDevIl();
        }

        #endregion

        #region 初始变量和存放单元

        private static bool _showCn = true; // 是否展示中文 初始值为True
        private static bool _charOc = true; // 是否读取占用 初始值为True
        private static int _boxPage; // 开箱当前页面 初始值为0
        private static int _charNow; // 当前配装角色
        private static int _topSpd; // 配装最高速度数值
        private static int _acce; // 配装加速度数值
        private static int _pow; // 配装力数值
        private static int _ctrl; // 配装控制数值
        private static int _lv; // 配装等级数值        
        private static bool _check1; // 内装勾选
        private static bool _check2; // 外装勾选
        private static readonly List<int> ListResult = new List<int>();
        private static readonly SortedList<int, List<Attr>> List212 = new SortedList<int, List<Attr>>(); //212属性用列表
        private static List<ListViewItem> _listViewItems = new List<ListViewItem>();
        private static bool _search; // 是否进行了条件搜索 初始值为false

        // 确定程序路径 进行文本读取
        public static readonly string Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        public static bool keysVersion = true; // 密匙版本：true韩服；false港服泰服
        public static int Characters = 0; // 配装角色数量
        // ReSharper disable once AssignNullToNotNullAttribute
        //private static readonly DirectoryInfo PathExe = new DirectoryInfo(Application.StartupPath); // exe目录
        //private static readonly string PathPkg = PathExe.Parent?.FullName; // 游戏目录
        private static string PathPkg = ""; // 游戏目录

        // 装备列表
        /// <summary>
        /// 展示道具列表,ItemStatus
        /// </summary>
        private static readonly List<ItemStatus> Item = new List<ItemStatus>();
        /// <summary>
        /// 非展示道具列表,ItemStatus2
        /// </summary>
        private static readonly List<ItemStatus2> ItemRest = new List<ItemStatus2>();
        /// <summary>
        /// 套装列表,ItemSetStatus
        /// </summary>
        private static readonly List<ItemSetStatus> ItemSet = new List<ItemSetStatus>();
        /// <summary>
        /// 宝石列表,ItemStatus
        /// </summary>
        private static readonly List<ItemStatus> ItemStone = new List<ItemStatus>();
        /// <summary>
        /// 箱子列表,ItemBoxStatus
        /// </summary>
        private static readonly List<ItemBoxStatus> ItemBox = new List<ItemBoxStatus>();

        // 道具属性列表 文本文件生成日期 图片路径
        private static readonly SortedList<int, AttrInfo> ItemAttrInfo = new SortedList<int, AttrInfo>();
        private static string _dataVer = string.Empty;
        private static ImageImporter _mImporter;

        private static Image _mActiveImage;

        // private static int _iOld = -1;
        private static readonly StringBuilder Itemdesc = new StringBuilder(byte.MaxValue);
        private static readonly StringBuilder Imagepath = new StringBuilder(byte.MaxValue);
        private static readonly StringBuilder Modelpath = new StringBuilder(byte.MaxValue);

        // 配装用变量
        private static readonly int[] P1 = new int[StaticVars.Positions]; // 存放的是索引

        //private static readonly string[] P1_stone = new string[Positions]; // 存放的是宝石
        private static readonly int[] P2 = new int[StaticVars.Positions]; // 存放的是索引

        //private static readonly string[] P2_stone = new string[Positions]; // 存放的是宝石
        private static readonly int[] P3 = new int[4]; // 存放的是索引
        private static readonly SortedList<int, string> IndexStone = new SortedList<int, string>(); // 存放装备和佩戴宝石对应关系
        private static readonly SortedList<int, float> AttrSet = new SortedList<int, float>(); // 存放计算属性的套装的列表
        private static readonly SortedList<int, float> AttrItem = new SortedList<int, float>();
        private static readonly SortedList<int, float> AttrStone = new SortedList<int, float>();

        #endregion

        #region 道具数据结构

        /// <summary>
        /// 需要展示的属性文本
        /// </summary>
        private class AttrInfo
        {
            public string Desc = string.Empty;
            public string Desc2 = string.Empty;
            public string Desc3 = string.Empty;
            public string Desc4 = string.Empty;
            public float Type;
        }

        /// <summary>
        /// 简单的道具信息
        /// </summary>
        private class ItemStatus2
        {
            // 非装备用
            public int ItemNum;
            public string Name = string.Empty;
            public string NameCh = "(noname)";

            public short PkgNum = -1; // 图片文件所在pkg -1代表无图片
            public long PicOffset = -1;

            /// <summary>
            /// 进行特定语言下装备名称的返回
            /// </summary>
            /// <returns>装备对应语言名称</returns>
            public string GetName()
            {
                if (_showCn)
                {
                    return string.CompareOrdinal(NameCh, "(noname)") == 0 ? Name : NameCh;
                }

                return Name;
            }
        }

        /// <summary>
        /// 道具箱子信息
        /// </summary>
        private class ItemBoxStatus
        {
            public List<float> WeightBasic = new List<float>(); // 基本 存放的是编号，箱子可以开到的道具的权重
            public List<int> MemberBasic = new List<int>(); // 基本 存放的是编号，箱子可以开到的道具
            public List<float> WeightSilver = new List<float>(); // 魔方银色 存放的是编号，箱子可以开到的道具的权重
            public List<int> MemberSilver = new List<int>(); // 魔方银色 存放的是编号，箱子可以开到的道具
            public List<float> WeightGold = new List<float>(); // 魔方金色 存放的是编号，箱子可以开到的道具的权重
            public List<int> MemberGold = new List<int>(); // 魔方金色 存放的是编号，箱子可以开到的道具
            public int BoxNum; // 存放的是编号
            public float WeightTotalBasic; // 基本 总权重
            public float WeightTotalSilver; // 魔方银色 总权重
            public float WeightTotalGold; // 魔方金色 总权重
            public int Position; // 道具位置编号
            public float SilverRate; // 魔方银色概率
        }

        /// <summary>
        /// 套装属性信息
        /// </summary>
        private class SetAttr
        {
            public ushort CompleteKey; // 利用二进制来表示需要的部位
            public int Num;
            public float Value;

            private string Get5000Map()
            {
                switch ((int)((Value + 2) / 3))
                {
                    case 1:
                        return Resources.CardPackMap5;
                    case 2:
                        return Resources.CardPackMap3;
                    case 3:
                        return Resources.CardPackMap4;
                    case 4:
                        return Resources.CardPackMap2;
                    case 5:
                        return Resources.CardPackMap1;
                    case 6:
                        return Resources.CardPackMap6;
                    case 7:
                        return Resources.CardPackMap7;
                    case 8:
                        return Resources.CardPackMap9;
                    case 9:
                        return Resources.CardPackMap8;
                    case 10:
                        return Resources.CardPackMap10;
                    case 11:
                        return Resources.CardPackMap11;
                    default:
                        return Resources.CardPackMap_Other;
                }
            }

            private List<Attr> Get5000Group()
            {
                List<Attr> list1 = new List<Attr>
                {
                    new Attr {Num = 2, Value = 0.5F}, new Attr {Num = 2, Value = 1.2F}, new Attr {Num = 2, Value = 0.3F}
                };
                List<Attr> list2 = new List<Attr>
                {
                    new Attr {Num = 2, Value = 0.3F}, new Attr {Num = 2, Value = 1.2F}, new Attr {Num = 2, Value = 0.5F}
                };
                List<Attr> list3 = new List<Attr>
                {
                    new Attr {Num = 2, Value = 0.3F}, new Attr {Num = 2, Value = 0.5F}, new Attr {Num = 2, Value = 1.2F}
                };
                switch ((int)((Value + 2) / 3))
                {
                    case 1:
                        return list1;
                    case 2:
                        return list2;
                    case 3:
                        return list3;
                    case 4:
                    case 5:
                        return list2;
                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                        return list3;
                    default:
                        return null;
                }
            }

            private Attr Get5000Attr()
            {
                switch ((int)((Value + 2) % 3))
                {
                    case 0:
                        return Get5000Group()[0];
                    case 1:
                        return Get5000Group()[0];
                    case 2:
                        return Get5000Group()[0];
                    default:
                        return null;
                }
            }

            public override string ToString()
            {
                string str = string.Empty;
                if (Num == 5000)
                {
                    str += ItemAttrInfo[Num].Desc;
                    str += Get5000Map();
                    str += ItemAttrInfo[Num].Desc2;

                    Attr temp = Get5000Attr();
                    str += ItemAttrInfo[temp.Num].Desc;
                    if (ItemAttrInfo[temp.Num].Type == 100)
                    {
                        str += Multi(temp.Value, 100f).ToString() + '%';
                    }
                    else if (ItemAttrInfo[Num].Type != 0)
                    {
                        str += Multi(temp.Value, ItemAttrInfo[Num].Type).ToString();
                    }

                    str += ItemAttrInfo[temp.Num].Desc2;
                }
                else
                {
                    str += ItemAttrInfo[Num].Desc;
                    if (ItemAttrInfo[Num].Type == 100)
                    {
                        str += Multi(Value, 100f).ToString() + '%';
                    }
                    else if (ItemAttrInfo[Num].Type != 0)
                    {
                        str += Multi(Value, ItemAttrInfo[Num].Type).ToString();
                    }

                    str += ItemAttrInfo[Num].Desc2;
                }

                str += "\r\n";
                return str;
            }
        }

        /// <summary>
        /// 道具套装信息
        /// </summary>
        private class ItemSetStatus
        {
            public List<SetAttr> Attr = new List<SetAttr>();
            public List<int> Member = new List<int>(); // 保存同套装道具编号
            public byte Avatar;
            public int SetNum;
            public byte Count;
            public string Name = string.Empty;
            public string NameCh = "(noname)";

            /// <summary>
            /// 进行特定语言下装备名称的返回
            /// </summary>
            /// <returns>装备对应语言名称</returns>
            // ReSharper disable once UnusedMember.Local
            public string GetName()
            {
                if (!_showCn) return Name;
                return string.CompareOrdinal(NameCh, "(noname)") == 0 ? Name : NameCh;
            }

            public string GetAvatar()
            {
                return Avatar == 0 ? Resources.Attr_AvatarOff : Resources.Attr_AvatarOn;
            }
        }

        /// <summary>
        /// 道具属性信息
        /// </summary>
        private class Attr
        {
            public int Num;
            public float Value;

            private string Get212()
            {
                int p = (int)Value;
                switch (p)
                {
                    case 1:
                        return Resources.PetSeries1;
                    case 2:
                        return Resources.PetSeries2;
                    case 3:
                        return Resources.PetSeries3;
                    case 4:
                        return Resources.PetSeries4;
                    case 5:
                        return Resources.PetSeries5;
                    case 6:
                        return Resources.PetSeries6;
                    case 7:
                        return Resources.PetSeries7;
                    case 8:
                        return Resources.PetSeries8;
                    case 9:
                        return Resources.PetSeries9;
                    case 10:
                        return Resources.PetSeries10;
                    case 11:
                        return Resources.PetSeries11;
                    case 12:
                        return Resources.PetSeries12;
                    case 13:
                        return Resources.PetSeries13;
                    case 14:
                        return Resources.PetSeries14;
                    case 15:
                        return Resources.PetSeries15;
                    default:
                        return Resources.PetSeries_Other;
                }
            }

            public override string ToString()
            {
                string str = string.Empty;
                if (Num == 212)
                {
                    if (List212.ContainsKey((int)Value))
                    {
                        int value = (int)Value;
                        str += ItemAttrInfo[Num].Desc;
                        str += Get212();
                        str += ItemAttrInfo[Num].Desc2;
                        foreach (var u in List212[value])
                        {
                            str += ItemAttrInfo[u.Num].Desc;
                            if (ItemAttrInfo[u.Num].Type == 100)
                            {
                                str += Multi(u.Value, 100f).ToString() + '%';
                            }
                            else
                            {
                                str += u.Value;
                            }

                            str += ItemAttrInfo[u.Num].Desc2;
                            str += "，";
                        }

                        str = Regex.Replace(str, @"[，]$", "");
                        str += ItemAttrInfo[Num].Desc3;
                        str += StaticVars.Limit212[value - 1];
                        str += ItemAttrInfo[Num].Desc4;
                    }
                }
                else
                {
                    str += ItemAttrInfo[Num].Desc;
                    if (ItemAttrInfo[Num].Type == 100)
                    {
                        str += Multi(Value, 100f).ToString() + '%';
                    }
                    else if (ItemAttrInfo[Num].Type != 0)
                    {
                        str += Multi(Value, ItemAttrInfo[Num].Type).ToString();
                    }

                    str += ItemAttrInfo[Num].Desc2;
                }

                str += "\r\n";
                return str;
            }
        }

        /// <summary>
        /// 道具信息
        /// </summary>
        private class ItemStatus
        {
            public List<Attr> Attr = new List<Attr>();
            public byte Avatar; // 0为内装 1为外装
            public byte Char; // 合作角色编号超过255的情况需要考虑更换数据类型

            public int Id; // 收藏编号

            // public ushort ItemKind; // 道具图片编号
            public int ItemNum;
            public byte Level; // 收藏等级
            public string Name = string.Empty;
            public string NameCh = "(noname)";

            public byte Point;

            // public int Position;
            public int Position2; // 根据页签设置装备位置
            public int SetNum;
            public short PkgNum = -1; // 图片文件所在pkg -1代表无图片
            public long PicOffset = -1; // 图片文件流偏移 -1代表无图片
            public byte Sex; // 装备性别 1男2女
            public short Occupation; // 装备占用位置 -1代表无占用
            public int Chars = -1; // 可以使用该装备的角色 -1代表全部可以使用
            public int Slot; // 宝石槽相关 每4个2进制位代表1个槽

            /// <summary>
            /// 进行特定语言下装备名称的返回
            /// </summary>
            /// <returns>装备对应语言名称</returns>
            public string GetName()
            {
                if (_showCn)
                {
                    if (string.CompareOrdinal(NameCh, "(noname)") == 0)
                    {
                        return Name;
                    }

                    return NameCh;
                }

                return Name;
            }

            /// <summary>
            /// 进行角色中文名称的返回
            /// </summary>
            /// <returns>角色中文名称</returns>
            public string GetChar()
            {
                return GetChar(Char);
            }

            public static string GetChar(int c)
            {
                switch (c)
                {
                    case 1:
                        return Resources.Character1;
                    case 2:
                        return Resources.Character2;
                    case 3:
                        return Resources.Character3;
                    case 4:
                        return Resources.Character4;
                    case 5:
                        return Resources.Character5;
                    case 6:
                        return Resources.Character6;
                    case 7:
                        return Resources.Character7;
                    case 8:
                        return Resources.Character8;
                    case 9:
                        return Resources.Character9;
                    case 10:
                        return Resources.Character10;
                    case 11:
                        return Resources.Character11;
                    case 12:
                        return Resources.Character12;
                    case 13:
                        return Resources.Character13;
                    case 14:
                        return Resources.Character14;
                    case 15:
                        return Resources.Character15;
                    case 16:
                        return Resources.Character16;
                    case 17:
                        return Resources.Character17;
                    case 18:
                        return Resources.Character18;
                    case 19:
                        return Resources.Character19;
                    case 20:
                        return Resources.Character20;
                    case 21:
                        return Resources.Character21;
                    case 22:
                        return Resources.Character22;
                    case 23:
                        return Resources.Character23;
                    case 24:
                        return Resources.Character24;
                    case 25:
                        return Resources.Character25;
                    case 26:
                        return Resources.Character26;
                    case 27:
                        return Resources.Character27;
                    case 28:
                        return Resources.Character28;
                    case 201:
                        return Resources.Character201;
                    case 202:
                        return Resources.Character202;
                    case 203:
                        return Resources.Character203;
                    case 204:
                        return Resources.Character204;
                    case 205:
                        return Resources.Character205;
                    case 206:
                        return Resources.Character206;
                    case 207:
                        return Resources.Character207;
                    case 208:
                        return Resources.Character208;
                    case 209:
                        return Resources.Character209;
                    case 210:
                        return Resources.Character210;
                    case 211:
                        return Resources.Character211;
                    case 212:
                        return Resources.Character212;
                    case 213:
                        return Resources.Character213;
                    case 214:
                        return Resources.Character214;
                    case 215:
                        return Resources.Character215;
                    case 216:
                        return Resources.Character216;
                    case 217:
                        return Resources.Character217;
                    case 218:
                        return Resources.Character218;
                    case 219:
                        return Resources.Character219;
                    case 220:
                        return Resources.Character220;
                    case 221:
                        return Resources.Character221;
                    case 222:
                        return Resources.Character222;
                    default:
                        return Resources.Character_Other;
                }
            }

            /// <summary>
            /// 进行角色中文名称的返回
            /// </summary>
            /// <returns>角色中文名称</returns>
            public string GetSex()
            {
                switch (Sex)
                {
                    case 1:
                        return Resources.Sex_Male;
                    case 2:
                        return Resources.Sex_Female;
                    case 3:
                        return Resources.Sex_Part;
                    default:
                        return Resources.Sex_Other;
                }
            }

            /// <summary>
            /// 获取部位中文名称
            /// </summary>
            /// <returns>部位中文名称</returns>
            public string GetPosition()
            {
                return GetPosition(Position2);
            }

            /// <summary>
            /// 获取部位中文名称
            /// </summary>
            /// <param name="p">部位的二进制位或栏位</param>
            /// <returns>部位中文名称</returns>
            public static string GetPosition(int p)
            {
                switch (p)
                {
                    case 1:
                        return Resources.Position_2001;
                    case 2:
                        return Resources.Position_2002;
                    case 3:
                        return Resources.Position_2003;
                    case 4:
                        return Resources.Position_2004;
                    case 5:
                        return Resources.Position_3001;
                    case 6:
                        return Resources.Position_3002;
                    case 7:
                        return Resources.Position_3004;
                    case 8:
                        return Resources.Position_3005;
                    case 9:
                        return Resources.Position_3003;
                    case 10:
                        return Resources.Position_4001;
                    case 11:
                        return Resources.Position_1003;
                    case 12:
                        return Resources.Position_3008;
                    case 13:
                        return Resources.Position_3009;
                    case 14:
                        return Resources.Position_3010;
                    case 1001:
                        return Resources.Position_1001;
                    case 1002:
                        return Resources.Position_1002;
                    case 1003:
                        return Resources.Position_1003;
                    case 1004:
                        return Resources.Position_1004;
                    case 2001:
                        return Resources.Position_2001;
                    case 2002:
                        return Resources.Position_2002;
                    case 2003:
                        return Resources.Position_2003;
                    case 2004:
                        return Resources.Position_2004;
                    case 3001:
                        return Resources.Position_3001;
                    case 3002:
                        return Resources.Position_3002;
                    case 3003:
                        return Resources.Position_3003;
                    case 3004:
                        return Resources.Position_3004;
                    case 3005:
                        return Resources.Position_3005;
                    case 3007:
                        return Resources.Position_3007;
                    case 3008:
                        return Resources.Position_3008;
                    case 3009:
                        return Resources.Position_3009;
                    case 3010:
                        return Resources.Position_3010;
                    case 4001:
                        return Resources.Position_4001;
                    case 5006:
                        return Resources.Position_5006;
                    case 5010:
                        return Resources.Position_5010;
                    case 9999:
                        return Resources.Position_Set;
                    default:
                        return Resources.Position_Other;
                }
            }

            public string GetAvatar()
            {
                return Avatar == 0 ? Resources.Attr_AvatarOff : Resources.Attr_AvatarOn;
            }
        }

        #endregion

        #region DevIL

        /// <summary>
        /// 图片读取初始化
        /// </summary>
        private static void InitDevIl()
        {
            _mImporter = new ImageImporter();
            new ImageState
            {
                AbsoluteFormat = DataFormat.BGRA,
                AbsoluteDataType = DataType.UnsignedByte,
                AbsoluteOrigin = OriginLocation.UpperLeft
            }.Apply();
            new CompressionState { KeepDxtcData = true }.Apply();
            new SaveState { OverwriteExistingFile = true }.Apply();
        }

        /// <summary>
        /// 返回指定图片的Bitmap
        /// </summary>
        /// <param name="offset">指定图片的文件偏移量</param>
        internal static System.Drawing.Image ShowDds(long offset, short pkgNum)
        {
            if (offset < 0)
            {
                return null;
            }

            byte[] input = PkgUnpack.PicFind(PathPkg, offset, pkgNum);
            try
            {
                using (MemoryStream ms = new MemoryStream(input))
                {
                    _mActiveImage = _mImporter.LoadImageFromStream(ImageType.Dds, ms);
                }
            }
            catch
            {
                // int num3 = (int) MessageBox.Show("Failed to read \"" + input + "\".", "Error",
                //     MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return null;
            }

            _mActiveImage.Bind();
            ImageInfo imageInfo = IL.GetImageInfo();
            Bitmap bitmap = new Bitmap(imageInfo.Width, imageInfo.Height,
                PixelFormat.Format32bppArgb);
            Rectangle rect = new Rectangle(0, 0, imageInfo.Width, imageInfo.Height);
            BitmapData bitmapdata =
                bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            IL.CopyPixels(0, 0, 0, imageInfo.Width, imageInfo.Height, 1, DataFormat.BGRA,
                DataType.UnsignedByte, bitmapdata.Scan0);
            bitmap.UnlockBits(bitmapdata);
            _mActiveImage.Dispose();
            // int num4 = bitmap.Width;
            // int num5 = bitmap.Height;
            // if (bitmap.Width > Screen.PrimaryScreen.WorkingArea.Width ||
            //     bitmap.Height > Screen.PrimaryScreen.WorkingArea.Height)
            // {
            //     if (bitmap.Width / bitmap.Height > Screen.PrimaryScreen.WorkingArea.Width /
            //         Screen.PrimaryScreen.WorkingArea.Height)
            //     {
            //         num4 = (int) ((double) Screen.PrimaryScreen.WorkingArea.Width * 0.8);
            //         num5 = (int) ((double) bitmap.Height / (double) bitmap.Width * (double) num4);
            //     }
            //     else
            //     {
            //         num5 = (int) ((double) Screen.PrimaryScreen.WorkingArea.Height * 0.8);
            //         num4 = (int) ((double) bitmap.Width / (double) bitmap.Height * (double) num5);
            //     }
            // }

            return bitmap;
        }

        #endregion

        #region LoadForm用道具数据结构

        /// <summary>
        /// 道具箱子信息
        /// </summary>
        private class ItemBoxStatusMaking
        {
            //public List<int> WeightBasic = new List<int>(); // 箱子可以开到的道具的权重
            //public List<int> MemberBasic = new List<int>(); // 箱子可以开到的道具
            public SortedList<int, float> MemWeiBasic = new SortedList<int, float>(); // 基本 道具，权重
            public SortedList<int, float> MemWeiSilver = new SortedList<int, float>(); // 魔方银色 道具，权重
            public SortedList<int, float> MemWeiGold = new SortedList<int, float>(); // 魔方金色 道具，权重
            public int BoxNum;
            public float WeightTotalBasic; // 基本 总权重
            public float WeightTotalSilver; // 魔方银色 总权重
            public float WeightTotalGold; // 魔方金色 总权重
            public int Position; // 道具位置编号
            public float SilverRate; // 魔方银色概率
        }

        /// <summary>
        /// 道具套装信息
        /// </summary>
        private class ItemSetStatusMaking
        {
            public readonly List<SetAttr> Attr = new List<SetAttr>();
            public List<int> Member = new List<int>(); // 保存同套装道具编号
            public byte Avatar;
            public int SetNum;
            public byte Count;
            public string Name = string.Empty;
            public string NameCh = "(noname)";
        }

        /// <summary>
        /// 道具信息
        /// </summary>
        private class ItemStatusMaking
        {
            public bool Demand; // 是否需要写入，是否有箱子能够开到该道具
            public bool Complete; // 生成时是否生成完整的信息，false则只保留编号名称和图片偏移
            public List<Attr> Attr = new List<Attr>();
            public byte Avatar; // 0为内装 1为外装
            public byte Char; // 合作角色编号超过255的情况需要考虑更换数据类型
            public int Id; // 收藏编号
            public ushort ItemKind; // 道具图片编号
            public int ItemNum;
            public byte Level; // 收藏等级
            public string Name = string.Empty;
            public string NameCh = "(noname)";
            public byte Point;
            public int Position;
            public int Position2; // 根据页签设置装备位置
            public int SetNum;
            public short PkgNum = -1; // 图片文件所在pkg -1代表无图片
            public long PicOffset = -1; // 图片文件流偏移 -1代表无图片
            public byte Sex; // 装备性别 1男2女
            public short Occupation; // 装备占用位置 -1代表无占用
            public int Chars = -1; // 可以使用该装备的角色 -1代表全部可以使用
            public int Slot; // 宝石槽相关 每4个2进制位代表1个槽
        }

        #endregion

        #region LoadForm交互

        /// <summary>
        /// 读取游戏文件目录并指定
        /// </summary>
        /// <param name="bw"></param>
        internal static void LoadItemData(BackgroundWorker bw)
        {
            if (!PathPkg.Equals(""))
            {
                if (!GetAttrData())
                {
                    _ = (int)MessageBox.Show(Resources.String_AttrListFailed, Resources.String_Error);
                    Environment.Exit(0);
                }

                if (!GetPicData())
                {
                    _ = (int)MessageBox.Show(Resources.String_PicListFailed, Resources.String_Error);
                    Environment.Exit(0);
                }

                _charOc = GetCharData();

                if (!GetItemList(bw))
                {
                    _ = (int)MessageBox.Show(Resources.String_ItemListFailed, Resources.String_Error);
                    Environment.Exit(0);
                }

                if (!_charOc)
                {
                    _ = (int)MessageBox.Show(Resources.String_OccupationListFailed, Resources.String_Error);
                    //tabPage1_5.Parent = null;
                }
            }
            else
            {
                _ = (int)MessageBox.Show(Resources.String_ItemListFailed, Resources.String_Error);
                Environment.Exit(0);
            }

            bw.ReportProgress(100);
            Thread.Sleep(3000);
        }

        /// <summary>
        /// 读取注册表尝试获取游戏路径，如果没能读取则允许手动指定
        /// </summary>
        /// <returns></returns>
        internal static bool TestItemData()
        {
            if (GetGamePath() && GetPicData())
            {
                return true;
            }
            else
            {
                // 手动指定
                return false;
            }
        }

        /// <summary>
        /// 手动指定游戏目录
        /// </summary>
        /// <returns>游戏目录是否有效</returns>
        private static bool ManualPath()
        {
            DialogResult dr = MessageBox.Show(Resources.String_ItemListFailed, Resources.String_Error, MessageBoxButtons.YesNo);
            if (dr == DialogResult.Yes)
            {
                OpenFileDialog op = new OpenFileDialog
                {
                    DefaultExt = "exe",
                    FileName = "talesrunner.exe",
                    Filter = "超级跑跑程序|talesrunner.exe",
                    InitialDirectory = Path.Replace("$\\", "")
                };
                if (op.ShowDialog() == DialogResult.OK)
                {
                    PathPkg = new FileInfo(op.FileName).DirectoryName;
                }
                op.Dispose();
                GC.Collect();
                if (GetPicData())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取属性文字_itemattr.txt
        /// </summary>
        /// <returns>是否读取</returns>
        private static bool GetAttrData()
        {
            string path = Path + "itemattr.txt";
            if (!File.Exists(path))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取是否存在角色文件
        /// </summary>
        /// <returns>是否读取</returns>
        private static bool GetCharData()
        {
            for (int i = 0; i < Characters; i++)
            {
                string path = PathPkg + "\\char" + StaticVars.CharacterPkg[i + 1] + ".pkg";
                if (!File.Exists(path))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取属性图片
        /// </summary>
        /// <returns>是否读取</returns>
        private static bool GetPicData()
        {
            string path = PathPkg + "\\tr9.pkg";
            if (!File.Exists(path))
            {
                return false;
            } 
            else
            {
                path = PathPkg + "\\tr15.pkg";
                if (!File.Exists(path))
                {
                    keysVersion = false; // 旧版密匙
                }
            }
            Characters = keysVersion ? StaticVars.Characters_kr : StaticVars.Characters_kr;
            return true;
        }

        // 文本编辑合并
        /// <summary>
        /// 获取道具列表_itemdata
        /// </summary>
        /// <returns>是否读取</returns>
        private static bool GetItemList(BackgroundWorker bw)
        {
            // if (!GetGamePath())
            // {
            //     return false;
            // }
            FileInfo fileInfo = new FileInfo(PathPkg + "\\tr4.pkg");
            string path1 = Path + "itemdata.txt";
            string path2 = Path + "itemsetdata.txt";
            string path3 = Path + "itemdata2.txt";

            if (File.Exists(path1) && File.Exists(path2) && File.Exists(path3))
            {
                string[] strArray1 = File.ReadAllLines(path1, Encoding.UTF8);
                string[] strArray2 = File.ReadAllLines(path2, Encoding.UTF8);
                string[] strArray3 = File.ReadAllLines(path3, Encoding.UTF8);
                if (File.Exists(fileInfo.FullName))
                {
                    if (Convert.ToInt64(strArray1[1].Split('|')[0]) != fileInfo.Length ||
                        (strArray1[1].Split('|')[1].Equals("0") && _charOc) ||
                        Convert.ToInt64(strArray2[0]) != fileInfo.Length ||
                        Convert.ToInt64(strArray3[0]) != fileInfo.Length)
                    {
                        MakeItemData(fileInfo, bw);
                    }
                    else
                    {
                        _charOc = strArray1[1].Split('|')[1].Equals("1");
                        //GetItemData(strArray1);
                    }
                }
                else
                {
                    _charOc = strArray1[1].Split('|')[1].Equals("1");
                    //GetItemData(strArray1);
                }
            }
            else
            {
                if (!File.Exists(fileInfo.FullName))
                {
                    return false;
                }

                MakeItemData(fileInfo, bw);
            }

            return true;
        }

        /// <summary>
        /// 读取文件并编写itemdata
        /// </summary>
        /// <param name="fileInfo">文件路径</param>
        /// <param name="bw">执行该动作的后台进程</param>
        private static void MakeItemData(FileInfo fileInfo, BackgroundWorker bw)
        {
            // 限制录入属性
            string path1 = Path + "itemattr.txt";
            List<int> attrs = new List<int>();
            foreach (string readAllLine in File.ReadAllLines(path1, Encoding.UTF8))
            {
                string[] strArray = readAllLine.Split('|');
                attrs.Add(Convert.ToInt32(strArray[0]));
            }

            // for (int i = 0; i < attrs.Count; i++)
            // {
            //     int num7 = (int) MessageBox.Show("" + i + " = " + attrs[i], "7");
            // }

            // 限制录入部位
            int[] limitPosition =
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 106, 112, 117, 241, 242, 247, 813
            };
            int[] limitPosition2 =
            {
                1001, 1002, 1003, 1004, 2001, 2002, 2003, 2004, 3001, 3002, 3003, 3004, 3005, 3007,
                3008, 3009, 3010, 4001, 5006, 5010, 9001, 9002, 9003, 9004, 9005, 90001
            };

            // 图片偏移
            FileInfo fileInfo1 = new FileInfo(PathPkg);
            SortedList<string, string> listPic = PkgUnpack.PicOffset(fileInfo1);
            //FileStream fs5 = new FileStream(path + "itempic.txt", FileMode.Create, FileAccess.Write);
            //StreamWriter export_file5 = new StreamWriter(fs5);
            //foreach (KeyValuePair<string, long> pair in List_pic)
            //{
            //    export_file5.Write(pair.Key + "," + pair.Value.ToString());
            //    export_file5.WriteLine();
            //}
            //export_file5.Close();
            bw.ReportProgress(4);
            //Action action = () =>
            //{
            //    PBarValue(4);
            //};
            //Invoke(action);


            // 角色装备占用部位和性别写入
            FileInfo fileInfo2 = new FileInfo(PathPkg);
            SortedList<int, string> listNames = new SortedList<int, string>();
            //pkgUnpack.Occupation1(path_pkg + "\\char20.pkg", "mh");

            string[] titles =
            {
                "clientiteminfo\\tblavataritemdesc", "clientiteminfo\\tblavataritemdescex",
                "clientiteminfo\\enchantsystem\\essenenchantsystemitem",
                "clientiteminfo\\enchantsystem\\essenenchantsystemstonemount", "tblluckybagresultitems_1",
                "clientiteminfo\\essenitemcollectiondesc", "clientiteminfo\\tblavataritemdescattr",
                "content\\alchemist\\essenalchemistmixcondition", "clientiteminfo\\tblavataritemsetdesc",
                "clientiteminfo\\tblavataritemsetattr", "archives\\essenarchives_exchangelist",
                "clientiteminfo\\essenitemcubereward", "clientiteminfo\\settingitemcubelist"
            };
            Encoding[] encodings =
            {
                Encoding.UTF8, Encoding.ASCII, Encoding.Unicode, Encoding.Unicode, Encoding.Unicode, Encoding.ASCII,
                Encoding.UTF8, Encoding.ASCII, Encoding.ASCII, Encoding.ASCII, Encoding.ASCII, Encoding.Unicode,
                Encoding.Unicode, Encoding.Unicode
            };

            SortedList<string, string[]> txtFileList = PkgUnpack.TxtText(fileInfo, titles, encodings);

            // 写入基本信息 图片偏移 写入套装基本信息
            SortedList<int, ItemStatusMaking> list = new SortedList<int, ItemStatusMaking>();
            SortedList<int, ItemSetStatusMaking> listSet = new SortedList<int, ItemSetStatusMaking>();
            SortedList<int, ItemBoxStatusMaking> listBox = new SortedList<int, ItemBoxStatusMaking>();
            string[] str1 = txtFileList["clientiteminfo\\tblavataritemdesc"];
            string[] str2 = txtFileList["clientiteminfo\\tblavataritemdescex"];
            for (int index1 = 2; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                // int num1 = (int) MessageBox.Show(str1[index1], "1");
                // int num2 = (int) MessageBox.Show(""+str1_1.Length, "2");

                // if (str1_1.Length != 27)
                // {
                //     String strs1_1 = "";
                //     for (int i = 0; i < str1_1.Length; i++)
                //     {
                //         strs1_1 += "str[" + i + "]" + str1_1[i] + "\r\n";
                //     }
                //
                //     int num3 = (int) MessageBox.Show(strs1_1, "3");
                // }

                if (Convert.ToInt32(str11[2]) == 1 || Convert.ToInt32(str11[2]) == 2)
                {
                    long offset = -1;
                    short pkgNum = -1;
                    string imageStr = GetItemImageName(Convert.ToByte(str11[3]), Convert.ToUInt16(str11[4]),
                        Convert.ToUInt16(str11[5]), Convert.ToByte(str11[26]));
                    if (listPic.ContainsKey(imageStr))
                    {
                        pkgNum = Convert.ToInt16(listPic[imageStr].Split(',')[0]);
                        offset = Convert.ToInt64(listPic[imageStr].Split(',')[1]);
                    }

                    listNames.Add(Convert.ToInt32(str11[0]),
                        GetItemModelName(Convert.ToByte(str11[3]), Convert.ToUInt16(str11[4]),
                            Convert.ToUInt16(str11[5])));

                    list.Add(Convert.ToInt32(str11[0]), new ItemStatusMaking
                    {
                        ItemNum = Convert.ToInt32(str11[0]),
                        Name = str11[11].Replace(",", " "),
                        NameCh = "(noname)",
                        // Avatar = 0,
                        Position = Convert.ToInt32(str11[4]),
                        Position2 = Convert.ToInt32(str11[12]),
                        ItemKind = Convert.ToUInt16(str11[5]),
                        // Point = 0,
                        // ID = 0,
                        Char = Convert.ToByte(str11[3]),
                        Level = Convert.ToByte(str11[26]),
                        // SetNum = 0,
                        PkgNum = pkgNum,
                        PicOffset = offset,
                        // Attr = new List<Attr>()
                        // Sex = sex
                        Occupation = Convert.ToInt16(str11[12].Equals("4001") && str11[4].Equals("10") ? 512 : 0)
                        // Chars = 0
                    });
                    if ((limitPosition.Contains(Convert.ToInt32(str11[4])) ||
                         (Convert.ToInt32(str11[4]) == 0 && Convert.ToInt32(str11[3]) != 0)) &&
                        limitPosition2.Contains(Convert.ToInt32(str11[12])) && Convert.ToByte(str11[3]) != 101)
                    {
                        // 如果是需要显示的装备，输出所有信息
                        list[Convert.ToInt32(str11[0])].Complete = true;
                        list[Convert.ToInt32(str11[0])].Demand = true;
                    }
                }
                else
                {
                    list.Add(Convert.ToInt32(str11[0]), new ItemStatusMaking
                    {
                        ItemNum = Convert.ToInt32(str11[0]),
                        Name = str11[11].Replace(",", " "),
                        NameCh = "(noname)",
                        // Avatar = 0,
                        // Position = Convert.ToInt32(str1_1[4]),
                        // Position2 = Convert.ToInt32(str1_1[12]),
                        // ItemKind = Convert.ToUInt16(str1_1[5]),
                        // Point = 0,
                        // ID = 0,
                        // Char = Convert.ToByte(str1_1[3]),
                        // Level = Convert.ToByte(str1_1[26]),
                        // SetNum = 0,
                        // PicOffset = -1,
                        // Attr = new List<Attr>()
                        // Sex = sex
                        // Occupation = 0
                        // Chars = 0
                    });

                    if (Convert.ToInt32(str11[2]) == 4 && Convert.ToInt32(str11[4]) == 112)
                    {
                        listSet.Add(Convert.ToInt32(str11[0]), new ItemSetStatusMaking
                        {
                            //Attr = new List<setAttr>(),
                            //MemberBasic = new List<int>(), // 保存同套装道具编号
                            //Avatar = 0,
                            SetNum = Convert.ToInt32(str11[0]),
                            //Count = 0,
                            Name = str11[11].Replace(",", " "),
                            NameCh = "(noname)"
                        });
                    }
                }
            }

            // 为个数道具指定图片
            for (int index1 = 2; index1 < str2.Length; ++index1)
            {
                string[] str21 = StringDivide(str2[index1]);
                if (list.ContainsKey(Convert.ToInt32(str21[1])) && list.ContainsKey(Convert.ToInt32(str21[2])))
                {
                    list[Convert.ToInt32(str21[1])].PkgNum = list[Convert.ToInt32(str21[2])].PkgNum;
                    list[Convert.ToInt32(str21[1])].PicOffset = list[Convert.ToInt32(str21[2])].PicOffset;
                }
            }

            bw.ReportProgress(6);

            // 宝石槽
            str1 = txtFileList["clientiteminfo\\enchantsystem\\essenenchantsystemitem"];
            for (int index1 = 1; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                if (list.ContainsKey(Convert.ToInt32(str11[0])))
                {
                    int slot = Convert.ToInt32(str11[2]);
                    for (int i = 0; i < (8 - Convert.ToInt32(str11[1])); i++)
                    {
                        slot *= 16;
                    }

                    list[Convert.ToInt32(str11[0])].Slot += slot;
                }
            }

            bw.ReportProgress(7);

            // 宝石属性
            str1 = txtFileList["clientiteminfo\\enchantsystem\\essenenchantsystemstonemount"];
            for (int index1 = 1; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                if (list.ContainsKey(Convert.ToInt32(str11[0])))
                {
                    list[Convert.ToInt32(str11[0])].Attr.Add(new Attr
                    {
                        Num = Convert.ToInt32(str11[1]),
                        Value = Convert.ToSingle(str11[2])
                    });
                }
            }

            bw.ReportProgress(8);

            // 返回角色占用和装备占用
            if (_charOc)
            {
                SortedList<int, int[]> listOc = PkgUnpack.Occupation(fileInfo2, listNames, bw);
                foreach (KeyValuePair<int, int[]> pair in listOc)
                {
                    if (list.ContainsKey(pair.Key))
                    {
                        list[pair.Key].Sex = Convert.ToByte(pair.Value[0]);
                        list[pair.Key].Occupation = Convert.ToInt16(pair.Value[1]);
                        list[pair.Key].Chars = Convert.ToInt32(pair.Value[2]);
                    }
                }

                listOc.Clear();
            }

            bw.ReportProgress(68);



            bw.ReportProgress(70);

            // 普通箱子内容物
            str1 = txtFileList["tblluckybagresultitems_1"];
            for (int index1 = 1; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                if (list.ContainsKey(Convert.ToInt32(str11[1])) && list.ContainsKey(Convert.ToInt32(str11[2])))
                {
                    list[Convert.ToInt32(str11[1])].Demand = true;
                    list[Convert.ToInt32(str11[2])].Demand = true;
                    if (listBox.ContainsKey(Convert.ToInt32(str11[2])))
                    {
                        listBox[Convert.ToInt32(str11[2])].MemWeiBasic
                            .Add(Convert.ToInt32(str11[1]), Convert.ToInt32(str11[3]));
                        listBox[Convert.ToInt32(str11[2])].WeightTotalBasic += Convert.ToInt32(str11[3]);
                    }
                    else
                    {
                        if (list[Convert.ToInt32(str11[2])].Position == 250 &&
                            list[Convert.ToInt32(str11[2])].PicOffset > 0)
                        {
                            listBox.Add(Convert.ToInt32(str11[2]), new ItemBoxStatusMaking
                            {
                                BoxNum = Convert.ToInt32(str11[2]),
                                //MemberBasic = new List<int> { Convert.ToInt32(str1_1[1]) },
                                //WeightBasic = new List<int> { Convert.ToInt32(str1_1[3]) },
                                MemWeiBasic = new SortedList<int, float>
                                    {{Convert.ToInt32(str11[1]), Convert.ToSingle(str11[3])}},
                                WeightTotalBasic = Convert.ToInt32(str11[3]),
                                Position = 250,
                                SilverRate = 0
                            });
                        }
                    }
                }
            }

            // 前线箱子道具
            str1 = txtFileList["archives\\essenarchives_exchangelist"];
            for (int index1 = 1; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                if (list.ContainsKey(Convert.ToInt32(str11[0])) && list.ContainsKey(Convert.ToInt32(str11[1])))
                {
                    list[Convert.ToInt32(str11[0])].Demand = true;
                    list[Convert.ToInt32(str11[1])].Demand = true;
                    if (listBox.ContainsKey(Convert.ToInt32(str11[0])))
                    {
                        listBox[Convert.ToInt32(str11[0])].MemWeiBasic
                            .Add(Convert.ToInt32(str11[1]), Convert.ToInt32(str11[2]));
                        listBox[Convert.ToInt32(str11[0])].WeightTotalBasic += Convert.ToInt32(str11[2]);
                    }
                    else
                    {
                        listBox.Add(Convert.ToInt32(str11[0]), new ItemBoxStatusMaking
                        {
                            BoxNum = Convert.ToInt32(str11[0]),
                            MemWeiBasic = new SortedList<int, float>
                                {{Convert.ToInt32(str11[1]), Convert.ToSingle(str11[2])}},
                            WeightTotalBasic = Convert.ToInt32(str11[2]),
                            Position = 396,
                            SilverRate = 0
                        });
                    }
                }
            }

            // 魔方道具奖励组
            str1 = txtFileList["clientiteminfo\\essenitemcubereward"];
            SortedList<int, SortedList<int, float>> cubeReward = new SortedList<int, SortedList<int, float>>();
            for (int index1 = 1; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                int num = Convert.ToInt32(str11[0]);
                if (list.ContainsKey(Convert.ToInt32(str11[1])))
                {
                    list[Convert.ToInt32(str11[1])].Demand = true;
                    if (cubeReward.ContainsKey(num))
                    {
                        cubeReward[num].Add(Convert.ToInt32(str11[1]), Convert.ToSingle(str11[3]));
                    }
                    else
                    {
                        cubeReward.Add(Convert.ToInt32(str11[0]),
                            new SortedList<int, float> { { Convert.ToInt32(str11[1]), Convert.ToSingle(str11[3]) } });
                    }
                }
            }

            // 魔方箱子道具
            str1 = txtFileList["clientiteminfo\\settingitemcubelist"];
            for (int index1 = 1; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                if (list.ContainsKey(Convert.ToInt32(str11[0])))
                {
                    list[Convert.ToInt32(str11[0])].Demand = true;
                    if (!listBox.ContainsKey(Convert.ToInt32(str11[0])))
                    {
                        listBox.Add(Convert.ToInt32(str11[0]), new ItemBoxStatusMaking
                        {
                            BoxNum = Convert.ToInt32(str11[0]),
                            MemWeiBasic = new SortedList<int, float>(),
                            MemWeiSilver = new SortedList<int, float>(),
                            MemWeiGold = new SortedList<int, float>(),
                            WeightTotalBasic = 0,
                            WeightTotalSilver = 0,
                            WeightTotalGold = 0,
                            Position = 395,
                            SilverRate = Convert.ToSingle(str11[14])
                        });
                        if (cubeReward.ContainsKey(Convert.ToInt32(str11[4])))
                        {
                            foreach (var t in cubeReward[Convert.ToInt32(str11[4])])
                            {
                                listBox[Convert.ToInt32(str11[0])].MemWeiBasic.Add(t.Key, t.Value);
                                listBox[Convert.ToInt32(str11[0])].WeightTotalBasic += t.Value;
                            }
                        }

                        if (cubeReward.ContainsKey(Convert.ToInt32(str11[8])))
                        {
                            foreach (var t in cubeReward[Convert.ToInt32(str11[8])])
                            {
                                listBox[Convert.ToInt32(str11[0])].MemWeiSilver.Add(t.Key, t.Value);
                                listBox[Convert.ToInt32(str11[0])].WeightTotalSilver += t.Value;
                            }
                        }

                        if (cubeReward.ContainsKey(Convert.ToInt32(str11[12])))
                        {
                            foreach (var t in cubeReward[Convert.ToInt32(str11[12])])
                            {
                                listBox[Convert.ToInt32(str11[0])].MemWeiGold.Add(t.Key, t.Value);
                                listBox[Convert.ToInt32(str11[0])].WeightTotalGold += t.Value;
                            }
                        }
                    }
                }
            }

            // 收藏分数和收藏编号
            str1 = txtFileList["clientiteminfo\\essenitemcollectiondesc"];
            for (int index1 = 1; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                if (list.ContainsKey(Convert.ToInt32(str11[0])))
                {
                    list[Convert.ToInt32(str11[0])].Point = Convert.ToByte(str11[1]);
                    list[Convert.ToInt32(str11[0])].Id = Convert.ToInt32(str11[2]);
                }
            }

            bw.ReportProgress(72);

            // 道具属性
            str1 = txtFileList["clientiteminfo\\tblavataritemdescattr"];
            for (int index1 = 2; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                if (list.ContainsKey(Convert.ToInt32(str11[1])))
                {
                    if (attrs.Contains(Convert.ToInt32(str11[2])))
                    {
                        list[Convert.ToInt32(str11[1])].Attr.Add(new Attr
                        {
                            Num = Convert.ToInt32(str11[2]),
                            Value = Convert.ToSingle(str11[3])
                        }
                        );
                    }
                    else if (Convert.ToInt32(str11[2]) == 5001 && Convert.ToSingle(str11[3]) != 0)
                    {
                        list[Convert.ToInt32(str11[1])].Avatar = 1;
                    }
                    else if (Convert.ToInt32(str11[2]) == 79)
                    {
                        list[Convert.ToInt32(str11[1])].Sex = Convert.ToByte(str11[3].Split('.')[0]);
                    }
                }
            }

            bw.ReportProgress(75);


            SortedList<int, ItemStatusMaking> listAttr = new SortedList<int, ItemStatusMaking>();
            // 炼金属性
            str1 = txtFileList["content\\alchemist\\essenalchemistmixcondition"];
            for (int index1 = 1; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                if (Convert.ToInt32(str11[1]) == 100 && list.ContainsKey(Convert.ToInt32(str11[0])))
                {
                    if (attrs.Contains(Convert.ToInt32(str11[3])))
                    {
                        if (listAttr.ContainsKey(Convert.ToInt32(str11[0])))
                        {
                            listAttr[Convert.ToInt32(str11[0])].Attr.Add(new Attr
                            {
                                Num = Convert.ToInt32(str11[3]),
                                Value = Convert.ToSingle(str11[5])
                            }
                            );
                        }
                        else
                        {
                            listAttr.Add(Convert.ToInt32(str11[0]), new ItemStatusMaking
                            {
                                ItemNum = Convert.ToInt32(str11[0]),
                                Avatar = 0,
                                Attr = new List<Attr>()
                            }
                            );
                            listAttr[Convert.ToInt32(str11[0])].Attr.Add(new Attr
                            {
                                Num = Convert.ToInt32(str11[3]),
                                Value = Convert.ToSingle(str11[5])
                            }
                            );
                        }
                    }
                    else if (Convert.ToInt32(str11[3]) == 5001)
                    {
                        if (listAttr.ContainsKey(Convert.ToInt32(str11[0])))
                        {
                            listAttr[Convert.ToInt32(str11[0])].Avatar = 1;
                        }
                        else
                        {
                            listAttr.Add(Convert.ToInt32(str11[0]), new ItemStatusMaking
                            {
                                ItemNum = Convert.ToInt32(str11[0]),
                                Avatar = 1,
                                Attr = new List<Attr>()
                            }
                            );
                        }
                    }
                }
            }

            bw.ReportProgress(78);

            // 炼金道具属性写入
            foreach (KeyValuePair<int, ItemStatusMaking> pair in listAttr)
            {
                if (list.ContainsKey(pair.Value.ItemNum))
                {
                    list[pair.Value.ItemNum].Attr = pair.Value.Attr;
                    list[pair.Value.ItemNum].Avatar = pair.Value.Avatar;
                }
            }

            bw.ReportProgress(79);

            // 套装编号
            str1 = txtFileList["clientiteminfo\\tblavataritemsetdesc"];
            for (int index1 = 1; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                if (list.ContainsKey(Convert.ToInt32(str11[1])))
                {
                    list[Convert.ToInt32(str11[1])].SetNum = Convert.ToInt32(str11[0]);
                }
            }

            bw.ReportProgress(80);

            // foreach (KeyValuePair<int, ItemStatusMaking> pair in list)
            // {
            //     if (pair.Value.Demand)
            //     {
            //         if (pair.Value.NameCh.Equals("(noname)"))
            //         {
            //             string str = Translate.Trans(pair.Value.Name);
            //             pair.Value.NameCh = str;
            //             listTrans.Add(pair.Value.ItemNum,str);
            //         }
            //     }
            // }
            // FileStream fs5 = new FileStream(Path + "itemdata.txt", FileMode.Create,
            //     FileAccess.Write);
            // StreamWriter exportFile5 = new StreamWriter(fs5);
            //
            //
            // foreach (KeyValuePair<int, string> pair in listTrans)
            // {
            //     exportFile5.Write(pair.Key+","+pair.Value);
            //     exportFile5.WriteLine();
            // }
            // exportFile5.Close();
            bw.ReportProgress(81);
            #region 查漏补缺区

            // 角色性别补全
            list[52139].Sex = 2;
            list[83403].Sex = 1;
            list[83404].Sex = 1;
            #endregion

            // FileStream fs3 = new FileStream(Form1.path + "itempic.txt", FileMode.Create,
            //     FileAccess.Write);
            // StreamWriter export_file1 = new StreamWriter(fs3);
            // foreach (KeyValuePair<string, long> pair in List_pic)
            // {
            //     export_file1.Write(pair.Key + "," + pair.Value);
            //     export_file1.WriteLine();
            // }
            // foreach (KeyValuePair<int, ItemStatusMaking> pair in List)
            // {
            //     if (List_pic.ContainsKey(GetItemImageName(pair.Value)))
            //     {
            //         pair.Value.PicOffset = List_pic[GetItemImageName(pair.Value)];
            //     }
            // }

            // 写入文本
            string date = DateTime.Now.ToString("yyyy/MM/dd"); // 2020-02-02
            FileStream fs2 = new FileStream(Path + "itemdata.txt", FileMode.Create,
                FileAccess.Write);
            StreamWriter exportFile2 = new StreamWriter(fs2);
            exportFile2.Write(date);
            exportFile2.WriteLine();
            exportFile2.Write(fileInfo.Length + "|" + (_charOc ? "1" : "0"));
            exportFile2.WriteLine();
            foreach (KeyValuePair<int, ItemStatusMaking> pair in list)
            {
                if (pair.Value.Demand)
                {
                    if (pair.Value.Complete)
                    {
                        exportFile2.Write(pair.Value.ItemNum + "," + pair.Value.Name + "," + pair.Value.NameCh + "," +
                                          pair.Value.Avatar + ","
                                          + pair.Value.Position + "," + pair.Value.Position2 + "," +
                                          pair.Value.ItemKind + "," +
                                          pair.Value.Point + "," + pair.Value.Id + "," +
                                          pair.Value.Char + "," + pair.Value.Level + "," + pair.Value.SetNum + "," +
                                          pair.Value.PkgNum + "," + pair.Value.PicOffset + "," +
                                          pair.Value.Sex + "," + pair.Value.Occupation + ","
                                          + pair.Value.Chars + "," + pair.Value.Slot + "|");

                        pair.Value.Attr.Sort((a, b) => a.Num.CompareTo(b.Num));

                        for (int i = 0; i < pair.Value.Attr.Count; i++)
                        {
                            exportFile2.Write(pair.Value.Attr[i].Num + "," + pair.Value.Attr[i].Value);
                            // int num6 = (int) MessageBox.Show("" + pair.Value.Attr[i].Num + "," + pair.Value.Attr[i].Value,
                            //     "6");
                            if (i != pair.Value.Attr.Count - 1)
                            {
                                exportFile2.Write(",");
                            }
                        }
                    }
                    else
                    {
                        // 资料不完全
                        exportFile2.Write(pair.Value.ItemNum + "," + pair.Value.Name + "," + pair.Value.NameCh + "," +
                                          pair.Value.PkgNum + "," + pair.Value.PicOffset + "|");
                    }

                    exportFile2.WriteLine();
                }
            }

            exportFile2.Close();


            // 箱子信息写入
            FileStream fs4 = new FileStream(Path + "itemdata2.txt", FileMode.Create, FileAccess.Write);
            StreamWriter exportFile4 = new StreamWriter(fs4);
            exportFile4.Write(fileInfo.Length);
            exportFile4.WriteLine();
            foreach (KeyValuePair<int, ItemBoxStatusMaking> pair in listBox)
            {
                exportFile4.Write(pair.Value.BoxNum + "," + pair.Value.WeightTotalBasic + "," +
                                  pair.Value.WeightTotalSilver + "," + pair.Value.WeightTotalGold + "," +
                                  pair.Value.SilverRate + "," + pair.Value.Position + "|");

                int i = 0;
                foreach (KeyValuePair<int, float> t in pair.Value.MemWeiBasic)
                {
                    exportFile4.Write(t.Key + "," + t.Value);
                    // int num6 = (int) MessageBox.Show("" + pair.Value.Attr[i].Num + "," + pair.Value.Attr[i].Value,
                    //     "6");
                    if (i != pair.Value.MemWeiBasic.Count - 1)
                    {
                        exportFile4.Write(",");
                        i++;
                    }
                }

                exportFile4.Write("|");
                i = 0;
                foreach (KeyValuePair<int, float> t in pair.Value.MemWeiSilver)
                {
                    exportFile4.Write(t.Key + "," + t.Value);
                    // int num6 = (int) MessageBox.Show("" + pair.Value.Attr[i].Num + "," + pair.Value.Attr[i].Value,
                    //     "6");
                    if (i != pair.Value.MemWeiSilver.Count - 1)
                    {
                        exportFile4.Write(",");
                        i++;
                    }
                }

                exportFile4.Write("|");
                i = 0;
                foreach (KeyValuePair<int, float> t in pair.Value.MemWeiGold)
                {
                    exportFile4.Write(t.Key + "," + t.Value);
                    // int num6 = (int) MessageBox.Show("" + pair.Value.Attr[i].Num + "," + pair.Value.Attr[i].Value,
                    //     "6");
                    if (i != pair.Value.MemWeiGold.Count - 1)
                    {
                        exportFile4.Write(",");
                        i++;
                    }
                }

                exportFile4.WriteLine();
            }

            exportFile4.Close();
            bw.ReportProgress(83);

            // 录入套装
            str1 = txtFileList["clientiteminfo\\tblavataritemsetdesc"];
            for (int index1 = 1; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                if (!listSet.ContainsKey(Convert.ToInt32(str11[0])))
                {
                    listSet.Add(Convert.ToInt32(str11[0]), new ItemSetStatusMaking
                    {
                        // Attr = new List<setAttr>(),
                        Member = new List<int> { Convert.ToInt32(str11[1]) },
                        // Avatar = 0,
                        SetNum = Convert.ToInt32(str11[0]),
                        Count = 1
                    });
                }
                else
                {
                    listSet[Convert.ToInt32(str11[0])].Member.Add(Convert.ToInt32(str11[1]));
                    listSet[Convert.ToInt32(str11[0])].Count++;
                }
            }

            bw.ReportProgress(86);

            // 录入套装属性
            str1 = txtFileList["clientiteminfo\\tblavataritemsetattr"];
            for (int index1 = 1; index1 < str1.Length; ++index1)
            {
                string[] str11 = StringDivide(str1[index1]);
                if (listSet.ContainsKey(Convert.ToInt32(str11[1])))
                {
                    // 完整Key0的属性叠加 外装属性判定 属性值不属于确认区间不写入
                    if (Convert.ToInt32(str11[5]) == 2)
                    {
                        listSet[Convert.ToInt32(str11[1])].Avatar = 1;
                    }

                    if (Convert.ToInt16(str11[2]) > 0)
                    {
                        listSet[Convert.ToInt32(str11[1])].Attr.Add(new SetAttr
                        {
                            CompleteKey = Convert.ToUInt16(str11[2]),
                            Num = Convert.ToInt32(str11[3]),
                            Value = Convert.ToSingle(str11[4])
                        });
                    }
                    else if (Convert.ToInt16(str11[2]) < 0)
                    {
                        str11[2] = "0";
                    }

                    if (attrs.Contains(Convert.ToInt32(str11[3])))
                    {
                        if (listSet[Convert.ToInt32(str11[1])].Attr.Exists(item =>
                            item.CompleteKey == 0 && item.Num == Convert.ToInt32(str11[3])))
                        {
                            float fTemp = listSet[Convert.ToInt32(str11[1])]
                                .Attr[
                                    listSet[Convert.ToInt32(str11[1])].Attr.FindIndex(item =>
                                        item.CompleteKey == 0 && item.Num == Convert.ToInt32(str11[3]))].Value;
                            listSet[Convert.ToInt32(str11[1])]
                                    .Attr[
                                        listSet[Convert.ToInt32(str11[1])].Attr.FindIndex(item =>
                                            item.CompleteKey == 0 && item.Num == Convert.ToInt32(str11[3]))].Value =
                                Plus(Convert.ToSingle(str11[4]), fTemp);
                        }
                        else
                        {
                            listSet[Convert.ToInt32(str11[1])].Attr.Add(new SetAttr
                            {
                                CompleteKey = 0,
                                Num = Convert.ToInt32(str11[3]),
                                Value = Convert.ToSingle(str11[4])
                            });
                        }
                    }
                }
            }

            bw.ReportProgress(93);

            // 写入文本
            FileStream fs3 = new FileStream(Path + "itemsetdata.txt", FileMode.Create,
                FileAccess.Write);
            StreamWriter exportFile3 = new StreamWriter(fs3);
            exportFile3.Write(fileInfo.Length);
            exportFile3.WriteLine();
            foreach (KeyValuePair<int, ItemSetStatusMaking> pair in listSet)
            {
                exportFile3.Write(pair.Value.SetNum + "," + pair.Value.Name + "," + pair.Value.NameCh + "," +
                                  pair.Value.Avatar + "," +
                                  pair.Value.Count + "|");
                pair.Value.Member.Sort(delegate (int a, int b)
                {
                    if (list.ContainsKey(a) && list.ContainsKey(b))
                    {
                        return list[a].Position.CompareTo(list[b].Position);
                    }

                    return 0;
                });
                pair.Value.Attr.Sort(delegate (SetAttr a, SetAttr b)
                {
                    if (a.CompleteKey.CompareTo(b.CompleteKey) == 0)
                    {
                        return a.Num.CompareTo(b.Num);
                    }

                    return a.CompleteKey.CompareTo(b.CompleteKey);
                });
                for (int i = 0; i < pair.Value.Attr.Count; i++)
                {
                    exportFile3.Write(pair.Value.Attr[i].CompleteKey + "," + pair.Value.Attr[i].Num + "," +
                                      pair.Value.Attr[i].Value);
                    if (i != pair.Value.Attr.Count - 1)
                    {
                        exportFile3.Write(",");
                    }
                }

                exportFile3.Write("|");
                for (int i = 0; i < pair.Value.Member.Count; i++)
                {
                    exportFile3.Write(pair.Value.Member[i]);
                    if (i != pair.Value.Member.Count - 1)
                    {
                        exportFile3.Write(",");
                    }
                }

                exportFile3.WriteLine();
            }

            exportFile3.Close();
            bw.ReportProgress(96);

            #region List释放

            // List内存释放
            listPic.Clear();
            listNames.Clear();
            attrs.Clear();
            txtFileList.Clear();
            list.Clear();
            listSet.Clear();
            listBox.Clear();
            listAttr.Clear();

            #endregion
        }

        /// <summary>
        /// 获取道具对应的图片名
        /// </summary>
        /// <param name="char"></param>
        /// <param name="position"></param>
        /// <param name="itemKind"></param>
        /// <param name="level"></param>
        /// <returns>图片名</returns>
        private static string GetItemImageName(byte @char, ushort position, ushort itemKind, byte level)
        {
            Imagepath.Length = 0;
            // if (Char > 50)
            // {
            //     int num4 = (int) MessageBox.Show("" + Name + " " + ItemNum + " " + Char + " " + Position + " " + ItemKind, "4");
            // }
            // Imagepath.Append("tr9\\guiex\\itemimage\\");
            if (@char < 100)
            {
                Imagepath.Append(StaticVars.Character[@char]);
            }
            else if (@char > 200)
            {
                Imagepath.Append(StaticVars.Character[@char - 200 + Characters]);
            }

            if (position <= StaticVars.Positions)
            {
                Imagepath.Append(StaticVars.Position[position]);
            }
            else if (position == 1000)
            {
                Imagepath.Length = 0;
                //Imagepath.Append("tr9\\guiex\\itemimage\\farm_object_");
                Imagepath.Append("farm_object_");
            }
            else if ((171 <= position && position <= 174) || position == 177)
            {
                Imagepath.Append(position + "_001.png");
                return Imagepath.ToString();
            }
            else
            {
                Imagepath.Append(position + "_");
            }

            //else
            //    Imagepath.Append(
            //        position[Position]);
            Imagepath.AppendFormat("{0:D3}", itemKind);

            if (level == 6) // 不需要删除"_ef_my"直接读取SSS图片
            {
                Imagepath.Append("_ef_my");
            }

            Imagepath.Append(".png");
            return Imagepath.ToString();
        }

        /// <summary>
        /// 获取道具对应的模型名
        /// </summary>
        /// <param name="char"></param>
        /// <param name="position"></param>
        /// <param name="itemKind"></param>
        /// <returns>模型名</returns>
        private static string GetItemModelName(byte @char, ushort position, ushort itemKind)
        {
            Modelpath.Length = 0;
            // if (Char > 50)
            // {
            //     int num4 = (int) MessageBox.Show("" + Name + " " + ItemNum + " " + Char + " " + Position + " " + ItemKind, "4");
            // }
            //Modelpath.Append("tr9\\guiex\\itemimage\\");
            if (itemKind > 1000)
            {
                Modelpath.Append(StaticVars.Character[0]);
            }
            else if (@char < 100)
            {
                Modelpath.Append(StaticVars.Character[@char]);
            }
            else if (@char > 200)
            {
                Modelpath.Append(StaticVars.Character[@char - 200 + Characters]);
            }

            if (position <= StaticVars.Positions)
            {
                Modelpath.Append(StaticVars.Position[position]);
            }
            else if (position == 106)
            {
                Modelpath.Append(StaticVars.Position[2]);
            }
            else if (position == 117)
            {
                Modelpath.Append(StaticVars.Position[2]);
            }

            if (@char == 0)
            {
                Modelpath.Append(itemKind);
            }
            else
            {
                Modelpath.AppendFormat("{0:D3}", itemKind);
            }

            Modelpath.Append(".pt1");
            return Modelpath.ToString();
        }

        #endregion

        #region 数据读取

        /// <summary>
        /// 获取文本
        /// </summary>
        internal static bool GetData()
        {
            GetAttrList();
            GetItemData();
            GetItemSetData();
            GetItemData2();
            for (int index = 0; index < Item.Count; ++index)
            {
                AddListView_Item(Item[index], index);
            }

            GC.Collect();
            return _charOc;
        }

        /// <summary>
        /// 获取属性文字_itemattr.txt
        /// </summary>
        /// <returns>是否读取</returns>
        private static void GetAttrList()
        {
            string path = Path + "itemattr.txt";
            if (!File.Exists(path))
            {
                return;
            }

            foreach (string readAllLine in File.ReadAllLines(path, Encoding.UTF8))
            {
                string[] strArray = readAllLine.Split('|');
                int int32 = Convert.ToInt32(strArray[0]);
                ItemAttrInfo.Add(int32, new AttrInfo());
                ItemAttrInfo[int32].Type = Convert.ToSingle(strArray[1]);
                ItemAttrInfo[int32].Desc = strArray[2];
                if (strArray.Length == 4)
                {
                    ItemAttrInfo[int32].Desc2 = strArray[3];
                }

                if (strArray.Length == 5)
                {
                    ItemAttrInfo[int32].Desc2 = strArray[3];
                    ItemAttrInfo[int32].Desc3 = strArray[4];
                }

                if (strArray.Length == 6)
                {
                    ItemAttrInfo[int32].Desc2 = strArray[3];
                    ItemAttrInfo[int32].Desc3 = strArray[4];
                    ItemAttrInfo[int32].Desc4 = strArray[5];
                }
            }

            // 212属性跟随数值写入
            List212.Add(1, new List<Attr> { new Attr { Num = 2, Value = 0.15F } });
            List212.Add(2, new List<Attr> { new Attr { Num = 121, Value = 0.1F } });
            List212.Add(3, new List<Attr> { new Attr { Num = 1, Value = 0.15F } });
            List212.Add(4, new List<Attr> { new Attr { Num = 87, Value = 0.1F }, new Attr { Num = 185, Value = 0.1F } });
            List212.Add(5, new List<Attr> { new Attr { Num = 2, Value = 0.3F } });
            List212.Add(6, new List<Attr> { new Attr { Num = 1, Value = 0.1F }, new Attr { Num = 2, Value = 0.2F } });
            List212.Add(7, new List<Attr> { new Attr { Num = 2, Value = 0.3F } });
            List212.Add(8, new List<Attr> { new Attr { Num = 1, Value = 0.1F }, new Attr { Num = 2, Value = 0.2F } });
            List212.Add(9, new List<Attr> { new Attr { Num = 2, Value = 0.3F } });
            List212.Add(10, new List<Attr> { new Attr { Num = 2, Value = 0.2F } });
            List212.Add(11, new List<Attr> { new Attr { Num = 2, Value = 0.25F } });
            List212.Add(12, new List<Attr> { new Attr { Num = 2, Value = 0.25F } });
            List212.Add(13, new List<Attr> { new Attr { Num = 2, Value = 0.25F } });
            List212.Add(14, new List<Attr> { new Attr { Num = 2, Value = 0.3F } });
            List212.Add(15, new List<Attr> { new Attr { Num = 2, Value = 0.3F } });
        }

        internal static string GetAttrListText()
        {
            string path = Path + "itemattr.txt";
            string text = string.Empty;

            foreach (string readAllLine in File.ReadAllLines(path, Encoding.UTF8))
            {
                string[] strArray = readAllLine.Split('|');
                int int32 = Convert.ToInt32(strArray[0]);
                string str2 = strArray[2];
                string str3 = string.Empty;
                string str4 = string.Empty;
                string str5 = string.Empty;
                if (strArray.Length == 4)
                {
                    str3 = strArray[3];
                }

                if (strArray.Length == 5)
                {
                    str3 = strArray[3];
                    str4 = strArray[4];
                }

                if (strArray.Length == 6)
                {
                    str3 = strArray[3];
                    str4 = strArray[4];
                    str5 = strArray[5];
                }

                if (int32 < 400)
                {
                    text += $"{int32}-{str2} {str3} {str4} {str5}\r\n";
                }
            }

            return text;
        }

        /// <summary>
        /// 读取itemdata
        /// </summary>
        private static void GetItemData()
        {
            // 翻译
            SortedList<int, string> itemTrans = new SortedList<int, string>();
            string path = Path + "itemtranslation.txt";
            if (File.Exists(path))
            {
                String[] str1 = File.ReadAllLines(Path + "itemtranslation.txt", Encoding.UTF8);
                foreach (var t in str1)
                {
                    string[] str11 = StringDivide(t);
                    itemTrans.Add(Convert.ToInt32(str11[0]), str11[1]);
                }
            }

            path = Path + "itemdata.txt";
            string[] strArray1 = File.ReadAllLines(path, Encoding.UTF8);
            // 追加了收藏等级和套装数据，针对属性为空的道具做了处理使其不会产生异常
            _dataVer = strArray1[0];
            _charOc = strArray1[1].Split('|')[1].Equals("1");
            for (int index1 = 2; index1 < strArray1.Length; ++index1)
            {
                string[] strArray2 = strArray1[index1].Split('|');
                string[] strArray3 = strArray2[0].Split(',');
                List<Attr> attrList = new List<Attr>();
                if (strArray2[1].Length != 0)
                {
                    // strArray2[1]空值不处理，非空才会处理
                    string[] strArray4 = strArray2[1].Split(',');
                    for (int index2 = 0; index2 < strArray4.Length; index2 += 2)
                    {
                        attrList.Add(new Attr
                        {
                            Num = Convert.ToInt32(strArray4[index2]),
                            Value = Convert.ToSingle(strArray4[index2 + 1])
                        });
                    }
                }

                string nameCH = (itemTrans.ContainsKey(Convert.ToInt32(strArray3[0]))) ? itemTrans[Convert.ToInt32(strArray3[0])] : "(noname)";

                if (strArray3.Length != 5)
                {
                    // 资料完全
                    if (Convert.ToUInt16(strArray3[4]) != 813) // 宝石
                    {
                        Item.Add(new ItemStatus
                        {
                            ItemNum = Convert.ToInt32(strArray3[0]),
                            Name = strArray3[1],
                            NameCh = nameCH,
                            Avatar = Convert.ToByte(strArray3[3]),
                            // Position = Convert.ToUInt16(strArray3[4]),
                            Position2 = Convert.ToUInt16(strArray3[5]),
                            // ItemKind = Convert.ToUInt16(strArray3[6]),
                            Point = Convert.ToByte(strArray3[7]),
                            Id = Convert.ToInt32(strArray3[8]),
                            Char = Convert.ToByte(strArray3[9]),
                            Level = Convert.ToByte(strArray3[10]),
                            SetNum = Convert.ToInt32(strArray3[11]),
                            PkgNum = Convert.ToInt16(strArray3[12]),
                            PicOffset = Convert.ToInt64(strArray3[13]),
                            Attr = attrList,
                            Sex = Convert.ToByte(strArray3[14]),
                            Occupation = Convert.ToInt16(strArray3[15]),
                            Chars = Convert.ToInt32(strArray3[16]),
                            Slot = Convert.ToInt32(strArray3[17])
                        });
                    }
                    else
                    {
                        ItemStone.Add(new ItemStatus
                        {
                            ItemNum = Convert.ToInt32(strArray3[0]),
                            Name = strArray3[1],
                            NameCh = nameCH,
                            Avatar = Convert.ToByte(strArray3[3]),
                            // Position = Convert.ToUInt16(strArray3[4]),
                            Position2 = Convert.ToUInt16(strArray3[5]),
                            // ItemKind = Convert.ToUInt16(strArray3[6]),
                            Point = Convert.ToByte(strArray3[7]),
                            Id = Convert.ToInt32(strArray3[8]),
                            Char = Convert.ToByte(strArray3[9]),
                            Level = Convert.ToByte(strArray3[10]),
                            SetNum = Convert.ToInt32(strArray3[11]),
                            PkgNum = Convert.ToInt16(strArray3[12]),
                            PicOffset = Convert.ToInt64(strArray3[13]),
                            Attr = attrList,
                            Sex = Convert.ToByte(strArray3[14]),
                            Occupation = Convert.ToInt16(strArray3[15]),
                            Chars = Convert.ToInt32(strArray3[16]),
                            Slot = Convert.ToInt32(strArray3[17])

                        });
                    }
                }
                else
                {
                    // 资料不完全
                    ItemRest.Add(new ItemStatus2
                    {
                        ItemNum = Convert.ToInt32(strArray3[0]),
                        Name = strArray3[1],
                        NameCh = nameCH,
                        PkgNum = Convert.ToInt16(strArray3[3]),
                        PicOffset = Convert.ToInt64(strArray3[4])
                    });
                }
            }
        }

        /// <summary>
        /// 读取itemsetdata
        /// </summary>
        private static void GetItemSetData()
        {
            string path = Path + "itemsetdata.txt";
            string[] strArray1 = File.ReadAllLines(path, Encoding.UTF8);
            for (int index1 = 1; index1 < strArray1.Length; ++index1)
            {
                string[] strArray2 = strArray1[index1].Split('|');
                string[] strArray3 = strArray2[0].Split(',');
                List<SetAttr> attrList = new List<SetAttr>();
                List<int> memberList = new List<int>();
                if (strArray2[1].Length != 0)
                {
                    // strArray2[1]空值不处理，非空才会处理
                    string[] strArray4 = strArray2[1].Split(',');
                    for (int index2 = 0; index2 < strArray4.Length; index2 += 3)
                    {
                        attrList.Add(new SetAttr
                        {
                            CompleteKey = Convert.ToUInt16(strArray4[index2]),
                            Num = Convert.ToInt32(strArray4[index2 + 1]),
                            Value = Convert.ToSingle(strArray4[index2 + 2])
                        });
                    }
                }

                if (strArray2[2].Length != 0)
                {
                    string[] strArray5 = strArray2[2].Split(',');
                    foreach (var t in strArray5)
                    {
                        memberList.Add(Convert.ToInt32(t));
                    }
                }

                ItemSet.Add(new ItemSetStatus
                {
                    SetNum = Convert.ToInt32(strArray3[0]),
                    Name = strArray3[1],
                    NameCh = strArray3[2],
                    Avatar = Convert.ToByte(strArray3[3]),
                    Count = Convert.ToByte(strArray3[4]),
                    Attr = attrList,
                    Member = memberList
                });
            }
        }

        /// <summary>
        /// 读取itemdata2
        /// </summary>
        private static void GetItemData2()
        {
            string path = Path + "itemdata2.txt";
            string[] strArray1 = File.ReadAllLines(path, Encoding.UTF8);
            string pattern1 = @"패키지$";
            string pattern2 = @"팩$";
            string pattern3 = @"팀 보상 상자$";
            string pattern4 = @"세트$";
            string pattern5 = @"세트S$";
            string pattern6 = @"세트A$";
            string pattern7 = @"개 세트$"; // 金币箱子也以此结尾
            string pattern8 = @"일차 상자$";
            string pattern9 = @"손오공의 선물$";
            string pattern10 = @"TR 상자$";
            string pattern11 = @"UP$";
            string pattern12 = @"^PC방 선물꾸러미";
            string pattern13 = @"흥부와 놀부1 상자";
            string pattern14 = @"흥부와 놀부2 상자";
            string pattern15 = @"해와 달 상자";
            string pattern16 = @"제크와 콩나무 상자";
            string pattern17 = @"복숭아 동자 상자";
            string pattern18 = @"알라딘 상자";
            string pattern19 = @"개구리 왕자 상자";
            string pattern20 = @"설녀 상자";
            string pattern21 = @"알리바바 상자";
            string pattern22 = @"피터팬 상자";
            string pattern23 = @"피리부는 사나이 상자";
            string pattern24 = @"이상한 나라의 앨리스 상자";
            string pattern25 = @"피노키오 모험 상자";
            string pattern26 = @"^포장된";
            string pattern27 = @"^실속 한가위 보따리";
            string pattern28 = @"^고급 한가위 보따리";
            string pattern29 = @"^봉인된 미니";
            string pattern30 = @"^봉인된 유령";
            string pattern31 = @"10억 증서";
            string pattern32 = @"학년 진급 보상$";
            string pattern33 = @"^버닝";
            string pattern34 = @"경험치 상자$";
            string pattern35 = @"도깨비 상자$";
            string pattern36 = @"연오 상자$";
            string pattern37 = @"보급품 세트";

            for (int index1 = 1; index1 < strArray1.Length; ++index1)
            {
                string[] strArray2 = strArray1[index1].Split('|');
                string[] strArray3 = strArray2[0].Split(',');
                int index = ItemRest.FindIndex(item => item.ItemNum.Equals(Convert.ToInt32(strArray3[0])));
                bool flag = !Regex.IsMatch(ItemRest[index].Name, pattern1) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern2) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern3) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern4) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern5) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern6) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern8) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern9) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern10) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern11) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern12) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern13) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern14) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern15) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern16) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern17) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern18) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern19) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern20) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern21) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern22) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern22) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern23) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern24) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern25) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern26) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern27) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern28) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern29) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern30) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern31) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern32) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern33) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern34) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern35) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern36) &&
                            !Regex.IsMatch(ItemRest[index].Name, pattern37);
                if (!flag)
                {
                    flag = Regex.IsMatch(ItemRest[index].Name, pattern7);
                }

                if (flag)
                {
                    List<int> memberList1 = new List<int>();
                    List<float> weightList1 = new List<float>();
                    List<int> memberList2 = new List<int>();
                    List<float> weightList2 = new List<float>();
                    List<int> memberList3 = new List<int>();
                    List<float> weightList3 = new List<float>();
                    if (strArray2[1].Length != 0)
                    {
                        // strArray2[1]空值不处理，非空才会处理
                        string[] strArray4 = strArray2[1].Split(',');
                        for (int index2 = 0; index2 < strArray4.Length; index2 += 2)
                        {
                            memberList1.Add(Convert.ToInt32(strArray4[index2]));
                            weightList1.Add(Convert.ToSingle(strArray4[index2 + 1]));
                        }
                    }

                    if (strArray2[2].Length != 0)
                    {
                        // strArray2[1]空值不处理，非空才会处理
                        string[] strArray4 = strArray2[2].Split(',');
                        for (int index2 = 0; index2 < strArray4.Length; index2 += 2)
                        {
                            memberList2.Add(Convert.ToInt32(strArray4[index2]));
                            weightList2.Add(Convert.ToSingle(strArray4[index2 + 1]));
                        }
                    }

                    if (strArray2[3].Length != 0)
                    {
                        // strArray2[1]空值不处理，非空才会处理
                        string[] strArray4 = strArray2[3].Split(',');
                        for (int index2 = 0; index2 < strArray4.Length; index2 += 2)
                        {
                            memberList3.Add(Convert.ToInt32(strArray4[index2]));
                            weightList3.Add(Convert.ToSingle(strArray4[index2 + 1]));
                        }
                    }

                    ItemBox.Add(new ItemBoxStatus
                    {
                        BoxNum = Convert.ToInt32(strArray3[0]),
                        WeightTotalBasic = Convert.ToSingle(strArray3[1]),
                        WeightTotalSilver = Convert.ToSingle(strArray3[2]),
                        WeightTotalGold = Convert.ToSingle(strArray3[3]),
                        SilverRate = Convert.ToSingle(strArray3[4]),
                        Position = Convert.ToInt32(strArray3[5]),
                        MemberBasic = memberList1,
                        WeightBasic = weightList1,
                        MemberSilver = memberList2,
                        WeightSilver = weightList2,
                        MemberGold = memberList3,
                        WeightGold = weightList3
                    });
                }
            }
        }

        internal static string GetDataVersion()
        {
            return _dataVer;
        }

        #endregion

        #region 列表视图相关

        internal static List<ListViewItem> GetListViewItems()
        {
            return _listViewItems;
        }

        /// <summary>
        /// 将条目添加到列表视图中
        /// </summary>
        /// <param name="itemst">要添加的条目</param>
        /// <param name="index">添加在的位置</param>
        private static void AddListView_Item(ItemStatus itemst, int index)
        {
            string str = itemst.GetName();
            ListViewItem owner = new ListViewItem(itemst.ItemNum.ToString());
            owner.SubItems.Add(new ListViewItem.ListViewSubItem(owner, str));
            owner.Tag = index;
            _listViewItems.Add(owner);
        }

        internal static void SetItem(int tagItem)
        {
            GetItemStatus(tagItem);
            GetItemImage(tagItem);
        }

        #endregion

        #region TabPage1相关

        /// <summary>
        /// 获取道具信息和属性
        /// </summary>
        /// <param name="tagItem">道具索引</param>
        internal static string[] GetItemStatus(int tagItem)
        {
            string[] strings = new string[12];

            strings[0] = Resources.Attr_Name + Item[tagItem].Name;
            strings[1] = Resources.Attr_NameCH + Item[tagItem].NameCh;
            strings[2] = Resources.Attr_Position + Item[tagItem].GetPosition();
            strings[3] = Resources.Attr_ItemNum + Item[tagItem].ItemNum;
            strings[4] = Resources.Attr_CollectLv + (Item[tagItem].Point == 0 ? "No" : StaticVars.Level[Item[tagItem].Level]);
            strings[5] = Resources.Attr_CollectNum + Item[tagItem].Id;
            strings[6] = Resources.Attr_CollectPt + Item[tagItem].Point;
            strings[11] = "";
            for (int i = 0; i < Characters; i++)
            {
                if ((Item[tagItem].Chars & (1 << i)) != 0)
                {
                    strings[11] = strings[11] + ItemStatus.GetChar(i + 1) + "\n";
                }
            }

            if (Item[tagItem].Char == 0)
            {
                strings[7] = Resources.Attr_Character + Item[tagItem].GetSex();
            }
            else
            {
                strings[7] = Resources.Attr_Character + Item[tagItem].GetChar() + (Item[tagItem].Position2 == 1001
                    ? " (" + Item[tagItem].GetSex().Substring(0, 1) + ")"
                    : "");
            }

            // 装备属性
            Itemdesc.Length = 0;
            Itemdesc.Append(Item[tagItem].GetAvatar()).Append("\r\n");
            foreach (var t in Item[tagItem].Attr)
            {
                Itemdesc.Append(t);
            }

            strings[8] = Itemdesc.ToString();

            int tagSet = ItemSet.FindIndex(item => item.SetNum == Item[tagItem].SetNum);
            // 套装属性
            if (tagSet != -1)
            {
                Itemdesc.Length = 0;
                Itemdesc.Append(ItemSet[tagSet].GetAvatar()).Append("\r\n");
                foreach (var t in ItemSet[tagSet].Attr)
                {
                    if (t.CompleteKey != 0)
                    {
                        break;
                    }

                    Itemdesc.Append(t);
                }

                strings[9] = Itemdesc.ToString();

                // 部分套装属性
                Itemdesc.Length = 0;
                Itemdesc.Append(ItemSet[tagSet].GetAvatar()).Append("\r\n");
                for (int index = 0; index < ItemSet[tagSet].Attr.Count; ++index)
                {
                    if (ItemSet[tagSet].Attr[index].CompleteKey != 0)
                    {
                        if ((index > 0 &&
                             (ItemSet[tagSet].Attr[index - 1].CompleteKey !=
                              ItemSet[tagSet].Attr[index].CompleteKey)) ||
                            index == 0)
                        {
                            int[] part = new int[10];
                            string s = Convert
                                .ToString(ItemSet[tagSet].Attr[index].CompleteKey, 2).PadLeft(13, '0'); // 二进制变换
                            int k = 0;
                            for (int l = 0; s.IndexOf("1", k, StringComparison.Ordinal) != -1; l++)
                            {
                                k = s.IndexOf("1", k, StringComparison.Ordinal);
                                part[l + 1] = 13 - k;
                                part[0] = l + 1; // 返回-1的情形不计入
                                k++;
                            }

                            Itemdesc.Append("\r\n[");
                            for (int x = 1; x <= part[0]; x++)
                            {
                                Itemdesc.Append(ItemStatus.GetPosition(part[x]));
                                Itemdesc.Append(x != part[0] ? " + " : "]\r\n");
                            }
                        }

                        Itemdesc.Append(ItemSet[tagSet].Attr[index]);
                    }
                }

                strings[10] = Itemdesc.ToString();
            }

            return strings;
        }

        /// <summary>
        /// 设置属性页图片
        /// </summary>
        /// <param name="tagItem">道具索引</param>
        internal static System.Drawing.Image[] GetItemImage(int tagItem)
        {
            System.Drawing.Image[] images = new System.Drawing.Image[11];
            images[0] = ShowDds(Item[tagItem].PicOffset, Item[tagItem].PkgNum);

            int tagSet = ItemSet.FindIndex(item => item.SetNum == Item[tagItem].SetNum);
            // 设置套装页签的图片显示
            if (tagSet != -1)
            {
                for (int i = 0; i < ItemSet[tagSet].Member.Count; i++)
                {
                    int tagSetItem = Item.FindIndex(item =>
                        item.ItemNum.Equals(ItemSet[tagSet].Member[i])); // 找不到时idx返回-1
                    if (tagSetItem != -1)
                    {
                        images[i + 1] = ShowDds(Item[tagSetItem].PicOffset, Item[tagSetItem].PkgNum);
                    }
                }

                for (int i = ItemSet[tagSet].Member.Count; i < 10; i++)
                {
                    images[i + 1] = null;
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    images[i + 1] = null;
                }
            }

            return images;
        }

        internal static int PictureBoxd_Click(int boxNum, int numItem)
        {
            int tagSet = GetItemSetTagByItem(numItem);

            if (tagSet != -1)
            {
                int int32;
                if (!_search) // 未进行部位搜索时点击不会进行跳转
                {
                    if (boxNum < ItemSet[tagSet].Count)
                    {
                        try
                        {
                            int32 = ItemSet[tagSet].Member[boxNum];
                        }
                        catch
                        {
                            return -1;
                        }

                        for (int index = 0; index < Item.Count; ++index)
                        {
                            if (Item[index].ItemNum == int32)
                            {
                                return index;
                            }
                        }
                    }
                }
                else // 进行部位搜索时 点击有可能不会进行跳转
                {
                    if (boxNum < ItemSet[tagSet].Count)
                    {
                        try
                        {
                            int32 = ItemSet[tagSet].Member[boxNum];
                        }
                        catch
                        {
                            return -1;
                        }

                        for (int index = 0; index < _listViewItems.Count; ++index)
                        {
                            if (Convert.ToInt32(_listViewItems[index].SubItems[0].Text) == int32)
                            {
                                return index;
                            }
                        }
                    }
                }
            }

            return -1;
        }

        #endregion

        #region TabPage2相关

        /// <summary>
        /// 点击韩文单选按钮动作响应
        /// </summary>
        internal static void radioButton3_CheckedChanged()
        {
            _showCn = false;
            _listViewItems = new List<ListViewItem>();

            if (!_search)
            {
                for (int index = 0; index < Item.Count; ++index)
                {
                    AddListView_Item(Item[index], index);
                }
            }
            else
            {
                for (int index = 0; index < ListResult.Count; ++index)
                {
                    AddListView_Item(Item[ListResult[index]], ListResult[index]);
                }
            }
        }

        internal static void radioButton4_CheckedChanged()
        {
            _showCn = true;
            _listViewItems = new List<ListViewItem>();
            if (!_search)
            {
                for (int index = 0; index < Item.Count; ++index)
                {
                    AddListView_Item(Item[index], index);
                }
            }
            else
            {
                for (int index = 0; index < ListResult.Count; ++index)
                {
                    AddListView_Item(Item[ListResult[index]], ListResult[index]);
                }
            }
        }

        internal static void FindItemList(int[] indexInts, float[] indexFloats, bool[] indexBools)
        {
            ListResult.Clear();
            // 增加了搜索条目，增加了搜索宠物时可以搜索到阿尔坤
            int selectedIndex1 = indexInts[0];
            int selectedIndex2 = indexInts[1];
            int selectedIndex3 = indexInts[2];
            int selectedIndex4 = indexInts[3];
            int selectedIndex5 = indexInts[4];
            int selectedIndex6 = indexInts[5];
            int mode = indexInts[6];
            float single1 = indexFloats[0];
            float single2 = indexFloats[1];
            float single3 = indexFloats[2];
            int int32 = indexInts[7];
            bool check1 = indexBools[0];
            bool check2 = indexBools[1];
            bool check3 = indexBools[2];
            bool radio1 = indexBools[3];
            bool radio2 = indexBools[4];
            bool radio3 = indexBools[5];
            bool radio4 = indexBools[6];
            bool radio5 = indexBools[7];
            bool radio6 = indexBools[8];
            bool radio7 = indexBools[9];
            bool checkSSS = indexBools[10];
            bool checkSS = indexBools[11];
            bool checkS = indexBools[12];
            bool checkA = indexBools[13];
            bool checkB = indexBools[14];
            bool checkC = indexBools[15];
            bool checkNo = indexBools[16];


            _listViewItems = new List<ListViewItem>();
            int[] numArray =
            {
                // 长度18
                0, 2001, 2002, 2003, 2004, 3001, 3002, 3004, 3005, 3003, 4001, 1003, 3008, 3009,
                3007, 5010, 5006, 1001
            };
            // 仅搜索相等时没必要排序
            bool flag = !((check1 && selectedIndex3 != 0) ||
                          (check2 && selectedIndex5 != 0) ||
                          (check3 && selectedIndex6 != 0));

            List<ItemStatus> listSort = new List<ItemStatus>();
            for (int index = 0; index < Item.Count; ++index)
            {
                bool flag1 = true;
                bool flag2 = true;
                bool flag3 = true;

                if (((uint)selectedIndex1 != 0 &&
                     (uint)Item[index].Position2 != numArray[selectedIndex1]) ||
                    (mode != 0 && (mode != 1 || Item[index].Avatar != 0) &&
                     (mode != 2 || Item[index].Avatar != 1))) // 不符合部位和内外装
                {
                    continue;
                }

                switch (Item[index].Level)
                {
                    case 0:
                        if (!checkC && !checkNo)
                        {
                            continue;
                        }
                        else if ((!checkNo && Item[index].Point == 0) || (!checkC && Item[index].Point != 0))
                        {
                            continue;
                        }

                        break;
                    case 1:
                        if (!checkC && !checkNo)
                        {
                            continue;
                        }
                        else if ((!checkNo && Item[index].Point == 0) || (!checkC && Item[index].Point != 0))
                        {
                            continue;
                        }

                        break;
                    case 2:
                        if (!checkB)
                        {
                            continue;
                        }

                        break;
                    case 3:
                        if (!checkA)
                        {
                            continue;
                        }

                        break;
                    case 4:
                        if (!checkS)
                        {
                            continue;
                        }

                        break;
                    case 5:
                        if (!checkSS)
                        {
                            continue;
                        }

                        break;
                    case 6:
                        if (!checkSSS)
                        {
                            continue;
                        }

                        break;
                    default: break;
                }

                // 自然排序
                if (radio7 || flag)
                {
                    if (check1)
                    {
                        flag1 = GetIsStatus(index, selectedIndex2,
                            selectedIndex3, single1);
                    }

                    if (check2)
                    {
                        flag2 = GetIsStatus(index, selectedIndex4,
                            selectedIndex5, single2);
                    }

                    if (check3)
                    {
                        if (ItemAttrInfo.ContainsKey(int32))
                        {
                            flag3 = GetIsStatus(index, int32, selectedIndex6,
                                single3, false);
                        }
                        else
                        {
                            _search = false;
                            return;
                        }
                    }

                    if (flag1 && flag2 && flag3)
                    {
                        AddListView_Item(Item[index], index); // 符合条件的道具写入列表
                        ListResult.Add(index);
                    }
                }
                else
                {
                    if (check1)
                    {
                        flag1 = GetIsStatus(index, selectedIndex2,
                            selectedIndex3, single1);
                    }

                    if (check2)
                    {
                        flag2 = GetIsStatus(index, selectedIndex4,
                            selectedIndex5, single2);
                    }

                    if (check3)
                    {
                        flag3 = GetIsStatus(index, int32, selectedIndex6,
                            single3, false);
                    }

                    if (flag1 && flag2 && flag3)
                    {
                        listSort.Add(Item[index]); // 符合条件的道具写入列表
                    }
                }
            }

            int[] attrNum =
            {
                2, 1, 38, 37, 9, 17, 18, 19, 20, 3, 7, 231, 181, 180, 5, 26, 214, 246, 245, 244,
                286, 287, 288
            };
            if (radio1)
            {
                listSort.Sort(delegate (ItemStatus a, ItemStatus b)
                {
                    int attra = a.Attr.FindIndex(item => item.Num.Equals(attrNum[selectedIndex2]));
                    int attrb = b.Attr.FindIndex(item => item.Num.Equals(attrNum[selectedIndex2]));
                    if (a.Attr[attra].Value.CompareTo(b.Attr[attrb].Value) == 0)
                    {
                        return a.ItemNum.CompareTo(b.ItemNum);
                    }

                    return a.Attr[attra].Value.CompareTo(b.Attr[attrb].Value);
                });
            }

            if (radio2)
            {
                // int num4 = (int) MessageBox.Show("RB6", "Debug");
                listSort.Sort(delegate (ItemStatus a, ItemStatus b)
                {
                    int attra = a.Attr.FindIndex(item => item.Num.Equals(attrNum[selectedIndex2]));
                    int attrb = b.Attr.FindIndex(item => item.Num.Equals(attrNum[selectedIndex2]));
                    if (a.Attr[attra].Value.CompareTo(b.Attr[attrb].Value) == 0)
                    {
                        return -a.ItemNum.CompareTo(b.ItemNum);
                    }

                    return -a.Attr[attra].Value.CompareTo(b.Attr[attrb].Value);
                });
            }

            if (radio3)
            {
                listSort.Sort(delegate (ItemStatus a, ItemStatus b)
                {
                    int attra = a.Attr.FindIndex(item => item.Num.Equals(attrNum[selectedIndex4]));
                    int attrb = b.Attr.FindIndex(item => item.Num.Equals(attrNum[selectedIndex4]));
                    if (a.Attr[attra].Value.CompareTo(b.Attr[attrb].Value) == 0)
                    {
                        return a.ItemNum.CompareTo(b.ItemNum);
                    }

                    return a.Attr[attra].Value.CompareTo(b.Attr[attrb].Value);
                });
            }

            if (radio4)
            {
                listSort.Sort(delegate (ItemStatus a, ItemStatus b)
                {
                    int attra = a.Attr.FindIndex(item => item.Num.Equals(attrNum[selectedIndex4]));
                    int attrb = b.Attr.FindIndex(item => item.Num.Equals(attrNum[selectedIndex4]));
                    if (a.Attr[attra].Value.CompareTo(b.Attr[attrb].Value) == 0)
                    {
                        return -a.ItemNum.CompareTo(b.ItemNum);
                    }

                    return -a.Attr[attra].Value.CompareTo(b.Attr[attrb].Value);
                });
            }

            if (radio5)
            {
                listSort.Sort(delegate (ItemStatus a, ItemStatus b)
                {
                    int attra = a.Attr.FindIndex(item => item.Num.Equals(int32));
                    int attrb = b.Attr.FindIndex(item => item.Num.Equals(int32));
                    if (a.Attr[attra].Value.CompareTo(b.Attr[attrb].Value) == 0)
                    {
                        return a.ItemNum.CompareTo(b.ItemNum);
                    }

                    return a.Attr[attra].Value.CompareTo(b.Attr[attrb].Value);
                });
            }

            if (radio6)
            {
                listSort.Sort(delegate (ItemStatus a, ItemStatus b)
                {
                    int attra = a.Attr.FindIndex(item => item.Num.Equals(int32));
                    int attrb = b.Attr.FindIndex(item => item.Num.Equals(int32));
                    if (a.Attr[attra].Value.CompareTo(b.Attr[attrb].Value) == 0)
                    {
                        return -a.ItemNum.CompareTo(b.ItemNum);
                    }

                    return -a.Attr[attra].Value.CompareTo(b.Attr[attrb].Value);
                });
            }

            foreach (ItemStatus t in listSort)
            {
                int i = Item.FindIndex(item => item.ItemNum.Equals(t.ItemNum));
                AddListView_Item(t, i);
                ListResult.Add(i);
            }

            listSort.Clear();

            // 视为进行了装备搜索
            _search = true;
        }

        /// <summary>
        /// 判断道具是否符合输入条件
        /// </summary>
        /// <param name="idx">道具索引</param>
        /// <param name="cindx">属性编号</param>
        /// <param name="cindx2">大于等于小于</param>
        /// <param name="value">数值</param>
        /// <param name="def">是否是自定义属性编号</param>
        /// <returns>表示是否符合条件的布尔变量</returns>
        private static bool GetIsStatus(int idx, int cindx, int cindx2, float value, bool def = true)
        {
            int index1;
            if (def)
            {
                switch (cindx)
                {
                    case 0:
                        index1 = 2;
                        break;
                    case 1:
                        index1 = 1;
                        break;
                    case 2:
                        index1 = 38;
                        break;
                    case 3:
                        index1 = 37;
                        break;
                    case 4:
                        index1 = 9;
                        break;
                    case 5:
                        index1 = 17;
                        break;
                    case 6:
                        index1 = 18;
                        break;
                    case 7:
                        index1 = 19;
                        break;
                    case 8:
                        index1 = 20;
                        break;
                    case 9:
                        index1 = 3;
                        break;
                    case 10:
                        index1 = 7;
                        break;
                    case 11:
                        index1 = 231;
                        break;
                    case 12:
                        index1 = 181;
                        break;
                    case 13:
                        index1 = 180;
                        break;
                    case 14:
                        index1 = 5;
                        break;
                    case 15:
                        index1 = 26;
                        break;
                    case 16:
                        index1 = 214;
                        break;
                    case 17:
                        index1 = 246;
                        break;
                    case 18:
                        index1 = 245;
                        break;
                    case 19:
                        index1 = 244;
                        break;
                    case 20:
                        index1 = 286;
                        break;
                    case 21:
                        index1 = 287;
                        break;
                    case 22:
                        index1 = 288;
                        break;
                    default:
                        return false;
                }
            }
            else
            {
                index1 = cindx;
            }

            value /= ItemAttrInfo[index1].Type;
            switch (cindx2)
            {
                case 0:
                    foreach (var t in Item[idx].Attr)
                    {
                        if (t.Num == index1 &&
                            t.Value == (double)value)
                        {
                            return true;
                        }
                    }

                    break;
                case 1:
                    foreach (var t in Item[idx].Attr)
                    {
                        if (t.Num == index1 &&
                            t.Value <= (double)value)
                        {
                            return true;
                        }
                    }

                    break;
                case 2:
                    foreach (var t in Item[idx].Attr)
                    {
                        if (t.Num == index1 &&
                            t.Value >= (double)value)
                        {
                            return true;
                        }
                    }

                    break;
                default:
                    return false;
            }

            return false;
        }

        /// <summary>
        /// 恢复全部按钮动作响应函数
        /// </summary>
        internal static void button3_Click()
        {
            _listViewItems = new List<ListViewItem>();
            for (int index = 0; index < Item.Count; ++index)
            {
                AddListView_Item(Item[index], index);
            }

            _search = false;
        }

        /// <summary>
        /// 搜索名称按钮动作响应
        /// </summary>
        internal static void button1_Click_string(string text)
        {
            ListResult.Clear();
            _listViewItems = new List<ListViewItem>();
            for (int index = 0; index < Item.Count; ++index)
            {
                if (Item[index].GetName().Contains(text ?? string.Empty))
                {
                    AddListView_Item(Item[index], index);
                    ListResult.Add(index);
                }
            }

            _search = true;
        }

        /// <summary>
        /// 搜索编号按钮动作响应
        /// </summary>
        internal static int button1_Click_Num(bool bools, int numItem)
        {
            bool radio1 = bools;
            int tag = -1;
            // 若进行装备搜索时则搜索虚拟列表而非List

            if (!_search)
            {
                tag = radio1
                    ? Item.FindIndex(item => item.ItemNum == numItem)
                    : Item.FindIndex(item => item.Id == numItem);
            }
            else
            {
                // 进行具体搜索时的搜藏编号搜索
                if (radio1)
                {
                    for (int index = 0; index < _listViewItems.Count; ++index)
                    {
                        if (Convert.ToInt32(_listViewItems[index].Text) == numItem)
                        {
                            tag = index;
                        }
                    }
                }
                else
                {
                    int numCollect = Item[Item.FindIndex(item => item.ItemNum == numItem)].Id;

                    for (int index = 0; index < _listViewItems.Count; ++index)
                    {
                        if (Convert.ToInt32(_listViewItems[index].Text) == numCollect)
                        {
                            tag = index;
                        }
                    }
                }
            }

            return tag;
        }

        #endregion

        #region TabPage5配装相关

        /// <summary>
        /// 单击配装栏的图片框跳转选择道具
        /// </summary>
        /// <param name="boxNum">点击的配装图片框序号</param>
        internal static int PictureBoxdddd_Click(int boxNum)
        {
            int tagItem = 0;
            int tag = -1;
            if (boxNum > 0 && boxNum < 20)
            {
                tagItem = P1[boxNum - 1];
            }
            else if (boxNum < 0)
            {
                tagItem = P2[-boxNum - 1];
            }
            else if (boxNum > 20)
            {
                tagItem = P3[boxNum - 21];
            }

            if (tagItem >= 0)
            {
                tag = button1_Click_Num(true, Item[tagItem].ItemNum);
            }

            return tag;
        }

        /// <summary>
        /// 配装页面发生改变时对界面进行修改
        /// 模式1为添加，模式2为删除，模式3为转换，模式4为读取后显示图片
        /// </summary>
        /// <param name="mode">用于判断模式</param>
        /// <param name="a1">模式1为tag，模式2为BoxNum</param>
        internal static void ItemChanged(int mode, int a1)
        {
            // 配装点击后图片显示和消除
            if (mode == 1)
            {
                int tagItem = Item.FindIndex(item => item.ItemNum.Equals(a1));
                if (Item[tagItem].PicOffset != -1)
                {
                    // ReSharper disable once NotAccessedVariable
                    bool flag = false;
                    if (Item[tagItem].Position2 == 3007 || Item[tagItem].Position2 == 5006 ||
                        Item[tagItem].Position2 == 5010)
                    {
                        switch (Item[tagItem].Position2)
                        {
                            case 3007:
                                flag = ConditionCheck(3, 1, 22, _charNow, tagItem);
                                break;
                            case 5006:
                                flag = ConditionCheck(3, 2, 23, _charNow, tagItem);
                                break;
                            case 5010:
                                flag = ConditionCheck(3, 3, 24, _charNow, tagItem);
                                break;
                        }
                    }
                    else
                    {
                        int[] part = new int[15];
                        string s = Convert.ToString(Item[tagItem].Occupation, 2).PadLeft(14, '0'); // 二进制变换
                        int k = 0;
                        for (int l = 0; s.IndexOf("1", k, StringComparison.Ordinal) != -1; l++)
                        {
                            k = s.IndexOf("1", k, StringComparison.Ordinal);
                            part[l + 1] = 14 - k;
                            part[0] = l + 1; // 返回-1的情形不计入
                            k++;
                        }

                        for (int l = 1; l <= part[0]; l++)
                        {
                            if (_check1)
                            {
                                flag = ConditionCheck(1, part[l] - 1, part[l], _charNow, tagItem);
                            }

                            if (_check2)
                            {
                                flag = ConditionCheck(2, part[l] - 1, -part[l], _charNow, tagItem);
                            }
                        }
                    }
                }
            }

            if (mode == 2)
            {
                if (a1 > 0 && a1 <= P1.Length)
                {
                    a1 = P1[a1 - 1];
                    for (int index = 0; index < P1.Length; index++)
                    {
                        if (P1[index] == a1)
                        {
                            P1[index] = -1;
                        }
                    }
                }
                else if (a1 < 0)
                {
                    a1 = P2[-a1 - 1];
                    for (int index = 0; index < P2.Length; index++)
                    {
                        if (P2[index] == a1)
                        {
                            P2[index] = -1;
                        }
                    }
                }
                else
                {
                    a1 = P3[a1 - 21];
                    for (int index = 1; index < P3.Length; index++)
                    {
                        if (P3[index] == a1)
                        {
                            P3[index] = -1;
                        }
                    }
                }
            }

            if (mode == 3)
            {
                //bool flag = false;
                for (int i = 0; i < StaticVars.Positions; i++)
                {
                    if (P1[i] >= 0)
                    {
                        if (Item[P1[i]].Char != 0)
                        {
                            if (Item[P1[i]].Char != (_charNow + 1))
                            {
                                ItemChanged(2, i + 1);
                            }
                        }
                        else
                        {
                            string str = Convert.ToString(Item[P1[i]].Chars, 2).PadLeft(Characters, '0');
                            if (str.Substring(Characters - _charNow - 1, 1).Equals("0"))
                            {
                                ItemChanged(2, i + 1);
                            }
                        }
                    }

                    if (P2[i] >= 0)
                    {
                        if (Item[P2[i]].Char != 0)
                        {
                            if (Item[P2[i]].Char != (_charNow + 1))
                            {
                                ItemChanged(2, -i - 1);
                            }
                        }
                        else
                        {
                            string str = Convert.ToString(Item[P2[i]].Chars, 2).PadLeft(Characters, '0');
                            if (str.Substring(Characters - _charNow - 1, 1).Equals("0"))
                            {
                                ItemChanged(2, -i - 1);
                            }
                        }
                    }
                }

                for (int i = 1; i < 4; i++)
                {
                    if (P3[i] >= 0)
                    {
                        if (Item[P3[i]].Char != 0)
                        {
                            if (Item[P3[i]].Char != (_charNow + 1))
                                ItemChanged(2, i + 21);
                        }
                        else
                        {
                            string str = Convert.ToString(Item[P3[i]].Chars, 2).PadLeft(Characters, '0');
                            if (str.Substring(Characters - _charNow - 1, 1).Equals("0"))
                            {
                                ItemChanged(2, i + 21);
                            }
                        }
                    }
                }
            }

            // if (mode == 4)
            // {
            //     //bool flag = false;
            //     for (int index = 0; index < Positions; index++)
            //     {
            //         if (P1[index] >= 0)
            //         {
            //             ShowDds(PkgUnpack.PicFind(PathPkg, Item[P1[index]].PicOffset), GetpictureBoxdddd(index + 1));
            //         }
            //         else
            //         {
            //             GetpictureBoxdddd(index + 1).Image = GetpictureBoxddddDefaultPic(index + 1);
            //         }
            //         if (P2[index] >= 0)
            //         {
            //             ShowDds(PkgUnpack.PicFind(PathPkg, Item[P2[index]].PicOffset), GetpictureBoxdddd(-index - 1));
            //         }
            //         else
            //         {
            //             GetpictureBoxdddd(-index - 1).Image = GetpictureBoxddddDefaultPic(-index - 1);
            //         }
            //     }
            //     for (int index = 1; index < 4; index++)
            //     {
            //         if (P3[index] >= 0)
            //         {
            //             ShowDds(PkgUnpack.PicFind(PathPkg, Item[P3[index]].PicOffset), GetpictureBoxdddd(index + 21));
            //         }
            //         else
            //         {
            //             GetpictureBoxdddd(index + 21).Image = GetpictureBoxddddDefaultPic(index + 21);
            //         }
            //     }
            // }
        }

        /// <summary>
        /// 返回主窗口配装需要的一切信息
        /// </summary>
        /// <returns>0-1属性文本，2-为图片文件名、偏移与道具名</returns>
        internal static string[] StatusShow()
        {
            int para = 3; // 保存的数据数量
            string[] strings = new string[2 + (4 + 2 * StaticVars.Positions) * para];
            string[] stringsA = AttrShow();
            strings[0] = stringsA[0];
            strings[1] = stringsA[1];
            for (int i = 0; i < StaticVars.Positions; i++)
            {
                if (P1[i] >= 0)
                {
                    strings[2 + i * para] = Item[P1[i]].PicOffset.ToString();
                    strings[2 + i * para + 1] = Item[P1[i]].GetName();
                    strings[2 + i * para + 2] = Item[P1[i]].PkgNum.ToString();
                }
                else
                {
                    strings[2 + i * para] = "-1";
                    strings[2 + i * para + 1] = string.Empty;
                    strings[2 + i * para + 2] = "-1";
                }

                if (P2[i] >= 0)
                {
                    strings[2 + i * para + para * StaticVars.Positions] = Item[P2[i]].PicOffset.ToString();
                    strings[2 + i * para + para * StaticVars.Positions + 1] = Item[P2[i]].GetName();
                    strings[2 + i * para + para * StaticVars.Positions + 2] = Item[P2[i]].PkgNum.ToString();
                }
                else
                {
                    strings[2 + i * para + para * StaticVars.Positions] = "-1";
                    strings[2 + i * para + para * StaticVars.Positions + 1] = string.Empty;
                    strings[2 + i * para + para * StaticVars.Positions + 2] = "-1";
                }

                if (i < 4)
                {
                    if (P3[i] >= 0)
                    {
                        strings[2 + i * para + 2 * para * StaticVars.Positions] = Item[P3[i]].PicOffset.ToString();
                        strings[2 + i * para + 2 * para * StaticVars.Positions + 1] = Item[P3[i]].GetName();
                        strings[2 + i * para + 2 * para * StaticVars.Positions + 2] = Item[P3[i]].PkgNum.ToString();
                    }
                    else
                    {
                        strings[2 + i * para + 2 * para * StaticVars.Positions] = "-1";
                        strings[2 + i * para + 2 * para * StaticVars.Positions + 1] = string.Empty;
                        strings[2 + i * para + 2 * para * StaticVars.Positions + 2] = "-1";


                    }
                }
            }

            return strings;
        }

        /// <summary>
        /// 刷新配装属性显示
        /// </summary>
        private static string[] AttrShow()
        {
            string[] strings = new string[2];
            // 装备属性判定
            ItemCheck();
            // 套装属性判定
            SetCheck();
            // 宝石属性判定
            StoneCheck();

            SortedList<int, float> attrAll = new SortedList<int, float>();
            foreach (KeyValuePair<int, float> t in AttrItem)
            {
                if (attrAll.ContainsKey(t.Key))
                {
                    attrAll[t.Key] = Plus(attrAll[t.Key], t.Value);
                }
                else
                {
                    attrAll.Add(t.Key, t.Value);
                }
            }

            foreach (KeyValuePair<int, float> t in AttrSet)
            {
                if (attrAll.ContainsKey(t.Key))
                {
                    attrAll[t.Key] = Plus(attrAll[t.Key], t.Value);
                }
                else
                {
                    attrAll.Add(t.Key, t.Value);
                }
            }

            foreach (KeyValuePair<int, float> t in AttrStone)
            {
                if (attrAll.ContainsKey(t.Key))
                {
                    attrAll[t.Key] = Plus(attrAll[t.Key], t.Value);
                }
                else
                {
                    attrAll.Add(t.Key, t.Value);
                }
            }

            Itemdesc.Length = 0;
            int attack = 0;
            int power = _pow;
            float critical = 0;
            float fury = 0;
            foreach (KeyValuePair<int, float> t in attrAll)
            {
                int num = t.Key;
                if (t.Value != 0)
                {
                    Itemdesc.Append(ItemAttrInfo[num].Desc);
                    if (ItemAttrInfo[num].Type == 100)
                    {
                        Itemdesc.Append(Multi(t.Value, 100f).ToString()).Append('%');
                    }
                    else if (ItemAttrInfo[num].Type == 0)
                    {
                        // 啥都不做
                    }
                    else
                    {
                        Itemdesc.Append(t.Value);
                    }

                    Itemdesc.Append(ItemAttrInfo[num].Desc2);
                    Itemdesc.Append("\r\n");
                }

                if (t.Key == 214)
                {
                    attack = (int)t.Value;
                }

                if (t.Key == 19)
                {
                    power += (int)t.Value;
                }

                if (t.Key == 245)
                {
                    critical = t.Value / 100;
                }

                if (t.Key == 246)
                {
                    fury = t.Value / 100;
                }
            }

            strings[0] = Itemdesc.ToString();

            Itemdesc.Length = 0;
            int level = _lv;
            int dNormal = (int)(50 + attack + power * 3 + level * 1.5);
            int dCrit = (int)(dNormal * (2 + critical));
            int dNormalF = (int)(dNormal * (1.2 + fury));
            int dCritF = (int)(dCrit * (1.2 + fury));
            Itemdesc.Append(Resources.Anubis_Normal + Resources.Anubis_Damage + " = " + dNormal);
            Itemdesc.Append("\r\n");
            Itemdesc.Append(Resources.Anubis_Crit + Resources.Anubis_Damage + " = " + dCrit);
            Itemdesc.Append("\r\n");
            Itemdesc.Append(Resources.Anubis_Fury + Resources.Anubis_Damage + " = " + dNormalF);
            Itemdesc.Append("\r\n");
            Itemdesc.Append(Resources.Anubis_Fury + Resources.Anubis_Crit + Resources.Anubis_Damage + " = " + dCritF);
            Itemdesc.Append("\r\n");
            Itemdesc.Append(Resources.Anubis_15 + Resources.Anubis_Normal + Resources.Anubis_Damage + " = " +
                            (int)Multi(1.5f, dNormal));
            Itemdesc.Append("\r\n");
            Itemdesc.Append(Resources.Anubis_15 + Resources.Anubis_Crit + Resources.Anubis_Damage + " = " +
                            (int)Multi(1.5f, dCrit));
            Itemdesc.Append("\r\n");
            Itemdesc.Append(Resources.Anubis_15 + Resources.Anubis_Fury + Resources.Anubis_Damage + " = " +
                            (int)Multi(1.5f, dNormalF));
            Itemdesc.Append("\r\n");
            Itemdesc.Append(Resources.Anubis_15 + Resources.Anubis_Fury + Resources.Anubis_Crit +
                            Resources.Anubis_Damage + " = " + (int)Multi(1.5f, dCritF));
            Itemdesc.Append("\r\n");
            strings[1] = Itemdesc.ToString();
            return strings;
        }

        /// <summary>
        /// 检查当前情况是否可以穿着该装备，若可以则进行穿着
        /// </summary>
        /// <param name="p">具体数组</param>
        /// <param name="position">数组下标</param>
        /// <param name="boxNum">图片框编号</param>
        /// <param name="char1">目前选定角色</param>
        /// <param name="tag">获取装备指定角色和可以穿着该装备的角色</param>
        private static bool ConditionCheck(int p, int position, int boxNum, int char1, int tag)
        {
            if (Item[tag].Char != 0)
            {
                if (Item[tag].Char != (char1 + 1))
                    return false;
            }
            else
            {
                string str = Convert.ToString(Item[tag].Chars, 2).PadLeft(Characters, '0');
                if (str.Substring(Characters - char1 - 1, 1).Equals("0"))
                {
                    return false;
                }
            }

            //_ = (int)MessageBox.Show(P+","+position+","+BoxNum + "," + Char1 + "," + Tag, "Debug");
            switch (p)
            {
                case 1:
                    if (P1[position] >= 0)
                    {
                        //_ = (int)MessageBox.Show("1", "Debug");
                        ItemChanged(2, boxNum);
                    }

                    P1[position] = tag;
                    break;
                case 2:
                    if (P2[position] >= 0)
                    {
                        //_ = (int)MessageBox.Show("2", "Debug");
                        ItemChanged(2, boxNum);
                    }

                    P2[position] = tag;
                    break;
                case 3:
                    if (P3[position] >= 0)
                    {
                        //_ = (int)MessageBox.Show("3","Debug");
                        ItemChanged(2, boxNum);
                    }

                    P3[position] = tag;
                    break;
            }

            return true;
        }

        /// <summary>
        /// 添加配装的道具属性
        /// </summary>
        private static void ItemCheck()
        {
            AttrItem.Clear();
            List<Attr> itemAttr;
            List<int> itemDup1 = new List<int>(); // 防止重复计算单件道具属性
            List<int> itemDup2 = new List<int>(); // 防止重复计算单件道具属性
            //int Num = Item.FindIndex(item => item.ItemNum == a1);
            for (int i = 0; i < StaticVars.Positions; i++)
            {
                if (P1[i] >= 0 && Item[P1[i]].Avatar == 0)
                {
                    // 如果已经添加过这件道具的属性就不再添加
                    if (!itemDup1.Contains(P1[i]))
                    {
                        itemAttr = Item[P1[i]].Attr;
                        foreach (var t in itemAttr)
                        {
                            if (AttrItem.ContainsKey(t.Num))
                            {
                                AttrItem[t.Num] = Plus(AttrItem[t.Num], t.Value);
                            }
                            else
                            {
                                AttrItem.Add(t.Num, t.Value);
                            }
                        }
                        itemDup1.Add(P1[i]);
                    }
                }

                if (P2[i] >= 0 && Item[P2[i]].Avatar == 1)
                {
                    if (!itemDup2.Contains(P2[i]))
                    {
                        itemAttr = Item[P2[i]].Attr;
                        foreach (var t in itemAttr)
                        {
                            if (AttrItem.ContainsKey(t.Num))
                            {
                                AttrItem[t.Num] = Plus(AttrItem[t.Num], t.Value);
                            }
                            else
                            {
                                AttrItem.Add(t.Num, t.Value);
                            }
                        }
                    }
                    itemDup2.Add(P2[i]);
                }

            }

            for (int i = 0; i < 4; i++)
            {
                if (P3[i] >= 0)
                {
                    itemAttr = Item[P3[i]].Attr;
                    foreach (var t in itemAttr)
                    {
                        if (AttrItem.ContainsKey(t.Num))
                        {
                            AttrItem[t.Num] = Plus(AttrItem[t.Num], t.Value);
                        }
                        else
                        {
                            AttrItem.Add(t.Num, t.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加配装的套装属性
        /// </summary>
        private static void SetCheck()
        {
            // 添加配装的套装属性
            AttrSet.Clear();
            List<int> itemSet = new List<int>();
            for (int i = 0; i < StaticVars.Positions; i++)
            {
                if (P1[i] >= 0)
                {
                    if (!itemSet.Contains(Item[P1[i]].SetNum))
                    {
                        int tag = ItemSet.FindIndex(item => item.SetNum == Item[P1[i]].SetNum);
                        if (tag > 0)
                        {
                            if (ItemSet[tag].Avatar == 0)
                            {
                                itemSet.Add(Item[P1[i]].SetNum);
                            }
                        }
                    }
                }

                if (P2[i] >= 0)
                {
                    if (!itemSet.Contains(Item[P2[i]].SetNum))
                    {
                        int tag = ItemSet.FindIndex(item => item.SetNum == Item[P2[i]].SetNum);
                        if (tag > 0)
                        {
                            if (ItemSet[tag].Avatar == 1)
                            {
                                itemSet.Add(Item[P2[i]].SetNum);
                            }
                        }
                    }
                }
            }

            foreach (int t in itemSet)
            {
                bool fullSet = true;
                string str = "".PadLeft(14, '0');
                int tag = ItemSet.FindIndex(item => item.SetNum == t);
                // 全套套装属性判断
                if (ItemSet[tag].Avatar == 0)
                {
                    for (int i = 0; i < ItemSet[tag].Count; i++)
                    {
                        if (!P1.Contains(Item.FindIndex(item => item.ItemNum == ItemSet[tag].Member[i])))
                        {
                            fullSet = false;
                        }
                        else
                        {
                            int[] oc = new int[15];
                            for (int j = 1; j < oc.Length; j++)
                            {
                                if (P1[j - 1] == Item.FindIndex(item => item.ItemNum == ItemSet[tag].Member[i]))
                                {
                                    oc[0]++;
                                    oc[oc[0]] = j - 1;
                                }
                            }

                            StringBuilder sb = new StringBuilder(str);
                            for (int j = 1; j <= oc[0]; j++)
                            {
                                sb.Replace('0', '1', 14 - oc[j] - 1, 1);
                            }

                            str = sb.ToString();
                        }

                        if (fullSet && (i == (ItemSet[tag].Count - 1)))
                        {
                            foreach (SetAttr attr in ItemSet[tag].Attr)
                            {
                                if (attr.CompleteKey == 0)
                                {
                                    if (AttrSet.ContainsKey(attr.Num))
                                    {
                                        AttrSet[attr.Num] = Plus(AttrSet[attr.Num], attr.Value);
                                    }
                                    else
                                    {
                                        AttrSet.Add(attr.Num, attr.Value);
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ItemSet[tag].Count; i++)
                    {
                        if (!P2.Contains(Item.FindIndex(item => item.ItemNum == ItemSet[tag].Member[i])))
                        {
                            fullSet = false;
                        }
                        else
                        {
                            int[] oc = new int[15];
                            for (int j = 1; j < oc.Length; j++)
                            {
                                if (P2[j - 1] == Item.FindIndex(item => item.ItemNum == ItemSet[tag].Member[i]))
                                {
                                    oc[0]++;
                                    oc[oc[0]] = j - 1;
                                }
                            }

                            StringBuilder sb = new StringBuilder(str);
                            for (int j = 1; j <= oc[0]; j++)
                            {
                                sb.Replace('0', '1', 14 - oc[j] - 1, 1);
                            }

                            str = sb.ToString();
                        }

                        if (fullSet && (i == (ItemSet[tag].Count - 1)))
                        {
                            foreach (SetAttr attr in ItemSet[tag].Attr)
                            {
                                if (attr.CompleteKey == 0)
                                {
                                    if (AttrSet.ContainsKey(attr.Num))
                                    {
                                        AttrSet[attr.Num] = Plus(AttrSet[attr.Num], attr.Value);
                                    }
                                    else
                                    {
                                        AttrSet.Add(attr.Num, attr.Value);
                                    }
                                }
                            }
                        }
                    }
                }

                ushort key = Convert.ToUInt16(str, 2);
                // 部分套装属性判断
                if (!fullSet)
                {
                    foreach (SetAttr attr in ItemSet[tag].Attr)
                    {
                        if (attr.CompleteKey != 0 && (attr.CompleteKey & key) == attr.CompleteKey)
                        {
                            if (AttrSet.ContainsKey(attr.Num))
                            {
                                AttrSet[attr.Num] = Plus(AttrSet[attr.Num], attr.Value);
                            }
                            else
                            {
                                AttrSet.Add(attr.Num, attr.Value);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 添加配装的宝石属性
        /// </summary>
        private static void StoneCheck()
        {
            AttrStone.Clear();
            List<int> itemDup1 = new List<int>(); // 防止重复计算单件道具属性
            List<int> itemDup2 = new List<int>(); // 防止重复计算单件道具属性
            //int Num = Item.FindIndex(item => item.ItemNum == a1);
            for (int i = 0; i < StaticVars.Positions; i++)
            {
                List<Attr> stoneAttr;
                if (P1[i] >= 0)
                {
                    if (!itemDup1.Contains(P1[i]))
                    {
                        if (Item[P1[i]].Avatar == 0)
                        {
                            int index = P1[i];
                            if (IndexStone.ContainsKey(index))
                            {
                                for (int j = 0; j < IndexStone[index].Length; j += 3)
                                {
                                    stoneAttr = StoneAttr(IndexStone[index].Substring(j, 3));
                                    if (stoneAttr != null)
                                    {
                                        foreach (var t in stoneAttr)
                                        {
                                            if (AttrStone.ContainsKey(t.Num))
                                            {
                                                AttrStone[t.Num] = Plus(AttrStone[t.Num], t.Value);
                                            }
                                            else
                                            {
                                                AttrStone.Add(t.Num, t.Value);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        itemDup1.Add(P1[i]);
                    }
                }

                if (P2[i] >= 0)
                {
                    if (!itemDup2.Contains(P2[i]))
                    {
                        if (Item[P2[i]].Avatar == 1)
                        {
                            int index = P2[i];
                            if (IndexStone.ContainsKey(index))
                            {
                                for (int j = 0; j < IndexStone[index].Length; j += 3)
                                {
                                    stoneAttr = StoneAttr(IndexStone[index].Substring(j, 3));
                                    if (stoneAttr != null)
                                    {
                                        foreach (var t in stoneAttr)
                                        {
                                            if (AttrStone.ContainsKey(t.Num))
                                            {
                                                AttrStone[t.Num] = Plus(AttrStone[t.Num], t.Value);
                                            }
                                            else
                                            {
                                                AttrStone.Add(t.Num, t.Value);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        itemDup2.Add(P2[i]);
                    }
                }
            }
        }

        internal static void SetTopSpeed(int i)
        {
            _topSpd = i;
        }

        internal static void SetAcceleration(int i)
        {
            _acce = i;
        }

        internal static void SetPower(int i)
        {
            _pow = i;
        }

        internal static void SetControl(int i)
        {
            _ctrl = i;
        }

        internal static void SetLevel(int i)
        {
            _lv = i;
        }

        internal static void SetCheck1(bool i)
        {
            _check1 = i;
        }

        internal static void SetCheck2(bool i)
        {
            _check2 = i;
        }

        #endregion

        #region TabPage5宝石相关

        /// <summary>
        /// 右击配装栏的图片框开启宝石镶嵌窗口
        /// </summary>
        /// <param name="boxNum">右击的配装图片框序号</param>
        internal static int PictureBoxdddd_RightClick(int boxNum)
        {
            int tagItem = -1;
            if (boxNum > 0 && boxNum < 20)
            {
                tagItem = P1[boxNum - 1];
            }
            else if (boxNum < 0)
            {
                tagItem = P2[-boxNum - 1];
            }

            return tagItem;
        }

        /// <summary>
        /// 获取返回的宝石信息
        /// </summary>
        /// <param name="tagItem">道具索引</param>
        /// <param name="stoneStr">宝石情况</param>
        internal static void StoneValue(int tagItem, string stoneStr)
        {
            if (IndexStone.ContainsKey(tagItem))
            {
                IndexStone[tagItem] = stoneStr;
            }
            else
            {
                IndexStone.Add(tagItem, stoneStr);
            }

            AttrShow();
        }

        /// <summary>
        /// 获取选定宝石的属性文本
        /// </summary>
        /// <param name="stoneStr">宝石编号</param>
        /// <returns>宝石属性文本</returns>
        internal static string StoneAttrStr(string stoneStr)
        {
            int stone = Convert.ToInt32(stoneStr);
            int stoneNum = stone / 100;
            int stoneColor = (stone / 10) % 10;
            int stoneLv = stone % 10;
            int tag = 0;
            switch (stoneColor)
            {
                case 1:
                    tag = 55525;
                    break; // 特殊
                case 2:
                    tag = 54605;
                    break; // 红色
                case 3:
                    tag = 54125;
                    break; // 橙色
                case 4:
                    tag = 53645;
                    break; // 绿色
                case 5:
                    tag = 55085;
                    break; // 蓝色
            }

            if (stoneLv > 1)
            {
                tag += ((stoneNum - 1) * 20 + (stoneLv - 2) * 4);
            }
            else
            {
                tag -= ((11 - stoneNum) * 4);
            }

            int index = ItemStone.FindIndex(item => item.ItemNum == tag);
            string str = string.Empty;
            if (index >= 0)
            {
                str += (ItemStone[index].GetName() + "\r\n");
            }

            foreach (Attr a in ItemStone[index].Attr)
            {
                str += (a + "\r\n");
            }

            return str;
        }

        /// <summary>
        /// 获取宝石对应的道具编号
        /// </summary>
        /// <param name="stoneStr">宝石情况</param>
        /// <returns>宝石道具属性列表</returns>
        private static List<Attr> StoneAttr(string stoneStr)
        {
            int stone = Convert.ToInt32(stoneStr);
            if (stone > 0)
            {
                int stoneNum = stone / 100;
                int stoneColor = (stone / 10) % 10;
                int stoneLv = stone % 10;
                int tag = 0;
                switch (stoneColor)
                {
                    case 1:
                        tag = 55525;
                        break; // 特殊
                    case 2:
                        tag = 54605;
                        break; // 红色
                    case 3:
                        tag = 54125;
                        break; // 橙色
                    case 4:
                        tag = 53645;
                        break; // 绿色
                    case 5:
                        tag = 55085;
                        break; // 蓝色
                }

                if (stoneLv > 1)
                {
                    tag += ((stoneNum - 1) * 20 + (stoneLv - 2) * 4);
                }
                else
                {
                    tag -= ((11 - stoneNum) * 4);
                }

                int index = ItemStone.FindIndex(item => item.ItemNum == tag);
                return ItemStone[index].Attr;
            }

            return null;
        }

        internal static int[] CharChange(int selectedIndex)
        {
            _charNow = selectedIndex;
            // long offset = Item.Find(item => item.ItemNum.Equals(CharNum[_charNow])).PicOffset; // 找不到时idx返回-1
            P3[0] = Item.FindIndex(item => item.ItemNum.Equals(StaticVars.CharNum[_charNow]));
            // 获取角色基础数值并返回
            int[] status = new int[4];
            status[0] = Convert.ToInt32(Item[P3[0]].Attr[Item[P3[0]].Attr.FindIndex(item => item.Num == 17)].Value);
            status[1] = Convert.ToInt32(Item[P3[0]].Attr[Item[P3[0]].Attr.FindIndex(item => item.Num == 18)].Value);
            status[2] = Convert.ToInt32(Item[P3[0]].Attr[Item[P3[0]].Attr.FindIndex(item => item.Num == 19)].Value);
            status[3] = Convert.ToInt32(Item[P3[0]].Attr[Item[P3[0]].Attr.FindIndex(item => item.Num == 20)].Value);
            _topSpd = status[0];
            _acce = status[1];
            _pow = status[2];
            _ctrl = status[3];
            return status;
        }

        internal static void SaveItv(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);
            StreamWriter exportFile = new StreamWriter(fs);
            for (int i = 0; i < StaticVars.Positions; i++)
            {
                exportFile.Write(P1[i] + "," + P2[i] + ",");
            }

            exportFile.Write(_charNow + ",");
            for (int i = 1; i < 4; i++)
            {
                exportFile.Write(P3[i]);
                if (i != 3)
                {
                    exportFile.Write(",");
                }
            }

            exportFile.WriteLine();
            foreach (KeyValuePair<int, string> t in IndexStone)
            {
                exportFile.Write(t.Key + "," + t.Value);
                exportFile.WriteLine();
            }

            exportFile.Close();
        }

        internal static int ReadItv(string fileName)
        {
            string[] strArray1 = File.ReadAllLines(fileName, Encoding.UTF8);
            string[] strArray2 = strArray1[0].Split(',');
            for (int i = 0; i < StaticVars.Positions * 2; i += 2)
            {
                P1[i / 2] = Convert.ToInt32(strArray2[i]);
                P2[i / 2] = Convert.ToInt32(strArray2[i + 1]);
            }

            _charNow = Convert.ToInt32(strArray2[StaticVars.Positions * 2]);
            P3[0] = Item.FindIndex(item => item.ItemNum.Equals(StaticVars.CharNum[_charNow]));
            for (int i = 1; i < 4; i++)
            {
                P3[i] = Convert.ToInt32(strArray2[i + StaticVars.Positions * 2]);
            }

            //Index_Stone.Clear();
            for (int i = 1; i < strArray1.Length; i++)
            {
                strArray2 = strArray1[i].Split(',');
                int index = Convert.ToInt32(strArray2[0]);
                if (IndexStone.ContainsKey(index))
                {
                    IndexStone[index] = strArray2[1];
                }
                else
                {
                    IndexStone.Add(index, strArray2[1]);
                }
            }

            StatusShow();
            return _charNow;
        }

        #endregion

        #region TabPage6相关

        /// <summary>
        /// 翻页用函数
        /// </summary>
        /// <param name="mode">1往前翻 2往后翻</param>
        /// <param name="page">翻得页数</param>
        internal static void BoxPageChange(int mode, int page)
        {
            int total = ItemBox.Count - 1; // -1用于防止总数正好是PerPage倍数的情况，会多出1页
            if (mode == 1)
            {
                if (_boxPage - page < 0)
                {
                    _boxPage = 0;
                }
                else
                {
                    _boxPage -= page;
                }
            }

            if (mode == 2)
            {
                if (_boxPage + page > (total / StaticVars.PerPage))
                {
                    _boxPage = (total / StaticVars.PerPage);
                }
                else
                {
                    _boxPage += page;
                }
            }
        }

        /// <summary>
        /// 显示开箱列表，模式1图片文本，模式2仅文本
        /// </summary>
        /// <param name="mode">模式</param>
        internal static string[] ShowBoxList(int mode)
        {
            int para = 3; // 传送的数据数量
            int total = ItemBox.Count;
            string[] strings = new string[para * StaticVars.PerPage + 2];
            if (mode == 1)
            {
                if (_boxPage != ((total - 1) / StaticVars.PerPage))
                {
                    for (int i = 0; i < StaticVars.PerPage; i++)
                    {
                        int index = ItemRest.FindIndex(item => item.ItemNum == ItemBox[_boxPage * StaticVars.PerPage + i].BoxNum);
                        strings[i * para] = ItemRest[index].GetName();
                        strings[i * para + 1] = ItemRest[index].PicOffset.ToString();
                        strings[i * para + 2] = ItemRest[index].PkgNum.ToString();
                    }
                }
                else
                {
                    for (int i = 0; i <= ((total - 1) % StaticVars.PerPage); i++)
                    {
                        int index = ItemRest.FindIndex(item => item.ItemNum == ItemBox[_boxPage * StaticVars.PerPage + i].BoxNum);
                        strings[i * para] = ItemRest[index].GetName();
                        strings[i * para + 1] = ItemRest[index].PicOffset.ToString();
                        strings[i * para + 2] = ItemRest[index].PkgNum.ToString();
                    }

                    for (int i = (total - 1) % StaticVars.PerPage; i < StaticVars.PerPage - 1; i++)
                    {
                        strings[i * para] = string.Empty;
                        strings[i * para + 1] = "-1";
                        strings[i * para + 2] = "-1";
                    }
                }
            }
            else if (mode == 2)
            {
                if (_boxPage != ((total - 1) / StaticVars.PerPage))
                {
                    for (int i = 0; i < StaticVars.PerPage; i++)
                    {
                        int index = ItemRest.FindIndex(item => item.ItemNum == ItemBox[_boxPage * StaticVars.PerPage + i].BoxNum);
                        strings[i * 2] = ItemRest[index].GetName();
                    }
                }
                else
                {
                    for (int i = 0; i < (total % StaticVars.PerPage); i++)
                    {
                        int index = ItemRest.FindIndex(item => item.ItemNum == ItemBox[_boxPage * StaticVars.PerPage + i].BoxNum);
                        strings[i * 2] = ItemRest[index].GetName();
                    }

                    for (int i = total % StaticVars.PerPage; i < StaticVars.PerPage; i++)
                    {
                        strings[i * 2] = string.Empty;
                    }
                }
            }

            strings[para * StaticVars.PerPage] = (_boxPage + 1) + "";
            strings[para * StaticVars.PerPage + 1] = (((total - 1) / StaticVars.PerPage) + 1) + "";
            return strings;
        }

        internal static int PictureBoxBoxd_RightClick(int boxNum)
        {
            return StaticVars.PerPage * _boxPage + boxNum - 1;
        }

        #endregion

        #region BoxForm相关

        internal static bool IsBoxable(int boxNum)
        {
            int tagBox = StaticVars.PerPage * _boxPage + boxNum - 1;
            if (tagBox >= ItemBox.Count)
            {
                return false;
            }

            return true;
        }

        internal static string GetBoxName(int tagBox)
        {
            int numItem = ItemBox[tagBox].BoxNum;
            int tagItem = ItemRest.FindIndex(item => item.ItemNum == numItem);
            return ItemRest[tagItem].GetName();
        }

        internal static List<int> GetBoxMember(int tagBox)
        {
            return ItemBox[tagBox].MemberBasic;
        }

        internal static List<int>[] GetBoxMembers(int tagBox)
        {
            List<int>[] returnLists = new List<int>[3];
            returnLists[0] = ItemBox[tagBox].MemberBasic;
            returnLists[1] = ItemBox[tagBox].MemberSilver;
            returnLists[2] = ItemBox[tagBox].MemberGold;
            return returnLists;
        }

        internal static List<float> GetBoxWeight(int tagBox)
        {
            return ItemBox[tagBox].WeightBasic;
        }

        internal static List<float>[] GetBoxWeights(int tagBox)
        {
            List<float>[] returnLists = new List<float>[3];
            returnLists[0] = ItemBox[tagBox].WeightBasic;
            returnLists[1] = ItemBox[tagBox].WeightSilver;
            returnLists[2] = ItemBox[tagBox].WeightGold;
            return returnLists;
        }

        internal static float GetBoxWeightTotal(int tagBox)
        {
            return ItemBox[tagBox].WeightTotalBasic;
        }

        internal static float[] GetBoxWeightTotals(int tagBox)
        {
            float[] returnLists = new float[3];
            returnLists[0] = ItemBox[tagBox].WeightTotalBasic;
            returnLists[1] = ItemBox[tagBox].WeightTotalSilver;
            returnLists[2] = ItemBox[tagBox].WeightTotalGold;
            return returnLists;
        }

        internal static List<short> GetBoxPkgNum(int tagBox)
        {
            List<short> picPkgNum = new List<short>();
            foreach (int t in ItemBox[tagBox].MemberBasic)
            {
                int index;
                if ((index = ItemRest.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picPkgNum.Add(ItemRest[index].PkgNum);
                }
                else if ((index = Item.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picPkgNum.Add(Item[index].PkgNum);
                }
                else
                {
                    picPkgNum.Add(-1);
                }
            }

            return picPkgNum;
        }

        internal static List<short>[] GetBoxPkgNums(int tagBox)
        {
            List<short>[] picPkgNum = new List<short>[3];
            picPkgNum[0] = new List<short>();
            picPkgNum[1] = new List<short>();
            picPkgNum[2] = new List<short>();
            foreach (int t in ItemBox[tagBox].MemberBasic)
            {
                int index;
                if ((index = ItemRest.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picPkgNum[0].Add(ItemRest[index].PkgNum);
                }
                else if ((index = Item.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picPkgNum[0].Add(Item[index].PkgNum);
                }
                else
                {
                    picPkgNum[0].Add(-1);
                }
            }

            foreach (int t in ItemBox[tagBox].MemberSilver)
            {
                int index;
                if ((index = ItemRest.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picPkgNum[1].Add(ItemRest[index].PkgNum);
                }
                else if ((index = Item.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picPkgNum[1].Add(Item[index].PkgNum);
                }
                else
                {
                    picPkgNum[1].Add(-1);
                }
            }

            foreach (int t in ItemBox[tagBox].MemberGold)
            {
                int index;
                if ((index = ItemRest.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picPkgNum[2].Add(ItemRest[index].PkgNum);
                }
                else if ((index = Item.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picPkgNum[2].Add(Item[index].PkgNum);
                }
                else
                {
                    picPkgNum[2].Add(-1);
                }
            }

            return picPkgNum;
        }

        internal static List<long> GetBoxOffset(int tagBox)
        {
            List<long> picOffset = new List<long>();
            foreach (int t in ItemBox[tagBox].MemberBasic)
            {
                int index;
                if ((index = ItemRest.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picOffset.Add(ItemRest[index].PicOffset);
                }
                else if ((index = Item.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picOffset.Add(Item[index].PicOffset);
                }
                else
                {
                    picOffset.Add(-1);
                }
            }

            return picOffset;
        }

        internal static List<long>[] GetBoxOffsets(int tagBox)
        {
            List<long>[] picOffset = new List<long>[3];
            picOffset[0] = new List<long>();
            picOffset[1] = new List<long>();
            picOffset[2] = new List<long>();
            foreach (int t in ItemBox[tagBox].MemberBasic)
            {
                int index;
                if ((index = ItemRest.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picOffset[0].Add(ItemRest[index].PicOffset);
                }
                else if ((index = Item.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picOffset[0].Add(Item[index].PicOffset);
                }
                else
                {
                    picOffset[0].Add(-1);
                }
            }

            foreach (int t in ItemBox[tagBox].MemberSilver)
            {
                int index;
                if ((index = ItemRest.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picOffset[1].Add(ItemRest[index].PicOffset);
                }
                else if ((index = Item.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picOffset[1].Add(Item[index].PicOffset);
                }
                else
                {
                    picOffset[1].Add(-1);
                }
            }

            foreach (int t in ItemBox[tagBox].MemberGold)
            {
                int index;
                if ((index = ItemRest.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picOffset[2].Add(ItemRest[index].PicOffset);
                }
                else if ((index = Item.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    picOffset[2].Add(Item[index].PicOffset);
                }
                else
                {
                    picOffset[2].Add(-1);
                }
            }

            return picOffset;
        }

        internal static List<string> GetBoxMemberName(int tagBox)
        {
            List<string> memberName = new List<string>();
            foreach (int t in ItemBox[tagBox].MemberBasic)
            {
                int index;
                if ((index = ItemRest.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    memberName.Add(ItemRest[index].GetName());
                }
                else if ((index = Item.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    memberName.Add(Item[index].GetName());
                }
                else
                {
                    memberName.Add(string.Empty);
                }
            }

            return memberName;
        }

        internal static List<string>[] GetBoxMemberNames(int tagBox)
        {
            List<string>[] memberName = new List<string>[3];
            memberName[0] = new List<string>();
            memberName[1] = new List<string>();
            memberName[2] = new List<string>();
            foreach (int t in ItemBox[tagBox].MemberBasic)
            {
                int index;
                if ((index = ItemRest.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    memberName[0].Add(ItemRest[index].GetName());
                }
                else if ((index = Item.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    memberName[0].Add(Item[index].GetName());
                }
                else
                {
                    memberName[0].Add(string.Empty);
                }
            }

            foreach (int t in ItemBox[tagBox].MemberSilver)
            {
                int index;
                if ((index = ItemRest.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    memberName[1].Add(ItemRest[index].GetName());
                }
                else if ((index = Item.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    memberName[1].Add(Item[index].GetName());
                }
                else
                {
                    memberName[1].Add(string.Empty);
                }
            }

            foreach (int t in ItemBox[tagBox].MemberGold)
            {
                int index;
                if ((index = ItemRest.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    memberName[2].Add(ItemRest[index].GetName());
                }
                else if ((index = Item.FindIndex(item => item.ItemNum == t)) != -1)
                {
                    memberName[2].Add(Item[index].GetName());
                }
                else
                {
                    memberName[2].Add(string.Empty);
                }
            }

            return memberName;
        }

        internal static int GetBoxPosition(int tagBox)
        {
            return ItemBox[tagBox].Position;
        }

        internal static float GetBoxSilverRate(int tagBox)
        {
            return ItemBox[tagBox].SilverRate;
        }

        #endregion

        #region StoneForm相关

        /// <summary>
        /// StoneForm构造用
        /// </summary>
        /// <param name="tagItem"></param>
        /// <returns>一个用于构造的字符串</returns>
        internal static string StoneSlots(int tagItem)
        {
            string str = Convert.ToString(Item[tagItem].Slot, 2).PadLeft(32, '0');
            if (!IndexStone.ContainsKey(tagItem))
            {
                str += string.Empty.PadLeft(24, '0');
            }
            else
            {
                str += IndexStone[tagItem];
            }

            return str;
        }

        internal static string GetItemName(int tag)
        {
            return Item[tag].GetName();
        }

        #endregion

        #region 道具编号转换

        internal static int GetItemTag(int numItem)
        {
            int tagItem = Item.FindIndex(item => item.ItemNum.Equals(numItem));
            return tagItem;
        }

        internal static int GetItemSetTag(int numSet)
        {
            int tagSet = ItemSet.FindIndex(item => item.SetNum.Equals(numSet));
            return tagSet;
        }

        internal static int GetItemSetTagByItem(int numItem)
        {
            int tagItem = Item.FindIndex(item => item.ItemNum.Equals(numItem));
            int numSet = Item[tagItem].SetNum;
            int tagSet = ItemSet.FindIndex(item => item.SetNum.Equals(numSet));
            return tagSet;
        }

        #endregion

        #region 数值计算

        /// <summary>
        /// 根据送入数据进行切割 可以切割itemdesc中道具说明内仍有引号的情况（最多2个引号） 有可能要修改
        /// </summary>
        /// <param name="sss"></param>
        /// <returns>字符串数组</returns>
        private static string[] StringDivide(string sss)
        {
            // 双引号开始标记
            int qutationStart = 0;
            // 双引号结束标记
            int qutationEnd = 0;
            char[] charStr = sss.ToCharArray();
            // 用于拼接字符 作为一个字段值
            StringBuilder stb = new StringBuilder();
            // 结果list
            List<string> list = new List<string>();
            int lastindex = sss.LastIndexOf('\"');
            // 逐个字符处理
            for (int i = 0; i < charStr.Length; i++)
            {
                // 在此之前还未遇到双引号并且当前的字符为\"
                if (qutationStart == 0 && charStr[i] == '\"')
                {
                    qutationStart = 1;
                    continue;
                }

                if (qutationStart == 1 && charStr[i] == '\"' && qutationEnd == 0)
                {
                    // 在此之前遇到了双引号并且当前的字符为\" 说明字段拼接该结束了
                    qutationStart = 0;
                    qutationEnd++;
                    // 当最后一个字符是双引号时，由于下次循环不会执行，所以在此保存一下
                    if (i == charStr.Length - 1 && stb.Length != 0)
                    {
                        list.Add(stb.ToString());
                        stb.Clear();
                    }

                    continue;
                }

                if (qutationStart == 1 && charStr[i] == ',' && qutationEnd == 0)
                {
                    // 处理 \"中国,北京\"这种不规范的字符串
                    stb.Append(charStr[i]);
                    continue;
                }

                if (qutationStart == 1 && charStr[i] == ',' && qutationEnd == 1 &&
                    i < lastindex)
                {
                    stb.Append(charStr[i]);
                    continue;
                }

                if (i == lastindex)
                {
                    continue;
                }

                if (charStr[i] == ',')
                {
                    // 字段结束，将拼接好的字段值存到list中
                    list.Add(stb.ToString());
                    stb.Clear();
                    continue;
                }

                // 不属于分隔符的就拼接
                stb.Append(charStr[i]);
                if (i == charStr.Length - 1 && stb.Length != 0)
                {
                    list.Add(stb.ToString());
                    stb.Clear();
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// 减少误差的加法计算
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns>float型运算结果</returns>
        internal static float Plus(float f1, float f2)
        {
            decimal d1 = (decimal)f1;
            decimal d2 = (decimal)f2;
            return (float)(d1 + d2);
        }

        /// <summary>
        /// 减少误差的减法计算
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns>float型运算结果</returns>
        internal static float Minus(float f1, float f2)
        {
            decimal d1 = (decimal)f1;
            decimal d2 = (decimal)f2;
            return (float)(d1 - d2);
        }

        /// <summary>
        /// 减少误差的乘法计算
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns>float型运算结果</returns>
        internal static float Multi(float f1, float f2)
        {
            decimal d1 = (decimal)f1;
            decimal d2 = (decimal)f2;
            return (float)(d1 * d2);
        }

        /// <summary>
        /// 减少误差的除法计算
        /// </summary>
        /// <param name="f1"></param>
        /// <param name="f2"></param>
        /// <returns>float型运算结果</returns>
        internal static float Divide(float f1, float f2)
        {
            decimal d1 = (decimal)f1;
            decimal d2 = (decimal)f2;
            return (float)(d1 / d2);
        }

        #endregion

        #region 文本翻译

        // // TODO 新增文本翻译 翻译关键字更正
        // /// <summary>
        // /// 语言变更
        // /// </summary>
        // private void SetLanguage()
        // {
        //     string str = Path + "language.ini";
        //     if (!File.Exists(str))
        //     {
        //         // int num = (int) MessageBox.Show("读取属性列表失败!", "错误");
        //         return;
        //     }
        //
        //     if (int.Parse(IniOperation.IniGetStringValue(str, "init", "language_select", null)) == 0)
        //     {
        //         radioButton3.Checked = true;
        //         _showCn = false;
        //     }
        //
        //     columnHeader1.Text =
        //         IniOperation.IniGetStringValue(str, "text", "list_item1",
        //             columnHeader1.Text);
        //     columnHeader2.Text =
        //         IniOperation.IniGetStringValue(str, "text", "list_item2",
        //             columnHeader2.Text);
        //     tabPage1_1.Text = IniOperation.IniGetStringValue(str, "text",
        //         "page1", tabPage1_1.Text);
        //     tabPage1_2.Text = IniOperation.IniGetStringValue(str, "text",
        //         "page2", tabPage1_2.Text);
        //     tabPage1_3.Text = IniOperation.IniGetStringValue(str, "text",
        //         "page3", tabPage1_3.Text);
        //     tabPage1_4.Text = IniOperation.IniGetStringValue(str, "text",
        //         "page4", tabPage1_4.Text);
        //     tabPage1_5.Text = IniOperation.IniGetStringValue(str, "text",
        //         "page5", tabPage1_5.Text);
        //     tabPage2_1.Text = IniOperation.IniGetStringValue(str, "text",
        //         "page1_1", tabPage2_1.Text);
        //     tabPage2_2.Text = IniOperation.IniGetStringValue(str, "text",
        //         "page1_2", tabPage2_2.Text);
        //     tabPage2_3.Text = IniOperation.IniGetStringValue(str, "text",
        //         "page1_3", tabPage2_3.Text);
        //     tabPage2_4.Text = IniOperation.IniGetStringValue(str, "text",
        //         "page1_4", tabPage2_4.Text);
        //     tabPage3_1.Text = IniOperation.IniGetStringValue(str, "text",
        //         "page5_1", tabPage3_1.Text);
        //     tabPage3_2.Text = IniOperation.IniGetStringValue(str, "text",
        //         "page5_2", tabPage3_2.Text);
        //     tabPage3_3.Text = IniOperation.IniGetStringValue(str, "text",
        //         "page5_3", tabPage3_3.Text);
        //     Text2 = IniOperation.IniGetStringValue(str, "text", "label2",
        //         label2.Text);
        //     Text3 = IniOperation.IniGetStringValue(str, "text", "label3",
        //         label3.Text);
        //     Text4 = IniOperation.IniGetStringValue(str, "text", "label4",
        //         label4.Text);
        //     Text5 = IniOperation.IniGetStringValue(str, "text", "label5",
        //         label5.Text);
        //     Text6 = IniOperation.IniGetStringValue(str, "text", "label6",
        //         label6.Text);
        //     Text7 = IniOperation.IniGetStringValue(str, "text", "label7",
        //         label7.Text);
        //     Text11 = IniOperation.IniGetStringValue(str, "text", "label11",
        //         label11.Text);
        //     Text12 = IniOperation.IniGetStringValue(str, "text", "label12",
        //         label12.Text);
        //     TextA1 = IniOperation.IniGetStringValue(str, "text", "labelA1",
        //         TextA1);
        //     TextA2 = IniOperation.IniGetStringValue(str, "text", "labelA2",
        //         TextA2);
        //     TextB1 = IniOperation.IniGetStringValue(str, "text", "labelB1",
        //         TextB1);
        //     TextB2 = IniOperation.IniGetStringValue(str, "text", "labelB2",
        //         TextB2);
        //     label2.Text = Text2;
        //     label3.Text = Text3;
        //     label4.Text = Text4;
        //     label5.Text = Text5;
        //     label6.Text = Text6;
        //     label7.Text = Text7;
        //     label11.Text = Text11;
        //     label12.Text = Text12;
        //     label13.Text = TextB1;
        //     groupBox1.Text = IniOperation.IniGetStringValue(str, "text", "group1", groupBox1.Text);
        //     groupBox2.Text = IniOperation.IniGetStringValue(str, "text", "group2", groupBox2.Text);
        //     groupBox3.Text = IniOperation.IniGetStringValue(str, "text", "group3", groupBox3.Text);
        //     groupBox4.Text = IniOperation.IniGetStringValue(str, "text", "group4", groupBox4.Text);
        //     groupBox5.Text = IniOperation.IniGetStringValue(str, "text", "group5", groupBox5.Text);
        //     groupBox6.Text = IniOperation.IniGetStringValue(str, "text", "group6", groupBox6.Text);
        //     radioButton1.Text = IniOperation.IniGetStringValue(str, "text",
        //         "radio1", radioButton1.Text);
        //     radioButton2.Text = IniOperation.IniGetStringValue(str, "text",
        //         "radio2", radioButton2.Text);
        //     radioButton3.Text = IniOperation.IniGetStringValue(str, "text",
        //         "radio3", radioButton3.Text);
        //     radioButton4.Text = IniOperation.IniGetStringValue(str, "text",
        //         "radio4", radioButton4.Text);
        //     radioButton5.Text = IniOperation.IniGetStringValue(str, "text",
        //         "radio5", radioButton5.Text);
        //     radioButton7.Text = radioButton5.Text;
        //     radioButton9.Text = radioButton5.Text;
        //     radioButton6.Text = IniOperation.IniGetStringValue(str, "text",
        //         "radio6", radioButton6.Text);
        //     radioButton8.Text = radioButton6.Text;
        //     radioButton10.Text = radioButton6.Text;
        //     radioButton11.Text = IniOperation.IniGetStringValue(str, "text",
        //         "radio7", radioButton11.Text); // 自然排序
        //     button1.Text = IniOperation.IniGetStringValue(str, "text",
        //         "button1", button1.Text);
        //     button2.Text = IniOperation.IniGetStringValue(str, "text",
        //         "button2", button2.Text);
        //     button3.Text = IniOperation.IniGetStringValue(str, "text",
        //         "button3", button3.Text);
        //     checkBox1.Text = IniOperation.IniGetStringValue(str, "text",
        //         "check1", checkBox1.Text);
        //     checkBox2.Text = IniOperation.IniGetStringValue(str, "text",
        //         "check2", checkBox2.Text);
        //     checkBox3.Text = IniOperation.IniGetStringValue(str, "text",
        //         "check3", checkBox3.Text);
        //     label9.Text = IniOperation.IniGetStringValue(str, "text", "label9", label9.Text);
        //     label10.Text = IniOperation.IniGetStringValue(str, "text", "label10", label10.Text);
        //     label14.Text = IniOperation.IniGetStringValue(str, "text", "label14", label14.Text);
        //     label15.Text = IniOperation.IniGetStringValue(str, "text", "label15", label15.Text);
        //     label16.Text = IniOperation.IniGetStringValue(str, "text", "label16", label16.Text);
        //     label17.Text = IniOperation.IniGetStringValue(str, "text", "label17", label17.Text);
        //     label18.Text = IniOperation.IniGetStringValue(str, "text", "label18", label18.Text);
        //     PositionOther = IniOperation.IniGetStringValue(str, "position",
        //         "all", PositionOther);
        //     PositionOther = IniOperation.IniGetStringValue(str,
        //         "position", "other", PositionOther);
        //     Position1001 =
        //         IniOperation.IniGetStringValue(str, "position", "1001",
        //             Position1001);
        //     Position1002 =
        //         IniOperation.IniGetStringValue(str, "position", "1002",
        //             Position1002);
        //     Position1003 =
        //         IniOperation.IniGetStringValue(str, "position", "1003",
        //             Position1003);
        //     Position1004 =
        //         IniOperation.IniGetStringValue(str, "position", "1004",
        //             Position1004);
        //     Position2001 =
        //         IniOperation.IniGetStringValue(str, "position", "2001",
        //             Position2001);
        //     Position2002 =
        //         IniOperation.IniGetStringValue(str, "position", "2002",
        //             Position2002);
        //     Position2003 =
        //         IniOperation.IniGetStringValue(str, "position", "2003",
        //             Position2003);
        //     Position2004 =
        //         IniOperation.IniGetStringValue(str, "position", "2004",
        //             Position2004);
        //     Position3001 =
        //         IniOperation.IniGetStringValue(str, "position", "3001",
        //             Position3001);
        //     Position3002 =
        //         IniOperation.IniGetStringValue(str, "position", "3002",
        //             Position3002);
        //     Position3003 =
        //         IniOperation.IniGetStringValue(str, "position", "3003",
        //             Position3003);
        //     Position3004 =
        //         IniOperation.IniGetStringValue(str, "position", "3004",
        //             Position3004);
        //     Position3005 =
        //         IniOperation.IniGetStringValue(str, "position", "3005",
        //             Position3005);
        //     Position3007 =
        //         IniOperation.IniGetStringValue(str, "position", "3007",
        //             Position3007);
        //     Position3008 =
        //         IniOperation.IniGetStringValue(str, "position", "3008",
        //             Position3008);
        //     Position3009 =
        //         IniOperation.IniGetStringValue(str, "position", "3009",
        //             Position3009);
        //     Position3010 =
        //         IniOperation.IniGetStringValue(str, "position", "3010",
        //             Position3010);
        //     Position4001 =
        //         IniOperation.IniGetStringValue(str, "position", "4001",
        //             Position4001);
        //     Position5006 =
        //         IniOperation.IniGetStringValue(str, "position", "5006",
        //             Position5006);
        //     Position5010 =
        //         IniOperation.IniGetStringValue(str, "position", "5010",
        //             Position5010);
        //     CharacterOther =
        //         IniOperation.IniGetStringValue(str, "character", "other",
        //             Character1);
        //     Character1 =
        //         IniOperation.IniGetStringValue(str, "character", "1",
        //             Character1);
        //     Character2 =
        //         IniOperation.IniGetStringValue(str, "character", "2",
        //             Character2);
        //     Character3 =
        //         IniOperation.IniGetStringValue(str, "character", "3",
        //             Character3);
        //     Character4 =
        //         IniOperation.IniGetStringValue(str, "character", "4",
        //             Character4);
        //     Character5 =
        //         IniOperation.IniGetStringValue(str, "character", "5",
        //             Character5);
        //     Character6 =
        //         IniOperation.IniGetStringValue(str, "character", "6",
        //             Character6);
        //     Character7 =
        //         IniOperation.IniGetStringValue(str, "character", "7",
        //             Character7);
        //     Character8 =
        //         IniOperation.IniGetStringValue(str, "character", "8",
        //             Character8);
        //     Character9 =
        //         IniOperation.IniGetStringValue(str, "character", "9",
        //             Character9);
        //     Character10 =
        //         IniOperation.IniGetStringValue(str, "character", "10",
        //             Character10);
        //     Character11 =
        //         IniOperation.IniGetStringValue(str, "character", "11",
        //             Character11);
        //     Character12 =
        //         IniOperation.IniGetStringValue(str, "character", "12",
        //             Character12);
        //     Character13 =
        //         IniOperation.IniGetStringValue(str, "character", "13",
        //             Character13);
        //     Character14 =
        //         IniOperation.IniGetStringValue(str, "character", "14",
        //             Character14);
        //     Character15 =
        //         IniOperation.IniGetStringValue(str, "character", "15",
        //             Character15);
        //     Character16 =
        //         IniOperation.IniGetStringValue(str, "character", "16",
        //             Character16);
        //     Character17 =
        //         IniOperation.IniGetStringValue(str, "character", "17",
        //             Character17);
        //     Character18 =
        //         IniOperation.IniGetStringValue(str, "character", "18",
        //             Character18);
        //     Character19 =
        //         IniOperation.IniGetStringValue(str, "character", "19",
        //             Character19);
        //     Character20 =
        //         IniOperation.IniGetStringValue(str, "character", "20",
        //             Character20);
        //     Character21 =
        //         IniOperation.IniGetStringValue(str, "character", "21",
        //             Character21);
        //     Character22 =
        //         IniOperation.IniGetStringValue(str, "character", "22",
        //             Character22);
        //     Character23 =
        //         IniOperation.IniGetStringValue(str, "character", "23",
        //             Character23);
        //     Character24 =
        //         IniOperation.IniGetStringValue(str, "character", "24",
        //             Character24);
        //     Character25 =
        //         IniOperation.IniGetStringValue(str, "character", "25",
        //             Character25);
        //     Character26 =
        //         IniOperation.IniGetStringValue(str, "character", "26",
        //             Character26);
        //     Character27 =
        //         IniOperation.IniGetStringValue(str, "character", "27",
        //             Character27);
        //     SexOther =
        //         IniOperation.IniGetStringValue(str, "sex", "other",
        //             Character1);
        //     Sex1 =
        //         IniOperation.IniGetStringValue(str, "sex", "1", Sex1);
        //     Sex2 =
        //         IniOperation.IniGetStringValue(str, "sex", "2", Sex2);
        //     Sex3 =
        //         IniOperation.IniGetStringValue(str, "sex", "3", Sex3);
        //     AnubisDamage = IniOperation.IniGetStringValue(str, "anubis", "damage", AnubisDamage);
        //     AnubisCrit = IniOperation.IniGetStringValue(str, "anubis", "crit", AnubisCrit) + " ";
        //     AnubisFury = IniOperation.IniGetStringValue(str, "anubis", "fury", AnubisFury) + " ";
        //     AnubisNormal = IniOperation.IniGetStringValue(str, "anubis", "normal", AnubisNormal) + " ";
        //     Anubis15 = IniOperation.IniGetStringValue(str, "anubis", "15", Anubis15) + " ";
        //
        //
        //     comboBox1.Items.Clear();
        //     for (int index = 1; index <= 18; ++index)
        //     {
        //         comboBox1.Items.Add(IniOperation.IniGetStringValue(str, "Text", "item1_" + index.ToString(), null));
        //     }
        //     comboBox1.SelectedIndex = 0;
        //
        //     comboBox7.Items.Clear();
        //     comboBox7.Items.Add(IniOperation.IniGetStringValue(str, "text", "item8_1", "内外装"));
        //     comboBox7.Items.Add(
        //         IniOperation.IniGetStringValue(str, "text", "item8_2",
        //             "内装"));
        //     comboBox7.Items.Add(
        //         IniOperation.IniGetStringValue(str, "text", "item8_3",
        //             "外装"));
        //     comboBox7.SelectedIndex = 0;
        //
        //     comboBox2.Items.Clear();
        //     for (int index = 1; index <= 23; ++index)
        //     {
        //         comboBox2.Items.Add(
        //             IniOperation.IniGetStringValue(str, "text", "item2_" + index.ToString(), null));
        //     }
        //     comboBox2.SelectedIndex = 0;
        //
        //     comboBox4.Items.Clear();
        //     for (int index = 1; index <= 23; ++index)
        //     {
        //         comboBox4.Items.Add(
        //             IniOperation.IniGetStringValue(str, "text",
        //                 "item2_" + index.ToString(), null));
        //     }
        //     comboBox4.SelectedIndex = 0;
        //
        //     comboBox8.Items.Clear();
        //     for (int index = 1; index <= Characters; ++index)
        //     {
        //         comboBox8.Items.Add(
        //             IniOperation.IniGetStringValue(str, "character", index.ToString(), null));
        //     }
        //     comboBox8.SelectedIndex = 0;
        //
        //     this.label1.Text = IniOperation
        //         .IniGetStringValue(str, "text", "label1_1", null)
        //         .Replace("<br>", "\r\n");
        //     Label label1 = this.label1;
        //     label1.Text = label1.Text + _dataVer + "\r\n\r\n";
        //     this.label1.Text += IniOperation
        //         .IniGetStringValue(str, "text", "label1_2", null)
        //         .Replace("<br>", "\r\n");
        //
        //     for (int index = 1; index <= Positions; ++index)
        //     {
        //         comboBox1.Items.Add(IniOperation.IniGetStringValue(str, "character", index.ToString(), null));
        //     }
        // }

        #endregion

        #region 注册表
        /// <summary>
        /// 通过注册表寻找游戏路径（需要管理员权限）
        /// 未找到的情况下调用手动指定
        /// </summary>
        /// <returns>是否寻找到游戏路径</returns>
        /// 版权声明：本文为CSDN博主「BillCYJ」的原创文章，遵循 CC 4.0 BY-SA 版权协议，转载请附上原文出处链接及本声明。
        /// 原文链接：https://blog.csdn.net/BillCYJ/article/details/93998131
        private static bool GetGamePath()
        {
            // RegistryKey 表示 Windows 注册表中的项级节点，OpenSubKey(String)以只读方式打开密钥
            RegistryKey SVNKey = Registry.CurrentUser.OpenSubKey("Software\\SGUP\\Apps\\2");
            if (SVNKey == null)
            {
                return ManualPath();
            }
            PathPkg = SVNKey.GetValue("GamePath") as string; // 获取到注册表中的TortoiseSVN的安装目录
            //int num = (int)MessageBox.Show(PathPkg, "错误");
            return true;
        }
        #endregion
    }
}