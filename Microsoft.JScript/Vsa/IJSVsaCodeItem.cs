// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.IJSVsaCodeItem
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.CodeDom;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Represents a code item to be compiled by the script engine.</summary>
[Guid("528BBC87-CCDC-4F07-B29C-9B10575DEB2F")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Obsolete(
    "Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[ComVisible(true)]
public interface IJSVsaCodeItem : IJSVsaItem
{
    /// <summary>Sets or gets the text of a specified code item, including auto-generated code, if any.</summary>
    /// <returns>Returns the source text of the code item.</returns>
    string SourceText
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set;
    }

    /// <summary>Gets the code document object model (CodeDOM) represented in the code item.</summary>
    /// <returns>Returns the CodeDOM for the code item.</returns>
    CodeObject CodeDOM
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>Appends specified text to the end of the code item.</summary>
    /// <param name="text">The text to be appended to the code item.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    void AppendSourceText(string text);

    /// <summary>Adds an event source to the code item. The code item uses the event source to hook up an event to the named event source by calling the IVsaSite.GetEventSourceInstance method, which is implemented by the host.</summary>
    /// <param name="eventSourceName">A programmatic name of the event source.</param>
    /// <param name="eventSourceType">The type name of the event source.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    void AddEventSource(string eventSourceName, string eventSourceType);

    /// <summary>Removes the specified event source from the code item.</summary>
    /// <param name="eventSourceName">The programmatic name of the event source to be removed.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    void RemoveEventSource(string eventSourceName);
}