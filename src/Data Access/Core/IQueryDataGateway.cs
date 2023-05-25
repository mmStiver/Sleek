using Sleek.DataAccess.Core;
using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.Core
{
    public interface IQueryDataGateway 
    {
        /// <summary>
        /// Executes a SELECT query and returns the first column of the first row in the result set, or a DBNull if no rows are found.
        /// </summary>
        /// <param name="query">The SELECT query to be executed.</param>
        /// <returns>An object representing the first column of the first row in the result set, or a DBNull if no rows are found.</returns>
        object? Execute(Select query);

        /// <summary>
        /// Executes a SELECT query with an optional setup action to configure the DbCommand and returns the first column of the first row in the result set, or a DBNull if no rows are found.
        /// </summary>
        /// <param name="query">The SELECT query to be executed.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <returns>An object representing the first column of the first row in the result set, or a DBNull if no rows are found.</returns>
        object? Execute(Select query, Action<DbCommand>? Setup);

        /// <summary>
        /// Executes a SELECT query with an input parameter and an optional setup action to configure the DbCommand and set the input parameter, and returns the first column of the first row in the result set, or a DBNull if no rows are found.
        /// </summary>
        /// <param name="query">The SELECT query to be executed.</param>
        /// <param name="input">The input parameter to be passed to the query.</param>
        /// <param name="Setup">An optional action to configure the DbCommand and set the input parameter before execution.</param>
        /// <returns>An object representing the first column of the first row in the result set, or a DBNull if no rows are found.</returns>
        object? Execute(Select query, object input, Action<DbCommand, object>? Setup);


        /// <summary>
        /// Executes a SELECT query and returns a single result of type TOutput.
        /// </summary>
        /// <typeparam name="TOutput">The type of the expected result.</typeparam>
        /// <param name="query">The SELECT query to be executed.</param>
        /// <returns>A single result of type TOutput or default(TOutput) if no result is found.</returns>
        TOutput? Execute<TOutput>(Select query);

        /// <summary>
        /// Executes a SELECT query with an optional setup action to configure the DbCommand and returns a single result of type TOutput.
        /// </summary>
        /// <typeparam name="TOutput">The type of the expected result.</typeparam>
        /// <param name="query">The SELECT query to be executed.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <returns>A single result of type TOutput or default(TOutput) if no result is found.</returns>
        TOutput? Execute<TOutput>(Select query, Action<DbCommand>? Setup);

        /// <summary>
        /// Executes a SELECT query with input parameters of type TInput and an optional setup action to configure the DbCommand, and returns a single result of type TOutput.
        /// </summary>
        /// <typeparam name="TInput">The type of the input parameters.</typeparam>
        /// <typeparam name="TOutput">The type of the expected result.</typeparam>
        /// <param name="query">The SELECT query to be executed.</param>
        /// <param name="Input">The input parameters of type TInput.</param>
        /// <param name="Setup">An optional action to configure the DbCommand with the input parameters before execution.</param>
        /// <returns>A single result of type TOutput or default(TOutput) if no result is found.</returns>
        TOutput? Execute<TInput, TOutput>(Select query, TInput Input, Action<DbCommand, TInput>? Setup);

        /// <summary>
        /// Executes a SELECT query and maps the result to an object of type TOutput using a provided Mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the expected result.</typeparam>
        /// <param name="Query">The SELECT query to be executed.</param>
        /// <param name="Mapper">The function to map the DbDataReader result to an object of type TOutput.</param>
        /// <returns>A single result of type TOutput or default(TOutput) if no result is found.</returns>
        TOutput? Execute<TOutput>(Select Query, Func<DbDataReader, TOutput> Mapper);

        /// <summary>
        /// Executes a SELECT query with an optional setup action to configure the DbCommand, and maps the result to an object of type TOutput using a provided Mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the expected result.</typeparam>
        /// <param name="Query">The SELECT query to be executed.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <param name="Mapper">The function to map the DbDataReader result to an object of type TOutput.</param>
        /// <returns>A single result of type TOutput or default(TOutput) if no result is found.</returns>
        TOutput? Execute<TOutput>(Select Query, Action<DbCommand>? Setup, Func<DbDataReader, TOutput> Mapper);

        /// <summary>
        /// Executes a SELECT query with input parameters of type TInput and an optional setup action to configure the DbCommand, and maps the result to an object of type TOutput using a provided Mapper function.
        /// </summary>
        /// <typeparam name="TInput">The type of the input parameters.</typeparam>
        /// <typeparam name="TOutput">The type of the expected result.</typeparam>
        /// <param name="Query">The SELECT query to be executed.</param>
        /// <param name="Input">The input parameters of type TInput.</param>
        /// <param name="Setup">An optional action to configure the DbCommand with the input parameters before execution.</param>
        /// <param name="Mapper">The function to map the DbDataReader result to an object of type TOutput.</param>
        /// <returns>A single result of type TOutput or default(TOutput) if no result is found.</returns>
        TOutput? Execute<TInput, TOutput>(Select Query, TInput Input, Action<DbCommand, TInput>? Setup, Func<DbDataReader, TOutput> Mapper);


        /// <summary>
        /// Executes a write (INSERT, UPDATE, DELETE) query and returns the number of rows affected.
        /// </summary>
        /// <param name="query">The write query to be executed.</param>
        /// <returns>The number of rows affected by the query execution.</returns>
        int Execute(Write query);

        /// <summary>
        /// Executes a write (INSERT, UPDATE, DELETE) query with an optional setup action to configure the DbCommand and returns the number of rows affected.
        /// </summary>
        /// <param name="query">The write query to be executed.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <returns>The number of rows affected by the query execution.</returns>
        int Execute(Write query, Action<DbCommand>? Setup);

        /// <summary>
        /// Executes a Data Definition Language (DDL) query, such as CREATE TABLE or ALTER TABLE, and returns the number of rows affected.
        /// </summary>
        /// <param name="query">The DDL query to be executed.</param>
        /// <returns>The number of rows affected by the query execution.</returns>
        int Execute(DataDefinitionQuery query);

    }
}