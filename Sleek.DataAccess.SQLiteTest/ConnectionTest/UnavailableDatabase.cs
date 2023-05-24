using Sleek.DataAccess.SQLite;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Sleek.DataAcess.SQLiteTest.ConnectionTest
{
    public class UnavailableDatabase
    {

        ISQLiteGateway facade;
        public UnavailableDatabase()
        {
            facade = new SQLiteGateway(TestData.nonexistantConnection);
        }

        [Fact]
        public void TestConnection_ReturnFalseToIndicateConnectionFailurey()
        {
            var result = facade.TestConnection();
            Assert.False(result);
        }

        [Fact]
        public void Execute_ThrowsSqlException()
        {
            Assert.Throws<SQLiteException>(() => facade.Execute(
                new DataDefinitionQuery() { Text = "CREATE TABLE #TestTemp;" }
            ));
        }
    }
}