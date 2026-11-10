// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.StackFrame
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a stack frame for executing code. This class contains methods for resolving scope. It belongs to the compilation and run-time state category.</summary>
public sealed class StackFrame : ScriptObject, IActivationObject
{
  internal ArgumentsObject caller_arguments;
  private JSLocalField[] fields;
  /// <summary>Stores the local variables for this stack frame.</summary>
  public object[] localVars;
  private FunctionScope nestedFunctionScope;
  internal object thisObject;
  /// <summary>Stores a reference to the caller of the current stack frame.</summary>
  public object closureInstance;

  internal StackFrame(
    ScriptObject parent,
    JSLocalField[] fields,
    object[] local_vars,
    object thisObject)
    : base(parent)
  {
    this.caller_arguments = (ArgumentsObject) null;
    this.fields = fields;
    this.localVars = local_vars;
    this.nestedFunctionScope = (FunctionScope) null;
    this.thisObject = thisObject;
    switch (parent)
    {
      case StackFrame _:
        this.closureInstance = ((StackFrame) parent).closureInstance;
        break;
      case JSObject _:
        this.closureInstance = (object) parent;
        break;
      default:
        this.closureInstance = (object) null;
        break;
    }
  }

  internal JSVariableField AddNewField(string name, object value, FieldAttributes attributeFlags)
  {
    this.AllocateFunctionScope();
    return this.nestedFunctionScope.AddNewField(name, value, attributeFlags);
  }

  private void AllocateFunctionScope()
  {
    if (this.nestedFunctionScope != null)
      return;
    this.nestedFunctionScope = new FunctionScope(this.parent);
    if (this.fields == null)
      return;
    int index = 0;
    for (int length = this.fields.Length; index < length; ++index)
      this.nestedFunctionScope.AddOuterScopeField(this.fields[index].Name, this.fields[index]);
  }

  /// <summary>Gets the current default scope, which will be either the contained object of a with statement scope, or the global scope.</summary>
  /// <returns>The current default scope.</returns>
  public object GetDefaultThisObject()
  {
    ScriptObject parent = this.GetParent();
    return parent is IActivationObject activationObject ? activationObject.GetDefaultThisObject() : (object) parent;
  }

  /// <summary>Gets a field that has the specified name by searching the scope hierarchy. This method searches only the scope that is specified by the lexical level.</summary>
  /// <returns>null in all cases.</returns>
  /// <param name="name">The name of the field to search for.</param>
  /// <param name="lexLevel">The lexical level to search.</param>
  public FieldInfo GetField(string name, int lexLevel) => (FieldInfo) null;

  /// <summary>Gets the global scope by searching the scope hierarchy until it finds a scope that includes static variables, for example, a module scope.</summary>
  /// <returns>The global scope.</returns>
  public GlobalScope GetGlobalScope() => ((IActivationObject) this.GetParent()).GetGlobalScope();

  /// <summary>Gets the local declaration of a field that has the specified name. This method is used at compile time for error checking.</summary>
  /// <returns>The local declaration of the field.</returns>
  /// <param name="name">The name of the field.</param>
  FieldInfo IActivationObject.GetLocalField(string name)
  {
    this.AllocateFunctionScope();
    return this.nestedFunctionScope.GetLocalField(name);
  }

  /// <summary>Retrieves an array of <see cref="T:System.Reflection.MemberInfo" /> objects that correspond to all members that match a specified name.</summary>
  /// <returns>An array of objects that match the <paramref name="name" /> argument.</returns>
  /// <param name="name">The member name to search for.</param>
  /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
  public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
  {
    this.AllocateFunctionScope();
    return this.nestedFunctionScope.GetMember(name, bindingAttr);
  }

  /// <summary>Retrieves an array of <see cref="T:System.Reflection.MemberInfo" /> objects that correspond to all members of the current object.</summary>
  /// <returns>An array of objects that contains all members of the current object.</returns>
  /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
  public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
  {
    this.AllocateFunctionScope();
    return this.nestedFunctionScope.GetMembers(bindingAttr);
  }

  internal override void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
  {
    throw new JScriptException(JSError.InternalError);
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal override object GetMemberValue(string name)
  {
    this.AllocateFunctionScope();
    return this.nestedFunctionScope.GetMemberValue(name);
  }

  /// <summary>Gets the value of the specified member by searching the scope hierarchy. This method searches only the scope that is specified by the lexical level.</summary>
  /// <returns>The value of the member <paramref name="name" />.</returns>
  /// <param name="name">The name of the member to search for.</param>
  /// <param name="lexlevel">The lexical level to search.</param>
  [DebuggerStepThrough]
  [DebuggerHidden]
  public object GetMemberValue(string name, int lexlevel)
  {
    if (lexlevel <= 0)
      return (object) Missing.Value;
    return this.nestedFunctionScope != null ? this.nestedFunctionScope.GetMemberValue(name, lexlevel) : ((IActivationObject) this.parent).GetMemberValue(name, lexlevel - 1);
  }

  /// <summary>Creates a new <see cref="T:Microsoft.JScript.StackFrame" /> object for a static method and pushes it onto the global scope stack.</summary>
  /// <param name="thisclass">The class that contains the static method.</param>
  /// <param name="fields">The local variables for the stack frame.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  public static void PushStackFrameForStaticMethod(
    RuntimeTypeHandle thisclass,
    JSLocalField[] fields,
    VsaEngine engine)
  {
    StackFrame.PushStackFrameForMethod((object) Type.GetTypeFromHandle(thisclass), fields, engine);
  }

  /// <summary>Creates a new <see cref="T:Microsoft.JScript.StackFrame" /> object for a method and pushes it onto the global scope stack.</summary>
  /// <param name="thisob">The object that contains the method.</param>
  /// <param name="fields">The local variables for the stack frame.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  public static void PushStackFrameForMethod(
    object thisob,
    JSLocalField[] fields,
    VsaEngine engine)
  {
    Globals globals = engine.Globals;
    IActivationObject activationObject = (IActivationObject) globals.ScopeStack.Peek();
    string name = thisob.GetType().Namespace;
    WithObject parent;
    if (name != null && name.Length > 0)
      parent = new WithObject((ScriptObject) new WithObject((ScriptObject) activationObject.GetGlobalScope(), (object) new WrappedNamespace(name, engine))
      {
        isKnownAtCompileTime = true
      }, thisob);
    else
      parent = new WithObject((ScriptObject) activationObject.GetGlobalScope(), thisob);
    parent.isKnownAtCompileTime = true;
    globals.ScopeStack.GuardedPush((object) new StackFrame((ScriptObject) parent, fields, new object[fields.Length], thisob)
    {
      closureInstance = thisob
    });
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal override void SetMemberValue(string name, object value)
  {
    this.AllocateFunctionScope();
    this.nestedFunctionScope.SetMemberValue(name, value, this);
  }
}
