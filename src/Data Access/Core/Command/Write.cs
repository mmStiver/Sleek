namespace Sleek.DataAccess.Core.Command
{
    public class Write : DataCommand
    {
        public string Text { get; init; } = String.Empty;
        public Write(){}

        public Write(string Text)
        {
            if (Text.Equals(String.Empty, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException();
            this.Text = Text;
        }

        public static explicit operator Write(string InputQuery)
         => new Write(InputQuery);

        public static explicit operator String(Write InputQuery)
            => InputQuery.Text;
    }
}
