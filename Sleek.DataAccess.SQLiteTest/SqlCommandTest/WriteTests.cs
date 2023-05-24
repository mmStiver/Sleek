using System.Data.Common;

namespace Sleek.DataAcess.SqlServerTest.SqlCommandTest
{
    public class WriteTests 
    {

        ISQLiteGateway facade;
        public WriteTests()
        {
            facade = new SQLiteGateway(TestData.localConnection);
        }

        #region Execute
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
            object? result = facade.Execute(query);
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
            object? result = facade.Execute(query);
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
            object? result = facade.Execute(query);
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
            object? result = facade.Execute(query);
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
            object? result =  facade.Execute(query, Setup);
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
            object? result =  facade.Execute(query, Setup);
            Assert.Equal(1, result);
        }
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
            object? result =  facade.Execute(query, Setup);
            Assert.Equal(1, result);
        }
 
        #endregion
       
    }
}