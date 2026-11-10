// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.EnumWrapper
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal abstract class EnumWrapper : IConvertible
{
  internal abstract object value { get; }

  internal abstract Type type { get; }

  internal abstract string name { get; }

  internal virtual IReflect classScopeOrType => (IReflect) this.type;

  TypeCode IConvertible.GetTypeCode() => Convert.GetTypeCode(this.value);

  bool IConvertible.ToBoolean(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToBoolean(provider);
  }

  char IConvertible.ToChar(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToChar(provider);
  }

  sbyte IConvertible.ToSByte(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToSByte(provider);
  }

  byte IConvertible.ToByte(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToByte(provider);
  }

  short IConvertible.ToInt16(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToInt16(provider);
  }

  ushort IConvertible.ToUInt16(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToUInt16(provider);
  }

  int IConvertible.ToInt32(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToInt32(provider);
  }

  uint IConvertible.ToUInt32(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToUInt32(provider);
  }

  long IConvertible.ToInt64(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToInt64(provider);
  }

  ulong IConvertible.ToUInt64(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToUInt64(provider);
  }

  float IConvertible.ToSingle(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToSingle(provider);
  }

  double IConvertible.ToDouble(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToDouble(provider);
  }

  Decimal IConvertible.ToDecimal(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToDecimal(provider);
  }

  DateTime IConvertible.ToDateTime(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToDateTime(provider);
  }

  string IConvertible.ToString(IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToString(provider);
  }

  object IConvertible.ToType(Type conversionType, IFormatProvider provider)
  {
    return ((IConvertible) this.value).ToType(conversionType, provider);
  }

  internal object ToNumericValue() => this.value;

  public override string ToString()
  {
    if (this.name != null)
      return this.name;
    foreach (FieldInfo field in this.type.GetFields(BindingFlags.Static | BindingFlags.Public))
    {
      if (StrictEquality.JScriptStrictEquals(this.value, field.GetValue((object) null)))
        return field.Name;
    }
    return Convert.ToString(this.value);
  }
}
