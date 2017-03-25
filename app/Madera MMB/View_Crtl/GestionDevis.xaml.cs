using Madera_MMB.CAD;
using Madera_MMB.Lib;
using Madera_MMB.Lib.Tools;
using Madera_MMB.Model;
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
    /// Logique d'interaction pour GestionDevis.xaml
    /// * Règles de gestion Devis :
    /// - Il ne peut y avoir qu’un seul devis estimatif et dossier technique par plan
    /// - L’état du devis ne peut être uniquement modifié par un commercial ayant les privilèges
    /// - Un devis n’a pas obligatoirement de remise
    /// - Un devis a un workflow spécifique :
    /// Brouillon ➡ Valider/Accepter par le client ➡ Facturer ➡ Clôturer
    ///           ➡ Refuser                        ➡ En Attente
    /// - Un devis brouillon valider ou refuser peut être modifié mais retourne à un état brouillon
    /// </summary>

    public partial class GestionDevis : Page
    {
        #region Properties
        private Plan plan { get; set; }
        private DevisCAD devisCAD { get; set; }
        private Client client { get; set; }
        private string nomCommerc { get; set; }
        private Connexion connexion { get; set; }
        #endregion

        #region Constructeur
        public GestionDevis(Connexion conn, Plan plan, string nomComm, Client client)
        {
            InitializeComponent();
            this.plan = plan;
            nomCommerc = nomComm;
            this.client = client;
            connexion = conn;

            devisCAD = new DevisCAD(connexion, plan);
            Initialize_Labels();
            Initialize_Devis();
        }
        #endregion

        #region Initialisation Container
        private void Initialize_Labels()
        {
            //RETURN VALUES
            //CLIENT
            nomClient.Content += "BOLZINGER Gabriel"; //RETURN REQUEST

            //COMMERCIAL
            nomCommercial.Content += "CROCCO David"; //RETURN REQUEST

            //DEVIS
            dateCreation.Content += "01/01/2016"; //RETURN REQUEST
            lastUpdate.Content += "20/05/2016"; //RETURN REQUEST
            currentStatus.Content += "Accepté"; //RETURN REQUEST


        }
        private void Initialize_Devis()
        {
            //TOP INFORMATION
            //CLIENT
            nomClientDevis.Content += "BOLZINGER Gabriel"; //RETURN REQUEST
            referenceClientDevis.Content += "#43654782"; //RETURN REQUEST
            adresseClientDevis.Content += "646 Route de Paris 31000 Toulouse";

            //COMMERCIAL
            nomCommerialDevis.Content += "CROCCO David"; //RETURN REQUEST
            referenceCommercialDevis.Content += "#54254312"; //RETURN REQUEST

            //PLAN
            dateDevis.Content += "20/05/2016";
            planDevis.Content += "Plan 1 Base Rectangle";

            //ARRAY RETURN BY REQUEST GET ALL COMPOSANT FROM PLAN
            string[,] listeComposants = { { "Base en L; Dimension 15x10x5", "1", "1500", "1800", "1800" }, 
                                     { "Couverture tuiles", "1", "2500", "3000", "3000" } };
            int nbrcomposant = 0;
            nbrcomposant = listeComposants.Length;

            for (int i = 0; i < nbrcomposant; i++)
            {
                //string liste_nom_composant = listeComposants[i,0]; //composants.nom
                //string quantitéçcomposant = listeComposants[i,1]; //composant.quantité
                //string prixHT = listeComposants[i,2]; //composant.prixHT
                //string prixTTC = listeComposants[i,3]; //composant.prixTTC
                //string prixTotal_composant = listeComposants[i,4]; //composant.prixTotalcomposant
            }

        }
        private void Initialize_Dialog_Modification_Devis()
        {
            var window = new SelectModalWindow();
            window.Titlelabel.Content = " Sélectionner l'état du devis";

            window.Retour.Click += delegate(object sender, RoutedEventArgs e)
            {
                window.Close();
            };

            window.Valider.Click += delegate(object sender, RoutedEventArgs e)
            {
                window.Close();
            };

            window.DataSelect.Text = "-- Choisir un état --";
            window.DataSelect.Items.Add("Accepté");
            window.DataSelect.Items.Add("Refusé");
            window.DataSelect.Items.Add("Facturé");
            window.DataSelect.Items.Add("En attente de paiement");
            window.DataSelect.Items.Add("Nouveau");
            window.DataSelect.Items.Add("Brouillon");

            window.ShowDialog();
        }
        private void Initialize_Dialog_Remise_Devis()
        {
            var window = new InputModalWindow();

            window.Retour.Click += delegate(object sender, RoutedEventArgs e)
            {
                window.Close();
            };

            window.Valider.Click += delegate(object sender, RoutedEventArgs e)
            {
                window.Close();
            };

            window.ShowDialog();
        }
        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            // ... Create a TreeViewItem.
            TreeViewItem item = new TreeViewItem();
            item.Header = "Porte";
            item.ItemsSource = new string[] { "Porte Bois 200x70", "Porte Metal 200x150", "Porte PVC 200x90" };
            item.IsExpanded = true;

            // ... Create a second TreeViewItem.
            TreeViewItem item2 = new TreeViewItem();
            item2.Header = "Mur";
            item2.ItemsSource = new string[] { "Mur 500x250", "Mur 1500x750", "Mur 2500x1000" };
            item2.IsExpanded = true;

            // ... Create a third TreeViewItem.
            TreeViewItem item3 = new TreeViewItem();
            item3.Header = "Fenetre";
            item3.ItemsSource = new string[] { "Double 75x50", "Triple 80x90", "Simple 150x90"};
            item3.IsExpanded = true;

            TreeViewItem main = new TreeViewItem();
            main.Header = "Base en L Dimension 15x10x5";
            main.IsExpanded = true;

            // ... Get TreeView reference and add both items.
            var coupe = sender as TreeView;
            
            main.Items.Add(item);
            main.Items.Add(item2);
            main.Items.Add(item3);

            coupe.Items.Add(main);

        }
        private void TreeView_SelectedItemChanged(object sender,
            RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = sender as TreeView;

            // ... Determine type of SelectedItem.
            if (tree.SelectedItem is TreeViewItem)
            {
                // ... Handle a TreeViewItem.
                var item = tree.SelectedItem as TreeViewItem;
                this.Title = "Selected header: " + item.Header.ToString();
            }
            else if (tree.SelectedItem is string)
            {
                // ... Handle a string.
                this.Title = "Selected: " + tree.SelectedItem.ToString();
            }
        }
        #endregion

        #region Listeners
        // Modification de l'état du devis
        private void BtnChangeStatusDevis_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Initialize_Dialog_Modification_Devis();
        }

        private void BtnAppliquerRemise_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Initialize_Dialog_Remise_Devis();
        }

        //EVENT ON "Export"
        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {

        }

        //EVENT ON "Voir Devis technique"
        private void BtnVoirDT_Click(object sender, RoutedEventArgs e)
        {
            //TOGGLE GRID AffichageDevis
            if (AffichageDevis.Visibility == Visibility.Visible)
            {
                AffichageDevis.Visibility = System.Windows.Visibility.Hidden;
                AfficherDevisTechnique.Visibility = System.Windows.Visibility.Visible;
                BtnVoirDT.Content = "Voir Devis Détaillé";
            }
            else
            {
                AffichageDevis.Visibility = System.Windows.Visibility.Visible;
                AfficherDevisTechnique.Visibility = System.Windows.Visibility.Hidden;
                BtnVoirDT.Content = "Voir Dossier Technique";
            }
            
        }

        //EVENT ON "Retour"
        private void BtnRetour_Click(object sender, RoutedEventArgs e)
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
