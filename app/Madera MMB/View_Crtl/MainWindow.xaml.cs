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
using System.Diagnostics;
using Madera_MMB.View_Crtl;
using Madera_MMB.Model;
using Madera_MMB.Lib;
using Madera_MMB.CAD;

namespace Madera_MMB.View_Crtl
{
    public partial class MainWindow : Window
    {
        #region Properties
        private Connexion connexion;
        private bool mySQLSync { get; set; }
        private View_Crtl.Authentification authentification { get; set; }
        private View_Crtl.GestionProjet gestionProjet { get; set; }
        private View_Crtl.GestionPlan gestionPlan { get; set; }
        private View_Crtl.GestionClient gestionClient { get; set; }
        private View_Crtl.ParametresClient parametresClient { get; set; }
        private View_Crtl.ParametresPlan parametresPlan { get; set; }
        //private View_Crtl.GestionDevis gestionDevis { get; set; }
        //private View_Crtl.Modelisation modelisation { get; set; }
        #endregion

        #region Ctor
        /// <summary>
        /// Constructeur du main window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            initSynchro();

            /// Test SYNCHRO import ///
            connexion.SyncCommMySQL();
            connexion.SyncParamPlan();
            connexion.SyncClient();
            connexion.SyncMetamodules();
            connexion.SyncMetaslot();
            connexion.SyncAssocMetaModuleMetaslot();

            /// Test CAD avec nouvelles données ///
            CommercialCAD commCAD = new CommercialCAD(connexion);
            ClientCAD clientCAD = new ClientCAD(connexion);
            CoupePrincipeCAD coupeCAD = new CoupePrincipeCAD(connexion);
            CouvertureCAD couvCAD = new CouvertureCAD(connexion);
            GammeCAD gamCAD = new GammeCAD(connexion);
            PlancherCAD plancherCAD = new PlancherCAD(connexion);

            connexion = new Connexion();
            //this.gestionClient = new GestionClient(connexion);
            //this.parametresClient = new ParametresClient();
            Mainframe.Content = gestionClient;

            Commercial commercialTest = new Commercial
                (
                    "COM003",
                    "Chevalier",
                    "Christophe",
                    "monemail@gmail.com",
                    "mdp"
                );

            this.authentification = new Authentification();
            this.gestionProjet = new GestionProjet(connexion, commercialTest);
            //this.gestionPlan = new GestionPlan(connexion, gestionProjet.proj);
            //this.parametresPlan = new ParametresPlan(connexion);
            Mainframe.Content = gestionProjet;

            /// Test SYNCHRO export ///
            connexion.ExpClients();
            connexion.ExpProjets();
            connexion.ExpPlans();
            connexion.ExpModules();

            Initialize_Listeners();
        }
        #endregion

        #region Process Synchro
        /// <summary>
        ///  Méthode testant les possibilitées de connexion aux bases de données locale et distante 
        /// </summary>
        private void initSynchro()
        {
            this.connexion = new Connexion();
            if (connexion.MySQLconnected == false)
            {
                mySQLSync = false;
                MessageBox.Show("Mode déconnecté");
            }
            else if (!connexion.SQLiteconnected)
            {
                MessageBox.Show("Base innaccessible ! Veuillez contacter l'administrateur. ");
                Application.Current.Shutdown();
            }
        }
        #endregion

        #region Initialisation
        /// <summary>
        /// Méthode qui contient l'ensemble des listeners pour chaque vue
        /// </summary>
        private void Initialize_Listeners()
        {
            Initialize_Listeners_Auth();
            Initialize_Listeners_GestionProjet();
            //Initialize_Listeners_GestionClient();
            //Initialize_Listeners_ParametresClient();
            //Initialize_Listeners_ParametresPlan();
            //Initialize_Listeners_Modelisation();
            //Initialize_Listeners_Devis();
        }
        #endregion

        #region Initialisation Auth
        /// <summary>
        /// Méthode qui contient l'ensemble des listeners de la vue authentification
        /// </summary>
        private void Initialize_Listeners_Auth()
        {
            // Click sur le bouton valider authentification pour aller dans la Vue Gestion Projet
            authentification.BtnValiderAuth.Click += delegate(object sender, RoutedEventArgs e)
            {
                Initialize_Listeners_GestionProjet();
                Mainframe.Content = gestionProjet;
            };
        }
        #endregion

        #region Initialisation Gestion Projet
        /// <summary>
        /// Méthode qui contient l'ensemble des listeners de la vue gestion projet
        /// </summary>
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
                Commercial commercialTest = new Commercial
                    (
                        "COM003",
                        "Chevalier",
                        "Christophe",
                        "monemail@gmail.com",
                        "mdp"
                    );

                if (gestionProjet.proj != null)
                {
                    this.gestionPlan = new GestionPlan(connexion, gestionProjet.proj);
                    Initialize_Listeners_GestionPlan();
                    Mainframe.Content = gestionPlan;
                }
                else
                {
                    MessageBox.Show("Vous devez sélectionner un projet avant de l'ouvrir !");
                }
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
        /// <summary>
        /// Méthode qui contient l'ensemble des listeners de la vue gestion client
        /// </summary>
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
        /// <summary>
        /// Méthode qui contient l'ensemble des listeners de la vue paramètres client
        /// </summary>
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
        /// <summary>
        /// Méthode qui contient l'ensemble des listeners de la vue gestion plan
        /// </summary>
        private void Initialize_Listeners_GestionPlan()
        {
            // Click sur le bouton créer un nouveau plan pour aller dans la Vue Paramètres Plan
            gestionPlan.BtnCréerPlan.Click += delegate (object sender, RoutedEventArgs e)
            {
                this.parametresPlan = new ParametresPlan(connexion);
                Initialize_Listeners_ParametresPlan();
                Mainframe.Content = parametresPlan;
            };
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

        #region Initialisation Paramètres Plan
        /// <summary>
        /// Méthode qui contient l'ensemble des listeners de la vue paramètres plan
        /// </summary>
        private void Initialize_Listeners_ParametresPlan()
        {
            // Click sur le bouton confirmer paramètres plan pour aller dans la Vue Modélisation
            parametresPlan.BtnConfirmerParamPlan.Click += delegate (object sender, RoutedEventArgs e)
            {
                //Mainframe.Content = gestionPlan;

                //Plan NewPlan = new Plan(projet);
                //NewPlan.reference = generateKey(projet);
                //parametresCAD.Projets.Add(NewPlan);
                //planCAD.InsertPlan(NewPlan);
            };

            // Click sur le bouton retour liste des plans pour aller dans la Vue Gestion Plan
            parametresPlan.BtnRetour.Click += delegate (object sender, RoutedEventArgs e)
            {
                Mainframe.Content = gestionPlan;
            };
        }
        #endregion

        #region Initialisation Modélisation
        /// <summary>
        /// Méthode qui contient l'ensemble des listeners de la vue modélisation
        /// </summary>
        //private void Initialize_Listeners_Modelisation()
        //{
        //    // Click sur le bouton quitter modélisation pour aller dans la Vue Gestion Plan
        //    modelisation.BtnQuitterModelisation.Click += delegate(object sender, RoutedEventArgs e)
        //    {
        //        Mainframe.Content = GestionPlan;
        //    };
        //}
        #endregion

        #region Initialisation Gestion Devis
        /// <summary>
        /// Méthode qui contient l'ensemble des listeners de la vue gestion devis
        /// </summary>
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
        #endregion

        #region Settings
        /// <summary>
        ///  Méthode définissant le comportement de l'application à la fermeture de la fenêtre
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }
        #endregion
    }
}