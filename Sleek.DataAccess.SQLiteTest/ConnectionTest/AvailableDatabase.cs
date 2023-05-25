using Sleek.DataAccess.SQLite;

namespace Sleek.DataAcess.SQLiteTest.ConnectionTest
{
    public class AvailableDatabase
    {
        ISQLiteGateway gateway;
        public AvailableDatabase()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(5);

            gateway = new SQLiteGateway(TestData.localConnection);
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
                new DataDefinitionQuery() { Text = @"CREATE TABLE CreateObject (Id INTEGER);" }
            );
            Assert.Equal(0, result);

        }
    }
}