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
            //clientCAD = new ClientCAD(this.connexion);
            // Les bindings exécutés dans le XAML ont pour DataContext une connexion
            //DataContext = connexion;
        }
        #endregion

        #region Listeners
        private void BtnConfirmerClient_Click(object sender, RoutedEventArgs e)
        {
            //Button btn = sender as Button;
            //Client cli = new Client();
            //cli.nom = ClientNom.Text;
            //cli.prenom = ClientPrenom.Text;
            //cli.email = ClientEmail.Text;
            //cli.adresse = ClientAdresse.Text;
            //cli.codePostal = ClientCodePostal.Text;
            //cli.ville = ClientVille.Text;
            //cli.telephone = ClientTelephone.Text;

            // On génére une nouvelle référence en commençant par les 2 premières lettres du nouveau client
            //cli.reference = generateKeyClient(cli);
            //clientCAD.Clients.Add(cli);
            //clientCAD.InsertClient(cli);
            //MessageBox.Show("Création du nouveau client SUCCESS !");

            //if (cli != null)
            //{
            //    // On génére une nouvelle référence en commençant par les 2 premières lettres du nouveau client
            //    cli.reference = generateKeyClient(cli);
            //    clientCAD.Clients.Add(cli);
            //    clientCAD.InsertClient(cli);
            //    MessageBox.Show("Création du nouveau client SUCCESS !");
            //}
            //else
            //{
            //    MessageBox.Show("Vous devez remplir les champs obligatoires !");
            //}
        }
        private void BtnRetourListeClient_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

            #region public methods
        public bool SetClient(Client client)
        {
            Client cli = new Client();
            if 
            (
                ClientNom.Text != "" && 
                ClientPrenom.Text != "" && 
                ClientEmail.Text != "" && 
                ClientAdresse.Text != "" &&
                ClientCodePostal.Text != "" &&
                ClientVille.Text != "" &&
                ClientTelephone.Text != ""
            )
            {
                if (cli != null)
                {
                    clientCAD.Clients.Add(cli);
                    // On génére une nouvelle référence en commençant par les 2 premières lettres du nouveau client
                    cli.reference = generateKeyClient(cli);

                    cli.nom = this.ClientNom.Text;
                    cli.prenom = this.ClientPrenom.Text;
                    cli.email = this.ClientEmail.Text;
                    cli.adresse = this.ClientAdresse.Text;
                    cli.codePostal = this.ClientCodePostal.Text;
                    cli.ville = this.ClientVille.Text;
                    cli.telephone = this.ClientTelephone.Text;
                }
                else
                {
                    this.Client = new Client
                        (
                            generateKeyClient(cli),
                            this.ClientNom.Text,
                            this.ClientPrenom.Text,
                            this.ClientAdresse.Text,
                            this.ClientEmail.Text,
                            this.ClientCodePostal.Text,
                            this.ClientVille.Text,
                            this.ClientTelephone.Text,
                            DateTime.Now,
                            DateTime.Today
                        );
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