using GalaSoft.MvvmLight;
using LiveCharts;
using LiveCharts.Wpf;
using System.Windows.Media;

namespace CBTCSpeedCurveTool.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region <ÊôÐÔ>
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

        #region <ÃüÁî>

        #endregion

        public MainViewModel()
        {
            Initproperties();
            InitCommands();
        }


        #region <·½·¨>
        private void Initproperties()
        {
            AuthorText = "¡ª¡ª by Pu Fan (Beijing Jiaotong University)";

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

        }
        #endregion
    }
}