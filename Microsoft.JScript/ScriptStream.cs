// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ScriptStream
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.IO;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a stream of data. This class sends its data to either a <see cref="T:Microsoft.JScript.COMCharStream" /> or a <see cref="T:System.Console" /> stream. </summary>
public class ScriptStream
{
    /// <summary>Stores a reference to either the standard console <see cref="P:System.Console.Out" /> stream or a custom <see cref="T:Microsoft.JScript.COMCharStream" />.</summary>
    public static TextWriter Out = Console.Out;

    /// <summary>Stores a reference to either the standard console <see cref="P:System.Console.Error" /> stream or a custom <see cref="T:Microsoft.JScript.COMCharStream" />.</summary>
    public static TextWriter Error = Console.Error;

    /// <summary>Writes a string that describes the current contents of the call stack. The string is written to the <see cref="F:Microsoft.JScript.ScriptStream.Out" /> stream.</summary>
    public static void PrintStackTrace()
    {
        try
        {
            throw new Exception();
        }
        catch (Exception ex)
        {
            ScriptStream.PrintStackTrace(ex);
        }
    }

    /// <summary>Writes a string that describes the contents of the call stack at the time the specified exception was thrown. The string is written to the <see cref="F:Microsoft.JScript.ScriptStream.Out" /> stream.</summary>
    /// <param name="e">The exception that was thrown.</param>
    public static void PrintStackTrace(Exception e)
    {
        ScriptStream.Out.WriteLine(e.StackTrace);
        ScriptStream.Out.Flush();
    }

    /// <summary>Writes the specified string to the stream. The string is written to the <see cref="F:Microsoft.JScript.ScriptStream.Out" /> stream.</summary>
    /// <param name="str">The string to write to the stream.</param>
    public static void Write(string str) => ScriptStream.Out.Write(str);

    /// <summary>Writes the specified string to the stream, followed by the line terminator. The string is written to the <see cref="F:Microsoft.JScript.ScriptStream.Out" /> stream.</summary>
    /// <param name="str">The string to write to the stream.</param>
    public static void WriteLine(string str) => ScriptStream.Out.WriteLine(str);
}