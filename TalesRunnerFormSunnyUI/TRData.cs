#define debug
#define kr
//#define hk

using DevIL.Unmanaged;
using DevIL;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using TalesRunnerFormCryptoClassLibrary;
using TalesRunnerFormSunnyUI.Properties;
using TRTextProcessingClassLibrary.Item;


namespace TalesRunnerFormSunnyUI.Data
{
    public static class TRData
    {
        #region 变量区域
        private static int _topSpd; // 配装最高速度数值
        private static int _acce; // 配装加速度数值
        private static int _pow; // 配装力数值
        private static int _ctrl; // 配装控制数值
        private static int _lv; // 配装等级数值

        // 配装用变量
        //private static readonly int[] P1 = new int[StaticVars.Positions]; // 存放的是索引
        //private static readonly int[] P2 = new int[StaticVars.Positions]; // 存放的是索引
        //private static readonly string[] P1_stone = new string[Positions]; // 存放的是宝石
        //private static readonly string[] P2_stone = new string[Positions]; // 存放的是宝石

        private static bool _showCN;

        private static ImageImporter _mImporter;
        private static DevIL.Image _mActiveImage;

        private static readonly SortedList<int, AttrInfo> ItemAttrInfo = new SortedList<int, AttrInfo>();

        static SortedList<int, uint> CharNum = new SortedList<int, uint>(); //存放所有角色道具编号
        static SortedList<int, string> CharName = new SortedList<int, string>(); //存放所有角色名称，后期不使用专门表格存储
        static SortedList<int, string> CharFileName = new SortedList<int, string>(); //存放所有角色的文件名前缀，0是all_
        static int CharacterNum; //常规角色的数量
        static SortedList<int, string> Position = new SortedList<int, string>(); //存放各个部位的名称，0是角色

        static CryptoClass crypto;

        private static string PathPkg = ""; // 游戏目录
        public static string[] PathPkgs = new string[4]; // 游戏目录
        public static string[] KeyVersions = new string[4]; // 对应的key编号
        #endregion

        #region 数据结构
        /// <summary>
        /// 需要展示的属性文本
        /// </summary>
        private class AttrInfo
        {
            public string Desc = string.Empty;
            public string Desc2 = string.Empty;
            public float Type;
        }

        /// <summary>
        /// 简单的道具信息
        /// </summary>
        private class ItemStatus2
        {
            // 非装备用
            public uint ItemNum;
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
                if (_showCN)
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
            public List<uint> MemberBasic = new List<uint>(); // 基本 存放的是编号，箱子可以开到的道具
            public List<float> WeightSilver = new List<float>(); // 魔方银色 存放的是编号，箱子可以开到的道具的权重
            public List<uint> MemberSilver = new List<uint>(); // 魔方银色 存放的是编号，箱子可以开到的道具
            public List<float> WeightGold = new List<float>(); // 魔方金色 存放的是编号，箱子可以开到的道具的权重
            public List<uint> MemberGold = new List<uint>(); // 魔方金色 存放的是编号，箱子可以开到的道具
            public uint BoxNum; // 存放的是编号
            public float WeightTotalBasic; // 基本 总权重
            public float WeightTotalSilver; // 魔方银色 总权重
            public float WeightTotalGold; // 魔方金色 总权重
            public uint Position; // 道具位置编号
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
                int index = (int)((Value + 2) / 3);
                if (index >= StaticVars.CardPack.Length)
                {
                    return StaticVars.CardPack[0];
                }
                else
                {
                    return StaticVars.CardPack[index];
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
                int index = (int)((Value + 2) % 3);
                return Get5000Group()[index];
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
                if (!_showCN) return Name;
                return string.CompareOrdinal(NameCh, "(noname)") == 0 ? Name : NameCh;
            }

            public string GetAvatar()
            {
                return Avatar == 0 ? "AvatarOff" : "AvatarOn";
            }
        }

        /// <summary>
        /// 道具属性信息
        /// </summary>
        private class Attr
        {
            public int Num;
            public float Value;

            public override string ToString()
            {
                string str = string.Empty;
               

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
            //public byte Sex; // 装备性别 1男2女
            public short Occupation; // 装备占用位置 -1代表无占用
            public long Chars = -1; // 可以使用该装备的角色 -1代表全部可以使用
            public int Slot; // 宝石槽相关 每4个2进制位代表1个槽
            public string Desc = "";

            /// <summary>
            /// 进行特定语言下装备名称的返回
            /// </summary>
            /// <returns>装备对应语言名称</returns>
            public string GetName()
            {
                if (_showCN)
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
                if (CharName.ContainsKey(c))
                {
                    return CharName[c];
                }
                else
                {
                    return CharName[0];
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
                if(Position.ContainsKey(p))
                {
                    return Position[p];
                }
                else
                { 
                    return "其他"; // TODO
                }
            }

            public string GetAvatar()
            {
                return Avatar == 0 ? "AvatarOff" : "AvatarOn";
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

            byte[] input = crypto.PicFind(PathPkg, offset, pkgNum);
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

        #region 加载用道具数据结构

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
            public uint ItemKind; // 道具图片编号
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
            public long Chars = -1; // 可以使用该装备的角色 -1代表全部可以使用
            public int Slot; // 宝石槽相关 每4个2进制位代表1个槽
            public string Desc = ""; // 道具介绍
        }

        #endregion


        #region 初始化
        public static void Init()
        {
#if kr
            string path = @"E:\TRKR";
#endif

#if hk

            string path = @"E:\TRHK";
#endif

            // 把基础情况先行写入
            CharNum.Add(0, 0);
            CharName.Add(0, "其他");
            CharFileName.Add(0, "all_");

            for (int i = 0; i < StaticVars.PositionName.Length; i++)
            {
                if (i < StaticVars.Position.Length)
                {
                    Position.Add(i, StaticVars.PositionName[i]);
                }

                Position.Add(StaticVars.PositionNum2[i], StaticVars.PositionName[i]);
                
            }

#if debug
            TestFiles(path);
#endif
        }
        #endregion

        #region 文件夹读取区域
        /// <summary>
        /// 测试路径是否有游戏内容
        /// </summary>
        /// <param name="folder">文件夹路径</param>
        /// <returns></returns>
        public static bool TestFiles(string folder)
        {
            bool flag = false;
            flag = TestScriptFiles(folder);
#if debug
            Console.WriteLine("TestScriptFiles: " + flag);
#endif
            if (!flag)
            {
                return false;
            }

            crypto = TestScriptKeys(folder);
#if debug
            Console.WriteLine("TestScriptKeys: " + crypto.ToString());
#endif
            if (crypto.Equals(null))
            {
                return false;
            }

            SortedList<uint, TblAvatarItemDescClass> itemdescList = GetItemDesc1(folder, crypto);

            uint[] charItemNumList = GetCharItemNums(itemdescList);

            ushort[] charNumList = GetCharNums(itemdescList);

#if debugged
            for (int i = 0; i < charItemNumList.Length; i++)
            {
                Console.WriteLine("charItemNumList[" + i + "]: " + charItemNumList[i]);
            }

            for (int i = 0; i < charNumList.Length; i++)
            {
                Console.WriteLine("charNumList[" + i + "]: " + charNumList[i]);
            }
#endif
            SortedList<string, string> folderList = new SortedList<string, string>();
            folderList = GetAllFolders(folder, crypto);


            flag = TestCharFiles(folder, crypto, folderList);

#if debug
            Console.WriteLine("TestCharFiles: " + flag);
#endif

            flag = TestImageFolders(folderList);

#if debug
            Console.WriteLine("TestImageFolders: " + flag);
#endif

            return false;
        }

        /// <summary>
        /// 测试路径是否有文本Pkg
        /// </summary>
        /// <param name="folder">文件夹路径</param>
        /// <returns></returns>
        private static bool TestScriptFiles(string folder)
        {
            string[] files = Directory.GetFiles(folder, "tr4.pkg");
#if debug
            Console.WriteLine("Number of tr4.pkg: " + files.Length);
#endif
            if (files.Length == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 测试加密的key
        /// </summary>
        /// <param name="folder"></param>
        /// <returns>key的index，-1代表需要自行输入key</returns>
        private static CryptoClass TestScriptKeys(string folder)
        {
            for (int i = 0; i < StaticVars.keys2.Length; i++)
            {
                CryptoClass crypto = new CryptoClass(StaticVars.keys2[i]);
                if (crypto.TestKey(folder))
                {
                    return crypto;
                }
            }
            return null;
        }

        private static SortedList<uint, TblAvatarItemDescClass> GetItemDesc1(string folder, CryptoClass crypto)
        {
            string scriptFile = folder + "\\" + "tr4.pkg";
            FileInfo fileInfo = new FileInfo(scriptFile);
            SortedList<uint, TblAvatarItemDescClass> keyValuePairs = new SortedList<uint, TblAvatarItemDescClass>();
            string[] itemdescList = crypto.GetTexts(fileInfo, "tblavataritemdesc.txt");
            for (int i = TblAvatarItemDescClass.startIndex; i < itemdescList.Length; i++)
            {
                TblAvatarItemDescClass tblAvatarItemDesc = new TblAvatarItemDescClass(itemdescList[i]);
                if (keyValuePairs.ContainsKey(tblAvatarItemDesc.fdItemNum))
                {
#if debug
                    Console.WriteLine("TblAvatarItemDescClass Duplicate Num: ");
#endif
                    continue;
                }
                keyValuePairs.Add(tblAvatarItemDesc.fdItemNum, tblAvatarItemDesc);
            }

            return keyValuePairs;
        }

        private static uint[] GetCharItemNums(SortedList<uint, TblAvatarItemDescClass> itemdescList)
        {
            IEnumerable<uint> Query =
                from item in itemdescList
                where item.Value.fdPosition == 0 && item.Value.fdCharacter != 0
                select item.Value.fdItemNum;
            return Query.ToArray();
        }

        private static ushort[] GetCharNums(SortedList<uint, TblAvatarItemDescClass> itemdescList)
        {
            IEnumerable<ushort> Query =
                from item in itemdescList
                where item.Value.fdPosition == 0 && item.Value.fdCharacter != 0
                select item.Value.fdCharacter;
            return Query.ToArray();
        }

        private static SortedList<string, string> GetAllFolders(string folder, CryptoClass crypto)
        {
            string[] files = Directory.GetFiles(folder, "*.pkg");
            SortedList<string, string> folder_dict = new SortedList<string, string>();
            SortedList<string, List<string>> folder_dict_list = new SortedList<string, List<string>>();
            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
#if debugged
                Console.WriteLine(file);
#endif
                foreach (var item in crypto.GetFolder(fileInfo))
                {
#if debugged
                    Console.WriteLine(item.Key);
#endif
                    if (folder_dict_list.ContainsKey(item.Key))
                    {
                        folder_dict_list[item.Key].Add(item.Value);
                    }
                    else
                    {
                        List<string> list = new List<string>();
                        list.Add(item.Value);
                        folder_dict_list.Add(item.Key, list);
                    }
                }
            }
            foreach (var item in folder_dict_list)
            {
                string[] values = item.Value.ToArray();
                string value = string.Empty;
                for (int i = 0; i < values.Length; i++)
                {
                    value += values[i];
                    if (i != values.Length - 1)
                    {
                        value += ",";
                    }
                }
                folder_dict.Add(item.Key, value);
            }

            return folder_dict;
        }

        /// <summary>
        /// 测试是否有所有需要的角色pkg
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static bool TestCharFiles(string folder, CryptoClass crypto, SortedList<string, string> folderList)
        {
            string[] list = crypto.GetCharacterPkg(folder, folderList);
            for (int i = 0; i < list.Length; i++)
            {
                string SearchParttern = list[i] + ".pkg";
                if(Directory.GetFiles(folder, SearchParttern).Length != 1)
                {
#if debug
                    Console.WriteLine(SearchParttern + " not exist.");
#endif
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 测试是否有必须的文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static bool TestImageFolders(SortedList<string, string> folderList)
        {
            for (int i = 0; i < StaticVars.Position.Length; i++)
            {
                string folderName = StaticVars.NecessaryFolder[0] + "\\" + StaticVars.Position[i];
                if (!folderList.ContainsKey(folderName))
                {
#if debug
                    Console.WriteLine(folderName + " not exist.");
#endif
                    return false;
                }
            }

            string folderName2 = StaticVars.NecessaryFolder[0] + "\\" + "etc";
            if (!folderList.ContainsKey(folderName2))
            {
#if debug
                Console.WriteLine(folderName2 + " not exist.");
#endif
                return false;
            }

            return true;
        }

        /// <summary>
        /// 加载本地txt文件，如果无则生成
        /// </summary>
        /// <param name="folder"></param>
        public static void LoadFiles(string folder)
        {
            return;
        }

        /// <summary>
        /// 加载道具情况，生成txt文本
        /// </summary>
        /// <param name="folder"></param>
        public static void LoadItemData(string folder)
        {
            return;
        }

        /// <summary>
        /// 加载套装情况，生成txt文本
        /// </summary>
        /// <param name="folder"></param>
        public static void LoadItemSetData(string folder)
        {
            return;
        }

        /// <summary>
        /// 加载属性
        /// </summary>
        /// <param name="folder"></param>
        public static void LoadItemAttr(string folder)
        {
            return;
        }

        /// <summary>
        /// 加载装备占用
        /// </summary>
        /// <param name="folder"></param>
        public static void LoadItemOccupation(string folder)
        {
            return;
        }

        /// <summary>
        /// 加载道具翻译
        /// </summary>
        /// <param name="folder"></param>
        public static void LoadItemTranslation(string folder)
        {
            return;
        }
        #endregion


        #region 数值计算
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

    }
}
