using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMapAnalyser
{
    internal class Program
    {
        static void Main(string[] args)
        {
            String folder = @"E:\GlobalTR";
            string[] files = Directory.GetFiles(folder, "map*.pkg");
            List<string> attrs = PkgUnpack.GetMapAttrs(files);
            attrs.Add("pkgname");
            attrs.Add("infoname");
            StreamWriter exportFile = File.AppendText(folder + "\\" + "mapAttr.txt");
            for (int i = 0; i < attrs.Count; i++)
            {
                exportFile.Write(attrs[i]);
                exportFile.WriteLine();
            }
            exportFile.Close();

            SortedList<int, string[]> list = PkgUnpack.GetMapAttrValues(files, attrs);
            exportFile = File.AppendText(folder + "\\" + "mapAttrValue.txt");
            for (int i = 0; i < attrs.Count; i++)
            {
                exportFile.Write(attrs[i]);
                if (i < attrs.Count - 1)
                {
                    exportFile.Write("|");
                }
            }
            exportFile.WriteLine();

            foreach (var item in list)
            {
                for (int i = 0; i < item.Value.Length; i++)
                {
                    exportFile.Write(item.Value[i]);
                    if (i < item.Value.Length - 1)
                    {
                        exportFile.Write("|");
                    }
                }
                exportFile.WriteLine();
            }
            exportFile.Close();


            Console.WriteLine("Unpack done");
        }
    }
}
