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
        public String refProjet { get; set; }
        #endregion

        #region Ctor
        public PlanCAD(String uneref)
        {
            this.refProjet = uneref;
            Connexion conn = new Connexion();
        }
        #endregion

        #region privates methods
        private void getPlans()
        {

        }
        private void insertClient() { }

        #endregion
    }
}
