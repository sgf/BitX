
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace BitXGenerator
{

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class BitFieldGeneratorAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => throw new System.NotImplementedException();

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.StructDeclaration);
        }

        private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            var s = context.Node as StructDeclarationSyntax;//查找结构体声明语法

            //var parent = s.Parent as StructDeclarationSyntax;
            //if (parent == null) return;
            //TODO: 查找bit字段. 

            var name = s.Identifier.Text;
            if (name != "BitFields") return;

            if (!parent.ChildNodes().Any(n => n == s))
                return;

            var diagnostic = Diagnostic.Create(Rule, s.GetLocation(), parent.Identifier.Text);
            context.ReportDiagnostic(diagnostic);
        }

        public const string DiagnosticId = "BitXGenerator"; 
        //private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        //private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        //private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Code Generator";
        private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, "AnalyzerTitle", "AnalyzerMessageFormat", Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: "AnalyzerDescription");
    }
}


