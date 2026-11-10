// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VsaItem
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

/// <summary>Represents items added to the .NET script engine. This includes code items, reference items, and global items.</summary>
public abstract class VsaItem : IJSVsaItem
{
    /// <summary>Specifies the name of the item.</summary>
    protected string name;

    internal string codebase;
    internal VsaEngine engine;

    /// <summary>Specifies the type of the item as <see cref="F:Microsoft.Vsa.VsaItemType.Code" />, <see cref="F:Microsoft.Vsa.VsaItemType.Reference" />, or <see cref="F:Microsoft.Vsa.VsaItemType.AppGlobal" />.</summary>
    protected JSVsaItemType type;

    /// <summary>Specifies the type of the item as <see cref="F:Microsoft.Vsa.VsaItemFlag.Class" />, <see cref="F:Microsoft.Vsa.VsaItemFlag.Module" />, or <see cref="F:Microsoft.Vsa.VsaItemFlag.None" />.</summary>
    protected JSVsaItemFlag flag;

    /// <summary>Specifies whether the current in-memory representation of the item differs from the persisted representation.</summary>
    protected bool isDirty;

    internal VsaItem(VsaEngine engine, string itemName, JSVsaItemType type, JSVsaItemFlag flag)
    {
        this.engine = engine;
        this.type = type;
        this.name = itemName;
        this.flag = flag;
        this.codebase = (string)null;
        this.isDirty = true;
    }

    internal virtual void CheckForErrors()
    {
    }

    internal virtual void Close() => this.engine = (VsaEngine)null;

    internal virtual void Compile()
    {
    }

    internal virtual Type GetCompiledType() => (Type)null;

    /// <summary>Gets or sets a value that indicates whether the current in-memory representation of the item differs from the persisted representation.</summary>
    /// <returns>true if the in-memory representation differs from the persisted representation; otherwise, false.</returns>
    public virtual bool IsDirty
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            if (this.engine == null)
                throw new JSVsaException(JSVsaError.EngineClosed);
            return this.isDirty;
        }
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set
        {
            if (this.engine == null)
                throw new JSVsaException(JSVsaError.EngineClosed);
            this.isDirty = value;
        }
    }

    /// <summary>Gets the codebase option.</summary>
    /// <returns>The value of the codebase option.</returns>
    /// <param name="name">The string literal "codebase".</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual object GetOption(string name)
    {
        if (this.engine == null)
            throw new JSVsaException(JSVsaError.EngineClosed);
        if (string.Compare(name, "codebase", StringComparison.OrdinalIgnoreCase) == 0)
            return (object)this.codebase;
        throw new JSVsaException(JSVsaError.OptionNotSupported);
    }

    /// <summary>Gets or sets the name of the item.</summary>
    /// <returns>The string literal name of the item.</returns>
    public virtual string Name
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            if (this.engine == null)
                throw new JSVsaException(JSVsaError.EngineClosed);
            return this.name;
        }
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set
        {
            if (this.engine == null)
                throw new JSVsaException(JSVsaError.EngineClosed);
            if (this.name == value)
                return;
            if (!this.engine.IsValidIdentifier(value))
                throw new JSVsaException(JSVsaError.ItemNameInvalid);
            foreach (IJSVsaItem jsVsaItem in (IEnumerable)this.engine.Items)
            {
                if (jsVsaItem.Name.Equals(value))
                    throw new JSVsaException(JSVsaError.ItemNameInUse);
            }

            this.name = value;
            this.isDirty = true;
            this.engine.IsDirty = true;
        }
    }

    internal virtual void Remove() => this.engine = (VsaEngine)null;

    internal virtual void Reset()
    {
    }

    internal virtual void Run()
    {
    }

    /// <summary>Sets the codebase option.</summary>
    /// <param name="name">The string literal "codebase".</param>
    /// <param name="value">The new value for the option.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual void SetOption(string name, object value)
    {
        if (this.engine == null)
            throw new JSVsaException(JSVsaError.EngineClosed);
        if (string.Compare(name, "codebase", StringComparison.OrdinalIgnoreCase) != 0)
            throw new JSVsaException(JSVsaError.OptionNotSupported);
        this.codebase = (string)value;
        this.isDirty = true;
        this.engine.IsDirty = true;
    }

    /// <summary>Gets the type of the item.</summary>
    /// <returns>One of the enumeration values. The value indicates the type of the item.</returns>
    public JSVsaItemType ItemType
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            if (this.engine == null)
                throw new JSVsaException(JSVsaError.EngineClosed);
            return this.type;
        }
    }
}