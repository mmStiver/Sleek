using System.Data.Common;
using System.Data.SqlClient;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sleek.DataAccess.SQLiteTest.CommandTest
{

    public class InsertTests : IClassFixture<SQLiteTestFixture>, IDisposable
    {
        ISQLiteGateway gateway;

        public InsertTests(SQLiteTestFixture context)
        {
            gateway = new SQLiteGateway(context.connection);
            gateway.Execute((DataDefinitionQuery)TestData.CreateIntegerAutoIncrementTable);
        }

        #region Execute
        [Fact]
        public void Execute_InsertIntoAutoIcrementedAsAWriteCommand_ReturnInsertCount()
        {
            var query = (Insert)"""
                INSERT INTO IntegerTestTable (column1, column2)
                VALUES("hello", 3); 
                """;
            object? Inserted = gateway.Execute((Write)query);

            Assert.NotNull(Inserted);
            Assert.IsType<int>(Inserted);
            Assert.Equal(1, Inserted);
        }

        [Fact]
        public void Execute_InsertIntoAutoIcremented_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO IntegerTestTable (column1, column2)
                VALUES("hello", 3); 
                """;
            object? Inserted = gateway.Execute(query);

            Assert.NotNull(Inserted);
            Assert.IsType<long>(Inserted);
            Assert.Equal(1L, Inserted);
        }
        [Fact]       
        public void Execute_SetupInsert_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO IntegerTestTable (column1, column2)
                VALUES(@col1, @col2); 
                """;
            var setup = (DbCommand cmd) =>
            {
                cmd.Parameters.Add(new SQLiteParameter("col1", "hello"));
                cmd.Parameters.Add(new SQLiteParameter("col2", 7));
            };
            object? result = gateway.Execute(query, setup);
            Assert.Equal(1L, result);
        }

        [Fact]
        public void Execute_SetupInsertQueryWithTypedParameter_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO IntegerTestTable (column1, column2)
                VALUES(@col1, @col2); 
                """;
            (string, int) input = ("hello", 6);

            var setup = (DbCommand cmd, object data) =>
            {
                (string, int) local = (((string, int))data);
                cmd.Parameters.Add(new SQLiteParameter("col1", local.Item1));
                cmd.Parameters.Add(new SQLiteParameter("col2", local.Item2));
            };
            object? result = gateway.Execute(query, input, setup);
            ;
            Assert.Equal(1L, result);
        }
        [Fact]
        public void Execute_SetupInsertQueryWithTypedParameter_PassedInputIsFedToSetup()
        {
            var query = (Insert)"""
                INSERT INTO IntegerTestTable (column1, column2)
                VALUES(@col1, @col2); 
                """;
            (string, int) input = ("hello", 6);

            var setup = (DbCommand cmd, (string, int) data) =>
            {
                Assert.Equal(input.Item1, data.Item1);
                cmd.Parameters.Add(new SQLiteParameter("col1", data.Item1));
                Assert.Equal(input.Item2, data.Item2);
                cmd.Parameters.Add(new SQLiteParameter("col2", data.Item2));
            };
            _ = gateway.Execute(query, input, setup);
        }
        #endregion

        #region Execute_T
        [Fact]
        public void Execute_T_InsertIntoAutoIcremented_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO IntegerTestTable (column1, column2)
                VALUES("hello", 3); 
                """;
            long? Inserted = gateway.Execute<long>(query);

            Assert.Equal(1, Inserted);
        }

        [Fact]
        public void Execute_T_SetupInsert_ReturnLastInsetedId()
        {
            var query = (Insert)"""
                INSERT INTO IntegerTestTable (column1, column2)
                VALUES(@col1, @col2); 
                """;
            var setup = (DbCommand cmd) =>
            {
                cmd.Parameters.Add(new SQLiteParameter("col1", "hello"));
                cmd.Parameters.Add(new SQLiteParameter("col2", 7));
            };
            long? result = gateway.Execute<long>(query, setup);
            Assert.Equal(1L, result);
        }

        [Fact]
        public void Execute_T_SetupInsertQueryWithTypedParameter_ReturnLastInsetedId()
        {

            var query = (Insert)"""
                INSERT INTO IntegerTestTable (column1, column2)
                VALUES(@col1, @col2); 
                """;
            (string, int) input = ("hello", 6);

            var setup = (DbCommand cmd, (string, int) data) =>
            {
                cmd.Parameters.Add(new SQLiteParameter("col1", data.Item1));
                cmd.Parameters.Add(new SQLiteParameter("col2", data.Item2));
            };
            long? result = gateway.Execute<(string, int), long>(query, input, setup);
            ;
            Assert.Equal(1L, result);
        }
        [Fact]
        public void Execute_T_SetupInsertQueryWithTypedParameter_PassedInputIsFedToSetup()
        {
            var query = (Insert)"""
                INSERT INTO IntegerTestTable (column1, column2)
                VALUES(@col1, @col2); 
                """;
            (string, int) input = ("hello", 6);
            var setup = (DbCommand cmd, (string, int) data) =>
            {
                Assert.Equal(input.Item1, data.Item1);
                    cmd.Parameters.Add(new SQLiteParameter("col1",  data.Item1));
                Assert.Equal(input.Item2, data.Item2);

                cmd.Parameters.Add(new SQLiteParameter("col2", data.Item2));
                };
            _ = gateway.Execute<(string, int), long>(query, input, setup);
        }
        #endregion

        public void Dispose()
        {
            gateway.Execute((DataDefinitionQuery)"DROP TABLE IntegerTestTable");
        }
    }
}