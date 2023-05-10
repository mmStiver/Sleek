using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleek.DataAccess.Core
{
    public interface IAsyncDataGateway : IQueryAsyncDataGateway, IProcedureAsyncDataGateway
    {
        Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default);

    }
}
