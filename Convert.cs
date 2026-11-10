// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Convert
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Converts data from one type to another. This is a utility class.</summary>
public sealed class Convert
{
  private static bool[,] promotable = new bool[19, 19]
  {
    {
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true
    },
    {
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false
    },
    {
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true
    },
    {
      false,
      false,
      false,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      false
    },
    {
      false,
      false,
      false,
      false,
      true,
      false,
      false,
      false,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      false
    },
    {
      false,
      false,
      false,
      false,
      false,
      true,
      false,
      true,
      false,
      true,
      false,
      true,
      false,
      true,
      true,
      true,
      true,
      true,
      false
    },
    {
      false,
      false,
      false,
      false,
      true,
      false,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      false
    },
    {
      false,
      false,
      false,
      false,
      false,
      true,
      false,
      true,
      false,
      true,
      false,
      true,
      false,
      true,
      true,
      true,
      true,
      true,
      false
    },
    {
      false,
      false,
      false,
      false,
      true,
      false,
      false,
      false,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      true,
      false
    },
    {
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      true,
      false,
      true,
      false,
      false,
      true,
      true,
      true,
      true,
      false
    },
    {
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      true,
      true,
      true,
      false,
      true,
      true,
      true,
      true,
      false
    },
    {
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      true,
      false,
      false,
      false,
      true,
      true,
      true,
      false
    },
    {
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      true,
      false,
      false,
      true,
      true,
      true,
      false
    },
    {
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      true,
      true,
      true,
      false,
      false,
      false
    },
    {
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      true,
      true,
      false,
      false,
      false
    },
    {
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      true,
      false,
      false,
      false
    },
    {
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      true,
      false,
      false
    },
    {
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      true,
      false
    },
    {
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      false,
      true
    }
  };
  private static int[] rgcchSig = new int[35]
  {
    53,
    34,
    27,
    24,
    22,
    20,
    19,
    18,
    17,
    17,
    16 /*0x10*/,
    16 /*0x10*/,
    15,
    15,
    14,
    14,
    14,
    14,
    14,
    13,
    13,
    13,
    13,
    13,
    13,
    12,
    12,
    12,
    12,
    12,
    12,
    12,
    12,
    12,
    12
  };

  /// <summary>Determines whether a variable contains an array index value.</summary>
  /// <returns>true if <paramref name="ast" /> contains an array index value; otherwise, false.</returns>
  /// <param name="ast">The variable to test.</param>
  public static bool IsBadIndex(AST ast)
  {
    if (!(ast is ConstantWrapper))
      return false;
    int num;
    try
    {
      num = (int) Convert.CoerceT(((ConstantWrapper) ast).value, typeof (int));
    }
    catch
    {
      return true;
    }
    return num < 0;
  }

  /// <summary>Determines whether a <see cref="T:System.Double" /> variable contains an integer value.</summary>
  /// <returns>If <paramref name="d" /> contains an integer value, <paramref name="d" /> is returned. If not, an exception is thrown.</returns>
  /// <param name="d">The <see cref="T:System.Double" /> to test.</param>
  public static double CheckIfDoubleIsInteger(double d)
  {
    return d == Math.Round(d) ? d : throw new JScriptException(JSError.TypeMismatch);
  }

  /// <summary>Determines whether a <see cref="T:System.Single" /> variable contains an integer value.</summary>
  /// <returns>If <paramref name="s" /> contains an integer value, <paramref name="s" /> is returned. If not, an exception is thrown.</returns>
  /// <param name="s">The <see cref="T:System.Single" /> to test.</param>
  public static float CheckIfSingleIsInteger(float s)
  {
    return (double) s == Math.Round((double) s) ? s : throw new JScriptException(JSError.TypeMismatch);
  }

  /// <summary>Converts the specified value to the specified type.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to <paramref name="type" />.</returns>
  /// <param name="value">The value to convert to a new type.</param>
  /// <param name="type">The type to convert <paramref name="value" /> to.</param>
  public static object Coerce(object value, object type) => Convert.Coerce(value, type, false);

  internal static object Coerce(object value, object type, bool explicitOK)
  {
    if (type is TypeExpression typeExpression)
      type = (object) typeExpression.ToIReflect();
    if (type is TypedArray typedArray)
    {
      IReflect elementType1 = typedArray.elementType;
      int rank = typedArray.rank;
      Type elementType2 = (object) (elementType1 as Type) != null ? (Type) elementType1 : (elementType1 is ClassScope ? ((ClassScope) elementType1).GetBakedSuperType() : typeof (object));
      if (value is ArrayObject arrayObject)
        return (object) arrayObject.ToNativeArray(elementType2);
      if (value is Array array && array.Rank == rank)
        type = (object) Convert.ToType(TypedArray.ToRankString(rank), elementType2);
      if (value == null || value is DBNull)
        return (object) null;
    }
    if (type is ClassScope classScope)
    {
      if (classScope.HasInstance(value))
        return value;
      if (classScope.owner is EnumDeclaration owner)
      {
        if (!(value is EnumWrapper enumWrapper))
          return (object) new DeclaredEnumValue(Convert.Coerce(value, (object) owner.baseType), (string) null, classScope);
        if (enumWrapper.classScopeOrType == classScope)
          return value;
        throw new JScriptException(JSError.TypeMismatch);
      }
      switch (value)
      {
        case null:
        case DBNull _:
          return (object) null;
        default:
          throw new JScriptException(JSError.TypeMismatch);
      }
    }
    else
    {
      if ((object) (type as Type) == null)
      {
        type = (object) Convert.ToType(Microsoft.JScript.Runtime.TypeRefs, (IReflect) type);
      }
      else
      {
        if (type == (object) typeof (Type) && value is ClassScope)
          return value;
        if (((Type) type).IsEnum)
        {
          if (value is EnumWrapper enumWrapper)
          {
            if (enumWrapper.classScopeOrType == type)
              return value;
            throw new JScriptException(JSError.TypeMismatch);
          }
          Type type1 = type as Type;
          return MetadataEnumValue.GetEnumValue(type1, Convert.CoerceT(value, Convert.GetUnderlyingType(type1), explicitOK));
        }
      }
      return Convert.CoerceT(value, (Type) type, explicitOK);
    }
  }

  internal static object CoerceT(object value, Type type) => Convert.CoerceT(value, type, false);

  /// <summary>Converts the specified value to the specified <see cref="T:System.Type" />, optionally allowing data loss.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to the type <paramref name="t" />.</returns>
  /// <param name="value">The value to convert to a new type.</param>
  /// <param name="t">The type to convert <paramref name="value" /> to.</param>
  /// <param name="explicitOK">true to allow data loss; otherwise, false.</param>
  public static object CoerceT(object value, Type t, bool explicitOK)
  {
    if (t == typeof (object) || t == typeof (string) && value is string)
      return value;
    if (t.IsEnum)
    {
      switch (t)
      {
        case EnumBuilder _:
        case TypeBuilder _:
          break;
        default:
          IConvertible iconvertible = Convert.GetIConvertible(value);
          TypeCode typeCode1 = Convert.GetTypeCode(value, iconvertible);
          if (typeCode1 == TypeCode.String)
            return Enum.Parse(t, iconvertible.ToString((IFormatProvider) CultureInfo.InvariantCulture));
          if (!explicitOK && typeCode1 != TypeCode.Empty)
          {
            Type type = value.GetType();
            if (type.IsEnum)
            {
              if (type != t)
                throw new JScriptException(JSError.TypeMismatch);
              return value;
            }
          }
          return Enum.ToObject(t, Convert.CoerceT(value, Convert.GetUnderlyingType(t), explicitOK));
      }
    }
    TypeCode typeCode2 = Type.GetTypeCode(t);
    if (typeCode2 != TypeCode.Object)
      return Convert.Coerce2(value, typeCode2, explicitOK);
    if (value is ConcatString)
      value = (object) value.ToString();
    if (value == null || value == DBNull.Value && t != typeof (object) || value is Missing || value is System.Reflection.Missing)
    {
      if (!t.IsValueType)
        return (object) null;
      if (!t.IsPublic && t.Assembly == typeof (ActiveXObjectConstructor).Assembly)
        throw new JScriptException(JSError.CantCreateObject);
      return Activator.CreateInstance(t);
    }
    if (t.IsAssignableFrom(value.GetType()))
      return value;
    if (typeof (Delegate).IsAssignableFrom(t))
    {
      switch (value)
      {
        case Closure _:
          return (object) ((Closure) value).ConvertToDelegate(t);
        case FunctionWrapper _:
          return (object) ((FunctionWrapper) value).ConvertToDelegate(t);
        case FunctionObject _:
          return value;
      }
    }
    else
    {
      switch (value)
      {
        case ArrayObject _ when typeof (Array).IsAssignableFrom(t):
          return (object) ((ArrayObject) value).ToNativeArray(t.GetElementType());
        case Array _ when t == typeof (ArrayObject) && ((Array) value).Rank == 1:
          if (Globals.contextEngine == null)
          {
            Globals.contextEngine = new VsaEngine(true);
            Globals.contextEngine.InitVsaEngine("JS7://Microsoft.JScript.Vsa.VsaEngine", (IJSVsaSite) new DefaultVsaSite());
          }
          return (object) Globals.contextEngine.GetOriginalArrayConstructor().ConstructWrapper((Array) value);
        case ClassScope _ when t == typeof (Type):
          return (object) ((ClassScope) value).GetTypeBuilderOrEnumBuilder();
        case TypedArray _ when t == typeof (Type):
          return (object) ((TypedArray) value).ToType();
      }
    }
    Type type1 = value.GetType();
    if (explicitOK)
    {
      MethodInfo method = t.GetMethod("op_Explicit", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder) null, new Type[1]
      {
        type1
      }, (ParameterModifier[]) null);
      if (method != (MethodInfo) null && (method.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope)
        return new JSMethodInfo(method).Invoke((object) null, BindingFlags.SuppressChangeType, (Binder) null, new object[1]
        {
          value
        }, (CultureInfo) null);
      MethodInfo toXxxxMethod = Convert.GetToXXXXMethod((IReflect) type1, t, explicitOK);
      if (toXxxxMethod != (MethodInfo) null && (toXxxxMethod.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope)
      {
        MethodInfo methodInfo = (MethodInfo) new JSMethodInfo(toXxxxMethod);
        if (!methodInfo.IsStatic)
          return methodInfo.Invoke(value, BindingFlags.SuppressChangeType, (Binder) null, new object[0], (CultureInfo) null);
        return methodInfo.Invoke((object) null, BindingFlags.SuppressChangeType, (Binder) null, new object[1]
        {
          value
        }, (CultureInfo) null);
      }
    }
    MethodInfo method1 = t.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder) null, new Type[1]
    {
      type1
    }, (ParameterModifier[]) null);
    if (method1 != (MethodInfo) null && (method1.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope)
      return new JSMethodInfo(method1).Invoke((object) null, BindingFlags.SuppressChangeType, (Binder) null, new object[1]
      {
        value
      }, (CultureInfo) null);
    MethodInfo toXxxxMethod1 = Convert.GetToXXXXMethod((IReflect) type1, t, false);
    if (toXxxxMethod1 != (MethodInfo) null && (toXxxxMethod1.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope)
    {
      MethodInfo methodInfo = (MethodInfo) new JSMethodInfo(toXxxxMethod1);
      if (!methodInfo.IsStatic)
        return methodInfo.Invoke(value, BindingFlags.SuppressChangeType, (Binder) null, new object[0], (CultureInfo) null);
      return methodInfo.Invoke((object) null, BindingFlags.SuppressChangeType, (Binder) null, new object[1]
      {
        value
      }, (CultureInfo) null);
    }
    if (t.IsByRef)
      return Convert.CoerceT(value, t.GetElementType());
    return value.GetType().IsCOMObject ? value : throw new JScriptException(JSError.TypeMismatch);
  }

  /// <summary>Converts the specified value to the specified <see cref="T:System.TypeCode" />, optionally allowing data loss.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to the <paramref name="target" /> type.</returns>
  /// <param name="value">The value to convert to a new type.</param>
  /// <param name="target">The type to convert <paramref name="value" /> to.</param>
  /// <param name="truncationPermitted">true to allow data loss; otherwise, false.</param>
  public static object Coerce2(object value, TypeCode target, bool truncationPermitted)
  {
    return truncationPermitted ? Convert.Coerce2WithTruncationPermitted(value, target) : Convert.Coerce2WithNoTrunctation(value, target);
  }

  private static object Coerce2WithNoTrunctation(object value, TypeCode target)
  {
    if (value is EnumWrapper)
      value = ((EnumWrapper) value).value;
    if (value is ConstantWrapper)
      value = ((ConstantWrapper) value).value;
    try
    {
      IConvertible iconvertible = Convert.GetIConvertible(value);
      switch (Convert.GetTypeCode(value, iconvertible))
      {
        case TypeCode.Empty:
          switch (target)
          {
            case TypeCode.DBNull:
              return (object) DBNull.Value;
            case TypeCode.Boolean:
              return (object) false;
            case TypeCode.Char:
              return (object) char.MinValue;
            case TypeCode.SByte:
              return (object) (sbyte) 0;
            case TypeCode.Byte:
              return (object) (byte) 0;
            case TypeCode.Int16:
              return (object) (short) 0;
            case TypeCode.UInt16:
              return (object) (ushort) 0;
            case TypeCode.Int32:
              return (object) 0;
            case TypeCode.UInt32:
              return (object) 0U;
            case TypeCode.Int64:
              return (object) 0L;
            case TypeCode.UInt64:
              return (object) 0UL;
            case TypeCode.Single:
              return (object) float.NaN;
            case TypeCode.Double:
              return (object) double.NaN;
            case TypeCode.Decimal:
              return (object) 0M;
            case TypeCode.DateTime:
              return (object) new DateTime(0L);
            case TypeCode.String:
              return (object) null;
          }
          break;
        case TypeCode.Object:
          if (!(value is System.Reflection.Missing) && (!(value is Missing) || target == TypeCode.Object))
          {
            switch (target)
            {
              case TypeCode.Boolean:
                return (object) Convert.ToBoolean(value, false);
              case TypeCode.Char:
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
                return Convert.Coerce2WithNoTrunctation((object) Convert.ToNumber(value, iconvertible), target);
              case TypeCode.DateTime:
                return value is DateObject ? DatePrototype.getVarDate((object) (DateObject) value) : Convert.Coerce2WithNoTrunctation((object) Convert.ToNumber(value, iconvertible), target);
              case TypeCode.String:
                return (object) Convert.ToString(value, iconvertible);
            }
          }
          else
            goto case TypeCode.Empty;
          break;
        case TypeCode.DBNull:
          switch (target)
          {
            case TypeCode.DBNull:
              return (object) DBNull.Value;
            case TypeCode.Boolean:
              return (object) false;
            case TypeCode.Char:
              return (object) char.MinValue;
            case TypeCode.SByte:
              return (object) (sbyte) 0;
            case TypeCode.Byte:
              return (object) (byte) 0;
            case TypeCode.Int16:
              return (object) (short) 0;
            case TypeCode.UInt16:
              return (object) (ushort) 0;
            case TypeCode.Int32:
              return (object) 0;
            case TypeCode.UInt32:
              return (object) 0U;
            case TypeCode.Int64:
              return (object) 0L;
            case TypeCode.UInt64:
              return (object) 0UL;
            case TypeCode.Single:
              return (object) 0.0f;
            case TypeCode.Double:
              return (object) 0.0;
            case TypeCode.Decimal:
              return (object) 0M;
            case TypeCode.DateTime:
              return (object) new DateTime(0L);
            case TypeCode.String:
              return (object) null;
          }
          break;
        case TypeCode.Boolean:
          bool boolean = iconvertible.ToBoolean((IFormatProvider) null);
          int ticks1 = boolean ? 1 : 0;
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) boolean;
            case TypeCode.Char:
              return (object) checked ((char) ticks1);
            case TypeCode.SByte:
              return (object) checked ((sbyte) ticks1);
            case TypeCode.Byte:
              return (object) checked ((byte) ticks1);
            case TypeCode.Int16:
              return (object) checked ((short) ticks1);
            case TypeCode.UInt16:
              return (object) checked ((ushort) ticks1);
            case TypeCode.Int32:
              return (object) ticks1;
            case TypeCode.UInt32:
              return (object) checked ((uint) ticks1);
            case TypeCode.Int64:
              return (object) (long) ticks1;
            case TypeCode.UInt64:
              return (object) checked ((ulong) ticks1);
            case TypeCode.Single:
              return (object) (float) ticks1;
            case TypeCode.Double:
              return (object) (double) ticks1;
            case TypeCode.Decimal:
              return (object) (Decimal) ticks1;
            case TypeCode.DateTime:
              return (object) new DateTime((long) ticks1);
            case TypeCode.String:
              return boolean ? (object) "true" : (object) "false";
          }
          break;
        case TypeCode.Char:
          char c = iconvertible.ToChar((IFormatProvider) null);
          ushort ticks2 = (ushort) c;
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) (ticks2 > (ushort) 0);
            case TypeCode.Char:
              return (object) c;
            case TypeCode.SByte:
              return (object) checked ((sbyte) ticks2);
            case TypeCode.Byte:
              return (object) checked ((byte) ticks2);
            case TypeCode.Int16:
              return (object) checked ((short) ticks2);
            case TypeCode.UInt16:
              return (object) ticks2;
            case TypeCode.Int32:
              return (object) (int) ticks2;
            case TypeCode.UInt32:
              return (object) (uint) ticks2;
            case TypeCode.Int64:
              return (object) (long) ticks2;
            case TypeCode.UInt64:
              return (object) (ulong) ticks2;
            case TypeCode.Single:
              return (object) (float) ticks2;
            case TypeCode.Double:
              return (object) (double) ticks2;
            case TypeCode.Decimal:
              return (object) (Decimal) ticks2;
            case TypeCode.DateTime:
              return (object) new DateTime((long) ticks2);
            case TypeCode.String:
              return (object) char.ToString(c);
          }
          break;
        case TypeCode.SByte:
          sbyte ticks3 = iconvertible.ToSByte((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) (ticks3 != (sbyte) 0);
            case TypeCode.Char:
              return (object) checked ((char) ticks3);
            case TypeCode.SByte:
              return (object) ticks3;
            case TypeCode.Byte:
              return (object) checked ((byte) ticks3);
            case TypeCode.Int16:
              return (object) (short) ticks3;
            case TypeCode.UInt16:
              return (object) checked ((ushort) ticks3);
            case TypeCode.Int32:
              return (object) (int) ticks3;
            case TypeCode.UInt32:
              return (object) checked ((uint) ticks3);
            case TypeCode.Int64:
              return (object) (long) ticks3;
            case TypeCode.UInt64:
              return (object) checked ((ulong) ticks3);
            case TypeCode.Single:
              return (object) (float) ticks3;
            case TypeCode.Double:
              return (object) (double) ticks3;
            case TypeCode.Decimal:
              return (object) (Decimal) ticks3;
            case TypeCode.DateTime:
              return (object) new DateTime((long) ticks3);
            case TypeCode.String:
              return (object) ticks3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          break;
        case TypeCode.Byte:
          byte ticks4 = iconvertible.ToByte((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) (ticks4 > (byte) 0);
            case TypeCode.Char:
              return (object) (char) ticks4;
            case TypeCode.SByte:
              return (object) checked ((sbyte) ticks4);
            case TypeCode.Byte:
              return (object) ticks4;
            case TypeCode.Int16:
              return (object) (short) ticks4;
            case TypeCode.UInt16:
              return (object) (ushort) ticks4;
            case TypeCode.Int32:
              return (object) (int) ticks4;
            case TypeCode.UInt32:
              return (object) (uint) ticks4;
            case TypeCode.Int64:
              return (object) (long) ticks4;
            case TypeCode.UInt64:
              return (object) (ulong) ticks4;
            case TypeCode.Single:
              return (object) (float) ticks4;
            case TypeCode.Double:
              return (object) (double) ticks4;
            case TypeCode.Decimal:
              return (object) (Decimal) ticks4;
            case TypeCode.DateTime:
              return (object) new DateTime((long) ticks4);
            case TypeCode.String:
              return (object) ticks4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          break;
        case TypeCode.Int16:
          short int16 = iconvertible.ToInt16((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) (int16 != (short) 0);
            case TypeCode.Char:
              return (object) checked ((char) int16);
            case TypeCode.SByte:
              return (object) checked ((sbyte) int16);
            case TypeCode.Byte:
              return (object) checked ((byte) int16);
            case TypeCode.Int16:
              return (object) int16;
            case TypeCode.UInt16:
              return (object) checked ((ushort) int16);
            case TypeCode.Int32:
              return (object) (int) int16;
            case TypeCode.UInt32:
              return (object) checked ((uint) int16);
            case TypeCode.Int64:
              return (object) (long) int16;
            case TypeCode.UInt64:
              return (object) checked ((ulong) int16);
            case TypeCode.Single:
              return (object) (float) int16;
            case TypeCode.Double:
              return (object) (double) int16;
            case TypeCode.Decimal:
              return (object) (Decimal) int16;
            case TypeCode.DateTime:
              return (object) new DateTime((long) int16);
            case TypeCode.String:
              return (object) int16.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          break;
        case TypeCode.UInt16:
          ushort uint16 = iconvertible.ToUInt16((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) (uint16 > (ushort) 0);
            case TypeCode.Char:
              return (object) (char) uint16;
            case TypeCode.SByte:
              return (object) checked ((sbyte) uint16);
            case TypeCode.Byte:
              return (object) checked ((byte) uint16);
            case TypeCode.Int16:
              return (object) checked ((short) uint16);
            case TypeCode.UInt16:
              return (object) uint16;
            case TypeCode.Int32:
              return (object) (int) uint16;
            case TypeCode.UInt32:
              return (object) (uint) uint16;
            case TypeCode.Int64:
              return (object) (long) uint16;
            case TypeCode.UInt64:
              return (object) (ulong) uint16;
            case TypeCode.Single:
              return (object) (float) uint16;
            case TypeCode.Double:
              return (object) (double) uint16;
            case TypeCode.Decimal:
              return (object) (Decimal) uint16;
            case TypeCode.DateTime:
              return (object) new DateTime((long) uint16);
            case TypeCode.String:
              return (object) uint16.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          break;
        case TypeCode.Int32:
          int int32 = iconvertible.ToInt32((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) (int32 != 0);
            case TypeCode.Char:
              return (object) checked ((char) int32);
            case TypeCode.SByte:
              return (object) checked ((sbyte) int32);
            case TypeCode.Byte:
              return (object) checked ((byte) int32);
            case TypeCode.Int16:
              return (object) checked ((short) int32);
            case TypeCode.UInt16:
              return (object) checked ((ushort) int32);
            case TypeCode.Int32:
              return (object) int32;
            case TypeCode.UInt32:
              return (object) checked ((uint) int32);
            case TypeCode.Int64:
              return (object) (long) int32;
            case TypeCode.UInt64:
              return (object) checked ((ulong) int32);
            case TypeCode.Single:
              return (object) (float) int32;
            case TypeCode.Double:
              return (object) (double) int32;
            case TypeCode.Decimal:
              return (object) (Decimal) int32;
            case TypeCode.DateTime:
              return (object) new DateTime((long) int32);
            case TypeCode.String:
              return (object) int32.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          break;
        case TypeCode.UInt32:
          uint uint32 = iconvertible.ToUInt32((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) (uint32 > 0U);
            case TypeCode.Char:
              return (object) checked ((char) uint32);
            case TypeCode.SByte:
              return (object) checked ((sbyte) uint32);
            case TypeCode.Byte:
              return (object) checked ((byte) uint32);
            case TypeCode.Int16:
              return (object) checked ((short) uint32);
            case TypeCode.UInt16:
              return (object) checked ((ushort) uint32);
            case TypeCode.Int32:
              return (object) checked ((int) uint32);
            case TypeCode.UInt32:
              return (object) uint32;
            case TypeCode.Int64:
              return (object) (long) uint32;
            case TypeCode.UInt64:
              return (object) (ulong) uint32;
            case TypeCode.Single:
              return (object) (float) uint32;
            case TypeCode.Double:
              return (object) (double) uint32;
            case TypeCode.Decimal:
              return (object) (Decimal) uint32;
            case TypeCode.DateTime:
              return (object) new DateTime((long) uint32);
            case TypeCode.String:
              return (object) uint32.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          break;
        case TypeCode.Int64:
          long int64 = iconvertible.ToInt64((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) (int64 != 0L);
            case TypeCode.Char:
              return (object) checked ((char) int64);
            case TypeCode.SByte:
              return (object) checked ((sbyte) int64);
            case TypeCode.Byte:
              return (object) checked ((byte) int64);
            case TypeCode.Int16:
              return (object) checked ((short) int64);
            case TypeCode.UInt16:
              return (object) checked ((ushort) int64);
            case TypeCode.Int32:
              return (object) checked ((int) int64);
            case TypeCode.UInt32:
              return (object) checked ((uint) int64);
            case TypeCode.Int64:
              return (object) int64;
            case TypeCode.UInt64:
              return (object) checked ((ulong) int64);
            case TypeCode.Single:
              return (object) (float) int64;
            case TypeCode.Double:
              return (object) (double) int64;
            case TypeCode.Decimal:
              return (object) (Decimal) int64;
            case TypeCode.DateTime:
              return (object) new DateTime(int64);
            case TypeCode.String:
              return (object) int64.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          break;
        case TypeCode.UInt64:
          ulong uint64 = iconvertible.ToUInt64((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) (uint64 > 0UL);
            case TypeCode.Char:
              return (object) checked ((char) uint64);
            case TypeCode.SByte:
              return (object) checked ((sbyte) uint64);
            case TypeCode.Byte:
              return (object) checked ((byte) uint64);
            case TypeCode.Int16:
              return (object) checked ((short) uint64);
            case TypeCode.UInt16:
              return (object) checked ((ushort) uint64);
            case TypeCode.Int32:
              return (object) checked ((int) uint64);
            case TypeCode.UInt32:
              return (object) checked ((uint) uint64);
            case TypeCode.Int64:
              return (object) checked ((long) uint64);
            case TypeCode.UInt64:
              return (object) uint64;
            case TypeCode.Single:
              return (object) (float) uint64;
            case TypeCode.Double:
              return (object) (double) uint64;
            case TypeCode.Decimal:
              return (object) (Decimal) uint64;
            case TypeCode.DateTime:
              return (object) new DateTime(checked ((long) uint64));
            case TypeCode.String:
              return (object) uint64.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          break;
        case TypeCode.Single:
          float single = iconvertible.ToSingle((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
              return (double) single != (double) single ? (object) false : (object) ((double) single != 0.0);
            case TypeCode.Single:
              return (object) single;
            case TypeCode.Double:
              return (object) (double) single;
            case TypeCode.Decimal:
              return (object) (Decimal) single;
            case TypeCode.String:
              return (object) Convert.ToString((double) single);
            default:
              if (Math.Round((double) single) == (double) single)
              {
                switch (target)
                {
                  case TypeCode.Char:
                    return (object) checked ((char) single);
                  case TypeCode.SByte:
                    return (object) checked ((sbyte) single);
                  case TypeCode.Byte:
                    return (object) checked ((byte) single);
                  case TypeCode.Int16:
                    return (object) checked ((short) single);
                  case TypeCode.UInt16:
                    return (object) checked ((ushort) single);
                  case TypeCode.Int32:
                    return (object) checked ((int) single);
                  case TypeCode.UInt32:
                    return (object) checked ((uint) single);
                  case TypeCode.Int64:
                    return (object) checked ((long) single);
                  case TypeCode.UInt64:
                    return (object) checked ((ulong) single);
                  case TypeCode.DateTime:
                    return (object) new DateTime(checked ((long) single));
                }
              }
              else
                break;
              break;
          }
          break;
        case TypeCode.Double:
          double num = iconvertible.ToDouble((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) Convert.ToBoolean(num);
            case TypeCode.Single:
              return (object) (float) num;
            case TypeCode.Double:
              return (object) num;
            case TypeCode.Decimal:
              return (object) (Decimal) num;
            case TypeCode.String:
              return (object) Convert.ToString(num);
            default:
              if (Math.Round(num) == num)
              {
                switch (target)
                {
                  case TypeCode.Char:
                    return (object) checked ((char) num);
                  case TypeCode.SByte:
                    return (object) checked ((sbyte) num);
                  case TypeCode.Byte:
                    return (object) checked ((byte) num);
                  case TypeCode.Int16:
                    return (object) checked ((short) num);
                  case TypeCode.UInt16:
                    return (object) checked ((ushort) num);
                  case TypeCode.Int32:
                    return (object) checked ((int) num);
                  case TypeCode.UInt32:
                    return (object) checked ((uint) num);
                  case TypeCode.Int64:
                    return (object) checked ((long) num);
                  case TypeCode.UInt64:
                    return (object) checked ((ulong) num);
                  case TypeCode.DateTime:
                    return (object) new DateTime(checked ((long) num));
                }
              }
              else
                break;
              break;
          }
          break;
        case TypeCode.Decimal:
          Decimal d = iconvertible.ToDecimal((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) (d != 0M);
            case TypeCode.Char:
              return (object) (char) Decimal.ToUInt16(d);
            case TypeCode.SByte:
              return (object) Decimal.ToSByte(d);
            case TypeCode.Byte:
              return (object) Decimal.ToByte(d);
            case TypeCode.Int16:
              return (object) Decimal.ToInt16(d);
            case TypeCode.UInt16:
              return (object) Decimal.ToUInt16(d);
            case TypeCode.Int32:
              return (object) Decimal.ToInt32(d);
            case TypeCode.UInt32:
              return (object) Decimal.ToUInt32(d);
            case TypeCode.Int64:
              return (object) Decimal.ToInt64(d);
            case TypeCode.UInt64:
              return (object) Decimal.ToUInt64(d);
            case TypeCode.Single:
              return (object) Decimal.ToSingle(d);
            case TypeCode.Double:
              return (object) Decimal.ToDouble(d);
            case TypeCode.Decimal:
              return (object) d;
            case TypeCode.DateTime:
              return (object) new DateTime(Decimal.ToInt64(d));
            case TypeCode.String:
              return (object) d.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          break;
        case TypeCode.DateTime:
          DateTime dateTime = iconvertible.ToDateTime((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
            case TypeCode.Char:
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
              return Convert.Coerce2WithNoTrunctation((object) dateTime.Ticks, target);
            case TypeCode.DateTime:
              return (object) dateTime;
            case TypeCode.String:
              return (object) dateTime.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          }
          break;
        case TypeCode.String:
          string str = iconvertible.ToString((IFormatProvider) null);
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) Convert.ToBoolean((object) str, false);
            case TypeCode.Char:
              return str.Length == 1 ? (object) str[0] : throw new JScriptException(JSError.TypeMismatch);
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Double:
              return Convert.Coerce2WithNoTrunctation((object) Convert.ToNumber(str), target);
            case TypeCode.Int64:
              try
              {
                return (object) long.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture);
              }
              catch
              {
                goto case TypeCode.SByte;
              }
            case TypeCode.UInt64:
              try
              {
                return (object) ulong.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture);
              }
              catch
              {
                goto case TypeCode.SByte;
              }
            case TypeCode.Single:
              try
              {
                return (object) float.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture);
              }
              catch
              {
                goto case TypeCode.SByte;
              }
            case TypeCode.Decimal:
              try
              {
                return (object) Decimal.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture);
              }
              catch
              {
                goto case TypeCode.SByte;
              }
            case TypeCode.DateTime:
              try
              {
                return (object) DateTime.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture);
              }
              catch
              {
                return DatePrototype.getVarDate((object) DateConstructor.ob.CreateInstance(new object[1]
                {
                  (object) DatePrototype.ParseDate(str)
                }));
              }
            case TypeCode.String:
              return (object) str;
          }
          break;
      }
    }
    catch (OverflowException ex)
    {
    }
    throw new JScriptException(JSError.TypeMismatch);
  }

  private static object Coerce2WithTruncationPermitted(object value, TypeCode target)
  {
    if (value is EnumWrapper)
      value = ((EnumWrapper) value).value;
    if (value is ConstantWrapper)
      value = ((ConstantWrapper) value).value;
    IConvertible iconvertible = Convert.GetIConvertible(value);
    switch (Convert.GetTypeCode(value, iconvertible))
    {
      case TypeCode.Empty:
        switch (target)
        {
          case TypeCode.DBNull:
            return (object) DBNull.Value;
          case TypeCode.Boolean:
            return (object) false;
          case TypeCode.Char:
            return (object) char.MinValue;
          case TypeCode.SByte:
            return (object) (sbyte) 0;
          case TypeCode.Byte:
            return (object) (byte) 0;
          case TypeCode.Int16:
            return (object) (short) 0;
          case TypeCode.UInt16:
            return (object) (ushort) 0;
          case TypeCode.Int32:
            return (object) 0;
          case TypeCode.UInt32:
            return (object) 0U;
          case TypeCode.Int64:
            return (object) 0L;
          case TypeCode.UInt64:
            return (object) 0UL;
          case TypeCode.Single:
            return (object) float.NaN;
          case TypeCode.Double:
            return (object) double.NaN;
          case TypeCode.Decimal:
            return (object) 0M;
          case TypeCode.DateTime:
            return (object) new DateTime(0L);
          case TypeCode.String:
            return (object) "undefined";
        }
        break;
      case TypeCode.Object:
        if (!(value is System.Reflection.Missing) && (!(value is Missing) || target == TypeCode.Object))
        {
          switch (target)
          {
            case TypeCode.Boolean:
              return (object) Convert.ToBoolean(value, iconvertible);
            case TypeCode.Char:
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
              return Convert.Coerce2WithTruncationPermitted((object) Convert.ToNumber(value, iconvertible), target);
            case TypeCode.DateTime:
              return value is DateObject ? DatePrototype.getVarDate((object) (DateObject) value) : Convert.Coerce2WithTruncationPermitted((object) Convert.ToNumber(value, iconvertible), target);
            case TypeCode.String:
              return (object) Convert.ToString(value, iconvertible);
          }
        }
        else
          goto case TypeCode.Empty;
        break;
      case TypeCode.DBNull:
        switch (target)
        {
          case TypeCode.DBNull:
            return (object) DBNull.Value;
          case TypeCode.Boolean:
            return (object) false;
          case TypeCode.Char:
            return (object) char.MinValue;
          case TypeCode.SByte:
            return (object) (sbyte) 0;
          case TypeCode.Byte:
            return (object) (byte) 0;
          case TypeCode.Int16:
            return (object) (short) 0;
          case TypeCode.UInt16:
            return (object) (ushort) 0;
          case TypeCode.Int32:
            return (object) 0;
          case TypeCode.UInt32:
            return (object) 0U;
          case TypeCode.Int64:
            return (object) 0L;
          case TypeCode.UInt64:
            return (object) 0UL;
          case TypeCode.Single:
            return (object) 0.0f;
          case TypeCode.Double:
            return (object) 0.0;
          case TypeCode.Decimal:
            return (object) 0M;
          case TypeCode.DateTime:
            return (object) new DateTime(0L);
          case TypeCode.String:
            return (object) "null";
        }
        break;
      case TypeCode.Boolean:
        bool boolean = iconvertible.ToBoolean((IFormatProvider) null);
        int ticks1 = boolean ? 1 : 0;
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) boolean;
          case TypeCode.Char:
            return (object) (char) ticks1;
          case TypeCode.SByte:
            return (object) (sbyte) ticks1;
          case TypeCode.Byte:
            return (object) (byte) ticks1;
          case TypeCode.Int16:
            return (object) (short) ticks1;
          case TypeCode.UInt16:
            return (object) (ushort) ticks1;
          case TypeCode.Int32:
            return (object) ticks1;
          case TypeCode.UInt32:
            return (object) (uint) ticks1;
          case TypeCode.Int64:
            return (object) (long) ticks1;
          case TypeCode.UInt64:
            return (object) (ulong) ticks1;
          case TypeCode.Single:
            return (object) (float) ticks1;
          case TypeCode.Double:
            return (object) (double) ticks1;
          case TypeCode.Decimal:
            return (object) (Decimal) ticks1;
          case TypeCode.DateTime:
            return (object) new DateTime((long) ticks1);
          case TypeCode.String:
            return !boolean ? (object) "false" : (object) "true";
        }
        break;
      case TypeCode.Char:
        char c = iconvertible.ToChar((IFormatProvider) null);
        ushort ticks2 = (ushort) c;
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) (ticks2 > (ushort) 0);
          case TypeCode.Char:
            return (object) c;
          case TypeCode.SByte:
            return (object) (sbyte) ticks2;
          case TypeCode.Byte:
            return (object) (byte) ticks2;
          case TypeCode.Int16:
            return (object) (short) ticks2;
          case TypeCode.UInt16:
            return (object) ticks2;
          case TypeCode.Int32:
            return (object) (int) ticks2;
          case TypeCode.UInt32:
            return (object) (uint) ticks2;
          case TypeCode.Int64:
            return (object) (long) ticks2;
          case TypeCode.UInt64:
            return (object) (ulong) ticks2;
          case TypeCode.Single:
            return (object) (float) ticks2;
          case TypeCode.Double:
            return (object) (double) ticks2;
          case TypeCode.Decimal:
            return (object) (Decimal) ticks2;
          case TypeCode.DateTime:
            return (object) new DateTime((long) ticks2);
          case TypeCode.String:
            return (object) char.ToString(c);
        }
        break;
      case TypeCode.SByte:
        sbyte ticks3 = iconvertible.ToSByte((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) (ticks3 != (sbyte) 0);
          case TypeCode.Char:
            return (object) (char) ticks3;
          case TypeCode.SByte:
            return (object) ticks3;
          case TypeCode.Byte:
            return (object) (byte) ticks3;
          case TypeCode.Int16:
            return (object) (short) ticks3;
          case TypeCode.UInt16:
            return (object) (ushort) ticks3;
          case TypeCode.Int32:
            return (object) (int) ticks3;
          case TypeCode.UInt32:
            return (object) (uint) ticks3;
          case TypeCode.Int64:
            return (object) (long) ticks3;
          case TypeCode.UInt64:
            return (object) (ulong) ticks3;
          case TypeCode.Single:
            return (object) (float) ticks3;
          case TypeCode.Double:
            return (object) (double) ticks3;
          case TypeCode.Decimal:
            return (object) (Decimal) ticks3;
          case TypeCode.DateTime:
            return (object) new DateTime((long) ticks3);
          case TypeCode.String:
            return (object) ticks3.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        break;
      case TypeCode.Byte:
        byte ticks4 = iconvertible.ToByte((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) (ticks4 > (byte) 0);
          case TypeCode.Char:
            return (object) (char) ticks4;
          case TypeCode.SByte:
            return (object) (sbyte) ticks4;
          case TypeCode.Byte:
            return (object) ticks4;
          case TypeCode.Int16:
            return (object) (short) ticks4;
          case TypeCode.UInt16:
            return (object) (ushort) ticks4;
          case TypeCode.Int32:
            return (object) (int) ticks4;
          case TypeCode.UInt32:
            return (object) (uint) ticks4;
          case TypeCode.Int64:
            return (object) (long) ticks4;
          case TypeCode.UInt64:
            return (object) (ulong) ticks4;
          case TypeCode.Single:
            return (object) (float) ticks4;
          case TypeCode.Double:
            return (object) (double) ticks4;
          case TypeCode.Decimal:
            return (object) (Decimal) ticks4;
          case TypeCode.DateTime:
            return (object) new DateTime((long) ticks4);
          case TypeCode.String:
            return (object) ticks4.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        break;
      case TypeCode.Int16:
        short int16 = iconvertible.ToInt16((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) (int16 != (short) 0);
          case TypeCode.Char:
            return (object) (char) int16;
          case TypeCode.SByte:
            return (object) (sbyte) int16;
          case TypeCode.Byte:
            return (object) (byte) int16;
          case TypeCode.Int16:
            return (object) int16;
          case TypeCode.UInt16:
            return (object) (ushort) int16;
          case TypeCode.Int32:
            return (object) (int) int16;
          case TypeCode.UInt32:
            return (object) (uint) int16;
          case TypeCode.Int64:
            return (object) (long) int16;
          case TypeCode.UInt64:
            return (object) (ulong) int16;
          case TypeCode.Single:
            return (object) (float) int16;
          case TypeCode.Double:
            return (object) (double) int16;
          case TypeCode.Decimal:
            return (object) (Decimal) int16;
          case TypeCode.DateTime:
            return (object) new DateTime((long) int16);
          case TypeCode.String:
            return (object) int16.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        break;
      case TypeCode.UInt16:
        ushort uint16 = iconvertible.ToUInt16((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) (uint16 > (ushort) 0);
          case TypeCode.Char:
            return (object) (char) uint16;
          case TypeCode.SByte:
            return (object) (sbyte) uint16;
          case TypeCode.Byte:
            return (object) (byte) uint16;
          case TypeCode.Int16:
            return (object) (short) uint16;
          case TypeCode.UInt16:
            return (object) uint16;
          case TypeCode.Int32:
            return (object) (int) uint16;
          case TypeCode.UInt32:
            return (object) (uint) uint16;
          case TypeCode.Int64:
            return (object) (long) uint16;
          case TypeCode.UInt64:
            return (object) (ulong) uint16;
          case TypeCode.Single:
            return (object) (float) uint16;
          case TypeCode.Double:
            return (object) (double) uint16;
          case TypeCode.Decimal:
            return (object) (Decimal) uint16;
          case TypeCode.DateTime:
            return (object) new DateTime((long) uint16);
          case TypeCode.String:
            return (object) uint16.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        break;
      case TypeCode.Int32:
        int int32 = iconvertible.ToInt32((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) (int32 != 0);
          case TypeCode.Char:
            return (object) (char) int32;
          case TypeCode.SByte:
            return (object) (sbyte) int32;
          case TypeCode.Byte:
            return (object) (byte) int32;
          case TypeCode.Int16:
            return (object) (short) int32;
          case TypeCode.UInt16:
            return (object) (ushort) int32;
          case TypeCode.Int32:
            return (object) int32;
          case TypeCode.UInt32:
            return (object) (uint) int32;
          case TypeCode.Int64:
            return (object) (long) int32;
          case TypeCode.UInt64:
            return (object) (ulong) int32;
          case TypeCode.Single:
            return (object) (float) int32;
          case TypeCode.Double:
            return (object) (double) int32;
          case TypeCode.Decimal:
            return (object) (Decimal) int32;
          case TypeCode.DateTime:
            return (object) new DateTime((long) int32);
          case TypeCode.String:
            return (object) int32.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        break;
      case TypeCode.UInt32:
        uint uint32 = iconvertible.ToUInt32((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) (uint32 > 0U);
          case TypeCode.Char:
            return (object) (char) uint32;
          case TypeCode.SByte:
            return (object) (sbyte) uint32;
          case TypeCode.Byte:
            return (object) (byte) uint32;
          case TypeCode.Int16:
            return (object) (short) uint32;
          case TypeCode.UInt16:
            return (object) (ushort) uint32;
          case TypeCode.Int32:
            return (object) (int) uint32;
          case TypeCode.UInt32:
            return (object) uint32;
          case TypeCode.Int64:
            return (object) (long) uint32;
          case TypeCode.UInt64:
            return (object) (ulong) uint32;
          case TypeCode.Single:
            return (object) (float) uint32;
          case TypeCode.Double:
            return (object) (double) uint32;
          case TypeCode.Decimal:
            return (object) (Decimal) uint32;
          case TypeCode.DateTime:
            return (object) new DateTime((long) uint32);
          case TypeCode.String:
            return (object) uint32.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        break;
      case TypeCode.Int64:
        long int64_1 = iconvertible.ToInt64((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) (int64_1 != 0L);
          case TypeCode.Char:
            return (object) (char) int64_1;
          case TypeCode.SByte:
            return (object) (sbyte) int64_1;
          case TypeCode.Byte:
            return (object) (byte) int64_1;
          case TypeCode.Int16:
            return (object) (short) int64_1;
          case TypeCode.UInt16:
            return (object) (ushort) int64_1;
          case TypeCode.Int32:
            return (object) (int) int64_1;
          case TypeCode.UInt32:
            return (object) (uint) int64_1;
          case TypeCode.Int64:
            return (object) int64_1;
          case TypeCode.UInt64:
            return (object) (ulong) int64_1;
          case TypeCode.Single:
            return (object) (float) int64_1;
          case TypeCode.Double:
            return (object) (double) int64_1;
          case TypeCode.Decimal:
            return (object) (Decimal) int64_1;
          case TypeCode.DateTime:
            return (object) new DateTime(int64_1);
          case TypeCode.String:
            return (object) int64_1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        break;
      case TypeCode.UInt64:
        ulong uint64 = iconvertible.ToUInt64((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) (uint64 > 0UL);
          case TypeCode.Char:
            return (object) (char) uint64;
          case TypeCode.SByte:
            return (object) (sbyte) uint64;
          case TypeCode.Byte:
            return (object) (byte) uint64;
          case TypeCode.Int16:
            return (object) (short) uint64;
          case TypeCode.UInt16:
            return (object) (ushort) uint64;
          case TypeCode.Int32:
            return (object) (int) uint64;
          case TypeCode.UInt32:
            return (object) (uint) uint64;
          case TypeCode.Int64:
            return (object) (long) uint64;
          case TypeCode.UInt64:
            return (object) uint64;
          case TypeCode.Single:
            return (object) (float) uint64;
          case TypeCode.Double:
            return (object) (double) uint64;
          case TypeCode.Decimal:
            return (object) (Decimal) uint64;
          case TypeCode.DateTime:
            return (object) new DateTime((long) uint64);
          case TypeCode.String:
            return (object) uint64.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        break;
      case TypeCode.Single:
        float single = iconvertible.ToSingle((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
            return (double) single != (double) single ? (object) false : (object) ((double) single != 0.0);
          case TypeCode.Single:
            return (object) single;
          case TypeCode.Double:
            return (object) (double) single;
          case TypeCode.Decimal:
            return (object) (Decimal) single;
          case TypeCode.String:
            return (object) Convert.ToString((double) single);
          default:
            long int64_2 = Microsoft.JScript.Runtime.DoubleToInt64((double) single);
            switch (target - 4)
            {
              case TypeCode.Empty:
                return (object) (char) int64_2;
              case TypeCode.Object:
                return (object) (sbyte) int64_2;
              case TypeCode.DBNull:
                return (object) (byte) int64_2;
              case TypeCode.Boolean:
                return (object) (short) int64_2;
              case TypeCode.Char:
                return (object) (ushort) int64_2;
              case TypeCode.SByte:
                return (object) (int) int64_2;
              case TypeCode.Byte:
                return (object) (uint) int64_2;
              case TypeCode.Int16:
                return (object) int64_2;
              case TypeCode.UInt16:
                return (object) (ulong) int64_2;
              case TypeCode.UInt64:
                return (object) new DateTime(int64_2);
            }
            break;
        }
        break;
      case TypeCode.Double:
        double num1 = iconvertible.ToDouble((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) Convert.ToBoolean(num1);
          case TypeCode.Single:
            return (object) (float) num1;
          case TypeCode.Double:
            return (object) num1;
          case TypeCode.Decimal:
            return (object) (Decimal) num1;
          case TypeCode.String:
            return (object) Convert.ToString(num1);
          default:
            long int64_3 = Microsoft.JScript.Runtime.DoubleToInt64(num1);
            switch (target - 4)
            {
              case TypeCode.Empty:
                return (object) (char) int64_3;
              case TypeCode.Object:
                return (object) (sbyte) int64_3;
              case TypeCode.DBNull:
                return (object) (byte) int64_3;
              case TypeCode.Boolean:
                return (object) (short) int64_3;
              case TypeCode.Char:
                return (object) (ushort) int64_3;
              case TypeCode.SByte:
                return (object) (int) int64_3;
              case TypeCode.Byte:
                return (object) (uint) int64_3;
              case TypeCode.Int16:
                return (object) int64_3;
              case TypeCode.UInt16:
                return (object) (ulong) int64_3;
              case TypeCode.UInt64:
                return (object) new DateTime(int64_3);
            }
            break;
        }
        break;
      case TypeCode.Decimal:
        Decimal num2 = iconvertible.ToDecimal((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) (num2 != 0M);
          case TypeCode.Char:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
          case TypeCode.UInt32:
          case TypeCode.Int64:
          case TypeCode.UInt64:
            return Convert.Coerce2WithTruncationPermitted((object) Microsoft.JScript.Runtime.UncheckedDecimalToInt64(num2), target);
          case TypeCode.Single:
            return (object) Decimal.ToSingle(num2);
          case TypeCode.Double:
            return (object) Decimal.ToDouble(num2);
          case TypeCode.Decimal:
            return (object) num2;
          case TypeCode.DateTime:
            return (object) new DateTime(Microsoft.JScript.Runtime.UncheckedDecimalToInt64(num2));
          case TypeCode.String:
            return (object) num2.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        break;
      case TypeCode.DateTime:
        DateTime dateTime = iconvertible.ToDateTime((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
          case TypeCode.Char:
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
            return Convert.Coerce2WithTruncationPermitted((object) dateTime.Ticks, target);
          case TypeCode.DateTime:
            return (object) dateTime;
          case TypeCode.String:
            return (object) dateTime.ToString((IFormatProvider) CultureInfo.InvariantCulture);
        }
        break;
      case TypeCode.String:
        string str = iconvertible.ToString((IFormatProvider) null);
        switch (target)
        {
          case TypeCode.Boolean:
            return (object) Convert.ToBoolean((object) str, false);
          case TypeCode.Char:
            return str.Length == 1 ? (object) str[0] : throw new JScriptException(JSError.TypeMismatch);
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
          case TypeCode.UInt32:
          case TypeCode.Double:
            return Convert.Coerce2WithTruncationPermitted((object) Convert.ToNumber(str), target);
          case TypeCode.Int64:
            try
            {
              return (object) long.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture);
            }
            catch
            {
              try
              {
                return (object) (long) ulong.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture);
              }
              catch
              {
                goto case TypeCode.SByte;
              }
            }
          case TypeCode.UInt64:
            try
            {
              return (object) ulong.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture);
            }
            catch
            {
              goto case TypeCode.SByte;
            }
          case TypeCode.Single:
            try
            {
              return (object) float.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture);
            }
            catch
            {
              goto case TypeCode.SByte;
            }
          case TypeCode.Decimal:
            try
            {
              return (object) Decimal.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture);
            }
            catch
            {
              goto case TypeCode.SByte;
            }
          case TypeCode.DateTime:
            return (object) DateTime.Parse(str, (IFormatProvider) CultureInfo.InvariantCulture);
          case TypeCode.String:
            return (object) str;
        }
        break;
    }
    throw new JScriptException(JSError.TypeMismatch);
  }

  internal static void Emit(AST ast, ILGenerator il, Type source_type, Type target_type)
  {
    Convert.Emit(ast, il, source_type, target_type, false);
  }

  internal static void Emit(
    AST ast,
    ILGenerator il,
    Type source_type,
    Type target_type,
    bool truncationPermitted)
  {
    if (source_type == target_type)
      return;
    if (target_type == Typeob.Void)
      il.Emit(OpCodes.Pop);
    else if (target_type.IsEnum)
    {
      if (source_type == Typeob.String || source_type == Typeob.Object)
      {
        il.Emit(OpCodes.Ldtoken, target_type);
        il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
        ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
        il.Emit(OpCodes.Call, CompilerGlobals.coerceTMethod);
        Convert.EmitUnbox(il, target_type, Type.GetTypeCode(Convert.GetUnderlyingType(target_type)));
      }
      else
        Convert.Emit(ast, il, source_type, Convert.GetUnderlyingType(target_type));
    }
    else
    {
      if (source_type.IsEnum)
      {
        if (target_type.IsPrimitive)
        {
          Convert.Emit(ast, il, Convert.GetUnderlyingType(source_type), target_type);
          return;
        }
        if (target_type == Typeob.Object || target_type == Typeob.Enum)
        {
          il.Emit(OpCodes.Box, source_type);
          return;
        }
        if (target_type == Typeob.String)
        {
          il.Emit(OpCodes.Box, source_type);
          ConstantWrapper.TranslateToILInt(il, 0);
          il.Emit(OpCodes.Call, CompilerGlobals.toStringMethod);
          return;
        }
      }
      while (source_type is TypeBuilder)
      {
        source_type = source_type.BaseType;
        if (source_type == (Type) null)
          source_type = Typeob.Object;
        if (source_type == target_type)
          return;
      }
      if (source_type.IsArray && target_type.IsArray)
        return;
      TypeCode typeCode1 = Type.GetTypeCode(source_type);
      TypeCode typeCode2 = target_type is TypeBuilder ? TypeCode.Object : Type.GetTypeCode(target_type);
      switch (typeCode1)
      {
        case TypeCode.Empty:
          return;
        case TypeCode.Object:
          if (source_type == Typeob.Void)
          {
            il.Emit(OpCodes.Ldnull);
            source_type = Typeob.Object;
          }
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type.IsArray || target_type == Typeob.Array)
              {
                if (source_type == Typeob.ArrayObject || source_type == Typeob.Object)
                {
                  if (target_type.IsArray)
                    il.Emit(OpCodes.Ldtoken, target_type.GetElementType());
                  else
                    il.Emit(OpCodes.Ldtoken, Typeob.Object);
                  il.Emit(OpCodes.Call, CompilerGlobals.toNativeArrayMethod);
                }
                il.Emit(OpCodes.Castclass, target_type);
                return;
              }
              if (target_type is TypeBuilder)
              {
                il.Emit(OpCodes.Castclass, target_type);
                return;
              }
              if (target_type == Typeob.Enum && source_type.BaseType == Typeob.Enum)
              {
                il.Emit(OpCodes.Box, source_type);
                return;
              }
              if (target_type == Typeob.Object || target_type.IsAssignableFrom(source_type))
              {
                if (!source_type.IsValueType)
                  return;
                il.Emit(OpCodes.Box, source_type);
                return;
              }
              if (Typeob.JSObject.IsAssignableFrom(target_type))
              {
                if (source_type.IsValueType)
                  il.Emit(OpCodes.Box, source_type);
                ast.EmitILToLoadEngine(il);
                il.Emit(OpCodes.Call, CompilerGlobals.toObject2Method);
                il.Emit(OpCodes.Castclass, target_type);
                return;
              }
              if (Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              if (target_type.IsValueType || target_type.IsArray)
              {
                il.Emit(OpCodes.Ldtoken, target_type);
                il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
                ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
                il.Emit(OpCodes.Call, CompilerGlobals.coerceTMethod);
              }
              if (target_type.IsValueType)
              {
                Convert.EmitUnbox(il, target_type, typeCode2);
                return;
              }
              il.Emit(OpCodes.Castclass, target_type);
              return;
            case TypeCode.DBNull:
              return;
            case TypeCode.Boolean:
              if (source_type.IsValueType)
                il.Emit(OpCodes.Box, source_type);
              ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
              il.Emit(OpCodes.Call, CompilerGlobals.toBooleanMethod);
              return;
            case TypeCode.Char:
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
            case TypeCode.UInt32:
            case TypeCode.Int64:
            case TypeCode.UInt64:
            case TypeCode.Decimal:
            case TypeCode.DateTime:
              if (source_type.IsValueType)
                il.Emit(OpCodes.Box, source_type);
              if (truncationPermitted && typeCode2 == TypeCode.Int32)
              {
                il.Emit(OpCodes.Call, CompilerGlobals.toInt32Method);
                return;
              }
              ConstantWrapper.TranslateToILInt(il, (int) typeCode2);
              ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
              il.Emit(OpCodes.Call, CompilerGlobals.coerce2Method);
              if (!target_type.IsValueType)
                return;
              Convert.EmitUnbox(il, target_type, typeCode2);
              return;
            case TypeCode.Single:
              if (source_type.IsValueType)
                il.Emit(OpCodes.Box, source_type);
              il.Emit(OpCodes.Call, CompilerGlobals.toNumberMethod);
              il.Emit(OpCodes.Conv_R4);
              return;
            case TypeCode.Double:
              if (source_type.IsValueType)
                il.Emit(OpCodes.Box, source_type);
              il.Emit(OpCodes.Call, CompilerGlobals.toNumberMethod);
              return;
            case TypeCode.Object | TypeCode.DateTime:
              return;
            case TypeCode.String:
              if (source_type.IsValueType)
                il.Emit(OpCodes.Box, source_type);
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Castclass, Typeob.String);
                return;
              }
              ConstantWrapper.TranslateToILInt(il, 1);
              il.Emit(OpCodes.Call, CompilerGlobals.toStringMethod);
              return;
            default:
              return;
          }
        case TypeCode.DBNull:
          if (source_type.IsValueType)
            il.Emit(OpCodes.Box, source_type);
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type == Typeob.Object)
                return;
              if (!target_type.IsValueType)
              {
                il.Emit(OpCodes.Pop);
                il.Emit(OpCodes.Ldnull);
                return;
              }
              break;
            case TypeCode.String:
              if (truncationPermitted)
                break;
              goto case TypeCode.Object;
          }
          if (target_type.IsValueType)
          {
            il.Emit(OpCodes.Ldtoken, target_type);
            il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
            ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
            il.Emit(OpCodes.Call, CompilerGlobals.coerceTMethod);
            Convert.EmitUnbox(il, target_type, typeCode2);
            return;
          }
          ConstantWrapper.TranslateToILInt(il, (int) typeCode2);
          ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
          il.Emit(OpCodes.Call, CompilerGlobals.coerce2Method);
          return;
        case TypeCode.Boolean:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
              return;
            case TypeCode.Char:
              return;
            case TypeCode.SByte:
              return;
            case TypeCode.Byte:
              return;
            case TypeCode.Int16:
              return;
            case TypeCode.UInt16:
              return;
            case TypeCode.Int32:
              return;
            case TypeCode.UInt32:
              return;
            case TypeCode.Int64:
            case TypeCode.UInt64:
              il.Emit(OpCodes.Conv_U8);
              return;
            case TypeCode.Single:
              il.Emit(OpCodes.Conv_R4);
              return;
            case TypeCode.Double:
              il.Emit(OpCodes.Conv_R8);
              return;
            case TypeCode.Decimal:
              il.Emit(OpCodes.Call, CompilerGlobals.int32ToDecimalMethod);
              return;
            case TypeCode.DateTime:
              il.Emit(OpCodes.Conv_I8);
              il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
              return;
            case TypeCode.String:
              Label label1 = il.DefineLabel();
              Label label2 = il.DefineLabel();
              il.Emit(OpCodes.Brfalse, label1);
              il.Emit(OpCodes.Ldstr, "true");
              il.Emit(OpCodes.Br, label2);
              il.MarkLabel(label1);
              il.Emit(OpCodes.Ldstr, "false");
              il.MarkLabel(label2);
              return;
          }
          break;
        case TypeCode.Char:
        case TypeCode.UInt16:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              return;
            case TypeCode.Char:
              return;
            case TypeCode.SByte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I1);
              return;
            case TypeCode.Byte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U1);
              return;
            case TypeCode.Int16:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I2);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I2);
              return;
            case TypeCode.UInt16:
              return;
            case TypeCode.Int32:
              return;
            case TypeCode.UInt32:
              return;
            case TypeCode.Int64:
              il.Emit(OpCodes.Conv_I8);
              return;
            case TypeCode.UInt64:
              il.Emit(OpCodes.Conv_U8);
              return;
            case TypeCode.Single:
            case TypeCode.Double:
              il.Emit(OpCodes.Conv_R_Un);
              return;
            case TypeCode.Decimal:
              il.Emit(OpCodes.Call, CompilerGlobals.uint32ToDecimalMethod);
              return;
            case TypeCode.DateTime:
              il.Emit(OpCodes.Conv_I8);
              il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
              return;
            case TypeCode.String:
              if (typeCode1 == TypeCode.Char)
              {
                il.Emit(OpCodes.Call, CompilerGlobals.convertCharToStringMethod);
                return;
              }
              Convert.EmitLdloca(il, Typeob.UInt32);
              il.Emit(OpCodes.Call, CompilerGlobals.uint32ToStringMethod);
              return;
          }
          break;
        case TypeCode.SByte:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              return;
            case TypeCode.Char:
            case TypeCode.UInt16:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U2);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U2);
              return;
            case TypeCode.SByte:
              return;
            case TypeCode.Byte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U1);
              return;
            case TypeCode.Int16:
              return;
            case TypeCode.Int32:
              return;
            case TypeCode.UInt32:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U4);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U4);
              return;
            case TypeCode.Int64:
              il.Emit(OpCodes.Conv_I8);
              return;
            case TypeCode.UInt64:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I8);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U8);
              return;
            case TypeCode.Single:
            case TypeCode.Double:
              il.Emit(OpCodes.Conv_R8);
              return;
            case TypeCode.Decimal:
              il.Emit(OpCodes.Call, CompilerGlobals.int32ToDecimalMethod);
              return;
            case TypeCode.DateTime:
              il.Emit(OpCodes.Conv_I8);
              il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
              return;
            case TypeCode.String:
              Convert.EmitLdloca(il, Typeob.Int32);
              il.Emit(OpCodes.Call, CompilerGlobals.int32ToStringMethod);
              return;
          }
          break;
        case TypeCode.Byte:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              return;
            case TypeCode.Char:
              return;
            case TypeCode.SByte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I1_Un);
              return;
            case TypeCode.Byte:
              return;
            case TypeCode.Int16:
              return;
            case TypeCode.UInt16:
              return;
            case TypeCode.Int32:
              return;
            case TypeCode.UInt32:
              return;
            case TypeCode.Int64:
            case TypeCode.UInt64:
              il.Emit(OpCodes.Conv_U8);
              return;
            case TypeCode.Single:
            case TypeCode.Double:
              il.Emit(OpCodes.Conv_R_Un);
              return;
            case TypeCode.Decimal:
              il.Emit(OpCodes.Call, CompilerGlobals.uint32ToDecimalMethod);
              return;
            case TypeCode.DateTime:
              il.Emit(OpCodes.Conv_I8);
              il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
              return;
            case TypeCode.String:
              Convert.EmitLdloca(il, Typeob.UInt32);
              il.Emit(OpCodes.Call, CompilerGlobals.uint32ToStringMethod);
              return;
          }
          break;
        case TypeCode.Int16:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              return;
            case TypeCode.Char:
            case TypeCode.UInt16:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U2);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U2);
              return;
            case TypeCode.SByte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I1);
              return;
            case TypeCode.Byte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U1);
              return;
            case TypeCode.Int16:
              return;
            case TypeCode.Int32:
              return;
            case TypeCode.UInt32:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U4);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U4);
              return;
            case TypeCode.Int64:
              il.Emit(OpCodes.Conv_I8);
              return;
            case TypeCode.UInt64:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I8);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U8);
              return;
            case TypeCode.Single:
            case TypeCode.Double:
              il.Emit(OpCodes.Conv_R8);
              return;
            case TypeCode.Decimal:
              il.Emit(OpCodes.Call, CompilerGlobals.int32ToDecimalMethod);
              return;
            case TypeCode.DateTime:
              il.Emit(OpCodes.Conv_I8);
              il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
              return;
            case TypeCode.String:
              Convert.EmitLdloca(il, Typeob.Int32);
              il.Emit(OpCodes.Call, CompilerGlobals.int32ToStringMethod);
              return;
          }
          break;
        case TypeCode.Int32:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              return;
            case TypeCode.Char:
            case TypeCode.UInt16:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U2);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U2);
              return;
            case TypeCode.SByte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I1);
              return;
            case TypeCode.Byte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U1);
              return;
            case TypeCode.Int16:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I2);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I2);
              return;
            case TypeCode.Int32:
              return;
            case TypeCode.UInt32:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U4);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U4);
              return;
            case TypeCode.Int64:
              il.Emit(OpCodes.Conv_I8);
              return;
            case TypeCode.UInt64:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U8);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U8);
              return;
            case TypeCode.Single:
            case TypeCode.Double:
              il.Emit(OpCodes.Conv_R8);
              return;
            case TypeCode.Decimal:
              il.Emit(OpCodes.Call, CompilerGlobals.int32ToDecimalMethod);
              return;
            case TypeCode.DateTime:
              il.Emit(OpCodes.Conv_I8);
              il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
              return;
            case TypeCode.String:
              Convert.EmitLdloca(il, Typeob.Int32);
              il.Emit(OpCodes.Call, CompilerGlobals.int32ToStringMethod);
              return;
          }
          break;
        case TypeCode.UInt32:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              return;
            case TypeCode.Char:
            case TypeCode.UInt16:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U2);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U2);
              return;
            case TypeCode.SByte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I1);
              return;
            case TypeCode.Byte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U1);
              return;
            case TypeCode.Int16:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I2);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I2);
              return;
            case TypeCode.Int32:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I4);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I4_Un);
              return;
            case TypeCode.UInt32:
              return;
            case TypeCode.Int64:
              il.Emit(OpCodes.Conv_I8);
              return;
            case TypeCode.UInt64:
              il.Emit(OpCodes.Conv_U8);
              return;
            case TypeCode.Single:
            case TypeCode.Double:
              il.Emit(OpCodes.Conv_R_Un);
              return;
            case TypeCode.Decimal:
              il.Emit(OpCodes.Call, CompilerGlobals.uint32ToDecimalMethod);
              return;
            case TypeCode.DateTime:
              il.Emit(OpCodes.Conv_I8);
              il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
              return;
            case TypeCode.String:
              Convert.EmitLdloca(il, Typeob.UInt32);
              il.Emit(OpCodes.Call, CompilerGlobals.uint32ToStringMethod);
              return;
          }
          break;
        case TypeCode.Int64:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Conv_I8);
              il.Emit(OpCodes.Ceq);
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              return;
            case TypeCode.Char:
            case TypeCode.UInt16:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U2);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U2);
              return;
            case TypeCode.SByte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I1);
              return;
            case TypeCode.Byte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U1);
              return;
            case TypeCode.Int16:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I2);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I2);
              return;
            case TypeCode.Int32:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I4);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I4);
              return;
            case TypeCode.UInt32:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U4);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U4);
              return;
            case TypeCode.Int64:
              return;
            case TypeCode.UInt64:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U8);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U8);
              return;
            case TypeCode.Single:
            case TypeCode.Double:
              il.Emit(OpCodes.Conv_R8);
              return;
            case TypeCode.Decimal:
              il.Emit(OpCodes.Call, CompilerGlobals.int64ToDecimalMethod);
              return;
            case TypeCode.DateTime:
              il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
              return;
            case TypeCode.String:
              Convert.EmitLdloca(il, Typeob.Int64);
              il.Emit(OpCodes.Call, CompilerGlobals.int64ToStringMethod);
              return;
          }
          break;
        case TypeCode.UInt64:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Conv_I8);
              il.Emit(OpCodes.Ceq);
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              return;
            case TypeCode.Char:
            case TypeCode.UInt16:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U2);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U2);
              return;
            case TypeCode.SByte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I1);
              return;
            case TypeCode.Byte:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U1);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U1);
              return;
            case TypeCode.Int16:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I2);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I2);
              return;
            case TypeCode.Int32:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I4);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I4);
              return;
            case TypeCode.UInt32:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_U4);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_U4);
              return;
            case TypeCode.Int64:
              if (truncationPermitted)
              {
                il.Emit(OpCodes.Conv_I8);
                return;
              }
              il.Emit(OpCodes.Conv_Ovf_I8_Un);
              return;
            case TypeCode.UInt64:
              return;
            case TypeCode.Single:
            case TypeCode.Double:
              il.Emit(OpCodes.Conv_R_Un);
              return;
            case TypeCode.Decimal:
              il.Emit(OpCodes.Call, CompilerGlobals.uint64ToDecimalMethod);
              return;
            case TypeCode.DateTime:
              il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
              return;
            case TypeCode.String:
              Convert.EmitLdloca(il, Typeob.UInt64);
              il.Emit(OpCodes.Call, CompilerGlobals.uint64ToStringMethod);
              return;
          }
          break;
        case TypeCode.Single:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
            case TypeCode.Decimal:
            case TypeCode.String:
              il.Emit(OpCodes.Conv_R8);
              Convert.Emit(ast, il, Typeob.Double, target_type);
              return;
            case TypeCode.Char:
            case TypeCode.UInt16:
              if (truncationPermitted)
              {
                Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_U2);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_U2);
              return;
            case TypeCode.SByte:
              if (truncationPermitted)
              {
                Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_I1);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_I1);
              return;
            case TypeCode.Byte:
              if (truncationPermitted)
              {
                Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_U1);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_U1);
              return;
            case TypeCode.Int16:
              if (truncationPermitted)
              {
                Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_I2);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_I2);
              return;
            case TypeCode.Int32:
              if (truncationPermitted)
              {
                Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_I4);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
              il.Emit(OpCodes.Conv_I4);
              return;
            case TypeCode.UInt32:
              if (truncationPermitted)
              {
                Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_Ovf_U4);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_U4);
              return;
            case TypeCode.Int64:
              if (truncationPermitted)
              {
                Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_I8);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
              il.Emit(OpCodes.Conv_I8);
              return;
            case TypeCode.UInt64:
              if (truncationPermitted)
              {
                Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_U8);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_U8);
              return;
            case TypeCode.Single:
              return;
            case TypeCode.Double:
              return;
            case TypeCode.DateTime:
              if (truncationPermitted)
              {
                Convert.EmitSingleToIntegerTruncatedConversion(il, OpCodes.Conv_I8);
              }
              else
              {
                il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
                il.Emit(OpCodes.Conv_Ovf_I8);
              }
              il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
              return;
          }
          break;
        case TypeCode.Double:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
              il.Emit(OpCodes.Call, CompilerGlobals.doubleToBooleanMethod);
              return;
            case TypeCode.Char:
            case TypeCode.UInt16:
              if (truncationPermitted)
              {
                Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_U2);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_U2);
              return;
            case TypeCode.SByte:
              if (truncationPermitted)
              {
                Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_I1);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_I1);
              return;
            case TypeCode.Byte:
              if (truncationPermitted)
              {
                Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_U1);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_U1);
              return;
            case TypeCode.Int16:
              if (truncationPermitted)
              {
                Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_I2);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_I2);
              return;
            case TypeCode.Int32:
              if (truncationPermitted)
              {
                Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_I4);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_I4);
              return;
            case TypeCode.UInt32:
              if (truncationPermitted)
              {
                Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_U4);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_U4);
              return;
            case TypeCode.Int64:
              if (truncationPermitted)
              {
                Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_I8);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
              il.Emit(OpCodes.Conv_I8);
              return;
            case TypeCode.UInt64:
              if (truncationPermitted)
              {
                Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_U8);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.checkIfDoubleIsIntegerMethod);
              il.Emit(OpCodes.Conv_Ovf_U8);
              return;
            case TypeCode.Single:
              return;
            case TypeCode.Double:
              return;
            case TypeCode.Decimal:
              il.Emit(OpCodes.Call, CompilerGlobals.doubleToDecimalMethod);
              return;
            case TypeCode.DateTime:
              if (truncationPermitted)
              {
                Convert.EmitDoubleToIntegerTruncatedConversion(il, OpCodes.Conv_I8);
              }
              else
              {
                il.Emit(OpCodes.Call, CompilerGlobals.checkIfSingleIsIntegerMethod);
                il.Emit(OpCodes.Conv_Ovf_I8);
              }
              il.Emit(OpCodes.Newobj, CompilerGlobals.dateTimeConstructor);
              return;
            case TypeCode.String:
              il.Emit(OpCodes.Call, CompilerGlobals.doubleToStringMethod);
              return;
          }
          break;
        case TypeCode.Decimal:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
              il.Emit(OpCodes.Ldsfld, CompilerGlobals.decimalZeroField);
              il.Emit(OpCodes.Call, CompilerGlobals.decimalCompare);
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              il.Emit(OpCodes.Ldc_I4_0);
              il.Emit(OpCodes.Ceq);
              return;
            case TypeCode.Char:
            case TypeCode.SByte:
            case TypeCode.Byte:
            case TypeCode.Int16:
            case TypeCode.UInt16:
            case TypeCode.Int32:
              if (truncationPermitted)
                Convert.EmitDecimalToIntegerTruncatedConversion(il, OpCodes.Conv_I4);
              else
                il.Emit(OpCodes.Call, CompilerGlobals.decimalToInt32Method);
              Convert.Emit(ast, il, Typeob.Int32, target_type, truncationPermitted);
              return;
            case TypeCode.UInt32:
              if (truncationPermitted)
              {
                Convert.EmitDecimalToIntegerTruncatedConversion(il, OpCodes.Conv_U4);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.decimalToUInt32Method);
              return;
            case TypeCode.Int64:
              if (truncationPermitted)
              {
                Convert.EmitDecimalToIntegerTruncatedConversion(il, OpCodes.Conv_I8);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.decimalToInt64Method);
              return;
            case TypeCode.UInt64:
              if (truncationPermitted)
              {
                Convert.EmitDecimalToIntegerTruncatedConversion(il, OpCodes.Conv_U8);
                return;
              }
              il.Emit(OpCodes.Call, CompilerGlobals.decimalToUInt64Method);
              return;
            case TypeCode.Single:
            case TypeCode.Double:
              il.Emit(OpCodes.Call, CompilerGlobals.decimalToDoubleMethod);
              Convert.Emit(ast, il, Typeob.Double, target_type, truncationPermitted);
              return;
            case TypeCode.Decimal:
              return;
            case TypeCode.DateTime:
              if (truncationPermitted)
                Convert.EmitDecimalToIntegerTruncatedConversion(il, OpCodes.Conv_I8);
              else
                il.Emit(OpCodes.Call, CompilerGlobals.decimalToInt64Method);
              Convert.Emit(ast, il, Typeob.Int64, target_type);
              return;
            case TypeCode.String:
              Convert.EmitLdloca(il, source_type);
              il.Emit(OpCodes.Call, CompilerGlobals.decimalToStringMethod);
              return;
          }
          break;
        case TypeCode.DateTime:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              il.Emit(OpCodes.Box, source_type);
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
            case TypeCode.Char:
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
              Convert.EmitLdloca(il, source_type);
              il.Emit(OpCodes.Call, CompilerGlobals.dateTimeToInt64Method);
              Convert.Emit(ast, il, Typeob.Int64, target_type, truncationPermitted);
              return;
            case TypeCode.DateTime:
              return;
            case TypeCode.String:
              Convert.EmitLdloca(il, source_type);
              il.Emit(OpCodes.Call, CompilerGlobals.dateTimeToStringMethod);
              return;
          }
          break;
        case TypeCode.String:
          switch (typeCode2)
          {
            case TypeCode.Object:
              if (target_type != Typeob.Object && !(target_type is TypeBuilder) && Convert.EmittedCallToConversionMethod(ast, il, source_type, target_type))
                return;
              Convert.Emit(ast, il, Typeob.Object, target_type);
              return;
            case TypeCode.Boolean:
            case TypeCode.Char:
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
            case TypeCode.DateTime:
              if (truncationPermitted && typeCode2 == TypeCode.Int32)
              {
                il.Emit(OpCodes.Call, CompilerGlobals.toInt32Method);
                return;
              }
              ConstantWrapper.TranslateToILInt(il, (int) typeCode2);
              ConstantWrapper.TranslateToILInt(il, truncationPermitted ? 1 : 0);
              il.Emit(OpCodes.Call, CompilerGlobals.coerce2Method);
              if (!target_type.IsValueType)
                return;
              Convert.EmitUnbox(il, target_type, typeCode2);
              return;
            case TypeCode.String:
              return;
          }
          break;
      }
      Convert.Emit(ast, il, source_type, Typeob.Object);
      il.Emit(OpCodes.Call, CompilerGlobals.throwTypeMismatch);
      LocalBuilder local = il.DeclareLocal(target_type);
      il.Emit(OpCodes.Ldloc, local);
    }
  }

  internal static void EmitSingleToIntegerTruncatedConversion(ILGenerator il, OpCode opConversion)
  {
    il.Emit(OpCodes.Conv_R8);
    Convert.EmitDoubleToIntegerTruncatedConversion(il, opConversion);
  }

  internal static void EmitDoubleToIntegerTruncatedConversion(ILGenerator il, OpCode opConversion)
  {
    il.Emit(OpCodes.Call, CompilerGlobals.doubleToInt64);
    if (opConversion.Equals(OpCodes.Conv_I8))
      return;
    il.Emit(opConversion);
  }

  internal static void EmitDecimalToIntegerTruncatedConversion(ILGenerator il, OpCode opConversion)
  {
    il.Emit(OpCodes.Call, CompilerGlobals.uncheckedDecimalToInt64Method);
    if (opConversion.Equals(OpCodes.Conv_I8))
      return;
    il.Emit(opConversion);
  }

  internal static void EmitUnbox(ILGenerator il, Type target_type, TypeCode target)
  {
    il.Emit(OpCodes.Unbox, target_type);
    switch (target)
    {
      case TypeCode.Boolean:
      case TypeCode.Byte:
        il.Emit(OpCodes.Ldind_U1);
        break;
      case TypeCode.Char:
      case TypeCode.UInt16:
        il.Emit(OpCodes.Ldind_U2);
        break;
      case TypeCode.SByte:
        il.Emit(OpCodes.Ldind_I1);
        break;
      case TypeCode.Int16:
        il.Emit(OpCodes.Ldind_I2);
        break;
      case TypeCode.Int32:
        il.Emit(OpCodes.Ldind_I4);
        break;
      case TypeCode.UInt32:
        il.Emit(OpCodes.Ldind_U4);
        break;
      case TypeCode.Int64:
      case TypeCode.UInt64:
        il.Emit(OpCodes.Ldind_I8);
        break;
      case TypeCode.Single:
        il.Emit(OpCodes.Ldind_R4);
        break;
      case TypeCode.Double:
        il.Emit(OpCodes.Ldind_R8);
        break;
      default:
        il.Emit(OpCodes.Ldobj, target_type);
        break;
    }
  }

  private static bool EmittedCallToConversionMethod(
    AST ast,
    ILGenerator il,
    Type source_type,
    Type target_type)
  {
    MethodInfo method1 = target_type.GetMethod("op_Explicit", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder) null, new Type[1]
    {
      source_type
    }, (ParameterModifier[]) null);
    if (method1 != (MethodInfo) null)
    {
      il.Emit(OpCodes.Call, method1);
      Convert.Emit(ast, il, method1.ReturnType, target_type);
      return true;
    }
    MethodInfo toXxxxMethod1 = Convert.GetToXXXXMethod((IReflect) source_type, target_type, true);
    if (toXxxxMethod1 != (MethodInfo) null)
    {
      il.Emit(OpCodes.Call, toXxxxMethod1);
      return true;
    }
    MethodInfo method2 = target_type.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder) null, new Type[1]
    {
      source_type
    }, (ParameterModifier[]) null);
    if (method2 != (MethodInfo) null)
    {
      il.Emit(OpCodes.Call, method2);
      Convert.Emit(ast, il, method2.ReturnType, target_type);
      return true;
    }
    MethodInfo toXxxxMethod2 = Convert.GetToXXXXMethod((IReflect) source_type, target_type, false);
    if (!(toXxxxMethod2 != (MethodInfo) null))
      return false;
    il.Emit(OpCodes.Call, toXxxxMethod2);
    return true;
  }

  internal static void EmitLdarg(ILGenerator il, short argNum)
  {
    switch (argNum)
    {
      case 0:
        il.Emit(OpCodes.Ldarg_0);
        break;
      case 1:
        il.Emit(OpCodes.Ldarg_1);
        break;
      case 2:
        il.Emit(OpCodes.Ldarg_2);
        break;
      case 3:
        il.Emit(OpCodes.Ldarg_3);
        break;
      default:
        if (argNum < (short) 256 /*0x0100*/)
        {
          il.Emit(OpCodes.Ldarg_S, (byte) argNum);
          break;
        }
        il.Emit(OpCodes.Ldarg, argNum);
        break;
    }
  }

  internal static void EmitLdloca(ILGenerator il, Type source_type)
  {
    LocalBuilder local = il.DeclareLocal(source_type);
    il.Emit(OpCodes.Stloc, local);
    il.Emit(OpCodes.Ldloca, local);
  }

  private static IReflect GetArrayElementType(IReflect ir)
  {
    if (ir is TypedArray)
      return ((TypedArray) ir).elementType;
    if ((object) (ir as Type) != null && ((Type) ir).IsArray)
      return (IReflect) ((Type) ir).GetElementType();
    return ir is ArrayObject || (object) ir == (object) Typeob.ArrayObject ? (IReflect) Typeob.Object : (IReflect) null;
  }

  internal static int GetArrayRank(IReflect ir)
  {
    if ((object) ir == (object) Typeob.ArrayObject || ir is ArrayObject)
      return 1;
    if (ir is TypedArray)
      return ((TypedArray) ir).rank;
    return (object) (ir as Type) != null && ((Type) ir).IsArray ? ((Type) ir).GetArrayRank() : -1;
  }

  internal static IConvertible GetIConvertible(object ob) => ob as IConvertible;

  private static MethodInfo GetToXXXXMethod(IReflect ir, Type desiredType, bool explicitOK)
  {
    switch (ir)
    {
      case TypeBuilder _:
      case EnumBuilder _:
        return (MethodInfo) null;
      default:
        MemberInfo[] member = ir.GetMember(explicitOK ? "op_Explicit" : "op_Implicit", BindingFlags.Static | BindingFlags.Public);
        if (member != null)
        {
          foreach (MemberInfo toXxxxMethod in member)
          {
            if ((object) (toXxxxMethod as MethodInfo) != null && ((MethodInfo) toXxxxMethod).ReturnType == desiredType)
              return (MethodInfo) toXxxxMethod;
          }
        }
        return (MethodInfo) null;
    }
  }

  internal static TypeCode GetTypeCode(object ob, IConvertible ic)
  {
    if (ob == null)
      return TypeCode.Empty;
    return ic == null ? TypeCode.Object : ic.GetTypeCode();
  }

  internal static TypeCode GetTypeCode(object ob)
  {
    return Convert.GetTypeCode(ob, Convert.GetIConvertible(ob));
  }

  internal static Type GetUnderlyingType(Type type)
  {
    return type is TypeBuilder ? type.UnderlyingSystemType : Enum.GetUnderlyingType(type);
  }

  internal static bool IsArray(IReflect ir)
  {
    if ((object) ir == (object) Typeob.Array || (object) ir == (object) Typeob.ArrayObject || ir is TypedArray || ir is ArrayObject)
      return true;
    return (object) (ir as Type) != null && ((Type) ir).IsArray;
  }

  private static bool IsArrayElementTypeKnown(IReflect ir)
  {
    if ((object) ir == (object) Typeob.ArrayObject || ir is TypedArray || ir is ArrayObject)
      return true;
    return (object) (ir as Type) != null && ((Type) ir).IsArray;
  }

  internal static bool IsArrayRankKnown(IReflect ir)
  {
    if ((object) ir == (object) Typeob.ArrayObject || ir is TypedArray || ir is ArrayObject)
      return true;
    return (object) (ir as Type) != null && ((Type) ir).IsArray;
  }

  internal static bool IsArrayType(IReflect ir)
  {
    if (ir is TypedArray || (object) ir == (object) Typeob.Array || (object) ir == (object) Typeob.ArrayObject)
      return true;
    return (object) (ir as Type) != null && ((Type) ir).IsArray;
  }

  internal static bool IsJScriptArray(IReflect ir)
  {
    return ir is ArrayObject || (object) ir == (object) Typeob.ArrayObject;
  }

  internal static bool IsPrimitiveSignedNumericType(Type t)
  {
    switch (Type.GetTypeCode(t))
    {
      case TypeCode.SByte:
      case TypeCode.Int16:
      case TypeCode.Int32:
      case TypeCode.Int64:
      case TypeCode.Single:
      case TypeCode.Double:
        return true;
      default:
        return false;
    }
  }

  internal static bool IsPrimitiveSignedIntegerType(Type t)
  {
    switch (Type.GetTypeCode(t))
    {
      case TypeCode.SByte:
      case TypeCode.Int16:
      case TypeCode.Int32:
      case TypeCode.Int64:
        return true;
      default:
        return false;
    }
  }

  internal static bool IsPrimitiveUnsignedIntegerType(Type t)
  {
    switch (Type.GetTypeCode(t))
    {
      case TypeCode.Byte:
      case TypeCode.UInt16:
      case TypeCode.UInt32:
      case TypeCode.UInt64:
        return true;
      default:
        return false;
    }
  }

  internal static bool IsPrimitiveIntegerType(Type t)
  {
    switch (Type.GetTypeCode(t))
    {
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
      case TypeCode.UInt32:
      case TypeCode.Int64:
      case TypeCode.UInt64:
        return true;
      default:
        return false;
    }
  }

  internal static bool IsPrimitiveNumericTypeCode(TypeCode tc)
  {
    switch (tc)
    {
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
        return true;
      default:
        return false;
    }
  }

  internal static bool IsPrimitiveNumericType(IReflect ir)
  {
    Type type = ir as Type;
    return !(type == (Type) null) && Convert.IsPrimitiveNumericTypeCode(Type.GetTypeCode(type));
  }

  internal static bool IsPrimitiveNumericTypeFitForDouble(IReflect ir)
  {
    Type type = ir as Type;
    if (type == (Type) null)
      return false;
    switch (Type.GetTypeCode(type))
    {
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
      case TypeCode.UInt32:
      case TypeCode.Single:
      case TypeCode.Double:
        return true;
      default:
        return false;
    }
  }

  private static bool IsPromotableTo(Type source_type, Type target_type)
  {
    TypeCode typeCode1 = Type.GetTypeCode(source_type);
    TypeCode typeCode2 = Type.GetTypeCode(target_type);
    if (Convert.promotable[(int) typeCode1, (int) typeCode2])
      return true;
    if ((typeCode1 == TypeCode.Object || typeCode1 == TypeCode.String) && typeCode2 == TypeCode.Object)
    {
      if (target_type.IsAssignableFrom(source_type) || target_type == Typeob.BooleanObject && source_type == Typeob.Boolean || target_type == Typeob.StringObject && source_type == Typeob.String || target_type == Typeob.NumberObject && Convert.IsPromotableTo(source_type, Typeob.Double))
        return true;
      if (target_type == Typeob.Array || source_type == Typeob.Array || target_type.IsArray || source_type.IsArray)
        return Convert.IsPromotableToArray((IReflect) source_type, (IReflect) target_type);
    }
    if (source_type == Typeob.BooleanObject && target_type == Typeob.Boolean || source_type == Typeob.StringObject && target_type == Typeob.String || source_type == Typeob.DateObject && target_type == Typeob.DateTime)
      return true;
    if (source_type == Typeob.NumberObject)
      return Convert.IsPrimitiveNumericType((IReflect) target_type);
    if (source_type.IsEnum)
      return !target_type.IsEnum && Convert.IsPromotableTo(Convert.GetUnderlyingType(source_type), target_type);
    if (target_type.IsEnum)
      return !source_type.IsEnum && Convert.IsPromotableTo(source_type, Convert.GetUnderlyingType(target_type));
    MethodInfo method = target_type.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder) null, new Type[1]
    {
      source_type
    }, (ParameterModifier[]) null);
    if (method != (MethodInfo) null && (method.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope)
      return true;
    MethodInfo toXxxxMethod = Convert.GetToXXXXMethod((IReflect) source_type, target_type, false);
    return toXxxxMethod != (MethodInfo) null && (toXxxxMethod.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope;
  }

  internal static bool IsPromotableTo(IReflect source_ir, IReflect target_ir)
  {
    if (source_ir is TypedArray || target_ir is TypedArray || source_ir is ArrayObject || target_ir is ArrayObject || (object) source_ir == (object) Typeob.ArrayObject || (object) target_ir == (object) Typeob.ArrayObject)
      return Convert.IsPromotableToArray(source_ir, target_ir);
    if (target_ir is ClassScope)
    {
      if (((ClassScope) target_ir).owner is EnumDeclaration)
      {
        if (Convert.IsPrimitiveNumericType(source_ir))
          return Convert.IsPromotableTo(source_ir, (IReflect) ((EnumDeclaration) ((ClassScope) target_ir).owner).baseType.ToType());
        return (object) source_ir == (object) Typeob.String || source_ir == target_ir;
      }
      return source_ir is ClassScope && ((ClassScope) source_ir).IsSameOrDerivedFrom((ClassScope) target_ir);
    }
    Type type;
    if ((object) (target_ir as Type) != null)
    {
      if ((object) target_ir == (object) Typeob.Object)
        return (object) (source_ir as Type) == null || !((Type) source_ir).IsByRef;
      type = (Type) target_ir;
    }
    else
      type = !(target_ir is ScriptFunction) ? Globals.TypeRefs.ToReferenceContext(target_ir.GetType()) : Typeob.ScriptFunction;
    return source_ir is ClassScope ? ((ClassScope) source_ir).IsPromotableTo(type) : Convert.IsPromotableTo((object) (source_ir as Type) != null ? (Type) source_ir : Globals.TypeRefs.ToReferenceContext(source_ir.GetType()), type);
  }

  private static bool IsPromotableToArray(IReflect source_ir, IReflect target_ir)
  {
    if (!Convert.IsArray(source_ir))
      return false;
    if ((object) target_ir == (object) Typeob.Object)
      return true;
    if (!Convert.IsArray(target_ir))
    {
      if ((object) (target_ir as Type) != null)
      {
        Type type = (Type) target_ir;
        if (type.IsInterface && type.IsAssignableFrom(Typeob.Array))
        {
          if (source_ir is TypedArray)
            return true;
          return (object) (source_ir as Type) != null && ((Type) source_ir).IsArray;
        }
      }
      return false;
    }
    if (Convert.IsJScriptArray(source_ir) && !Convert.IsJScriptArray(target_ir))
      return false;
    if ((object) target_ir == (object) Typeob.Array)
      return !Convert.IsJScriptArray(source_ir);
    if ((object) source_ir == (object) Typeob.Array)
      return false;
    if (Convert.GetArrayRank(source_ir) == 1 && Convert.IsJScriptArray(target_ir))
      return true;
    if (Convert.GetArrayRank(source_ir) != Convert.GetArrayRank(target_ir))
      return false;
    IReflect arrayElementType1 = Convert.GetArrayElementType(source_ir);
    IReflect arrayElementType2 = Convert.GetArrayElementType(target_ir);
    if (arrayElementType1 == null || arrayElementType2 == null)
      return false;
    return (object) (arrayElementType1 as Type) != null && ((Type) arrayElementType1).IsValueType || (object) (arrayElementType2 as Type) != null && ((Type) arrayElementType2).IsValueType ? arrayElementType1 == arrayElementType2 : Convert.IsPromotableTo(arrayElementType1, arrayElementType2);
  }

  private static bool IsWhiteSpace(char c)
  {
    switch (c)
    {
      case '\t':
      case '\n':
      case '\v':
      case '\f':
      case '\r':
        return true;
      default:
        if (c != ' ' && c != ' ')
          return c >= '\u0080' && char.IsWhiteSpace(c);
        goto case '\t';
    }
  }

  private static bool IsWhiteSpaceTrailer(char[] s, int i, int max)
  {
    for (; i < max; ++i)
    {
      if (!Convert.IsWhiteSpace(s[i]))
        return false;
    }
    return true;
  }

  internal static object LiteralToNumber(string str)
  {
    return Convert.LiteralToNumber(str, (Context) null);
  }

  internal static object LiteralToNumber(string str, Context context)
  {
    uint rdx = 10;
    if (str[0] == '0' && str.Length > 1)
      rdx = str[1] == 'x' || str[1] == 'X' ? 16U /*0x10*/ : 8U;
    object radix = Convert.parseRadix(str.ToCharArray(), rdx, rdx == 16U /*0x10*/ ? 2 : 0, 1, false);
    if (radix != null)
    {
      if (rdx == 8U && context != null && radix is int num && num > 7)
        context.HandleError(JSError.OctalLiteralsAreDeprecated);
      return radix;
    }
    context.HandleError(JSError.BadOctalLiteral);
    return Convert.parseRadix(str.ToCharArray(), 10U, 0, 1, false);
  }

  internal static bool NeedsWrapper(TypeCode code)
  {
    return (uint) (code - 3) <= 12U || code == TypeCode.String;
  }

  private static double DoubleParse(string str)
  {
    try
    {
      return double.Parse(str, NumberStyles.Float, (IFormatProvider) CultureInfo.InvariantCulture);
    }
    catch (OverflowException ex)
    {
      int index = 0;
      int length = str.Length;
      while (index < length && Convert.IsWhiteSpace(str[index]))
        ++index;
      return index < length && str[index] == '-' ? double.NegativeInfinity : double.PositiveInfinity;
    }
  }

  private static object parseRadix(char[] s, uint rdx, int i, int sign, bool ignoreTrailers)
  {
    int length = s.Length;
    if (i >= length)
      return (object) null;
    ulong num1 = ulong.MaxValue / (ulong) rdx;
    int num2 = Convert.RadixDigit(s[i], rdx);
    if (num2 < 0)
      return (object) null;
    ulong radix = (ulong) num2;
    int startIndex = i;
    while (++i != length)
    {
      int num3 = Convert.RadixDigit(s[i], rdx);
      if (num3 < 0)
      {
        if (!ignoreTrailers && !Convert.IsWhiteSpaceTrailer(s, i, length))
          return (object) null;
        break;
      }
      if (radix <= num1)
      {
        ulong num4 = radix * (ulong) rdx;
        ulong num5 = num4 + (ulong) num3;
        if (num4 <= num5)
        {
          radix = num5;
          continue;
        }
      }
      if (rdx == 10U)
      {
        try
        {
          double num6 = Convert.DoubleParse(new string(s, startIndex, length - startIndex));
          if (num6 == num6)
            return (object) ((double) sign * num6);
          if (!ignoreTrailers)
            return (object) null;
        }
        catch
        {
        }
      }
      double num7 = (double) radix * (double) rdx + (double) num3;
      while (++i != length)
      {
        int num8 = Convert.RadixDigit(s[i], rdx);
        if (num8 < 0)
          return ignoreTrailers || Convert.IsWhiteSpaceTrailer(s, i, length) ? (object) ((double) sign * num7) : (object) null;
        num7 = num7 * (double) rdx + (double) num8;
      }
      return (object) ((double) sign * num7);
    }
    if (sign < 0)
    {
      if (radix <= 2147483648UL /*0x80000000*/)
        return (object) (int) -(long) radix;
      if (radix < 9223372036854775808UL /*0x8000000000000000*/)
        return (object) -(long) radix;
      return radix == 9223372036854775808UL /*0x8000000000000000*/ ? (object) long.MinValue : (object) -(double) radix;
    }
    if (radix <= (ulong) int.MaxValue)
      return (object) (int) radix;
    return radix <= (ulong) long.MaxValue ? (object) (long) radix : (object) radix;
  }

  private static int RadixDigit(char c, uint r)
  {
    int num;
    if (c >= '0' && c <= '9')
      num = (int) c - 48 /*0x30*/;
    else if (c >= 'A' && c <= 'Z')
    {
      num = 10 + (int) c - 65;
    }
    else
    {
      if (c < 'a' || c > 'z')
        return -1;
      num = 10 + (int) c - 97;
    }
    return (long) num >= (long) r ? -1 : num;
  }

  /// <summary>Throws a <see cref="T:Microsoft.JScript.JScriptException" /> exception in all cases.</summary>
  /// <param name="val">The source code that caused the exception.</param>
  [DebuggerStepThrough]
  [DebuggerHidden]
  public static void ThrowTypeMismatch(object val)
  {
    throw new JScriptException(JSError.TypeMismatch, new Context(new DocumentContext("", (VsaEngine) null), val.ToString()));
  }

  /// <summary>Converts the specified <see cref="T:System.Double" /> to a <see cref="T:System.Boolean" />.</summary>
  /// <returns>false if <paramref name="d" /> is 0; otherwise, true.</returns>
  /// <param name="d">The value to convert.</param>
  public static bool ToBoolean(double d) => d == d && d != 0.0;

  /// <summary>Converts the specified <see cref="T:System.Object" /> to a <see cref="T:System.Boolean" />.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to a <see cref="T:System.Boolean" />.</returns>
  /// <param name="value">The value to convert.</param>
  [DebuggerStepThrough]
  [DebuggerHidden]
  public static bool ToBoolean(object value)
  {
    return value is bool flag ? flag : Convert.ToBoolean(value, Convert.GetIConvertible(value));
  }

  /// <summary>Converts the specified <see cref="T:System.Object" /> value to a <see cref="T:System.Boolean" /> structure, optionally allowing data loss.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to a <see cref="T:System.Boolean" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="explicitConversion">true to allow data loss; otherwise, false.</param>
  [DebuggerStepThrough]
  [DebuggerHidden]
  public static bool ToBoolean(object value, bool explicitConversion)
  {
    if (value is bool boolean)
      return boolean;
    return !explicitConversion && value is BooleanObject ? ((BooleanObject) value).value : Convert.ToBoolean(value, Convert.GetIConvertible(value));
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal static bool ToBoolean(object value, IConvertible ic)
  {
    switch (Convert.GetTypeCode(value, ic))
    {
      case TypeCode.Empty:
        return false;
      case TypeCode.Object:
        switch (value)
        {
          case Missing _:
          case System.Reflection.Missing _:
            return false;
          default:
            Type type = value.GetType();
            MethodInfo method = type.GetMethod("op_True", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder) null, new Type[1]
            {
              type
            }, (ParameterModifier[]) null);
            if (!(method != (MethodInfo) null) || (method.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || !(method.ReturnType == typeof (bool)))
              return true;
            return (bool) new JSMethodInfo(method).Invoke((object) null, BindingFlags.SuppressChangeType, (Binder) null, new object[1]
            {
              value
            }, (CultureInfo) null);
        }
      case TypeCode.DBNull:
        return false;
      case TypeCode.Boolean:
        return ic.ToBoolean((IFormatProvider) null);
      case TypeCode.Char:
        return ic.ToChar((IFormatProvider) null) > char.MinValue;
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
        return ic.ToInt32((IFormatProvider) null) != 0;
      case TypeCode.UInt32:
      case TypeCode.Int64:
        return ic.ToInt64((IFormatProvider) null) != 0L;
      case TypeCode.UInt64:
        return ic.ToUInt64((IFormatProvider) null) > 0UL;
      case TypeCode.Single:
      case TypeCode.Double:
        double num = ic.ToDouble((IFormatProvider) null);
        return num == num && num != 0.0;
      case TypeCode.Decimal:
        return ic.ToDecimal((IFormatProvider) null) != 0M;
      case TypeCode.DateTime:
        return true;
      case TypeCode.String:
        return ic.ToString((IFormatProvider) null).Length != 0;
      default:
        return false;
    }
  }

  internal static char ToChar(object value) => (char) Convert.ToUint32(value);

  private static char ToDigit(int digit)
  {
    return digit >= 10 ? (char) (97 + digit - 10) : (char) (48 /*0x30*/ + digit);
  }

  /// <summary>Converts the specified value to an <see cref="T:System.Object" /> that represents a collection of <see cref="T:Microsoft.JScript.AST" /> objects.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to an <see cref="T:System.Object" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  public static object ToForInObject(object value, VsaEngine engine)
  {
    if (value is ScriptObject)
      return value;
    IConvertible iconvertible = Convert.GetIConvertible(value);
    switch (Convert.GetTypeCode(value, iconvertible))
    {
      case TypeCode.Object:
        return value;
      case TypeCode.Boolean:
        return (object) engine.Globals.globalObject.originalBoolean.ConstructImplicitWrapper(iconvertible.ToBoolean((IFormatProvider) null));
      case TypeCode.Char:
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
        return (object) engine.Globals.globalObject.originalNumber.ConstructImplicitWrapper(value);
      case TypeCode.DateTime:
        return value;
      case TypeCode.String:
        return (object) engine.Globals.globalObject.originalString.ConstructImplicitWrapper(iconvertible.ToString((IFormatProvider) null));
      default:
        return (object) engine.Globals.globalObject.originalObject.ConstructObject();
    }
  }

  internal static double ToInteger(double number)
  {
    return number != number ? 0.0 : (double) Math.Sign(number) * Math.Floor(Math.Abs(number));
  }

  internal static double ToInteger(object value)
  {
    switch (value)
    {
      case double number:
        return Convert.ToInteger(number);
      case int integer:
        return (double) integer;
      default:
        return Convert.ToInteger(value, Convert.GetIConvertible(value));
    }
  }

  internal static double ToInteger(object value, IConvertible ic)
  {
    switch (Convert.GetTypeCode(value, ic))
    {
      case TypeCode.Empty:
        return 0.0;
      case TypeCode.Object:
      case TypeCode.DateTime:
        object primitive = Convert.ToPrimitive(value, PreferredType.Number, ref ic);
        return primitive != value ? Convert.ToInteger(Convert.ToNumber(primitive, ic)) : double.NaN;
      case TypeCode.DBNull:
        return 0.0;
      case TypeCode.Boolean:
        return ic.ToBoolean((IFormatProvider) null) ? 1.0 : 0.0;
      case TypeCode.Char:
        return (double) ic.ToChar((IFormatProvider) null);
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
      case TypeCode.UInt32:
      case TypeCode.Int64:
      case TypeCode.UInt64:
        return ic.ToDouble((IFormatProvider) null);
      case TypeCode.Single:
      case TypeCode.Double:
      case TypeCode.Decimal:
        return Convert.ToInteger(ic.ToDouble((IFormatProvider) null));
      case TypeCode.String:
        return Convert.ToInteger(Convert.ToNumber(ic.ToString((IFormatProvider) null)));
      default:
        return 0.0;
    }
  }

  /// <summary>Converts the specified value to an <see cref="T:System.Int32" />.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to an <see cref="T:System.Int32" />.</returns>
  /// <param name="value">The value to convert.</param>
  public static int ToInt32(object value)
  {
    switch (value)
    {
      case double val:
        return (int) Microsoft.JScript.Runtime.DoubleToInt64(val);
      case int int32:
        return int32;
      default:
        return Convert.ToInt32(value, Convert.GetIConvertible(value));
    }
  }

  internal static int ToInt32(object value, IConvertible ic)
  {
    switch (Convert.GetTypeCode(value, ic))
    {
      case TypeCode.Empty:
        return 0;
      case TypeCode.Object:
      case TypeCode.DateTime:
        object primitive = Convert.ToPrimitive(value, PreferredType.Number, ref ic);
        return primitive != value ? Convert.ToInt32(primitive, ic) : 0;
      case TypeCode.DBNull:
        return 0;
      case TypeCode.Boolean:
        return !ic.ToBoolean((IFormatProvider) null) ? 0 : 1;
      case TypeCode.Char:
        return (int) ic.ToChar((IFormatProvider) null);
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
        return ic.ToInt32((IFormatProvider) null);
      case TypeCode.UInt32:
      case TypeCode.Int64:
        return (int) ic.ToInt64((IFormatProvider) null);
      case TypeCode.UInt64:
        return (int) ic.ToUInt64((IFormatProvider) null);
      case TypeCode.Single:
      case TypeCode.Double:
        return (int) Microsoft.JScript.Runtime.DoubleToInt64(ic.ToDouble((IFormatProvider) null));
      case TypeCode.Decimal:
        return (int) Microsoft.JScript.Runtime.UncheckedDecimalToInt64(ic.ToDecimal((IFormatProvider) null));
      case TypeCode.String:
        return (int) Microsoft.JScript.Runtime.DoubleToInt64(Convert.ToNumber(ic.ToString((IFormatProvider) null)));
      default:
        return 0;
    }
  }

  internal static IReflect ToIReflect(Type t, VsaEngine engine)
  {
    GlobalObject globalObject = engine.Globals.globalObject;
    object ireflect = (object) t;
    if (t == Typeob.ArrayObject)
      ireflect = (object) globalObject.originalArray.Construct();
    else if (t == Typeob.BooleanObject)
      ireflect = (object) globalObject.originalBoolean.Construct();
    else if (t == Typeob.DateObject)
      ireflect = globalObject.originalDate.Construct(new object[0]);
    else if (t == Typeob.EnumeratorObject)
      ireflect = globalObject.originalEnumerator.Construct(new object[0]);
    else if (t == Typeob.ErrorObject)
      ireflect = (object) globalObject.originalError.Construct((object) new object[0]);
    else if (t == Typeob.EvalErrorObject)
      ireflect = (object) globalObject.originalEvalError.Construct((object) new object[0]);
    else if (t == Typeob.JSObject)
      ireflect = globalObject.originalObject.Construct(new object[0]);
    else if (t == Typeob.NumberObject)
      ireflect = (object) globalObject.originalNumber.Construct();
    else if (t == Typeob.RangeErrorObject)
      ireflect = (object) globalObject.originalRangeError.Construct((object) new object[0]);
    else if (t == Typeob.ReferenceErrorObject)
      ireflect = (object) globalObject.originalReferenceError.Construct((object) new object[0]);
    else if (t == Typeob.RegExpObject)
      ireflect = globalObject.originalRegExp.Construct(new object[0]);
    else if (t == Typeob.ScriptFunction)
      ireflect = (object) FunctionPrototype.ob;
    else if (t == Typeob.StringObject)
      ireflect = (object) globalObject.originalString.Construct();
    else if (t == Typeob.SyntaxErrorObject)
      ireflect = (object) globalObject.originalSyntaxError.Construct((object) new object[0]);
    else if (t == Typeob.TypeErrorObject)
      ireflect = (object) globalObject.originalTypeError.Construct((object) new object[0]);
    else if (t == Typeob.URIErrorObject)
      ireflect = (object) globalObject.originalURIError.Construct((object) new object[0]);
    else if (t == Typeob.VBArrayObject)
      ireflect = (object) globalObject.originalVBArray.Construct();
    else if (t == Typeob.ArgumentsObject)
      ireflect = globalObject.originalObject.Construct(new object[0]);
    return (IReflect) ireflect;
  }

  /// <summary>Converts the specified <see cref="T:System.Object" /> to a <see cref="T:System.Double" />.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to a <see cref="T:System.Double" />.</returns>
  /// <param name="value">The value to convert.</param>
  public static double ToNumber(object value)
  {
    switch (value)
    {
      case int number1:
        return (double) number1;
      case double number2:
        return number2;
      default:
        return Convert.ToNumber(value, Convert.GetIConvertible(value));
    }
  }

  internal static double ToNumber(object value, IConvertible ic)
  {
    switch (Convert.GetTypeCode(value, ic))
    {
      case TypeCode.Empty:
        return double.NaN;
      case TypeCode.Object:
      case TypeCode.DateTime:
        object primitive = Convert.ToPrimitive(value, PreferredType.Number, ref ic);
        return primitive != value ? Convert.ToNumber(primitive, ic) : double.NaN;
      case TypeCode.DBNull:
        return 0.0;
      case TypeCode.Boolean:
        return ic.ToBoolean((IFormatProvider) null) ? 1.0 : 0.0;
      case TypeCode.Char:
        return (double) ic.ToChar((IFormatProvider) null);
      case TypeCode.SByte:
      case TypeCode.Byte:
      case TypeCode.Int16:
      case TypeCode.UInt16:
      case TypeCode.Int32:
        return (double) ic.ToInt32((IFormatProvider) null);
      case TypeCode.UInt32:
      case TypeCode.Int64:
        return (double) ic.ToInt64((IFormatProvider) null);
      case TypeCode.UInt64:
        return (double) ic.ToUInt64((IFormatProvider) null);
      case TypeCode.Single:
      case TypeCode.Double:
      case TypeCode.Decimal:
        return ic.ToDouble((IFormatProvider) null);
      case TypeCode.String:
        return Convert.ToNumber(ic.ToString((IFormatProvider) null));
      default:
        return 0.0;
    }
  }

  /// <summary>Converts the specified <see cref="T:System.String" /> to a <see cref="T:System.Double" />.</summary>
  /// <returns>
  /// <paramref name="str" /> converted to a <see cref="T:System.Double" />.</returns>
  /// <param name="str">The value to convert.</param>
  public static double ToNumber(string str)
  {
    return Convert.ToNumber(str, true, false, (object) Missing.Value);
  }

  internal static double ToNumber(string str, bool hexOK, bool octalOK, object radix)
  {
    if (!octalOK)
    {
      try
      {
        double number = Convert.DoubleParse(str);
        if (number != 0.0)
          return number;
        int index = 0;
        int length = str.Length;
        while (index < length && Convert.IsWhiteSpace(str[index]))
          ++index;
        return index < length && str[index] == '-' ? -0.0 : 0.0;
      }
      catch
      {
        int length = str.Length;
        int index = length - 1;
        int num = 0;
        while (num < length && Convert.IsWhiteSpace(str[num]))
          ++num;
        if (hexOK)
        {
          while (index >= num && Convert.IsWhiteSpace(str[index]))
            --index;
          if (num > index)
            return 0.0;
          if (index < length - 1)
            return Convert.ToNumber(str.Substring(num, index - num + 1), hexOK, octalOK, radix);
        }
        else
        {
          if (length - num >= 8 && string.CompareOrdinal(str, num, "Infinity", 0, 8) == 0)
            return double.PositiveInfinity;
          if (length - num >= 9 && string.CompareOrdinal(str, num, "-Infinity", 0, 8) == 0)
            return double.NegativeInfinity;
          if (length - num >= 9 && string.CompareOrdinal(str, num, "+Infinity", 0, 8) == 0)
            return double.PositiveInfinity;
          while (index >= num && JSScanner.IsDigit(str[index]))
            --index;
          while (index >= num && !JSScanner.IsDigit(str[index]))
            --index;
          return index < length - 1 ? Convert.ToNumber(str.Substring(num, index - num + 1), hexOK, octalOK, radix) : double.NaN;
        }
      }
    }
    int length1 = str.Length;
    int num1 = 0;
    while (num1 < length1 && Convert.IsWhiteSpace(str[num1]))
      ++num1;
    if (num1 >= length1)
      return hexOK & octalOK ? double.NaN : 0.0;
    int sign = 1;
    bool flag1 = false;
    if (str[num1] == '-')
    {
      sign = -1;
      ++num1;
      flag1 = true;
    }
    else if (str[num1] == '+')
    {
      ++num1;
      flag1 = true;
    }
    while (num1 < length1 && Convert.IsWhiteSpace(str[num1]))
      ++num1;
    bool flag2 = radix == null || radix is Missing;
    if (num1 + 8 <= length1 & flag2 && !octalOK && str.Substring(num1, 8).Equals("Infinity"))
      return sign <= 0 ? double.NegativeInfinity : double.PositiveInfinity;
    int rdx = 10;
    if (!flag2)
      rdx = Convert.ToInt32(radix);
    if (rdx == 0)
    {
      flag2 = true;
      rdx = 10;
    }
    else if (rdx < 2 || rdx > 36)
      return double.NaN;
    if (num1 < length1 - 2 && str[num1] == '0')
    {
      if (str[num1 + 1] == 'x' || str[num1 + 1] == 'X')
      {
        if (!hexOK)
          return 0.0;
        if (flag1 && !octalOK)
          return double.NaN;
        if (flag2)
        {
          rdx = 16 /*0x10*/;
          num1 += 2;
        }
        else if (rdx == 16 /*0x10*/)
          num1 += 2;
      }
      else if (octalOK & flag2)
        rdx = 8;
    }
    return num1 < length1 ? Convert.ToNumber(Convert.parseRadix(str.ToCharArray(), (uint) rdx, num1, sign, hexOK & octalOK)) : double.NaN;
  }

  internal static string ToLocaleString(object value)
  {
    return Convert.ToString(value, PreferredType.LocaleString, true);
  }

  /// <summary>Converts the specified <see cref="T:Microsoft.JScript.ArrayObject" /> to a typed <see cref="T:System.Array" />.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to an <see cref="T:System.Array" /> if this is possible; otherwise, <paramref name="value" />.</returns>
  /// <param name="value">The <see cref="T:Microsoft.JScript.ArrayObject" /> to convert. </param>
  /// <param name="handle">The type of the elements in the array.</param>
  public static object ToNativeArray(object value, RuntimeTypeHandle handle)
  {
    if (!(value is ArrayObject))
      return value;
    Type typeFromHandle = Type.GetTypeFromHandle(handle);
    return (object) ((ArrayObject) value).ToNativeArray(typeFromHandle);
  }

  /// <summary>Converts the specified value to an <see cref="T:System.Object" />. Throws an error if it cannot perform the conversion.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to an <see cref="T:System.Object" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  public static object ToObject(object value, VsaEngine engine)
  {
    switch (value)
    {
      case ScriptObject _:
        return value;
      case string str:
        return (object) engine.Globals.globalObject.originalString.ConstructImplicitWrapper(str);
      default:
        IConvertible iconvertible = Convert.GetIConvertible(value);
        switch (Convert.GetTypeCode(value, iconvertible))
        {
          case TypeCode.Object:
            return value is Array ? (object) engine.Globals.globalObject.originalArray.ConstructImplicitWrapper((Array) value) : value;
          case TypeCode.Boolean:
            return (object) engine.Globals.globalObject.originalBoolean.ConstructImplicitWrapper(iconvertible.ToBoolean((IFormatProvider) null));
          case TypeCode.Char:
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
            return (object) engine.Globals.globalObject.originalNumber.ConstructImplicitWrapper(value);
          case TypeCode.DateTime:
            return (object) iconvertible.ToDateTime((IFormatProvider) null);
          case TypeCode.String:
            return (object) engine.Globals.globalObject.originalString.ConstructImplicitWrapper(iconvertible.ToString((IFormatProvider) null));
          default:
            throw new JScriptException(JSError.NeedObject);
        }
    }
  }

  /// <summary>Converts the specified value to an <see cref="T:System.Object" />. Returns null if it cannot perform the conversion.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to an <see cref="T:System.Object" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="engine">A reference to the scripting engine.</param>
  public static object ToObject2(object value, VsaEngine engine)
  {
    if (value is ScriptObject)
      return value;
    IConvertible iconvertible = Convert.GetIConvertible(value);
    switch (Convert.GetTypeCode(value, iconvertible))
    {
      case TypeCode.Object:
        return value is Array ? (object) engine.Globals.globalObject.originalArray.ConstructImplicitWrapper((Array) value) : value;
      case TypeCode.Boolean:
        return (object) engine.Globals.globalObject.originalBoolean.ConstructImplicitWrapper(iconvertible.ToBoolean((IFormatProvider) null));
      case TypeCode.Char:
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
        return (object) engine.Globals.globalObject.originalNumber.ConstructImplicitWrapper(value);
      case TypeCode.DateTime:
        return (object) iconvertible.ToDateTime((IFormatProvider) null);
      case TypeCode.String:
        return (object) engine.Globals.globalObject.originalString.ConstructImplicitWrapper(iconvertible.ToString((IFormatProvider) null));
      default:
        return (object) null;
    }
  }

  internal static object ToObject3(object value, VsaEngine engine)
  {
    if (value is ScriptObject)
      return value;
    IConvertible iconvertible = Convert.GetIConvertible(value);
    switch (Convert.GetTypeCode(value, iconvertible))
    {
      case TypeCode.Object:
        return value is Array ? (object) engine.Globals.globalObject.originalArray.ConstructWrapper((Array) value) : value;
      case TypeCode.Boolean:
        return (object) engine.Globals.globalObject.originalBoolean.ConstructWrapper(iconvertible.ToBoolean((IFormatProvider) null));
      case TypeCode.Char:
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
        return (object) engine.Globals.globalObject.originalNumber.ConstructWrapper(value);
      case TypeCode.DateTime:
        return (object) iconvertible.ToDateTime((IFormatProvider) null);
      case TypeCode.String:
        return (object) engine.Globals.globalObject.originalString.ConstructWrapper(iconvertible.ToString((IFormatProvider) null));
      default:
        return (object) null;
    }
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal static object ToPrimitive(object value, PreferredType preferredType)
  {
    IConvertible iconvertible = Convert.GetIConvertible(value);
    TypeCode typeCode = Convert.GetTypeCode(value, iconvertible);
    return Convert.ToPrimitive(value, preferredType, iconvertible, typeCode);
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal static object ToPrimitive(
    object value,
    PreferredType preferredType,
    ref IConvertible ic)
  {
    TypeCode typeCode = Convert.GetTypeCode(value, ic);
    switch (typeCode)
    {
      case TypeCode.Object:
      case TypeCode.DateTime:
        object primitive = Convert.ToPrimitive(value, preferredType, ic, typeCode);
        if (primitive != value)
        {
          value = primitive;
          ic = Convert.GetIConvertible(value);
          break;
        }
        break;
    }
    return value;
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  private static object ToPrimitive(
    object value,
    PreferredType preferredType,
    IConvertible ic,
    TypeCode tcode)
  {
    switch (tcode)
    {
      case TypeCode.Object:
        if (value is Array array && array.Rank == 1)
          value = (object) new ArrayWrapper((ScriptObject) ArrayPrototype.ob, array, true);
        if (value is ScriptObject)
        {
          object defaultValue = ((ScriptObject) value).GetDefaultValue(preferredType);
          if (Convert.GetTypeCode(defaultValue) != TypeCode.Object)
            return defaultValue;
          if ((value != defaultValue || preferredType != PreferredType.String) && preferredType != PreferredType.LocaleString)
            throw new JScriptException(JSError.TypeMismatch);
          if (!(value is JSObject))
            return (object) value.ToString();
          ScriptObject parent = ((ScriptObject) value).GetParent();
          return parent is ClassScope ? (object) ((ClassScope) parent).GetFullName() : (object) "[object Object]";
        }
        if (value is Missing || value is System.Reflection.Missing)
          return (object) null;
        IReflect ir = !(value is IReflect) || (object) (value as Type) != null ? (IReflect) value.GetType() : (IReflect) value;
        MethodInfo toXxxxMethod;
        if (preferredType == PreferredType.String || preferredType == PreferredType.LocaleString)
        {
          toXxxxMethod = Convert.GetToXXXXMethod(ir, typeof (string), true);
        }
        else
        {
          toXxxxMethod = Convert.GetToXXXXMethod(ir, typeof (double), true);
          if (toXxxxMethod == (MethodInfo) null)
            toXxxxMethod = Convert.GetToXXXXMethod(ir, typeof (long), true);
          if (toXxxxMethod == (MethodInfo) null)
            toXxxxMethod = Convert.GetToXXXXMethod(ir, typeof (ulong), true);
        }
        if (toXxxxMethod != (MethodInfo) null)
          return new JSMethodInfo(toXxxxMethod).Invoke((object) null, BindingFlags.SuppressChangeType, (Binder) null, new object[1]
          {
            value
          }, (CultureInfo) null);
        try
        {
          try
          {
            MemberInfo prop = LateBinding.SelectMember(JSBinder.GetDefaultMembers(Microsoft.JScript.Runtime.TypeRefs, ir));
            if (prop != (MemberInfo) null)
            {
              switch (prop.MemberType)
              {
                case MemberTypes.Event:
                  return (object) null;
                case MemberTypes.Field:
                  return ((FieldInfo) prop).GetValue(value);
                case MemberTypes.Method:
                  return ((MethodBase) prop).Invoke(value, new object[0]);
                case MemberTypes.Property:
                  return JSProperty.GetValue((PropertyInfo) prop, value, (object[]) null);
                case MemberTypes.NestedType:
                  return (object) prop;
              }
            }
            if (value == ir)
            {
              Type type = value.GetType();
              if (TypeReflector.GetTypeReflectorFor(type).Is__ComObject() && (!VsaEngine.executeForJSEE || !(value is IDebuggerObject)))
                ir = (IReflect) type;
            }
            if (!VsaEngine.executeForJSEE || !(ir is IDebuggerObject debuggerObject))
              return ir.InvokeMember(string.Empty, BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.ExactBinding | BindingFlags.SuppressChangeType, (Binder) null, value, new object[0], (ParameterModifier[]) null, (CultureInfo) null, new string[0]);
            if (debuggerObject.IsScriptObject())
              return ir.InvokeMember($"< JScript-{preferredType.ToString()} >", BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.ExactBinding | BindingFlags.SuppressChangeType, (Binder) null, value, new object[0], (ParameterModifier[]) null, (CultureInfo) null, new string[0]);
            throw new JScriptException(JSError.NonSupportedInDebugger);
          }
          catch (TargetInvocationException ex)
          {
            throw ex.InnerException;
          }
        }
        catch (ArgumentException ex)
        {
        }
        catch (IndexOutOfRangeException ex)
        {
        }
        catch (MissingMemberException ex)
        {
        }
        catch (SecurityException ex)
        {
        }
        catch (TargetParameterCountException ex)
        {
        }
        catch (COMException ex)
        {
          if (ex.ErrorCode != -2147352573 /*0x80020003*/)
            throw ex;
        }
        if (preferredType == PreferredType.Number)
          return value;
        if (value.GetType().IsCOMObject)
          return (object) "ActiveXObject";
        return value is char[] ? (object) new string((char[]) value) : (object) value.ToString();
      case TypeCode.DateTime:
        return DateConstructor.ob.Construct(ic.ToDateTime((IFormatProvider) null)).GetDefaultValue(preferredType);
      default:
        return value;
    }
  }

  internal static string ToString(object value)
  {
    return Convert.ToString(value, PreferredType.String, true);
  }

  /// <summary>Converts the specified <see cref="T:System.Object" /> to a <see cref="T:System.String" />, optionally allowing data loss.</summary>
  /// <returns>
  /// <paramref name="value" /> converted to a <see cref="T:System.String" />.</returns>
  /// <param name="value">The value to convert.</param>
  /// <param name="explicitOK">true to allow data loss; otherwise, false.</param>
  [DebuggerStepThrough]
  [DebuggerHidden]
  public static string ToString(object value, bool explicitOK)
  {
    return Convert.ToString(value, PreferredType.String, explicitOK);
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal static string ToString(object value, IConvertible ic)
  {
    return Convert.ToString(value, PreferredType.String, ic, true);
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal static string ToString(object value, PreferredType pref, bool explicitOK)
  {
    switch (value)
    {
      case string str:
        return str;
      case StringObject stringObject:
        if (stringObject.noExpando)
          return stringObject.value;
        break;
    }
    return Convert.ToString(value, pref, Convert.GetIConvertible(value), explicitOK);
  }

  [DebuggerStepThrough]
  [DebuggerHidden]
  internal static string ToString(
    object value,
    PreferredType pref,
    IConvertible ic,
    bool explicitOK)
  {
    switch (value)
    {
      case Enum @enum:
        return @enum.ToString("G");
      case EnumWrapper enumWrapper:
        return enumWrapper.ToString();
      default:
        TypeCode typeCode = Convert.GetTypeCode(value, ic);
        if (pref == PreferredType.LocaleString)
        {
          switch (typeCode - 5)
          {
            case TypeCode.Empty:
            case TypeCode.Object:
            case TypeCode.DBNull:
            case TypeCode.Boolean:
            case TypeCode.Char:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.Int32:
              double num = ic.ToDouble((IFormatProvider) null);
              return num.ToString(num <= -1E+15 || num >= 1E+15 ? "g" : "n", (IFormatProvider) NumberFormatInfo.CurrentInfo);
            case TypeCode.Byte:
              return ic.ToInt64((IFormatProvider) null).ToString("n", (IFormatProvider) NumberFormatInfo.CurrentInfo);
            case TypeCode.Int16:
              return ic.ToUInt64((IFormatProvider) null).ToString("n", (IFormatProvider) NumberFormatInfo.CurrentInfo);
            case TypeCode.UInt32:
              return ic.ToDecimal((IFormatProvider) null).ToString("n", (IFormatProvider) NumberFormatInfo.CurrentInfo);
          }
        }
        switch (typeCode)
        {
          case TypeCode.Empty:
            return !explicitOK ? (string) null : "undefined";
          case TypeCode.Object:
            return Convert.ToString(Convert.ToPrimitive(value, pref, ref ic), ic);
          case TypeCode.DBNull:
            return !explicitOK ? (string) null : "null";
          case TypeCode.Boolean:
            return !ic.ToBoolean((IFormatProvider) null) ? "false" : "true";
          case TypeCode.Char:
          case TypeCode.SByte:
          case TypeCode.Byte:
          case TypeCode.Int16:
          case TypeCode.UInt16:
          case TypeCode.Int32:
          case TypeCode.UInt32:
          case TypeCode.Int64:
          case TypeCode.UInt64:
          case TypeCode.Decimal:
          case TypeCode.String:
            return ic.ToString((IFormatProvider) null);
          case TypeCode.Single:
          case TypeCode.Double:
            return Convert.ToString(ic.ToDouble((IFormatProvider) null));
          case TypeCode.DateTime:
            return Convert.ToString((object) DateConstructor.ob.Construct(ic.ToDateTime((IFormatProvider) null)));
          default:
            return (string) null;
        }
    }
  }

  /// <summary>Converts the specified <see cref="T:System.Boolean" /> to a <see cref="T:System.String" />.</summary>
  /// <returns>
  /// <paramref name="b" /> converted to a <see cref="T:System.String" />.</returns>
  /// <param name="b">The value to convert.</param>
  public static string ToString(bool b) => !b ? "false" : "true";

  /// <summary>Converts the specified <see cref="T:System.Double" /> to a <see cref="T:System.String" />.</summary>
  /// <returns>
  /// <paramref name="d" /> converted to a <see cref="T:System.String" />.</returns>
  /// <param name="d">The value to convert.</param>
  public static string ToString(double d)
  {
    long num1 = (long) d;
    if ((double) num1 == d)
      return num1.ToString((IFormatProvider) CultureInfo.InvariantCulture);
    if (d != d)
      return "NaN";
    if (double.IsPositiveInfinity(d))
      return "Infinity";
    if (double.IsNegativeInfinity(d))
      return "-Infinity";
    double num2 = d < 0.0 ? -d : d;
    int index = 15;
    string str = num2.ToString("e14", (IFormatProvider) CultureInfo.InvariantCulture);
    if (Convert.DoubleParse(str) != num2)
    {
      str = num2.ToString("e15", (IFormatProvider) CultureInfo.InvariantCulture);
      index = 16 /*0x10*/;
      if (Convert.DoubleParse(str) != num2)
      {
        str = num2.ToString("e16", (IFormatProvider) CultureInfo.InvariantCulture);
        index = 17;
        if (Convert.DoubleParse(str) != num2)
        {
          str = num2.ToString("e17", (IFormatProvider) CultureInfo.InvariantCulture);
          index = 18;
        }
      }
    }
    int num3 = int.Parse(str.Substring(index + 2, str.Length - (index + 2)), (IFormatProvider) CultureInfo.InvariantCulture);
    while (str[index] == '0')
      --index;
    int num4 = num3 + 1;
    if (index <= num4 && num4 <= 21)
    {
      StringBuilder stringBuilder = new StringBuilder(num4 + 1);
      if (d < 0.0)
        stringBuilder.Append('-');
      stringBuilder.Append(str[0]);
      if (index > 1)
        stringBuilder.Append(str, 2, index - 1);
      if (num3 - index >= 0)
        stringBuilder.Append('0', num4 - index);
      return stringBuilder.ToString();
    }
    if (0 < num4 && num4 <= 21)
    {
      StringBuilder stringBuilder = new StringBuilder(index + 2);
      if (d < 0.0)
        stringBuilder.Append('-');
      stringBuilder.Append(str[0]);
      if (num4 > 1)
        stringBuilder.Append(str, 2, num4 - 1);
      stringBuilder.Append('.');
      stringBuilder.Append(str, num4 + 1, index - num4);
      return stringBuilder.ToString();
    }
    if (-6 < num4 && num4 <= 0)
    {
      StringBuilder stringBuilder = new StringBuilder(2 - num4);
      if (d < 0.0)
        stringBuilder.Append("-0.");
      else
        stringBuilder.Append("0.");
      if (num4 < 0)
        stringBuilder.Append('0', -num4);
      stringBuilder.Append(str[0]);
      stringBuilder.Append(str, 2, index - 1);
      return stringBuilder.ToString();
    }
    StringBuilder stringBuilder1 = new StringBuilder(28);
    if (d < 0.0)
      stringBuilder1.Append('-');
    stringBuilder1.Append(str.Substring(0, index == 1 ? 1 : index + 1));
    stringBuilder1.Append('e');
    if (num3 >= 0)
      stringBuilder1.Append('+');
    stringBuilder1.Append(num3);
    return stringBuilder1.ToString();
  }

  internal static string ToString(object value, int radix)
  {
    if (radix == 10 || radix < 2 || radix > 36)
      return Convert.ToString(value);
    double num1 = Convert.ToNumber(value);
    if (num1 == 0.0)
      return "0";
    if (double.IsNaN(num1))
      return "NaN";
    if (double.IsPositiveInfinity(num1))
      return "Infinity";
    if (double.IsNegativeInfinity(num1))
      return "-Infinity";
    StringBuilder stringBuilder = new StringBuilder();
    if (num1 < 0.0)
    {
      stringBuilder.Append('-');
      num1 = -num1;
    }
    int num2 = Convert.rgcchSig[radix - 2];
    if (num1 < 8.6736173798840355E-19 || num1 >= 2.30584300921369E+18)
    {
      int y = (int) Math.Log(num1, (double) radix) + 1;
      double d = Math.Pow((double) radix, (double) y);
      if (double.IsPositiveInfinity(d))
        d = Math.Pow((double) radix, (double) --y);
      else if (d == 0.0)
        d = Math.Pow((double) radix, (double) ++y);
      double num3 = num1 / d;
      while (num3 < 1.0)
      {
        num3 *= (double) radix;
        --y;
      }
      int digit1 = (int) num3;
      stringBuilder.Append(Convert.ToDigit(digit1));
      int num4 = num2 - 1;
      double num5 = num3 - (double) digit1;
      if (num5 != 0.0)
      {
        stringBuilder.Append('.');
        double num6;
        int digit2;
        for (; num5 != 0.0 && num4-- > 0; num5 = num6 - (double) digit2)
        {
          num6 = num5 * (double) radix;
          digit2 = (int) num6;
          if (digit2 >= radix)
            digit2 = radix - 1;
          stringBuilder.Append(Convert.ToDigit(digit2));
        }
      }
      stringBuilder.Append(y >= 0 ? "(e+" : "(e");
      stringBuilder.Append(y.ToString((IFormatProvider) CultureInfo.InvariantCulture));
      stringBuilder.Append(')');
    }
    else
    {
      int num7;
      if (num1 >= 1.0)
      {
        num7 = 1;
        double num8;
        double num9;
        for (num8 = 1.0; (num9 = num8 * (double) radix) <= num1; num8 = num9)
          ++num7;
        for (int index = 0; index < num7; ++index)
        {
          int digit = (int) (num1 / num8);
          if (digit >= radix)
            digit = radix - 1;
          stringBuilder.Append(Convert.ToDigit(digit));
          num1 -= (double) digit * num8;
          num8 /= (double) radix;
        }
      }
      else
      {
        stringBuilder.Append('0');
        num7 = 0;
      }
      if (num1 != 0.0 && num7 < num2)
      {
        stringBuilder.Append('.');
        while (num1 != 0.0 && num7 < num2)
        {
          double num10 = num1 * (double) radix;
          int digit = (int) num10;
          if (digit >= radix)
            digit = radix - 1;
          stringBuilder.Append(Convert.ToDigit(digit));
          num1 = num10 - (double) digit;
          if (digit != 0 || num7 != 0)
            ++num7;
        }
      }
    }
    return stringBuilder.ToString();
  }

  internal static Type ToType(IReflect ir) => Convert.ToType(Globals.TypeRefs, ir);

  internal static Type ToType(TypeReferences typeRefs, IReflect ir)
  {
    if ((object) (ir as Type) != null)
      return (Type) ir;
    switch (ir)
    {
      case ClassScope _:
        return ((ClassScope) ir).GetTypeBuilderOrEnumBuilder();
      case TypedArray _:
        return typeRefs.ToReferenceContext(((TypedArray) ir).ToType());
      case ScriptFunction _:
        return typeRefs.ScriptFunction;
      default:
        return typeRefs.ToReferenceContext(ir.GetType());
    }
  }

  internal static Type ToType(string descriptor, Type elementType)
  {
    Module module = elementType.Module;
    return module is ModuleBuilder ? module.GetType(elementType.FullName + descriptor) : module.Assembly.GetType(elementType.FullName + descriptor);
  }

  internal static string ToTypeName(IReflect ir)
  {
    switch (ir)
    {
      case ClassScope _:
        return ((ActivationObject) ir).GetName();
      case JSObject _:
        return ((JSObject) ir).GetClassName();
      case GlobalScope _:
        return "Global Object";
      default:
        return ir.ToString();
    }
  }

  internal static uint ToUint32(object value)
  {
    return value is uint num ? num : Convert.ToUint32(value, Convert.GetIConvertible(value));
  }

  internal static uint ToUint32(object value, IConvertible ic)
  {
    switch (Convert.GetTypeCode(value, ic))
    {
      case TypeCode.Empty:
        return 0;
      case TypeCode.Object:
      case TypeCode.DateTime:
        object primitive = Convert.ToPrimitive(value, PreferredType.Number, ref ic);
        return primitive != value ? Convert.ToUint32(primitive, ic) : 0U;
      case TypeCode.DBNull:
        return 0;
      case TypeCode.Boolean:
        return !ic.ToBoolean((IFormatProvider) null) ? 0U : 1U;
      case TypeCode.Char:
        return (uint) ic.ToChar((IFormatProvider) null);
      case TypeCode.SByte:
      case TypeCode.Int16:
      case TypeCode.Int32:
      case TypeCode.Int64:
        return (uint) ic.ToInt64((IFormatProvider) null);
      case TypeCode.Byte:
      case TypeCode.UInt16:
      case TypeCode.UInt32:
        return ic.ToUInt32((IFormatProvider) null);
      case TypeCode.UInt64:
        return (uint) ic.ToUInt64((IFormatProvider) null);
      case TypeCode.Single:
      case TypeCode.Double:
        return (uint) Microsoft.JScript.Runtime.DoubleToInt64(ic.ToDouble((IFormatProvider) null));
      case TypeCode.Decimal:
        return (uint) Microsoft.JScript.Runtime.UncheckedDecimalToInt64(ic.ToDecimal((IFormatProvider) null));
      case TypeCode.String:
        return (uint) Microsoft.JScript.Runtime.DoubleToInt64(Convert.ToNumber(ic.ToString((IFormatProvider) null)));
      default:
        return 0;
    }
  }
}
