namespace Sleek.DataAccess.Core.Command
{
    public class Insert : Write
    {
        public Insert(string Text) : base(Text) {
            if (Text.Equals(String.Empty, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException();
            if (!Text.StartsWith("insert Into", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException();

        }


        public static explicit operator Insert(string InputQuery)
         => new Insert(InputQuery);

        public static explicit operator String(Insert InputQuery)
            => InputQuery.Text;

        public static void Create(string v)
        => new Insert(v);
    }
}
