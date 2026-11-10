// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.JSVsaException
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>This type supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
[Guid("064C47AC-C9DF-4FCD-9009-E9299D620018")]
[Obsolete("Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[ComVisible(true)]
[Serializable]
[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
public sealed class JSVsaException : ExternalException
{
  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.Vsa.JSVsaException" /> class with a system-supplied message that describes the error. This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
  public JSVsaException()
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.Vsa.JSVsaException" /> class with a specified message that describes the error. This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
  /// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture.</param>
  public JSVsaException(string message)
    : base(message)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.Vsa.JSVsaException" /> class with a specified error message and a reference to the inner exception that is the cause of this exception. This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
  /// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture. </param>
  /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not null, the current exception is raised in a catch block that handles the inner exception. </param>
  public JSVsaException(string message, Exception innerException)
    : base(message, innerException)
  {
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.Vsa.JSVsaException" /> class with serialized data. This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
  /// <param name="info">The object that holds the serialized object data. </param>
  /// <param name="context">The contextual information about the source or destination. </param>
  public JSVsaException(SerializationInfo info, StreamingContext context)
    : base(info, context)
  {
    this.HResult = (int) info.GetValue("VsaException_HResult", typeof (int));
    this.HelpLink = (string) info.GetValue("VsaException_HelpLink", typeof (string));
    this.Source = (string) info.GetValue("VsaException_Source", typeof (string));
  }

  /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.Vsa.JSVsaException" /> class with serialized data. This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
  /// <param name="info">The object that holds the serialized object data. </param>
  /// <param name="context">The contextual information about the source or destination. </param>
  [SecurityCritical]
  [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
  [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
  public override void GetObjectData(SerializationInfo info, StreamingContext context)
  {
    base.GetObjectData(info, context);
    info.AddValue("VsaException_HResult", this.HResult);
    info.AddValue("VsaException_HelpLink", (object) this.HelpLink);
    info.AddValue("VsaException_Source", (object) this.Source);
  }

  /// <summary>This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
  /// <returns>A string representation of the current exception.</returns>
  public override string ToString()
  {
    return this.Message != null && "" != this.Message ? $"Microsoft.JScript.Vsa.JSVsaException: {Enum.GetName(((JSVsaError) this.HResult).GetType(), (object) (JSVsaError) this.HResult)} (0x{string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0,8:X}", new object[1]
    {
      (object) this.HResult
    })}): {this.Message}" : $"Microsoft.JScript.Vsa.JSVsaException: {Enum.GetName(((JSVsaError) this.HResult).GetType(), (object) (JSVsaError) this.HResult)} (0x{string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0,8:X}", new object[1]
    {
      (object) this.HResult
    })}).";
  }

  /// <summary>This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
  /// <param name="error">A <see cref="T:Microsoft.JScript.Vsa.JSVsaError" /> value that describes the reason for the exception.</param>
  public JSVsaException(JSVsaError error)
    : base(string.Empty, (int) error)
  {
  }

  /// <summary>This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
  /// <param name="error">A <see cref="T:Microsoft.JScript.Vsa.JSVsaError" /> value that describes the reason for the exception.</param>
  /// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture. </param>
  public JSVsaException(JSVsaError error, string message)
    : base(message, (int) error)
  {
  }

  /// <summary>This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
  /// <param name="error">A <see cref="T:Microsoft.JScript.Vsa.JSVsaError" /> value that describes the reason for the exception.</param>
  /// <param name="message">The message that describes the exception. The caller of this constructor is required to ensure that this string has been localized for the current system culture. </param>
  /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException" /> parameter is not null, the current exception is raised in a catch block that handles the inner exception. </param>
  public JSVsaException(JSVsaError error, string message, Exception innerException)
    : base(message, innerException)
  {
    this.HResult = (int) error;
  }

  /// <summary>This member supports the .NET Framework infrastructure and is not intended to be used directly from your code.</summary>
  /// <returns>A <see cref="T:Microsoft.JScript.Vsa.JSVsaError" /> value that describes the reason for the exception.</returns>
  public JSVsaError ErrorCode => (JSVsaError) this.HResult;
}
