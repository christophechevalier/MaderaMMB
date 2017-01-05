using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{

    // TO DO
        // - ctor OK
        // - Méthodes privées :
        //    - ajouterModule OK
        //    - supprimerModule OK
        //    - générerDevis - Manque CAD
        //    - tracerSlot
        //    - sauvegarder - Manque CAD
    // 

    class Plan
    {
        #region properties
        public string reference { get; set; }
        public string label { get; set; }
        public Projet projet { get; set; }
        public DateTime creation { get; set; }
        public DateTime modification { get; set; }
        public Couverture couverture { get; set; }
        public CoupePrincipe coupePrincipe { get; set; }
        public Plancher plancher { get; set; }
        public Gamme gamme { get; set; }
        public List<Module> modules { get; set; }
        #endregion

        #region Ctor
        public Plan(Projet unprojet,Plancher unplancher, Couverture unecouverture, CoupePrincipe unecoupe, Gamme unegamme)
        {
            this.projet = unprojet;
            this.plancher = unplancher;
            this.couverture = unecouverture;
            this.coupePrincipe = unecoupe;
            this.gamme = unegamme;
        }
        public Plan() { }
        #endregion

        #region privates methods
        private void ajouterModule(Module module)
        {
            this.modules.Add(module);
        }
        private void supprimerModule(Module module)
        {
            this.modules.Remove(module);
        }
        private void genereDevis()
        {
            Devis generateDevis = new Devis();
        }
        private void tracerSlot()
        {
            
        }

        // Manque CAD / utilisée ailleurs
        // private void sauvegarder();

        #endregion
    }
}
