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

namespace TalesRunnerFormSunnyUI
{
    public partial class FUnbox : UIPage
    {
        public FUnbox()
        {
            InitializeComponent();

            TRForm.RegisterFUnbox(this);
        }
    }
}
