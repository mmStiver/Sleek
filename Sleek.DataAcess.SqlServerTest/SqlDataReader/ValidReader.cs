using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleek.DataAcess.SqlServerTest.SqlDataReader
{
    public class ValidReader
    {
        [Fact]
        public void Read_ValidReader_ReturnsTrue()
        {
            using (SqlConnection connection = new SqlConnection(""))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        bool hasRows = reader.Read();
                        Assert.True(hasRows);
                    }
                }
            }
        }

        [Fact]
        public void NextResult_ValidReader_ReturnsTrue()
        {
            using (SqlConnection connection = new SqlConnection(""))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        bool hasResultSets = reader.NextResult();
                        Assert.True(hasResultSets);
                    }
                }
            }
        }

       
    }
}