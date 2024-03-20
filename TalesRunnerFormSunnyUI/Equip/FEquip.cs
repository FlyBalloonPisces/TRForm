using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalesRunnerFormSunnyUI;
using TalesRunnerFormSunnyUI.Data;

namespace TalesRunnerFormSunnyUI.Equip
{
    public partial class FEquip : UIPage
    {
        public FEquip()
        {
            InitializeComponent();

            TRForm.RegisterFEquip(this);
        }

        public override void Init()
        {
            base.Init();
            uiFlowLayoutPanel1.Clear();
            index = 0;
            for (int i = 0; i < 12; i++)
            {
                uiButton1_Click(null, null);
            }

            uiPagination1_PageChanged(uiPagination1, null, 1, 12);
        }

        private int index;
        UIFlowItem flowItem;

        private void uiButton1_Click(object sender, System.EventArgs e)
        {
            flowItem = new UIFlowItem();
            flowItem.SetDPIScale();
            flowItem.Text = "Item" + index++.ToString("D2");
            flowItem.Name = flowItem.Text;
            flowItem.Click += Btn_Click;

            //建议用封装的方法Add
            uiFlowLayoutPanel1.Add(flowItem);

            //或者Panel.Controls.Add
            //uiFlowLayoutPanel1.Panel.Controls.Add(btn);

            //不能用原生方法Controls.Add
            //----uiFlowLayoutPanel1.Controls.Add(btn);----

            //uiButton3.Enabled = true;

            this.Render();
        }

        private void Btn_Click(object sender, System.EventArgs e)
        {
            var button = (UIFlowItem)sender;
            ShowInfoTip(button.Text);
        }

        private void uiPagination1_PageChanged(object sender, object pagingSource, int pageIndex, int count)
        {
            //未连接数据库，通过模拟数据来实现
            //一般通过ORM的分页去取数据来填充
            //pageIndex：第几页，和界面对应，从1开始，取数据可能要用pageIndex - 1
            //count：单页数据量，也就是PageSize值

            //根据名称获取
            //var btn = uiFlowLayoutPanel1.Get("Button01");

            //遍历，方法一
            for (int i = 0; i < uiFlowLayoutPanel1.ControlCount; i++)
            {
                //Console.WriteLine(uiFlowLayoutPanel1.Get(i).Name);
                var item = (UIFlowItem)uiFlowLayoutPanel1.Get(i);
                int num = ((pageIndex - 1) * count + i);
                if (num <= ((UIPagination)sender).TotalCount)
                {
                    if (!item.Visible)
                    {
                        item.Show();
                    }
                    string labela = "Item" + num.ToString();
                    string labelb = num.ToString();
                    item.UIFlowItem_ChangeLabel(labela, labelb);
                }
                else
                {
                    if (item.Visible)
                    {
                        item.Hide();
                    }
                }
            }
        }
    }
}
