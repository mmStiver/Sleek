using Sleek.DataAccess.Core.Command;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.Core
{
    public interface IProcedureAsyncDataGateway
    {
        Task<object?> ExecuteAsync(StoredProcedure Query, CancellationToken cancellationToken = default);
        Task<object?> ExecuteAsync(StoredProcedure Query, Action<SqlCommand>? Setup, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, Action<SqlCommand>? Setup, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, Action<SqlCommand>? Setup, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TOutput>(StoredProcedure Query, Action<SqlCommand>? Setup, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default);
      
    }
}

