// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.SourceState
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Specifies the state of source code text. This class is used together with other classes that provide information to color JScript code in an editor.</summary>
[Guid("BA731F6A-3D25-39AE-BE18-C2EF646AE58B")]
[ComVisible(true)]
public enum SourceState
{
  /// <summary>Normal text.</summary>
  STATE_COLOR_NORMAL,
  /// <summary>Comment text.</summary>
  STATE_COLOR_COMMENT,
  /// <summary>String literal text.</summary>
  STATE_COLOR_STRING,
}
