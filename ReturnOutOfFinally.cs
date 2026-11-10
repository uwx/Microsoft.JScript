// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ReturnOutOfFinally
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Runtime.Serialization;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents the exception state when code execution returns out of a finally block.  </summary>
[Serializable]
public sealed class ReturnOutOfFinally : ApplicationException
{
  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.ReturnOutOfFinally" /> class.</summary>
  public ReturnOutOfFinally()
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.ReturnOutOfFinally" /> class, using the specified error message.</summary>
  /// <param name="m">The message that describes the error.</param>
  public ReturnOutOfFinally(string m)
    : base(m)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.ReturnOutOfFinally" /> class, using the specified error message and a reference to the inner exception that is the cause of this exception.</summary>
  /// <param name="m">The message that describes the error.</param>
  /// <param name="e">The exception that is the cause of the current exception.</param>
  public ReturnOutOfFinally(string m, Exception e)
    : base(m, e)
  {
  }

  private ReturnOutOfFinally(SerializationInfo s, StreamingContext c)
    : base(s, c)
  {
  }
}
