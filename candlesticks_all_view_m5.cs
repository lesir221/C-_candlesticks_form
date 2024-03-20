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

    public partial class candlesticks_all_view_v1_m5 : Form
    {
        public string[] ohlc_lines;
        public Dictionary<int, List<ScottPlot.OHLC>> all_5m = new Dictionary<int, List<ScottPlot.OHLC>>();
        public Dictionary<int, ScottPlot.FormsPlot> formsplot = new Dictionary<int, ScottPlot.FormsPlot>();
        public Dictionary<int, ScottPlot.Plottable.Polygon> polygon = new Dictionary<int, ScottPlot.Plottable.Polygon>();
        public Dictionary<int, double[]> plot_high = new Dictionary<int, double[]>();
        public Dictionary<int, double[]> plot_low = new Dictionary<int, double[]>();

        public Dictionary<int, Label> label = new Dictionary<int, Label>();

        public Dictionary<int, ScottPlot.Plottable.ScatterPlot> scatter = new Dictionary<int, ScottPlot.Plottable.ScatterPlot>();
        public Dictionary<int, ScottPlot.Plottable.HLine> hline = new Dictionary<int, ScottPlot.Plottable.HLine>();

        public Dictionary<(int, int), List<(double, double)>> ma = new Dictionary<(int, int), List<(double, double)>>();
        public Dictionary<(int, int), double[]> ma_x = new Dictionary<(int, int), double[]>();
        public Dictionary<(int, int), double[]> ma_y = new Dictionary<(int, int), double[]>();

        public Dictionary<int,string> dir = new Dictionary<int, string>();

        public Dictionary<int, (double[], double[], double[])> fill = new Dictionary<int, (double[], double[], double[])>();
        public Dictionary<int, (double[], double[], double[])> fill2 = new Dictionary<int, (double[], double[], double[])>();

        public string[] name;
        public int c = 0;

        //細項控制開關
        public string swit;
        public string period;
        public string output;

        /// <windowsform_set>
        public candlesticks_all_view_v1_m5()
        {
            InitializeComponent();


            formsplot[1] = formsPlot1;
            formsplot[2] = formsPlot2;
            formsplot[3] = formsPlot3;
            formsplot[4] = formsPlot4;
            formsplot[5] = formsPlot5;
            formsplot[6] = formsPlot6;
            formsplot[7] = formsPlot7;
            formsplot[8] = formsPlot8;
            formsplot[9] = formsPlot9;

            label[1] = label1;
            label[2] = label2;
            label[3] = label3;
            label[4] = label4;
            label[5] = label5;
            label[6] = label6;
            label[7] = label7;
            label[8] = label8;
            label[9] = label9;

  

        }
        private void candlesticks_all_view_Load(object sender, EventArgs e)
        {
            //this.Size = new Size(1800,1600);
            this.WindowState = FormWindowState.Maximized;

            int top = 30;
            int pointx = 5;
            int pointy = 0;
            int width = 630;
            int height = 330;



            // plot
            if (true)
            {
                //formsplot1
                formsPlot1.Width = width;
                formsPlot1.Height = height;
                formsPlot1.Location = new Point(pointx, top + pointy);

                //formsplot2
                formsPlot2.Width = width;
                formsPlot2.Height = height;
                formsPlot2.Location = new Point(pointx * 2 + width, top + pointy);

                //formsplot3
                formsPlot3.Width = width;
                formsPlot3.Height = height;
                formsPlot3.Location = new Point(pointx * 3 + width * 2, top + pointy);

                //formsplot4
                formsPlot4.Width = width;
                formsPlot4.Height = height;
                formsPlot4.Location = new Point(pointx, top + pointy * 2 + height);

                //formsplot5
                formsPlot5.Width = width;
                formsPlot5.Height = height;
                formsPlot5.Location = new Point(pointx * 2 + width, top + pointy * 2 + height);

                //formsplot6
                formsPlot6.Width = width;
                formsPlot6.Height = height;
                formsPlot6.Location = new Point(pointx * 3 + width * 2, top + pointy * 2 + height);

                //formsplot7
                formsPlot7.Width = width;
                formsPlot7.Height = height;
                formsPlot7.Location = new Point(pointx, top + pointy * 3 + height * 2);

                //formsplot8
                formsPlot8.Width = width;
                formsPlot8.Height = height;
                formsPlot8.Location = new Point(pointx * 2 + width, top + pointy * 3 + height * 2);

                //formsplot9
                formsPlot9.Width = width;
                formsPlot9.Height = height;
                formsPlot9.Location = new Point(pointx * 3 + width * 2, top + pointy * 3 + height * 2);
            }
            // lable
            if (true)
            {
                //lable
                label1.Location = new Point(50 + pointx, top + pointy);
                label2.Location = new Point(50 + pointx * 2 + width, top + pointy);
                label3.Location = new Point(50 + pointx * 3 + width * 2, top + pointy);
                label4.Location = new Point(50 + pointx, top + pointy * 2 + height);
                label5.Location = new Point(50 + pointx * 2 + width, top + pointy * 2 + height);
                label6.Location = new Point(50 + pointx * 3 + width * 2, top + pointy * 2 + height);
                label7.Location = new Point(50 + pointx, top + pointy * 3 + height * 2);
                label8.Location = new Point(50 + pointx * 2 + width, top + pointy * 3 + height * 2);
                label9.Location = new Point(50 + pointx * 3 + width * 2, top + pointy * 3 + height * 2);
            }
            //button
            if (true)
            {
                view_old_set.Location = new Point(pointx * 3 + width * 2 + 300, top + pointy - 20);
                dn.Location = new Point(pointx * 3 + width * 2 + 450, top + pointy - 20);
                up.Location = new Point(pointx * 3 + width * 2 + 200, top + pointy - 20);

                jpg1.Location = new Point(550 + pointx, top + pointy);
                jpg2.Location = new Point(550 + pointx * 2 + width, top + pointy);
                jpg3.Location = new Point(550 + pointx * 3 + width * 2, top + pointy);
                jpg4.Location = new Point(550 + pointx, top + pointy * 2 + height);
                jpg5.Location = new Point(550 + pointx * 2 + width, top + pointy * 2 + height);
                jpg6.Location = new Point(550 + pointx * 3 + width * 2, top + pointy * 2 + height);
                jpg7.Location = new Point(550 + pointx, top + pointy * 3 + height * 2);
                jpg8.Location = new Point(550 + pointx * 2 + width, top + pointy * 3 + height * 2);
                jpg9.Location = new Point(550 + pointx * 3 + width * 2, top + pointy * 3 + height * 2);

            }
        }
        /// </summary>


        /// <plot_set>
        //public void plot_set_unknow()
        //{


        //    for(int i =1; i <= 9; i++)
        //    {
        //        formsplot[i].Plot.SetAxisLimitsX(150, 168);
        //        formsplot[i].Plot.SetAxisLimitsY(plot_low[i].Skip(149).Take(20).ToArray().Min()*0.9999, plot_high[i].Skip(149).Take(20).ToArray().Max()*1.0001);
        //        //formsplot[i].Refresh();
        //    }
        //}
        //public void plot_set_know()
        //{

        //}

        public void plot_set_candlestick(string swit,string per)
        {
            Console.WriteLine("03");

            if(per == "m5")
            {
                for (int i = 1; i <= 9; i++)
                {
                    formsplot[i].Plot.Clear();
                    //Console.WriteLine(fill2[1].Item2);
                    //Console.WriteLine(all_5m[0].Count());
                    //formsplot[i].Plot.AddCandlesticks(all_5m[i + 9 * c].ToArray());
                    try
                    {
                        //candlestick
                        formsplot[i].Plot.AddCandlesticks(all_5m[i + 9 * c].ToArray());

                        //scatter
                        formsplot[i].Plot.AddScatter(ma_x[(i + 9 * c, 20)], ma_y[(i + 9 * c, 20)], markerSize: 0);
                        formsplot[i].Plot.AddScatter(ma_x[(i + 9 * c, 120)], ma_y[(i + 9 * c, 120)], markerSize: 0);
                        //fillbetween
                        //polygon[i] = formsplot[i].Plot.AddPolygon(fill[i + 9 * c].Item1.Concat(fill[i + 9 * c].Item1.Reverse()).ToArray(), fill[i + 9 * c].Item2.Concat(fill[i + 9 * c].Item3.Reverse()).ToArray());
                        polygon[i] = formsplot[i].Plot.AddPolygon(fill2[i + 9 * c].Item1.Concat(fill2[i + 9 * c].Item1.Reverse()).ToArray(), fill2[i + 9 * c].Item2.Concat(fill2[i + 9 * c].Item3.Reverse()).ToArray());
                        //Console.WriteLine(fill2[0].Item1.Count());

                        if (swit == "unknow")
                        {
                            formsplot[i].Plot.SetAxisLimitsX(150, 168);
                            formsplot[i].Plot.SetAxisLimitsY(plot_low[i + 9 * c].Skip(149).Take(20).ToArray().Min() * 0.9999, plot_high[i + 9 * c].Skip(149).Take(20).ToArray().Max() * 1.0001);

                            var annotation1 = formsplot[i].Plot.AddAnnotation(((int)Math.Abs(fill[i + 9 * c].Item2[0] - fill[i + 9 * c].Item3[0])).ToString(), alignment: ScottPlot.Alignment.UpperLeft);
                            annotation1.Font.Size = 16;
                            var annotation2 = formsplot[i].Plot.AddAnnotation(((int)Math.Abs(fill[i + 9 * c].Item2[17] - fill[i + 9 * c].Item3[17])).ToString(), alignment: ScottPlot.Alignment.UpperRight);
                            annotation2.Font.Size = 16;
                            //Console.WriteLine(fill2[i].Item2[150].ToString());
                        }
                        else
                        {
                            formsplot[i].Plot.SetAxisLimitsX(150, 225);
                            var annotation1 = formsplot[i].Plot.AddAnnotation(((int)Math.Abs(fill[i + 9 * c].Item2[0] - fill[i + 9 * c].Item3[0])).ToString(), alignment: ScottPlot.Alignment.UpperLeft);
                            annotation1.Font.Size = 16;
                            var annotation2 = formsplot[i].Plot.AddAnnotation(((int)Math.Abs(fill[i + 9 * c].Item2[17] - fill[i + 9 * c].Item3[17])).ToString(), alignment: ScottPlot.Alignment.UpperCenter);
                            annotation2.Font.Size = 16;

                            formsplot[i].Plot.AxisAutoY();
                        }
                    }
                    catch
                    {
                        Console.WriteLine("03_error");
                    }
                    formsplot[i].Refresh();
                }
            }


            double[] x_tick = { 839, 855, 915, 975, 1139 };
            string[] time = { "05:00", "09:00", "10:00", "11:00", "13:45" };


            if (per == "m1")
            {
                for (int i = 1; i <= 9; i++)
                {
                    formsplot[i].Plot.XAxis.ManualTickPositions(x_tick, time);

                    formsplot[i].Plot.Clear();
                    try
                    {
                        //candlestick
                        formsplot[i].Plot.AddCandlesticks(all_5m[i + 9 * c].ToArray());
                        //scatter
                        formsplot[i].Plot.AddScatter(ma_x[(i + 9 * c, 20)], ma_y[(i + 9 * c, 20)], markerSize: 0);
                        formsplot[i].Plot.AddScatter(ma_x[(i + 9 * c, 120)], ma_y[(i + 9 * c, 120)], markerSize: 0);
                        //fillbetween
                        //polygon[i] = formsplot[i].Plot.AddPolygon(fill[i + 9 * c].Item1.Concat(fill[i + 9 * c].Item1.Reverse()).ToArray(), fill[i + 9 * c].Item2.Concat(fill[i + 9 * c].Item3.Reverse()).ToArray());
                        polygon[i] = formsplot[i].Plot.AddPolygon(fill2[i + 9 * c].Item1.Concat(fill2[i + 9 * c].Item1.Reverse()).ToArray(), fill2[i + 9 * c].Item2.Concat(fill2[i + 9 * c].Item3.Reverse()).ToArray());
                        //Console.WriteLine(fill2[0].Item1.Count());

                        if (swit == "unknow")
                        {
                            formsplot[i].Plot.SetAxisLimitsX(800, 915);
                            formsplot[i].Plot.SetAxisLimitsY(plot_low[i + 9 * c].Skip(800).Take(115).ToArray().Min() * 0.9999, plot_high[i + 9 * c].Skip(800).Take(115).ToArray().Max() * 1.0001);

                            //var annotation1 = formsplot[i].Plot.AddAnnotation(((int)Math.Abs(fill[i + 9 * c].Item2[0] - fill[i + 9 * c].Item3[0])).ToString(), alignment: ScottPlot.Alignment.UpperLeft);
                            //annotation1.Font.Size = 16;
                            //var annotation2 = formsplot[i].Plot.AddAnnotation(((int)Math.Abs(fill[i + 9 * c].Item2[17] - fill[i + 9 * c].Item3[17])).ToString(), alignment: ScottPlot.Alignment.UpperRight);
                            //annotation2.Font.Size = 16;
                            //Console.WriteLine(fill2[i].Item2[150].ToString());
                        }
                        else
                        {
                            formsplot[i].Plot.SetAxisLimitsX(800, 1139);
                            formsplot[i].Plot.SetAxisLimitsY(plot_low[i + 9 * c].Skip(800).Take(399).ToArray().Min() * 0.9999, plot_high[i + 9 * c].Skip(800).Take(399).ToArray().Max() * 1.0001);

                            //var annotation1 = formsplot[i].Plot.AddAnnotation(((int)Math.Abs(fill[i + 9 * c].Item2[0] - fill[i + 9 * c].Item3[0])).ToString(), alignment: ScottPlot.Alignment.UpperLeft);
                            //annotation1.Font.Size = 16;
                            //var annotation2 = formsplot[i].Plot.AddAnnotation(((int)Math.Abs(fill[i + 9 * c].Item2[17] - fill[i + 9 * c].Item3[17])).ToString(), alignment: ScottPlot.Alignment.UpperCenter);
                            //annotation2.Font.Size = 16;

                            //formsplot[i].Plot.AxisAutoY();
                            //formsplot[i].Plot.SaveFig(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\all_date\m1\view" + dir[i + 9 * c] + ".png", width: 1200, height: 700);

                        }
                    }
                    catch
                    {
                        Console.WriteLine("03_error");
                    }
                    formsplot[i].Refresh();
                    
                }
            }


        }

        public void plot_set_hlineANDvline(string per)
        {
            if(per == "m5")
            {
                for (int i = 1; i <= 9; i++)
                {
                    formsplot[i].Plot.AddHorizontalLine(all_5m[i + 9 * c].ToArray()[167].Close);
                    formsplot[i].Plot.AddVerticalLine(167);
                    formsplot[i].Plot.AddVerticalLine(176);
                    formsplot[i].Plot.AddVerticalLine(182);
                    //formsplot[i].Plot.AxisAutoY();
                    formsplot[i].Refresh();
                }
            }

        }
        /// </summary>

        public void plot_set_ma()
        {
            //for (int i = 1; i <= 9; i++)
            //{
            //    //formsplot[i].Plot.AddScatter(ma_x[(i, 20)], ma_y[(i, 20)], markerSize: 0);
            //    formsplot[i].Plot.AxisAutoY();
            //    formsplot[i].Refresh();

            //    //Console.WriteLine(all_5m[i + 9 * c].ToArray()[167].Close);
            //    //Console.WriteLine(ma_y[(i, 20)][167]);
            //}
        }

        public void label_set()
        {
            for (int i = 1; i <= 9; i++)
            {
                label[i].Text = dir[i + 9 * c];
               // Console.WriteLine(dir[i + 9 * c]);
            }

        }

        public void output_png(string output)
        {
            if(output == "yes")
            {
                for (int i = 1; i <= 9; i++)
                {
                    formsplot[i].Plot.SaveFig(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\all_date\m1\view\" + dir[i + 9 * c] + ".png", width: 1500, height: 900);
                }
            }
        }



        /// <old_view>
        public string[] file_name(string type)
        {
            Console.WriteLine("1");

            string folderPath = @"E:\期貨 外匯模擬\TX_RTP\2023\grap\" + type;
            string[] files = Directory.GetFiles(folderPath, "*.csv");
            List<string> list_name = new List<string>();

            foreach (string file in files)
            {
                // ?文件路?中提取文件名
                string fileName = Path.GetFileNameWithoutExtension(file);
                string[] parts = fileName.Split('_');
                // ?取日期部分，前提是文件名遵循?定的命名?定
                if (parts.Length >= 3)
                {
                    string datePart = $"{parts[0]}_{parts[1]}_{parts[2]}";
                    list_name.Add(datePart);
                }
            }

            // 使用 HashSet<T> ?移除重复?
            HashSet<string> uniqueItems = new HashSet<string>(list_name.ToArray());
            // ? HashSet<T> ??回??（如果需要）
            string[] uniqueArray = new HashSet<string>(uniqueItems).ToArray();

            return uniqueArray;
            Console.WriteLine("01_complete");

        }
        public string[] file_name_know(string type)
        {
            string folderPath = @"E:\期貨 外匯模擬\TX_RTP\2023\grap\" + type;
            string[] files = Directory.GetFiles(folderPath, "*.png");
            List<string> list_name = new List<string>();

            foreach (string file in files)
            {
                // ?文件路?中提取文件名
                string fileName = Path.GetFileNameWithoutExtension(file);
                string[] parts = fileName.Split('_');
                // ?取日期部分，前提是文件名遵循?定的命名?定
                if (parts.Length >= 3)
                {
                    string datePart = $"{parts[0]}_{parts[1]}_{parts[2]}";
                    list_name.Add(datePart);
                }
            }

            //Console.WriteLine(list_name.Count());
            //foreach(var i in list_name)
            //{
            //    Console.WriteLine(i);
            //}
  

            // 使用 HashSet<T> ?移除重复?
            HashSet<string> uniqueItems = new HashSet<string>(list_name.ToArray());
            // ? HashSet<T> ??回??（如果需要）
            string[] uniqueArray = new HashSet<string>(uniqueItems).ToArray();

            return uniqueArray;

        }

        public void read_csv(string type, string[] name)
        {
            int a = 1;
            int i = 0;

            Console.WriteLine("2");

            foreach (string str in name)
            {

                i = 0;
                List<ScottPlot.OHLC> table1 = new List<ScottPlot.OHLC>();
                List<double> mv20_x = new List<double>();
                List<double> mv20_y = new List<double>();
                List<double> mv120_x = new List<double>();
                List<double> mv120_y = new List<double>();

                List<double> fill_x = new List<double>();
                List<double> fill_y1 = new List<double>();
                List<double> fill_y2 = new List<double>();
                List<double> fill2_x = new List<double>();
                List<double> fill2_y1 = new List<double>();
                List<double> fill2_y2 = new List<double>();

                List<double> high = new List<double>();
                List<double> low = new List<double>();
                List<double> sign_high = new List<double>();
                List<double> sign_low = new List<double>();

                //                List<double> 

                if (type == "upto")
                {
                    ohlc_lines = File.ReadAllLines("E:\\期貨 外匯模擬\\TX_RTP\\2023\\grap\\" + type + "\\" + str + "_5m.csv");
                    foreach (string line in ohlc_lines)
                    {
                        table1.Add
                            (
                                new ScottPlot.OHLC
                                (
                                    double.Parse(line.Split(',')[1]),
                                    double.Parse(line.Split(',')[2]),
                                    double.Parse(line.Split(',')[3]),
                                    double.Parse(line.Split(',')[4]),
                                    (double)i,
                                    1
                                )
                            );

                        i++;
                    }
                    all_5m[a] = table1;
                    a++;
                }
                if (type == "wednesdays")
                {
                    ohlc_lines = File.ReadAllLines("E:\\期貨 外匯模擬\\TX_RTP\\2023\\grap\\" + type + "\\" + str + ".csv");
                    var ohlc_lines_to = ohlc_lines.Skip(1);
                    foreach (string line in ohlc_lines_to)
                    {
                        table1.Add
                            (
                                new ScottPlot.OHLC
                                (
                                    double.Parse(line.Split(',')[3]),
                                    double.Parse(line.Split(',')[4]),
                                    double.Parse(line.Split(',')[5]),
                                    double.Parse(line.Split(',')[6]),
                                    (double)i,
                                    1
                                )
                            );

                        i++;
                    }
                    all_5m[a] = table1;
                    a++;
                }
                if (type == @"all_date\m5")
                {
                    Console.WriteLine("2_1");
                    dir[a] = str;
                    ohlc_lines = File.ReadAllLines("E:\\期貨 外匯模擬\\TX_RTP\\2023\\grap\\" + type + "\\" + str + ".csv");
                    var ohlc_lines_to = ohlc_lines.Skip(1);
                    foreach (string line in ohlc_lines_to)
                    {
                        //ohlc
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
                        high.Add(double.Parse(line.Split(',')[3]));
                        low.Add(double.Parse(line.Split(',')[4]));

                        //ma20 & ma120
                        if (line.Split(',')[6] != "")
                        {
                            mv20_x.Add
                            (
                                i
                            );
                            mv20_y.Add
                            (
                            double.Parse(line.Split(',')[6])
                            );
                            high.Add(double.Parse(line.Split(',')[6]));
                            low.Add(double.Parse(line.Split(',')[6]));
                        }
                        if (line.Split(',')[7] != "")
                        {
                            mv120_x.Add
                            (
                                i
                            );
                            mv120_y.Add
                            (
                            double.Parse(line.Split(',')[7])
                            );
                            high.Add(double.Parse(line.Split(',')[7]));
                            low.Add(double.Parse(line.Split(',')[7]));
                        }
                        //fillbetween
                        if (i >= 150 & i <= 182)
                        {
                            fill_x.Add(i);
                            fill_y1.Add(double.Parse(line.Split(',')[6]));
                            fill_y2.Add(double.Parse(line.Split(',')[7]));
                        }
                        //fillbetween2
                        if (true)
                        {
                            //fill2_x.Add(i);
                            //if (line.Split(',')[7] != "")
                            //{
                            //    fill2_y1.Add(double.Parse(line.Split(',')[6]));
                            //}
                            //else
                            //{
                            //    fill2_y1.Add(null);
                            //}
                            //if (line.Split(',')[7] != "")
                            //{
                            //    fill2_y2.Add(double.Parse(line.Split(',')[7]));
                            //}
                            //else
                            //{
                            //    fill2_y2.Add(null);
                            //}
                            if (line.Split(',')[7] != "")
                            {
                                fill2_x.Add(i);
                                fill2_y1.Add(double.Parse(line.Split(',')[6]));
                                fill2_y2.Add(double.Parse(line.Split(',')[7]));
                            }
                        }

                        //zoom_high & low
                        sign_high.Add(high.Max());
                        sign_low.Add(low.Min());

                        i++;
                    }
                    all_5m[a] = table1;
                    ma_x[(a, 20)] = mv20_x.ToArray();
                    ma_y[(a, 20)] = mv20_y.ToArray();
                    ma_x[(a, 120)] = mv120_x.ToArray();
                    ma_y[(a, 120)] = mv120_y.ToArray();

                    fill[a] = (fill_x.ToArray(), fill_y1.ToArray(), fill_y2.ToArray());
                    fill2[a] = (fill2_x.ToArray(), fill2_y1.ToArray(), fill2_y2.ToArray());
                    plot_high[a] = sign_high.ToArray();
                    plot_low[a] = sign_low.ToArray();

                    Console.WriteLine(all_5m.Count());

                    //mv_x.Clear();
                    //mv_y.Clear();

                    //Console.WriteLine(i);
                    //Console.WriteLine(ma_y[(a, 20)][167]);
                    //Console.WriteLine(all_5m[a][167]);

                    a++;
                }


                if (type == "h_open")
                {
                    dir[a] = str;
                    ohlc_lines = File.ReadAllLines("E:\\期貨 外匯模擬\\TX_RTP\\2023\\grap\\" + type + "\\" + str + ".csv");
                    var ohlc_lines_to = ohlc_lines.Skip(1);
                    foreach (string line in ohlc_lines_to)
                    {
                        //ohlc
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
                        //ma20 & ma120
                        if (line.Split(',')[6] != "")
                        {
                            mv20_x.Add
                            (
                                i
                            );
                            mv20_y.Add
                            (
                            double.Parse(line.Split(',')[6])
                            );
                        }
                        if (line.Split(',')[7] != "")
                        {
                            mv120_x.Add
                            (
                                i
                            );
                            mv120_y.Add
                            (
                            double.Parse(line.Split(',')[7])
                            );
                        }
                        //fillbetween
                        if (i >= 150 & i <= 180)
                        {
                            fill_x.Add(i);
                            fill_y1.Add(double.Parse(line.Split(',')[6]));
                            fill_y2.Add(double.Parse(line.Split(',')[7]));
                        }

                        i++;
                    }
                    all_5m[a] = table1;
                    ma_x[(a, 20)] = mv20_x.ToArray();
                    ma_y[(a, 20)] = mv20_y.ToArray();
                    ma_x[(a, 120)] = mv120_x.ToArray();
                    ma_y[(a, 120)] = mv120_y.ToArray();

                    fill[a] = (fill_x.ToArray(), fill_y1.ToArray(), fill_y2.ToArray());
                    //fill2[a] = (fill2_x.ToArray(), fill2_y1.ToArray(), fill2_y2.ToArray());
                    a++;
                }
                else
                {
                    Console.WriteLine("2_1");
                    dir[a] = str;
                    ohlc_lines = File.ReadAllLines("E:\\期貨 外匯模擬\\TX_RTP\\2023\\grap\\" + type + "\\" + str + ".csv");
                    var ohlc_lines_to = ohlc_lines.Skip(1);
                    foreach (string line in ohlc_lines_to)
                    {
                        //ohlc
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
                        high.Add(double.Parse(line.Split(',')[3]));
                        low.Add(double.Parse(line.Split(',')[4]));

                        //ma20 & ma120
                        if (line.Split(',')[6] != "")
                        {
                            mv20_x.Add
                            (
                                i
                            );
                            mv20_y.Add
                            (
                            double.Parse(line.Split(',')[6])
                            );
                            high.Add(double.Parse(line.Split(',')[6]));
                            low.Add(double.Parse(line.Split(',')[6]));
                        }
                        if (line.Split(',')[7] != "")
                        {
                            mv120_x.Add
                            (
                                i
                            );
                            mv120_y.Add
                            (
                            double.Parse(line.Split(',')[7])
                            );
                            high.Add(double.Parse(line.Split(',')[7]));
                            low.Add(double.Parse(line.Split(',')[7]));
                        }
                        //fillbetween
                        if (i >= 150 & i <= 182)
                        {
                            fill_x.Add(i);
                            fill_y1.Add(double.Parse(line.Split(',')[6]));
                            fill_y2.Add(double.Parse(line.Split(',')[7]));
                        }
                        //fillbetween2
                        if (true)
                        {
                            if (line.Split(',')[7] != "")
                            {
                                fill2_x.Add(i);
                                fill2_y1.Add(double.Parse(line.Split(',')[6]));
                                fill2_y2.Add(double.Parse(line.Split(',')[7]));
                            }
                        }

                        //zoom_high & low
                        sign_high.Add(high.Max());
                        sign_low.Add(low.Min());

                        i++;
                    }
                    all_5m[a] = table1;
                    ma_x[(a, 20)] = mv20_x.ToArray();
                    ma_y[(a, 20)] = mv20_y.ToArray();
                    ma_x[(a, 120)] = mv120_x.ToArray();
                    ma_y[(a, 120)] = mv120_y.ToArray();

                    fill[a] = (fill_x.ToArray(), fill_y1.ToArray(), fill_y2.ToArray());
                    fill2[a] = (fill2_x.ToArray(), fill2_y1.ToArray(), fill2_y2.ToArray());
                    plot_high[a] = sign_high.ToArray();
                    plot_low[a] = sign_low.ToArray();
                    a++;
                }

            }

            //foreach(var j in dir)
            //{
            //    Console.WriteLine(j);
            //}
            Console.WriteLine("02_complete");
        }
        /// </summary>

        /// <old_view_button>
        private void dn_Click(object sender, EventArgs e)
        {
            try
            {
                plot_set_candlestick(swit,period);
                plot_set_hlineANDvline(period);
                label_set();
                output_png(output);


            }
            catch
            { Console.WriteLine("02"); }

            c++;
        }
        private void view_old_Click(object sender, EventArgs e)
        {
            c = 0;
            //upto
            //var name = file_name("upto");
            //read_csv("upto", name);
            //plot_set();
            //upto

            //wednesdays
            //var name = file_name("wednesdays");
            //read_csv("wednesdays", name);
            //plot_set();
            //wednesdays




            //wednesdays

            //h_open
            //var name = file_name("h_open");
            //read_csv("h_open", name);
            //plot_set();

            //wednesdays

            //Console.WriteLine(name.Count());


            //
            if (false)
            {
                //all_date_unkown
                //將(all_date)過篩(0430~0845)到view
                var name = file_name(@"all_date\m5");
                read_csv(@"all_date\m5", name);
                if (true)
                {
                    swit = "know";
                    period = "m5";
                }
               


                //plot_set_know();


                //all_date_kown
                //在過篩_覺得(all_date)可行後放進\view\0845中的png,將這些可行的(0430~0845)拿出來檢討
                //var name = file_name_know(@"view\0845");
                //read_csv("all_date", name);
                //swit = "know";
            }
            if (true)
            {
                //all_date_unkown
                //將(all_date)過篩(0430~0845)到view
                var name = file_name(@"all_date\m1");
                read_csv(@"all_date\m1", name);
                if (true)
                {
                    swit = "know";
                    period = "m1";
                    output = "yes";
                }



                //plot_set_know();


                //all_date_kown
                //在過篩_覺得(all_date)可行後放進\view\0845中的png,將這些可行的(0430~0845)拿出來檢討
                //var name = file_name_know(@"view\0845");
                //read_csv("all_date", name);
                //swit = "know";
            }


        }

        /// </summary>


        ///<jpg_out>
        private void jpg1_Click(object sender, EventArgs e)
        {
            formsPlot1.Plot.SaveFig(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\view\" + label1.Text + ".png" , width: 600, height: 350);
        }
        private void jpg2_Click(object sender, EventArgs e)
        {
            formsPlot2.Plot.SaveFig(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\view\" + label2.Text + ".png", width: 600, height: 350);
        }
        private void jpg3_Click(object sender, EventArgs e)
        {
            formsPlot3.Plot.SaveFig(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\view\" + label3.Text + ".png", width: 600, height: 350);
        }
        private void jpg4_Click(object sender, EventArgs e)
        {
            formsPlot4.Plot.SaveFig(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\view\" + label4.Text + ".png", width: 600, height: 350);
        }
        private void jpg5_Click(object sender, EventArgs e)
        {
            formsPlot5.Plot.SaveFig(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\view\" + label5.Text + ".png", width: 600, height: 350);
        }
        private void jpg6_Click(object sender, EventArgs e)
        {
            formsPlot6.Plot.SaveFig(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\view\" + label6.Text + ".png", width: 600, height: 350);
        }
        private void jpg7_Click(object sender, EventArgs e)
        {
            formsPlot7.Plot.SaveFig(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\view\" + label7.Text + ".png", width: 600, height: 350);
        }
        private void jpg8_Click(object sender, EventArgs e)
        {
            formsPlot8.Plot.SaveFig(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\view\" + label8.Text + ".png", width: 600, height: 350);
        }
        private void jpg9_Click(object sender, EventArgs e)
        {
            formsPlot9.Plot.SaveFig(@"E:\期貨 外匯模擬\TX_RTP\2023\grap\view\" + label9.Text + ".png", width: 600, height: 350);
        }

        ///</jpg_out>/>

        // <all_view>

    }
}
