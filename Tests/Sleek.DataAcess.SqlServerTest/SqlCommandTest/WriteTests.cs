using System.Data.SqlClient;


namespace Sleek.DataAcess.SqlServerTest.SqlCommandTest
{
    public class WriteTests : IClassFixture<SqlServerTestFixture>, IDisposable
    {

        ISqlServerGateway facade;
        string connectionString;
        public WriteTests(SqlServerTestFixture context)
        {
            facade = new SqlServerGateway(context.connectionString);
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
            object? result = await facade.ExecuteAsync(query);
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
            object? result = await facade.ExecuteAsync(query);
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
            object? result = await facade.ExecuteAsync(query);
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
            object? result = await facade.ExecuteAsync(query);
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
            var Setup = (SqlCommand command) => { 
                command.Parameters.Add(new SqlParameter("@streetAddress", address));
                command.Parameters.Add(new SqlParameter("@city", city));
                command.Parameters.Add(new SqlParameter("@state", state));
                command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
                command.Parameters.Add(new SqlParameter("@country", country));
            };
            object? result = await facade.ExecuteAsync(query, Setup);
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
            var Setup = (SqlCommand command) => {
                command.Parameters.Add(new SqlParameter("@streetAddress", address));
                command.Parameters.Add(new SqlParameter("@city", city));
                command.Parameters.Add(new SqlParameter("@state", state));
                command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
                command.Parameters.Add(new SqlParameter("@country", country));
            };
            object? result = await facade.ExecuteAsync(query, Setup);
            Assert.Equal(1, result);
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
            var Setup = (SqlCommand command) => {
                command.Parameters.Add(new SqlParameter("@addressId", 1));
            };
            object? result = await facade.ExecuteAsync(query, Setup);
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
            var Setup = (SqlCommand command) => {
                command.Parameters.Add(new SqlParameter("@streetAddress", address));
                command.Parameters.Add(new SqlParameter("@city", city));
                command.Parameters.Add(new SqlParameter("@state", state));
                command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
                command.Parameters.Add(new SqlParameter("@country", country));
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
            var Setup = (SqlCommand command) => {
                command.Parameters.Add(new SqlParameter("@streetAddress", address));
                command.Parameters.Add(new SqlParameter("@city", city));
                command.Parameters.Add(new SqlParameter("@state", state));
                command.Parameters.Add(new SqlParameter("@postalCode", postalCode));
                command.Parameters.Add(new SqlParameter("@country", country));
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
            var Setup = (SqlCommand command) => {
                command.Parameters.Add(new SqlParameter("@addressId", 1));
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

                using (var command = new SqlCommand("DROP TABLE dbo.Address", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}