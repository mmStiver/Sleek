using NuGet.Frameworks;

namespace Sleek.DataAccess.Command
{
    public class InsertTest
    {
        [Fact]
        public Insert InsertCtor_InsertQueryString_AllGood()
        =>
            Assert.IsType<Insert>(new Insert("iNsErT InTo"));


        [Fact]
        public Exception InsertCtor_EmptyString_ThrowsException()
        =>
            Assert.Throws<ArgumentException>(() => new Insert(""));
        

        [Fact]
        public Exception InsertCtor_Jibberish_ThrowsException()
        =>
            Assert.Throws<ArgumentException>(() => new Insert("jibberish 5548769 445;"));
        

        [Fact]
        public Exception InsertCtor_CSharpCode_ThrowsException()
        =>
            Assert.Throws<ArgumentException>(() => new Insert("public Exception MyMethod() => "));
        

        [Fact]
        public Exception InsertCtor_SelectQuery_ThrowsException()
        =>
            Assert.Throws<ArgumentException>(() => new Insert("SELECT * FROM MyTable"));
        

        [Fact]
        public Exception Create_EmptyString_ThrowsException()
        =>
            Assert.Throws<ArgumentException>(() => Insert.Create(""));
        

        [Fact]
        public Exception Create_Jibberish_ThrowsException()
        =>
            Assert.Throws<ArgumentException>(() => Insert.Create("jibberish 5548769 445;"));
        

        [Fact]
        public Exception Create_CSharpCode_ThrowsException()
        =>
            Assert.Throws<ArgumentException>(() => Insert.Create("public Exception MyMethod() => "));
        

        [Fact]
        public Exception Create_SqlQuery_ThrowsException()
        =>
            Assert.Throws<ArgumentException>(() => Insert.Create("SELECT * FROM MyTable"));
        

        [Fact]
        public Exception Cast_EmptyString_ThrowsException()
        =>
            Assert.Throws<ArgumentException>(() => (Insert)"");
        

        [Fact]
        public Exception Cast_Jibberish_ThrowsException()
        =>
            Assert.Throws<ArgumentException>(() => (Insert)"jibberish 5548769 445;");
        

        [Fact]
        public Exception Cast_CSharpCode_ThrowsException()
        => Assert.Throws<ArgumentException>(() => (Insert)"public int MyMethod() => ");
        

        [Fact]
        public Exception Cast_SqlQuery_ThrowsException()
        =>
            Assert.Throws<ArgumentException>(() => (Insert)"SELECT * FROM MyTable");
        
    }
}