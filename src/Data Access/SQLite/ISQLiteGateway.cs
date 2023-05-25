using Sleek.DataAccess.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleek.DataAccess.SQLite
{
    public interface ISQLiteGateway : IQueryDataGateway
    {
        bool TestConnection();
    }
}
