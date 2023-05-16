using Sleek.DataAccess.Core;
using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Sleek.DataAccess.SQLite
{
    public class SQLiteGateway : ISQLiteGateway
    {
        private readonly string connectionConfiguration;

        /// <summary>
        /// Initializes a new instance of the SQLLiteGateway class with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to be used for connecting to the SQLite database.</param>
        public SQLiteGateway(string connectionString)
        {
            connectionConfiguration = connectionString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object? Execute(Select query)
        {
            throw new NotImplementedException();
        }

        public object? Execute(Select query, Action<SqlCommand>? Setup)
        {
            throw new NotImplementedException();
        }

        public object? Execute(Select query, object input, Action<SqlCommand, object>? Setup)
        {
            throw new NotImplementedException();
        }

        public TOutput? Execute<TOutput>(Select query)
        {
            throw new NotImplementedException();
        }

        public TOutput? Execute<TOutput>(Select query, Action<SqlCommand>? Setup)
        {
            throw new NotImplementedException();
        }

        public TOutput? Execute<TInput, TOutput>(Select query, TInput Input, Action<SqlCommand, TInput>? Setup)
        {
            throw new NotImplementedException();
        }

        public TOutput? Execute<TOutput>(Select Query, Func<DbDataReader, TOutput> Mapper)
        {
            throw new NotImplementedException();
        }

        public TOutput? Execute<TOutput>(Select Query, Action<SqlCommand>? Setup, Func<DbDataReader, TOutput> Mapper)
        {
            throw new NotImplementedException();
        }

        public TOutput? Execute<TInput, TOutput>(Select Query, TInput Input, Action<SqlCommand, TInput>? Setup, Func<DbDataReader, TOutput> Mapper)
        {
            throw new NotImplementedException();
        }

        public int Execute(Write query)
        {
            throw new NotImplementedException();
        }

        public int Execute(Write query, Action<SqlCommand>? Setup)
        {
            throw new NotImplementedException();
        }

        public int Execute(DataDefinitionQuery query)
        {
            throw new NotImplementedException();
        }

        public bool TestConnection()
        {

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connectionConfiguration))
                {
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}