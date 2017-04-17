using System;
using System.Data.SQLite;
using System.IO;

namespace App1
{
    /// <summary>
    /// Class to manage database transactions.
    /// </summary>
    public class DBHelper
    {
        private string _connString;
        private object _mutex = new object();
        private string _path;

        /// <summary>
        /// Constructor of the class.
        /// </summary>
        /// <param name="path">Path where to SQLite database file is located.</param>
        public DBHelper(string path)
        {
            _path = path;
            _connString = "Data Source=" + path + ";Version=3;";
        }

        /// <summary>
        /// Method that creates the database for the application.
        /// </summary>
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

        /// <summary>
        /// Method that creates shortened link table.
        /// </summary>
        /// <param name="dbConn">Connection object to connect to database.</param>
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

        /// <summary>
        /// Method that creates users table.
        /// </summary>
        /// <param name="dbConn">Connection object to connect to database.</param>
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

        /// <summary>
        /// Method that inserts a user to the database.
        /// </summary>
        /// <param name="username">Username of the user.</param>
        /// <param name="password">Password of the user.</param>
        /// <param name="secret">Secret of the user.</param>
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

        /// <summary>
        /// Method that finds a user in the database.
        /// </summary>
        /// <param name="username">Username of the user.</param>
        /// <returns>User object.</returns>
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

        /// <summary>
        /// Method that finds a user in the database.
        /// </summary>
        /// <param name="secret">Secret of the user.</param>
        /// <returns>User object.</returns>
        public dynamic GetUserBySecret(string secret)
        {
            dynamic user = null;
            lock (_mutex)
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connString))
                {
                    conn.Open();
                    string sql = "select * from users where secret='" + secret + "'";
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

        /// <summary>
        /// Method that inserts a link to the database.
        /// </summary>
        /// <param name="shortlink">Shortened link.</param>
        /// <param name="link">Original link</param>
        /// <param name="username">Username of the user that made the request.</param>
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

        /// <summary>
        /// Method that gets the 
        /// </summary>
        /// <param name="shortlink"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Method that deletes a shortened URL.
        /// </summary>
        /// <param name="shortlink">Shortened URL</param>
        /// <param name="username">Username of the user which made the delete request</param>
        public void DeleteLink(string shortlink, string username)
        {
            lock (_mutex)
            {
                using (SQLiteConnection conn = new SQLiteConnection(_connString))
                {
                    conn.Open();

                    string sql = "delete from links where shortlink='" + shortlink + "' and username='" + username + "'";
                    SQLiteCommand command = new SQLiteCommand(sql, conn);
                    command.ExecuteNonQuery();
                    Console.WriteLine("   + App1 | Done deleting link.");
                }
            }
        }

    }
}
