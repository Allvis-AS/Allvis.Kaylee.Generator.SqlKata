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
            NormalizedContactEmail TEXT(254) {
                computed = true;
            }
            Hash BINARY(128);
            Picture? VARBINARY(MAX);
            ETag ROWVERSION;
        }

        keys {
            primary = UserId;
        }

        mutations {
            FullName(FirstName, LastName);
            ContactInformation(ContactEmail);
        }

        entity Role {
            fields {
                RoleId GUID;
                Flag INT {
                    default = 0;
                }
            }

            keys {
                primary = RoleId;
            }

            mutations {
                Flag(Flag);
            }
        
            entity Log {
                fields {
                    LogId INT AUTO INCREMENT;
                    Content TEXT(500);
                }

                keys {
                    primary = LogId;
                }

                entity Trace {
                    fields {
                        TraceId GUID;
                    }

                    keys {
                        primary = TraceId;
                    }
                }
            }
        }
    }
}
";
        }
    }
}