using System;
using System.IO;

namespace TRUnpack
{
    class Program
    {
        static void Main(string[] args)
        {
            PkgUnpack.InitDevIl();
            int i = 0;
            for (i = 0; i < args.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(args[i]);
                PkgUnpack.Unpack(fileInfo);
            }
            //FileInfo fileInfo = new FileInfo("C:\\Games\\TalesRunner\\tr13.pkg");
            //PkgUnpack.Unpack(fileInfo);

            Console.WriteLine("Unpack done");
        }
    }
}
