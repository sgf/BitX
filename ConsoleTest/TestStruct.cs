using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BitX;
using System.IO.Packaging;
using System.Numerics;
using System.Runtime.InteropServices;

namespace WinFormsAppAsyncDemo;


using ByteOffset = System.Runtime.InteropServices.FieldOffsetAttribute;


[BitX]
public struct TestStruct1 {

    public Bit3_0 BF1;
}

[BitX]
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public struct TestStruct
{
    [ByteOffset(0)]
    public Bit3_0 BF1;

    [ByteOffset(0)]
    public Bit5_0 BF2;

    [ByteOffset(1)]
    public Bit1_0 BF3;

    [ByteOffset(1)]
    public Bit7_0 BF4;

    [ByteOffset(1)]
    public Bit7_0 BF5;

    [ByteOffset(1)]
    public Bit7_0 BF6;

    [ByteOffset(1)]
    public Fixed23<MyStruct> fixed23;

    [ByteOffset(1)]
    public Fixed23<MyStruct> fixed24;

    [ByteOffset(1)]
    public Fixed25<MyStruct> fixed25;
}

public struct MyStruct
{

}
