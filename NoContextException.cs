// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.NoContextException
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Runtime.Serialization;

#nullable disable
namespace Microsoft.JScript;

/// <summary>The exception that is thrown when there is no code <see cref="T:Microsoft.JScript.Context" /> associated with a <see cref="T:Microsoft.JScript.JScriptException" />.</summary>
[Serializable]
public class NoContextException : ApplicationException
{
  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.NoContextException" /> class.</summary>
  public NoContextException()
    : base(JScriptException.Localize("No Source Context available", CultureInfo.CurrentUICulture))
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.NoContextException" /> class, using the specified error message.</summary>
  /// <param name="m">The message that describes the error.</param>
  public NoContextException(string m)
    : base(m)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.NoContextException" /> class, using the specified error message and a reference to the inner exception that is the cause of this exception.</summary>
  /// <param name="m">The message that describes the error.</param>
  /// <param name="e">The exception that is the cause of the current exception.</param>
  public NoContextException(string m, Exception e)
    : base(m, e)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.NoContextException" /> class, using the specified serialized data.</summary>
  /// <param name="s">The serialized object data about the exception that is being thrown.</param>
  /// <param name="c">Contextual information about the source or destination.</param>
  protected NoContextException(SerializationInfo s, StreamingContext c)
    : base(s, c)
  {
  }
}
