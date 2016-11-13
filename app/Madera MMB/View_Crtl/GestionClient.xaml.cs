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
        List<Panel> listPanel = new List<Panel>();
        int index;

        public GestionClient()
        {
            InitializeComponent();
        }
        private void Btn_Ouvrir_Projet_Client_Click(object sender, RoutedEventArgs e)
        { 
        
        }
        private void Btn_List_Client_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Btn_Creer_Client_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Btn_Creer_Projet_Click(object sender, RoutedEventArgs e)
        {
            ModalCreationNouveauProjet.Visibility = System.Windows.Visibility.Visible;
        }
        private void Btn_Ouvrir_Projet_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Btn_Se_Deconnecter_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Btn_Valider_Click(object sender, RoutedEventArgs e)
        {
            ModalCreationNouveauProjet.Visibility = System.Windows.Visibility.Collapsed;
        }
        private void Btn_Retour_Click(object sender, RoutedEventArgs e)
        {
            ModalCreationNouveauProjet.Visibility = System.Windows.Visibility.Hidden;
        }
        private void Select_Nom_Client_Click(object sender, RoutedEventArgs e)
        {
            CbSelectNomClient.SelectedIndex = 0;
        }
    }
}
