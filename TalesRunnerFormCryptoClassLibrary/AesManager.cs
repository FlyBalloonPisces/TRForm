using System;
using System.Collections.Concurrent;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TalesRunnerFormCryptoClassLibrary
{
    internal static class AesManager
    {
        private static readonly ConcurrentDictionary<string, Crypto> _dict = new ConcurrentDictionary<string, Crypto>();

        private static readonly byte[] _defaultKey = new byte[] { 0x0D, 0x68, 0x07, 0x6F, 0x0A, 0x09, 0x07, 0x6C, 0x65, 0x73, 0x0D, 0x75, 0x6E, 0x0A, 0x65, 0x0D };

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
            MemoryStream mStream = new MemoryStream();
            RijndaelManaged aes = new RijndaelManaged();
            byte[] plainBytes = Encoding.UTF8.GetBytes(keytext);

            aes.Key = _defaultKey;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.None;

            if (keytext != "default")
            {
                CryptoStream cryptoStream = new CryptoStream(mStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
                try
                {
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    aes.Key = mStream.ToArray();
                }
                finally
                {
                    cryptoStream.Close();
                    mStream.Close();
                    aes.Clear();
                }
            }

            MemoryStream mStream2 = new MemoryStream();
            plainBytes = aes.Key;
            var xor = new byte[16];
            CryptoStream cryptoStream2 = new CryptoStream(mStream2, aes.CreateEncryptor(), CryptoStreamMode.Write);
            try
            {
                cryptoStream2.Write(plainBytes, 0, plainBytes.Length);
                cryptoStream2.FlushFinalBlock();
                xor = mStream2.ToArray();
            }
            finally
            {
                cryptoStream2.Close();
                mStream2.Close();
                aes.Clear();
            }

            crypto = new Crypto(aes.Key, xor);
            _dict.TryAdd(keytext, crypto);

            return true;
        }

        internal static bool TryGet(string keytext, out Crypto crypto)
        {
            return _dict.TryGetValue(keytext, out crypto);
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


            //internal void Decrypt(ReadOnlySpan<byte> src, Span<byte> dest)
            //{
            //    int n = src.Length % 16;
            //    int len = src.Length - n;
            //    if (len > 0)
            //        _aes.DecryptEcb(src[..len], dest[..len], PaddingMode.None);

            //    if (n == 0)
            //        return;

            //    src = src[len..];
            //    dest = dest[len..];
            //    var vectorSrc = Unsafe.ReadUnaligned<Vector128<byte>>(ref MemoryMarshal.GetReference(src));
            //    var vectorXor = Unsafe.ReadUnaligned<Vector128<byte>>(ref MemoryMarshal.GetArrayDataReference(_xor));
            //    var vectorDest = vectorSrc ^ vectorXor;
            //    Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(dest), vectorDest);
            //}

            //internal void Encrypt(ReadOnlySpan<byte> src, Span<byte> dest)
            //{
            //    int n = src.Length % 16;
            //    int len = src.Length - n;
            //    if (len > 0)
            //        _aes.EncryptEcb(src[..len], dest[..len], PaddingMode.None);

            //    src = src[len..];
            //    dest = dest[len..];
            //    var xor = _xor.AsSpan();
            //    for (int i = 0; i < n; i++)
            //    {
            //        dest[i] = (byte)(src[i] ^ xor[i]);
            //    }
            //}
        }
    }
}
