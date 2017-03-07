using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using Madera_MMB.Lib;
using System.Data.SQLite;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Madera_MMB.CAD
{
    // INotifyPropertyChanged permet de signaler au moteur de bindings qu'un élément a changé
    public class ClientCAD : INotifyPropertyChanged
    {
        #region properties
        public Connexion Conn { get; set; }
        public string SQLQuery { get; set; }

        private ObservableCollection<Client> _clients;
        public ObservableCollection<Client> Clients
        {
            get
            {
                if (this._clients == null)
                {
                    this._clients = new ObservableCollection<Client>();
                }
                return this._clients;
            }
            set { _clients = value; }
        }
        #endregion

        #region Events
        private void Clients_CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("Clients"));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Ctor
        public ClientCAD(Connexion laConnexion)
        {
            // Instanciations
            Conn = new Connexion();
            Clients = new ObservableCollection<Client>();
            _clients.CollectionChanged += Clients_CollectionChanged;
            // Appel des méthodes dans le ctor
            ListAllClients();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Méthode pour sélectionner la liste de tous les clients existants
        /// </summary>
        public void ListAllClients()
        {
            // Nom du/des champs mis directement dans la requête pour éviter d'avoir à passer par QSqlRecord 
            SQLQuery = "SELECT refClient, nom, prenom, adresse, codePostal, ville, email, telephone, dateCreation, dateModification FROM client";
            //SQLQuery = "SELECT * FROM client;

            // Ouverture de la connexion
            Conn.LiteCo.Open();
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, Conn.LiteCo))
            {
                Trace.WriteLine(SQLQuery);
                try
                {
                    // Execute le lecteur de donnée
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Trace.WriteLine("#### GET CLIENTS DATA ####");
                        while (reader.Read())
                        {
                            Trace.WriteLine(
                                reader.GetString(0) +
                                reader.GetString(1) +
                                reader.GetString(2) +
                                reader.GetString(3) +
                                reader.GetString(4) +
                                reader.GetString(5) +
                                reader.GetString(6) +
                                reader.GetString(7) +
                                reader.GetString(8) +
                                reader.GetString(9));
                            Client cli = new Client
                                (
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    reader.GetString(4),
                                    reader.GetString(5),
                                    reader.GetString(6),
                                    reader.GetString(7),
                                    reader.GetString(8),
                                    reader.GetString(9)
                                );
                            Clients.Add(cli);
                        }
                    }
                    Trace.WriteLine("#### GET CLIENTS DATA SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION CLIENTS ################################################# \n" + ex.ToString() + "\n");
                }
            }
            Conn.LiteCo.Close();
        }

        /// <summary>
        /// Méthode permettant d'insérer un nouveau client dans la abse locale
        /// </summary>
        /// <param name="client">Client à insérer dans la base</param>
        public void InsertClient(Client client)
        {
            SQLQuery = "INSERT INTO `client` (`refClient`, `nom`, `prenom`, `adresse`, `codePostal`, `ville`, `email`, `telephone`, `dateCreation`, `dateModification`)" +
            "VALUES (" + client.reference + "," + client.nom + "," + client.prenom + "," + client.adresse + ";" + client.codePostal + "," + client.ville + "," + client.email + "," + client.telephone + "," + client.creation + "," + client.modification +";";
            Conn.InsertSQliteQuery(SQLQuery);
        }

        /// <summary>
        /// Méthode permettant de mettre à jour les informations d'un client dans la base locale
        /// </summary>
        /// <param name="client">Client à mettre à jour</param>
        public void UpdateClient(Client client)
        {
            SQLQuery = "REPLACE INTO `client` (`refClient`, `nom`, `prenom`, `adresse`, `codePostal`, `ville`, `email`, `telephone`, `dateCreation`, `dateModification`)" +
            "VALUES (" + client.reference + "," + client.nom + "," + client.prenom + "," + client.adresse + ";" + client.codePostal + "," + client.ville + "," + client.email + "," + client.telephone + "," + client.creation + "," + client.modification + ";";
            Conn.InsertSQliteQuery(SQLQuery);
        }
        #endregion
    }
}
