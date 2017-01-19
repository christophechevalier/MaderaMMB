using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Madera_MMB.Model
{
    class MetaModule
    {
        #region properties
        public string reference { get; set; }
        public string label { get; set; }
        public int prixHT { get; set; }
        public int nbSlot { get; set; }
        public Bitmap image { get; set; }
        public Gamme gamme { get; set; }
        public List<MetaSlot> metaslots { get; set; }
        public List<Composant> composants { get; set; }

        #endregion

        #region Ctor
        public MetaModule(string reference, string label, int prix, int nbslot, Bitmap image, Gamme gamme, List<Composant> composants, List<MetaSlot> metaslots)
        {
            this.reference = reference;
            this.label = label;
            this.prixHT = prix;
            this.nbSlot = nbslot;
            this.image = image;
            this.gamme = gamme;
            this.composants = composants;
            this.metaslots = metaslots;
        }
        #endregion

        #region privates methods

        #endregion
    }
}