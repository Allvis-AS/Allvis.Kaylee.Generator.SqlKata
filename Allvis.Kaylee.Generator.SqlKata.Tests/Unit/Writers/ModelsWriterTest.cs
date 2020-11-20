using Xunit;
using Xunit.Categories;
using Allvis.Kaylee.Generator.SqlKata.Writers;
using System.IO;
using Allvis.Kaylee.Generator.SqlKata.Tests.Fixtures;
using System.Threading.Tasks;
using Allvis.Kaylee.Generator.SqlKata.Utilities;
using Allvis.Kaylee.Analyzer;

namespace Allvis.Kaylee.Generator.SqlKata.Tests.Unit.Extensions
{
    [UnitTest]
    public class ModelsWriterTest
    {
        [Fact]
        public async Task TestWrite()
        {
            // Arrange
            var tSchema = AuthSchemaFixture.Create();
            var ast = KayleeHelper.Parse(tSchema);
            // Act
            var models = ModelsWriter.Write(ast);
            // Assert
            foreach (var model in models)
            {
                var path = Path.Combine("Models", $"{model.HintName}.cs");
                await DebugUtils.WriteGeneratedFileToDisk(path, model.Source).ConfigureAwait(false);
            }
            Assert.Collection(models, model =>
            {
                Assert.Equal("Allvis.Kaylee.Generated.SqlKata.Models.auth.User", model.HintName);
                Assert.Equal(@"#nullable enable

namespace Allvis.Kaylee.Generated.SqlKata.Models.auth
{
    public class User
    {
        public global::System.Guid UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string ContactEmail { get; set; } = string.Empty;
        public string NormalizedContactEmail { get; set; } = string.Empty;
        public byte[] Hash { get; set; } = global::System.Array.Empty<byte>();
        public byte[]? Picture { get; set; }
    }
}
", model.Source);
            }, model =>
            {
                Assert.Equal("Allvis.Kaylee.Generated.SqlKata.Models.auth.UserRole", model.HintName);
                Assert.Equal(@"#nullable enable

namespace Allvis.Kaylee.Generated.SqlKata.Models.auth
{
    public class UserRole
    {
        public global::System.Guid UserId { get; set; }
        public global::System.Guid RoleId { get; set; }
        public int Flag { get; set; }
    }
}
", model.Source);
            });
        }
    }
}