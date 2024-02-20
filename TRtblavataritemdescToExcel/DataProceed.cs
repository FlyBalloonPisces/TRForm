using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRtblavataritemdescToExcel
{
    static class DataProceed
    {
        public static void TxtSlice(string txtPath)
        {
            string Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string[] temp = txtPath.Split('\\');
            string TxtName = temp[temp.Length - 1].Split('.')[0];
            string xlsPath = Path + TxtName + ".xlsx";
            //FileStream fileTxt = File.Open(filePath, FileMode.Open);
            
            FileStream file = new FileStream(xlsPath, FileMode.OpenOrCreate, FileAccess.Write); ;
            ISheet sheet;
            IWorkbook workbook = new XSSFWorkbook();
            sheet = workbook.CreateSheet(TxtName);

            string[] strs = File.ReadAllLines(txtPath);

            for (int i = 0; i < strs.Length; i++)
            {
                sheet.CreateRow(i); //创建行
                string[] strsAfter = StringDivide(strs[i]);
                for (int j = 0; j < strsAfter.Length; j++)
                {
                    sheet.GetRow(i).CreateCell(j);//创建单元格
                    sheet.GetRow(i).GetCell(j).SetCellValue(strsAfter[j]);
                    Console.WriteLine((i) + "行" + j + "列 = " + strsAfter[j]);
                }
            }
            workbook.Write(file);
            file.Close();
            workbook.Close();
        }

        /// <summary>
        /// 根据送入数据进行切割 可以切割itemdesc中道具说明内仍有引号的情况（最多2个引号） 有可能要修改
        /// </summary>
        /// <param name="sss"></param>
        /// <returns>字符串数组</returns>
        private static string[] StringDivide(string sss)
        {
            // 双引号开始标记
            int qutationStart = 0;
            int qutationEnd = 0;
            char[] charStr = sss.ToCharArray();
            // 用于拼接字符 作为一个字段值
            StringBuilder stb = new StringBuilder();
            // 结果list
            List<string> list = new List<string>();
            int lastindex = sss.LastIndexOf('\"');
            // 逐个字符处理
            for (int i = 0; i < charStr.Length; i++)
            {
                // 在此之前还未遇到双引号并且当前的字符为"
                if (qutationStart == 0 && charStr[i] == '\"')
                {
                    qutationStart = 1;
                    continue;
                }

                if (qutationStart == 1 && charStr[i] == '\"')
                {
                    // 在此之前遇到了双引号并且当前的字符为" 说明字段拼接可能要结束了
                    qutationEnd = 1;
                    // 当最后一个字符是双引号时，由于下次循环不会执行，所以在此保存一下
                    if (i == charStr.Length - 1 && stb.Length != 0)
                    {
                        list.Add(stb.ToString());
                        stb.Clear();
                    }
                    continue;
                }

                if (qutationEnd == 1 && charStr[i] != ',')
                {
                    // 在此之前遇到了第二个双引号并且当前的字符不为,说明字段拼接没有结束，是双重引号
                    qutationEnd = 0;
                    stb.Append('\"');
                    stb.Append(charStr[i]);

                    continue;
                }

                if (qutationEnd == 1 && charStr[i] == ',')
                {
                    // 在此之前遇到了第二个双引号并且当前的字符为,说明字段拼接该结束了
                    qutationStart = 0;
                    qutationEnd = 0;

                    list.Add(stb.ToString());
                    stb.Clear();

                    continue;
                }

                if (qutationStart == 1 && charStr[i] == ',')
                {
                    // 处理 \"中国,北京\"这种不规范的字符串
                    stb.Append(charStr[i]);
                    continue;
                }

                if (i == lastindex)
                {
                    continue;
                }

                if (charStr[i] == ',')
                {
                    // 字段结束，将拼接好的字段值存到list中
                    list.Add(stb.ToString());
                    stb.Clear();
                    continue;
                }

                // 不属于分隔符的就拼接
                stb.Append(charStr[i]);

                if (i == charStr.Length - 1 && stb.Length != 0)
                {
                    list.Add(stb.ToString());
                    stb.Clear();
                }
            }

            return list.ToArray();
        }

    }
}
