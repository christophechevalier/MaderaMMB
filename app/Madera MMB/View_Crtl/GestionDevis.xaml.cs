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
        private Devis devis { get; set; }
        private Commercial commercial { get; set; }
        private Connexion connexion { get; set; }
        private float totalTTC { get; set; }
        private float totalHT { get; set; }
        private float totalinitialTTC { get; set; }
        private float totalinitialHT { get; set; }
        private bool remise { get; set; }
        private SelectModalWindow window { get; set; }
        #endregion

        #region Constructeur
        public GestionDevis(Connexion conn, Plan plan, Commercial commercial, Client client)
        {
            InitializeComponent();

            this.plan = plan;
            this.commercial = commercial;
            this.client = client;
            connexion = conn;
            devisCAD = new DevisCAD(connexion, plan);

            this.DataContext = devisCAD;

            if(devisCAD.getDevisByPlan(plan) == null)
                this.devis = new Devis(plan);
            else
                this.devis = devisCAD.getDevisByPlan(plan);

            remise = false;
            Initialize_Labels();
            Initialize_Devis();
            Initialize_Dialog_Modification_Devis();
        }
        #endregion

        #region Initialisation Container
        /// <summary>
        /// Méthode (Ré)initialisant le contenu des des différents labels de la vue
        /// </summary>
        private void Initialize_Labels()
        {
            //RETURN VALUES
            //CLIENT
            nomClient.Content = "Nom du client : ";
            nomClient.Content += client.nom; 

            //COMMERCIAL
            nomCommercial.Content = "Nom du commercial : ";
            nomCommercial.Content += commercial.nom; 

            //DEVIS
            dateCreation.Content = "Date de création : ";
            dateCreation.Content += plan.creation.ToString(); 

            lastUpdate.Content = "Date de dernière mise à jour : ";
            lastUpdate.Content += plan.modification.ToString();

            currentStatus.Content = "Statut actuel : ";
            currentStatus.Content += devis.etat;

            nomClientDevis.Content = "";
            nomClientDevis.Content += client.nom; 
            referenceClientDevis.Content = "";
            referenceClientDevis.Content += client.reference;
            adresseClientDevis.Content = "";
            adresseClientDevis.Content += client.adresse;

            //COMMERCIAL
            nomCommerialDevis.Content = "";
            nomCommerialDevis.Content += commercial.nom;
            referenceCommercialDevis.Content = "";
            referenceCommercialDevis.Content += commercial.reference;

            //PLAN
            dateDevis.Content = "";
            dateDevis.Content += devis.creation;
            planDevis.Content = "";
            planDevis.Content += devis.plan.label;

        }
        private void Initialize_Devis()
        {
            this.totalHT = 0;
            this.totalTTC = 0;
            /// COUPE PRINCIPE ///
            this.ListeComposants.Content += "Base " + this.plan.coupePrincipe.label + " dimensions " + this.plan.coupePrincipe.longueur + "x" + this.plan.coupePrincipe.largeur + "\n";
            this.Quantité.Content += "1\n";
            int coupeprixht = this.plan.coupePrincipe.prixHT;
            totalHT += coupeprixht;
            this.PrixUnitaire.Content += coupeprixht + "\n";
            this.PrixHT.Content += coupeprixht + "\n";
            float coupeprixttc = coupeprixht + ((coupeprixht * 20) / 100);
            totalTTC += coupeprixttc;
            this.PrixTTC.Content += coupeprixttc.ToString() + "\n";

            /// COUVERTURE ///
            this.ListeComposants.Content += "Couverture " + this.plan.couverture.type + "\n";
            this.Quantité.Content += "1\n";
            float couvprixht = this.plan.couverture.prixHT * this.plan.coupePrincipe.longueur * this.plan.coupePrincipe.largeur;
            totalHT += couvprixht;
            this.PrixUnitaire.Content += couvprixht + "\n";
            this.PrixHT.Content += couvprixht + "\n";
            float couvprixttc = couvprixht + ((couvprixht * 20) / 100);
            totalTTC += couvprixttc;
            this.PrixTTC.Content += couvprixttc.ToString() + "\n";

            /// PLANCHER ///
            this.ListeComposants.Content += "Plancher " + this.plan.plancher.type + "\n";
            this.Quantité.Content += "1\n";
            float planchprixht = this.plan.plancher.prixHT * this.plan.coupePrincipe.longueur * this.plan.coupePrincipe.largeur;
            totalHT += planchprixht;
            this.PrixUnitaire.Content += planchprixht + "\n";
            this.PrixHT.Content += planchprixht + "\n";
            float planchprixttc = planchprixht + ((planchprixht * 20) / 100);
            totalTTC += planchprixttc;
            this.PrixTTC.Content += planchprixttc.ToString() + "\n";

            /// MODULES ///
            int cpt = 0;
            int aggregatHT = 0;
            for(int i=0 ; i <= plan.modules.Count-1 ; i++)
            {
                if(i != plan.modules.Count-1)
                {                    
                    if (plan.modules[i].meta.label == plan.modules[i + 1].meta.label)
                    {
                        cpt++;
                        aggregatHT += plan.modules[i].meta.prixHT + plan.modules[i + 1].meta.prixHT;
                    }
                    else
                    {
                        PrixUnitaire.Content += plan.modules[i].meta.prixHT + "\n";
                        this.ListeComposants.Content += plan.modules[i].meta.label + "\n";
                        this.Quantité.Content += cpt + "\n";
                        if(aggregatHT!= 0)
                        {
                            totalHT += aggregatHT;
                            this.PrixHT.Content += aggregatHT + "\n";
                            float aggregatTTC = aggregatHT + ((aggregatHT * 20) / 100);
                            totalTTC += aggregatTTC;
                            this.PrixTTC.Content += aggregatTTC + "\n";
                        }
                        else
                        {
                            totalHT += plan.modules[i].meta.prixHT;
                            this.PrixHT.Content += plan.modules[i].meta.prixHT + "\n";
                            totalTTC += plan.modules[i].meta.prixHT + ((plan.modules[i].meta.prixHT * 20) / 100);
                            this.PrixTTC.Content += plan.modules[i].meta.prixHT + ((plan.modules[i].meta.prixHT*20)/100)+ "\n";
                        }
                        aggregatHT = 0;
                        cpt = 0;
                    }
                }
                else
                {
                    if (plan.modules[i].meta.label == plan.modules[i - 1].meta.label)
                    {
                        cpt++;
                        aggregatHT += plan.modules[i].meta.prixHT + plan.modules[i - 1].meta.prixHT;
                    }
                    this.ListeComposants.Content += plan.modules[i].meta.label + "\n";
                    this.Quantité.Content += cpt + "\n";
                    if (aggregatHT != 0)
                    {
                        totalHT += aggregatHT;
                        this.PrixHT.Content += aggregatHT + "\n";
                        float aggregatTTC = aggregatHT + ((aggregatHT * 20) / 100);
                        totalTTC += aggregatTTC;
                        this.PrixTTC.Content += aggregatTTC + "\n";
                    }
                    else
                    {
                        totalHT += plan.modules[i].meta.prixHT;
                        this.PrixHT.Content += plan.modules[i].meta.prixHT + "\n";
                        totalTTC += plan.modules[i].meta.prixHT + ((plan.modules[i].meta.prixHT * 20) / 100);
                        this.PrixTTC.Content += plan.modules[i].meta.prixHT + ((plan.modules[i].meta.prixHT * 20) / 100) + "\n";
                    }
                }
            }

            /// TOTAUX ///
            LabTotalHT.Content = "";
            LabTotalHT.Content = totalHT + " €";

            LabTotalTTC.Content = "";
            LabTotalTTC.Content = totalTTC + " €";
        }
        private void Initialize_Dialog_Modification_Devis()
        {
            this.window = new SelectModalWindow();
            window.TitleLabel.Content = "Sélectionner l'état du devis";
            window.DataSelect.Text = "-- Choisir un état --";
            window.DataSelect.Items.Add("Accepté");
            window.DataSelect.Items.Add("Refusé");
            window.DataSelect.Items.Add("Facturé");
            window.DataSelect.Items.Add("En attente de paiement");
            window.DataSelect.Items.Add("Nouveau");
            window.DataSelect.Items.Add("Brouillon");

            window.Retour.Click += delegate(object sender, RoutedEventArgs e)
            {
                window.Close();
            };

            window.Valider.Click += delegate(object sender, RoutedEventArgs e)
            {
                if(window.DataSelect.SelectedItem != null && (string)window.DataSelect.SelectedItem != "-- Choisir un état --")
                {
                    this.devis.etat = window.DataSelect.SelectedItem.ToString();
                    currentStatus.Content = "Statut actuel : ";
                    currentStatus.Content += devis.etat;
                    this.devisCAD.changeStatusDevis(this.devis, this.devis.etat);
                    window.Close();
                }
                else
                    MessageBox.Show("Un état doit être sélectionné");
            };
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
                if(window.txtAnswer.Value != null)
                {
                    int percentage = (int)window.txtAnswer.Value;
                    this.Remise.Content = "";
                    this.Remise.Content += "Remise : " + window.txtAnswer.Value + " %";

                    if (remise == false)
                    {
                        this.totalinitialHT = totalHT;
                        this.totalinitialTTC = totalTTC;
                    }

                    totalHT = totalinitialHT - ((totalinitialHT * percentage) / 100);
                    totalTTC = totalHT + ((totalHT * 20) / 100);

                    LabTotalHT.Content = "";
                    LabTotalHT.Content = totalHT + " €";

                    LabTotalTTC.Content = "";
                    LabTotalTTC.Content = totalTTC + " €";

                    remise = true;
                    window.Close();
                }
                else
                {
                    MessageBox.Show("Aucune valeur n'est renseignée");
                }
                
            };

            window.ShowDialog();
        }
        private void TreeView_Loaded(object sender, RoutedEventArgs e)
        {
            var coupe = sender as TreeView;

            TreeViewItem coupeItem = new TreeViewItem();
            coupeItem.Header = "Base " + plan.coupePrincipe.label + " " + plan.coupePrincipe.longueur + "x" + plan.coupePrincipe.largeur;
            coupeItem.IsExpanded = true;

            TreeViewItem couvItem = new TreeViewItem();
            couvItem.Header = "Couverture " + this.plan.couverture.type;
            couvItem.IsExpanded = true;

            TreeViewItem planchItem = new TreeViewItem();
            planchItem.Header = "Plancher " + this.plan.plancher.type;
            planchItem.IsExpanded = true;

            coupeItem.Items.Add(couvItem);
            coupeItem.Items.Add(planchItem);

            List<Module> firstlist = plan.modules;
            List<Module> thirdList = new List<Module>();
            int i;
            bool trouv = false;

            foreach (Module mod in firstlist)
            {
                i = 0;
                List<Module> secondlist = new List<Module>(firstlist);

                foreach (Module mod3 in thirdList)
                {
                    if (mod.meta.label == mod3.meta.label)
                    {
                        trouv = true;
                    }
                }
                thirdList.Add(mod);

                if (!trouv && mod.parent == null)
                {
                    for (int x = 0; x <= secondlist.Count - 1; x++)
                    {
                        if (secondlist[x].meta.label == mod.meta.label)
                        {
                            i++;
                            secondlist[x] = null;
                        }
                        else
                        {
                            if (secondlist[x].parent == null || secondlist[x].parent.label != mod.meta.label)
                            {
                                secondlist[x] = null;
                            }
                        }
                    }

                    TreeViewItem modparent = new TreeViewItem();
                    modparent.Header = mod.meta.label + " X" + i;
                    modparent.IsExpanded = true;

                    foreach (Module mod2 in secondlist)
                    {
                        if (mod2 != null)
                        {
                            TreeViewItem modenfant = new TreeViewItem();
                            modenfant.Header = mod2.meta.label;
                            modenfant.IsExpanded = true;
                            modparent.Items.Add(modenfant);
                            thirdList.Add(mod2);
                        }
                    }
                    coupeItem.Items.Add(modparent);
                }
                trouv = false;

            }
            coupe.Items.Add(coupeItem);
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
            this.window.ShowDialog();
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
