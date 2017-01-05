using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;
using System.Data;

namespace Madera_MMB.CAD
{
    class GammeCAD
    {
        #region properties
        private List<Gamme> listegamme { get; set; }
        public string SQLQuery { get; set; }
        public Connexion conn { get; set; }
        public Gamme gamme { get; set; }
        #endregion

        #region Ctor
        public GammeCAD(Connexion co)
        {
            listegamme = new List<Gamme>();
            this.conn = co;
        }
        #endregion

        #region privates methods
        private void listAllGamme() 
        {
            SQLQuery = "SELECT * FROM Gamme";
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Gamme gamme = new Gamme(reader.GetString(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                    listegamme.Add(gamme);
                }
            }
            finally
            {
                reader.Close();
            }
        }

        #endregion

        #region public methods
        public Gamme getGammebyNom(string nom)
        {
            SQLQuery = "SELECT * FROM Gamme WHERE nom = " + nom;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    this.gamme = new Gamme(reader.GetString(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4));
                }
            }
            finally
            {
                reader.Close();
            }
            return gamme;
        }
        #endregion
    }
}
