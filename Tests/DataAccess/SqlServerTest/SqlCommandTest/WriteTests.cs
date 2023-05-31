using System.Data.Common;
using System.Data.SqlClient;
using System.Net;


namespace Sleek.DataAccess.SqlServerTest.DbCommandTest
{
    public class WriteTests : IClassFixture<SqlServerTestFixture>, IDisposable
    {

        ISqlServerGateway gateway;
        string connectionString;

        public Address TestAddress { get; }

        public WriteTests(SqlServerTestFixture context)
        {
            gateway = new SqlServerGateway(context.connectionString);
            connectionString = context.connectionString;

            using (var connection = new SqlConnection(context.connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(TestData.CreateAddressTable, connection))
                {
                    command.ExecuteNonQuery();
                }
                using (var command = new SqlCommand(TestData.InsertIntoAddressTable, connection))
                {
                    command.ExecuteNonQuery();
                }
                    ;
                TestAddress = new Address
                {
                    Id = 6,
                    State = "ON",
                    StreetAddress = "123 Fake Street",
                    City = "Windsor",
                    Country = "Canada",
                    PostalCode = "12345"
                };
            }
        }
        #region ExecuteAsync
        [Fact]
        public async Task ExecuteAsync_InsertNewAddressIntoTable_ReturnsInsertCountAsInt()
        {
            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES('123 Main St', 'New York', 'NY', '10001', 'USA'); 
                """
            };
            object? result = await gateway.ExecuteAsync(query);
            Assert.IsType<int>(result);
        }
        [Fact]
        public async Task ExecuteAsync_InsertNewAddressIntoTable_ReturnInsertCount()
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
            object? result = await gateway.ExecuteAsync(query);
            Assert.Equal(3, result);
        }
        [Fact]
        public async Task ExecuteAsync_UpdateSingleAddress_ReturnUpdateCount()
        {
            var query = new Write()
            {
                Text = """
                UPDATE Address 
                Set StreetAddress = '123 Fake Street'
                Where Id = 1 
                """
            };
            object? result = await gateway.ExecuteAsync(query);
            Assert.Equal(1, result);
        }
        [Fact]
        public async Task ExecuteAsync_UpdateAllAddresses_ReturnUpdateCount()
        {
            var query = new Write()
            {
                Text = """
                UPDATE Address 
                Set Country = 'United States Of America'
                """
            };
            object? result = await gateway.ExecuteAsync(query);
            Assert.Equal(2, result);
        }
        [Fact]
        public async Task ExecuteAsync_InsertparameterizedNewAddressIntoTable_ReturnsInsertCountAsInt()
        {
            string address = "123 Main St", 
                city = "New York", 
                state =  "NY", 
                postalCode = "10001", 
                country =  "USA"; 

            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES(@streetAddress, @city, @state, @postalCode, @country);
                """
            };
            var Setup = (DbCommand command) => { 
                command.Parameters.Add(new SqlParameter("@streetAddress", address));
                command.Parameters.Add(new SqlParameter("@city", city));
                command.Parameters.Add(new SqlParameter("@state", state));
                command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
                command.Parameters.Add(new SqlParameter("@country", country));
            };
            object? result = await gateway.ExecuteAsync(query, Setup);
            Assert.IsType<int>(result);
        }
        [Fact]
        public async Task ExecuteAsync_InsertParameterizedNewAddressIntoTable_ReturnInsertCount()
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
                command.Parameters.Add(new SqlParameter("@streetAddress", address));
                command.Parameters.Add(new SqlParameter("@city", city));
                command.Parameters.Add(new SqlParameter("@state", state));
                command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
                command.Parameters.Add(new SqlParameter("@country", country));
            };
            object? result = await gateway.ExecuteAsync(query, Setup);
            Assert.Equal(1, result);
        }
        [Fact]
        public async Task ExecuteAsync_SetupAddressPassedByObject_ReturnInsertCount()
        {
            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES(@streetName, @city, @state, @postalcode, @country);
                """
            };
            var setup = (DbCommand cmd, object data) => {
                Address address = (Address)data;
                cmd.Parameters.Add(new SqlParameter("streetName", address.StreetAddress));
                cmd.Parameters.Add(new SqlParameter("city", address.City));
                cmd.Parameters.Add(new SqlParameter("state", address.State));
                cmd.Parameters.Add(new SqlParameter("postalcode", address.PostalCode));
                cmd.Parameters.Add(new SqlParameter("country", address.Country));
            };
            object? result = await gateway.ExecuteAsync(query, TestAddress, setup);
            Assert.Equal(1, result);
        }
        [Fact]
        public async Task ExecuteAsync_SetupAddressPassedByObject_InputPassedToSetupFunction()
        {
            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES(@streetName, @city, @state, @postalcode, @country);
                """
            };
            var setup = (DbCommand cmd, object data) => {
                Assert.IsType<Address>(data);
                Address address = (Address)data;
                Assert.Equal(TestAddress.StreetAddress, address.StreetAddress);
                cmd.Parameters.Add(new SqlParameter("streetName", address.StreetAddress));
                Assert.Equal(TestAddress.City, address.City);
                cmd.Parameters.Add(new SqlParameter("city", address.City));
                Assert.Equal(TestAddress.State, address.State);
                cmd.Parameters.Add(new SqlParameter("state", address.State));
                Assert.Equal(TestAddress.PostalCode, address.PostalCode);
                cmd.Parameters.Add(new SqlParameter("postalcode", address.PostalCode));
                Assert.Equal(TestAddress.Country, address.Country);
                cmd.Parameters.Add(new SqlParameter("country", address.Country));
            };
            _ = await gateway.ExecuteAsync(query, TestAddress, setup);
        }

        [Fact]
        public async Task ExecuteAsync_UpdateparameterizedSingleAddress_ReturnUpdateCount()
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
                command.Parameters.Add(new SqlParameter("@addressId", 1));
            };
            object? result = await gateway.ExecuteAsync(query, Setup);
            Assert.Equal(1, result);
        }       
        
        #endregion

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
                command.Parameters.Add(new SqlParameter("@streetAddress", address));
                command.Parameters.Add(new SqlParameter("@city", city));
                command.Parameters.Add(new SqlParameter("@state", state));
                command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
                command.Parameters.Add(new SqlParameter("@country", country));
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
                command.Parameters.Add(new SqlParameter("@streetAddress", address));
                command.Parameters.Add(new SqlParameter("@city", city));
                command.Parameters.Add(new SqlParameter("@state", state));
                command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
                command.Parameters.Add(new SqlParameter("@country", country));
            };
            object? result =  gateway.Execute(query, Setup);
            Assert.Equal(1, result);
        }
        [Fact]
        public void Execute_SetupAddressPassedByObject_ReturnInsertCount()
        {
            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES(@streetName, @city, @state, @postalcode, @country);
                """
            };
            var setup = (DbCommand cmd, object data) => {
                Address address = (Address)data;
                cmd.Parameters.Add(new SqlParameter("streetName", address.StreetAddress));
                cmd.Parameters.Add(new SqlParameter("city", address.City));
                cmd.Parameters.Add(new SqlParameter("state", address.State));
                cmd.Parameters.Add(new SqlParameter("postalcode", address.PostalCode));
                cmd.Parameters.Add(new SqlParameter("country", address.Country));
            };
            object? result = gateway.Execute(query, TestAddress, setup);
            Assert.Equal(1, result);
        }
        [Fact]
        public void Execute_SetupAddressPassedByObject_InputPassedToSetupFunction()
        {
            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES(@streetName, @city, @state, @postalcode, @country);
                """
            };
            var setup = (DbCommand cmd, object data) => {
                Assert.IsType<Address>(data);
                Address address = (Address)data;
                Assert.Equal(TestAddress.StreetAddress, address.StreetAddress);
                cmd.Parameters.Add(new SqlParameter("streetName", address.StreetAddress));
                Assert.Equal(TestAddress.City, address.City);
                cmd.Parameters.Add(new SqlParameter("city", address.City));
                Assert.Equal(TestAddress.State, address.State);
                cmd.Parameters.Add(new SqlParameter("state", address.State));
                Assert.Equal(TestAddress.PostalCode, address.PostalCode);
                cmd.Parameters.Add(new SqlParameter("postalcode", address.PostalCode));
                Assert.Equal(TestAddress.Country, address.Country);
                cmd.Parameters.Add(new SqlParameter("country", address.Country));
            };
            _ = gateway.Execute(query, TestAddress, setup);
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
                command.Parameters.Add(new SqlParameter("@addressId", 1));
            };
            object? result =  gateway.Execute(query, Setup);
            Assert.Equal(1, result);
        }

        #endregion


        [Fact]
        public async Task ExecuteAsync_T_SetupAddressPassedByType_ReturnInsertCount()
        {
            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES(@streetName, @city, @state, @postalcode, @country);
                """
            };
            var setup = (DbCommand cmd, Address data) => {
                Address address = (Address)data;
                cmd.Parameters.Add(new SqlParameter("streetName", address.StreetAddress));
                cmd.Parameters.Add(new SqlParameter("city", address.City));
                cmd.Parameters.Add(new SqlParameter("state", address.State));
                cmd.Parameters.Add(new SqlParameter("postalcode", address.PostalCode));
                cmd.Parameters.Add(new SqlParameter("country", address.Country));
            };
            int result = await gateway.ExecuteAsync<Address>(query, TestAddress, setup);
            Assert.Equal(1, result);
        }
        [Fact]
        public async Task ExecuteAsync_T_SetupAddressPassedByType_InputPassedToSetupFunction()
        {
            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES(@streetName, @city, @state, @postalcode, @country);
                """
            };
            var setup = (DbCommand cmd, Address data) => {
                Address address = (Address)data;
                Assert.Equal(TestAddress.StreetAddress, address.StreetAddress);
                cmd.Parameters.Add(new SqlParameter("streetName", address.StreetAddress));
                Assert.Equal(TestAddress.City, address.City);
                cmd.Parameters.Add(new SqlParameter("city", address.City));
                Assert.Equal(TestAddress.State, address.State);
                cmd.Parameters.Add(new SqlParameter("state", address.State));
                Assert.Equal(TestAddress.PostalCode, address.PostalCode);
                cmd.Parameters.Add(new SqlParameter("postalcode", address.PostalCode));
                Assert.Equal(TestAddress.Country, address.Country);
                cmd.Parameters.Add(new SqlParameter("country", address.Country));
            };
            int result = await gateway.ExecuteAsync<Address>(query, TestAddress, setup);
            Assert.Equal(1, result);
        }


        [Fact]
        public void Execute_T_SetupAddressPassedByType_ReturnInsertCount()
        {
            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES(@streetName, @city, @state, @postalcode, @country);
                """
            };
            var setup = (DbCommand cmd, Address data) => {
                Address address = (Address)data;
                cmd.Parameters.Add(new SqlParameter("streetName", address.StreetAddress));
                cmd.Parameters.Add(new SqlParameter("city", address.City));
                cmd.Parameters.Add(new SqlParameter("state", address.State));
                cmd.Parameters.Add(new SqlParameter("postalcode", address.PostalCode));
                cmd.Parameters.Add(new SqlParameter("country", address.Country));
            };
            int result = gateway.Execute<Address>(query, TestAddress, setup);
            Assert.Equal(1, result);
        }
        [Fact]
        public void Execute_T_SetupAddressPassedByType_InputPassedToSetupFunction()
        {
            var query = new Write()
            {
                Text = """
                INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                VALUES(@streetName, @city, @state, @postalcode, @country);
                """
            };
            var setup = (DbCommand cmd, Address data) => {
                Address address = (Address)data;
                Assert.Equal(TestAddress.StreetAddress, address.StreetAddress);
                cmd.Parameters.Add(new SqlParameter("streetName", address.StreetAddress));
                Assert.Equal(TestAddress.City, address.City);
                cmd.Parameters.Add(new SqlParameter("city", address.City));
                Assert.Equal(TestAddress.State, address.State);
                cmd.Parameters.Add(new SqlParameter("state", address.State));
                Assert.Equal(TestAddress.PostalCode, address.PostalCode);
                cmd.Parameters.Add(new SqlParameter("postalcode", address.PostalCode));
                Assert.Equal(TestAddress.Country, address.Country);
                cmd.Parameters.Add(new SqlParameter("country", address.Country));
            };
            int result = gateway.Execute<Address>(query, TestAddress, setup);
            Assert.Equal(1, result);
        }

        public void Dispose()
        {
            using (var connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand("DROP TABLE dbo.Address", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}