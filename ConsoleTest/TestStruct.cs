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
[StructLayout(LayoutKind.Explicit, Pack = 1)]
public struct TestStruct
{
    //public Fixed24<MyStruct> bit23;
    //public Fixed23<MyStruct> bit24;
    [ByteOffset(0)]
    public Bit3_0 BF1;

    [ByteOffset(0)]
    public Bit5_0 BF2;

    [ByteOffset(1)]
    public Bit1_0 BF3;

    [ByteOffset(1)]
    public Bit7_0 BF4;

}

public struct MyStruct
{

}
