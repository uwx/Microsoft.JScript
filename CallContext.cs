// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.CallContext
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

internal class CallContext
{
  internal readonly Context sourceContext;
  private readonly LateBinding callee;
  private readonly object[] actual_parameters;

  internal CallContext(Context sourceContext, LateBinding callee, object[] actual_parameters)
  {
    this.sourceContext = sourceContext;
    this.callee = callee;
    this.actual_parameters = actual_parameters;
  }

  internal string FunctionName() => this.callee == null ? "eval" : this.callee.ToString();
}
