namespace TalesRunnerFormSunnyUI
{
    partial class FMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("装备");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("属性");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("列表");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("开箱");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("兑换");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("设置");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FMain));
            this.StyleManager = new Sunny.UI.UIStyleManager(this.components);
            this.uiSplitContainer1 = new Sunny.UI.UISplitContainer();
            this.uiTabControl1 = new Sunny.UI.UITabControl();
            this.uiNavBar1 = new Sunny.UI.UINavBar();
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainer1)).BeginInit();
            this.uiSplitContainer1.Panel2.SuspendLayout();
            this.uiSplitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // StyleManager
            // 
            this.StyleManager.DPIScale = true;
            this.StyleManager.GlobalFont = true;
            this.StyleManager.GlobalFontName = "阿里巴巴普惠体";
            this.StyleManager.GlobalRectangle = true;
            // 
            // uiSplitContainer1
            // 
            this.uiSplitContainer1.CollapsePanel = Sunny.UI.UISplitContainer.UICollapsePanel.None;
            this.uiSplitContainer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.uiSplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSplitContainer1.IsSplitterFixed = true;
            this.uiSplitContainer1.Location = new System.Drawing.Point(0, 35);
            this.uiSplitContainer1.MinimumSize = new System.Drawing.Size(20, 20);
            this.uiSplitContainer1.Name = "uiSplitContainer1";
            // 
            // uiSplitContainer1.Panel2
            // 
            this.uiSplitContainer1.Panel2.Controls.Add(this.uiTabControl1);
            this.uiSplitContainer1.Panel2.Controls.Add(this.uiNavBar1);
            this.uiSplitContainer1.Size = new System.Drawing.Size(1400, 733);
            this.uiSplitContainer1.SplitterDistance = 290;
            this.uiSplitContainer1.SplitterWidth = 10;
            this.uiSplitContainer1.TabIndex = 0;
            // 
            // uiTabControl1
            // 
            this.uiTabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.uiTabControl1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiTabControl1.Frame = this;
            this.uiTabControl1.ItemSize = new System.Drawing.Size(0, 1);
            this.uiTabControl1.Location = new System.Drawing.Point(0, 50);
            this.uiTabControl1.MainPage = "";
            this.uiTabControl1.Name = "uiTabControl1";
            this.uiTabControl1.SelectedIndex = 0;
            this.uiTabControl1.Size = new System.Drawing.Size(1100, 683);
            this.uiTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.uiTabControl1.TabIndex = 1;
            this.uiTabControl1.TabVisible = false;
            this.uiTabControl1.TipsFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            // 
            // uiNavBar1
            // 
            this.uiNavBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.uiNavBar1.DropMenuFont = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiNavBar1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.uiNavBar1.Location = new System.Drawing.Point(0, 0);
            this.uiNavBar1.Name = "uiNavBar1";
            treeNode1.Name = "FEquip";
            treeNode1.Text = "装备";
            treeNode2.Name = "FStat";
            treeNode2.Text = "属性";
            treeNode3.Name = "FList";
            treeNode3.Text = "列表";
            treeNode4.Name = "FUnbox";
            treeNode4.Text = "开箱";
            treeNode5.Name = "FExcahnge";
            treeNode5.Text = "兑换";
            treeNode6.Name = "FAbout";
            treeNode6.Text = "设置";
            this.uiNavBar1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6});
            this.uiNavBar1.Size = new System.Drawing.Size(1100, 50);
            this.uiNavBar1.TabControl = this.uiTabControl1;
            this.uiNavBar1.TabIndex = 0;
            this.uiNavBar1.Text = "uiNavBar1";
            this.uiNavBar1.MenuItemClick += new Sunny.UI.UINavBar.OnMenuItemClick(this.uiNavBar1_MenuItemClick);
            // 
            // FMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1400, 768);
            this.Controls.Add(this.uiSplitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainTabControl = this.uiTabControl1;
            this.Name = "FMain";
            this.ShowTitleIcon = true;
            this.Text = "ItemStatus";
            this.ZoomScaleRect = new System.Drawing.Rectangle(19, 19, 800, 450);
            this.uiSplitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiSplitContainer1)).EndInit();
            this.uiSplitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Sunny.UI.UIStyleManager StyleManager;
        private Sunny.UI.UISplitContainer uiSplitContainer1;
        private Sunny.UI.UITabControl uiTabControl1;
        private Sunny.UI.UINavBar uiNavBar1;
    }
}

