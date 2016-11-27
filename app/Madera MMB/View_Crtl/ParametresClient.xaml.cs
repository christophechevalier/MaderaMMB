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
    /// Logique d'interaction pour ParametresClient.xaml
    /// </summary>
    public partial class ParametresClient : Page
    {
        #region Constructeur
        public ParametresClient()
        {
            InitializeComponent();
            Initialize_Labels();
        }
        #endregion

        #region Initialisation Containers
        private void Initialize_Labels()
        {
            NomCommercial.Content += "José Répas";
        }
        #endregion

        #region Listeners
        private void BtnConfirmerClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRetourListeClient_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}