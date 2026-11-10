// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.BreakOutOfFinally
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents the exception state when code execution breaks out of a finally block.  </summary>
[Serializable]
public sealed class BreakOutOfFinally : ApplicationException
{
  /// <summary>Stores the target of the exception.</summary>
  public int target;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.BreakOutOfFinally" /> class, using the specified target.</summary>
  /// <param name="target">The target of the exception.</param>
  public BreakOutOfFinally(int target) => this.target = target;

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.BreakOutOfFinally" /> class, using the specified error message.</summary>
  /// <param name="m">The message that describes the error.</param>
  public BreakOutOfFinally(string m)
    : base(m)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.BreakOutOfFinally" /> class, using the specified error message and a reference to the inner exception that is the cause of this exception.</summary>
  /// <param name="m">The message that describes the error.</param>
  /// <param name="e">The exception that is the cause of the current exception.</param>
  public BreakOutOfFinally(string m, Exception e)
    : base(m, e)
  {
  }

  private BreakOutOfFinally(SerializationInfo s, StreamingContext c)
    : base(s, c)
  {
    this.target = s.GetInt32("Target");
  }

  /// <summary>Stores information about the exception in the <see cref="T:System.Runtime.Serialization.SerializationInfo" />. The stored information includes the <see cref="F:Microsoft.JScript.ContinueOutOfFinally.target" /> value.</summary>
  /// <param name="s">The serialized object data about the exception that is being thrown.</param>
  /// <param name="c">Contextual information about the source or destination.</param>
  [SecurityCritical]
  [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
  public override void GetObjectData(SerializationInfo s, StreamingContext c)
  {
    base.GetObjectData(s, c);
    s.AddValue("Target", this.target);
  }
}
