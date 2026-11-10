// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.RegExpLiteral
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class RegExpLiteral : AST
{
  private string source;
  private bool ignoreCase;
  private bool global;
  private bool multiline;
  private JSGlobalField regExpVar;
  private static int counter;

  internal RegExpLiteral(string source, string flags, Context context)
    : base(context)
  {
    this.source = source;
    this.ignoreCase = this.global = this.multiline = false;
    if (flags == null)
      return;
    for (int index = 0; index < flags.Length; ++index)
    {
      switch (flags[index])
      {
        case 'g':
          this.global = !this.global ? true : throw new JScriptException(JSError.RegExpSyntax);
          break;
        case 'i':
          this.ignoreCase = !this.ignoreCase ? true : throw new JScriptException(JSError.RegExpSyntax);
          break;
        case 'm':
          this.multiline = !this.multiline ? true : throw new JScriptException(JSError.RegExpSyntax);
          break;
        default:
          throw new JScriptException(JSError.RegExpSyntax);
      }
    }
  }

  internal override object Evaluate()
  {
    if (VsaEngine.executeForJSEE)
      throw new JScriptException(JSError.NonSupportedInDebugger);
    RegExpObject regExpObject = (RegExpObject) this.Globals.RegExpTable[(object) this];
    if (regExpObject == null)
    {
      regExpObject = (RegExpObject) this.Engine.GetOriginalRegExpConstructor().Construct(this.source, this.ignoreCase, this.global, this.multiline);
      this.Globals.RegExpTable[(object) this] = (object) regExpObject;
    }
    return (object) regExpObject;
  }

  internal override IReflect InferType(JSField inferenceTarget) => (IReflect) Typeob.RegExpObject;

  internal override AST PartiallyEvaluate()
  {
    string name = "regexp " + RegExpLiteral.counter++.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    JSGlobalField jsGlobalField = (JSGlobalField) ((ActivationObject) this.Engine.GetGlobalScope().GetObject()).AddNewField(name, (object) null, FieldAttributes.Assembly);
    jsGlobalField.type = new TypeExpression((AST) new ConstantWrapper((object) Typeob.RegExpObject, this.context));
    this.regExpVar = jsGlobalField;
    return (AST) this;
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    il.Emit(OpCodes.Ldsfld, (FieldInfo) this.regExpVar.GetMetaData());
    Convert.Emit((AST) this, il, Typeob.RegExpObject, rtype);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    ScriptObject scriptObject = this.Engine.ScriptObjectStackTop();
    while (scriptObject != null && (scriptObject is WithObject || scriptObject is BlockScope))
      scriptObject = scriptObject.GetParent();
    if (scriptObject is FunctionScope)
    {
      this.EmitILToLoadEngine(il);
      il.Emit(OpCodes.Pop);
    }
    il.Emit(OpCodes.Ldsfld, (FieldInfo) this.regExpVar.GetMetaData());
    Label label = il.DefineLabel();
    il.Emit(OpCodes.Brtrue, label);
    this.EmitILToLoadEngine(il);
    il.Emit(OpCodes.Call, CompilerGlobals.getOriginalRegExpConstructorMethod);
    il.Emit(OpCodes.Ldstr, this.source);
    if (this.ignoreCase)
      il.Emit(OpCodes.Ldc_I4_1);
    else
      il.Emit(OpCodes.Ldc_I4_0);
    if (this.global)
      il.Emit(OpCodes.Ldc_I4_1);
    else
      il.Emit(OpCodes.Ldc_I4_0);
    if (this.multiline)
      il.Emit(OpCodes.Ldc_I4_1);
    else
      il.Emit(OpCodes.Ldc_I4_0);
    il.Emit(OpCodes.Call, CompilerGlobals.regExpConstructMethod);
    il.Emit(OpCodes.Castclass, Typeob.RegExpObject);
    il.Emit(OpCodes.Stsfld, (FieldInfo) this.regExpVar.GetMetaData());
    il.MarkLabel(label);
  }
}
