using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    /// <summary>
    /// * Règles de gestion Client :
    /// - Ce qui définit l’unicité d’un client, c’est son adresse et/ou son email
    /// - On peut modifier les informations relatives aux clients
    /// - Dans le cas où le nom et/ou prénom sont modifiés, on met à jour la liste des noms des clients qui pourront être sélectionnés dans un nouveau projet
    /// - On ne peut pas supprimer un client ou modifier la référence d’un client
    /// </summary>
    class Client
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
        #endregion

        #region Ctor
        public Client(string reference, string nom, string prenom, string adresse, string codePostal, string ville, string email,  string telephone)
        {
            this.reference = reference;
            this.nom = nom;
            this.prenom = prenom;
            this.adresse = adresse;
            this.codePostal = codePostal;
            this.ville = ville;
            this.email = email;
            this.telephone = telephone;
        }
        public Client() { }
        #endregion
    }
}
