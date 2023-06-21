using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sleek.DataAccess.Core.Command
{
    public class DataDefinitionQuery : DataCommand
    {
        public string Text;

        public DataDefinitionQuery() { }

        public DataDefinitionQuery(string Text) => this.Text = Text;

        public static explicit operator DataDefinitionQuery(string InputQuery)
         => new DataDefinitionQuery(InputQuery);

        public static explicit operator String(DataDefinitionQuery InputQuery)
            => InputQuery.Text;
    }
}
