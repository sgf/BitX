// See https://aka.ms/new-console-template for more information
using BitX;
using WinFormsAppAsyncDemo;


TestStruct testStruct = new TestStruct();
testStruct.BF1 = 1;
testStruct.BF2 = 2;
testStruct.BF3 = 1;
testStruct.BF4 = 4;

Console.WriteLine($"Hello, World! {testStruct.BF4}");

