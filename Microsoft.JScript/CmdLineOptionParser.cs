// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.CmdLineOptionParser
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Determines whether options that are specified to the command-line compiler jsc.exe are correctly formed, and reads the arguments if any are included.</summary>
public class CmdLineOptionParser
{
    /// <summary>Verifies that the specified option is correctly formed.</summary>
    /// <returns>true if the option is correctly formed; otherwise, false.</returns>
    /// <param name="option">The command-line option that the user specified.</param>
    /// <param name="prefix">The option name.</param>
    public static bool IsSimpleOption(string option, string prefix)
    {
        return string.Compare(option, prefix, StringComparison.OrdinalIgnoreCase) == 0;
    }

    /// <summary>Gets the argument from the specified command-line option. Applies to options that have only one name, such as /out:&lt;filename&gt;.</summary>
    /// <returns>The argument, "" if the argument is not specified, or null if the option or argument is not correctly formed.</returns>
    /// <param name="option">The command-line option that the user specified.</param>
    /// <param name="prefix">The option name.</param>
    public static string IsArgumentOption(string option, string prefix)
    {
        int length = prefix.Length;
        if (option.Length < length ||
            string.Compare(option, 0, prefix, 0, length, StringComparison.OrdinalIgnoreCase) != 0)
            return (string)null;
        if (option.Length == length)
            return "";
        return ':' != option[length] ? (string)null : option.Substring(length + 1);
    }

    /// <summary>Gets the argument from the specified command-line option. Applies to options that can be specified by a short or long form of the option name, such as /r or /reference for /r[eference]:&lt;file list&gt;.</summary>
    /// <returns>The argument, "" if the argument is not specified, or null if the option or argument is not correctly formed.</returns>
    /// <param name="option">The command-line option that the user specified.</param>
    /// <param name="shortPrefix">The short form of the option name.</param>
    /// <param name="longPrefix">The long form of the option name.</param>
    public static string IsArgumentOption(string option, string shortPrefix, string longPrefix)
    {
        return CmdLineOptionParser.IsArgumentOption(option, shortPrefix) ??
               CmdLineOptionParser.IsArgumentOption(option, longPrefix);
    }

    /// <summary>Gets the + or - argument from the specified command-line option. Applies to options that have only one name, such as /debug[+|-].</summary>
    /// <returns>false for -; true for + or if the argument is not specified; null if the option or argument is not correctly formed.</returns>
    /// <param name="option">The command-line option that the user specified.</param>
    /// <param name="prefix">The option name.</param>
    public static object IsBooleanOption(string option, string prefix)
    {
        int length = prefix.Length;
        if (option.Length < prefix.Length ||
            string.Compare(option, 0, prefix, 0, length, StringComparison.OrdinalIgnoreCase) != 0)
            return (object)null;
        if (option.Length == length)
            return (object)true;
        if (option.Length != length + 1)
            return (object)null;
        if ('-' == option[length])
            return (object)false;
        return '+' == option[length] ? (object)true : (object)null;
    }

    /// <summary>Gets the + or - argument from the specified command-line option. Applies to options that can be specified by a short or long form of the option name.</summary>
    /// <returns>false for -; true for + or if the argument is not specified; null if the option or argument is not correctly formed.</returns>
    /// <param name="option">The command-line option that the user specified.</param>
    /// <param name="shortPrefix">The short form of the option name.</param>
    /// <param name="longPrefix">The long form of the option name.</param>
    public static object IsBooleanOption(string option, string shortPrefix, string longPrefix)
    {
        return CmdLineOptionParser.IsBooleanOption(option, shortPrefix) ??
               CmdLineOptionParser.IsBooleanOption(option, longPrefix);
    }
}