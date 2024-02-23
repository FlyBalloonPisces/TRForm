#define debug

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
#if debug
            string path = @"E:\TRHK";
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

            uint[] charItemNumList = GetCharItemNums(folder, crypto, itemdescList);

#if debug
            for (int i = 0; i < charItemNumList.Length; i++)
            {
                Console.WriteLine("charItemNumList[" + i + "]: " + charItemNumList[i]);
            }
#endif

            //flag = TestCharFiles(folder, crypto);
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

        private static uint[] GetCharItemNums(string folder, CryptoClass crypto, SortedList<uint, TblAvatarItemDescClass> itemdescList)
        {
            IEnumerable<uint> Query =
                from item in itemdescList
                where item.Value.fdPosition == 0 && item.Value.fdCharacter != 0
                select item.Value.fdItemNum;
            return Query.ToArray();
        }

        /// <summary>
        /// 测试是否有所有需要的角色pkg
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static bool TestCharFiles(string folder, CryptoClass crypto)
        {
            List<string> charPKGList = new List<string>();
            string[] files = Directory.GetFiles(folder, "char*.pkg");
            //keyValuePairs = Unpack.TestChar(folder, StaticVars.keys2[index]);
            return false;
        }

        /// <summary>
        /// 测试是否有必须的文件夹
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static bool TestNecessaryFolders(string folder)
        {
            return false;
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
