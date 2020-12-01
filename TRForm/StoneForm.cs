using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace TalesRunnerForm
{
    public partial class StoneForm : Form
    {
        private int _selected;
        private Point _mPoint;
        // private readonly MainForm _form;
        private const string StoneSpecial = "特殊";
        private const string StoneRed = "红色";
        private const string StoneOrange = "橙色";
        private const string StoneGreen = "绿色";
        private const string StoneBlue = "蓝色";
        private static int _tag;
        private static readonly int[] SlotLimit = { 0, 0, 0, 0, 0, 0, 0, 0 };
        private static readonly int[] SlotStone = { 0, 0, 0, 0, 0, 0, 0, 0 }; // 字母、颜色、等级

        #region 委托
        /// <summary>
        /// 委托，返回保存的宝石镶嵌数值到父窗体
        /// </summary>
        /// <param name="tag">道具索引</param>
        /// <param name="stone">宝石镶嵌数据</param>
        private delegate void StoneValue(int tag, string stone);

        /// <summary>
        /// 委托，获取宝石对应的属性文本
        /// </summary>
        /// <param name="stone">宝石序号</param>
        /// <returns>属性文本</returns>
        private delegate string StoneAttr(string stone);

        private delegate string StoneSlot(int tag);
        #endregion

        /// <summary>
        /// 测试用构造函数
        /// </summary>
        public StoneForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tag">道具索引</param>
        public StoneForm(int tag)
        {
            InitializeComponent();
            StoneSlot ss = TrData.StoneSlots;
            string str = ss(tag);
            _tag = tag;
            string str1 = str.Substring(0, 32);
            string str2 = str.Substring(32, 24);
            for (int i = 0; i < 8; i++)
            {
                SlotLimit[i] = Convert.ToInt32(str1.Substring(i * 4, 4), 2);
                SlotStone[i] = Convert.ToInt32(str2.Substring(i * 3, 3));
            }
        }

        /// <summary>
        /// 窗体初始化，设置各项多选框内容
        /// </summary>
        private void StoneForm_Load(object sender, EventArgs e)
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
                    foreach (Control control1 in (ArrangedElementCollection)Controls) // 页面1控件加载
                    {
                        SetControl(control1);
                    }
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

            StoneSlot ss = TrData.GetItemName;
            label1.Text = ss(_tag);
            comboBox1.Items.Add(StoneSpecial);
            comboBox1.Items.Add(StoneRed);
            comboBox1.Items.Add(StoneOrange);
            comboBox1.Items.Add(StoneGreen);
            comboBox1.Items.Add(StoneBlue);
            comboBox1.SelectedIndex = 0;
            comboBox2.Items.Add("A");
            comboBox2.Items.Add("B");
            comboBox2.Items.Add("C");
            comboBox2.Items.Add("D");
            comboBox2.Items.Add("E");
            comboBox2.Items.Add("F");
            comboBox2.Items.Add("G");
            comboBox2.Items.Add("H");
            comboBox2.Items.Add("I");
            comboBox2.Items.Add("J");
            comboBox2.SelectedIndex = 0;
            pictureBox1.Image = SlotStone[0] != 0 ? StoneImage(SlotStone[0] % 100) : SlotImage(SlotLimit[0]);
            pictureBox2.Image = SlotStone[1] != 0 ? StoneImage(SlotStone[1] % 100) : SlotImage(SlotLimit[1]);
            pictureBox3.Image = SlotStone[2] != 0 ? StoneImage(SlotStone[2] % 100) : SlotImage(SlotLimit[2]);
            pictureBox4.Image = SlotStone[3] != 0 ? StoneImage(SlotStone[3] % 100) : SlotImage(SlotLimit[3]);
            pictureBox5.Image = SlotStone[4] != 0 ? StoneImage(SlotStone[4] % 100) : SlotImage(SlotLimit[4]);
            pictureBox6.Image = SlotStone[5] != 0 ? StoneImage(SlotStone[5] % 100) : SlotImage(SlotLimit[5]);
            pictureBox7.Image = SlotStone[6] != 0 ? StoneImage(SlotStone[6] % 100) : SlotImage(SlotLimit[6]);
            pictureBox8.Image = SlotStone[7] != 0 ? StoneImage(SlotStone[7] % 100) : SlotImage(SlotLimit[7]);
        }

        /// <summary>
        /// 根据选择的图片框Tag获取选择的图片框
        /// </summary>
        /// <returns>选择的图片框</returns>
        private PictureBox GetPictureBox()
        {
            switch (_selected)
            {
                case 1: return pictureBox1;
                case 2: return pictureBox2;
                case 3: return pictureBox3;
                case 4: return pictureBox4;
                case 5: return pictureBox5;
                case 6: return pictureBox6;
                case 7: return pictureBox7;
                case 8: return pictureBox8;
                default: return null;
            }
        }

        /// <summary>
        /// 点击图片框动作响应
        /// </summary>
        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (_selected != 0)
            {
                GetPictureBox().BorderStyle = BorderStyle.None;
            }
            PictureBox pbox = sender as PictureBox;
            _selected = Convert.ToInt32(pbox.Tag);
            GetPictureBox().BorderStyle = BorderStyle.FixedSingle;
            StoneAttr sa = TrData.StoneAttrStr;
            textBox1.Text = SlotStone[_selected - 1] != 0 ? sa(SlotStone[_selected - 1].ToString()) : "";
            // TODO 加入插槽限制语句
        }

        /// <summary>
        /// 取消按钮动作响应
        /// </summary>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        /// <summary>
        /// 多选框内容变化时，调整等级多选框
        /// </summary>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();
            if (comboBox1.SelectedIndex != 0)
            {
                comboBox3.Items.Add("LV0");
            }
            comboBox3.Items.Add("LV1");
            comboBox3.Items.Add("LV2");
            comboBox3.Items.Add("LV3");
            comboBox3.Items.Add("LV4");
            comboBox3.Items.Add("LV5");
            comboBox3.SelectedIndex = 0;
        }

        /// <summary>
        /// 镶嵌按钮动作响应
        /// </summary>
        private void button3_Click(object sender, EventArgs e)
        {
            if (ConditionCheck(comboBox1.SelectedIndex)) // 颜色符合
            {
                int p = (comboBox2.SelectedIndex + 1) * 100;
                p += (comboBox1.SelectedIndex + 1) * 10;
                switch (comboBox3.SelectedItem)
                {
                    case "LV0": p += 1; break;
                    case "LV1": p += 2; break;
                    case "LV2": p += 3; break;
                    case "LV3": p += 4; break;
                    case "LV4": p += 5; break;
                    case "LV5": p += 6; break;
                }
                GetPictureBox().Image = StoneImage(p % 100);
                SlotStone[_selected - 1] = p;
            }
        }

        /// <summary>
        /// 检查是否可以镶嵌
        /// </summary>
        private bool ConditionCheck(int color)
        {
            if (_selected != 0)
            {
                int limit = SlotLimit[_selected - 1];
                if (limit == 0 || limit == 1)
                {
                    return false;
                }
                if (color == 0)
                {
                    if (limit != 7 && limit != 12)
                    {
                        return true;
                    }
                    return false;
                }
                if (color == 1)
                {
                    if (limit == 6 || limit == 11 || limit == 13)
                    {
                        return true;
                    }
                    return false;
                }
                if (color == 2)
                {
                    if (limit == 5 || limit == 10 || limit == 13)
                    {
                        return true;
                    }
                    return false;
                }
                if (color == 3)
                {
                    if (limit == 4 || limit == 9 || limit == 13)
                    {
                        return true;
                    }
                    return false;
                }
                if (color == 4)
                {
                    if (limit == 7 || limit == 12)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// 拆卸按钮动作响应
        /// </summary>
        private void button4_Click(object sender, EventArgs e)
        {
            if (_selected != 0)
            {
                GetPictureBox().Image = SlotImage(SlotLimit[_selected - 1]);
                SlotStone[_selected - 1] = 0;
            }
        }

        /// <summary>
        /// 返回宝石图片
        /// </summary>
        /// <param name="i">宝石编号</param>
        /// <returns>对应宝石图片</returns>
        private static Image StoneImage(int i)
        {
            switch (i)
            {
                case 12:
                    return Properties.Resources.stone_special_1;
                case 13:
                    return Properties.Resources.stone_special_2;
                case 14:
                    return Properties.Resources.stone_special_3;
                case 15:
                    return Properties.Resources.stone_special_4;
                case 16:
                    return Properties.Resources.stone_special_5;
                case 21:
                    return Properties.Resources.stone_red_0;
                case 22:
                    return Properties.Resources.stone_red_1;
                case 23:
                    return Properties.Resources.stone_red_2;
                case 24:
                    return Properties.Resources.stone_red_3;
                case 25:
                    return Properties.Resources.stone_red_4;
                case 26:
                    return Properties.Resources.stone_red_5;
                case 31:
                    return Properties.Resources.stone_orange_0;
                case 32:
                    return Properties.Resources.stone_orange_1;
                case 33:
                    return Properties.Resources.stone_orange_2;
                case 34:
                    return Properties.Resources.stone_orange_3;
                case 35:
                    return Properties.Resources.stone_orange_4;
                case 36:
                    return Properties.Resources.stone_orange_5;
                case 41:
                    return Properties.Resources.stone_green_0;
                case 42:
                    return Properties.Resources.stone_green_1;
                case 43:
                    return Properties.Resources.stone_green_2;
                case 44:
                    return Properties.Resources.stone_green_3;
                case 45:
                    return Properties.Resources.stone_green_4;
                case 46:
                    return Properties.Resources.stone_green_5;
                case 51:
                    return Properties.Resources.stone_blue_0;
                case 52:
                    return Properties.Resources.stone_blue_1;
                case 53:
                    return Properties.Resources.stone_blue_2;
                case 54:
                    return Properties.Resources.stone_blue_3;
                case 55:
                    return Properties.Resources.stone_blue_4;
                case 56:
                    return Properties.Resources.stone_blue_5;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 返回宝石槽图片
        /// </summary>
        /// <param name="i">宝石槽编号</param>
        /// <returns>对应宝石槽图片</returns>
        private static Image SlotImage(int i)
        {
            switch (i)
            {
                case 0:
                    return Properties.Resources._1;
                case 1:
                    return Properties.Resources._1;
                case 2:
                    return Properties.Resources._2;
                case 4:
                    return Properties.Resources._4;
                case 5:
                    return Properties.Resources._5;
                case 6:
                    return Properties.Resources._6;
                case 7:
                    return Properties.Resources._7;
                case 9:
                    return Properties.Resources._9;
                case 10:
                    return Properties.Resources._10;
                case 11:
                    return Properties.Resources._11;
                case 12:
                    return Properties.Resources._12;
                case 13:
                    return Properties.Resources._2;
                default:
                    return Properties.Resources._1;
            }
        }

        /// <summary>
        /// 保存按钮动作响应
        /// </summary>
        private void button1_Click(object sender, EventArgs e)
        {
            StoneValue sv = TrData.StoneValue;
            string str = null;
            for (int i = 0; i < 8; i++)
            {
                str += SlotStone[i].ToString().PadLeft(3, '0');
            }
            sv(_tag, str);
            this.Dispose();
        }

        #region 窗体移动
        /// <summary>
        /// 鼠标按下
        /// </summary>
        private void label1_MouseDown(object sender, MouseEventArgs e)
        {
            _mPoint = new Point(e.X, e.Y);
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        private void label1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - _mPoint.X, this.Location.Y + e.Y - _mPoint.Y);
            }
        }
        #endregion

    }
}
