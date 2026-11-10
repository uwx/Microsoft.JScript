// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Closure
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a closure function. This class belongs to the objects and functions category.</summary>
public sealed class Closure : ScriptFunction
{
  internal FunctionObject func;
  private ScriptObject enclosing_scope;
  private object declaringObject;
  /// <summary>The arguments to use for the function.</summary>
  public object arguments;
  /// <summary>The calling scope information from the stack frame.</summary>
  public object caller;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.Closure" /> class.</summary>
  /// <param name="func">The function that this closure represents.</param>
  public Closure(FunctionObject func)
    : this(func, (object) null)
  {
    if (!(func.enclosing_scope is StackFrame))
      return;
    this.enclosing_scope = func.enclosing_scope;
  }

  internal Closure(FunctionObject func, object declaringObject)
    : base(func.GetParent(), func.name, func.GetNumberOfFormalParameters())
  {
    this.func = func;
    this.engine = func.engine;
    this.proto = (object) new JSPrototypeObject(((ScriptObject) func.proto).GetParent(), (ScriptFunction) this);
    this.enclosing_scope = this.engine.ScriptObjectStackTop();
    this.arguments = (object) DBNull.Value;
    this.caller = (object) DBNull.Value;
    this.declaringObject = declaringObject;
    this.noExpando = func.noExpando;
    if (!func.isExpandoMethod)
      return;
    StackFrame stackFrame = new StackFrame((ScriptObject) new WithObject(this.enclosing_scope, declaringObject), new JSLocalField[0], new object[0], (object) null);
    this.enclosing_scope = (ScriptObject) stackFrame;
    stackFrame.closureInstance = declaringObject;
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal override object Call(object[] args, object thisob)
  {
    return this.Call(args, thisob, (Binder) JSBinder.ob, (CultureInfo) null);
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal override object Call(object[] args, object thisob, Binder binder, CultureInfo culture)
  {
    if (this.func.isExpandoMethod)
      ((StackFrame) this.enclosing_scope).thisObject = thisob;
    else if (this.declaringObject != null && !(this.declaringObject is ClassScope))
      thisob = this.declaringObject;
    if (thisob == null)
      thisob = ((IActivationObject) this.engine.ScriptObjectStackTop()).GetDefaultThisObject();
    if (this.enclosing_scope is ClassScope && this.declaringObject == null)
    {
      if (thisob is StackFrame)
        thisob = ((StackFrame) thisob).closureInstance;
      if (!this.func.isStatic && !((ClassScope) this.enclosing_scope).HasInstance(thisob))
        throw new JScriptException(JSError.InvalidCall);
    }
    return this.func.Call(args, thisob, this.enclosing_scope, this, binder, culture);
  }

  [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
  internal Delegate ConvertToDelegate(Type delegateType)
  {
    return Delegate.CreateDelegate(delegateType, this.declaringObject, this.func.name);
  }

  /// <summary>Creates a string representation of the function, such as the name of the function or the context.</summary>
  /// <returns>A string representation of the function.</returns>
  public override string ToString() => this.func.ToString();
}
