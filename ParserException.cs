// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ParserException
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;

#nullable disable
namespace Microsoft.JScript;

/// <summary>This class is used by the JScript parser to represent parser exceptions.</summary>
[Serializable]
public class ParserException : Exception
{
  internal ParserException()
    : base(JScriptException.Localize("Parser Exception", CultureInfo.CurrentUICulture))
  {
  }
}
