using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Odbc;
using System.Data.SQLite;

namespace Madera_MMB.Lib
{
    class Connexion
    {
        Connexion() {}

        private void SQLiteLocalConnexion()
        {
            SQLiteConnection liteConn = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
            liteConn.Open();

           // Pour exécuter une requête sur une base SQLite //
           // SQLiteCommand command = new SQLiteCommand("tapper requête ici", liteConn);
           // command.ExecuteNonQuery();
        }

        private void SQLRemoteConnexion()
        {
            var conn = new OdbcConnection();
            conn.ConnectionString =
            "Driver={MySql};" +
            "Server=db.domain.com;" +
            "Option=131072;" +
            "Port=3306;" +
            "Stmt=;" +
            "DataBase=DataBaseName;" +
            "Uid=UserName;" +
            "Pwd=Secret"; 
            conn.Open();
        }


    }
}
