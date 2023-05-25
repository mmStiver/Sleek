using System.Data.Common;

namespace Sleek.DataAcess.SQLiteTest.CommandTest
{
    public class SelectScalarResults : IClassFixture<SQLiteTestFixture>
    {

        ISQLiteGateway facade;
        public SelectScalarResults(SQLiteTestFixture fixture) {
            facade = new SQLiteGateway(fixture.connection);
        }

        #region Execute
        [Fact]
        public void Execute_ExecuteScalarOnMultipleColumns_ReturnsOnlyFirstResult()
        {
            var query = new Select() { Text = "SELECT 1, null, 'non';" };
            object? result = facade.Execute(query);
            Assert.IsType<Int64>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarFilterSalaryAndCountResults_ReturnsCountOfFoundRecords()
        {
            var query = new Select() { Text = $"SELECT COUNT(*) FROM {TestData.TestTableName} WHERE Salary >= @minSalary;" };
            Action<DbCommand>? setup = (Command) => {
                Command.Parameters.Add(new SQLiteParameter("@minSalary", (Decimal)62000));
            };
            object? result = facade.Execute(query, setup);
            Assert.Equal((long)3, result);
        }

        [Fact]
        public void Execute_ExecuteScalarFilterSalaryByPassedValue_ReturnsCountOfFoundRecords()
        {
            Decimal salary = 62000;
            var query = new Select() { Text = $"SELECT COUNT(*) FROM {TestData.TestTableName} WHERE Salary >= @minSalary;" };
            Action<DbCommand, object>? setup = (Command, minSalary) => {
                Command.Parameters.Add(new SQLiteParameter("@minSalary", (Decimal)minSalary));
            };
            object? result = facade.Execute(query, salary, setup);
            Assert.Equal((long)3, result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsNull()
        {
            var query = new Select() { Text = "SELECT null;" };
            object? result = facade.Execute(query);
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsIntegerAsLongValue()
        {
            var query = new Select() { Text = "SELECT CAST(42 AS int);" };
            object? result = facade.Execute(query);
            Assert.IsType<Int64>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsRealAsdoubleValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS real);" };
            object? result = facade.Execute(query);
            Assert.IsType<double>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsNumericAsDoubleValue()
        {
            var query = new Select() { Text = "SELECT CAST(75.5 AS NUMERIC(5, 2));" };
            object? result = facade.Execute(query);
            Assert.IsType<double>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsTextValue()
        {
            var query = new Select() { Text = "SELECT CAST('Hello' AS nvarchar(10));" };
            object? result = facade.Execute(query);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsBLOBValue()
        {
            var query = new Select() { Text = "SELECT CAST(x'0011223344556677' AS BLOB);" };
            object? result = facade.Execute(query);
            Assert.IsType<byte[]>(result);
        }

        #endregion

        #region Execute_T
        [Fact]
        public void Execute_T_ExecuteScalarOnMultipleColumns_ReturnsOnlyFirstResult()
        {
            var query = new Select() { Text = "SELECT 1, null, 'non';" };
            object? result = facade.Execute<long>(query);
            Assert.IsType<long>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarFilterSalaryAndCountResults_ReturnsCountOfFoundRecords()
        {
            var query = new Select() { Text = $"SELECT COUNT(*) FROM {TestData.TestTableName} WHERE Salary >= @minSalary;" };
            Action<DbCommand>? setup = (Command) => {
                Command.Parameters.Add(new SQLiteParameter("@minSalary", 62000));
            };
            object? result = facade.Execute<long>(query, setup);
            Assert.Equal((long)3, result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsNull()
        {
            var query = new Select() { Text = "SELECT null;" };
            object? result = facade.Execute<object>(query);
            Assert.Null(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsIntValue()
        {
            var query = new Select() { Text = "SELECT CAST(42 AS int);" };
            object? result = facade.Execute<long>(query);
            Assert.IsType<long>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsRealValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS real);" };
            object? result = facade.Execute<double>(query);
            Assert.IsType<double>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsNumericValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS decimal(5, 2));" };
            object? result = facade.Execute<double>(query);
            Assert.IsType<double>(result);
        }

       
        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsTextValue()
        {
            var query = new Select() { Text = "SELECT CAST('Hello' AS nvarchar(10));" };
            object? result = facade.Execute<string>(query);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsBLOBValue()
        {
            var query = new Select() { Text = "SELECT CAST(0x424242 AS blob);" };
            object? result = facade.Execute<Byte[]>(query);
            Assert.IsType<byte[]>(result);
        }

      
        #endregion

        #region ExecuteIO_T
       
        [Fact]
        public void Execute_IO_ExecuteScalarFilterSalaryAndCountResults_ReturnsCountOfFoundRecords()
        {
            var query = new Select() { Text = $"SELECT COUNT(*) FROM {TestData.TestTableName} WHERE Salary >= @minSalary;" };
            Decimal minSalary = 62000;
            Action<DbCommand, Decimal>? setup = (Command, salary) => {
                Command.Parameters.Add(new SQLiteParameter("@minSalary", salary));
            };
            long? result = facade.Execute<Decimal, long>(query, minSalary, setup);
            Assert.Equal(3, result);
        }

        #endregion

    }
}
