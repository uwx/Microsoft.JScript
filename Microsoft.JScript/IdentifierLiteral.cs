// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IdentifierLiteral
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class IdentifierLiteral : AST
{
    private string identifier;

    internal IdentifierLiteral(string identifier, Context context)
        : base(context)
    {
        this.identifier = identifier;
    }

    internal override object Evaluate()
    {
        throw new JScriptException(JSError.InternalError, this.context);
    }

    internal override AST PartiallyEvaluate()
    {
        throw new JScriptException(JSError.InternalError, this.context);
    }

    public override string ToString() => this.identifier;

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        throw new JScriptException(JSError.InternalError, this.context);
    }
}