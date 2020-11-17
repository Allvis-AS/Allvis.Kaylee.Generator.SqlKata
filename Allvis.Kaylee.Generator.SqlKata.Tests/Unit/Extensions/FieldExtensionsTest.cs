using Xunit;
using Xunit.Categories;
using Allvis.Kaylee.Generator.SqlKata.Extensions;
using Allvis.Kaylee.Generator.SqlKata.Tests.Fixtures;
using Allvis.Kaylee.Analyzer;

namespace Allvis.Kaylee.Generator.SqlKata.Tests.Unit.Extensions
{
    [UnitTest]
    public class FieldExtensionsTest
    {
        [Theory]
        [InlineData(true, "UserId")]
        [InlineData(false, "FirstName")]
        [InlineData(false, "ContactEmail")]
        public void TestHasDefault(bool expected, string fieldName)
        {
            // Arrange
            var tSchema = AuthSchemaFixture.Create();
            var ast = KayleeHelper.Parse(tSchema);
            var user = ast.Locate("auth", new[] { "User" });
            var field = user.Locate(fieldName);
            // Act
            var hasDefault = field.HasDefault();
            // Assert
            Assert.Equal(expected, hasDefault);
        }
    }
}