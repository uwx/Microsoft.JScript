// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.IJSVsaItems
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Defines an interface for a collection of <see cref="T:Microsoft.JScript.Vsa.IJSVsaItem" /> objects, which can be addressed either by name or by index.</summary>
[Guid("172341E0-9B0D-43E6-9EFF-75E030A46461")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
[Obsolete(
    "Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[ComVisible(true)]
public interface IJSVsaItems : IEnumerable
{
    /// <summary>Gets the number of items in the specified collection.</summary>
    /// <returns>Returns the integer value for the number of items in the collection.</returns>
    int Count
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>Gets an item from the collection by its name.</summary>
    /// <returns>Returns the item specified by name.</returns>
    /// <param name="name">The name of the item to retrieve from the collection. </param>
    IJSVsaItem this[string name]
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>Gets an item from the collection by its index value.</summary>
    /// <returns>Returns the item at the specified index.</returns>
    /// <param name="index">A 0-based index of the retrievable items.</param>
    IJSVsaItem this[int index]
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get;
    }

    /// <summary>Creates a new instance of one of the <see cref="T:Microsoft.JScript.Vsa.IJSVsaItem" /> types, as defined in the <see cref="T:Microsoft.JScript.Vsa.JSVsaItemType" /> enumeration.</summary>
    /// <returns>Returns a reference to the <see cref="T:Microsoft.JScript.Vsa.IJSVsaItem" /> object created.</returns>
    /// <param name="name">The name to associate with the new item. In cases where the item is a reference item type, the name parameter must be exactly the same as the name of the referenced assembly, as set with the <see cref="P:Microsoft.JScript.Vsa.IJSVsaReferenceItem.AssemblyName" /> property. In JScript, however, if you do not specify an AssemblyName, JScript will use the ItemName as the name of the assembly.</param>
    /// <param name="itemType">The type of item created, as defined in the <see cref="T:Microsoft.JScript.Vsa.JSVsaItemType" /> enumeration.</param>
    /// <param name="itemFlag">The optional flag to specify the initial content of a Code item. </param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    IJSVsaItem CreateItem(string name, JSVsaItemType itemType, JSVsaItemFlag itemFlag);

    /// <summary>Removes an item from the collection, as specified by its name.</summary>
    /// <param name="name">The name of the item to be removed from the collection.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    void Remove(string name);

    /// <summary>Removes an item from the collection, as specified by its index value.</summary>
    /// <param name="index">The index value of the item to be removed. </param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    void Remove(int index);
}