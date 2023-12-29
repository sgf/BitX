using BitX;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using BaseType = byte;

namespace WinFormsAppAsyncDemo;

[BitX]
public struct TestStruct1
{

    //public Bit3_0 BF1;

    public Bit5_0 BF2;

    public Fixed23<MyStruct> fixed24;
}

[BitX]
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public struct TestStruct
{
    [FieldOffset(0)]
    public Bit1_0 BF0;

    [FieldOffset(0)]
    public Bit1_1 BF1;

    [FieldOffset(0)]
    public Bit2_2 BF2;

    [FieldOffset(0)]
    public Bit1_4 BF3;

    [FieldOffset(0)]
    public Bit4_5 BF4;

    [FieldOffset(1)]
    public Bit5_0 BF5;

    [FieldOffset(1)]
    public Bit7_0 BF6;

    [FieldOffset(1)]
    public Fixed23<MyStruct> fixed23;

    [FieldOffset(1)]
    public Fixed23<MyStruct> fixed24;

    [FieldOffset(1)]
    public Fixed25<MyStruct> fixed25;
}

public struct MyStruct
{

}



//public struct Bit3_0 : IEquatable<Bit3_0>
//{
//    public const int Size = 3;
//    public const int Offset = 0;
//    public const int Mask = 0b00000111;
//    public const BaseType Max = 7;// Mask >> Offset;
//    public const BaseType Min = 0;

//    public BaseType _Value;
//    public BaseType Value { get => (BaseType)((_Value & Mask) >> Offset); set => _Value |= (BaseType)(value << Offset & Mask); }

//    public Bit3_0(BaseType x) => Value = x;
//    public bool Equals(Bit3_0 other) => Value == other.Value;

//    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Bit3_0 other && Equals(other);

//    public override int GetHashCode() => Value.GetHashCode();
//    public static bool operator ==(Bit3_0 x, Bit3_0 y) => x.Value == y.Value;
//    public static bool operator !=(Bit3_0 x, Bit3_0 y) => x.Value != y.Value;

//    public override string ToString() => Value.ToString();

//    public static explicit operator Bit3_0(byte x) => new Bit3_0(x);
//    public static explicit operator Bit3_0(short x) => (Bit3_0)(BaseType)x;
//    public static explicit operator Bit3_0(ushort x) => (Bit3_0)(BaseType)x;
//    public static explicit operator Bit3_0(int x) => (Bit3_0)(BaseType)x;
//    public static explicit operator Bit3_0(uint x) => (Bit3_0)(BaseType)x;
//    public static explicit operator Bit3_0(long x) => (Bit3_0)(BaseType)x;
//    public static explicit operator Bit3_0(ulong x) => (Bit3_0)(BaseType)x;


//    public static implicit operator Bit3_0 byte( x) => x.Value;

//    public static implicit operator byte(Bit3_0 x) => x.Value;
//    public static implicit operator short(Bit3_0 x) => x.Value;
//    public static implicit operator ushort(Bit3_0 x) => x.Value;
//    public static implicit operator int(Bit3_0 x) => x.Value;
//    public static implicit operator uint(Bit3_0 x) => x.Value;
//    public static implicit operator long(Bit3_0 x) => x.Value;
//    public static implicit operator ulong(Bit3_0 x) => x.Value;


//    public bool this[int index]
//    {
//        get
//        {
//            if (index >= Size) throw new IndexOutOfRangeException();
//            return (_Value & (1 << (index & Mask))) != 0;
//        }
//        set
//        {
//            if (index >= Size) throw new IndexOutOfRangeException();
//            _Value |= (BaseType)(1 << index);
//        }
//    }
//    //https://stackoverflow.com/a/24250656/2024178
//}



//[InlineArray(29)]
//public struct Fixed29<T>
//{
//    private T _element0;

//    public const int Length = 23;

//    public Span<T> GetSpan() => MemoryMarshal.CreateSpan(ref Unsafe.As<Fixed23<T>, T>(ref this), 23);

//    T this[int index] { get => this[index]; set => this[index] = value; }
//}
