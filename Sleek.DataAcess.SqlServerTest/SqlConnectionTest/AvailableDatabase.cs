namespace Sleek.DataAccess.SqlServerTest.SqlConnectionTest
{
    [Collection("SQL Server Database collection")]
    public class AvailableDatabase
    {
        ISqlServerGateway gateway;
        public AvailableDatabase(SqlServerTestFixture context)
        {
            TimeSpan timeout = TimeSpan.FromSeconds(5);

            gateway = new SqlServerGateway(context.connectionString);
        }

        [Fact]
        public void TestConnection_ReturnFalseToIndicateConnectionFailurey()
        {
            var result = gateway.TestConnection();
            Assert.True(result);
        }

        [Fact]
        public async Task TestConnectionAsync_ReturnFalseToIndicateConnectionFailurey()
        {
            var result = await gateway.TestConnectionAsync();
            Assert.True(result);
        }

        [Fact]
        public async Task ExecuteAsync_SendDDLCommand_CreateObject()
        {
            int result = await gateway.ExecuteAsync(
                new DataDefinitionQuery() { Text = @"CREATE TABLE #TestTemp (Id tinyint);" }
            );
            Assert.Equal(-1, result);

        }

        [Fact]
        public void Execute_SendDDLCommand_CreateObject()
        {
            int result = gateway.Execute(
                new DataDefinitionQuery() { Text = @"CREATE TABLE #TestTemp (Id tinyint);" }
            );
            Assert.Equal(-1, result);

        }
    }
}