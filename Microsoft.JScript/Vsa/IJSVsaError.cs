// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.IJSVsaError
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Provides access to compilation errors encountered during execution.</summary>
[Guid("425EA439-6417-4F3E-BCC9-1AFAC79E3F66")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Obsolete(
    "Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[ComVisible(true)]
public interface IJSVsaError
{
    /// <summary>Gets the line number on which an error occurs.</summary>
    /// <returns>Returns the line number on which the error has occurred.</returns>
    int Line
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>Sets the severity of the error.</summary>
    /// <returns>Returns an integer (0-4) that represents the error severity.</returns>
    int Severity
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>Gets a brief description of the error, in some instances returning a reference to the token in the source code that is causing the error.</summary>
    /// <returns>Returns a string literal description of the error.</returns>
    string Description
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>Gets the text of the source code from the line that caused the error.</summary>
    /// <returns>Returns the string literal source code from the line that caused the error.</returns>
    string LineText
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>Gets a reference to the IVsaItem object that generated the error.</summary>
    /// <returns>Returns a reference to the object that generated the error.</returns>
    IJSVsaItem SourceItem
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>Gets the ending column number for the source text that caused the error, if available.</summary>
    /// <returns>Returns the ending column number for the source text that caused the error, if available.</returns>
    int EndColumn
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>Gets the starting column number for the source text that caused the error, if available.</summary>
    /// <returns>Returns the starting column number for the source text that caused the error, if available.</returns>
    int StartColumn
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>Gets a number that uniquely identifies the error.</summary>
    /// <returns>Returns the number that uniquely identifies the error.</returns>
    int Number
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>Gets the fully qualified name of the source item that contained the error, in a format recognizable by the script engine.</summary>
    /// <returns>Returns the fully qualified name of the source item that contained the error.</returns>
    string SourceMoniker
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }
}