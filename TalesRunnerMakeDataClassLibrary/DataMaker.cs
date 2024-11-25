using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalesRunnerMakeDataClassLibrary
{
    public class DataMaker
    {
        public string GamePath { get; set; } //游戏文件夹路径
        //public string GamePath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        public DataMaker() { }

        public DataMaker(string gamePath)
        {
            GamePath = gamePath;
        }

        public void MakeData()
        {

            return;
        }
    }
}
