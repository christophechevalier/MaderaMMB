
using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;
using System.Data;

namespace Madera_MMB.CAD
{
    class FamilleComposantCAD
    {
        #region properties
        public Connexion conn { get; set; }
        public string SQLQuery { get; set; }
        #endregion

        #region Ctor
        public FamilleComposantCAD(Connexion conn)
        {
            this.conn = conn;
        }
        #endregion

        #region public methods

        public FamilleDeComposant getFamilleCompByName(string nom)
        {
            SQLQuery = "SELECT * FROM famillecomposant WHERE nom = " + nom;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;

            using (var reader = command.ExecuteReader())
            {
                FamilleDeComposant famille = new FamilleDeComposant(reader.GetString(0));
                return famille;
            }
        }

        #endregion
    }
}