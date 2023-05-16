using Sleek.DataAccess.SQLite;

namespace Sleek.DataAcess.SqlServerTest.SqlConnectionTest
{
    public class AvailableDatabase : IClassFixture<SQLiteTestFixture>
    {
        ISQLiteGateway gateway;
        public AvailableDatabase(SQLiteTestFixture context)
        {
            TimeSpan timeout = TimeSpan.FromSeconds(5);

            gateway = new SQLiteGateway(context.connectionString);
        }

        [Fact]
        public void TestConnection_ReturnFalseToIndicateConnectionFailurey()
        {
            var result = gateway.TestConnection();
            Assert.True(result);
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