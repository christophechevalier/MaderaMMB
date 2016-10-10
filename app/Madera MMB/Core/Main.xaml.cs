using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Madera_MMB.View_Crtl;

namespace Madera_MMB
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        [STAThread]

        public static void Main()
        {

            App app = new App();

            app.StartupUri = new System.Uri("MainWindow.xaml", System.UriKind.Relative);

            app.Run();

        }
    }
}
