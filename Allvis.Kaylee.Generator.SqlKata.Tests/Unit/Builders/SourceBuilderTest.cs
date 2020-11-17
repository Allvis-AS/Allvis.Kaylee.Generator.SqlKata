using Xunit;
using Xunit.Categories;
using Allvis.Kaylee.Generator.SqlKata.Builders;

namespace Allvis.Kaylee.Generator.SqlKata.Tests.Unit.Builders
{
    [UnitTest]
    public class SourceBuilderTest
    {
        [Fact]
        public void TestComplete()
        {
            // Arrange
            var sb = new SourceBuilder();
            // Act
            sb.AL("namespace foo");
            sb.B(sb =>
            {
                sb.AL("public class Bar");
                sb.B(sb =>
                {
                    sb.A("System.Console.WriteLine(", indent: true);
                    sb.A("\"Hello World!\"");
                    sb.A(");");
                    sb.NL();
                    sb.I(sb =>
                    {
                        sb.AL("// Indented comment");
                    });
                });
            });
            var value = sb.ToString();
            // Assert
            Assert.Equal(@"namespace foo
{
    public class Bar
    {
        System.Console.WriteLine(""Hello World!"");
            // Indented comment
    }
}
", value);
        }
    }
}