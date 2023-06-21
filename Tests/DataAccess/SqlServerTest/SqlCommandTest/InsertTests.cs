using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace Sleek.DataAccess.SqlServerTest.DbCommandTest
{
    [Collection("SQL Server Database collection")]

    public class InsertTests : IDisposable
    {
        ISqlServerGateway gateway;


        public InsertTests(SqlServerTestFixture context)
        {
            gateway = new SqlServerGateway(context.connectionString);

        }

        #region ExecuteAsync
        [Fact]
        public void Execute_InsertIntoAutoIcrementedAsAWriteCommand_ReturnInsertCount()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            object? Inserted = gateway.Execute((Write)query);

            Assert.NotNull(Inserted);
            Assert.IsType<int>(Inserted);
            Assert.Equal(1, Inserted);
        }

        [Fact]
        public void Execute_InsertQueryWithoutOutputStatement_ReturnNothing()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt (IsTrue)
                VALUES(1); 
                """;
            object? Inserted = gateway.Execute(query);

            Assert.Null(Inserted);
        }

        [Fact]
        public async Task ExecuteAsync_InsertIntoAutoIcrementedInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            object? Inserted = await gateway.ExecuteAsync(query);

            Assert.NotNull(Inserted);
            Assert.IsType<int>(Inserted);
            Assert.Equal(1, Inserted);
        }
        [Fact]
        public async Task ExecuteAsync_InsertIntoAutoIcrementedBigInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestLong (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            object? Inserted = await gateway.ExecuteAsync(query);
            Assert.NotNull(Inserted);
            Assert.IsType<long>(Inserted);
            Assert.Equal(1L, Inserted);
        }
        [Fact]
        public async Task ExecuteAsync_InsertIntoAutoIcrementedSmallInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestShort (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            object? Inserted = await gateway.ExecuteAsync(query);

            Assert.NotNull(Inserted);
            Assert.IsType<short>(Inserted);
            Assert.Equal((short)1, Inserted);
        }
        [Fact]
        public async Task ExecuteAsync_InsertIntoAutoIcrementedTinyInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestByte (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            object? Inserted = await gateway.ExecuteAsync(query);
            Assert.NotNull(Inserted);
            Assert.IsType<byte>(Inserted);
            Assert.Equal((byte)1, Inserted);
        }
        [Fact]
        public async Task ExecuteAsync_InsertIntoTableWithOutputUUID_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestGuid (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;

            object? Inserted = await gateway.ExecuteAsync(query);

            Assert.NotNull(Inserted);
            Assert.IsType<Guid>(Inserted);
            Assert.NotEqual(Guid.Empty, Inserted);
        }
        [Fact]
        public async Task ExecuteAsync_SetupInsertQuery_ReturnLastInsetedId()
        {

            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(@setupVal); 
                """;
            var setup = (DbCommand cmd) =>
            {
                cmd.Parameters.Add(new SqlParameter("setupVal", true));
            };
            object result = await gateway.ExecuteAsync(query, setup);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task ExecuteAsync_SetupInsertQueryWithParameter_ReturnLastInsetedId()
        {

            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(@setupVal); 
                """;
            var setup = (DbCommand cmd, object data) =>
            {
                cmd.Parameters.Add(new SqlParameter("setupVal", data));
            };
            object result = await gateway.ExecuteAsync(query, true, setup);
            Assert.Equal(1, result);
        }
        [Fact]
        public async Task ExecuteAsync_SetupInsertQueryWithParameter_PassedInputIsFedToSetup()
        {

            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(@setupVal); 
                """;
            var setup = (DbCommand cmd, object data) =>
            {
                Assert.Equal(true, data);
                cmd.Parameters.Add(new SqlParameter("setupVal", data));
            };
            _ = await gateway.ExecuteAsync(query, true, setup);
        }

        #endregion


        #region ExecuteAsync_T
        [Fact]
        public async Task ExecuteAsync_T_InsertIntoAutoIcrementedInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            int? Inserted = await gateway.ExecuteAsync<int>(query);

            Assert.Equal(1, Inserted);
        }
        [Fact]
        public async Task ExecuteAsync_T_InsertIntoAutoIcrementedBigInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestLong (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            Int64? Inserted = await gateway.ExecuteAsync<Int64>(query);

            Assert.Equal(1, Inserted);
        }
        [Fact]
        public async Task ExecuteAsync_T_InsertIntoAutoIcrementedSmallInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestShort (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            Int16? Inserted = await gateway.ExecuteAsync<Int16>(query);

            Assert.True(Inserted.HasValue);
            Assert.Equal((short)1, Inserted.Value);
        }
        [Fact]
        public async Task ExecuteAsync_T_InsertIntoAutoIcrementedTinyInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestByte(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            byte? Inserted = await gateway.ExecuteAsync<byte>(query);
            Assert.True(Inserted.HasValue);
            Assert.Equal((byte)1, Inserted.Value);
        }
        [Fact]
        public async Task ExecuteAsync_T_InsertIntoTableWithOutputUUID_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestGuid (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;

            Guid? Inserted = await gateway.ExecuteAsync<Guid>(query);

            Assert.True(Inserted.HasValue);
            Assert.NotEqual(Guid.Empty, Inserted);
        }

        [Fact]
        public async Task ExecuteAsync_T_SetupInsert_ReturnLastInsetedId()
        {

            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(@setupVal); 
                """;
            var setup = (DbCommand cmd) =>
            {
                cmd.Parameters.Add(new SqlParameter("setupVal", true));
            };
            int? result = await gateway.ExecuteAsync<int>(query, setup);
            Assert.Equal(1, result);
        }

        [Fact]
        public async Task ExecuteAsync_T_SetupInsertQueryWithTypedParameter_ReturnLastInsetedId()
        {

            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(@setupVal); 
                """;
            var setup = (DbCommand cmd, bool data) =>
            {
                cmd.Parameters.Add(new SqlParameter("setupVal", data));
            };
            int? result = await gateway.ExecuteAsync<bool, int>(query, true, setup);
            ;
            Assert.Equal(1, result);
        }
        [Fact]
        public async Task ExecuteAsync_T_SetupInsertQueryWithTypedParameter_PassedInputIsFedToSetup()
        {

            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(@setupVal); 
                """;
            var setup = (DbCommand cmd, bool data) =>
            {
                Assert.True(data);
                cmd.Parameters.Add(new SqlParameter("setupVal", data));
            };
            _ = await gateway.ExecuteAsync<bool, int>(query, true, setup);
        }


        #endregion

        #region Execute
        [Fact]
        public void Execute_InsertIntoAutoIcrementedInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            object? Inserted = gateway.Execute(query);

            Assert.NotNull(Inserted);
            Assert.IsType<int>(Inserted);
            Assert.Equal(1, Inserted);
        }
        [Fact]
        public void Execute_InsertIntoAutoIcrementedBigInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestLong (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            object? Inserted = gateway.Execute(query);
            Assert.NotNull(Inserted);
            Assert.IsType<long>(Inserted);
            Assert.Equal(1L, Inserted);
        }
        [Fact]
        public void Execute_InsertIntoAutoIcrementedSmallInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestShort(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            object? Inserted = gateway.Execute(query);

            Assert.NotNull(Inserted);
            Assert.IsType<short>(Inserted);
            Assert.Equal((short)1, Inserted);
        }
        [Fact]
        public void Execute_InsertIntoAutoIcrementedTinyInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestByte(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            object? Inserted = gateway.Execute(query);
            Assert.NotNull(Inserted);
            Assert.IsType<byte>(Inserted);
            Assert.Equal((byte)1, Inserted);
        }
        [Fact]
        public void Execute_InsertIntoTableWithOutputUUID_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestGuid (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;

            object? Inserted = gateway.Execute(query);

            Assert.NotNull(Inserted);
            Assert.IsType<Guid>(Inserted);
            Assert.NotEqual(Guid.Empty, Inserted);
        }
        [Fact]
        public void Execute_SetupInsertQuery_ReturnLastInsetedId()
        {

            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(@setupVal); 
                """;
            var setup = (DbCommand cmd) =>
            {
                cmd.Parameters.Add(new SqlParameter("setupVal", true));
            };
            object result = gateway.Execute(query, setup);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Execute_SetupInsertQueryWithParameter_ReturnLastInsetedId()
        {

            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(@setupVal); 
                """;
            var setup = (DbCommand cmd, object data) =>
            {
                cmd.Parameters.Add(new SqlParameter("setupVal", data));
            };
            object result = gateway.Execute(query, true, setup);
            Assert.Equal(1, result);
        }
        [Fact]
        public void Execute_SetupInsertQueryWithParameter_PassedInputIsFedToSetup()
        {

            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(@setupVal); 
                """;
            var setup = (DbCommand cmd, object data) =>
            {
                Assert.Equal(true, data);
                cmd.Parameters.Add(new SqlParameter("setupVal", data));
            };
            _ = gateway.Execute(query, true, setup);
        }
        #endregion

        #region Execute_T
        [Fact]
        public void Execute_T_InsertIntoAutoIcrementedInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            int? Inserted = gateway.Execute<int>(query);

            Assert.Equal(1, Inserted);
        }
        [Fact]
        public void Execute_T_InsertIntoAutoIcrementedBigInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestLong (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            Int64? Inserted = gateway.Execute<Int64>(query);

            Assert.Equal(1, Inserted);
        }
        [Fact]
        public void Execute_T_InsertIntoAutoIcrementedSmallInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestShort(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            Int16? Inserted = gateway.Execute<Int16>(query);

            Assert.True(Inserted.HasValue);
            Assert.Equal(1, Inserted.Value);
        }
        [Fact]
        public void Execute_T_InsertIntoAutoIcrementedTinyInt_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestByte(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;
            byte? Inserted = gateway.Execute<byte>(query);
            Assert.True(Inserted.HasValue);
            Assert.Equal(1, Inserted.Value);
        }
        [Fact]
        public void Execute_T_InsertIntoTableWithOutputUUID_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO InsertCmdTestGuid (IsTrue)
                OUTPUT Inserted.MyId
                VALUES(1); 
                """;

            Guid? Inserted = gateway.Execute<Guid>(query);

            Assert.True(Inserted.HasValue);
            Assert.NotEqual(Guid.Empty, Inserted);
        }
        [Fact]
        public void Execute_T_SetupInsert_ReturnLastInsetedId()
        {

            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(@setupVal); 
                """;
            var setup = (DbCommand cmd) =>
            {
                cmd.Parameters.Add(new SqlParameter("setupVal", true));
            };
            int? result = gateway.Execute<int>(query, setup);
            Assert.Equal(1, result);
        }

        [Fact]
        public void Execute_T_SetupInsertQueryWithTypedParameter_ReturnLastInsetedId()
        {

            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(@setupVal); 
                """;
            var setup = (DbCommand cmd, bool data) =>
            {
                cmd.Parameters.Add(new SqlParameter("setupVal", data));
            };
            int? result = gateway.Execute<bool, int>(query, true, setup);
            ;
            Assert.Equal(1, result);
        }
        [Fact]
        public void Execute_T_SetupInsertQueryWithTypedParameter_PassedInputIsFedToSetup()
        {

            var query = (Insert)"""
                INSERT INTO InsertCmdTestInt(IsTrue)
                OUTPUT Inserted.MyId
                VALUES(@setupVal); 
                """;
            var setup = (DbCommand cmd, bool data) =>
            {
                Assert.True(data);
                cmd.Parameters.Add(new SqlParameter("setupVal", data));
            };
            _ = gateway.Execute<bool, int>(query, true, setup);
        }
        #endregion

        public void Dispose()
        {
            gateway.Execute((DataDefinitionQuery)"TRUNCATE TABLE InsertCmdTestInt");
            gateway.Execute((DataDefinitionQuery)"TRUNCATE TABLE InsertCmdTestLong");
            gateway.Execute((DataDefinitionQuery)"TRUNCATE TABLE InsertCmdTestShort");
            gateway.Execute((DataDefinitionQuery)"TRUNCATE TABLE InsertCmdTestByte");
            gateway.Execute((DataDefinitionQuery)"TRUNCATE TABLE InsertCmdTestGuid");
        }
    }
}