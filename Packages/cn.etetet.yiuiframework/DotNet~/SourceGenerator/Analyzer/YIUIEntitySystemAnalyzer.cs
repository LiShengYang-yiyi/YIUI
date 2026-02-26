using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace ET
{
    public static class YIUIEntitySystemAnalyzerRule
    {
        private const string Title = "YIUI Entity类存在未实现的事件函数";

        private const string MessageFormat = "YIUI Entity类: {0} 存在未实现的事件函数";

        private const string Description = "YIUI Entity类存在未实现的事件函数.";

        public static readonly DiagnosticDescriptor Rule =
                new(YIUIDiagnosticIds.YIUIEntitySystemAnalyzerRuleId,
                    Title,
                    MessageFormat,
                    YIUIDiagnosticCategories.Model,
                    DiagnosticSeverity.Error,
                    true,
                    Description);
    }

    public static class YIUIEntitySystemMethodNeedSystemOfAttrAnalyzerRule
    {
        private const string Title = "YIUI EntitySystem标签只能添加在含有EntitySystemOf标签的静态类中";

        private const string MessageFormat = "YIUI 方法:{0}的{1}标签只能添加在含有{2}标签的静态类中";

        private const string Description = "YIUI EntitySystem标签只能添加在含有EntitySystemOf标签的静态类中.";

        public static readonly DiagnosticDescriptor Rule =
                new(YIUIDiagnosticIds.YIUIEntitySystemMethodNeedSystemOfAttrAnalyzerRuleId,
                    Title,
                    MessageFormat,
                    YIUIDiagnosticCategories.Model,
                    DiagnosticSeverity.Error,
                    true,
                    Description);
    }

    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class YIUIEntitySystemAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(YIUIEntitySystemAnalyzerRule.Rule, YIUIEntitySystemMethodNeedSystemOfAttrAnalyzerRule.Rule);

        public override void Initialize(AnalysisContext context)
        {
            if (!AnalyzerGlobalSetting.EnableAnalyzer)
            {
                return;
            }

            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(this.Analyzer, SymbolKind.NamedType);
            context.RegisterSymbolAction(this.AnalyzeIsSystemMethodValid, SymbolKind.NamedType);
        }

        private static ImmutableArray<ETSystemData> SupportedETSystemDatas => ImmutableArray.Create(new ETSystemData(Definition.EntitySystemOfAttribute, Definition.EntitySystemAttribute, Definition.EntityType, Definition.EntitySystemAttributeMetaName,

            //自定义
            new SystemMethodData("ET.Client.IYIUIWindowClose", "YIUIWindowClose", "async ETTask", "bool"),
            new SystemMethodData("ET.Client.IYIUIBackClose", "YIUIBackClose", "async ETTask", "ET.Client.YIUIEventPanelInfo"),
            new SystemMethodData("ET.Client.IYIUIBackOpen", "YIUIBackOpen", "async ETTask", "ET.Client.YIUIEventPanelInfo"),
            new SystemMethodData("ET.Client.IYIUIBackHomeClose", "YIUIBackHomeClose", "async ETTask", "ET.Client.YIUIEventPanelInfo"),

            //async ETTask<bool>
            new SystemMethodData("ET.Client.IYIUIClose", "YIUIClose", "async ETTask<bool>"),
            new SystemMethodData("ET.Client.IYIUIOpen", "YIUIOpen", "async ETTask<bool>"),
            new SystemMethodData("ET.Client.IYIUIDisClose", "YIUIDisClose", "async ETTask<bool>"),

            //async ETTask
            new SystemMethodData("ET.IAwakeAsync", "AwakeAsync", "async ETTask"),
            new SystemMethodData("ET.IDynamicEvent", "DynamicEvent", "async ETTask"),
            new SystemMethodData("ET.Client.IYIUIBackHomeOpen", "YIUIBackHomeOpen", "async ETTask"),
            new SystemMethodData("ET.Client.IYIUICloseTween", "YIUICloseTween", "async ETTask"),
            new SystemMethodData("ET.Client.IYIUIOpenTween", "YIUIOpenTween", "async ETTask"),

            //void
            new SystemMethodData("ET.Client.IYIUIBind", "YIUIBind"),
            new SystemMethodData("ET.Client.IYIUIDisable", "YIUIDisable"),
            new SystemMethodData("ET.Client.IYIUIEnable", "YIUIEnable"),
            new SystemMethodData("ET.Client.IYIUIInitialize", "YIUIInitialize"),
            new SystemMethodData("ET.Client.IYIUICloseTweenEnd", "YIUICloseTweenEnd"),
            new SystemMethodData("ET.Client.IYIUIOpenTweenEnd", "YIUIOpenTweenEnd")));

        private class ETSystemData
        {
            public string             EntityTypeName;
            public string             SystemOfAttribute;
            public string             SystemAttributeShowName;
            public string             SystemAttributeMetaName;
            public SystemMethodData[] SystemMethods;

            public ETSystemData(string systemOfAttribute, string systemAttributeShowName, string entityTypeName, string systemAttributeMetaName, params SystemMethodData[] systemMethods)
            {
                this.SystemOfAttribute       = systemOfAttribute;
                this.SystemAttributeShowName = systemAttributeShowName;
                this.EntityTypeName          = entityTypeName;
                this.SystemAttributeMetaName = systemAttributeMetaName;
                this.SystemMethods           = systemMethods;
            }
        }

        private struct SystemMethodData
        {
            public string   InterfaceName;
            public string   MethodName;
            public string   MethodReturnType;
            public string   ExtraParameter;
            public string[] ExtraParameterTypes;

            public SystemMethodData(string interfaceName, string methodName, string methodReturnType = "", string extraParameter = "")
            {
                InterfaceName = interfaceName;
                MethodName    = methodName;
                if (!string.IsNullOrEmpty(methodReturnType))
                {
                    MethodReturnType = $"{methodName}|{methodReturnType}";
                }
                else
                {
                    MethodReturnType = methodName;
                }

                ExtraParameter      = extraParameter;
                ExtraParameterTypes = extraParameter.Split('/');
            }
        }

        private void Analyzer(SymbolAnalysisContext context)
        {
            if (!(context.Symbol is INamedTypeSymbol namedTypeSymbol))
            {
                return;
            }

            ImmutableDictionary<string, string?>.Builder? builder = null;
            foreach (ETSystemData? supportedEtSystemData in SupportedETSystemDatas)
            {
                if (supportedEtSystemData != null)
                {
                    this.AnalyzeETSystem(context, supportedEtSystemData, ref builder);
                }
            }

            this.ReportNeedGenerateSystem(context, namedTypeSymbol, ref builder);
        }

        private void AnalyzeETSystem(SymbolAnalysisContext context, ETSystemData etSystemData, ref ImmutableDictionary<string, string?>.Builder? builder)
        {
            if (!(context.Symbol is INamedTypeSymbol namedTypeSymbol))
            {
                return;
            }

            // 筛选出含有SystemOf标签的类
            AttributeData? attr = namedTypeSymbol.GetFirstAttribute(etSystemData.SystemOfAttribute);
            if (attr == null)
            {
                return;
            }

            // 获取所属的实体类symbol
            if (attr.ConstructorArguments[0].Value is not INamedTypeSymbol entityTypeSymbol)
            {
                return;
            }

            bool ignoreAwake = false;
            if (attr.ConstructorArguments.Length >= 2 && attr.ConstructorArguments[1].Value is bool ignore)
            {
                ignoreAwake = ignore;
            }

            // 排除非Entity子类
            if (entityTypeSymbol.BaseType?.ToString() != etSystemData.EntityTypeName)
            {
                return;
            }

            foreach (INamedTypeSymbol? interfaceTypeSymbol in entityTypeSymbol.AllInterfaces)
            {
                if (ignoreAwake && interfaceTypeSymbol.IsInterface(Definition.IAwakeInterface))
                {
                    continue;
                }

                foreach (SystemMethodData systemMethodData in etSystemData.SystemMethods)
                {
                    if (interfaceTypeSymbol.IsInterface(systemMethodData.InterfaceName))
                    {
                        var methodName = systemMethodData.MethodName;
                        var str        = new StringBuilder();

                        if (interfaceTypeSymbol.IsGenericType)
                        {
                            bool has = false;
                            if (!string.IsNullOrEmpty(systemMethodData.ExtraParameter))
                            {
                                has = !HasMethodWithExtraParams(namedTypeSymbol, methodName, entityTypeSymbol, interfaceTypeSymbol.TypeArguments, systemMethodData.ExtraParameterTypes);
                            }
                            else
                            {
                                var typeArgs = ImmutableArray.Create<ITypeSymbol>(entityTypeSymbol).AddRange(interfaceTypeSymbol.TypeArguments);
                                has = !namedTypeSymbol.HasMethodWithParams(methodName, typeArgs.ToArray());
                            }

                            if (has)
                            {
                                var args = new StringBuilder();
                                str.Append(entityTypeSymbol);
                                str.Append("/");
                                str.Append(etSystemData.SystemAttributeShowName);
                                foreach (ITypeSymbol? typeArgument in interfaceTypeSymbol.TypeArguments)
                                {
                                    str.Append("/");
                                    str.Append(typeArgument);
                                    args.Append(typeArgument);
                                }

                                if (!string.IsNullOrEmpty(systemMethodData.ExtraParameter))
                                {
                                    str.Append("/");
                                    str.Append(systemMethodData.ExtraParameter);
                                }

                                AddProperty(ref builder, $"{systemMethodData.MethodReturnType}`{args}", str.ToString());
                            }
                        }
                        else
                        {
                            bool has = false;
                            if (!string.IsNullOrEmpty(systemMethodData.ExtraParameter))
                            {
                                has = !HasMethodWithExtraParams(namedTypeSymbol, methodName, entityTypeSymbol, interfaceTypeSymbol.TypeArguments, systemMethodData.ExtraParameterTypes);
                            }
                            else
                            {
                                has = !namedTypeSymbol.HasMethodWithParams(methodName, entityTypeSymbol);
                            }

                            if (has)
                            {
                                str.Append(entityTypeSymbol);
                                str.Append("/");
                                str.Append(etSystemData.SystemAttributeShowName);
                                if (!string.IsNullOrEmpty(systemMethodData.ExtraParameter))
                                {
                                    str.Append($"/{systemMethodData.ExtraParameter}");
                                }

                                AddProperty(ref builder, systemMethodData.MethodReturnType, str.ToString());
                            }
                        }

                        break;
                    }
                }
            }
        }

        //额外参数判断
        private bool HasMethodWithExtraParams(INamedTypeSymbol namedTypeSymbol, string methodName, ITypeSymbol? self, ImmutableArray<ITypeSymbol> typeArguments, string[] extraParameterTypes)
        {
            var tempArgumentList = new List<string>();

            if (self != null)
            {
                tempArgumentList.Add(self.ToString());
            }

            if (typeArguments != null)
            {
                foreach (ITypeSymbol? typeArgument in typeArguments)
                {
                    if (typeArgument != null)
                    {
                        tempArgumentList.Add(typeArgument.ToString());
                    }
                }
            }

            if (extraParameterTypes is { Length: > 0 })
            {
                tempArgumentList.AddRange(extraParameterTypes);
            }

            foreach (var member in namedTypeSymbol.GetMembers())
            {
                if (member is not IMethodSymbol methodSymbol)
                {
                    continue;
                }

                if (methodSymbol.Name != methodName)
                {
                    continue;
                }

                if (tempArgumentList.Count == 0)
                {
                    return true;
                }

                if (tempArgumentList.Count != methodSymbol.Parameters.Length)
                {
                    return false;
                }

                for (int i = 0; i < tempArgumentList.Count; i++)
                {
                    if (tempArgumentList[i] != methodSymbol.Parameters[i].Type.ToString())
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        private void AddProperty(ref ImmutableDictionary<string, string?>.Builder? builder, string methodMetaName, string methodArgs)
        {
            if (builder == null)
            {
                builder = ImmutableDictionary.CreateBuilder<string, string?>();
            }

            if (builder.TryGetValue(Definition.EntitySystemInterfaceSequence, out string? seqValue))
            {
                builder[Definition.EntitySystemInterfaceSequence] = $"{seqValue}/{methodMetaName}";
            }
            else
            {
                builder.Add(Definition.EntitySystemInterfaceSequence, methodMetaName);
            }

            if (!builder.ContainsKey(methodMetaName))
            {
                builder.Add(methodMetaName, methodArgs);
            }
        }

        private void ReportNeedGenerateSystem(SymbolAnalysisContext context, INamedTypeSymbol namedTypeSymbol, ref ImmutableDictionary<string, string?>.Builder? builder)
        {
            if (builder == null)
            {
                return;
            }

            foreach (SyntaxReference? reference in namedTypeSymbol.DeclaringSyntaxReferences)
            {
                if (reference.GetSyntax() is ClassDeclarationSyntax classDeclarationSyntax)
                {
                    Diagnostic diagnostic = Diagnostic.Create(YIUIEntitySystemAnalyzerRule.Rule, classDeclarationSyntax.Identifier.GetLocation(),
                        builder.ToImmutable(), namedTypeSymbol.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }

        private void AnalyzeIsSystemMethodValid(SymbolAnalysisContext context)
        {
            if (!(context.Symbol is INamedTypeSymbol namedTypeSymbol))
            {
                return;
            }

            foreach (ISymbol? symbol in namedTypeSymbol.GetMembers())
            {
                if (symbol is not IMethodSymbol methodSymbol)
                {
                    continue;
                }

                foreach (var etSystemData in SupportedETSystemDatas)
                {
                    if (!methodSymbol.HasAttribute(etSystemData.SystemAttributeMetaName))
                    {
                        continue;
                    }

                    if (methodSymbol.Parameters.Length == 0)
                    {
                        continue;
                    }

                    AttributeData? attr = namedTypeSymbol.GetFirstAttribute(etSystemData.SystemOfAttribute);
                    if (attr == null || attr.ConstructorArguments[0].Value is not INamedTypeSymbol entityTypeSymbol
                     || entityTypeSymbol.ToString() != methodSymbol.Parameters[0].Type.ToString())
                    {
                        ReportNeedSystemOfAttr(context, methodSymbol, etSystemData);
                    }
                }
            }
        }

        private void ReportNeedSystemOfAttr(SymbolAnalysisContext context, IMethodSymbol methodSymbol, ETSystemData etSystemData)
        {
            foreach (SyntaxReference? reference in methodSymbol.DeclaringSyntaxReferences)
            {
                if (reference.GetSyntax() is MethodDeclarationSyntax methodDeclarationSyntax)
                {
                    Diagnostic diagnostic = Diagnostic.Create(YIUIEntitySystemMethodNeedSystemOfAttrAnalyzerRule.Rule, methodDeclarationSyntax.Identifier.GetLocation()
                      , methodSymbol.Name, etSystemData.SystemAttributeShowName, etSystemData.SystemOfAttribute);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}