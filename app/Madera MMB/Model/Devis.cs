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
        public string nom { get; set; }
        public string etat { get; set; }
        public string quantite { get; set; }
        public string unite { get; set; }
        public DateTime creation { get; set; }
        public int margeCommercial { get; set; }
        public int margeEntreprise { get;set;}
        public int prixTotalHT { get; set; }
        public int prixTotalTTC { get; set; }
        public Plan plan { get; set; }
        public Projet projet { get; set; }
        public Client client { get; set; }
        public Commercial commercial { get; set; }

        #endregion

        #region Ctor
        public Devis(Plan unplan, Projet unprojet, Client unclient, Commercial uncommercial)
        {
            this.plan = unplan;
            this.projet = unprojet;
            this.client = unclient;
            this.commercial = uncommercial;
        }
        public Devis() { }
        #endregion

        #region privates methods

        private void calculerPrixHT();
        private void calculerPrixTTC();
        private void verifierExpiration();
        private void changerEtat();
        private void verifierOffrePromo();
        private void genererDocTech();

        #endregion
    }
}
