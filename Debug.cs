// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Debug
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;

#nullable disable
namespace Microsoft.JScript;

internal static class Debug
{
  [Conditional("ASSERTION")]
  public static void Assert(bool condition)
  {
    if (!condition)
      throw new AssertException("Assertion fired");
  }

  [Conditional("ASSERTION")]
  public static void Assert(bool condition, string message)
  {
    if (!condition)
      throw new AssertException(message);
  }

  [Conditional("ASSERTION")]
  public static void NotImplemented(string message)
  {
    throw new AssertException("Method Not Yet Implemented");
  }

  [Conditional("ASSERTION")]
  public static void PostCondition(bool condition)
  {
    if (!condition)
      throw new PostConditionException("PostCondition missed");
  }

  [Conditional("ASSERTION")]
  public static void PostCondition(bool condition, string message)
  {
    if (!condition)
      throw new PostConditionException(message);
  }

  [Conditional("ASSERTION")]
  public static void PreCondition(bool condition)
  {
    if (!condition)
      throw new PreConditionException("PreCondition missed");
  }

  [Conditional("ASSERTION")]
  public static void PreCondition(bool condition, string message)
  {
    if (!condition)
      throw new PreConditionException(message);
  }

  [Conditional("LOGGING")]
  public static void Print(string str) => ScriptStream.Out.WriteLine(str);

  [Conditional("LOGGING")]
  internal static void PrintLine(string message) => ScriptStream.Out.WriteLine(message);

  [Conditional("LOGGING")]
  public static void PrintStack() => ScriptStream.PrintStackTrace();

  [Conditional("LOGGING")]
  public static void PrintStack(Exception e) => ScriptStream.PrintStackTrace(e);
}
