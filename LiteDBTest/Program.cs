using LiteDB;
using System;
using System.IO;
using LiteDBTest.Item;

namespace LiteDBTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileInfo fileInfo = new FileInfo(@"G:\TalesrunnerKR\tr4.pkg");
            var dataStrings = Unpack.UnpackForSpecificFile(fileInfo, "tblavataritemdesc.txt");


            using (var db = new LiteDatabase(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + @"MyData.db"))
            {
                var col = db.GetCollection<TblAvatarItemDesc>("tblAvatarItemDescClass");

                var num = 0;
                for (int i = 2; i < dataStrings.Length; i++)
                {

                    var temp = new TblAvatarItemDesc(dataStrings[i]);
                    // 插入新顾客文档 (Id 自增)
                    col.Insert(temp);
                    Console.WriteLine(i);
                }

                
            }
        }
    }
}
