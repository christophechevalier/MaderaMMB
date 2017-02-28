using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Madera_MMB.Model
{
    public class Plancher
    {
        #region properties
        public string type { get; set; }
        public int prixHT { get; set; }
        public BitmapImage image { get; set; }
        #endregion

        #region Ctor
        public Plancher(string type, int prix, BitmapImage img = null)
        {
            this.type = type;
            this.prixHT = prix;
            image = img;
        }
        public Plancher() { }
        #endregion
    }
}
