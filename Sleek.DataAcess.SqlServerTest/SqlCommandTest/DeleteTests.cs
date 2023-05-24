using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAcess.SqlServerTest.DbCommandTest
{
    public class DeleteTests : IClassFixture<SqlServerTestFixture>, IDisposable
    {

        ISqlServerGateway facade;
        string connectionString;
        public DeleteTests(SqlServerTestFixture context)
        {
            facade = new SqlServerGateway(context.connectionString);
            connectionString = context.connectionString;

            using (var connection = new SqlConnection(context.connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(TestData.CreatePhoneTable, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(TestData.InsertIntoPhoneNumTable, connection))
                {
                    command.ExecuteNonQuery();
                }
                    ;
                
            }
        }
        #region ExecuteAsync
        [Fact]
        public async Task ExecuteAsync_DeleteSinglePhone_ReturnDeleteCount()
        {
            var query = new Write()
            {
                Text = """
                Delete From PhoneNumber 
                Where Id = 1 
                """
            };
            object? result = await facade.ExecuteAsync(query);
            Assert.Equal(1, result);
        }
        [Fact]
        public async Task ExecuteAsync_DeleteAllPhonees_ReturnDeleteCount()
        {
            var query = new Write()
            {
                Text = """
                Delete From PhoneNumber
                """
            };
            object? result = await facade.ExecuteAsync(query);
            Assert.Equal(5, result);
        }
        [Fact]
        public async Task ExecuteAsync_DeleteparameterizedSinglePhone_ReturnDeleteCount()
        {
            var query = new Write()
            {
                Text = """
                Delete From PhoneNumber 
                Where Id = 1 
                """
            };
            var Setup = (DbCommand command) => {
                command.Parameters.Add(new SqlParameter("@PhoneId", 1));
            };
            object? result = await facade.ExecuteAsync(query, Setup);
            Assert.Equal(1, result);
        }
        #endregion

        #region Execute
        [Fact]
        public void Execute_DeleteSinglePhone_ReturnDeleteCount()
        {
            var query = new Write()
            {
                Text = """
                Delete From PhoneNumber 
                Where Id = 1 
                """
            };
            object? result = facade.Execute(query);
            Assert.Equal(1, result);
        }
        [Fact]
        public void Execute_DeleteAllPhonees_ReturnDeleteCount()
        {
            var query = new Write()
            {
                Text = """
                Delete From PhoneNumber
                """
            };
            object? result = facade.Execute(query);
            Assert.Equal(5, result);
        }
        [Fact]
        public void Execute_DeleteparameterizedSinglePhone_ReturnDeleteCount()
        {
            var query = new Write()
            {
                Text = """
                Delete From PhoneNumber 
                Where Id = 1 
                """
            };
            var Setup = (DbCommand command) => {
                command.Parameters.Add(new SqlParameter("@PhoneId", 1));
            };
            object? result =  facade.Execute(query, Setup);
            Assert.Equal(1, result);
        }
        #endregion

        public void Dispose()
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("DROP TABLE dbo.PhoneNumber", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}