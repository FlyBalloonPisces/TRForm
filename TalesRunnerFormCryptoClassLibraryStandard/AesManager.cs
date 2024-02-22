using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace TalesRunnerFormCryptoClassLibrary
{
    internal static class AesManager
    {
        private static readonly ConcurrentDictionary<string, Crypto> _dict = new ConcurrentDictionary<string, Crypto>();

        private static readonly byte[] _defaultKey = new byte[] { 0x0D, 0x68, 0x07, 0x6F, 0x0A, 0x09, 0x07, 0x6C, 0x65, 0x73, 0x0D, 0x75, 0x6E, 0x0A, 0x65, 0x0D };

        //private static readonly byte[] _defaultKeyXor = new byte[] { 0x5, 0x5b, 0xcb, 0x64, 0xfb, 0xc2, 0xce, 0xb4, 0x77, 0x8b, 0x1b, 0xb8, 0xe9, 0xb5, 0x9c, 0xc6 };

        internal static bool TryAddOrGetKey(string keytext, out Crypto crypto)
        {
            if (keytext != "default" && (keytext.Length < 16 || keytext.Length > 38))
            {
                crypto = null;
                return false;
            }

            if (_dict.TryGetValue(keytext, out crypto))
            {
                return true;
            }

            byte[] data = Encoding.UTF8.GetBytes(keytext);
            List<byte> encryptData = new List<byte>();

            using (RijndaelManaged aes = new RijndaelManaged())
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;
                aes.Key = _defaultKey;

                if (keytext != "default")
                {
                    ICryptoTransform cTransform = aes.CreateEncryptor();
                    byte[] outData = cTransform.TransformFinalBlock(data, 0, data.Length);
                    encryptData.AddRange(outData);
                    aes.Key = encryptData.ToArray();
                    encryptData.Clear();
                }


                byte[] data2 = new byte[16];
                Array.Copy(aes.Key, 0, data2, 0, data2.Length);
                ICryptoTransform cTransform2 = aes.CreateEncryptor();
                byte[] outData2 = cTransform2.TransformFinalBlock(data2, 0, data2.Length);
                encryptData.AddRange(outData2);
                byte[] xor = encryptData.ToArray();


                crypto = new Crypto(aes.Key, xor);
                _dict.TryAdd(keytext, crypto);

            }
            return true;
        }

        internal class Crypto
        {
            private readonly byte[] _aes;
            private readonly byte[] _xor;

            internal Crypto(byte[] aes, byte[] xor)
            {
                _aes = aes;
                _xor = xor;
            }

            public byte[] GetAes()
            {
                return _aes;
            }

            public byte[] GetXor()
            {
                return _xor;
            }


            internal byte[] Decrypt(byte[] data)
            {
                List<byte> decryptData = new List<byte>();
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.None;
                    aes.Key = _aes;

                    int length = data.Length;
                    int i = 0;
                    if (length >= 16)
                    {
                        int groupNum = length / 16;
                        ICryptoTransform cTransform = aes.CreateDecryptor();
                        byte[] outData = cTransform.TransformFinalBlock(data, 0, groupNum * 16);
                        decryptData.AddRange(outData);
                        i = groupNum * 16;
                    }
                    while (i < length)
                    {
                        decryptData.Add(Convert.ToByte(data[i] ^ _xor[i % 16]));
                        i++;
                    }
                    return decryptData.ToArray();

                }
            }

            internal byte[] Encrypt(byte[] data)
            {
                List<byte> encryptData = new List<byte>();
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.None;
                    aes.Key = _aes;

                    int length = data.Length;
                    int i = 0;
                    if (length >= 16)
                    {
                        int groupNum = length / 16;
                        ICryptoTransform cTransform = aes.CreateEncryptor();
                        byte[] outData = cTransform.TransformFinalBlock(data, 0, groupNum * 16);
                        encryptData.AddRange(outData);
                        i = groupNum * 16;
                    }
                    while (i < length)
                    {
                        encryptData.Add(Convert.ToByte(data[i] ^ _xor[i % 16]));
                        i++;
                    }
                    return encryptData.ToArray();

                }
            }
        }
    }
}
