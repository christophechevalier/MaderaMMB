using System;
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
        {
            get { return _MySQLconnected; }
            set { _MySQLconnected = value; RaisePropertyChanged("MySQLconnected"); }
        }
        public bool SQLiteconnected { get; set; }
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

        #region Synchro Import
        /// <summary>
        ///  Méthode de synchronisation des données des commerciaux depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        public void SyncCommMySQL()
        {
            MySqlDataReader Reader;
            string query;
            Trace.WriteLine(" ############# TEST SYNC COMMERCIAL ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM Commercial", MySQLCo);
            try
            {
                MySQLCo.Open();
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
                    }
                }
                LiteCo.Close();
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC COMMERCIAL SUCCESS ############# \n");
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC COMMERCIAL FAIL ############# \n");
            }
        }

        /// <summary>
        ///  Méthode de synchronisation des données des projets selon un commercial depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        /// <param name="commercial">le commercial dont les projets vont être chargés</param>
        public void SynCProjetsComm(Model.Commercial commercial)
        {
            MySqlDataReader Reader;
            string query;
            Trace.WriteLine(" ############# TEST SYNC PROJETS FROM COMMERCIAL ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM projet WHERE refCommercial = '"+commercial.reference+"'", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;
                LiteCo.Open();
                while (Reader.Read())
                {
                    query = "replace into projet(refProjet, nom, dateCreation, dateModification, refClient, refCommercial) values('" +
                    Reader.GetValue(0).ToString() + "','" +
                    Reader.GetValue(1).ToString() + "','" +
                    Reader.GetValue(2).ToString() + "','" +
                    Reader.GetValue(3).ToString() + "','" +
                    Reader.GetValue(4).ToString() + "','" +
                    Reader.GetValue(5).ToString() + "')";

                    SQLiteCommand command = new SQLiteCommand(query, LiteCo);
                    try
                    {
                        i = i + command.ExecuteNonQuery();
                    }
                    catch (System.Data.SQLite.SQLiteException e)
                    {
                        Trace.WriteLine(e.ToString());
                        LiteCo.Close();
                    }
                }
                LiteCo.Close();
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC PROJETS FROM COMMERCIAL SUCCESS ############# \n");
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC PROJETS FROM COMMERCIAL FAIL ############# \n");
            }
        }

        /// <summary>
        ///  Méthode de synchronisation des données des projets selon un commercial depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        /// <param name="commercial">le commercial dont les projets vont être chargés</param>
        public void SynCPlansProj(Model.Projet projet)
        {
            MySqlDataReader Reader;
            string query;
            Trace.WriteLine(" ############# TEST SYNC PLANS FROM PROJET ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM plan WHERE refProjet = '"+projet.reference+"'", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;
                LiteCo.Open();
                while (Reader.Read())
                {
                    query = "replace into plan(refPlan, label, dateCreation, dateModification, refProjet, typePlancher, typeCouverture, idCoupe, nomGamme) values('" +
                    Reader.GetValue(0).ToString() + "','" +
                    Reader.GetValue(1).ToString() + "','" +
                    Reader.GetValue(2).ToString() + "','" +
                    Reader.GetValue(3).ToString() + "','" +
                    Reader.GetValue(4).ToString() + "','" +
                    Reader.GetValue(5).ToString() + "','" +
                    Reader.GetValue(6).ToString() + "','" +
                    Reader.GetValue(7).ToString() + "','" +
                    Reader.GetValue(8).ToString() + "')";

                    SQLiteCommand command = new SQLiteCommand(query, LiteCo);
                    try
                    {
                        i = i + command.ExecuteNonQuery();
                    }
                    catch (System.Data.SQLite.SQLiteException e)
                    {
                        Trace.WriteLine(e.ToString());
                        LiteCo.Close();
                    }
                }
                LiteCo.Close();
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC PLANS FROM PROJET SUCCESS ############# \n");
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC PLANS FROM PROJET FAIL ############# \n");
            }
        }

        /// <summary>
        ///   Méthode de appelant les méthodes de synchronisation des données de paramètre des plans
        /// </summary>
        public void SyncParamPlan()
        {
            SyncCoupePrincipe();
            SyncCouverture();
            SyncPlancher();
            SyncGamme();
        }

        /// <summary>
        ///  Méthode de synchronisation des données des clients depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        public void SyncClient()
        {
            MySqlDataReader Reader;
            string query;
            Trace.WriteLine(" ############# TEST SYNC CLIENT ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM client", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;
                LiteCo.Open();
                while (Reader.Read())
                {
                    query = "replace into client(refClient,nom,prenom,adresse,codePostal,ville,email,telephone,dateCreation,dateModification) values(@refClient, @nom, @prenom, @adresse, @codePostal, @ville, @email, @telephone, @dateCreation, @dateModification)";

                    using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
                    {
                        command.Parameters.AddWithValue("@refClient", Reader.GetString(0));
                        command.Parameters.AddWithValue("@nom", Reader.GetString(1));
                        command.Parameters.AddWithValue("@prenom", Reader.GetString(2));
                        command.Parameters.AddWithValue("@adresse", Reader.GetString(3));
                        command.Parameters.AddWithValue("@codePostal", Reader.GetString(4));
                        command.Parameters.AddWithValue("@ville", Reader.GetString(5));
                        command.Parameters.AddWithValue("@email", Reader.GetString(6));
                        command.Parameters.AddWithValue("@telephone", Reader.GetString(7));
                        command.Parameters.AddWithValue("@dateCreation", Reader.GetString(8));
                        command.Parameters.AddWithValue("@dateModification", Reader.GetString(9));

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
                Trace.WriteLine(" ############# SYNC CLIENT SUCCESS ############# \n");
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC CLIENT FAIL ############# \n");
            }
        }

        /// <summary>
        ///  Méthode de synchronisation des données des métamodules depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        public void SyncMetamodules()
        {
            string dateSQLite = "'" + DateTime.Now.ToString() + "'";
            MySqlDataReader Reader;
            string query;
            int sqlitebool = 1;
            LiteCo.Open();
            Trace.WriteLine(" ############# TEST SYNC METAMODULES ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM metamodule", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;
                while (Reader.Read())
                {
                    Byte[] data = (Byte[])Reader.GetValue(4);
                    query = "replace into metamodule(refMetaModule,label,prixHT,nbSlot,dateMaj,nomGamme,statut,image) values(@refMetaModule,@label,@prixHT,@nbSlot,@dateMaj,@nomGamme,@statut,@image)";
                    using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
                    {
                        command.Parameters.AddWithValue("@refMetaModule", Reader.GetString(0));
                        command.Parameters.AddWithValue("@label", Reader.GetString(1));
                        command.Parameters.AddWithValue("@prixHT", Reader.GetInt32(2));
                        command.Parameters.AddWithValue("@nbSlot", Reader.GetInt32(3));                 
                        command.Parameters.AddWithValue("@dateMaj", dateSQLite);
                        command.Parameters.AddWithValue("@nomGamme", Reader.GetString(5));
                        command.Parameters.AddWithValue("@statut", sqlitebool);
                        command.Parameters.AddWithValue("@image", data);
                        try
                        {
                            i = i + command.ExecuteNonQuery();
                        }
                        catch (System.Data.SQLite.SQLiteException e)
                        {
                            Trace.WriteLine(e.ToString());
                            MySQLCo.Close();
                        }
                    }
                }
                MySQLCo.Close();
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC METAMODULES FAIL ############# \n");
            }

            query = "UPDATE metamodule SET statut = 0 WHERE dateMaj !=" + dateSQLite + ";";
            using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
            {
                try
                {
                    command.ExecuteNonQuery();
                    Trace.WriteLine(" ############# SYNC METAMODULES SUCCESS ############# \n");
                }
                catch (SQLiteException e)
                {
                    Trace.WriteLine(e.ToString());
                    Trace.WriteLine(" ############# SYNC METAMODULESFAIL ############# \n");
                }
            }
            LiteCo.Close();
        }

        /// <summary>
        ///  Méthode de synchronisation des données des métaslots depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        public void SyncMetaslot()
        {
            MySqlDataReader Reader;
            string query;
            LiteCo.Open();
            Trace.WriteLine(" ############# TEST SYNC METASLOTS ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM metaslot", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;
                while (Reader.Read())
                {
                    query = "replace into metaslot(idMetaSlot,label,numSlotPosition,refMetaModule) values(@idMetaSlot,@label,@numSlotPosition,@refMetaModule)";
                    using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
                    {
                        command.Parameters.AddWithValue("@idMetaSlot", Reader.GetInt32(0));
                        command.Parameters.AddWithValue("@label", Reader.GetString(1));
                        command.Parameters.AddWithValue("@numSlotPosition", Reader.GetInt32(2));
                        command.Parameters.AddWithValue("@refMetaModule", Reader.GetString(3));
                        try
                        {
                            i = i + command.ExecuteNonQuery();
                        }
                        catch (System.Data.SQLite.SQLiteException e)
                        {
                            Trace.WriteLine(e.ToString());
                            MySQLCo.Close();
                        }
                    }
                }
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC METASLOTS SUCCESS ############# \n");
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC METASLOTS FAIL ############# \n");
            }
            LiteCo.Close();
        }

        /// <summary>
        ///  Méthode de synchronisation des données d'association des métamodules et métaslots depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        public void SyncAssocMetaModuleMetaslot()
        {
            MySqlDataReader Reader;
            string query;
            LiteCo.Open();
            Trace.WriteLine(" ############# TEST SYNC ASSOCIATION METAMODULES/METASLOTS ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM metamodul_has_metaslot", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;
                while (Reader.Read())
                {
                    query = "replace into Composant_has_MetaModule(idComposition,refMetaModule,idMetaSlot) values(@idComposition,@refMetaModule,@idMetaSlot)";
                    using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
                    {
                        command.Parameters.AddWithValue("@idComposition", Reader.GetInt32(0));
                        command.Parameters.AddWithValue("@refMetaModule", Reader.GetString(1));
                        command.Parameters.AddWithValue("@idMetaSlot", Reader.GetInt32(2));
                        try
                        {
                            i = i + command.ExecuteNonQuery();
                        }
                        catch (SQLiteException e)
                        {
                            Trace.WriteLine(e.ToString());
                            MySQLCo.Close();
                        }
                    }
                }
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC ASSOCIATION METAMODULES/METASLOTS SUCCESS ############# \n");
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC ASSOCIATION METAMODULES/METASLOTS FAIL ############# \n");
            }
            LiteCo.Close();
        }

        #endregion

        #region Synchro Export
        /// <summary>
        /// Méthode exportant les données des clients en base SQLite vers la base MySQL
        /// </summary>
        public void ExpClients()
        {
            LiteCo.Open();
            Trace.WriteLine(" ############# TEST EXPORT CLIENTS ############# \n");
            string query = "SELECT * FROM client";
            using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
            {
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string mySQLquery = "INSERT into client (refClient, nom, prenom, adresse, codePostal, ville, email, telephone, dateCreation, dateModification)" +
                                "VALUES(@refClient, @nom,  @prenom, @adresse, @codePostal, @ville, @email, @telephone, @dateCreation, @dateModification)" +
                                "ON DUPLICATE KEY UPDATE nom= @nom, prenom= @prenom, adresse= @adresse, codePostal= @codePostal, ville= @ville, email= @email, telephone= @telephone, dateCreation= @dateCreation, dateModification= @dateModification";
                            using (MySqlCommand expClients = new MySqlCommand(mySQLquery, MySQLCo))
                            {
                                expClients.Parameters.AddWithValue("@refClient", reader.GetString(0));
                                expClients.Parameters.AddWithValue("@nom", reader.GetString(1));
                                expClients.Parameters.AddWithValue("@prenom", reader.GetString(2));
                                expClients.Parameters.AddWithValue("@adresse", reader.GetString(3));
                                expClients.Parameters.AddWithValue("@codePostal", reader.GetString(4));
                                expClients.Parameters.AddWithValue("@ville", reader.GetString(5));
                                expClients.Parameters.AddWithValue("@email", reader.GetString(6));
                                expClients.Parameters.AddWithValue("@telephone", reader.GetString(7));
                                expClients.Parameters.AddWithValue("@dateCreation", reader.GetString(8));
                                expClients.Parameters.AddWithValue("@dateModification", reader.GetString(9));
                                try
                                {
                                    expClients.ExecuteNonQuery();
                                }
                                catch (MySqlException e)
                                {
                                    Trace.WriteLine(" \n ################################################# EXPORT CLIENTS FAIL ################################################# \n" + e.ToString() + "\n");
                                }
                            }
                        }
                    }
                    Trace.WriteLine("#### EXPORT CLIENTS SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# EXPORT CLIENTS FAIL ################################################# \n" + ex.ToString() + "\n");
                }
                MySQLCo.Close();
            }
            LiteCo.Close();
        }

        /// <summary>
        /// Méthode exportant les données des projets en base SQLite vers la base MySQL
        /// </summary>
        public void ExpProjets()
        {
            LiteCo.Open();
            Trace.WriteLine(" ############# TEST EXPORT PROJETS ############# \n");
            string query = "SELECT * FROM projet";
            using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
            {
                MySQLCo.Open();
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string mySQLquery = "INSERT into projet(refProjet, nom,  dateCreation, dateModification, refClient, refCommercial)" +
                                "VALUES(@refProjet, @nom,  @dateCreation, @dateModification, @refClient, @refCommercial)" +
                                "ON DUPLICATE KEY UPDATE nom= @nom,dateCreation= @dateCreation,dateModification= @dateModification,refClient= @refClient,refCommercial= @refCommercial";
                            using (MySqlCommand expProjets = new MySqlCommand(mySQLquery, MySQLCo))
                            {
                                expProjets.Parameters.AddWithValue("@refProjet", reader.GetString(0));
                                expProjets.Parameters.AddWithValue("@nom", reader.GetString(1));
                                expProjets.Parameters.AddWithValue("@dateCreation", reader.GetString(2));
                                expProjets.Parameters.AddWithValue("@dateModification", reader.GetString(3));
                                expProjets.Parameters.AddWithValue("@refClient", reader.GetString(4));
                                expProjets.Parameters.AddWithValue("@refCommercial", reader.GetString(5));
                                try
                                {
                                    expProjets.ExecuteNonQuery();
                                }
                                catch (MySqlException e)
                                {
                                    Trace.WriteLine(" \n ################################################# EXPORT PROJETS FAIL ################################################# \n" + e.ToString() + "\n");
                                }
                            }
                        }
                    }
                    Trace.WriteLine("#### EXPORT PROJETS SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# EXPORT PROJETS FAIL ################################################# \n" + ex.ToString() + "\n");
                }
                MySQLCo.Close();
            }
            LiteCo.Close();
        }

        /// <summary>
        /// Méthode exportant les données des plans en base SQLite vers la base MySQL
        /// </summary>
        public void ExpPlans()
        {
            LiteCo.Open();
            Trace.WriteLine(" ############# TEST EXPORT PLANS ############# \n");
            string query = "SELECT refPlan, label, dateCreation, dateModification, refProjet, typePlancher, typeCouverture, idCoupe, nomGamme FROM plan";
            using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
            {
                MySQLCo.Open();
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string mySQLquery = "INSERT into plan(refPlan, label,  dateCreation, dateModification, refProjet, typePlancher, typeCouverture, idCoupe, nomGamme)" +
                                "VALUES(@refPlan, @label,  @dateCreation, @dateModification, @refProjet, @typePlancher, @typeCouverture, @idCoupe, @nomGamme)" +
                                "ON DUPLICATE KEY UPDATE label= @label, dateCreation= @dateCreation,dateModification= @dateModification,refProjet= @refProjet,typePlancher= @typePlancher,typeCouverture= @typeCouverture,idCoupe= @idCoupe,nomGamme= @nomGamme";
                            using (MySqlCommand expPlans = new MySqlCommand(mySQLquery, MySQLCo))
                            {
                                expPlans.Parameters.AddWithValue("@refPlan", reader.GetString(0));
                                expPlans.Parameters.AddWithValue("@label", reader.GetString(1));
                                expPlans.Parameters.AddWithValue("@dateCreation", reader.GetString(2));
                                expPlans.Parameters.AddWithValue("@dateModification", reader.GetString(3));
                                expPlans.Parameters.AddWithValue("@refProjet", reader.GetString(4));
                                expPlans.Parameters.AddWithValue("@typePlancher", reader.GetString(5));
                                expPlans.Parameters.AddWithValue("@typeCouverture", reader.GetString(6));
                                expPlans.Parameters.AddWithValue("@idCoupe", reader.GetInt32(7));
                                expPlans.Parameters.AddWithValue("@nomGamme", reader.GetString(8));
                                try
                                {
                                    expPlans.ExecuteNonQuery();
                                }
                                catch (MySqlException e)
                                {
                                    Trace.WriteLine(" \n ################################################# EXPORT PLANS FAIL ################################################# \n" + e.ToString() + "\n");
                                }
                            }
                        }
                    }
                    Trace.WriteLine("#### EXPORT PLANS SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# EXPORT PLANS FAIL ################################################# \n" + ex.ToString() + "\n");
                }
                MySQLCo.Close();
            }
            LiteCo.Close();
        }

        /// <summary>
        /// Méthode exportant les données des plans en base SQLite vers la base MySQL
        /// </summary>
        public void ExpModules()
        {
            LiteCo.Open();
            Trace.WriteLine(" ############# TEST EXPORT MODULES ############# \n");
            string query = "SELECT * FROM module";
            using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
            {
                MySQLCo.Open();
                try
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string mySQLquery = "INSERT into module(idModule, coordonneeDebutX, coordonneeDebutY, colspan, rowspan, refMetaModule, refPlan)" +
                                "VALUES(@idModule, @coordonneeDebutX,  @coordonneeDebutY, @colspan, @rowspan, @refMetaModule, @refPlan)" +
                                "ON DUPLICATE KEY UPDATE colspan= @colspan,rowspan= @rowspan,refMetaModule= @refMetaModule";
                            using (MySqlCommand expModules = new MySqlCommand(mySQLquery, MySQLCo))
                            {
                                //Trace.WriteLine(mySQLquery);
                                expModules.Parameters.AddWithValue("@idModule", reader.GetInt32(0));
                                expModules.Parameters.AddWithValue("@coordonneeDebutX", reader.GetInt32(1));
                                expModules.Parameters.AddWithValue("@coordonneeDebutY", reader.GetInt32(2));
                                expModules.Parameters.AddWithValue("@colspan", reader.GetInt32(3));
                                expModules.Parameters.AddWithValue("@rowspan", reader.GetInt32(4));
                                expModules.Parameters.AddWithValue("@refMetaModule", reader.GetString(5));
                                expModules.Parameters.AddWithValue("@refPlan", reader.GetString(6));
                                try
                                {
                                    expModules.ExecuteNonQuery();
                                }
                                catch (MySqlException e)
                                {
                                    Trace.WriteLine(e.ToString());
                                }
                            }
                        }
                    }
                    Trace.WriteLine("#### EXPORT MODULES SUCCESS ####");
                }
                catch (SQLiteException ex)
                {
                    Trace.WriteLine(" \n ################################################# EXPORT MODULES FAIL ################################################# \n" + ex.ToString() + "\n");
                }
                MySQLCo.Close();
            }
            LiteCo.Close();
        }
        #endregion

        #region Test
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
                catch (System.Data.SQLite.SQLiteException e)
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
                    Trace.WriteLine(" ############# DATA ############# ");
                    while (reader.Read())
                    {
                        
                        for (int i = 0; i < reader.VisibleFieldCount; i++)
                        {
                            if (reader.GetValue(i).GetType() == typeof (DateTime))
                            {
                                Trace.Write("["+reader.GetDateTime(i).ToString()+"]");
                            }
                            else
                                Trace.Write("["+reader.GetValue(i).ToString()+"]");
                            Trace.WriteLine(" ############# " + reader.GetValue(i).GetType().ToString() + " ############# \n");
                        }
                        Trace.WriteLine(""); 
                    }
                    Trace.WriteLine(" ############# END DATA ############# ");
                }
                finally
                {
                    reader.Close();
                    LiteCo.Close();
                }
            }
            catch (SQLiteException ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }
        #endregion

        #endregion

        #region Privates Methods
        /// <summary>
        ///   Méthode de synchronisation des données des coupes de principe depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        private void SyncCoupePrincipe()
        {
            string dateSQLite = "'" + DateTime.Now.ToString() + "'";
            MySqlDataReader Reader;
            string query;
            int sqlitebool = 1;
            LiteCo.Open();
            Trace.WriteLine(" ############# TEST SYNC COUPE PRINCIPE ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM coupeprincipe", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;
                while (Reader.Read())
                {
                    Byte[] data = (Byte[])Reader.GetValue(5);
                    query = "replace into coupeprincipe(idCoupe, label, longueur, largeur, prixHT, image, statut, dateMaj) values(@idCoupe, @label, @longueur, @largeur, @prixHT, @image, @statut, @dateMaj)";

                    using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
                    {
                        command.Parameters.AddWithValue("@idCoupe", Reader.GetInt32(0));
                        command.Parameters.AddWithValue("@label", Reader.GetString(1));
                        command.Parameters.AddWithValue("@longueur", Reader.GetInt32(2));
                        command.Parameters.AddWithValue("@largeur", Reader.GetInt32(3));
                        command.Parameters.AddWithValue("@prixHT", Reader.GetInt32(4));
                        command.Parameters.AddWithValue("@image", data);
                        command.Parameters.AddWithValue("@statut", sqlitebool);
                        command.Parameters.AddWithValue("@dateMaj", dateSQLite);
                        try
                        {
                            i = i + command.ExecuteNonQuery();

                        }
                        catch (SQLiteException e)
                        {
                            Trace.WriteLine(e.ToString());
                            Trace.WriteLine(" ############# SYNC COUPE PRINCIPE FAIL ############# \n");
                        }
                    }
                }
                MySQLCo.Close();
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC COUPE PRINCIPE FAIL ############# \n");
            }

            query = "UPDATE coupeprincipe SET statut = 0 WHERE dateMaj !=" + dateSQLite + ";";
            using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
            {
                try
                {
                    command.ExecuteNonQuery();
                    Trace.WriteLine(" ############# SYNC COUPE PRINCIPE SUCCESS ############# \n");
                }
                catch (SQLiteException e)
                {
                    Trace.WriteLine(e.ToString());
                    Trace.WriteLine(" ############# SYNC COUPE PRINCIPE FAIL ############# \n");
                }
            }
            LiteCo.Close();
        }

        /// <summary>
        ///   Méthode de synchronisation des données des couvertures depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        private void SyncCouverture()
        {
            string dateSQLite = "'" + DateTime.Now.ToString() + "'";
            MySqlDataReader Reader;
            string query;
            int sqlitebool = 1;
            LiteCo.Open();
            Trace.WriteLine(" ############# TEST SYNC COUVERTURE ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM couverture", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;
                while (Reader.Read())
                {
                    Byte[] data = (Byte[])Reader.GetValue(2);
                    query = "replace into couverture(typeCouverture, prixHT, image, statut, dateMaj) values(@typeCouverture, @prixHT, @image, @statut, @dateMaj)";

                    using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
                    {
                        command.Parameters.AddWithValue("@typeCouverture", Reader.GetString(0));
                        command.Parameters.AddWithValue("@prixHT", Reader.GetUInt32(1));
                        command.Parameters.AddWithValue("@image", data);
                        command.Parameters.AddWithValue("@statut", sqlitebool);
                        command.Parameters.AddWithValue("@dateMaj", dateSQLite);
                        try
                        {
                            i = i + command.ExecuteNonQuery();
                        }
                        catch (System.Data.SQLite.SQLiteException e)
                        {
                            Trace.WriteLine(e.ToString());
                            MySQLCo.Close();
                            Trace.WriteLine(" ############# SYNC COUVERTURE FAIL ############# \n");
                        }
                    }
                }
                MySQLCo.Close();
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC COUVERTURE FAIL ############# \n");
            }

            query = "UPDATE couverture SET statut = 0 WHERE dateMaj != " + dateSQLite + ";";
            using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
            {
                try
                {
                    command.ExecuteNonQuery();
                    Trace.WriteLine(" ############# SYNC COUVERTURE SUCCESS ############# \n");
                }
                catch (SQLiteException e)
                {
                    Trace.WriteLine(e.ToString());
                    Trace.WriteLine(" ############# SYNC COUVERTURE FAIL ############# \n");
                }
            }
            LiteCo.Close();
        }

        /// <summary>
        ///   Méthode de synchronisation des données des planchers depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        private void SyncPlancher()
        {
            string dateSQLite = "'" + DateTime.Now.ToString() + "'"; ;
            MySqlDataReader Reader;
            string query;
            int sqlitebool = 1;
            LiteCo.Open();
            Trace.WriteLine(" ############# TEST SYNC PLANCHER ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM plancher", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;

                while (Reader.Read())
                {
                    Byte[] data = (Byte[])Reader.GetValue(2);
                    query = "replace into plancher(typePlancher, prixHT, image, statut, dateMaj) values(@typePlancher, @prixHT, @image, @statut, @dateMaj)";

                    using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
                    {
                        command.Parameters.AddWithValue("@typePlancher", Reader.GetString(0));
                        command.Parameters.AddWithValue("@prixHT", Reader.GetUInt32(1));
                        command.Parameters.AddWithValue("@image", data);
                        command.Parameters.AddWithValue("@statut", sqlitebool);
                        command.Parameters.AddWithValue("@dateMaj", dateSQLite);
                        try
                        {
                            i = i + command.ExecuteNonQuery();
                        }
                        catch (System.Data.SQLite.SQLiteException e)
                        {
                            Trace.WriteLine(e.ToString());
                            MySQLCo.Close();
                        }
                    }
                }
                MySQLCo.Close();
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC PLANCHER FAIL ############# \n");
            }
            query = "UPDATE plancher SET statut = 0 WHERE dateMaj <>" + dateSQLite + ";";
            using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
            {
                try
                {
                    command.ExecuteNonQuery();
                    Trace.WriteLine(" ############# SYNC PLANCHER SUCCESS ############# \n");
                }
                catch (SQLiteException e)
                {
                    Trace.WriteLine(e.ToString());
                    Trace.WriteLine(" ############# SYNC PLANCHER FAIL ############# \n");
                }
            }
            LiteCo.Close();
        }

        /// <summary>
        ///   Méthode de synchronisation des données des gammes depuis la base distante MYSQL vers la base locale SQLite
        /// </summary>
        private void SyncGamme()
        {
            string dateSQLite = "'"+DateTime.Now.ToString()+"'";
            MySqlDataReader Reader;
            string query;
            int sqlitebool = 1;
            LiteCo.Open();
            Trace.WriteLine(" ############# TEST SYNC GAMME ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM gamme", MySQLCo);
            try
            {
                MySQLCo.Open();
                Reader = selectComms.ExecuteReader();
                int i = 0;

                while (Reader.Read())
                {
                    Byte[] data = (Byte[])Reader.GetValue(5);
                    query = "replace into gamme(nomGamme, offrePromo,typeIsolant,typeFinition,qualiteHuisserie, image, statut, dateMaj) values(@nom, @offrePromo, @typeIsolant, @typeFinition, @qualiteHuisserie, @image, @statut, @dateMaj)";

                    using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
                    {
                        command.Parameters.AddWithValue("@nom", Reader.GetString(0));
                        command.Parameters.AddWithValue("@offrePromo", Reader.GetUInt32(1));
                        command.Parameters.AddWithValue("@typeIsolant", Reader.GetString(2));
                        command.Parameters.AddWithValue("@typeFinition", Reader.GetString(3));
                        command.Parameters.AddWithValue("@qualiteHuisserie", Reader.GetString(4));
                        command.Parameters.AddWithValue("@image", data);
                        command.Parameters.AddWithValue("@statut", sqlitebool);
                        command.Parameters.AddWithValue("@dateMaj", dateSQLite);
                        try
                        {
                            i = i + command.ExecuteNonQuery();
                            Trace.WriteLine("Date générée : " + dateSQLite);
                        }
                        catch (System.Data.SQLite.SQLiteException e)
                        {
                            Trace.WriteLine(e.ToString());
                            MySQLCo.Close();
                        }
                    }
                }
                MySQLCo.Close();
            }
            catch (MySqlException e)
            {
                Trace.WriteLine(e.ToString());
                MySQLCo.Close();
                Trace.WriteLine(" ############# SYNC GAMME FAIL ############# \n");
            }
            query = "UPDATE gamme SET statut = 0 WHERE dateMaj <>" + dateSQLite + ";";
            using (SQLiteCommand command = new SQLiteCommand(query, LiteCo))
            {
                try
                {
                    command.ExecuteNonQuery();
                    Trace.WriteLine(" ############# SYNC GAMME SUCCESS ############# \n");
                    Trace.WriteLine(query);
                }
                catch (SQLiteException e)
                {
                    Trace.WriteLine(e.ToString());
                    Trace.WriteLine(" ############# SYNC GAMME FAIL ############# \n");
                }
            }
            LiteCo.Close();
        }

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
                MySQLCo.Close();
                Trace.WriteLine(" \n ################################################# MYSQL DATABASE REACHED,  BEGIN SYNCHRONISATION ... ################################################# \n");
                MySQLCo.Close();
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

        #region Tools
        #endregion
    }
}