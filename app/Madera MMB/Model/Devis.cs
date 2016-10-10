using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    class Devis
    {
        #region properties
        public int reference { get; set; }
        public string nom { get; set; }
        public string etat { get; set; }
        public int quantite { get; set; }
        public string unite { get; set; }
        public DateTime creation { get; set; }
        public int margeCommercial { get; set; }
        public int margeEntreprise { get;set;}
        public int prixTotalHT { get; set; }
        public int prixTotalTTC { get; set; }
        #endregion

        #region Ctor

        #endregion

        //#region privates methods
        //private void calculerPrixHT();
        //private void calculerPrixTTC();
        //private void verifierExpiration();
        //private void valider();
        //private void refuser();
        //private void facturer();
        //private void verifierOffrePromo();
        //private void genererDocTech();

        //#endregion
    }
}
