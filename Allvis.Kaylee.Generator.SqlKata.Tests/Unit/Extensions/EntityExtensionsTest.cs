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
        public void TestGetViewName()
        {
            // Arrange
            var tSchema = AuthSchemaFixture.Create();
            var ast = KayleeHelper.Parse(tSchema);
            var userRole = ast.Locate("auth", new[] { "User", "Role" });
            // Act
            var viewName = userRole.GetViewName();
            // Assert
            Assert.Equal("auth.v_UserRole", viewName);
        }

        [Fact]
        public void TestGetTableName()
        {
            // Arrange
            var tSchema = AuthSchemaFixture.Create();
            var ast = KayleeHelper.Parse(tSchema);
            var userRole = ast.Locate("auth", new[] { "User", "Role" });
            // Act
            var viewName = userRole.GetTableName();
            // Assert
            Assert.Equal("auth.tbl_UserRole", viewName);
        }

        [Fact]
        public void TestGetModelName()
        {
            // Arrange
            var tSchema = AuthSchemaFixture.Create();
            var ast = KayleeHelper.Parse(tSchema);
            var userRole = ast.Locate("auth", new[] { "User", "Role" });
            // Act
            var modelName = userRole.GetModelName();
            // Assert
            Assert.Equal("UserRole", modelName);
        }
    }
}