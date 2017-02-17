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
        // Déclaration des classes de connexion et d'accès aux données de l'application //
        private Connexion conn { get; set; }
        private CommercialCAD commCAD { get; set; }

        private bool mySQLSync { get; set; }
        // Déclaration et initialisation des Vues de l'application //
        private View_Crtl.Authentification Authentification { get; set; } 
        private View_Crtl.GestionProjet GestionProjet { get; set; }
        private View_Crtl.GestionPlan GestionPlan { get; set; }
        private View_Crtl.GestionClient GestionClient { get; set; }
        private View_Crtl.ParametresClient ParametresClient { get; set; }
        private View_Crtl.ParametresPlan ParametresPlan { get; set; }
        private View_Crtl.GestionDevis GestionDevis { get; set; }
        private View_Crtl.Modelisation Modelisation { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            initSynchro();
            Initialize_Listeners();
            Mainframe.Content = ParametresPlan;
        }

        #region Process Synchro
        /// <summary>
        ///  Méthode testant les possibilités de connexion aux bases de données locale et distante 
        /// </summary>
        private void initSynchro()
        {
            this.conn = new Connexion();
            if (conn.MySQLconnected == false)
            {
                mySQLSync = false;
                MessageBox.Show("Mode déconnecté");

    
                ResourceDictionary resource = this.Resources;
                
                resource["Logo"] = "../Lib/logo_offline.png";
                resource["Logo"] = resource["LogoOffline"];
                Trace.WriteLine(resource["Logo"]);
                this.UpdateLayout();
            }
            else if (!this.conn.SyncCommMySQL())
            {
                MessageBox.Show("Erreur de synchronisation ! ");
            }
            if (!conn.SQLiteconnected)
            {
                MessageBox.Show("Base innaccessible ! Veuillez contacter l'administrateur. ");
                Application.Current.Shutdown();
            }
        }
        #endregion

        #region Initialisation
        /// <summary>
        ///  Méthode initialisant toutes les vues et appelant toutes les méthodes d'abonnement aux vues
        /// </summary>
        private void Initialize_Listeners()
        {
            this.Authentification = new Authentification();
            this.GestionProjet = new GestionProjet();
            this.GestionPlan = new GestionPlan();
            this.GestionClient = new GestionClient();
            this.ParametresClient = new ParametresClient();
            this.ParametresPlan = new ParametresPlan(conn);
            this.GestionDevis = new GestionDevis();
            this.Modelisation = new Modelisation();

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
        /// <summary>
        ///  Méthode d'abonnement à la validation du formulaire d'authentification avec contrôles des identifiants
        /// </summary>
        private void Initialize_Listeners_Auth()
        {
            commCAD = new CommercialCAD(this.conn);
            // Click sur le bouton valider authentification pour aller dans la Vue Gestion Projet
            Authentification.BtnValiderAuth.Click += delegate(object sender, RoutedEventArgs e)
            {
                string id = Authentification.username.Text;
                string mdp = Authentification.password.Password;
                foreach(var comm in commCAD.listeAllCommerciaux)
                {
                    Trace.WriteLine(comm.email + " " + comm.motDePasse);
                    if(comm.email == id && comm.motDePasse == mdp)
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
        /// <summary>
        ///  Méthode d'abonnements à la vue Gestion Projet pour naviguer vers Gestion Client / Gestion Plan / Paramètre Client
        /// </summary>
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
        /// <summary>
        ///  Méthode d'abonnement à la vue Gestion CLient pour naviguer vers Paramètres Client / Gestion Projet
        /// </summary>
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
        /// <summary>
        ///  Méthode d'abonnement à la vue Paramètres Client pour naviguer vers Gestion Client
        /// </summary>
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
        /// <summary>
        ///  Méthode d'abonnement à la vue Gestion Plan pour naviguer vers Paramètres Plan / Modélisation
        /// </summary>
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
        /// <summary>
        ///  Méthode d'abonnement à la vue Paramètres Plan pour naviguer vers Modélisation / Gestion Plan
        /// </summary>
        private void Initialize_Listeners_ParametresPlan()
        {
            // Click sur le bouton confirmer paramètres plan pour aller dans la Vue Modélisation
            ParametresPlan.BtnConfirmerParamPlan.Click += delegate(object sender, RoutedEventArgs e)
            {
                if(ParametresPlan.SetPlan())
                {
                    Mainframe.Content = Modelisation;
                }
            };
            // Click sur le bouton retour liste des plans pour aller dans la Vue Gestion Plan
            ParametresPlan.BtnRetour.Click += delegate(object sender, RoutedEventArgs e)
            {
                Mainframe.Content = GestionPlan;
            };
        }
        #endregion

        #region Initialisation Modélisation
        /// <summary>
        ///  Méthode d'abonnement à la vue Modélisation pour naviguer vers Gestion Plan
        /// </summary>
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
        /// <summary>
        ///  Méthode d'abonnement à la vue Gestion Devis pour naviguer vers Gestion Plan
        /// </summary>
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

        /// <summary>
        ///  Méthode définissant le comportement de l'application à la fermeture de la fenêtre
        /// </summary>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

    }
}