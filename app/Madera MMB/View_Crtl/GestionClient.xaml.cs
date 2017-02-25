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
using Madera_MMB.Lib;
using Madera_MMB.CAD;
using Madera_MMB.Model;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour GestionClient.xaml
    /// * Règles de gestion Client :
    /// - Ce qui définit l’unicité d’un client, c’est son adresse et/ou son email
    /// - On peut modifier les informations relatives aux clients
    /// - Dans le cas où le nom et/ou prénom sont modifiés, on met à jour la liste des noms des clients qui pourront être sélectionnés dans un nouveau projet
    /// - On ne peut pas supprimer un client ou modifier la référence d’un client
    /// </summary>
    public partial class GestionClient : Page
    {
        #region Properties
        private Connexion connexion { get; set; }
        private ClientCAD clientCAD { get; set; }
        private Client cli { get; set; }
        ListView selectedClientsListView = new ListView();
        #endregion

        #region Constructeur
        public GestionClient(Connexion co)
        {
            // Instanciations
            InitializeComponent();
            connexion = co;
            clientCAD = new ClientCAD(this.connexion);

            // Appel des méthodes dans le ctor
            Initialize_Liste_Clients_Wrapper();
        }
        #endregion

        #region Initialisation Container
        private void Initialize_Liste_Clients_Wrapper()
        {
            if (clientCAD.clients != null)
            {
                // TODO : Utiliser la liste d'objet existante -> clientCAD.clients
                //List<Client> nomsComplet = new List<Client>();
                List<String> nomsComplet = new List<string>();

                foreach (var cli in clientCAD.clients)
                {
                    //ListItem item = new ListItem();
                    //item = tbNomComplet.Text;

                    //nomsComplet.Add(cli.nom + " " + cli.prenom);

                    //ListViewItem item = new ListViewItem();
                    //item.Text = cli.nom;
                    //item.Tag = cli;

                    //ListeClients.Items.Add(item);

                    nomsComplet.Add(cli.nom + " " + cli.prenom);

                    // Value Non client
                    lblNomClient.Content = "";
                    lblNomClient.Content = cli.nom + " " + cli.prenom;

                    // Value Date création
                    lblDateCreation.Content = "";
                    lblDateCreation.Content = cli.creation;

                    // Value Date modification
                    lblDateModification.Content = "";
                    lblDateModification.Content = cli.modification;

                    // Value Nom Commercial
                    lblNomCommercial.Content = "";
                    //lblNomCommercial.Content = cli.commercial.nom + " " + cli.commercial.prenom;
                }
                // TODO : Remplacer le string par la liste des clients défini dans le cad
                //ListeClients.ItemsSource = clientCAD.clients;
                ListeClients.ItemsSource = nomsComplet;
            }
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
            //ListItem item = sender as ListItem;

            //if (e.AddedItems.Count > 0)
            //{
            //    ClientNom.Text = e.AddedItems[0].ToString();
            //    ClientPrenom.Text = e.AddedItems[0].ToString();
            //    ClientTelephone.Text = e.AddedItems[0].ToString();
            //    ClientEmail.Text = e.AddedItems[0].ToString();
            //    ClientAdresse.Text = e.AddedItems[0].ToString();
            //    ClientCodePostal.Text = e.AddedItems[0].ToString();
            //    ClientVille.Text = e.AddedItems[0].ToString();
            //}
            //else
            //{
            //    // Display default string.
            //    ClientNom.Text = "Empty";
            //    ClientPrenom.Text = "Empty";
            //}
        }
        #endregion
    }
}
