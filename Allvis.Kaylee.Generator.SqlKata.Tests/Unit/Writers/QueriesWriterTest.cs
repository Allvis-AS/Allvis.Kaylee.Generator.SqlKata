using Xunit;
using Xunit.Categories;
using Allvis.Kaylee.Generator.SqlKata.Writers;
using System.IO;
using Allvis.Kaylee.Generator.SqlKata.Tests.Fixtures;
using System.Threading.Tasks;
using System.Diagnostics;
using Allvis.Kaylee.Generator.SqlKata.Utilities;
using Allvis.Kaylee.Analyzer;

namespace Allvis.Kaylee.Generator.SqlKata.Tests.Unit.Extensions
{
    [UnitTest]
    public class QueriesWriterTest
    {
        [Fact]
        public async Task TestWrite()
        {
            // Arrange
            var tSchema = AuthSchemaFixture.Create();
            var ast = KayleeHelper.Parse(tSchema);
            // Act
            var source = QueriesWriter.Write(ast);
            // Assert
            await DebugUtils.WriteGeneratedFileToDisk("Queries.cs", source).ConfigureAwait(false);
            Assert.Equal(@"namespace Allvis.Kaylee.Generated.SqlKata
{
    public static class Queries
    {
        public static SqlKata.Query auth_User_Exists(System.Guid userId)
        {
            return new SqlKata.Query(""auth.v_User"")
                .Where(""UserId"", userId)
                .SelectRaw(""1"")
                .Limit(1);
        }
        public static SqlKata.Query auth_UserRole_Exists(System.Guid userId, System.Guid roleId)
        {
            return new SqlKata.Query(""auth.v_UserRole"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .SelectRaw(""1"")
                .Limit(1);
        }
    }
}
", source);
        }
    }
}