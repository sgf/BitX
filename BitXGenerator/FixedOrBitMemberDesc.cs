using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace BitX;

public static class LogS
{

    public static void WriteLine(string msg)
    {
        Log.Print(msg);
        Trace.WriteLine(msg);
    }


}

internal record struct FixedOrBitMemberDesc(bool IsMissing, bool IsFixedOrBit, string TypeName, int FixedOrBitSize, int BitOffset)
{
    //public readonly string FieldName;
    //public readonly string TypeName;

    public static ConcurrentDictionary<int, byte> CachedFixedCode = new();
    public static ConcurrentDictionary<long, byte> CachedBitCode = new();
    internal static bool Get(SyntaxNode syntaxNode, out FixedOrBitMemberDesc fixedFieldDesc, out TypeSyntax ts)
    {
        ts = default!;
        fixedFieldDesc = default;
        if (syntaxNode.IsIncompleteMember(out var ims))
            ts = ims.GetTypeSyntax()!;
        else if (syntaxNode.IsField(out var fds))
            ts = fds.GetTypeSyntax();

        if (ts?.GetTypeDesc(out var typeDesc) != true)
        {
            //if (!Debugger.IsAttached)
            //    Debugger.Launch();
            //Log.Print(ts.GetText().ToString());
            if (ts == null) return false;
            return false;
        }
        if (!typeDesc.IsFixed && !typeDesc.IsBit)
        {
            if (!Debugger.IsAttached)
                Debugger.Launch();
            //Log.Print(ts.GetText().ToString());
            return false;
        }

        if (typeDesc.IsFixed)
        {
            if (!CachedFixedCode.TryAdd(typeDesc.FixedOrBitSize, 0))
                return false;
        }
        else if (typeDesc.IsBit)
        {
            if (!CachedBitCode.TryAdd((long)typeDesc.FixedOrBitSize << 32 | (long)typeDesc.BitOffset, 0))
                return false;
        }
        else
        {
            //context.ReportDiagnostic(Diagnostic.Create(
        }
        fixedFieldDesc = new(typeDesc.IsMissing, typeDesc.IsFixed == true, typeDesc.Name, typeDesc.FixedOrBitSize, typeDesc.BitOffset);
        return true;
    }


}


public record struct TypeDesc(string Name, bool IsGeneric, string[] GenericArgTypeNames
    , bool IsMissing, bool IsFixed, bool IsBit, int FixedOrBitSize = 0, int BitOffset = 0)
{
    public string FirstGenericArgTypeName => GenericArgTypeNames.FirstOrDefault();
    //public bool IsFixed => Name.Length > "Fixed".Length && Name.StartsWith("Fixed");
}

public static class SyntaxEx
{
    public static bool IsParentStruct(this SyntaxNode node)
    {
        var parent = node.Parent;
        return (parent.IsKind(SyntaxKind.RecordStructDeclaration) || parent.IsKind(SyntaxKind.StructDeclaration));
    }

    public static bool IsStructFieldOrIncompleteMember(this SyntaxNode node)
    {
        if ((node.IsField() | node.IsIncompleteMember()) &&
            node.IsParentStruct())
            return true;
        return false;
    }

    public static bool IsField(this SyntaxNode syntaxNode)
        => syntaxNode.IsKind(SyntaxKind.FieldDeclaration);

    public static bool IsField(this SyntaxNode syntaxNode, out FieldDeclarationSyntax fds)
    {
        var isIcm = syntaxNode.IsKind(SyntaxKind.FieldDeclaration);
        if (isIcm && syntaxNode is FieldDeclarationSyntax _fds)
        {
            fds = _fds;
            return true;
        }
        fds = default!;
        return false;
    }

    public static bool IsIncompleteMember(this SyntaxNode syntaxNode)
        => syntaxNode.IsKind(SyntaxKind.IncompleteMember);

    public static bool IsIncompleteMember(this SyntaxNode syntaxNode, out IncompleteMemberSyntax ims)
    {
        var isIcm = syntaxNode.IsKind(SyntaxKind.IncompleteMember);
        if (isIcm && syntaxNode is IncompleteMemberSyntax _ims)
        {
            ims = _ims;
            return true;
        }
        ims = default!;
        return false;
    }

    public static bool IsGenericType(this IncompleteMemberSyntax ims, out GenericNameSyntax gns)//IdentifierNameSyntax
    {
        if (ims.Type?.IsGeneric(out var _gns) == true)
        {
            gns = _gns;
            return true;
        }
        gns = default!;
        return false;
    }

    public static bool GetTypeDesc(this FieldDeclarationSyntax fds, out TypeDesc typeDesc)
    {
        var type = fds.GetTypeSyntax();
        if (type == null)
        {
            typeDesc = default!;
            return false;
        }
        return type.GetTypeDesc(out typeDesc);
    }


    public static bool GetTypeDesc(this IncompleteMemberSyntax ims, out TypeDesc typeDesc)
    {
        var type = ims.GetTypeSyntax();
        if (type == null)
        {
            typeDesc = default!;
            return false;
        }
        return type.GetTypeDesc(out typeDesc);
    }

    public static bool GetTypeDesc(this TypeSyntax ts, out TypeDesc typeDesc)
    {
        var typeIsMissing = ts.IsMissing;
        var typeName = "";
        var isGeneric = false;
        var typeArguments = Array.Empty<string>();
        if (ts.IsNormal(out var ins))
        {
            typeName = ins.TypeName();
            isGeneric = false;
        }
        else if (ts.IsGeneric(out var gns))
        {
            typeName = gns.TypeName();
            isGeneric = true;
            typeArguments = gns.TypeArgumentList.Arguments.Select(x => x.TypeName()).ToArray();
        }
        if (typeName.Length > "Fixed".Length && typeName.StartsWith("Fixed"))
        {
            if (int.TryParse(typeName.Replace("Fixed", ""), out var cntOfStructs)
                && cntOfStructs > 0)//必须大于0,否则没意义
            {
                typeDesc = new(typeName, isGeneric, typeArguments, typeIsMissing, IsFixed: true, IsBit: false, cntOfStructs);
                return true;
            }
        }
        else if (typeName.Length >= "Bit0_0".Length && typeName.StartsWith("Bit"))
        {
            var partsOfTypeName = typeName.Replace("Bit", "").Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            if (partsOfTypeName.Length == 2 && int.TryParse(partsOfTypeName[0], out var cntOfBit) && int.TryParse(partsOfTypeName[1], out var bitOffset))
            {
                typeDesc = new(typeName, isGeneric, typeArguments, typeIsMissing, IsFixed: false, IsBit: true, cntOfBit, bitOffset);
                return true;
            }
        }
        typeDesc = default;
        return false;
    }


    public static bool IsGenericType(this FieldDeclarationSyntax fds, out GenericNameSyntax gns)
    {
        if (fds.Declaration.Type.IsGeneric(out var _gns))
        {
            gns = _gns;
            return true;
        }
        gns = default!;
        return false;
    }


    public static bool IsNormal(this TypeSyntax typeSyntax, out IdentifierNameSyntax ins)
    {
        if (typeSyntax is IdentifierNameSyntax _ins)
        {
            ins = _ins;
            return true;
        }
        ins = default!;
        return false;
    }
    public static bool IsGeneric(this TypeSyntax typeSyntax, out GenericNameSyntax gns)
    {
        if (typeSyntax is GenericNameSyntax _gns)
        {
            gns = _gns;
            return true;
        }
        gns = default!;
        return false;
    }

    //public static string FieldTypeName(this FieldDeclarationSyntax fieldDeclarationSyntax)
    //{
    //    var declaration = fieldDeclarationSyntax.Declaration;
    //    declaration.Variables.FirstOrDefault();

    //    if (fieldTypeSynTax is GenericNameSyntax fieldTypeGenericNameSynTax
    //    declaration.Type.
    //}


    public static TypeSyntax GetTypeSyntax(this FieldDeclarationSyntax fds)
    => fds.Declaration.Type;

    public static TypeSyntax? GetTypeSyntax(this IncompleteMemberSyntax ims)
    => ims.Type;

    public static string TypeName(this SimpleNameSyntax sns)
        => sns.Identifier.Text;

    public static string TypeName(this TypeSyntax ts)
    {
        if (ts.IsNormal(out var ins))
            return ins.TypeName();
        else if (ts.IsGeneric(out var gns))
            return gns.TypeName();
        return "";
    }

    //public static string TypeName(this IdentifierNameSyntax ins)
    //{
    //    var typeName = ins.Identifier.Text;
    //    //var genericArgType = gns.TypeArgumentList.Arguments.FirstOrDefault();//泛型参数
    //    return typeName;
    //}
    //public static string TypeName(this GenericNameSyntax gns)
    //{
    //    var typeName = gns.Identifier.Text;
    //    //var genericArgType = gns.TypeArgumentList.Arguments.FirstOrDefault();//泛型参数
    //    return typeName;
    //}


}