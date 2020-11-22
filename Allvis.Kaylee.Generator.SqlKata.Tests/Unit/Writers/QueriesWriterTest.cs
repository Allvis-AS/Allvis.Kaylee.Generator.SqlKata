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
        public static global::SqlKata.Query Count_auth_User()
        {
            return new global::SqlKata.Query(""auth.v_User"")
                .AsCount();
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
                .Select(""ETag"")
                .Select(""RAM4"")
                .Select(""Price"");
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
                .Select(""ETag"")
                .Select(""RAM4"")
                .Select(""Price"");
        }
        public static global::SqlKata.Query Insert_auth_User(global::System.Guid? userId, string? firstName, string? lastName, string contactEmail, byte[] hash, byte[]? picture, long rAM4, decimal price)
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
            _columns.Add(""RAM4"");
            _values.Add(rAM4);
            _columns.Add(""Price"");
            _values.Add(price);
            return new global::SqlKata.Query(""auth.tbl_User"")
                .AsInsert(_columns, _values);
        }
        public static global::SqlKata.Query Insert_auth_User(global::System.Collections.Generic.IEnumerable<(global::System.Guid UserId, string? FirstName, string? LastName, string ContactEmail, byte[] Hash, byte[]? Picture, long RAM4, decimal Price)> rows)
        {
            var _columns = new string[] {
                ""UserId"",
                ""FirstName"",
                ""LastName"",
                ""ContactEmail"",
                ""Hash"",
                ""Picture"",
                ""RAM4"",
                ""Price""
            };
            var _values = rows.Select(_row => new object?[] {
                _row.UserId,
                _row.FirstName,
                _row.LastName,
                _row.ContactEmail,
                _row.Hash,
                _row.Picture,
                _row.RAM4,
                _row.Price
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
        public static global::SqlKata.Query Exists_auth_UserTask(global::System.Guid userId, int taskId)
        {
            return new global::SqlKata.Query(""auth.v_UserTask"")
                .Where(""UserId"", userId)
                .Where(""TaskId"", taskId)
                .SelectRaw(""1"")
                .Limit(1);
        }
        public static global::SqlKata.Query Count_auth_UserTask_GroupBy_UserId()
        {
            return new global::SqlKata.Query(""auth.v_UserTask"")
                .GroupBy(""UserId"")
                .Select(""UserId"")
                .SelectRaw(""COUNT(*) as Count"");
        }
        public static global::SqlKata.Query Count_auth_UserTask(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_UserTask"")
                .Where(""UserId"", userId)
                .AsCount();
        }
        public static global::SqlKata.Query Count_auth_UserTask()
        {
            return new global::SqlKata.Query(""auth.v_UserTask"")
                .AsCount();
        }
        public static global::SqlKata.Query Get_auth_UserTask(global::System.Guid userId, int taskId)
        {
            return new global::SqlKata.Query(""auth.v_UserTask"")
                .Where(""UserId"", userId)
                .Where(""TaskId"", taskId)
                .Select(""UserId"")
                .Select(""TaskId"")
                .Select(""Todo"");
        }
        public static global::SqlKata.Query Get_auth_UserTask(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_UserTask"")
                .Where(""UserId"", userId)
                .Select(""UserId"")
                .Select(""TaskId"")
                .Select(""Todo"");
        }
        public static global::SqlKata.Query Get_auth_UserTask()
        {
            return new global::SqlKata.Query(""auth.v_UserTask"")
                .Select(""UserId"")
                .Select(""TaskId"")
                .Select(""Todo"");
        }
        public static global::SqlKata.Query Exists_auth_UserRole(global::System.Guid userId, global::System.Guid roleId)
        {
            return new global::SqlKata.Query(""auth.v_UserRole"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .SelectRaw(""1"")
                .Limit(1);
        }
        public static global::SqlKata.Query Count_auth_UserRole_GroupBy_UserId()
        {
            return new global::SqlKata.Query(""auth.v_UserRole"")
                .GroupBy(""UserId"")
                .Select(""UserId"")
                .SelectRaw(""COUNT(*) as Count"");
        }
        public static global::SqlKata.Query Count_auth_UserRole(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_UserRole"")
                .Where(""UserId"", userId)
                .AsCount();
        }
        public static global::SqlKata.Query Count_auth_UserRole()
        {
            return new global::SqlKata.Query(""auth.v_UserRole"")
                .AsCount();
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
        public static global::SqlKata.Query Exists_auth_UserRoleLog(global::System.Guid userId, global::System.Guid roleId, int logId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLog"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .Where(""LogId"", logId)
                .SelectRaw(""1"")
                .Limit(1);
        }
        public static global::SqlKata.Query Count_auth_UserRoleLog_GroupBy_UserId_RoleId(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLog"")
                .Where(""UserId"", userId)
                .GroupBy(""UserId"")
                .GroupBy(""RoleId"")
                .Select(""UserId"")
                .Select(""RoleId"")
                .SelectRaw(""COUNT(*) as Count"");
        }
        public static global::SqlKata.Query Count_auth_UserRoleLog_GroupBy_UserId_RoleId()
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLog"")
                .GroupBy(""UserId"")
                .GroupBy(""RoleId"")
                .Select(""UserId"")
                .Select(""RoleId"")
                .SelectRaw(""COUNT(*) as Count"");
        }
        public static global::SqlKata.Query Count_auth_UserRoleLog(global::System.Guid userId, global::System.Guid roleId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLog"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .AsCount();
        }
        public static global::SqlKata.Query Count_auth_UserRoleLog_GroupBy_UserId()
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLog"")
                .GroupBy(""UserId"")
                .Select(""UserId"")
                .SelectRaw(""COUNT(*) as Count"");
        }
        public static global::SqlKata.Query Count_auth_UserRoleLog(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLog"")
                .Where(""UserId"", userId)
                .AsCount();
        }
        public static global::SqlKata.Query Count_auth_UserRoleLog()
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLog"")
                .AsCount();
        }
        public static global::SqlKata.Query Get_auth_UserRoleLog(global::System.Guid userId, global::System.Guid roleId, int logId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLog"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .Where(""LogId"", logId)
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""LogId"")
                .Select(""Content"");
        }
        public static global::SqlKata.Query Get_auth_UserRoleLog(global::System.Guid userId, global::System.Guid roleId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLog"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""LogId"")
                .Select(""Content"");
        }
        public static global::SqlKata.Query Get_auth_UserRoleLog(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLog"")
                .Where(""UserId"", userId)
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""LogId"")
                .Select(""Content"");
        }
        public static global::SqlKata.Query Get_auth_UserRoleLog()
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLog"")
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""LogId"")
                .Select(""Content"");
        }
        public static global::SqlKata.Query Insert_auth_UserRoleLog(global::System.Guid userId, global::System.Guid roleId, string content)
        {
            var _columns = new global::System.Collections.Generic.List<string>();
            var _values = new global::System.Collections.Generic.List<object?>();
            _columns.Add(""UserId"");
            _values.Add(userId);
            _columns.Add(""RoleId"");
            _values.Add(roleId);
            _columns.Add(""Content"");
            _values.Add(content);
            return new global::SqlKata.Query(""auth.tbl_UserRoleLog"")
                .AsInsert(_columns, _values);
        }
        public static global::SqlKata.Query Insert_auth_UserRoleLog(global::System.Collections.Generic.IEnumerable<(global::System.Guid UserId, global::System.Guid RoleId, string Content)> rows)
        {
            var _columns = new string[] {
                ""UserId"",
                ""RoleId"",
                ""Content""
            };
            var _values = rows.Select(_row => new object?[] {
                _row.UserId,
                _row.RoleId,
                _row.Content
            });
            return new global::SqlKata.Query(""auth.tbl_UserRoleLog"")
                .AsInsert(_columns, _values);
        }
        public static global::SqlKata.Query Delete_auth_UserRoleLog(global::System.Guid userId, global::System.Guid roleId, int logId)
        {
            return new global::SqlKata.Query(""auth.tbl_UserRoleLog"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .Where(""LogId"", logId)
                .AsDelete();
        }
        public static global::SqlKata.Query Exists_auth_UserRoleLogTrace(global::System.Guid userId, global::System.Guid roleId, int logId, global::System.Guid traceId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .Where(""LogId"", logId)
                .Where(""TraceId"", traceId)
                .SelectRaw(""1"")
                .Limit(1);
        }
        public static global::SqlKata.Query Count_auth_UserRoleLogTrace_GroupBy_UserId_RoleId_LogId(global::System.Guid userId, global::System.Guid roleId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .GroupBy(""UserId"")
                .GroupBy(""RoleId"")
                .GroupBy(""LogId"")
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""LogId"")
                .SelectRaw(""COUNT(*) as Count"");
        }
        public static global::SqlKata.Query Count_auth_UserRoleLogTrace_GroupBy_UserId_RoleId_LogId(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .Where(""UserId"", userId)
                .GroupBy(""UserId"")
                .GroupBy(""RoleId"")
                .GroupBy(""LogId"")
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""LogId"")
                .SelectRaw(""COUNT(*) as Count"");
        }
        public static global::SqlKata.Query Count_auth_UserRoleLogTrace_GroupBy_UserId_RoleId_LogId()
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .GroupBy(""UserId"")
                .GroupBy(""RoleId"")
                .GroupBy(""LogId"")
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""LogId"")
                .SelectRaw(""COUNT(*) as Count"");
        }
        public static global::SqlKata.Query Count_auth_UserRoleLogTrace(global::System.Guid userId, global::System.Guid roleId, int logId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .Where(""LogId"", logId)
                .AsCount();
        }
        public static global::SqlKata.Query Count_auth_UserRoleLogTrace_GroupBy_UserId_RoleId(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .Where(""UserId"", userId)
                .GroupBy(""UserId"")
                .GroupBy(""RoleId"")
                .Select(""UserId"")
                .Select(""RoleId"")
                .SelectRaw(""COUNT(*) as Count"");
        }
        public static global::SqlKata.Query Count_auth_UserRoleLogTrace_GroupBy_UserId_RoleId()
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .GroupBy(""UserId"")
                .GroupBy(""RoleId"")
                .Select(""UserId"")
                .Select(""RoleId"")
                .SelectRaw(""COUNT(*) as Count"");
        }
        public static global::SqlKata.Query Count_auth_UserRoleLogTrace(global::System.Guid userId, global::System.Guid roleId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .AsCount();
        }
        public static global::SqlKata.Query Count_auth_UserRoleLogTrace_GroupBy_UserId()
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .GroupBy(""UserId"")
                .Select(""UserId"")
                .SelectRaw(""COUNT(*) as Count"");
        }
        public static global::SqlKata.Query Count_auth_UserRoleLogTrace(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .Where(""UserId"", userId)
                .AsCount();
        }
        public static global::SqlKata.Query Count_auth_UserRoleLogTrace()
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .AsCount();
        }
        public static global::SqlKata.Query Get_auth_UserRoleLogTrace(global::System.Guid userId, global::System.Guid roleId, int logId, global::System.Guid traceId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .Where(""LogId"", logId)
                .Where(""TraceId"", traceId)
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""LogId"")
                .Select(""TraceId"");
        }
        public static global::SqlKata.Query Get_auth_UserRoleLogTrace(global::System.Guid userId, global::System.Guid roleId, int logId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .Where(""LogId"", logId)
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""LogId"")
                .Select(""TraceId"");
        }
        public static global::SqlKata.Query Get_auth_UserRoleLogTrace(global::System.Guid userId, global::System.Guid roleId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""LogId"")
                .Select(""TraceId"");
        }
        public static global::SqlKata.Query Get_auth_UserRoleLogTrace(global::System.Guid userId)
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .Where(""UserId"", userId)
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""LogId"")
                .Select(""TraceId"");
        }
        public static global::SqlKata.Query Get_auth_UserRoleLogTrace()
        {
            return new global::SqlKata.Query(""auth.v_UserRoleLogTrace"")
                .Select(""UserId"")
                .Select(""RoleId"")
                .Select(""LogId"")
                .Select(""TraceId"");
        }
        public static global::SqlKata.Query Insert_auth_UserRoleLogTrace(global::System.Guid userId, global::System.Guid roleId, int logId, global::System.Guid traceId)
        {
            var _columns = new global::System.Collections.Generic.List<string>();
            var _values = new global::System.Collections.Generic.List<object?>();
            _columns.Add(""UserId"");
            _values.Add(userId);
            _columns.Add(""RoleId"");
            _values.Add(roleId);
            _columns.Add(""LogId"");
            _values.Add(logId);
            _columns.Add(""TraceId"");
            _values.Add(traceId);
            return new global::SqlKata.Query(""auth.tbl_UserRoleLogTrace"")
                .AsInsert(_columns, _values);
        }
        public static global::SqlKata.Query Insert_auth_UserRoleLogTrace(global::System.Collections.Generic.IEnumerable<(global::System.Guid UserId, global::System.Guid RoleId, int LogId, global::System.Guid TraceId)> rows)
        {
            var _columns = new string[] {
                ""UserId"",
                ""RoleId"",
                ""LogId"",
                ""TraceId""
            };
            var _values = rows.Select(_row => new object?[] {
                _row.UserId,
                _row.RoleId,
                _row.LogId,
                _row.TraceId
            });
            return new global::SqlKata.Query(""auth.tbl_UserRoleLogTrace"")
                .AsInsert(_columns, _values);
        }
        public static global::SqlKata.Query Delete_auth_UserRoleLogTrace(global::System.Guid userId, global::System.Guid roleId, int logId, global::System.Guid traceId)
        {
            return new global::SqlKata.Query(""auth.tbl_UserRoleLogTrace"")
                .Where(""UserId"", userId)
                .Where(""RoleId"", roleId)
                .Where(""LogId"", logId)
                .Where(""TraceId"", traceId)
                .AsDelete();
        }
    }
}
", source);
        }
    }
}