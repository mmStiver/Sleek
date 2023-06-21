
using Sleek.DataAccess.Core;
using Sleek.DataAccess.Core.Command;
using System.Data.Common;

namespace Sleek.DataAccess.SqlServer
{
    public interface ISqlServerGateway : IAsyncDataGateway, ISyncDataGateway
    {
    }
}
