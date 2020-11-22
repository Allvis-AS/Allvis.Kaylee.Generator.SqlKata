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
            RAM4 BIGINT;
            Price DECIMAL(19, 4);
        }

        keys {
            primary = UserId;
        }

        mutations {
            FullName(FirstName, LastName);
            ContactInformation(ContactEmail);
        }

        query Task {
            fields {
                TaskId INT;
                Todo TEXT(100);
            }

            keys {
                primary = TaskId;
            }
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