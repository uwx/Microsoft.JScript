// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.AssemblyCustomAttributeList
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>This class is used by the JScript parser to store the attributes that are assigned to an assembly.</summary>
public sealed class AssemblyCustomAttributeList : AST
{
  private CustomAttributeList list;
  internal bool okToUse;

  internal AssemblyCustomAttributeList(CustomAttributeList list)
    : base(list.context)
  {
    this.list = list;
    this.okToUse = false;
  }

  internal override object Evaluate() => (object) null;

  internal void Process()
  {
    this.okToUse = true;
    this.list.SetTarget((AST) this);
    this.list.PartiallyEvaluate();
  }

  internal override AST PartiallyEvaluate()
  {
    if (!this.okToUse)
      this.context.HandleError(JSError.AssemblyAttributesMustBeGlobal);
    return (AST) this;
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    foreach (CustomAttributeBuilder attributeBuilder in this.list.GetCustomAttributeBuilders(false))
      this.compilerGlobals.assemblyBuilder.SetCustomAttribute(attributeBuilder);
    if (!(rtype != Typeob.Void))
      return;
    il.Emit(OpCodes.Ldnull);
    if (!rtype.IsValueType)
      return;
    Convert.Emit((AST) this, il, Typeob.Object, rtype);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
  }
}
