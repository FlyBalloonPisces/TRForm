using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Security.Cryptography;
using System.Text;

namespace TRToolkit.Package
{
    internal static class AesManager
    {
        private static readonly ConcurrentDictionary<string, Crypto> _dict = new ();

        private static readonly byte[] _defaultKey = new byte[] { 0x0D, 0x68, 0x07, 0x6F, 0x0A, 0x09, 0x07, 0x6C, 0x65, 0x73, 0x0D, 0x75, 0x6E, 0x0A, 0x65, 0x0D };

        internal static bool TryAddOrGetKey(string keytext, [MaybeNullWhen(false)] out Crypto crypto)
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

            Aes aes = Aes.Create();
            aes.Key = _defaultKey;

            if (keytext != "default")
            {
                int n = keytext.Length == 32 ? 32 : 16;
                Span<byte> buffer = stackalloc byte[n];
                Encoding.ASCII.GetBytes(keytext, buffer);
                aes.Key = aes.EncryptEcb(buffer, PaddingMode.None);
            }

            var xor = aes.EncryptEcb(aes.Key.AsSpan(0, 16), PaddingMode.None);

            crypto = new Crypto(aes, xor);
            _dict.TryAdd(keytext, crypto);

            return true;
        }

        internal static bool TryGet(string keytext, [MaybeNullWhen(false)] out Crypto crypto)
        {
            return _dict.TryGetValue(keytext, out crypto);
        }

        internal class Crypto : IDisposable
        {
            private readonly Aes _aes;
            private readonly byte[] _xor;

            internal Crypto(Aes aes, byte[] xor)
            {
                _aes = aes;
                _xor = xor;
            }

            public Aes GetAes()
            {
                return _aes;
            }

            public byte[] GetXor()
            {
                return _xor;
            }

            public void Dispose()
            {
                _aes?.Dispose();
            }

            internal void Decrypt(ReadOnlySpan<byte> src, Span<byte> dest)
            {
                int n = src.Length % 16;
                int len = src.Length - n;
                if (len > 0)
                    _aes.DecryptEcb(src[..len], dest[..len], PaddingMode.None);

                if (n == 0)
                    return;

                src = src[len..];
                dest = dest[len..];
                var vectorSrc = Unsafe.ReadUnaligned<Vector128<byte>>(ref MemoryMarshal.GetReference(src));
                var vectorXor = Unsafe.ReadUnaligned<Vector128<byte>>(ref MemoryMarshal.GetArrayDataReference(_xor));
                var vectorDest = vectorSrc ^ vectorXor;
                Unsafe.WriteUnaligned(ref MemoryMarshal.GetReference(dest), vectorDest);
            }

            internal void Encrypt(ReadOnlySpan<byte> src, Span<byte> dest)
            {
                int n = src.Length % 16;
                int len = src.Length - n;
                if (len > 0)
                    _aes.EncryptEcb(src[..len], dest[..len], PaddingMode.None);

                src = src[len..];
                dest = dest[len..];
                var xor = _xor.AsSpan();
                for (int i = 0; i < n; i++)
                {
                    dest[i] = (byte)(src[i] ^ xor[i]);
                }
            }
        }
    }
}
