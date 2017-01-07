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
        public string nomComposant { get; set; }
        public FamilleDeComposant famille { get; set; }
        #endregion

        #region Ctor
        public Composant(FamilleDeComposant fam)
        {
            this.famille = fam;
        }
        public Composant() { }
        #endregion

        #region privates methods

        #endregion
    }
}
