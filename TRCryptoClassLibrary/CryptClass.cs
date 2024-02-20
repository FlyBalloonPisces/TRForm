#define debug

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using zlib;

namespace TRCryptoClassLibrary
{
    public class CryptClass
    {
        /// <summary>
        /// key数组
        /// </summary>
        private byte[] aes_key1;
        private byte[] xor_key1;
        private byte[] aes_key2;
        private byte[] xor_key2;

        /// <summary>
        /// 基础构造函数，TODO
        /// </summary>
        public CryptClass() 
        {
            aes_key1 = new byte[256];
            aes_key2 = new byte[256];
            xor_key1 = new byte[256];
            xor_key2 = new byte[256];
        }

        /// <summary>
        /// 正式构造函数
        /// </summary>
        /// <param name="aes_key1">第一轮解密用key</param>
        /// <param name="xor_key1">第一轮解密用key</param>
        /// <param name="aes_key2">文本解密用key</param>
        /// <param name="xor_key2">文本解密用key</param>
        public CryptClass(byte[] aes_key1, byte[] xor_key1, byte[] aes_key2, byte[] xor_key2)
        {
            this.aes_key1 = aes_key1;
            this.xor_key1 = xor_key1;
            this.aes_key2 = aes_key2;
            this.xor_key2 = xor_key2;
        }

        /// <summary>
        /// 解包pkg
        /// </summary>
        /// <param name="data">解密前字节数据</param>
        /// <returns>解密后字节数据</returns>
        private byte[] Decrypt(byte[] data)
        {
            List<byte> decryptData = new List<byte>();
            byte[] aesKey = this.aes_key1;
            byte[] xorKey = this.xor_key1;

            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                aes.Key = aesKey;

                int length = data.Length;
                int i = 0;
                // Console.WriteLine("Length = " + length);
                if (length >= 16)
                {
                    int groupNum = length / 16;
                    ICryptoTransform cTransform = aes.CreateDecryptor();
                    byte[] outData = cTransform.TransformFinalBlock(data, 0, groupNum * 16);
                    decryptData.AddRange(outData);
                    // Array.Copy(out_data, decrypt_data, out_data.Length);
                    i = groupNum * 16;
                }
                while (i < length)
                {
                    // decrypt_data[i] = Convert.ToByte(data[i] ^ xor_key[i % 16]);
                    decryptData.Add(Convert.ToByte(data[i] ^ xorKey[i % 16]));
                    i++;
                }

                // Console.WriteLine("decrypt_data = " + System.Text.Encoding.UTF8.GetString (decrypt_data));

                return decryptData.ToArray();

            }
        }

        /// <summary>
        /// 文本解密
        /// </summary>
        /// <param name="data">解密前数据</param>
        /// <returns>解密后数据</returns>
        private byte[] Decrypt2(byte[] data)
        {
            List<byte> decryptData = new List<byte>();
            byte[] aesKey = this.aes_key2;
            byte[] xorKey = this.xor_key2;
            // MemoryStream mStream = new MemoryStream(data);
            using (Aes aes = Aes.Create())
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                aes.Key = aesKey;

                int length = data.Length;
                int i = 0;
                // Console.WriteLine("Length = " + length);
                if (length >= 16)
                {
                    int groupNum = length / 16;
                    ICryptoTransform cTransform = aes.CreateDecryptor();
                    byte[] outData = cTransform.TransformFinalBlock(data, 0, groupNum * 16);
                    decryptData.AddRange(outData);
                    // Array.Copy(out_data, decrypt_data, out_data.Length);
                    i = groupNum * 16;
                }
                while (i < length)
                {
                    // decrypt_data[i] = Convert.ToByte(data[i] ^ xor_key[i % 16]);
                    decryptData.Add(Convert.ToByte(data[i] ^ xorKey[i % 16]));
                    i++;
                }

                // Console.WriteLine("decrypt_data = " + System.Text.Encoding.UTF8.GetString (decrypt_data));

                return decryptData.ToArray();

            }
        }

        /// <summary>
        /// 解压缩字节数组
        /// </summary>
        /// <param name="sourceByte">需要被解压缩的字节数组</param>
        /// <returns>解压后的字节数组</returns>
        private static byte[] DeCompressBytes(byte[] sourceByte)
        {
            using (MemoryStream outStream = new MemoryStream())
            using (ZOutputStream outZStream = new ZOutputStream(outStream))
            {
                outZStream.Write(sourceByte, 0, sourceByte.Length);
                outZStream.Flush();
                outZStream.finish();
                return outStream.ToArray();
            }
        }

        public void Unpack(FileInfo fileInfo)
        {
            string pkg_path = fileInfo.FullName; // 获得pkg_path

            //string pkg_dir = fileInfo.FullName.Split('.')[0]; // 获取文件夹
            string pkg_dir = fileInfo.DirectoryName; // 获取文件夹
            string pkg_name = fileInfo.Name.Split('.')[0]; // 获取文件名：tr1
#if debugged
            Console.WriteLine("path:" + pkg_path);
            Console.WriteLine("dir:" + pkg_dir);
            Console.WriteLine("pkgname:" + pkg_name);
#endif
            FileStream fs = new FileStream(pkg_path, FileMode.Open, FileAccess.Read); // 打开文件
            BinaryReader pkg = new BinaryReader(fs);
            byte[] file_header = pkg.ReadBytes(12); // 阅读文件前12字
            file_header = Decrypt(file_header); // 解码
            if (Encoding.UTF8.GetString(file_header) != "ACAC35E5-4B7")
            {
                Console.WriteLine("Not a valid .pkg file or decryption key has changed");
                return;
            }

            fs.Seek(20, SeekOrigin.Begin); // 从头开始，偏移0x14，读取
            int offset = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取偏移量
            fs.Seek(offset, 0); // 从头开始，偏移offset，读取
            fs.Seek(4, SeekOrigin.Current); // 从当前位置开始，偏移0x4，读取
            int file_num = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取文件数量
            fs.Seek(4, SeekOrigin.Current); // 从当前位置开始，偏移0x4，读取
            int num = 0; // 解压的文件数量
                         // 解包，一个文件分成多个单元进行分解
            while (num < file_num)
            {
                int entry_size = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取文件，反序读取4个字节
                                                                            // Console.WriteLine("file_dir = " + entry_size);
                byte[] entry_data = pkg.ReadBytes(entry_size); // 读取文件
                long next_entry = pkg.BaseStream.Position; // 返回文件当前位置
                byte[] decompressed_entry_data = DeCompressBytes(entry_data); // 解压缩文件数据
                int length = 0;
                for (int i = 0; i < decompressed_entry_data.Length; i++)
                {
                    if (decompressed_entry_data[i] == 0)
                    {
                        length = i;
                        break;
                    }
                }

                byte[] decompressed_entry_title = new byte[length];
                for (int i = 0; i < decompressed_entry_title.Length; i++)
                {
                    decompressed_entry_title[i] = decompressed_entry_data[i];
                }
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //引入949编码需要
                string file_title = Encoding.GetEncoding(949).GetString(decompressed_entry_title); //character/characterXX/xxx.pt1
#if debugged
                //http://www.exceloffice.net/archives/5750
                Console.WriteLine("title = " + file_title);
#endif
                string file_path =
                    pkg_dir + "\\" + pkg_name + "\\" + file_title; // 把目录和文件名合成一个路径
                string unpack_path = pkg_name + "\\" + file_title; // 把pkg名和文件名合成一个路径，在解压过程中显示
                //string unpack_path = Encoding.GetEncoding(949).GetString(decompressed_entry_title); // 把pkg名和文件名合成一个路径，在解压过程中显示
                string file_dir = Path.GetDirectoryName(file_path); // 返回文件路径

#if debugged
                int part_num =
                    BitConverter.ToInt32(decompressed_entry_data.Skip(0x410).Take(4)
                        .ToArray(), 0); // 获得单元数量
                offset = BitConverter.ToInt32(decompressed_entry_data.Skip(0x414).Take(4)
                    .ToArray(), 0); // 获取偏移量
                fs.Seek(offset, SeekOrigin.Begin);
                List<byte> decrypted_file_data_list = new List<byte>();
                for (int i = 0; i < part_num; i++)
                {
                    fs.Seek(0x8, SeekOrigin.Current);
                    int file_size =
                        BitConverter.ToInt32(pkg.ReadBytes(4), 0); // encrypted file data size
                    fs.Seek(0x4, SeekOrigin.Current);
                    int encrypt_type = BitConverter.ToInt32(pkg.ReadBytes(4), 0);
                    //Console.WriteLine("encrypt_type1 = " + (encrypt_type & 1));
                    //Console.WriteLine("encrypt_type2 = " + (encrypt_type & 2));
                    byte[] file_data = pkg.ReadBytes(file_size); // encrypted file data
                    if ((encrypt_type & 1) == 1)
                    {
                        file_data = DeCompressBytes(file_data);
                    }

                    if ((encrypt_type & 2) == 2)
                    {
                        file_data = Decrypt2(file_data);
                    }

                    decrypted_file_data_list.AddRange(file_data);
                }

                byte[] decrypted_file_data = decrypted_file_data_list.ToArray();

                if (!Directory.Exists(file_dir))
                {
                    Directory.CreateDirectory(file_dir);
                }

                string[] file_name_split = file_path.Split('.');
                string fileExtension = file_name_split[file_name_split.Length - 1].ToLower();

                FileStream fs2 = new FileStream(file_path, FileMode.Create, FileAccess.Write);
                BinaryWriter export_file = new BinaryWriter(fs2);
                export_file.Write(decrypted_file_data);
                export_file.Close();
#endif
                fs.Seek(next_entry, 0);
                num += 1;
                Console.WriteLine("Unpacking files " + num + " / " + file_num + ": " + unpack_path);
            }

            pkg.Close();
            Console.WriteLine("Unpack done");
            GC.Collect();
        }

        public SortedList<string, string> GetFolder(FileInfo fileInfo)
        {
            SortedList<string, string> folder_dict = new SortedList<string, string>();  
            string pkg_path = fileInfo.FullName; // 获得pkg_path

            //string pkg_dir = fileInfo.FullName.Split('.')[0]; // 获取文件夹
            string pkg_dir = fileInfo.DirectoryName; // 获取文件夹
            string pkg_name = fileInfo.Name.Split('.')[0]; // 获取文件名：tr1

            FileStream fs = new FileStream(pkg_path, FileMode.Open, FileAccess.Read); // 打开文件
            BinaryReader pkg = new BinaryReader(fs);
            byte[] file_header = pkg.ReadBytes(12); // 阅读文件前12字
            file_header = Decrypt(file_header); // 解码
            if (Encoding.UTF8.GetString(file_header) != "ACAC35E5-4B7")
            {
                Console.WriteLine("Not a valid .pkg file or decryption key has changed");
                return folder_dict;
            }

            fs.Seek(20, SeekOrigin.Begin); // 从头开始，偏移0x14，读取
            int offset = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取偏移量
            fs.Seek(offset, 0); // 从头开始，偏移offset，读取
            fs.Seek(4, SeekOrigin.Current); // 从当前位置开始，偏移0x4，读取
            int file_num = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取文件数量
            fs.Seek(4, SeekOrigin.Current); // 从当前位置开始，偏移0x4，读取
            int num = 0; // 解压的文件数量
                         // 解包，一个文件分成多个单元进行分解
            while (num < file_num)
            {
                int entry_size = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取文件，反序读取4个字节
                                                                            // Console.WriteLine("file_dir = " + entry_size);
                byte[] entry_data = pkg.ReadBytes(entry_size); // 读取文件
                long next_entry = pkg.BaseStream.Position; // 返回文件当前位置
                byte[] decompressed_entry_data = DeCompressBytes(entry_data); // 解压缩文件数据
                int length = 0;
                for (int i = 0; i < decompressed_entry_data.Length; i++)
                {
                    if (decompressed_entry_data[i] == 0)
                    {
                        length = i;
                        break;
                    }
                }

                byte[] decompressed_entry_title = new byte[length];
                for (int i = 0; i < decompressed_entry_title.Length; i++)
                {
                    decompressed_entry_title[i] = decompressed_entry_data[i];
                }
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //引入949编码需要
                string file_title = Encoding.GetEncoding(949).GetString(decompressed_entry_title); //character/characterXX/xxx.pt1
#if debugged
                //http://www.exceloffice.net/archives/5750
                Console.WriteLine("title = " + file_title);
#endif

                string[] folders = file_title.Split("\\");
                
                if (folders.Length >= 2)
                {
                    string folderName = string.Empty;
                    for (int i = 0; i < folders.Length - 1; i++)
                    {
                        folderName = string.Empty;
                        for (int j = 0; j <= i; j++)
                        {
                            folderName += folders[j];
                            if (j != i)
                            {
                                folderName += "\\";
                            }
                        }

                        if (!folder_dict.ContainsKey(folderName))
                        {
                            folder_dict.Add(folderName, pkg_name);
                        }

                    }


                    //else
                    //{
                    //    if (!folder_dict[folderName].Contains(pkg_name))
                    //    {
                    //        folder_dict[folderName].Add(pkg_name);
                    //    }
                    //}
                }

                //string file_path =
                //    pkg_dir + "\\" + pkg_name + "\\" + file_title; // 把目录和文件名合成一个路径
                //string unpack_path = pkg_name + "\\" + file_title; // 把pkg名和文件名合成一个路径，在解压过程中显示
                ////string unpack_path = Encoding.GetEncoding(949).GetString(decompressed_entry_title); // 把pkg名和文件名合成一个路径，在解压过程中显示
                //string file_dir = Path.GetDirectoryName(file_path); // 返回文件路径

                fs.Seek(next_entry, 0);
                num += 1;
                //Console.WriteLine("Unpacking files " + num + " / " + file_num + ": " + unpack_path);
            }

            pkg.Close();
            //Console.WriteLine("Unpack done");
            GC.Collect();
            return folder_dict;
        }

        public string[] GetTexts(FileInfo fileInfo, string fileName)
        {
            List<string> stringList = new List<string>();
            string pkg_path = fileInfo.FullName; // 获得pkg_path

            //string pkg_dir = fileInfo.FullName.Split('.')[0]; // 获取文件夹
            string pkg_dir = fileInfo.DirectoryName; // 获取文件夹
            string pkg_name = fileInfo.Name.Split('.')[0]; // 获取文件名：tr1

            FileStream fs = new FileStream(pkg_path, FileMode.Open, FileAccess.Read); // 打开文件
            BinaryReader pkg = new BinaryReader(fs);
            byte[] file_header = pkg.ReadBytes(12); // 阅读文件前12字
            file_header = Decrypt(file_header); // 解码
            if (Encoding.UTF8.GetString(file_header) != "ACAC35E5-4B7")
            {
                Console.WriteLine("Not a valid .pkg file or decryption key has changed");
                return stringList.ToArray();
            }

            fs.Seek(20, SeekOrigin.Begin); // 从头开始，偏移0x14，读取
            int offset = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取偏移量
            fs.Seek(offset, 0); // 从头开始，偏移offset，读取
            fs.Seek(4, SeekOrigin.Current); // 从当前位置开始，偏移0x4，读取
            int file_num = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取文件数量
            fs.Seek(4, SeekOrigin.Current); // 从当前位置开始，偏移0x4，读取
            int num = 0; // 解压的文件数量
                         // 解包，一个文件分成多个单元进行分解
            while (num < file_num)
            {
                int entry_size = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取文件，反序读取4个字节
                                                                            // Console.WriteLine("file_dir = " + entry_size);
                byte[] entry_data = pkg.ReadBytes(entry_size); // 读取文件
                long next_entry = pkg.BaseStream.Position; // 返回文件当前位置
                byte[] decompressed_entry_data = DeCompressBytes(entry_data); // 解压缩文件数据
                int length = 0;
                for (int i = 0; i < decompressed_entry_data.Length; i++)
                {
                    if (decompressed_entry_data[i] == 0)
                    {
                        length = i;
                        break;
                    }
                }

                byte[] decompressed_entry_title = new byte[length];
                for (int i = 0; i < decompressed_entry_title.Length; i++)
                {
                    decompressed_entry_title[i] = decompressed_entry_data[i];
                }
                //http://www.exceloffice.net/archives/5750
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //引入949编码需要
                string file_title = Encoding.GetEncoding(949).GetString(decompressed_entry_title); //character/characterXX/xxx.pt1
                string[] titles = file_title.Split("\\");
                if (fileName.Equals(titles.GetValue(titles.Length - 1)))
                {
                    int part_num =
    BitConverter.ToInt32(decompressed_entry_data.Skip(0x410).Take(4)
        .ToArray(), 0); // 获得单元数量
                    offset = BitConverter.ToInt32(decompressed_entry_data.Skip(0x414).Take(4)
                        .ToArray(), 0); // 获取偏移量
                    fs.Seek(offset, SeekOrigin.Begin);
                    List<byte> decrypted_file_data_list = new List<byte>();
                    for (int i = 0; i < part_num; i++)
                    {
                        fs.Seek(0x8, SeekOrigin.Current);
                        int file_size =
                            BitConverter.ToInt32(pkg.ReadBytes(4), 0); // encrypted file data size
                        fs.Seek(0x4, SeekOrigin.Current);
                        int encrypt_type = BitConverter.ToInt32(pkg.ReadBytes(4), 0);
                        //Console.WriteLine("encrypt_type1 = " + (encrypt_type & 1));
                        //Console.WriteLine("encrypt_type2 = " + (encrypt_type & 2));
                        byte[] file_data = pkg.ReadBytes(file_size); // encrypted file data
                        if ((encrypt_type & 1) == 1)
                        {
                            file_data = DeCompressBytes(file_data);
                        }

                        if ((encrypt_type & 2) == 2)
                        {
                            file_data = Decrypt2(file_data);
                        }

                        decrypted_file_data_list.AddRange(file_data);
                    }

                    byte[] decrypted_file_data = decrypted_file_data_list.ToArray();

                    
                    MemoryStream ms = new MemoryStream(decrypted_file_data);
                    StreamReader sr = new StreamReader(ms, Encoding.GetEncoding(949));
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        stringList.Add(line);
                    }

                    sr.Close();
                }

                //string file_path =
                //    pkg_dir + "\\" + pkg_name + "\\" + file_title; // 把目录和文件名合成一个路径
                //string unpack_path = pkg_name + "\\" + file_title; // 把pkg名和文件名合成一个路径，在解压过程中显示
                ////string unpack_path = Encoding.GetEncoding(949).GetString(decompressed_entry_title); // 把pkg名和文件名合成一个路径，在解压过程中显示
                //string file_dir = Path.GetDirectoryName(file_path); // 返回文件路径

                fs.Seek(next_entry, 0);
                num += 1;
                //Console.WriteLine("Unpacking files " + num + " / " + file_num + ": " + unpack_path);
            }

            pkg.Close();
            //Console.WriteLine("Unpack done");
            GC.Collect();
            return stringList.ToArray();
        }

        public SortedList<string, long[]> GetOccupation(string folder, SortedList<int, string> char_List, string[] positionNames, List<string> itemNameList)
        {
            // 全角色占用


            // 普通角色数量
            int charCount = 0;
            SortedList<int, string> charList = new SortedList<int, string>();
            foreach (var item in char_List)
            {
                if (item.Key < 100)
                {
                    charList.Add(item.Key, item.Value);
                }
            }

            // 各角色公共服饰 TODO 改长度
            SortedList<string, short>[] listOc = new SortedList<string, short>[charList.Count];
            //SortedList<string, short> listOcAll = new SortedList<string, short>();
            // 各角色专属服饰
            SortedList<string, short> listChar = new SortedList<string, short>();



            //int ch = 1;
            foreach (var item in charList)
            {
                if (item.Key <= 100)
                {
                    //28,char15,character\\character28\\vrd_set.ca3
                    int indexInList = charList.IndexOfKey(item.Key);
                    string[] temp = charList[item.Key].Split(",");
                    string pkgName = temp[0];
                    temp = temp[1].Split("\\\\");
                    string charFolderName = temp[1];
                    string ca3Name = temp[2];
                    string charShortName = ca3Name.Split('_')[0];
                    string charAllShortName = "all";


                    string pkgPath = folder + "\\" + pkgName + ".pkg"; // 获得pkg_path
                    FileStream fs = new FileStream(pkgPath, FileMode.Open, FileAccess.Read); // 打开文件
                    listOc[indexInList] = new SortedList<string, short>();

                    SortedList<string, string> listOcBinary = new SortedList<string, string>();
                    int[] positionBinary = new int[positionNames.Length];


                    using (BinaryReader pkg = new BinaryReader(fs))
                    {
                        //_ = (int)MessageBox.Show("3", "Debug");
                        byte[] fileHeader = pkg.ReadBytes(12); // 阅读文件前12字
                        fileHeader = Decrypt(fileHeader); // 解码
                        if (Encoding.UTF8.GetString(fileHeader) != "ACAC35E5-4B7")
                        {
                            // _ = (int)MessageBox.Show("读取装备数据失败!", "错误");
                            return null;
                        }

                        fs.Seek(20, SeekOrigin.Begin); // 从头开始，偏移0x14，读取
                        int offset = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取偏移量
                        fs.Seek(offset, 0); // 从头开始，偏移offset，读取
                        fs.Seek(4, SeekOrigin.Current); // 从当前位置开始，偏移0x4，读取
                        int fileNum = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取文件数量
                        fs.Seek(4, SeekOrigin.Current); // 从当前位置开始，偏移0x4，读取
                        int num = 0; // 解压的文件数量
                                     // 解包，一个文件分成多个单元进行分解
                        while (num < fileNum)
                        {
                            int entrySize = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取文件，反序读取4个字节
                            byte[] entryData = pkg.ReadBytes(entrySize); // 读取文件
                            long nextEntry = pkg.BaseStream.Position; // 返回文件当前位置
                            byte[] decompressedEntryData = DeCompressBytes(entryData); // 解压缩文件数据
                            int length = 0;
                            for (int i = 0; i < decompressedEntryData.Length; i++)
                            {
                                if (decompressedEntryData[i] == 0)
                                {
                                    length = i;
                                    break;
                                }
                            }
                            byte[] decompressedEntryTitle = new byte[length];
                            for (int i = 0; i < decompressedEntryTitle.Length; i++)
                            {
                                decompressedEntryTitle[i] = decompressedEntryData[i];
                            }

                            //http://www.exceloffice.net/archives/5750
                            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //引入949编码需要
                            string title = Encoding.GetEncoding(949).GetString(decompressedEntryTitle);
                            string[] titleSplits = title.Split('\\');

                            if (titleSplits.Length == 3)
                            {
                                if (titleSplits[1].Equals(charFolderName))
                                {
                                    string fileName = titleSplits[2]; // 把目录和文件名合成一个路径

                                    if (fileName.Split('.').Length == 2 && fileName.Split('_')[0].Equals(charShortName) && fileName.Split('_').Length >= 3)
                                    {
                                        if (fileName.Split('.')[1].Equals("pt1") && IsNumberic(fileName.Split('_')[2].Split('.')[0])) // 获取pt1文件以获取装备占用
                                        {
                                            int partNum = BitConverter.ToInt32(decompressedEntryData.Skip(0x410).Take(4).ToArray(), 0); // 获得单元数量
                                            offset = BitConverter.ToInt32(decompressedEntryData.Skip(0x414).Take(4)
                                                .ToArray(), 0); // 获取偏移量
                                            fs.Seek(offset, SeekOrigin.Begin);
                                            List<byte> decryptedFileDataList = new List<byte>();
                                            for (int i = 0; i < partNum; i++)
                                            {
                                                fs.Seek(0x8, SeekOrigin.Current);
                                                int fileSize =
                                                    BitConverter.ToInt32(pkg.ReadBytes(4), 0); // encrypted file data size
                                                fs.Seek(0x4, SeekOrigin.Current);
                                                int encryptType = BitConverter.ToInt32(pkg.ReadBytes(4), 0);
                                                byte[] fileData = pkg.ReadBytes(fileSize); // encrypted file data
                                                if ((encryptType & 1) == 1)
                                                {
                                                    fileData = DeCompressBytes(fileData);
                                                }

                                                if ((encryptType & 2) == 2)
                                                {
                                                    fileData = Decrypt2(fileData);
                                                }
                                                decryptedFileDataList.AddRange(fileData);
                                            }
                                            byte[] decryptedFileData = decryptedFileDataList.ToArray();
                                            byte[] dataHeader = new byte[14];
                                            Array.Copy(decryptedFileData, 0, dataHeader, 0, 14);
                                            if (Encoding.UTF8.GetString(dataHeader) == "CharModelPart2")
                                            {
                                                byte[] dataOccupy = { 0, 0, 0, 0 }; // 角色占用占4字节
                                                                                    // 读取每段数据最后的字节并进行加和以获取部位占用
                                                foreach (var index in IndexesOf(decryptedFileData, 0x40, new byte[] { 0x2e, 0x6d, 0x31 }))
                                                {
                                                    long p = index + 3;
                                                    for (int i = 0; i < 4; i++)
                                                    {
                                                        dataOccupy[i] += decryptedFileData[p + i];
                                                    }
                                                }
                                                string dataOccupy1 = Convert.ToString(dataOccupy[3], 2).PadLeft(8, '0') + Convert.ToString(dataOccupy[2], 2).PadLeft(8, '0') + Convert.ToString(dataOccupy[1], 2).PadLeft(8, '0') + Convert.ToString(dataOccupy[0], 2).PadLeft(8, '0');
                                                //data_occupy1.Reverse();
                                                listOcBinary.Add(fileName, dataOccupy1);

                                                //FileStream fs2 = new FileStream(Form1.path + "Character" + ch + "_" + Form2.character[ch] + "occupy.txt", FileMode.Create, FileAccess.Write);
                                                //StreamWriter export_file = new StreamWriter(fs2);
                                                //export_file.Write(Form2.character[ch].Replace("_", ""));
                                                //export_file.WriteLine();
                                                //foreach (KeyValuePair<string,string> pair in List_ocBinary)
                                                //{
                                                //    export_file.Write(pair.Key+","+pair.Value);
                                                //    export_file.WriteLine();
                                                //}
                                                //export_file.Close();
                                            }
                                            else
                                            {
                                                // 非角色文件
#if debug
                                                Console.WriteLine("Not CharModelPart2 PTFile: " + title);
#endif
                                            }
                                        }
                                    }
                                    else if (fileName.Equals(ca3Name)) // 获取角色本身的各部位编号
                                    {
                                        int partNum = BitConverter.ToInt32(decompressedEntryData.Skip(0x410).Take(4).ToArray(), 0); // 获得单元数量
                                        offset = BitConverter.ToInt32(decompressedEntryData.Skip(0x414).Take(4)
                                            .ToArray(), 0); // 获取偏移量
                                        fs.Seek(offset, SeekOrigin.Begin);
                                        List<byte> decryptedFileDataList = new List<byte>();
                                        for (int i = 0; i < partNum; i++)
                                        {
                                            fs.Seek(0x8, SeekOrigin.Current);
                                            int fileSize =
                                                BitConverter.ToInt32(pkg.ReadBytes(4), 0); // encrypted file data size
                                            fs.Seek(0x4, SeekOrigin.Current);
                                            int encryptType = BitConverter.ToInt32(pkg.ReadBytes(4), 0);
                                            byte[] fileData = pkg.ReadBytes(fileSize); // encrypted file data
                                            if ((encryptType & 1) == 1)
                                            {
                                                fileData = DeCompressBytes(fileData);
                                            }

                                            if ((encryptType & 2) == 2)
                                            {
                                                fileData = Decrypt2(fileData);
                                            }
                                            decryptedFileDataList.AddRange(fileData);
                                        }
                                        byte[] decryptedFileData = decryptedFileDataList.ToArray();
                                        byte[] dataHeader = new byte[18];
                                        Array.Copy(decryptedFileData, 0, dataHeader, 0, 18);
                                        if (Encoding.UTF8.GetString(dataHeader) == "CharAnimationFile3")
                                        {
#if debugged
                                            _ = (int)MessageBox.Show("Length = "+ decrypted_file_data[0x1c] + ",Amount = "+ decrypted_file_data[0x18], "Debug");
#endif
                                            int position = 0x1c;
                                            for (int i = 0; i < decryptedFileData[0x18]; i++)
                                            {
                                                var nameLength = decryptedFileData[position];
                                                byte[] dataPosition = new byte[nameLength];
                                                Array.Copy(decryptedFileData, position += 2, dataPosition, 0, nameLength);
                                                string str = Encoding.UTF8.GetString(dataPosition);
                                                //_ = (int)MessageBox.Show(str + "," + ch, "Debug");
                                                if (positionNames.Contains(str))
                                                {
                                                    //_ = (int)MessageBox.Show(str + "," + ch, "Debug2");
                                                    // 表示ca3开头第i个单词，装备内部的32字节的第i个代表该部位
                                                    for (int index = 0; index < positionNames.Length; index++)
                                                    {
                                                        if (positionNames[index].Equals(str))
                                                        {
#if debugged
                                                            //_ = (int)MessageBox.Show(index + ","+str+"," +ch, "Debug");
#endif
                                                            positionBinary[index] = i + 1;
                                                        }
                                                    }
                                                }
                                                position += nameLength;
                                            }
#if debug
                                            FileStream fs2 = new FileStream(folder + "\\" + charFolderName + ".txt", FileMode.Create, FileAccess.Write);
                                            StreamWriter export_file = new StreamWriter(fs2);
                                            export_file.Write(ca3Name);
                                            export_file.WriteLine();
                                            for (int i =0;i < positionBinary.Length;i++)
                                            {
                                                // 部位顺序为二进制数值的第i个
                                                export_file.Write(positionBinary[i]);
                                                export_file.WriteLine();
                                            }
                                            export_file.Close();
#endif
                                        }

                                        else
                                        {
                                            // 非角色文件
#if debug
                                            Console.WriteLine("Not CharAnimationFile3 CA3File: " + title);
#endif
                                        }
                                    }
                                }

                            }
                            fs.Seek(nextEntry, 0);
                            num++;
                        }
                    }

                    // 将占用的字节和角色本身的部位进行统一
                    foreach (KeyValuePair<string, string> pair in listOcBinary)
                    {
                        short occupation = 0;

                        for (int i = 0; i < positionBinary.Length; i++)
                        {
                            if (positionBinary[i] > 0)
                            {
                                int twoX = 1;
                                for (int j = 0; j < i; j++)
                                {
                                    twoX *= 2;
                                }
                                occupation += Convert.ToInt16(Convert.ToInt32(pair.Value.Substring(31 - (positionBinary[i] - 1), 1)) * twoX);
                            }
                        }
                        
                        //_ = (int)MessageBox.Show("Name:" + pair.Key + ",Value:" + pair.Value + ",Oc:" + occupation, "debug");
                        if (Convert.ToInt32(pair.Key.Split('_')[2].Split('.')[0]) >= 1000)
                        {
                            // 公共部件编号1001起步，个人部件编号1起步
                            // 如果是公共部件，则转化为公共名称all开头，便于比较
                            listOc[indexInList].Add(pair.Key.Replace(charShortName, charAllShortName), occupation);
                        }
                        else
                        {
                            // 反之直接加入专属
                            listChar.Add(pair.Key, occupation);
                            listOc[indexInList].Add(pair.Key, occupation);
                        }
                    }
                    listOcBinary.Clear();

#if debug
                    FileStream fs3 = new FileStream(folder + "\\" + charFolderName + "occupy.txt", FileMode.Create, FileAccess.Write);
                    StreamWriter export_file2 = new StreamWriter(fs3);
                    foreach (KeyValuePair<string, short> pair in listOc[indexInList])
                    {
                        export_file2.Write(pair.Key + "," + pair.Value);
                        export_file2.WriteLine();
                    }
                    export_file2.Close();
#endif
                }

            }


            // TODO 考虑单写，搞清数量等
            // 直接返回装备字符串+占用二进制+角色，不再使用角色
            //const long constMale = 0b001010011011000111011001010101001;
            //const long constFemale = 0b110101100100011000100110101010110;
            //const long constAll = 0b111111111111011111111111111111111;
            SortedList<string, long[]> listResult = new SortedList<string, long[]>();

            foreach (var item in itemNameList)
            {
                // listName，key = itemNum, value = fileName
                long chars = 0;
                long p = 1; // 注意要和chars类型一致
                int ch = 1;
                long[] values = new long[charList.Count + 1];
                while (ch <= charList.Count)
                {
                    if (listOc[ch - 1].ContainsKey(item))
                    {
                        chars += p;
                        values[ch - 1] = listOc[ch - 1][item];
                    }
#if debug
                    if (chars < 0)
                    {
                        Console.WriteLine();
                    }
#endif
                    p *= 2;
                    ch++;
                }

                values[charList.Count] = chars;
                listResult.Add(item, values);
                
            }
            //_ = (int)MessageBox.Show(ch + "", "Debug3");
            //List_names.Clear();
            return listResult;
        }

        /// <summary>
        /// 报告指定的 System.Byte[] 在此实例中的匹配项的索引枚举。
        /// </summary>
        /// <param name="source">被执行查找的 System.Byte[]</param>
        /// <param name="start">查找的起始位置</param>
        /// <param name="pattern">执行查找的 System.Byte[]</param>
        /// https://www.cnblogs.com/lindexi/p/12086916.html
        private static IEnumerable<long> IndexesOf(byte[] source, int start, byte[] pattern)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            return IndexesOf2();

            IEnumerable<long> IndexesOf2()
            {
                long valueLength = source.LongLength;
                long patternLength = pattern.LongLength;

                if ((valueLength == 0) || (patternLength == 0) || (patternLength > valueLength))
                {
                    yield break;
                }

                var badCharacters = new long[256];

                for (var i = 0; i < 256; i++)
                {
                    badCharacters[i] = patternLength;
                }

                var lastPatternByte = patternLength - 1;

                for (long i = 0; i < lastPatternByte; i++)
                {
                    badCharacters[pattern[i]] = lastPatternByte - i;
                }

                long index = start;

                while (index <= valueLength - patternLength)
                {
                    for (var i = lastPatternByte; source[index + i] == pattern[i]; i--)
                    {
                        if (i == 0)
                        {
                            yield return index;
                            break;
                        }
                    }

                    index += badCharacters[source[index + lastPatternByte]];
                }
            }
        }

        /// <summary>
        /// 判断输入字符串是否为纯数字的函数
        /// </summary>
        /// <param name="message"></param>
        private static bool IsNumberic(string message)
        {
            System.Text.RegularExpressions.Regex rex =
            new System.Text.RegularExpressions.Regex(@"^\d+$");
            return rex.IsMatch(message);
        }


    }
}
