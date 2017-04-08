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
    public class DevisCAD
    {
        #region properties
        public Devis dev { get; set; }
        public Connexion connexion { get; set; }
        public Plan plan { get; set; }
        public Projet projet { get; set; }
        public string SQLQuery { get; set; }
        #endregion

        #region Ctor
        /// <summary>
        /// Constructeur du devis avec connexion / plan en paramètre
        /// </summary>
        /// <param name="laConnexion"></param>
        /// <param name="unprojet"></param>
        /// <param name="clientCAD"></param>
        /// <param name="commercialCAD"></param>
        public DevisCAD(Connexion conn, Plan pln)
        {
            connexion = conn;
            plan = pln;
        }
        #endregion

        #region privates methods


        //REQUETE CHANGEMENT D'ETAT DEVIS
        public void changeStatusDevis(Devis devis, string etat)
        {
            SQLQuery = "UPDATE devis SET etat ='" + etat + "' WHERE refDevis = '" + devis.reference + "';";
            connexion.InsertSQliteQuery(SQLQuery);
        }

        #endregion

        #region public methods
        //REQUETE SELECT DEVIS PAR PLAN
        /// <summary>
        /// Retourne un devis selon le plan renseigné
        /// </summary>
        /// <returns>Devis</returns>
        public Devis getDevisByPlan(Plan plan)
        {
            //MODIFICATION A EFFECTUER Transformer LIST => objet Devis
            SQLQuery = "SELECT refDevis, etat ,dateCreation, prixTotalHT, prixTotalTTC FROM Devis WHERE refPlan = '" + plan.reference +"'";

            //CONNEXION BDD
            connexion.LiteCo.Open();

            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, connexion.LiteCo))
            {
                Trace.WriteLine(SQLQuery);
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Trace.WriteLine("#### GET DEVIS DATA ####");
                        while (reader.Read())
                        {

                            this.dev = new Devis
                            (
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetFloat(3),
                                reader.GetFloat(4),
                                plan
                            );
                        }
                    }
                    Trace.WriteLine("#### GET DEVIS DATA SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# GET DEVIS DATAS ERROR ################################################# \n" + ex.ToString() + "\n");
                }
            }
            connexion.LiteCo.Close();
            return dev;
        }

        //REQUETE CREATION DEVIS
        /// <summary>
        /// Méthode de création d'un devis
        /// </summary>
        /// <param name="devis"></param>
        /// <param name="refPlan"></param>
        public void insertDevis(Devis devis)
        {
            SQLQuery = "INSERT INTO devis (refDevis, nom, etat, dateCreation, prixTotalHT, prixTotalTTC, refPlan)" +
            "VALUES ('" + devis.reference + "','" + devis.etat + "','" + devis.creation + "',"  + devis.prixTotalHT + "," + devis.prixTotalTTC + ",'" + devis.plan.reference+"'"+
            "ON DUPLICATE KEY UPDATE etat='"+ devis.etat + "', prixTotalHT="+ devis.prixTotalHT + ", prixTotalHT="+ devis.prixTotalTTC ;
            connexion.InsertSQliteQuery(SQLQuery);
        }

        #endregion
    }
}