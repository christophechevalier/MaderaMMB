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
using Madera_MMB.Lib;
using Madera_MMB.Lib.Tools;
using Madera_MMB.CAD;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Connexion Conn { get; set; }
        private ErrorModalWindow errorWindow { get; set; }
        private CommercialCAD CommCAD { get; set; }

        View_Crtl.Authentification Authentification = new Authentification();
        View_Crtl.GestionProjet GestionProjet = new GestionProjet();
        View_Crtl.GestionPlan GestionPlan = new GestionPlan();
        View_Crtl.GestionClient GestionClient = new GestionClient();
        View_Crtl.ParametresClient ParametresClient = new ParametresClient();
        View_Crtl.ParametresPlan ParametresPlan = new ParametresPlan();
        View_Crtl.GestionDevis GestionDevis = new GestionDevis();
        View_Crtl.Modelisation Modelisation = new Modelisation();


        public MainWindow()
        {
            InitializeComponent();
            Initialize_Listeners();
            this.errorWindow = new ErrorModalWindow();
            this.Conn = new Connexion();

            Mainframe.Content = Authentification;
            CommCAD = new CommercialCAD(this.Conn);

            if (!Conn.MySQLconnected)
            {
                errorWindow.message.Content = " Mode déconnecté ";
                errorWindow.ShowDialog();
            }

            Console.WriteLine("TEST INTERMEDIAIRE1");
            if (!Conn.SQLiteconnected)
            {
                errorWindow.message.Content = " Base innaccessible ! Veuillez contacter l'administrateur.  Application inutilisable ";
                errorWindow.BtnOK.Click += delegate(object sender, RoutedEventArgs e)
                {
                    Application.Current.Shutdown();
                };
                errorWindow.ShowDialog();
            }

            Console.WriteLine("TEST INTERMEDIAIRE2");
            if(!this.Conn.SyncCommMySQL())
            {
                errorWindow.message.Content = "Erreur de récupération des données Commerciaux ! ";
                errorWindow.ShowDialog();
            }

            // TEST QUERY SQLite //
            string query = "REPLACE INTO Commercial (refCommercial, nom, prenom, motDePasse) VALUES ('003', 'yololnom', 'yololprenom', 'yololmdp')";
            Conn.InsertSQliteQuery(query);
            string myquery = "SELECT * FROM Commercial;";
            Conn.SelectSQLiteQuery(myquery);
        }

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
            //Initialize_Listeners_Devis();
        }
        #endregion

        #region Initialisation ModalError
        private void Initialize_Listeners_ModalError()
        {
            // Click sur le bouton valider authentification pour aller dans la Vue Gestion Projet
            errorWindow.BtnOK.Click += delegate(object sender, RoutedEventArgs e)
            {
                errorWindow.Close();
            };
        }
        #endregion

        #region Initialisation Auth
        private void Initialize_Listeners_Auth()
        {
            // Click sur le bouton valider authentification pour aller dans la Vue Gestion Projet
            Authentification.BtnValiderAuth.Click += delegate(object sender, RoutedEventArgs e)
            {
                CommCAD.getAllComm();
                string mdp = Authentification.username.Text;
                string id = Authentification.password.Password;
                foreach(var comm in CommCAD.listeAllCommerciaux)
                {
                    Console.WriteLine(comm.reference + "  /  " + comm.motDePasse);
                    if(comm.reference == id && comm.motDePasse == mdp)
                    {
                        Mainframe.Content = GestionProjet;
                    }
                    else
                    {
                        errorWindow.message.Content = "Mauvais couple identifiant/mot de passe ! ";
                        errorWindow.ShowDialog();
                    }
                }
            };
        }
        #endregion

        #region Initialisation Gestion Projet
        private void Initialize_Listeners_GestionProjet()
        {
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

    }
}