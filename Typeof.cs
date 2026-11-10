// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Typeof
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

/// <summary>Determines the data types of objects. This class belongs to the abstract syntax tree category.</summary>
public sealed class Typeof : UnaryOp
{
  internal Typeof(Context context, AST operand)
    : base(context, operand)
  {
  }

  internal override object Evaluate()
  {
    try
    {
      return (object) Typeof.JScriptTypeof(this.operand.Evaluate(), VsaEngine.executeForJSEE);
    }
    catch (JScriptException ex)
    {
      if ((ex.Number & (int) ushort.MaxValue) == 5009)
        return (object) "undefined";
      throw ex;
    }
  }

  internal override IReflect InferType(JSField inference_target) => (IReflect) Typeob.String;

  /// <summary>Determines the data type of the specified object.</summary>
  /// <returns>The type of <paramref name="value" />.</returns>
  /// <param name="value">The object to determine the type of.</param>
  public static string JScriptTypeof(object value) => Typeof.JScriptTypeof(value, false);

  internal static string JScriptTypeof(object value, bool checkForDebuggerObject)
  {
    switch (Convert.GetTypeCode(value))
    {
      case TypeCode.Empty:
        return "undefined";
      case TypeCode.Object:
        if (value is Missing || value is System.Reflection.Missing)
          return "undefined";
        return checkForDebuggerObject && value is IDebuggerObject debuggerObject ? (!debuggerObject.IsScriptFunction() ? "object" : "function") : (!(value is ScriptFunction) ? "object" : "function");
      case TypeCode.DBNull:
        return "object";
      case TypeCode.Boolean:
        return "boolean";
      case TypeCode.Char:
      case TypeCode.String:
        return "string";
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
      case TypeCode.UInt32:
      case TypeCode.Int64:
      case TypeCode.UInt64:
      case TypeCode.Single:
      case TypeCode.Double:
      case TypeCode.Decimal:
        return "number";
      case TypeCode.DateTime:
        return "date";
      default:
        return "unknown";
    }
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    if (this.operand is Binding)
      ((Binding) this.operand).TranslateToIL(il, Typeob.Object, true);
    else
      this.operand.TranslateToIL(il, Typeob.Object);
    il.Emit(OpCodes.Call, CompilerGlobals.jScriptTypeofMethod);
    Convert.Emit((AST) this, il, Typeob.String, rtype);
  }
}
