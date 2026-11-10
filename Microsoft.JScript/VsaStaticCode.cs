// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VsaStaticCode
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.CodeDom;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal class VsaStaticCode : VsaItem, IJSVsaCodeItem, IJSVsaItem
{
    internal Context codeContext;
    private Type compiledClass;
    private ScriptBlock block;

    internal VsaStaticCode(VsaEngine engine, string itemName, JSVsaItemFlag flag)
        : base(engine, itemName, JSVsaItemType.Code, flag)
    {
        this.compiledClass = (Type)null;
        this.codeContext = new Context(new DocumentContext((VsaItem)this), "");
    }

    public void AddEventSource(string eventSourceName, string eventSourceType)
    {
        if (this.engine == null)
            throw new JSVsaException(JSVsaError.EngineClosed);
        throw new NotSupportedException();
    }

    public CodeObject CodeDOM
    {
        get
        {
            if (this.engine == null)
                throw new JSVsaException(JSVsaError.EngineClosed);
            throw new JSVsaException(JSVsaError.CodeDOMNotAvailable);
        }
    }

    public void AppendSourceText(string SourceCode)
    {
        if (this.engine == null)
            throw new JSVsaException(JSVsaError.EngineClosed);
        switch (SourceCode)
        {
            case null:
                break;
            case "":
                break;
            default:
                this.codeContext.SetSourceContext(this.codeContext.document,
                    this.codeContext.source_string + SourceCode);
                this.compiledClass = (Type)null;
                this.isDirty = true;
                this.engine.IsDirty = true;
                break;
        }
    }

    internal override void CheckForErrors()
    {
        if (!(this.compiledClass == (Type)null))
            return;
        AST ast = (AST)new JSParser(this.codeContext).Parse();
    }

    internal override void Close()
    {
        base.Close();
        this.codeContext = (Context)null;
        this.compiledClass = (Type)null;
    }

    internal override Type GetCompiledType()
    {
        TypeBuilder compiledClass = this.compiledClass as TypeBuilder;
        if ((Type)compiledClass != (Type)null)
            this.compiledClass = compiledClass.CreateType();
        return this.compiledClass;
    }

    public override string Name
    {
        set
        {
            base.Name = value;
            if (this.codebase != null)
                return;
            string rootMoniker = this.engine.RootMoniker;
            this.codeContext.document.documentName = rootMoniker +
                                                     (rootMoniker.EndsWith("/", StringComparison.Ordinal) ? "" : "/") +
                                                     this.name;
        }
    }

    internal void Parse()
    {
        if (this.block != null || !(this.compiledClass == (Type)null))
            return;
        GlobalScope globalScope = (GlobalScope)this.engine.GetGlobalScope().GetObject();
        globalScope.evilScript = !globalScope.fast || this.engine.GetStaticCodeBlockCount() > 1;
        this.engine.Globals.ScopeStack.Push((object)globalScope);
        try
        {
            JSParser jsParser = new JSParser(this.codeContext);
            this.block = jsParser.Parse();
            if (!jsParser.HasAborted)
                return;
            this.block = (ScriptBlock)null;
        }
        finally
        {
            this.engine.Globals.ScopeStack.Pop();
        }
    }

    internal void ProcessAssemblyAttributeLists()
    {
        if (this.block == null)
            return;
        this.block.ProcessAssemblyAttributeLists();
    }

    internal void PartiallyEvaluate()
    {
        if (this.block == null || !(this.compiledClass == (Type)null))
            return;
        this.engine.Globals.ScopeStack.Push((object)(GlobalScope)this.engine.GetGlobalScope().GetObject());
        try
        {
            this.block.PartiallyEvaluate();
            if (this.engine.HasErrors && !this.engine.alwaysGenerateIL)
                throw new EndOfFile();
        }
        finally
        {
            this.engine.Globals.ScopeStack.Pop();
        }
    }

    internal override void Remove()
    {
        if (this.engine == null)
            throw new JSVsaException(JSVsaError.EngineClosed);
        base.Remove();
    }

    public void RemoveEventSource(string eventSourceName)
    {
        if (this.engine == null)
            throw new JSVsaException(JSVsaError.EngineClosed);
        throw new NotSupportedException();
    }

    internal override void Reset()
    {
        this.compiledClass = (Type)null;
        this.block = (ScriptBlock)null;
        this.codeContext = new Context(new DocumentContext((VsaItem)this), this.codeContext.source_string);
    }

    internal override void Run()
    {
        if (!(this.compiledClass != (Type)null))
            return;
        GlobalScope instance =
            (GlobalScope)Activator.CreateInstance(this.GetCompiledType(), this.engine.GetGlobalScope().GetObject());
        this.engine.Globals.ScopeStack.Push((object)instance);
        try
        {
            MethodInfo method = this.compiledClass.GetMethod("Global Code");
            try
            {
                method.Invoke((object)instance, (object[])null);
            }
            catch (TargetInvocationException ex)
            {
                throw ex.InnerException;
            }
        }
        finally
        {
            this.engine.Globals.ScopeStack.Pop();
        }
    }

    public override void SetOption(string name, object value)
    {
        if (this.engine == null)
            throw new JSVsaException(JSVsaError.EngineClosed);
        if (string.Compare(name, "codebase", StringComparison.OrdinalIgnoreCase) != 0)
            throw new JSVsaException(JSVsaError.OptionNotSupported);
        this.codebase = (string)value;
        this.codeContext.document.documentName = this.codebase;
        this.isDirty = true;
        this.engine.IsDirty = true;
    }

    public object SourceContext
    {
        get => (object)null;
        set { }
    }

    public string SourceText
    {
        get
        {
            if (this.engine == null)
                throw new JSVsaException(JSVsaError.EngineClosed);
            return this.codeContext.source_string;
        }
        set
        {
            if (this.engine == null)
                throw new JSVsaException(JSVsaError.EngineClosed);
            this.codeContext.SetSourceContext(this.codeContext.document, value == null ? "" : value);
            this.compiledClass = (Type)null;
            this.isDirty = true;
            this.engine.IsDirty = true;
        }
    }

    internal void TranslateToIL()
    {
        if (this.block == null || !(this.compiledClass == (Type)null))
            return;
        this.engine.Globals.ScopeStack.Push((object)(GlobalScope)this.engine.GetGlobalScope().GetObject());
        try
        {
            this.compiledClass = (Type)this.block.TranslateToILClass(this.engine.CompilerGlobals, false);
        }
        finally
        {
            this.engine.Globals.ScopeStack.Pop();
        }
    }
}