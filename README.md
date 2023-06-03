# Sleek

[![Buy me a coffee](https://img.shields.io/badge/Buy%20Me%20A%20Coffee-donate-yellow.svg)](https://www.buymeacoffee.com/mmstiver)
![License: MIT](https://img.shields.io/badge/License-MIT-green.svg)


Sleek is a lightweight and easy-to-use library designed to simplify ADO.NET boilerplate code. It provides a minimalistic and straightforward approach to execute SQL queries and stored procedures, enabling developers to focus on their application logic. Sleek offers both synchronous and asynchronous execution methods, as well as support for custom command setup and result mapping.

# Table of Contents

- [Overview](#overview)
- [Getting Started](#getting-started)
- [Using the Sync Version](#using-the-sync-version)
  - [Executing a Query](#executing-a-query)
  - [Using a Setup Lambda](#using-a-setup-lambda)
  - [Using a Mapper](#using-a-mapper)
  - [Combining Setup and Mapper](#combining-setup-and-mapper)
- [Using the Async Version](#using-the-async-version)
- [Write Queries and DDL Queries](#write-queries-and-ddl-queries)
  - [Write Queries with a Setup Lambda](#write-queries-with-a-setup-lambda)
  - [DDL Queries](#ddl-queries)
- [Calling Stored Procedures](#calling-stored-procedures)
  - [Calling a Stored Procedure with a Mapper](#calling-a-stored-procedure-with-a-mapper)
  - [Calling a Stored Procedure with a Setup Lambda and Mapper](#calling-a-stored-procedure-with-a-setup-lambda-and-mapper)

## Overview

Sleek provides an abstraction over ADO.NET, offering a simplified API to interact with any database. It offers methods to execute SQL queries and stored procedures, along with optional setup and mapper functions to fine-tune command execution and result processing. In scenarios where you don't need a full ORM, and you just want to quickly script out some queries, Sleek offers the ability to create succinct C# data access code.

```
  var gateway = new SqlServerGateway(connectionString);
  var insertPersonSql = new Write(){Text = "INSERT INTO Person (Name, Age) OUTPUT INSERTED.Id VALUES (@Name, @Age)"};
  gateway.Execute(insertPersonSql, (cmd) =>
    {
        cmd.Parameters.AddWithValue("@Name", "Mike");
        cmd.Parameters.AddWithValue("@Age", 15);
    }, out int insertedId);
    
  var selectQuery = new Select(){Text = "SELECT * FROM Person WHERE Id = @Id"};
  gateway.Execute<Person>(selectQuery, (cmd) =>cmd.Parameters.AddWithValue("@Id", insertedId),
    (DbDataReader reader) => 
   {
      var person = new Person();
     while (reader.Read())
     {
         person.Id = reader.GetInt32(reader.GetOrdinal("Id"));
         person.Name = reader.GetString(reader.GetOrdinal("Name"));
         person.Age = reader.GetInt32(reader.GetOrdinal("Age"));
      }
      return person;
    });
```

Compared to the traditional ado.net code to query Sql Server, which can easily become a mess of nested brackets:

```    
using (SqlConnection conn = new SqlConnection(connectionString))
    {
        await conn.OpenAsync(cancellationToken);

        // INSERT
        string insertSql = "INSERT INTO Person (Name, Age) OUTPUT INSERTED.Id VALUES (@Name, @Age);";
        using (SqlCommand cmd = new SqlCommand(insertSql, conn))
        {
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Age", age);

            // Get the inserted id
            int insertedId = (int)await cmd.ExecuteScalarAsync(cancellationToken);

            // SELECT
            string selectSql = "SELECT * FROM Person WHERE Id = @Id";
            using (SqlCommand selectCmd = new SqlCommand(selectSql, conn))
            {
                selectCmd.Parameters.AddWithValue("@Id", insertedId);

                using (SqlDataReader reader = await selectCmd.ExecuteReaderAsync(cancellationToken))
                {
                    if (await reader.ReadAsync(cancellationToken))
                    {
                        return new Person()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Age = reader.GetInt32(reader.GetOrdinal("Age")),
                        };
                    }
                }
            }
        }
  }
}
```

## Getting Started

To start using Sleek, install the library via NuGet or Github and create an instance of SqlServerGateway (or another data gateway implementation, if available) with the appropriate connection string.

```csharp

using Sleek;

// Set up the data gateway
var connectionConfiguration = "...";
var dataGateway = new SqlServerGateway(connectionConfiguration);
```

## Synchronous Execution
### Executing a Query

To execute a simple SQL query or stored procedure, use the Execute method.

```csharp

var query = new Select("SELECT * FROM Users WHERE UserId = 1");
var result = dataGateway.Execute(query);
```

## Using a Setup Lambda

You can provide a setup lambda to configure the SqlCommand before executing the query.

```csharp

var query = new Select("SELECT * FROM Users WHERE UserId = @UserId");
var result = dataGateway.Execute(query, cmd => cmd.Parameters.AddWithValue("@UserId", 1));
```

### Using a Mapper

To map the result set to a custom data structure, provide a mapper function.

```

var query = new Select("SELECT * FROM Users");
var users = dataGateway.Execute<IEnumerable<User>>(query, reader =>
{
    var userList = new List<User>();
    while (reader.Read())
    {
        userList.Add(new User
        {
            UserId = reader.GetInt32(0),
            UserName = reader.GetString(1),
            Email = reader.GetString(2)
        });
    }
    return userList;
});
```

### Combining Setup and Mapper

You can combine setup and mapper functions to execute more complex queries and process the results.

```

var query = new Select("SELECT * FROM Users WHERE RoleId = @RoleId");
var users = dataGateway.Execute<IEnumerable<User>>(query, cmd => cmd.Parameters.AddWithValue("@RoleId", 1), reader =>
{
    var userList = new List<User>();
    while (reader.Read())
    {
        userList.Add(new User
        {
            UserId = reader.GetInt32(0),
            UserName = reader.GetString(1),
            Email = reader.GetString(2)
        });
    }
    return userList;
});
```

### Asynchronous Execution

Sleek also provides asynchronous counterparts for all synchronous methods. Simply replace the Execute method with ExecuteAsync and use the await keyword.

```

var query = new Select("SELECT * FROM Users WHERE UserId = 1");
var result = await dataGateway.ExecuteAsync(query);
```

For more complex examples, refer to the sections on using a setup lambda, mapper, or both in the synchronous execution part, and replace the Execute method with `
User
Add a section for using write queries (using a setup lambda), DDL queries and calling stored procs (using a mapper)


## Write Queries and DDL Queries

In addition to executing SELECT queries, Sleek also supports write queries (INSERT, UPDATE, DELETE) and Data Definition Language (DDL) queries (CREATE, ALTER, DROP).
Write Queries with a Setup Lambda

You can provide a setup lambda to configure the SqlCommand before executing a write query.

```

var writeQuery = new Write("INSERT INTO Users (UserName, Email) VALUES (@UserName, @Email)");
int affectedRows = dataGateway.Execute(writeQuery, cmd =>
{
    cmd.Parameters.AddWithValue("@UserName", "JohnDoe");
    cmd.Parameters.AddWithValue("@Email", "john.doe@example.com");
});
```

## DDL Queries

To execute DDL queries, use the Execute method with a DataDefinitionQuery instance.

```
var ddlQuery = new DataDefinitionQuery("CREATE TABLE TestTable (Id INT PRIMARY KEY, Name NVARCHAR(50))");
int result = dataGateway.Execute(ddlQuery);
```

## Calling Stored Procedures

Sleek simplifies calling stored procedures and handling their results.
Calling a Stored Procedure with a Mapper

To call a stored procedure and map the result set to a custom data structure, provide a mapper function.

```
var storedProcedure = new StoredProcedure("GetUsersByRole");
var users = dataGateway.Execute<IEnumerable<User>>(storedProcedure, reader =>
{
    var userList = new List<User>();
    while (reader.Read())
    {
        userList.Add(new User
        {
            UserId = reader.GetInt32(0),
            UserName = reader.GetString(1),
            Email = reader.GetString(2)
        });
    }
    return userList;
});
```

## Calling a Stored Procedure with a Setup Lambda and Mapper

You can combine setup and mapper functions when calling a stored procedure to configure the command and process the results.

```
var storedProcedure = new StoredProcedure("GetUsersByRole");
var users = dataGateway.Execute<IEnumerable<User>>(storedProcedure, cmd => cmd.Parameters.AddWithValue("@RoleId", 1), reader =>
{
    var userList = new List<User>();
    while (reader.Read())
    {
        userList.Add(new User
        {
            UserId = reader.GetInt32(0),
            UserName = reader.GetString(1),
            Email = reader.GetString(2)
        });
    }
    return userList;
});
```
