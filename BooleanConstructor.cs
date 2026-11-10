// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.BooleanConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. This class belongs to the built-in object model category.</summary>
public sealed class BooleanConstructor : ScriptFunction
{
  internal static readonly BooleanConstructor ob = new BooleanConstructor();
  private BooleanPrototype originalPrototype;

  internal BooleanConstructor()
    : base((ScriptObject) FunctionPrototype.ob, "Boolean", 1)
  {
    this.originalPrototype = BooleanPrototype.ob;
    BooleanPrototype._constructor = this;
    this.proto = (object) BooleanPrototype.ob;
  }

  internal BooleanConstructor(
    LenientFunctionPrototype parent,
    LenientBooleanPrototype prototypeProp)
    : base((ScriptObject) parent, "Boolean", 1)
  {
    this.originalPrototype = (BooleanPrototype) prototypeProp;
    prototypeProp.constructor = (object) this;
    this.proto = (object) prototypeProp;
    this.noExpando = false;
  }

  internal override object Call(object[] args, object thisob)
  {
    return args.Length == 0 ? (object) false : (object) Convert.ToBoolean(args[0]);
  }

  internal BooleanObject Construct()
  {
    return new BooleanObject((ScriptObject) this.originalPrototype, false, false);
  }

  internal override object Construct(object[] args) => (object) this.CreateInstance(args);

  internal BooleanObject ConstructImplicitWrapper(bool arg)
  {
    return new BooleanObject((ScriptObject) this.originalPrototype, arg, true);
  }

  internal BooleanObject ConstructWrapper(bool arg)
  {
    return new BooleanObject((ScriptObject) this.originalPrototype, arg, false);
  }

  /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
  /// <returns>The instance of the object.</returns>
  /// <param name="args">The arguments to pass to the constructor.</param>
  [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
  public BooleanObject CreateInstance(params object[] args)
  {
    return new BooleanObject((ScriptObject) this.originalPrototype, args.Length != 0 && Convert.ToBoolean(args[0]), false);
  }

  /// <summary>Invokes a method of a prototype-based object, passing in the specified arguments.</summary>
  /// <returns>The result of the call to the method.</returns>
  /// <param name="arg">The arguments to pass to the method.</param>
  public bool Invoke(object arg) => Convert.ToBoolean(arg);
}
