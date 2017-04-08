using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Madera_MMB.Lib;
using Madera_MMB.CAD;
using Madera_MMB.Model;
using System.Diagnostics;

namespace Madera_MMB.View_Crtl
{
    /// <summary>
    /// Logique d'interaction pour ParametresClient.xaml
    /// </summary>
    public partial class ParametresClient : Page
    {
        #region Properties
        public Connexion Conn { get; set; }
        public ClientCAD clientCAD { get; set; }
        private GestionClient gestionClient { get; set; }
        private Client _client { get; set; }
        public Client Client
        {
            get { return this._client; }
            set
            {
                _client = value;
                setLabels();
            }
        }
        #endregion

        #region Constructeur
        /// <summary>
        /// Constructeur qui prend en paramètre la connexion
        /// </summary>
        /// <param name="co"></param>
        public ParametresClient(Connexion co, ClientCAD CADclient)
        {
            // Instanciations
            InitializeComponent();
            Conn = co;
            this.clientCAD = CADclient;
            this.DataContext = clientCAD;
        }

        public ParametresClient(Connexion co, ClientCAD CADclient, Client cli)
        {
            // Instanciations
            InitializeComponent();
            Conn = co;
            this.clientCAD = CADclient;
            this.DataContext = clientCAD;

            ClientNom.Text = cli.nom;
            ClientPrenom.Text = cli.prenom;
            ClientEmail.Text = cli.email;
            ClientAdresse.Text = cli.adresse;
            ClientCodePostal.Text = cli.codePostal;
            ClientVille.Text = cli.ville;
            ClientTelephone.Text = cli.telephone;

            this.Client = cli;
        }
        #endregion

        #region Listeners
        private void BtnConfirmerClient_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnRetourListeClient_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region public methods
        /// <summary>
        /// Méthode pour set un client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public bool SetClient(Client client)
        {
            if 
            (
                ClientNom.Text != "" && 
                ClientPrenom.Text != "" && 
                ClientAdresse.Text != "" &&
                ClientCodePostal.Text != "" &&
                ClientVille.Text != "" &&
                ClientTelephone.Text != ""
            )
            {
                if (this.Client != null)
                {
                    Client.nom = this.ClientNom.Text;
                    Client.prenom = this.ClientPrenom.Text;
                    Client.email = this.ClientEmail.Text;
                    Client.adresse = this.ClientAdresse.Text;
                    Client.codePostal = this.ClientCodePostal.Text;
                    Client.ville = this.ClientVille.Text;
                    Client.telephone = this.ClientTelephone.Text;
                }
                else
                {
                    this.Client = new Client
                        (
                            "",
                            this.ClientNom.Text,
                            this.ClientPrenom.Text,
                            this.ClientAdresse.Text,
                            this.ClientCodePostal.Text,
                            this.ClientVille.Text,
                            this.ClientEmail.Text,
                            this.ClientTelephone.Text,
                            DateTime.Now.ToString(),
                            DateTime.Now.ToString()
                        );
                        this.Client.reference = generateKeyClient(this.Client);
                }
                return true;
            }
            return false;
        }
        #endregion

        #region private methods
        /// <summary>
        /// Change les valeurs du formulaire client à partir des informations du client reçu
        /// </summary>
        private void setLabels()
        {
            this.ClientNom.Text = this.Client.nom;
            this.ClientPrenom.Text = this.Client.prenom;
            this.ClientEmail.Text = this.Client.email;
            this.ClientAdresse.Text = this.Client.adresse;
            this.ClientCodePostal.Text = this.Client.codePostal;
            this.ClientVille.Text = this.Client.ville;
            this.ClientTelephone.Text = this.Client.telephone;
        }
        #endregion

        #region Tools
        private string generateKeyClient(Client cli)
        {
            string key = cli.nom.Substring(0, 1) + cli.prenom.Substring(0, 1);
            Random rand = new Random();
            int temp = rand.Next(000000, 999999);
            key += temp.ToString();
            return key;
        }
        #endregion
    }
}