using System.Data;
using System.Data.SQLite;
using Xunit.Extensions;
using Xunit.Abstractions;
using Xunit.Sdk;
using Sleek.DataAcess;
using System.Data.Common;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using static System.Net.Mime.MediaTypeNames;

namespace Sleek.DataAccess.SQLiteTest
{

    public class SQLiteTestFixture : IDisposable
    {
        public readonly SQLiteConnection connection;
        public SQLiteTestFixture()
        {
            connection = new SQLiteConnection(TestData.localConnection);
            connection.Open();
            using (var command1 = new SQLiteCommand(@"PRAGMA journal_mode = 'wal'", connection))
                command1.ExecuteNonQuery();

            using (var command = new SQLiteCommand(TestData.CreatePersonTable, connection))
            {
                var resl = command.ExecuteNonQuery();

                Console.WriteLine(resl);
            }

            using (var command = new SQLiteCommand(TestData.InsertIntoPersonTable, connection))
            {
                command.ExecuteNonQuery();
            }

            byte[] testData = { 0x12, 0x34, 0x56, 0x78, 0x90 };
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = $"UPDATE {TestData.TestTableName} SET ProfileImage = @blobData WHERE Id = 1";
                command.Parameters.AddWithValue("@blobData", testData);
                command.ExecuteNonQuery();

            }
        }
        

        public void Dispose()
        {
            if (connection.State != System.Data.ConnectionState.Closed)
                connection.Close();

            connection.Dispose();
        }
    }
}