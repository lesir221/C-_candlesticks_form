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
        //datetimepicker
        DateTime date;
        string selectdate, selectdate_to;

        //data_set
        public Dictionary<int, List<ScottPlot.OHLC>> pre_m = new Dictionary<int, List<ScottPlot.OHLC>>();
        public Dictionary<int, List<ScottPlot.OHLC>> next_m = new Dictionary<int, List<ScottPlot.OHLC>>();
        public Dictionary<int, List<ScottPlot.OHLC>> next_m_for = new Dictionary<int, List<ScottPlot.OHLC>>();

        //ma
        public Dictionary<(int, int), (List<double>, List<double>)> _ma = new Dictionary<(int, int), (List<double>, List<double>)>();
        public Dictionary<(int, int), (List<double>, List<double>)> ma_ = new Dictionary<(int, int), (List<double>, List<double>)>();
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

        public candlesticks_trader_v1()
        {
            InitializeComponent();
            //scottplot_set
            formsplot[1] = formsPlot1;
            formsplot[2] = formsPlot2;
            formsplot[3] = formsPlot3;

            for(int i = 1; i <= 3; i++)
            {
                formsplot[i].Plot.Style(dataBackground: System.Drawing.Color.Black);
                formsplot[i].Plot.Grid(color: Color.DimGray);
 
            }

            double[] x_tick = { 839, 855, 915, 975, 1139};
            string[] time = { "05:00", "09:00", "10:00", "11:00", "13:45"};
            formsplot[1].Plot.XAxis.ManualTickPositions(x_tick, time);
            double[] x_tick2 = { 279, 285, 305, 325, 379};
            formsplot[2].Plot.XAxis.ManualTickPositions(x_tick2, time);
            double[] x_tick3 = { 167, 171, 183, 195, 227};
            formsplot[3].Plot.XAxis.ManualTickPositions(x_tick3, time);
        }

        private void candlesticks_trader_v1_Load(object sender, EventArgs e)
        {
            //this.Size = new Size(1800,1600);
            this.WindowState = FormWindowState.Maximized;

            int top = 30;
            int pointx = 5;
            int pointy = 80;
            int width = 600;
            int height = 500;
            
            //timespicker
            dateTimePicker1.Location = new Point(1650, 30);

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
                label1.Location = new Point(50, 670);
                label2.Location = new Point(500, 670);
                label3.Location = new Point(950, 670);

                label4.Location = new Point(250, 670);
                label5.Location = new Point(700, 670);

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

                listBox_long.Location = new Point(50, 700);
                listBox_longtime.Location = new Point(240, 700);

                listBox_short.Location = new Point(500, 700);
                listBox_shorttime.Location = new Point(690, 700);

                listBox_profit.Location = new Point(950, 700);

            }
            //button
            if (true)
            {
                date_next.Location = new Point(1860, 20);
                date_pre.Location = new Point(1600, 20);

                next_bt.Location = new Point(1650, 700);

                long_bt.Location = new Point(1350, 700);
                long_s_bt.Location= new Point(1500, 700);

                short_bt.Location= new Point(1350, 780);
                short_s_bt.Location= new Point(1500, 780);
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
                Console.WriteLine(pre_m[i].Count);
                //candlesticks
                candlesticks[i] = formsplot[i].Plot.AddCandlesticks(pre_m[i].ToArray());
                candlesticks[i].ColorUp = System.Drawing.Color.Red;
                candlesticks[i].ColorDown = System.Drawing.Color.White;

                //scatter
                formsplot[i].Plot.AddScatter(_ma[(i, 20)].Item1.ToArray(), _ma[(i, 20)].Item2.ToArray(), lineWidth: 2, markerSize: 0);
                formsplot[i].Plot.AddScatter(_ma[(i, 120)].Item1.ToArray(), _ma[(i, 120)].Item2.ToArray(), lineWidth: 2, markerSize: 0);
                //polygon
                polygon[i] = formsplot[i].Plot.AddPolygon(fill[i].Item1.Concat(fill[i].Item1.Reverse()).ToArray(), fill[i].Item2.Concat(fill[i].Item3.Reverse()).ToArray());

                //show_x_stick  
                plot_x_set(i,next_bt_count);
                //show_y_stick  
                plot_y_set(i,next_bt_count);

                //refresh
                //formsplot[i].Plot.AxisAuto();
                //formsplot[i].Plot.AxisAutoY();
                formsplot[i].Refresh();
            }

           // Console.WriteLine(pre_m[1].Skip(690).Take(150).ToArray().Max());

        }

        public void plot_x_set(int i,int count)
        {
            //if(next_bt_count == 0)
            //{
                if (i == 1) { formsplot[1].Plot.SetAxisLimitsX(690 + count, 840+0.5 + count); }
                if (i == 2) { formsplot[2].Plot.SetAxisLimitsX(230 + count/3, 280+0.5 + count/3); }
                if (i == 3) { formsplot[3].Plot.SetAxisLimitsX(138 + count/5, 168+0.5 + count/5); }
                
            //}
            if (next_bt_count != 0)
            {


            }
        }

        public void plot_y_set(int i ,int count)
        {
            int skip = 0;
            int take = 0;
            int to_ = 19 ;

            List<double> high = new List<double>() ;
            List<double> low = new List<double>();
            List<ScottPlot.OHLC> pre = new List<ScottPlot.OHLC>();
            //if (next_bt_count == 0)
            //{ 
                var mv_20 = _ma[(i,20)].Item1.Zip(_ma[(i,20)].Item2, (x, y) => (x, y));
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
                    skip = 240 + count/3;
                    take = 50;
                    mv_add[1] = mv_20.FirstOrDefault(point => point.x == to_).y;
                    mv_add[2] = mv_20.FirstOrDefault(point => point.x == to_ + take).y;
                    mv_add[3] = mv_120.FirstOrDefault(point => point.x == to_).y;
                    mv_add[4] = mv_120.FirstOrDefault(point => point.x == to_ + take).y;
                }
                if (i == 3)
                {
                    to_ = 119 + count / 5;
                    skip = 138 + count/5;
                    take = 30;
                    mv_add[1] = mv_20.FirstOrDefault(point => point.x == to_).y;
                    mv_add[2] = mv_20.FirstOrDefault(point => point.x == to_ + take).y;
                    mv_add[3] = mv_120.FirstOrDefault(point => point.x == to_).y;
                    mv_add[4] = mv_120.FirstOrDefault(point => point.x == to_ + take).y;
                }

                //將cnadlesticks & ma20 ma120 高低都加入對比
            //    foreach (var j in pre_m[i].Skip(skip - (i-1) * take).Take(i * take).ToArray())
                foreach (var j in pre_m[i].Skip(skip).Take(take+1).ToArray())
                {
                    high.Add(j.High);
                    low.Add(j.Low);
                }
                foreach(var j in mv_add.Values)
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

            next_bt_count = 0;
            a = 0;
        }

        public void data_set()
        {

            try
            {
                // 1=m1 2=m3 3=m5
                (pre_m[1], next_m[1], _ma[(1, 20)], ma_[(1, 20)], _ma[(1, 120)], ma_[(1, 120)], fill[1]) = load_csv(selectdate_to, "m1");
                (pre_m[2], next_m[2], _ma[(2, 20)], ma_[(2, 20)], _ma[(2, 120)], ma_[(2, 120)], fill[2]) = load_csv(selectdate_to, "m3");
                (pre_m[3], next_m[3], _ma[(3, 20)], ma_[(3, 20)], _ma[(3, 120)], ma_[(3, 120)], fill[3]) = load_csv(selectdate_to, "m5");


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


        public (List<ScottPlot.OHLC>, List<ScottPlot.OHLC>,(List<double>, List<double>), (List<double>, List<double>), (List<double>, List<double>), (List<double>, List<double>), (double[], double[], double[])) load_csv(string date, string period)
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
            var ohlc_lines = File.ReadAllLines("E:\\期貨 外匯模擬\\TX_RTP\\2023\\grap\\all_date\\" + period+ "\\" + date + ".csv");
            var ohlc_org = ohlc_lines.Skip(1).ToArray();
            //Console.WriteLine("E:\\期貨 外匯模擬\\TX_RTP\\2023\\grap\\all_date\\" + period + "\\" + date + ".csv");

            int i =  0;
            int a;
            foreach(var line in ohlc_org)
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
                    mv20_x.Add( i );
                    mv20_y.Add( double.Parse(line.Split(',')[6]) );
                }
                if (line.Split(',')[7] != "")
                {
                    mv120_x.Add( i );
                    mv120_y.Add( double.Parse(line.Split(',')[7]) );
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

            return (table2, table3,(_mv20_x,_mv20_y), (mv20_x_, mv20_y_), (_mv120_x, _mv120_y), (mv120_x_, mv120_y_), (fill_x.ToArray(), fill_y1.ToArray(), fill_y2.ToArray()));
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
            data_plus();

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

                formsplot[i].Plot.AddScatter(_ma[(i, 20)].Item1.ToArray(),_ma[(i,20)].Item2.ToArray(), lineWidth: 2, markerSize:0);
                formsplot[i].Plot.AddScatter(_ma[(i, 120)].Item1.ToArray(), _ma[(i, 120)].Item2.ToArray(), lineWidth: 2, markerSize: 0);
                polygon[i] = formsplot[i].Plot.AddPolygon(fill[i].Item1.Concat(fill[i].Item1.Reverse()).ToArray(), fill[i].Item2.Concat(fill[i].Item3.Reverse()).ToArray());

                plot_x_set(i, next_bt_count);
                plot_y_set(i, next_bt_count);
                formsplot[i].Refresh();
            }
        }

        ///</methon>
        ///

        //交易界面區>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

        private void long_bt_Click(object sender, EventArgs e)
        {
            combine_L = avg_L * count_L + pre_m[1][pre_m[1].Count() - 1].Close;
            count_L++;
            avg_L = combine_L / count_L;

            listBox_long.Items.Add((pre_m[1].Count() - 1).ToString() + " : " + count_L + " : " + Math.Round(avg_L, 2));
            //
            listBox_longtime.Items.Add("L : " + (pre_m[1].Count() - 1).ToString() + " : " + pre_m[1][pre_m[1].Count() - 1].Close.ToString());
            //
            label4.Text = count_L.ToString();

        }

        private void long_s_bt_Click(object sender, EventArgs e)
        {
            if (count_L != 0)
            {
                count_L--;
                profit_L = profit_L + (pre_m[1][pre_m[1].Count() - 1].Close - avg_L);
                profit_T = profit_L + profit_S;

                listBox_long.Items.Add((pre_m[1].Count() - 1).ToString() + " : " + count_L + " : " + Math.Round(avg_L, 2) + " : " + (pre_m[1][pre_m[1].Count() - 1].Close - avg_L));
                listBox_profit.Items.Add("L" + " : " + (pre_m[1].Count() - 1).ToString() + " : " + (pre_m[1][pre_m[1].Count() - 1].Close - avg_L));
                //
                listBox_longtime.Items.Add("LS : " + (pre_m[1].Count() - 1).ToString() + " : " + pre_m[1][pre_m[1].Count() - 1].Close.ToString());
                //
                label1.Text = "Long : " + Math.Round(profit_L, 2);
                label3.Text = "Total : " + Math.Round(profit_T, 2);
                label4.Text = count_L.ToString();

            }
        }

        private void short_bt_Click(object sender, EventArgs e)
        {
            combine_S = avg_S * count_S + pre_m[1][pre_m[1].Count() - 1].Close;
            count_S++;
            avg_S = combine_S / count_S;

            listBox_short.Items.Add((pre_m[1].Count() - 1).ToString() + " : " + count_S + " : " + Math.Round(avg_S, 2));
            //
            listBox_shorttime.Items.Add("S : " + (pre_m[1].Count() - 1).ToString() + " : " + pre_m[1][pre_m[1].Count() - 1].Close.ToString());
            //
            label5.Text = count_S.ToString();
            
        }

        private void short_s_bt_Click(object sender, EventArgs e)
        {
            if (count_S != 0)
            {
                count_S--;
                profit_S = profit_S - (pre_m[1][pre_m[1].Count() - 1].Close - avg_S);
                profit_T = profit_L + profit_S;

                listBox_short.Items.Add((pre_m[1].Count() - 1).ToString() + " : " + count_S + " : " + Math.Round(avg_S, 2) + " : " + -(pre_m[1][pre_m[1].Count() - 1].Close - avg_S));
                listBox_profit.Items.Add("S" + " : " + (pre_m[1].Count() - 1).ToString() + " : " + -(pre_m[1][pre_m[1].Count() - 1].Close - avg_S));
                //
                listBox_shorttime.Items.Add("SS : " + (pre_m[1].Count() - 1).ToString() + " : " + pre_m[1][pre_m[1].Count() - 1].Close.ToString());
                //

                label2.Text = "Short : " + Math.Round(profit_S, 2);
                label3.Text = "Total : " + Math.Round(profit_T, 2);
                label5.Text = count_S.ToString();
            }

        }

        //交易界面區>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>


    }


}
