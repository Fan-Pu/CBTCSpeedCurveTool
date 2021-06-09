using LiveCharts;
using LiveCharts.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTCSpeedCurveTool.Classes
{
    class XYPoint
    {
        public double X { get; set; }

        public double Y { get; set; }

        public XYPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// 映射数据点
        /// </summary>
        public static void Mapping()
        {
            var mapper = Mappers.Xy<XYPoint>()
                .X(m => m.X)
                .Y(m => m.Y);
            Charting.For<XYPoint>(mapper);
        }
    }
}
