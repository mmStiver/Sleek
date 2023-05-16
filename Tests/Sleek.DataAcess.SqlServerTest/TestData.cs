using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Sleek.DataAcess.SqlServerTest
{
    internal static class TestData
    {
        public static readonly string TestDatabase = "SleekTestDB";
        internal static readonly string TestTableName = "DummyPerson";

        public static readonly string localConnection = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;Connect Timeout={0};";
        public static readonly string databaseConnection = @"Server=(localdb)\MSSQLLocalDB;Database={0};Integrated Security=true;Connect Timeout={1};";


        public static readonly string CreateDatabase = "CREATE DATABASE {0}";

        public static readonly string CreatePersonTable = """
            -- Create the Person table
            CREATE TABLE DummyPerson (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                FirstName NVARCHAR(50),
                LastName NVARCHAR(50),
                BirthDate DATE,
                Age SMALLINT,
                IsActive BIT,
                Height FLOAT,
                Weight DECIMAL(5, 2),
                CreatedAt DATETIME2,
                UpdatedAt DATETIMEOFFSET,
                AdditionalInfo XML,
                ProfileImage VARBINARY(MAX),
                Salary MONEY,
                PhoneNumber CHAR(10),
                UniqueId UNIQUEIDENTIFIER
            );
            """;
        public static readonly string CreateAddressTable = """
            -- Create the Person table
            CREATE TABLE Address (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                StreetAddress NVARCHAR(100) NOT NULL,
                City NVARCHAR(50) NOT NULL,
                State NVARCHAR(50) NULL,
                PostalCode NVARCHAR(20) NOT NULL,
                Country NVARCHAR(50) NOT NULL
            );
            """;

        public static readonly string
            CreatePhoneTable = """
                        CREATE TABLE PhoneNumber (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                Number CHAR(10) NOT NULL
            );
            """;

        public static readonly string InsertIntoPersonTable = """
            INSERT INTO DummyPerson (FirstName, LastName, BirthDate, Age, IsActive, Height, Weight, CreatedAt, UpdatedAt, AdditionalInfo, ProfileImage, Salary, PhoneNumber, UniqueId)
            VALUES ('John', 'Doe', '1987-05-01', 36, 1, 180.5, 75.2, '2023-05-01T10:30:00', '2023-05-01T10:30:00+00:00', '<info><hobby>Coding</hobby></info>', NULL, 55000, '5551234567', '0136c08d-e0c2-4f96-8ab5-7e0a8b4923af'),
            ('Jane', 'Doe', '1990-10-10', 32, 1, 165.3, 58.7, '2023-05-01T10:35:00', '2023-05-01T10:35:00+00:00', '<info><hobby>Painting</hobby></info>', NULL, 58000, '5552345678', 'd4bb9acc-02e0-4854-b10c-e3cb2773f246'),
            ('Alice', 'Johnson', '1992-03-15', 31, 0, 170.0, 62.5, '2023-05-01T10:40:00', '2023-05-01T10:40:00+00:00', '<info><hobby>Photography</hobby></info>', NULL, 62000, '5553456789', 'caec0826-e45c-4ef1-9086-cb8d8283a572'),
            ('Bob', 'Smith', '1985-08-20', 37, 1, 182.2, 80.5, '2023-05-01T10:45:00', '2023-05-01T10:45:00+00:00', '<info><hobby>Traveling</hobby></info>', NULL, 64000, '5554567890', '11804a12-2c9e-4690-af3a-0c2a806458e0'),
            ('Charlie', 'Brown', '1997-04-30', 25, 0, 175.8, 68.5, '2023-05-01T10:47:00','2023-05-01T10:53:00+00:00', '<info><hobby>Boardgames</hobby></info>', NULL, 67000, '2345678901', '2b32d8af-35bb-4e1c-84f1-3c66d063e958')
            """;

        public static readonly string InsertIntoAddressTable = """
            INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
            VALUES('987 Elm St', 'San Francisco', 'CA', '94102', 'USA'),
                ('654 Pine St', 'Austin', 'TX', '73301', 'USA');
            """;

        public static readonly string InsertIntoPhoneNumTable = """
            INSERT INTO PhoneNumber (Number)
            VALUES ('1234567890'),
            ('2345678901'),
            ('3456789012'),
            ('4567890123'),
            ('5678901234');
            """;

        public static readonly (string Name, string Code) DDLCreateTableProcedure = ("CreateTable", """
                CREATE OR ALTER PROCEDURE CreateTable
                AS
                BEGIN
                    CREATE TABLE #TempData
                    (
                        Id INT PRIMARY KEY IDENTITY(1,1)
                    );
                    SELECT * FROM #TempPerson
                    DROP TABLE #TempPerson
                END
                """);
        public static readonly (string Name, string Code) InsertAddressProcedure = ("InsertAddress", """
                CREATE OR ALTER PROCEDURE InsertAddress
                    @streetAddress NVARCHAR(50), 
                    @city NVARCHAR(50), 
                    @state NVARCHAR(50), 
                    @postalCode NVARCHAR(50), 
                    @country NVARCHAR(50)
                AS
                BEGIN
                    INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
                    VALUES(@streetAddress, @city, @state, @postalCode, @country);
                
                END
                """);
        public static readonly (string Name, string Code) UpdateAddressProcedure = ("UpdateAddress", """
                CREATE OR ALTER PROCEDURE UpdateAddress(
                    @addressId INT
                )
                AS
                BEGIN
                    UPDATE Address 
                    SET StreetAddress = '123 Fake Street'
                    WHERE Id = @addressId;
                END
                """);
        public static readonly (string Name, string Code) GetDateProcedure = ("GetDate", """
                CREATE OR ALTER PROCEDURE GetDate
                AS
                BEGIN                    
                    SELECT CAST('2023-05-03 14:30:00' AS datetime2(7));
                END
                """);
        public static readonly (string Name, string Code) GetDateOffsetProcedure = ("GetDateOffset", """
                CREATE OR ALTER PROCEDURE GetDateOffset
                AS
                BEGIN                    
                    SELECT CAST('2023-05-03 14:30:00' AS DateTimeOffset(7));
                END
                """);
        public static readonly (string Name, string Code) GetNullProcedure = ("GetNull", """
                CREATE OR ALTER PROCEDURE GetNull
                AS
                BEGIN                    
                    SELECT Null
                END
                """);
        public static readonly (string Name, string Code) GetIntProcedure = ("GetInt", """
                CREATE OR ALTER PROCEDURE GetInt
                AS
                BEGIN                    
                    SELECT 42
                END
                """);
        public static readonly (string Name, string Code) GetNumericProcedure = ("GetNumeric", """
                CREATE OR ALTER PROCEDURE GetNumeric
                AS
                BEGIN                    
                    SELECT 500.51
                END
                """);

        public static readonly (string Name, string Code) GetPersonProcedure = ("GetPerson", """
                CREATE OR ALTER PROCEDURE GetPerson
                (
                    @PersonId MONEY = NULL,
                    @MinSalary MONEY = NULL
                )
                AS
                BEGIN                    
                    SELECT * from dbo.DummyPerson 
                    WHERE (@PersonId IS NULL OR Id = @PersonId)
                    AND (@minSalary IS NULL OR Salary >= @minSalary)
                    ORDER BY 1 
                END
                """);
        public static readonly (string Name, string Code) GetUUIDProcedure = ("GetUUID", """
                CREATE OR ALTER PROCEDURE GetUUID
                AS
                BEGIN                    
                    SELECT NEWID();
                END
                """);
    }
}