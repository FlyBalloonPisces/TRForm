//#define logic

using Sunny.UI;
using TalesRunnerFormSunnyUI.About;
using TalesRunnerFormSunnyUI.Equip;
using TalesRunnerFormSunnyUI.List;
using TalesRunnerFormSunnyUI.Data;
using TalesRunnerFormSunnyUI.Exchange;
using TalesRunnerFormSunnyUI.MyRoom;
using TalesRunnerFormSunnyUI.Stat;
using System;

namespace TalesRunnerFormSunnyUI
{
    public partial class FMain : UIForm
    {
        public FMain()
        {
            InitializeComponent();

            FMyRoom fMyRoom = new FMyRoom(this);
            fMyRoom.TopLevel = false;
            fMyRoom.Name = "fMyRoom";
            uiSplitContainer1.Panel1.Controls.Add(fMyRoom);
            fMyRoom.Show();

            uiNavBar1.TabControl = this.uiTabControl1;

            //增加页面到Main
            AddPage(new FEquip(), 1000);
            AddPage(new FStat(), 2000);
            AddPage(new FList(), 3000);
            AddPage(new FUnbox(), 4000);
            AddPage(new FExchange(), 5000);
            AddPage(new FAbout(), 6000);

            //设置Header节点索引
            uiNavBar1.SetNodePageIndex(uiNavBar1.Nodes[0], 1000);
            uiNavBar1.SetNodePageIndex(uiNavBar1.Nodes[1], 2000);
            uiNavBar1.SetNodePageIndex(uiNavBar1.Nodes[2], 3000);
            uiNavBar1.SetNodePageIndex(uiNavBar1.Nodes[3], 4000);
            uiNavBar1.SetNodePageIndex(uiNavBar1.Nodes[4], 5000);
            uiNavBar1.SetNodePageIndex(uiNavBar1.Nodes[5], 6000);

            //显示默认界面
            uiNavBar1.SelectedIndex = 5;

            timer1.Start();

            uiPanel2.Text = "小工具版本：0.0.0.1";
            uiPanel4.Text = "服务器版本：0.0.0.2";
            



            TRForm.RegisterFMain(this);
            //TRForm.RegisterFMyRoom(fMyRoom);

            test();
#if logic
            TRData.Init();
#endif
        }

        private void uiNavBar1_MenuItemClick(string itemText, int menuIndex, int pageIndex)
        {
            this.ShowProcessForm();
            SelectPage(pageIndex);
            this.HideProcessForm();
        }

        private void test()
        {
            var myroom = uiSplitContainer1.Panel1.Controls["fMyRoom"] as FMyRoom;
            myroom.test();
        }

        public void JumpPage(int menuIndex)
        {
            uiNavBar1.SelectedIndex = menuIndex;
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            uiPanel3.Text = DateTime.Now.DateTimeString();
        }
    }
}
