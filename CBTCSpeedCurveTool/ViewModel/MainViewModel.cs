using CBTCSpeedCurveTool.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;

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

            CBTCSeries = new SeriesCollection()
            {
                new LineSeries()
                {
                    Values=new ChartValues<double>{3,5,7,4},
                    Fill=new SolidColorBrush(Colors.Transparent),
                },
                 new LineSeries()
                {
                    Values=new ChartValues<double>{5,6,2,7}
                },
            };

            //添加坐标轴
            AxisXCollection = new AxesCollection()
            {
                new Axis()
                {
                    Title = "Distance (m)",
                    FontSize=AxisFontSize,
                    Foreground=AxisBrush
                }
            };
            AxisYCollection = new AxesCollection()
            {
                new Axis()
                {
                    Title = "Velocity (m/s)",
                    FontSize=AxisFontSize,
                    Foreground=AxisBrush,
                }
            };
        }

        private void InitCommands()
        {
            SetParametersCM = new RelayCommand(SetParametersFunc);
        }

        private void SetParametersFunc()
        {
            SetParamView view = new SetParamView();
            view.ShowDialog();
        }
        #endregion
    }
}