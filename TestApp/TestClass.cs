#define debug

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRTextProcessingClassLibrary.Item;

namespace TestApp
{
    internal class TestClass
    {
        private byte[] aesKey =
        [
                0x0D, 0x68, 0x07, 0x6F, 0x0A, 0x09, 0x07, 0x6C, 0x65, 0x73,
                0x0D, 0x75, 0x6E, 0x0A, 0x65, 0x0D
        ];
        private byte[] xorKey =
        [
                0x05, 0x5B, 0xCB, 0x64, 0xFB, 0xC2, 0xCE, 0xB4, 0x77, 0x8B,
                0x1B, 0xB8, 0xE9, 0xB5, 0x9C, 0xC6
        ];
        private byte[] aesKey2 =
        [        
                //韩服
                0x3c, 0x5, 0x64, 0x7c, 0xc8, 0xa0, 0xa8, 0x69, 0xf, 0xc0,
                0x14, 0x5b, 0xb3, 0x8e, 0x47, 0xf8, 0x51, 0xaa, 0xe3, 0xca,
                0x77, 0xdc, 0xbe, 0x36, 0x80, 0x52, 0x1d, 0xb8, 0xb, 0xae,
                0x78, 0xfa
        ];
        private byte[] xorKey2 =
        [
                // 韩服
                0x41, 0x68, 0x1f, 0x16, 0x88, 0xa8, 0xa8, 0x7, 0xe, 0x5a,
                0x7b, 0x9f, 0x17, 0x38, 0x10, 0x51
        ];

        private string[] positionNames =
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

        internal static readonly string[] position =
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

        public TestClass()
        {

        }

        public void testUnpack(string folder)
        {
            TRCryptoClassLibrary.CryptClass crypto = new TRCryptoClassLibrary.CryptClass(aesKey, xorKey, aesKey2, xorKey2);
            string[] files = Directory.GetFiles(folder, "*.pkg");

            foreach (string file in files)
            {
                FileInfo fileInfo = new FileInfo(file);
                Console.WriteLine(file);
                crypto.Unpack(fileInfo);
            }
            //for
            //crypto.Unpack();
        }

        public SortedList<string, string> testFolder(string folder)
        {
            TRCryptoClassLibrary.CryptClass crypto = new TRCryptoClassLibrary.CryptClass(aesKey, xorKey, aesKey2, xorKey2);
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
                //crypto.GetFolder(fileInfo);
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

        public SortedList<int, string> testCharacter(string folder)
        {
            TRCryptoClassLibrary.CryptClass crypto = new TRCryptoClassLibrary.CryptClass(aesKey, xorKey, aesKey2, xorKey2);
            string scriptFile = folder + "\\" + "tr4.pkg";
            FileInfo fileInfo = new FileInfo(scriptFile);
            SortedList<int, string> dict = new SortedList<int, string>();
            SortedList<string, string> folder_dict = this.testFolder(folder);
            string[] startup = crypto.GetTexts(fileInfo, "startup.py");
            for (int i = 0; i < startup.Length; i++)
            {
                if (startup[i].StartsWith("obj.registerCharacterModel"))
                {
                    string[] temp = TRTextProcessingClassLibrary.Tool.StringDivideClass.StringDivide(startup[i]);
#if debugged
                    Console.WriteLine(startup[i]);
                    for (int j = 0; j < temp.Length; j++)
                    {
                        Console.WriteLine(j + ":" + temp[j]);
                    }
#endif

                    string ca3Name = temp[1].Trim();
                    string[] temp2 = ca3Name.Split("\\\\");
#if debugged
                    for (int j = 0; j < temp2.Length; j++)
                    {
                        Console.WriteLine(j + ":" + temp2[j]);
                    }
#endif
                    string charFolderName = temp2[0] + "\\" + temp2[1];
#if debugged
                    Console.WriteLine(charFolderName);
#endif
                    int charNum = int.Parse(temp2[1].Substring(9));
                    if (folder_dict.ContainsKey(charFolderName))
                    {
                        if (!dict.ContainsKey(charNum))
                        {
                            dict.Add(charNum, folder_dict[charFolderName] + "," + ca3Name);
                        }
                    }
                }
            }
            return dict;
        }

        public List<string> testItemFileName(string folder)
        {
            SortedList<int, string> char_List = this.testCharacter(folder);
            SortedList<int, string> charList = new SortedList<int, string>();
            TRCryptoClassLibrary.CryptClass crypto = new TRCryptoClassLibrary.CryptClass(aesKey, xorKey, aesKey2, xorKey2);
            string scriptFile = folder + "\\" + "tr4.pkg";
            FileInfo fileInfo = new FileInfo(scriptFile);
            List<string> list = new List<string>();
            SortedList<string, string> folder_list = this.testFolder(folder);
            string[] itemdesc = crypto.GetTexts(fileInfo, "tblavataritemdesc.txt");

            string charAllShortName = "all" + "_";
            foreach (var item in char_List)
            {
                if (item.Key < 100)
                {
                    charList.Add(item.Key, item.Value.Split(",")[1].Split("\\\\")[2].Split('_')[0]);
                }
            }

            for (int i = TRTextProcessingClassLibrary.Item.TblAvatarItemDescClass.startIndex; i < itemdesc.Length; i++)
            {
                TRTextProcessingClassLibrary.Item.TblAvatarItemDescClass tblAvatarItemDesc = new TRTextProcessingClassLibrary.Item.TblAvatarItemDescClass(itemdesc[i]);
                StringBuilder fileNameBuilder = new StringBuilder(byte.MaxValue);
                if (tblAvatarItemDesc.fdPosition <= positionNames.Length && tblAvatarItemDesc.fdPosition > 0)
                {
                    if (charList.ContainsKey(tblAvatarItemDesc.fdCharacter))
                    {
                        fileNameBuilder.Append(charList[tblAvatarItemDesc.fdCharacter] + "_");
                    }
                    else if (tblAvatarItemDesc.fdItemKind > 1000)
                    {
                        fileNameBuilder.Append(charAllShortName);
                    }


                    if (tblAvatarItemDesc.fdPosition <= positionNames.Length) //StaticVars.Positions
                    {
                        fileNameBuilder.Append(position[tblAvatarItemDesc.fdPosition]);
                    }
                    else if (tblAvatarItemDesc.fdPosition == 106)
                    {
                        fileNameBuilder.Append(position[2]);
                    }
                    else if (tblAvatarItemDesc.fdPosition == 117)
                    {
                        fileNameBuilder.Append(position[2]);
                    }

                    if (tblAvatarItemDesc.fdCharacter == 0)
                    {
                        fileNameBuilder.Append(tblAvatarItemDesc.fdItemKind);
                    }
                    else
                    {
                        fileNameBuilder.AppendFormat("{0:D3}", tblAvatarItemDesc.fdItemKind);
                    }

                    fileNameBuilder.Append(".pt1");

                    string str = fileNameBuilder.ToString();
                    if (str.Split("_").Length == 3 && !list.Contains(str))
                    {
                        list.Add(str);
                    }

                    
                }
            }
            return list;
        }

        public SortedList<string, long[]> testOccupation(string folder)
        {
            SortedList<int, string> charList = this.testCharacter(folder);
            List<string> itemNameList = this.testItemFileName(folder);
            TRCryptoClassLibrary.CryptClass crypto = new TRCryptoClassLibrary.CryptClass(aesKey, xorKey, aesKey2, xorKey2);
            SortedList<string, long[]> result_list = crypto.GetOccupation(folder, charList, positionNames, itemNameList);

            return result_list;
        }

        public SortedList<uint, TblAvatarItemDescClass> testItemChar(string folder)
        {

            TRCryptoClassLibrary.CryptClass crypto = new TRCryptoClassLibrary.CryptClass(aesKey, xorKey, aesKey2, xorKey2);
            string scriptFile = folder + "\\" + "tr4.pkg";
            FileInfo fileInfo = new FileInfo(scriptFile);
            SortedList<uint, TblAvatarItemDescClass> dict = new SortedList<uint, TblAvatarItemDescClass>();
            string[] texts = crypto.GetTexts(fileInfo, "tblavataritemdesc.txt");
            for (int i = TblAvatarItemDescClass.startIndex; i < texts.Length; i++)
            {
                TblAvatarItemDescClass tblAvatarItemDesc = new TblAvatarItemDescClass(texts[i]);
                if (tblAvatarItemDesc.fdCharacter != 0 && tblAvatarItemDesc.fdPosition == 0)
                dict.Add(tblAvatarItemDesc.fdItemNum, tblAvatarItemDesc);
            }
            return dict;
        }
    }
}
