using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TalesRunnerForm.Properties;

namespace TalesRunnerForm
{
    /// <summary>
    /// 加载进度条窗口
    /// </summary>
    public partial class LoadForm : Form
    {
        #region 委托

        private delegate void DoSomeThing();

        private delegate void DoBackGroundTask(BackgroundWorker bw);
        private DoBackGroundTask _doBackGroundTask;

        #endregion

        #region 窗体初始化
        /// <summary>
        /// 窗体构造函数
        /// </summary>
        public LoadForm()
        {
            InitializeComponent();
            bw.RunWorkerAsync();
            DoSomeThing doSomeThing = TrData.InitData;
            doSomeThing();
        }

        /// <summary>
        /// 窗体初始化
        /// </summary>
        private void Form2_Load(object sender, EventArgs e)
        {
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = graphics.DpiX;
                float dpiY = graphics.DpiY;
                if (dpiX != 100.0 || dpiY != 100.0)
                {
                    Width =
                        (int)(Width * (double)dpiX / 100.0);
                    Height =
                        (int)(Height * (double)dpiY / 100.0);
                    SetControl(this.PBar);
                    SetControl(this.label1);
                }

                void SetControl(Control ctrl) // 控件加载，位置设置
                {
                    Point location = ctrl.Location;
                    ctrl.Location = new Point(
                        (int)(location.X * (double)dpiX / 100.0),
                        (int)(location.Y * (double)dpiX / 100.0));
                    ctrl.Width =
                        (int)(ctrl.Width * (double)dpiX / 100.0);
                    ctrl.Height =
                        (int)(ctrl.Height * (double)dpiX / 100.0);
                }
            }

            DoubleBuffered = true;
        }
        #endregion

        #region 后台任务
        /// <summary>
        /// 后台任务 生成文本
        /// </summary>
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            bw.ReportProgress(0);
            _doBackGroundTask += TrData.LoadItemData;
            _doBackGroundTask(bw);
        }

        /// <summary>
        /// 后台任务完成时执行代码
        /// </summary>
        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GC.Collect(); // 强制进行垃圾回收
            //MainForm form = new MainForm();
            //form.Show();
            this.Dispose();
        }

        /// <summary>
        /// 设置后台任务进度值
        /// </summary>
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.PBar.Value = e.ProgressPercentage;
            this.label1.Text = e.ProgressPercentage + Resources.String_ProgressPercent;
        }

        #endregion

        #region 窗体拖动用
        private Point _mPoint;
        private void Form2_MouseDown(object sender, MouseEventArgs e)
        {
            _mPoint = new Point(e.X, e.Y);
        }

        private void Form2_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - _mPoint.X, this.Location.Y + e.Y - _mPoint.Y);
            }
        }
        #endregion
    }
}
