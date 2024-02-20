// See https://aka.ms/new-console-template for more information
using TestApp;
using TRTextProcessingClassLibrary.Item;



Console.WriteLine("Hello, World!");

String path = @"E:\TRKR";

TestClass testClass = new TestClass();
//testClass.test(path);
StreamWriter exportFile = File.AppendText(path + "\\_test\\" + "test.txt");


//获得pkg和文件夹对应表
SortedList<string, string> folder_dict = testClass.testFolder(path);
exportFile = File.AppendText(path + "\\_test\\" + "testFolder.txt");
foreach (var item in folder_dict)
{
    exportFile.Write(item.Key + "," + item.Value);
    exportFile.WriteLine();
    Console.WriteLine(item.Key + "," + item.Value);
}
exportFile.Close();

#region 角色功能
//获得角色pkg对应表,key = charNum， value = pkgName+ca3Name
SortedList<int, string> dict = testClass.testCharacter(path);
exportFile = File.AppendText(path + "\\_test\\" + "testCharacter.txt");
foreach (var item in dict)
{
    exportFile.Write(item.Key + "," + item.Value);
    exportFile.WriteLine();
    Console.WriteLine(item.Key + "," + item.Value);
}
exportFile.Close();

//获得角色名称，道具编号，角色编号
SortedList<uint, TblAvatarItemDescClass> avatarItemDescList = testClass.testItemChar(path);
exportFile = File.AppendText(path + "\\_test\\" + "testItemChar.txt");
foreach (var item in avatarItemDescList)
{
    exportFile.Write(item.Key + "," + item.Value.fdCharacter + "," + item.Value.fdItemName);
    exportFile.WriteLine();
    Console.WriteLine(item.Key + "," + item.Value.fdCharacter + "," + item.Value.fdItemName);
}
exportFile.Close();

//获得角色占用
SortedList<string, long[]> result_list = testClass.testOccupation(path);
exportFile = File.AppendText(path + "\\_test\\" + "testOccupation.txt");
foreach (var item in result_list)
{
    exportFile.Write("key = " + item.Key + ", chars = " + item.Value[35] + ", ");
    for (int i = 0; i < item.Value.Length - 1; i++)
    {
        exportFile.Write("char" + (i + 1) + "oc = " + item.Value[i]);
        if (i < item.Value.Length - 2)
        {
            exportFile.Write(", " );
        }
    }
    exportFile.WriteLine();
}
#endregion



//第一种方法
//string[] files = Directory.GetFiles(path, "*.pkg");

//foreach (var file in files)
//{
//    Console.WriteLine(file);
//}

//第二种方法
//DirectoryInfo folder = new DirectoryInfo(path);

//foreach (FileInfo file in folder.GetFiles("*.txt"))
//{
//    Console.WriteLine(file.FullName);
//}
