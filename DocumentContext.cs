// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.DocumentContext
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Diagnostics.SymbolStore;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a source code file that is written in the JScript language.</summary>
public class DocumentContext
{
  internal string documentName;
  internal ISymbolDocumentWriter documentWriter;
  internal int startLine;
  internal int startCol;
  internal int lastLineInSource;
  internal VsaItem sourceItem;
  internal VsaEngine engine;
  internal bool debugOn;
  private CompilerGlobals _compilerGlobals;
  private SimpleHashtable reportedVariables;
  private bool checkForFirst;
  private int firstStartLine;
  private int firstStartCol;
  private int firstEndLine;
  private int firstEndCol;
  internal static readonly Guid language = new Guid("3a12d0b6-c26c-11d0-b442-00a0244a1dd2");
  internal static readonly Guid vendor = new Guid("994b45c4-e6e9-11d2-903f-00c04fa302a1");

  internal DocumentContext(string name, VsaEngine engine)
  {
    this.documentName = name;
    this.documentWriter = (ISymbolDocumentWriter) null;
    this.startLine = 0;
    this.startCol = 0;
    this.lastLineInSource = 0;
    this.sourceItem = (VsaItem) null;
    this.engine = engine;
    this.debugOn = engine != null && engine.GenerateDebugInfo;
    this._compilerGlobals = (CompilerGlobals) null;
    this.reportedVariables = (SimpleHashtable) null;
    this.checkForFirst = false;
  }

  internal DocumentContext(VsaItem sourceItem)
  {
    if (sourceItem.codebase != null)
    {
      this.documentName = sourceItem.codebase;
    }
    else
    {
      string rootMoniker = sourceItem.engine.RootMoniker;
      this.documentName = rootMoniker + (rootMoniker.EndsWith("/", StringComparison.Ordinal) ? "" : "/") + sourceItem.Name;
    }
    this.documentWriter = (ISymbolDocumentWriter) null;
    this.startLine = 0;
    this.startCol = 0;
    this.lastLineInSource = 0;
    this.sourceItem = sourceItem;
    this.engine = sourceItem.engine;
    this.debugOn = this.engine != null && this.engine.GenerateDebugInfo;
    this._compilerGlobals = (CompilerGlobals) null;
    this.checkForFirst = false;
  }

  internal DocumentContext(
    string documentName,
    int startLine,
    int startCol,
    int lastLineInSource,
    VsaItem sourceItem)
  {
    this.documentName = documentName;
    this.documentWriter = (ISymbolDocumentWriter) null;
    this.startLine = startLine;
    this.startCol = startCol;
    this.lastLineInSource = lastLineInSource;
    this.sourceItem = sourceItem;
    this.engine = sourceItem.engine;
    this.debugOn = this.engine != null && this.engine.GenerateDebugInfo;
    this._compilerGlobals = (CompilerGlobals) null;
    this.checkForFirst = false;
  }

  internal CompilerGlobals compilerGlobals
  {
    get
    {
      if (this._compilerGlobals == null)
        this._compilerGlobals = this.engine.CompilerGlobals;
      return this._compilerGlobals;
    }
  }

  internal void EmitLineInfo(ILGenerator ilgen, int line, int column, int endLine, int endColumn)
  {
    if (!this.debugOn)
      return;
    if (this.checkForFirst && line == this.firstStartLine && column == this.firstStartCol && endLine == this.firstEndLine && endColumn == this.firstEndCol)
    {
      this.checkForFirst = false;
    }
    else
    {
      if (this.documentWriter == null)
        this.documentWriter = this.GetSymDocument(this.documentName);
      ilgen.MarkSequencePoint(this.documentWriter, this.startLine + line - this.lastLineInSource, this.startCol + column + 1, this.startLine - this.lastLineInSource + endLine, this.startCol + endColumn + 1);
    }
  }

  internal void EmitFirstLineInfo(
    ILGenerator ilgen,
    int line,
    int column,
    int endLine,
    int endColumn)
  {
    this.EmitLineInfo(ilgen, line, column, endLine, endColumn);
    this.checkForFirst = true;
    this.firstStartLine = line;
    this.firstStartCol = column;
    this.firstEndLine = endLine;
    this.firstEndCol = endColumn;
  }

  private ISymbolDocumentWriter GetSymDocument(string documentName)
  {
    SimpleHashtable documents = this.compilerGlobals.documents;
    object symDocument = documents[(object) documentName];
    if (symDocument == null)
    {
      symDocument = (object) this._compilerGlobals.module.DefineDocument(this.documentName, DocumentContext.language, DocumentContext.vendor, Guid.Empty);
      documents[(object) documentName] = symDocument;
    }
    return (ISymbolDocumentWriter) symDocument;
  }

  internal void HandleError(JScriptException error)
  {
    if (this.sourceItem == null)
    {
      if (error.Severity == 0)
        throw error;
    }
    else if (!this.sourceItem.engine.OnCompilerError(error))
      throw new EndOfFile();
  }

  internal bool HasAlreadySeenErrorFor(string varName)
  {
    if (this.reportedVariables == null)
      this.reportedVariables = new SimpleHashtable(8U);
    else if (this.reportedVariables[(object) varName] != null)
      return true;
    this.reportedVariables[(object) varName] = (object) varName;
    return false;
  }
}
