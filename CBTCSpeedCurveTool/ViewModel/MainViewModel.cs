using CBTCSpeedCurveTool.Classes;
using CBTCSpeedCurveTool.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Separator = LiveCharts.Wpf.Separator;

namespace CBTCSpeedCurveTool.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region <属性>
        private string authorText;
        public string AuthorText
        {
            get { return authorText; }
            set { authorText = value; RaisePropertyChanged(); }
        }

        private SeriesCollection _CBTCSeries;
        public SeriesCollection CBTCSeries
        {
            get { return _CBTCSeries; }
            set { _CBTCSeries = value; RaisePropertyChanged(); }
        }

        private AxesCollection _AxisXCollection;
        public AxesCollection AxisXCollection
        {
            get { return _AxisXCollection; }
            set { _AxisXCollection = value; RaisePropertyChanged(); }
        }

        private AxesCollection _AxisYCollection;
        public AxesCollection AxisYCollection
        {
            get { return _AxisYCollection; }
            set { _AxisYCollection = value; RaisePropertyChanged(); }
        }

        private int AxisFontSize = 15;

        private SolidColorBrush AxisBrush = new SolidColorBrush(Colors.Black);
        #endregion

        #region <命令>
        public RelayCommand SetParametersCM { get; set; }
        public RelayCommand UpdateCanvasCM { get; set; }
        #endregion

        public MainViewModel()
        {
            Initproperties();
            InitCommands();
        }


        #region <方法>
        private void Initproperties()
        {
            AuthorText = "—— by Pu Fan (Beijing Jiaotong University)";

            CBTCSeries = new SeriesCollection();

            //添加坐标轴
            AxisXCollection = new AxesCollection()
            {
                new Axis()
                {
                    Title = "Distance (m)",
                    FontSize=AxisFontSize,
                    Foreground=AxisBrush,
                    Separator=new Separator()
                    {
                        Stroke=new SolidColorBrush(Colors.LightGray),
                    }
                }
            };
            AxisYCollection = new AxesCollection()
            {
                new Axis()
                {
                    Title = "Velocity (km/h)",                 
                    FontSize=AxisFontSize,
                    Foreground=AxisBrush,
                    Separator=new Separator()
                    {
                        Stroke=new SolidColorBrush(Colors.LightGray),
                    }
                }
            };

            //设置数据类型
            XYPoint.Mapping();
        }

        private void InitCommands()
        {
            SetParametersCM = new RelayCommand(SetParametersFunc);
            UpdateCanvasCM = new RelayCommand(UpdateCanvasFunc);
        }

        private void SetParametersFunc()
        {
            SetParamView view = new SetParamView();
            view.ShowDialog();
        }

        private void UpdateCanvasFunc()
        {
            try
            {
                //目标距离
                double target_distance = GlobalParams.ParamList[0].RealParamValue;
                //最大牵引加速度
                double max_traction_acc = GlobalParams.ParamList[1].RealParamValue;
                //紧急制动最小减速度
                double min_em_braking_dec = GlobalParams.ParamList[2].RealParamValue;
                //最大速度
                double max_veloc = GlobalParams.ParamList[3].RealParamValue;
                //最大惰行加速度
                double max_coast_acc = GlobalParams.ParamList[4].RealParamValue;
                //ATP响应时间
                double atp_respon_t = GlobalParams.ParamList[5].RealParamValue;
                //牵引切除时间
                double trac_cancle_t = GlobalParams.ParamList[6].RealParamValue;
                //紧急制动建立时间
                double em_brakong_launch_t = GlobalParams.ParamList[7].RealParamValue;
                //紧急制动建立额外时间
                double em_brakong_launch_extra_t = GlobalParams.ParamList[8].RealParamValue;
                //临时限速
                double temp_speed_restr = GlobalParams.ParamList[9].RealParamValue;
                //临时限速起点
                double temp_speed_restr_start_point = GlobalParams.ParamList[10].RealParamValue;
                //临时限速终点
                double temp_speed_restr_end_point = GlobalParams.ParamList[11].RealParamValue;

                GlobalParams.DistanceStep = target_distance / GlobalParams.PointsCount;
                //设置x轴范围
                AxisXCollection[0].SetRange(0, target_distance);
                //设置y轴范围
                AxisYCollection[0].SetRange(0, max_veloc);

                CBTCSeries = new SeriesCollection();
                List<LineSeries> lines_to_add = new List<LineSeries>();

                //绘制最大速度曲线
                var max_speed_line = new LineSeries()
                {
                    Title = "最大速度",
                    Fill = new SolidColorBrush(Colors.Transparent),
                    LineSmoothness = 0,
                };
                var max_speed_points = new ChartValues<XYPoint>()
                {
                    new XYPoint(0,max_veloc),
                    new XYPoint(target_distance,max_veloc)
                };
                max_speed_line.Values = max_speed_points;
                lines_to_add.Add(max_speed_line);

                //绘制临时限速曲线
                var temp_speed_color = Colors.Gray;
                var temp_speed_brush = new SolidColorBrush(Color.FromArgb(100, temp_speed_color.R, temp_speed_color.G, temp_speed_color.B));
                var temp_speed_restr_line = new LineSeries()
                {
                    Title = "临时限速",
                    Stroke=temp_speed_brush,
                    Fill=temp_speed_brush,
                    LineSmoothness = 0,
                };
                var temp_speed_restr_points = new ChartValues<XYPoint>()
                {
                    new XYPoint(temp_speed_restr_start_point,temp_speed_restr),
                    new XYPoint(temp_speed_restr_end_point,temp_speed_restr)
                };
                temp_speed_restr_line.Values = temp_speed_restr_points;
                lines_to_add.Add(temp_speed_restr_line);

                //绘制GEBR曲线
                var GEBR_line = new LineSeries()
                {
                    Title= "GEBR曲线",
                    Fill = new SolidColorBrush(Colors.Transparent),
                    LineSmoothness = 0,
                };
                var GEBR_points = new ChartValues<XYPoint>();
                double cur_y = 0;
                double cur_x = target_distance;
                while (GEBR_points.Count <= GlobalParams.PointsCount)
                {
                    //设定速度不超过临时限速
                    double primal_cur_y = cur_y;
                    XYPoint addtion_point = null;
                    if (cur_x >= temp_speed_restr_start_point && cur_x <= temp_speed_restr_end_point)
                    {
                        cur_y = Math.Min(cur_y, temp_speed_restr / 3.6);
                        //限速终点处且改点大于限速值再添加一个重复的点
                        if ((cur_x == temp_speed_restr_start_point || cur_x == temp_speed_restr_end_point) &&
                            primal_cur_y > temp_speed_restr / 3.6)
                        {
                            addtion_point = new XYPoint(cur_x, primal_cur_y * 3.6);
                        }
                    }

                    //设定速度不超过最大值
                    cur_y = Math.Min(cur_y, max_veloc / 3.6);
                    //限速起点，先添加限速值再添加超速值
                    if (cur_x == temp_speed_restr_start_point)
                    {                       
                        if (addtion_point != null)
                        {
                            GEBR_points.Add(new XYPoint(cur_x, cur_y * 3.6));
                            GEBR_points.Add(addtion_point);
                            cur_y = primal_cur_y;
                        }
                        else
                        {
                            GEBR_points.Add(new XYPoint(cur_x, cur_y * 3.6));
                        }
                    }
                    //限速终点，先添加超速值再添加限速值
                    else if (cur_x == temp_speed_restr_end_point)
                    {                     
                        if (addtion_point != null)
                        {
                            GEBR_points.Add(addtion_point);
                            GEBR_points.Add(new XYPoint(cur_x, cur_y * 3.6));
                            cur_y = primal_cur_y;
                        }
                        else
                        {
                            GEBR_points.Add(new XYPoint(cur_x, cur_y * 3.6));
                        }
                    }
                    else
                    {
                        GEBR_points.Add(new XYPoint(cur_x, cur_y * 3.6));
                    }
                    double next_cur_x = cur_x - GlobalParams.DistanceStep;
                    double next_cur_y = Math.Pow(2 * min_em_braking_dec * GlobalParams.DistanceStep + Math.Pow(cur_y, 2), 0.5);
                    cur_x = next_cur_x;
                    cur_y = next_cur_y;
                }
                GEBR_line.Values = GEBR_points;
                lines_to_add.Add(GEBR_line);

                //绘制连接线
                var ATP_points = new ChartValues<XYPoint>();
                var conn_line_color = Colors.Black;
                var conn_line_brush = new SolidColorBrush(Color.FromArgb(70, conn_line_color.R, conn_line_color.G, conn_line_color.B));
                var reversed_GEBR_points = GEBR_points.Reverse();
                bool last_point_at_res_end = false;
                foreach (var point in reversed_GEBR_points)
                {
                    //计算数据点
                    var conn_points = new ChartValues<XYPoint>();
                    //各点速度和距离
                    //d点
                    double yd = point.Y / 3.6;
                    double xd = point.X;
                    //c点
                    double yc = Math.Max(0, yd - em_brakong_launch_extra_t * max_coast_acc);
                    double xc = xd - (Math.Pow(yd, 2) - Math.Pow(yc, 2)) / (2 * max_coast_acc);
                    //b点
                    double yb = Math.Max(0, yc - em_brakong_launch_t * max_coast_acc);
                    double xb = xc - (Math.Pow(yc, 2) - Math.Pow(yb, 2)) / (2 * max_coast_acc);
                    //a点
                    double ya = Math.Max(0, yb - trac_cancle_t * max_traction_acc);
                    double xa = xb - (Math.Pow(yb, 2) - Math.Pow(ya, 2)) / (2 * max_traction_acc);
                    //0点
                    double y0 = Math.Max(0, ya - atp_respon_t * max_traction_acc);
                    double x0 = xa - (Math.Pow(ya, 2) - Math.Pow(y0, 2)) / (2 * max_traction_acc);

                    var conn_line = new LineSeries()
                    {
                        Title = null,
                        Fill = new SolidColorBrush(Colors.Transparent),
                        Stroke = conn_line_brush,
                        LineSmoothness = 0,
                        StrokeDashArray = new DoubleCollection(2),
                        PointGeometrySize = 5,
                    };
                    conn_line.SetValue(Panel.ZIndexProperty, 100);
                    var conn_line_points = new ChartValues<XYPoint>()
                        {
                            new XYPoint(x0,y0*3.6),
                            new XYPoint(xa,ya*3.6),
                            new XYPoint(xb,yb*3.6),
                            new XYPoint(xc,yc*3.6),
                            new XYPoint(xd,yd*3.6)
                        };
                    conn_line.Values = conn_line_points;

                    if (x0 >= temp_speed_restr_start_point && x0 <= temp_speed_restr_end_point)
                    {
                        lines_to_add.Add(conn_line);
                        if (y0 * 3.6 <= temp_speed_restr)
                        {
                            //lines_to_add.Add(conn_line);

                            //添加ATP曲线数据
                            ATP_points.Add(new XYPoint(x0, y0 * 3.6));
                        }
                        else
                        {
                            //添加ATP曲线数据
                            ATP_points.Add(new XYPoint(x0, ATP_points[ATP_points.Count - 1].Y));
                            last_point_at_res_end = true;
                        }
                    }
                    else
                    {
                        lines_to_add.Add(conn_line);

                        //添加ATP曲线数据
                        if (last_point_at_res_end)
                        {
                            ATP_points.Add(new XYPoint(x0, ATP_points[ATP_points.Count - 1].Y));
                            last_point_at_res_end = false;
                        }
                        ATP_points.Add(new XYPoint(x0, y0 * 3.6));
                    }
                }

                //绘制ATP曲线
                var atp_line_color = Colors.Green;
                var atp_line_brush = new SolidColorBrush(Color.FromArgb(100, atp_line_color.R, atp_line_color.G, atp_line_color.B));
                var atp_line = new LineSeries()
                {
                    Title = "ATP防护曲线",
                    Fill = atp_line_brush,
                    Stroke = atp_line_brush,
                    LineSmoothness = 0,
                };
                atp_line.Values = ATP_points;
                lines_to_add.Add(atp_line);

                CBTCSeries.AddRange(lines_to_add);
            }
            catch
            {
                MessageBox.Show("未设置参数！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }            
        }
        #endregion
    }
}