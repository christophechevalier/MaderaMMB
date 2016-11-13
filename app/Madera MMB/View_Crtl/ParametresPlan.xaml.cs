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

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour ParametresPlan.xaml
    /// </summary>
    public partial class ParametresPlan : Page
    {
        public ParametresPlan()
        {
            InitializeComponent();
        }

        private void cp_Button_Click(object sender, RoutedEventArgs e)
        {
            forme.Visibility = System.Windows.Visibility.Visible;
            taille.Visibility = System.Windows.Visibility.Visible;
        }
    }
}
