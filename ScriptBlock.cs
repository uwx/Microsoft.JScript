// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ScriptBlock
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.CodeDom.Compiler;
using System;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a block of JScript code. This class belongs to the abstract syntax tree category.</summary>
public class ScriptBlock : AST
{
  private Block statement_block;
  private JSField[] fields;
  private GlobalScope own_scope;

  internal ScriptBlock(Context context, Block statement_block)
    : base(context)
  {
    this.statement_block = statement_block;
    this.own_scope = (GlobalScope) this.Engine.ScriptObjectStackTop();
    this.fields = (JSField[]) null;
  }

  internal override object Evaluate()
  {
    if (this.fields == null)
      this.fields = this.own_scope.GetFields();
    int index = 0;
    for (int length = this.fields.Length; index < length; ++index)
    {
      FieldInfo field = (FieldInfo) this.fields[index];
      if (!(field is JSExpandoField))
      {
        object func = field.GetValue((object) this.own_scope);
        switch (func)
        {
          case FunctionObject _:
            ((ScriptObject) func).engine = this.Engine;
            this.own_scope.AddFieldOrUseExistingField(field.Name, (object) new Closure((FunctionObject) func), field.Attributes);
            continue;
          case ClassScope _:
            this.own_scope.AddFieldOrUseExistingField(field.Name, func, field.Attributes);
            continue;
          default:
            this.own_scope.AddFieldOrUseExistingField(field.Name, (object) Missing.Value, field.Attributes);
            continue;
        }
      }
    }
    object obj = this.statement_block.Evaluate();
    if (obj is Completion)
      obj = ((Completion) obj).value;
    return obj;
  }

  internal void ProcessAssemblyAttributeLists()
  {
    this.statement_block.ProcessAssemblyAttributeLists();
  }

  internal override AST PartiallyEvaluate()
  {
    this.statement_block.PartiallyEvaluate();
    if (this.Engine.PEFileKind == PEFileKinds.Dll && this.Engine.doSaveAfterCompile)
      this.statement_block.ComplainAboutAnythingOtherThanClassOrPackage();
    this.fields = this.own_scope.GetFields();
    return (AST) this;
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    Expression expression = this.statement_block.ToExpression();
    if (expression != null)
    {
      expression.TranslateToIL(il, rtype);
    }
    else
    {
      this.statement_block.TranslateToIL(il, Typeob.Void);
      new ConstantWrapper((object) null, this.context).TranslateToIL(il, rtype);
    }
  }

  internal TypeBuilder TranslateToILClass(CompilerGlobals compilerGlobals)
  {
    return this.TranslateToILClass(compilerGlobals, true);
  }

  internal TypeBuilder TranslateToILClass(CompilerGlobals compilerGlobals, bool pushScope)
  {
    CompilerGlobals compilerGlobals1 = compilerGlobals;
    ModuleBuilder module = compilerGlobals.module;
    string name = "JScript " + this.Engine.classCounter++.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    Type globalScope = Typeob.GlobalScope;
    TypeBuilder typeBuilder1;
    TypeBuilder typeBuilder2 = typeBuilder1 = module.DefineType(name, TypeAttributes.Public, globalScope, (Type[]) null);
    compilerGlobals1.classwriter = typeBuilder1;
    TypeBuilder ilClass = typeBuilder2;
    compilerGlobals.classwriter.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.compilerGlobalScopeAttributeCtor, new object[0]));
    if ((Type) null == (Type) compilerGlobals.globalScopeClassWriter)
      compilerGlobals.globalScopeClassWriter = ilClass;
    ILGenerator ilGenerator1 = compilerGlobals.classwriter.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[1]
    {
      Typeob.GlobalScope
    }).GetILGenerator();
    ilGenerator1.Emit(OpCodes.Ldarg_0);
    ilGenerator1.Emit(OpCodes.Ldarg_1);
    ilGenerator1.Emit(OpCodes.Dup);
    ilGenerator1.Emit(OpCodes.Ldfld, CompilerGlobals.engineField);
    ilGenerator1.Emit(OpCodes.Call, CompilerGlobals.globalScopeConstructor);
    ilGenerator1.Emit(OpCodes.Ret);
    ILGenerator ilGenerator2 = ilClass.DefineMethod("Global Code", MethodAttributes.Public, Typeob.Object, (Type[]) null).GetILGenerator();
    if (this.Engine.GenerateDebugInfo)
    {
      for (ScriptObject parent = this.own_scope.GetParent(); parent != null; parent = parent.GetParent())
      {
        if (parent is WrappedNamespace && !((WrappedNamespace) parent).name.Equals(""))
          ilGenerator2.UsingNamespace(((WrappedNamespace) parent).name);
      }
    }
    int startLine = this.context.StartLine;
    int startColumn = this.context.StartColumn;
    this.GetFirstExecutableContext()?.EmitFirstLineInfo(ilGenerator2);
    if (pushScope)
    {
      this.EmitILToLoadEngine(ilGenerator2);
      ilGenerator2.Emit(OpCodes.Ldarg_0);
      ilGenerator2.Emit(OpCodes.Call, CompilerGlobals.pushScriptObjectMethod);
    }
    this.TranslateToILInitializer(ilGenerator2);
    this.TranslateToIL(ilGenerator2, Typeob.Object);
    if (pushScope)
    {
      this.EmitILToLoadEngine(ilGenerator2);
      ilGenerator2.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
      ilGenerator2.Emit(OpCodes.Pop);
    }
    ilGenerator2.Emit(OpCodes.Ret);
    return ilClass;
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
    int length = this.fields.Length;
    if (length > 0)
    {
      for (int index = 0; index < length; ++index)
      {
        JSGlobalField field = this.fields[index] as JSGlobalField;
        if (!((FieldInfo) field == (FieldInfo) null))
        {
          Type fieldType = field.FieldType;
          if (field.IsLiteral && fieldType != Typeob.ScriptFunction && fieldType != Typeob.Type || field.metaData != null)
          {
            if ((fieldType.IsPrimitive || fieldType == Typeob.String || fieldType.IsEnum) && field.metaData == null)
              this.compilerGlobals.classwriter.DefineField(field.Name, fieldType, field.Attributes).SetConstant(field.value);
          }
          else if (!(field.value is FunctionObject) || !((FunctionObject) field.value).suppressIL)
          {
            FieldBuilder fieldBuilder = this.compilerGlobals.classwriter.DefineField(field.Name, fieldType, field.Attributes & ~(FieldAttributes.InitOnly | FieldAttributes.Literal) | FieldAttributes.Static);
            field.metaData = (object) fieldBuilder;
            field.WriteCustomAttribute(this.Engine.doCRS);
          }
        }
      }
    }
    this.statement_block.TranslateToILInitializer(il);
  }

  internal override Context GetFirstExecutableContext()
  {
    return this.statement_block.GetFirstExecutableContext();
  }
}
