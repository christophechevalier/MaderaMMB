using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Madera_MMB.View_Crtl;


namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
      
        public MainWindow()
        {
            InitializeComponent();



            //Logo.Source = new BitmapImage(new Uri("Lib/logo_madera.png", UriKind.Absolute));

            //Mainframe.Source = new Uri("View_Crtl/Authentification.xaml", UriKind.Absolute);

            Mainframe.Content = new View_Crtl.Authentification();

            //MainFrame.NavigationService.Navigate(new View_Crtl.Authentification());
        }

    }
}
