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
using TalesRunnerFormSunnyUI;

namespace TalesRunnerFormSunnyUI.MyRoom
{
    public partial class FMyRoom : UIPage
    {
        private FMain parent;

        public FMyRoom()
        {
            InitializeComponent();
            uiButton1.Text = "属性\r\n系统";
            SyncAttrValue();
        }

        public FMyRoom(FMain parent)
        {
            InitializeComponent();
            uiButton1.Text = "属性\r\n系统";
            SyncAttrValue();

            this.parent = parent;
        }

        private void SyncAttrValue()
        {
            uiLabelMaxSpdValue.Text = uiProcessBarMaxSpd.Value.ToString();
            uiLabelAccValue.Text = uiProcessBarAcc.Value.ToString();
            uiLabelPowValue.Text = uiProcessBarPow.Value.ToString();
            uiLabelCtrlValue.Text = uiProcessBarCtrl.Value.ToString();
        }

        public void test()
        {
            uiProcessBarMaxSpd.Value = 12;
            uiProcessBarAcc.Value = 12;
            uiProcessBarPow.Value = 12; 
            uiProcessBarCtrl.Value = 12;

            SyncAttrValue();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            
            this.parent.JumpPage(1);
        }
    }
}
