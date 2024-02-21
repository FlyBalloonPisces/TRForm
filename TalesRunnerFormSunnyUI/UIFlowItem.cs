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

namespace TalesRunnerFormSunnyUI
{
    public partial class UIFlowItem : UIUserControl
    {
        public UIFlowItem()
        {
            InitializeComponent();
        }

        private void UIFlowItem_Paint(object sender, PaintEventArgs e)
        {
            this.uiLabel1.Font = Font;
            this.uiLabel2.Font = Font;
            this.uiButton1.Font = Font;
            this.uiButton2.Font = Font;
            this.uiButton3.Font = Font;
            //e.Graphics.FillEllipse(Color.Lime, new Rectangle(10, 10, 20, 20));
            //e.Graphics.DrawString(Text, Font, ForeColor, new Rectangle(35, 0, Width, 40), ContentAlignment.MiddleLeft);
            //e.Graphics.DrawLine(ForeColor, 10, 40, Width - 20, 40);
            //e.Graphics.DrawString(Text, Font, ForeColor, new Rectangle(10, 40, Width, Height - 40), ContentAlignment.MiddleLeft);
            //e.Graphics.DrawString("ItemNum", Font, ForeColor, new Rectangle(10, 80, Width, Height - 80), ContentAlignment.MiddleLeft);
        }

        public void UIFlowItem_ChangeLabel(string labela, string labelb)
        {
            this.uiLabel1.Text = labela;
            this.uiLabel2.Text = labelb;
        }
    }
}
