// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.DebugBreak
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>This class is used by the JScript parser to represent a breakpoint in a debugger.</summary>
public class DebugBreak : AST
{
  internal DebugBreak(Context context)
    : base(context)
  {
  }

  internal override object Evaluate()
  {
    Debugger.Break();
    return (object) new Completion();
  }

  internal override AST PartiallyEvaluate() => (AST) this;

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    this.context.EmitLineInfo(il);
    il.Emit(OpCodes.Call, CompilerGlobals.debugBreak);
    if (!this.context.document.debugOn)
      return;
    il.Emit(OpCodes.Nop);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
  }
}
