using System;
using Xamarin.Forms;
using System.IO;
using RestDbExample.Interfaces;
using RestDbExample.Droid;

[assembly: Dependency(typeof(SQLite_Android))]

namespace RestDbExample.Droid
{
    public class SQLite_Android : ISQLite
    {
        public SQLite_Android()
        {
        }

        #region ISQLite implementation
        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "SQLite.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);
            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }
        #endregion
    }
}
