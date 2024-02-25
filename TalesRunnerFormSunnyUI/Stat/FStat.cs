using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TalesRunnerFormSunnyUI.Stat
{
    public partial class FStat : UIPage
    {
        public FStat()
        {
            InitializeComponent();
            StatusBasicChanged();
            StatusValueChanged();
        }

        private void uiIntegerUpDown31_ValueChanged(object sender, int value)
        {
            StatusBasicChanged();
        }

        private void uiIntegerUpDown32_ValueChanged(object sender, int value)
        {
            StatusBasicChanged();
        }

        private void uiIntegerUpDown33_ValueChanged(object sender, int value)
        {
            StatusBasicChanged();
        }

        private void uiIntegerUpDown34_ValueChanged(object sender, int value)
        {
            StatusBasicChanged();
        }

        private void StatusBasicChanged()
        {
            int sum = uiIntegerUpDown31.Value + uiIntegerUpDown32.Value + uiIntegerUpDown33.Value + uiIntegerUpDown34.Value;
            this.uiLabel31.Text = "属性总和：" + sum;
        }

        private void StatusValueChanged()
        {
            // TODO:按照权重计算
            int sum1 = 0;
            int sum2 = 0;
            int sum3 = 0;
            int sum = sum1 + sum2 + sum3;
            this.uiLabel32.Text = "剩余属性点：" + (31 - sum);
        }
    }
}
