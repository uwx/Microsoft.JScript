// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Import
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Imports namespaces for use in code. This class belongs to the abstract syntax tree category.</summary>
public sealed class Import : AST
{
  private string name;

  internal Import(Context context, AST name)
    : base(context)
  {
    if (name == null)
      return;
    WrappedNamespace wrappedNamespace = name.EvaluateAsWrappedNamespace(true);
    this.Engine.SetEnclosingContext((ScriptObject) wrappedNamespace);
    this.name = wrappedNamespace.name;
  }

  internal override object Evaluate() => (object) new Completion();

  internal override AST PartiallyEvaluate() => (AST) this;

  /// <summary>Imports the specified namespace for use by the specified scripting engine.</summary>
  /// <param name="name">The namespace to import.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  public static void JScriptImport(string name, VsaEngine engine)
  {
    int length = name.IndexOf('.');
    string name1 = length > 0 ? name.Substring(0, length) : name;
    GlobalScope globalScope = ((IActivationObject) engine.ScriptObjectStackTop()).GetGlobalScope();
    if (globalScope.GetLocalField(name1) == (FieldInfo) null)
    {
      FieldInfo fieldInfo = (FieldInfo) globalScope.AddNewField(name1, (object) Namespace.GetNamespace(name1, engine), FieldAttributes.Public | FieldAttributes.Literal);
    }
    engine.SetEnclosingContext((ScriptObject) new WrappedNamespace(name, engine, false));
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    il.Emit(OpCodes.Ldstr, this.name);
    this.EmitILToLoadEngine(il);
    il.Emit(OpCodes.Call, CompilerGlobals.jScriptImportMethod);
  }
}
