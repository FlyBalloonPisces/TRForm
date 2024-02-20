// See https://aka.ms/new-console-template for more information
using System.Collections;
using TRToolkit.Package;

Console.WriteLine("Hello, World!");

string str = "8913899DAED9410CA8C3ABB2D16102DD";
//string str = "D55EA427E57F4914A68FF2466B8366C4";
//string str = "FF4080A5FED649978D94A0031EFFCE3D";
//string str = "A73FF15DBE914FCC880037FB32E4D314";
//string str = "4ADEC4FE0A8B4E28964A508B229F341A";
//string str = "420831A87C5F4A1998E8A6146C16586B";
//string str = "BAEEE46EFC2D4E60A08DF99C7ED8B93A";
//string str = "4BA1ACFBCDCC4EF8A6F237B83201F2BE";
//string str = "5375AED5A9564292A6CD6552E37129EB";
//string str = "37720E5F97AF456fADEF817FE7EA14BB";

string[] strs = {
    "8913899DAED9410CA8C3ABB2D16102DD" ,
    "D55EA427E57F4914A68FF2466B8366C4",
    "FF4080A5FED649978D94A0031EFFCE3D",
    "A73FF15DBE914FCC880037FB32E4D314",
    "4ADEC4FE0A8B4E28964A508B229F341A",
    "420831A87C5F4A1998E8A6146C16586B",
    "BAEEE46EFC2D4E60A08DF99C7ED8B93A",
    "4BA1ACFBCDCC4EF8A6F237B83201F2BE",
    "5375AED5A9564292A6CD6552E37129EB",
    "37720E5F97AF456fADEF817FE7EA14BB"
};

AesManager.Crypto crypto;
for (int i = 0; i < strs.Length; i++)
{
    if (AesManager.TryAddOrGetKey(strs[i], out crypto))
    {

        byte[] aes_key = crypto.GetAes().Key;
        //byte[] xor_key = crypto.GetXor();

        //Console.WriteLine(strs[i]);
        Console.WriteLine("new byte[]");
        Console.WriteLine("{");
        for (int j = 0; j < aes_key.Length; j++)
        {
            Console.Write("0x" + Convert.ToString(aes_key[j], 16));
            if (j < aes_key.Length - 1)
            {
                Console.Write(", ");
            }
            if (j % 10 == 9)
            {
                Console.WriteLine();
            }
        }
        Console.WriteLine();
        Console.WriteLine("}");
    }
}

for (int i = 0; i < strs.Length; i++)
{
    if (AesManager.TryAddOrGetKey(strs[i], out crypto))
    {

        //byte[] aes_key = crypto.GetAes().Key;
        byte[] xor_key = crypto.GetXor();

        //Console.WriteLine(strs[i]);
        Console.WriteLine("new byte[]");
        Console.WriteLine("{");
        for (int j = 0; j < xor_key.Length; j++)
        {
            Console.Write("0x" + Convert.ToString(xor_key[j], 16));
            if (j < xor_key.Length - 1)
            {
                Console.Write(", ");
            }
            if (j % 10 == 9)
            {
                Console.WriteLine();
            }
        }
        Console.WriteLine();
        Console.WriteLine("}");
    }
}



//byte[] aesKey_old =
//{
//                0xc, 0xf6, 0xaf, 0xd5, 0x0, 0x48, 0xfe, 0x99, 0xe1, 0xab,
//                0xf9, 0xb6, 0x70, 0x68, 0xad, 0xcd, 0x28, 0x3, 0x8a, 0x6d,
//                0x16, 0x85, 0xe3, 0x7b, 0xeb, 0x9, 0xb, 0x48, 0x4f, 0xb1,
//                0x7e, 0x3
//};

//Console.WriteLine(System.Text.Encoding.Default.GetString(aesKey_old));

Console.ReadLine();