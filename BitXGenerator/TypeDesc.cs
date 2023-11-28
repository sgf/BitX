using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;

namespace BitX;

public static class LogS
{

    public static void WriteLine(string msg)
    {
        Log.Print(msg);
        Trace.WriteLine(msg);
    }


}

public record struct TypeDesc(string Name, bool IsGeneric, string[] GenericArgTypeNames, bool IsFixed, bool IsBit, int FixedOrBitSize = 0, int BitOffset = 0):IEquatable<TypeDesc>
{
    public string FirstGenericArgTypeName => GenericArgTypeNames.FirstOrDefault();
    //public bool IsFixed => Name.Length > "Fixed".Length && Name.StartsWith("Fixed");

    public bool Equals(TypeDesc other) => this.Name.Equals(other.Name);

    public override int GetHashCode() => this.Name.GetHashCode();

}

public static class SyntaxEx
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsStruct(this SyntaxNode node)
    {
        return (node.IsKind(SyntaxKind.RecordStructDeclaration) || node.IsKind(SyntaxKind.StructDeclaration));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParentStruct(this SyntaxNode node)
    {
        var parent = node.Parent;
        return (parent.IsKind(SyntaxKind.RecordStructDeclaration) || parent.IsKind(SyntaxKind.StructDeclaration));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsStructFieldOrIncompleteMember(this SyntaxNode node)
    {
        if ((node.IsField() | node.IsIncompleteMember()) &&
            node.IsParentStruct())
            return true;
        return false;
    }


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsFieldOrIncompleteMember(this SyntaxNode node)
    {
        if ((node.IsField() | node.IsIncompleteMember()))
            return true;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsField(this SyntaxNode syntaxNode)
        => syntaxNode.IsKind(SyntaxKind.FieldDeclaration);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsIncompleteMember(this SyntaxNode syntaxNode)
        => syntaxNode.IsKind(SyntaxKind.IncompleteMember);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                typeDesc = new(typeName, isGeneric, typeArguments, IsFixed: true, IsBit: false, cntOfStructs);
                return true;
            }
        }
        else if (typeName.Length >= "Bit0_0".Length && typeName.StartsWith("Bit"))
        {
            var partsOfTypeName = typeName.Replace("Bit", "").Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            if (partsOfTypeName.Length == 2 && int.TryParse(partsOfTypeName[0], out var cntOfBit) && int.TryParse(partsOfTypeName[1], out var bitOffset))
            {
                typeDesc = new(typeName, isGeneric, typeArguments, IsFixed: false, IsBit: true, cntOfBit, bitOffset);
                return true;
            }
        }
        typeDesc = default;
        return false;
    }

    public static bool IsFixedTypeName(string typeName, out TypeDesc typeDesc)
    {
        if (typeName.Length > "Fixed".Length && typeName.StartsWith("Fixed"))
        {
            if (int.TryParse(typeName.Replace("Fixed", ""), out var cntOfStructs)
                && cntOfStructs > 0)//必须大于0,否则没意义
            {
                typeDesc = new(typeName, default, Array.Empty<string>(), IsFixed: true, IsBit: false, cntOfStructs);
                return true;
            }
        }
        typeDesc = default; return false;
    }

    public static bool IsBitTypeName(string typeName,out TypeDesc typeDesc)
    {
        if (typeName.Length >= "Bit0_0".Length && typeName.StartsWith("Bit"))
        {
            var partsOfTypeName = typeName.Replace("Bit", "").Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
            if (partsOfTypeName.Length == 2 && int.TryParse(partsOfTypeName[0], out var cntOfBit) && int.TryParse(partsOfTypeName[1], out var bitOffset))
            {
                typeDesc = new(typeName, default, Array.Empty<string>(), IsFixed: false, IsBit: true, cntOfBit, bitOffset);
                return true;
            }
        }
        typeDesc = default; return false;
    }



    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeSyntax GetTypeSyntax(this FieldDeclarationSyntax fds)
    => fds.Declaration.Type;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TypeSyntax? GetTypeSyntax(this IncompleteMemberSyntax ims)
    => ims.Type;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string TypeName(this SimpleNameSyntax sns)
        => sns.Identifier.Text;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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