﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace BitX
{

    internal static class Log
    {
        public static List<string> Logs { get; } = new();

        public static void Print(string msg) => Logs.Add("//\t" + msg);

        // More print methods ...

        public static void FlushLogs(GeneratorExecutionContext context)
        {
            context.AddSource($"logs.g.txt", SourceText.From(string.Join("\n", Logs), Encoding.UTF8));
        }

        public static void FlushLogs(IncrementalGeneratorInitializationContext context)
        {
            context.RegisterPostInitializationOutput(
                ctx => ctx.AddSource("logs.g.txt", SourceText.From(string.Join("\n", Logs), Encoding.UTF8))
            );
        }
    }

    public static class Util
    {
        /// <summary>
        /// Match(name, "Bit\d{1,2}")
        /// </summary>
        /// <param name="name"></param>
        public static int? MatchName(string name)
        {
            int matchDig(char c)
            {
                if ('0' <= c && c <= '9') return c - '0';
                else return -1;
            }

            if (name.Length < 4 || name[0] != 'B' || name[1] != 'i' || name[2] != 't') return null;

            var num = 0;
            for (int i = 3; i < name.Length; i++)
            {
                var d = matchDig(name[i]);
                if (d < 0) return null;
                num = num * 10 + d;
            }
            return num;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct Union
        {
            [FieldOffset(0)]
            public ulong Ulong;
            [FieldOffset(0)]
            public double Double;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int Log2(ulong value)
        {
            var u = default(Union);
            u.Double = value + 0.5;
            return 32 - 1054 + (int)(u.Ulong >> 52);
        }

        public static string GetCapableType(int bits)
        {
            if (bits <= 8) return "byte";
            if (bits <= 16) return "ushort";
            if (bits <= 32) return "uint";
            if (bits <= 64) return "ulong";
            throw new IndexOutOfRangeException();
        }

        public static ulong GetNumber(object obj)
        {
            switch (obj)
            {
                case int x: return (ulong)x;
                case uint x: return x;
                case long x: return (ulong)x;
                case ulong x: return x;
                case short x: return (ulong)x;
                case ushort x: return x;
                case sbyte x: return (ulong)x;
                case byte x: return x;
                default: throw new NotSupportedException();
            }
        }

        public static StringBuilder AppendLineIf(this StringBuilder _this,bool condition,string str)
        {
            return condition ? _this.AppendLine(str) : _this;
        }
    }

}
