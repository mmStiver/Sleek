using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Sleek.DataAcess.SqlServerTest
{
    public class SqlServerTestFixture : IDisposable
    {
        public readonly string connectionString;
        private static readonly object GlobalLock = new object();
        TimeSpan timeout = TimeSpan.FromSeconds(30);

        public SqlServerTestFixture()
        {
            this.connectionString = string.Format(TestData.databaseConnection, TestData.TestDatabase, timeout.TotalSeconds);

            lock (GlobalLock) {
                SetupData();
            }
            
        }

        private void SetupData()
        {
            var localInstance = string.Format(TestData.localConnection, timeout.TotalSeconds);
            var localDatabase = this.connectionString;
            
            // Create test database
            if (!TestConnection(localDatabase))
            {
                using (var connection = new SqlConnection(localInstance))
                {
                    connection.Open();
                    var query = string.Format(TestData.CreateDatabase, TestData.TestDatabase);
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }

            // Connect to the newly created database

            using (var connection = new SqlConnection(localDatabase))
            {
                connection.Open();

                // Create a table
                if (!TableExists(connection, TestData.TestTableName))
                {
                    using (var command = new SqlCommand(TestData.CreatePersonTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                
                    // Insert data
                    using (var command = new SqlCommand(TestData.InsertIntoPersonTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }

                using (var command = new SqlCommand(TestData.GetPersonProcedure.Code, connection))
                    command.ExecuteNonQuery();
                using (var command = new SqlCommand(TestData.GetNullProcedure.Code, connection))
                    command.ExecuteNonQuery();
                using (var command = new SqlCommand(TestData.GetIntProcedure.Code, connection))
                    command.ExecuteNonQuery();
                using (var command = new SqlCommand(TestData.GetNumericProcedure.Code, connection))
                    command.ExecuteNonQuery();
                using (var command = new SqlCommand(TestData.GetDateProcedure.Code, connection))
                    command.ExecuteNonQuery();
                using (var command = new SqlCommand(TestData.GetDateOffsetProcedure.Code, connection))
                    command.ExecuteNonQuery();
                using (var command = new SqlCommand(TestData.GetUUIDProcedure.Code, connection))
                    command.ExecuteNonQuery();
                if(!ProcExists(connection, TestData.InsertAddressProcedure.Name))
                    using (var command = new SqlCommand(TestData.InsertAddressProcedure.Code, connection))
                        command.ExecuteNonQuery();

                if(!ProcExists(connection, "[dbo].[UpdateAddress]"))
                    using (var command = new SqlCommand(TestData.UpdateAddressProcedure.Code, connection))
                        command.ExecuteNonQuery();
            }
        }

        public bool TestConnection(string connectionString)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var command = new SqlCommand("SELECT SYSDATETIME();", connection))
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
        public bool TableExists(SqlConnection connection, string tableName)
        {
            try
            {
                if(connection.State != ConnectionState.Open) 
                    connection.Open();

                using (var command = new SqlCommand(
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
        public bool ProcExists(SqlConnection connection, string procName)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();

                using (var command = new SqlCommand(
                    "SELECT 1 FROM sys.objects WHERE object_id = OBJECT_ID(@procName) AND type in (N'P', N'PC')",
                    connection))
                {
                     command.Parameters.AddWithValue("@procName", procName);

                    object? val = command.ExecuteScalar();
                    return (val != DBNull.Value) ? ((int)val) == 1: false;
                    
                }

            }
            catch (SqlException)
            {
                return false;
            }
        }
        public void TruncateData(SqlConnection connection, string tableName)
        {
            if (connection.State != System.Data.ConnectionState.Open)
                connection.Open();

            using (var command = new SqlCommand(
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
            //     using (var command = new SqlCommand($"CREATE TABLE #TestTemp", connection))
            //     {
            //         command.ExecuteNonQuery();
            //     }
            // }
        }
    }
}