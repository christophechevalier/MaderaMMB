using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Madera_MMB.Model
{
    class Projet
    {
        #region properties
        public int reference { get; set; }
        public string nom { get; set; }
        public DateTime creation { get; set; }
        public DateTime modification { get; set; }
        public Commercial commercial { get; set; }
        public Client client { get; set; }

        #endregion

        #region Ctor

        // projet existant depuis la base

        // nouveau projet (connaitre déjà le commercial et le client)

        #endregion
    }
}
