using Xunit;
using Xunit.Categories;
using Allvis.Kaylee.Generator.SqlKata.Extensions;
using Allvis.Kaylee.Generator.SqlKata.Builders;

namespace Allvis.Kaylee.Generator.SqlKata.Tests.Unit.Extensions
{
    [UnitTest]
    public class SourceBuilderExtensionsTest
    {
        [Fact]
        public void TestPublicClass()
        {
            // Arrange
            var sb = new SourceBuilder();
            // Act
            sb.PublicClass("Hello.World", "Foo", sb =>
            {
                sb.AL("System.Console.WriteLine(\"Hello World!\");");
            });
            // Assert
            Assert.Equal(@"namespace Hello.World
{
    public class Foo
    {
        System.Console.WriteLine(""Hello World!"");
    }
}
", sb.ToString());
        }

        [Fact]
        public void TestPublicStaticClass()
        {
            // Arrange
            var sb = new SourceBuilder();
            // Act
            sb.PublicStaticClass("Hello.World", "Foo", sb =>
            {
                sb.AL("System.Console.WriteLine(\"Hello World!\");");
            });
            // Assert
            Assert.Equal(@"namespace Hello.World
{
    public static class Foo
    {
        System.Console.WriteLine(""Hello World!"");
    }
}
", sb.ToString());
        }

        [Fact]
        public void TestPublicStaticMethod_Optional()
        {
            // Arrange
            var sb = new SourceBuilder();
            // Act
            sb.PublicStaticMethod("bool", "IsHappy", new[] { (true, "string", "roommate"), (false, "int", "age") }, sb =>
            {
                sb.AL("return roommate == \"Jacob\" && age < 20;");
            });
            // Assert
            Assert.Equal(@"public static bool IsHappy(string? roommate, int age)
{
    return roommate == ""Jacob"" && age < 20;
}
", sb.ToString());
        }

        [Fact]
        public void TestPublicStaticMethod()
        {
            // Arrange
            var sb = new SourceBuilder();
            // Act
            sb.PublicStaticMethod("bool", "IsHappy", new[] { ("string", "roommate"), ("int", "age") }, sb =>
            {
                sb.AL("return roommate == \"Jacob\" && age < 20;");
            });
            // Assert
            Assert.Equal(@"public static bool IsHappy(string roommate, int age)
{
    return roommate == ""Jacob"" && age < 20;
}
", sb.ToString());
        }
    }
}