using DevIL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using zlib;

namespace TRUnpack
{
    internal static class PkgUnpack
    {
        private static byte[] Decrypt(byte[] data)
        {
            List<byte> decryptData = new List<byte>();
            byte[] aesKey =
            {
                0x0D, 0x68, 0x07, 0x6F, 0x0A, 0x09, 0x07, 0x6C, 0x65, 0x73,
                0x0D, 0x75, 0x6E, 0x0A, 0x65, 0x0D
            };
            byte[] xorKey =
            {
                0x05, 0x5B, 0xCB, 0x64, 0xFB, 0xC2, 0xCE, 0xB4, 0x77, 0x8B,
                0x1B, 0xB8, 0xE9, 0xB5, 0x9C, 0xC6
            };
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
            byte[] aesKey_old =
            {
                0xdb, 0x27, 0xb, 0xbb, 0x82, 0x88, 0xdf, 0xf3, 0x44, 0xee,
                0xef, 0x93, 0x67, 0xd1, 0xb5, 0xc2, 0xb6, 0xda, 0x17, 0x59,
                0x7, 0x75, 0x6, 0x8f, 0x32, 0x4a, 0x9f, 0x29, 0x49, 0x52,
                0x32, 0xc2
            };
            byte[] aesKey = {
                0xc, 0xf6, 0xaf, 0xd5, 0x0, 0x48, 0xfe, 0x99, 0xe1, 0xab, 
                0xf9, 0xb6, 0x70, 0x68, 0xad, 0xcd, 0x28, 0x3, 0x8a, 0x6d, 
                0x16, 0x85, 0xe3, 0x7b, 0xeb, 0x9, 0xb, 0x48, 0x4f, 0xb1, 
                0x7e, 0x3
            };
            byte[] xorKey_old =
            {
                0x1c, 0x67, 0x5b, 0xd4, 0x5b, 0x4a, 0x46, 0x74, 0x31, 0x7,
               0x4b, 0x82, 0xab, 0x3f, 0x55, 0xfd
            };
            byte[] xorKey =
{
                0x7c, 0x82, 0x37, 0xd5, 0x2c, 0xf8, 0x81, 0x9, 0x4d, 0x76, 0x5, 0xf5, 0xe5, 0x47, 0xe8, 0xdf
            };
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

        public static void Unpack(FileInfo fileInfo)
        {
            string pkg_path = fileInfo.FullName; // 获得pkg_path
            string pkg_dir = fileInfo.FullName.Split('.')[0]; // 获取文件夹
            string pkg_name = fileInfo.Name.Split('.')[0]; // 获取文件名：tr1
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

                // Console.WriteLine("file_dir = " + Encoding.UTF8.GetString(decompressed_entry_title));
                string file_path =
                    pkg_dir + "\\" +
                    Encoding.UTF8.GetString(decompressed_entry_title); // 把目录和文件名合成一个路径
                string unpack_path = pkg_name + "\\" +
                    Encoding.UTF8.GetString(decompressed_entry_title); // 把pkg名和文件名合成一个路径，在解压过程中显示
                string file_dir = Path.GetDirectoryName(file_path); // 返回文件路径

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
                if (fileExtension == "png" || fileExtension == "dds" || fileExtension == "jpg" || fileExtension == "jpeg")
                {
                    ShowDds(decrypted_file_data, file_path);
                }
                else
                {
                    // 文档写入
                    FileStream fs2 = new FileStream(file_path, FileMode.Create, FileAccess.Write);
                    BinaryWriter export_file = new BinaryWriter(fs2);
                    export_file.Write(decrypted_file_data);
                    export_file.Close();
                }


                fs.Seek(next_entry, 0);
                num += 1;
                Console.WriteLine("Unpacking files " + num + " / " + file_num + ": " + unpack_path);
            }

            pkg.Close();
            Console.WriteLine("Unpack done");
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

        ///// <summary>
        ///// 判断输入字符串是否为纯数字的函数
        ///// </summary>
        ///// <param name="message"></param>
        //private static bool IsNumberic(string message)
        //{
        //    System.Text.RegularExpressions.Regex rex =
        //    new System.Text.RegularExpressions.Regex(@"^\d+$");
        //    return rex.IsMatch(message);
        //}

        #region DevIL

        private static ImageImporter _mImporter;
        private static ImageExporter _mExporter;

        private static DevIL.Image _mActiveImage;


        /// <summary>
        /// 图片读取初始化
        /// </summary>
        public static void InitDevIl()
        {
            _mImporter = new ImageImporter();
            _mExporter = new ImageExporter();
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
        internal static void ShowDds(byte[] input, string filepath)
        {
            //byte[] input = PkgUnpack.PicFind(PathPkg, offset, pkgNum);
            try
            {
                using (MemoryStream ms = new MemoryStream(input))
                {
                    _mActiveImage = _mImporter.LoadImageFromStream(ms);
                }
            }
            catch
            {
                // int num3 = (int) MessageBox.Show("Failed to read \"" + input + "\".", "Error",
                //     MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }

            _mExporter.SaveImage(_mActiveImage, filepath);
            //_mActiveImage.Dispose();

            //return bitmap;
        }
        #endregion
    }
}