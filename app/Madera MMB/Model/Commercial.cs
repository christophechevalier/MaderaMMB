using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    public class Commercial
    {
        #region properties
        public string reference { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string motDePasse { get; set; }
        public string email { get; set; }
        #endregion

        #region Ctor
        public Commercial(string reference, string nom, string prenom, string email, string motDePasse)
        {
            this.reference = reference;
            this.nom = nom;
            this.prenom = prenom;
            this.email = email;
            this.motDePasse = motDePasse;
        }
        public Commercial() { }
        #endregion
    }


}
