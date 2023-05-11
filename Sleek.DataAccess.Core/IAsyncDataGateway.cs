using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleek.DataAccess.Core
{
    public interface IAsyncDataGateway : IQueryAsyncDataGateway, IProcedureAsyncDataGateway
    {
        /// <summary>
        /// Tests the database connection asynchronously.
        /// </summary>
        /// <param name="cancellationToken">An optional cancellation token to cancel the operation.</param>
        /// <returns>A Task representing a boolean value indicating whether the connection was successful (true) or not (false).</returns>
        Task<bool> TestConnectionAsync(CancellationToken cancellationToken = default);

    }
}
