using System;
using System.Text;
using Allvis.Kaylee.Generator.SqlKata.Extensions;

namespace Allvis.Kaylee.Generator.SqlKata.Builders
{
    public class SourceBuilder
    {
        private readonly StringBuilder stringBuilder;
        private readonly int level;

        public SourceBuilder(int level = 0) : this(new StringBuilder(), level) { }
        public SourceBuilder(StringBuilder stringBuilder, int level = 0)
        {
            this.stringBuilder = stringBuilder;
            this.level = level;
        }

        public void NL() => NewLine();
        public void NewLine()
        {
            Append(Environment.NewLine);
        }

        public void A(string value, bool indent = false) => Append(value, indent);
        public void Append(string value, bool indent = false)
        {
            stringBuilder.Append(indent ? value.Indent(level) : value);
        }

        public void AL(string line) => AppendLine(line);
        public void AppendLine(string line)
        {
            stringBuilder.AppendLine(line.Indent(level));
        }

        public void B(Action<SourceBuilder> builder) => Block(builder);
        public void Block(Action<SourceBuilder> builder)
        {
            AppendLine("{");
            Indent(builder);
            AppendLine("}");
        }

        public void I(Action<SourceBuilder> builder, int levels = 1) => Indent(builder, levels);
        public void Indent(Action<SourceBuilder> builder, int levels = 1)
        {
            var sourceBuilder = new SourceBuilder(stringBuilder, level + levels);
            builder.Invoke(sourceBuilder);
        }

        public override string ToString() => stringBuilder.ToString();
    }
}