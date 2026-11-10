// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VsaItems
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Collections;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a collection of <see cref="T:Microsoft.Vsa.IVsaItem" /> objects that can be addressed either by name or by index.</summary>
public sealed class VsaItems : IJSVsaItems, IEnumerable
{
    private ArrayList items;
    private bool isClosed;
    private VsaEngine engine;
    internal int staticCodeBlockCount;

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.VsaItems" /> class.</summary>
    /// <param name="engine">A reference to the scripting engine.</param>
    public VsaItems(VsaEngine engine)
    {
        this.engine = engine;
        this.staticCodeBlockCount = 0;
        this.items = new ArrayList(10);
    }

    /// <summary>Gets the item from the collection that is designated by the specified index value.</summary>
    /// <returns>The item at <paramref name="index" />.</returns>
    /// <param name="index">The index of the item to get.</param>
    public IJSVsaItem this[int index]
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            if (this.isClosed)
                throw new JSVsaException(JSVsaError.EngineClosed);
            if (index < 0 || index >= this.items.Count)
                throw new JSVsaException(JSVsaError.ItemNotFound);
            return (IJSVsaItem)this.items[index];
        }
    }

    /// <summary>Gets the item from the collection that is designated by the specified name.</summary>
    /// <returns>The item that is named <paramref name="itemName" />.</returns>
    /// <param name="itemName">The name of the item to get.</param>
    public IJSVsaItem this[string itemName]
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            if (this.isClosed)
                throw new JSVsaException(JSVsaError.EngineClosed);
            if (itemName != null)
            {
                int index = 0;
                for (int count = this.items.Count; index < count; ++index)
                {
                    IJSVsaItem jsVsaItem = (IJSVsaItem)this.items[index];
                    if (jsVsaItem.Name.Equals(itemName))
                        return jsVsaItem;
                }
            }

            throw new JSVsaException(JSVsaError.ItemNotFound);
        }
    }

    /// <summary>Gets the number of items in the collection.</summary>
    /// <returns>The number of items in the collection.</returns>
    public int Count
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            if (this.isClosed)
                throw new JSVsaException(JSVsaError.EngineClosed);
            return this.items != null ? this.items.Count : 0;
        }
    }

    /// <summary>Disconnects each item in the collection from the scripting engine.</summary>
    public void Close()
    {
        if (this.isClosed)
            throw new JSVsaException(JSVsaError.EngineClosed);
        this.TryObtainLock();
        try
        {
            this.isClosed = true;
            foreach (VsaItem vsaItem in this.items)
                vsaItem.Close();
            this.items = (ArrayList)null;
        }
        finally
        {
            this.ReleaseLock();
            this.engine = (VsaEngine)null;
        }
    }

    /// <summary>Creates a new item and adds it to the collection.</summary>
    /// <returns>The newly created item.</returns>
    /// <param name="name">The name of the new item.</param>
    /// <param name="itemType">One of the enumeration values. The value specifies the type of the new item as <see cref="F:Microsoft.JScript.Vsa.JSVsaItemType.Code" />, <see cref="F:Microsoft.JScript.Vsa.JSVsaItemType.Reference" />, or <see cref="F:Microsoft.JScript.Vsa.JSVsaItemType.AppGlobal" />.</param>
    /// <param name="itemFlag">One of the enumeration values. The value specifies the initial content of a <see cref="F:Microsoft.JScript.Vsa.JSVsaItemType.Code" /> item as <see cref="F:Microsoft.JScript.Vsa.JSVsaItemFlag.Class" />, <see cref="F:Microsoft.JScript.Vsa.JSVsaItemFlag.Module" />, or <see cref="F:Microsoft.JScript.Vsa.JSVsaItemFlag.None" />.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public IJSVsaItem CreateItem(string name, JSVsaItemType itemType, JSVsaItemFlag itemFlag)
    {
        if (this.isClosed)
            throw new JSVsaException(JSVsaError.EngineClosed);
        if (this.engine.IsRunning)
            throw new JSVsaException(JSVsaError.EngineRunning);
        this.TryObtainLock();
        try
        {
            if (itemType != JSVsaItemType.Reference && !this.engine.IsValidIdentifier(name))
                throw new JSVsaException(JSVsaError.ItemNameInvalid);
            foreach (VsaItem vsaItem in this.items)
            {
                if (vsaItem.Name.Equals(name))
                    throw new JSVsaException(JSVsaError.ItemNameInUse);
            }

            IJSVsaItem jsVsaItem = (IJSVsaItem)null;
            switch (itemType)
            {
                case JSVsaItemType.Reference:
                    if (itemFlag != JSVsaItemFlag.None)
                        throw new JSVsaException(JSVsaError.ItemFlagNotSupported);
                    jsVsaItem = (IJSVsaItem)new VsaReference(this.engine, name);
                    break;
                case JSVsaItemType.AppGlobal:
                    if (itemFlag != JSVsaItemFlag.None)
                        throw new JSVsaException(JSVsaError.ItemFlagNotSupported);
                    jsVsaItem = (IJSVsaItem)new VsaHostObject(this.engine, name, JSVsaItemType.AppGlobal);
                    ((VsaHostObject)jsVsaItem).isVisible = true;
                    break;
                case JSVsaItemType.Code:
                    if (itemFlag == JSVsaItemFlag.Class)
                        throw new JSVsaException(JSVsaError.ItemFlagNotSupported);
                    jsVsaItem = (IJSVsaItem)new VsaStaticCode(this.engine, name, itemFlag);
                    ++this.staticCodeBlockCount;
                    break;
            }

            if (jsVsaItem == null)
                throw new JSVsaException(JSVsaError.ItemTypeNotSupported);
            this.items.Add((object)jsVsaItem);
            this.engine.IsDirty = true;
            return jsVsaItem;
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Gets an enumerator that iterates over the collection.</summary>
    /// <returns>An enumerator that iterates over the collection.</returns>
    public IEnumerator GetEnumerator()
    {
        if (this.isClosed)
            throw new JSVsaException(JSVsaError.EngineClosed);
        return this.items.GetEnumerator();
    }

    /// <summary>Removes the item that is designated by the specified name from the collection.</summary>
    /// <param name="itemName">The name of the item to remove.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public void Remove(string itemName)
    {
        if (this.isClosed)
            throw new JSVsaException(JSVsaError.EngineClosed);
        this.TryObtainLock();
        try
        {
            if (itemName == null)
                throw new ArgumentNullException(nameof(itemName));
            int index = 0;
            for (int count = this.items.Count; index < count; ++index)
            {
                IJSVsaItem jsVsaItem = (IJSVsaItem)this.items[index];
                if (jsVsaItem.Name.Equals(itemName))
                {
                    ((VsaItem)jsVsaItem).Remove();
                    this.items.RemoveAt(index);
                    this.engine.IsDirty = true;
                    if (!(jsVsaItem is VsaStaticCode))
                        return;
                    --this.staticCodeBlockCount;
                    return;
                }
            }

            throw new JSVsaException(JSVsaError.ItemNotFound);
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Removes the item that is designated by the specified index value from the collection.</summary>
    /// <param name="itemIndex">The index of the item to remove.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public void Remove(int itemIndex)
    {
        if (this.isClosed)
            throw new JSVsaException(JSVsaError.EngineClosed);
        this.TryObtainLock();
        try
        {
            if (0 > itemIndex || itemIndex >= this.items.Count)
                throw new JSVsaException(JSVsaError.ItemNotFound);
            VsaItem vsaItem = (VsaItem)this.items[itemIndex];
            vsaItem.Remove();
            this.items.RemoveAt(itemIndex);
            if (!(vsaItem is VsaStaticCode))
                return;
            --this.staticCodeBlockCount;
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    private void TryObtainLock() => this.engine.TryObtainLock();

    private void ReleaseLock() => this.engine.ReleaseLock();
}