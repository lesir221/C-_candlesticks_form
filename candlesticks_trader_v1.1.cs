using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test_Nplot
{
    public partial class candlesticks_trader_v1 : Form
    {
        //formsplot
        public Dictionary<int, ScottPlot.FormsPlot> formsplot = new Dictionary<int, ScottPlot.FormsPlot>();
        public Dictionary<int, ScottPlot.Plottable.FinancePlot> candlesticks = new Dictionary<int, ScottPlot.Plottable.FinancePlot>();
        private ScottPlot.Plottable.Annotation annotation;
        private ScottPlot.Plottable.Annotation annotation2;
        public ScottPlot.Plottable.VLine vvline;
        public ScottPlot.Plottable.HLine hhline;
        public Dictionary<int, ScottPlot.Plottable.VLine> vLine = new Dictionary<int, ScottPlot.Plottable.VLine>();
        public Dictionary<int, ScottPlot.Plottable.HLine> hLine = new Dictionary<int, ScottPlot.Plottable.HLine>();
        //private ScottPlot.Plottable.VLine vLine;
        //private ScottPlot.Plottable.HLine hLine;

        //datetimepicker
        DateTime date;
        string selectdate, selectdate_to;

        //data_set
        public Dictionary<int, List<ScottPlot.OHLC>> all_m = new Dictionary<int, List<ScottPlot.OHLC>>();
        public Dictionary<int, List<ScottPlot.OHLC>> pre_m = new Dictionary<int, List<ScottPlot.OHLC>>();
        public Dictionary<int, List<ScottPlot.OHLC>> next_m = new Dictionary<int, List<ScottPlot.OHLC>>();
        public Dictionary<int, List<ScottPlot.OHLC>> next_m_for = new Dictionary<int, List<ScottPlot.OHLC>>();

        //ma
        public Dictionary<(int, int), (List<double>, List<double>)> _ma = new Dictionary<(int, int), (List<double>, List<double>)>();
        public Dictionary<(int, int), (List<double>, List<double>)> ma_ = new Dictionary<(int, int), (List<double>, List<double>)>();
        public Dictionary<(int, int), (List<double>, List<double>)> ma = new Dictionary<(int, int), (List<double>, List<double>)>();
        public Dictionary<int, double> mv_add = new Dictionary<int, double>();
        public Dictionary<int, (double[], double[], double[])> fill = new Dictionary<int, (double[], double[], double[])>();

        //data_plus
        //
        public Dictionary<int, List<ScottPlot.OHLC>> nex_1m_for = new Dictionary<int, List<ScottPlot.OHLC>>();
        //
        public Dictionary<int, double> m_h = new Dictionary<int, double>();
        public Dictionary<int, double> m_l = new Dictionary<int, double>();


        //ploygon
        public Dictionary<int, ScottPlot.Plottable.Polygon> polygon = new Dictionary<int, ScottPlot.Plottable.Polygon>();


        //next_bt
        public int next_bt_count = 0;
        public int a = 0;

        //trade
        public int count_L, count_S = 0;
        public double combine_L, combine_S, avg_L, avg_S = 0;
        public double profit_L, profit_S, profit_T = 0;
        //stop
        public Dictionary<string, List<(double, double)>> stop = new Dictionary<string, List<(double, double)>>();
        public int L_stop_profit;
        public int L_stop_lose;
        public int S_stop_profit;
        public int S_stop_lose;

        //marker_trade_signal
        //private ScottPlot.Plottable.DraggableMarkerPlot currentLing;
        public Dictionary<(int, int), ScottPlot.Plottable.DraggableMarkerPlot> currentLing = new Dictionary<(int, int), ScottPlot.Plottable.DraggableMarkerPlot>();
        public int listbox_1_count, listbox_2_count, listbox_3_count = 0;

        //formsplot_drop
        public ScottPlot.Plottable.ScatterPlot dif_line1;
        public ScottPlot.Plottable.ScatterPlot dif_line2;
        public ScottPlot.Plottable.ScatterPlot dif_line3;
        public ScottPlot.Plottable.Annotation dif_anno1;
        public ScottPlot.Plottable.Annotation dif_anno2;
        public ScottPlot.Plottable.Annotation dif_anno3;
        public (double X, double Y) pointA;
        public (double X, double Y) pointB;
        public bool isDragging = false; // is mouse drag?


        public candlesticks_trader_v1()
        {
            InitializeComponent();

            //date
            date = dateTimePicker1.Value;
            Console.WriteLine(date);
            //scottplot_set
            formsplot[1] = formsPlot1;
            formsplot[2] = formsPlot2;
            formsplot[3] = formsPlot3;
            formsPlot1.Name = "1";
            formsPlot2.Name = "2";
            formsPlot3.Name = "3";

            // stop mouse 
            for (int i = 1; i <= 3; i++)
            {
                formsplot[i].Configuration.LeftClickDragPan = false;
                formsplot[i].Configuration.RightClickDragZoom = false;
                formsplot[i].RightClicked -= formsplot[i].DefaultRightClickEvent;
            }

            //set issue
            this.listBox_longtime.SelectedIndexChanged += new System.EventHandler(this.ListBox1_SelectedIndexChanged);
            this.listBox_shorttime.SelectedIndexChanged += new System.EventHandler(this.ListBox2_SelectedIndexChanged);

            formsPlot1.MouseMove += new System.Windows.Forms.MouseEventHandler(Plot_MouseMove);
            formsPlot1.MouseLeave += new System.EventHandler(Plot_MouseLeave);
            formsPlot2.MouseMove += new System.Windows.Forms.MouseEventHandler(Plot_MouseMove);
            formsPlot2.MouseLeave += new System.EventHandler(Plot_MouseLeave);
            formsPlot3.MouseMove += new System.Windows.Forms.MouseEventHandler(Plot_MouseMove);
            formsPlot3.MouseLeave += new System.EventHandler(Plot_MouseLeave);

            //set drag
            formsPlot1.MouseDown += FormsPlot_MouseDown;
            formsPlot1.MouseUp += FormsPlot_MouseUp;
            formsPlot2.MouseDown += FormsPlot_MouseDown;
            formsPlot2.MouseUp += FormsPlot_MouseUp;
            formsPlot3.MouseDown += FormsPlot_MouseDown;
            formsPlot3.MouseUp += FormsPlot_MouseUp;

            //Set keypress
            //this.AcceptButton = next_bt;
            this.KeyPreview = true; // 這樣表單就能預覽鍵盤事件
            this.KeyDown += new KeyEventHandler(MyForm_KeyDown); // 給表單添加 KeyDown 事件處理程序



            //set_stop
            stop["L"] = new List<(double, double)>();
            stop["S"] = new List<(double, double)>();
            stop["P_L"] = new List<(double, double)>();
            stop["P_S"] = new List<(double, double)>();


            //十字線
            for (int i = 1; i <= 3; i++)
            {
                vLine[i] = formsplot[i].Plot.AddVerticalLine(0);
                hLine[i] = formsplot[i].Plot.AddHorizontalLine(0);
            }

            for (int i = 1; i <= 3; i++)
            {
                formsplot[i].Plot.Style(dataBackground: System.Drawing.Color.Black);
                formsplot[i].Plot.Grid(color: Color.DimGray);

            }
 

            double[] x_tick = { 839, 855, 915, 975, 1139 };
            string[] time = { "05:00", "09:00", "10:00", "11:00", "13:45" };
            formsplot[1].Plot.XAxis.ManualTickPositions(x_tick, time);
            double[] x_tick2 = { 279, 285, 305, 325, 379 };
            formsplot[2].Plot.XAxis.ManualTickPositions(x_tick2, time);
            double[] x_tick3 = { 167, 171, 183, 195, 227 };
            formsplot[3].Plot.XAxis.ManualTickPositions(x_tick3, time);
        }

        private void candlesticks_trader_v1_Load(object sender, EventArgs e)
        {
            //this.Size = new Size(1800,1600);
            this.WindowState = FormWindowState.Maximized;

            int top = 30;
            int pointx = 5;
            int pointy = 10;
            int width = 600;
            int height = 500;

            //timespicker
            dateTimePicker1.Location = new Point(1400, 650);
            date_next.Location = new Point(1610, 650);
            date_pre.Location = new Point(1350, 650);

            //form_set
            if (true)
            {
                //formsplot1
                formsPlot1.Width = width;
                formsPlot1.Height = height;
                formsPlot1.Location = new Point(pointx, pointy);

                //formsplot2
                formsPlot2.Width = width;
                formsPlot2.Height = height;
                formsPlot2.Location = new Point(pointx * 2 + width, pointy);

                //formsplot3
                formsPlot3.Width = width;
                formsPlot3.Height = height;
                formsPlot3.Location = new Point(pointx * 3 + width * 2, pointy);

                //hscrollbar
                hScrollBar1.Size = new Size(600, hScrollBar1.Height);
                hScrollBar1.Location = new Point(pointx, pointy + height + 20);
                hScrollBar2.Size = new Size(600, hScrollBar2.Height);
                hScrollBar2.Location = new Point(pointx * 2 + width, pointy + height + 20);
                hScrollBar3.Size = new Size(600, hScrollBar3.Height);
                hScrollBar3.Location = new Point(pointx * 3 + width * 2, pointy + height + 20);
                hScrollBar4.Size = new Size(1800, hScrollBar4.Height);
                hScrollBar4.Location = new Point(pointx, pointy + height + 60);


            }
            // lable
            if (true)
            {
                //lable
                label1.Location = new Point(50, 600);
                label2.Location = new Point(500, 600);
                label3.Location = new Point(950, 600);

                label4.Location = new Point(250, 600);
                label5.Location = new Point(700, 600);

                label1.Text = "Long : ";
                label2.Text = "Short : ";
                label3.Text = "Total : ";
                label4.Text = " 0 ";
                label5.Text = " 0 ";

            }

            // viewlist
            if (true)
            {
                //lable

                listBox_long.Location = new Point(50, 630);
                listBox_longtime.Location = new Point(240, 630);

                listBox_short.Location = new Point(500, 630);
                listBox_shorttime.Location = new Point(690, 630);

                listBox_profit.Location = new Point(950, 630);

                listBox_Lstop.Location = new Point(50, 900);
                listBox_Sstop.Location = new Point(500, 900);

            }
            //button
            if (true)
            {


                next_bt.Location = new Point(1650, 700);

                long_bt.Location = new Point(1350, 700);
                long_s_bt.Location = new Point(1500, 700);

                short_bt.Location = new Point(1350, 780);
                short_s_bt.Location = new Point(1500, 780);

                singal_view.Location = new Point(1350, 860);
                signal_clear.Location = new Point(1500, 860);
                png_out.Location = new Point(1650, 860);

                list_out.Location = new Point(950, 950);
                list_in.Location = new Point(1070, 950);



            }
            //set_stop
            if (true)
            {

                long_stop_profit.Location = new Point(50, 950);
                long_stop_loss.Location = new Point(180, 950);
                set_Lstop.Location = new Point(300, 950);
                Lstop_clear.Location = new Point(300, 990);

                short_stop_profit.Location = new Point(500, 950);
                short_stop_loss.Location = new Point(630, 950);
                set_Sstop.Location = new Point(750, 950);
                Sstop_clear.Location = new Point(750, 990);

            }


        }


        private void date_pre_Click(object sender, EventArgs e)
        {
            date = date.AddDays(-1);
            dateTimePicker1.Value = date;
            selectdate_to = date.ToString("yyyy_M_d");
            star();
        }

        private void date_next_Click(object sender, EventArgs e)
        {
            date = date.AddDays(1);
            dateTimePicker1.Value = date;
            selectdate_to = date.ToString("yyyy_M_d");
            star();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            //很重要 > 日期格式是 yyyy/MM/dd
            date = dateTimePicker1.Value;
            //selectdate = date.ToString("yyyy/MM/dd");
            //selectdate_to = dateTimePicker1.Value.ToString("yyyy_MM_dd");
            selectdate_to = date.ToString("yyyy_M_d");

            star();


        }

        public void star()
        {
            try
            {
                data_clear();

                data_set();
                plot_add();
            }
            catch
            {

            }
        }


        ///<methon>
        ///
        public void plot_add()
        {

            for (int i = 1; i <= 3; i++)
            {
                vLine[i] = formsplot[i].Plot.AddVerticalLine(0);
                hLine[i] = formsplot[i].Plot.AddHorizontalLine(0);
            }

            for (int i = 1; i <= 3; i++)
            {
                Console.WriteLine(pre_m[i].Count);
                //candlesticks
                candlesticks[i] = formsplot[i].Plot.AddCandlesticks(pre_m[i].ToArray());
                candlesticks[i].ColorUp = System.Drawing.Color.Red;
                candlesticks[i].ColorDown = System.Drawing.Color.White;

                //scatter
                formsplot[i].Plot.AddScatter(_ma[(i, 20)].Item1.ToArray(), _ma[(i, 20)].Item2.ToArray(), lineWidth: 2, markerSize: 0);
                formsplot[i].Plot.AddScatter(_ma[(i, 120)].Item1.ToArray(), _ma[(i, 120)].Item2.ToArray(), lineWidth: 2, markerSize: 0);
                //polygon
                //polygon[i] = formsplot[i].Plot.AddPolygon(fill[i].Item1.Concat(fill[i].Item1.Reverse()).ToArray(), fill[i].Item2.Concat(fill[i].Item3.Reverse()).ToArray());

                //show_x_stick  
                plot_x_set(i, next_bt_count);
                //show_y_stick  
                plot_y_set(i, next_bt_count);

                //refresh
                //formsplot[i].Plot.AxisAuto();
                //formsplot[i].Plot.AxisAutoY();
                formsplot[i].Refresh();
            }

            // Console.WriteLine(pre_m[1].Skip(690).Take(150).ToArray().Max());

        }

        public void plot_x_set(int i, int count)
        {
            //if(next_bt_count == 0)
            //{
            if (i == 1) { formsplot[1].Plot.SetAxisLimitsX(760 + count, 840 + 0.5 + count); }
            if (i == 2) { formsplot[2].Plot.SetAxisLimitsX(230 + count / 3, 280 + 0.5 + count / 3); }
            if (i == 3) { formsplot[3].Plot.SetAxisLimitsX(138 + count / 5, 168 + 0.5 + count / 5); }

            //}
            if (next_bt_count != 0)
            {


            }
        }

        public void plot_y_set(int i, int count)
        {
            int skip = 0;
            int take = 0;
            int to_ = 19;

            List<double> high = new List<double>();
            List<double> low = new List<double>();
            List<ScottPlot.OHLC> pre = new List<ScottPlot.OHLC>();
            //if (next_bt_count == 0)
            //{ 
            var mv_20 = _ma[(i, 20)].Item1.Zip(_ma[(i, 20)].Item2, (x, y) => (x, y));
            var mv_120 = _ma[(i, 120)].Item1.Zip(_ma[(i, 120)].Item2, (x, y) => (x, y));

            //抓取ma20 ma120 前&後 的值
            if (i == 1)
            {
                to_ = 671 + count;
                skip = 690 + count;
                take = 150;
                mv_add[1] = mv_20.FirstOrDefault(point => point.x == to_).y;
                mv_add[2] = mv_20.FirstOrDefault(point => point.x == to_ + take).y;
                mv_add[3] = mv_120.FirstOrDefault(point => point.x == to_).y;
                mv_add[4] = mv_120.FirstOrDefault(point => point.x == to_ + take).y;
            }
            if (i == 2)
            {
                to_ = 221 + count / 3;
                skip = 240 + count / 3;
                take = 50;
                mv_add[1] = mv_20.FirstOrDefault(point => point.x == to_).y;
                mv_add[2] = mv_20.FirstOrDefault(point => point.x == to_ + take).y;
                mv_add[3] = mv_120.FirstOrDefault(point => point.x == to_).y;
                mv_add[4] = mv_120.FirstOrDefault(point => point.x == to_ + take).y;
            }
            if (i == 3)
            {
                to_ = 119 + count / 5;
                skip = 138 + count / 5;
                take = 30;
                mv_add[1] = mv_20.FirstOrDefault(point => point.x == to_).y;
                mv_add[2] = mv_20.FirstOrDefault(point => point.x == to_ + take).y;
                mv_add[3] = mv_120.FirstOrDefault(point => point.x == to_).y;
                mv_add[4] = mv_120.FirstOrDefault(point => point.x == to_ + take).y;
            }

            //將cnadlesticks & ma20 ma120 高低都加入對比
            //    foreach (var j in pre_m[i].Skip(skip - (i-1) * take).Take(i * take).ToArray())
            foreach (var j in pre_m[i].Skip(skip).Take(take + 1).ToArray())
            {
                high.Add(j.High);
                low.Add(j.Low);
            }
            foreach (var j in mv_add.Values)
            {
                high.Add(j);
                low.Add(j); ;
            }

            formsplot[i].Plot.SetAxisLimitsY(low.Min() * 0.9999, high.Max() * 1.0001);

            //}
            if (next_bt_count != 0)
            {



            }



            //return (high.Max(),low.Min());

        }


        public void data_clear()
        {
            formsplot[1].Plot.Clear();
            formsplot[2].Plot.Clear();
            formsplot[3].Plot.Clear();

            listBox_long.Items.Clear();
            listBox_longtime.Items.Clear();
            listBox_short.Items.Clear();
            listBox_shorttime.Items.Clear();
            listBox_profit.Items.Clear();

            listBox_Lstop.Items.Clear();
            listBox_Sstop.Items.Clear();

            //trade_table
            count_L = 0;
            count_S = 0;
            combine_L = 0;
            combine_S = 0;
            avg_L = 0;
            avg_S = 0;
            profit_L = 0;
            profit_S = 0;
            profit_T = 0;
            label1.Text = "Long : ";
            label2.Text = "Short : ";
            label3.Text = "Total : ";
            label4.Text = count_L.ToString();
            label5.Text = count_S.ToString();

            //marker
            listbox_1_count = 0;
            listbox_2_count = 0;
            listbox_3_count = 0;

            next_bt_count = 0;
            a = 0;
        }

        public void data_set()
        {

            try
            {
                // 1=m1 2=m3 3=m5
                (all_m[1], pre_m[1], next_m[1], ma[(1, 20)], ma[(1, 120)], _ma[(1, 20)], ma_[(1, 20)], _ma[(1, 120)], ma_[(1, 120)], fill[1]) = load_csv(selectdate_to, "m1");
                (all_m[2], pre_m[2], next_m[2], ma[(2, 20)], ma[(2, 120)], _ma[(2, 20)], ma_[(2, 20)], _ma[(2, 120)], ma_[(2, 120)], fill[2]) = load_csv(selectdate_to, "m3");
                (all_m[3], pre_m[3], next_m[3], ma[(3, 20)], ma[(3, 120)], _ma[(3, 20)], ma_[(3, 20)], _ma[(3, 120)], ma_[(3, 120)], fill[3]) = load_csv(selectdate_to, "m5");


                next_m_for[1] = new List<ScottPlot.OHLC>();
                next_m_for[2] = new List<ScottPlot.OHLC>();
                next_m_for[3] = new List<ScottPlot.OHLC>();
                copy_1m();

            }
            catch
            {

            }
            //Console.WriteLine(selectdate_to);
            //Console.WriteLine(pre_m[1].Count);

        }
        public void copy_1m()
        {
            for (int i = 1; i <= 3; i++)
            {
                int cnt = 0;
                foreach (var j in next_m[1].ToArray())
                {
                    next_m_for[i].Add
                    (
                        new ScottPlot.OHLC
                        (
                            j.Open,
                            j.High,
                            j.Low,
                            j.Close,
                            cnt,
                            1
                        )
                    );
                    cnt++;
                }
                Console.WriteLine(next_m_for[i]);
            }

        }


        public (List<ScottPlot.OHLC>, List<ScottPlot.OHLC>, List<ScottPlot.OHLC>, (List<double>, List<double>), (List<double>, List<double>), (List<double>, List<double>), (List<double>, List<double>), (List<double>, List<double>), (List<double>, List<double>), (double[], double[], double[])) load_csv(string date, string period)
        {

            //list<OHLC>
            List<ScottPlot.OHLC> table1 = new List<ScottPlot.OHLC>();
            List<ScottPlot.OHLC> table2 = new List<ScottPlot.OHLC>();
            List<ScottPlot.OHLC> table3 = new List<ScottPlot.OHLC>();
            //ma
            Dictionary<int, (double[], double[])> mv = new Dictionary<int, (double[], double[])>();
            List<double> mv20_x = new List<double>();
            List<double> mv20_y = new List<double>();
            List<double> _mv20_x = new List<double>();
            List<double> _mv20_y = new List<double>();
            List<double> mv20_x_ = new List<double>();
            List<double> mv20_y_ = new List<double>();

            List<double> mv120_x = new List<double>();
            List<double> mv120_y = new List<double>();
            List<double> _mv120_x = new List<double>();
            List<double> _mv120_y = new List<double>();
            List<double> mv120_x_ = new List<double>();
            List<double> mv120_y_ = new List<double>();

            //fill_polygan
            List<double> fill_x = new List<double>();
            List<double> fill_y1 = new List<double>();
            List<double> fill_y2 = new List<double>();

            // Read the CSV file
            var ohlc_lines = File.ReadAllLines("E:\\期貨 外匯模擬\\TX_RTP\\2023\\grap\\all_date\\" + period + "\\" + date + ".csv");
            var ohlc_org = ohlc_lines.Skip(1).ToArray();
            //Console.WriteLine("E:\\期貨 外匯模擬\\TX_RTP\\2023\\grap\\all_date\\" + period + "\\" + date + ".csv");

            int i = 0;
            int a;
            foreach (var line in ohlc_org)
            {
                table1.Add
                (
                    new ScottPlot.OHLC
                    (
                        double.Parse(line.Split(',')[2]),
                        double.Parse(line.Split(',')[3]),
                        double.Parse(line.Split(',')[4]),
                        double.Parse(line.Split(',')[5]),
                        (double)i,
                        1
                    )
                );

                ////ma20 & ma120
                if (line.Split(',')[6] != "")
                {
                    mv20_x.Add(i);
                    mv20_y.Add(double.Parse(line.Split(',')[6]));
                }
                if (line.Split(',')[7] != "")
                {
                    mv120_x.Add(i);
                    mv120_y.Add(double.Parse(line.Split(',')[7]));
                }

                //polygon
                if (period == "m1")
                {
                    if (i >= 750 & i <= 900)
                    {
                        fill_x.Add(i);
                        fill_y1.Add(double.Parse(line.Split(',')[6]));
                        fill_y2.Add(double.Parse(line.Split(',')[7]));
                    }
                }
                if (period == "m3")
                {
                    if (i >= 250 & i <= 300)
                    {
                        fill_x.Add(i);
                        fill_y1.Add(double.Parse(line.Split(',')[6]));
                        fill_y2.Add(double.Parse(line.Split(',')[7]));
                    }
                }
                if (period == "m5")
                {
                    if (i >= 150 & i <= 180)
                    {
                        fill_x.Add(i);
                        fill_y1.Add(double.Parse(line.Split(',')[6]));
                        fill_y2.Add(double.Parse(line.Split(',')[7]));
                    }
                }


                i++;
            }

            // _ma ma_
            if (true)
            {
                if (period == "m1")
                {
                    _mv20_x = mv20_x.Take(821).ToList();
                    _mv20_y = mv20_y.Take(821).ToList();
                    _mv120_x = mv120_x.Take(721).ToList();
                    _mv120_y = mv120_y.Take(721).ToList();
                    mv20_x_ = mv20_x.Skip(821).ToList();
                    mv20_y_ = mv20_y.Skip(821).ToList();
                    mv120_x_ = mv120_x.Skip(721).ToList();
                    mv120_y_ = mv120_y.Skip(721).ToList();
                }
                if (period == "m3")
                {
                    _mv20_x = mv20_x.Take(261).ToList();
                    _mv20_y = mv20_y.Take(261).ToList();
                    _mv120_x = mv120_x.Take(161).ToList();
                    _mv120_y = mv120_y.Take(161).ToList();
                    mv20_x_ = mv20_x.Skip(261).ToList();
                    mv20_y_ = mv20_y.Skip(261).ToList();
                    mv120_x_ = mv120_x.Skip(161).ToList();
                    mv120_y_ = mv120_y.Skip(161).ToList();
                }
                if (period == "m5")
                {
                    _mv20_x = mv20_x.Take(149).ToList();
                    _mv20_y = mv20_y.Take(149).ToList();
                    _mv120_x = mv120_x.Take(49).ToList();
                    _mv120_y = mv120_y.Take(49).ToList();
                    mv20_x_ = mv20_x.Skip(149).ToList();
                    mv20_y_ = mv20_y.Skip(149).ToList();
                    mv120_x_ = mv120_x.Skip(49).ToList();
                    mv120_y_ = mv120_y.Skip(49).ToList();
                }
            }

            //OHLC m1 m3 m5 
            if (true)
            {
                if (period == "m1")
                {
                    //cnadleticks
                    table2 = table1.Take(840).ToList();
                    table3 = table1.Skip(840).ToList();
                }
                if (period == "m3")
                {
                    table2 = table1.Take(280).ToList();
                    table3 = table1.Skip(280).ToList();
                }
                if (period == "m5")
                {
                    table2 = table1.Take(168).ToList();
                    table3 = table1.Skip(168).ToList();
                }
                //改table3
                //for (int k = 0; k < table3.Count; k++)
                //{
                //    table3[k].DateTime = DateTime.FromOADate(k);
                //}

            }

            return (table1, table2, table3, (mv20_x, mv20_y), (mv120_x, mv120_y), (_mv20_x, _mv20_y), (mv20_x_, mv20_y_), (_mv120_x, _mv120_y), (mv120_x_, mv120_y_), (fill_x.ToArray(), fill_y1.ToArray(), fill_y2.ToArray()));
            //Console.WriteLine(i);
        }


        public void data_plus()
        {
            //1m
            next_m_for[1][next_bt_count].DateTime = DateTime.FromOADate(pre_m[1].Count);
            pre_m[1].Add(next_m[1][next_bt_count]);
            _ma[(1, 20)].Item1.Add(ma_[(1, 20)].Item1[next_bt_count]);
            _ma[(1, 20)].Item2.Add(ma_[(1, 20)].Item2[next_bt_count]);
            _ma[(1, 120)].Item1.Add(ma_[(1, 120)].Item1[next_bt_count]);
            _ma[(1, 120)].Item2.Add(ma_[(1, 120)].Item2[next_bt_count]);

            ////3m
            //定3m開盤
            if (next_bt_count % 3 == 0)
            {
                //
                next_m_for[2][next_bt_count].DateTime = DateTime.FromOADate(pre_m[2].Count);
                pre_m[2].Add(next_m_for[2][next_bt_count]);

                m_h[2] = pre_m[2][pre_m[2].Count - 1].High;
                m_l[2] = pre_m[2][pre_m[2].Count - 1].Low;

                _ma[(2, 20)].Item1.Add(ma_[(2, 20)].Item1[next_bt_count / 3]);
                _ma[(2, 20)].Item2.Add(ma_[(2, 20)].Item2[next_bt_count / 3]);
                _ma[(2, 120)].Item1.Add(ma_[(2, 120)].Item1[next_bt_count / 3]);
                _ma[(2, 120)].Item2.Add(ma_[(2, 120)].Item2[next_bt_count / 3]);

            }
            if (next_bt_count % 3 != 0)
            {
                //3m_h
                if (m_h[2] < next_m_for[2][next_bt_count].High)
                {
                    pre_m[2][pre_m[2].Count() - 1].High = next_m_for[2][next_bt_count].High;
                }
                //3m_l
                if (m_l[2] > next_m_for[2][next_bt_count].Low)
                {
                    pre_m[2][pre_m[2].Count() - 1].Low = next_m_for[2][next_bt_count].Low;
                }
                //3m_c
                pre_m[2][pre_m[2].Count() - 1].Close = next_m_for[2][next_bt_count].Close;
                //Console.WriteLine(pre_m[2][pre_m[2].Count() - 1]);
                //Console.WriteLine(pre_m[2].Count());
            }

            //5m
            //定5m開盤
            if (next_bt_count % 5 == 0)
            {
                ////nex_1m_for[3][a].DateTime = DateTime.FromOADate(pre_3m.Count);
                next_m_for[3][next_bt_count].DateTime = DateTime.FromOADate(pre_m[3].Count);
                pre_m[3].Add(next_m_for[3][next_bt_count]);

                m_h[3] = pre_m[3][pre_m[3].Count - 1].High;
                m_l[3] = pre_m[3][pre_m[3].Count - 1].Low;

                _ma[(3, 20)].Item1.Add(ma_[(3, 20)].Item1[next_bt_count / 5]);
                _ma[(3, 20)].Item2.Add(ma_[(3, 20)].Item2[next_bt_count / 5]);
                _ma[(3, 120)].Item1.Add(ma_[(3, 120)].Item1[next_bt_count / 5]);
                _ma[(3, 120)].Item2.Add(ma_[(3, 120)].Item2[next_bt_count / 5]);

            }

            if (next_bt_count % 5 != 0)
            {
                //3m_h
                if (m_h[3] < next_m_for[3][next_bt_count].High)
                {
                    pre_m[3][pre_m[3].Count() - 1].High = next_m_for[3][next_bt_count].High;
                }
                //3m_l
                if (m_l[3] > next_m_for[3][next_bt_count].Low)
                {
                    pre_m[3][pre_m[3].Count() - 1].Low = next_m_for[3][next_bt_count].Low;
                }
                //3m_c
                pre_m[3][pre_m[3].Count() - 1].Close = next_m_for[3][next_bt_count].Close;
            }

            next_bt_count++;
            a++;
        }

        private void next_bt_Click(object sender, EventArgs e)
        {

            try
            {
                data_plus();
                price_stop();

            }
            catch
            {

            }

            //Console.WriteLine(pre_m[1][pre_m.Count() - 1]);
            //Console.WriteLine(next_bt_count);

            for (int i = 1; i <= 3; i++)
            {
                formsplot[i].Plot.Clear();
                //candlesticks
                candlesticks[i] = formsplot[i].Plot.AddCandlesticks(pre_m[i].ToArray());
                candlesticks[i].ColorUp = System.Drawing.Color.Red;
                candlesticks[i].ColorDown = System.Drawing.Color.White;
                //candlesticks[i].Add(pre_m[i][pre_m[i].Count-1]);

                //
                string anna_c = pre_m[i][pre_m[i].Count() - 1].Close.ToString();
                string dif = Math.Abs(pre_m[i][pre_m[i].Count() - 1].Close - pre_m[i][pre_m[i].Count() - 1].Open).ToString();

                string T;
                string JL;
                if (pre_m[i][pre_m[i].Count() - 1].Close > pre_m[i][pre_m[i].Count() - 1].Open)
                {
                    T = Math.Abs(pre_m[i][pre_m[i].Count() - 1].Open - pre_m[i][pre_m[i].Count() - 1].Low).ToString();
                    JL = Math.Abs(pre_m[i][pre_m[i].Count() - 1].High - pre_m[i][pre_m[i].Count() - 1].Close).ToString();

                }
                else if (pre_m[i][pre_m[i].Count() - 1].Close < pre_m[i][pre_m[i].Count() - 1].Open)
                {
                    T = Math.Abs(pre_m[i][pre_m[i].Count() - 1].Close - pre_m[i][pre_m[i].Count() - 1].Low).ToString();
                    JL = Math.Abs(pre_m[i][pre_m[i].Count() - 1].High - pre_m[i][pre_m[i].Count() - 1].Open).ToString();
                }
                else
                {
                    T = "";
                    JL = "";
                }


                annotation2 = formsplot[i].Plot.AddAnnotation("C : " + anna_c + "\n" + "JL : " + JL + "\n" + "Dif : " + dif + "\n" + "T : " + T, alignment: ScottPlot.Alignment.UpperCenter);
                annotation2.Font.Size = 16;
                //Console.WriteLine(pre_m[i][pre_m[i].Count() - 1].Close);

                formsplot[i].Plot.AddScatter(_ma[(i, 20)].Item1.ToArray(), _ma[(i, 20)].Item2.ToArray(), lineWidth: 2, markerSize: 0,color:Color.Violet);
                formsplot[i].Plot.AddScatter(_ma[(i, 120)].Item1.ToArray(), _ma[(i, 120)].Item2.ToArray(), lineWidth: 2, markerSize: 0,color:Color.Red);
                // polygon[i] = formsplot[i].Plot.AddPolygon(fill[i].Item1.Concat(fill[i].Item1.Reverse()).ToArray(), fill[i].Item2.Concat(fill[i].Item3.Reverse()).ToArray());

                plot_x_set(i, next_bt_count);
                plot_y_set(i, next_bt_count);
                formsplot[i].Refresh();
            }
            for (int i = 1; i <= 3; i++)
            {
                vLine[i] = formsplot[i].Plot.AddVerticalLine(0);
                hLine[i] = formsplot[i].Plot.AddHorizontalLine(0);
            }

            clear_singal();
            run_all_singal("next");
        }

        private void list_in_Click(object sender, EventArgs e)
        {

            star();

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // 設置初始目錄
                openFileDialog.InitialDirectory = @"E:\期貨 外匯模擬\TX_RTP\2023\grap\trade_pratise\";

                // 設置對話框的標題
                openFileDialog.Title = "select_file";
                openFileDialog.Filter = "文本文件 (*.csv)|*.csv";

                // 設置文件過濾器的默認索引，例如顯示所有文件
                openFileDialog.FilterIndex = 2;
                openFileDialog.Multiselect = false;

                // 打開對話框
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    selectdate_to = Path.GetFileNameWithoutExtension(filePath);
                    star();


                    //Console.WriteLine(filePath);
                    //Console.WriteLine(fileNameWithoutExtension);

                    string csvData = File.ReadAllText(filePath);

                    // 分割長空行來確定不同的數據塊
                    string[] sections = csvData.Split(new string[] { "\n\n\n\n" }, StringSplitOptions.RemoveEmptyEntries);


                    // 提取Long數據，忽略CSV文件頭部和尾部的空格或換行符
                    //ling
                    string[] longDataArray = sections[0].Trim().Split('\n');
                    List<string> longDataList = new List<string>();
                    string[] longtimeDataArray = sections[1].Trim().Split('\n');
                    List<string> longtimeDataList = new List<string>();

                    //short
                    string[] shortDataArray = sections[2].Trim().Split('\n');
                    List<string> shortDataList = new List<string>();
                    string[] shorttimeDataArray = sections[3].Trim().Split('\n');
                    List<string> shorttimeDataList = new List<string>();


                    foreach (var i in longDataArray)
                    {
                        longDataList.Add(i.Replace(",", " : "));
                    }
                    foreach (var i in longtimeDataArray)
                    {
                        longtimeDataList.Add(i.Replace(",", " : "));
                    }

                    foreach (var i in shortDataArray)
                    {
                        shortDataList.Add(i.Replace(",", " : "));
                    }
                    foreach (var i in shorttimeDataArray)
                    {
                        shorttimeDataList.Add(i.Replace(",", " : "));
                    }

                    // 對於每個List去掉標題行
                    longDataList.RemoveAt(0);
                    longtimeDataList.RemoveAt(0);
                    shortDataList.RemoveAt(0);
                    shorttimeDataList.RemoveAt(0);


                    listBox_long.Items.AddRange(longDataList.ToArray());
                    listBox_longtime.Items.AddRange(longtimeDataList.ToArray());
                    listBox_short.Items.AddRange(shortDataList.ToArray());
                    listBox_shorttime.Items.AddRange(shorttimeDataList.ToArray());



                }
            }



        }

        private void list_out_Click(object sender, EventArgs e)
        {
            List<string> list_long = new List<string>();
            list_long.Add("L_index,count,avg_price,profit");
            List<string> list_short = new List<string>();
            list_short.Add("S_index,count,avg_price,profit");

            List<string> list_longtime = new List<string>();
            list_longtime.Add("type,count,trade_price");
            List<string> list_shorttime = new List<string>();
            list_shorttime.Add("type,count,trade_price");

            foreach (var item in listBox_long.Items)
            {
                list_long.Add(item.ToString().Replace(" : ", ","));
            }
            foreach (var item in listBox_longtime.Items)
            {
                list_longtime.Add(item.ToString().Replace(" : ", ","));
            }
            //----------------------------------------------------------
            foreach (var item in listBox_short.Items)
            {
                list_short.Add(item.ToString().Replace(" : ", ","));
            }
            foreach (var item in listBox_shorttime.Items)
            {
                list_shorttime.Add(item.ToString().Replace(" : ", ","));
            }


            StringBuilder csvContent = new StringBuilder();
            foreach (var line in list_long)
            {
                csvContent.AppendLine(line);
            }
            csvContent.AppendLine("\n\n\n");
            foreach (var line in list_longtime)
            {
                csvContent.AppendLine(line);
            }
            csvContent.AppendLine("\n\n\n");
            //----------------------------------------------------------
            foreach (var line in list_short)
            {
                csvContent.AppendLine(line);
            }
            csvContent.AppendLine("\n\n\n");
            foreach (var line in list_shorttime)
            {
                csvContent.AppendLine(line);
            }


            File.WriteAllText(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\trade_pratise\" + selectdate_to + ".csv", csvContent.ToString());

        }


        ///</methon>
        ///

        //交易界面區>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        //private void long_bt_Click(object sender, EventArgs e)
        //{
        //    combine_L = avg_L * count_L + pre_m[1][pre_m[1].Count() - 1].Close;
        //    count_L++;
        //    avg_L = combine_L / count_L;

        //    listBox_long.Items.Add((pre_m[1].Count() - 1).ToString() + " : " + count_L + " : " + Math.Round(avg_L, 2));
        //    //
        //    listBox_longtime.Items.Add("L : " + (pre_m[1].Count() - 1).ToString() + " : " + pre_m[1][pre_m[1].Count() - 1].Close.ToString());
        //    //
        //    label4.Text = count_L.ToString();

        //}



        //private void long_s_bt_Click(object sender, EventArgs e)
        //{
        //    if (count_L != 0)
        //    {
        //        count_L--;
        //        profit_L = profit_L + (pre_m[1][pre_m[1].Count() - 1].Close - avg_L);
        //        profit_T = profit_L + profit_S;

        //        listBox_long.Items.Add((pre_m[1].Count() - 1).ToString() + " : " + count_L + " : " + Math.Round(avg_L, 2) + " : " + (pre_m[1][pre_m[1].Count() - 1].Close - avg_L));
        //        listBox_profit.Items.Add("L" + " : " + (pre_m[1].Count() - 1).ToString() + " : " + (pre_m[1][pre_m[1].Count() - 1].Close - avg_L));
        //        //
        //        listBox_longtime.Items.Add("LS : " + (pre_m[1].Count() - 1).ToString() + " : " + pre_m[1][pre_m[1].Count() - 1].Close.ToString());
        //        //
        //        label1.Text = "Long : " + Math.Round(profit_L, 2);
        //        label3.Text = "Total : " + Math.Round(profit_T, 2);
        //        label4.Text = count_L.ToString();

        //    }
        //}

        //private void short_bt_Click(object sender, EventArgs e)
        //{
        //    combine_S = avg_S * count_S + pre_m[1][pre_m[1].Count() - 1].Close;
        //    count_S++;
        //    avg_S = combine_S / count_S;

        //    listBox_short.Items.Add((pre_m[1].Count() - 1).ToString() + " : " + count_S + " : " + Math.Round(avg_S, 2));
        //    //
        //    listBox_shorttime.Items.Add("S : " + (pre_m[1].Count() - 1).ToString() + " : " + pre_m[1][pre_m[1].Count() - 1].Close.ToString());
        //    //
        //    label5.Text = count_S.ToString();

        //}

        //private void short_s_bt_Click(object sender, EventArgs e)
        //{
        //    if (count_S != 0)
        //    {
        //        count_S--;
        //        profit_S = profit_S - (pre_m[1][pre_m[1].Count() - 1].Close - avg_S);
        //        profit_T = profit_L + profit_S;

        //        listBox_short.Items.Add((pre_m[1].Count() - 1).ToString() + " : " + count_S + " : " + Math.Round(avg_S, 2) + " : " + -(pre_m[1][pre_m[1].Count() - 1].Close - avg_S));
        //        listBox_profit.Items.Add("S" + " : " + (pre_m[1].Count() - 1).ToString() + " : " + -(pre_m[1][pre_m[1].Count() - 1].Close - avg_S));
        //        //
        //        listBox_shorttime.Items.Add("SS : " + (pre_m[1].Count() - 1).ToString() + " : " + pre_m[1][pre_m[1].Count() - 1].Close.ToString());
        //        //

        //        label2.Text = "Short : " + Math.Round(profit_S, 2);
        //        label3.Text = "Total : " + Math.Round(profit_T, 2);
        //        label5.Text = count_S.ToString();
        //    }

        //}


        private void long_bt_Click(object sender, EventArgs e)
        {
            combine_L = avg_L * count_L + pre_m[1][pre_m[1].Count() - 1].Close;
            count_L++;
            avg_L = combine_L / count_L;

            list_soft("long", pre_m[1][pre_m[1].Count() - 1].Close);
        }
        private void long_s_bt_Click(object sender, EventArgs e)
        {
            if (count_L != 0)
            {
                count_L--;
                profit_L = profit_L + (pre_m[1][pre_m[1].Count() - 1].Close - avg_L);
                profit_T = profit_L + profit_S;

                list_soft("long_s", pre_m[1][pre_m[1].Count() - 1].Close);
            }
        }
        private void short_bt_Click(object sender, EventArgs e)
        {
            combine_S = avg_S * count_S + pre_m[1][pre_m[1].Count() - 1].Close;
            count_S++;
            avg_S = combine_S / count_S;

            list_soft("short", pre_m[1][pre_m[1].Count() - 1].Close);
        }
        private void short_s_bt_Click(object sender, EventArgs e)
        {
            if (count_S != 0)
            {
                count_S--;
                profit_S = profit_S - (pre_m[1][pre_m[1].Count() - 1].Close - avg_S);
                profit_T = profit_L + profit_S;

                list_soft("short_s", pre_m[1][pre_m[1].Count() - 1].Close);
            }
        }


        public void list_soft(string trade_type, double over_price)
        {
            if (trade_type == "long")
            {
                listBox_long.Items.Add((pre_m[1].Count() - 1).ToString() + " : " + count_L + " : " + Math.Round(avg_L, 2));
                //
                listBox_longtime.Items.Add("L : " + (pre_m[1].Count() - 1).ToString() + " : " + over_price.ToString());
                //
                label4.Text = count_L.ToString();
            }
            if (trade_type == "long_s")
            {
                Console.WriteLine("list_soft(long_s)");
                listBox_long.Items.Add((pre_m[1].Count() - 1).ToString() + " : " + count_L + " : " + Math.Round(avg_L, 2) + " : " + (over_price - avg_L));
                listBox_profit.Items.Add("L" + " : " + (pre_m[1].Count() - 1).ToString() + " : " + (over_price - avg_L));
                //
                listBox_longtime.Items.Add("LS : " + (pre_m[1].Count() - 1).ToString() + " : " + over_price.ToString());
                //
                label1.Text = "Long : " + Math.Round(profit_L, 2);
                label3.Text = "Total : " + Math.Round(profit_T, 2);
                label4.Text = count_L.ToString();
            }


            if (trade_type == "short")
            {
                listBox_short.Items.Add((pre_m[1].Count() - 1).ToString() + " : " + count_S + " : " + Math.Round(avg_S, 2));
                //
                listBox_shorttime.Items.Add("S : " + (pre_m[1].Count() - 1).ToString() + " : " + over_price.ToString());
                //
                label5.Text = count_S.ToString();
            }
            if (trade_type == "short_s")
            {
                listBox_short.Items.Add((pre_m[1].Count() - 1).ToString() + " : " + count_S + " : " + Math.Round(avg_S, 2) + " : " + -(over_price - avg_S));
                listBox_profit.Items.Add("S" + " : " + (pre_m[1].Count() - 1).ToString() + " : " + -(over_price - avg_S));
                //
                listBox_shorttime.Items.Add("SS : " + (pre_m[1].Count() - 1).ToString() + " : " + over_price.ToString());
                //

                label2.Text = "Short : " + Math.Round(profit_S, 2);
                label3.Text = "Total : " + Math.Round(profit_T, 2);
                label5.Text = count_S.ToString();
            }
        }



        private void set_Lstop_Click(object sender, EventArgs e)
        {
            bool success1 = int.TryParse(long_stop_profit.Text, out L_stop_profit);
            bool success2 = int.TryParse(long_stop_loss.Text, out L_stop_lose);

            //stop[].item1 = profit
            //stop[].item2 = loss

            if (success1 & success2)
            {
                stop["L"].Clear();
                stop["P_L"].Clear();
                stop["L"].Add((L_stop_profit, L_stop_lose));
                stop["P_L"].Add((pre_m[1][pre_m[1].Count() - 1].Close + L_stop_profit, pre_m[1][pre_m[1].Count() - 1].Close - L_stop_lose));
                //stop["P_L"].Add((L_stop_profit, L_stop_lose));
                listBox_Lstop.Items.Add("Profit : " + stop["P_L"][0].Item1 + " _ " + "Loss : " + stop["P_L"][0].Item2);
            }

            Console.WriteLine("S : " + stop["S"].Count());
            Console.WriteLine("L : " + stop["L"].Count());
            Console.WriteLine(stop["L"][0]);
            Console.WriteLine(stop["P_L"][0]);

            long_stop_profit.Clear();
            long_stop_loss.Clear();
        }
        private void set_Sstop_Click(object sender, EventArgs e)
        {
            bool success1 = int.TryParse(short_stop_profit.Text, out S_stop_profit);
            bool success2 = int.TryParse(short_stop_loss.Text, out S_stop_lose);

            if (success1 & success2)
            {
                stop["S"].Clear();
                stop["P_S"].Clear();
                stop["S"].Add((S_stop_profit, S_stop_lose));
                stop["P_S"].Add((pre_m[1][pre_m[1].Count() - 1].Close - S_stop_profit, pre_m[1][pre_m[1].Count() - 1].Close + S_stop_lose));
                listBox_Sstop.Items.Add("Profit : " + stop["P_S"][0].Item1 + " _ " + "Loss : " + stop["P_S"][0].Item2);
            }

            //stop["L"].Clear();
            Console.WriteLine("S : " + stop["S"].Count());
            Console.WriteLine("L : " + stop["L"].Count());
            Console.WriteLine(stop["S"][0]);
            Console.WriteLine(stop["P_S"][0]);

            short_stop_profit.Clear();
            short_stop_loss.Clear();
        }

        private void Lstop_clear_Click(object sender, EventArgs e)
        {
            listBox_Lstop.Items.Clear();
            stop["L"].Clear();
            stop["P_L"].Clear();
        }

        private void Sstop_clear_Click(object sender, EventArgs e)
        {
            listBox_Sstop.Items.Clear();
            stop["S"].Clear();
            stop["P_S"].Clear();

        }



        public void price_stop()
        {
            bool shouldClearStopLists = false;
            //Long處理
            if (stop["P_L"].Count != 0)
            {

                // 停利
                if (pre_m[1][pre_m[1].Count() - 1].High > stop["P_L"][0].Item1)
                {
                    count_L--;
                    profit_L = profit_L + stop["L"][0].Item1;
                    profit_T = profit_L + profit_S;
                    list_soft("long_s", stop["P_L"][0].Item1);

                    shouldClearStopLists = true;
                }
                // 停損
                else if (pre_m[1][pre_m[1].Count() - 1].Low < stop["P_L"][0].Item2)
                {
                    count_L--;
                    profit_L = profit_L - stop["L"][0].Item2;
                    profit_T = profit_L + profit_S;
                    list_soft("long_s", stop["P_L"][0].Item2);

                    shouldClearStopLists = true;
                }

                // 根據之前的判斷結果執行清除操作
                if (shouldClearStopLists)
                {
                    listBox_Lstop.Items.Clear();
                    stop["L"].Clear();
                    stop["P_L"].Clear();
                }

            }

            //Short處理
            if (stop["P_S"].Count != 0)
            {
                // 停利
                if (pre_m[1][pre_m[1].Count() - 1].Low < stop["P_S"][0].Item1)
                {
                    count_S--;
                    profit_S = profit_S + stop["S"][0].Item1;
                    profit_T = profit_L + profit_S;
                    list_soft("short_s", stop["P_S"][0].Item1);

                    shouldClearStopLists = true;
                }
                // 停損
                else if (pre_m[1][pre_m[1].Count() - 1].High > stop["P_S"][0].Item2)
                {
                    count_S--;
                    profit_S = profit_S - stop["S"][0].Item2;
                    profit_T = profit_L + profit_S;
                    list_soft("short_s", stop["P_S"][0].Item2);

                    shouldClearStopLists = true;
                }

                // 根據之前的判斷結果執行清除操作
                if (shouldClearStopLists)
                {
                    listBox_Sstop.Items.Clear();
                    stop["S"].Clear();
                    stop["P_S"].Clear();
                }
              

            }
        }


        //list_in | list_out>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //formsplot[1].Plot.Remove(currentLing);

            // 獲取當前選中的項目
            string currentItem = listBox_longtime.SelectedItem.ToString();
            // 執行需要的動作
            string[] item = currentItem.Split(new string[] { " : " }, StringSplitOptions.None);

            run_L_singal(item,15,5);
        }
        private void ListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //formsplot[1].Plot.Remove(currentLing);

            // 獲取當前選中的項目
            string currentItem = listBox_shorttime.SelectedItem.ToString();
            // 執行需要的動作
            string[] item = currentItem.Split(new string[] { " : " }, StringSplitOptions.None);

            run_S_singal(item,15,5);

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void signal_clear_Click(object sender, EventArgs e)
        {
            clear_singal();
        }
        private void singal_view_Click(object sender, EventArgs e)
        {
            clear_singal();
            run_all_singal("view");
        }

        public void run_all_singal(string str)
        {
            int size = 0;
            int width = 0;
            if (str == "png_out")
            {
                size = 7;
                width = 3;
            }
            else
            {
                size = 15;
                width = 4;
            }

            foreach (var i in listBox_longtime.Items)
            {
                string[] item = i.ToString().Split(new string[] { " : " }, StringSplitOptions.None);
                run_L_singal(item,size, width);

                //Console.WriteLine(i);
            }
            foreach (var i in listBox_shorttime.Items)
            {
                string[] item = i.ToString().Split(new string[] { " : " }, StringSplitOptions.None);
                run_S_singal(item,size, width);
            }
        }
        public void clear_singal()
        {
            try
            {
                for (int i = 1; i <= 3; i++)
                {
                    int count = currentLing.Keys.Count(key => key.Item1 == i);

                    for (var j = 0; j <= count - 1; j++)
                    {
                        formsplot[i].Plot.Remove(currentLing[(i, j)]);
                    }
                }

            }
            catch { }

            for (int i = 1; i <= 3; i++)
            {
                formsplot[i].Refresh();
            }
        }


        public void run_L_singal(string[] item,int size,int width)
        {
            
            //formsplot[1]_listbox1(1m)
            if (true)
            {
                if (item[0] == "L")
                {
                    currentLing[(1, listbox_1_count)] = new ScottPlot.Plottable.DraggableMarkerPlot()
                    {
                        X = double.Parse(item[1]),
                        Y = double.Parse(item[2]),
                        Color = Color.Cyan,
                        MarkerShape = ScottPlot.MarkerShape.filledTriangleUp,
                        MarkerSize = size,
                        Text = "L",
                    };
                    Console.WriteLine("L");

                }
                if (item[0] == "LS")
                {
                    currentLing[(1, listbox_1_count)] = new ScottPlot.Plottable.DraggableMarkerPlot()
                    {
                        X = double.Parse(item[1]),
                        Y = double.Parse(item[2]),
                        Color = Color.Cyan,
                        MarkerShape = ScottPlot.MarkerShape.openCircle,
                        MarkerSize = size,
                        MarkerLineWidth = width,
                        Text = "LS",
                    };
                    Console.WriteLine("LS");

                }
                formsplot[1].Plot.Add(currentLing[(1, listbox_1_count)]);
                formsplot[1].Refresh();
                listbox_1_count++;
            }

            //formsplot[2]_listbox1(3m)
            if (true)
            {
                double x = 0;
                var num = (int.Parse(item[1]) - 839) / 3;
                var numda = (int.Parse(item[1]) - 839) % 3;

                //Console.WriteLine(num);
                //Console.WriteLine(numda);
                if (numda == 0)
                {
                    x = num;
                }
                else
                {
                    x = num + 1;
                }

                if (item[0] == "L")
                {
                    currentLing[(2, listbox_2_count)] = new ScottPlot.Plottable.DraggableMarkerPlot()
                    {
                        X = 279 + x,
                        Y = double.Parse(item[2]),
                        Color = Color.Cyan,
                        MarkerShape = ScottPlot.MarkerShape.filledTriangleUp,
                        MarkerSize = size,
                        Text = "L",
                    };
                    // Console.WriteLine("L");

                }
                if (item[0] == "LS")
                {
                    currentLing[(2, listbox_2_count)] = new ScottPlot.Plottable.DraggableMarkerPlot()
                    {
                        X = 279 + x,
                        Y = double.Parse(item[2]),
                        Color = Color.Cyan,
                        MarkerShape = ScottPlot.MarkerShape.openCircle,
                        MarkerSize = size,
                        MarkerLineWidth = width,
                        Text = "LS",
                    };
                    // Console.WriteLine("LS");

                }
                formsplot[2].Plot.Add(currentLing[(2, listbox_2_count)]);
                formsplot[2].Refresh();
                listbox_2_count++;
            }

            //formsplot[3]_listbox1(5m)
            if (true)
            {
                double x = 0;
                var num = (int.Parse(item[1]) - 839) / 5;
                var numda = (int.Parse(item[1]) - 839) % 5;

                //Console.WriteLine(num);
                //Console.WriteLine(numda);
                if (numda == 0)
                {
                    x = num;
                }
                else
                {
                    x = num + 1;
                }

                if (item[0] == "L")
                {
                    currentLing[(3, listbox_3_count)] = new ScottPlot.Plottable.DraggableMarkerPlot()
                    {
                        X = 167 + x,
                        Y = double.Parse(item[2]),
                        Color = Color.Cyan,
                        MarkerShape = ScottPlot.MarkerShape.filledTriangleUp,
                        MarkerSize = size,
                        Text = "L",
                    };
                    // Console.WriteLine("L");

                }
                if (item[0] == "LS")
                {
                    currentLing[(3, listbox_3_count)] = new ScottPlot.Plottable.DraggableMarkerPlot()
                    {
                        X = 167 + x,
                        Y = double.Parse(item[2]),
                        Color = Color.Cyan,
                        MarkerShape = ScottPlot.MarkerShape.openCircle,
                        MarkerSize = size,
                        MarkerLineWidth = width,
                        Text = "LS",
                    };
                    // Console.WriteLine("LS");

                }
                formsplot[3].Plot.Add(currentLing[(3, listbox_3_count)]);
                formsplot[3].Refresh();
                listbox_3_count++;
            }
        }
        public void run_S_singal(string[] item,int size,int width)
        {


            //formsplot[1]_listbox2(1m)
            if (true)
            {
                if (item[0] == "S")
                {
                    currentLing[(1, listbox_1_count)] = new ScottPlot.Plottable.DraggableMarkerPlot()
                    {
                        X = double.Parse(item[1]),
                        Y = double.Parse(item[2]),
                        Color = Color.Lime,
                        MarkerShape = ScottPlot.MarkerShape.filledTriangleDown,
                        MarkerSize = size,
                        Text = "S",
                    };
                    Console.WriteLine("S");

                }
                if (item[0] == "SS")
                {
                    currentLing[(1, listbox_1_count)] = new ScottPlot.Plottable.DraggableMarkerPlot()
                    {
                        X = double.Parse(item[1]),
                        Y = double.Parse(item[2]),
                        Color = Color.Lime,
                        MarkerShape = ScottPlot.MarkerShape.openCircle,
                        MarkerSize = size,
                        MarkerLineWidth = width,
                        Text = "SS",
                    };
                    Console.WriteLine("SS");

                }
                formsplot[1].Plot.Add(currentLing[(1, listbox_1_count)]);
                formsplot[1].Refresh();
                listbox_1_count++;
            }

            //formsplot[2]_listbox1(3m)
            if (true)
            {
                double x = 0;
                var num = (int.Parse(item[1]) - 839) / 3;
                var numda = (int.Parse(item[1]) - 839) % 3;

                if (numda == 0)
                {
                    x = num;
                }
                else
                {
                    x = num + 1;
                }

                if (item[0] == "S")
                {
                    currentLing[(2, listbox_2_count)] = new ScottPlot.Plottable.DraggableMarkerPlot()
                    {
                        X = 279 + x,
                        Y = double.Parse(item[2]),
                        Color = Color.Lime,
                        MarkerShape = ScottPlot.MarkerShape.filledTriangleUp,
                        MarkerSize = size,
                        Text = "S",
                    };

                }
                if (item[0] == "SS")
                {
                    currentLing[(2, listbox_2_count)] = new ScottPlot.Plottable.DraggableMarkerPlot()
                    {
                        X = 279 + x,
                        Y = double.Parse(item[2]),
                        Color = Color.Lime,
                        MarkerShape = ScottPlot.MarkerShape.openCircle,
                        MarkerSize = size,
                        MarkerLineWidth = width,
                        Text = "SS",
                    };

                }
                formsplot[2].Plot.Add(currentLing[(2, listbox_2_count)]);
                formsplot[2].Refresh();
                listbox_2_count++;
            }

            //formsplot[3]_listbox1(5m)
            if (true)
            {
                double x = 0;
                var num = (int.Parse(item[1]) - 839) / 5;
                var numda = (int.Parse(item[1]) - 839) % 5;

                if (numda == 0)
                {
                    x = num;
                }
                else
                {
                    x = num + 1;
                }

                if (item[0] == "S")
                {
                    currentLing[(3, listbox_3_count)] = new ScottPlot.Plottable.DraggableMarkerPlot()
                    {
                        X = 167 + x,
                        Y = double.Parse(item[2]),
                        Color = Color.Lime,
                        MarkerShape = ScottPlot.MarkerShape.filledTriangleUp,
                        MarkerSize = size,
                        Text = "S",
                    };

                }
                if (item[0] == "SS")
                {
                    currentLing[(3, listbox_3_count)] = new ScottPlot.Plottable.DraggableMarkerPlot()
                    {
                        X = 167 + x,
                        Y = double.Parse(item[2]),
                        Color = Color.Lime,
                        MarkerShape = ScottPlot.MarkerShape.openCircle,
                        MarkerSize = size,
                        MarkerLineWidth = width,
                        Text = "SS",
                    };

                }
                formsplot[3].Plot.Add(currentLing[(3, listbox_3_count)]);
                formsplot[3].Refresh();
                listbox_3_count++;
            }
        }
        //交易界面區>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


        //十字線>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        private void Plot_MouseMove(object sender, MouseEventArgs e)
        {
            ScottPlot.FormsPlot formsPlot = sender as ScottPlot.FormsPlot;

            // 計算鼠標指針當前位置的數據坐標
            double mouseX = (int)formsPlot.Plot.GetCoordinateX(e.X);
            double mouseY = (int)formsPlot.Plot.GetCoordinateY(e.Y);

            annotation = formsPlot.Plot.AddAnnotation(((int)mouseY).ToString(), alignment: ScottPlot.Alignment.UpperLeft);
            annotation.Font.Size = 16;

            vLine[int.Parse(formsPlot.Name)].X = mouseX;
            hLine[int.Parse(formsPlot.Name)].Y = mouseY;
            vLine[int.Parse(formsPlot.Name)].IsVisible = true;
            hLine[int.Parse(formsPlot.Name)].IsVisible = true;

            vLine[int.Parse(formsPlot.Name)].LineWidth = 3;
            hLine[int.Parse(formsPlot.Name)].LineWidth = 3;

            formsPlot.Refresh();

        }
        public void Plot_MouseLeave(object sender, EventArgs e)
        {
            ScottPlot.FormsPlot formsPlot = sender as ScottPlot.FormsPlot;
            vLine[int.Parse(formsPlot.Name)].IsVisible = false;
            hLine[int.Parse(formsPlot.Name)].IsVisible = false;

            formsPlot.Refresh();
        }

        ////十字線>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


        //mouse_drop
        //formsplot1
        private void FormsPlot_MouseDown(object sender, MouseEventArgs e)
        {
            //ScottPlot.FormsPlot formsPlot = sender as ScottPlot.FormsPlot;
            ScottPlot.FormsPlot formsPlot = sender as ScottPlot.FormsPlot;
            if (e.Button == MouseButtons.Left)
            {
               // pointA = formsPlot1.GetMouseCoordinates();
                pointA = formsPlot.GetMouseCoordinates();
                
                isDragging = true; // 開始拖動
            }
        }
        private void FormsPlot_MouseUp(object sender, MouseEventArgs e)
        {
            //ScottPlot.FormsPlot formsPlot = sender as ScottPlot.FormsPlot;
            ScottPlot.FormsPlot formsPlot = sender as ScottPlot.FormsPlot;
            if (e.Button == MouseButtons.Left && isDragging)
            {
                // pointA = formsPlot1.GetMouseCoordinates();
                pointB = formsPlot.GetMouseCoordinates();

                isDragging = false; // 結束拖動

               // HandlePoints(1);
                HandlePoints(formsPlot);
                // 處理坐標點A和B
            }
            if (e.Button == MouseButtons.Right)
            {
                if (dif_line1 != null)
                {
                    formsPlot1.Plot.Remove(dif_line1);
                    formsPlot1.Plot.Remove(dif_anno1);
                    formsPlot1.Refresh(); 
                }
            }
        }

        private void HandlePoints(ScottPlot.FormsPlot formsplot)
        {
            double dif = Math.Abs(pointA.Y - pointB.Y);
            Console.WriteLine(dif);
            List<double> x = new List<double>();
            List<double> y = new List<double>();

            x.Add(pointA.X);
            x.Add(pointB.X);
            y.Add(pointA.Y);
            y.Add(pointB.Y);

            dif_line1 = formsplot.Plot.AddScatterLines(x.ToArray(), y.ToArray(), lineWidth: 3, color: Color.LightBlue);
            dif_anno1 = formsplot.Plot.AddAnnotation(Math.Round(dif, 0).ToString(), alignment: ScottPlot.Alignment.LowerCenter);
            dif_anno1.Font.Size = 16;
        }


        //out_png
        private void png_out_Click(object sender, EventArgs e)
        {


            for(int i = 1; i <= 3; i++)
            {
                candlesticks[i] = formsplot[i].Plot.AddCandlesticks(all_m[i].ToArray());
                formsplot[i].Plot.AddScatter(ma[(i, 20)].Item1.ToArray(), ma[(i, 20)].Item2.ToArray(), lineWidth: 2, markerSize: 0);
                formsplot[i].Plot.AddScatter(ma[(i, 120)].Item1.ToArray(), ma[(i, 120)].Item2.ToArray(), lineWidth: 2, markerSize: 0);

                if (true)
                {
                    int skip = 0;

                    List<double> high = new List<double>();
                    List<double> low = new List<double>();
                    List<ScottPlot.OHLC> pre = new List<ScottPlot.OHLC>();

                    var mv_20 = ma[(i, 20)].Item1.Zip(ma[(i, 20)].Item2, (x, y) => (x, y));
                    var mv_120 = ma[(i, 120)].Item1.Zip(ma[(i, 120)].Item2, (x, y) => (x, y));

                    //抓取ma20 ma120 前&後 的值
                    if (i == 1)
                    {
                        skip = 690;
                        mv_add[1] = mv_20.FirstOrDefault(point => point.x == 671).y;
                        mv_add[2] = mv_20.FirstOrDefault(point => point.x == 1139).y;
                        mv_add[3] = mv_120.FirstOrDefault(point => point.x == 671).y;
                        mv_add[4] = mv_120.FirstOrDefault(point => point.x == 1139).y;
                    }
                    if (i == 2)
                    {
                        skip = 240;
                        mv_add[1] = mv_20.FirstOrDefault(point => point.x == 221).y;
                        mv_add[2] = mv_20.FirstOrDefault(point => point.x == 379).y;
                        mv_add[3] = mv_120.FirstOrDefault(point => point.x == 221).y;
                        mv_add[4] = mv_120.FirstOrDefault(point => point.x == 379).y;
                    }
                    if (i == 3)
                    {
                        skip = 138;
                        mv_add[1] = mv_20.FirstOrDefault(point => point.x == 119).y;
                        mv_add[2] = mv_20.FirstOrDefault(point => point.x == 227).y;
                        mv_add[3] = mv_120.FirstOrDefault(point => point.x == 119).y;
                        mv_add[4] = mv_120.FirstOrDefault(point => point.x == 227).y;
                    }

                    //將cnadlesticks & ma20 ma120 高低都加入對比
                    foreach (var j in all_m[i].Skip(skip).ToArray())
                    {
                        high.Add(j.High);
                        low.Add(j.Low);
                    }
                    foreach (var j in mv_add.Values)
                    {
                        high.Add(j);
                        low.Add(j); ;
                    }

                    formsplot[i].Plot.SetAxisLimitsY(low.Min() * 0.9999, high.Max() * 1.0001);

                }
            }

            formsplot[1].Plot.SetAxisLimitsX(690, 1140); 
            formsplot[2].Plot.SetAxisLimitsX(240, 380); 
            formsplot[3].Plot.SetAxisLimitsX(138, 228);


            clear_singal();
            run_all_singal("png_out");

            for (int i = 1; i <= 3; i++)
            {
                formsplot[i].Refresh();
            }

            //combine bmp
            if (true)
            {
                Bitmap bmp1 = formsPlot1.Plot.Render(2400, 900); 
                Bitmap bmp2 = formsPlot2.Plot.Render(1200, 900); 
                Bitmap bmp3 = formsPlot3.Plot.Render(1200, 900); 

                int width = 4800;
                int height = 900;
                using (Bitmap combinedBmp = new Bitmap(width, height))
                {
                    using (Graphics g = Graphics.FromImage(combinedBmp))
                    {
                        // 繪制第一個圖像
                        g.DrawImage(bmp1, 0, 0);

                        // 繪制第二個圖像，緊隨第一個圖像之後
                        g.DrawImage(bmp2, bmp1.Width, 0);
                        g.DrawImage(bmp3, bmp1.Width + bmp2.Width, 0);

                    }

                    // 保存合並後的圖像到文件
                    combinedBmp.Save(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\trade_pratise\" + selectdate_to + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }


        }

        private void MyForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D | e.KeyCode == Keys.Right)
            {
                next_bt.PerformClick();
            }
        }

    }
}
