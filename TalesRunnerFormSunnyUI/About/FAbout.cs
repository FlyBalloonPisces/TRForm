using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalesRunnerFormSunnyUI.Data;

namespace TalesRunnerFormSunnyUI.About
{
    public partial class FAbout : UIPage
    {
        public FAbout()
        {
            InitializeComponent();
        }

        private void uiSymbolButtonReboot1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Restart();

        }

        private void uiSymbolButtonReboot2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Restart();

        }

        private void uiSymbolButtonReboot3_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Restart();


        }

        private void uiSymbolButtonReboot4_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Restart();

        }

        private void uiSymbolButtonRefresh1_Click(object sender, EventArgs e)
        {
            int keyNum = -1;
            TRData.ServerInfo serverInfo = new TRData.ServerInfo();
            serverInfo.ServerNum = 0;
            serverInfo.KeyNum = keyNum;
            serverInfo.Path = uiTextBoxDir1.Text;
            if (uiTextBoxKey1.Text.Length == 0)
            {
                if(TRData.TestScriptKeys(serverInfo))
                {
                    UIMessageTip.ShowOk();
                }
                else
                {
                    UIMessageTip.ShowError();
                }
            }
            else
            {
                serverInfo.Key = uiTextBoxKey1.Text;
                if (TRData.TestScriptKey(serverInfo))
                {
                    UIMessageTip.ShowOk();
                }
                else
                {
                    UIMessageTip.ShowError();
                }
            }

           

        }

        private void uiSymbolButtonDir1_Click(object sender, EventArgs e)
        {
            string dir = "";
            if (DirEx.SelectDirEx("扩展打开文件夹", ref dir))
            {
                uiTextBoxDir1.Text = dir;
            }
        }

        private void uiSymbolButtonDir2_Click(object sender, EventArgs e)
        {
            string dir = "";
            if (DirEx.SelectDirEx("扩展打开文件夹", ref dir))
            {
                uiTextBoxDir2.Text = dir;
            }
        }

        private void uiSymbolButtonDir3_Click(object sender, EventArgs e)
        {
            string dir = "";
            if (DirEx.SelectDirEx("扩展打开文件夹", ref dir))
            {
                uiTextBoxDir3.Text = dir;
            }
        }

        private void uiSymbolButtonDir4_Click(object sender, EventArgs e)
        {
            string dir = "";
            if (DirEx.SelectDirEx("扩展打开文件夹", ref dir))
            {
                uiTextBoxDir4.Text = dir;
            }
        }
    }
}
