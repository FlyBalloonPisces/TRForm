using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalesRunnerFormCryptoClassLibrary;
using TRCryptoClassLibrary;

namespace DllTest2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string str = "8913899DAED9410CA8C3ABB2D16102DD";

            byte[] aes_key = TRCryptoClassLibrary.GetKeyClass.AesKey(str);
            byte[] xor_key = TRCryptoClassLibrary.GetKeyClass.XorKey(str);

            for (int i = 0; i < aes_key.Length; i++)
            {
                Console.Write("0x" + Convert.ToString(aes_key[i], 16) + ",");
            }
            Console.WriteLine();
            for (int i = 0; i < xor_key.Length; i++)
            {
                Console.Write("0x" + Convert.ToString(xor_key[i], 16) + ",");
            }

            aes_key = TalesRunnerFormCryptoClassLibrary.GetKeyClass.AesKey(str);
            xor_key = TalesRunnerFormCryptoClassLibrary.GetKeyClass.XorKey(str);

            for (int i = 0; i < aes_key.Length; i++)
            {
                Console.Write("0x" + Convert.ToString(aes_key[i], 16) + ",");
            }
            Console.WriteLine();
            for (int i = 0; i < xor_key.Length; i++)
            {
                Console.Write("0x" + Convert.ToString(xor_key[i], 16) + ",");
            }


        }
    }
}
