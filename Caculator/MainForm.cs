using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using ShadowDemo;

namespace Caculator
{
    public partial class MainForm : Form
    {


        //Code Layout {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;


        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        public MainForm()
        {
            InitializeComponent();
            strBuider = new StringBuilder("0");
            exprestionBuilder = new StringBuilder();

        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= 0x00020000; // This form has to have the WS_EX_LAYERED extended style
                return cp;
            }
        }


        private void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            var panel = sender as TableLayoutPanel;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            var rectangle = e.CellBounds;

            using (var pen = new Pen(Color.FromArgb(30, 120, 255), 1))
            {
                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Center;
                pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;

                if (e.Row == (panel.RowCount - 1))
                {
                    rectangle.Height -= 1;
                }

                if (e.Column == (panel.ColumnCount - 1))
                {
                    rectangle.Width -= 1;
                }

                e.Graphics.DrawRectangle(pen, rectangle);
            }


        }


        private void Form1_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {

                Dropshadow shadow = new Dropshadow(this)
                {
                    ShadowBlur = 0,
                    ShadowSpread = 1,
                    ShadowColor = Color.FromArgb(140, 140, 140)

                };
                shadow.RefreshShadow();

            }
        }

        private void tableLayoutPanel3_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            Rectangle rect = new Rectangle(0, 0, panel.Width, panel.Height);
            LinearGradientBrush linGrBrush = new LinearGradientBrush(rect, Color.FromArgb(255, 211, 211, 211),
            Color.FromArgb(255, 0, 0, 0)
            , 90);
            e.Graphics.FillRectangle(linGrBrush, rect);
        }

        private void bt_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void bt_minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        // }


        //Code control {
        public const String _SUM = "sum";
        public const String _MINUS = "minus";
        public const String _MULTIPLICATION = "multiplication";
        public const String _DIVISION = "division";

        public StringBuilder strBuider;
        public StringBuilder exprestionBuilder;
        public String mOperator;
        public bool isOperator = false;
        double a = 0, b = 0;

        private void bt_clear_Click(object sender, EventArgs e)
        {
            a = 0;
            b = 0;
            isOperator = false;
            exprestionBuilder.Clear();
            updateExpression();
            setResultZero();
            clearStringBuilder();
        }

        private void bt_sqrt_Click(object sender, EventArgs e)
        {

        }

        private void bt_pow_Click(object sender, EventArgs e)
        {

        }

        private void bt_1_x_Click(object sender, EventArgs e)
        {

        }

        private void bt_percent_Click(object sender, EventArgs e)
        {

        }

        private void bt_clear_entry_Click(object sender, EventArgs e)
        {
            setResultZero();
            clearStringBuilder();
        }

        private void bt_bspc_Click(object sender, EventArgs e)
        {
            if (strBuider.Length > 0)
            {
                strBuider.Remove(strBuider.Length - 1, 1);
                updateResult();
                if (strBuider.Length == 0)
                {
                    clearStringBuilder();
                    setResultZero();
                }
            }


        }

        public void AppendOperator(bool isOper, String _operator)
        {
            if (isOper && !isFirstOperator)
            {
                exprestionBuilder.Remove(exprestionBuilder.Length - 3, 3);
                Append(_operator);
            }
            else
            {
                exprestionBuilder.Append(b);
                Append(_operator);
                updateResult();
                clearStringBuilder();
                isOperator = true;
            }
        }
        public void Append(String _operator)
        {

            switch (_operator)
            {
                case _SUM:
                    exprestionBuilder.Append(" + ");
                    break;
                case _MINUS:
                    exprestionBuilder.Append(" － ");
                    break;
                case _MULTIPLICATION:
                    exprestionBuilder.Append(" ✕ ");
                    break;
                case _DIVISION:
                    exprestionBuilder.Append(" ÷ ");
                    break;
                default:
                    return;
            }
        }
        // Bốn toán tử + - * /{
        bool isFirstOperator = true;
        private void bt_divis_Click(object sender, EventArgs e)
        {
            mOperator = _DIVISION;
            isOperator = true;
            if (isEqualClicked && !isOperator)
                b = a;
            AppendOperator(isOperator, _DIVISION);
            updateExpression();
        }

        private void bt_multi_Click(object sender, EventArgs e)
        {
            mOperator = _MULTIPLICATION;
            isOperator = true;
            if (isEqualClicked && !isOperator)
                b = a;
            AppendOperator(isOperator, _MULTIPLICATION);
            updateExpression();
        }

        private void bt_minus_Click(object sender, EventArgs e)
        {
            mOperator = _MINUS;
            isOperator = true;
            if (isEqualClicked && !isOperator)
                b = a;
            AppendOperator(isOperator, _MINUS);
            updateExpression();
        }

        private void bt_sum_Click(object sender, EventArgs e)
        {
            mOperator = _SUM;
            isOperator = true;
            if (isEqualClicked && !isOperator)
                b = a;
            AppendOperator(isOperator, _SUM);
            updateExpression();
        }
        // }
        bool isFirst = true;
        bool isEqualClicked = false;
        private void bt_equal_Click(object sender, EventArgs e)
        {
            isEqualClicked = true;
            resetExpression();
            updateExpression();
            if (!String.IsNullOrEmpty(mOperator))
            {
                if (isFirst)
                {
                    a = b;
                    a = ExcuteCalculate(a, b, mOperator);
                    lb_result.Text = a.ToString();
                }
                else
                {
                    a = ExcuteCalculate(a, b, mOperator);
                    lb_result.Text = a.ToString();
                }
            }
        }
        private void bt_random_Click(object sender, EventArgs e)
        {
            lb_result.Text = new Random().Next(0, 1000).ToString();
            clearStringBuilder();

        }
        private void bt_dot_Click(object sender, EventArgs e)
        {

        }
        private void bt_ct_Click(object sender, EventArgs e)
        {

        }
        // 10 number{
        private void bt_9_Click(object sender, EventArgs e)
        {
            stringBuilderAppend(bt_9.Text);
        }
        private void bt_8_Click(object sender, EventArgs e)
        {
            stringBuilderAppend(bt_8.Text);
        }
        private void bt_7_Click(object sender, EventArgs e)
        {
            stringBuilderAppend(bt_7.Text);
        }
        private void bt_6_Click(object sender, EventArgs e)
        {
            stringBuilderAppend(bt_6.Text);
        }
        private void bt_5_Click(object sender, EventArgs e)
        {
            stringBuilderAppend(bt_5.Text);
        }
        private void bt_4_Click(object sender, EventArgs e)
        {
            stringBuilderAppend(bt_4.Text);
        }
        private void bt_3_Click(object sender, EventArgs e)
        {
            stringBuilderAppend(bt_3.Text);
        }
        private void bt_2_Click(object sender, EventArgs e)
        {
            stringBuilderAppend(bt_2.Text);
        }
        private void bt_1_Click(object sender, EventArgs e)
        {
            stringBuilderAppend(bt_1.Text);
        }
        private void bt_0_Click(object sender, EventArgs e)
        {
            stringBuilderAppend(bt_0.Text);
        }
        // }

        public void stringBuilderAppend(String number)
        {
            if (isOperator)
            {
                strBuider.Clear();
                strBuider.Append(number);
                updateResult();
                isOperator = false;
            }
            else
            {
                if (b == 0)
                {
                    strBuider.Clear();
                    strBuider.Append(number);
                    updateResult();
                }
                else
                {
                    strBuider.Append(number);
                    updateResult();
                }
            }
        }
        public void updateResult()
        {
            lb_result.Text = strBuider.ToString();
            b = Double.Parse(strBuider.ToString());
        }
        public void updateExpression()
        {
            lb_expresstion.Text = exprestionBuilder.ToString();
        }
        public void setResultZero()
        {
            lb_result.Text = "0";
        }
        public void clearStringBuilder()
        {
            strBuider = new StringBuilder("0");
        }
        public void resetExpression()
        {
            exprestionBuilder.Clear();
            isOperator = false;
        }

        public double ExcuteCalculate(double a, double b, String expression)
        {
            isFirst = false;
            switch (expression)
            {
                case _SUM:
                    return a + b;
                case _MINUS:
                    return a - b;
                case _MULTIPLICATION:
                    return a * b;
                case _DIVISION:
                    return a / b;
                default:
                    return 0;
            }
        }
    }
}
