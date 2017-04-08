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
        private Plan unplan;

        private bool mySQLSync { get; set; }
        private View_Crtl.Authentification authentification { get; set; }
        private View_Crtl.GestionProjet gestionProjet { get; set; }
        private View_Crtl.GestionPlan gestionPlan { get; set; }
        private View_Crtl.GestionClient gestionClient { get; set; }
        private View_Crtl.ParametresClient parametresClient { get; set; }
        private View_Crtl.ParametresPlan parametresPlan { get; set; }
        private View_Crtl.GestionDevis gestionDevis { get; set; }
        private View_Crtl.Modelisation modelisation { get; set; }
        private CommercialCAD commCAD { get; set; }
        private ClientCAD clientCAD { get; set; }
        private CoupePrincipeCAD coupeCAD { get; set; }
        private CouvertureCAD couvCAD { get; set; }
        private GammeCAD gamCAD { get; set; }
        private PlancherCAD plancherCAD { get; set; }



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
            if(connexion.MySQLconnected)
                connexion.SyncCommMySQL();

            /// Test CAD avec nouvelles données ///
            commCAD = new CommercialCAD(connexion);
            clientCAD = new ClientCAD(connexion);
            coupeCAD = new CoupePrincipeCAD(connexion);
            couvCAD = new CouvertureCAD(connexion);
            gamCAD = new GammeCAD(connexion);
            plancherCAD = new PlancherCAD(connexion);

            this.authentification = new Authentification(this.connexion);

            Mainframe.Content = authentification;

            Initialize_Listeners_Auth();
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

        #region Initialisation Auth
        /// <summary>
        /// Méthode qui contient l'ensemble des listeners de la vue authentification
        /// </summary>
        private void Initialize_Listeners_Auth()
        {
            // Click sur le bouton valider authentification pour aller dans la Vue Gestion Projet
            authentification.BtnValiderAuth.Click += delegate (object sender, RoutedEventArgs e)
            {
                string id = authentification.username.Text;
                string mdp = authentification.password.Password;
                Trace.WriteLine(id + "  " + mdp);
                bool authentifié = false;
                Commercial comm_authentifié = new Commercial();
                foreach (var comm in commCAD.commerciaux)
                {
                    Trace.WriteLine(comm.reference + " / " + comm.motDePasse);
                    if (comm.reference == id && comm.motDePasse == mdp)
                    {
                        comm_authentifié = comm;
                        authentifié = true;   
                    }
                }
                if(authentifié && comm_authentifié != null)
                {
                    this.gestionProjet = new GestionProjet(connexion, comm_authentifié);
                    Initialize_Listeners_GestionProjet();
                    Mainframe.Content = gestionProjet;
                }
                else
                {
                    MessageBox.Show("Mauvais couple identifiant/mot de passe ! ");
                }

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
                if (connexion.MySQLconnected)
                {
                    connexion.ExpClients();
                    connexion.ExpProjets();
                }
                this.gestionClient = new GestionClient(connexion, clientCAD);
                Initialize_Listeners_GestionClient();
                Mainframe.Content = gestionClient;
            };
            // Click sur le bouton ouvrir un projet client pour aller dans la Vue Gestion Plan
            gestionProjet.BtnOuvrirProjet.Click += delegate(object sender, RoutedEventArgs e)
            {
                if (connexion.MySQLconnected)
                    connexion.ExpProjets();
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
                if (connexion.MySQLconnected)
                    connexion.ExpProjets();
                this.parametresClient = new ParametresClient(connexion, clientCAD);
                Initialize_Listeners_ParametresClient();
                Mainframe.Content = parametresClient;
            };
            // Click sur le bouton se déconnecter de l'application pour aller dans la Vue Auth
            gestionProjet.BtnSeDeconnecter.Click += delegate(object sender, RoutedEventArgs e)
            {
                if (connexion.MySQLconnected)
                    connexion.ExpProjets();
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
                if (gestionClient.cli != null)
                {
                    this.parametresClient = new ParametresClient(connexion, gestionClient.clientCAD, gestionClient.cli);
                    Initialize_Listeners_ParametresClient();
                    Mainframe.Content = parametresClient;
                }
            };
            // Click sur le bouton créer un nouveau client pour aller dans la Vue Paramètre Client
            gestionClient.BtnCreerClient.Click += delegate(object sender, RoutedEventArgs e)
            {
                this.parametresClient = new ParametresClient(connexion, gestionClient.clientCAD);
                Initialize_Listeners_ParametresClient();
                Mainframe.Content = parametresClient;
            };
            // Click sur le bouton retour pour aller dans la Vue Gestion Projet
            gestionClient.BtnRetourListeProjet.Click += delegate(object sender, RoutedEventArgs e)
            {
                if (connexion.MySQLconnected)
                    connexion.ExpClients();
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
                if (parametresClient.SetClient(parametresClient.Client))
                {
                    MessageBox.Show("L'édition s'est bien effectué !");
                    parametresClient.clientCAD.InsertClient(parametresClient.Client);
                    gestionClient = new GestionClient(connexion, parametresClient.clientCAD);
                    Mainframe.Content = gestionClient;
                    Initialize_Listeners_GestionClient();
                }
                else
                {
                    MessageBox.Show("Un des champs obligatoires n'est pas renseigné");
                }
            };
            // Click sur le bouton retour pour aller dans la Vue Gestion Client
            parametresClient.BtnRetourListeProjet.Click += delegate(object sender, RoutedEventArgs e)
            {
                if (connexion.MySQLconnected)
                    connexion.ExpClients();
                gestionClient = new GestionClient(connexion, parametresClient.clientCAD);
                Mainframe.Content = gestionClient;
                Initialize_Listeners_GestionClient();
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
                this.parametresPlan = new ParametresPlan(connexion, gestionPlan.planCAD);
                Initialize_Listeners_ParametresPlan();
                Mainframe.Content = parametresPlan;
            };
            // Click sur le bouton ouvrir un plan pour aller dans la Vue Modélisation
            gestionPlan.BtnOuvrirPlan.Click += delegate (object sender, RoutedEventArgs e)
            {
                if (gestionPlan.plan != null)
                {
                    this.modelisation = new Modelisation(connexion, gestionPlan.plan, gestionPlan.planCAD);
                    Initialize_Listeners_Modelisation();
                    Mainframe.Content = modelisation;
                }
                else
                {
                    MessageBox.Show("Vous devez sélectionner un plan avant de l'ouvrir !");
                }
            };
            // Click sur le bouton editer plan pour aller dans la Vue Paramètres Plan
            gestionPlan.BtnEditerPlan.Click += delegate (object sender, RoutedEventArgs e)
            {
                if (gestionPlan.plan != null)
                {
                    this.parametresPlan = new ParametresPlan(connexion, gestionPlan.planCAD);
                    this.parametresPlan.Plan = gestionPlan.plan;
                    Initialize_Listeners_ParametresPlan();
                    Mainframe.Content = parametresPlan;
                }
                else
                {
                    MessageBox.Show("Vous devez sélectionner un plan avant de l'éditer !");
                }
            };
            // Click sur le bouton consulter le devis pour aller dans la Vue Gestion Devis
            gestionPlan.BtnConsulterDevis.Click += delegate (object sender, RoutedEventArgs e)
            {
                if (connexion.MySQLconnected)
                    connexion.ExpPlans();
                this.gestionDevis = new GestionDevis(connexion, gestionPlan.plan, gestionPlan.projet.commercial, gestionPlan.projet.client);
                Initialize_Listeners_Devis();
                Mainframe.Content = gestionDevis;
            };
            // Click sur le bouton retour liste de projets pour aller dans la Vue Gestion Projet
            gestionPlan.BtnRetour.Click += delegate(object sender, RoutedEventArgs e)
            {
                if (connexion.MySQLconnected)
                    connexion.ExpPlans();
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
                if (parametresPlan.SetPlan())
                {
                    MessageBox.Show("Plan créé/modifié");
                    parametresPlan.planCAD.InsertPlan(parametresPlan.Plan);
                    parametresPlan.Plan = null;
                    gestionPlan.planCAD.ListAllPlansByProject();
                    Mainframe.Content = gestionPlan;
                }
                else
                {
                    MessageBox.Show("Un des champs obligatoires n'est pas renseigné");
                }
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
        private void Initialize_Listeners_Modelisation()
        {
            // Click sur le bouton quitter modélisation pour aller dans la Vue Gestion Plan
            modelisation.BtnQuitterModelisation.Click += delegate (object sender, RoutedEventArgs e)
            {
                if(connexion.MySQLconnected)
                {
                    connexion.ExpModules();
                    connexion.ExpPlans();
                }
                Mainframe.Content = gestionPlan;
            };
        }
        #endregion

        #region Initialisation Gestion Devis
        /// <summary>
        /// Méthode qui contient l'ensemble des listeners de la vue gestion devis
        /// </summary>
        private void Initialize_Listeners_Devis()
        {
            // Click sur le bouton retour liste des plans pour aller dans la Vue Gestion Plan
            gestionDevis.BtnRetour.Click += delegate (object sender, RoutedEventArgs e)
            {
                gestionDevis.devisCAD.insertDevis(gestionDevis.devis);
                if (connexion.MySQLconnected)
                    connexion.ExpDevis();
                Mainframe.Content = this.gestionPlan;
            };
            // Click sur le bouton exporter un devis client en PDF
            gestionDevis.BtnExportDevis.Click += delegate (object sender, RoutedEventArgs e)
            {

            };
        }
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