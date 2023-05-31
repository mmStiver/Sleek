using Sleek.DataAccess.Core.Command;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.Core
{
    public interface IQueryAsyncDataGateway
    {
        /// <summary>
        /// Executes a Select query asynchronously and returns the scalar result.
        /// </summary>
        /// <param name="query">The Select query object.</param>
        /// <param name="cancellationToken">An optional CancellationToken to cancel the operation.</param>
        /// <returns>A Task containing the scalar result or null if the result set is empty.</returns>
        Task<object?> ExecuteAsync(Select query, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Executes a Select query asynchronously and returns the scalar result.
        /// </summary>
        /// <param name="query">The Select query object.</param>
        /// <param name="Setup">An optional action to perform additional setup for the DbCommand.</param>
        /// <param name="cancellationToken">An optional CancellationToken to cancel the operation.</param>
        /// <returns>A Task containing the scalar result or null if the result set is empty.</returns>
        Task<object?> ExecuteAsync(Select query, Action<DbCommand>? Setup, CancellationToken cancellationToken = default);
        /// <summary>
        /// Executes an asynchronous SELECT query with an input parameter and returns the result as an object.
        /// </summary>
        /// <param name="query">The SELECT query to be executed.</param>
        /// <param name="input">The input parameter to be passed to the query.</param>
        /// <param name="Setup">An optional action to configure the DbCommand and set the input parameter before execution.</param>
        /// <param name="cancellationToken">An optional CancellationToken to allow cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the result as an object, or null if no records were found.</returns>
        Task<object?> ExecuteAsync(Select query, object input, Action<DbCommand, object>? Setup, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes an asynchronous SELECT query and maps the result to a specified type.
        /// </summary>
        /// <typeparam name="TOutput">The type to which the result should be mapped.</typeparam>
        /// <param name="Query">The SELECT query to be executed.</param>
        /// <param name="cancellationToken">An optional CancellationToken to allow cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the mapped result or default value of TOutput if no records were found.</returns>
        Task<TOutput?> ExecuteAsync<TOutput>(Select Query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes an asynchronous SELECT query and maps the result to a specified type using an optional setup action.
        /// </summary>
        /// <typeparam name="TOutput">The type to which the result should be mapped.</typeparam>
        /// <param name="Query">The SELECT query to be executed.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <param name="cancellationToken">An optional CancellationToken to allow cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the mapped result or default value of TOutput if no records were found.</returns>
        Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Action<DbCommand>? Setup, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes an asynchronous SELECT query with a typed input parameter and maps the result to a specified output type.
        /// </summary>
        /// <typeparam name="TInput">The type of the input parameter.</typeparam>
        /// <typeparam name="TOutput">The type to which the result should be mapped.</typeparam>
        /// <param name="Query">The SELECT query to be executed.</param>
        /// <param name="input">The input parameter to be passed to the query.</param>
        /// <param name="Setup">An optional action to configure the DbCommand and set the input parameter before execution.</param>
        /// <param name="cancellationToken">An optional CancellationToken to allow cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the mapped result or default value of TOutput if no records were found.</returns>
        Task<TOutput?> ExecuteAsync<TInput, TOutput>(Select Query, TInput input, Action<DbCommand, TInput>? Setup, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes an asynchronous SELECT query and maps the result to a specified type using a provided mapping function.
        /// </summary>
        /// <typeparam name="TOutput">The type to which the result should be mapped.</typeparam>
        /// <param name="Query">The SELECT query to be executed.</param>
        /// <param name="Mapper">A function to map the result from DbDataReader to the specified output type.</param>
        /// <param name="cancellationToken">An optional CancellationToken to allow cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the mapped result or default value of TOutput if no records were found.</returns>
        Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes an asynchronous SELECT query and maps the result to a specified type using a provided asynchronous mapping function.
        /// </summary>
        /// <typeparam name="TOutput">The type to which the result should be mapped.</typeparam>
        /// <param name="Query">The SELECT query to be executed.</param>
        /// <param name="Mapper">An asynchronous function to map the result from DbDataReader to the specified output type.</param>
        /// <param name="cancellationToken">An optional CancellationToken to allow cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the mapped result or default value of TOutput if no records were found.</returns>
        Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default);
        /// <summary>
        /// Executes an asynchronous SELECT query and maps the result to a specified type using a provided mapping function.
        /// </summary>
        /// <typeparam name="TOutput">The type to which the result should be mapped.</typeparam>
        /// <param name="Query">The SELECT query to be executed.</param>
        /// <param name="Mapper">A function to map the result from DbDataReader to the specified output type.</param>
        /// <param name="cancellationToken">An optional CancellationToken to allow cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the mapped result or default value of TOutput if no records were found.</returns>
        Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Action<DbCommand>? Setup, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default);
        /// <summary>
        /// Executes a Select query asynchronously with a single input parameter and maps the result set to an output object using the provided mapper function.
        /// </summary>
        /// <typeparam name="TInput">The type of the input object.</typeparam>
        /// <typeparam name="TOutput">The type of the output object.</typeparam>
        /// <param name="Query">The Select query object.</param>
        /// <param name="Input">The input object.</param>
        /// <param name="Setup">An optional action to perform additional setup for the DbCommand.</param>
        /// <param name="Mapper">A function to map the DbDataReader result set to the output object.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing the output object or null if the result set is empty.</returns>
        Task<TOutput?> ExecuteAsync<TInput, TOutput>(Select Query, TInput Input, Action<DbCommand, TInput>? Setup, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default);
        /// <summary>
        /// Executes an asynchronous SELECT query and maps the results to a specified type using a provided asynchronous mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type to which the result should be mapped.</typeparam>
        /// <param name="Query">The SELECT query to be executed.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <param name="Mapper">An asynchronous function that maps the DbDataReader to the desired output type.</param>
        /// <param name="cancellationToken">An optional CancellationToken to allow cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the mapped result or default value of TOutput if no records were found.</returns>
        Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Action<DbCommand>? Setup, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default);
        /// <summary>
        /// Executes an asynchronous SELECT query with input parameters and maps the results to a specified type using a provided asynchronous mapper function.
        /// </summary>
        /// <typeparam name="TInput">The type of the input parameter.</typeparam>
        /// <typeparam name="TOutput">The type to which the result should be mapped.</typeparam>
        /// <param name="Query">The SELECT query to be executed.</param>
        /// <param name="input">The input parameter to be passed to the query.</param>
        /// <param name="Setup">An optional action to configure the DbCommand and set the input parameter before execution.</param>
        /// <param name="Mapper">An asynchronous function that maps the DbDataReader to the desired output type.</param>
        /// <param name="cancellationToken">An optional CancellationToken to allow cancellation of the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the mapped result or default value of TOutput if no records were found.</returns>
        Task<TOutput?> ExecuteAsync<TInput, TOutput>(Select Query, TInput input, Action<DbCommand, TInput>? Setup, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default);
        /// <summary>
        /// Executes a Write query (INSERT, UPDATE, DELETE) asynchronously and returns the number of rows affected.
        /// </summary>
        /// <param name="Query">The Write query object.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing the number of rows affected.</returns>
        Task<int> ExecuteAsync(Write Query, CancellationToken cancellationToken = default);
        /// <summary>
        /// Executes a Write query (INSERT, UPDATE, DELETE) asynchronously and returns the number of rows affected.
        /// </summary>
        /// <param name="Query">The Write query object.</param>
        /// <param name="Setup">An optional action to configure the DbCommand and set the input parameter before execution.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing the number of rows affected.</returns>
        Task<int> ExecuteAsync(Write Query, Action<DbCommand>? Setup, CancellationToken cancellationToken = default);
        /// <summary>
        /// Executes a Write query (INSERT, UPDATE, DELETE) asynchronously and returns the number of rows affected.
        /// </summary>
        /// <param name="Query">The Write query object.</param>
        /// <param name="input">The input parameter to be passed to the query.</param>
        /// <param name="Setup">An optional action to configure the DbCommand and set the input parameter before execution.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing the number of rows affected.</returns>
        Task<int> ExecuteAsync(Write Query, object Input, Action<DbCommand, object>? Setup, CancellationToken cancellationToken = default);
        /// <summary>
        /// Executes a Write query (INSERT, UPDATE, DELETE) asynchronously and returns the number of rows affected.
        /// </summary>
        /// <param name="Query">The Write query object.</param>
        /// <param name="input">The input parameter to be passed to the query.</param>
        /// <param name="Setup">An optional action to configure the DbCommand and set the input parameter before execution.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing the number of rows affected.</returns>
        Task<int> ExecuteAsync<TInput>(Write Query, TInput Input, Action<DbCommand, TInput>? Setup, CancellationToken cancellationToken = default);
        /// <summary>
        /// Executes a DataDefinitionQuery (CREATE, ALTER, DROP) asynchronously and returns the number of rows affected.
        /// </summary>
        /// <param name="Query">The DataDefinitionQuery object.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing the number of rows affected.</returns>
        Task<int> ExecuteAsync(DataDefinitionQuery Query, CancellationToken cancellationToken = default);
    }
}

