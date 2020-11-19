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
        public static SqlKata.Query Exists_auth_User(System.Guid userId)
        {
            return new SqlKata.Query(""auth.v_User"")
                .Where(""UserId"", userId)
                .SelectRaw(""1"")
                .Limit(1);
        }
        public static SqlKata.Query Get_auth_User(System.Guid userId)
        {
            return new SqlKata.Query(""auth.v_User"")
                .Where(""UserId"", userId)
                .Select(""UserId"")
                .Select(""FirstName"")
                .Select(""LastName"")
                .Select(""ContactEmail"")
                .Select(""NormalizedContactEmail"")
                .Select(""Hash"")
                .Select(""Picture"");
        }
        public static SqlKata.Query Get_auth_User()
        {
            return new SqlKata.Query(""auth.v_User"")
                .Select(""UserId"")
                .Select(""FirstName"")
                .Select(""LastName"")
                .Select(""ContactEmail"")
                .Select(""NormalizedContactEmail"")
                .Select(""Hash"")
                .Select(""Picture"");
        }
        public static SqlKata.Query Insert_auth_User(System.Guid? userId, string? firstName, string? lastName, string contactEmail, byte[] hash, byte[]? picture)
        {
            var _columns = new System.Collections.Generic.List<string>();
            var _values = new System.Collections.Generic.List<object>();
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
            return new SqlKata.Query(""auth.tbl_User"")
                .AsInsert(_columns, _values);
        }
        public static SqlKata.Query Insert_auth_User(System.Collections.Generic.IEnumerable<(System.Guid UserId, string? FirstName, string? LastName, string ContactEmail, byte[] Hash, byte[]? Picture)> rows)
        {
            var _columns = new string[] {
                ""UserId"",
                ""FirstName"",
                ""LastName"",
                ""ContactEmail"",
                ""Hash"",
                ""Picture""
            };
            var _values = rows.Select(_row => new object[] {
                _row.UserId,
                _row.FirstName,
                _row.LastName,
                _row.ContactEmail,
                _row.Hash,
                _row.Picture
            });
            return new SqlKata.Query(""auth.tbl_User"")
                .AsInsert(_columns, _values);
        }
        public static SqlKata.Query Delete_auth_User(System.Guid userId)
        {
            return new SqlKata.Query(""auth.tbl_User"")
                .Where(""UserId"", userId)
                .AsDelete();
        }
        public static SqlKata.Query Update_auth_User_FullName(System.Guid userId, string? firstName, string? lastName)
        {
            var _columns = new string[] {
                ""FirstName"",
                ""LastName""
            };
            var _values = new object[] {
                firstName,
                lastName
            };
            return new SqlKata.Query(""auth.tbl_User"")
                .Where(""UserId"", userId)
                .AsUpdate(_columns, _values);
        }
        public static SqlKata.Query Update_auth_User_ContactInformation(System.Guid userId, string contactEmail)
        {
            var _columns = new string[] {
                ""ContactEmail""
            };
            var _values = new object[] {
                contactEmail
            };
            return new SqlKata.Query(""auth.tbl_User"")
                .Where(""UserId"", userId)
                .AsUpdate(_columns, _values);
        }
        public static SqlKata.Query Exists_auth_UserRole(System.Guid userId, System.Guid roleId)
        {
            return new SqlKata.Query(""auth.v_UserRole"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .SelectRaw(""1"")
                .Limit(1);
        }
        public static SqlKata.Query Get_auth_UserRole(System.Guid userId, System.Guid roleId)
        {
            return new SqlKata.Query(""auth.v_UserRole"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""Flag"");
        }
        public static SqlKata.Query Get_auth_UserRole(System.Guid userId)
        {
            return new SqlKata.Query(""auth.v_UserRole"")
                .Where(""UserId"", userId)
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""Flag"");
        }
        public static SqlKata.Query Get_auth_UserRole()
        {
            return new SqlKata.Query(""auth.v_UserRole"")
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""Flag"");
        }
        public static SqlKata.Query Insert_auth_UserRole(System.Guid userId, System.Guid roleId, int? flag)
        {
            var _columns = new System.Collections.Generic.List<string>();
            var _values = new System.Collections.Generic.List<object>();
            _columns.Add(""UserId"");
            _values.Add(userId);
            _columns.Add(""RoleId"");
            _values.Add(roleId);
            if (flag != null)
            {
                _columns.Add(""Flag"");
                _values.Add(flag);
            }
            return new SqlKata.Query(""auth.tbl_UserRole"")
                .AsInsert(_columns, _values);
        }
        public static SqlKata.Query Insert_auth_UserRole(System.Collections.Generic.IEnumerable<(System.Guid UserId, System.Guid RoleId, int Flag)> rows)
        {
            var _columns = new string[] {
                ""UserId"",
                ""RoleId"",
                ""Flag""
            };
            var _values = rows.Select(_row => new object[] {
                _row.UserId,
                _row.RoleId,
                _row.Flag
            });
            return new SqlKata.Query(""auth.tbl_UserRole"")
                .AsInsert(_columns, _values);
        }
        public static SqlKata.Query Delete_auth_UserRole(System.Guid userId, System.Guid roleId)
        {
            return new SqlKata.Query(""auth.tbl_UserRole"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .AsDelete();
        }
        public static SqlKata.Query Update_auth_UserRole_Flag(System.Guid userId, System.Guid roleId, int flag)
        {
            var _columns = new string[] {
                ""Flag""
            };
            var _values = new object[] {
                flag
            };
            return new SqlKata.Query(""auth.tbl_UserRole"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .AsUpdate(_columns, _values);
        }
    }
}
", source);
        }
    }
}