using System;
using System.Data.SQLite;
using System.IO;

namespace App1
{
    public class DBHelper
    {
        private string _connString;
        private object _mutex = new object();
        private string _path;

        public DBHelper(string path)
        {
            _path = path;
            _connString = "Data Source=" + path + ";Version=3;";
        }

        public void CreateDatabase()
        {
            if (!File.Exists(_path))
            {
                Console.WriteLine("   + App1 | Database does not exist. Creating new database...");
                SQLiteConnection.CreateFile(_path);
            }

            using (SQLiteConnection conn = new SQLiteConnection(_connString))
            {
                conn.Open();

                CreateUsersTable(conn);
                CreateLinksTable(conn);
            }
        }

        private void CreateLinksTable(SQLiteConnection dbConn)
        {
            var tableExistsQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='links';";
            using (SQLiteCommand cmd = new SQLiteCommand(tableExistsQuery, dbConn))
            {
                string result = (string)cmd.ExecuteScalar();
                // If table doesn't exist then create it
                if (string.IsNullOrEmpty(result))
                {
                    Console.WriteLine("   + App1 | Links Table does not exist. Creating new table...");
                    string createTableQuery = "CREATE TABLE links (id INTEGER PRIMARY KEY AUTOINCREMENT, shortlink TEXT, link TEXT, username TEXT)";
                    using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, dbConn))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        private void CreateUsersTable(SQLiteConnection dbConn)
        {
            var tableExistsQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='users';";
            using (SQLiteCommand cmd = new SQLiteCommand(tableExistsQuery, dbConn))
            {
                string result = (string)cmd.ExecuteScalar();
                // If table doesn't exist then create it
                if (string.IsNullOrEmpty(result))
                {
                    Console.WriteLine("   + App1 | Users Table does not exist. Creating new table...");
                    string createTableQuery = "CREATE TABLE users (id INTEGER PRIMARY KEY AUTOINCREMENT, username TEXT, password TEXT, secret TEXT)";
                    using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableQuery, dbConn))
                    {
                        createTableCommand.ExecuteNonQuery();
                    }
                }
            }
        }

        public void CreateUser(string username, string password, string secret)
        {
            lock (_mutex)
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connString))
                {
                    conn.Open();

                    string sql = "insert into users (username, password, secret) values ('" + username + "', '" + password + "', '" + secret + "')";
                    SQLiteCommand command = new SQLiteCommand(sql, conn);
                    command.ExecuteNonQuery();
                    Console.WriteLine("   + App1 | Done creating new user.");
                }
            }
        }

        public void CreateLink(string shortlink, string link, string username)
        {
            lock (_mutex)
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connString))
                {
                    conn.Open();

                    string sql = "insert into links (shortlink, link, username) values ('" + shortlink + "', '" + link + "', '" + username + "')";
                    SQLiteCommand command = new SQLiteCommand(sql, conn);
                    command.ExecuteNonQuery();
                    Console.WriteLine("   + App1 | Done creating shortened link.");
                }
            }
        }

        public string GetLink(string shortlink)
        {
            string link = null;
            lock (_mutex)
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connString))
                {
                    conn.Open();
                    string sql = "select * from links where shortlink='" + shortlink + "'";
                    using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                link = (string)reader["link"];
                                Console.WriteLine("   + App1 | Link: {0}", link);
                            }
                        }
                    }
                }
            }
            return link;
        }

        public dynamic GetUser(string username)
        {
            dynamic user = null;
            lock (_mutex)
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connString))
                {
                    conn.Open();
                    string sql = "select * from users where username='" + username + "'";
                    using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                user = new
                                {
                                    username = (string)reader["username"],
                                    password = (string)reader["password"],
                                    secret = (string)reader["secret"]
                                };
                                Console.WriteLine("   + App1 | User: {0}", user);
                            }
                        }
                    }
                }
            }
            return user;
        }

    }
}
