using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTCSpeedCurveTool.Classes
{
    class GlobalParams
    {
        public static List<ParamItem> ParamList;

        /// <summary>
        /// 点的数量
        /// </summary>
        public static int PointsCount;

        /// <summary>
        /// 距离增量
        /// </summary>
        public static double DistanceStep;

        /// <summary>
        /// 是否绘制连接线
        /// </summary>
        public static bool ShowConnLine;
    }
}
