using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using TalesRunnerForm.Properties;
using Color = System.Drawing.Color;
using Image = System.Drawing.Image;

namespace TalesRunnerForm
{
    /// <summary>
    /// 道具属性工具窗口
    /// </summary>
    public partial class MainForm : Form
    {
        #region 初始数据
        // 用于显示符合条件道具的列表
        private List<ListViewItem> _listViewItems = new List<ListViewItem>();

        // 确定程序路径 进行文本读取
        public static readonly string Path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        private static Point _mPoint;

        // 防止列表多选的变量
        private int _iOld = -1;

        // 开箱页每页16个箱子
        private const int PerPage = 30;
        #endregion

        #region 委托
        private delegate int SetInt(int t);
        private SetInt _setInt;

        private delegate int SetInt2(int t, int u);
        private SetInt2 _setInt2;

        private delegate void SetIntVoid(int t);
        private SetIntVoid _setIntVoid;

        private delegate void SetBooleanVoid(bool t);
        private SetBooleanVoid _setBooleanVoid;

        private delegate void SetInt2Void(int t, int u);
        private SetInt2Void _setInt2Void;

        private delegate void SetItemChanged(int t, int u);
        private SetItemChanged _setItemChanged;

        private delegate void SetString(string s);
        private SetString _setString;

        private delegate void DoSomeThing();
        private DoSomeThing _doSomeThing;

        private delegate int GetTagByNum(int t);
        private GetTagByNum _getTagByNum;

        private delegate bool GetBoolean();
        private GetBoolean _getBoolean;

        private delegate bool GetBooleanByTag(int t);
        private GetBooleanByTag _getBooleanByTag;

        private delegate string GetString();
        private GetString _getString;

        private delegate int GetIntByString(string s);
        private GetIntByString _getIntByString;

        private delegate string[] GetStringsByTag(int t);
        private GetStringsByTag _getStringsByTag;

        private delegate Image[] GetImagesByTag(int t);
        private GetImagesByTag _getImagesByTag;

        private delegate string[] GetStrings();
        private GetStrings _getStrings;

        private delegate string[] GetStringsByInt(int i);
        private GetStringsByInt _getStringsByInt;

        private delegate Image ShowDds(long offset);
        private ShowDds _showDds;

        private delegate List<ListViewItem> GetListViewItems();
        private GetListViewItems _getListViewItems;

        private delegate void GetFind(int[] indexInts, float[] indexFloats, bool[] indexBools);
        private GetFind _getFind;

        private delegate int GetNumFind(bool bools, int numItem);
        private GetNumFind _getNumFind;
        #endregion

        #region 窗体初始化
        /// <summary>
        /// 窗体构造函数
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 窗口关闭时退出程序
        /// </summary>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// 窗体加载
        /// 加载进度条窗口，调整控件大小，读取数据，设置语言，设置列表视图
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            _getBoolean = TrData.TestItemData;
            if (!_getBoolean())
            {
                Environment.Exit(0);
            }
            LoadForm form = new LoadForm();
            form.ShowDialog();
            tabControl1.SetStyle(2);
            tabControl2.SetStyle(1);
            tabControl3.SetStyle(4);
            // 根据DPI调整控件大小
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
                    columnHeader1.Width =
                        (int)(columnHeader1.Width *
                            (double)dpiX / 100.0);
                    columnHeader2.Width =
                        (int)(columnHeader2.Width *
                            (double)dpiX / 100.0);
                    foreach (Control control1 in (ArrangedElementCollection)Controls) // 页面1控件加载
                    {
                        if (control1 is Button)
                        {
                            control1.Width = (int)(control1.Width * (double)dpiX / 100.0);
                            control1.Height = (int)(control1.Height * (double)dpiX / 100.0);
                            control1.Location = new Point(Width - control1.Width, 0);
                        }
                        else
                        {
                            SetControl(control1);
                        }
                    }
                    foreach (Control control1 in (ArrangedElementCollection)tabPage1_1.Controls) // 页面1控件加载
                    {
                        SetControl(control1);
                        if (control1 is TabControl)
                        {
                            foreach (Control control2 in (ArrangedElementCollection)control1
                                .Controls)
                            {
                                if (control2 is TabPage)
                                {
                                    foreach (Control control3 in
                                        (ArrangedElementCollection)control2.Controls)
                                    {
                                        SetControl(control3);
                                    }
                                }
                                else
                                {
                                    SetControl(control2);
                                }
                            }
                        }
                    }

                    foreach (Control control1 in (ArrangedElementCollection)tabPage1_2.Controls)
                    {
                        SetControl(control1);
                        if (control1 is TabControl)
                        {
                            foreach (Control control2 in (ArrangedElementCollection)control1.Controls)
                            {
                                if (control2 is TabPage)
                                {
                                    foreach (Control control3 in (ArrangedElementCollection)control2.Controls)
                                    {
                                        SetControl(control3);
                                        if (control3 is GroupBox)
                                        {
                                            foreach (Control control4 in (ArrangedElementCollection)control3.Controls)
                                            {
                                                SetControl(control4);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    SetControl(control2);
                                }
                            }
                        }
                    }

                    foreach (Control control1 in (ArrangedElementCollection)tabPage1_5.Controls)
                    {
                        SetControl(control1);
                        if (control1 is TabControl)
                        {
                            foreach (Control control2 in (ArrangedElementCollection)control1.Controls)
                            {
                                if (control2 is TabPage)
                                {
                                    foreach (Control control3 in (ArrangedElementCollection)control2.Controls)
                                    {
                                        SetControl(control3);
                                        if (control3 is GroupBox)
                                        {
                                            foreach (Control control4 in (ArrangedElementCollection)control3
                                                .Controls)
                                            {
                                                SetControl(control4);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    SetControl(control2);
                                }
                            }
                        }
                    }
                    foreach (Control control1 in (ArrangedElementCollection)tabPage1_6.Controls)
                    {
                        SetControl(control1);
                    }
                    SetControl(textBox12);
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
            _showDds = TrData.ShowDds;
            _setItemChanged = TrData.ItemChanged;
            _getBoolean = TrData.GetData;
            if (!_getBoolean())
            {
                tabPage1_5.Parent = null;
            }

            _getString = TrData.GetAttrListText;
            textBox7.Text = _getString();

            _getString = TrData.GetDataVersion;
            textBox12.Text =
                Resources.About1 +
                _getString() +
                Resources.About2;
            label13.Text = Resources.String_SetImage;
            foreach (Control control in (ArrangedElementCollection)groupBox3.Controls)
            {
                if (control is ComboBox box)
                {
                    box.SelectedIndex = 0;
                }
            }

            foreach (Control control in (ArrangedElementCollection)groupBox6.Controls)
            {
                if (control is ComboBox box)
                {
                    box.SelectedIndex = 0;
                }
            }
            _iOld = -1;
            _getListViewItems = TrData.GetListViewItems;
            _listViewItems = _getListViewItems();
            listView1.VirtualListSize = _listViewItems.Count;
            ShowBoxList(1);
            GC.Collect();
        }

        #endregion

        #region 列表视图相关
        /// <summary>
        /// 列表视图选择条目变化时的动作响应函数
        /// </summary>
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            // ListView失去焦点选中行不能高亮显示的问题解决
            // int num = (int) MessageBox.Show("点击", "Debug");
            if (listView1.SelectedIndices.Count > 0)
            {
                if (_iOld == -1)
                {
                    listView1.Items[listView1.SelectedIndices[0]].BackColor = SystemColors.Highlight; //设置选中项的背景颜色 
                    listView1.Items[listView1.SelectedIndices[0]].ForeColor = Color.White;
                    _iOld = listView1.SelectedIndices[0];
                }
                else if (listView1.SelectedIndices[0] != _iOld)
                {
                    listView1.Items[listView1.SelectedIndices[0]].BackColor = SystemColors.Highlight; //设置选中项的背景颜色 
                    listView1.Items[listView1.SelectedIndices[0]].ForeColor = Color.White;
                    listView1.Items[_iOld].BackColor = SystemColors.Window; //恢复默认背景色
                    listView1.Items[_iOld].ForeColor = Color.Black;
                    _iOld = listView1.SelectedIndices[0];
                }

                int tag = (int)listView1.Items[e.ItemIndex].Tag; // 列表内编号

                SetItemStatus(tag);
                SetItemImage(tag);
            }
            else
            {
                listView1.Items[_iOld].BackColor = SystemColors.Window;
                listView1.Items[_iOld].ForeColor = Color.Black;
                _iOld = -1;
            }
        }

        /// <summary>
        /// 虚拟列表相关
        /// </summary>
        private void listView1_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
        {
            e.Item = _listViewItems[e.ItemIndex];
        }

        /// <summary>
        /// 双击列表是将选定道具加入配装界面
        /// </summary>
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            int num = Convert.ToInt32(listView1.Items[listView1.SelectedIndices[0]].SubItems[0].Text);
            ItemChanged(1, num);
        }

        private void listView1_ClearSelection()
        {
            if (_iOld > -1)
            {
                listView1.Items[_iOld].BackColor = SystemColors.Window; //恢复默认背景色
                listView1.Items[_iOld].ForeColor = Color.Black;
                _iOld = -1;
            }
        }
        #endregion

        #region 图片框相关
        /// <summary>
        /// 单击图片框时的动作响应，这里是进行分流
        /// </summary>
        private void pictureBox_Click(object sender, EventArgs e)
        {
            PictureBox picBox = sender as PictureBox;
            MouseEventArgs mouseE = (MouseEventArgs)e;
            int tag = Convert.ToInt32(picBox.Tag);
            if (1 < tag && tag < 20)
            {
                PictureBoxd_Click(tag - 2);
            }

            if (30 < tag && tag < 50)
            {
                if (mouseE.Button == MouseButtons.Left)
                {
                    PictureBoxdddd_Click(tag - 30);
                }
                else
                {
                    PictureBoxdddd_RightClick(tag - 30);
                }
            }

            if (-30 > tag && tag > -50)
            {
                if (mouseE.Button == MouseButtons.Left)
                {
                    PictureBoxdddd_Click(tag + 30);
                }
                else
                {
                    PictureBoxdddd_RightClick(tag + 30);
                }
            }

            if (20 < tag && tag < 30)
            {
                PictureBoxdddd_Click(tag);
            }

            if (50 < tag && tag < 81)
            {
                if (mouseE.Button == MouseButtons.Right)
                {
                    PictureBoxBoxd_RightClick(tag - 50);
                }
            }
        }

        /// <summary>
        /// 双击图片框时的动作响应，这里是进行分流
        /// </summary>
        private void pictureBox_DoubleClick(object sender, EventArgs e)
        {
            PictureBox picBox = sender as PictureBox;
            int tag = Convert.ToInt32(picBox.Tag);
            if (30 < tag && tag < 50)
            {
                PictureBoxdddd_DoubleClick(tag - 30);
            }

            if (-30 > tag && tag > -50)
            {
                PictureBoxdddd_DoubleClick(tag + 30);
            }

            if (20 < tag && tag < 30)
            {
                PictureBoxdddd_DoubleClick(tag);
            }
        }
        #endregion

        #region tabPage1属性页面
        /// <summary>
        /// 点击套装页签内图片时跳转到对应道具
        /// </summary>
        /// <param name="boxNum">点击的套装图片框序号</param>
        private void PictureBoxd_Click(int boxNum)
        {
            int numItem;
            try
            {
                // 程序开启未选定任何道具时会抛出异常
                numItem = Convert.ToInt32(listView1.Items[listView1.SelectedIndices[0]].SubItems[0].Text);
            }
            catch
            {
                return;
            }

            _setInt2 = TrData.PictureBoxd_Click;
            int tagItem = _setInt2(boxNum, numItem);
            if (tagItem >= 0)
            {
                listView1.Items[tagItem].Selected = true;
                listView1.EnsureVisible(tagItem);
            }
        }

        /// <summary>
        /// 设置属性页图片
        /// </summary>
        /// <param name="tagItem">装备索引</param>
        private void SetItemImage(int tagItem)
        {
            _getImagesByTag = TrData.GetItemImage;
            Image[] images = _getImagesByTag(tagItem);
            pictureBox1.Image = images[0];
            pictureBox2.Image = images[1];
            pictureBox3.Image = images[2];
            pictureBox4.Image = images[3];
            pictureBox5.Image = images[4];
            pictureBox6.Image = images[5];
            pictureBox7.Image = images[6];
            pictureBox8.Image = images[7];
            pictureBox9.Image = images[8];
            pictureBox10.Image = images[9];
            pictureBox11.Image = images[10];
            if (images[0] == null)
            {
                label8.Show();
            }
            else
            {
                label8.Hide();
            }
        }

        /// <summary>
        /// 显示道具信息和属性
        /// </summary>
        /// <param name="tagItem">装备索引</param>
        private void SetItemStatus(int tagItem) // 
        {
            _getStringsByTag = TrData.GetItemStatus;
            string[] strings = _getStringsByTag(tagItem);
            label2.Text = strings[0];
            label3.Text = strings[1];
            label4.Text = strings[2];
            label5.Text = strings[3];
            label11.Text = strings[4];
            label7.Text = strings[5];
            label6.Text = strings[6];
            label12.Text = strings[7];
            toolTip1.SetToolTip(label12, strings[11]);
            textBox1.Text = strings[8];
            textBox8.Text = strings[9];
            textBox9.Text = strings[10];
        }
        #endregion

        #region tabPage2搜索页面
        /// <summary>
        /// 点击韩文单选按钮动作响应
        /// </summary>
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton3.Checked)
            {
                return;
            }

            _doSomeThing = TrData.radioButton3_CheckedChanged;
            _doSomeThing();
            listView1_ClearSelection();
            _getListViewItems = TrData.GetListViewItems;
            _listViewItems = _getListViewItems();
            listView1.VirtualListSize = _listViewItems.Count;
            listView1.RedrawItems(0, listView1.VirtualListSize - 1, true);

            ShowBoxList(2);
        }

        /// <summary>
        /// 点击中文单选按钮动作响应
        /// </summary>
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton4.Checked)
            {
                return;
            }

            _doSomeThing = TrData.radioButton4_CheckedChanged;
            _doSomeThing();
            listView1_ClearSelection();
            _getListViewItems = TrData.GetListViewItems;
            _listViewItems = _getListViewItems();
            listView1.VirtualListSize = _listViewItems.Count;
            listView1.RedrawItems(0, listView1.VirtualListSize - 1, true);

            ShowBoxList(2);
        }

        /// <summary>
        /// 搜索按钮动作响应
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            // Search用于判断是否发生对列表产生变化的搜索
            int[] indexInts = new int[8];
            float[] indexFloats = new float[3];
            bool[] indexBools = new bool[17];
            indexInts[0] = comboBox1.SelectedIndex;
            indexInts[1] = comboBox2.SelectedIndex;
            indexInts[2] = comboBox3.SelectedIndex;
            indexInts[3] = comboBox4.SelectedIndex;
            indexInts[4] = comboBox5.SelectedIndex;
            indexInts[5] = comboBox6.SelectedIndex;
            indexInts[6] = comboBox7.SelectedIndex;
            indexBools[0] = checkBox1.Checked;
            indexBools[1] = checkBox2.Checked;
            indexBools[2] = checkBox3.Checked;
            indexBools[3] = radioButton5.Checked;
            indexBools[4] = radioButton6.Checked;
            indexBools[5] = radioButton7.Checked;
            indexBools[6] = radioButton8.Checked;
            indexBools[7] = radioButton9.Checked;
            indexBools[8] = radioButton10.Checked;
            indexBools[9] = radioButton11.Checked;
            indexBools[10] = checkBox6.Checked; //SSS
            indexBools[11] = checkBox7.Checked; //SS
            indexBools[12] = checkBox8.Checked; //S
            indexBools[13] = checkBox9.Checked; //A
            indexBools[14] = checkBox10.Checked; //B
            indexBools[15] = checkBox11.Checked; //C
            indexBools[16] = checkBox12.Checked; //No
            try
            {
                indexFloats[0] = Convert.ToSingle(textBox3.Text);
                indexFloats[1] = Convert.ToSingle(textBox4.Text);
                indexFloats[2] = Convert.ToSingle(textBox5.Text);
                indexInts[7] = Convert.ToInt32(textBox6.Text);
            }
            catch
            {
                return;
            }

            _getFind = TrData.FindItemList;
            _getFind(indexInts, indexFloats, indexBools);
            listView1_ClearSelection();
            _getListViewItems = TrData.GetListViewItems;
            _listViewItems = _getListViewItems();
            listView1.VirtualListSize = _listViewItems.Count;
        }

        /// <summary>
        /// 恢复全部按钮动作响应函数
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            // 获取选中行第一列编号 -> num
            int num = -1;
            if (listView1.SelectedIndices.Count > 0)
            {
                num = Convert.ToInt32(listView1.Items[listView1.SelectedIndices[0]].Text);
            }

            _getTagByNum = TrData.GetItemTag;
            int tag = _getTagByNum(num);
            _doSomeThing = TrData.button3_Click;
            _doSomeThing();
            _getListViewItems = TrData.GetListViewItems;
            _listViewItems = _getListViewItems();
            listView1.VirtualListSize = _listViewItems.Count;

            if (tag > -1)
            {
                listView1.Items[tag].Selected = true;
                listView1.EnsureVisible(tag);
            }
        }

        /// <summary>
        /// 搜索编号按钮动作响应
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            // 若进行装备搜索时则搜索虚拟列表而非List
            int int32 = 0;
            if (radioButton1.Checked || radioButton2.Checked)
            {
                try
                {
                    int32 = Convert.ToInt32(textBox2.Text);
                }
                catch
                {
                    return;
                }

                if (int32 <= 0)
                {
                    return;
                }
            }

            if (radioButton1.Checked || radioButton2.Checked)
            {
                _getNumFind = TrData.button1_Click_Num;
                int index = _getNumFind(radioButton1.Checked, int32);
                if (index >= 0)
                {
                    listView1.Items[index].Selected = true;
                    listView1.EnsureVisible(index);
                }
            }
            else
            {
                _setString = TrData.button1_Click_string;
                _setString(textBox2.Text);
                listView1_ClearSelection();
                _getListViewItems = TrData.GetListViewItems;
                _listViewItems = _getListViewItems();
                listView1.VirtualListSize = _listViewItems.Count; // 虚拟列表长度即为符合条件的道具数量
            }

        }

        /// <summary>
        /// 搜索条件1选定时对应单选按钮可以使用
        /// </summary>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                radioButton5.Enabled = true;
                radioButton6.Enabled = true;
            }
            else
            {
                radioButton11.Select();
                radioButton5.Enabled = false;
                radioButton6.Enabled = false;
            }
        }

        /// <summary>
        /// 搜索条件2选定时对应单选按钮可以使用
        /// </summary>
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                radioButton7.Enabled = true;
                radioButton8.Enabled = true;
            }
            else
            {
                radioButton11.Select();
                radioButton7.Enabled = false;
                radioButton8.Enabled = false;
            }
        }

        /// <summary>
        /// 搜索条件3选定时对应单选按钮可以使用
        /// </summary>
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                radioButton9.Enabled = true;
                radioButton10.Enabled = true;
            }
            else
            {
                radioButton11.Select();
                radioButton9.Enabled = false;
                radioButton10.Enabled = false;
            }
        }

        /// <summary>
        /// 等级全选选择框
        /// </summary>
        private void checkBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox13.Checked)
            {
                checkBox6.Checked = true;
                checkBox7.Checked = true;
                checkBox8.Checked = true;
                checkBox9.Checked = true;
                checkBox10.Checked = true;
                checkBox11.Checked = true;
                checkBox12.Checked = true;
            }
            else
            {
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;
                checkBox9.Checked = false;
                checkBox10.Checked = false;
                checkBox11.Checked = false;
                checkBox12.Checked = false;
            }
        }
        #endregion

        #region tabPage5配装页面
        /// <summary>
        /// 单击配装栏的图片框跳转选择道具
        /// </summary>
        /// <param name="boxNum">点击的配装图片框序号</param>
        private void PictureBoxdddd_Click(int boxNum)
        {
            _setInt = TrData.PictureBoxdddd_Click;
            int tag = _setInt(boxNum);
            if (tag >= 0)
            {
                listView1.Items[tag].Selected = true;
                listView1.EnsureVisible(tag);
            }
        }

        /// <summary>
        /// 双击配装栏的图片框卸下选择道具
        /// </summary>
        /// <param name="boxNum">点击的配装图片框序号</param>
        private void PictureBoxdddd_DoubleClick(int boxNum)
        {
            ItemChanged(2, boxNum);
            //label18.Text = "DoubleClick " + BoxNum;
        }

        /// <summary>
        /// 右击配装栏的图片框开启宝石镶嵌窗口
        /// </summary>
        /// <param name="boxNum">右击的配装图片框序号</param>
        private void PictureBoxdddd_RightClick(int boxNum)
        {
            _setInt = TrData.PictureBoxdddd_RightClick;
            int tagItem = _setInt(boxNum);
            if (tagItem >= 0)
            {
                StoneForm stoneForm = new StoneForm(tagItem);
                stoneForm.ShowDialog();
                if (stoneForm.IsDisposed)
                {
                    StatusShow();
                }
            }
        }

        /// <summary>
        /// 展示属性
        /// </summary>
        private void StatusShow()
        {
            _getStrings = TrData.StatusShow;
            string[] strings = _getStrings();
            textBox10.Text = strings[0];
            textBox11.Text = strings[1];
            for (int i = 2; i < strings.Length; i += 2)
            {
                long pic = Convert.ToInt64(strings[i]);
                if (pic >= 0)
                {
                    if (i < 2 + TrData.Positions * 2)
                    {
                        GetpictureBoxdddd(i / 2).Image = _showDds(pic);
                        toolTip1.SetToolTip(GetpictureBoxdddd(i / 2), strings[i + 1]);
                    }
                    else if (i < 2 + TrData.Positions * 4)
                    {
                        GetpictureBoxdddd(-i / 2 + TrData.Positions).Image = _showDds(pic);
                        toolTip1.SetToolTip(GetpictureBoxdddd(-i / 2 + TrData.Positions), strings[i + 1]);
                    }
                    else
                    {
                        GetpictureBoxdddd(i / 2 - TrData.Positions * 2 + 20).Image = _showDds(pic);
                        toolTip1.SetToolTip(GetpictureBoxdddd(i / 2 - TrData.Positions * 2 + 20), strings[i + 1]);
                    }
                }
                else
                {
                    if (i < 2 + TrData.Positions * 2)
                    {
                        GetpictureBoxdddd(i / 2).Image = GetpictureBoxddddDefaultPic(i / 2);
                        toolTip1.SetToolTip(GetpictureBoxdddd(i / 2), strings[i + 1]);
                    }
                    else if (i < 2 + TrData.Positions * 4)
                    {
                        GetpictureBoxdddd(-i / 2 + TrData.Positions).Image = GetpictureBoxddddDefaultPic(-i / 2 + TrData.Positions);
                        toolTip1.SetToolTip(GetpictureBoxdddd(-i / 2 + TrData.Positions), strings[i + 1]);
                    }
                    else
                    {
                        GetpictureBoxdddd(i / 2 - TrData.Positions * 2 + 20).Image = GetpictureBoxddddDefaultPic(i / 2 - TrData.Positions * 2 + 20);
                        toolTip1.SetToolTip(GetpictureBoxdddd(i / 2 - TrData.Positions * 2 + 20), strings[i + 1]);
                    }
                }
            }
        }

        /// <summary>
        /// 配装页面发生改变时对界面进行修改
        /// 模式1为添加，模式2为删除，模式3为转换，模式4为读取后显示图片
        /// </summary>
        /// <param name="mode">用于判断模式</param>
        /// <param name="a1">模式1为tag，模式2为BoxNum</param>
        private void ItemChanged(int mode, int a1)
        {
            _setItemChanged(mode, a1);
            StatusShow();
        }

        /// <summary>
        /// 获取点击的配装图片框
        /// </summary>
        /// <param name="id">图片框序号</param>
        /// <returns>对应的图片框对象</returns>
        private PictureBox GetpictureBoxdddd(int id) // 
        {
            switch (id)
            {
                case 21: return pictureBox_1001;
                case 22: return pictureBox_3007;
                case 23: return pictureBox_5006;
                case 24: return pictureBox_5010;
                case 1: return pictureBox_2001;
                case 2: return pictureBox_2002;
                case 3: return pictureBox_2003;
                case 4: return pictureBox_2004;
                case 5: return pictureBox_3001;
                case 6: return pictureBox_3002;
                case 7: return pictureBox_3004;
                case 8: return pictureBox_3005;
                case 9: return pictureBox_3003;
                case 10: return pictureBox_4001;
                case 11: return pictureBox_1003;
                case 12: return pictureBox_3008;
                case 13: return pictureBox_3009;
                case 14: return pictureBox_3010;
                case -1: return pictureBox_2001A;
                case -2: return pictureBox_2002A;
                case -3: return pictureBox_2003A;
                case -4: return pictureBox_2004A;
                case -5: return pictureBox_3001A;
                case -6: return pictureBox_3002A;
                case -7: return pictureBox_3004A;
                case -8: return pictureBox_3005A;
                case -9: return pictureBox_3003A;
                case -10: return pictureBox_4001A;
                case -11: return pictureBox_1003A;
                case -12: return pictureBox_3008A;
                case -13: return pictureBox_3009A;
                case -14: return pictureBox_3010A;
                default: return null;
            }
        }

        /// <summary>
        /// 获取点击的配装图片框的默认图片
        /// </summary>
        /// <param name="id">图片框序号</param>
        /// <returns>对应的图片框的默认图片</returns>
        private static Bitmap GetpictureBoxddddDefaultPic(int id) // 
        {
            switch (id)
            {
                case 22: return Resources.position3007;
                case 23: return Resources.position5006;
                case 24: return Resources.position5010;
                case 1: return Resources.position2001;
                case 2: return Resources.position2002;
                case 3: return Resources.position2003;
                case 4: return Resources.position2004;
                case 5: return Resources.position3001;
                case 6: return Resources.position3002;
                case 7: return Resources.position3004;
                case 8: return Resources.position3005;
                case 9: return Resources.position3003;
                case 10: return Resources.position4001;
                case 11: return Resources.position1003;
                case 12: return Resources.position3008;
                case 13: return Resources.position3009;
                case 14: return Resources.position3010;
                case -1: return Resources.position2001;
                case -2: return Resources.position2002;
                case -3: return Resources.position2003;
                case -4: return Resources.position2004;
                case -5: return Resources.position3001;
                case -6: return Resources.position3002;
                case -7: return Resources.position3004;
                case -8: return Resources.position3005;
                case -9: return Resources.position3003;
                case -10: return Resources.position4001;
                case -11: return Resources.position1003;
                case -12: return Resources.position3008;
                case -13: return Resources.position3009;
                case -14: return Resources.position3010;
                default: return null;
            }
        }

        /// <summary>
        /// 配装角色变化动作响应
        /// </summary>
        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            _setIntVoid = TrData.CharChange;
            _setIntVoid(comboBox8.SelectedIndex);
            ItemChanged(3, 0);
        }

        /// <summary>
        /// 点击保存按钮进行保存
        /// </summary>
        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog sv = new SaveFileDialog
            {
                DefaultExt = "itv",
                FileName = "saveFileDialog1",
                Filter = Resources.String_FileFormatDesc,
                InitialDirectory = Path.Replace("$\\", "")
            };
            if (sv.ShowDialog() == DialogResult.OK)
            {
                _setString = TrData.SaveItv;
                _setString(sv.FileName);
            }
            sv.Dispose();

            GC.Collect();
        }

        /// <summary>
        /// 点击读取按钮进行读取
        /// </summary>
        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                DefaultExt = "itv",
                FileName = "openFileDialog1",
                Filter = Resources.String_FileFormatDesc,
                InitialDirectory = Path.Replace("$\\", "")
            };
            if (op.ShowDialog() == DialogResult.OK)
            {
                _getIntByString = TrData.ReadItv;
                comboBox8.SelectedIndex = _getIntByString(op.FileName);
            }
            op.Dispose();
            StatusShow();
            GC.Collect();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            _setBooleanVoid = TrData.SetCheck1;
            _setBooleanVoid(checkBox4.Checked);
            StatusShow();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            _setBooleanVoid = TrData.SetCheck2;
            _setBooleanVoid(checkBox5.Checked);
            StatusShow();
        }

        private void numeric1_ValueChanged(object sender, EventArgs e)
        {
            _setIntVoid = TrData.SetTopSpeed;
            _setIntVoid((int)numeric1.Value);
            StatusShow();
        }

        private void numeric2_ValueChanged(object sender, EventArgs e)
        {
            _setIntVoid = TrData.SetAcceleration;
            _setIntVoid((int)numeric2.Value);
            StatusShow();
        }

        private void numeric3_ValueChanged(object sender, EventArgs e)
        {
            _setIntVoid = TrData.SetPower;
            _setIntVoid((int)numeric3.Value);
            StatusShow();
        }

        private void numeric4_ValueChanged(object sender, EventArgs e)
        {
            _setIntVoid = TrData.SetControl;
            _setIntVoid((int)numeric4.Value);
            StatusShow();
        }

        private void numeric5_ValueChanged(object sender, EventArgs e)
        {
            _setIntVoid = TrData.SetLevel;
            _setIntVoid((int)numeric5.Value);
            StatusShow();
        }
        #endregion

        #region tabPage6开箱页面
        private PictureBox GetpictureBoxBoxd(int id) // 
        {
            switch (id)
            {
                case 0: return pictureBoxB1;
                case 1: return pictureBoxB2;
                case 2: return pictureBoxB3;
                case 3: return pictureBoxB4;
                case 4: return pictureBoxB5;
                case 5: return pictureBoxB6;
                case 6: return pictureBoxB7;
                case 7: return pictureBoxB8;
                case 8: return pictureBoxB9;
                case 9: return pictureBoxB10;
                case 10: return pictureBoxB11;
                case 11: return pictureBoxB12;
                case 12: return pictureBoxB13;
                case 13: return pictureBoxB14;
                case 14: return pictureBoxB15;
                case 15: return pictureBoxB16;
                case 16: return pictureBoxB17;
                case 17: return pictureBoxB18;
                case 18: return pictureBoxB19;
                case 19: return pictureBoxB20;
                case 20: return pictureBoxB21;
                case 21: return pictureBoxB22;
                case 22: return pictureBoxB23;
                case 23: return pictureBoxB24;
                case 24: return pictureBoxB25;
                case 25: return pictureBoxB26;
                case 26: return pictureBoxB27;
                case 27: return pictureBoxB28;
                case 28: return pictureBoxB29;
                case 29: return pictureBoxB30;

                default: return null;
            }
        }

        /// <summary>
        /// 翻页用函数
        /// </summary>
        /// <param name="mode">1往前翻 2往后翻</param>
        /// <param name="page">翻得页数</param>
        private void BoxPageChange(int mode, int page)
        {
            _setInt2Void = TrData.BoxPageChange;
            _setInt2Void(mode, page);
            ShowBoxList(1);
        }

        /// <summary>
        /// 显示开箱列表，模式1图片文本，模式2仅文本
        /// </summary>
        /// <param name="mode">模式</param>
        private void ShowBoxList(int mode)
        {
            _getStringsByInt = TrData.ShowBoxList;
            string[] strings = _getStringsByInt(mode);
            if (mode == 1)
            {
                for (int i = 0; i < PerPage; i++)
                {
                    long offset = Convert.ToInt64(strings[i * 2 + 1]);
                    GetpictureBoxBoxd(i).Image = _showDds(offset);
                    toolTip1.SetToolTip(GetpictureBoxBoxd(i), strings[i * 2]);
                }
            }
            else
            {
                for (int i = 0; i < PerPage; i++)
                {
                    toolTip1.SetToolTip(GetpictureBoxBoxd(i), strings[i * 2]);
                }
            }
            label21.Text = strings[2 * PerPage] + Resources.String_BetweenPages + strings[2 * PerPage + 1];
        }

        private void PictureBoxBoxd_RightClick(int boxNum)
        {
            _getBooleanByTag = TrData.IsBoxable;
            if (_getBooleanByTag(boxNum))
            {
                _setInt = TrData.PictureBoxBoxd_RightClick;
                int tag = _setInt(boxNum);
                BoxForm form = new BoxForm(tag);
                form.ShowDialog();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            BoxPageChange(1, 1);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            BoxPageChange(2, 1);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            BoxPageChange(1, 5);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            BoxPageChange(2, 5);
        }
        #endregion

        #region 窗体拖动用
        /// <summary>
        /// 点击右上角红色方框关闭窗口
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            _mPoint = new Point(e.X, e.Y);
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Location = new Point(Location.X + e.X - _mPoint.X, Location.Y + e.Y - _mPoint.Y);
            }
        }

        #endregion
    }
}