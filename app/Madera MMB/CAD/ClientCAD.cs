using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;
using System.Diagnostics;

namespace Madera_MMB.CAD
{
    class ClientCAD
    {
        #region properties
        public Connexion conn { get; set; }
        public string SQLQuery { get; set; }
        public Commercial commercial { get; set; }
        public List<Client> clients { get; set; }
        #endregion

        #region Ctor
        public ClientCAD(Connexion laConnexion)
        {
            // Instanciations
            conn = laConnexion;
            clients = new List<Client>();

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
                            clients.Add(cli);
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

        // TODO : Faire une méthode pour permettre de créer un nouveau client en bdd
        //public void insertClient(Client client)
        //{
        //    SQLQuery = "INSERT INTO `client` (`refClient`, `nom`, `prenom`, `adresse`, `codePostal`, `ville`, `email`, `telephone`, `dateCreation`, `dateModification`)" +
        //    "VALUES (" + client.reference + "," + client.nom + "," + client.prenom + "," + client.adresse + ";" + client.codePostal + "," + client.ville + "," + client.email + "," + client.telephone + "," + client.dateCreation + "," + client.dateModification + "," + client.dateModification ";";
        //    conn.InsertSQliteQuery(SQLQuery);
        //}
        #endregion
    }
}
