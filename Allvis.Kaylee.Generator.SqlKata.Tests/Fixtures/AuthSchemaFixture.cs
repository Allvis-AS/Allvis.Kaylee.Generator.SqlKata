namespace Allvis.Kaylee.Generator.SqlKata.Tests.Fixtures
{
    public static class AuthSchemaFixture
    {
        public static string Create()
        {
            return @"
schema auth {
    entity User {
        fields {
            UserId GUID {
                default = NEWID();
            }
            FirstName? TEXT(100);
            LastName? TEXT(100);
            ContactEmail TEXT(254);
        }

        keys {
            primary = UserId;
        }

        entity Role {
            fields {
                RoleId GUID;
            }

            keys {
                primary = RoleId;
            }
        }
    }
}
";
        }
    }
}