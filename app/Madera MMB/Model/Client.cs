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
        public DateTime creation { get; set; }
        public DateTime modification { get; set; }
        public string nomprenom
        {
            get { return prenom + " " + nom; }
            set { }
        }
        #endregion

        #region Ctor
        /// <summary>
        /// Constructeur de Client à la création dans ParamClient
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <param name="adresse"></param>
        /// <param name="codePostal"></param>
        /// <param name="ville"></param>
        /// <param name="email"></param>
        /// <param name="telephone"></param>
        /// <param name="creation"></param>
        public Client(string reference, string nom, string prenom, string adresse, string codePostal, string ville, string email, string telephone, DateTime creation)
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
        }

        /// <summary>
        /// Constructeur de Client à la génération depuis BDD
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="nom"></param>
        /// <param name="prenom"></param>
        /// <param name="adresse"></param>
        /// <param name="codePostal"></param>
        /// <param name="ville"></param>
        /// <param name="email"></param>
        /// <param name="telephone"></param>
        /// <param name="creation"></param>
        /// <param name="modification"></param>
        public Client(string reference, string nom, string prenom, string adresse, string codePostal, string ville, string email, string telephone, DateTime creation, DateTime modification)
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
