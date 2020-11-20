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
            Assert.Equal(@"#nullable enable

using System.Linq;

namespace Allvis.Kaylee.Generated.SqlKata
{
    public static class Queries
    {
        public static global::SqlKata.Query Exists_auth_User(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_User"")
                .Where(""UserId"", userId)
                .SelectRaw(""1"")
                .Limit(1);
        }
        public static global::SqlKata.Query Get_auth_User(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_User"")
                .Where(""UserId"", userId)
                .Select(""UserId"")
                .Select(""FirstName"")
                .Select(""LastName"")
                .Select(""ContactEmail"")
                .Select(""NormalizedContactEmail"")
                .Select(""Hash"")
                .Select(""Picture"")
                .Select(""ETag"");
        }
        public static global::SqlKata.Query Get_auth_User()
        {
            return new global::SqlKata.Query(""auth.v_User"")
                .Select(""UserId"")
                .Select(""FirstName"")
                .Select(""LastName"")
                .Select(""ContactEmail"")
                .Select(""NormalizedContactEmail"")
                .Select(""Hash"")
                .Select(""Picture"")
                .Select(""ETag"");
        }
        public static global::SqlKata.Query Insert_auth_User(global::System.Guid? userId, string? firstName, string? lastName, string contactEmail, byte[] hash, byte[]? picture)
        {
            var _columns = new global::System.Collections.Generic.List<string>();
            var _values = new global::System.Collections.Generic.List<object?>();
            if (userId != null)
            {
                _columns.Add(""UserId"");
                _values.Add(userId);
            }
            if (firstName != null)
            {
                _columns.Add(""FirstName"");
                _values.Add(firstName);
            }
            if (lastName != null)
            {
                _columns.Add(""LastName"");
                _values.Add(lastName);
            }
            _columns.Add(""ContactEmail"");
            _values.Add(contactEmail);
            _columns.Add(""Hash"");
            _values.Add(hash);
            if (picture != null)
            {
                _columns.Add(""Picture"");
                _values.Add(picture);
            }
            return new global::SqlKata.Query(""auth.tbl_User"")
                .AsInsert(_columns, _values);
        }
        public static global::SqlKata.Query Insert_auth_User(global::System.Collections.Generic.IEnumerable<(global::System.Guid UserId, string? FirstName, string? LastName, string ContactEmail, byte[] Hash, byte[]? Picture)> rows)
        {
            var _columns = new string[] {
                ""UserId"",
                ""FirstName"",
                ""LastName"",
                ""ContactEmail"",
                ""Hash"",
                ""Picture""
            };
            var _values = rows.Select(_row => new object?[] {
                _row.UserId,
                _row.FirstName,
                _row.LastName,
                _row.ContactEmail,
                _row.Hash,
                _row.Picture
            });
            return new global::SqlKata.Query(""auth.tbl_User"")
                .AsInsert(_columns, _values);
        }
        public static global::SqlKata.Query Delete_auth_User(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.tbl_User"")
                .Where(""UserId"", userId)
                .AsDelete();
        }
        public static global::SqlKata.Query Update_auth_User_FullName(global::System.Guid userId, string? firstName, string? lastName)
        {
            var _columns = new string[] {
                ""FirstName"",
                ""LastName""
            };
            var _values = new object?[] {
                firstName,
                lastName
            };
            return new global::SqlKata.Query(""auth.tbl_User"")
                .Where(""UserId"", userId)
                .AsUpdate(_columns, _values);
        }
        public static global::SqlKata.Query Update_auth_User_ContactInformation(global::System.Guid userId, string contactEmail)
        {
            var _columns = new string[] {
                ""ContactEmail""
            };
            var _values = new object?[] {
                contactEmail
            };
            return new global::SqlKata.Query(""auth.tbl_User"")
                .Where(""UserId"", userId)
                .AsUpdate(_columns, _values);
        }
        public static global::SqlKata.Query Exists_auth_UserRole(global::System.Guid userId, global::System.Guid roleId)
        {
            return new global::SqlKata.Query(""auth.v_UserRole"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .SelectRaw(""1"")
                .Limit(1);
        }
        public static global::SqlKata.Query Get_auth_UserRole(global::System.Guid userId, global::System.Guid roleId)
        {
            return new global::SqlKata.Query(""auth.v_UserRole"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""Flag"");
        }
        public static global::SqlKata.Query Get_auth_UserRole(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_UserRole"")
                .Where(""UserId"", userId)
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""Flag"");
        }
        public static global::SqlKata.Query Get_auth_UserRole()
        {
            return new global::SqlKata.Query(""auth.v_UserRole"")
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""Flag"");
        }
        public static global::SqlKata.Query Insert_auth_UserRole(global::System.Guid userId, global::System.Guid roleId, int? flag)
        {
            var _columns = new global::System.Collections.Generic.List<string>();
            var _values = new global::System.Collections.Generic.List<object?>();
            _columns.Add(""UserId"");
            _values.Add(userId);
            _columns.Add(""RoleId"");
            _values.Add(roleId);
            if (flag != null)
            {
                _columns.Add(""Flag"");
                _values.Add(flag);
            }
            return new global::SqlKata.Query(""auth.tbl_UserRole"")
                .AsInsert(_columns, _values);
        }
        public static global::SqlKata.Query Insert_auth_UserRole(global::System.Collections.Generic.IEnumerable<(global::System.Guid UserId, global::System.Guid RoleId, int Flag)> rows)
        {
            var _columns = new string[] {
                ""UserId"",
                ""RoleId"",
                ""Flag""
            };
            var _values = rows.Select(_row => new object?[] {
                _row.UserId,
                _row.RoleId,
                _row.Flag
            });
            return new global::SqlKata.Query(""auth.tbl_UserRole"")
                .AsInsert(_columns, _values);
        }
        public static global::SqlKata.Query Delete_auth_UserRole(global::System.Guid userId, global::System.Guid roleId)
        {
            return new global::SqlKata.Query(""auth.tbl_UserRole"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .AsDelete();
        }
        public static global::SqlKata.Query Update_auth_UserRole_Flag(global::System.Guid userId, global::System.Guid roleId, int flag)
        {
            var _columns = new string[] {
                ""Flag""
            };
            var _values = new object?[] {
                flag
            };
            return new global::SqlKata.Query(""auth.tbl_UserRole"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .AsUpdate(_columns, _values);
        }
        public static global::SqlKata.Query Exists_auth_UserLog(global::System.Guid userId, int logId)
        {
            return new global::SqlKata.Query(""auth.v_UserLog"")
                .Where(""UserId"", userId)
                .Where(""LogId"", logId)
                .SelectRaw(""1"")
                .Limit(1);
        }
        public static global::SqlKata.Query Get_auth_UserLog(global::System.Guid userId, int logId)
        {
            return new global::SqlKata.Query(""auth.v_UserLog"")
                .Where(""UserId"", userId)
                .Where(""LogId"", logId)
                .Select(""UserId"")
                .Select(""LogId"")
                .Select(""Content"");
        }
        public static global::SqlKata.Query Get_auth_UserLog(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_UserLog"")
                .Where(""UserId"", userId)
                .Select(""UserId"")
                .Select(""LogId"")
                .Select(""Content"");
        }
        public static global::SqlKata.Query Get_auth_UserLog()
        {
            return new global::SqlKata.Query(""auth.v_UserLog"")
                .Select(""UserId"")
                .Select(""LogId"")
                .Select(""Content"");
        }
        public static global::SqlKata.Query Insert_auth_UserLog(global::System.Guid userId, string content)
        {
            var _columns = new global::System.Collections.Generic.List<string>();
            var _values = new global::System.Collections.Generic.List<object?>();
            _columns.Add(""UserId"");
            _values.Add(userId);
            _columns.Add(""Content"");
            _values.Add(content);
            return new global::SqlKata.Query(""auth.tbl_UserLog"")
                .AsInsert(_columns, _values);
        }
        public static global::SqlKata.Query Insert_auth_UserLog(global::System.Collections.Generic.IEnumerable<(global::System.Guid UserId, string Content)> rows)
        {
            var _columns = new string[] {
                ""UserId"",
                ""Content""
            };
            var _values = rows.Select(_row => new object?[] {
                _row.UserId,
                _row.Content
            });
            return new global::SqlKata.Query(""auth.tbl_UserLog"")
                .AsInsert(_columns, _values);
        }
        public static global::SqlKata.Query Delete_auth_UserLog(global::System.Guid userId, int logId)
        {
            return new global::SqlKata.Query(""auth.tbl_UserLog"")
                .Where(""UserId"", userId)
                .Where(""LogId"", logId)
                .AsDelete();
        }
    }
}
", source);
        }
    }
}