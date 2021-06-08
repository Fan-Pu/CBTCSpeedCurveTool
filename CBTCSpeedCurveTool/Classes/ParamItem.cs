using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBTCSpeedCurveTool.Classes
{
    class ParamItem
    {
        public string ParamName { get; set; }

        public string ParamValue { get; set; }

        public double RealParamValue
        {
            get { return double.Parse(ParamValue); }
        }

        public string ParamUnit { get; set; }


        public ParamItem(string p_name,string p_value,string p_unit)
        {
            ParamName = p_name;
            ParamValue = p_value;
            ParamUnit = p_unit;
        }
    }
}
