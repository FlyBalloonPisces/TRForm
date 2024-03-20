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

namespace TalesRunnerFormSunnyUI.List
{
    public partial class FList : UIPage
    {
        public FList()
        {
            InitializeComponent();

            TRForm.RegisterFList(this);
        }
    }
}
