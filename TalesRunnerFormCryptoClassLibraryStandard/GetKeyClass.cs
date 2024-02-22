namespace TalesRunnerFormCryptoClassLibrary
{
    public class GetKeyClass
    {
        //public GetKeyClass() { 
        //CryptClass2.Equals
        //}
        /// <summary>
        /// 返回key字符串生成的aes字节数组
        /// </summary>
        /// <param name="key">key的字符串</param>
        /// <returns>生成的aes字节数组</returns>
        public static byte[] AesKey(string key)
        {
            if (key == null)
            {
                return null;
            }
            else
            {
                AesManager.Crypto crypto;
                if (AesManager.TryAddOrGetKey(key, out crypto))
                {

                    byte[] aes_key = crypto.GetAes();
                    return aes_key;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 返回key字符串生成的aes字节数组
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>生成的xor字节数组</returns>
        public static byte[] XorKey(string key)
        {
            if (key == null)
            {
                return null;
            }
            else
            {
                AesManager.Crypto crypto;
                if (AesManager.TryAddOrGetKey(key, out crypto))
                {

                    byte[] xor_key = crypto.GetXor();
                    return xor_key;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}