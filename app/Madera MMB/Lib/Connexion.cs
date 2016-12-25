using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.SQLite;

namespace Madera_MMB.Lib
{
    public class Connexion
    {
        public SQLiteConnection LiteCo { get; set; }
        public MySqlConnection SQLCo { get; set; }
        public Connexion()
        {
            // Partie SQLite //
            SQLiteConnection LiteCo = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            LiteCo.Open();
            // Pour exécuter une requête sur une base SQLite //
            // SQLiteCommand command = new SQLiteCommand("tapper requête ici", liteConn);
            // command.ExecuteNonQuery();

            // Partie MySQL //
            string connectionString = "SERVER=localhost;DATABASE=madera_mmb;UID=root;PASSWORD=;";
            SQLCo = new MySqlConnection(connectionString);
        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                SQLCo.Open();
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
    }
}
