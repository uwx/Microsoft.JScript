// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.VsaSite
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Globalization;
using System.IO;

#nullable disable
namespace Microsoft.JScript;

internal class VsaSite : BaseVsaSite
{
    public int warningLevel = 4;
    public bool treatWarningsAsErrors;
    public TextWriter output = Console.Out;

    public VsaSite(TextWriter redirectedOutput) => this.output = redirectedOutput;

    public override bool OnCompilerError(IJSVsaError error)
    {
        int severity = error.Severity;
        if (severity > this.warningLevel)
            return true;
        bool fIsWarning = severity != 0 && !this.treatWarningsAsErrors;
        this.PrintError(error.SourceMoniker, error.Line, error.StartColumn, fIsWarning, error.Number,
            error.Description);
        return true;
    }

    private void PrintError(
        string sourceFile,
        int line,
        int column,
        bool fIsWarning,
        int number,
        string message)
    {
        string str = (10000 + (number & (int)ushort.MaxValue)).ToString((IFormatProvider)CultureInfo.InvariantCulture)
            .Substring(1);
        if (string.Compare(sourceFile, "no source", StringComparison.Ordinal) != 0)
            this.output.Write(
                $"{sourceFile}({line.ToString((IFormatProvider)CultureInfo.InvariantCulture)},{column.ToString((IFormatProvider)CultureInfo.InvariantCulture)}) : ");
        this.output.WriteLine($"{(fIsWarning ? "warning JS" : "error JS")}{str}: {message}");
    }
}