
using System.Data.Common;

namespace Sleek.DataAcess.SqlServerTest.SqlCommandTest
{
    public class DeleteTests : IClassFixture<SQLiteTestFixture>, IDisposable
    {

        ISQLiteGateway facade;
        public DeleteTests()
        {
            facade = new SQLiteGateway(TestData.localConnection);
        }

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
                command.Parameters.Add(new SQLiteParameter("@PhoneId", 1));
            };
            object? result =  facade.Execute(query, Setup);
            Assert.Equal(1, result);
        }
        #endregion

        public void Dispose()
        {
           
        }
    }
}