#define debug

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using zlib;

namespace TRCryptoClassLibrary
{
    internal class CryptClass
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
#if debug
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
#if debug
                Console.WriteLine("title = " + Encoding.UTF8.GetString(decompressed_entry_title));
#endif
                string file_title = Encoding.GetEncoding(949).GetString(decompressed_entry_title); //character/characterXX/xxx.pt1
                string file_path =
                    pkg_dir + "\\" + pkg_name + "\\" + file_title; // 把目录和文件名合成一个路径
                string unpack_path = pkg_name + "\\" + file_title; // 把pkg名和文件名合成一个路径，在解压过程中显示
                //string unpack_path = Encoding.GetEncoding(949).GetString(decompressed_entry_title); // 把pkg名和文件名合成一个路径，在解压过程中显示
                string file_dir = Path.GetDirectoryName(file_path); // 返回文件路径

#if debug
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
    }
}
