// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ErrorConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. This class belongs to the built-in object model category.</summary>
public sealed class ErrorConstructor : ScriptFunction
{
  internal static readonly ErrorConstructor ob = new ErrorConstructor();
  internal static readonly ErrorConstructor evalOb = new ErrorConstructor("EvalError", ErrorType.EvalError);
  internal static readonly ErrorConstructor rangeOb = new ErrorConstructor("RangeError", ErrorType.RangeError);
  internal static readonly ErrorConstructor referenceOb = new ErrorConstructor("ReferenceError", ErrorType.ReferenceError);
  internal static readonly ErrorConstructor syntaxOb = new ErrorConstructor("SyntaxError", ErrorType.SyntaxError);
  internal static readonly ErrorConstructor typeOb = new ErrorConstructor("TypeError", ErrorType.TypeError);
  internal static readonly ErrorConstructor uriOb = new ErrorConstructor("URIError", ErrorType.URIError);
  private ErrorPrototype originalPrototype;
  private ErrorType type;
  private GlobalObject globalObject;

  internal ErrorConstructor()
    : base((ScriptObject) ErrorPrototype.ob, "Error", 2)
  {
    this.originalPrototype = ErrorPrototype.ob;
    ErrorPrototype.ob._constructor = this;
    this.proto = (object) ErrorPrototype.ob;
    this.type = ErrorType.OtherError;
    this.globalObject = GlobalObject.commonInstance;
  }

  internal ErrorConstructor(
    LenientFunctionPrototype parent,
    LenientErrorPrototype prototypeProp,
    GlobalObject globalObject)
    : base((ScriptObject) parent, "Error", 2)
  {
    this.originalPrototype = (ErrorPrototype) prototypeProp;
    prototypeProp.constructor = (object) this;
    this.proto = (object) prototypeProp;
    this.type = ErrorType.OtherError;
    this.globalObject = globalObject;
    this.noExpando = false;
  }

  internal ErrorConstructor(string subtypeName, ErrorType type)
    : base(ErrorConstructor.ob.parent, subtypeName, 2)
  {
    this.originalPrototype = new ErrorPrototype((ScriptObject) ErrorConstructor.ob.originalPrototype, subtypeName);
    this.originalPrototype._constructor = this;
    this.proto = (object) this.originalPrototype;
    this.type = type;
    this.globalObject = GlobalObject.commonInstance;
  }

  internal ErrorConstructor(
    string subtypeName,
    ErrorType type,
    ErrorConstructor error,
    GlobalObject globalObject)
    : base(error.parent, subtypeName, 2)
  {
    this.originalPrototype = (ErrorPrototype) new LenientErrorPrototype((LenientFunctionPrototype) error.parent, (ScriptObject) error.originalPrototype, subtypeName);
    this.noExpando = false;
    this.originalPrototype._constructor = this;
    this.proto = (object) this.originalPrototype;
    this.type = type;
    this.globalObject = globalObject;
    this.noExpando = false;
  }

  internal override object Call(object[] args, object thisob)
  {
    return (object) this.Construct((object) args);
  }

  internal override object Construct(object[] args) => (object) this.CreateInstance(args);

  internal ErrorObject Construct(object e)
  {
    if (!(e is JScriptException) || this != this.globalObject.originalError)
    {
      switch (this.type)
      {
        case ErrorType.EvalError:
          return (ErrorObject) new EvalErrorObject(this.originalPrototype, e);
        case ErrorType.RangeError:
          return (ErrorObject) new RangeErrorObject(this.originalPrototype, e);
        case ErrorType.ReferenceError:
          return (ErrorObject) new ReferenceErrorObject(this.originalPrototype, e);
        case ErrorType.SyntaxError:
          return (ErrorObject) new SyntaxErrorObject(this.originalPrototype, e);
        case ErrorType.TypeError:
          return (ErrorObject) new TypeErrorObject(this.originalPrototype, e);
        case ErrorType.URIError:
          return (ErrorObject) new URIErrorObject(this.originalPrototype, e);
        default:
          return new ErrorObject(this.originalPrototype, e);
      }
    }
    else
    {
      switch (((JScriptException) e).GetErrorType())
      {
        case ErrorType.EvalError:
          return this.globalObject.originalEvalError.Construct(e);
        case ErrorType.RangeError:
          return this.globalObject.originalRangeError.Construct(e);
        case ErrorType.ReferenceError:
          return this.globalObject.originalReferenceError.Construct(e);
        case ErrorType.SyntaxError:
          return this.globalObject.originalSyntaxError.Construct(e);
        case ErrorType.TypeError:
          return this.globalObject.originalTypeError.Construct(e);
        case ErrorType.URIError:
          return this.globalObject.originalURIError.Construct(e);
        default:
          return new ErrorObject(this.originalPrototype, e);
      }
    }
  }

  /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
  /// <returns>The instance of the object.</returns>
  /// <param name="args">The arguments to pass to the constructor.</param>
  [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
  public ErrorObject CreateInstance(params object[] args)
  {
    switch (this.type)
    {
      case ErrorType.EvalError:
        return (ErrorObject) new EvalErrorObject(this.originalPrototype, args);
      case ErrorType.RangeError:
        return (ErrorObject) new RangeErrorObject(this.originalPrototype, args);
      case ErrorType.ReferenceError:
        return (ErrorObject) new ReferenceErrorObject(this.originalPrototype, args);
      case ErrorType.SyntaxError:
        return (ErrorObject) new SyntaxErrorObject(this.originalPrototype, args);
      case ErrorType.TypeError:
        return (ErrorObject) new TypeErrorObject(this.originalPrototype, args);
      case ErrorType.URIError:
        return (ErrorObject) new URIErrorObject(this.originalPrototype, args);
      default:
        return new ErrorObject(this.originalPrototype, args);
    }
  }

  /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
  /// <returns>The instance of the object.</returns>
  /// <param name="args">The arguments to pass to the constructor.</param>
  [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
  public object Invoke(params object[] args) => (object) this.CreateInstance(args);
}
