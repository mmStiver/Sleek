using Sleek.DataAccess.Core;
using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.SqlServer
{
    public partial class SqlServerGateway : ISqlServerGateway
    {
        #region IAsyncDataGateway
        /// <summary>
        /// Executes a SELECT query asynchronously and returns the first column of the first row in the result set as an object.
        /// </summary>
        /// <param name="query">The SELECT query to be executed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the first column of the first row in the result set or null if the result set is empty.</returns>
        public async Task<object?> ExecuteAsync(Select query, CancellationToken cancellationToken = default)
            => await ExecuteAsync(query, null, cancellationToken).ConfigureAwait(false);
        
        /// <summary>
        /// Executes a SQL SELECT command asynchronously and retrieves a single object value.
        /// </summary>
        /// <param name="query">An instance of the Select class containing the SQL command text</param>
        /// <param name="Setup">An action to perform additional configuration on the SQL command, or null if no additional configuration is needed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the first column of the first row in the result set, or a default value if the result set is empty or the result is of type <see cref="DBNull"/>.</returns>
        public async Task<object?> ExecuteAsync(Select query, Action<DbCommand>? Setup = null, CancellationToken cancellationToken = default)
            => await GetSingle<object?>(query.Text, CommandType.Text, Setup, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a SQL SELECT command asynchronously and retrieves a single object value.
        /// </summary>
        /// <param name="query">An instance of the Select class containing the SQL command text.</param>
        /// <param name="input">The input parameter for the SQL command.</param>
        /// <param name="Setup">An action to perform additional configuration on the SQL command, or null if no additional configuration is needed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the first column of the first row in the result set, or a default value if the result set is empty or the result is of type <see cref="DBNull"/>.</returns>
        public async Task<object?> ExecuteAsync(Select query, object input, Action<DbCommand, object>? Setup, CancellationToken cancellationToken = default)
            => await GetSingle<object, object?>(query.Text, CommandType.Text, input, Setup, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a SQL SELECT command asynchronously and maps the first row of the result to a specified type.
        /// </summary>
        /// <typeparam name="TOutput">The type of object to return.</typeparam>
        /// <param name="query">An instance of the Select class containing the SQL command text.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the first row of the result set, or default if the result set is empty.</returns>
        public async Task<TOutput?> ExecuteAsync<TOutput>(Select query, CancellationToken cancellationToken = default)
            => await ExecuteAsync<TOutput>(query, (Action<DbCommand>?)null, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a SQL SELECT command asynchronously using a setup action and maps the first row of the result to a specified type.
        /// </summary>
        /// <typeparam name="TOutput">The type of object to return.</typeparam>
        /// <param name="query">An instance of the Select class containing the SQL command text.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the first row of the result set, or default if the result set is empty.</returns>
        public async Task<TOutput?> ExecuteAsync<TOutput>(Select query, Action<DbCommand>? Setup, CancellationToken cancellationToken = default)
             => await GetSingle<TOutput>(query.Text, CommandType.Text, Setup, cancellationToken).ConfigureAwait(false);

        //query + reader
        // <summary>
        /// Executes a SQL SELECT command asynchronously and maps the result set using a specified mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of object to return.</typeparam>
        /// <param name="Query">An instance of the Select class containing the SQL command text.</param>
        /// <param name="Mapper">A function to map the result set to an instance of TOutput.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the mapped result set.</returns>
        public async Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default)
            => await ExecuteAsync<TOutput>(Query, null, Mapper, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a SQL SELECT command asynchronously and maps the result set using a specified asynchronous mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of object to return.</typeparam>
        /// <param name="Query">An instance of the Select class containing the SQL command text.</param>
        /// <param name="Mapper">An asynchronous function to map the result set to an instance of TOutput.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the mapped result set.</returns>
        public async Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default)
           => await ExecuteAsync<TOutput>(Query, null, Mapper, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a SQL SELECT command asynchronously and retrieves a single value of type <typeparamref name="TOutput"/>.
        /// </summary>
        /// <typeparam name="TInput">The type of the input parameter.</typeparam>
        /// <typeparam name="TOutput">The type of the returned value.</typeparam>
        /// <param name="Query">An instance of the Select class containing the SQL command text.</param>
        /// <param name="input">The input parameter for the SQL command.</param>
        /// <param name="Setup">An action to perform additional configuration on the SQL command, or null if no additional configuration is needed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the first column of the first row in the result set, or a default value if the result set is empty or the result is of type <see cref="DBNull"/>.</returns>
        public async Task<TOutput?> ExecuteAsync<TInput, TOutput>(Select Query, TInput input, Action<DbCommand, TInput>? Setup, CancellationToken cancellationToken = default)
            => await GetSingle<TInput, TOutput?>(Query.Text, CommandType.Text, input, Setup, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a SQL SELECT command asynchronously using a setup action and maps the result set using a specified mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of object to return.</typeparam>
        /// <param name="Query">An instance of the Select class containing the SQL command text.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <param name="Mapper">A function to map the result set to an instance of TOutput.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the mapped result set.</returns>
        public async Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Action<DbCommand>? Setup, Func<DbDataReader, TOutput>? Mapper, CancellationToken cancellationToken = default)
            => await GetReader<TOutput>(Query.Text, CommandType.Text, Setup, Mapper, cancellationToken);

        /// <summary>
        /// Executes a SQL SELECT command asynchronously using a setup action and maps the result set using a specified asynchronous mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of object to return.</typeparam>
        /// <param name="Query">An instance of the Select class containing the SQL command text.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <param name="Mapper">An asynchronous function to map the result set to an instance of TOutput.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the mapped result set.</returns>
        public async Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Action<DbCommand>? Setup, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default)
            => await GetReader<TOutput>(Query.Text, CommandType.Text, Setup, Mapper, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a SQL SELECT command asynchronously and maps the result to a custom object.
        /// </summary>
        /// <typeparam name="TInput">The type of the input parameter for the SQL command.</typeparam>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="Query">An instance of the Select class containing the SQL command text.</param>
        /// <param name="input">The input parameter for the SQL command.</param>
        /// <param name="Setup">An action to perform additional configuration on the SQL command, or null if no additional configuration is needed.</param>
        /// <param name="Mapper">A function to map the data reader result to a custom object.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the custom object mapped from the data reader result.</returns>
        public async Task<TOutput?> ExecuteAsync<TInput, TOutput>(Select Query, TInput input, Action<DbCommand, TInput>? Setup, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default)
            => await GetReader<TInput, TOutput>(Query.Text, CommandType.Text, input, Setup, Mapper, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a SQL SELECT command asynchronously and maps the result to a custom object.
        /// </summary>
        /// <typeparam name="TInput">The type of the input parameter for the SQL command.</typeparam>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="Query">An instance of the Select class containing the SQL command text.</param>
        /// <param name="input">The input parameter for the SQL command.</param>
        /// <param name="Setup">An action to perform additional configuration on the SQL command, or null if no additional configuration is needed.</param>
        /// <param name="Mapper">An asynchronous function to map the result set to an instance of TOutput.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the custom object mapped from the data reader result.</returns>
        public async Task<TOutput?> ExecuteAsync<TInput, TOutput>(Select Query, TInput input, Action<DbCommand, TInput>? Setup, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default)
                => await GetReader<TInput, TOutput>(Query.Text, CommandType.Text, input, Setup, Mapper, cancellationToken).ConfigureAwait(false);


        //sproc scalar
        /// <summary>
        /// Executes a stored procedure asynchronously using a setup action.
        /// </summary>
        /// <param name="procedure">An instance of the StoredProcedure class containing the name of the stored procedure.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the first column of the first row in the result set returned by the query.</returns>
        public async Task<object?> ExecuteAsync(StoredProcedure procedure, CancellationToken cancellationToken)
            => await ExecuteAsync(procedure, null, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a stored procedure asynchronously using a setup action.
        /// </summary>
        /// <param name="procedure">An instance of the StoredProcedure class containing the name of the stored procedure.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the first column of the first row in the result set returned by the query.</returns>
        public async Task<object?> ExecuteAsync(StoredProcedure procedure, Action<DbCommand>? Setup, CancellationToken cancellationToken)
            => await GetSingle<object>(procedure.Name, CommandType.StoredProcedure, Setup, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a stored procedure asynchronously using a setup action and returns the result as a specified type.
        /// </summary>
        /// <typeparam name="TOutput">The type of object to return.</typeparam>
        /// <param name="procedure">An instance of the StoredProcedure class containing the name of the stored procedure.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the first column of the first row in the result set returned by the query, converted to type TOutput.</returns>
        public async Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure procedure, CancellationToken cancellationToken)
            => await GetSingle<TOutput>(procedure.Name, CommandType.StoredProcedure, null, cancellationToken).ConfigureAwait(false);
        /// <summary>
        /// Executes a stored procedure asynchronously using a setup action and returns the result as a specified type.
        /// </summary>
        /// <typeparam name="TOutput">The type of object to return.</typeparam>
        /// <param name="procedure">An instance of the StoredProcedure class containing the name of the stored procedure.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the first column of the first row in the result set returned by the query, converted to type TOutput.</returns>

        public async Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure procedure, Action<DbCommand>? Setup, CancellationToken cancellationToken = default)
             => await GetSingle<TOutput>(procedure.Name, CommandType.StoredProcedure, Setup, cancellationToken).ConfigureAwait(false);

        //sproc reader
        /// <summary>
        /// Executes a stored procedure asynchronously and maps the result to a specified type using a mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of object to map the result to.</typeparam>
        /// <param name="procedure">An instance of the StoredProcedure class containing the name of the stored procedure.</param>
        /// <param name="Mapper">A function to map the data reader to the type TOutput.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the result of the mapper function.</returns>
        public async Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure procedure, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default)
            => await ExecuteAsync<TOutput>(procedure, null, Mapper, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a stored procedure asynchronously using a setup action and maps the result to a specified type using a mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of object to map the result to.</typeparam>
        /// <param name="procedure">An instance of the StoredProcedure class containing the name of the stored procedure.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <param name="Mapper">A function to map the data reader to the type TOutput.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the result of the mapper function.</returns>
        public async Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure procedure, Action<DbCommand>? Setup, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default)
            => await GetReader<TOutput>(procedure.Name, CommandType.StoredProcedure, Setup, Mapper, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a stored procedure asynchronously and maps the result to a specified type using an asynchronous mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of object to map the result to.</typeparam>
        /// <param name="procedure">An instance of the StoredProcedure class containing the name of the stored procedure.</param>
        /// <param name="Mapper">A function to map the data reader to the type TOutput. This function can run asynchronously.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the result of the mapper function.</returns>
        public async Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure procedure, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default)
            => await ExecuteAsync(procedure, null, Mapper, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a stored procedure asynchronously using a setup action and maps the result to a specified type using an asynchronous mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of object to map the result to.</typeparam>
        /// <param name="Query">An instance of the StoredProcedure class containing the name of the stored procedure.</param>
        /// <param name="Setup">An action to setup the SQL command.</param>
        /// <param name="Mapper">A function to map the data reader to the type TOutput. This function can run asynchronously.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. he task result is the result of the mapper function.</returns>
        public async Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, Action<DbCommand>? Setup, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default)
            => await GetReader(Query.Name, CommandType.StoredProcedure, Setup, Mapper, cancellationToken).ConfigureAwait(false);

        //NonQuery
        /// <summary>
        /// Executes a non-query statement asynchronously and returns the number of rows affected.
        /// </summary>
        /// <param name="Query">The non-query statement to be executed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the number of rows affected.</returns>
        public async Task<int> ExecuteAsync(Write Query, CancellationToken cancellationToken = default)
            => await ExecuteAsync(Query, null, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Executes a SQL WRITE command asynchronously, such as an INSERT, UPDATE, DELETE, or other command that modifies data.
        /// </summary>
        /// <param name="Query">An instance of the Write class containing the SQL command text.</param>
        /// <param name="Setup">An action to perform additional configuration on the SQL command, or null if no additional configuration is needed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the number of rows affected.</returns>
        public async Task<int> ExecuteAsync(Write Query, Action<DbCommand>? Setup, CancellationToken cancellationToken = default)
            => await PostAsync<object>(Query.Text, CommandType.Text, null, Setup, null, cancellationToken).ConfigureAwait(false);
        public Task<int> ExecuteAsync(Write Query, object Input, Action<DbCommand, object?> Setup, CancellationToken cancellationToken = default)
           => PostAsync<object>(Query.Text, CommandType.Text, Input, null, Setup, cancellationToken);

        public Task<int> ExecuteAsync<TInput>(Write Query, TInput Input, Action<DbCommand, TInput?>? Setup, CancellationToken cancellationToken = default)
            => PostAsync<TInput>(Query.Text, CommandType.Text, Input, null, Setup, cancellationToken);

        public async Task<object> ExecuteAsync(Insert Query, CancellationToken cancellationToken = default)
            => await ExecuteAsync(Query, (Action<DbCommand>?)null, cancellationToken);
        public async Task<object?> ExecuteAsync(Insert Query, Action<DbCommand>? ActionSetup, CancellationToken cancellationToken = default)
        => await ExecuteAsync(Query, null, ActionSetup, null, cancellationToken);
        public async Task<object?> ExecuteAsync(Insert Query, object Input,
          Action<DbCommand, object>? ParamSetup,
          CancellationToken cancellationToken = default)
        => await ExecuteAsync(Query, Input, null, ParamSetup, cancellationToken);
        public async Task<TOutput?> ExecuteAsync<TOutput>(Insert Query, Action<DbCommand>? Setup, CancellationToken cancellationToken = default) where TOutput : struct
            =>(TOutput) (await ExecuteAsync(Query, null, Setup, null, cancellationToken));
        
        private async Task<object?> ExecuteAsync(Insert Query, object? Input,
            Action<DbCommand>? ActionSetup,
            Action<DbCommand, object>? ParamSetup,
            CancellationToken cancellationToken = default)
        {
            using (SqlConnection conn = new SqlConnection(this.connectionConfiguration))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand((string)Query, conn))
                {
                    if (ActionSetup != null) ActionSetup.Invoke(cmd);
                    if (ParamSetup != null) ParamSetup.Invoke(cmd, Input);
                    return await cmd.ExecuteScalarAsync(cancellationToken);
                }
            }
        }
     
        public async Task<Nullable<TOutput>> ExecuteAsync<TInput, TOutput>(Insert Query, TInput? Input,
            Action<DbCommand, TInput>? ParamSetup,
            CancellationToken cancellationToken = default)
            where TOutput : struct
        {
            using (SqlConnection conn = new SqlConnection(this.connectionConfiguration))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand((string)Query, conn))
                {
                    if (ParamSetup != null) ParamSetup.Invoke(cmd, Input);
                    var newId =  await cmd.ExecuteScalarAsync(cancellationToken);
                    return (newId != null && newId is not DBNull && newId is TOutput)
                        ? new Nullable<TOutput>((TOutput)Convert.ChangeType(newId, typeof(TOutput)))
                        : null;
                }
            }
        }

        public async Task<Nullable<TOutput>> ExecuteAsync<TOutput>(Insert Query, CancellationToken cancellationToken = default)
            where TOutput : struct
            => await ExecuteAsync<TOutput>(Query, (Action<DbCommand, object> ?)null, cancellationToken);
        public async Task<Nullable<TOutput>> ExecuteAsync<TOutput>(Insert Query, Action<DbCommand, object>? Setup, CancellationToken cancellationToken = default) 
            where TOutput : struct
        {
            using (SqlConnection conn = new SqlConnection(this.connectionConfiguration))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand((string)Query, conn))
                {
                    var newId = await cmd.ExecuteScalarAsync(cancellationToken);                   
                    return (newId !=null &&  newId is not DBNull && newId is TOutput)
                        ? new Nullable<TOutput>((TOutput)Convert.ChangeType(newId, typeof(TOutput)))
                        : null;
                }
            }
        }

        /// <summary>
        /// Executes a SQL Data Definition Language (DDL) command asynchronously, such as CREATE, ALTER, DROP, etc.
        /// </summary>
        /// <param name="Query">An instance of the DataDefinitionQuery class containing the SQL command text.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the number of rows affected.</returns>
        public async Task<int> ExecuteAsync(DataDefinitionQuery Query, CancellationToken cancellationToken = default)
            => await PostAsync<object>(Query.Text, CommandType.Text, null, null, null, cancellationToken).ConfigureAwait(false);
        #endregion

        /// <summary>
        /// Executes a non-query statement asynchronously and returns the number of rows affected.
        /// </summary>
        /// <param name="text">The SQL command text or stored procedure name to be executed.</param>
        /// <param name="commandType">Specifies how the command text is interpreted, either as a Text command or as a StoredProcedure.</param>
        /// <param name="Setup">An optional action to perform additional setup for the DbCommand.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the number of rows affected.</returns>
        private async Task<int> PostAsync<TInput>(string text,
           CommandType commandType,
            TInput? Input,
           Action<DbCommand>? Setup,
            Action<DbCommand, TInput?>? inSetup,
           CancellationToken cancellationToken)
        {
            SqlConnection connection = new SqlConnection(connectionConfiguration);
            await using (connection.ConfigureAwait(false))
            {
                DbCommand command = new SqlCommand(text, connection);
                await using (command.ConfigureAwait(false))
                {
                    if (Setup != null)
                        Setup.Invoke(command);
                    if (inSetup != null)
                        inSetup.Invoke(command, Input);
                    command.CommandType = commandType;
                    connection.Open();
                    return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
                }
            }
        }

        private async Task<TOutput?> GetSingle<TOutput>(string text,
            CommandType commandType,
            Action<DbCommand>? Setup,
            CancellationToken cancellationToken)
        {
            SqlConnection connection = new SqlConnection(connectionConfiguration);
            await using (connection.ConfigureAwait(false))
            {
                DbCommand command = new SqlCommand(text, connection);
                await using (command.ConfigureAwait(false))
                {
                    if (Setup != null)
                        Setup.Invoke(command);

                    command.CommandType = commandType;
                    connection.Open();

                    var scalar = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
                    return (scalar is not DBNull)
                        ? (TOutput)scalar
                        : default(TOutput?);
                }
            }
        }

        /// <summary>
        /// Executes a SQL command asynchronously and returns a single value of type <typeparamref name="TOutput"/>.
        /// </summary>
        /// <typeparam name="TInput">The type of the input parameter.</typeparam>
        /// <typeparam name="TOutput">The type of the returned value.</typeparam>
        /// <param name="text">The SQL command text.</param>
        /// <param name="commandType">The type of SQL command to create.</param>
        /// <param name="input">The input parameter for the SQL command.</param>
        /// <param name="Setup">An action to perform additional configuration on the SQL command, or null if no additional configuration is needed.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the first column of the first row in the result set, or a default
        private async Task<TOutput?> GetSingle<TInput, TOutput>(string text,
           CommandType commandType,
           TInput input,
           Action<DbCommand, TInput>? Setup,
           CancellationToken cancellationToken)
        {
            SqlConnection connection = new SqlConnection(connectionConfiguration);
            await using (connection.ConfigureAwait(false))
            {
                DbCommand command = new SqlCommand(text, connection);
                await using (command.ConfigureAwait(false))
                {
                    if (Setup != null)
                        Setup.Invoke(command, input);

                    command.CommandType = commandType;
                    connection.Open();

                    var scalar = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
                    return (scalar is not DBNull)
                        ? (TOutput)scalar
                        : default(TOutput?); ;
                }
            }
        }

        /// <summary>
        /// Executes a query asynchronously and maps the result set to a single output using the provided mapper function.
        /// </summary>
        /// <typeparam name="TInput">The type of the input parameter.</typeparam>
        /// <typeparam name="TOutput">The type of the output object.</typeparam>
        /// <param name="text">The SQL command text or stored procedure name to be executed.</param>
        /// <param name="commandType">Specifies how the command text is interpreted, either as a Text command or as a StoredProcedure.</param>
        /// <param name="input">The input parameter to be used in the DbCommand setup.</param>
        /// <param name="Setup">An optional action to perform additional setup for the DbCommand with the input parameter.</param>
        /// <param name="Mapper">A function to map the DbDataReader result set to the output object.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the output object or null if the result set is empty.</returns>
        private async Task<TOutput?> GetReader<TInput, TOutput>(string text,
             CommandType commandType,
             TInput input,
              Action<DbCommand, TInput>? Setup,
             Func<DbDataReader, TOutput> Mapper,
             CancellationToken cancellationToken)
        {
            SqlConnection connection = new SqlConnection(connectionConfiguration);
            await using (connection.ConfigureAwait(false))
            {
                DbCommand command = new SqlCommand(text, connection);
                await using (command.ConfigureAwait(false))
                {
                    if (Setup != null)
                        Setup.Invoke(command, input);

                    command.CommandType = commandType;
                    connection.Open();

                    DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                    return Mapper.Invoke(reader);
                }
            }
        }
        private async Task<TOutput?> GetReader<TOutput>(string text,
            CommandType commandType,
             Action<DbCommand>? Setup,
            Func<DbDataReader, TOutput> Mapper,
            CancellationToken cancellationToken)
        {
            SqlConnection connection = new SqlConnection(connectionConfiguration);
            await using (connection.ConfigureAwait(false))
            {
                DbCommand command = new SqlCommand(text, connection);
                await using (command.ConfigureAwait(false))
                {
                    if (Setup != null)
                        Setup.Invoke(command);

                    command.CommandType = commandType;
                    connection.Open();

                    DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                    return Mapper.Invoke(reader);
                }
            }
        }
        private async Task<TOutput?> GetReader<TOutput>(string text,
            CommandType commandType,
             Action<DbCommand>? Setup,
            Func<DbDataReader, Task<TOutput>> Mapper,
            CancellationToken cancellationToken)
        {
            SqlConnection connection = new SqlConnection(connectionConfiguration);
            await using (connection.ConfigureAwait(false))
            {
                DbCommand command = new SqlCommand(text, connection);
                await using (command.ConfigureAwait(false))
                {
                    if (Setup != null)
                        Setup.Invoke(command);

                    command.CommandType = commandType;
                    connection.Open();

                    DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                    return await Mapper.Invoke(reader);
                }
            }
        }

        private async Task<TOutput?> GetReader<TInput, TOutput>(string text,
             CommandType commandType,
             TInput input,
              Action<DbCommand, TInput>? Setup,
             Func<DbDataReader, Task<TOutput>> Mapper,
             CancellationToken cancellationToken)
        {
            SqlConnection connection = new SqlConnection(connectionConfiguration);
            await using (connection.ConfigureAwait(false))
            {
                DbCommand command = new SqlCommand(text, connection);
                await using (command.ConfigureAwait(false))
                {
                    if (Setup != null)
                        Setup.Invoke(command, input);

                    command.CommandType = commandType;
                    connection.Open();

                    DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                    return await Mapper.Invoke(reader).ConfigureAwait(false);
                }
            }
        }

    }
}