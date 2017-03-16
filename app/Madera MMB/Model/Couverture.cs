using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Madera_MMB.Model
{
    public class Couverture
    {
        #region properties
        public string type { get;set; }
        public int prixHT { get; set; }
        public string modification { get; set; }
        public bool statut { get; set; }
        public BitmapImage image { get; set; }
        #endregion

        #region Ctor
        public Couverture(string type, int prix, string modification, bool statut, BitmapImage img)
        {
            this.type = type;
            this.prixHT = prix;
            this.modification = modification;
            this.statut = statut;
            this.image = img;
        }
        public Couverture() { }
        #endregion
    }
}
