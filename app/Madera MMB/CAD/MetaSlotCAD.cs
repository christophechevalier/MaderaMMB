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
    public class MetaSlotCAD
    {
        #region properties
        private List<MetaSlot> listemetaslot { get; set; }
        public Connexion conn { get; set; }
        public string SQLQuery { get; set; }
        #endregion

        #region Ctor
        public MetaSlotCAD(Connexion co)
        {
            this.conn = co;
            listemetaslot = new List<MetaSlot>();
        }
        #endregion

        #region privates methods
        private MetaSlot GetMetaSlotById(int id)
        {
            SQLQuery = "SELECT * FROM metaslot WHERE idMetaSlot = " + id;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;

            using (var reader = command.ExecuteReader())
            {
                MetaSlot metaslot = new MetaSlot(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(3));
                reader.Close();
                return metaslot;
            }
        }
        #endregion

        #region public methods
        public List<MetaSlot> getMetaslotByMetaModule(string refmetamodule)
        {
            SQLQuery = "SELECT idMetaSlot FROM metamodul_has_metaslot WHERE refMetaModule = " + refmetamodule;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    listemetaslot.Add(GetMetaSlotById(2));
                }
            }
            finally
            {
                reader.Close();
            }
            return this.listemetaslot;
        }
        #endregion
    }
}