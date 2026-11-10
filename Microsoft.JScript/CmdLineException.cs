// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.CmdLineException
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents errors that occur when you run the command-line compiler jsc.exe. </summary>
[Serializable]
public class CmdLineException : Exception
{
    private CmdLineError errorCode;
    private string context;
    private CultureInfo culture;
    private const int LOCALE_USER_DEFAULT = 1024 /*0x0400*/;

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.CmdLineException" /> class, using the specified error code and culture information.</summary>
    /// <param name="errorCode">One of the enumeration values. Specifies what the error is.</param>
    /// <param name="culture">Culture-specific information.</param>
    public CmdLineException(CmdLineError errorCode, CultureInfo culture)
    {
        this.culture = culture;
        this.errorCode = errorCode;
    }

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.CmdLineException" /> class, using the specified error code, context, and culture information.</summary>
    /// <param name="errorCode">One of the enumeration values. Specifies what the error is.</param>
    /// <param name="context">The context in which the error occurred.</param>
    /// <param name="culture">Culture-specific information.</param>
    public CmdLineException(CmdLineError errorCode, string context, CultureInfo culture)
    {
        this.culture = culture;
        this.errorCode = errorCode;
        if (!(context != ""))
            return;
        this.context = context;
    }

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.CmdLineException" /> class.</summary>
    public CmdLineException()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.CmdLineException" /> class, using the specified error message.</summary>
    /// <param name="m">The message that describes the error.</param>
    public CmdLineException(string m)
        : base(m)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.CmdLineException" /> class, using the specified error message and a reference to the inner exception that is the cause of this exception.</summary>
    /// <param name="m">The message that describes the error.</param>
    /// <param name="e">The exception that is the cause of the current exception, or null if no inner exception is specified.</param>
    public CmdLineException(string m, Exception e)
        : base(m, e)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.CmdLineException" /> class, using serialized data.</summary>
    /// <param name="s">The serialized object data about the exception that is being thrown.</param>
    /// <param name="c">Contextual information about the source or destination.</param>
    protected CmdLineException(SerializationInfo s, StreamingContext c)
        : base(s, c)
    {
        this.errorCode = (CmdLineError)s.GetInt32("ErrorCode");
        this.context = s.GetString("Context");
        int int32 = s.GetInt32("LCID");
        if (int32 == 1024 /*0x0400*/)
            return;
        this.culture = new CultureInfo(int32);
    }

    /// <summary>Stores serialized object data about the exception that is being thrown.</summary>
    /// <param name="s">Serialized object data about the exception that is being thrown.</param>
    /// <param name="c">Contextual information about the source or destination</param>
    [SecurityCritical]
    [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
    public override void GetObjectData(SerializationInfo s, StreamingContext c)
    {
        base.GetObjectData(s, c);
        s.AddValue("ErrorCode", (int)this.errorCode);
        s.AddValue("Context", (object)this.context);
        int num = 1024 /*0x0400*/;
        if (this.culture != null)
            num = this.culture.LCID;
        s.AddValue("LCID", num);
    }

    /// <summary>Gets a message that describes the current exception.</summary>
    /// <returns>The error message that explains the reason for the exception.</returns>
    public override string Message
    {
        get
        {
            string str = JScriptException.Localize(this.ResourceKey(this.errorCode), this.context, this.culture);
            return
                $"fatal error JS{((int)(10000 + this.errorCode)).ToString((IFormatProvider)CultureInfo.InvariantCulture).Substring(1)}: {str}";
        }
    }

    /// <summary>Returns the key for the specified error in a resource file.</summary>
    /// <returns>The key for <paramref name="errorCode" />.</returns>
    /// <param name="errorCode">One of the enumeration values. Specifies the error to get the key for.</param>
    public string ResourceKey(CmdLineError errorCode)
    {
        switch (errorCode)
        {
            case CmdLineError.AssemblyNotFound:
                return "Assembly not found";
            case CmdLineError.CannotCreateEngine:
                return "Cannot create JScript engine";
            case CmdLineError.CompilerConstant:
                return "Compiler constant";
            case CmdLineError.DuplicateFileAsSourceAndAssembly:
                return "Duplicate file as source and assembly";
            case CmdLineError.DuplicateResourceFile:
                return "Duplicate resource file";
            case CmdLineError.DuplicateResourceName:
                return "Duplicate resource name";
            case CmdLineError.DuplicateSourceFile:
                return "Duplicate source file";
            case CmdLineError.ErrorSavingCompiledState:
                return "Error saving compiled state";
            case CmdLineError.InvalidAssembly:
                return "Invalid assembly";
            case CmdLineError.InvalidCodePage:
                return "Invalid code page";
            case CmdLineError.InvalidDefinition:
                return "Invalid definition";
            case CmdLineError.InvalidLocaleID:
                return "Invalid Locale ID";
            case CmdLineError.InvalidTarget:
                return "Invalid target";
            case CmdLineError.InvalidSourceFile:
                return "Invalid source file";
            case CmdLineError.InvalidWarningLevel:
                return "Invalid warning level";
            case CmdLineError.MultipleOutputNames:
                return "Multiple output filenames";
            case CmdLineError.MultipleTargets:
                return "Multiple targets";
            case CmdLineError.MissingDefineArgument:
                return "Missing define argument";
            case CmdLineError.MissingExtension:
                return "Missing extension";
            case CmdLineError.MissingLibArgument:
                return "Missing lib argument";
            case CmdLineError.ManagedResourceNotFound:
                return "Managed resource not found";
            case CmdLineError.NestedResponseFiles:
                return "Nested response files";
            case CmdLineError.NoCodePage:
                return "No code page";
            case CmdLineError.NoFileName:
                return "No filename";
            case CmdLineError.NoInputSourcesSpecified:
                return "No input sources specified";
            case CmdLineError.NoLocaleID:
                return "No Locale ID";
            case CmdLineError.NoWarningLevel:
                return "No warning level";
            case CmdLineError.ResourceNotFound:
                return "Resource not found";
            case CmdLineError.UnknownOption:
                return "Unknown option";
            case CmdLineError.InvalidVersion:
                return "Invalid version";
            case CmdLineError.SourceFileTooBig:
                return "Source file too big";
            case CmdLineError.MultipleWin32Resources:
                return "Multiple win32resources";
            case CmdLineError.MissingReference:
                return "Missing reference";
            case CmdLineError.SourceNotFound:
                return "Source not found";
            case CmdLineError.InvalidCharacters:
                return "Invalid characters";
            case CmdLineError.InvalidForCompilerOptions:
                return "Invalid for CompilerOptions";
            case CmdLineError.IncompatibleTargets:
                return "Incompatible targets";
            case CmdLineError.InvalidPlatform:
                return "Invalid platform";
            default:
                return "No description available";
        }
    }
}