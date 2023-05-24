﻿using System.Data.Common;

namespace Sleek.DataAcess.SqlServerTest.DbCommandTest
{
    public class SelectScalarResults  
    {

        ISQLiteGateway facade;
        public SelectScalarResults() {
            facade = new SQLiteGateway(TestData.localConnection);
        }

        #region Execute
        [Fact]
        public void Execute_ExecuteScalarOnMultipleColumns_ReturnsOnlyFirstResult()
        {
            var query = new Select() { Text = "SELECT 1, null, 'non';" };
            object? result = facade.Execute(query);
            Assert.IsType<int>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarFilterSalaryAndCountResults_ReturnsCountOfFoundRecords()
        {
            var query = new Select() { Text = $"SELECT COUNT(*) FROM {TestData.TestTableName} WHERE Salary >= @minSalary;" };
            Action<DbCommand>? setup = (Command) => {
                Command.Parameters.Add(new SQLiteParameter("@minSalary", 62000));
            };
            object? result = facade.Execute(query, setup);
            Assert.Equal(3, result);
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
            Assert.Equal(3, result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsNull()
        {
            var query = new Select() { Text = "SELECT null;" };
            object? result = facade.Execute(query);
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsBitValue()
        {
            var query = new Select() { Text = "SELECT CAST(1 AS bit);" };
            object? result =  facade.Execute(query);
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsTinyIntValue()
        {
            var query = new Select() { Text = "SELECT CAST(42 AS tinyint);" };
            object? result =  facade.Execute(query);
            Assert.IsType<byte>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsSmallIntValue()
        {
            var query = new Select() { Text = "SELECT CAST(42 AS smallint);" };
            object? result =  facade.Execute(query);
            Assert.IsType<short>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsIntValue()
        {
            var query = new Select() { Text = "SELECT CAST(42 AS int);" };
            object? result = facade.Execute(query);
            Assert.IsType<int>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsBigIntValue()
        {
            var query = new Select() { Text = "SELECT CAST(42 AS bigint);" };
            object? result = facade.Execute(query);
            Assert.IsType<long>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsFloatValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS float);" };
            object? result = facade.Execute(query);
            Assert.IsType<double>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsRealValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS real);" };
            object? result = facade.Execute(query);
            Assert.IsType<float>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsDecimalValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS decimal(5, 2));" };
            object? result = facade.Execute(query);
            Assert.IsType<decimal>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsSmallMoneyValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS smallmoney);" };
            object? result = facade.Execute(query);
            Assert.IsType<decimal>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsMoneyValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS money);" };
            object? result = facade.Execute(query);
            Assert.IsType<decimal>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsCharValue()
        {
            var query = new Select() { Text = "SELECT CAST('A' AS char(1));" };
            object? result = facade.Execute(query);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsVarCharValue()
        {
            var query = new Select() { Text = "SELECT CAST('Hello' AS varchar(10));" };
            object? result = facade.Execute(query);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsNVarCharValue()
        {
            var query = new Select() { Text = "SELECT CAST('Hello' AS nvarchar(10));" };
            object? result = facade.Execute(query);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsBinaryValue()
        {
            var query = new Select() { Text = "SELECT CAST(0x42 AS binary(1));" };
            object? result = facade.Execute(query);
            Assert.IsType<byte[]>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsVarBinaryValue()
        {
            var query = new Select() { Text = "SELECT CAST(0x424242 AS varbinary(3));" };
            object? result = facade.Execute(query);
            Assert.IsType<byte[]>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsUniqueIdentifierValue()
        {
            var query = new Select() { Text = "SELECT NEWID();" };
            object? result = facade.Execute(query);
            Assert.IsType<Guid>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsSmallDateTimeValue()
        {
            var query = new Select() { Text = "SELECT CAST('2023-05-03 14:30:00' AS smalldatetime);" };
            object? result = facade.Execute(query);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsDateTimeValue()
        {
            var query = new Select() { Text = "SELECT CAST('2023-05-03 14:30:00' AS datetime);" };
            object? result = facade.Execute(query);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsDateValue()
        {
            var query = new Select() { Text = "SELECT CAST('2023-05-03' AS date);" };
            object? result =  facade.Execute(query);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsTimeValue()
        {
            var query = new Select() { Text = "SELECT CAST('14:30:00' AS time);" };
            object? result =  facade.Execute(query);
            Assert.IsType<TimeSpan>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsDateTime2Value()
        {
            var query = new Select() { Text = "SELECT CAST('2023-05-03 14:30:00' AS datetime2(7));" };
            object? result =  facade.Execute(query);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnSelect_ReturnsDateTimeOffsetValue()
        {
            var query = new Select() { Text = "SELECT CAST('2023-05-03 14:30:00 +01:00' AS datetimeoffset(7));" };
            object? result =  facade.Execute(query);
            Assert.IsType<DateTimeOffset>(result);
        }
        #endregion

        #region Execute_T
        [Fact]
        public void Execute_T_ExecuteScalarOnMultipleColumns_ReturnsOnlyFirstResult()
        {
            var query = new Select() { Text = "SELECT 1, null, 'non';" };
            object? result = facade.Execute<int>(query);
            Assert.IsType<int>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarFilterSalaryAndCountResults_ReturnsCountOfFoundRecords()
        {
            var query = new Select() { Text = $"SELECT COUNT(*) FROM {TestData.TestTableName} WHERE Salary >= @minSalary;" };
            Action<DbCommand>? setup = (Command) => {
                Command.Parameters.Add(new SQLiteParameter("@minSalary", 62000));
            };
            object? result = facade.Execute<int>(query, setup);
            Assert.Equal(3, result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsNull()
        {
            var query = new Select() { Text = "SELECT null;" };
            object? result = facade.Execute<object>(query);
            Assert.Null(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsBitValue()
        {
            var query = new Select() { Text = "SELECT CAST(1 AS bit);" };
            object? result = facade.Execute<bool>(query);
            Assert.IsType<bool>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsTinyIntValue()
        {
            var query = new Select() { Text = "SELECT CAST(42 AS tinyint);" };
            object? result = facade.Execute<byte>(query);
            Assert.IsType<byte>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsSmallIntValue()
        {
            var query = new Select() { Text = "SELECT CAST(42 AS smallint);" };
            object? result = facade.Execute<short>(query);
            Assert.IsType<short>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsIntValue()
        {
            var query = new Select() { Text = "SELECT CAST(42 AS int);" };
            object? result = facade.Execute<int>(query);
            Assert.IsType<int>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsBigIntValue()
        {
            var query = new Select() { Text = "SELECT CAST(42 AS bigint);" };
            object? result = facade.Execute<long>(query);
            Assert.IsType<long>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsFloatValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS float);" };
            object? result = facade.Execute<double>(query);
            Assert.IsType<double>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsRealValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS real);" };
            object? result = facade.Execute<float>(query);
            Assert.IsType<float>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsDecimalValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS decimal(5, 2));" };
            object? result = facade.Execute<decimal>(query);
            Assert.IsType<decimal>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsSmallMoneyValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS smallmoney);" };
            object? result = facade.Execute<decimal>(query);
            Assert.IsType<decimal>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsMoneyValue()
        {
            var query = new Select() { Text = "SELECT CAST(42.42 AS money);" };
            object? result = facade.Execute<Decimal>(query);
            Assert.IsType<decimal>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsCharValue()
        {
            var query = new Select() { Text = "SELECT CAST('A' AS char(1));" };
            object? result = facade.Execute<string>(query);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsVarCharValue()
        {
            var query = new Select() { Text = "SELECT CAST('Hello' AS varchar(10));" };
            object? result = facade.Execute<string>(query);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsNVarCharValue()
        {
            var query = new Select() { Text = "SELECT CAST('Hello' AS nvarchar(10));" };
            object? result = facade.Execute<string>(query);
            Assert.IsType<string>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsBinaryValue()
        {
            var query = new Select() { Text = "SELECT CAST(0x42 AS binary(1));" };
            object? result = facade.Execute<byte[]>(query);
            Assert.IsType<byte[]>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsVarBinaryValue()
        {
            var query = new Select() { Text = "SELECT CAST(0x424242 AS varbinary(3));" };
            object? result = facade.Execute<Byte[]>(query);
            Assert.IsType<byte[]>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsUniqueIdentifierValue()
        {
            var query = new Select() { Text = "SELECT NEWID();" };
            object? result = facade.Execute<Guid>(query);
            Assert.IsType<Guid>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsSmallDateTimeValue()
        {
            var query = new Select() { Text = "SELECT CAST('2023-05-03 14:30:00' AS smalldatetime);" };
            object? result = facade.Execute<DateTime>(query);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsDateTimeValue()
        {
            var query = new Select() { Text = "SELECT CAST('2023-05-03 14:30:00' AS datetime);" };
            object? result = facade.Execute<DateTime>(query);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsDateValue()
        {
            var query = new Select() { Text = "SELECT CAST('2023-05-03' AS date);" };
            object? result = facade.Execute<DateTime>(query);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsTimeValue()
        {
            var query = new Select() { Text = "SELECT CAST('14:30:00' AS time);" };
            object? result = facade.Execute<TimeSpan>(query);
            Assert.IsType<TimeSpan>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsDateTime2Value()
        {
            var query = new Select() { Text = "SELECT CAST('2023-05-03 14:30:00' AS datetime2(7));" };
            object? result = facade.Execute<DateTime>(query);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnSelect_ReturnsDateTimeOffsetValue()
        {
            var query = new Select() { Text = "SELECT CAST('2023-05-03 14:30:00 +01:00' AS datetimeoffset(7));" };
            object? result = facade.Execute<DateTimeOffset>(query);
            Assert.IsType<DateTimeOffset>(result);
        }
        #endregion

        #region Execute_T
       
        [Fact]
        public void Execute_IO_ExecuteScalarFilterSalaryAndCountResults_ReturnsCountOfFoundRecords()
        {
            var query = new Select() { Text = $"SELECT COUNT(*) FROM {TestData.TestTableName} WHERE Salary >= @minSalary;" };
            Decimal minSalary = 62000;
            Action<DbCommand, Decimal>? setup = (Command, salary) => {
                Command.Parameters.Add(new SQLiteParameter("@minSalary", salary));
            };
            int? result = facade.Execute<Decimal, int>(query, minSalary, setup);
            Assert.Equal(3, result);
        }

        #endregion

    }
}
