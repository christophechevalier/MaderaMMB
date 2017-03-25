using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    // TO DO
    // - properties OK
    // - ctor OK
    // - Méthodes privées :
    //    - calculerPrixHT X
    //    - calculerPrixTTC X
    //    - verifierExpiration X
    //    - changerEtat X
    //    - verifierOffrePromo X
    //    - genererDocTech X
    //
    class Devis
    {
        #region properties
        public string reference { get; set; }
        public string etat { get; set; }
        public string quantite { get; set; }
        public string unite { get; set; }
        public string creation { get; set; }
        public float margeCommercial { get; set; }
        public float margeEntreprise { get;set;}
        public float prixTotalHT { get; set; }
        public float prixTotalTTC { get; set; }
        public Plan plan { get; set; }


        #endregion

        #region Ctor
        public Devis(Plan unplan)
        {
            this.plan = unplan;
        }
        public Devis() { }
        #endregion

        #region privates methods

        private void calculerPrixHT()
        {
            this.prixTotalHT = 0;
            foreach(Module mod in plan.modules)
            {
                prixTotalHT += mod.metaModule.prixHT;
            }
        }
        private void calculerPrixTTC()
        {

        }
        //private void verifierExpiration();
        //private void changerEtat();
        //private void verifierOffrePromo();
        //private void genererDocTech();

        #endregion
    }
}
