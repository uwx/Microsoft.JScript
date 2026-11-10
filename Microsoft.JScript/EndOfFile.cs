// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.EndOfFile
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>This class is used by the JScript parser to represent reaching the end of a file as an exception. Code can throw this error to stop the parser.</summary>
[Serializable]
public class EndOfFile : ParserException
{
    internal EndOfFile()
    {
    }
}