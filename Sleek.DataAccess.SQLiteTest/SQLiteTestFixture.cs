using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Threading;
using Sleek.DataAcess.SqlServerTest;

public class SQLiteTestFixture : IDisposable
    {
        public readonly string connectionString;
        private static readonly object GlobalLock = new object();

        public SQLiteTestFixture()
        {
                var connectionString = TestData.localConnection;
                var connection = new SQLiteConnection(connectionString);
                connection.Open();

            lock (GlobalLock) {
                SetupData();
            }
        }
        
        private void SetupData()
        { 
            // Create test database
            if (!TestConnection(this.connectionString))
            {
                using (var connection = new SQLiteConnection(this.connectionString))
                {
                    connection.Open();
                    var query = string.Format(TestData.CreateDatabase, TestData.TestDatabase);
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }

            // Connect to the newly created database
            using (var connection = new SQLiteConnection(this.connectionString))
            {
                connection.Open();

                // Create a table
                if (!TableExists(connection, TestData.TestTableName))
                {
                    using (var command = new SQLiteCommand(TestData.CreatePersonTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                
                    // Insert data
                    using (var command = new SQLiteCommand(TestData.InsertIntoPersonTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
           
                using (var command = new SQLiteCommand(@"PRAGMA journal_mode = 'wal'", connection))
                    command.ExecuteNonQuery();
 
            }
        }

        public bool TestConnection(string connectionString)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SQLiteCommand("SELECT SYSDATETIME();", connection))
                    {
                        command.ExecuteScalar();
                    }
                }
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }
        public bool TableExists(SQLiteConnection connection, string tableName)
        {
            try
            {
                if(connection.State != ConnectionState.Open) 
                    connection.Open();

                using (var command = new SQLiteCommand(
                    "SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = @TableName;",
                    connection))
                {
                    command.Parameters.AddWithValue("@TableName", tableName);

                    int tableCount = (int)command.ExecuteScalar();
                    return tableCount > 0;
                }
                
            }
            catch (SqlException)
            {
                return false;
            }
        }
        public void TruncateData(SQLiteConnection connection, string tableName)
        {
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            using (var command = new SQLiteCommand(
                $"TRUNCATE TABLE {tableName};",
                connection))
            {
                var _ = command.ExecuteScalar();
            }
        }


        public void Dispose()
        {
           // using (var connection = new SqlConnection(this.connectionString))
           //     TruncateData(connection, TestData.TestTableName);
       // }
            // {
            //     connection.Open();
            //
            //     using (var command = new SQLiteCommand($"CREATE TABLE #TestTemp", connection))
            //     {
            //         command.ExecuteNonQuery();
            //     }
            // }
        }
    }
