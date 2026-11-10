// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JScriptCodeProvider
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Provides access to instances of the JScript code generator and code compiler. </summary>
[DesignerCategory("code")]
[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
public sealed class JScriptCodeProvider : CodeDomProvider
{
    private JSCodeGenerator generator;

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.JScriptCodeProvider" /> class.</summary>
    public JScriptCodeProvider() => this.generator = new JSCodeGenerator();

    /// <summary>Returns an instance of the JScript code generator.</summary>
    /// <returns>An instance of the JScript code generator.</returns>
    public override ICodeGenerator CreateGenerator() => (ICodeGenerator)this.generator;

    /// <summary>Returns an instance of the JScript code compiler.</summary>
    /// <returns>An instance of the JScript code compiler.</returns>
    public override ICodeCompiler CreateCompiler() => (ICodeCompiler)this.generator;

    /// <summary>Gets the file name extension for JScript code files.</summary>
    /// <returns>The string literal "js".</returns>
    public override string FileExtension => "js";
}