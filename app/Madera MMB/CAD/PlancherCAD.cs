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
    public class PlancherCAD
    {
        #region properties
        private List<Plancher> listeplancher { get; set; }
        public string SQLQuery { get; set; }
        public Connexion conn { get; set; }
        public Plancher plancher { get; set; }
        #endregion

        #region Ctor
        public PlancherCAD(Connexion co)
        {
            listeplancher = new List<Plancher>();
            this.conn = co;
            listAllPlancher();
        }
        #endregion

        #region privates methods
        private void listAllPlancher() 
        {
            SQLQuery = "SELECT * FROM plancher";
            conn.LiteCo.Open();
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Plancher plancher = new Plancher(reader.GetString(0), reader.GetInt32(1));
                            listeplancher.Add(plancher);
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION PLANCHERS ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();
        }
        #endregion

        #region public methods
        public Plancher getPlancherbyType(string type)
        {
            SQLQuery = "SELECT * FROM Plancher WHERE typePlancher = " + type;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    this.plancher = new Plancher(reader.GetString(0), reader.GetInt32(1));
                }
            }
            finally
            {
                reader.Close();
            }
            return plancher;
        }
        #endregion
    }
}
