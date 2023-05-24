
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAcess.SqlServerTest.DbCommandTest
{
    public class SprocScalarResults : IClassFixture<SqlServerTestFixture> 
    {

        ISqlServerGateway facade;
        public SprocScalarResults(SqlServerTestFixture context) {
            facade = new SqlServerGateway(context.connectionString);
        }

        #region ExecuteAsync
        [Fact]
        public async Task ExecuteAsync_ExecuteScalarOnProcedure_ReturnsOnlyFirstResult()
        {
            var proc = new StoredProcedure { Name = TestData.GetPersonProcedure.Name };
            object? result = await facade.ExecuteAsync(proc);
            Assert.IsType<int>(result);
        }

        [Fact]
        public async Task ExecuteAsync_ExecuteScalarFilterSalaryAndCountResults_ReturnsCountOfFoundRecords()
        {
            var proc = new StoredProcedure { Name = TestData.GetPersonProcedure.Name };
            Action<DbCommand>? setup = (Command) => {
                Command.Parameters.Add(new SqlParameter("@minSalary", 62000));
            };
            object? result = await facade.ExecuteAsync(proc, setup);
            Assert.Equal(3, result);
        }


        [Fact]
        public async Task ExecuteAsync_ExecuteScalarOnProcedure_ReturnsNull()
        {
            var proc = new StoredProcedure { Name = TestData.GetNullProcedure.Name };
            object? result = await facade.ExecuteAsync(proc);
            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteAsync_ExecuteScalarOnProcedure_ReturnsIntValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetIntProcedure.Name };
            object? result = await facade.ExecuteAsync(proc);
            Assert.IsType<int>(result);
        }

        

        [Fact]
        public async Task ExecuteAsync_ExecuteScalarOnProcedure_ReturnsDecimalValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetNumericProcedure.Name };
            object? result = await facade.ExecuteAsync(proc);
            Assert.IsType<decimal>(result);
        }

       
        [Fact]
        public async Task ExecuteAsync_ExecuteScalarOnProcedure_ReturnsUniqueIdentifierValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetUUIDProcedure.Name };
            object? result = await facade.ExecuteAsync(proc);
            Assert.IsType<Guid>(result);
        }

       [Fact]
        public async Task ExecuteAsync_ExecuteScalarOnProcedure_ReturnsDateTime2Value()
        {
            var proc = new StoredProcedure { Name = TestData.GetDateProcedure.Name };
            object? result = await facade.ExecuteAsync(proc);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public async Task ExecuteAsync_ExecuteScalarOnProcedure_ReturnsDateTimeOffsetValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetDateOffsetProcedure.Name };
            object? result = await facade.ExecuteAsync(proc);
            Assert.IsType<DateTimeOffset>(result);
        }
        #endregion

        #region Execute
        [Fact]
        public void Execute_ExecuteScalarOnProcedur_ReturnsOnlyFirstResult()
        {
            var proc = new StoredProcedure { Name = TestData.GetPersonProcedure.Name };
            object? result = facade.Execute(proc);
            Assert.IsType<int>(result);
        }


        [Fact]
        public void Execute_ExecuteScalarFilterSalaryAndCountResults_ReturnsCountOfFoundRecords()
        {
            var proc = new StoredProcedure { Name = TestData.GetPersonProcedure.Name };
            Action<DbCommand>? setup = (Command) => {
                Command.Parameters.Add(new SqlParameter("@minSalary", 62000));
            };
            object? result = facade.Execute(proc, setup);
            Assert.Equal(3, result);
        }


        [Fact]
        public void Execute_ExecuteScalarOnProcedure_ReturnsNull()
        {
            var proc = new StoredProcedure { Name = TestData.GetNullProcedure.Name };
            object? result = facade.Execute(proc);
            Assert.Null(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnProcedure_ReturnsIntValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetIntProcedure.Name };
            object? result = facade.Execute(proc);
            Assert.IsType<int>(result);
        }

        
        [Fact]
        public void Execute_ExecuteScalarOnProcedure_ReturnsDecimalValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetNumericProcedure.Name };
            object? result = facade.Execute(proc);
            Assert.IsType<decimal>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnProcedure_ReturnsUniqueIdentifierValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetUUIDProcedure.Name };
            object? result = facade.Execute(proc);
            Assert.IsType<Guid>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnProcedure_ReturnsDateTime2Value()
        {
            var proc = new StoredProcedure { Name = TestData.GetDateProcedure.Name };
            object? result =  facade.Execute(proc);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public void Execute_ExecuteScalarOnProcedure_ReturnsDateTimeOffsetValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetDateOffsetProcedure.Name };
            object? result = facade.Execute(proc);
            Assert.IsType<DateTimeOffset>(result);
        }
        #endregion

        #region ExecuteAsync_T
        [Fact]
        public async Task ExecuteAsync_T_ExecuteScalarOnProcedure_ReturnsOnlyFirstResult()
        {
            var proc = new StoredProcedure { Name = TestData.GetPersonProcedure.Name };
            object? result = await facade.ExecuteAsync<int>(proc);
            Assert.IsType<int>(result);
        }


        [Fact]
        public async Task ExecuteAsync_T_ExecuteScalarFilterSalaryAndCountResults_ReturnsCountOfFoundRecords()
        {
            var proc = new StoredProcedure { Name = TestData.GetPersonProcedure.Name };
            Action<DbCommand>? setup = (Command) => {
                Command.Parameters.Add(new SqlParameter("@minSalary", 62000));
            };
            int result = await facade.ExecuteAsync<int>(proc, setup);
            Assert.Equal(3, result);
        }


        [Fact]
        public async Task ExecuteAsync_T_ExecuteScalarOnProcedure_ReturnsNull()
        {
            var proc = new StoredProcedure { Name = TestData.GetNullProcedure.Name };
            object? result = await facade.ExecuteAsync<object>(proc);
            Assert.Null(result);
        }

        [Fact]
        public async Task ExecuteAsync_T_ExecuteScalarOnProcedure_ReturnsIntValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetIntProcedure.Name };
            object? result = await facade.ExecuteAsync<int>(proc);
            Assert.IsType<int>(result);
        }

        [Fact]
        public async Task ExecuteAsync_T_ExecuteScalarOnProcedure_ReturnsDecimalValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetNumericProcedure.Name };
            object? result = await facade.ExecuteAsync<decimal>(proc);
            Assert.IsType<decimal>(result);
        }

        [Fact]
        public async Task ExecuteAsync_T_ExecuteScalarOnProcedure_ReturnsUniqueIdentifierValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetUUIDProcedure.Name };
            object? result = await facade.ExecuteAsync<Guid>(proc);
            Assert.IsType<Guid>(result);
        }

        [Fact]
        public async Task ExecuteAsync_T_ExecuteScalarOnProcedure_ReturnsDateTime2Value()
        {
            var proc = new StoredProcedure { Name = TestData.GetDateProcedure.Name };
            object? result = await facade.ExecuteAsync<DateTime>(proc);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public async Task ExecuteAsync_T_ExecuteScalarOnProcedure_ReturnsDateTimeOffsetValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetDateOffsetProcedure.Name };
            object? result = await facade.ExecuteAsync<DateTimeOffset>(proc);
            Assert.IsType<DateTimeOffset>(result);
        }
        #endregion

        #region Execute_T
        [Fact]
        public void Execute_T_ExecuteScalarOnProcedur_ReturnsOnlyFirstResult()
        {
            var proc = new StoredProcedure { Name = TestData.GetPersonProcedure.Name };
            object? result = facade.Execute<int>(proc);
            Assert.IsType<int>(result);
        }


        [Fact]
        public void Execute_T_ExecuteScalarFilterSalaryAndCountResults_ReturnsCountOfFoundRecords()
        {
            var proc = new StoredProcedure { Name = TestData.GetPersonProcedure.Name };
            Action<DbCommand>? setup = (Command) => {
                Command.Parameters.Add(new SqlParameter("@minSalary", 62000));
            };
            int result = facade.Execute<int>(proc, setup);
            Assert.Equal(3, result);
        }


        [Fact]
        public void Execute_T_ExecuteScalarOnProcedure_ReturnsNull()
        {
            var proc = new StoredProcedure { Name = TestData.GetNullProcedure.Name };
            object? result = facade.Execute<object>(proc);
            Assert.Null(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnProcedure_ReturnsIntValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetIntProcedure.Name };
            object? result = facade.Execute<int>(proc);
            Assert.IsType<int>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnProcedure_ReturnsDecimalValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetNumericProcedure.Name  };
            object? result = facade.Execute<decimal>(proc);
            Assert.IsType<decimal>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnProcedure_ReturnsUniqueIdentifierValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetUUIDProcedure.Name };
            object? result = facade.Execute<Guid>(proc);
            Assert.IsType<Guid>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnProcedure_ReturnsDateTime2Value()
        {
            var proc = new StoredProcedure { Name = TestData.GetDateProcedure.Name };
            object? result = facade.Execute<DateTime>(proc);
            Assert.IsType<DateTime>(result);
        }

        [Fact]
        public void Execute_T_ExecuteScalarOnProcedure_ReturnsDateTimeOffsetValue()
        {
            var proc = new StoredProcedure { Name = TestData.GetDateOffsetProcedure.Name };
            object? result = facade.Execute<DateTimeOffset>(proc);
            Assert.IsType<DateTimeOffset>(result);
        }
        #endregion




    }
}
