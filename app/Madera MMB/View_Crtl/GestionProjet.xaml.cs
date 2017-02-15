using Madera_MMB.Lib.Tools;
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
        private Projet proj { get; set; }
        #endregion

        #region Constructeur
        public GestionProjet(Connexion co, Commercial com)
        {
            // Instanciations
            InitializeComponent();
            connexion = co;
            commercial = com;
            projetCAD = new ProjetCAD(this.connexion, this.commercial);

            // Appel des méthodes dans le ctor
            Initialize_Client_Wrapper();
            Initialize_Menu_Wrapper();
        }
        #endregion

        #region Initialisation Container
        /// <summary>
        /// Méthode pour parcourir la liste des projets existant en bdd
        /// Pour chaque projet sélectionné, on aura le nom du client, le nom d'un commercial, la date de création/modification, 
        /// et le nombre de plans associés
        /// </summary>
        private void Initialize_Client_Wrapper()
        {
            if (projetCAD.projets != null)
            {
                foreach (var proj in projetCAD.projets)
                {
                    ToggleButton UnClient = new ToggleButton();
                    UnClient.Background = Brushes.White;
                    UnClient.Width = 120;
                    UnClient.Height = 120;
                    Thickness margin = UnClient.Margin;
                    margin.Left = 20;
                    margin.Right = 20;
                    margin.Bottom = 20;
                    margin.Top = 20;
                    UnClient.Margin = margin;

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

                    UnClient.Content = sp;

                    // Active un projet client lors de la sélection
                    UnClient.Click += delegate(object sender, RoutedEventArgs e)
                    {
                        ToggleButton active = sender as ToggleButton;
                        foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(WrapClients))
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
                        lblNbPlans.Content = projetCAD.countPlansProjet(proj.reference);

                        // Value Date modification
                        lblDateModification.Content = "";
                        lblDateModification.Content = proj.modification;

                        // Value Nom Commercial
                        lblNomCommercial.Content = "";
                        lblNomCommercial.Content = proj.commercial.nom + " " + proj.commercial.prenom;
                    };
                    WrapClients.Children.Add(UnClient);
                }
            }
        }

        private void Initialize_Menu_Wrapper()
        {
            // TODO : Faire en sorte que les boutons du menu soit activé lorsqu'on clic dessus
        }

        private void Initialize_Dialog_Creation_Projet()
        {
            var window = new SelectModalWindow();
            window.Title = "Nouveau Projet ";
            window.Titlelabel.Content = " Sélectionner un client pour votre nouveau projet ";

            window.Retour.Click += delegate(object sender, RoutedEventArgs e)
            {
                window.Close();
            };

            window.Valider.Click += delegate(object sender, RoutedEventArgs e)
            {
                window.Close();
            };

            window.DataSelect.Text = "-- Choisir un client --";
            window.DataSelect.Items.Add("CERISIER Madeleine");
            window.DataSelect.Items.Add("CHENES Yves");
            window.DataSelect.Items.Add("NOISETTIER Louise");
            window.DataSelect.Items.Add("TILLEULE Bertrand");
            window.DataSelect.Items.Add("SAPIN Léo");
            window.DataSelect.Items.Add("PORIER José");
            window.DataSelect.Items.Add("PRUNIER Marion");

            window.ShowDialog();
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
            if (Clients.Visibility == System.Windows.Visibility.Hidden)
            {
                Clients.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {

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
