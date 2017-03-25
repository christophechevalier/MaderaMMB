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
    class DevisCAD
    {
        #region properties
        public Devis dev { get; set; }
        public Connexion conn { get; set; }
        public Plan plan { get; set; }
        public Plancher plancher { get; set; }
        public CoupePrincipe coupe { get; set; }
        public Gamme gamme { get; set; }
        public Couverture couverture { get; set; }
        public List<Module> modules { get; set; }
        public Client client { get; set; }
        public Commercial commercial { get; set; }
        public Projet projet { get; set; }
        public string SQLQuery { get; set; }
        public ClientCAD clientCAD { get; set; }
        public CommercialCAD commercialCAD { get; set; }
        #endregion

        #region Ctor
        /// <summary>
        /// Constructeur du devis avec connexion / plan en paramètre
        /// </summary>
        /// <param name="laConnexion"></param>
        /// <param name="unprojet"></param>
        /// <param name="clientCAD"></param>
        /// <param name="commercialCAD"></param>
        public DevisCAD( Plan pln)
        {
            plan = pln;
            dev = new Devis();

            getDevisByPlan();
        }
        #endregion

        #region privates methods
        //REQUETE SELECT DEVIS PAR PLAN
        /// <summary>
        /// Méthode pour récupérer le devis d'un plan
        /// </summary>
        private void getDevisByPlan() 
        {
            //MODIFICATION A EFFECTUER Transformer LIST => objet Devis
            SQLQuery = "SELECT refDevis, nom, etat, quantite, unite, dateCreation, margeCommercial, margeEntreprise, prixTotalHT, prixTotalTTC, refPlan, refProjet, refClient, refCommercial FROM Devis WHERE refPlan = " + plan.reference;

            //CONNEXION BDD
            conn.LiteCo.Open();

            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                Trace.WriteLine(SQLQuery);
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Trace.WriteLine("#### GET DEVIS DATA ####");
                        while(reader.Read())
                        {
                            Trace.WriteLine(
                                reader.GetString(0) +
                                reader.GetString(1) +
                                reader.GetString(2) +
                                reader.GetString(3) +
                                reader.GetString(4) +
                                reader.GetDateTime(5) +
                                reader.GetInt32(6) +
                                reader.GetInt32(7) +
                                reader.GetInt32(8) +
                                reader.GetInt32(9) +
                                plan +
                                projet +
                                client + 
                                commercial);

                            Devis devis = new Devis
                            (
                                    reader.GetString(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    reader.GetString(4),
                                    reader.GetDateTime(5),
                                    reader.GetInt32(6),
                                    reader.GetInt32(7),
                                    reader.GetInt32(8),
                                    reader.GetInt32(9),
                                    new Plan("Ref001", "Plan X 50x50", "2017-01-01", "2017-28-02", projet, plancher, couverture, coupe, gamme, modules),
                                    new Projet(client, commercial),
                                    client,
                                    commercial
                              );
                        }
                    }
                    Trace.WriteLine("#### GET DEVIS DATA SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION PROJETS ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();   
        }

        //REQUETE CREATION DEVIS
        /// <summary>
        /// Méthode de création d'un devis
        /// </summary>
        /// <param name="devis"></param>
        /// <param name="refProjet"></param>
        /// <param name="refPlan"></param>
        /// <param name="refClient"></param>
        /// <param name="refCommercial"></param>
        private void insertDevis(Devis devis, string refProjet, string refPlan, string refClient, string refCommercial) 
        {
            SQLQuery = "INSERT INTO devis (refDevis, nom, etat, quantite, unite, dateCreation, margeCommercial, margeEntreprise, prixTotalHT, prixTotalTTC, refPlan, refProjet, refClient, refCommercial)" +
            "VALUES (" + devis.reference + "," + devis.nom + "," + devis.etat + "," + devis.quantite + "," + devis.unite + "," + devis.creation + "," + devis.margeCommercial + "," + devis.margeEntreprise + "," + devis.prixTotalHT + "," + devis.prixTotalTTC + "," + devis.plan.reference + "," + devis.projet.reference + "," + refClient + "," + refCommercial + ";";
            conn.InsertSQliteQuery(SQLQuery);
        }

        //REQUETE CHANGEMENT D'ETAT DEVIS
        private void changeStatusDevis(Devis devis, string etat)
        {
            SQLQuery = "UPDATE devis SET etat =" + etat + " WHERE refDevis = " + devis.reference + ";";
            // A TERMINER
        }

        #endregion
    }
}