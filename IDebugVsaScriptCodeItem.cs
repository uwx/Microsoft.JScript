// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IDebugVsaScriptCodeItem
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods that evaluate expressions and parse strings. This interface is typically implemented by an object that represents a code block, and it is typically called by a debugger. This interface provides extra functionality that is needed by an IActiveScript host.</summary>
[Guid("6DFE759A-CB8B-4ca0-A973-1D04E0BF0B53")]
[ComVisible(true)]
public interface IDebugVsaScriptCodeItem
{
  /// <summary>When implemented in a class, evaluates the block, expression, or statement represented by the code item and returns the value. It is typically called by a debugger that needs to evaluate an expression (in a Watch window, for example). For expressions, the value is the result of the expression. For blocks and statements, the resulting expression is the last expression calculated by the block or statement. </summary>
  /// <returns>Returns a value that represents the evaluation of the statement, expression, or block.</returns>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  [return: MarshalAs(UnmanagedType.Interface)]
  object Evaluate();

  /// <summary>When implemented in a class, parses strings and returns the parsed-out information. This method is typically called by a debugger that needs to parse information about a breakpoint that is represented as a string. The debugger might have a breakpoint typed in by the user in the form functionname-arguments-ILoffset. A named breakpoint string can have a complex format, for example, F.B(C.D[], int, arg : double ) : C.Abc + 123. The parameter list is optional, as is the IL offset.</summary>
  /// <returns>Returns true if the named breakpoint is successfully parsed, false otherwise.</returns>
  /// <param name="input">A well-formed named breakpoint string giving a function, optional argument list, and optional IL offset.</param>
  /// <param name="functionName">Out parameter returning the name of the function parsed out of the input string, or the empty string if the input string is not well formed.</param>
  /// <param name="nargs">Out parameter returning the number of arguments in the optional argument list. Returns zero if there is no arguments list or if the string is not well formed.</param>
  /// <param name="arguments">Out parameter returning a space-separated list of the names of the types of all the arguments in the arguments list, or an empty string if the arguments list was missing or the input string was not well formed.</param>
  /// <param name="returnType">Out parameter returning a string containing the return type of the function named in the input, or the empty string if the return type was missing or the input string was not well formed.</param>
  /// <param name="offset">Unsigned long integer giving the IL offset from the input string, or zero if there was no IL offset or the string was not well formed.</param>
  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  bool ParseNamedBreakPoint(
    string input,
    out string functionName,
    out int nargs,
    out string arguments,
    out string returnType,
    out ulong offset);
}
