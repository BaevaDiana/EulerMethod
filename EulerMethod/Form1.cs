using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;


namespace EulerMethod
{
    public partial class Form1 : Form
    {
        public int n;
        public string varPoint = "0";

        ZedGraphControl zedGrapgControl1 = new ZedGraphControl();
        public Form1()
        {
            InitializeComponent();
        }
        protected override void OnLoad(EventArgs e)
        {
            zedGrapgControl1.Location = new Point(10, 30);
            zedGrapgControl1.Name = "text";
            zedGrapgControl1.Size = new Size(500, 500);
            Controls.Add(zedGrapgControl1);
            GraphPane my_Pane = zedGrapgControl1.GraphPane;
            my_Pane.Title.Text = "Результат:";
            my_Pane.XAxis.Title.Text = "X";
            my_Pane.YAxis.Title.Text = "Y";
        }
        private void GetSize()
        {
            zedGrapgControl1.Location = new Point(10, 10);
            zedGrapgControl1.Size = new Size(ClientRectangle.Width - 20, ClientRectangle.Height - 20);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            GetSize();
        }
        static double f1(double x, double y)//Исходное дифференциальное уравнение
        {
            return ((2 * x * Math.Pow(y, -2) * Math.Pow(Math.E, -2 * x * x)) - (2 * x * y)) / 3;
        }
        static double f2(double x)//Точное решение задачи Коши 
        {
            double pw = ((0 * Math.Pow(Math.E, Math.Pow(x, 2))) - 1);
            return ((-1) / (Math.Pow(Math.E, ((2 * Math.Pow(x, 2)) / 3))));
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            Clear(zedGrapgControl1);
            GriddenOn(zedGrapgControl1.GraphPane);
        }
        private void Clear(ZedGraphControl Zed_GraphControl)
        {
            //GraphPane pane = Zed_GraphControl.GraphPane;
            zedGrapgControl1.GraphPane.CurveList.Clear();
            zedGrapgControl1.GraphPane.GraphObjList.Clear();

            zedGrapgControl1.GraphPane.XAxis.Type = AxisType.Linear;
            zedGrapgControl1.GraphPane.XAxis.Scale.TextLabels = null;
            zedGrapgControl1.GraphPane.XAxis.MajorGrid.IsVisible = false;
            zedGrapgControl1.GraphPane.YAxis.MajorGrid.IsVisible = false;
            zedGrapgControl1.GraphPane.YAxis.MinorGrid.IsVisible = false;
            zedGrapgControl1.GraphPane.XAxis.MinorGrid.IsVisible = false;
            zedGrapgControl1.RestoreScale(zedGrapgControl1.GraphPane);

            zedGrapgControl1.AxisChange();
            zedGrapgControl1.Invalidate();
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                varPoint = "0";
                MessageBox.Show("Введите N:");
            }
            else
            {
                varPoint = "1";
                n = Convert.ToInt32(textBox1.Text);
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void Eiler(ZedGraphControl Zed_GraphControl)//сам метод ломанных Эйлера
        {
            GraphPane my_Pane = Zed_GraphControl.GraphPane;
            PointPairList list = new PointPairList();
            try
            {
                double maxNev, curNev, h, x, y;
                int i;
                maxNev = 0; curNev = maxNev;
                x = 0; y = -1;
                h = (double)(1 - 0) / n;
                for (i = 0; i < n + 1; i++)
                {
                    list.Add(x, y);
                    curNev = Math.Abs((y + h * f1(x, y)) - f2(x));
                    if (curNev > maxNev)
                    {
                        maxNev = curNev;

                    }
                    y = y + h * f1(x, y);
                    x += h;
                }
                PointPairList listMIN = new PointPairList();
                PointPairList listMAX = new PointPairList();

                LineItem d1 = my_Pane.AddCurve("Результат метода Эйлера", list, Color.Blue, SymbolType.None);
                textBox2.Text = maxNev.ToString();
                zedGrapgControl1.AxisChange();
                zedGrapgControl1.Invalidate();
            }
            catch
            {
                MessageBox.Show("Некорректный ввод данных.");
            }
        }
        private void GriddenOn(GraphPane my_Pane)
        {
            //GraphPane my_Pane = Zed_GraphControl.GraphPane;
            my_Pane.XAxis.MajorGrid.IsVisible = true;
            my_Pane.YAxis.MajorGrid.IsVisible = true;
            my_Pane.YAxis.MinorGrid.IsVisible = true;
            my_Pane.XAxis.MinorGrid.IsVisible = true;
        }
        private void BTN_EM(object sender, EventArgs e)
        {
            GriddenOn(zedGrapgControl1.GraphPane);
            Eiler(zedGrapgControl1);
        }
        private void Rez(ZedGraphControl Zed_GraphControl)//Построение графика точного решения
        {
            double a, b, h1; int i;
            a = 0; b = 1;
            h1 = (double)(b - a) / 100D;
            double[] coorX = new double[100 + 1];
            double[] coorY = new double[100 + 1];
            GraphPane my_Pane = Zed_GraphControl.GraphPane;
            PointPairList list2 = new PointPairList();
            for (i = 0; i < 100 + 1; i++)
            {
                coorX[i] = a + i * h1;
                coorY[i] = f2(coorX[i]);
                list2.Add(coorX[i], coorY[i]);
            }
            LineItem myCircle = my_Pane.AddCurve("Точное решение", list2, Color.Red, SymbolType.None);
            zedGrapgControl1.AxisChange();
            zedGrapgControl1.Invalidate();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            GriddenOn(zedGrapgControl1.GraphPane);
            Eiler(zedGrapgControl1);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            GriddenOn(zedGrapgControl1.GraphPane);
            Rez(zedGrapgControl1);

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
