using Madera_MMB.Lib.Tools;
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
        public string creation { get; set; }
        public float prixTotalHT { get; set; }
        public float prixTotalTTC { get; set; }
        public Plan plan { get; set; }


        #endregion

        #region Ctor
        /// <summary>
        /// Constructeur de la classe à la création d'un nouveau devis depuis l'application
        /// </summary>
        /// <param name="unplan">Prend un plan à partir du quel créer le devis</param>
        public Devis(Plan unplan)
        {
            Random number = new Random();
            this.reference = unplan.reference + " D:" + number.Next(0, 9999); 
            this.plan = unplan;
            this.creation = DateTime.Now.ToString();
        }

        /// <summary>
        /// Constructeur de la classe à la récupération un devis existant depuis la base de données
        /// </summary>
        /// <param name="unplan">Prend un plan à partir du quel créer le devis</param>
        public Devis(string reference, string etat, string creation, float prixHT, float prixTTC, Plan plan)
        {
            this.reference = reference;
            this.etat = etat;
            this.creation = creation;
            this.prixTotalHT = prixHT;
            this.prixTotalTTC = prixTTC;
            this.plan = plan;
        }
        #endregion

        #region privates methods

        /// <summary>
        /// Méthode calculant le prix total Taxes Comprises du plan
        /// </summary>
        /// <returns>prix TTC</returns>
        private float calculerPrixTTC()
        {
            float prixHT = calculerPrixHT();
            prixHT += (prixHT * 20) / 100;
            this.prixTotalTTC = prixHT;
            return prixTotalTTC;          
        }

        /// <summary>
        /// Méthode calculant le prixc Total Hors Taxes des éléments du plan
        /// </summary>
        /// <returns>prix HT</returns>
        private float calculerPrixHT()
        {
            int prixHT = 0;
            foreach (Module mod in plan.modules)
            {
                prixHT += mod.meta.prixHT;
            }
            int prixcouv = calculCouverture();
            int prixplanch = calculPlancher();
            int prixcoupe = this.plan.coupePrincipe.prixHT;
            this.prixTotalHT = prixHT + prixcouv + prixplanch + prixcoupe;
            return prixTotalHT;
        }

        /// <summary>
        /// Méthode calculant le prix HT de la couverture de ce plan selon la coupe de principe
        /// </summary>
        /// <returns>Prix total HT de la couverture</returns>
        private int calculCouverture()
        {
            int prixcouv = this.plan.couverture.prixHT;
            int quotient1 = this.plan.coupePrincipe.largeur;
            int quotient2 = this.plan.coupePrincipe.longueur;

            return prixcouv * quotient1 * quotient2;
        }

        /// <summary>
        /// Méthode calculant le prix HT du plancher de ce plan selon la coupe de principe
        /// </summary>
        /// <returns>Prix total HT du plancher</returns>
        private int calculPlancher()
        {
            int prixplanch = this.plan.plancher.prixHT;
            int quotient1 = this.plan.coupePrincipe.largeur;
            int quotient2 = this.plan.coupePrincipe.longueur;

            return prixplanch * quotient1 * quotient2;
        }

        /// <summary>
        /// Méthode déterminant si la gamme du plan a bien été respectée pour tous les modules du plan
        /// </summary>
        /// <returns></returns>
        private bool checkGamme()
        {
            bool isgamme = true;
            foreach (Module mod in plan.modules)
            {
                if (mod.meta.gamme != plan.gamme)
                    isgamme = false;
            }
            return isgamme;
        }


        //private void verifierExpiration();
        //private void changerEtat();
        //private void verifierOffrePromo();
        //private void genererDocTech();

        #endregion
    }
}
