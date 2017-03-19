using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace Madera_MMB.Model
{
    public class MetaModule
    {
        #region properties
        public string reference { get; set; }
        public string label { get; set; }
        public int prixHT { get; set; }
        public int nbSlot { get; set; }
        public Gamme gamme { get; set; }
        public List<MetaSlot> metaslots { get; set; }
        public bool statut { get; set; }
        public BitmapImage image { get; set; }
        #endregion

        #region Ctor
        public MetaModule(string reference, string label, int prix, int nbslot, Gamme gamme, List<MetaSlot> metaslots, bool statut, BitmapImage image)
        {
            this.reference = reference;
            this.label = label;
            this.prixHT = prix;
            this.nbSlot = nbslot;
            this.gamme = gamme;
            this.metaslots = metaslots;
            this.statut = statut;
            this.image = image;
        }
        public MetaModule() { }
        #endregion
    }
}