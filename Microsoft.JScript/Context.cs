// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Context
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents JScript source code. This class belongs to the compilation and run-time state category.</summary>
public class Context
{
    internal DocumentContext document;
    internal string source_string;
    internal int lineNumber;
    internal int startLinePos;
    internal int startPos;
    internal int endLineNumber;
    internal int endLinePos;
    internal int endPos;
    internal JSToken token;
    internal int errorReported;

    internal Context(DocumentContext document, string source_string)
    {
        this.document = document;
        this.source_string = source_string;
        this.lineNumber = 1;
        this.startLinePos = 0;
        this.startPos = 0;
        this.endLineNumber = 1;
        this.endLinePos = 0;
        this.endPos = source_string == null ? -1 : source_string.Length;
        this.token = JSToken.None;
        this.errorReported = 1000000;
    }

    internal Context(
        DocumentContext document,
        string source_string,
        int lineNumber,
        int startLinePos,
        int startPos,
        int endLineNumber,
        int endLinePos,
        int endPos,
        JSToken token)
    {
        this.document = document;
        this.source_string = source_string;
        this.lineNumber = lineNumber;
        this.startLinePos = startLinePos;
        this.startPos = startPos;
        this.endLineNumber = endLineNumber;
        this.endLinePos = endLinePos;
        this.endPos = endPos;
        this.token = token;
        this.errorReported = 1000000;
    }

    internal Context Clone()
    {
        return new Context(this.document, this.source_string, this.lineNumber, this.startLinePos, this.startPos,
            this.endLineNumber, this.endLinePos, this.endPos, this.token)
        {
            errorReported = this.errorReported
        };
    }

    internal Context CombineWith(Context other)
    {
        return new Context(this.document, this.source_string, this.lineNumber, this.startLinePos, this.startPos,
            other.endLineNumber, other.endLinePos, other.endPos, this.token);
    }

    internal void EmitLineInfo(ILGenerator ilgen)
    {
        this.document.EmitLineInfo(ilgen, this.StartLine, this.StartColumn, this.EndLine, this.EndColumn);
    }

    internal void EmitFirstLineInfo(ILGenerator ilgen)
    {
        this.document.EmitFirstLineInfo(ilgen, this.StartLine, this.StartColumn, this.EndLine, this.EndColumn);
    }

    /// <summary>Gets the column number in the line where this code ends. This includes whitespace at the start of the line.</summary>
    /// <returns>The column number where this code ends.</returns>
    public int EndColumn => this.endPos - this.endLinePos;

    /// <summary>Gets the ending line number of this code.</summary>
    /// <returns>The ending line number of this code.</returns>
    public int EndLine => this.endLineNumber;

    /// <summary>Gets the position in the code string where this code ends, because all lines of code are stored in a single string.</summary>
    /// <returns>The position in the code string where this code ends.</returns>
    public int EndPosition => this.endPos;

    internal bool Equals(string str)
    {
        int length = this.endPos - this.startPos;
        return length == str.Length && string.CompareOrdinal(this.source_string, this.startPos, str, 0, length) == 0;
    }

    internal bool Equals(Context ctx)
    {
        return this.source_string == ctx.source_string && this.lineNumber == ctx.lineNumber &&
               this.startLinePos == ctx.startLinePos && this.startPos == ctx.startPos &&
               this.endLineNumber == ctx.endLineNumber && this.endLinePos == ctx.endLinePos &&
               this.endPos == ctx.endPos && this.token == ctx.token;
    }

    /// <summary>Gets the code that the context contains, discarding any whitespace.</summary>
    /// <returns>The code that the context contains.</returns>
    public string GetCode()
    {
        return this.endPos > this.startPos && this.endPos <= this.source_string.Length
            ? this.source_string.Substring(this.startPos, this.endPos - this.startPos)
            : (string)null;
    }

    /// <summary>Gets an individual unit of code, or a token, that the context contains.</summary>
    /// <returns>An individual unit of code, or a token, that the context contains.</returns>
    public JSToken GetToken() => this.token;

    internal void HandleError(JSError errorId) => this.HandleError(errorId, (string)null, false);

    internal void HandleError(JSError errorId, bool treatAsError)
    {
        this.HandleError(errorId, (string)null, treatAsError);
    }

    internal void HandleError(JSError errorId, string message)
    {
        this.HandleError(errorId, message, false);
    }

    internal void HandleError(JSError errorId, string message, bool treatAsError)
    {
        if (errorId == JSError.UndeclaredVariable && this.document.HasAlreadySeenErrorFor(this.GetCode()))
            return;
        JScriptException error = new JScriptException(errorId, this);
        if (message != null)
            error.value = (object)message;
        if (treatAsError)
            error.isError = treatAsError;
        int severity = error.Severity;
        if (severity >= this.errorReported)
            return;
        this.document.HandleError(error);
        this.errorReported = severity;
    }

    internal void SetSourceContext(DocumentContext document, string source)
    {
        this.source_string = source;
        this.endPos = source.Length;
        this.document = document;
    }

    /// <summary>Gets the column number in the line where this code starts. This includes whitespace at the start of the line.</summary>
    /// <returns>The column number where this code starts.</returns>
    public int StartColumn => this.startPos - this.startLinePos;

    /// <summary>Gets the line number where this code starts.</summary>
    /// <returns>The line number where this code starts.</returns>
    public int StartLine => this.lineNumber;

    /// <summary>Gets the position in the code string where this code starts, because all lines of code are stored in a single string.</summary>
    /// <returns>The position in the code string where this code starts.</returns>
    public int StartPosition => this.startPos;

    internal void UpdateWith(Context other)
    {
        this.endPos = other.endPos;
        this.endLineNumber = other.endLineNumber;
        this.endLinePos = other.endLinePos;
    }
}