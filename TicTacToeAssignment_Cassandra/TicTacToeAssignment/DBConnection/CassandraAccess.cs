 
using System;
using Cassandra;
using System.Linq;

namespace TicTacToeAssignment.DBConnection
{
    public class CassandraAccess
    {
        public static void SelectUsername()
        {
            var cluster = Cluster.Builder()
                                   .AddContactPoints("127.0.0.1")
                                   .Build();

            // Connect to the nodes using a keyspace
            var session = cluster.Connect("tictactoe");
            var rs = session.Execute("SELECT * FROM userdetails");
        }

        public static void InsertIntoUserTable(string FName, string LName, string UserName)
        {
            var cluster = Cluster.Builder()
                                   .AddContactPoints("127.0.0.1")
                                   .Build();

            // Connect to the nodes using a keyspace
            var session = cluster.Connect("tictactoe");
            Random random = new Random();
            int TokenID = random.Next(0, 1000);
            bool userAlreadyExists = CheckIfUserAlreadyExists(UserName);

            if (userAlreadyExists)
                throw new Exception("UserName already exists! Try with new UserName!!!");

            string query = "Insert into  UserDetails  (FName, LName, UserName, TokenID) values ('" + FName + "', '" + LName + "', '" + UserName + "', " + TokenID + ")";
            var rs = session.Execute(query);
        }

        public static bool CheckIfUserAlreadyExists(string userId)
        {
            var cluster = Cluster.Builder()
                                    .AddContactPoints("127.0.0.1")
                                    .Build();
            var session = cluster.Connect("tictactoe");
            var rs = session.Execute("SELECT * FROM userdetails where username = '" + userId + "'");
            if (rs.Count() > 0)
                return true;
            else
                return false;
        }

        public static bool AuthenticateUser(int tokenId)
        {
            var cluster = Cluster.Builder()
                                    .AddContactPoints("127.0.0.1")
                                    .Build();
            var session = cluster.Connect("tictactoe");
            string query = "SELECT * FROM UserDetails where TokenID = " + tokenId + " ALLOW FILTERING";
            var rs = session.Execute(query);
            if (rs.Count() > 0)
                return true;
            else
                return false;
        }

        public static void LogEntryForRequest(Log log)
        {
            var cluster = Cluster.Builder()
                                      .AddContactPoints("127.0.0.1")
                                      .Build();
            var session = cluster.Connect("tictactoe");
            string query = "Insert into  LogDetails  (LogId, Request, Response, Exception, Comments) values (now(),'" + log.Request + "', '" + log.Response + "', '" + log.Exception + "', '" + log.Comment + "')";
            var rs = session.Execute(query);
        }

    }
}
