using Xunit;
using Xunit.Categories;
using Allvis.Kaylee.Generator.SqlKata.Extensions;

namespace Allvis.Kaylee.Generator.SqlKata.Tests.Unit.Extensions
{
    [UnitTest]
    public class GeneratorExecutionContextExtensionsTest
    {
        [Theory]
        [InlineData(true, "core.kay")]
        [InlineData(true, "/temp/auth.kay")]
        [InlineData(false, "/temp/auth")]
        [InlineData(false, "/temp/auth.model")]
        public void TestIsKayleeFile(bool expected, string path)
        {
            // Act
            var isKayleeFile = GeneratorExecutionContextExtensions.IsKayleeFile(path);
            // Assert
            Assert.Equal(expected, isKayleeFile);
        }
    }
}