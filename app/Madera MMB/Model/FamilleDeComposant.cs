using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    class FamilleDeComposant
    {
        #region properties
        public string nomFamille { get; set; }

        #endregion

        #region Ctor
        public FamilleDeComposant(string nom)
        {
            this.nomFamille = nom;
        }
        #endregion

    }
}
