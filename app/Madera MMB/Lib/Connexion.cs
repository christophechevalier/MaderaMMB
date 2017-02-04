using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SQLite;
using System.IO;

namespace Madera_MMB.Lib
{
    public class Connexion
    {
        #region Properties
        public SQLiteConnection LiteCo { get; set; }
        public MySqlConnection MySQLCo { get; set; }
        public bool MySQLconnected { get; set; }
        public SQLiteDataAdapter DataAdapter { get; set; }
        #endregion

        #region Ctor
        public Connexion()
        {
            // Test Connexion MySQL //
            MySQLconnected = OpenMySQLConnection();

            // Partie SQLite //

            if (File.Exists("Madera.bdd"))
            {
                Console.Write("\n<--------------------------------------- Fichier SQLite déjà créé  --------------------------------------->\n");
                Console.Write("\n<--------------------------------------- Suppression et re-génération  --------------------------------------->\n");
                File.Delete("Madera.bdd");
                CreateSQLiteBase();
            }
            else
            {
                if (CreateSQLiteBase())
                {
                    Console.Write("\n<---------------------------------------  Fichier SQLite créé  --------------------------------------->\n");
                }
                else
                {

                }

            }

            // Pour exécuter une requête sur une base SQLite //
            // SQLiteCommand command = new SQLiteCommand("tapper requête ici", liteConn);
            // command.ExecuteNonQuery();

        }
        #endregion


        #region Public Methods
        public bool SyncCommMySQL()
        {
            MySqlDataReader Reader;
            string query;
            Console.Write(" ############# TEST SYNC COMMERCIAL ############# \n");
            MySqlCommand selectComms = new MySqlCommand("SELECT * FROM Commercial", MySQLCo);
            try
            {
                Reader = selectComms.ExecuteReader();
                int i = 0;
                LiteCo.Open();
                while (Reader.Read())
                {
                    for (int x = 0; x < Reader.VisibleFieldCount; x++)
                    {
                        Console.Write(" ############# " + Reader.GetValue(x).ToString() + " ############# \n");
                    }

                    query = "replace into commercial(refCommercial, nom, prenom, motDePasse) values('" +
                    Reader.GetValue(0).ToString() + "','" +
                    Reader.GetValue(1).ToString() + "','" +
                    Reader.GetValue(2).ToString() + "','" +
                    Reader.GetValue(3).ToString() + "')";

                    SQLiteCommand command = new SQLiteCommand(query, LiteCo);
                    Console.WriteLine("################" + query + "################");
                    try
                    {
                        i = i + command.ExecuteNonQuery();  
                    }
                    catch (System.Data.SQLite.SQLiteException e)
                    {
                        Console.Write(e.ToString());
                        LiteCo.Close();
                        return false;
                    } 
                }
                LiteCo.Close();
                return true;
            }
            catch(MySqlException e)
            {
                Console.Write(e.ToString());
                return false;
            }
          
        }
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
                    Console.Write(e.ToString());
                    LiteCo.Close(); 
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.ToString());
                LiteCo.Close();
            }
            LiteCo.Close();
        }

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
                        for (int i = 0; i < reader.VisibleFieldCount; i++)
                        {
                            Console.Write(" ############# " + reader.GetValue(i).ToString() + " ############# \n");
                        }
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            catch (SQLiteException ex)
            {
                Console.Write(ex.ToString());
            }
        }
        #endregion

        #region Privates Methods
        private bool CreateSQLiteBase()
        {
            if (File.Exists("SQLiteScript.sql"))
            {
                string strCommand = File.ReadAllText("SQLiteScript.sql");
                //string cmd = " CREATE TABLE CLIENT (`refClient` varchar(20) NOT NULL UNIQUE,  `nom` varchar(45) NOT NULL,  `prenom` varchar(45) NOT NULL,  PRIMARY KEY (`refClient`))";


                SQLiteConnection.CreateFile("Madera.bdd");
                // Instanciation d'une nouvelle connexion à la base de données //
                LiteCo = new SQLiteConnection("Data Source=Madera.bdd;Version=3;");
                LiteCo.Open();

                SQLiteCommand command = new SQLiteCommand(strCommand, LiteCo);

                try
                {
                    command.ExecuteNonQuery();
                    LiteCo.Close();
                    Console.Write(" \n ################################################# CREATION BASE SQLITE SUCCESS ################################################# \n");
                    return true;
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    Console.Write(" \n ################################################# ERREUR CREATION BASE SQLITE ################################################# \n" + ex.ToString() + "\n");
                    LiteCo.Close();
                    return false;
                }
            }
            else
            {
                LiteCo.Close();
                return false;
            }

        }
        // Partie MySQL //
        private bool OpenMySQLConnection()
        {
            string connectionString = "SERVER=localhost;DATABASE=madera_mmb;UID=root;PASSWORD=;";
            MySQLCo = new MySqlConnection(connectionString);
            try
            {
                MySQLCo.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based 
                //on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        Console.Write("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.Write("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }
        #endregion
    }
}