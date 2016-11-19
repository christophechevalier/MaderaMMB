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
    /// Logique d'interaction pour GestionClient.xaml
    /// </summary>
    public partial class GestionClient : Page
    {
        #region Constructeur
        public GestionClient()
        {
            InitializeComponent();
            Initialize_Labels();
        }
        #endregion

        #region Initialisation Container
        private void Initialize_Labels()
        {
            nomCommercial.Content += "José Répas";
        }
        #endregion

        #region Listeners
        private void BtnEditerClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCreerClient_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRetourListeProjet_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}
