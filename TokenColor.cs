// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.TokenColor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Specifies the colors that individual units of code should have when text is being colorized in an editor. The members of this enumeration are used by the <see cref="P:Microsoft.JScript.ITokenColorInfo.Color" /> property of <see cref="T:Microsoft.JScript.ITokenColorInfo" /> objects.</summary>
[Guid("78008402-40D4-30BF-BD91-A61D8815625E")]
[ComVisible(true)]
public enum TokenColor
{
  /// <summary>The color for a unit of code that represents text. This is the default value.</summary>
  COLOR_TEXT,
  /// <summary>The color for a unit of code that represents an identifier.</summary>
  COLOR_IDENTIFIER,
  /// <summary>The color for a unit of code that represents a keyword.</summary>
  COLOR_KEYWORD,
  /// <summary>The color for a unit of code that represents a comment.</summary>
  COLOR_COMMENT,
  /// <summary>The color for a unit of code that represents an operator.</summary>
  COLOR_OPERATOR,
  /// <summary>The color for a unit of code that represents a numeric literal.</summary>
  COLOR_NUMBER,
  /// <summary>The color for a unit of code that represents a string literal.</summary>
  COLOR_STRING,
  /// <summary>Not used.</summary>
  COLOR_CONDITIONAL_COMP,
}
