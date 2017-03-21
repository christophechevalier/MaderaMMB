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
        public Gamme gamme { get; set; }
        public List<MetaSlot> metaslots { get; set; }
        public bool statut { get; set; }
        public BitmapImage image { get; set; }
        public int taille { get; set; }
        public int ecart { get; set; }
        #endregion

        #region Ctor
        public MetaModule(string reference, string label, int prix, Gamme gamme, bool statut, BitmapImage image, int taille, int ecart)
        {
            this.reference = reference;
            this.label = label;
            this.prixHT = prix;
            this.gamme = gamme;
            this.metaslots = new List<MetaSlot>();
            this.statut = statut;
            this.image = image;
            this.taille = taille;
            this.ecart = ecart;
        }
        public MetaModule() { }
        #endregion
    }
}