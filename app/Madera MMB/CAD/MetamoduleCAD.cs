using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Drawing;

namespace Madera_MMB.CAD
{
    class MetamoduleCAD
    {
        #region properties
        private List<MetaModule> listemodule { get; set; }
        public Connexion conn { get; set; }
        public string SQLQuery { get; set; }
        public GammeCAD gammeCAD { get; set; }
        public MetaSlotCAD metaslotCAD { get; set; }
        public ComposantCAD compCAD { get; set; }
        private Bitmap image { get; set; }
        private MetaModule metamodule { get; set; }

        #endregion

        #region Ctor
        public MetamoduleCAD (Connexion co, ComposantCAD compCAD, MetaSlotCAD metaslotCAD, GammeCAD gamCAD)
        {
            this.conn = co;
            this.gammeCAD = gamCAD;
            this.metaslotCAD = metaslotCAD;
            this.compCAD = compCAD;
            listemodule = new List<MetaModule>();
        }
        #endregion

        #region privates methods
        private void listAllMetaModules() 
        {
            SQLQuery = "SELECT * FROM Metamodule";
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    string queryimage = "SELEC image From metamodule where refMetaModule = " + reader.GetString(0);
                    SQLiteCommand cmdimg = (SQLiteCommand)conn.LiteCo.CreateCommand();
                    cmdimg.CommandText = queryimage;
                   
                    using (var readerimg = cmdimg.ExecuteReader())
                    {
                        while(readerimg.Read())
                        {
                            byte[]buffer = GetBytes(readerimg);
                            MemoryStream ms = new MemoryStream(buffer);
                            Bitmap image = new Bitmap(ms);

                            MetaModule metamodule = new MetaModule(
                                reader.GetString(0),
                                reader.GetString(1), 
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                image, 
                                this.gammeCAD.getGammebyNom(reader.GetString(5)), 
                                this.compCAD.listComposantByMetamodule(reader.GetString(0)),
                                this.metaslotCAD.getMetaslotByMetaModule(reader.GetString(0))
                                );
                            listemodule.Add(metamodule);
                        }
                        readerimg.Close();
                    }
                    
                }
            }
            finally
            {
                reader.Close();
            }
        }

        static byte[] GetBytes(SQLiteDataReader reader)
        {
            const int CHUNK_SIZE = 2 * 1024;
            byte[] buffer = new byte[CHUNK_SIZE];
            long bytesRead;
            long fieldOffset = 0;
            using (MemoryStream stream = new MemoryStream())
            {
                while ((bytesRead = reader.GetBytes(0, fieldOffset, buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, (int)bytesRead);
                    fieldOffset += bytesRead;
                }
                return stream.ToArray();
            }
        }
        #endregion

        #region public methods
        public MetaModule getMetaModuleByRef(string reference)
        {
            SQLQuery = "SELECT * FROM Metamodule WHERE refMetaModule = " + reference;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    string queryimage = "SELEC image From metamodule where refMetaModule = " + reader.GetString(0);
                    SQLiteCommand cmdimg = (SQLiteCommand)conn.LiteCo.CreateCommand();
                    cmdimg.CommandText = queryimage;

                    using (var readerimg = cmdimg.ExecuteReader())
                    {
                        while (readerimg.Read())
                        {
                            byte[] buffer = GetBytes(readerimg);
                            MemoryStream ms = new MemoryStream(buffer);
                            this.image = new Bitmap(ms);
                        }
                        readerimg.Close();
                    }

                                this.metamodule = new MetaModule(
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                this.image,
                                this.gammeCAD.getGammebyNom(reader.GetString(5)),
                                this.compCAD.listComposantByMetamodule(reader.GetString(0)),
                                this.metaslotCAD.getMetaslotByMetaModule(reader.GetString(0)));
                    reader.Close();
                }
            }
            finally
            {
                reader.Close();
            }
            return metamodule;
        }
        #endregion
    }
}
