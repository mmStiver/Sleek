using Sleek.DataAccess.Core;
using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.SqlServer
{
    public class SqlServerGateway :  ISqlServerGateway
    {

        private readonly string connectionConfiguration;

        public SqlServerGateway(string connectionString)
        {
            connectionConfiguration = connectionString;
        }

        #region IAsyncDataGateway
        //query scalar
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
        public async Task<int> ExecuteAsync(Write Query, CancellationToken cancellationToken = default)
            => await ExecuteAsync(Query, null, cancellationToken);
        public async Task<int> ExecuteAsync(Write Query, Action<SqlCommand>? Setup, CancellationToken cancellationToken = default)
            => await Post(Query.Text, CommandType.Text, Setup, cancellationToken);

        public async Task<int> ExecuteAsync(DataDefinitionQuery Query, CancellationToken cancellationToken = default)
        {
            return await Post(Query.Text,CommandType.Text, null, cancellationToken);
        }
      
        #endregion 

        #region ISyncDataGateway

        //--sync
        public object? Execute(Select query)
            => Execute(query, null);
        public object? Execute(Select query, Action<SqlCommand>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
                using (SqlCommand command = new SqlCommand(query.Text, connection))
                {
                    if(Setup != null)
                        Setup.Invoke(command);
                    command.CommandType = CommandType.Text;
                    connection.Open();

                    var scalar = command.ExecuteScalar();
                    return (scalar is not DBNull)
                        ? scalar
                        : null;
                }
        }
        public object? Execute(Select query, object input, Action<SqlCommand, object>? Setup)
            => GetSingle<object>(query.Text, CommandType.Text, input, Setup);
        public object? Execute(StoredProcedure procedure)
            => Execute(procedure, null);
        public object? Execute(StoredProcedure procedure, Action<SqlCommand>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (SqlCommand command = new SqlCommand(procedure.Name, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = CommandType.StoredProcedure;
                connection.Open();

                var scalar = command.ExecuteScalar();
                return (scalar is not DBNull)
                        ? scalar
                        : null;
            }
        }

        public TOutput? Execute<TOutput>(Select query)
            => Execute<TOutput>(query, (Action<SqlCommand>?)null);
        public TOutput? Execute<TOutput>(Select query, Action<SqlCommand>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
                using (SqlCommand command = new SqlCommand(query.Text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = CommandType.Text;
                connection.Open();
                var res = command.ExecuteScalar();
                return (res is not DBNull)
                    ? (TOutput)res
                    : default(TOutput?);
            }
        }
        public TOutput? Execute<TInput, TOutput>(Select query, TInput Input, Action<SqlCommand, TInput>? Setup)
            => GetSingle<TInput, TOutput>(query.Text, CommandType.Text, Input, Setup);
        public TOutput? Execute<TOutput>(Select Query, Func<DbDataReader, TOutput> Mapper)
            => Execute<TOutput>(Query, null, Mapper);
        public TOutput? Execute<TOutput>(Select Query, Action<SqlCommand>? Setup, Func<DbDataReader, TOutput> Mapper)
            => GetReader<TOutput>(Query.Text, CommandType.Text, Setup, Mapper);

        public TOutput? Execute<TOutput>(StoredProcedure procedure) 
            => Execute<TOutput>(procedure, (Action<SqlCommand>?)null);
        public TOutput? Execute<TOutput>(StoredProcedure procedure, Action<SqlCommand>? Setup)
            => GetSingle<TOutput>(procedure.Name, CommandType.StoredProcedure, Setup);

        public TOutput? Execute<TOutput>(StoredProcedure procedure, Func<DbDataReader, TOutput> Mapper)
            => Execute(procedure, null, Mapper);
        public TOutput? Execute<TOutput>(StoredProcedure procedure, Action<SqlCommand>? Setup, Func<DbDataReader, TOutput> Mapper)
            => GetReader<TOutput>(procedure.Name, CommandType.StoredProcedure, Setup, Mapper);

        public int Execute(Write Query)
            => Execute(Query, null);
        public int Execute(Write Query, Action<SqlCommand>? Setup)
           => Post(Query.Text, CommandType.Text, Setup);

        public int Execute(DataDefinitionQuery Query)
        {
            using (SqlConnection connection = new(this.connectionConfiguration))
            using (SqlCommand command = new SqlCommand(Query.Text, connection))
            {
                command.CommandType = CommandType.Text;
                connection.Open();

                return command.ExecuteNonQuery();
            }
        }
        #endregion

        #region TestConnection
        public async Task<bool> TestConnectionAsync(CancellationToken cancellationToken)
        {
            try
            {
                await using (var connection = new SqlConnection(this.connectionConfiguration))
                {
                    connection.Open();

                    await using (var command = new SqlCommand("SELECT 1;", connection))
                    {
                        await command.ExecuteScalarAsync(cancellationToken);
                    }
                }
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }

        public bool TestConnection()
        {
            try
            {
                using (var connection = new SqlConnection(this.connectionConfiguration))
                {
                    connection.Open();

                    using (var command = new SqlCommand("SELECT 1;", connection))
                    {
                        command.ExecuteScalar();
                    }
                }
                return true;
            }
            catch (SqlException)
            {
                return false;
            }
        }
        #endregion

        //Sync
        private int Post(string text, CommandType commandType, Action<SqlCommand>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = commandType;
                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
        private TOutput? GetSingle<TOutput>(string text,
           CommandType commandType,
           Action<SqlCommand>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
                using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = commandType;
                connection.Open();

                var scalar = command.ExecuteScalar();
                return (scalar is not DBNull)
                    ? (TOutput)scalar
                    : default(TOutput?); ;
            }
        }
        private TOutput? GetSingle<TOutput>(string text,
           CommandType commandType,
           object input,
           Action<SqlCommand, object>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command, input);

                command.CommandType = commandType;
                connection.Open();

                var scalar = command.ExecuteScalar();
                return (scalar is not DBNull)
                    ? (TOutput)scalar
                    : default(TOutput?); ;
            }
        }
        private TOutput? GetSingle<Tinput, TOutput>(string text,
           CommandType commandType,
           Tinput input,
           Action<SqlCommand, Tinput>? Setup)
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command, input);

                command.CommandType = commandType;
                connection.Open();

                var scalar = command.ExecuteScalar();
                return (scalar is not DBNull)
                    ? (TOutput)scalar
                    : default(TOutput?); ;
            }
        }
        private TOutput? GetReader<TOutput>(string text,
          CommandType commandType,
           Action<SqlCommand>? Setup,
          Func<DbDataReader, TOutput> Mapper
            )
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command);

                command.CommandType = commandType;
                connection.Open();

                DbDataReader reader = command.ExecuteReader();
                return Mapper.Invoke(reader);
            }
        }
        private TOutput? GetReader<TInput, TOutput>(string text,
          CommandType commandType,
          TInput input,
           Action<SqlCommand, TInput>? Setup,
          Func<DbDataReader, TOutput> Mapper
            )
        {
            using (SqlConnection connection = new SqlConnection(connectionConfiguration))
            using (SqlCommand command = new SqlCommand(text, connection))
            {
                if (Setup != null)
                    Setup.Invoke(command, input);

                command.CommandType = commandType;
                connection.Open();

                DbDataReader reader = command.ExecuteReader();
                return Mapper.Invoke(reader);
            }
        }

        //Async
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

       //public async Task<object?> ExecuteAsync(Select query, object? input, Action<SqlCommand, object?>? Setup, CancellationToken cancellationToken = default)
       //    => await GetSingle<TInput, TOutput>(Query.Text, CommandType.Text, Input, Setup);


        public TOutput? Execute<TInput, TOutput>(Select Query, TInput Input, Action<SqlCommand, TInput>? Setup, Func<DbDataReader, TOutput> Mapper)
       => GetReader<TInput, TOutput>(Query.Text, CommandType.Text, Input, Setup, Mapper);

        

      

        
    }
}