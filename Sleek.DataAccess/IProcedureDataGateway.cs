using Sleek.DataAccess.Core;
using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.Core
{
    public interface IProcedureDataGateway 
    {
        object? Execute(StoredProcedure procedure);
        object? Execute(StoredProcedure procedure, Action<SqlCommand>? Setup);
        TOutput? Execute<TOutput>(StoredProcedure procedure);
        TOutput? Execute<TOutput>(StoredProcedure procedure, Action<SqlCommand>? Setup);
        TOutput? Execute<TOutput>(StoredProcedure procedure, Func<DbDataReader, TOutput> Mapper);
        TOutput? Execute<TOutput>(StoredProcedure procedure, Action<SqlCommand>? Setup, Func<DbDataReader, TOutput> Mapper);
    }
}