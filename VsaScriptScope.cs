// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VsaScriptScope
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.Collections;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

internal class VsaScriptScope : VsaItem, IVsaScriptScope, IJSVsaItem, IDebugScriptScope
{
  private VsaScriptScope parent;
  private GlobalScope scope;
  private ArrayList items;
  private bool isCompiled;
  private bool isClosed;

  internal VsaScriptScope(VsaEngine engine, string itemName, VsaScriptScope parent)
    : base(engine, itemName, (JSVsaItemType) 19, JSVsaItemFlag.None)
  {
    this.parent = parent;
    this.scope = (GlobalScope) null;
    this.items = new ArrayList(8);
    this.isCompiled = false;
    this.isClosed = false;
  }

  public virtual object GetObject()
  {
    if (this.scope == null)
      this.scope = this.parent == null ? new GlobalScope((GlobalScope) null, this.engine) : new GlobalScope((GlobalScope) this.parent.GetObject(), this.engine, false);
    return (object) this.scope;
  }

  public IVsaScriptScope Parent => (IVsaScriptScope) this.parent;

  public virtual IJSVsaItem AddItem(string itemName, JSVsaItemType type)
  {
    VsaItem vsaItem = (VsaItem) null;
    if (this.isClosed)
      throw new JSVsaException(JSVsaError.EngineClosed);
    if (this.GetItem(itemName) != null)
      throw new JSVsaException(JSVsaError.ItemNameInUse);
    switch (type)
    {
      case (JSVsaItemType) 16 /*0x10*/:
      case (JSVsaItemType) 17:
      case (JSVsaItemType) 18:
        vsaItem = (VsaItem) new VsaHostObject(this.engine, itemName, type, this);
        if (type == (JSVsaItemType) 17 || type == (JSVsaItemType) 18)
          ((VsaHostObject) vsaItem).exposeMembers = true;
        if (type == (JSVsaItemType) 16 /*0x10*/ || type == (JSVsaItemType) 18)
          ((VsaHostObject) vsaItem).isVisible = true;
        if (this.engine.IsRunning)
        {
          vsaItem.Compile();
          vsaItem.Run();
          break;
        }
        break;
      case (JSVsaItemType) 19:
        vsaItem = (VsaItem) new VsaScriptScope(this.engine, itemName, this);
        break;
      case (JSVsaItemType) 20:
        vsaItem = (VsaItem) new VsaScriptCode(this.engine, itemName, type, (IVsaScriptScope) this);
        break;
      case (JSVsaItemType) 21:
        if (!this.engine.IsRunning)
          throw new JSVsaException(JSVsaError.EngineNotRunning);
        vsaItem = (VsaItem) new VsaScriptCode(this.engine, itemName, type, (IVsaScriptScope) this);
        break;
      case (JSVsaItemType) 22:
        if (!this.engine.IsRunning)
          throw new JSVsaException(JSVsaError.EngineNotRunning);
        vsaItem = (VsaItem) new VsaScriptCode(this.engine, itemName, type, (IVsaScriptScope) this);
        break;
    }
    if (vsaItem == null)
      throw new JSVsaException(JSVsaError.ItemTypeNotSupported);
    this.items.Add((object) vsaItem);
    return (IJSVsaItem) vsaItem;
  }

  public virtual IJSVsaItem GetItem(string itemName)
  {
    int index = 0;
    for (int count = this.items.Count; index < count; ++index)
    {
      VsaItem vsaItem = (VsaItem) this.items[index];
      if (vsaItem.Name == null && itemName == null || vsaItem.Name != null && vsaItem.Name.Equals(itemName))
        return (IJSVsaItem) this.items[index];
    }
    return (IJSVsaItem) null;
  }

  public virtual void RemoveItem(string itemName)
  {
    int index = 0;
    for (int count = this.items.Count; index < count; ++index)
    {
      VsaItem vsaItem = (VsaItem) this.items[index];
      if (vsaItem.Name == null && itemName == null || vsaItem.Name != null && vsaItem.Name.Equals(itemName))
      {
        vsaItem.Remove();
        this.items.Remove((object) index);
        return;
      }
    }
    throw new JSVsaException(JSVsaError.ItemNotFound);
  }

  public virtual void RemoveItem(IJSVsaItem item)
  {
    int index = 0;
    for (int count = this.items.Count; index < count; ++index)
    {
      VsaItem vsaItem = (VsaItem) this.items[index];
      if (vsaItem == item)
      {
        vsaItem.Remove();
        this.items.Remove((object) index);
        return;
      }
    }
    throw new JSVsaException(JSVsaError.ItemNotFound);
  }

  public virtual int GetItemCount() => this.items.Count;

  public virtual IJSVsaItem GetItemAtIndex(int index)
  {
    if (index < this.items.Count)
      return (IJSVsaItem) this.items[index];
    throw new JSVsaException(JSVsaError.ItemNotFound);
  }

  public virtual void RemoveItemAtIndex(int index)
  {
    if (index >= this.items.Count)
      throw new JSVsaException(JSVsaError.ItemNotFound);
    ((VsaItem) this.items[index]).Remove();
    this.items.Remove((object) index);
  }

  public virtual IJSVsaItem CreateDynamicItem(string itemName, JSVsaItemType type)
  {
    if (this.engine.IsRunning)
      return this.AddItem(itemName, type);
    throw new JSVsaException(JSVsaError.EngineNotRunning);
  }

  internal override void CheckForErrors()
  {
    if (this.items.Count == 0)
      return;
    try
    {
      this.engine.Globals.ScopeStack.Push((object) (ScriptObject) this.GetObject());
      foreach (VsaItem vsaItem in this.items)
        vsaItem.CheckForErrors();
    }
    finally
    {
      this.engine.Globals.ScopeStack.Pop();
    }
  }

  internal override void Compile()
  {
    if (this.items.Count == 0 || this.isCompiled)
      return;
    this.isCompiled = true;
    try
    {
      this.engine.Globals.ScopeStack.Push((object) (ScriptObject) this.GetObject());
      try
      {
        foreach (VsaItem vsaItem in this.items)
          vsaItem.Compile();
      }
      finally
      {
        this.engine.Globals.ScopeStack.Pop();
      }
    }
    catch
    {
      this.isCompiled = false;
      throw;
    }
  }

  internal override void Reset()
  {
    foreach (VsaItem vsaItem in this.items)
      vsaItem.Reset();
  }

  internal void ReRun(GlobalScope scope)
  {
    foreach (object obj in this.items)
    {
      if (obj is VsaHostObject)
        ((VsaHostObject) obj).ReRun(scope);
    }
    if (this.parent == null)
      return;
    this.parent.ReRun(scope);
  }

  internal override void Run()
  {
    if (this.items.Count == 0)
      return;
    try
    {
      this.engine.Globals.ScopeStack.Push((object) (ScriptObject) this.GetObject());
      foreach (VsaItem vsaItem in this.items)
        vsaItem.Run();
    }
    finally
    {
      this.engine.Globals.ScopeStack.Pop();
    }
  }

  internal override void Close()
  {
    foreach (VsaItem vsaItem in this.items)
      vsaItem.Close();
    this.items = (ArrayList) null;
    this.parent = (VsaScriptScope) null;
    this.scope = (GlobalScope) null;
    this.isClosed = true;
  }

  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public virtual void SetThisValue(object thisValue)
  {
    if (this.scope == null)
      return;
    this.scope.thisObject = thisValue;
  }
}
