// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Package
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

/// <summary>Creates packages of JScript code for use by other code. This class belongs to the abstract syntax tree category.</summary>
public sealed class Package : AST
{
  private string name;
  private ASTList classList;
  private PackageScope scope;

  internal Package(string name, AST id, ASTList classList, Context context)
    : base(context)
  {
    this.name = name;
    this.classList = classList;
    this.scope = (PackageScope) this.Globals.ScopeStack.Peek();
    this.scope.owner = this;
    this.Engine.AddPackage(this.scope);
    switch (id)
    {
      case Lookup lookup:
        lookup.EvaluateAsWrappedNamespace(true);
        break;
      case Member member:
        member.EvaluateAsWrappedNamespace(true);
        break;
    }
  }

  internal override object Evaluate()
  {
    this.Globals.ScopeStack.Push((object) this.scope);
    try
    {
      int i = 0;
      for (int count = this.classList.count; i < count; ++i)
        this.classList[i].Evaluate();
      return (object) new Completion();
    }
    finally
    {
      this.Globals.ScopeStack.Pop();
    }
  }

  /// <summary>Creates the specified package by using the specified scripting engine.</summary>
  /// <param name="rootName">The name of the package.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  public static void JScriptPackage(string rootName, VsaEngine engine)
  {
    GlobalScope globalScope = ((IActivationObject) engine.ScriptObjectStackTop()).GetGlobalScope();
    if (!(globalScope.GetLocalField(rootName) == (FieldInfo) null))
      return;
    FieldInfo fieldInfo = (FieldInfo) globalScope.AddNewField(rootName, (object) Namespace.GetNamespace(rootName, engine), FieldAttributes.Public | FieldAttributes.Literal);
  }

  internal void MergeWith(Package p)
  {
    int i = 0;
    for (int count = p.classList.count; i < count; ++i)
      this.classList.Append(p.classList[i]);
    this.scope.MergeWith(p.scope);
  }

  internal override AST PartiallyEvaluate()
  {
    this.scope.AddOwnName();
    this.Globals.ScopeStack.Push((object) this.scope);
    try
    {
      int i = 0;
      for (int count = this.classList.count; i < count; ++i)
        this.classList[i].PartiallyEvaluate();
      return (AST) this;
    }
    finally
    {
      this.Globals.ScopeStack.Pop();
    }
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    this.Globals.ScopeStack.Push((object) this.scope);
    int i = 0;
    for (int count = this.classList.count; i < count; ++i)
      this.classList[i].TranslateToIL(il, Typeob.Void);
    this.Globals.ScopeStack.Pop();
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    string str = this.name;
    int length = str.IndexOf('.');
    if (length > 0)
      str = str.Substring(0, length);
    il.Emit(OpCodes.Ldstr, str);
    this.EmitILToLoadEngine(il);
    il.Emit(OpCodes.Call, CompilerGlobals.jScriptPackageMethod);
    this.Globals.ScopeStack.Push((object) this.scope);
    int i = 0;
    for (int count = this.classList.count; i < count; ++i)
      this.classList[i].TranslateToILInitializer(il);
    this.Globals.ScopeStack.Pop();
  }

  internal override Context GetFirstExecutableContext() => (Context) null;
}
