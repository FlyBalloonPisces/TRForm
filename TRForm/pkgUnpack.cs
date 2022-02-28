using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using zlib;

namespace TalesRunnerForm
{
    internal static class PkgUnpack
    {
        private static byte[] Decrypt(byte[] data)
        {
            List<byte> decryptData = new List<byte>();
            byte[] aesKey = TrData.keysVersion ? StaticVars.aesKey1_kr : StaticVars.aesKey1_hk;
            byte[] xorKey = TrData.keysVersion ? StaticVars.xorKey1_kr : StaticVars.xorKey1_hk;

            using (RijndaelManaged aes = new RijndaelManaged())
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

        private static byte[] Decrypt2(byte[] data)
        {
            List<byte> decryptData = new List<byte>();
            byte[] aesKey = TrData.keysVersion ? StaticVars.aesKey2_kr : StaticVars.aesKey2_hk;
            byte[] xorKey = TrData.keysVersion ? StaticVars.xorKey2_kr : StaticVars.xorKey2_hk;
            // MemoryStream mStream = new MemoryStream(data);
            using (RijndaelManaged aes = new RijndaelManaged())
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

        //private static void Unpack(FileInfo fileInfo)
        //{
        //    String pkg_path = fileInfo.FullName; // 获得pkg_path
        //    String pkg_name = fileInfo.Name.Split('.')[0]; // 获取文件名：tr1
        //                                                   // Console.WriteLine("pkg_name = " + pkg_name);
        //    FileStream fs = new FileStream(pkg_path, FileMode.Open, FileAccess.Read); // 打开文件
        //    BinaryReader pkg = new BinaryReader(fs);
        //    byte[] file_header = pkg.ReadBytes(12); // 阅读文件前12字
        //    file_header = decrypt(file_header); // 解码
        //    if (Encoding.UTF8.GetString(file_header) != "ACAC35E5-4B7")
        //    {
        //        Console.WriteLine("Not a valid .pkg file or decryption key has changed");
        //        return;
        //    }

        //    fs.Seek(20, SeekOrigin.Begin); // 从头开始，偏移0x14，读取
        //    int offset = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取偏移量
        //    fs.Seek(offset, 0); // 从头开始，偏移offset，读取
        //    fs.Seek(4, SeekOrigin.Current); // 从当前位置开始，偏移0x4，读取
        //    int file_num = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取文件数量
        //    fs.Seek(4, SeekOrigin.Current); // 从当前位置开始，偏移0x4，读取
        //    int num = 0; // 解压的文件数量
        //                 // 解包，一个文件分成多个单元进行分解
        //    while (num < file_num)
        //    {
        //        int entry_size = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取文件，反序读取4个字节
        //                                                                    // Console.WriteLine("file_dir = " + entry_size);
        //        byte[] entry_data = pkg.ReadBytes(entry_size); // 读取文件
        //        long next_entry = pkg.BaseStream.Position; // 返回文件当前位置
        //        byte[] decompressed_entry_data = deCompressBytes(entry_data); // 解压缩文件数据
        //        int length = 0;
        //        for (int i = 0; i < decompressed_entry_data.Length; i++)
        //        {
        //            if (decompressed_entry_data[i] == 0)
        //            {
        //                length = i;
        //                break;
        //            }
        //        }

        //        byte[] decompressed_entry_title = new byte[length];
        //        for (int i = 0; i < decompressed_entry_title.Length; i++)
        //        {
        //            decompressed_entry_title[i] = decompressed_entry_data[i];
        //        }

        //        // Console.WriteLine("file_dir = " + Encoding.UTF8.GetString(decompressed_entry_title));
        //        String file_path =
        //            pkg_name + "\\" +
        //            Encoding.UTF8.GetString(decompressed_entry_title); // 把目录和文件名合成一个路径
        //        String file_dir = Path.GetDirectoryName(file_path); // 返回文件路径

        //        int part_num =
        //            BitConverter.ToInt32(decompressed_entry_data.Skip(0x410).Take(4)
        //                .ToArray(), 0); // 获得单元数量
        //        offset = BitConverter.ToInt32(decompressed_entry_data.Skip(0x414).Take(4)
        //            .ToArray(), 0); // 获取偏移量
        //        fs.Seek(offset, SeekOrigin.Begin);
        //        List<byte> decrypted_file_data_list = new List<byte>();
        //        for (int i = 0; i < part_num; i++)
        //        {
        //            fs.Seek(0x8, SeekOrigin.Current);
        //            int file_size =
        //                BitConverter.ToInt32(pkg.ReadBytes(4), 0); // encrypted file data size
        //            fs.Seek(0x4, SeekOrigin.Current);
        //            int encrypt_type = BitConverter.ToInt32(pkg.ReadBytes(4), 0);
        //            Console.WriteLine("encrypt_type1 = " + (encrypt_type & 1));
        //            Console.WriteLine("encrypt_type2 = " + (encrypt_type & 2));
        //            byte[] file_data = pkg.ReadBytes(file_size); // encrypted file data
        //            if ((encrypt_type & 1) == 1)
        //            {
        //                file_data = deCompressBytes(file_data);
        //            }

        //            if ((encrypt_type & 2) == 2)
        //            {
        //                file_data = decrypt2(file_data);
        //            }

        //            decrypted_file_data_list.AddRange(file_data);
        //        }

        //        byte[] decrypted_file_data = decrypted_file_data_list.ToArray();

        //        // 文档写入
        //        if (!Directory.Exists(file_dir))
        //        {
        //            Directory.CreateDirectory(file_dir);
        //        }

        //        FileStream fs2 = new FileStream(file_path, FileMode.Create, FileAccess.Write);
        //        BinaryWriter export_file = new BinaryWriter(fs2);
        //        export_file.Write(decrypted_file_data);
        //        export_file.Close();
        //        fs.Seek(next_entry, 0);
        //        num += 1;
        //    }

        //    pkg.Close();
        //    Console.WriteLine("Unpack done");
        //}

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

        public static SortedList<string, string[]> TxtText(FileInfo fileInfo, string[] str, Encoding[] encoding)
        {
            SortedList<string, string[]> returnList = new SortedList<string, string[]>();
            string pkgPath = fileInfo.FullName; // 获得pkg_path
            string pkgName = fileInfo.Name.Split('.')[0]; // 获取文件名：tr1
            // Console.WriteLine("pkg_name = " + pkg_name);
            FileStream fs = new FileStream(pkgPath, FileMode.Open, FileAccess.Read); // 打开文件
            using (BinaryReader pkg = new BinaryReader(fs))
            {
                // Console.WriteLine("Length = " + fs.Length);
                byte[] fileHeader = pkg.ReadBytes(12); // 阅读文件前12字节
                fileHeader = Decrypt(fileHeader); // 解码
                if (Encoding.UTF8.GetString(fileHeader) != "ACAC35E5-4B7")
                {
                    // int message = (int) MessageBox.Show("读取装备列表失败!", "错误");
                    // Environment.Exit(0);
                    // Console.WriteLine("Not a valid .pkg file or decryption key has changed");
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
                    // Console.WriteLine("entry_size = " + entry_size);
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

                    // Console.WriteLine("file_dir = " + encoding.GetString(decompressed_entry_title));
                    string filePath = pkgName + "\\" + Encoding.UTF8.GetString(decompressedEntryTitle); // 把目录和文件名合成一个路径
                    // String file_dir = Path.GetDirectoryName(file_path); // 返回文件路径
                    // Console.WriteLine("path = "+file_path);
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
                            // Console.WriteLine("2");
                            fileData = DeCompressBytes(fileData);
                        }

                        if ((encryptType & 2) == 2)
                        {
                            fileData = Decrypt2(fileData);
                        }
                        decryptedFileDataList.AddRange(fileData);
                    }

                    byte[] decryptedFileData = decryptedFileDataList.ToArray();
                    for (int index = 0; index < str.Length; index++)
                    {
                        if (filePath.Equals("tr4\\script\\" + str[index] + ".txt")) // 获取文件信息
                        {
                            List<string> stringList = new List<string>();
                            MemoryStream ms = new MemoryStream(decryptedFileData);
                            StreamReader sr = new StreamReader(ms, encoding[index]);
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                stringList.Add(line);
                            }

                            sr.Close();
                            //decrypted_file_data_list.Clear();
                            returnList.Add(str[index], stringList.ToArray());
                        }
                    }
                    fs.Seek(nextEntry, 0);
                    num++;
                }
            }
            // pkg.Close();
            return returnList;
        }

        public static SortedList<string, string> PicOffset(FileInfo fileInfo)
        {
            SortedList<string, string> listPic = new SortedList<string, string>();
            string pkgDir = fileInfo.FullName; // 获得pkg_path
            // string pkgName = fileInfo.Name.Split('.')[0]; // 获取文件名：tr9
            string[] tr_pkg = TrData.keysVersion ? StaticVars.tr_pkg_kr : StaticVars.tr_pkg_hk;
            // Console.WriteLine("pkg_name = " + pkg_name);
            for (int pkgNum = 0; pkgNum < tr_pkg.Length; pkgNum++)
            {
                string pkgPath = pkgDir + "\\tr" + tr_pkg[pkgNum] + ".pkg";
                FileStream fs = new FileStream(pkgPath, FileMode.Open, FileAccess.Read); // 打开文件
                using (BinaryReader pkg = new BinaryReader(fs))
                {
                    byte[] fileHeader = pkg.ReadBytes(12); // 阅读文件前12字
                    fileHeader = Decrypt(fileHeader); // 解码
                    if (Encoding.UTF8.GetString(fileHeader) != "ACAC35E5-4B7")
                    {
                        // int message = (int) MessageBox.Show("读取装备图片失败!", "错误");
                        // Environment.Exit(0);
                        // Console.WriteLine("Not a valid .pkg file or decryption key has changed");
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
                        long entryOffset = pkg.BaseStream.Position;
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
                        // Console.WriteLine("file_dir = " + Encoding.UTF8.GetString(decompressed_entry_title));
                        //string filePath =
                        //    pkgName + "\\" +
                        //    Encoding.UTF8.GetString(decompressedEntryTitle); // 把目录和文件名合成一个路径
                        string[] filePaths = Encoding.UTF8.GetString(decompressedEntryTitle).Split('\\'); // 把目录和文件名合成一个路径
                        string fileName = filePaths[filePaths.Length - 1];
                        //if (filePaths.Length <= 2 || fileName.Split('_').Length > 3)
                        //{
                        //    int xyz = 1;
                        //}
                        string[] fileNames = fileName.Split('_');
                        if (fileNames.Length > 1)
                        {
                            // 非carditemimg.png类似道具
                            string filePosition = fileNames[0];
                            if (fileNames.Length >= 3)
                            {
                                // 包括SSS
                                filePosition = fileNames[1];
                            }
                            else if (fileNames.Length == 2)
                            {
                                filePosition = fileNames[0];
                            }
                            string folderPosition = filePaths[2]; // topbody等位置名称
                            if (filePosition == folderPosition || folderPosition == "etc")
                            {
                                string entryFileAndOffset = tr_pkg[pkgNum] + "," + entryOffset.ToString();
                                if (!listPic.ContainsKey(fileName))
                                {
                                    listPic.Add(fileName, entryFileAndOffset);

                                }
                            }
                        }
                        else
                        {
                            string entryFileAndOffset = tr_pkg[pkgNum] + "," + entryOffset.ToString();
                            if (!listPic.ContainsKey(fileName))
                            {
                                listPic.Add(fileName, entryFileAndOffset);

                            }
                        }
                        fs.Seek(nextEntry, 0);
                        num++;
                    }
                }
            }

            //pkg.Close();
            return listPic;
        }

        public static byte[] PicFind(string str, long picOffset, short pkgNum)
        {
            string pkgPath = str + "\\tr" + pkgNum + ".pkg"; // 获得pkg_path
            FileStream fs = new FileStream(pkgPath, FileMode.Open, FileAccess.Read); // 打开文件
            using (BinaryReader pkg = new BinaryReader(fs))
            {
                byte[] fileHeader = pkg.ReadBytes(12); // 阅读文件前12字
                fileHeader = Decrypt(fileHeader); // 解码
                if (Encoding.UTF8.GetString(fileHeader) != "ACAC35E5-4B7")
                {
                    // _ = (int)MessageBox.Show("读取装备图片失败!", "错误");
                    // Environment.Exit(0);
                    // Console.WriteLine("Not a valid .pkg file or decryption key has changed");
                    return null;
                }
                //file_header = null;
                fs.Seek(picOffset, SeekOrigin.Begin); // 从头开始，偏移0x14，读取
                int entrySize = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // 读取文件，反序读取4个字节
                                                                           // Console.WriteLine("file_dir = " + entry_size);
                byte[] entryData = pkg.ReadBytes(entrySize); // 读取文件
                byte[] decompressedEntryData = DeCompressBytes(entryData); // 解压缩文件数据
                //entry_data = null;
                int partNum =
                    BitConverter.ToInt32(decompressedEntryData.Skip(0x410).Take(4)
                        .ToArray(), 0); // 获得单元数量
                long offset = BitConverter.ToInt32(decompressedEntryData.Skip(0x414).Take(4)
                    .ToArray(), 0); // 获取偏移量
                //decompressed_entry_data = null;
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
                    //file_data = null;
                }
                // byte[] decrypted_file_data = decrypted_file_data_list.ToArray();
                // decrypted_file_data_list.Clear();
                // MemoryStream ms = new MemoryStream(decrypted_file_data);
                // decrypted_file_data = null; 

                //pkg.Close();
                // return ms;

                return decryptedFileDataList.ToArray();

            }
        }

        private delegate void PbValue(int n);

        internal static SortedList<int, int[]> Occupation(FileInfo fileInfo, SortedList<int, string> listNames, BackgroundWorker bw)
        {
            // 全角色占用


            // 各角色公共服饰
            SortedList<string, short>[] listOc = new SortedList<string, short>[TrData.Characters];
            // 各角色专属服饰
            SortedList<string, short> listChar = new SortedList<string, short>();

            int ch = 1;
            while (ch <= TrData.Characters)
            {
                string pkgPath = fileInfo.FullName + "\\char" + StaticVars.CharacterPkg[ch] + ".pkg"; // 获得pkg_path
                FileStream fs = new FileStream(pkgPath, FileMode.Open, FileAccess.Read); // 打开文件
                listOc[ch - 1] = new SortedList<string, short>();

                SortedList<string, string> listOcBinary = new SortedList<string, string>();
                int[] positionBinary = new int[StaticVars.Positions];
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
                        if (Encoding.UTF8.GetString(decompressedEntryTitle).Split('\\').Length == 3)
                        {
                            if (Encoding.UTF8.GetString(decompressedEntryTitle).Split('\\')[1].Equals("character" + ch))
                            {
                                string fileName = Encoding.UTF8.GetString(decompressedEntryTitle).Split('\\')[2]; // 把目录和文件名合成一个路径
                                //string test = Encoding.UTF8.GetString(decompressedEntryTitle);
                                if (fileName.Split('.').Length == 2 && (fileName.Split('_')[0] + "_").Equals(StaticVars.Character[ch]) && fileName.Split('_').Length >= 3)
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
                                    }
                                }
                                else if (fileName.Equals(StaticVars.Character[ch] + "set.ca3")) // 获取角色本身的各部位编号
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
                                        //_ = (int)MessageBox.Show("Length = "+ decrypted_file_data[0x1c] + ",Amount = "+ decrypted_file_data[0x18], "Debug");
                                        int position = 0x1c;
                                        for (int i = 0; i < decryptedFileData[0x18]; i++)
                                        {
                                            var nameLength = decryptedFileData[position];
                                            byte[] dataPosition = new byte[nameLength];
                                            Array.Copy(decryptedFileData, position += 2, dataPosition, 0, nameLength);
                                            string str = Encoding.UTF8.GetString(dataPosition);
                                            //_ = (int)MessageBox.Show(str + "," + ch, "Debug");
                                            if (StaticVars.positionNames.Contains(str))
                                            {
                                                //_ = (int)MessageBox.Show(str + "," + ch, "Debug2");
                                                for (int index = 0; index < StaticVars.positionNames.Length; index++)
                                                {
                                                    if (StaticVars.positionNames[index].Equals(str))
                                                    {
                                                        //_ = (int)MessageBox.Show(index + ","+str+"," +ch, "Debug");
                                                        positionBinary[index] = i + 1;
                                                    }
                                                }
                                            }
                                            position += nameLength;
                                        }

                                        //FileStream fs2 = new FileStream(Form1.path + "Character"+ch+"_"+Form2.character[ch].Replace("_", "") + ".txt", FileMode.Create, FileAccess.Write);
                                        //StreamWriter export_file = new StreamWriter(fs2);
                                        //export_file.Write(Form2.character[ch].Replace("_",""));
                                        //export_file.WriteLine();
                                        //for (int i =0;i < PositionBinary.Length;i++)
                                        //{
                                        //    export_file.Write(PositionBinary[i]-1);
                                        //    export_file.WriteLine();
                                        //}
                                        //export_file.Close();
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
                        //if (!listOc[ch - 1].ContainsKey(pair.Key.Replace(TrData.Character[ch], TrData.Character[0])))
                        //{
                        // 如果是公共部件，则转化为公共名称all_开头
                        listOc[ch - 1].Add(pair.Key.Replace(StaticVars.Character[ch], StaticVars.Character[0]), occupation);
                        //}

                    }
                    else
                    {
                        //if (!listChar.ContainsKey(pair.Key))
                        //{
                        // 反之直接加入专属
                        listChar.Add(pair.Key, occupation);
                        //}

                    }
                }
                listOcBinary.Clear();
                //_ = (int)MessageBox.Show(ch + "", "Debug");
                PbValue pb = bw.ReportProgress;
                pb(8 + (2 * ch));
                //Action action = () =>
                //{
                //    bw.PBarValue(8 + (2 * ch));
                //};
                //bw.Invoke(action);
                //fm.PBarValue(8 + (2 * ch));

                ch++;
            }
            //FileStream fs2 = new FileStream(Form1.path + "itemoccupyChar.txt", FileMode.Create, FileAccess.Write);
            //StreamWriter export_file = new StreamWriter(fs2);
            //foreach (KeyValuePair<string, short> pair in List_oc)
            //{
            //    export_file.Write(pair.Key + "," + pair.Value);
            //    export_file.WriteLine();
            //}
            //export_file.Close();
            // TODO 已到 露露亚
            const int constMale = 0b010000011000111011001010101001;
            const int constFemale = 0b101100100011000100110101010110;
            const int constAll = 0b111100111011111111111111111111;
            int pos = 1;
            SortedList<int, int[]> listResult = new SortedList<int, int[]>();
            foreach (KeyValuePair<int, string> pair in listNames)
            {
                int chars = 0;
                int p = 1;
                ch = 1;
                while (ch <= TrData.Characters)
                {
                    if (listOc[ch - 1].ContainsKey(pair.Value))
                    {
                        chars += p;
                    }
                    p *= 2;
                    ch++;
                }
                if (chars != 0)
                {
                    if ((chars & constAll) == constAll)
                    {
                        listResult.Add(pair.Key, new[] { 0, listOc[19][pair.Value], chars });
                    }
                    else if ((chars & constMale) == constMale)
                    {
                        listResult.Add(pair.Key, new[] { 1, listOc[17][pair.Value], chars });
                    }
                    else if ((chars & constFemale) == constFemale)
                    {
                        listResult.Add(pair.Key, new[] { 2, listOc[19][pair.Value], chars });
                    }
                    else
                    {
                        const byte flag = 1;
                        int i = 0;
                        int chars2 = chars;
                        while ((chars2 & flag) == 0)
                        {
                            chars2 >>= 1;
                            i++;
                        }
                        listResult.Add(pair.Key, new[] { 3, listOc[i][pair.Value], chars });
                    }
                }
                else
                {
                    if (listChar.ContainsKey(pair.Value))
                    {
                        listResult.Add(pair.Key, new[] { 0, listChar[pair.Value], 0 });
                    }
                }
                if (pair.Key >= pos * 1000)
                {
                    //_ = (int)MessageBox.Show(pair.Key + "," + Pos, "Debug2");
                    pos = (pair.Key / 1000) + 1;
                }
            }
            //_ = (int)MessageBox.Show(ch + "", "Debug3");
            //List_names.Clear();
            return listResult;
        }

        ///// <summary>
        ///// 报告指定的 System.Byte[] 在此实例中的第一个匹配项的索引。
        ///// </summary>
        ///// <param name="srcBytes">被执行查找的 System.Byte[]。</param>
        ///// <param name="searchBytes">要查找的 System.Byte[]。</param>
        ///// <returns>如果找到该字节数组，则为 searchBytes 的索引位置；如果未找到该字节数组，则为 -1。如果 searchBytes 为 null 或者长度为0，则返回值为 -1。</returns>
        ///// 版权声明：本文为CSDN博主「微wx笑」的原创文章，遵循 CC 4.0 BY - SA 版权协议，转载请附上原文出处链接及本声明。
        ///// 原文链接：https://blog.csdn.net/testcs_dn/java/article/details/25276333
        //private static int IndexOf(byte[] srcBytes, byte[] searchBytes)
        //{
        //    if (srcBytes == null) { return -1; }
        //    if (searchBytes == null) { return -1; }
        //    if (srcBytes.Length == 0) { return -1; }
        //    if (searchBytes.Length == 0) { return -1; }
        //    if (srcBytes.Length < searchBytes.Length) { return -1; }
        //    for (int i = 0; i < srcBytes.Length - searchBytes.Length; i++)
        //    {
        //        if (srcBytes[i] == searchBytes[0])
        //        {
        //            if (searchBytes.Length == 1) { return i; }
        //            bool flag = true;
        //            for (int j = 1; j < searchBytes.Length; j++)
        //            {
        //                if (srcBytes[i + j] != searchBytes[j])
        //                {
        //                    flag = false;
        //                    break;
        //                }
        //            }
        //            if (flag) { return i; }
        //        }
        //    }
        //    return -1;
        //}

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