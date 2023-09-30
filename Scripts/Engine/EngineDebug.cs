namespace LD54.Engine;

using System;
using System.Diagnostics;

public static class EngineDebug
{
    public static void PrintLn(string text)
    {
        //Console.Out.WriteLine(text);
        Debug.WriteLine(text);
    }
}
