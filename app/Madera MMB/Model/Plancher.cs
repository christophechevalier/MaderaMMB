using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    public class Plancher
    {
        #region properties
        public string type { get; set; }
        public int prixHT { get; set; }

        #endregion

        #region Ctor
        public Plancher(string type, int prix)
        {
            this.type = type;
            this.prixHT = prix;
        }
        #endregion

        #region privates methods

        #endregion
    }
}
