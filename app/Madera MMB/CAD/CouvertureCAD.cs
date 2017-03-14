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
    public class CouvertureCAD
    {
        #region properties
        public List<Couverture> Listecouverture { get; set; }
        public Connexion conn { get; set; }
        private Couverture couverture { get; set; }
        private string SQLQuery { get; set; }
        #endregion

        #region Ctor
        public CouvertureCAD(Connexion co)
        {
            this.conn = co;
            Listecouverture = new List<Couverture>();
            listAllCouverture();
        }
        #endregion

        #region privates methods
        private void listAllCouverture()
        {
            SQLQuery = "SELECT * FROM couverture WHERE statut = 1";
            conn.LiteCo.Open();
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Trace.Write("#### GET COUVERTURES DATA #### \n");
                        while (reader.Read())
                        {
                            Byte[] data = (Byte[])reader.GetValue(3);

                            Couverture couverture = new Couverture
                            (
                                reader.GetString(0),
                                reader.GetInt32(1),
                                reader.GetBoolean(2),
                                ToImage(data)
                            );
                            Listecouverture.Add(couverture);
                        }
                    }
                    Trace.WriteLine("#### GET COUVERTURES DATA SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION COUVERTURES ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();
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