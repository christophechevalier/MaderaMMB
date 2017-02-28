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
    class ComposantCAD
    {
        #region properties
        public List<Composant> listecomposant { get; set; }
        public Connexion conn { get; set; }
        public string SQLQuery { get; set; }
        FamilleComposantCAD familleCAD { get; set; }
        public int idcomp { get; set; }
        #endregion

        #region Ctor
        public ComposantCAD(Connexion co)
        {
            conn = co;
            familleCAD = new FamilleComposantCAD(conn);
        }
        #endregion

        #region privates methods
        private Composant getComposantByID(int id)
        {
            SQLQuery = "SELECT * FROM composant WHERE id_composant = " + id;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;

            using (var reader = command.ExecuteReader())
            {
                Composant composant = new Composant(reader.GetInt32(0), reader.GetString(1), this.familleCAD.getFamilleCompByName(reader.GetString(2)));
                return composant;
            }

        }

        #endregion

        #region public methods
        public List<Composant> listComposantByMetamodule(string refMetamodule)
        {

            SQLQuery = "SELECT id_composant FROM composant_has_metamodule WHERE refMetaModule = " + refMetamodule;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

             try
            {
                while (reader.Read())
                {
                    Composant composant = getComposantByID(reader.GetInt32(0));
                    this.listecomposant.Add(composant);
                }
            }
            finally
            {
                reader.Close();
            }
             return listecomposant;

        }
        #endregion
    }
}
