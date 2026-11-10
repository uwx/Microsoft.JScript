// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.TypeReferences
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class TypeReferences
{
  private static readonly Microsoft.JScript.SimpleHashtable _predefinedTypeTable = new Microsoft.JScript.SimpleHashtable(34U);
  private Type[] _typeTable;
  private Module _jscriptReferenceModule;
  private const int TypeReferenceStartOfSpecialCases = 81;
  private const int TypeReferenceArrayLength = 83;

  internal TypeReferences(Module jscriptReferenceModule)
  {
    this._jscriptReferenceModule = jscriptReferenceModule;
    this._typeTable = new Type[83];
  }

  internal Type GetPredefinedType(string typeName)
  {
    object obj = TypeReferences._predefinedTypeTable[(object) typeName];
    Type predefinedType = obj as Type;
    if (predefinedType == (Type) null && obj is TypeReferences.TypeReference typeRef)
      predefinedType = this.GetTypeReference(typeRef);
    return predefinedType;
  }

  static TypeReferences()
  {
    TypeReferences._predefinedTypeTable[(object) "boolean"] = (object) typeof (bool);
    TypeReferences._predefinedTypeTable[(object) "byte"] = (object) typeof (byte);
    TypeReferences._predefinedTypeTable[(object) "char"] = (object) typeof (char);
    TypeReferences._predefinedTypeTable[(object) "decimal"] = (object) typeof (System.Decimal);
    TypeReferences._predefinedTypeTable[(object) "double"] = (object) typeof (double);
    TypeReferences._predefinedTypeTable[(object) "float"] = (object) typeof (float);
    TypeReferences._predefinedTypeTable[(object) "int"] = (object) typeof (int);
    TypeReferences._predefinedTypeTable[(object) "long"] = (object) typeof (long);
    TypeReferences._predefinedTypeTable[(object) "sbyte"] = (object) typeof (sbyte);
    TypeReferences._predefinedTypeTable[(object) "short"] = (object) typeof (short);
    TypeReferences._predefinedTypeTable[(object) "void"] = (object) typeof (void);
    TypeReferences._predefinedTypeTable[(object) "uint"] = (object) typeof (uint);
    TypeReferences._predefinedTypeTable[(object) "ulong"] = (object) typeof (ulong);
    TypeReferences._predefinedTypeTable[(object) "ushort"] = (object) typeof (ushort);
    TypeReferences._predefinedTypeTable[(object) "ActiveXObject"] = (object) typeof (object);
    TypeReferences._predefinedTypeTable[(object) nameof (Boolean)] = (object) typeof (bool);
    TypeReferences._predefinedTypeTable[(object) "Number"] = (object) typeof (double);
    TypeReferences._predefinedTypeTable[(object) nameof (Object)] = (object) typeof (object);
    TypeReferences._predefinedTypeTable[(object) nameof (String)] = (object) typeof (string);
    TypeReferences._predefinedTypeTable[(object) nameof (Type)] = (object) typeof (Type);
    TypeReferences._predefinedTypeTable[(object) nameof (Array)] = (object) TypeReferences.TypeReference.ArrayObject;
    TypeReferences._predefinedTypeTable[(object) "Date"] = (object) TypeReferences.TypeReference.DateObject;
    TypeReferences._predefinedTypeTable[(object) "Enumerator"] = (object) TypeReferences.TypeReference.EnumeratorObject;
    TypeReferences._predefinedTypeTable[(object) "Error"] = (object) TypeReferences.TypeReference.ErrorObject;
    TypeReferences._predefinedTypeTable[(object) "EvalError"] = (object) TypeReferences.TypeReference.EvalErrorObject;
    TypeReferences._predefinedTypeTable[(object) "Function"] = (object) TypeReferences.TypeReference.ScriptFunction;
    TypeReferences._predefinedTypeTable[(object) "RangeError"] = (object) TypeReferences.TypeReference.RangeErrorObject;
    TypeReferences._predefinedTypeTable[(object) "ReferenceError"] = (object) TypeReferences.TypeReference.ReferenceErrorObject;
    TypeReferences._predefinedTypeTable[(object) "RegExp"] = (object) TypeReferences.TypeReference.RegExpObject;
    TypeReferences._predefinedTypeTable[(object) "SyntaxError"] = (object) TypeReferences.TypeReference.SyntaxErrorObject;
    TypeReferences._predefinedTypeTable[(object) "TypeError"] = (object) TypeReferences.TypeReference.TypeErrorObject;
    TypeReferences._predefinedTypeTable[(object) "URIError"] = (object) TypeReferences.TypeReference.URIErrorObject;
    TypeReferences._predefinedTypeTable[(object) "VBArray"] = (object) TypeReferences.TypeReference.VBArrayObject;
  }

  private Module JScriptReferenceModule => this._jscriptReferenceModule;

  private Type GetTypeReference(TypeReferences.TypeReference typeRef)
  {
    Type type = this._typeTable[(int) typeRef];
    if ((Type) null == type)
    {
      string str = "Microsoft.JScript.";
      switch (typeRef)
      {
        case TypeReferences.TypeReference.BaseVsaStartup:
          str = "Microsoft.JScript.Vsa.";
          break;
        case TypeReferences.TypeReference.VsaEngine:
          str = "Microsoft.JScript.Vsa.";
          break;
      }
      type = this.JScriptReferenceModule.GetType(str + System.Enum.GetName(typeof (TypeReferences.TypeReference), (object) (int) typeRef));
      this._typeTable[(int) typeRef] = type;
    }
    return type;
  }

  internal Type ArgumentsObject
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.ArgumentsObject);
  }

  internal Type ArrayConstructor
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.ArrayConstructor);
  }

  internal Type ArrayObject => this.GetTypeReference(TypeReferences.TypeReference.ArrayObject);

  internal Type ArrayWrapper => this.GetTypeReference(TypeReferences.TypeReference.ArrayWrapper);

  internal Type BaseVsaStartup
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.BaseVsaStartup);
  }

  internal Type Binding => this.GetTypeReference(TypeReferences.TypeReference.Binding);

  internal Type BitwiseBinary => this.GetTypeReference(TypeReferences.TypeReference.BitwiseBinary);

  internal Type BooleanObject => this.GetTypeReference(TypeReferences.TypeReference.BooleanObject);

  internal Type BreakOutOfFinally
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.BreakOutOfFinally);
  }

  internal Type BuiltinFunction
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.BuiltinFunction);
  }

  internal Type ClassScope => this.GetTypeReference(TypeReferences.TypeReference.ClassScope);

  internal Type Closure => this.GetTypeReference(TypeReferences.TypeReference.Closure);

  internal Type ContinueOutOfFinally
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.ContinueOutOfFinally);
  }

  internal Type Convert => this.GetTypeReference(TypeReferences.TypeReference.Convert);

  internal Type DateObject => this.GetTypeReference(TypeReferences.TypeReference.DateObject);

  internal Type Empty => this.GetTypeReference(TypeReferences.TypeReference.Empty);

  internal Type EnumeratorObject
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.EnumeratorObject);
  }

  internal Type Equality => this.GetTypeReference(TypeReferences.TypeReference.Equality);

  internal Type ErrorObject => this.GetTypeReference(TypeReferences.TypeReference.ErrorObject);

  internal Type Eval => this.GetTypeReference(TypeReferences.TypeReference.Eval);

  internal Type EvalErrorObject
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.EvalErrorObject);
  }

  internal Type Expando => this.GetTypeReference(TypeReferences.TypeReference.Expando);

  internal Type FieldAccessor => this.GetTypeReference(TypeReferences.TypeReference.FieldAccessor);

  internal Type ForIn => this.GetTypeReference(TypeReferences.TypeReference.ForIn);

  internal Type FunctionDeclaration
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.FunctionDeclaration);
  }

  internal Type FunctionExpression
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.FunctionExpression);
  }

  internal Type FunctionObject
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.FunctionObject);
  }

  internal Type FunctionWrapper
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.FunctionWrapper);
  }

  internal Type GlobalObject => this.GetTypeReference(TypeReferences.TypeReference.GlobalObject);

  internal Type GlobalScope => this.GetTypeReference(TypeReferences.TypeReference.GlobalScope);

  internal Type Globals => this.GetTypeReference(TypeReferences.TypeReference.Globals);

  internal Type Hide => this.GetTypeReference(TypeReferences.TypeReference.Hide);

  internal Type IActivationObject
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.IActivationObject);
  }

  internal Type INeedEngine => this.GetTypeReference(TypeReferences.TypeReference.INeedEngine);

  internal Type Import => this.GetTypeReference(TypeReferences.TypeReference.Import);

  internal Type In => this.GetTypeReference(TypeReferences.TypeReference.In);

  internal Type Instanceof => this.GetTypeReference(TypeReferences.TypeReference.Instanceof);

  internal Type JSError => this.GetTypeReference(TypeReferences.TypeReference.JSError);

  internal Type JSFunctionAttribute
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.JSFunctionAttribute);
  }

  internal Type JSFunctionAttributeEnum
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.JSFunctionAttributeEnum);
  }

  internal Type JSLocalField => this.GetTypeReference(TypeReferences.TypeReference.JSLocalField);

  internal Type JSObject => this.GetTypeReference(TypeReferences.TypeReference.JSObject);

  internal Type JScriptException
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.JScriptException);
  }

  internal Type LateBinding => this.GetTypeReference(TypeReferences.TypeReference.LateBinding);

  internal Type LenientGlobalObject
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.LenientGlobalObject);
  }

  internal Type MathObject => this.GetTypeReference(TypeReferences.TypeReference.MathObject);

  internal Type MethodInvoker => this.GetTypeReference(TypeReferences.TypeReference.MethodInvoker);

  internal Type Missing => this.GetTypeReference(TypeReferences.TypeReference.Missing);

  internal Type Namespace => this.GetTypeReference(TypeReferences.TypeReference.Namespace);

  internal Type NotRecommended
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.NotRecommended);
  }

  internal Type NumberObject => this.GetTypeReference(TypeReferences.TypeReference.NumberObject);

  internal Type NumericBinary => this.GetTypeReference(TypeReferences.TypeReference.NumericBinary);

  internal Type NumericUnary => this.GetTypeReference(TypeReferences.TypeReference.NumericUnary);

  internal Type ObjectConstructor
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.ObjectConstructor);
  }

  internal Type Override => this.GetTypeReference(TypeReferences.TypeReference.Override);

  internal Type Package => this.GetTypeReference(TypeReferences.TypeReference.Package);

  internal Type Plus => this.GetTypeReference(TypeReferences.TypeReference.Plus);

  internal Type PostOrPrefixOperator
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.PostOrPrefixOperator);
  }

  internal Type RangeErrorObject
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.RangeErrorObject);
  }

  internal Type ReferenceAttribute
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.ReferenceAttribute);
  }

  internal Type ReferenceErrorObject
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.ReferenceErrorObject);
  }

  internal Type RegExpConstructor
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.RegExpConstructor);
  }

  internal Type RegExpObject => this.GetTypeReference(TypeReferences.TypeReference.RegExpObject);

  internal Type Relational => this.GetTypeReference(TypeReferences.TypeReference.Relational);

  internal Type ReturnOutOfFinally
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.ReturnOutOfFinally);
  }

  internal Type Runtime => this.GetTypeReference(TypeReferences.TypeReference.Runtime);

  internal Type ScriptFunction
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.ScriptFunction);
  }

  internal Type ScriptObject => this.GetTypeReference(TypeReferences.TypeReference.ScriptObject);

  internal Type ScriptStream => this.GetTypeReference(TypeReferences.TypeReference.ScriptStream);

  internal Type SimpleHashtable
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.SimpleHashtable);
  }

  internal Type StackFrame => this.GetTypeReference(TypeReferences.TypeReference.StackFrame);

  internal Type StrictEquality
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.StrictEquality);
  }

  internal Type StringObject => this.GetTypeReference(TypeReferences.TypeReference.StringObject);

  internal Type SyntaxErrorObject
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.SyntaxErrorObject);
  }

  internal Type Throw => this.GetTypeReference(TypeReferences.TypeReference.Throw);

  internal Type Try => this.GetTypeReference(TypeReferences.TypeReference.Try);

  internal Type TypedArray => this.GetTypeReference(TypeReferences.TypeReference.TypedArray);

  internal Type TypeErrorObject
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.TypeErrorObject);
  }

  internal Type Typeof => this.GetTypeReference(TypeReferences.TypeReference.Typeof);

  internal Type URIErrorObject
  {
    get => this.GetTypeReference(TypeReferences.TypeReference.URIErrorObject);
  }

  internal Type VBArrayObject => this.GetTypeReference(TypeReferences.TypeReference.VBArrayObject);

  internal Type With => this.GetTypeReference(TypeReferences.TypeReference.With);

  internal Type VsaEngine => this.GetTypeReference(TypeReferences.TypeReference.VsaEngine);

  internal Type Array => typeof (System.Array);

  internal Type Attribute => typeof (System.Attribute);

  internal Type AttributeUsageAttribute => typeof (System.AttributeUsageAttribute);

  internal Type Byte => typeof (byte);

  internal Type Boolean => typeof (bool);

  internal Type Char => typeof (char);

  internal Type CLSCompliantAttribute => typeof (System.CLSCompliantAttribute);

  internal Type ContextStaticAttribute => typeof (System.ContextStaticAttribute);

  internal Type DateTime => typeof (System.DateTime);

  internal Type DBNull => typeof (System.DBNull);

  internal Type Delegate => typeof (System.Delegate);

  internal Type Decimal => typeof (System.Decimal);

  internal Type Double => typeof (double);

  internal Type Enum => typeof (System.Enum);

  internal Type Exception => typeof (System.Exception);

  internal Type IConvertible => typeof (System.IConvertible);

  internal Type IntPtr => typeof (System.IntPtr);

  internal Type Int16 => typeof (short);

  internal Type Int32 => typeof (int);

  internal Type Int64 => typeof (long);

  internal Type Object => typeof (object);

  internal Type ObsoleteAttribute => typeof (System.ObsoleteAttribute);

  internal Type ParamArrayAttribute => typeof (System.ParamArrayAttribute);

  internal Type RuntimeTypeHandle => typeof (System.RuntimeTypeHandle);

  internal Type SByte => typeof (sbyte);

  internal Type Single => typeof (float);

  internal Type STAThreadAttribute => typeof (System.STAThreadAttribute);

  internal Type String => typeof (string);

  internal Type Type => typeof (Type);

  internal Type TypeCode => typeof (System.TypeCode);

  internal Type UIntPtr => typeof (System.UIntPtr);

  internal Type UInt16 => typeof (ushort);

  internal Type UInt32 => typeof (uint);

  internal Type UInt64 => typeof (ulong);

  internal Type ValueType => typeof (System.ValueType);

  internal Type Void => typeof (void);

  internal Type IEnumerable => typeof (System.Collections.IEnumerable);

  internal Type IEnumerator => typeof (System.Collections.IEnumerator);

  internal Type IList => typeof (System.Collections.IList);

  internal Type Debugger => typeof (System.Diagnostics.Debugger);

  internal Type DebuggableAttribute => typeof (System.Diagnostics.DebuggableAttribute);

  internal Type DebuggerHiddenAttribute => typeof (System.Diagnostics.DebuggerHiddenAttribute);

  internal Type DebuggerStepThroughAttribute => typeof (System.Diagnostics.DebuggerStepThroughAttribute);

  internal Type DefaultMemberAttribute => typeof (System.Reflection.DefaultMemberAttribute);

  internal Type EventInfo => typeof (System.Reflection.EventInfo);

  internal Type FieldInfo => typeof (System.Reflection.FieldInfo);

  internal Type CompilerGlobalScopeAttribute => typeof (System.Runtime.CompilerServices.CompilerGlobalScopeAttribute);

  internal Type RequiredAttributeAttribute => typeof (System.Runtime.CompilerServices.RequiredAttributeAttribute);

  internal Type CoClassAttribute => typeof (System.Runtime.InteropServices.CoClassAttribute);

  internal Type IExpando => typeof (System.Runtime.InteropServices.Expando.IExpando);

  internal Type CodeAccessSecurityAttribute => typeof (System.Security.Permissions.CodeAccessSecurityAttribute);

  internal Type AllowPartiallyTrustedCallersAttribute
  {
    get => typeof (System.Security.AllowPartiallyTrustedCallersAttribute);
  }

  internal Type ArrayOfObject => typeof (object[]);

  internal Type ArrayOfString => typeof (string[]);

  internal Type SystemConvert => typeof (System.Convert);

  internal Type ReflectionMissing => typeof (System.Reflection.Missing);

  internal MethodInfo constructArrayMethod => this.ArrayConstructor.GetMethod("ConstructArray");

  internal MethodInfo isMissingMethod => this.Binding.GetMethod("IsMissing");

  internal ConstructorInfo bitwiseBinaryConstructor
  {
    get
    {
      return this.BitwiseBinary.GetConstructor(new Type[1]
      {
        this.Int32
      });
    }
  }

  internal MethodInfo evaluateBitwiseBinaryMethod
  {
    get => this.BitwiseBinary.GetMethod("EvaluateBitwiseBinary");
  }

  internal ConstructorInfo breakOutOfFinallyConstructor
  {
    get
    {
      return this.BreakOutOfFinally.GetConstructor(new Type[1]
      {
        this.Int32
      });
    }
  }

  internal ConstructorInfo closureConstructor
  {
    get
    {
      return this.Closure.GetConstructor(new Type[1]
      {
        this.FunctionObject
      });
    }
  }

  internal ConstructorInfo continueOutOfFinallyConstructor
  {
    get
    {
      return this.ContinueOutOfFinally.GetConstructor(new Type[1]
      {
        this.Int32
      });
    }
  }

  internal MethodInfo checkIfDoubleIsIntegerMethod
  {
    get => this.Convert.GetMethod("CheckIfDoubleIsInteger");
  }

  internal MethodInfo checkIfSingleIsIntegerMethod
  {
    get => this.Convert.GetMethod("CheckIfSingleIsInteger");
  }

  internal MethodInfo coerce2Method => this.Convert.GetMethod("Coerce2");

  internal MethodInfo coerceTMethod => this.Convert.GetMethod("CoerceT");

  internal MethodInfo throwTypeMismatch => this.Convert.GetMethod("ThrowTypeMismatch");

  internal MethodInfo doubleToBooleanMethod
  {
    get
    {
      return this.Convert.GetMethod("ToBoolean", new Type[1]
      {
        this.Double
      });
    }
  }

  internal MethodInfo toBooleanMethod
  {
    get
    {
      return this.Convert.GetMethod("ToBoolean", new Type[2]
      {
        this.Object,
        this.Boolean
      });
    }
  }

  internal MethodInfo toForInObjectMethod
  {
    get
    {
      return this.Convert.GetMethod("ToForInObject", new Type[2]
      {
        this.Object,
        this.VsaEngine
      });
    }
  }

  internal MethodInfo toInt32Method
  {
    get
    {
      return this.Convert.GetMethod("ToInt32", new Type[1]
      {
        this.Object
      });
    }
  }

  internal MethodInfo toNativeArrayMethod => this.Convert.GetMethod("ToNativeArray");

  internal MethodInfo toNumberMethod
  {
    get
    {
      return this.Convert.GetMethod("ToNumber", new Type[1]
      {
        this.Object
      });
    }
  }

  internal MethodInfo toObjectMethod
  {
    get
    {
      return this.Convert.GetMethod("ToObject", new Type[2]
      {
        this.Object,
        this.VsaEngine
      });
    }
  }

  internal MethodInfo toObject2Method
  {
    get
    {
      return this.Convert.GetMethod("ToObject2", new Type[2]
      {
        this.Object,
        this.VsaEngine
      });
    }
  }

  internal MethodInfo doubleToStringMethod
  {
    get
    {
      return this.Convert.GetMethod("ToString", new Type[1]
      {
        this.Double
      });
    }
  }

  internal MethodInfo toStringMethod
  {
    get
    {
      return this.Convert.GetMethod("ToString", new Type[2]
      {
        this.Object,
        this.Boolean
      });
    }
  }

  internal System.Reflection.FieldInfo undefinedField => this.Empty.GetField("Value");

  internal ConstructorInfo equalityConstructor
  {
    get
    {
      return this.Equality.GetConstructor(new Type[1]
      {
        this.Int32
      });
    }
  }

  internal MethodInfo evaluateEqualityMethod
  {
    get
    {
      return this.Equality.GetMethod("EvaluateEquality", new Type[2]
      {
        this.Object,
        this.Object
      });
    }
  }

  internal MethodInfo jScriptEqualsMethod => this.Equality.GetMethod("JScriptEquals");

  internal MethodInfo jScriptEvaluateMethod1
  {
    get
    {
      return this.Eval.GetMethod("JScriptEvaluate", new Type[2]
      {
        this.Object,
        this.VsaEngine
      });
    }
  }

  internal MethodInfo jScriptEvaluateMethod2
  {
    get
    {
      return this.Eval.GetMethod("JScriptEvaluate", new Type[3]
      {
        this.Object,
        this.Object,
        this.VsaEngine
      });
    }
  }

  internal MethodInfo jScriptGetEnumeratorMethod => this.ForIn.GetMethod("JScriptGetEnumerator");

  internal MethodInfo jScriptFunctionDeclarationMethod
  {
    get => this.FunctionDeclaration.GetMethod("JScriptFunctionDeclaration");
  }

  internal MethodInfo jScriptFunctionExpressionMethod
  {
    get => this.FunctionExpression.GetMethod("JScriptFunctionExpression");
  }

  internal System.Reflection.FieldInfo contextEngineField => this.Globals.GetField("contextEngine");

  internal MethodInfo fastConstructArrayLiteralMethod
  {
    get => this.Globals.GetMethod("ConstructArrayLiteral");
  }

  internal ConstructorInfo globalScopeConstructor
  {
    get
    {
      return this.GlobalScope.GetConstructor(new Type[2]
      {
        this.GlobalScope,
        this.VsaEngine
      });
    }
  }

  internal MethodInfo getDefaultThisObjectMethod
  {
    get => this.IActivationObject.GetMethod("GetDefaultThisObject");
  }

  internal MethodInfo getFieldMethod
  {
    get
    {
      return this.IActivationObject.GetMethod("GetField", new Type[2]
      {
        this.String,
        this.Int32
      });
    }
  }

  internal MethodInfo getGlobalScopeMethod => this.IActivationObject.GetMethod("GetGlobalScope");

  internal MethodInfo getMemberValueMethod
  {
    get
    {
      return this.IActivationObject.GetMethod("GetMemberValue", new Type[2]
      {
        this.String,
        this.Int32
      });
    }
  }

  internal MethodInfo jScriptImportMethod => this.Import.GetMethod("JScriptImport");

  internal MethodInfo jScriptInMethod => this.In.GetMethod("JScriptIn");

  internal MethodInfo getEngineMethod => this.INeedEngine.GetMethod("GetEngine");

  internal MethodInfo setEngineMethod => this.INeedEngine.GetMethod("SetEngine");

  internal MethodInfo jScriptInstanceofMethod => this.Instanceof.GetMethod("JScriptInstanceof");

  internal ConstructorInfo scriptExceptionConstructor
  {
    get
    {
      return this.JScriptException.GetConstructor(new Type[1]
      {
        this.JSError
      });
    }
  }

  internal ConstructorInfo jsFunctionAttributeConstructor
  {
    get
    {
      return this.JSFunctionAttribute.GetConstructor(new Type[1]
      {
        this.JSFunctionAttributeEnum
      });
    }
  }

  internal ConstructorInfo jsLocalFieldConstructor
  {
    get
    {
      return this.JSLocalField.GetConstructor(new Type[3]
      {
        this.String,
        this.RuntimeTypeHandle,
        this.Int32
      });
    }
  }

  internal MethodInfo setMemberValue2Method
  {
    get
    {
      return this.JSObject.GetMethod("SetMemberValue2", new Type[2]
      {
        this.String,
        this.Object
      });
    }
  }

  internal ConstructorInfo lateBindingConstructor2
  {
    get
    {
      return this.LateBinding.GetConstructor(new Type[2]
      {
        this.String,
        this.Object
      });
    }
  }

  internal ConstructorInfo lateBindingConstructor
  {
    get
    {
      return this.LateBinding.GetConstructor(new Type[1]
      {
        this.String
      });
    }
  }

  internal System.Reflection.FieldInfo objectField => this.LateBinding.GetField("obj");

  internal MethodInfo callMethod
  {
    get
    {
      return this.LateBinding.GetMethod("Call", new Type[4]
      {
        this.ArrayOfObject,
        this.Boolean,
        this.Boolean,
        this.VsaEngine
      });
    }
  }

  internal MethodInfo callValueMethod
  {
    get
    {
      return this.LateBinding.GetMethod("CallValue", new Type[6]
      {
        this.Object,
        this.Object,
        this.ArrayOfObject,
        this.Boolean,
        this.Boolean,
        this.VsaEngine
      });
    }
  }

  internal MethodInfo callValue2Method
  {
    get
    {
      return this.LateBinding.GetMethod("CallValue2", new Type[6]
      {
        this.Object,
        this.Object,
        this.ArrayOfObject,
        this.Boolean,
        this.Boolean,
        this.VsaEngine
      });
    }
  }

  internal MethodInfo deleteMethod => this.LateBinding.GetMethod("Delete");

  internal MethodInfo deleteMemberMethod => this.LateBinding.GetMethod("DeleteMember");

  internal MethodInfo getNonMissingValueMethod => this.LateBinding.GetMethod("GetNonMissingValue");

  internal MethodInfo getValue2Method => this.LateBinding.GetMethod("GetValue2");

  internal MethodInfo setIndexedPropertyValueStaticMethod
  {
    get => this.LateBinding.GetMethod("SetIndexedPropertyValueStatic");
  }

  internal MethodInfo setValueMethod => this.LateBinding.GetMethod("SetValue");

  internal System.Reflection.FieldInfo missingField => this.Missing.GetField("Value");

  internal MethodInfo getNamespaceMethod => this.Namespace.GetMethod("GetNamespace");

  internal ConstructorInfo numericBinaryConstructor
  {
    get
    {
      return this.NumericBinary.GetConstructor(new Type[1]
      {
        this.Int32
      });
    }
  }

  internal MethodInfo numericbinaryDoOpMethod => this.NumericBinary.GetMethod("DoOp");

  internal MethodInfo evaluateNumericBinaryMethod
  {
    get => this.NumericBinary.GetMethod("EvaluateNumericBinary");
  }

  internal ConstructorInfo numericUnaryConstructor
  {
    get
    {
      return this.NumericUnary.GetConstructor(new Type[1]
      {
        this.Int32
      });
    }
  }

  internal MethodInfo evaluateUnaryMethod => this.NumericUnary.GetMethod("EvaluateUnary");

  internal MethodInfo constructObjectMethod => this.ObjectConstructor.GetMethod("ConstructObject");

  internal MethodInfo jScriptPackageMethod => this.Package.GetMethod("JScriptPackage");

  internal ConstructorInfo plusConstructor => this.Plus.GetConstructor(new Type[0]);

  internal MethodInfo plusDoOpMethod => this.Plus.GetMethod("DoOp");

  internal MethodInfo evaluatePlusMethod => this.Plus.GetMethod("EvaluatePlus");

  internal ConstructorInfo postOrPrefixConstructor
  {
    get
    {
      return this.PostOrPrefixOperator.GetConstructor(new Type[1]
      {
        this.Int32
      });
    }
  }

  internal MethodInfo evaluatePostOrPrefixOperatorMethod
  {
    get => this.PostOrPrefixOperator.GetMethod("EvaluatePostOrPrefix");
  }

  internal ConstructorInfo referenceAttributeConstructor
  {
    get
    {
      return this.ReferenceAttribute.GetConstructor(new Type[1]
      {
        this.String
      });
    }
  }

  internal MethodInfo regExpConstructMethod
  {
    get
    {
      return this.RegExpConstructor.GetMethod("Construct", new Type[4]
      {
        this.String,
        this.Boolean,
        this.Boolean,
        this.Boolean
      });
    }
  }

  internal ConstructorInfo relationalConstructor
  {
    get
    {
      return this.Relational.GetConstructor(new Type[1]
      {
        this.Int32
      });
    }
  }

  internal MethodInfo evaluateRelationalMethod => this.Relational.GetMethod("EvaluateRelational");

  internal MethodInfo jScriptCompareMethod => this.Relational.GetMethod("JScriptCompare");

  internal ConstructorInfo returnOutOfFinallyConstructor
  {
    get => this.ReturnOutOfFinally.GetConstructor(new Type[0]);
  }

  internal MethodInfo doubleToInt64 => this.Runtime.GetMethod("DoubleToInt64");

  internal MethodInfo uncheckedDecimalToInt64Method
  {
    get => this.Runtime.GetMethod("UncheckedDecimalToInt64");
  }

  internal System.Reflection.FieldInfo engineField => this.ScriptObject.GetField("engine");

  internal MethodInfo getParentMethod => this.ScriptObject.GetMethod("GetParent");

  internal MethodInfo writeMethod => this.ScriptStream.GetMethod("Write");

  internal MethodInfo writeLineMethod => this.ScriptStream.GetMethod("WriteLine");

  internal ConstructorInfo hashtableCtor
  {
    get
    {
      return this.SimpleHashtable.GetConstructor(new Type[1]
      {
        this.UInt32
      });
    }
  }

  internal MethodInfo hashtableGetItem
  {
    get
    {
      return this.SimpleHashtable.GetMethod("get_Item", new Type[1]
      {
        this.Object
      });
    }
  }

  internal MethodInfo hashTableGetEnumerator
  {
    get => this.SimpleHashtable.GetMethod("GetEnumerator", Type.EmptyTypes);
  }

  internal MethodInfo hashtableRemove
  {
    get
    {
      return this.SimpleHashtable.GetMethod("Remove", new Type[1]
      {
        this.Object
      });
    }
  }

  internal MethodInfo hashtableSetItem
  {
    get
    {
      return this.SimpleHashtable.GetMethod("set_Item", new Type[2]
      {
        this.Object,
        this.Object
      });
    }
  }

  internal System.Reflection.FieldInfo closureInstanceField
  {
    get => this.StackFrame.GetField("closureInstance");
  }

  internal System.Reflection.FieldInfo localVarsField => this.StackFrame.GetField("localVars");

  internal MethodInfo pushStackFrameForMethod
  {
    get => this.StackFrame.GetMethod("PushStackFrameForMethod");
  }

  internal MethodInfo pushStackFrameForStaticMethod
  {
    get => this.StackFrame.GetMethod("PushStackFrameForStaticMethod");
  }

  internal MethodInfo jScriptStrictEqualsMethod
  {
    get
    {
      return this.StrictEquality.GetMethod("JScriptStrictEquals", new Type[2]
      {
        this.Object,
        this.Object
      });
    }
  }

  internal MethodInfo jScriptThrowMethod => this.Throw.GetMethod("JScriptThrow");

  internal MethodInfo jScriptExceptionValueMethod => this.Try.GetMethod("JScriptExceptionValue");

  internal MethodInfo jScriptTypeofMethod => this.Typeof.GetMethod("JScriptTypeof");

  internal ConstructorInfo vsaEngineConstructor => this.VsaEngine.GetConstructor(new Type[0]);

  internal MethodInfo createVsaEngine => this.VsaEngine.GetMethod("CreateEngine", new Type[0]);

  internal MethodInfo createVsaEngineWithType
  {
    get
    {
      return this.VsaEngine.GetMethod("CreateEngineWithType", new Type[1]
      {
        this.RuntimeTypeHandle
      });
    }
  }

  internal MethodInfo getOriginalArrayConstructorMethod
  {
    get => this.VsaEngine.GetMethod("GetOriginalArrayConstructor");
  }

  internal MethodInfo getOriginalObjectConstructorMethod
  {
    get => this.VsaEngine.GetMethod("GetOriginalObjectConstructor");
  }

  internal MethodInfo getOriginalRegExpConstructorMethod
  {
    get => this.VsaEngine.GetMethod("GetOriginalRegExpConstructor");
  }

  internal MethodInfo popScriptObjectMethod => this.VsaEngine.GetMethod("PopScriptObject");

  internal MethodInfo pushScriptObjectMethod => this.VsaEngine.GetMethod("PushScriptObject");

  internal MethodInfo scriptObjectStackTopMethod
  {
    get => this.VsaEngine.GetMethod("ScriptObjectStackTop");
  }

  internal MethodInfo getLenientGlobalObjectMethod
  {
    get => this.VsaEngine.GetProperty("LenientGlobalObject").GetGetMethod();
  }

  internal MethodInfo jScriptWithMethod => this.With.GetMethod("JScriptWith");

  internal ConstructorInfo clsCompliantAttributeCtor
  {
    get
    {
      return this.CLSCompliantAttribute.GetConstructor(new Type[1]
      {
        this.Boolean
      });
    }
  }

  internal MethodInfo getEnumeratorMethod
  {
    get => this.IEnumerable.GetMethod("GetEnumerator", Type.EmptyTypes);
  }

  internal MethodInfo moveNextMethod => this.IEnumerator.GetMethod("MoveNext", Type.EmptyTypes);

  internal MethodInfo getCurrentMethod
  {
    get => this.IEnumerator.GetProperty("Current", Type.EmptyTypes).GetGetMethod();
  }

  internal ConstructorInfo contextStaticAttributeCtor
  {
    get => this.ContextStaticAttribute.GetConstructor(new Type[0]);
  }

  internal MethodInfo changeTypeMethod
  {
    get
    {
      return this.SystemConvert.GetMethod("ChangeType", new Type[2]
      {
        this.Object,
        this.TypeCode
      });
    }
  }

  internal MethodInfo convertCharToStringMethod
  {
    get
    {
      return this.SystemConvert.GetMethod("ToString", new Type[1]
      {
        this.Char
      });
    }
  }

  internal ConstructorInfo dateTimeConstructor
  {
    get
    {
      return this.DateTime.GetConstructor(new Type[1]
      {
        this.Int64
      });
    }
  }

  internal MethodInfo dateTimeToStringMethod => this.DateTime.GetMethod("ToString", new Type[0]);

  internal MethodInfo dateTimeToInt64Method => this.DateTime.GetProperty("Ticks").GetGetMethod();

  internal ConstructorInfo decimalConstructor
  {
    get
    {
      return this.Decimal.GetConstructor(new Type[5]
      {
        this.Int32,
        this.Int32,
        this.Int32,
        this.Boolean,
        this.Byte
      });
    }
  }

  internal System.Reflection.FieldInfo decimalZeroField => this.Decimal.GetField("Zero");

  internal MethodInfo decimalCompare
  {
    get
    {
      return this.Decimal.GetMethod("Compare", new Type[2]
      {
        this.Decimal,
        this.Decimal
      });
    }
  }

  internal MethodInfo doubleToDecimalMethod
  {
    get
    {
      return this.Decimal.GetMethod("op_Explicit", new Type[1]
      {
        this.Double
      });
    }
  }

  internal MethodInfo int32ToDecimalMethod
  {
    get
    {
      return this.Decimal.GetMethod("op_Implicit", new Type[1]
      {
        this.Int32
      });
    }
  }

  internal MethodInfo int64ToDecimalMethod
  {
    get
    {
      return this.Decimal.GetMethod("op_Implicit", new Type[1]
      {
        this.Int64
      });
    }
  }

  internal MethodInfo uint32ToDecimalMethod
  {
    get
    {
      return this.Decimal.GetMethod("op_Implicit", new Type[1]
      {
        this.UInt32
      });
    }
  }

  internal MethodInfo uint64ToDecimalMethod
  {
    get
    {
      return this.Decimal.GetMethod("op_Implicit", new Type[1]
      {
        this.UInt64
      });
    }
  }

  internal MethodInfo decimalToDoubleMethod
  {
    get
    {
      return this.Decimal.GetMethod("ToDouble", new Type[1]
      {
        this.Decimal
      });
    }
  }

  internal MethodInfo decimalToInt32Method
  {
    get
    {
      return this.Decimal.GetMethod("ToInt32", new Type[1]
      {
        this.Decimal
      });
    }
  }

  internal MethodInfo decimalToInt64Method
  {
    get
    {
      return this.Decimal.GetMethod("ToInt64", new Type[1]
      {
        this.Decimal
      });
    }
  }

  internal MethodInfo decimalToStringMethod => this.Decimal.GetMethod("ToString", new Type[0]);

  internal MethodInfo decimalToUInt32Method
  {
    get
    {
      return this.Decimal.GetMethod("ToUInt32", new Type[1]
      {
        this.Decimal
      });
    }
  }

  internal MethodInfo decimalToUInt64Method
  {
    get
    {
      return this.Decimal.GetMethod("ToUInt64", new Type[1]
      {
        this.Decimal
      });
    }
  }

  internal MethodInfo debugBreak => this.Debugger.GetMethod("Break", new Type[0]);

  internal ConstructorInfo debuggerHiddenAttributeCtor
  {
    get => this.DebuggerHiddenAttribute.GetConstructor(new Type[0]);
  }

  internal ConstructorInfo debuggerStepThroughAttributeCtor
  {
    get => this.DebuggerStepThroughAttribute.GetConstructor(new Type[0]);
  }

  internal MethodInfo int32ToStringMethod => this.Int32.GetMethod("ToString", new Type[0]);

  internal MethodInfo int64ToStringMethod => this.Int64.GetMethod("ToString", new Type[0]);

  internal MethodInfo equalsMethod
  {
    get
    {
      return this.Object.GetMethod("Equals", new Type[1]
      {
        this.Object
      });
    }
  }

  internal ConstructorInfo defaultMemberAttributeCtor
  {
    get
    {
      return this.DefaultMemberAttribute.GetConstructor(new Type[1]
      {
        this.String
      });
    }
  }

  internal MethodInfo getFieldValueMethod
  {
    get
    {
      return this.FieldInfo.GetMethod("GetValue", new Type[1]
      {
        this.Object
      });
    }
  }

  internal MethodInfo setFieldValueMethod
  {
    get
    {
      return this.FieldInfo.GetMethod("SetValue", new Type[2]
      {
        this.Object,
        this.Object
      });
    }
  }

  internal System.Reflection.FieldInfo systemReflectionMissingField
  {
    get => this.ReflectionMissing.GetField("Value");
  }

  internal ConstructorInfo compilerGlobalScopeAttributeCtor
  {
    get => this.CompilerGlobalScopeAttribute.GetConstructor(new Type[0]);
  }

  internal MethodInfo stringConcatArrMethod
  {
    get
    {
      return this.String.GetMethod("Concat", new Type[1]
      {
        this.ArrayOfString
      });
    }
  }

  internal MethodInfo stringConcat4Method
  {
    get
    {
      return this.String.GetMethod("Concat", new Type[4]
      {
        this.String,
        this.String,
        this.String,
        this.String
      });
    }
  }

  internal MethodInfo stringConcat3Method
  {
    get
    {
      return this.String.GetMethod("Concat", new Type[3]
      {
        this.String,
        this.String,
        this.String
      });
    }
  }

  internal MethodInfo stringConcat2Method
  {
    get
    {
      return this.String.GetMethod("Concat", new Type[2]
      {
        this.String,
        this.String
      });
    }
  }

  internal MethodInfo stringEqualsMethod
  {
    get
    {
      return this.String.GetMethod("Equals", new Type[2]
      {
        this.String,
        this.String
      });
    }
  }

  internal MethodInfo stringLengthMethod => this.String.GetProperty("Length").GetGetMethod();

  internal MethodInfo getMethodMethod
  {
    get
    {
      return this.Type.GetMethod("GetMethod", new Type[1]
      {
        this.String
      });
    }
  }

  internal MethodInfo getTypeMethod
  {
    get
    {
      return this.Type.GetMethod("GetType", new Type[1]
      {
        this.String
      });
    }
  }

  internal MethodInfo getTypeFromHandleMethod
  {
    get
    {
      return this.Type.GetMethod("GetTypeFromHandle", new Type[1]
      {
        this.RuntimeTypeHandle
      });
    }
  }

  internal MethodInfo uint32ToStringMethod => this.UInt32.GetMethod("ToString", new Type[0]);

  internal MethodInfo uint64ToStringMethod => this.UInt64.GetMethod("ToString", new Type[0]);

  internal Type ToReferenceContext(Type type)
  {
    if (this.InReferenceContext(type))
      return type;
    return type.IsArray ? Microsoft.JScript.Convert.ToType(Microsoft.JScript.TypedArray.ToRankString(type.GetArrayRank()), this.ToReferenceContext(type.GetElementType())) : this.JScriptReferenceModule.ResolveType(type.MetadataToken, (Type[]) null, (Type[]) null);
  }

  internal IReflect ToReferenceContext(IReflect ireflect)
  {
    return (object) (ireflect as Type) != null ? (IReflect) this.ToReferenceContext((Type) ireflect) : ireflect;
  }

  internal MethodInfo ToReferenceContext(MethodInfo method)
  {
    switch (method)
    {
      case JSMethod _:
        method = ((JSMethod) method).GetMethodInfo((CompilerGlobals) null);
        break;
      case JSMethodInfo _:
        method = ((JSMethodInfo) method).method;
        break;
    }
    return (MethodInfo) this.MapMemberInfoToReferenceContext((MemberInfo) method);
  }

  internal PropertyInfo ToReferenceContext(PropertyInfo property)
  {
    return (PropertyInfo) this.MapMemberInfoToReferenceContext((MemberInfo) property);
  }

  internal System.Reflection.FieldInfo ToReferenceContext(System.Reflection.FieldInfo field)
  {
    return (System.Reflection.FieldInfo) this.MapMemberInfoToReferenceContext((MemberInfo) field);
  }

  internal ConstructorInfo ToReferenceContext(ConstructorInfo constructor)
  {
    return (ConstructorInfo) this.MapMemberInfoToReferenceContext((MemberInfo) constructor);
  }

  private MemberInfo MapMemberInfoToReferenceContext(MemberInfo member)
  {
    return this.InReferenceContext(member.DeclaringType) ? member : this.JScriptReferenceModule.ResolveMember(member.MetadataToken);
  }

  internal bool InReferenceContext(Type type)
  {
    if (type == (Type) null)
      return true;
    Assembly assembly = type.Assembly;
    return assembly.ReflectionOnly || assembly != typeof (TypeReferences).Assembly || !this.JScriptReferenceModule.Assembly.ReflectionOnly;
  }

  internal bool InReferenceContext(MemberInfo member)
  {
    if (member == (MemberInfo) null)
      return true;
    switch (member)
    {
      case JSMethod _:
        member = (MemberInfo) ((JSMethod) member).GetMethodInfo((CompilerGlobals) null);
        break;
      case JSMethodInfo _:
        member = (MemberInfo) ((JSMethodInfo) member).method;
        break;
    }
    return this.InReferenceContext(member.DeclaringType);
  }

  internal bool InReferenceContext(IReflect ireflect)
  {
    return ireflect == null || (object) (ireflect as Type) == null || this.InReferenceContext((Type) ireflect);
  }

  internal static Type ToExecutionContext(Type type)
  {
    return TypeReferences.InExecutionContext(type) ? type : typeof (TypeReferences).Module.ResolveType(type.MetadataToken, (Type[]) null, (Type[]) null);
  }

  internal static IReflect ToExecutionContext(IReflect ireflect)
  {
    return (object) (ireflect as Type) != null ? (IReflect) TypeReferences.ToExecutionContext((Type) ireflect) : ireflect;
  }

  internal static MethodInfo ToExecutionContext(MethodInfo method)
  {
    switch (method)
    {
      case JSMethod _:
        method = ((JSMethod) method).GetMethodInfo((CompilerGlobals) null);
        break;
      case JSMethodInfo _:
        method = ((JSMethodInfo) method).method;
        break;
    }
    return (MethodInfo) TypeReferences.MapMemberInfoToExecutionContext((MemberInfo) method);
  }

  internal static PropertyInfo ToExecutionContext(PropertyInfo property)
  {
    return (PropertyInfo) TypeReferences.MapMemberInfoToExecutionContext((MemberInfo) property);
  }

  internal static System.Reflection.FieldInfo ToExecutionContext(System.Reflection.FieldInfo field)
  {
    return (System.Reflection.FieldInfo) TypeReferences.MapMemberInfoToExecutionContext((MemberInfo) field);
  }

  internal static ConstructorInfo ToExecutionContext(ConstructorInfo constructor)
  {
    return (ConstructorInfo) TypeReferences.MapMemberInfoToExecutionContext((MemberInfo) constructor);
  }

  private static MemberInfo MapMemberInfoToExecutionContext(MemberInfo member)
  {
    return TypeReferences.InExecutionContext(member.DeclaringType) ? member : typeof (TypeReferences).Module.ResolveMember(member.MetadataToken);
  }

  internal static bool InExecutionContext(Type type)
  {
    if (type == (Type) null)
      return true;
    Assembly assembly = type.Assembly;
    return !assembly.ReflectionOnly || assembly.Location != typeof (TypeReferences).Assembly.Location;
  }

  internal static object GetDefaultParameterValue(ParameterInfo parameter)
  {
    return parameter.GetType().Assembly == typeof (TypeReferences).Assembly || !parameter.Member.DeclaringType.Assembly.ReflectionOnly ? parameter.DefaultValue : parameter.RawDefaultValue;
  }

  internal static object GetConstantValue(System.Reflection.FieldInfo field)
  {
    if (field.GetType().Assembly == typeof (TypeReferences).Assembly || !field.DeclaringType.Assembly.ReflectionOnly)
      return field.GetValue((object) null);
    Type fieldType = field.FieldType;
    object rawConstantValue = field.GetRawConstantValue();
    return fieldType.IsEnum ? MetadataEnumValue.GetEnumValue(fieldType, rawConstantValue) : rawConstantValue;
  }

  private enum TypeReference
  {
    ArgumentsObject,
    ArrayConstructor,
    ArrayObject,
    ArrayWrapper,
    Binding,
    BitwiseBinary,
    BooleanObject,
    BreakOutOfFinally,
    BuiltinFunction,
    ClassScope,
    Closure,
    ContinueOutOfFinally,
    Convert,
    DateObject,
    Empty,
    EnumeratorObject,
    Equality,
    ErrorObject,
    Eval,
    EvalErrorObject,
    Expando,
    FieldAccessor,
    ForIn,
    FunctionDeclaration,
    FunctionExpression,
    FunctionObject,
    FunctionWrapper,
    GlobalObject,
    GlobalScope,
    Globals,
    Hide,
    IActivationObject,
    INeedEngine,
    Import,
    In,
    Instanceof,
    JSError,
    JSFunctionAttribute,
    JSFunctionAttributeEnum,
    JSLocalField,
    JSObject,
    JScriptException,
    LateBinding,
    LenientGlobalObject,
    MathObject,
    MethodInvoker,
    Missing,
    Namespace,
    NotRecommended,
    NumberObject,
    NumericBinary,
    NumericUnary,
    ObjectConstructor,
    Override,
    Package,
    Plus,
    PostOrPrefixOperator,
    RangeErrorObject,
    ReferenceAttribute,
    ReferenceErrorObject,
    RegExpConstructor,
    RegExpObject,
    Relational,
    ReturnOutOfFinally,
    Runtime,
    ScriptFunction,
    ScriptObject,
    ScriptStream,
    SimpleHashtable,
    StackFrame,
    StrictEquality,
    StringObject,
    SyntaxErrorObject,
    Throw,
    Try,
    TypedArray,
    TypeErrorObject,
    Typeof,
    URIErrorObject,
    VBArrayObject,
    With,
    BaseVsaStartup,
    VsaEngine,
  }
}
