using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;
using System.Data;
using System.Diagnostics;

namespace Madera_MMB.CAD
{
    public class CouvertureCAD
    {
        #region properties
        private List<Couverture> listecouverture { get; set; }
        public string SQLQuery { get; set; }
        public Connexion conn { get; set; }
        public Couverture couv { get; set; }
        #endregion

        #region Ctor
        public CouvertureCAD(Connexion co)
        {
            listecouverture = new List<Couverture>();
            this.conn = co;
            listAllCouverture();
        }
        #endregion

        #region privates methods
        private void listAllCouverture() 
        {
            SQLQuery = "SELECT * FROM couverture";
            conn.LiteCo.Open();
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Couverture couverture = new Couverture(reader.GetString(0), reader.GetInt32(1));
                            listecouverture.Add(couverture);
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION COUVERTURES ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();
        }
        #endregion

        #region public methods
        public Couverture getCouvbyType(string type)
        {
            SQLQuery = "SELECT * FROM Couverture WHERE typeCouverture = " + type;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    this.couv = new Couverture(reader.GetString(0), reader.GetInt32(1));
                }
            }
            finally
            {
                reader.Close();
            }
            return couv;
        }
        #endregion
    }
}