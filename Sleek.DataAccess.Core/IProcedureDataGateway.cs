using Sleek.DataAccess.Core;
using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.Core
{
    public interface IProcedureDataGateway 
    {
        /// <summary>
        /// Executes a stored procedure and returns the result as an object.
        /// </summary>
        /// <param name="procedure">The stored procedure to execute.</param>
        /// <returns>An object representing the result of the query execution.</returns>
        object? Execute(StoredProcedure procedure);

        /// <summary>
        /// Executes a stored procedure with custom command setup and returns the result as an object.
        /// </summary>
        /// <param name="procedure">The stored procedure to execute.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <returns>An object representing the result of the query execution.</returns>
        object? Execute(StoredProcedure procedure, Action<DbCommand>? Setup);

        /// <summary>
        /// Executes a stored procedure and returns the result as an object of type TOutput.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="procedure">The stored procedure to execute.</param>
        /// <returns>An object of type TOutput representing the result of the query execution.</returns>
        TOutput? Execute<TOutput>(StoredProcedure procedure);

        /// <summary>
        /// Executes a stored procedure with custom command setup and returns the result as an object of type TOutput.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="procedure">The stored procedure to execute.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <returns>An object of type TOutput representing the result of the query execution.</returns>
        TOutput? Execute<TOutput>(StoredProcedure procedure, Action<DbCommand>? Setup);

        /// <summary>
        /// Executes a stored procedure and maps the result using the provided mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="procedure">The stored procedure to execute.</param>
        /// <param name="Mapper">A function to map the DbDataReader to the desired result type.</param>
        /// <returns>An object of type TOutput representing the result of the query execution.</returns>
        TOutput? Execute<TOutput>(StoredProcedure procedure, Func<DbDataReader, TOutput> Mapper);

        /// <summary>
        /// Executes a stored procedure with custom command setup and maps the result using the provided mapper function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the result.</typeparam>
        /// <param name="procedure">The stored procedure to execute.</param>
        /// <param name="Setup">An optional action to configure the DbCommand before execution.</param>
        /// <param name="Mapper">A function to map the DbDataReader to the desired result type.</param>
        /// <returns>An object of type TOutput representing the result of the query execution.</returns>
        TOutput? Execute<TOutput>(StoredProcedure procedure, Action<DbCommand>? Setup, Func<DbDataReader, TOutput> Mapper);
    }
}