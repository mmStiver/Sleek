using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleek.DataAcess.SqlServerTest.SqlCommandTest
{
    public class IllegalCommand
    {


        public IllegalCommand() { 
        
        }
      

        [Fact]
        public void ExecuteReader_CommandNotAssociatedWithConnection_ThrowsInvalidOperationException()
        {
            using (SqlCommand command = new SqlCommand("", new SqlConnection("")))
            {
                Assert.Throws<System.InvalidOperationException>(() => command.ExecuteReader());
            }
        }

        [Fact]
        public void ExecuteNonQuery_CommandNotAssociatedWithConnection_ThrowsInvalidOperationException()
        {
            using (SqlCommand command = new SqlCommand("", new SqlConnection("")))
            {
                Assert.Throws<System.InvalidOperationException>(() => command.ExecuteNonQuery());
            }
        }

    }
}
