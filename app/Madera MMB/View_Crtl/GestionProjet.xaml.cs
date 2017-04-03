using Madera_MMB.Lib.Tools;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Madera_MMB.Lib;
using Madera_MMB.CAD;
using Madera_MMB.Model;
using System.Diagnostics;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour GestionProjet.xaml
    /// </summary>
    public partial class GestionProjet : Page
    {
        #region Properties
        private Connexion connexion { get; set; }
        private Commercial commercial { get; set; }
        private ProjetCAD projetCAD { get; set; }
        public ClientCAD clientCAD { get; set; }
        public Projet proj { get; set; }
        public Commercial commercial_authentifié { get; set; }
        #endregion

        #region Constructeur
        /// <summary>
        /// Constructeur qui prend en paramètre la connexion et le commercial authentifié
        /// </summary>
        /// <param name="co"></param>
        /// <param name="com"></param>
        public GestionProjet(Connexion co, Commercial com)
        {
            // Instanciations
            InitializeComponent();
            connexion = co;
            commercial = com;
            clientCAD = new ClientCAD(this.connexion);
            projetCAD = new ProjetCAD(this.connexion, this.commercial, clientCAD.Clients);
            DataContext = projetCAD;
        }
        #endregion

        #region Initialisation Container
        private void Initialize_Dialog_Creation_Projet()
        {
            if (projetCAD.Projets != null)
            {
                var window = new SelectModalWindow();
                window.Title = "Nouveau Projet ";
                window.TitleLabel.Content = "Nouveau Projet Client :";

                window.DataSelect.Text = "Sélectionnez un client";
                window.DataSelect.ItemsSource = clientCAD.Clients;
                window.DataSelect.DisplayMemberPath = "nomprenom";

                // Permet de set l'image dynamiquement
                BitmapImage bm = new BitmapImage(new Uri("../../Lib/Images/folder_client.png", UriKind.RelativeOrAbsolute));
                window.TitleImage.Source = bm;

                window.Retour.Click += delegate(object sender, RoutedEventArgs e)
                {
                    window.Close();
                };

                window.Valider.Click += delegate(object sender, RoutedEventArgs e)
                {
                    Client getClient = new Client();

                    if (window.DataSelect.SelectedIndex > 0)
                    {
                        getClient = (Client)window.DataSelect.SelectedItem;

                        int i = 1;
                        foreach (Projet proj in projetCAD.Projets)
                        {
                            if (proj.client.nomprenom == getClient.nomprenom)
                            {
                                i++;
                            }
                        }

                        Projet NewProjet = new Projet(getClient, commercial);
                        NewProjet.reference = generateKeyProjet(getClient, commercial);
                        NewProjet.nom = NewProjet.client.nomprenom + " (" + i + ") ";
                        projetCAD.Projets.Add(NewProjet);
                        projetCAD.InsertProjet(NewProjet);
                        window.Close();
                    }
                    else
                    {
                        MessageBox.Show("Vous devez sélectionner un client pour le nouveau projet !");
                    }
                };              
                window.ShowDialog();
            }
        }
        #endregion

        #region Listeners
        // Afficher la liste des clients existants
        private void Btn_List_Client_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        }
        // Créer un nouveau client
        private void Btn_Creer_Client_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        }
        // Créer un nouveau projet
        private void Btn_Creer_Projet_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Initialize_Dialog_Creation_Projet();
        }
        // Ouvrir un projet client déjà selectionner
        private void Btn_Ouvrir_Projet_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        }
        // Se déconnecter
        private void Btn_Se_Deconnecter_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        }
        // Valider - Suivant
        private void Btn_Valider_Click(object sender, RoutedEventArgs e)
        {
        }
        // Retour - Précedent
        private void Btn_Retour_Click(object sender, RoutedEventArgs e)
        {
        }
        // Editer un client
        private void Btn_Editer_Client_Click(object sender, RoutedEventArgs e)
        {
        }
        // Ouvrir un projet client
        private void Btn_Select_Projet_Client_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            proj = (Projet)btn.DataContext;

            foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(WrapProjets))
            {
                tgbt.IsChecked = false;
            }
            btn.IsChecked = true;

            // Value Non client
            lblNomClient.Content = "";
            lblNomClient.Content = proj.client.nom + " " + proj.client.prenom;

            // Value Date création
            lblDateCreation.Content = "";
            lblDateCreation.Content = proj.creation.Date;

            // TODO : Value Statut Dernier Devis
            lblStatut.Content = "?";

            // Value Nombre de plans
            lblNbPlans.Content = "";
            lblNbPlans.Content = this.projetCAD.CountPlansProjet(proj.reference);

            // Value Date modification
            lblDateModification.Content = "";
            lblDateModification.Content = proj.modification.Date;

            // Value Nom Commercial
            lblNomCommercial.Content = "";
            lblNomCommercial.Content = proj.commercial.nom + " " + proj.commercial.prenom;
        }
        #endregion

        #region Tools
        private string generateKeyProjet(Client client, Commercial comm)
        {
            string key = comm.nom.Substring(0, 1) + comm.prenom.Substring(0, 1) + client.nom.Substring(0, 1) + client.prenom.Substring(0, 1);
            Random rand = new Random();
            int temp = rand.Next(000000, 999999);
            key += temp.ToString();
            return key;
        }
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        #endregion
    }
}
