using Sleek.DataAccess.Core.Command;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Sleek.DataAccess.Core
{
    public interface ISyncDataGateway : IProcedureDataGateway, IQueryDataGateway
    {
       
        bool TestConnection();
    }
}