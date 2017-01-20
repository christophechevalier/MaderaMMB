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
                Console.Write("<--------------------------------------- Fichier SQLite déjà créé  --------------------------------------->");
                Console.Write("<--------------------------------------- Suppression et re-génération  --------------------------------------->");
                File.Delete("Madera.bdd");
                CreateSQLiteBase();
            }
            else
            {
                if (CreateSQLiteBase())
                {
                    Console.Write("<---------------------------------------  Fichier SQLite créé  --------------------------------------->");
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
        public bool Synchronisation()
        {
            if (MySQLconnected == true)
            {
                MySqlDataReader Reader;
                string query;
                MySqlCommand selectMetaModules = new MySqlCommand("SELECT * FROM metamodule", MySQLCo);
                Reader = selectMetaModules.ExecuteReader();
                int i = 0;
                while (Reader.Read())
                {
                    query = "insert into metamodule(refMetaModule, label, prixHT, nbSLot, image, nomGamme) values(" +
                    Reader.GetValue(0).ToString() + "," +
                    Reader.GetValue(1).ToString() + "," +
                    Reader.GetValue(2).ToString() + "," +
                    Reader.GetValue(3).ToString() + "," +
                    Reader.GetValue(4).ToString() + "," +
                    Reader.GetValue(5).ToString() + ")";

                    SQLiteCommand command = new SQLiteCommand(query, LiteCo);
                    try
                    {
                        i = i + command.ExecuteNonQuery();
                    }
                    catch (System.Data.SQLite.SQLiteException e)
                    {
                        Console.Write(e.ToString());
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public void InsertSQliteQuery(string query)
        {
            try
            {
                SQLiteCommand command = new SQLiteCommand(query,LiteCo);
                command.ExecuteNonQuery();
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
            if(File.Exists("SQLiteScript.sql"))
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
                    return true;
                }
                catch(System.Data.SQLite.SQLiteException ex)
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
