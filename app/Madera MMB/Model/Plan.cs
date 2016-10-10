using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    class Plan
    {
        #region properties
        public int reference { get; set; }
        public string label { get; set; }
        public DateTime creation { get; set; }
        public Gamme gamme { get; set; }
        public Plancher plancher { get;set;}
        public CoupePrincipe coupePrincipe { get; set; }
        public Couverture couverture { get; set; }
        #endregion

        #region Ctor

        #endregion

        //#region privates methods
        //private void ajouterModule();
        //private void supprimerModule();
        //private void genereDevis();
        //private void tracerSLot();
        //private void sauvegarder();

        //#endregion
    }
}
