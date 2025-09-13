using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MoreCommands.Generator;

[Generator]
public sealed class StaticCommandGenerator : ISourceGenerator
{
    private static readonly string CommandsNamespacePrefix = "MoreCommands.Commands";
    private const string RegistryNamespace = "MoreCommands.Common";

    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new Receiver());
    }

    public void Execute(GeneratorExecutionContext context)
    {
        if (context.SyntaxReceiver is not Receiver receiver)
        {
            return;
        }

        var compilation = context.Compilation;
        var actionTypeDef = compilation.GetTypeByMetadataName("System.Action`1");
        var stringType = compilation.GetSpecialType(SpecialType.System_String);
        var stringArrayType = stringType is null ? null : compilation.CreateArrayTypeSymbol(stringType);
        var commandTagType = compilation.GetTypeByMetadataName("MoreCommands.Common.CommandTag");

        var validCommandClasses = new List<INamedTypeSymbol>();
        var perClassAliases = new Dictionary<INamedTypeSymbol, List<string>>(SymbolEqualityComparer.Default);

        foreach (var cls in receiver.Candidates)
        {
            var model = compilation.GetSemanticModel(cls.SyntaxTree);
            if (model.GetDeclaredSymbol(cls) is not INamedTypeSymbol symbol)
            {
                continue;
            }

            // only top-level static classes in target namespace
            if (!symbol.IsStatic) continue;
            var ns = GetFullNamespace(symbol.ContainingNamespace);
            if (ns is null || !ns.StartsWith(CommandsNamespacePrefix, StringComparison.Ordinal)) continue;

            // methods/properties
            bool hasAliases = HasStaticPropertyOfType(symbol, "Aliases", stringArrayType);
            bool hasTag = HasStaticPropertyOfType(symbol, "Tag", commandTagType);
            bool hasCallback = HasStaticGetCallback(symbol, actionTypeDef, stringArrayType);
            bool hasDescription = HasStaticMemberOfType(symbol, "Description", stringType);

            if (!hasAliases)
            {
                Report(context, symbol, "MC0001", $"{symbol.Name} must declare public static string[] Aliases {{ get; }}.");
            }
            if (!hasTag)
            {
                Report(context, symbol, "MC0002", $"{symbol.Name} must declare public static MoreCommands.Common.CommandTag Tag {{ get; }}.");
            }
            if (!hasCallback)
            {
                Report(context, symbol, "MC0003", $"{symbol.Name} must declare public static System.Action<string[]> GetCallback().");
            }
            if (!hasDescription)
            {
                Report(context, symbol, "MC0008", $"{symbol.Name} must declare public static string Description {{ get; }} or public static string Description.");
            }

            if (hasAliases && hasTag && hasCallback && hasDescription)
            {
                validCommandClasses.Add(symbol);
                var aliases = TryExtractAliasesLiterals(symbol);
                if (aliases != null)
                {
                    // Empty check
                    if (aliases.Count == 0)
                    {
                        Report(context, symbol, "MC0004", $"{symbol.Name}.Aliases must contain at least one alias.");
                    }
                    // Per-class duplicates and empties
                    var seenLocal = new HashSet<string>(StringComparer.Ordinal);
                    foreach (var a in aliases)
                    {
                        if (string.IsNullOrWhiteSpace(a))
                        {
                            Report(context, symbol, "MC0005", $"{symbol.Name}.Aliases contains empty or whitespace alias which is not allowed.");
                        }
                        else if (!seenLocal.Add(a))
                        {
                            Report(context, symbol, "MC0007", $"{symbol.Name}.Aliases contains duplicate alias '{a}'. Aliases within a command must be unique.");
                        }
                    }
                    perClassAliases[symbol] = aliases;
                }
            }
        }

        // Cross-class uniqueness
        var aliasOwner = new Dictionary<string, INamedTypeSymbol>(StringComparer.Ordinal);
        foreach (var kvp in perClassAliases)
        {
            var owner = kvp.Key;
            foreach (var a in kvp.Value)
            {
                if (string.IsNullOrWhiteSpace(a)) continue;
                if (!aliasOwner.ContainsKey(a))
                {
                    aliasOwner.Add(a, owner);
                }
                else
                {
                    var firstOwner = aliasOwner[a];
                    Report(context, owner, "MC0006", $"Alias '{a}' is already used by {firstOwner.Name}. Aliases must be unique across commands.");
                }
            }
        }

        // Generate registration if any
        var src = GenerateRegistrationSource(validCommandClasses);
        context.AddSource("CommandRegistry_Registration.g.cs", src);
    }

    private static string GenerateRegistrationSource(List<INamedTypeSymbol> commands)
    {
        var sb = new StringBuilder();
        sb.AppendLine("// <auto-generated/>");
        sb.AppendLine($"namespace {RegistryNamespace};");
        sb.AppendLine();
        sb.AppendLine("public static partial class CommandRegistry");
        sb.AppendLine("{");
        sb.AppendLine("    static partial void RegisterAll()");
        sb.AppendLine("    {");
        foreach (var c in commands)
        {
            var fqn = c.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
            sb.AppendLine($"        Register(new CommandDescriptor {{");
            sb.AppendLine($"            Aliases = {fqn}.Aliases,");
            sb.AppendLine($"            Tag = {fqn}.Tag,");
            sb.AppendLine($"            Callback = {fqn}.GetCallback(),");
            sb.AppendLine($"            DeclaringType = typeof({fqn}),");
            sb.AppendLine($"            Description = {fqn}.Description,");
            sb.AppendLine($"        }});");
        }
        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }

    private static bool HasStaticPropertyOfType(INamedTypeSymbol type, string name, ITypeSymbol expected)
    {
        if (expected is null) return false;
        var prop = type.GetMembers(name).OfType<IPropertySymbol>().FirstOrDefault(p => p.IsStatic && p.DeclaredAccessibility == Accessibility.Public);
        return prop is not null && SymbolEqualityComparer.Default.Equals(prop.Type, expected);
    }

    private static bool HasStaticMemberOfType(INamedTypeSymbol type, string name, ITypeSymbol expected)
    {
        if (expected is null) return false;
        var prop = type.GetMembers(name).OfType<IPropertySymbol>().FirstOrDefault(p => p.IsStatic && p.DeclaredAccessibility == Accessibility.Public);
        if (prop is not null && SymbolEqualityComparer.Default.Equals(prop.Type, expected)) return true;
        var field = type.GetMembers(name).OfType<IFieldSymbol>().FirstOrDefault(f => f.IsStatic && f.DeclaredAccessibility == Accessibility.Public);
        return field is not null && SymbolEqualityComparer.Default.Equals(field.Type, expected);
    }

    private static bool HasStaticGetCallback(INamedTypeSymbol type, INamedTypeSymbol actionDef, ITypeSymbol stringArray)
    {
        if (actionDef is null || stringArray is null) return false;
        var method = type.GetMembers("GetCallback").OfType<IMethodSymbol>().FirstOrDefault(m => m.IsStatic && m.DeclaredAccessibility == Accessibility.Public && m.Parameters.Length == 0);
        if (method is null) return false;
        var constructed = actionDef.Construct(stringArray);
        return SymbolEqualityComparer.Default.Equals(method.ReturnType, constructed);
    }

    private static string GetFullNamespace(INamespaceSymbol ns)
    {
        if (ns is null || ns.IsGlobalNamespace) return string.Empty;
        var parts = new List<string>();
        var cur = ns;
        while (cur != null && !cur.IsGlobalNamespace)
        {
            parts.Add(cur.Name);
            cur = cur.ContainingNamespace;
        }
        parts.Reverse();
        return string.Join(".", parts);
    }

    private static void Report(GeneratorExecutionContext ctx, INamedTypeSymbol symbol, string id, string message)
    {
        var diag = Diagnostic.Create(new DiagnosticDescriptor(id, id, message, "Usage", DiagnosticSeverity.Error, isEnabledByDefault: true), symbol.Locations.FirstOrDefault());
        ctx.ReportDiagnostic(diag);
    }

    private static List<string> TryExtractAliasesLiterals(INamedTypeSymbol commandType)
    {
        // Prefer property named Aliases
        var member = commandType.GetMembers("Aliases").FirstOrDefault();
        if (member is IPropertySymbol prop)
        {
            foreach (var r in prop.DeclaringSyntaxReferences)
            {
                if (r.GetSyntax() is PropertyDeclarationSyntax p)
                {
                    // Expression-bodied property: public static string[] Aliases => ["foo", "bar"]; or return new[] { ... };
                    var expr = p.ExpressionBody?.Expression;
                    if (expr != null)
                    {
                        var vals = ExtractStringLiterals(expr);
                        if (vals != null) return vals;
                    }
                    // Accessor with return statement
                    if (p.AccessorList != null)
                    {
                        foreach (var acc in p.AccessorList.Accessors)
                        {
                            var body = acc.ExpressionBody?.Expression ?? (SyntaxNode)acc.Body;
                            if (body != null)
                            {
                                var vals = ExtractStringLiterals(body);
                                if (vals != null) return vals;
                            }
                        }
                    }
                }
            }
        }
        else if (member is IFieldSymbol field)
        {
            foreach (var r in field.DeclaringSyntaxReferences)
            {
                if (r.GetSyntax() is VariableDeclaratorSyntax v && v.Initializer != null)
                {
                    var vals = ExtractStringLiterals(v.Initializer.Value);
                    if (vals != null) return vals;
                }
            }
        }
        return null;
    }

    private static List<string> ExtractStringLiterals(SyntaxNode node)
    {
        var tokens = node.DescendantTokens().Where(t => t.IsKind(SyntaxKind.StringLiteralToken)).ToList();
        if (tokens.Count == 0) return new List<string>();
        var result = new List<string>(tokens.Count);
        foreach (var t in tokens)
        {
            result.Add(t.ValueText);
        }
        return result;
    }
}

file sealed class Receiver : ISyntaxReceiver
{
    public List<ClassDeclarationSyntax> Candidates { get; } = new List<ClassDeclarationSyntax>();

    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax cds && cds.Modifiers.Any(m => m.IsKind(SyntaxKind.StaticKeyword)))
        {
            Candidates.Add(cds);
        }
    }
}


