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
                PkgUnpack.UnpackForDemo(fileInfo);
            }
            //FileInfo fileInfo = new FileInfo("E:\\TRKR\\tr4.pkg");
            //PkgUnpack.Unpack(fileInfo);

            Console.WriteLine("Unpack done");
        }
    }
}
