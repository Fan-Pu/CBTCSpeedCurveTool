using CBTCSpeedCurveTool.Classes;
using CBTCSpeedCurveTool.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CBTCSpeedCurveTool.ViewModel
{
    class SetParamViewModel:ViewModelBase
    {
        #region <属性>
        private SetParamView view;

        private List<ParamItem> _ParamList;
        public List<ParamItem> ParamList
        {
            get { return _ParamList; }
            set { _ParamList = value;RaisePropertyChanged(); }
        }
        #endregion

        #region <命令>
        public RelayCommand OkButtonCM { get; set; }
        public RelayCommand CancleButtonCM { get; set; }
        #endregion

        public SetParamViewModel(SetParamView v)
        {
            view = v;
            InitProperties();
            InitCommands();
        }


        #region <方法>
        private void InitProperties()
        {
            ParamList = new List<ParamItem>()
            {
                new ParamItem("目标距离","2500","m"),
                new ParamItem("最大牵引加速度","0.6","m/s^2"),
                new ParamItem("紧急制动最小减速度","1.03","m/s^2"),
                new ParamItem("最大速度","200","km/h"),
                new ParamItem("最大惰行加速度","0.05","m/s^2"),
                new ParamItem("ATP响应时间","0.8","s"),
                new ParamItem("牵引切除时间","0.8","s"),
                new ParamItem("紧急制动建立时间","2","s"),
                new ParamItem("紧急制动建立额外时间","1.5","s"),
                new ParamItem("临时限速","150","km/h"),
                new ParamItem("临时限速起点坐标","1400","m"),
                new ParamItem("临时限速终点坐标","1800","m")
            };
        }

        private void InitCommands()
        {
            OkButtonCM = new RelayCommand(OkButtonFunc);
            CancleButtonCM = new RelayCommand(CancleButtonFunc);
        }

        private void OkButtonFunc()
        {
            try
            {
                foreach(ParamItem item in ParamList)
                {
                    //检查是否能取到double类型数据
                    double _ = item.RealParamValue;
                }
                MessageBox.Show("设置成功！", "通知", MessageBoxButton.OK, MessageBoxImage.Information);
                view.Close();
                GlobalParams.ParamList = new List<ParamItem>(ParamList);
                GlobalParams.PointsCount = 50;
                GlobalParams.ShowConnLine = true;
            }
            catch
            {
                MessageBox.Show("参数格式错误！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancleButtonFunc()
        {
            view.Close();
        }
        #endregion
    }
}
