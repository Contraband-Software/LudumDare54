namespace LD54.Engine.Dev;

using System;
using System.Diagnostics;

public static class EngineDebug
{
    public static void PrintLn(string text)
    {
        #if SAM
        Console.Out.WriteLine(text);
        #else
        Debug.WriteLine(text);
        #endif
    }
}
