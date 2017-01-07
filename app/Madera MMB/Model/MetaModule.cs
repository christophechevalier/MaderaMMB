using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    class MetaModule
    {
        #region properties
        private string reference { get;set;}
        public string label { get; set; }
        public int prixHT { get; set; }
        public int nbSlot { get; set; }
        public Gamme gamme { get; set; }
        public List<MetaSlot> metaslots { get; set; }
        public List<Composant> composants { get; set; }

        #endregion

        #region Ctor
        public MetaModule(List<Composant> composants)
        {
            this.composants = composants;
        }
        #endregion

        #region privates methods

        #endregion
    }
}
