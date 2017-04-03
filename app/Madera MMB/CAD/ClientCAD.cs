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
            Conn = laConnexion;
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
            Clients.Clear();

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
        /// Méthode qui permet de créer un nouveau client
        /// </summary>
        /// <param name="client"></param>
        public void InsertClient(Client client)
        {
            string SQLQuery = "REPLACE INTO client (refClient, nom, prenom, adresse, codePostal, ville, email, telephone, dateCreation, dateModification)" +
                "VALUES (@refClient, @nom, @prenom, @adresse, @codePostal, @ville, @email, @telephone, @dateCreation, @dateModification)";

            // Ouverture de la connexion
            Conn.LiteCo.Open();
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, Conn.LiteCo))
            {
                Trace.WriteLine(SQLQuery);
                try
                {
                    command.Parameters.AddWithValue("@refClient", client.reference);
                    command.Parameters.AddWithValue("@nom", client.nom);
                    command.Parameters.AddWithValue("@prenom", client.prenom);
                    command.Parameters.AddWithValue("@adresse", client.adresse);
                    command.Parameters.AddWithValue("@codePostal", client.codePostal);
                    command.Parameters.AddWithValue("@ville", client.ville);
                    command.Parameters.AddWithValue("@email", client.email);
                    command.Parameters.AddWithValue("@telephone", client.telephone);
                    command.Parameters.AddWithValue("@dateCreation", DateTime.Now.ToString());
                    command.Parameters.AddWithValue("@dateModification", DateTime.Now.ToString());

                    command.ExecuteNonQuery();
                    Trace.WriteLine("#### INSERT NOUVEAU CLIENT DATA SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR INSERTION NOUVEAU CLIENT ################################################# \n" + ex.ToString() + "\n");
                }
            }
            Conn.LiteCo.Close();
            ListAllClients();
        }
        #endregion
    }
}
