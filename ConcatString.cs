// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ConcatString
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

internal sealed class ConcatString : IConvertible
{
  private StringBuilder buf;
  private bool isOwner;
  private int length;

  internal ConcatString(string str1, string str2)
  {
    this.length = str1.Length + str2.Length;
    int capacity = this.length * 2;
    if (capacity < 256 /*0x0100*/)
      capacity = 256 /*0x0100*/;
    this.buf = new StringBuilder(str1, capacity);
    this.buf.Append(str2);
    this.isOwner = true;
  }

  internal ConcatString(ConcatString str1, string str2)
  {
    this.length = str1.length + str2.Length;
    if (str1.isOwner)
    {
      this.buf = str1.buf;
      str1.isOwner = false;
    }
    else
    {
      int capacity = this.length * 2;
      if (capacity < 256 /*0x0100*/)
        capacity = 256 /*0x0100*/;
      this.buf = new StringBuilder(str1.ToString(), capacity);
    }
    this.buf.Append(str2);
    this.isOwner = true;
  }

  TypeCode IConvertible.GetTypeCode() => TypeCode.String;

  bool IConvertible.ToBoolean(IFormatProvider provider)
  {
    return this.ToIConvertible().ToBoolean(provider);
  }

  char IConvertible.ToChar(IFormatProvider provider) => this.ToIConvertible().ToChar(provider);

  sbyte IConvertible.ToSByte(IFormatProvider provider) => this.ToIConvertible().ToSByte(provider);

  byte IConvertible.ToByte(IFormatProvider provider) => this.ToIConvertible().ToByte(provider);

  short IConvertible.ToInt16(IFormatProvider provider) => this.ToIConvertible().ToInt16(provider);

  ushort IConvertible.ToUInt16(IFormatProvider provider)
  {
    return this.ToIConvertible().ToUInt16(provider);
  }

  private IConvertible ToIConvertible() => (IConvertible) this.ToString();

  int IConvertible.ToInt32(IFormatProvider provider) => this.ToIConvertible().ToInt32(provider);

  uint IConvertible.ToUInt32(IFormatProvider provider) => this.ToIConvertible().ToUInt32(provider);

  long IConvertible.ToInt64(IFormatProvider provider) => this.ToIConvertible().ToInt64(provider);

  ulong IConvertible.ToUInt64(IFormatProvider provider) => this.ToIConvertible().ToUInt64(provider);

  float IConvertible.ToSingle(IFormatProvider provider) => this.ToIConvertible().ToSingle(provider);

  double IConvertible.ToDouble(IFormatProvider provider)
  {
    return this.ToIConvertible().ToDouble(provider);
  }

  Decimal IConvertible.ToDecimal(IFormatProvider provider)
  {
    return this.ToIConvertible().ToDecimal(provider);
  }

  DateTime IConvertible.ToDateTime(IFormatProvider provider)
  {
    return this.ToIConvertible().ToDateTime(provider);
  }

  string IConvertible.ToString(IFormatProvider provider)
  {
    return this.ToIConvertible().ToString(provider);
  }

  object IConvertible.ToType(Type conversionType, IFormatProvider provider)
  {
    return this.ToIConvertible().ToType(conversionType, provider);
  }

  public override string ToString() => this.buf.ToString(0, this.length);
}
