// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.RegExpObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Text.RegularExpressions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a regular expression object. This class belongs to the built-in object model category.</summary>
public sealed class RegExpObject : JSObject
{
    internal RegExpConstructor regExpConst;
    private string sourceInt;
    internal bool ignoreCaseInt;
    internal bool globalInt;
    internal bool multilineInt;
    internal Regex regex;
    internal object lastIndexInt;

    internal RegExpObject(
        RegExpPrototype parent,
        string source,
        bool ignoreCase,
        bool global,
        bool multiline,
        RegExpConstructor regExpConst)
        : base((ScriptObject)parent)
    {
        this.regExpConst = regExpConst;
        this.sourceInt = source;
        this.ignoreCaseInt = ignoreCase;
        this.globalInt = global;
        this.multilineInt = multiline;
        RegexOptions options = RegexOptions.ECMAScript | RegexOptions.CultureInvariant;
        if (ignoreCase)
            options |= RegexOptions.IgnoreCase;
        if (multiline)
            options |= RegexOptions.Multiline;
        try
        {
            this.regex = new Regex(source, options);
        }
        catch (ArgumentException ex)
        {
            throw new JScriptException(JSError.RegExpSyntax);
        }

        this.lastIndexInt = (object)0;
        this.noExpando = false;
    }

    internal RegExpObject(Regex regex)
        : base((ScriptObject)null)
    {
        this.regExpConst = (RegExpConstructor)null;
        this.sourceInt = "";
        this.ignoreCaseInt = (regex.Options & RegexOptions.IgnoreCase) != 0;
        this.globalInt = false;
        this.multilineInt = (regex.Options & RegexOptions.Multiline) != 0;
        this.regex = regex;
        this.lastIndexInt = (object)0;
        this.noExpando = true;
    }

    internal RegExpObject compile(string source, string flags)
    {
        this.sourceInt = source;
        this.ignoreCaseInt = this.globalInt = this.multilineInt = false;
        RegexOptions options = RegexOptions.ECMAScript | RegexOptions.CultureInvariant;
        for (int index = 0; index < flags.Length; ++index)
        {
            switch (flags[index])
            {
                case 'g':
                    this.globalInt = !this.globalInt ? true : throw new JScriptException(JSError.RegExpSyntax);
                    break;
                case 'i':
                    this.ignoreCaseInt = !this.ignoreCaseInt ? true : throw new JScriptException(JSError.RegExpSyntax);
                    options |= RegexOptions.IgnoreCase;
                    break;
                case 'm':
                    this.multilineInt = !this.multilineInt ? true : throw new JScriptException(JSError.RegExpSyntax);
                    options |= RegexOptions.Multiline;
                    break;
                default:
                    throw new JScriptException(JSError.RegExpSyntax);
            }
        }

        try
        {
            this.regex = new Regex(source, options);
        }
        catch (ArgumentException ex)
        {
            throw new JScriptException(JSError.RegExpSyntax);
        }

        return this;
    }

    internal object exec(string input)
    {
        Match match = (Match)null;
        if (!this.globalInt)
        {
            match = this.regex.Match(input);
        }
        else
        {
            int int64 = (int)Runtime.DoubleToInt64(Convert.ToInteger(this.lastIndexInt));
            if (int64 <= 0)
                match = this.regex.Match(input);
            else if (int64 <= input.Length)
                match = this.regex.Match(input, int64);
        }

        if (match == null || !match.Success)
        {
            this.lastIndexInt = (object)0;
            return (object)DBNull.Value;
        }

        this.lastIndexInt = (object)this.regExpConst.UpdateConstructor(this.regex, match, input);
        return (object)new RegExpMatch(this.regExpConst.arrayPrototype, this.regex, match, input);
    }

    internal override string GetClassName() => "RegExp";

    internal bool test(string input)
    {
        Match match = (Match)null;
        if (!this.globalInt)
        {
            match = this.regex.Match(input);
        }
        else
        {
            int int64 = (int)Runtime.DoubleToInt64(Convert.ToInteger(this.lastIndexInt));
            if (int64 <= 0)
                match = this.regex.Match(input);
            else if (int64 <= input.Length)
                match = this.regex.Match(input, int64);
        }

        if (match == null || !match.Success)
        {
            this.lastIndexInt = (object)0;
            return false;
        }

        this.lastIndexInt = (object)this.regExpConst.UpdateConstructor(this.regex, match, input);
        return true;
    }

    /// <summary>Creates a string representation of this object that contains the regular expression pattern and flags.</summary>
    /// <returns>A string representation of this object.</returns>
    public override string ToString()
    {
        return
            $"/{this.sourceInt}/{(this.ignoreCaseInt ? "i" : "")}{(this.globalInt ? "g" : "")}{(this.multilineInt ? "m" : "")}";
    }

    /// <summary>Gets the regular expression pattern that is represented by this object.</summary>
    /// <returns>The regular expression pattern that is represented by this object.</returns>
    public string source => this.sourceInt;

    /// <summary>Gets a value indicating whether searches are performed with the case-insensitive flag (/i).</summary>
    /// <returns>true if case-insensitive searches are performed; otherwise, false.</returns>
    public bool ignoreCase => this.ignoreCaseInt;

    /// <summary>Gets a value indicating whether searches are performed with the global flag (/g), matching all occurrences.</summary>
    /// <returns>true if global searches are performed; otherwise, false.</returns>
    public bool global => this.globalInt;

    /// <summary>Gets a value indicating whether searches are performed with the multiline flag (/m).</summary>
    /// <returns>true if searches are performed in multiline mode; otherwise, false.</returns>
    public bool multiline => this.multilineInt;

    /// <summary>Gets or sets the index of the last successful match.</summary>
    /// <returns>The index of the last successful match, if there was one.</returns>
    public object lastIndex
    {
        get => this.lastIndexInt;
        set => this.lastIndexInt = value;
    }
}