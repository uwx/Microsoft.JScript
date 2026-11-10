// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VariableDeclaration
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class VariableDeclaration : AST
{
  internal Lookup identifier;
  private TypeExpression type;
  internal AST initializer;
  internal JSVariableField field;
  private Completion completion;

  internal VariableDeclaration(
    Context context,
    Lookup identifier,
    TypeExpression type,
    AST initializer,
    FieldAttributes attributes,
    CustomAttributeList customAttributes)
    : base(context)
  {
    if (initializer != null)
      this.context.UpdateWith(initializer.context);
    else if (type != null)
      this.context.UpdateWith(type.context);
    this.identifier = identifier;
    this.type = type;
    this.initializer = initializer;
    ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
    while (scriptObject is WithObject)
      scriptObject = scriptObject.GetParent();
    string name = this.identifier.ToString();
    if (scriptObject is ClassScope)
    {
      if (name == ((ClassScope) scriptObject).name)
      {
        identifier.context.HandleError(JSError.CannotUseNameOfClass);
        name += " var";
      }
    }
    else if (attributes != FieldAttributes.PrivateScope)
    {
      this.context.HandleError(JSError.NotInsideClass);
      attributes = FieldAttributes.Public;
    }
    else
      attributes |= FieldAttributes.Public;
    FieldInfo localField = ((IActivationObject) scriptObject).GetLocalField(name);
    if (localField != (FieldInfo) null)
    {
      if (localField.IsLiteral || scriptObject is ClassScope || type != null)
        identifier.context.HandleError(JSError.DuplicateName, true);
      this.type = type = (TypeExpression) null;
    }
    this.field = !(scriptObject is ActivationObject) ? ((StackFrame) scriptObject).AddNewField(this.identifier.ToString(), (object) null, attributes | FieldAttributes.Static) : (localField == (FieldInfo) null || localField is JSVariableField ? ((ActivationObject) scriptObject).AddFieldOrUseExistingField(this.identifier.ToString(), (object) Missing.Value, attributes) : ((ActivationObject) scriptObject).AddNewField(this.identifier.ToString(), (object) null, attributes));
    this.field.type = type;
    this.field.customAttributes = customAttributes;
    this.field.originalContext = context;
    if (this.field is JSLocalField)
      ((JSLocalField) this.field).debugOn = this.identifier.context.document.debugOn;
    this.completion = new Completion();
  }

  internal override object Evaluate()
  {
    ScriptObject scope = this.Globals.ScopeStack.Peek();
    object obj = (object) null;
    if (this.initializer != null)
      obj = this.initializer.Evaluate();
    if (this.type != null)
    {
      obj = Convert.Coerce(obj, (object) this.type);
    }
    else
    {
      while (scope is BlockScope)
        scope = scope.GetParent();
      if (scope is WithObject)
        this.identifier.SetWithValue((WithObject) scope, obj);
      while (scope is WithObject || scope is BlockScope)
        scope = scope.GetParent();
      if (this.initializer == null && !(this.field.value is Missing))
      {
        this.completion.value = this.field.value;
        return (object) this.completion;
      }
    }
    this.field.SetValue((object) scope, this.completion.value = obj);
    return (object) this.completion;
  }

  internal override AST PartiallyEvaluate()
  {
    AST ast = (AST) (this.identifier = (Lookup) this.identifier.PartiallyEvaluateAsReference());
    if (this.type != null)
      this.field.type = this.type = (TypeExpression) this.type.PartiallyEvaluate();
    else if (this.initializer == null && !(this.field is JSLocalField) && this.field.value is Missing)
    {
      ast.context.HandleError(JSError.VariableLeftUninitialized);
      this.field.type = this.type = new TypeExpression((AST) new ConstantWrapper((object) Typeob.Object, ast.context));
    }
    if (this.initializer != null)
    {
      if (this.field.IsStatic)
      {
        ClassScope classScope = (ClassScope) null;
        for (ScriptObject scriptObject = this.Engine.ScriptObjectStackTop(); scriptObject != null; scriptObject = scriptObject.GetParent())
        {
          classScope = scriptObject as ClassScope;
          if (classScope != null)
            break;
        }
        if (classScope != null)
          classScope.inStaticInitializerCode = true;
        this.initializer = this.initializer.PartiallyEvaluate();
        if (classScope != null)
          classScope.inStaticInitializerCode = false;
      }
      else
        this.initializer = this.initializer.PartiallyEvaluate();
      ast.SetPartialValue(this.initializer);
    }
    if ((FieldInfo) this.field != (FieldInfo) null && this.field.customAttributes != null)
      this.field.customAttributes.PartiallyEvaluate();
    return (AST) this;
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    if (this.initializer == null)
      return;
    if (this.context.document.debugOn && this.initializer.context != null)
      this.context.EmitLineInfo(il);
    Lookup identifier = this.identifier;
    identifier.TranslateToILPreSet(il, true);
    identifier.TranslateToILSet(il, true, this.initializer);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    if (this.type != null)
      this.type.TranslateToILInitializer(il);
    if (this.initializer == null)
      return;
    this.initializer.TranslateToILInitializer(il);
  }

  internal override Context GetFirstExecutableContext()
  {
    return this.initializer == null ? (Context) null : this.context;
  }
}
