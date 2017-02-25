using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    class Composant
    {
        #region properties
        public int id { get; set; }
        public string nomComposant { get; set; }
        public FamilleDeComposant famille { get; set; }
        #endregion

        #region Ctor
        public Composant(int id, string nom, FamilleDeComposant fam)
        {
            this.id = id;
            this.nomComposant = nom;
            this.famille = fam;
        }
        public Composant() { }
        #endregion
    }
}
