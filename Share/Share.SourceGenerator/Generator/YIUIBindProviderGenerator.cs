using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using ET.Analyzer;

namespace ET.Generator;

[Generator(LanguageNames.CSharp)]
public class YIUIBindProviderGenerator : ISourceGenerator
{
    private const string YIUIAttribute = "YIUIFramework.YIUIAttribute";
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(SyntaxReceiver.Create);
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxContextReceiver is not SyntaxReceiver receiver)
        {
            return;
        }
        var datas = receiver.Datas;

        var sb = new StringBuilder();
        sb.AppendLine($"//count = {datas.Count}");
        foreach (var data in datas)
        {
            if (data == null) continue;
            try
            {

                var classType = data.classTypeSymbol.ToDisplayString();

                var codeType = "Panel";
                var panelLayer = "Any";
                if (data.AttributeData.ConstructorArguments.Length == 2)
                {
                    panelLayer = data.AttributeData.ConstructorArguments[1].ToCSharpString();
                }

                codeType = data.AttributeData.ConstructorArguments[0].ToCSharpString();
                sb.AppendLine($$"""
                                new YIUIBindVo
                                {
                                    PkgName       = {{classType}}.PkgName,
                                    ResName       = {{classType}}.ResName,
                                    CodeType      = {{codeType}},
                                    PanelLayer    = {{panelLayer}},
                                    ComponentType = typeof({{classType}}),
                                },
                """);

            }
            catch (Exception ex)
            {
                sb.AppendLine($"// ERROR: {ex.Message}");
            }
        }

        var bindingCode = $$"""
        using YIUIFramework;
        namespace YIUICodeGenerated
        {
            public static class YIUIBindProvider
            {
                public static YIUIBindVo[] Get()
                {
                    var list          = new YIUIBindVo[]
                    {
                        {{sb}}
                    };
                    return list;
                }
            }
        }
        """;
        if (datas.Count == 0) context.AddSource("YIUIBindProvider.g.cs", sb.ToString());
        else context.AddSource("YIUIBindProvider.g.cs", bindingCode);
    }

    class SyntaxReceiver : ISyntaxContextReceiver
    {
        internal static ISyntaxContextReceiver Create()
        {
            return new SyntaxReceiver();
        }
        public List<YIUIData> Datas { get; } = new List<YIUIData>();

        public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
        {
            if (context.Node is not ClassDeclarationSyntax classDeclarationSyntax
                || classDeclarationSyntax.AttributeLists.Count == 0) return;

            var classTypeSymbol = context.SemanticModel.GetDeclaredSymbol(context.Node) as INamedTypeSymbol;
            if(classTypeSymbol == null) return;

            var attribute = classTypeSymbol.GetFirstAttribute(YIUIAttribute);
            if (attribute == null) return;

            this.Datas.Add(new YIUIData()
            {
                classTypeSymbol = classTypeSymbol,
                AttributeData = attribute,
            });
        }
    }

    public record struct YIUIData
    {
        public INamedTypeSymbol classTypeSymbol;
        public AttributeData AttributeData;
    }
}
