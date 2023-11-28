using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Resources;
using System.Text;
using BitX;

namespace BitXGenerator
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class BitNAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "BitNAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        //private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.BitNAnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Syntax"; 
        private const string BitNAnalyzerDescription = "An attempt was made to assign a value to a variable whose type cannot store the value."; 
        private const string BitNAnalyzerTitle = "Literal conversion error";
        private const string BitNAnalyzerMessageFormat = "Constant value '{0}' cannot be converted to a '{1}'";
        private static DiagnosticDescriptor Rule = 
            new (DiagnosticId, BitNAnalyzerTitle, BitNAnalyzerMessageFormat, Category, 
                DiagnosticSeverity.Error, isEnabledByDefault: true, description: BitNAnalyzerDescription);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzerNumericAction, SyntaxKind.NumericLiteralExpression);
        }

        private void AnalyzerNumericAction(SyntaxNodeAnalysisContext obj)
        {
            var node = (LiteralExpressionSyntax)obj.Node;
            var t = obj.SemanticModel.GetTypeInfo(node);
            if (t.ConvertedType == null) return;

            var typeName = t.ConvertedType.Name;
            if(SyntaxEx.IsBitTypeName(typeName,out var td))
            {
                var num = Util.GetNumber(node.Token.Value);
                var bitMax = FixedOrBitGenerator.GetMaxByBitCount(td.FixedOrBitSize);
                var dig = Util.Log2(num);
                if (dig> (int)bitMax)
                    obj.ReportDiagnostic(Diagnostic.Create(Rule, node.GetLocation(), node.Token.Text, typeName));
                //else
            }
            else
            {
                //todo: UnaryPlus + Literal: -)
                //todo: UnaryMinus + Literal: -)
                //todo: Cast + Literal: (byte)1
            }

        }
    }
}
