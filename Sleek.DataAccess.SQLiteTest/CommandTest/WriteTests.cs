using Sleek.DataAccess.SQLiteTest;
using System.Data.Common;

namespace Sleek.DataAcess.SQLiteTest.CommandTest
{
    [Collection("SQLite Database collection")]
    public class WriteTests : IDisposable
    {

        ISQLiteGateway gateway;
        public WriteTests(SQLiteTestFixture testFixture)
        {
            gateway = new SQLiteGateway(testFixture.connection);
            gateway.Execute(new DataDefinitionQuery() { Text = TestData.CreatePhoneTable });
            gateway.Execute(new DataDefinitionQuery() { Text = TestData.CreateAddressTable });
            gateway.Execute(new Write() { Text = TestData.InsertPhoneTable });
            gateway.Execute(new Write() { Text = TestData.InsertIntoAddressTable });
        }

        #region ExecuteDelete
        [Fact]
        public void Execute_DeleteNonexistantId_ReturnDeleteCountOfNone()
        {

            var query = new Write()
            {
                Text = """
                Delete From PhoneNumber 
                Where Id = 100
                """
            };
            object? result = gateway.Execute(query);

            Assert.Equal(0, result);
        }
        [Fact]
        public void Execute_DeleteSinglePhoneById_ReturnDeleteCountOfOne()
        {

            var query = new Write()
            {
                Text = """
                Delete From PhoneNumber 
                Where Id = 1 
                """
            };
            object? result = gateway.Execute(query);

            Assert.Equal(1, result);
        }
        [Fact]
        public void Execute_DeleteAllPhones_ReturnDeleteCount()
        {
            long count1 = gateway.Execute<long>(new Select() { Text = "SELECT COUNT(*) FROM PhoneNumber" });

            var query = new Write()
            {
                Text = """
                Delete From DummyPerson
                """
            };
            object? result = gateway.Execute(query);

            long count2 = gateway.Execute<long>(new Select() { Text = "SELECT COUNT(*) FROM PhoneNumber" });


            Assert.Equal(5, result);
        }
        [Fact]
        public void Execute_DeleteparameterizedSinglePhone_ReturnDeleteCount()
        {
            var query = new Write()
            {
                Text = """
                Delete From PhoneNumber 
                Where Id = @Id 
                """
            };
            var Setup = (DbCommand command) => {
                command.Parameters.Add(new SQLiteParameter("@Id", 1));
            };
            object? result = gateway.Execute(query, Setup);
            Assert.Equal(1, result);
        }

    
        #endregion

        #region ExecuteInsert
        [Fact]
        public void Execute_InsertNewAddressIntoTable_ReturnsInsertCountAsInt()
        {
            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES('123 Main St', 'New York', 'NY', '10001', 'USA'); 
                """
            };
            object? result = gateway.Execute(query);
            Assert.IsType<int>(result);
        }
        [Fact]
        public void Execute_InsertNewAddressIntoTable_ReturnInsertCount()
        {
            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES('123 Main St', 'New York', 'NY', '10001', 'USA'),
                 ('456 Maple Ave', 'Los Angeles', 'CA', '90001', 'USA'),
                 ('789 Oak St', 'Chicago', 'IL', '60601', 'USA');
                """
            };
            object? result = gateway.Execute(query);
            Assert.Equal(3, result);
        }
        [Fact]
        public void Execute_UpdateSingleAddress_ReturnUpdateCount()
        {
            var query = new Write()
            {
                Text = """
                UPDATE Address 
                Set StreetAddress = '123 Fake Street'
                Where Id = 1 
                """
            };
            object? result = gateway.Execute(query);
            Assert.Equal(1, result);
        }
        [Fact]
        public void Execute_UpdateAllAddresses_ReturnUpdateCount()
        {
            var query = new Write()
            {
                Text = """
                UPDATE Address 
                Set Country = 'United States Of America'
                """
            };
            object? result = gateway.Execute(query);
            Assert.Equal(2, result);
        }     
        [Fact]
        public void Execute_InsertParameterizedNewAddressIntoTable_ReturnsInsertCountAsInt()
        {
            string address = "123 Main St",
                city = "New York",
                state = "NY",
                postalCode = "10001",
                country = "USA";

            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES(@streetAddress, @city, @state, @postalCode, @country);
                """
            };
            var Setup = (DbCommand command) => {
                command.Parameters.Add(new SQLiteParameter("@streetAddress", address));
                command.Parameters.Add(new SQLiteParameter("@city", city));
                command.Parameters.Add(new SQLiteParameter("@state", state));
                command.Parameters.Add(new SQLiteParameter("@postalCode", postalCode));
                command.Parameters.Add(new SQLiteParameter("@country", country));
            };
            object? result =  gateway.Execute(query, Setup);
            Assert.IsType<int>(result);
        }
        [Fact]
        public void Execute_InsertParameterizedNewAddressIntoTable_ReturnInsertCount()
        {
            string address = "123 Main St",
               city = "New York",
               state = "NY",
               postalCode = "10001",
               country = "USA";

            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES(@streetAddress, @city, @state, @postalCode, @country);
                """
            };
            var Setup = (DbCommand command) => {
                command.Parameters.Add(new SQLiteParameter("@streetAddress", address));
                command.Parameters.Add(new SQLiteParameter("@city", city));
                command.Parameters.Add(new SQLiteParameter("@state", state));
                command.Parameters.Add(new SQLiteParameter("@postalCode", postalCode));
                command.Parameters.Add(new SQLiteParameter("@country", country));
            };
            object? result =  gateway.Execute(query, Setup);
            Assert.Equal(1, result);
        }
        #endregion

        #region ExecuteUpdate
        [Fact]
        public void Execute_UpdateParameterizedSingleAddress_ReturnUpdateCount()
        {
            var query = new Write()
            {
                Text = """
                UPDATE Address 
                Set StreetAddress = '123 Fake Street'
                Where Id = @addressId 
                """
            };
            var Setup = (DbCommand command) => {
                command.Parameters.Add(new SQLiteParameter("@addressId", 1));
            };
            object? result =  gateway.Execute(query, Setup);
            Assert.Equal(1, result);
        }

        #endregion

        public void Dispose()
        {
            gateway.Execute(new DataDefinitionQuery() { Text = "DROP TABLE PhoneNumber" });
            gateway.Execute(new DataDefinitionQuery() { Text = "DROP TABLE Address" });

        }
    }
}