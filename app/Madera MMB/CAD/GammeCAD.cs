using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;
using System.Data;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace Madera_MMB.CAD
{
    public class GammeCAD
    {
        #region properties
        public List<Gamme> Listegamme { get; set; }
        public string SQLQuery { get; set; }
        public Connexion conn { get; set; }
        private Gamme gamme { get; set; }
        #endregion

        #region Ctor
        public GammeCAD(Connexion co)
        {
            Listegamme = new List<Gamme>();
            this.conn = co;
            listAllGamme();
        }
        #endregion

        #region privates methods
        private void listAllGamme() 
        {
            SQLQuery = "SELECT * FROM gamme";
            conn.LiteCo.Open();
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Byte[] data = (Byte[])reader.GetValue(5);
                            Gamme gamme = new Gamme(reader.GetString(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), ToImage(data));
                            Listegamme.Add(gamme);
                        }
                    }
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION GAMMES ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();
        }
        #endregion

        #region public methods
        /// <summary>
        /// Renvoie une gamme selon son nom
        /// </summary>
        /// <param name="type">nom de la gamme recherchée</param>
        /// <returns></returns>
        public Gamme getGammebyNom(string nom)
        {
            SQLQuery = "SELECT * FROM Gamme WHERE nom = " + nom;
            SQLiteCommand command = (SQLiteCommand)conn.LiteCo.CreateCommand();
            command.CommandText = SQLQuery;
            SQLiteDataReader reader = command.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    Byte[] data = (Byte[])reader.GetValue(5);
                    Gamme gamme = new Gamme(reader.GetString(0), reader.GetInt32(1), reader.GetString(2), reader.GetString(3), reader.GetString(4), ToImage(data));
                }
            }
            finally
            {
                reader.Close();
            }
            return gamme;
        }
        #endregion

        #region Tools
        /// <summary>
        /// Méthode de conversion de type byte array en BitmapImage
        /// </summary>
        /// <param name="array">tableau d'octets de l'image</param>
        /// <returns></returns>
        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        #endregion
    }
}
