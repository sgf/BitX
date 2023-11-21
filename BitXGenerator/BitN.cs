using System;
using System.Collections.Generic;
using System.Text;

namespace BitX;

//    if (i == 9) baseType = "ushort";
//else if (i == 17) baseType = "uint";
//else if (i == 33) baseType = "ulong";

public struct Bit1 : IEquatable<Bit1>
{
    public byte Value;
    public Bit1 (byte x) => Value = x;

    public bool Equals(Bit1 other) => Value == other.Value;
    public override bool Equals(object obj) => obj is Bit1 other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public static bool operator ==(Bit1 x, Bit1 y) => x.Value == y.Value;
    public static bool operator !=(Bit1 x, Bit1 y) => x.Value != y.Value;

    public static implicit operator Bit1(byte x) => new Bit1(x);
    public static implicit operator byte(Bit1 x) => x.Value;

    public override string ToString() => Value.ToString();
}


public struct Bit2 : IEquatable<Bit2>
{
    public byte Value;
    public Bit2(byte x) => Value = x;

    public bool Equals(Bit2 other) => Value == other.Value;
    public override bool Equals(object obj) => obj is Bit2 other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public static bool operator ==(Bit2 x, Bit2 y) => x.Value == y.Value;
    public static bool operator !=(Bit2 x, Bit2 y) => x.Value != y.Value;

    public static implicit operator Bit2(byte x) => new Bit2(x);
    public static implicit operator byte(Bit2 x) => x.Value;

    public override string ToString() => Value.ToString();
}

public struct Bit3 : IEquatable<Bit3>
{
    public byte Value;
    public Bit3(byte x) => Value = x;

    public bool Equals(Bit3 other) => Value == other.Value;
    public override bool Equals(object obj) => obj is Bit3 other && Equals(other);
    public override int GetHashCode() => Value.GetHashCode();
    public static bool operator ==(Bit3 x, Bit3 y) => x.Value == y.Value;
    public static bool operator !=(Bit3 x, Bit3 y) => x.Value != y.Value;

    public static implicit operator Bit3(byte x) => new Bit3(x);
    public static implicit operator byte(Bit3 x) => x.Value;

    public override string ToString() => Value.ToString();
}
