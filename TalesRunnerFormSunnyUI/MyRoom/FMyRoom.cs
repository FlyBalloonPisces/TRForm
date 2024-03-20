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
using TalesRunnerFormSunnyUI.Data;

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

            TRForm.RegisterFMyRoom(this);
        }

        public FMyRoom(FMain parent)
        {
            InitializeComponent();
            uiButton1.Text = "属性\r\n系统";
            SyncAttrValue();

            TRForm.RegisterFMyRoom(this);

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

        public void SetAttrValue(int value1, int value2, int value3, int value4)
        {
            uiProcessBarMaxSpd.Value = value1;
            uiProcessBarAcc.Value = value2;
            uiProcessBarPow.Value = value3;
            uiProcessBarCtrl.Value = value4;

            SyncAttrValue();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            
            this.parent.JumpPage(1);
        }
    }
}
