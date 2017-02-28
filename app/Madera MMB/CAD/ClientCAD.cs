using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;
using System.Diagnostics;
using System.ComponentModel;

namespace Madera_MMB.CAD
{
    class ClientCAD : INotifyPropertyChanged
    {
        #region properties
        public Connexion conn { get; set; }
        public string SQLQuery { get; set; }
        public Commercial commercial { get; set; }

        private List<Client> _clients;
        public List<Client> Clients
        {
            get { return _clients; }
            set { _clients = value; RaisePropertyChanged("Clients"); }
        }
        #endregion

        #region events
        private void RaisePropertyChanged(String property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Ctor
        public ClientCAD(Connexion laConnexion)
        {
            // Instanciations
            conn = laConnexion;
            Clients = new List<Client>();

            // Appel des méthodes dans le ctor
            listAllClients();
        }
        #endregion

        #region public methods
        /// <summary>
        /// Méthode pour sélectionner la liste de tous les clients existants
        /// </summary>
        public void listAllClients()
        {
            // Nom du/des champs mis directement dans la requête pour éviter d'avoir à passer par QSqlRecord 
            SQLQuery = "SELECT refClient, nom, prenom, adresse, codePostal, ville, email, telephone, dateCreation, dateModification FROM client";
            //SQLQuery = "SELECT * FROM client;

            // Ouverture de la connexion
            conn.LiteCo.Open();
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
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
            conn.LiteCo.Close();
        }

        /// <summary>
        /// Méthode permettant d'insérer un nouveau client dans la abse locale
        /// </summary>
        /// <param name="client">Client à insérer dans la base</param>
        public void InsertClient(Client client)
        {
            SQLQuery = "INSERT INTO `client` (`refClient`, `nom`, `prenom`, `adresse`, `codePostal`, `ville`, `email`, `telephone`, `dateCreation`, `dateModification`)" +
            "VALUES (" + client.reference + "," + client.nom + "," + client.prenom + "," + client.adresse + ";" + client.codePostal + "," + client.ville + "," + client.email + "," + client.telephone + "," + client.creation + "," + client.modification +";";
            conn.InsertSQliteQuery(SQLQuery);
        }

        /// <summary>
        /// Méthode permettant de mettre à jour les informations d'un client dans la base locale
        /// </summary>
        /// <param name="client">Client à mettre à jour</param>
        public void UpdateClient(Client client)
        {
            SQLQuery = "REPLACE INTO `client` (`refClient`, `nom`, `prenom`, `adresse`, `codePostal`, `ville`, `email`, `telephone`, `dateCreation`, `dateModification`)" +
            "VALUES (" + client.reference + "," + client.nom + "," + client.prenom + "," + client.adresse + ";" + client.codePostal + "," + client.ville + "," + client.email + "," + client.telephone + "," + client.creation + "," + client.modification + ";";
            conn.InsertSQliteQuery(SQLQuery);
        }
        #endregion
    }
}
