// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VsaScriptCode
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.CodeDom;
using System.Reflection;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

internal class VsaScriptCode : 
  VsaItem,
  IVsaScriptCodeItem,
  IJSVsaCodeItem,
  IJSVsaItem,
  IDebugVsaScriptCodeItem
{
  private Context codeContext;
  private ScriptBlock binaryCode;
  internal bool executed;
  private VsaScriptScope scope;
  private Type compiledBlock;
  private bool compileToIL;
  private bool optimize;

  [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
  internal VsaScriptCode(
    VsaEngine engine,
    string itemName,
    JSVsaItemType type,
    IVsaScriptScope scope)
    : base(engine, itemName, type, JSVsaItemFlag.None)
  {
    this.binaryCode = (ScriptBlock) null;
    this.executed = false;
    this.scope = (VsaScriptScope) scope;
    this.codeContext = new Context(new DocumentContext((VsaItem) this), (string) null);
    this.compiledBlock = (Type) null;
    this.compileToIL = true;
    this.optimize = true;
  }

  internal override void Close()
  {
    base.Close();
    this.binaryCode = (ScriptBlock) null;
    this.scope = (VsaScriptScope) null;
    this.codeContext = (Context) null;
    this.compiledBlock = (Type) null;
  }

  public CodeObject CodeDOM => throw new JSVsaException(JSVsaError.CodeDOMNotAvailable);

  internal override void Compile()
  {
    if (this.binaryCode != null)
      return;
    JSParser jsParser = new JSParser(this.codeContext);
    this.binaryCode = this.ItemType != (JSVsaItemType) 22 ? jsParser.Parse() : jsParser.ParseExpressionItem();
    if (this.optimize && !jsParser.HasAborted)
    {
      this.binaryCode.ProcessAssemblyAttributeLists();
      this.binaryCode.PartiallyEvaluate();
    }
    if (this.engine.HasErrors && !this.engine.alwaysGenerateIL)
      throw new EndOfFile();
    if (!this.compileToIL)
      return;
    this.compiledBlock = this.binaryCode.TranslateToILClass(this.engine.CompilerGlobals).CreateType();
  }

  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public virtual object Execute()
  {
    if (!this.engine.IsRunning)
      throw new JSVsaException(JSVsaError.EngineNotRunning);
    this.engine.Globals.ScopeStack.Push((object) (ScriptObject) this.scope.GetObject());
    try
    {
      this.Compile();
      return this.RunCode();
    }
    finally
    {
      this.engine.Globals.ScopeStack.Pop();
    }
  }

  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public virtual object Evaluate() => this.Execute();

  public override string Name
  {
    set
    {
      this.name = value;
      if (this.codebase != null)
        return;
      string rootMoniker = this.engine.RootMoniker;
      this.codeContext.document.documentName = rootMoniker + (rootMoniker.EndsWith("/", StringComparison.Ordinal) ? "" : "/") + this.name;
    }
  }

  [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
  public virtual bool ParseNamedBreakPoint(
    string input,
    out string functionName,
    out int nargs,
    out string arguments,
    out string returnType,
    out ulong offset)
  {
    functionName = "";
    nargs = 0;
    arguments = "";
    returnType = "";
    offset = 0UL;
    string[] namedBreakpoint = new JSParser(this.codeContext).ParseNamedBreakpoint(out nargs);
    if (namedBreakpoint == null || namedBreakpoint.Length != 4)
      return false;
    if (namedBreakpoint[0] != null)
      functionName = namedBreakpoint[0];
    if (namedBreakpoint[1] != null)
      arguments = namedBreakpoint[1];
    if (namedBreakpoint[2] != null)
      returnType = namedBreakpoint[2];
    if (namedBreakpoint[3] != null)
      offset = ((IConvertible) Convert.LiteralToNumber(namedBreakpoint[3])).ToUInt64((IFormatProvider) null);
    return true;
  }

  internal override Type GetCompiledType() => this.compiledBlock;

  public override object GetOption(string name)
  {
    if (string.Compare(name, "il", StringComparison.OrdinalIgnoreCase) == 0)
      return (object) this.compileToIL;
    return string.Compare(name, "optimize", StringComparison.OrdinalIgnoreCase) == 0 ? (object) this.optimize : base.GetOption(name);
  }

  internal override void Reset()
  {
    this.binaryCode = (ScriptBlock) null;
    this.compiledBlock = (Type) null;
    this.executed = false;
    this.codeContext = new Context(new DocumentContext((VsaItem) this), this.codeContext.source_string);
  }

  internal override void Run()
  {
    if (this.executed)
      return;
    this.RunCode();
  }

  private object RunCode()
  {
    if (this.binaryCode == null)
      return (object) null;
    object obj;
    if ((Type) null != this.compiledBlock)
    {
      GlobalScope instance = (GlobalScope) Activator.CreateInstance(this.compiledBlock, this.scope.GetObject());
      this.scope.ReRun(instance);
      MethodInfo method = this.compiledBlock.GetMethod("Global Code");
      try
      {
        Microsoft.JScript.JScriptCallContext.SetData("JScript:" + this.compiledBlock.Assembly.FullName, (object) this.engine);
        obj = method.Invoke((object) instance, (object[]) null);
      }
      catch (TargetInvocationException ex)
      {
        throw ex.InnerException;
      }
    }
    else
      obj = this.binaryCode.Evaluate();
    this.executed = true;
    return obj;
  }

  public IVsaScriptScope Scope => (IVsaScriptScope) this.scope;

  public override void SetOption(string name, object value)
  {
    if (string.Compare(name, "il", StringComparison.OrdinalIgnoreCase) == 0)
    {
      this.compileToIL = (bool) value;
      if (!this.compileToIL)
        return;
      this.optimize = true;
    }
    else if (string.Compare(name, "optimize", StringComparison.OrdinalIgnoreCase) == 0)
    {
      this.optimize = (bool) value;
      if (this.optimize)
        return;
      this.compileToIL = false;
    }
    else if (string.Compare(name, "codebase", StringComparison.OrdinalIgnoreCase) == 0)
    {
      this.codebase = (string) value;
      this.codeContext.document.documentName = this.codebase;
    }
    else
      base.SetOption(name, value);
  }

  public object SourceContext
  {
    get => (object) null;
    set
    {
    }
  }

  public void AddEventSource(string EventSourceName, string EventSourceType)
  {
  }

  public void RemoveEventSource(string EventSourceName)
  {
  }

  public string SourceText
  {
    get => this.codeContext.source_string;
    set
    {
      this.codeContext.SetSourceContext(this.codeContext.document, value == null ? "" : value);
      this.executed = false;
      this.binaryCode = (ScriptBlock) null;
    }
  }

  public void AppendSourceText(string SourceCode)
  {
    switch (SourceCode)
    {
      case null:
        break;
      case "":
        break;
      default:
        this.codeContext.SetSourceContext(this.codeContext.document, this.codeContext.source_string + SourceCode);
        this.executed = false;
        this.binaryCode = (ScriptBlock) null;
        break;
    }
  }

  public int StartColumn
  {
    get => this.codeContext.document.startCol;
    set => this.codeContext.document.startCol = value;
  }

  public int StartLine
  {
    get => this.codeContext.document.startLine;
    set => this.codeContext.document.startLine = value;
  }
}
