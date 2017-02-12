using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Madera_MMB.View_Crtl;
using Madera_MMB.Model;
using Madera_MMB.Lib;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private View_Crtl.Authentification authentification { get; set; }
        private View_Crtl.GestionProjet gestionProjet { get; set; }
        private View_Crtl.GestionPlan gestionPlan { get; set; }
        private View_Crtl.GestionClient gestionClient { get; set; }
        private View_Crtl.ParametresClient parametresClient { get; set; }
        //private View_Crtl.ParametresPlan parametresPlan { get; set; }
        //private View_Crtl.GestionDevis gestionDevis { get; set; }
        //private View_Crtl.Modelisation modelisation { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Connexion connexion = new Connexion();
            Commercial commercialTest = new Commercial
                (
                    "003", 
                    "Chevalier",
                    "Christophe",
                    "monemail@gmail.com",
                    "mdp"
                );

            this.authentification = new Authentification();
            this.gestionProjet = new GestionProjet(connexion, commercialTest);
            this.gestionPlan = new GestionPlan();
            this.gestionClient = new GestionClient();
            this.parametresClient = new ParametresClient();

            // Démarrage de l'application sur la vue authentification
            Mainframe.Content = authentification;

            // Tmp : Commercial authentifié avec la référence 003
            string query = " SELECT * FROM projet WHERE refCommercial = '003'";
            connexion.SelectSQLiteQuery(query);

            Initialize_Listeners();
        }

        #region Initialisation
        private void Initialize_Listeners()
        {
            Initialize_Listeners_Auth();
            Initialize_Listeners_GestionProjet();
            Initialize_Listeners_GestionClient();
            Initialize_Listeners_ParametresClient();
            Initialize_Listeners_GestionPlan();
            //Initialize_Listeners_ParametresPlan();
            //Initialize_Listeners_Modelisation();
            //Initialize_Listeners_Devis();
        }
        #endregion

        #region Initialisation Auth
        private void Initialize_Listeners_Auth()
        {
            // Click sur le bouton valider authentification pour aller dans la Vue Gestion Projet
            authentification.BtnValiderAuth.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = gestionProjet;
            };
        }
        #endregion

        #region Initialisation Gestion Projet
        private void Initialize_Listeners_GestionProjet()
        {
            // Click sur le bouton listes des clients pour aller dans la Vue Gestion Client
            gestionProjet.BtnListeClient.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = gestionClient;
            };
            // Click sur le bouton ouvrir un projet client pour aller dans la Vue Gestion Plan
            gestionProjet.BtnOuvrirProjet.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = gestionPlan;
            };
            // Click sur le bouton créer un nouveau client pour aller dans la Vue Paramètre Client
            gestionProjet.BtnCreerClient.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = parametresClient;
            };
            // Click sur le bouton se déconnecter de l'application pour aller dans la Vue Auth
            gestionProjet.BtnSeDeconnecter.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = authentification;
            };
        }
        #endregion

        #region Initialisation Gestion Client
        private void Initialize_Listeners_GestionClient()
        {
            // Click sur le bouton éditer un client pour aller dans la Vue Paramètre Client
            gestionClient.BtnEditerClient.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = parametresClient;
            };
            // Click sur le bouton créer un nouveau client pour aller dans la Vue Paramètre Client
            gestionClient.BtnCreerClient.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = parametresClient;
            };
            // Click sur le bouton retour pour aller dans la Vue Gestion Projet
            gestionClient.BtnRetourListeProjet.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = gestionProjet;
            };
        }
        #endregion

        #region Initialisation Paramètres Client
        private void Initialize_Listeners_ParametresClient()
        {
            // Click sur le bouton valider paramètres client pour aller dans la Vue Gestion Client
            parametresClient.BtnConfirmerClient.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = gestionClient;
            };
            // Click sur le bouton retour pour aller dans la Vue Gestion Client
            parametresClient.BtnRetourListeProjet.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = gestionClient;
            };
        }
        #endregion

        #region Initialisation Gestion Plan
        private void Initialize_Listeners_GestionPlan()
        {
            // Click sur le bouton créer un nouveau plan pour aller dans la Vue Paramètres Plan
            //gestionPlan.BtnCréerPlan.Click += delegate(object sender, RoutedEventArgs e)
            //{
            //    Mainframe.Content = parametresPlan;
            //};
            // Click sur le bouton ouvrir un plan pour aller dans la Vue Modélisation
            //gestionPlan.BtnOuvrirPlan.Click += delegate(object sender, RoutedEventArgs e)
            //{
            //    Mainframe.Content = modelisation;
            //};
            // Click sur le bouton consulter le devis pour aller dans la Vue Gestion Devis
            //gestionPlan.BtnConsulterDevis.Click += delegate(object sender, RoutedEventArgs e)
            //{
            //    Mainframe.Content = gestionDevis;
            //};
            // Click sur le bouton copier plan pour aller dans la Vue ???
            gestionPlan.BtnCopierPlan.Click += delegate(object sender, RoutedEventArgs e)
            {

            };
            // Click sur le bouton retour liste de projets pour aller dans la Vue Gestion Projet
            gestionPlan.BtnRetour.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = gestionProjet;
            };
        }
        #endregion

        //#region Initialisation Paramètres Plan
        //private void Initialize_Listeners_ParametresPlan()
        //{
        //    // Click sur le bouton confirmer paramètres plan pour aller dans la Vue Modélisation
        //    parametresPlan.BtnConfirmerParamPlan.Click += delegate(object sender, RoutedEventArgs e)
        //    {
        //        Mainframe.Content = modelisation;
        //    };
        //    // Click sur le bouton retour liste des plans pour aller dans la Vue Gestion Plan
        //    parametresPlan.BtnRetour.Click += delegate(object sender, RoutedEventArgs e)
        //    {
        //        Mainframe.Content = gestionPlan;
        //    };
        //}
        //#endregion

        //#region Initialisation Modélisation
        //private void Initialize_Listeners_Modelisation()
        //{
        //    // Click sur le bouton quitter modélisation pour aller dans la Vue Gestion Plan
        //    modelisation.BtnQuitterModelisation.Click += delegate(object sender, RoutedEventArgs e)
        //    {
        //        Mainframe.Content = GestionPlan;
        //    };
        //}
        //#endregion

        //#region Initialisation Gestion Devis
        //private void Initialize_Listeners_Devis()
        //{
        //    // Click sur le bouton retour liste des plans pour aller dans la Vue Gestion Plan
        //    gestionDevis.BtnRetour.Click += delegate(object sender, RoutedEventArgs e)
        //    {
        //        Mainframe.Content = GestionPlan;
        //    };
        //    // Click sur le bouton exporter un devis client pour ???
        //    gestionDevis.BtnExportDevis.Click += delegate(object sender, RoutedEventArgs e)
        //    {

        //    };
        //}
        //#endregion
    }
}