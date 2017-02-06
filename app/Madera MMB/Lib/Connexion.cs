﻿using System;
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
        public bool SQLiteconnected { get; set; }
        public SQLiteDataAdapter DataAdapter { get; set; }
        #endregion

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
        public bool SyncCommMySQL()
        {
            MySqlDataReader Reader;
            string query;
            MySQLCo.Open();
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
                MySQLCo.Close();
                return true;
            }
            catch(MySqlException e)
            {
                Console.Write(e.ToString());
                MySQLCo.Close();
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
                        //for (int i = 0; i < reader.VisibleFieldCount; i++)
                        //{
                        //    Console.Write(" ############# " + reader.GetValue(i).ToString() + " ############# \n");
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
                Console.Write(ex.ToString());
            }
        }
 
        #endregion


        #region Privates Methods
        // Partie SQLite //
        private bool CreateSQLiteBase()
        {
            if (File.Exists("Madera.bdd"))
            {
                Console.Write(" \n ################################################# SQLITE DATABASE EXISTS ################################################# \n");
                try
                {
                    LiteCo = new SQLiteConnection("Data Source=Madera.bdd;Version=3;");
                    Console.Write(" \n ################################################# SQLITE DATABASE CONNECTED ################################################# \n");
                    return true;
                }
                catch (System.Data.SQLite.SQLiteException ex)
                {
                    Console.Write(" \n ################################################# ERROR CONNECTION BASE SQLITE ################################################# \n" + ex.ToString() + "\n");
                    LiteCo.Close();
                    return false;
                }
            }
            else
            {
                Console.Write(" \n ################################################# SQLITE DATABASE NOT EXISTS ################################################# \n");
                string strCommand = File.ReadAllText("SQLiteScript.sql");
                LiteCo = new SQLiteConnection("Data Source=Madera.bdd;Version=3;");
                SQLiteCommand command = new SQLiteCommand(strCommand, LiteCo);
                LiteCo.Open();
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
        }

        // Partie MySQL //
        private bool OpenMySQLConnection()
        {
            string connectionString = "SERVER=localhost;DATABASE=madera_mmb;UID=root;PASSWORD=;";
            try
            {
                MySQLCo = new MySqlConnection(connectionString);
                Console.Write(" \n ################################################# MYSQL DATABASE REACHED,  BEGIN SYNCHRONISATION ... ################################################# \n");
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("################################################# ERROR CONNECTION MYSQL SERVER #################################################");
                Console.WriteLine(ex.ToString());
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