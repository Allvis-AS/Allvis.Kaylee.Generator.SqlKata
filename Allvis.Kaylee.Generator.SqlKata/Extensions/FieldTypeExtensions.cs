using System;
using Allvis.Kaylee.Analyzer.Enums;

namespace Allvis.Kaylee.Generator.SqlKata.Extensions
{
    public static class FieldTypeExtensions
    {
        public static string ToCSharp(this FieldType fieldType)
        {
            return fieldType switch
            {
                FieldType.BIT => "bool",
                FieldType.TINYINT => "byte",
                FieldType.INT => "int",
                FieldType.CHAR => "string",
                FieldType.TEXT => "string",
                FieldType.GUID => "global::System.Guid",
                FieldType.DATE => "global::System.DateTimeOffset",
                FieldType.ROWVERSION => "string",
                FieldType.VARBINARY => "byte[]",
                FieldType.BINARY => "byte[]",
                _ => throw new ArgumentOutOfRangeException(nameof(fieldType))
            };
        }

        public static string ToCSharpIdentity(this FieldType fieldType)
        {
            return fieldType switch
            {
                FieldType.BIT => "",
                FieldType.TINYINT => "",
                FieldType.INT => "",
                FieldType.CHAR => "string.Empty",
                FieldType.TEXT => "string.Empty",
                FieldType.GUID => "",
                FieldType.DATE => "",
                FieldType.ROWVERSION => "string.Empty",
                FieldType.VARBINARY => "global::System.Array.Empty<byte>()",
                FieldType.BINARY => "global::System.Array.Empty<byte>()",
                _ => throw new ArgumentOutOfRangeException(nameof(fieldType))
            };
        }
    }
}