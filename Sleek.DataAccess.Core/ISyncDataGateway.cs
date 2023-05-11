using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.Core
{
    public interface ISyncDataGateway : IProcedureDataGateway, IQueryDataGateway
    {
        /// <summary>
        /// Tests the database connection.
        /// </summary>
        /// <returns>A boolean value indicating whether the connection was successful (true) or not (false).</returns>
        bool TestConnection();
    }
}