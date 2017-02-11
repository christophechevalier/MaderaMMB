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
    class CommercialCAD
    {
        #region properties
        public List<Commercial> listeAllCommerciaux { get; set; }
        public Connexion conn { get; set; }
        public string SQLQuery { get; set; }
        #endregion

        #region Ctor
        public CommercialCAD(Connexion laConnexion)
        {
            this.conn = laConnexion;
            listeAllCommerciaux = new List<Commercial>();
            getAllCommerciaux();
        }
        #endregion

        #region privates methods
        public Commercial getCommercialbyRef(Commercial commercial)
        {
            SQLQuery = "SELECT * FROM Commercial";
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Commercial com = new Commercial();
                    com.reference = reader.GetString(0);
                    com.nom = reader.GetString(1);
                    com.prenom = reader.GetString(2);
                    com.email = reader.GetString(3);
                }
            }
            finally
            {
                reader.Close();
            }
            return commercial;
        }

        private void getAllCommerciaux()
        {
            SQLQuery = "SELECT * FROM Commercial";
            this.conn.LiteCo.Open();
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Commercial com = new Commercial
                        (
                            reader.GetString(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            reader.GetString(3),
                            reader.GetString(4)
                        );
                    listeAllCommerciaux.Add(com);
                }
            }
            finally
            {
                reader.Close();
            }
            conn.LiteCo.Close();
        }
        private void insertCommercial(Commercial commercial)
        {
            SQLQuery = "INSERT INTO `commercial` (`refCommercial`, `nom`, `prenom`, `motDePasse`)" +
            "VALUES (" + commercial.reference + "," + commercial.nom + "," + commercial.prenom + "," + commercial.motDePasse + ";";
            conn.InsertSQliteQuery(SQLQuery);
        }
        #endregion
    }
}
