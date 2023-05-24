using System.Data.SqlClient;

namespace Sleek.DataAccess.SqlServerTest.SqlConnectionTest
{
    public class UnavailableDatabase
    {

        ISqlServerGateway facade;
        public UnavailableDatabase()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(1);
            var connection = String.Format(TestData.localConnection, timeout.TotalSeconds) + ";Database=NotExistDb";
            facade = new SqlServerGateway(connection);
        }

        [Fact]
        public void TestConnection_ReturnFalseToIndicateConnectionFailurey()
        {
            var result = facade.TestConnection();
            Assert.False(result);
        }

        [Fact]
        public async Task TestConnectionAsync_ReturnFalseToIndicateConnectionFailurey()
        {
            var result = await facade.TestConnectionAsync();
            Assert.False(result);
        }

        [Fact]
        public async Task ExecuteAsync_ThrowsSqlException()
        {
            await Assert.ThrowsAsync<SqlException>(() => facade.ExecuteAsync(
                new DataDefinitionQuery() { Text = "CREATE TABLE #TestTemp;" }
            ));
        }

        [Fact]
        public void Execute_ThrowsSqlException()
        {
            Assert.Throws<SqlException>(() => facade.Execute(
                new DataDefinitionQuery() { Text = "CREATE TABLE #TestTemp;" }
            ));
        }
    }
}