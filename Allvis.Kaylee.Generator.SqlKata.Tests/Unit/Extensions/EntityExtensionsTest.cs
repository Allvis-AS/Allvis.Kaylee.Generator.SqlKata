using Xunit;
using Xunit.Categories;
using Allvis.Kaylee.Generator.SqlKata.Extensions;
using Allvis.Kaylee.Generator.SqlKata.Tests.Fixtures;
using System.Linq;
using Allvis.Kaylee.Analyzer;

namespace Allvis.Kaylee.Generator.SqlKata.Tests.Unit.Extensions
{
    [UnitTest]
    public class EntityExtensionsTest
    {
        [Fact]
        public void TestGetFullPrimaryKey()
        {
            // Arrange
            var tSchema = AuthSchemaFixture.Create();
            var ast = KayleeHelper.Parse(tSchema);
            var userRole = ast.Locate("auth", new[] { "User", "Role" });
            // Act
            var fullPrimaryKey = userRole.GetFullPrimaryKey();
            // Assert
            var expected = new[] {
                userRole.Parent.PrimaryKey.Single(),
                userRole.PrimaryKey.Single()
            };
            Assert.Equal(expected, fullPrimaryKey);
        }
    }
}