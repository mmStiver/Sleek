using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Sleek.DataAccess.SqlServerTest
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
                if (!TableExists(connection, "InsertCmdTestLong"))
                    CreateTempTable(connection, "InsertCmdTestLong", typeof(long));
                if (!TableExists(connection, "InsertCmdTestInt"))
                    CreateTempTable(connection, "InsertCmdTestInt", typeof(int));
                if (!TableExists(connection, "InsertCmdTestShort"))
                    CreateTempTable(connection, "InsertCmdTestShort", typeof(short));
                if (!TableExists(connection, "InsertCmdTestByte"))
                    CreateTempTable(connection, "InsertCmdTestByte", typeof(byte));
                if (!TableExists(connection, "InsertCmdTestGuid"))
                    CreateTempTable(connection, "InsertCmdTestGuid", typeof(Guid));

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
                    return (val == null || val == DBNull.Value) ? false : ((int)val) == 1;
                    
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

        private void DropTempTable(SqlConnection connection, String TableName)
        {
            using (var cmd = new SqlCommand($"DROP TABLE {TableName}", connection))
            {
                cmd.ExecuteNonQuery();
            }
        }
        private void CreateTempTable(SqlConnection connection, String TableName, Type t)
        {
            if (t == typeof(Guid)) {
                using (var cmd = new SqlCommand(GetUUIDTempTableQuery(TableName), connection))
                    cmd.ExecuteNonQuery();
            }
            else
            { 
                using (var cmd = new SqlCommand(GetTempTableQuery(TableName, t), connection))
                    cmd.ExecuteNonQuery();
            }
        }

        private string GetTempTableQuery(String TableName, Type t)
            => $"CREATE TABLE {TableName} ( MyID {GetIdentityString(t)} PRIMARY KEY IDENTITY(1,1), IsTrue BIT)";
        private string GetUUIDTempTableQuery(String TableName)
            => $"CREATE TABLE {TableName} ( MyID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), IsTrue BIT)";
       
        
        private string GetIdentityString(Type t)
            => t switch
            {
                var type when t == typeof(Int64) => "BIGINT",
                var type when t == typeof(Int32) => "INT",
                var type when t == typeof(Int16) => "SmallINT",
                var type when t == typeof(byte) => "TINYINT",
                _ => throw new NotSupportedException()

            };

        //var type when t == typeof(Guid) => "UNIQUEIDENTIFIER",

        public void Dispose()
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();
                if (!TableExists(connection, "InsertCmdTestInt"))
                    DropTempTable(connection, "InsertCmdTestInt");
                if (!TableExists(connection, "InsertCmdTestLong"))
                    DropTempTable(connection, "InsertCmdTestLong");
                if (!TableExists(connection, "InsertCmdTestShort"))
                    DropTempTable(connection, "InsertCmdTestShort");
                if (!TableExists(connection, "InsertCmdTestByte"))
                    DropTempTable(connection, "InsertCmdTestByte");
                if (!TableExists(connection, "InsertCmdTestGuid"))
                    DropTempTable(connection, "InsertCmdTestGuid");
            }
        }
    }
    
    [CollectionDefinition("SQL Server Database collection")]
    public class SqlServerCollection : ICollectionFixture<SqlServerTestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}