using System.Runtime.InteropServices;
using WinFormsAppAsyncDemo;

namespace BitX.XTest;

public class UnitTest1
{
    [Fact]
    public unsafe void Test1()
    {
        int i = 600;
        byte b = (byte)i;
        TestStruct ts = default;
        ts.BF1 = 1;
        ts.BF2 = 2;
        ts.BF4 = 8;
        ts.BF5 = 32;
        ts.BF6 = 128;
        Bit1_0 b1;
        Bit3_0 b2 = (Bit3_0)7;
        //Bit5_0 b3;
        //Bit7_0 b4;

        Assert.True(b2 == 7);
        Assert.True(b2 == 8);
        Assert.True(sizeof(WinFormsAppAsyncDemo.Bit3_0) == 8);
        //Assert.True(bit3_0 == 8);

    }
}
