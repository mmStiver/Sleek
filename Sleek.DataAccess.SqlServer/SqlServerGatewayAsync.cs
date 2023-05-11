using Sleek.DataAccess.Core;
using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.SqlServer
{
    public partial class SqlServerGateway :  ISqlServerGateway
    {
        #region IAsyncDataGateway
        /// <summary>
        /// Executes a SELECT query asynchronously and returns the first column of the first row in the result set as an object.
        /// </summary>
        /// <param name="query">The SELECT query to be executed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the first column of the first row in the result set or null if the result set is empty.</returns>
        public async Task<object?> ExecuteAsync(Select query, CancellationToken cancellationToken = default)
            => await ExecuteAsync(query, null, cancellationToken);
        public async Task<object?> ExecuteAsync(Select query, Action< SqlCommand>? Setup = null, CancellationToken cancellationToken = default)
            => await GetSingle<object?>(query.Text, CommandType.Text, Setup, cancellationToken);
        public async Task<object?> ExecuteAsync(Select query, object input, Action<SqlCommand, object>? Setup, CancellationToken cancellationToken = default)
            => await GetSingle<object, object?>(query.Text, CommandType.Text, input, Setup, cancellationToken);

        public async Task<TOutput?> ExecuteAsync<TOutput>(Select query, CancellationToken cancellationToken = default)
            => await ExecuteAsync<TOutput>(query, (Action<SqlCommand>?)null, cancellationToken);
        public async Task<TOutput?> ExecuteAsync<TOutput>(Select query, Action<SqlCommand>? Setup, CancellationToken cancellationToken = default)
             => await GetSingle<TOutput>(query.Text, CommandType.Text, Setup, cancellationToken);
       
        //query + reader
        public async Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default)
            => await ExecuteAsync<TOutput>(Query, null, Mapper, cancellationToken);
        public async Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default)
           => await ExecuteAsync<TOutput>(Query, null, Mapper, cancellationToken);
        public async Task<TOutput?> ExecuteAsync<TInput, TOutput>(Select Query, TInput input, Action<SqlCommand, TInput>? Setup, CancellationToken cancellationToken = default)
            => await GetSingle<TInput, TOutput?>(Query.Text, CommandType.Text, input, Setup, cancellationToken);

        public async Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Action<SqlCommand>? Setup, Func<DbDataReader, TOutput>? Mapper, CancellationToken cancellationToken = default)
        => await GetReader<TOutput>(Query.Text, CommandType.Text,  Setup, Mapper,cancellationToken);
        public async Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Action<SqlCommand>? Setup, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default)
        => await GetReader<TOutput>(Query.Text, CommandType.Text, Setup, Mapper, cancellationToken);
        public async Task<TOutput?> ExecuteAsync<TInput, TOutput>(Select Query, TInput input, Action<SqlCommand, TInput>? Setup, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default)
                => await GetReader<TInput, TOutput>(Query.Text, CommandType.Text, input, Setup, Mapper, cancellationToken);

        public async Task<TOutput?> ExecuteAsync<TInput, TOutput>(Select Query, TInput input, Action<SqlCommand, TInput>? Setup, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default)
                => await GetReader<TInput, TOutput>(Query.Text, CommandType.Text, input, Setup, Mapper, cancellationToken);


        //sproc scalar
        public async Task<object?> ExecuteAsync(StoredProcedure procedure, CancellationToken cancellationToken)
            => await ExecuteAsync(procedure, null, cancellationToken);
        public async Task<object?> ExecuteAsync(StoredProcedure procedure, Action<SqlCommand>? Setup, CancellationToken cancellationToken)
            => await GetSingle<object>(procedure.Name, CommandType.StoredProcedure, Setup, cancellationToken);
        public async Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure procedure, CancellationToken cancellationToken)
            => await GetSingle<TOutput>(procedure.Name, CommandType.StoredProcedure, null, cancellationToken);
        public async Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure procedure, Action<SqlCommand>? Setup, CancellationToken cancellationToken = default)
             => await GetSingle<TOutput>(procedure.Name, CommandType.StoredProcedure, Setup, cancellationToken);

        //sproc reader
        public async Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure procedure, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default)
            => await ExecuteAsync<TOutput>(procedure, null, Mapper, cancellationToken);
        public Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure procedure, Action<SqlCommand>? Setup, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default)
            => GetReader<TOutput>(procedure.Name, CommandType.StoredProcedure, Setup, Mapper, cancellationToken);
        public async Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure procedure, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default)
            => await ExecuteAsync(procedure, null, Mapper, cancellationToken);
        public async Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, Action<SqlCommand>? Setup, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default)
            => await GetReader(Query.Name, CommandType.StoredProcedure, Setup, Mapper, cancellationToken);

        //NonQuery
        /// <summary>
        /// Executes a non-query statement asynchronously and returns the number of rows affected.
        /// </summary>
        /// <param name="Query">The non-query statement to be executed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the number of rows affected.</returns>
        public async Task<int> ExecuteAsync(Write Query, CancellationToken cancellationToken = default)
            => await ExecuteAsync(Query, null, cancellationToken);
        public async Task<int> ExecuteAsync(Write Query, Action<SqlCommand>? Setup, CancellationToken cancellationToken = default)
            => await Post(Query.Text, CommandType.Text, Setup, cancellationToken);

        public async Task<int> ExecuteAsync(DataDefinitionQuery Query, CancellationToken cancellationToken = default)
        {
            return await Post(Query.Text,CommandType.Text, null, cancellationToken);
        }

        #endregion

        /// <summary>
        /// Executes a non-query statement asynchronously and returns the number of rows affected.
        /// </summary>
        /// <param name="text">The SQL command text or stored procedure name to be executed.</param>
        /// <param name="commandType">Specifies how the command text is interpreted, either as a Text command or as a StoredProcedure.</param>
        /// <param name="Setup">An optional action to perform additional setup for the SqlCommand.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the number of rows affected.</returns>
        private async Task<int> Post(string text,
           CommandType commandType,
           Action<SqlCommand>? Setup,
           CancellationToken cancellationToken)
        {
            await using (SqlConnection connection = new SqlConnection(connectionConfiguration))
                await using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);
                command.CommandType = commandType;
                connection.Open();
                return await command.ExecuteNonQueryAsync(cancellationToken);
            }
        }

        private async Task<TOutput?> GetSingle<TOutput>(string text,
            CommandType commandType,
            Action<SqlCommand>? Setup,
            CancellationToken cancellationToken)
        {
            await using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            await using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = commandType;
                connection.Open();

                var scalar = await command.ExecuteScalarAsync(cancellationToken);
                return (scalar is not DBNull)
                    ? (TOutput)scalar
                    : default(TOutput?); ;
            }
        }
        private async Task<TOutput?> GetSingle<TInput, TOutput>(string text,
           CommandType commandType,
           TInput input,
           Action<SqlCommand, TInput>? Setup,
           CancellationToken cancellationToken)
        {
            await using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            await using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command, input);

                command.CommandType = commandType;
                connection.Open();

                var scalar = await command.ExecuteScalarAsync(cancellationToken);
                return (scalar is not DBNull)
                    ? (TOutput)scalar
                    : default(TOutput?); ;
            }
        }
        /// <summary>
        /// Executes a query asynchronously and maps the result set to a single output using the provided mapper function.
        /// </summary>
        /// <typeparam name="TInput">The type of the input parameter.</typeparam>
        /// <typeparam name="TOutput">The type of the output object.</typeparam>
        /// <param name="text">The SQL command text or stored procedure name to be executed.</param>
        /// <param name="commandType">Specifies how the command text is interpreted, either as a Text command or as a StoredProcedure.</param>
        /// <param name="input">The input parameter to be used in the SqlCommand setup.</param>
        /// <param name="Setup">An optional action to perform additional setup for the SqlCommand with the input parameter.</param>
        /// <param name="Mapper">A function to map the DbDataReader result set to the output object.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the output object or null if the result set is empty.</returns>
        private async Task<TOutput?> GetReader<TInput, TOutput>(string text,
             CommandType commandType,
             TInput input,
              Action<SqlCommand, TInput>? Setup,
             Func<DbDataReader, TOutput> Mapper,
             CancellationToken cancellationToken)
        {
            await using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            await using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command, input);

                command.CommandType = commandType;
                connection.Open();

                DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
                return Mapper.Invoke(reader);
            }
        }
        private async Task<TOutput?> GetReader<TOutput>(string text,
            CommandType commandType,
             Action<SqlCommand>? Setup,
            Func<DbDataReader, TOutput> Mapper,
            CancellationToken cancellationToken)
        {
            await using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            await using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = commandType;
                connection.Open();

                DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
                return Mapper.Invoke(reader);
            }
        }
        private async Task<TOutput?> GetReader<TOutput>(string text,
            CommandType commandType,
             Action<SqlCommand>? Setup,
            Func<DbDataReader, Task<TOutput>> Mapper,
            CancellationToken cancellationToken)
        {
            await using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            await using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = commandType;
                connection.Open();

                DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
                return await Mapper.Invoke(reader);
            }
        }

        private async Task<TOutput?> GetReader<TInput, TOutput>(string text,
             CommandType commandType,
             TInput input,
              Action<SqlCommand, TInput>? Setup,
             Func<DbDataReader, Task<TOutput>> Mapper,
             CancellationToken cancellationToken)
        {
            await using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            await using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command, input);

                command.CommandType = commandType;
                connection.Open();

                DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);
                return await Mapper.Invoke(reader);
            }
        }
    }
}