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
    class PlanCAD
    {
        #region properties
        public List<Plan> listePlanParProjet { get; set; }
        public Connexion conn { get; set; }
        public Projet projet { get; set; }
        public string SQLQuery{get; set;}
        public CouvertureCAD couvCAD {get;set;}
        public CoupePrincipeCAD coupeCAD { get; set; }
        public PlancherCAD plancherCAD { get; set; }
        public GammeCAD gammeCAD { get; set; }

        #endregion

        #region Ctor
        public PlanCAD(Connexion laConnexion, Projet unprojet, CouvertureCAD couvCAD, CoupePrincipeCAD coupeCAD, PlancherCAD plancherCAD, GammeCAD gammeCAD)
        {
            this.projet = unprojet;
            Connexion conn = laConnexion;
            listePlanParProjet = new List<Plan>();
            this.couvCAD = couvCAD;
            this.coupeCAD = coupeCAD;
            this.plancherCAD = plancherCAD;
            this.gammeCAD = gammeCAD;
            
        }
        #endregion

        #region privates methods
        private void listPlanParProjet()
        {
            SQLQuery = "SELECT * FROM Plan WHERE refProjet = " + projet.reference;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Plan plan = new Plan();
                    plan.reference = reader.GetString(0);
                    plan.label = reader.GetString(1);
                    plan.creation = reader.GetDateTime(2);
                    plan.modification = reader.GetDateTime(3);
                    plan.projet = this.projet;
                    plan.couverture = couvCAD.getCouvbyType(reader.GetString(7));
                    plan.coupePrincipe = coupeCAD.getCoupebyId(8);
                    plan.plancher = plancherCAD.getPlancherbyType(reader.GetString(9));
                    plan.gamme = gammeCAD.getGammebyNom(reader.GetString(10));

                    listePlanParProjet.Add(plan);
                }
            }
            finally
            {
                reader.Close();
            }
        }
        private void insertPlan(Plan plan, string refClient, string refCommercial) 
        {
            SQLQuery = "INSERT INTO `plan` (`refPlan`, `label`, `dateCreation`, `dateModification`, `refProjet`, `refClient`, `refCommercial`, `typeCouverture`, `id_coupe`, `typePlancher`, `nomGamme`)"+
            "VALUES ("+plan.reference+","+plan.label+","+plan.creation+","+plan.modification+","+plan.projet.reference+","+refClient+","+refCommercial+","+plan.couverture.type+","+plan.coupePrincipe.id+","+plan.plancher.type+","+plan.gamme.nom+";";
            conn.InsertSQliteQuery(SQLQuery);
        }
        #endregion
    }
}
