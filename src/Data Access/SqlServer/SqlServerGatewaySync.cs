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

        /// <summary>
        /// Executes a SQL select query and returns the first column of the first row in the result set returned by the query.
        /// </summary>
        /// <param name="query">An instance of the Select class representing the SQL select query.</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public object? Execute(Select query)
            => Execute(query, null);

        /// <summary>
        /// Executes a SQL select query using a setup action and returns the first column of the first row in the result set returned by the query.
        /// </summary>
        /// <param name="query">An instance of the Select class representing the SQL select query.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public object? Execute(Select query, Action<DbCommand>? Setup)
            => GetSingle<object>(query.Text, CommandType.Text, Setup);

        /// <summary>
        /// Executes a SQL select query using a setup action and an input object and returns the first column of the first row in the result set returned by the query.
        /// </summary>
        /// <param name="query">An instance of the Select class representing the SQL select query.</param>
        /// <param name="input">An object that is used in the setup action.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public object? Execute(Select query, object input, Action<DbCommand, object>? Setup)
            => GetSingle<object>(query.Text, CommandType.Text, input, Setup);

        /// <summary>
        /// Executes a stored procedure without any setup action and returns the first column of the first row in the result set returned by the procedure.
        /// </summary>
        /// <param name="procedure">An instance of the StoredProcedure class representing the stored procedure to execute.</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public object? Execute(StoredProcedure procedure)
            => Execute(procedure, null);

        /// <summary>
        /// Executes a stored procedure using a setup action and returns the first column of the first row in the result set returned by the procedure.
        /// </summary>
        /// <param name="procedure">An instance of the StoredProcedure class representing the stored procedure to execute.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <returns>The first column of the first row in the result set, or a null reference if the result set is empty.</returns>
        public object? Execute(StoredProcedure procedure, Action<DbCommand>? Setup)
            => GetSingle<object>(procedure.Name, CommandType.StoredProcedure, Setup);

        /// <summary>
        /// Executes a Select query and returns the first column of the first row in the result set returned by the query.
        /// </summary>
        /// <param name="query">The Select query to be executed.</param>
        /// <returns>An object that contains the value of the first column of the first row in the result set or null if the result set is empty.</returns>
        public TOutput? Execute<TOutput>(Select query)
            => Execute<TOutput>(query, (Action<DbCommand>?)null);
        /// <summary>
        /// Executes a SELECT query using a setup action and maps the result set to an instance of the specified type.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="query">An instance of the Select class representing the query to execute.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <returns>An instance of the specified type representing the result set, or null if the result set is empty.</returns>
        public TOutput? Execute<TOutput>(Select query, Action<DbCommand>? Setup)
            => GetSingle<TOutput>(query.Text, CommandType.Text, Setup);

        /// <summary>
        /// Executes a SELECT query with a specific input parameter using a setup action and maps the result set to an instance of the specified type.
        /// </summary>
        /// <typeparam name="TInput">The type of the input parameter.</typeparam>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="query">An instance of the Select class representing the query to execute.</param>
        /// <param name="Input">The input parameter for the query.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <returns>An instance of the specified type representing the result set, or null if the result set is empty.</returns>
        public TOutput? Execute<TInput, TOutput>(Select query, TInput Input, Action<DbCommand, TInput>? Setup)
            => GetSingle<TInput, TOutput>(query.Text, CommandType.Text, Input, Setup);

        /// <summary>
        /// Executes a SELECT query and uses a mapper function to map the result set to an instance of the specified type.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="Query">An instance of the Select class representing the query to execute.</param>
        /// <param name="Mapper">A function to map the result set to an instance of the specified type.</param>
        /// <returns>An instance of the specified type representing the result set, or null if the result set is empty.</returns>
        public TOutput? Execute<TOutput>(Select Query, Func<DbDataReader, TOutput> Mapper)
            => Execute<TOutput>(Query, null, Mapper);

        /// <summary>
        /// Executes a SELECT query using a setup action and a mapper function and maps the result set to an instance of the specified type.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="Query">An instance of the Select class representing the query to execute.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <param name="Mapper">A function to map the result set to an instance of the specified type.</param>
        /// <returns>An instance of the specified type representing the result set, or null if the result set is empty.</returns>
        public TOutput? Execute<TOutput>(Select Query, Action<DbCommand>? Setup, Func<DbDataReader, TOutput> Mapper)
            => GetReader<TOutput>(Query.Text, CommandType.Text, Setup, Mapper);

        /// <summary>
        /// Executes a SELECT query with a specific input parameter using a setup action and a mapper function, and maps the result set to an instance of the specified type.
        /// </summary>
        /// <typeparam name="TInput">The type of the input parameter.</typeparam>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="Query">An instance of the Select class representing the query to execute.</param>
        /// <param name="Input">The input parameter for the query.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <param name="Mapper">A function to map the result set to an instance of the specified type.</param>
        /// <returns>An instance of the specified type representing the result set, or null if the result set is empty.</returns>
        public TOutput? Execute<TInput, TOutput>(Select Query, TInput Input, Action<DbCommand, TInput>? Setup, Func<DbDataReader, TOutput> Mapper)
            => GetReader<TInput, TOutput>(Query.Text, CommandType.Text, Input, Setup, Mapper);

        /// <summary>
        /// Executes a stored procedure and maps the result set to an instance of the specified type.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="procedure">An instance of the StoredProcedure class representing the procedure to execute.</param>
        /// <returns>An instance of the specified type representing the result set, or null if the result set is empty.</returns>
        public TOutput? Execute<TOutput>(StoredProcedure procedure)
            => Execute<TOutput>(procedure, (Action<DbCommand>?)null);

        /// <summary>
        /// Executes a stored procedure with a specific setup action, and maps the result set to an instance of the specified type.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="procedure">An instance of the StoredProcedure class representing the procedure to execute.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <returns>An instance of the specified type representing the result set, or null if the result set is empty.</returns>
        public TOutput? Execute<TOutput>(StoredProcedure procedure, Action<DbCommand>? Setup)
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
        /// <param name="Setup">An optional action to perform additional setup for the DbCommand.</param>
        /// <param name="Mapper">A function to map the DbDataReader result set to the output object.</param>
        /// <returns>The output object or null if the result set is empty.</returns>
        public TOutput? Execute<TOutput>(StoredProcedure procedure, Action<DbCommand>? Setup, Func<DbDataReader, TOutput> Mapper)
            => GetReader<TOutput>(procedure.Name, CommandType.StoredProcedure, Setup, Mapper);

        /// <summary>
        /// Executes a write query and returns the number of rows affected.
        /// </summary>
        /// <param name="Query">An instance of the Write class representing the query to execute.</param>
        /// <returns>The number of rows affected by the query.</returns>
        public int Execute(Write Query)
            => Execute(Query, null);

        /// <summary>
        /// Executes a write query with a specific setup action and returns the number of rows affected.
        /// </summary>
        /// <param name="Query">An instance of the Write class representing the query to execute.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <returns>The number of rows affected by the query.</returns>
        public int Execute(Write Query, Action<DbCommand>? Setup)
           => Post<object>(Query.Text, CommandType.Text, null,  Setup, null);

        /// <summary>
        /// Executes a write query with a specific setup action and returns the number of rows affected.
        /// </summary>
        /// <param name="input">The input object.</param>
        /// <param name="Query">An instance of the Write class representing the query to execute.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <returns>The number of rows affected by the query.</returns>
        public int Execute(Write Query, object Input, Action<DbCommand, object>? Setup)
        => Post<object>(Query.Text, CommandType.Text, Input, null, Setup);

        /// <summary
        /// Executes a write query with a specific setup action and returns the number of rows affected.
        /// </summary>
        /// <typeparam name="TInput">The type of the input object.</typeparam>
        /// <param name="input">The input object.</param>
        /// <param name="Query">An instance of the Write class representing the query to execute.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <returns>The number of rows affected by the query.</returns>
        public int Execute<TInput>(Write Query, TInput Input, Action<DbCommand, TInput>? Setup)
        => Post<TInput>(Query.Text, CommandType.Text, Input, null, Setup);

        /// <summary>
        /// Executes a DataDefinitionQuery and returns the number of rows affected.
        /// </summary>
        /// <param name="Query">The DataDefinitionQuery to be executed.</param>
        /// <returns>The number of rows affected.</returns>
        public int Execute(DataDefinitionQuery Query)
        {
            using (SqlConnection connection = new(this.connectionConfiguration))
            using (DbCommand command = new SqlCommand(Query.Text, connection))
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
        /// <param name="Setup">An optional action to perform additional setup for the DbCommand.</param>
        /// <returns>The number of rows affected.</returns>
        private int Post<TInput>(string text, 
            CommandType commandType, 
            TInput? Input, 
            Action<DbCommand>? Setup,
            Action<DbCommand, TInput?>? inSetup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
                using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);
                if (inSetup != null)
                    inSetup.Invoke(command, Input);

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
        /// <param name="Setup">An optional action to perform additional setup for the DbCommand.</param>
        /// <returns>The output object or null if the result set is empty.</returns>
        private TOutput? GetSingle<TOutput>(string text,
           CommandType commandType,
           Action<DbCommand>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
                using (DbCommand command = new SqlCommand(text, connection))
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
           Action<DbCommand, object>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (DbCommand command = new SqlCommand(text, connection))
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
           Action<DbCommand, Tinput>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (DbCommand command = new SqlCommand(text, connection))
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
           Action<DbCommand>? Setup,
          Func<DbDataReader, TOutput> Mapper
            )
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (DbCommand command = new SqlCommand(text, connection))
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
        /// <param name="Setup">An optional action to perform additional setup for the DbCommand.</param>
        /// <param name="Mapper">A function to map the DbDataReader result set to the output object.</param>
        /// <returns>The output object or null if the result set is empty.</returns>
        private TOutput? GetReader<TInput, TOutput>(string text,
          CommandType commandType,
          TInput input,
           Action<DbCommand, TInput>? Setup,
          Func<DbDataReader, TOutput> Mapper
            )
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (DbCommand command = new SqlCommand(text, connection))
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