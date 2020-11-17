using Xunit;
using Xunit.Categories;
using Allvis.Kaylee.Generator.SqlKata.Extensions;

namespace Allvis.Kaylee.Generator.SqlKata.Tests.Unit.Extensions
{
    [UnitTest]
    public class StringExtensionsTest
    {
        [Theory]
        [InlineData("  Hello World!", "Hello World!", 1, 2)]
        [InlineData("    Hello World!", "Hello World!", 1, 4)]
        [InlineData("    Hello World!", "Hello World!", 2, 2)]
        public void TestIndent(string expected, string value, int levels, int spaces)
        {
            // Act
            var indented = value.Indent(levels, spaces);
            // Assert
            Assert.Equal(expected, indented);
        }

        [Fact]
        public void TestJoin_Parameters()
        {
            // Arrange
            var parameters = new [] {("string", "foo"), ("int", "bar")};
            // Act
            var joined = parameters.Join();
            // Assert
            Assert.Equal("string foo, int bar", joined);
        }
    }
}