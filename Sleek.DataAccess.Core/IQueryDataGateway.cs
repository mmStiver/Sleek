using Sleek.DataAccess.Core;
using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.Core
{
    public interface IQueryDataGateway 
    {
        object? Execute(Select query);
        object? Execute(Select query, Action<SqlCommand>? Setup);
        object? Execute(Select query, object input, Action<SqlCommand, object>? Setup);

        TOutput? Execute<TOutput>(Select query);
        TOutput? Execute<TOutput>(Select query, Action<SqlCommand>? Setup);
        TOutput? Execute<TInput, TOutput>(Select query, TInput Input, Action<SqlCommand, TInput>? Setup);
        TOutput? Execute<TOutput>(Select Query, Func<DbDataReader, TOutput> Mapper);
        TOutput? Execute<TOutput>(Select Query, Action<SqlCommand>? Setup, Func<DbDataReader, TOutput> Mapper);
        TOutput? Execute<TInput, TOutput>(Select Query, TInput Input, Action<SqlCommand, TInput>? Setup, Func<DbDataReader, TOutput> Mapper);

        int Execute(Write query);
        int Execute(Write query, Action<SqlCommand>? Setup);
        int Execute(DataDefinitionQuery query);
    }
}