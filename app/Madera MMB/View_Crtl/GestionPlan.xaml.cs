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
using System.Diagnostics;
using System.IO;
using Madera_MMB.Model;
using System.ComponentModel;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour GestionClient.xaml
    /// * Règles de gestion Plan :
    /// - On ne peut pas supprimer un plan existant
    /// - On peut consulter, modifier un plan
    /// - On peut créer un ou plusieurs plan pour un même projet
    /// - On peut copier un plan à la fois
    /// - On peut renommer un plan que si son nom est unique
    /// - Pour créer un plan, il faut renseigner au minimum ses paramètres
    /// </summary>
    public partial class GestionPlan : Page
    {
        #region Properties
        private Connexion connexion { get; set; }
        private Projet projet { get; set; }
        private PlanCAD planCAD { get; set; }
        #endregion

        #region Constructeur
        /// <summary>
        /// Constructeur qui prend en paramètre la connexion et le projet sélectionné
        /// </summary>
        /// <param name="co"></param>
        /// <param name="unprojet"></param>
        public GestionPlan(Connexion co, Projet unprojet)
        {
            // Instanciations
            InitializeComponent();
            connexion = co;
            projet = unprojet;
            DataContext = connexion;

            planCAD = new PlanCAD(this.connexion, this.projet);

            // Appel des méthodes dans le ctor
            InitializeComponent();
            Initialize_Plan_Wrapper();
            planCAD.ListAllPlansByProject();
        }
        #endregion

        #region Initialisation Container
        /// <summary>
        /// Méthode pour parcourir la liste des plans d'un projet existant en bdd
        /// Pour chaque plan sélectionné, on aura la date de création/modification et sa base plan, 
        /// </summary>
        private void Initialize_Plan_Wrapper()
        {
            if (planCAD.plans != null)
            {
                foreach (var plan in planCAD.plans)
                {
                    ToggleButton UnPlan = new ToggleButton();
                    UnPlan.Background = Brushes.White;
                    UnPlan.Width = 120;
                    UnPlan.Height = 120;
                    Thickness margin = UnPlan.Margin;
                    margin.Left = 20;
                    margin.Right = 20;
                    margin.Bottom = 20;
                    margin.Top = 20;
                    UnPlan.Margin = margin;

                    Image img = new Image();
                    img.Width = 70;
                    img.Height = 70;
                    img.VerticalAlignment = VerticalAlignment.Top;
                    string source = "../Lib/Images/house_black.png";
                    Uri imageUri = new Uri(source, UriKind.Relative);
                    BitmapImage imageBitmap = new BitmapImage(imageUri);
                    img.Source = imageBitmap;

                    TextBlock tb = new TextBlock();
                    tb.Text = plan.label;
                    tb.VerticalAlignment = VerticalAlignment.Bottom;
                    tb.HorizontalAlignment = HorizontalAlignment.Center;
                    tb.Height = 50;

                    StackPanel sp = new StackPanel();
                    sp.Children.Add(img);
                    sp.Children.Add(tb);

                    UnPlan.Content = sp;

                    // Active un plan lors de la sélection
                    UnPlan.Click += delegate(object sender, RoutedEventArgs e)
                    {
                        ToggleButton active = sender as ToggleButton;
                        foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(WrapPlans))
                        {
                            tgbt.IsChecked = false;
                        }
                        active.IsChecked = true;

                        // Value Date création
                        lblDateCreation.Content = "";
                        lblDateCreation.Content = plan.creation;

                        // TODO : Value Statut Dernier Devis
                        lblStatut.Content = "?";

                        // Value Date modification
                        lblDateModification.Content = "";
                        lblDateModification.Content = plan.modification;

                        // Value Base Plan
                        lblBasePlan.Content = "";
                        lblBasePlan.Content = "?";
                    };
                    WrapPlans.Children.Add(UnPlan);
                }
            }

        }
        #endregion

        #region Listeners
        private void BtnCréerPlan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOuvrirPlan_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnConsulterDevis_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCopierPlan_Click(object sender, RoutedEventArgs e)
        {

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
