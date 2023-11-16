using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace BitXGenerator
{
    internal class FileName
    {
        static void EmitAttributes(IncrementalGeneratorPostInitializationContext context)
        {
            context.AddSource("StructureOfArraysGeneratorAttributes.cs", """
using System;

namespace StructureOfArraysGenerator
{
    internal interface IMultiArray<T>
    {
        int Length { get; }
        ReadOnlySpan<byte> GetRawSpan();
#if NET7_0_OR_GREATER
        static abstract int GetByteSize(int length);
        static abstract T Create(int length, ArraySegment<byte> arrayOffset);
#endif
    }

    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    internal sealed class MultiArrayAttribute : Attribute
    {
        public Type Type { get; }
        public string[] Members { get; }
        public bool IncludeProperty { get; }

        public MultiArrayAttribute(Type type, bool includeProperty = false)
        {
            this.Type = type;
            this.IncludeProperty = includeProperty;
            this.Members = Array.Empty<string>();
        }

        public MultiArrayAttribute(Type type, params string[] members)
        {
            this.Type = type;
            this.IncludeProperty = false;
            this.Members = members;
        }
    }

    [AttributeUsage(AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    internal sealed class MultiArrayListAttribute : Attribute
    {
        public string TypeName { get; }

        public MultiArrayListAttribute()
        {
            this.TypeName = "";
        }

        public MultiArrayListAttribute(string typeName)
        {
            this.TypeName = typeName;
        }
    }
}
""");
        }
    }
}
