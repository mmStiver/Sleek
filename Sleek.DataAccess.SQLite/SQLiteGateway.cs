using Sleek.DataAccess.Core;
using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Sleek.DataAccess.SQLite
{
    public class SQLiteGateway : ISQLiteGateway, IDisposable
    {
        private readonly string connectionConfiguration;
        private readonly bool persist;
        private SQLiteConnection? connection;

        /// <summary>
        /// Initializes a new instance of the SQLLiteGateway class with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to be used for connecting to the SQLite database.</param>
        /// <param name="persistConnection">Keep the connection open for the lifespan of the data gateway</param>
        public SQLiteGateway(string connectionString, bool persistConnection = false)
        {
            this.connectionConfiguration = connectionString;
            this.persist = persistConnection;
            this.connection = null;
        }

        /// <summary>
        /// Initializes a new instance of the SQLLiteGateway class with the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to be used for connecting to the SQLite database.</param>
        /// <param name="persistConnection">Keep the connection open for the lifespan of the data gateway</param>
        public SQLiteGateway(SQLiteConnection connection)
        {
            this.connectionConfiguration = connection.ConnectionString;
            this.persist = true;
            this.connection = connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object? Execute(Select Query)
            => Execute(Query, null);

        public object? Execute(Select Query, Action<DbCommand>? Setup)
              => GetSingle<object?>(Query.Text, Setup);

        public object? Execute(Select Query, object Input, Action<DbCommand, object>? Setup)
            => GetSingle<object, object?>(Query.Text, Input, Setup);

        public TOutput? Execute<TOutput>(Select Query)
            => GetSingle<TOutput?>(Query.Text, null);

        public TOutput? Execute<TOutput>(Select Query, Action<DbCommand>? Setup)
            => GetSingle<TOutput?>(Query.Text, Setup);

        public TOutput? Execute<TInput, TOutput>(Select Query, TInput Input, Action<DbCommand, TInput>? Setup)
            => GetSingle<TInput, TOutput>(Query.Text, Input, Setup);

        public TOutput? Execute<TOutput>(Select Query, Func<DbDataReader, TOutput> Mapper)
            => Execute<TOutput>(Query, null, Mapper);

        public TOutput? Execute<TOutput>(Select Query, Action<DbCommand>? Setup, Func<DbDataReader, TOutput> Mapper)
            => Execute<TOutput>(Query, Setup, Mapper);

        public TOutput? Execute<TInput, TOutput>(Select Query, TInput Input, Action<DbCommand, TInput>? Setup, Func<DbDataReader, TOutput> Mapper)
            => GetReader(Query.Text, Input, null, Mapper);


        public int Execute(Write Query)
            => Execute(Query, null);

        public int Execute(Write Query, Action<DbCommand>? Setup)
            => Post(Query.Text, Setup);

        public int Execute(DataDefinitionQuery Query)
            => Post(Query.Text, null);

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

        public void Dispose()
        {
            if (connection != null)
            {
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
                this.connection.Dispose();
                this.connection = null;
            }
        }

        /// <summary>
        /// Executes a non-query command (INSERT, UPDATE, DELETE) and returns the number of rows affected.
        /// </summary>
        /// <param name="text">The command text.</param>
        /// <param name="commandType">The type of the command (Text only).</param>
        /// <param name="Setup">An optional action to perform additional setup for the DbCommand.</param>
        /// <returns>The number of rows affected.</returns>
        private int Post(string text, Action<DbCommand>? Setup)
        {
            try
            {
                SQLiteConnection connection = GetOrOpenConnection();
                using (SQLiteCommand command = new SQLiteCommand(text, connection))
                {
                    if (Setup != null)
                        Setup.Invoke(command);

                    command.CommandType = CommandType.Text;

                    return command.ExecuteNonQuery();
                }
            }
            finally
            {
                if (!persist)
                    this.Dispose();
            }
        }

        private TOutput? GetSingle<TOutput>(string text, Action<DbCommand>? Setup)
        {
            try
            {
                SQLiteConnection connection = GetOrOpenConnection();
                using (DbCommand command = new SQLiteCommand(text, connection))
                {
                    if (Setup != null)
                        Setup.Invoke(command);

                    command.CommandType = CommandType.Text;

                    var scalar = command.ExecuteScalar();
                    return (scalar is not DBNull)
                        ? (TOutput?)scalar
                        : default;
                }
            }
            finally
            {
                if (!persist)
                    this.Dispose();
            }
        }
        private TOutput? GetSingle<TInput, TOutput>(string text, TInput? input, Action<DbCommand, TInput>? Setup)
        {
            try
            {
                SQLiteConnection connection = GetOrOpenConnection();
                using (DbCommand command = new SQLiteCommand(text, connection))
                {
                    if (Setup != null)
                        Setup.Invoke(command, input);

                    command.CommandType = CommandType.Text;

                    var scalar = command.ExecuteScalar();
                    return (scalar is not DBNull)
                        ? (TOutput?)scalar
                        : default;
                }
            }
            finally
            {
                if (!persist)
                    this.Dispose();
            }
        }
        private TOutput? GetReader<TOutput>(string text,
   Action<DbCommand>? Setup,
  Func<DbDataReader, TOutput> Mapper
    )
        {
            try
            {
                SQLiteConnection connection = GetOrOpenConnection();
                using (DbCommand command = new SQLiteCommand(text, connection))
                {
                    if (Setup != null)
                        Setup.Invoke(command);

                    command.CommandType = CommandType.Text;
                 

                    DbDataReader reader = command.ExecuteReader();
                    return Mapper.Invoke(reader);
                }
            }
            finally
            {
                if (!persist)
                    this.Dispose();
            }
        }
        /// <summary>
        /// Executes a command with an input parameter and maps the result set to a single output using the provided mapper function.
        /// </summary>
        /// <typeparam name="TInput">The type of the input object.</typeparam>
        /// <typeparam name="TOutput">The type of the output object.</typeparam>
        /// <param name="text">The command text.</param>
        /// <param name="commandType">The type of the command (Text, StoredProcedure).</param>
        /// <param name="input">The input object.</param>
        /// <param name="Setup">An optional action to perform additional setup for the DbCommand.</param>
        /// <param name="Mapper">A function to map the DbDataReader result set to the output object.</param>
        /// <returns>The output object or null if the result set is empty.</returns>
        private TOutput? GetReader<TInput, TOutput>(string text,
          TInput input,
           Action<DbCommand, TInput>? Setup,
          Func<DbDataReader, TOutput> Mapper
            )
        {
            try
            {
                SQLiteConnection connection = GetOrOpenConnection();
                using (DbCommand command = new SQLiteCommand(text, connection))
                {
                    if (Setup != null)
                        Setup.Invoke(command, input);

                    command.CommandType = CommandType.Text;
               
                    DbDataReader reader = command.ExecuteReader();
                    return Mapper.Invoke(reader);
                }
            }
            finally
            {
                if (!persist)
                    this.Dispose();
            }
        }
        private SQLiteConnection GetOrOpenConnection()
        {
            if (persist && connection != null && connection.State == ConnectionState.Open)
                return connection;

            return connection = new SQLiteConnection(this.connectionConfiguration).OpenAndReturn();
        }
    }
}