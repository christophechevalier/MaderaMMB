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

        #endregion

        #region Ctor
        public Gamme(string nom, int offre, string isolant, string finition, string huisserie, BitmapImage img)
        {
            this.nom = nom;
            offrePromo = offre;
            typeIsolant = isolant;
            typeFinition = finition;
            qualiteHuisserie = huisserie;
            image = img;
        }
        #endregion

        #region privates methods

        #endregion
    }
}
