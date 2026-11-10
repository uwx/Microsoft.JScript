// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IErrorHandler
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods to handle errors that occur when code is being parsed.</summary>
[Guid("E93D012C-56BB-4f32-864F-7C75EDA17B14")]
[ComVisible(true)]
public interface IErrorHandler
{
  /// <summary>When implemented in a class, determines how to handle an error that occurs when code is being parsed.</summary>
  /// <returns>false to handle the error; true to ignore the error and continue parsing.</returns>
  /// <param name="error">An object that contains information about the error.</param>
  bool OnCompilerError(IVsaFullErrorInfo error);
}
