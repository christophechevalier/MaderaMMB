using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
            Initialize_Liste_Clients_Wrapper();
        }
        #endregion

        #region Initialisation Containers
        private void Initialize_Labels()
        {
            NomCommercial.Content += "José Répas";
        }
        private void Initialize_Liste_Clients_Wrapper()
        {
            List<String> nomsComplet = new List<string>();
            List<String> noms = new List<string>();
            List<String> prenoms = new List<string>();
            List<String> telephones = new List<string>();
            List<String> emails = new List<string>();
            List<String> adresses = new List<string>();
            List<String> codepostal = new List<string>();
            List<String> villes = new List<string>();

            for (int i = 0; i < 9; i++)
            {
                nomsComplet.Add("NOM " + "Prénom " + i.ToString());
                noms.Add("Prénom " + i.ToString());
                prenoms.Add("Prénom " + i.ToString());
                telephones.Add("xx-xx-xx-xx-xx " + i.ToString());
                emails.Add("xxxx@xxx.fr " + i.ToString());
                adresses.Add("xxxxxxxxxxxxxxx " + i.ToString());
                codepostal.Add("xxxxx " + i.ToString());
                villes.Add("xxxx " + i.ToString());
            }

            ListeClients.ItemsSource = nomsComplet;
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

        private void ListeClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListItem item = sender as ListItem;

            if (e.AddedItems.Count > 0)
            {
                ClientNom.Text = e.AddedItems[0].ToString();
                ClientPrenom.Text = e.AddedItems[0].ToString();
                ClientTelephone.Text = e.AddedItems[0].ToString();
                ClientEmail.Text = e.AddedItems[0].ToString();
                ClientAdresse.Text = e.AddedItems[0].ToString();
                ClientCodePostal.Text = e.AddedItems[0].ToString();
                ClientVille.Text = e.AddedItems[0].ToString();
            }
        }
        #endregion
    }
}
