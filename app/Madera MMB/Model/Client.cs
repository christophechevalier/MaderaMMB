using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
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

        // client existant depuis la base

        // nouveau client

        #endregion
    }
}
