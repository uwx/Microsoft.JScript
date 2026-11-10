// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ReplaceWithString
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Text.RegularExpressions;

#nullable disable
namespace Microsoft.JScript;

internal class ReplaceWithString : RegExpReplace
{
  private string replaceString;

  internal ReplaceWithString(string replaceString) => this.replaceString = replaceString;

  internal override string Evaluate(Match match)
  {
    this.lastMatch = match;
    return match.Result(this.replaceString);
  }
}
