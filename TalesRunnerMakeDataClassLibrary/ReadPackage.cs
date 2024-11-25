using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalesRunnerMakeDataClassLibrary
{
    /// <summary>
    /// 负责和解包类对接
    /// </summary>
    internal class ReadPackage
    {
        public string[] TargetPackages { get; set; } //需要读取的pkg文件名

        public string[] TargetTexts { get; set; } //需要读取的文本文件名


        public ReadPackage() { }

        public ReadPackage(string[] targetPackages, string[] targetTexts)
        {
            TargetPackages = targetPackages;
            TargetTexts = targetTexts;
        }

        public void SetTargetPackages(string[] targetPackages)
        { 
            TargetPackages = targetPackages; 
        }

        public void SetTargetTexts(string[] targetTexts)
        {
            TargetTexts = targetTexts; 
        }

        /// <summary>
        /// 我也不晓得这个函数用来干啥
        /// </summary>
        /// <returns></returns>
        public List<string> GetTextsFromMultipleFile()
        {
            List<string> result = new List<string>();

            int i = 0;
            for (i = 0; i < TargetPackages.Length; i++)
            {
                FileInfo fileInfo = new FileInfo(TargetPackages[i]);
                result.AddRange(Unpacker.GetTexts(fileInfo));
            }

            return result;
        }

        public List<string> GetTextsFromSingleFile()
        {
            throw new NotImplementedException();
        }


    }
}
