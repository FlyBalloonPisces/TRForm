using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalesRunnerFormSunnyUI.Data;

namespace TalesRunnerFormSunnyUI.Stat
{
    public partial class FStat : UIPage
    {
        public FStat()
        {
            InitializeComponent();
            //uiIntegerUpDown31.Value = 3;
            //uiIntegerUpDown32.Value = 3;
            //uiIntegerUpDown33.Value = 3;
            //uiIntegerUpDown34.Value = 3;

            StatusBasicChanged();
            StatusValueChanged();

            TRForm.RegisterFStat(this);
        }

        private void uiIntegerUpDown31_ValueChanged(object sender, int value)
        {
            StatusBasicRange();
            StatusBasicChanged();
        }

        private void uiIntegerUpDown32_ValueChanged(object sender, int value)
        {
            StatusBasicRange();
            StatusBasicChanged();
        }

        private void uiIntegerUpDown33_ValueChanged(object sender, int value)
        {
            StatusBasicRange();
            StatusBasicChanged();
        }

        private void uiIntegerUpDown34_ValueChanged(object sender, int value)
        {
            StatusBasicRange();
            StatusBasicChanged();
        }

        private bool StatusBasicRange()
        {
            int sum = uiIntegerUpDown31.Value + uiIntegerUpDown32.Value + uiIntegerUpDown33.Value + uiIntegerUpDown34.Value;
            if (sum == 12)
            {
                uiIntegerUpDown31.Minimum = uiIntegerUpDown31.Value;
                uiIntegerUpDown32.Minimum = uiIntegerUpDown32.Value;
                uiIntegerUpDown33.Minimum = uiIntegerUpDown33.Value;
                uiIntegerUpDown34.Minimum = uiIntegerUpDown34.Value;
                return false;
            } 
            else if (sum == 15)
            {
                uiIntegerUpDown31.Maximum = uiIntegerUpDown31.Value;
                uiIntegerUpDown32.Maximum = uiIntegerUpDown32.Value;
                uiIntegerUpDown33.Maximum = uiIntegerUpDown33.Value;
                uiIntegerUpDown34.Maximum = uiIntegerUpDown34.Value;
                return false;
            }
            else
            {
                uiIntegerUpDown31.Minimum = 1;
                uiIntegerUpDown32.Minimum = 1;
                uiIntegerUpDown33.Minimum = 1;
                uiIntegerUpDown34.Minimum = 1;
                uiIntegerUpDown31.Maximum = 6;
                uiIntegerUpDown32.Maximum = 6;
                uiIntegerUpDown33.Maximum = 6;
                uiIntegerUpDown34.Maximum = 6;
            }
                
            return true;
        }

        private void StatusBasicChanged()
        {
            int sum = uiIntegerUpDown31.Value + uiIntegerUpDown32.Value + uiIntegerUpDown33.Value + uiIntegerUpDown34.Value;
            uiLabel31.Text = "属性总和：" + sum;
            //TRForm.StatStatusBasicChanged(uiIntegerUpDown31.Value, uiIntegerUpDown32.Value, uiIntegerUpDown33.Value, uiIntegerUpDown34.Value);
            TRData.StatStatusBasicChanged(uiIntegerUpDown31.Value, uiIntegerUpDown32.Value, uiIntegerUpDown33.Value, uiIntegerUpDown34.Value);
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
