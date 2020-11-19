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
            Assert.Equal(@"namespace Allvis.Kaylee.Generated.SqlKata.Extensions
{
    public static class QueryFactoryExtensions
    {
        public static async System.Threading.Tasks.Task<bool> Exists_auth_User(this SqlKata.Execution.QueryFactory _db, System.Guid userId)
        {
            var _rows = await _db.GetAsync<int>(Queries.Exists_auth_User(userId)).ConfigureAwait(false);
            return System.Linq.Enumerable.Any(_rows);
        }
        public static System.Threading.Tasks.Task<Models.auth.User> Get_auth_User(this SqlKata.Execution.QueryFactory _db, System.Guid userId)
        {
            return _db.FirstAsync<Models.auth.User>(Queries.Get_auth_User(userId));
        }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Models.auth.User>> Get_auth_User(this SqlKata.Execution.QueryFactory _db)
        {
            return _db.GetAsync<Models.auth.User>(Queries.Get_auth_User());
        }
        public static System.Threading.Tasks.Task<int> Insert_auth_User(this SqlKata.Execution.QueryFactory _db, System.Guid? userId, string? firstName, string? lastName, string contactEmail, byte[] hash, byte[]? picture)
        {
            return _db.ExecuteAsync(Queries.Insert_auth_User(userId, firstName, lastName, contactEmail, hash, picture));
        }
        public static System.Threading.Tasks.Task<int> Insert_auth_User(this SqlKata.Execution.QueryFactory _db, System.Collections.Generic.IEnumerable<(System.Guid UserId, string? FirstName, string? LastName, string ContactEmail, byte[] Hash, byte[]? Picture)> rows)
        {
            return _db.ExecuteAsync(Queries.Insert_auth_User(rows));
        }
        public static System.Threading.Tasks.Task<int> Delete_auth_User(this SqlKata.Execution.QueryFactory _db, System.Guid userId)
        {
            return _db.ExecuteAsync(Queries.Delete_auth_User(userId));
        }
        public static System.Threading.Tasks.Task<int> Update_auth_User_FullName(this SqlKata.Execution.QueryFactory _db, System.Guid userId, string? firstName, string? lastName)
        {
            return _db.ExecuteAsync(Queries.Update_auth_User_FullName(userId, firstName, lastName));
        }
        public static System.Threading.Tasks.Task<int> Update_auth_User_ContactInformation(this SqlKata.Execution.QueryFactory _db, System.Guid userId, string contactEmail)
        {
            return _db.ExecuteAsync(Queries.Update_auth_User_ContactInformation(userId, contactEmail));
        }
        public static async System.Threading.Tasks.Task<bool> Exists_auth_UserRole(this SqlKata.Execution.QueryFactory _db, System.Guid userId, System.Guid roleId)
        {
            var _rows = await _db.GetAsync<int>(Queries.Exists_auth_UserRole(userId, roleId)).ConfigureAwait(false);
            return System.Linq.Enumerable.Any(_rows);
        }
        public static System.Threading.Tasks.Task<Models.auth.UserRole> Get_auth_UserRole(this SqlKata.Execution.QueryFactory _db, System.Guid userId, System.Guid roleId)
        {
            return _db.FirstAsync<Models.auth.UserRole>(Queries.Get_auth_UserRole(userId, roleId));
        }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Models.auth.UserRole>> Get_auth_UserRole(this SqlKata.Execution.QueryFactory _db, System.Guid userId)
        {
            return _db.GetAsync<Models.auth.UserRole>(Queries.Get_auth_UserRole(userId));
        }
        public static System.Threading.Tasks.Task<System.Collections.Generic.IEnumerable<Models.auth.UserRole>> Get_auth_UserRole(this SqlKata.Execution.QueryFactory _db)
        {
            return _db.GetAsync<Models.auth.UserRole>(Queries.Get_auth_UserRole());
        }
        public static System.Threading.Tasks.Task<int> Insert_auth_UserRole(this SqlKata.Execution.QueryFactory _db, System.Guid userId, System.Guid roleId, int? flag)
        {
            return _db.ExecuteAsync(Queries.Insert_auth_UserRole(userId, roleId, flag));
        }
        public static System.Threading.Tasks.Task<int> Insert_auth_UserRole(this SqlKata.Execution.QueryFactory _db, System.Collections.Generic.IEnumerable<(System.Guid UserId, System.Guid RoleId, int Flag)> rows)
        {
            return _db.ExecuteAsync(Queries.Insert_auth_UserRole(rows));
        }
        public static System.Threading.Tasks.Task<int> Delete_auth_UserRole(this SqlKata.Execution.QueryFactory _db, System.Guid userId, System.Guid roleId)
        {
            return _db.ExecuteAsync(Queries.Delete_auth_UserRole(userId, roleId));
        }
        public static System.Threading.Tasks.Task<int> Update_auth_UserRole_Flag(this SqlKata.Execution.QueryFactory _db, System.Guid userId, System.Guid roleId, int flag)
        {
            return _db.ExecuteAsync(Queries.Update_auth_UserRole_Flag(userId, roleId, flag));
        }
    }
}
", source);
        }
    }
}