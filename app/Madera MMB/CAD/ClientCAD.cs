using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;

namespace Madera_MMB.CAD
{
    class ClientCAD
    {
        #region properties
        public List<Client> listeClient { get; set; }
        public Connexion conn { get; set; }
        public string SQLQuery { get; set; }
        #endregion

        #region Ctor
        public ClientCAD(Connexion laConnexion)
        {
            Connexion conn = laConnexion;
            listeClient = new List<Client>();
        }
        #endregion

        #region privates methods
        public Client getClientbyRef(Client client)
        {
            SQLQuery = "SELECT * FROM Client";
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
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
                            reader.GetString(7)
                        );
                    listeClient.Add(cli);
                }
            }
            finally
            {
                reader.Close();
            }
            return client;
        }
        private void listAllClient()
        {
            SQLQuery = "SELECT * FROM Client";
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Client cli = new Client(
                        reader.GetString(0),
                        reader.GetString(1),
                        reader.GetString(2),
                        reader.GetString(3),
                        reader.GetString(4),
                        reader.GetString(5),
                        reader.GetString(6),
                        reader.GetString(7)
                        );
                    listeClient.Add(cli);
                }
            }
            finally
            {
                reader.Close();
            }
        }
        private void insertClient(Client client)
        {
            SQLQuery = "INSERT INTO `client` (`refClient`, `nom`, `prenom`, `adresse`, `codePostal`, `ville`, `email`, `telephone`)" +
            "VALUES (" + client.reference + "," + client.nom + "," + client.prenom + "," + client.adresse + ";" + client.codePostal + "," + client.ville + "," + client.email + "," + client.telephone + ";";
            conn.InsertSQliteQuery(SQLQuery);
        }
        #endregion
    }
}
