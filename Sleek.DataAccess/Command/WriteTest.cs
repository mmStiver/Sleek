using NuGet.Frameworks;

namespace Sleek.DataAccess.Command
{
    public class WriteTest
    {
        [Fact]
        public Exception Write_CastFromEmptyString_ThrowsException()
            => Assert.Throws<ArgumentException>(()=> (Write)"" );
    }
}