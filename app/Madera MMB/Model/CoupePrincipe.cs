using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    class CoupePrincipe
    {
        #region properties

        public int id { get; set; }
        public string label { get; set; }
        public int longueur { get; set; }
        public int largeur { get; set; }
        public int prixHT { get; set; }
        #endregion

        #region Ctor
        public CoupePrincipe(int id, string label, int longueur, int largeur, int prix)
        {
            this.id = id;
            this.label = label;
            this.longueur = longueur;
            this.largeur = largeur;
            this.prixHT = prix;
        }
        #endregion

        #region privates methods

        #endregion
    }
}
