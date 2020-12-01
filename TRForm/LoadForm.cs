using ControlEx;

namespace TalesRunnerForm
{
    partial class LoadForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadForm));
            this.bw = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.PBar = new ControlEx.ProgressBarEx();
            this.SuspendLayout();
            // 
            // bw
            // 
            this.bw.WorkerReportsProgress = true;
            this.bw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bw_DoWork);
            this.bw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bw_ProgressChanged);
            this.bw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bw_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(232, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "000 %";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Highlight;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 27);
            this.label2.TabIndex = 2;
            this.label2.Text = "Loading...";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseDown);
            this.label2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseMove);
            // 
            // PBar
            // 
            this.PBar.ImageBorder = null;
            this.PBar.ImageEmpty = ((System.Drawing.Image)(resources.GetObject("PBar.ImageEmpty")));
            this.PBar.ImageFill = ((System.Drawing.Image)(resources.GetObject("PBar.ImageFill")));
            this.PBar.ImageLeft = global::TalesRunnerForm.Properties.Resources._1024_gameu_l;
            this.PBar.ImageRight = global::TalesRunnerForm.Properties.Resources._1024_gameu_r;
            this.PBar.ImageTag = null;
            this.PBar.IsDrag = true;
            this.PBar.LoadLength = 119;
            this.PBar.Location = new System.Drawing.Point(13, 31);
            this.PBar.Name = "PBar";
            this.PBar.Size = new System.Drawing.Size(268, 41);
            this.PBar.TabIndex = 3;
            this.PBar.Value = 50;
            this.PBar.ZoomRate = true;
            // 
            // LoadForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(293, 109);
            this.ControlBox = false;
            this.Controls.Add(this.PBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Loading...";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker bw;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ProgressBarEx PBar;
    }
}