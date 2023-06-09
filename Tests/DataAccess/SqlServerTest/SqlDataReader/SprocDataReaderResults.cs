﻿using Sleek.DataAcess.SqlServerTest;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.SqlServerTest.SqlDataReader
{
    [Collection("SQL Server Database collection")]
    public class SprocsDataReaderResults 
    {

        ISqlServerGateway facade;
        private readonly DummyPerson person;

        public SprocsDataReaderResults(SqlServerTestFixture context)
        {
            facade = new SqlServerGateway(context.connectionString);
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

        #region ExecuteAsync_T
        [Fact]
        public async Task ExecuteAsync_T_ExecuteReaderOnTable_ReturnsAllRows()
        {
            var proc = new StoredProcedure() { Name = "GetPerson" };
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

            IEnumerable<DummyPerson>? result = await facade.ExecuteAsync(proc, mapper);
            Assert.IsType<List<DummyPerson>>(result);
            var list = result as List<DummyPerson>;
            Assert.NotNull(list);
            Assert.Equal(5, list.Count);
        }
        [Fact]
        public async Task ExecuteAsync_T_ExecuteReaderOnTable_ReturnsMappedObjects()
        {
            var proc = new StoredProcedure() { Name = "GetPerson" };
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

            IEnumerable<DummyPerson>? result = await facade.ExecuteAsync(proc, mapper);
            Assert.Collection(result ?? Enumerable.Empty<DummyPerson>(),
                e => Assert.Equal(1, e.Id),
                e => Assert.Equal(2, e.Id),
                e => Assert.Equal(3, e.Id),
                e => Assert.Equal(4, e.Id),
                e => Assert.Equal(5, e.Id)
                );

        }

        [Fact]
        public async Task ExecuteAsync_T_ExecuteReaderOnTableById_ReturnsMappedObject()
        {
            var proc = new StoredProcedure() { Name = "GetPerson" };

            Func<DbDataReader, DummyPerson>? mapper = (reader) =>
            {
                reader.Read();
               
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
            DummyPerson? result = await facade.ExecuteAsync(proc, Mapper: mapper);
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
        public async Task ExecuteAsync_T_ExecuteReaderFilterSalaryAndCountResults_ReturnsMappedObjects()
        {
            var proc = new StoredProcedure() { Name = "GetPerson" };
            Action<DbCommand>? setup = (Command) =>
            {
                Command.Parameters.Add(new SqlParameter("@minSalary", 62000));
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

            IEnumerable<DummyPerson>? result = await facade.ExecuteAsync(proc, setup, mapper);
            Assert.All(result ?? Enumerable.Empty<DummyPerson>(),
                e => Assert.True(e.Salary >= 62000)
                );
        }

        [Fact]
        public async Task ExecuteAsync_T_ExecuteReaderLoadedIntoDataTable_ReturnsDataTableWithColumnValues()
        {
            var proc = new StoredProcedure() { Name = "GetPerson" };
            Action<DbCommand>? setup = (Command) =>
            {
                Command.Parameters.Add(new SqlParameter("@minSalary", 62000));
            };
            Func<DbDataReader, DataTable>? mapper = (reader) =>
            {
                DataTable dataTable = new();
                dataTable.Load(reader);
                return dataTable;
            };

            DataTable? result = await facade.ExecuteAsync(proc, setup, mapper);
            Assert.NotNull(result);
            Assert.True(result.Rows.Count > 0, "No rows were loaded into the DataTable.");
        }


        [Fact]
        public async Task ExecuteAsync_T_ExecuteReaderOnTable_ReturnsAllRowsAsyncronously()
        {
            var proc = new StoredProcedure() { Name = "GetPerson" };
            Func<DbDataReader, Task<IEnumerable<DummyPerson>>>? mapper = async (reader) =>
            {
                var objs = new List<DummyPerson>();
                while (await reader.ReadAsync())
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

            IEnumerable<DummyPerson>? result = await facade.ExecuteAsync(proc, mapper);
            Assert.IsType<List<DummyPerson>>(result);
            var list = result as List<DummyPerson>;
            Assert.NotNull(list);
            Assert.Equal(5, list.Count);
        }
        [Fact]
        public async Task ExecuteAsync_T_ExecuteReaderOnTable_ReturnsMappedObjectsAsyncronously()
        {
            var proc = new StoredProcedure() { Name = "GetPerson" };
            Func<DbDataReader, Task<IEnumerable<DummyPerson>>>? mapper = async (reader) =>
            {
                var objs = new List<DummyPerson>();
                while (await reader.ReadAsync())
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

            IEnumerable<DummyPerson>? result = await facade.ExecuteAsync(proc, mapper);
            Assert.Collection(result ?? Enumerable.Empty<DummyPerson>(),
                e => Assert.Equal(1, e.Id),
                e => Assert.Equal(2, e.Id),
                e => Assert.Equal(3, e.Id),
                e => Assert.Equal(4, e.Id),
                e => Assert.Equal(5, e.Id)
                );

        }

        [Fact]
        public async Task ExecuteAsync_T_ExecuteReaderOnTableById_ReturnsMappedObjectAsyncronously()
        {
            var proc = new StoredProcedure() { Name = "GetPerson" };

            Func<DbDataReader, Task<DummyPerson>>? mapper = async (reader) =>
            {
                await reader.ReadAsync();
                
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
            DummyPerson? result = await facade.ExecuteAsync(proc, Mapper: mapper);
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
        public async Task ExecuteAsync_T_ExecuteReaderFilterSalaryAndCountResults_ReturnsMappedObjectsAsyncronously()
        {
            var proc = new StoredProcedure() { Name = "GetPerson" };
            Action<DbCommand>? setup = (Command) =>
            {
                Command.Parameters.Add(new SqlParameter("@minSalary", 62000));
            };
            Func<DbDataReader, Task<IEnumerable<DummyPerson>>>? mapper = async (reader) =>
            {
                var objs = new List<DummyPerson>();
                while (await reader.ReadAsync())
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

            IEnumerable<DummyPerson>? result = await facade.ExecuteAsync(proc, setup, mapper);
            Assert.All(result ?? Enumerable.Empty<DummyPerson>(),
                e => Assert.True(e.Salary >= 62000)
                );
        }
        #endregion

        #region Execute_T
        [Fact]
        public void Execute_T_ExecuteReaderOnTable_ReturnsAllRows()
        {
            var proc = new StoredProcedure() { Name = "GetPerson" };
            Func<DbDataReader, IEnumerable<DummyPerson>> mapper = (reader) =>
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

            IEnumerable<DummyPerson>? result = facade.Execute(proc, mapper);
            Assert.IsType<List<DummyPerson>>(result);
            var list = result as List<DummyPerson>;
            Assert.NotNull(list);
            Assert.Equal(5, list.Count);
        }
        [Fact]
        public void Execute_T_ExecuteReaderOnTable_ReturnsMappedObjects()
        {
            var proc = new StoredProcedure() { Name = "GetPerson" };
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

            IEnumerable<DummyPerson>? result = facade.Execute(proc, mapper);
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
            var proc = new StoredProcedure() { Name = "GetPerson" };
            //{ Name = $"SELECT * FROM {TestData.TestTableName} WHERE Id = 1;" };

            Func<DbDataReader, DummyPerson>? mapper = (reader) =>
            {
                reader.Read();
                
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
            DummyPerson? result = facade.Execute(proc, mapper);
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
            var proc = new StoredProcedure() { Name = "GetPerson" };
            Action<DbCommand>? setup = (Command) =>
            {
                Command.Parameters.Add(new SqlParameter("@minSalary", 62000));
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

            IEnumerable<DummyPerson>? result = facade.Execute(proc, setup, mapper);
            Assert.All(result ?? Enumerable.Empty<DummyPerson>(),
                e => Assert.True(e.Salary >= 62000)
                );
        }

        [Fact]
        public void ExecuteAsync_ExecuteReaderLoadedIntoDataTable_ReturnsDataTableWithColumnValues()
        {
            var proc = new StoredProcedure() { Name = "GetPerson" };
            Action<DbCommand>? setup = (Command) =>
            {
                Command.Parameters.Add(new SqlParameter("@minSalary", 62000));
            };
            Func<DbDataReader, DataTable>? mapper = (reader) =>
            {
                DataTable dataTable = new();
                dataTable.Load(reader);
                return dataTable;
            };

            DataTable? result = facade.Execute(proc, setup, mapper);
            Assert.NotNull(result);
            Assert.True(result.Rows.Count > 0, "No rows were loaded into the DataTable.");
        }
        #endregion

    }
}



