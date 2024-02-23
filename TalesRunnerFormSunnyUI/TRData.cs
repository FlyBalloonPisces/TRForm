#define debug
#define kr
//#define hk

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalesRunnerFormCryptoClassLibrary;
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

            CryptoClass crypto = TestScriptKeys(folder);
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




    }
}
