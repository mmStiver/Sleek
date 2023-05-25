using Sleek.DataAccess.Core.Command;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.Core
{
    public interface IProcedureAsyncDataGateway
    {
        /// <summary>
        /// Executes a stored procedure asynchronously and returns a single scalar value.
        /// </summary>
        /// <param name="Query">The stored procedure to execute.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing a single object as the result of the query execution.</returns>
        Task<object?> ExecuteAsync(StoredProcedure Query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a stored procedure asynchronously with custom command setup and returns a single scalar value.
        /// </summary>
        /// <param name="Query">The stored procedure to execute.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing a single object as the result of the query execution.</returns>
        Task<object?> ExecuteAsync(StoredProcedure Query, Action<DbCommand>? Setup, CancellationToken cancellationToken = default);

        // <summary>
        /// Executes a stored procedure asynchronously and returns a single scalar value of the specified type.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="Query">The stored procedure to execute.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing a single object of type TOutput as the result of the query execution.</returns>
        Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a stored procedure asynchronously with custom command setup and returns a single scalar value of the specified type.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="Query">The stored procedure to execute.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing a single object of type TOutput as the result of the query execution.</returns>
        Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, Action<DbCommand>? Setup, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a stored procedure asynchronously and maps the result using the provided mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="Query">The stored procedure to execute.</param>
        /// <param name="Mapper">A function to map the DbDataReader to the desired result type.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing a single object of type TOutput as the result of the query execution.</returns>
        Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a stored procedure asynchronously with custom command setup and maps the result using the provided mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="Query">The stored procedure to execute.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <param name="Mapper">A function to map the DbDataReader to the desired result type.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing a single object of type TOutput as/// the result of the query execution.</returns>
        Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, Action<DbCommand>? Setup, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a stored procedure asynchronously and maps the result using the provided asynchronous mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="Query">The stored procedure to execute.</param>
        /// <param name="Mapper">An asynchronous function to map the DbDataReader to the desired result type.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing a single object of type TOutput as the result of the query execution.</returns>
        Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default);

        /// <summary>
        /// Executes a stored procedure asynchronously with custom command setup and maps the result using the provided asynchronous mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="Query">The stored procedure to execute.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <param name="Mapper">An asynchronous function to map the DbDataReader to the desired result type.</param>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing a single object of type TOutput as the result of the query execution.</returns>
        Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, Action<DbCommand>? Setup, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default);

    }
}

