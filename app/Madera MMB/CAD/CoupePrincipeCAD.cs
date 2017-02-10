﻿using Madera_MMB.Model;
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
    public class CoupePrincipeCAD
    {
        #region properties
        public List<CoupePrincipe> listecoupeprincipe { get; set; }
        public string SQLQuery { get; set; }
        public Connexion conn { get; set; }
        public CoupePrincipe coupe { get; set; }

        #endregion

        #region Ctor
        public CoupePrincipeCAD(Connexion co)
        {
            this.conn = co;
            listecoupeprincipe = new List<CoupePrincipe>();
            listAllCoupePrincipe();
        }
        #endregion

        #region privates methods
        private void listAllCoupePrincipe() 
        {
            SQLQuery = "SELECT * FROM coupeprincipe";
            conn.LiteCo.Open();
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CoupePrincipe coupe = new CoupePrincipe(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4));

                            listecoupeprincipe.Add(coupe);
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION COUPES PRINCIPE ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();
        }
        #endregion

        #region public methods
        public CoupePrincipe getCoupebyId(int id)
        {
            SQLQuery = "SELECT * FROM Coupeprincipe WHERE id_coupe = " + id;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    this.coupe = new CoupePrincipe(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetInt32(4));
                }
            }
            finally
            {
                reader.Close();
            }
            return coupe;
        }
        #endregion
    }
}
