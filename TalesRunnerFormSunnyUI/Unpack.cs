#define debug

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TalesRunnerFormSunnyUI.Data;
using zlib;
using TalesRunnerFormCryptoClassLibrary;

namespace TalesRunnerFormSunnyUI
{
    internal static class Unpack
    {
        /// <summary>
        /// 基础的文本解包
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] Decrypt(byte[] data)
        {
            List<byte> decryptData = new List<byte>();
            byte[] aesKey = StaticVars.aesKey1;
            byte[] xorKey = StaticVars.xorKey1;

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
        /// 目前的文本解包
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private static byte[] Decrypt2(byte[] data, string key)
        {
            List<byte> decryptData = new List<byte>();
            byte[] aesKey = TalesRunnerFormCryptoClassLibrary.GetKeyClass.AesKey(key);
            byte[] xorKey = TalesRunnerFormCryptoClassLibrary.GetKeyClass.XorKey(key);
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

        /// <summary>
        /// 测试解包用文件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool TestKey(string folder, string key)
        {
            string pkgPath = folder + "\\" + "tr4.pkg"; // 获得pkg_path，TODO后期改成文本读取的路径
            string pkgName = "tr4"; // 获取文件名：tr4，TODO后期改成文本读取的路径
            FileStream fs = new FileStream(pkgPath, FileMode.Open, FileAccess.Read); // 打开文件
            using (BinaryReader pkg = new BinaryReader(fs))
            {
                byte[] fileHeader = pkg.ReadBytes(12); // 阅读文件前12字节
                fileHeader = Decrypt(fileHeader); // 解码
                if (Encoding.UTF8.GetString(fileHeader) != "ACAC35E5-4B7")
                {
#if debug
                    Console.WriteLine("Not a valid .pkg file or decryption key has changed");
#endif
                    return false;
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

                    string filePath = pkgName + "\\" + Encoding.GetEncoding(949).GetString(decompressedEntryTitle); // 把目录和文件名合成一个路径
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
                            fileData = Decrypt2(fileData, key);
                        }
                        decryptedFileDataList.AddRange(fileData);
                    }

                    byte[] decryptedFileData = decryptedFileDataList.ToArray();

                    if (filePath.Equals("tr4\\script\\clientiteminfo\\tblavataritemdesc.txt")) // 获取文件信息
                    {
                        List<string> stringList = new List<string>();
                        MemoryStream ms = new MemoryStream(decryptedFileData);
                        StreamReader sr = new StreamReader(ms, Encoding.UTF8);
                        string line = sr.ReadLine();
                        if (line.Equals("_talesrunner_"))
                        {
                            sr.Close();
                            return true;
                        }
                        else
                        {
                            sr.Close();
                            return false;
                        }
                        
                        //decrypted_file_data_list.Clear();
                    }

                    fs.Seek(nextEntry, 0);
                    num++;
                }
            }
            // pkg.Close();
            return false;
        }
    }
}
