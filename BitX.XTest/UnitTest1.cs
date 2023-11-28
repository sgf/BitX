namespace BitX.XTest;

public class UnitTest1
{
    [Fact]
    public unsafe void Test1()
    {
        int i = 600;
        byte b = (byte)i;
        WinFormsAppAsyncDemo.Bit3_0 bit3_0;
        bit3_0 = 7;
        Assert.True(bit3_0 == 8);
        Assert.True(sizeof(WinFormsAppAsyncDemo.Bit3_0) == 8);
        Assert.True(bit3_0 == 8);

    }
}