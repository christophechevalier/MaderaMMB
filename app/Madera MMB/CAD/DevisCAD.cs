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
    class DevisCAD
    {
        #region properties
        private List<Devis> listeAllDevisParPlan { get; set; }
        public Connexion conn { get; set; }
        public Plan plan { get; set; }
        public Projet projet { get; set; }
        public string SQLQuery { get; set; }
        public ClientCAD clientCAD { get; set; }
        public CommercialCAD commercialCAD { get; set; }
        #endregion

        #region Ctor
        public DevisCAD(Connexion laConnexion, Projet unprojet, ClientCAD clientCAD, CommercialCAD commercialCAD)
        {
            this.projet = unprojet;
            Connexion conn = laConnexion;
            listeAllDevisParPlan = new List<Devis>();
            this.clientCAD = clientCAD;
            this.commercialCAD = commercialCAD;
        }
        #endregion

        #region privates methods
        //REQUETE SELECT DEVIS PAR PLAN
        /// <summary>
        /// Méthode pour récupérer le devis d'un plan
        /// </summary>
        private void listAllDevisParPlan() 
        {
            SQLQuery = "SELECT * FROM Devis WHERE refPlan = " + plan.reference;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Devis devis = new Devis();
                    devis.reference = reader.GetString(0);
                    devis.nom = reader.GetString(1);
                    devis.etat = reader.GetString(2);
                    devis.quantite = reader.GetString(3);
                    devis.unite = reader.GetString(4);
                    devis.creation = reader.GetDateTime(5);
                    devis.margeCommercial = reader.GetInt32(6);
                    devis.margeEntreprise = reader.GetInt32(7);
                    devis.prixTotalHT = reader.GetInt32(6);
                    devis.prixTotalTTC = reader.GetInt32(6);
                    devis.plan = this.plan;
                    devis.projet = this.projet;

                    listeAllDevisParPlan.Add(devis);
                }
            }
            finally
            {
                reader.Close();
            }
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
        #endregion
    }
}