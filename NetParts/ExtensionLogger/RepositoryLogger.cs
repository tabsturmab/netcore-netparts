using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using NetParts.Models;

namespace NetParts.ExtensionLogger
{
    public class RepositoryLogger
    {
        private string ConnectionString { get; set; }
        public RepositoryLogger(string connection)
        {
            ConnectionString = connection;
        }
        private bool ExecuteNonQuery(string commandStr, List<SqlParameter> paramList)
        {
            var result = false;
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                using (var command = new SqlCommand(commandStr, conn))
                {
                    command.Parameters.AddRange(paramList.ToArray());
                    var count = command.ExecuteNonQuery();
                    result = count > 0;
                }
            }
            return result;
        }
        public bool InsertLog(LogEvent log)
        {
            var command = $@"INSERT INTO EventLog (EventID,LogLevel,Message,CreatedTime) VALUES (@EventID, @LogLevel, @Message, @CreatedTime)";

            var paramList = new List<SqlParameter>
            {
                new SqlParameter("EventID", log.EventId),
                new SqlParameter("LogLevel", log.LogLevel),
                new SqlParameter("Message", log.Message),
                new SqlParameter("CreatedTime", log.CreatedTime)
            };

            return ExecuteNonQuery(command, paramList);
        }
    }
}
