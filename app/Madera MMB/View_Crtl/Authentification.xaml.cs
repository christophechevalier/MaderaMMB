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
using System.Windows.Markup;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour Page1.xaml
    /// </summary>
    public partial class Authentification : Page, IComponentConnector
    {
        public Authentification()
        {
            InitializeComponent();
        }

        #region listeners
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        }
        #endregion


    }
}
