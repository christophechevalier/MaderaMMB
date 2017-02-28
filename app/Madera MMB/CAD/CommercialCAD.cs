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
    class CommercialCAD
    {
        #region properties
        public Connexion conn { get; set; }
        public string SQLQuery { get; set; }
        public List<Commercial> commerciaux { get; set; }
        #endregion

        #region Ctor
        public CommercialCAD(Connexion laConnexion)
        {
            // Instanciations
            conn = laConnexion;
            commerciaux = new List<Commercial>();

            // Appel des méthodes dans le ctor
            listAllCommerciaux();
        }
        #endregion

        #region public methods
        /// <summary>
        /// Méthode pour sélectionner la liste de tous les commerciaux existants
        /// </summary>
        public void listAllCommerciaux()
        {
            // Nom du/des champs mis directement dans la requête pour éviter d'avoir à passer par QSqlRecord 
            SQLQuery = "SELECT refCommercial, nom, prenom, email, motDePasse FROM commercial";
            //SQLQuery = "SELECT * FROM commercial;

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
                        Trace.WriteLine("#### GET COMMERCIAUX DATA ####");
                        while (reader.Read())
                        {
                            Trace.WriteLine(
                                reader.GetString(0) +
                                reader.GetString(1) +
                                reader.GetString(2) +
                                reader.GetString(3) +
                                reader.GetString(4));
                            Commercial com = new Commercial
                                (
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    reader.GetString(4)
                                );
                            commerciaux.Add(com);
                        }
                    }
                    Trace.WriteLine("#### GET COMMERCIAUX DATA SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION COMMERCIAUX ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();
        }
        #endregion
    }
}
