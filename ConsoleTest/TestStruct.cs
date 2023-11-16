using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsAppAsyncDemo
{

    using System.IO.Packaging;
    using System.Numerics;
    using System.Runtime.InteropServices;


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TestStruct
    {
        public Fixed24<MyStruct> bit23;
        public Fixed23<MyStruct> bit24;
        public Bit3_0 BF1;
        public Bit3_0 BF2;
    }

    public struct MyStruct
    {

    }



}
