using Fluent;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace CBTCSpeedCurveTool
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        static App()
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {
            //ThemeManager.ChangeThemeBaseColor(this, "Colorful");

            base.OnStartup(e);
        }
    }
}
