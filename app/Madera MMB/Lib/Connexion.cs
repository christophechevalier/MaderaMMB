using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.IO;
using System.ComponentModel;

namespace Madera_MMB.Lib
{
    public class Connexion : INotifyPropertyChanged
    {
        #region Properties
        public SQLiteConnection LiteCo { get; set; }
        public MySqlConnection MySQLCo { get; set; }
        private bool _MySQLconnected;
        public bool MySQLconnected
        { get { return _MySQLconnected; }
          set { _MySQLconnected = value; RaisePropertyChanged("MySQLconnected");}
        }
        public bool SQLiteconnected { get; set; }
        public SQLiteDataAdapter DataAdapter { get; set; }
        #endregion

        private void RaisePropertyChanged(String property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #region Ctor
        public Connexion()
        {
            // Test Connexion MySQL //
            MySQLconnected = OpenMySQLConnection();
            // Test Connexion SQLite //
            SQLiteconnected = CreateSQLiteBase();
        }
        #endregion

        #region Public Methods
        /// <summary>
        ///   Méthode de synchronisation des données des commerciaux depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        /// <returns>booléen renseignant le succès ou l'échec de la synchronisation des données des Commerciaux</returns>
        public bool SyncCommMySQL()
        {
            MySqlDataReader Reader;
            string query;

            Trace.WriteLine(" ############# TEST SYNC COMMERCIAL ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM Commercial", MySQLCo);
            try
            {
                Reader = selectComms.ExecuteReader();
                int i = 0;
                LiteCo.Open();
                while (Reader.Read())
                {
                    query = "replace into commercial(refCommercial, nom, prenom, email, motDePasse) values('" +
                    Reader.GetValue(0).ToString() + "','" +
                    Reader.GetValue(1).ToString() + "','" +
                    Reader.GetValue(2).ToString() + "','" +
                    Reader.GetValue(3).ToString() + "','" +
                    Reader.GetValue(4).ToString() + "')";

                    SQLiteCommand command = new SQLiteCommand(query, LiteCo);
                    try
                    {
                        i = i + command.ExecuteNonQuery();  
                    }
                    catch (System.Data.SQLite.SQLiteException e)
                    {
                        Trace.WriteLine(e.ToString());
                        LiteCo.Close();
                        return false;
                    } 
                }
                LiteCo.Close();
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC COMMERCIAL SUCCESS ############# \n");
                return true;
            }
            catch(MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC COMMERCIAL FAIL ############# \n");
                return false;
            }        
        }

        /// <summary>
        ///   Méthode de appelant les méthodes de synchronisation des données de paramètre des plans
        /// </summary>
        /// <returns>booléen renseignant le succès ou l'échec de la synchronisation des données des coupes de principe</returns>
        public void SyncParamPlan()
        {
            SyncCoupePrincipe();
            SyncCouverture();
            SyncPlancher();
            SyncGamme();
        }

        /// <summary>
        ///   Méthode de synchronisation des données des coupes de principe depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        public void SyncCoupePrincipe()
        {
            MySqlDataReader Reader;
            string query;
            Trace.WriteLine(" ############# TEST SYNC COUPE PRINCIPE ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM coupeprincipe", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;
                LiteCo.Open();
                while (Reader.Read())
                {
                    Byte[] data = (Byte[])Reader.GetValue(5);
                    query = "replace into coupeprincipe(id_coupe, label, longueur, largeur, prixHT, image) values(@id, @label, @longueur, @largeur, @prixHT, @image)";

                    using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
                    {
                        command.Parameters.AddWithValue("@id", Reader.GetInt32(0));
                        command.Parameters.AddWithValue("@label", Reader.GetString(1));
                        command.Parameters.AddWithValue("@longueur", Reader.GetInt32(2));
                        command.Parameters.AddWithValue("@largeur", Reader.GetInt32(3));
                        command.Parameters.AddWithValue("@prixHT", Reader.GetInt32(3));
                        command.Parameters.AddWithValue("@image", data);
                        try
                        {
                            i = i + command.ExecuteNonQuery();
                        }
                        catch (System.Data.SQLite.SQLiteException e)
                        {
                            Trace.WriteLine(e.ToString());
                            LiteCo.Close();
                            MySQLCo.Close();
                        }
                    }
                }
                LiteCo.Close();
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC COUPE PRINCIPE SUCESS ############# \n");
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC COUPE PRINCIPE FAIL ############# \n");
            }
        }

        /// <summary>
        ///   Méthode de synchronisation des données des couvertures depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        public void SyncCouverture()
        {
            MySqlDataReader Reader;
            string query;
            Trace.WriteLine(" ############# TEST SYNC COUVERTURE ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM couverture", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;
                LiteCo.Open();
                while (Reader.Read())
                {
                    Byte[] data = (Byte[])Reader.GetValue(2);
                    query = "replace into couverture(typeCouverture, prixHT, image) values(@typeCouverture, @prixHT, @image)";

                    using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
                    {
                        command.Parameters.AddWithValue("@typeCouverture", Reader.GetString(0));
                        command.Parameters.AddWithValue("@prixHT", Reader.GetUInt32(1));
                        command.Parameters.AddWithValue("@image", data);
                        try
                        {
                            i = i + command.ExecuteNonQuery();
                        }
                        catch (System.Data.SQLite.SQLiteException e)
                        {
                            Trace.WriteLine(e.ToString());
                            LiteCo.Close();
                            MySQLCo.Close();
                        }
                    }
                }
                LiteCo.Close();
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC COUVERTURE SUCESS ############# \n");
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC COUVERTURE FAIL ############# \n");
            }
        }

        /// <summary>
        ///   Méthode de synchronisation des données des planchers depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        public void SyncPlancher()
        {
            MySqlDataReader Reader;
            string query;
            Trace.WriteLine(" ############# TEST SYNC PLANCHER ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM plancher", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;
                LiteCo.Open();
                while (Reader.Read())
                {
                    Byte[] data = (Byte[])Reader.GetValue(2);
                    query = "replace into plancher(typePlancher, prixHT, image) values(@typePlancher, @prixHT, @image)";

                    using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
                    {
                        command.Parameters.AddWithValue("@typePlancher", Reader.GetString(0));
                        command.Parameters.AddWithValue("@prixHT", Reader.GetUInt32(1));
                        command.Parameters.AddWithValue("@image", data);
                        try
                        {
                            i = i + command.ExecuteNonQuery();
                        }
                        catch (System.Data.SQLite.SQLiteException e)
                        {
                            Trace.WriteLine(e.ToString());
                            LiteCo.Close();
                            MySQLCo.Close();
                        }
                    }
                }
                LiteCo.Close();
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC PLANCHER SUCESS ############# \n");
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC PLANCHER FAIL ############# \n");
            }
        }

        /// <summary>
        ///   Méthode de synchronisation des données des gammes depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        public void SyncGamme()
        {
            MySqlDataReader Reader;
            string query;
            Trace.WriteLine(" ############# TEST SYNC GAMME ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM gamme", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;
                LiteCo.Open();
                while (Reader.Read())
                {
                    Byte[] data = (Byte[])Reader.GetValue(5);
                    query = "replace into gamme(nom, offrePromo,typeIsolant,typeFinition,qualiteHuisserie, image) values(@nom, @offrePromo, @typeIsolant, @typeFinition, @qualiteHuisserie, @image)";

                    using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
                    {
                        command.Parameters.AddWithValue("@nom", Reader.GetString(0));
                        command.Parameters.AddWithValue("@offrePromo", Reader.GetUInt32(1));
                        command.Parameters.AddWithValue("@typeIsolant", Reader.GetString(2));
                        command.Parameters.AddWithValue("@typeFinition", Reader.GetString(3));
                        command.Parameters.AddWithValue("@qualiteHuisserie", Reader.GetString(4));
                        command.Parameters.AddWithValue("@image", data);
                        try
                        {
                            i = i + command.ExecuteNonQuery();
                        }
                        catch (System.Data.SQLite.SQLiteException e)
                        {
                            Trace.WriteLine(e.ToString());
                            LiteCo.Close();
                            MySQLCo.Close();
                        }
                    }
                }
                LiteCo.Close();
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC GAMME SUCESS ############# \n");
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC GAMME FAIL ############# \n");
            }
        }

        /// <summary>
        /// Méthode de test d'insertion dans la base SQLite
        /// </summary>
        /// <param name="query">Requête d'insertion</param>
        public void InsertSQliteQuery(string query)
        {
            try
            {
                LiteCo.Open();
                SQLiteCommand command = new SQLiteCommand(query, LiteCo);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch(System.Data.SQLite.SQLiteException e)
                {
                    Trace.WriteLine(e.ToString());
                    LiteCo.Close(); 
                }
            }
            catch (SQLiteException ex)
            {
                Trace.WriteLine(ex.ToString());
                LiteCo.Close();
            }
            LiteCo.Close();
        }

        /// <summary>
        /// Méthode de test de sélection depuis la base SQLite
        /// </summary>
        /// <param name="query">Requête de sélection</param>
        public void SelectSQLiteQuery(string query)
        {
            try
            {
                LiteCo.Open();
                SQLiteCommand command = (SQLiteCommand)this.LiteCo.CreateCommand();
                command.CommandText = query;
                SQLiteDataReader reader = command.ExecuteReader();

                try
                {
                    while (reader.Read())
                    {
                        //for (int i = 0; i < reader.VisibleFieldCount; i++)
                        //{
                        //    Trace.WriteLine(" ############# " + reader.GetValue(i).ToString() + " ############# \n");
                        //}
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            catch (SQLiteException ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }
 
        #endregion

        #region Privates Methods
        /// <summary>
        /// Méthode testant l'existence d'une base SQLite, la créé si inexistante
        /// </summary>
        /// <returns>booléen renseignant le succès ou l'échec de la création/connexion à l base SQLite</returns>
        private bool CreateSQLiteBase()
        {
            if (File.Exists("Madera.bdd"))
            {
                Trace.WriteLine(" \n ################################################# SQLITE DATABASE EXISTS ################################################# \n");
                try
                {
                    this.LiteCo = new SQLiteConnection("Data Source=Madera.bdd;Version=3;");
                    Trace.WriteLine(" \n ################################################# SQLITE DATABASE CONNECTED ################################################# \n");
                    LiteCo.Close();
                    return true;
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERROR CONNECTION BASE SQLITE ################################################# \n" + ex.ToString() + "\n");
                    LiteCo.Close();
                    return false;
                }
            }
            else
            {
                Trace.WriteLine(" \n ################################################# SQLITE DATABASE NOT EXISTS ################################################# \n");
                string strCommand = File.ReadAllText("SQLiteScript.sql");
                LiteCo = new SQLiteConnection("Data Source=Madera.bdd;Version=3;");
                SQLiteCommand command = new SQLiteCommand(strCommand, LiteCo);
                LiteCo.Open();
                try
                {
                    command.ExecuteNonQuery();
                    LiteCo.Close();
                    Trace.WriteLine(" \n ################################################# CREATION BASE SQLITE SUCCESS ################################################# \n");
                    return true;
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# ERREUR CREATION BASE SQLITE ################################################# \n" + ex.ToString() + "\n");
                    LiteCo.Close();
                    return false;
                }
            }
        }

        /// <summary>
        /// Méthode testant la connexion à la base distante MySQL
        /// </summary>
        /// <returns>booléen renseignant le succès ou l'échec de la création/connexion à l base MySQL</returns>
        private bool OpenMySQLConnection()
        {
            string connectionString = "SERVER=localhost;DATABASE=madera_mmb;UID=root;PASSWORD=;";
            try
            {
                MySQLCo = new MySqlConnection(connectionString);
                MySQLCo.Open();
                Trace.WriteLine(" \n ################################################# MYSQL DATABASE REACHED,  BEGIN SYNCHRONISATION ... ################################################# \n");
                return true;
            }
            catch (MySqlException ex)
            {
                Trace.WriteLine("################################################# ERROR CONNECTION MYSQL SERVER #################################################");
                Trace.WriteLine(ex.ToString());
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Trace.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Trace.WriteLine("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }
        #endregion
    }
}