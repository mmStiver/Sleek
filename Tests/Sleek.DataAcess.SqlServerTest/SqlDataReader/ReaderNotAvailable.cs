using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleek.DataAcess.SqlServerTest.SqlDataReader
{
    public class ReaderNotAvailable
    {
        [Fact]
        public void Read_ReaderNotAvailable_ThrowsInvalidOperationException()
        {
            using (SqlConnection connection = new SqlConnection(""))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("", connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Close();
                        Assert.Throws<System.InvalidOperationException>(() => reader.Read());
                    }
                }
            }
        }
    }
}
