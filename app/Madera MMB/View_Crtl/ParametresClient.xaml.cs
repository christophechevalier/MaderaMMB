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
        private Connexion connexion { get; set; }
        private ClientCAD clientCAD { get; set; }
        private Client cli { get; set; }
        #endregion

        #region Constructeur
        /// <summary>
        /// Constructeur qui prend en paramètre la connexion
        /// </summary>
        /// <param name="co"></param>
        public ParametresClient(Connexion co)
        {
            // Instanciations
            InitializeComponent();
            connexion = co;
            clientCAD = new ClientCAD(this.connexion);
            // Les bindings exécutés dans le XAML ont pour DataContext une connexion
            DataContext = connexion;
            //ListeClients.ItemsSource = clientCAD.Clients;
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
    }
}