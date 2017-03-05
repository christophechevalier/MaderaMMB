using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Madera_MMB.Model
{
    public class Gamme
    {
        #region properties
        public string nom { get; set; }
        public int offrePromo { get; set; }
        public string typeIsolant { get; set; }
        public string typeFinition { get; set; }
        public string qualiteHuisserie { get; set; }
        public BitmapImage image { get; set; }
        public bool statut { get; set; }
        #endregion

        #region Ctor
        public Gamme(string nom, int offre, string isolant, string finition, string huisserie, BitmapImage img, bool statut)
        {
            this.nom = nom;
            this.offrePromo = offre;
            this.typeIsolant = isolant;
            this.typeFinition = finition;
            this.qualiteHuisserie = huisserie;
            image = img;
            this.statut = statut;
        }
        public Gamme() { }
        #endregion
    }
}
