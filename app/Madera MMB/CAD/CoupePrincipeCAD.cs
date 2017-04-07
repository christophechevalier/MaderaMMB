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
using System.IO;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Madera_MMB.CAD
{
    public class CoupePrincipeCAD
    {
        #region properties
        public List<CoupePrincipe> Listecoupeprincipe { get; set; }
        public Connexion conn { get; set; }
        public CoupePrincipe coupe { get; set; }
        private string SQLQuery { get; set; }

        #endregion

        #region Ctor
        public CoupePrincipeCAD(Connexion co)
        {
            this.conn = co;
            Listecoupeprincipe = new List<CoupePrincipe>();
            listAllCoupePrincipe();
        }
        #endregion

        #region privates methods
        private void listAllCoupePrincipe()
        {
            SQLQuery = "SELECT * FROM coupeprincipe WHERE statut = 1 order by label desc";
            conn.LiteCo.Open();
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Trace.Write("#### GET COUPE PRINCIPE DATA #### \n");
                        while (reader.Read())
                        {
                            Byte[] data = (Byte[])reader.GetValue(5);

                            CoupePrincipe coupe = new CoupePrincipe
                            (
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                reader.GetInt32(4),
                                reader.GetBoolean(6), 
                                ToImage(data));
                            Listecoupeprincipe.Add(coupe);
                        }
                    }
                    Trace.WriteLine("#### GET COUPE PRINCIPE DATA SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION COUPES PRINCIPE ################################################# \n" + ex.ToString() + "\n");
                }
            }
            conn.LiteCo.Close();
        }
        #endregion

        #region Tools
        /// <summary>
        /// Méthode de conversion de type byte array en BitmapImage
        /// </summary>
        /// <param name="array"> tableau d'octets de l'image</param>
        /// <returns></returns>
        public BitmapImage ToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }
        #endregion
    }
}