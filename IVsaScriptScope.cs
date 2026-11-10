// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IVsaScriptScope
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods for resolving scope. The global scope object of the <see cref="T:Microsoft.JScript.Vsa.VsaEngine" /> implements this interface.</summary>
[Guid("ED4BAE22-2F3C-419a-B487-CF869E716B95")]
[ComVisible(true)]
public interface IVsaScriptScope : IJSVsaItem
{
  /// <summary>When implemented in a class, gets a reference to the parent object.</summary>
  /// <returns>The parent scope.</returns>
  IVsaScriptScope Parent { get; }

  /// <summary>When implemented in a class, adds an item to the scope. </summary>
  /// <returns>The new item, if it is successfully added.</returns>
  /// <param name="itemName">The name of the item.</param>
  /// <param name="type">One of the enumeration values. Specifies the type of the item to add.</param>
  IJSVsaItem AddItem(string itemName, JSVsaItemType type);

  /// <summary>When implemented in a class, gets the item from the scope that is designated by the specified name.</summary>
  /// <returns>The item that is named <paramref name="itemName" />.</returns>
  /// <param name="itemName">The name of the item to get.</param>
  IJSVsaItem GetItem(string itemName);

  /// <summary>When implemented in a class, removes the item that is designated by the specified name from the scope.</summary>
  /// <param name="itemName">The name of the item to remove.</param>
  void RemoveItem(string itemName);

  /// <summary>When implemented in a class, removes the specified <see cref="T:Microsoft.JScript.Vsa.IJSVsaItem" /> object from the scope.</summary>
  /// <param name="item">The item to remove.</param>
  void RemoveItem(IJSVsaItem item);

  /// <summary>When implemented in a class, gets the number of items in the scope.</summary>
  /// <returns>The number of items in the scope.</returns>
  int GetItemCount();

  /// <summary>When implemented in a class, gets the item from the scope that is designated by the specified index value.</summary>
  /// <returns>The item at <paramref name="index" />.</returns>
  /// <param name="index">The index of the item to get.</param>
  IJSVsaItem GetItemAtIndex(int index);

  /// <summary>When implemented in a class, removes the item that is designated by the specified index value from the scope.</summary>
  /// <param name="index">The index of the item to remove.</param>
  void RemoveItemAtIndex(int index);

  /// <summary>When implemented in a class, gets a reference to the global scope.</summary>
  /// <returns>A reference to the global scope.</returns>
  object GetObject();

  /// <summary>When implemented in a class, adds an item to the scope. The <see cref="T:Microsoft.JScript.Vsa.VsaEngine" /> must be running to use this method.</summary>
  /// <returns>The new item, if it is successfully added.</returns>
  /// <param name="itemName">The name of the item.</param>
  /// <param name="type">One of the enumeration values. Specifies the type of the item to add.</param>
  IJSVsaItem CreateDynamicItem(string itemName, JSVsaItemType type);
}
