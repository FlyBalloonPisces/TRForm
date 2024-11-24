using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using zlib;

namespace LiteDBTest
{
    internal class Unpack
    {
        public static string[] UnpackForSpecificFile(FileInfo fileInfo, string fileName)
        {
            List<string> texts = new List<string>();
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
                return null;
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
                string file_title = Encoding.GetEncoding(949).GetString(decompressed_entry_title);
                string file_path = pkg_dir + "\\" + file_title; // 把目录和文件名合成一个路径
                string unpack_path = file_title; // 把pkg名和文件名合成一个路径，在解压过程中显示
                string[] temp = file_title.Split('\\');
                string file_name = temp[temp.Length - 1];
                //string unpack_path = pkg_name + "\\" + file_title; // 把pkg名和文件名合成一个路径，在解压过程中显示
                //string unpack_path = Encoding.GetEncoding(949).GetString(decompressed_entry_title); // 把pkg名和文件名合成一个路径，在解压过程中显示
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
                    int file_size = BitConverter.ToInt32(pkg.ReadBytes(4), 0); // encrypted file data size
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
                        //file_data = Decrypt(file_data);
                    }

                    decrypted_file_data_list.AddRange(file_data);
                }

                byte[] decrypted_file_data = decrypted_file_data_list.ToArray();

                if (file_name.Equals(fileName)) // 获取文件信息
                {
                    List<string> stringList = new List<string>();
                    MemoryStream ms = new MemoryStream(decrypted_file_data);
                    StreamReader sr = new StreamReader(ms, Encoding.GetEncoding(949));
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        texts.Add(line);
                    }

                    sr.Close();
                    //decrypted_file_data_list.Clear();
                }

                //if (!Directory.Exists(file_dir))
                //{
                //    Directory.CreateDirectory(file_dir);
                //}

                //string[] file_name_split = file_path.Split('.');
                //string fileExtension = file_name_split[file_name_split.Length - 1].ToLower();
                //if (fileExtension == "png" || fileExtension == "dds" || fileExtension == "jpg" || fileExtension == "jpeg")
                //{
                //    ShowDds(decrypted_file_data, file_path);
                //    //FileStream fs2 = new FileStream(file_path, FileMode.Create, FileAccess.Write);
                //    //BinaryWriter export_file = new BinaryWriter(fs2);
                //    //export_file.Write(decrypted_file_data);
                //    //export_file.Close();
                //}
                //else
                //{
                //    // 文档写入
                //    FileStream fs2 = new FileStream(file_path, FileMode.Create, FileAccess.Write);
                //    BinaryWriter export_file = new BinaryWriter(fs2);
                //    export_file.Write(decrypted_file_data);
                //    export_file.Close();
                //}
                fs.Seek(next_entry, 0);
                num += 1;
                Console.WriteLine("Unpacking files " + num + " / " + file_num + ": " + unpack_path);
            }

            pkg.Close();
            Console.WriteLine("Unpack done");
            GC.Collect();

            return texts.ToArray();
        }


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
            int keyNum = 1;
            byte[][] aesKeys =
            {
new byte[]
{
0xd7, 0x7b, 0xe2, 0xf8, 0x67, 0x26, 0xcf, 0x24, 0x49, 0x29,
0xb5, 0xcc, 0x2c, 0x18, 0x43, 0x6d, 0xe1, 0xbb, 0xe3, 0xf6,
0xdd, 0x60, 0x28, 0xb8, 0x3e, 0xa9, 0xf0, 0x3f, 0x8, 0xf0,
0x53, 0xed
},
new byte[]
{
0x3c, 0x5, 0x64, 0x7c, 0xc8, 0xa0, 0xa8, 0x69, 0xf, 0xc0,
0x14, 0x5b, 0xb3, 0x8e, 0x47, 0xf8, 0x51, 0xaa, 0xe3, 0xca,
0x77, 0xdc, 0xbe, 0x36, 0x80, 0x52, 0x1d, 0xb8, 0xb, 0xae,
0x78, 0xfa
},
new byte[]
{
0xc, 0xf6, 0xaf, 0xd5, 0x0, 0x48, 0xfe, 0x99, 0xe1, 0xab,
0xf9, 0xb6, 0x70, 0x68, 0xad, 0xcd, 0x28, 0x3, 0x8a, 0x6d,
0x16, 0x85, 0xe3, 0x7b, 0xeb, 0x9, 0xb, 0x48, 0x4f, 0xb1,
0x7e, 0x3
},
new byte[]
{
0xdb, 0x27, 0xb, 0xbb, 0x82, 0x88, 0xdf, 0xf3, 0x44, 0xee,
0xef, 0x93, 0x67, 0xd1, 0xb5, 0xc2, 0xb6, 0xda, 0x17, 0x59,
0x7, 0x75, 0x6, 0x8f, 0x32, 0x4a, 0x9f, 0x29, 0x49, 0x52,
0x32, 0xc2
},
new byte[]
{
0xfd, 0xd7, 0x15, 0xcb, 0xbe, 0xbf, 0xa5, 0xff, 0xef, 0x9e,
0xed, 0x97, 0xce, 0x96, 0xd3, 0xf, 0x4c, 0xdc, 0xa0, 0x1d,
0xaf, 0x5f, 0xcf, 0xa2, 0xd8, 0xb1, 0x58, 0x8, 0xb9, 0xb6,
0xc1, 0xa
},
new byte[]
{
0x25, 0x84, 0x11, 0xa0, 0x1c, 0x66, 0x52, 0x96, 0x2, 0xbe,
0x36, 0x41, 0xc6, 0x53, 0x44, 0xe2, 0xec, 0xf9, 0x19, 0xb4,
0x5a, 0xc5, 0x17, 0x36, 0xe5, 0x45, 0x5a, 0xaf, 0x36, 0xc6,
0xef, 0x1f
},
new byte[]
{
0xfe, 0x62, 0x32, 0xf9, 0xd2, 0x60, 0xdc, 0x61, 0x58, 0x45,
0x2e, 0xdb, 0x4c, 0xc9, 0x58, 0x73, 0x96, 0xc0, 0x4e, 0xc4,
0x88, 0xf3, 0x37, 0xa2, 0x2, 0xb6, 0xc5, 0xe3, 0xd9, 0xa4,
0xe7, 0xd5
},
new byte[]
{
0x9a, 0x2a, 0xd7, 0xbc, 0x4a, 0xae, 0xbb, 0xfc, 0x41, 0xb8,
0x9a, 0x56, 0x30, 0x1b, 0x8c, 0x4a, 0x3f, 0x80, 0xd4, 0x11,
0xee, 0x15, 0x23, 0x4b, 0x12, 0x6c, 0x72, 0x3f, 0x7a, 0xa8,
0x53, 0x20
},
new byte[]
{
0x84, 0x87, 0x25, 0x7d, 0x60, 0xd3, 0x66, 0xc9, 0x72, 0xed,
0x61, 0x37, 0x64, 0xa9, 0xd1, 0xe5, 0x37, 0xbc, 0xe3, 0x69,
0x23, 0x73, 0xb9, 0xd7, 0xac, 0x26, 0xc6, 0x37, 0x6, 0x5,
0xde, 0x40
},
new byte[]
{
0xd8, 0x27, 0x3a, 0xe5, 0xb9, 0xc, 0x43, 0x85, 0x1c, 0xa9,
0x43, 0x90, 0x23, 0x62, 0x32, 0xa0, 0x8d, 0xaa, 0x88, 0xb7,
0xfe, 0x86, 0x4d, 0x45, 0x22, 0x35, 0x18, 0x59, 0xc0, 0xaa,
0xb4, 0x2f
}
            };
            byte[][] xorKeys =
            {
new byte[]
{
0xb8, 0x1d, 0xb1, 0x7f, 0x9d, 0x1e, 0xb1, 0xc9, 0x7e, 0x19,
0xf4, 0x6f, 0xd6, 0x7c, 0x4c, 0xcc
},
new byte[]
{
0x41, 0x68, 0x1f, 0x16, 0x88, 0xa8, 0xa8, 0x7, 0xe, 0x5a,
0x7b, 0x9f, 0x17, 0x38, 0x10, 0x51
},
new byte[]
{
0x7c, 0x82, 0x37, 0xd5, 0x2c, 0xf8, 0x81, 0x9, 0x4d, 0x76,
0x5, 0xf5, 0xe5, 0x47, 0xe8, 0xdf
},
new byte[]
{
0x1c, 0x67, 0x5b, 0xd4, 0x5b, 0x4a, 0x46, 0x74, 0x31, 0x7,
0x4b, 0x82, 0xab, 0x3f, 0x55, 0xfd
},
new byte[]
{
0x20, 0x44, 0xb2, 0xa3, 0x63, 0xc7, 0x47, 0x88, 0x4d, 0x1e,
0x2f, 0x12, 0x90, 0x39, 0x3c, 0x8e
},
new byte[]
{
0x94, 0x45, 0x31, 0xc, 0x5f, 0xbf, 0x71, 0x76, 0xa6, 0x1,
0x52, 0xa7, 0x1e, 0x3a, 0x16, 0xb7
},
new byte[]
{
0xda, 0xf0, 0xc9, 0x4c, 0xc7, 0x76, 0x3d, 0xc9, 0x59, 0xa4,
0xf1, 0x12, 0x56, 0xf6, 0xe0, 0xd5
},
new byte[]
{
0x70, 0xb4, 0x91, 0x4b, 0x8e, 0xfe, 0xe9, 0x90, 0x7c, 0xdf,
0xcf, 0x85, 0x50, 0x38, 0x41, 0x13
},
new byte[]
{
0x5f, 0xfe, 0x31, 0x51, 0x4c, 0x2d, 0xc2, 0xf5, 0x1b, 0x8d,
0xc0, 0x20, 0x86, 0xea, 0xd3, 0x25
},
new byte[]
{
0x8b, 0x30, 0x69, 0x69, 0xa2, 0x2, 0x1a, 0xe0, 0x43, 0xbc,
0xe, 0x9b, 0xdc, 0xa8, 0xdf, 0x64
}
            };
            // MemoryStream mStream = new MemoryStream(data);
            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                aes.Key = aesKeys[keyNum];

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
                    decryptData.Add(Convert.ToByte(data[i] ^ xorKeys[keyNum][i % 16]));
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

    }
}
