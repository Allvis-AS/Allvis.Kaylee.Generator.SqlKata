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
    public class QueryFactoryExtensionsWriterTest
    {
        [Fact]
        public async Task TestWrite()
        {
            // Arrange
            var tSchema = AuthSchemaFixture.Create();
            var ast = KayleeHelper.Parse(tSchema);
            // Act
            var source = QueryFactoryExtensionsWriter.Write(ast);
            // Assert
            await DebugUtils.WriteGeneratedFileToDisk("QueryFactoryExtensions.cs", source).ConfigureAwait(false);
            Assert.Equal(@"#nullable enable

using System.Linq;

namespace Allvis.Kaylee.Generated.SqlKata.Extensions
{
    public static class QueryFactoryExtensions
    {
        public static async global::System.Threading.Tasks.Task<bool> Exists_auth_User(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId)
        {
            var _rows = await _db.GetAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Exists_auth_User(userId)).ConfigureAwait(false);
            return _rows.Any();
        }
        public static global::System.Threading.Tasks.Task<int> Count_auth_User(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId)
        {
            return _db.ExecuteScalarAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Count_auth_User(userId));
        }
        public static global::System.Threading.Tasks.Task<int> Count_auth_User(this global::SqlKata.Execution.QueryFactory _db)
        {
            return _db.ExecuteScalarAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Count_auth_User());
        }
        public static global::System.Threading.Tasks.Task<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.User> Get_auth_User(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId)
        {
            return _db.FirstAsync<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.User>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Get_auth_User(userId));
        }
        public static global::System.Threading.Tasks.Task<global::System.Collections.Generic.IEnumerable<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.User>> Get_auth_User(this global::SqlKata.Execution.QueryFactory _db)
        {
            return _db.GetAsync<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.User>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Get_auth_User());
        }
        public static global::System.Threading.Tasks.Task<int> Insert_auth_User(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid? userId, string? firstName, string? lastName, string contactEmail, byte[] hash, byte[]? picture)
        {
            return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Insert_auth_User(userId, firstName, lastName, contactEmail, hash, picture));
        }
        public static global::System.Threading.Tasks.Task<int> Insert_auth_User(this global::SqlKata.Execution.QueryFactory _db, global::System.Collections.Generic.IEnumerable<(global::System.Guid UserId, string? FirstName, string? LastName, string ContactEmail, byte[] Hash, byte[]? Picture)> rows)
        {
            return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Insert_auth_User(rows));
        }
        public static global::System.Threading.Tasks.Task<int> Delete_auth_User(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId)
        {
            return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Delete_auth_User(userId));
        }
        public static global::System.Threading.Tasks.Task<int> Update_auth_User_FullName(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, string? firstName, string? lastName)
        {
            return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Update_auth_User_FullName(userId, firstName, lastName));
        }
        public static global::System.Threading.Tasks.Task<int> Update_auth_User_ContactInformation(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, string contactEmail)
        {
            return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Update_auth_User_ContactInformation(userId, contactEmail));
        }
        public static async global::System.Threading.Tasks.Task<bool> Exists_auth_UserRole(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, global::System.Guid roleId)
        {
            var _rows = await _db.GetAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Exists_auth_UserRole(userId, roleId)).ConfigureAwait(false);
            return _rows.Any();
        }
        public static global::System.Threading.Tasks.Task<int> Count_auth_UserRole(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, global::System.Guid roleId)
        {
            return _db.ExecuteScalarAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Count_auth_UserRole(userId, roleId));
        }
        public static global::System.Threading.Tasks.Task<int> Count_auth_UserRole(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId)
        {
            return _db.ExecuteScalarAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Count_auth_UserRole(userId));
        }
        public static global::System.Threading.Tasks.Task<int> Count_auth_UserRole(this global::SqlKata.Execution.QueryFactory _db)
        {
            return _db.ExecuteScalarAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Count_auth_UserRole());
        }
        public static global::System.Threading.Tasks.Task<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.UserRole> Get_auth_UserRole(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, global::System.Guid roleId)
        {
            return _db.FirstAsync<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.UserRole>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Get_auth_UserRole(userId, roleId));
        }
        public static global::System.Threading.Tasks.Task<global::System.Collections.Generic.IEnumerable<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.UserRole>> Get_auth_UserRole(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId)
        {
            return _db.GetAsync<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.UserRole>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Get_auth_UserRole(userId));
        }
        public static global::System.Threading.Tasks.Task<global::System.Collections.Generic.IEnumerable<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.UserRole>> Get_auth_UserRole(this global::SqlKata.Execution.QueryFactory _db)
        {
            return _db.GetAsync<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.UserRole>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Get_auth_UserRole());
        }
        public static global::System.Threading.Tasks.Task<int> Insert_auth_UserRole(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, global::System.Guid roleId, int? flag)
        {
            return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Insert_auth_UserRole(userId, roleId, flag));
        }
        public static global::System.Threading.Tasks.Task<int> Insert_auth_UserRole(this global::SqlKata.Execution.QueryFactory _db, global::System.Collections.Generic.IEnumerable<(global::System.Guid UserId, global::System.Guid RoleId, int Flag)> rows)
        {
            return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Insert_auth_UserRole(rows));
        }
        public static global::System.Threading.Tasks.Task<int> Delete_auth_UserRole(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, global::System.Guid roleId)
        {
            return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Delete_auth_UserRole(userId, roleId));
        }
        public static global::System.Threading.Tasks.Task<int> Update_auth_UserRole_Flag(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, global::System.Guid roleId, int flag)
        {
            return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Update_auth_UserRole_Flag(userId, roleId, flag));
        }
        public static async global::System.Threading.Tasks.Task<bool> Exists_auth_UserLog(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, int logId)
        {
            var _rows = await _db.GetAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Exists_auth_UserLog(userId, logId)).ConfigureAwait(false);
            return _rows.Any();
        }
        public static global::System.Threading.Tasks.Task<int> Count_auth_UserLog(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, int logId)
        {
            return _db.ExecuteScalarAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Count_auth_UserLog(userId, logId));
        }
        public static global::System.Threading.Tasks.Task<int> Count_auth_UserLog(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId)
        {
            return _db.ExecuteScalarAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Count_auth_UserLog(userId));
        }
        public static global::System.Threading.Tasks.Task<int> Count_auth_UserLog(this global::SqlKata.Execution.QueryFactory _db)
        {
            return _db.ExecuteScalarAsync<int>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Count_auth_UserLog());
        }
        public static global::System.Threading.Tasks.Task<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.UserLog> Get_auth_UserLog(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, int logId)
        {
            return _db.FirstAsync<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.UserLog>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Get_auth_UserLog(userId, logId));
        }
        public static global::System.Threading.Tasks.Task<global::System.Collections.Generic.IEnumerable<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.UserLog>> Get_auth_UserLog(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId)
        {
            return _db.GetAsync<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.UserLog>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Get_auth_UserLog(userId));
        }
        public static global::System.Threading.Tasks.Task<global::System.Collections.Generic.IEnumerable<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.UserLog>> Get_auth_UserLog(this global::SqlKata.Execution.QueryFactory _db)
        {
            return _db.GetAsync<global::Allvis.Kaylee.Generated.SqlKata.Models.auth.UserLog>(global::Allvis.Kaylee.Generated.SqlKata.Queries.Get_auth_UserLog());
        }
        public static global::System.Threading.Tasks.Task<int> Insert_auth_UserLog(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, string content)
        {
            return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Insert_auth_UserLog(userId, content));
        }
        public static global::System.Threading.Tasks.Task<int> Insert_auth_UserLog(this global::SqlKata.Execution.QueryFactory _db, global::System.Collections.Generic.IEnumerable<(global::System.Guid UserId, string Content)> rows)
        {
            return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Insert_auth_UserLog(rows));
        }
        public static global::System.Threading.Tasks.Task<int> Delete_auth_UserLog(this global::SqlKata.Execution.QueryFactory _db, global::System.Guid userId, int logId)
        {
            return _db.ExecuteAsync(global::Allvis.Kaylee.Generated.SqlKata.Queries.Delete_auth_UserLog(userId, logId));
        }
    }
}
", source);
        }
    }
}