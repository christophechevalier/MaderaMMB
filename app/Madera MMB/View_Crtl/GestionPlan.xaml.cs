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
        private ProjetCAD projetCAD { get; set; }
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
            //DataContext = connexion;

            planCAD = new PlanCAD(this.connexion, this.projet);
            DataContext = planCAD;

            // Appel des méthodes dans le ctor
            InitializeComponent();
        }
        #endregion

        #region Listeners
        private void BtnCréerPlan_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        }

        private void BtnOuvrirPlan_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        }

        private void BtnConsulterDevis_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        }

        private void BtnCopierPlan_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
        }

        private void Btn_Select_Plan_Projet_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;
            Plan plan = (Plan)btn.DataContext;

            foreach (ToggleButton tgbt in FindVisualChildren<ToggleButton>(WrapPlans))
            {
                tgbt.IsChecked = false;
            }
            btn.IsChecked = true;

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
        }
        #endregion

        #region Tools
        public string generateKey(Projet projet)
        {
            int count = 0;
            for (int i = 0; i < planCAD.Plans.Count; i++)
            {
                i++;
            }

            string key = projet.reference + "-P" + count;
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
