using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Madera_MMB.Lib;
using Madera_MMB.CAD;
using Madera_MMB.Model;
using System.Diagnostics;

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
        public ClientCAD clientCAD { get; set; }
        public Client cli { get; set; }
        #endregion

        #region Constructeur
        public GestionClient(Connexion co, ClientCAD CADclient)
        {
            // Instanciations
            InitializeComponent();
            connexion = co;
            if (this.connexion.MySQLconnected)
                this.connexion.SyncClient();
            this.clientCAD = CADclient;
            this.clientCAD.ListAllClients();
            DataContext = connexion;
            ListeClients.ItemsSource = clientCAD.Clients;
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
            if (ListeClients.SelectedItem != null)
            {
                Client client = (Client)ListeClients.SelectedItem;
                ClientNom.Text = client.nom;
                ClientPrenom.Text = client.prenom;
                ClientEmail.Text = client.email;
                ClientAdresse.Text = client.adresse;
                ClientCodePostal.Text = client.codePostal;
                ClientVille.Text = client.ville;
                ClientTelephone.Text = client.telephone;

                this.cli = client;

                lblDateCreation.Content = "";
                lblDateCreation.Content = cli.creation;

                lblDateModification.Content = "";
                lblDateModification.Content = cli.modification;
            }
        }
        #endregion
    }
}
