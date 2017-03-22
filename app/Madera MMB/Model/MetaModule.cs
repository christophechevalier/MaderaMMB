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
        public bool statut { get; set; }
        public BitmapImage image { get; set; }
        public string dataMaj { get; set; }
        public int taille { get; set; }
        public int ecart { get; set; }
        public List<MetaSlot> metaslots { get; set; }
        #endregion

        #region Ctor
        public MetaModule() { }

        public MetaModule(string reference, string label, int prix, BitmapImage image, bool statut, string dateMaj, Gamme gamme, int taille, int ecart)
        {
            this.reference = reference;
            this.label = label;
            this.prixHT = prix;
            this.gamme = gamme;
            this.statut = statut;
            this.image = image;
            this.dataMaj = dataMaj;
            this.taille = taille;
            this.ecart = ecart;
            this.metaslots = new List<MetaSlot>();
        }
        #endregion
    }
}