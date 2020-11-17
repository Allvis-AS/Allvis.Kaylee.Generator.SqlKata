using System.Text;
using Allvis.Kaylee.Analyzer;
using Allvis.Kaylee.Analyzer.Exceptions;
using Allvis.Kaylee.Analyzer.Models;
using Allvis.Kaylee.Generator.SqlKata.Extensions;
using Allvis.Kaylee.Generator.SqlKata.Writers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Allvis.Kaylee.Generator.SqlKata
{
    [Generator]
    public class SqlKataGenerator : ISourceGenerator
    {
        private static readonly DiagnosticDescriptor FailedToReadFileError = new DiagnosticDescriptor(
            id: "KAYSQLKAT001",
            title: "Failed to read file",
            messageFormat: "Failed to read the contents of '{0}'",
            category: "SqlKataGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        private static readonly DiagnosticDescriptor FailedToParseError = new DiagnosticDescriptor(
            id: "KAYSQLKAT002",
            title: "Failed to parse model",
            messageFormat: "Failed to parse the Kaylee model - Exception: {0}",
            category: "SqlKataGenerator",
            DiagnosticSeverity.Error,
            isEnabledByDefault: true);

        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public void Execute(GeneratorExecutionContext context)
        {
            var ast = ParseModel(context);
            if (ast == null)
            {
                return;
            }
            AddModels(context, ast);
            AddQueries(context, ast);
            AddQueryFactoryExtensions(context, ast);
        }

        private Ast? ParseModel(GeneratorExecutionContext context)
        {
            var model = ReadModel(context);
            if (model == null)
            {
                return null;
            }
            try
            {
                return KayleeHelper.Parse(model);
            }
            catch (ParseException e)
            {
                context.ReportDiagnostic(Diagnostic.Create(FailedToParseError, Location.None, e.ToString()));
                return null;
            }
        }

        private string? ReadModel(GeneratorExecutionContext context)
        {
            var sb = new StringBuilder();
            var files = context.GetKayleeModelFiles();
            foreach (var file in files)
            {
                var text = file.GetText(context.CancellationToken);
                if (text == null)
                {
                    context.ReportDiagnostic(Diagnostic.Create(FailedToReadFileError, Location.None, file.Path));
                    return null;
                }
                sb.AppendLine(text.ToString());
            }
            return sb.ToString();
        }

        private void AddModels(GeneratorExecutionContext context, Ast ast)
        {
            var models = ModelsWriter.Write(ast);
            foreach (var model in models)
            {
                var source = SourceText.From(model.Source);
                context.AddSource(model.HintName, source);
            }
        }

        private void AddQueries(GeneratorExecutionContext context, Ast ast)
        {
            var queries = QueriesWriter.Write(ast);
            var source = SourceText.From(queries);
            context.AddSource("Allvis.Kaylee.Generated.SqlKata.Queries", source);
        }

        private void AddQueryFactoryExtensions(GeneratorExecutionContext context, Ast ast)
        {
            var queryFactoryExtensions = QueryFactoryExtensionsWriter.Write(ast);
            var source = SourceText.From(queryFactoryExtensions);
            context.AddSource("Allvis.Kaylee.Generated.SqlKata.Extensions.QueryFactoryExtensions", source);
        }
    }
}