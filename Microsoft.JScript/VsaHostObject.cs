// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VsaHostObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class VsaHostObject : VsaItem, IJSVsaGlobalItem, IJSVsaItem
{
    private object hostObject;
    internal bool exposeMembers;
    internal bool isVisible;
    private bool exposed;
    private bool compiled;
    private VsaScriptScope scope;
    private FieldInfo field;
    private string typeString;

    internal VsaHostObject(VsaEngine engine, string itemName, JSVsaItemType type)
        : this(engine, itemName, type, (VsaScriptScope)null)
    {
    }

    internal VsaHostObject(
        VsaEngine engine,
        string itemName,
        JSVsaItemType type,
        VsaScriptScope scope)
        : base(engine, itemName, type, JSVsaItemFlag.None)
    {
        this.hostObject = (object)null;
        this.exposeMembers = false;
        this.isVisible = false;
        this.exposed = false;
        this.compiled = false;
        this.scope = scope;
        this.field = (FieldInfo)null;
        this.typeString = "System.Object";
    }

    public bool ExposeMembers
    {
        get
        {
            if (this.engine == null)
                throw new JSVsaException(JSVsaError.EngineClosed);
            return this.exposeMembers;
        }
        set
        {
            if (this.engine == null)
                throw new JSVsaException(JSVsaError.EngineClosed);
            this.exposeMembers = value;
        }
    }

    internal FieldInfo Field
    {
        get
        {
            JSVariableField field = this.field as JSVariableField;
            return (FieldInfo)field != (FieldInfo)null ? (FieldInfo)field.GetMetaData() : this.field;
        }
    }

    public object GetObject()
    {
        if (this.engine == null)
            throw new JSVsaException(JSVsaError.EngineClosed);
        if (this.hostObject == null)
        {
            if (this.engine.Site == null)
                throw new JSVsaException(JSVsaError.SiteNotSet);
            this.hostObject = this.engine.Site.GetGlobalInstance(this.name);
        }

        return this.hostObject;
    }

    private void AddNamedItemNamespace()
    {
        GlobalScope parent1 = (GlobalScope)this.Scope.GetObject();
        if (parent1.isComponentScope)
            parent1 = (GlobalScope)parent1.GetParent();
        ScriptObject parent2 = parent1.GetParent();
        VsaNamedItemScope parent3 = new VsaNamedItemScope(this.GetObject(), parent2, this.engine);
        parent1.SetParent((ScriptObject)parent3);
        parent3.SetParent(parent2);
    }

    private void RemoveNamedItemNamespace()
    {
        ScriptObject scriptObject = (ScriptObject)this.Scope.GetObject();
        for (ScriptObject parent = scriptObject.GetParent(); parent != null; parent = parent.GetParent())
        {
            if (parent is VsaNamedItemScope && ((VsaNamedItemScope)parent).namedItem == this.hostObject)
            {
                scriptObject.SetParent(parent.GetParent());
                break;
            }

            scriptObject = parent;
        }
    }

    internal override void Remove()
    {
        base.Remove();
        if (!this.exposed)
            return;
        if (this.exposeMembers)
            this.RemoveNamedItemNamespace();
        if (this.isVisible)
            ((ScriptObject)this.Scope.GetObject()).DeleteMember(this.name);
        this.hostObject = (object)null;
        this.exposed = false;
    }

    internal override void CheckForErrors() => this.Compile();

    internal override void Compile()
    {
        if (this.compiled || !this.isVisible)
            return;
        JSVariableField jsVariableField =
            ((ActivationObject)this.Scope.GetObject()).AddFieldOrUseExistingField(this.name, (object)null,
                FieldAttributes.Public | FieldAttributes.Static);
        Type type = this.engine.GetType(this.typeString);
        if (type != (Type)null)
            jsVariableField.type = new TypeExpression((AST)new ConstantWrapper((object)type, (Context)null));
        this.field = (FieldInfo)jsVariableField;
    }

    internal override void Run()
    {
        if (this.exposed)
            return;
        if (this.isVisible)
            this.field = (FieldInfo)((ActivationObject)this.Scope.GetObject()).AddFieldOrUseExistingField(this.name,
                this.GetObject(), FieldAttributes.Public | FieldAttributes.Static);
        if (this.exposeMembers)
            this.AddNamedItemNamespace();
        this.exposed = true;
    }

    internal void ReRun(GlobalScope scope)
    {
        if (!(this.field is JSGlobalField))
            return;
        ((JSGlobalField)this.field).ILField = scope.GetField(this.name, BindingFlags.Static | BindingFlags.Public);
        this.field.SetValue((object)scope, this.GetObject());
        this.field = (FieldInfo)null;
    }

    internal override void Reset()
    {
        base.Reset();
        this.hostObject = (object)null;
        this.exposed = false;
        this.compiled = false;
        this.scope = (VsaScriptScope)null;
    }

    private VsaScriptScope Scope
    {
        get
        {
            if (this.scope == null)
                this.scope = (VsaScriptScope)this.engine.GetGlobalScope();
            return this.scope;
        }
    }

    internal override void Close()
    {
        this.Remove();
        base.Close();
        this.hostObject = (object)null;
        this.scope = (VsaScriptScope)null;
    }

    public string TypeString
    {
        get
        {
            if (this.engine == null)
                throw new JSVsaException(JSVsaError.EngineClosed);
            return this.typeString;
        }
        set
        {
            if (this.engine == null)
                throw new JSVsaException(JSVsaError.EngineClosed);
            this.typeString = value;
            this.isDirty = true;
            this.engine.IsDirty = true;
        }
    }
}