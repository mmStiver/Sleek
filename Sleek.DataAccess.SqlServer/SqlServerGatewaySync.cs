using Sleek.DataAccess.Core;
using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.SqlServer
{
    public partial class SqlServerGateway :  ISqlServerGateway
    {
        #region ISyncDataGateway

        //--sync
        public object? Execute(Select query)
            => Execute(query, null);
        public object? Execute(Select query, Action<SqlCommand>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
                using (SqlCommand command = new SqlCommand(query.Text, connection))
                {
                    if(Setup != null)
                        Setup.Invoke(command);
                    command.CommandType = CommandType.Text;
                    connection.Open();

                    var scalar = command.ExecuteScalar();
                    return (scalar is not DBNull)
                        ? scalar
                        : null;
                }
        }
        public object? Execute(Select query, object input, Action<SqlCommand, object>? Setup)
            => GetSingle<object>(query.Text, CommandType.Text, input, Setup);
        public object? Execute(StoredProcedure procedure)
            => Execute(procedure, null);
        public object? Execute(StoredProcedure procedure, Action<SqlCommand>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (SqlCommand command = new SqlCommand(procedure.Name, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                var scalar = command.ExecuteScalar();
                return (scalar is not DBNull)
                        ? scalar
                        : null;
            }
        }
        /// <summary>
        /// Executes a Select query and returns the first column of the first row in the result set returned by the query.
        /// </summary>
        /// <param name="query">The Select query to be executed.</param>
        /// <returns>An object that contains the value of the first column of the first row in the result set or null if the result set is empty.</returns>
        public TOutput? Execute<TOutput>(Select query)
            => Execute<TOutput>(query, (Action<SqlCommand>?)null);
        public TOutput? Execute<TOutput>(Select query, Action<SqlCommand>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
                using (SqlCommand command = new SqlCommand(query.Text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = CommandType.Text;
                connection.Open();
                var res = command.ExecuteScalar();
                return (res is not DBNull)
                    ? (TOutput)res
                    : default(TOutput?);
            }
        }
        public TOutput? Execute<TInput, TOutput>(Select query, TInput Input, Action<SqlCommand, TInput>? Setup)
            => GetSingle<TInput, TOutput>(query.Text, CommandType.Text, Input, Setup);
        public TOutput? Execute<TOutput>(Select Query, Func<DbDataReader, TOutput> Mapper)
            => Execute<TOutput>(Query, null, Mapper);
        public TOutput? Execute<TOutput>(Select Query, Action<SqlCommand>? Setup, Func<DbDataReader, TOutput> Mapper)
            => GetReader<TOutput>(Query.Text, CommandType.Text, Setup, Mapper);
        public TOutput? Execute<TInput, TOutput>(Select Query, TInput Input, Action<SqlCommand, TInput>? Setup, Func<DbDataReader, TOutput> Mapper)
      => GetReader<TInput, TOutput>(Query.Text, CommandType.Text, Input, Setup, Mapper);
        public TOutput? Execute<TOutput>(StoredProcedure procedure) 
            => Execute<TOutput>(procedure, (Action<SqlCommand>?)null);
       
        public TOutput? Execute<TOutput>(StoredProcedure procedure, Action<SqlCommand>? Setup)
            => GetSingle<TOutput>(procedure.Name, CommandType.StoredProcedure, Setup);
        /// <summary>
        /// Executes a stored procedure and maps the result set to a single output using the provided mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output object.</typeparam>
        /// <param name="procedure">The stored procedure to be executed.</param>
        /// <param name="Mapper">A function to map the DbDataReader result set to the output object.</param>
        /// <returns>The output object or null if the result set is empty.</returns>
        public TOutput? Execute<TOutput>(StoredProcedure procedure, Func<DbDataReader, TOutput> Mapper)
            => Execute(procedure, null, Mapper);
        /// <summary>
        /// Executes a stored procedure and maps the result set to a single output using the provided mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output object.</typeparam>
        /// <param name="procedure">The stored procedure to be executed.</param>
        /// <param name="Setup">An optional action to perform additional setup for the SqlCommand.</param>
        /// <param name="Mapper">A function to map the DbDataReader result set to the output object.</param>
        /// <returns>The output object or null if the result set is empty.</returns>
        public TOutput? Execute<TOutput>(StoredProcedure procedure, Action<SqlCommand>? Setup, Func<DbDataReader, TOutput> Mapper)
            => GetReader<TOutput>(procedure.Name, CommandType.StoredProcedure, Setup, Mapper);

        public int Execute(Write Query)
            => Execute(Query, null);
        public int Execute(Write Query, Action<SqlCommand>? Setup)
           => Post(Query.Text, CommandType.Text, Setup);
        /// <summary>
        /// Executes a DataDefinitionQuery and returns the number of rows affected.
        /// </summary>
        /// <param name="Query">The DataDefinitionQuery to be executed.</param>
        /// <returns>The number of rows affected.</returns>
        public int Execute(DataDefinitionQuery Query)
        {
            using (SqlConnection connection = new(this.connectionConfiguration))
            using (SqlCommand command = new SqlCommand(Query.Text, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();

                return command.ExecuteNonQuery();
            }
        }
        #endregion

        /// <summary>
        /// Executes a non-query command (INSERT, UPDATE, DELETE) and returns the number of rows affected.
        /// </summary>
        /// <param name="text">The command text.</param>
        /// <param name="commandType">The type of the command (Text, StoredProcedure).</param>
        /// <param name="Setup">An optional action to perform additional setup for the SqlCommand.</param>
        /// <returns>The number of rows affected.</returns>
        private int Post(string text, CommandType commandType, Action<SqlCommand>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = commandType;
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Executes a command and returns a single output of type TOutput.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output object.</typeparam>
        /// <param name="text">The command text.</param>
        /// <param name="commandType">The type of the command (Text, StoredProcedure).</param>
        /// <param name="Setup">An optional action to perform additional setup for the SqlCommand.</param>
        /// <returns>The output object or null if the result set is empty.</returns>
        private TOutput? GetSingle<TOutput>(string text,
           CommandType commandType,
           Action<SqlCommand>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
                using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = commandType;
                connection.Open();

                var scalar = command.ExecuteScalar();
                return (scalar is not DBNull)
                    ? (TOutput)scalar
                    : default(TOutput?); ;
            }
        }
        private TOutput? GetSingle<TOutput>(string text,
           CommandType commandType,
           object input,
           Action<SqlCommand, object>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command, input);

                command.CommandType = commandType;
                connection.Open();

                var scalar = command.ExecuteScalar();
                return (scalar is not DBNull)
                    ? (TOutput)scalar
                    : default(TOutput?); ;
            }
        }
        private TOutput? GetSingle<Tinput, TOutput>(string text,
           CommandType commandType,
           Tinput input,
           Action<SqlCommand, Tinput>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command, input);

                command.CommandType = commandType;
                connection.Open();

                var scalar = command.ExecuteScalar();
                return (scalar is not DBNull)
                    ? (TOutput)scalar
                    : default(TOutput?); ;
            }
        }
        private TOutput? GetReader<TOutput>(string text,
          CommandType commandType,
           Action<SqlCommand>? Setup,
          Func<DbDataReader, TOutput> Mapper
            )
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = commandType;
                connection.Open();

                DbDataReader reader = command.ExecuteReader();
                return Mapper.Invoke(reader);
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
        /// <param name="Setup">An optional action to perform additional setup for the SqlCommand.</param>
        /// <param name="Mapper">A function to map the DbDataReader result set to the output object.</param>
        /// <returns>The output object or null if the result set is empty.</returns>
        private TOutput? GetReader<TInput, TOutput>(string text,
          CommandType commandType,
          TInput input,
           Action<SqlCommand, TInput>? Setup,
          Func<DbDataReader, TOutput> Mapper
            )
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command, input);

                command.CommandType = commandType;
                connection.Open();

                DbDataReader reader = command.ExecuteReader();
                return Mapper.Invoke(reader);
            }
        }
    }
}