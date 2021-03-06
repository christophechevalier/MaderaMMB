﻿using Madera_MMB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Madera_MMB.Lib;
using System.Data.SQLite;
using System.Diagnostics;
using System.Windows.Media.Imaging;

namespace Madera_MMB.CAD
{
    public class PlancherCAD
    {
        #region properties
        public List<Plancher> Listeplancher { get; set; }
        public string SQLQuery { get; set; }
        public Connexion conn { get; set; }
        private Plancher plancher { get; set; }
        #endregion

        #region Ctor
        public PlancherCAD(Connexion co)
        {
            Listeplancher = new List<Plancher>();
            this.conn = co;
            listAllPlancher();
        }
        #endregion

        #region privates methods
        private void listAllPlancher()
        {
            SQLQuery = "SELECT * FROM plancher WHERE statut = 1";
            conn.LiteCo.Open();
            using (SQLiteCommand command = new SQLiteCommand(SQLQuery, conn.LiteCo))
            {
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        Trace.Write("#### GET PLANCHERS DATA #### \n");
                        while (reader.Read())
                        {
                            Byte[] data = (Byte[])reader.GetValue(2);
                            Plancher plancher = new Plancher
                            (
                                reader.GetString(0), 
                                reader.GetInt32(1),
                                reader.GetBoolean(3), 
                                ToImage(data));
                            Listeplancher.Add(plancher);
                        }
                    }
                    Trace.Write("#### GET PLANCHERS DATA SUCCESS #### \n");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR RECUPERATION PLANCHERS ################################################# \n" + ex.ToString() + "\n");
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