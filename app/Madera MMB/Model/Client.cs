using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    public class Client
    {
        #region properties
        public string reference { get; set; }
        public string nom { get; set; }
        public string prenom { get; set; }
        public string adresse { get; set; }
        public string codePostal { get; set; }
        public string ville { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
        public string creation { get; set; }
        public string modification { get; set; }
        public string nomprenom
        {
            get { return prenom + " " + nom; }
            set { }
        }
        #endregion

        #region Ctor
        public Client(string reference, string nom, string prenom, string adresse, string codePostal, string ville, string email, string telephone, string creation, string modification)
        {
            this.reference = reference;
            this.nom = nom;
            this.prenom = prenom;
            this.adresse = adresse;
            this.codePostal = codePostal;
            this.ville = ville;
            this.email = email;
            this.telephone = telephone;
            this.creation = creation;
            this.modification = modification;
        }
        public Client() { }
        #endregion
    }
}
