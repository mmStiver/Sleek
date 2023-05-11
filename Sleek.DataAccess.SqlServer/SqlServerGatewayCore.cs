using Sleek.DataAccess.Core;
using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.SqlServer
{
    public partial class SqlServerGateway :  ISqlServerGateway
    {
        private readonly string connectionConfiguration;

        /// <summary>
        /// Initializes a new instance of the SqlServerGateway class with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to be used for connecting to the SQL Server database.</param>
        public SqlServerGateway(string connectionString)
        {
            connectionConfiguration = connectionString;
        }

        #region TestConnection

        /// <summary>
        /// Asynchronously tests the database connection by executing a simple SELECT statement.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation</param>
        /// <returns>Returns true if the connection is successfully established and the query is executed; otherwise, returns false.</returns>
        public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken)
        {
            try
            {
                await using (var connection = new SqlConnection(this.connectionConfiguration))
                {
                    connection.Open();

                    await using (var command = new SqlCommand("SELECT 1;", connection))
                    {
                        object? result = await command.ExecuteScalarAsync();

                        // If the result is null or not equal to 1, the database is not accessible
                        if (result == null || Convert.ToInt32(result) != 1)
                        {
                            return false;
                        };
                    }
                }
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        /// <summary>
        /// Synchronously tests the database connection by executing a simple SELECT statement.
        /// </summary>
        /// <returns>Returns true if the connection is successfully established and the query is executed; otherwise, returns false.</returns>
        public bool TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(this.connectionConfiguration))
                {
                    connection.Open();

                    using (var command = new SqlCommand("SELECT 1;", connection))
                    {
                        object result = command.ExecuteScalar();

                        // If the result is null or not equal to 1, the database is not accessible
                        if (result == null || Convert.ToInt32(result) != 1)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }
        #endregion

    }
}