using Sleek.DataAccess.Core.Command;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.Core
{
    public interface IQueryAsyncDataGateway
    {
        Task<object?> ExecuteAsync(Select query, CancellationToken cancellationToken = default);
        Task<object?> ExecuteAsync(Select query, Action<SqlCommand>? Setup, CancellationToken cancellationToken = default);
        Task<object?> ExecuteAsync(Select query, object input, Action<SqlCommand, object>? Setup, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TOutput>(Select Query, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Action<SqlCommand>? Setup, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TInput, TOutput>(Select Query, TInput input, Action<SqlCommand, TInput>? Setup, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Action<SqlCommand>? Setup, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TInput, TOutput>(Select Query, TInput input, Action<SqlCommand, TInput>? Setup, Func<DbDataReader, TOutput> Mapper, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TOutput>(Select Query, Action<SqlCommand>? Setup, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default);
        Task<TOutput?> ExecuteAsync<TInput, TOutput>(Select Query, TInput input, Action<SqlCommand, TInput>? Setup, Func<DbDataReader, Task<TOutput>> Mapper, CancellationToken cancellationToken = default);
        Task<int> ExecuteAsync(Write Query, CancellationToken cancellationToken = default);
        Task<int> ExecuteAsync(Write Query, Action<SqlCommand>? Setup, CancellationToken cancellationToken = default);
        Task<int> ExecuteAsync(DataDefinitionQuery Query, CancellationToken cancellationToken = default);
    }
}

