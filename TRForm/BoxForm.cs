using ControlEx;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using TalesRunnerForm.Properties;

namespace TalesRunnerForm
{
    public partial class BoxForm : Form
    {
        private Point _mPoint;
        private readonly string _boxName = string.Empty;
        private readonly int _itemNum;
        private readonly int _position;
        private readonly List<int> _memberTotal = new List<int>(); // 全奖品池
        // private readonly List<int> _memberBasic = new List<int>(); // 普通奖品池
        private readonly List<int> _memberSilver = new List<int>(); // 银色奖品池
        private readonly List<int> _memberGold = new List<int>(); // 金色奖品池
        private readonly List<string> _memberNameTotal = new List<string>(); // 全奖品池
        private readonly List<string> _memberNameBasic = new List<string>(); // 普通奖品池 箱子可以开到的道具的名字
        private readonly List<string> _memberNameSilver = new List<string>(); // 银色奖品池 箱子可以开到的道具的名字
        private readonly List<string> _memberNameGold = new List<string>(); // 金色奖品池 箱子可以开到的道具的名字
        private readonly List<float> _weightBasic = new List<float>(); // 普通奖品池 箱子可以开到的道具的权重
        private readonly List<float> _weightSilver = new List<float>(); // 银色奖品池 箱子可以开到的道具的权重
        private readonly List<float> _weightGold = new List<float>(); // 金色奖品池 箱子可以开到的道具的权重
        private readonly List<short> _picPkgNumTotal = new List<short>(); // 全奖品池
        private readonly List<short> _picPkgNumBasic = new List<short>(); // 普通奖品池 箱子可以开到的道具的图片所在文件
        private readonly List<short> _picPkgNumSilver = new List<short>(); // 银色奖品池 箱子可以开到的道具的图片所在文件
        private readonly List<short> _picPkgNumGold = new List<short>(); // 金色奖品池 箱子可以开到的道具的图片所在文件
        private readonly List<long> _picOffsetTotal = new List<long>(); // 全奖品池
        private readonly List<long> _picOffsetBasic = new List<long>(); // 普通奖品池 箱子可以开到的道具的图片文件偏移
        private readonly List<long> _picOffsetSilver = new List<long>(); // 银色奖品池 箱子可以开到的道具的图片文件偏移
        private readonly List<long> _picOffsetGold = new List<long>(); // 金色奖品池 箱子可以开到的道具的图片文件偏移
        private readonly float _weightTotalBasic; // 总权重
        private readonly float _weightTotalSilver; // 总权重
        private readonly float _weightTotalGold; // 总权重
        private readonly float _silverRate; // 升级银色奖池概率
        private readonly SortedList<int, int> _listResult = new SortedList<int, int>();
        private readonly List<int> _listCubeResult = new List<int>(); // 魔方前线用

        private const int PerPageShow = 25;
        private int _nowPageShow;
        private const int PerPageLuck = 4;
        private int _nowPageLuck;
        private int _clickTimes; // 魔方前线用
        private int _cubeKind; // 魔方用
        private bool[] _reveal = { false, false, false, false, false };

        #region 委托
        private delegate int GetInt(int t);

        private delegate float GetFloat(int t);
        private delegate float[] GetFloats(int t);

        private delegate string GetString(int t);

        private delegate List<string> GetStringList(int t);
        private delegate List<string>[] GetStringLists(int t);

        private delegate List<int> GetIntList(int t);
        private delegate List<int>[] GetIntLists(int t);

        private delegate List<short> GetShortList(int t);
        private delegate List<short>[] GetShortLists(int t);

        private delegate List<long> GetLongList(int t);
        private delegate List<long>[] GetLongLists(int t);

        private delegate List<float> GetFloatList(int t);
        private delegate List<float>[] GetFloatLists(int t);

        private delegate Image ShowDds(long offset, short pkgNum);
        private ShowDds _showDds;

        private delegate float Calculation(float f1, float f2);

        private delegate bool BoxBan(int t);
        private BoxBan boxBan;
        #endregion

        /// <summary>
        /// 测试用构造函数
        /// </summary>
        public BoxForm()
        {
            InitializeComponent();
        }

        public BoxForm(int tag)
        {
            List<int> memberBasic;
            InitializeComponent();
            GetString getString = TrData.GetBoxName;
            _boxName = getString(tag);
            GetInt getInt = TrData.GetBoxNum;
            _itemNum = getInt(tag);
            getInt = TrData.GetBoxPosition;
            _position = getInt(tag);
            if (_position != 395) // 非魔方道具
            {
                GetIntList getIntList = TrData.GetBoxMember;
                memberBasic = getIntList(tag);
                GetStringList getStringList = TrData.GetBoxMemberName;
                _memberNameBasic = getStringList(tag);
                GetFloatList getFloatList = TrData.GetBoxWeight;
                _weightBasic = getFloatList(tag);
                GetShortList getShortList = TrData.GetBoxPkgNum;
                _picPkgNumBasic = getShortList(tag);
                GetLongList getLongList = TrData.GetBoxOffset;
                _picOffsetBasic = getLongList(tag);
                GetFloat getFloat = TrData.GetBoxWeightTotal;
                _weightTotalBasic = getFloat(tag);
            }
            else
            {
                GetIntLists getIntLists = TrData.GetBoxMembers;
                List<int>[] intLists = getIntLists(tag);
                memberBasic = intLists[0];
                _memberSilver = intLists[1];
                _memberGold = intLists[2];
                GetStringLists getStringLists = TrData.GetBoxMemberNames;
                List<string>[] stringLists = getStringLists(tag);
                _memberNameBasic = stringLists[0];
                _memberNameSilver = stringLists[1];
                _memberNameGold = stringLists[2];
                GetFloatLists getFloatLists = TrData.GetBoxWeights;
                List<float>[] floatLists = getFloatLists(tag);
                _weightBasic = floatLists[0];
                _weightSilver = floatLists[1];
                _weightGold = floatLists[2];
                GetShortLists getShortLists = TrData.GetBoxPkgNums;
                List<short>[] shortLists = getShortLists(tag);
                _picPkgNumBasic = shortLists[0];
                _picPkgNumSilver = shortLists[1];
                _picPkgNumGold = shortLists[2];

                GetLongLists getLongLists = TrData.GetBoxOffsets;
                List<long>[] longLists = getLongLists(tag);
                _picOffsetBasic = longLists[0];
                _picOffsetSilver = longLists[1];
                _picOffsetGold = longLists[2];
                GetFloats getFloats = TrData.GetBoxWeightTotals;
                float[] floats = getFloats(tag);
                _weightTotalBasic = floats[0];
                _weightTotalSilver = floats[1];
                _weightTotalGold = floats[2];
                GetFloat getFloat = TrData.GetBoxSilverRate;
                _silverRate = getFloat(tag);
            }
            for (int index = 0; index < memberBasic.Count; index++)
            {
                if (!_memberTotal.Contains(memberBasic[index]))
                {
                    _memberTotal.Add(memberBasic[index]);
                    _memberNameTotal.Add(_memberNameBasic[index]);
                    _picPkgNumTotal.Add(_picPkgNumBasic[index]);
                    _picOffsetTotal.Add(_picOffsetBasic[index]);
                }
            }
            for (int index = 0; index < _memberSilver.Count; index++)
            {
                if (!_memberTotal.Contains(_memberSilver[index]))
                {
                    _memberTotal.Add(_memberSilver[index]);
                    _memberNameTotal.Add(_memberNameSilver[index]);
                    _picPkgNumTotal.Add(_picPkgNumSilver[index]);
                    _picOffsetTotal.Add(_picOffsetSilver[index]);
                }
            }
            for (int index = 0; index < _memberGold.Count; index++)
            {
                if (!_memberTotal.Contains(_memberGold[index]))
                {
                    _memberTotal.Add(_memberGold[index]);
                    _memberNameTotal.Add(_memberNameGold[index]);
                    _picPkgNumTotal.Add(_picPkgNumGold[index]);
                    _picOffsetTotal.Add(_picOffsetGold[index]);
                }
            }
        }

        private void BoxForm_Load(object sender, EventArgs e)
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
                        if (control1 is Button)
                        {
                            control1.Width = (int)(control1.Width * (double)dpiX / 100.0);
                            control1.Height = (int)(control1.Height * (double)dpiX / 100.0);
                            control1.Location = new Point(Width - control1.Width, 0);
                        }
                        else if (control1 is GroupBoxEx)
                        {
                            foreach (Control control2 in (ArrangedElementCollection)control1.Controls)
                            {
                                SetControl(control2);
                            }
                            SetControl(control1);
                        }
                        else
                        {
                            SetControl(control1);
                        }
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

            _showDds = TrData.ShowDds;
            label19.Text = _boxName;
            int total = _memberTotal.Count;
            if ((total - 1) / PerPageShow > 0)
            {
                for (int i = 0; i < PerPageShow; i++)
                {
                    int index = i;
                    GetpictureBox(i).Image = _picOffsetTotal[index] >= 0 ? _showDds(_picOffsetTotal[index], _picPkgNumTotal[index]) : Resources.noImage;
                    toolTip1.SetToolTip(GetpictureBox(i), _memberNameTotal[index]);
                }
            }
            else
            {
                for (int i = 0; i < total; i++)
                {
                    int index = i;
                    GetpictureBox(i).Image = _picOffsetTotal[index] >= 0 ? _showDds(_picOffsetTotal[index], _picPkgNumTotal[index]) : Resources.noImage;
                    toolTip1.SetToolTip(GetpictureBox(i), _memberNameTotal[index]);
                }
                for (int i = total; i < PerPageShow; i++)
                {
                    GetpictureBox(i).Image = null;
                    toolTip1.SetToolTip(GetpictureBox(i), null);
                }
            }

            if (_position == 250)
            {
                groupBoxEx3.Parent = null;
                groupBoxEx3.Enabled = false;
                groupBoxEx4.Parent = null;
                groupBoxEx4.Enabled = false;
            }
            else if (_position == 395)
            {
                groupBoxEx2.Parent = null;
                groupBoxEx2.Enabled = false;
                groupBoxEx4.Parent = null;
                groupBoxEx4.Enabled = false;
            }
            else //if (_position == 396)
            {
                groupBoxEx2.Parent = null;
                groupBoxEx2.Enabled = false;
                groupBoxEx3.Parent = null;
                groupBoxEx3.Enabled = false;
            }

            int k = (total - 1) / PerPageShow + 1;
            label31.Text = (_nowPageShow < 9 ? "0" + (_nowPageShow + 1) : "" + (_nowPageShow + 1)) + Resources.String_BetweenPages + (k < 10 ? "0" + k : "" + k);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private PictureBox GetpictureBox(int tag)
        {
            switch (tag)
            {
                // 奖励展示
                case 0: return pictureBox1;
                case 1: return pictureBox2;
                case 2: return pictureBox3;
                case 3: return pictureBox4;
                case 4: return pictureBox5;
                case 5: return pictureBox6;
                case 6: return pictureBox7;
                case 7: return pictureBox8;
                case 8: return pictureBox9;
                case 9: return pictureBox10;
                case 10: return pictureBox11;
                case 11: return pictureBox12;
                case 12: return pictureBox13;
                case 13: return pictureBox14;
                case 14: return pictureBox15;
                case 15: return pictureBox16;
                case 16: return pictureBox17;
                case 17: return pictureBox18;
                case 18: return pictureBox19;
                case 19: return pictureBox20;
                case 20: return pictureBox21;
                case 21: return pictureBox22;
                case 22: return pictureBox23;
                case 23: return pictureBox24;
                case 24: return pictureBox25;
                // 普通开箱
                case 31: return pictureBox26;
                case 32: return pictureBox27;
                case 33: return pictureBox28;
                case 34: return pictureBox29;
                // 魔方开箱
                case 41: return pictureBox30;
                case 42: return pictureBox31;
                case 43: return pictureBox32;
                case 44: return pictureBox33;
                case 45: return pictureBox34;
                // 前线开箱
                case 51: return pictureBox35;
                case 52: return pictureBox36;
                case 53: return pictureBox37;
                // 其他
                default: return null;
            }
        }

        private Label GetLabel(int i)
        {
            switch (i)
            {
                // 普通开箱
                case 0: return label3;
                case 1: return label4;
                case 2: return label5;
                case 3: return label6;
                case 4: return label7;
                case 5: return label8;
                case 6: return label9;
                case 7: return label10;
                // 魔方开箱
                case 41: return label11;
                case 42: return label12;
                case 43: return label13;
                case 44: return label14;
                case 45: return label15;
                case -41: return label16;
                case -42: return label17;
                case -43: return label18;
                case -44: return label22;
                case -45: return label23;
                // 前线开箱
                case 51: return label24;
                case 52: return label25;
                case 53: return label26;
                case -51: return label27;
                case -52: return label28;
                case -53: return label29;
                // 其他
                default: return null;
            }
        }

        #region 奖品展示
        private void PageChange_Show(int mode, int pace)
        {
            _showDds = TrData.ShowDds;
            int total = _memberTotal.Count - 1;
            if (mode == 1)
            {
                if (_nowPageShow - pace < 0)
                {
                    _nowPageShow = 0;
                }
                else
                {
                    _nowPageShow -= pace;
                }
            }
            if (mode == 2)
            {
                if (_nowPageShow + pace > (total / PerPageShow))
                {
                    _nowPageShow = (total / PerPageShow);
                }
                else
                {
                    _nowPageShow += pace;
                }
            }
            total++;
            if (_nowPageShow != (total / PerPageShow))
            {
                for (int i = 0; i < PerPageShow; i++)
                {
                    int index = _nowPageShow * PerPageShow + i;
                    GetpictureBox(i).Image = _showDds(_picOffsetTotal[index], _picPkgNumTotal[index]);
                    toolTip1.SetToolTip(GetpictureBox(i), _memberNameTotal[index]);
                }
            }
            else
            {
                for (int i = 0; i < (total % PerPageShow); i++)
                {
                    int index = _nowPageShow * PerPageShow + i;
                    GetpictureBox(i).Image = _showDds(_picOffsetTotal[index], _picPkgNumTotal[index]);
                    toolTip1.SetToolTip(GetpictureBox(i), _memberNameTotal[index]);
                }
                for (int i = total % PerPageShow; i < PerPageShow; i++)
                {
                    GetpictureBox(i).Image = null;
                    toolTip1.SetToolTip(GetpictureBox(i), null);
                }
            }
            int k = (total - 1) / PerPageShow + 1;
            label31.Text = (_nowPageShow < 9 ? "0" + (_nowPageShow + 1) : "" + (_nowPageShow + 1)) + Resources.String_BetweenPages + (k < 10 ? "0" + k : "" + k);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PageChange_Show(1, 1);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            PageChange_Show(2, 1);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            PageChange_Show(1, 2);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            PageChange_Show(2, 2);
        }
        #endregion

        #region 普通抽选
        private void PageChange_Luck(int mode, int pace)
        {
            int total = _listResult.Count - 1;
            if (mode == 1)
            {
                if (_nowPageLuck - pace < 0)
                {
                    _nowPageLuck = 0;
                }
                else
                {
                    _nowPageLuck -= pace;
                }
            }
            if (mode == 2)
            {
                if (_nowPageLuck + pace > (total / PerPageLuck))
                {
                    _nowPageLuck = (total / PerPageLuck);
                }
                else
                {
                    _nowPageLuck += pace;
                }
            }
            PageShow_Luck();
        }

        private void PageShow_Luck()
        {
            IList<int> times = _listResult.Values;
            IList<int> indexes = _listResult.Keys;
            int total = _listResult.Count;
            if ((total - 1) / PerPageLuck > 0)
            {
                button1.Enabled = true;
                button3.Enabled = true;
            }
            if (_nowPageLuck != (total / PerPageLuck))
            {
                for (int i = 0; i < PerPageLuck; i++)
                {
                    int index = _nowPageLuck * PerPageLuck + i;
                    GetpictureBox(i + 31).Image = _showDds(_picOffsetBasic[indexes[index]], _picPkgNumBasic[indexes[index]]);
                    GetLabel(i).Text = _memberNameBasic[indexes[index]];
                    GetLabel(i + 4).Text = Resources.String_X + times[index];
                    //toolTip1.SetToolTip(GetpictureBox(i), MemberName[index]);
                }
            }
            else
            {
                for (int i = 0; i < (total % PerPageLuck); i++)
                {
                    int index = _nowPageLuck * PerPageLuck + i;
                    GetpictureBox(i + 31).Image = _showDds(_picOffsetBasic[indexes[index]], _picPkgNumBasic[indexes[index]]);
                    GetLabel(i).Text = _memberNameBasic[indexes[index]];
                    GetLabel(i + 4).Text = Resources.String_X + times[index];
                    //toolTip1.SetToolTip(GetpictureBox(i), MemberName[index]);
                }
                for (int i = total % PerPageLuck; i < PerPageLuck; i++)
                {
                    GetpictureBox(i + 31).Image = null;
                    GetLabel(i).Text = null;
                    GetLabel(i + 4).Text = null;
                    //toolTip1.SetToolTip(GetpictureBox(i), null);
                }
            }
            int k = (total - 1) / PerPageLuck + 1;
            label32.Text = (_nowPageLuck < 9 ? "0" + (_nowPageLuck + 1) : "" + (_nowPageLuck + 1)) + Resources.String_BetweenPages + (k < 10 ? "0" + k : "" + k);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            _nowPageLuck = 0;
            _listResult.Clear();
            var r = new Random();
            for (int i = 0; i < numeric1.Value; i++)
            {
                Calculation calculation = TrData.Multi;
                float weightCurrent = calculation(_weightTotalBasic, (float)r.NextDouble());
                int index;
                calculation = TrData.Minus;
                for (index = 0; weightCurrent >= _weightBasic[index]; index++)
                {
                    weightCurrent = calculation(weightCurrent, _weightBasic[index]);
                }
                if (_listResult.ContainsKey(index))
                {
                    _listResult[index]++;
                }
                else
                {
                    _listResult.Add(index, 1);
                }
            }
            PageShow_Luck();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PageChange_Luck(1, 5);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            PageChange_Luck(1, 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PageChange_Luck(2, 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PageChange_Luck(2, 5);
        }
        #endregion

        #region 魔方抽选
        private void button7_Click(object sender, EventArgs e)
        {
            // 使遮挡标签显示
            label16.Enabled = true;
            label17.Enabled = true;
            label18.Enabled = true;
            label22.Enabled = true;
            label23.Enabled = true;

            label16.Show();
            label17.Show();
            label18.Show();
            label22.Show();
            label23.Show();

            _clickTimes = 0;
            _listCubeResult.Clear();
            _reveal = new[] { false, false, false, false, false };
            var r = new Random();
            float weightTotal;
            List<float> weight;

            if (comboBox1.SelectedIndex == 0)
            {
                if ((float)r.NextDouble() < _silverRate)
                {
                    label30.Text = Resources.String_Silver;
                    label16.Text = Resources.String_Silver;
                    label17.Text = Resources.String_Silver;
                    label18.Text = Resources.String_Silver;
                    label22.Text = Resources.String_Silver;
                    label23.Text = Resources.String_Silver;
                    weightTotal = _weightTotalSilver;
                    weight = _weightSilver;
                    _cubeKind = 1;
                }
                else
                {
                    label30.Text = Resources.String_Basic;
                    label16.Text = Resources.String_Basic;
                    label17.Text = Resources.String_Basic;
                    label18.Text = Resources.String_Basic;
                    label22.Text = Resources.String_Basic;
                    label23.Text = Resources.String_Basic;
                    weightTotal = _weightTotalBasic;
                    weight = _weightBasic;
                    _cubeKind = 0;
                }
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                label30.Text = Resources.String_Silver;
                label16.Text = Resources.String_Silver;
                label17.Text = Resources.String_Silver;
                label18.Text = Resources.String_Silver;
                label22.Text = Resources.String_Silver;
                label23.Text = Resources.String_Silver;
                weightTotal = _weightTotalSilver;
                weight = _weightSilver;
                _cubeKind = 1;
            }
            else //if (comboBox1.SelectedIndex == 2)
            {
                label30.Text = Resources.String_Gold;
                label16.Text = Resources.String_Gold;
                label17.Text = Resources.String_Gold;
                label18.Text = Resources.String_Gold;
                label22.Text = Resources.String_Gold;
                label23.Text = Resources.String_Gold;
                weightTotal = _weightTotalGold;
                weight = _weightGold;
                _cubeKind = 2;
            }

            for (int i = 0; i < 5; i++)
            {
                Calculation calculation = TrData.Multi;
                float weightCurrent = calculation(weightTotal, (float)r.NextDouble());
                int index;
                calculation = TrData.Minus;
                for (index = 0; weightCurrent >= weight[index]; index++)
                {
                    weightCurrent = calculation(weightCurrent, weight[index]);
                }
                _listCubeResult.Add(index);
            }
        }

        private void label16_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            int tag = Convert.ToInt32(label.Tag.ToString());
            label.Hide();
            label.Enabled = false;
            _reveal[tag - 41] = true;
            Reveal(tag);
            GetLabel(tag).BorderStyle = BorderStyle.FixedSingle;
            if (_clickTimes > 2)
            {
                for (int index = 0; index < 5; index++)
                {
                    if (!_reveal[index])
                    {
                        GetLabel(-index - 41).Hide();
                        GetLabel(-index - 41).Enabled = false;
                        GetLabel(index + 41).BorderStyle = BorderStyle.None;
                        Reveal(index + 41);
                    }
                }
            }
        }
        #endregion

        #region 前线抽选
        private void button12_Click(object sender, EventArgs e)
        {
            // 使遮挡标签显示
            label27.Enabled = true;
            label28.Enabled = true;
            label29.Enabled = true;
            label27.Hide();
            label28.Hide();
            label29.Hide();
            label27.Enabled = false;
            label28.Enabled = false;
            label29.Enabled = false;

            _clickTimes = 0;
            _listCubeResult.Clear();
            _reveal = new[] { false, false, false, false, false };
            var r = new Random();
            float weightTotal = _weightTotalBasic;
            List<float> weight = _weightBasic;


            for (int i = 0; i < 3; i++)
            {
                Calculation calculation = TrData.Multi;
                float weightCurrent = calculation(weightTotal, (float)r.NextDouble());
                int index;
                calculation = TrData.Minus;
                for (index = 0; weightCurrent >= weight[index]; index++)
                {
                    weightCurrent = calculation(weightCurrent, weight[index]);
                }
                _listCubeResult.Add(index);
            }
            _cubeKind = 0;

            int index1 = 0;
            while (index1 < 3)
            {
                int i = r.Next(0, 3);
                if (!_reveal[i])
                {
                    GetLabel(-i - 51).Hide();
                    GetLabel(-i - 51).Enabled = false;
                    GetLabel(i + 51).BorderStyle = BorderStyle.None;
                    Reveal(i + 51);
                    _reveal[i] = true;
                    index1++;
                }
            }
            _reveal = new[] { false, false, false, false, false };
            _clickTimes = 0;
            button12.Enabled = false; // 防止生成多个后台任务
            backgroundWorker1.RunWorkerAsync();
            // 后续代码在后台任务完成方法内
        }

        private void label27_Click(object sender, EventArgs e)
        {
            Label label = sender as Label;
            int tag = Convert.ToInt32(label.Tag.ToString());
            label.Hide();
            label.Enabled = false;
            _reveal[tag - 51] = true;
            Reveal(tag);
            GetLabel(tag).BorderStyle = BorderStyle.FixedSingle;
            if (_clickTimes > 0)
            {
                for (int index = 0; index < 3; index++)
                {
                    if (!_reveal[index])
                    {
                        GetLabel(-index - 51).Hide();
                        GetLabel(-index - 51).Enabled = false;
                        GetLabel(index + 51).BorderStyle = BorderStyle.None;
                        Reveal(index + 51);
                    }
                }
            }
        }

        #endregion

        #region 抽选显示
        private void Reveal(int tag)
        {
            if (_cubeKind == 0)
            {
                GetpictureBox(tag).Image = _showDds(_picOffsetBasic[_listCubeResult[_clickTimes]], _picPkgNumBasic[_listCubeResult[_clickTimes]]);
                GetLabel(tag).Text = _memberNameBasic[_listCubeResult[_clickTimes]];
            }
            else if (_cubeKind == 1)
            {
                GetpictureBox(tag).Image = _showDds(_picOffsetSilver[_listCubeResult[_clickTimes]], _picPkgNumSilver[_listCubeResult[_clickTimes]]);
                GetLabel(tag).Text = _memberNameSilver[_listCubeResult[_clickTimes]];
            }
            else
            {
                GetpictureBox(tag).Image = _showDds(_picOffsetGold[_listCubeResult[_clickTimes]], _picPkgNumGold[_listCubeResult[_clickTimes]]);
                GetLabel(tag).Text = _memberNameGold[_listCubeResult[_clickTimes]];
            }
            _clickTimes++;
        }


        #endregion

        #region 前线抽选后台
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            Thread.Sleep(5000);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            // 使遮挡标签显示
            label27.Enabled = true;
            label28.Enabled = true;
            label29.Enabled = true;

            label27.Show();
            label28.Show();
            label29.Show();

            button12.Enabled = true;
        }

        #endregion

        private void label19_MouseDown(object sender, MouseEventArgs e)
        {
            _mPoint = new Point(e.X, e.Y);
        }

        private void label19_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Location = new Point(Location.X + e.X - _mPoint.X, Location.Y + e.Y - _mPoint.Y);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            boxBan = TrData.BoxBan;
            bool flag = boxBan(_itemNum);
            if (flag)
            {
                label33.Text = "已将本箱子放入黑名单";
                button13.Enabled = false;
            }
            else
            {
                label33.Text = "出现错误 请重试";
            }
        }
    }
}
