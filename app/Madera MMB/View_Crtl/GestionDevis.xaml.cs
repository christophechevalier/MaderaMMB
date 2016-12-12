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

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour GestionDevis.xaml
    /// </summary>

    //DEVIS
    public partial class GestionDevis : Page
    {
        #region Properties

        #endregion

        #region Constructeur
        public GestionDevis()
        {
            InitializeComponent();
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
        #endregion

        #region Listeners
        //EVENT ON "Changer l'état"
        private void BtnChangeStatusDevis_Click(object sender, RoutedEventArgs e)
        {
            //SHOW MODAL
        }

        //COMBO BOX DISPLAY CHOICE
        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize Status list
            List<string> data = new List<string>();
            data.Add("Brouillon");
            data.Add("Accepté");
            data.Add("Refusé");
            data.Add("En attente");
            data.Add("Facturé");

            // ... Get the ComboBox reference.
            var comboBox = sender as ComboBox;

            // ... Assign the ItemsSource to the List.
            comboBox.ItemsSource = data;

            // ... Make the first item selected.
            comboBox.SelectedIndex = 0;
        }

        //DISPLAY SELECTED CHOICE IN COMBO BOX
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get the ComboBox.
            var comboBox = sender as ComboBox;

            // ... Set SelectedItem as Window Title.
            string value = comboBox.SelectedItem as string;
            this.Title = "Selected: " + value;
        }


        //EVENT ON "Appliquer remise"
        private void BtnAppliquerRemise_Click(object sender, RoutedEventArgs e)
        {
            //SHOW MODAL
        }

        //EVENT ON "Export"
        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {

        }

        //EVENT ON "Voir Devis technique"
        private void BtnVoirDT_Click(object sender, RoutedEventArgs e)
        {
            //TOGGLE GRID AffichageDevis

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

    //MODAL REMISE
    public partial class ModalRemise : Window
    {

    }

    //MODAL ETAT
    public partial class ModalEtat : Window
    {

    }
}
