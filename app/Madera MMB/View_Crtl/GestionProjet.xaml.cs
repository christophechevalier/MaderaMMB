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

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour GestionProjet.xaml
    /// * Règles de gestion Projet :
    /// - On ne peut pas supprimer un projet ni le modifier (nom)
    /// - On peut consulter un projet existant
    /// - On peut créer plusieurs projets pour le même client
    /// - Pour créer un nouveau projet, il faut obligatoirement sélectionner un client dans la liste
    /// - Le nom d’un projet est unique par projet client
    /// </summary>
    public partial class GestionProjet : Page
    {
        #region Properties
        private Connexion connexion { get; set; }
        private Commercial commercial { get; set; }
        private ProjetCAD projetCAD { get; set; }
        private GestionPlan gestionPlan { get; set; }
        public ClientCAD clientCAD { get; set; }
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
            // Appel des méthodes dans le ctor
            Initialize_Projet_Wrapper();
            Initialize_Menu_Wrapper();
        }
        #endregion

        #region Initialisation Container
        /// <summary>
        /// Méthode pour parcourir la liste des projets existant en bdd
        /// Pour chaque projet sélectionné, on aura le nom du client, le nom d'un commercial, la date de création/modification, 
        /// et le nombre de plans associés
        /// </summary>
        private void Initialize_Projet_Wrapper()
        {
            if (projetCAD.Projets != null)
            {
                foreach (var proj in projetCAD.Projets)
                {
                    ToggleButton UnProjet = new ToggleButton();
                    UnProjet.Background = Brushes.White;
                    UnProjet.Width = 120;
                    UnProjet.Height = 120;
                    Thickness margin = UnProjet.Margin;
                    margin.Left = 20;
                    margin.Right = 20;
                    margin.Bottom = 20;
                    margin.Top = 20;
                    UnProjet.Margin = margin;

                    Image img = new Image();
                    img.Width = 70;
                    img.Height = 70;
                    img.VerticalAlignment = VerticalAlignment.Top;
                    string source = "../Lib/Images/folder.png";
                    Uri imageUri = new Uri(source, UriKind.Relative);
                    BitmapImage imageBitmap = new BitmapImage(imageUri);
                    img.Source = imageBitmap;

                    TextBlock tb = new TextBlock();
                    tb.Text = proj.nom;
                    tb.VerticalAlignment = VerticalAlignment.Bottom;
                    tb.HorizontalAlignment = HorizontalAlignment.Center;
                    tb.Height = 70;

                    StackPanel sp = new StackPanel();
                    sp.Children.Add(img);
                    sp.Children.Add(tb);

                    UnProjet.Content = sp;

                    // Active un projet client lors de la sélection
                    UnProjet.Click += delegate(object sender, RoutedEventArgs e)
                    {
                        ToggleButton active = sender as ToggleButton;
                        foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(WrapProjets))
                        {
                            tgbt.IsChecked = false;
                        }
                        active.IsChecked = true;

                        // Value Non client
                        lblNomClient.Content = "";
                        lblNomClient.Content = proj.client.nom + " " + proj.client.prenom;

                        // Value Date création
                        lblDateCreation.Content = "";
                        lblDateCreation.Content = proj.creation;

                        // TODO : Value Statut Dernier Devis
                        lblStatut.Content = "?";

                        // Value Nombre de plans
                        lblNbPlans.Content = "";
                        lblNbPlans.Content = this.projetCAD.CountPlansProjet(proj.reference);

                        // Value Date modification
                        lblDateModification.Content = "";
                        lblDateModification.Content = proj.modification;

                        // Value Nom Commercial
                        lblNomCommercial.Content = "";
                        lblNomCommercial.Content = proj.commercial.nom + " " + proj.commercial.prenom;
                    };
                    WrapProjets.Children.Add(UnProjet);
                }
            }
        }

        private void Initialize_Menu_Wrapper()
        {
            // TODO : Faire en sorte que les boutons du menu soit activé lorsqu'on clic dessus
        }

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

                //Projet NewProjet = new Projet();

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
                        NewProjet.reference = generateKey(getClient, commercial);

                        this.projetCAD.InsertProjet(NewProjet.reference, NewProjet.client.nomprenom + " (" + i + ") ", NewProjet.client.reference, NewProjet.commercial.reference);

                        window.Close();

                        this.projetCAD.ListAllProjects();
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

        private string generateKey(Client client, Commercial comm)
        {
            string key = comm.nom.Substring(0, 1) + comm.prenom.Substring(0, 1) + client.nom.Substring(0, 1) + client.prenom.Substring(0, 1);
            Random rand = new Random();
            int temp = rand.Next(000000, 999999);
            key += temp.ToString();
            return key;
        }

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
            //planCAD.listAllPlansByProject();

            //ToggleButton btn = sender as ToggleButton;
            //if (Projets.Visibility == System.Windows.Visibility.Hidden)
            //{
            //    Projets.Visibility = System.Windows.Visibility.Visible;
            //    planCAD.listAllPlansByProject();
            //}

            //btn.IsChecked = true;
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
        // Sélectionner un client pour créer un projet
        private void Select_Nom_Client_Click(object sender, RoutedEventArgs e)
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
            if (Projets.Visibility == System.Windows.Visibility.Hidden)
            {
                Projets.Visibility = System.Windows.Visibility.Visible;
            }

            btn.IsChecked = true;
        }
        #endregion

        #region Tools
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
