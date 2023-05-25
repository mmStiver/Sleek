using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Sleek.DataAccess.SQLiteTest
{
    internal static class TestData
    {
        internal static readonly string TestTableName = "DummyPerson";

        public static readonly string localConnection = @"Data Source=:memory:;Version=3";
        public static readonly string nonexistantConnection = @"Data Source=/path/to/database.db;Version=3;";

        public static readonly string CreatePersonTable = """
            CREATE TABLE DummyPerson (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FirstName TEXT,
                LastName TEXT,
                BirthDate DATE,
                Age INTEGER,
                IsActive INTEGER,
                Height REAL,
                Weight NUMERIC(5, 2),
                CreatedAt TEXT,
                UpdatedAt TEXT,
                AdditionalInfo TEXT,
                ProfileImage BLOB,
                Salary NUMERIC,
                PhoneNumber TEXT,
                UniqueId TEXT
            );
            """;
        public static readonly string CreateAddressTable = """
            -- Create the Person table
                CREATE TABLE Address (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                StreetAddress NVARCHAR(100) NOT NULL,
                City NVARCHAR(50) NOT NULL,
                State NVARCHAR(50) NULL,
                PostalCode NVARCHAR(20) NOT NULL,
                Country NVARCHAR(50) NOT NULL
            );
            """;

        public static readonly string CreatePhoneTable = """
            CREATE TABLE PhoneNumber (Id INTEGER PRIMARY KEY AUTOINCREMENT, Value TEXT)
         """;
        public static readonly string InsertPhoneTable = """ 
        INSERT INTO PhoneNumber (Value) VALUES ('1234567890'),('9876543210'),('4567890123'),('7890123456'),('2345678901')
        """;
        public static readonly string InsertIntoAddressTable = """
            INSERT INTO Address (StreetAddress, City, State, PostalCode, Country)
            VALUES('987 Elm St', 'San Francisco', 'CA', '94102', 'USA'),
                ('654 Pine St', 'Austin', 'TX', '73301', 'USA');
            """;
        public static readonly string InsertIntoPersonTable = """
            INSERT INTO DummyPerson (FirstName, LastName, BirthDate, Age, IsActive, Height, Weight, CreatedAt, UpdatedAt,AdditionalInfo,ProfileImage,Salary,PhoneNumber,UniqueId)
            VALUES ('John', 'Doe', '1980-06-15', 30, 1, 1.75, 75.5, '2023-05-01', '2023-05-01', 'Some additional info 1', x'0123456789ABCDEF', 50000.50, '1234567890', 'ABC123'),
            ('Jane', 'Smith', '1992-02-28', 29, 0, 1.63, 61.2, '2023-05-02', '2023-05-03', 'Some additional info 2', x'FEDCBA9876543210', 63000.75, '9876543210', 'DEF456'),
            ('Michael', 'Johnson', '1985-11-10', 36, 1, 1.82, 82.7, '2023-05-03', '2023-05-04', 'Some additional info 3', x'1122334455667788', 75000.25, '4567890123', 'GHI789'),
            ('Emily', 'Brown', '1998-08-22', 23, 1, 1.67, 54.9, '2023-05-04', '2023-05-05', 'Some additional info 4', x'AABBCCDDEEFF0011', 40000.80, '7890123456', 'JKL012'),
            ('Daniel', 'Wilson', '1995-04-03', 26, 0, 1.79, 68.3, '2023-05-05', '2023-05-06', 'Some additional info 5', x'0011223344556677', 85000.60, '2345678901', 'MNO345');
            """;

    }
}