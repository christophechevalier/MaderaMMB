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

    /// <summary>
    /// Classe Devis 
    /// </summary>

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

        /// <summary>
        /// Méthode de calcul du prix TTC du module
        /// </summary>
        private void calculerPrixHT()
        {

        }

        /// <summary>
        /// Méthode de calcul du prix TTC du module
        /// </summary>
        private void calculerPrixTTC()
        {

        }

        /// <summary>
        /// Méthode de vérification de la validité du devis 
        /// </summary>
        private void verifierExpiration()
        {

        }
        /// <summary>
        /// Méthode pour changer l'état d'un devis
        /// </summary>
        private void changerEtat()
        {

        }

        /// <summary>
        /// Méthode de vérification de l'offre promotionnelle
        /// </summary>
        private void verifierOffrePromo()
        {

        }

        /// <summary>
        /// Méthode de génération du devis
        /// </summary>
        
        private void genererDocTech()
        {

        }

        #endregion
    }
}
