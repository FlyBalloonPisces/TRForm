using System;
using System.Collections.Generic;
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
        public string GamePath { get; set; } //游戏文件夹路径

        public ReadPackage() { }

        public ReadPackage(string gamePath)
        {  GamePath = gamePath; }


    }
}
