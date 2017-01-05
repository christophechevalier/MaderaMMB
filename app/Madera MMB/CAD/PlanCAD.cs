using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.Odbc;
using System.Data.SQLite;

namespace Madera_MMB.CAD
{
    class PlanCAD
    {
        #region properties
        public List<Plan> listePlanParProjet { get; set; }
        public Connexion conn { get; set; }
        public string refProjet { get; set; }
        public string SQLQuery{get; set;}
        #endregion

        #region Ctor
        public PlanCAD(Connexion laConnexion, String uneref)
        {
            this.refProjet = uneref;
            Connexion conn = laConnexion;
        }
        #endregion

        #region privates methods
        private void getPlans()
        {
            SQLQuery = "SELECT * FROM Plan WHERE refProjet = " + refProjet;
            this.conn.ExecSQliteQuery(SQLQuery);
        }
        private void insertPlan(Plan plan, string refClient, string refCommercial) 
        {
            SQLQuery = "INSERT INTO `plan` (`refPlan`, `label`, `dateCreation`, `dateModification`, `refProjet`, `refClient`, `refCommercial`, `typeCouverture`, `id_coupe`, `typePlancher`, `nomGamme`)"+
            "VALUES ("+plan.reference+","+plan.label+","+plan.creation+","+plan.modification+","+plan.projet.reference+","+refClient+","+refCommercial+","+plan.couverture.type+","+plan.coupePrincipe.id+","+plan.plancher.type+","+plan.gamme.nom+";";
            this.conn.ExecSQliteQuery(SQLQuery);
        }

        #endregion
    }
}
