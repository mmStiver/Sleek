using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.Data;

namespace Sleek.DataAccess.SQLiteTest.SqlDataReader
{
    [Collection("SQLite Database collection")]

    public class SelectDataReaderResults
    {

        ISQLiteGateway gateway;
        private readonly DummyPerson person;

        public SelectDataReaderResults(SQLiteTestFixture context)
        {
            gateway = new SQLiteGateway(context.connection);
            person = new DummyPerson()
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1987, 05, 01),
                Age = 36,
                IsActive = true,
                Height = (double)180.5,
                Weight = (decimal)75.2,
                CreatedAt = DateTime.Parse("2023-05-01 10:30:00.0000000"),
                UpdatedAt = DateTimeOffset.Parse("2023-05-01 10:30:00.0000000 +00:00"),
                AdditionalInfo = "<info><hobby>Coding</hobby></info>",
                ProfileImage = null,
                Salary = 55000,
                PhoneNumber = "5551234567",
                UniqueId = new Guid("0136C08D-E0C2-4F96-8AB5-7E0A8B4923AF")
            };

        }

        private IEnumerable<DummyPerson> MapReaderToPerson(DbDataReader reader)
        {
            var objs = new List<DummyPerson>();
            while (reader.Read())
            {
                objs.Add(new DummyPerson()
                {

                    Id = reader.GetInt64(reader.GetOrdinal("Id"))
                    ,
                    FirstName = reader.GetString(reader.GetOrdinal("FirstName"))
                    ,
                    LastName = reader.GetString(reader.GetOrdinal("LastName"))
                    ,
                    BirthDate = reader.GetDateTime(reader.GetOrdinal("BirthDate"))
                    ,
                    Age = reader.GetInt64(reader.GetOrdinal("Age"))
                    ,
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
                    ,
                    Height = reader.GetDouble(reader.GetOrdinal("Height"))
                    ,
                    Weight = reader.GetDecimal(reader.GetOrdinal("Weight"))
                    ,
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    ,
                    UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                    ,
                    AdditionalInfo = reader.GetString(reader.GetOrdinal("AdditionalInfo"))
                    ,
                    ProfileImage = (byte[])reader.GetValue(reader.GetOrdinal("ProfileImage"))
                    ,
                    Salary = reader.GetDecimal(reader.GetOrdinal("Salary"))
                    ,
                    PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber"))
                    ,
                    UniqueId = reader.GetString(reader.GetOrdinal("UniqueId"))

                });
            }
            return objs.AsEnumerable();
        }
        public DummyPerson MapDummyPersonFromDataReader(SQLiteDataReader reader)
        {
            DummyPerson person = 

            return person;
        }

        #region Execute_T
        [Fact]
        public void Execute_T_ExecuteReaderOnTable_ReturnsAllRows()
        {
            var query = new Select() { Text = $"SELECT * FROM {TestData.TestTableName}" };
            Func<DbDataReader, IEnumerable<DummyPerson>>? mapper = (reader) =>
            {
                var objs = new List<DummyPerson>();
                while (reader.Read())
                {
                    objs.Add(new()
                    {
                        Id = (int)reader["Id"],
                        FirstName = reader["FirstName"] as string,
                        LastName = reader["LastName"] as string,
                        BirthDate = (DateTime)reader["BirthDate"],
                        Age = (short)reader["Age"],
                        IsActive = (bool)reader["IsActive"],
                        Height = (double)reader["Height"],
                        Weight = (decimal)reader["Weight"],
                        CreatedAt = (DateTime)reader["CreatedAt"],
                        UpdatedAt = (DateTimeOffset)reader["UpdatedAt"],
                        AdditionalInfo = reader["AdditionalInfo"] as string,
                        ProfileImage = reader["ProfileImage"] as byte[],
                        Salary = (decimal)reader["Salary"],
                        PhoneNumber = reader["PhoneNumber"] as string,
                        UniqueId = (Guid)reader["UniqueId"]
                    });
                }
                return objs.AsEnumerable();
            };

            IEnumerable<DummyPerson>? result = gateway.Execute(query, mapper);
            Assert.IsType<List<DummyPerson>>(result);
            var list = result as List<DummyPerson>;
            Assert.NotNull(list);
            Assert.Equal(5, list.Count);
        }
        [Fact]
        public void Execute_T_ExecuteReaderOnTable_ReturnsMappedObjects()
        {
            var query = new Select() { Text = $"SELECT * FROM {TestData.TestTableName} ORDER BY 1" };
            Func<DbDataReader, IEnumerable<DummyPerson>>? mapper = (reader) =>
            {
                var objs = new List<DummyPerson>();
                while (reader.Read())
                {
                    objs.Add(new()
                    {
                        Id = (int)reader["Id"],
                        FirstName = reader["FirstName"] as string,
                        LastName = reader["LastName"] as string,
                        BirthDate = (DateTime)reader["BirthDate"],
                        Age = (short)reader["Age"],
                        IsActive = (bool)reader["IsActive"],
                        Height = (double)reader["Height"],
                        Weight = (decimal)reader["Weight"],
                        CreatedAt = (DateTime)reader["CreatedAt"],
                        UpdatedAt = (DateTimeOffset)reader["UpdatedAt"],
                        AdditionalInfo = reader["AdditionalInfo"] as string,
                        ProfileImage = reader["ProfileImage"] as byte[],
                        Salary = (decimal)reader["Salary"],
                        PhoneNumber = reader["PhoneNumber"] as string,
                        UniqueId = (Guid)reader["UniqueId"]
                    });
                }
                return objs.AsEnumerable();
            };

            IEnumerable<DummyPerson>? result = gateway.Execute(query, mapper);
            Assert.Collection(result ?? Enumerable.Empty<DummyPerson>(),
                e => Assert.Equal(1, e.Id),
                e => Assert.Equal(2, e.Id),
                e => Assert.Equal(3, e.Id),
                e => Assert.Equal(4, e.Id),
                e => Assert.Equal(5, e.Id)
                );

        }

        [Fact]
        public void Execute_T_ExecuteReaderOnTableById_ReturnsMappedObject()
        {
            var query = new Select() { Text = $"SELECT * FROM {TestData.TestTableName} WHERE Id = 1;" };

            Func<DbDataReader, DummyPerson>? mapper = (reader) =>
            {
                reader.Read();
                Debug.WriteLine(reader["UpdatedAt"].ToString());

                DummyPerson person = new()
                {
                    Id = (int)reader["Id"],
                    FirstName = reader["FirstName"] as string,
                    LastName = reader["LastName"] as string,
                    BirthDate = (DateTime)reader["BirthDate"],
                    Age = (short)reader["Age"],
                    IsActive = (bool)reader["IsActive"],
                    Height = (double)reader["Height"],
                    Weight = (decimal)reader["Weight"],
                    CreatedAt = (DateTime)reader["CreatedAt"],
                    UpdatedAt = (DateTimeOffset)reader["UpdatedAt"],
                    AdditionalInfo = reader["AdditionalInfo"] as string,
                    ProfileImage = reader["ProfileImage"] as byte[],
                    Salary = (decimal)reader["Salary"],
                    PhoneNumber = reader["PhoneNumber"] as string,
                    UniqueId = (Guid)reader["UniqueId"]
                };

                return person;
            };
            DummyPerson? result = gateway.Execute(query, mapper);
            Assert.NotNull(result);
            Assert.Equal(person.Id, result.Id);
            Assert.Equal(person.FirstName, result.FirstName);
            Assert.Equal(person.LastName, result.LastName);
            Assert.Equal(person.BirthDate, result.BirthDate);
            Assert.Equal(person.Age, result.Age);
            Assert.Equal(person.IsActive, result.IsActive);
            Assert.Equal(person.Height, result.Height);
            Assert.Equal(person.Weight, result.Weight);
            Assert.Equal(person.CreatedAt, result.CreatedAt);
            Assert.Equal(person.UpdatedAt, result.UpdatedAt);
            Assert.Equal(person.AdditionalInfo, result.AdditionalInfo);
            Assert.Equal(person.ProfileImage, result.ProfileImage);
            Assert.Equal(person.Salary, result.Salary);
            Assert.Equal(person.PhoneNumber, result.PhoneNumber);
            Assert.Equal(person.UniqueId, result.UniqueId);
        }
        [Fact]
        public void Execute_T_ExecuteReaderFilterSalaryAndCountResults_ReturnsMappedObjects()
        {
            var query = new Select() { Text = $"SELECT * FROM {TestData.TestTableName} WHERE Salary >= @minSalary;" };
            Action<DbCommand>? setup = (Command) =>
            {
                Command.Parameters.Add(new SQLiteParameter("@minSalary", 62000));
            };
            Func<DbDataReader, IEnumerable<DummyPerson>>? mapper = (reader) =>
            {
                var objs = new List<DummyPerson>();
                while (reader.Read())
                {
                    objs.Add(new()
                    {
                        Id = (int)reader["Id"],
                        FirstName = reader["FirstName"] as string,
                        LastName = reader["LastName"] as string,
                        BirthDate = (DateTime)reader["BirthDate"],
                        Age = (short)reader["Age"],
                        IsActive = (bool)reader["IsActive"],
                        Height = (double)reader["Height"],
                        Weight = (decimal)reader["Weight"],
                        CreatedAt = (DateTime)reader["CreatedAt"],
                        UpdatedAt = (DateTimeOffset)reader["UpdatedAt"],
                        AdditionalInfo = reader["AdditionalInfo"] as string,
                        ProfileImage = reader["ProfileImage"] as byte[],
                        Salary = (decimal)reader["Salary"],
                        PhoneNumber = reader["PhoneNumber"] as string,
                        UniqueId = (Guid)reader["UniqueId"]
                    });
                }
                return objs.AsEnumerable();
            };

            IEnumerable<DummyPerson>? result = gateway.Execute(query, setup, mapper);
            Assert.All(result ?? Enumerable.Empty<DummyPerson>(),
                e => Assert.True(e.Salary >= 62000)
                );
        }

        [Fact]
        public void ExecuteAsync_ExecuteReaderLoadedIntoDataTable_ReturnsDataTableWithColumnValues()
        {
            var query = new Select() { Text = $"SELECT * FROM {TestData.TestTableName} WHERE Salary >= @minSalary;" };
            Action<DbCommand>? setup = (Command) =>
            {
                Command.Parameters.Add(new SQLiteParameter("@minSalary", 62000));
            };
            Func<DbDataReader, DataTable>? mapper = (reader) =>
            {
                DataTable dataTable = new();
                dataTable.Load(reader);
                return dataTable;
            };

            DataTable? result = gateway.Execute(query, setup, mapper);
            Assert.NotNull(result);
            Assert.True(result.Rows.Count > 0, "No rows were loaded into the DataTable.");
        }
        #endregion

        #region Execute_IO
        [Fact]
        public void Execute_IO_ExecuteReaderFilterById_ReturnsMappedObjects()
        {
            int personId = 3;
            var query = new Select() { Text = $"SELECT * FROM {TestData.TestTableName} WHERE Id >= @personId;" };
            Action<DbCommand, int>? setup = (Command, id) =>
            {
                Command.Parameters.Add(new SQLiteParameter("@personId", (int)id));
            };
            var mapper = MapReaderToPerson;

            IEnumerable<DummyPerson>? result = gateway.Execute<int, IEnumerable<DummyPerson>>(query, personId, setup, mapper);

            Assert.All(result ?? Enumerable.Empty<DummyPerson>(),
                e => Assert.True(e.Id >= personId)
                );
        }

        [Fact]
        public void Execute_IO_ExecuteReaderFilterSalary_ReturnsMappedObjects()
        {
            Decimal salary = 62000;
            var query = new Select() { Text = $"SELECT * FROM {TestData.TestTableName} WHERE Salary >= @minSalary;" };
            Action<DbCommand, Decimal>? setup = (Command, minSalary) =>
            {
                Command.Parameters.Add(new SQLiteParameter("@minSalary", minSalary));
            };
            var mapper = MapReaderToPerson;

            IEnumerable<DummyPerson>? result = gateway.Execute<Decimal, IEnumerable<DummyPerson>>(query, salary, setup, mapper);
            Assert.All(result ?? Enumerable.Empty<DummyPerson>(),
                e => Assert.True(e.Salary >= 62000)
                );
        }

        [Fact]
        public void Execute_IO_ExecuteReaderFilterSalaryAndCountResults_ReturnsMappedObjectsAsyncronously()
        {
            Decimal salary = 62000;
            var query = new Select() { Text = $"SELECT COUNT(*) FROM {TestData.TestTableName} WHERE Salary >= @minSalary;" };
            Action<DbCommand, Decimal>? setup = (Command, minSalary) =>
            {
                Command.Parameters.Add(new SQLiteParameter("@minSalary", minSalary));
            };

            int result = gateway.Execute<Decimal, int>(query, salary, setup);
            Assert.Equal(3, result);
        }
        #endregion
    }
}



