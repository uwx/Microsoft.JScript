// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.IJSVsaGlobalItem
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Describes global objects added to the script engine.</summary>
[Guid("15B2CCE5-D1EA-4EB9-9E06-8729C72D631B")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Obsolete(
    "Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[ComVisible(true)]
public interface IJSVsaGlobalItem : IJSVsaItem
{
    /// <summary>Gets or sets the type of the global item.</summary>
    /// <returns>Returns the item type of the global item.</returns>
    string TypeString
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set;
    }

    /// <summary>Sets a value indicating whether the members of the global object should be made available to the script engine. [Not presently supported.]</summary>
    /// <returns>Returns TRUE if public members of the global object are available to the script engine without qualification, as if they are part of the global namespace. Returns FALSE if a member of the global object must be qualified with the object's name.</returns>
    bool ExposeMembers
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set;
    }
}