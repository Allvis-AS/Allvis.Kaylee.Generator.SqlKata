using Xunit;
using Xunit.Categories;
using Allvis.Kaylee.Generator.SqlKata.Writers;
using Allvis.Kaylee.Generator.SqlKata.Tests.Fixtures;
using System.Threading.Tasks;
using Allvis.Kaylee.Generator.SqlKata.Utilities;
using Allvis.Kaylee.Analyzer;

namespace Allvis.Kaylee.Generator.SqlKata.Tests.Unit.Extensions
{
    [UnitTest]
    public class EntitiesWriterTest
    {
        [Fact]
        public async Task TestWrite()
        {
            // Arrange
            var tSchema = AuthSchemaFixture.Create();
            var ast = KayleeHelper.Parse(tSchema);
            // Act
            var entities = EntitiesWriter.Write(ast);
            // Assert
            await DebugUtils.WriteGeneratedFileToDisk("Entities.cs", entities).ConfigureAwait(false);
            Assert.Equal(@"#nullable enable

namespace Allvis.Kaylee.Generated.SqlKata
{
    public static class Entities
    {
        public static class auth
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
                public byte[] ETag { get; set; } = global::System.Array.Empty<byte>();
                public long RAM4 { get; set; }
                public decimal Price { get; set; }
            }
            public class UserTask
            {
                public global::System.Guid UserId { get; set; }
                public int TaskId { get; set; }
                public string Todo { get; set; } = string.Empty;
            }
            public class UserRole
            {
                public global::System.Guid UserId { get; set; }
                public global::System.Guid RoleId { get; set; }
                public int Flag { get; set; }
            }
            public class UserRoleLog
            {
                public global::System.Guid UserId { get; set; }
                public global::System.Guid RoleId { get; set; }
                public int LogId { get; set; }
                public string Content { get; set; } = string.Empty;
            }
            public class UserRoleLogTrace
            {
                public global::System.Guid UserId { get; set; }
                public global::System.Guid RoleId { get; set; }
                public int LogId { get; set; }
                public global::System.Guid TraceId { get; set; }
            }
        }
    }
}
", entities);
        }
    }
}