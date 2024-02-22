using System;
using TalesRunnerFormCryptoClassLibrary;

namespace DllTest2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string str = "8913899DAED9410CA8C3ABB2D16102DD";

            byte[] aes_key = GetKeyClass.AesKey(str);
            byte[] xor_key = GetKeyClass.XorKey(str);

            for (int i = 0; i < aes_key.Length; i++)
            {
                Console.Write("0x" + Convert.ToString(aes_key[i], 16) + ",");
            }
            Console.WriteLine();
            for (int i = 0; i < xor_key.Length; i++)
            {
                Console.Write("0x" + Convert.ToString(xor_key[i], 16) + ",");
            }

            Console.ReadLine();
        }
    }
}
