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
using Madera_MMB.Lib;
using Madera_MMB.CAD;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// *Règle de gestion Authentification :
    /// - Identifiant/mot de passe saisis doivent correspondre à ceux de l'un des Commerciaux de la liste renvoyée par CommercialCAD
    /// 
    /// *Règles de gestion Général / Synchronisation :
    /// - Démarrage, connexion BDD SQLite; Si erreur de connexion BDD SQLite -> Message d'erreur critique, application ferme
    /// - Test Connexion BDD distante; Si erreur de connexion BDD distante -> mode offline / Sinon -> mode online
    /// - Si mode online -> chargement des commerciaux MYSQL dans BDD SQLite
    /// - Authentification
    /// - Si online -> chargement des Projets/Clients/Plans selon commercial authentifié MYSQL dans BDD SQLite
    /// - Chargement Projets/Clients/Plans/Devis selon commercial authentifié depuis BDD SQLite
    /// - Gestion Projet/Valider création projet : export données vers BDD SQLite, si online -> export aussi vers BDD distante
    /// - Gestion Client/Valider création/modification client : export données vers BDD SQLite, si online -> export aussi vers BDD distante
    /// - Modélisation/Valider création/modification plan : export données vers BDD SQLite, si online -> export aussi vers BDD distante
    /// - Gestion Devis/Valider création projet : export données vers BDD SQLite, si online -> export aussi vers BDD distante
    /// </summary>
    public partial class MainWindow : Window
    {

        private Connexion Conn { get; set; }
        private CommercialCAD CommCAD { get; set; }
        private View_Crtl.Authentification Authentification = new Authentification();
        private View_Crtl.GestionProjet GestionProjet = new GestionProjet();
        private View_Crtl.GestionPlan GestionPlan = new GestionPlan();
        private View_Crtl.GestionClient GestionClient = new GestionClient();
        private View_Crtl.ParametresClient ParametresClient = new ParametresClient();
        private View_Crtl.ParametresPlan ParametresPlan = new ParametresPlan();
        private View_Crtl.GestionDevis GestionDevis = new GestionDevis();
        private View_Crtl.Modelisation Modelisation = new Modelisation();


        public MainWindow()
        {
            InitializeComponent();
            Initialize_Listeners();
            Mainframe.Content = Authentification;
            initSynchro();
        }

        #region Process Synchro
        private void initSynchro()
        {
            if (!Conn.MySQLconnected)
            {
                MessageBox.Show("Mode déconnecté");

            }
            else if (!this.Conn.SyncCommMySQL())
            {
                MessageBox.Show("Erreur de synchronisation ! ");
            }

            Debug.WriteLine("TEST INTERMEDIAIRE1");
            if (!Conn.SQLiteconnected)
            {
                MessageBox.Show("Base innaccessible ! Veuillez contacter l'administrateur. ");
                Application.Current.Shutdown();
            }

            Debug.WriteLine("TEST INTERMEDIAIRE2");
 
            // TEST QUERY SQLite //
            string query = "REPLACE INTO Commercial (refCommercial, nom, prenom, motDePasse) VALUES ('003', 'yololnom', 'yololprenom', 'yololmdp')";
            Conn.InsertSQliteQuery(query);
            string myquery = "SELECT * FROM Commercial;";
            Conn.SelectSQLiteQuery(myquery);
        }
        #endregion

        #region Initialisation
        private void Initialize_Listeners()
        {
            Initialize_Listeners_Auth();
            Initialize_Listeners_GestionProjet();
            Initialize_Listeners_GestionClient();
            Initialize_Listeners_ParametresClient();
            Initialize_Listeners_GestionPlan();
            Initialize_Listeners_ParametresPlan();
            Initialize_Listeners_Modelisation();
            Initialize_Listeners_Devis();
        }
        #endregion


        #region Initialisation Auth
        private void Initialize_Listeners_Auth()
        {
            //this.errorWindow = new ErrorModalWindow();
            this.Conn = new Connexion();
            CommCAD = new CommercialCAD(this.Conn);
            // Click sur le bouton valider authentification pour aller dans la Vue Gestion Projet
            Authentification.BtnValiderAuth.Click += delegate(object sender, RoutedEventArgs e)
            {
                string id = Authentification.username.Text;
                string mdp = Authentification.password.Password;
                foreach(var comm in CommCAD.listeAllCommerciaux)
                {
                    if(comm.reference == id && comm.motDePasse == mdp)
                        GestionProjet.commercialAuthentifié = comm;
                }
                if (GestionProjet.commercialAuthentifié != null)
                {
                    Mainframe.Content = GestionProjet;
                }
                else
                {
                    MessageBox.Show("Mauvais couple identifiant/mot de passe ! ");
                }
            };
        }
        #endregion

        #region Initialisation Gestion Projet
        private void Initialize_Listeners_GestionProjet()
        {
            this.GestionProjet = new View_Crtl.GestionProjet();
            // Click sur le bouton listes des clients pour aller dans la Vue Gestion Client
            GestionProjet.BtnListeClient.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = GestionClient;
            };
            // Click sur le bouton ouvrir un projet client pour aller dans la Vue Gestion Plan
            GestionProjet.BtnOuvrirProjet.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = GestionPlan;
            };
            // Click sur le bouton créer un nouveau client pour aller dans la Vue Paramètre Client
            GestionProjet.BtnCreerClient.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = ParametresClient;
            };
            // Click sur le bouton se déconnecter de l'application pour aller dans la Vue Auth
            GestionProjet.BtnSeDeconnecter.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = Authentification;
            };
        }
        #endregion

        #region Initialisation Gestion Client
        private void Initialize_Listeners_GestionClient()
        {
            // Click sur le bouton éditer un client pour aller dans la Vue Paramètre Client
            GestionClient.BtnEditerClient.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = ParametresClient;
            };
            // Click sur le bouton créer un nouveau client pour aller dans la Vue Paramètre Client
            GestionClient.BtnCreerClient.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = ParametresClient;
            };
            // Click sur le bouton retour pour aller dans la Vue Gestion Projet
            GestionClient.BtnRetourListeProjet.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = GestionProjet;
            };
        }
        #endregion

        #region Initialisation Paramètres Client
        private void Initialize_Listeners_ParametresClient()
        {
            // Click sur le bouton valider paramètres client pour aller dans la Vue Gestion Client
            ParametresClient.BtnConfirmerClient.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = GestionClient;
            };
            // Click sur le bouton retour pour aller dans la Vue Gestion Client
            ParametresClient.BtnRetourListeProjet.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = GestionClient;
            };
        }
        #endregion

        #region Initialisation Gestion Plan
        private void Initialize_Listeners_GestionPlan()
        {
            // Click sur le bouton créer un nouveau plan pour aller dans la Vue Paramètres Plan
            GestionPlan.BtnCréerPlan.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = ParametresPlan;
            };
            // Click sur le bouton ouvrir un plan pour aller dans la Vue Modélisation
            GestionPlan.BtnOuvrirPlan.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = Modelisation;
            };
            // Click sur le bouton consulter le devis pour aller dans la Vue Gestion Devis
            //GestionPlan.BtnConsulterDevis.Click += delegate(object sender, RoutedEventArgs e)
            //{
            //    Mainframe.Content = GestionDevis;
            //};
            // Click sur le bouton copier plan pour aller dans la Vue ???
            GestionPlan.BtnCopierPlan.Click += delegate(object sender, RoutedEventArgs e)
            {

            };
            // Click sur le bouton retour liste de projets pour aller dans la Vue Gestion Projet
            GestionPlan.BtnRetour.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = GestionProjet;
            };
        }
        #endregion

        #region Initialisation Paramètres Plan
        private void Initialize_Listeners_ParametresPlan()
        {
            // Click sur le bouton confirmer paramètres plan pour aller dans la Vue Modélisation
            ParametresPlan.BtnConfirmerParamPlan.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = Modelisation;
            };
            // Click sur le bouton retour liste des plans pour aller dans la Vue Gestion Plan
            ParametresPlan.BtnRetour.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = GestionPlan;
            };
        }
        #endregion

        #region Initialisation Modélisation
        private void Initialize_Listeners_Modelisation()
        {
            // Click sur le bouton quitter modélisation pour aller dans la Vue Gestion Plan
            Modelisation.BtnQuitterModelisation.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = GestionPlan;
            };
        }
        #endregion

        #region Initialisation Gestion Devis
        private void Initialize_Listeners_Devis()
        {
            // Click sur le bouton retour liste des plans pour aller dans la Vue Gestion Plan
            GestionDevis.BtnRetour.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = GestionPlan;
            };
            // Click sur le bouton exporter un devis client pour ???
            GestionDevis.BtnExportDevis.Click += delegate(object sender, RoutedEventArgs e)
            {

            };
        }
        #endregion

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

    }
}