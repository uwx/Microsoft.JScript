// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.CmdLineError
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Specifies errors that occur when you run the command-line compiler jsc.exe. This enumeration is used to create <see cref="T:Microsoft.JScript.CmdLineException" /> objects.</summary>
public enum CmdLineError
{
    /// <summary>No error. This is the default value.</summary>
    NoError = 0,

    /// <summary>The referenced assembly '{0}' could not be found.</summary>
    AssemblyNotFound = 2001, // 0x000007D1

    /// <summary>Cannot create JScript engine.</summary>
    CannotCreateEngine = 2002, // 0x000007D2

    /// <summary>Cannot change value of compiler constant '{0}'.</summary>
    CompilerConstant = 2003, // 0x000007D3

    /// <summary>Duplicate file name: '{0}' is listed as both a source file and an imported assembly.</summary>
    DuplicateFileAsSourceAndAssembly = 2004, // 0x000007D4

    /// <summary>Resource file '{0}' referenced more than once.</summary>
    DuplicateResourceFile = 2005, // 0x000007D5

    /// <summary>Resource name '{0}' is not unique.</summary>
    DuplicateResourceName = 2006, // 0x000007D6

    /// <summary>Duplicate file name: '{0}' is listed as a source file more than once.</summary>
    DuplicateSourceFile = 2007, // 0x000007D7

    /// <summary>Error saving compiled state: {0}.</summary>
    ErrorSavingCompiledState = 2008, // 0x000007D8

    /// <summary>'{0}' is not a valid assembly.</summary>
    InvalidAssembly = 2009, // 0x000007D9

    /// <summary>Code page '{0}' is invalid or not installed.</summary>
    InvalidCodePage = 2010, // 0x000007DA

    /// <summary>Definition '{0}' must be assigned a Boolean or integral value.</summary>
    InvalidDefinition = 2011, // 0x000007DB

    /// <summary>Locale ID '{0}' is invalid or not installed.</summary>
    InvalidLocaleID = 2012, // 0x000007DC

    /// <summary>Target '{0}' is invalid. Specify 'exe' or 'library'.</summary>
    InvalidTarget = 2013, // 0x000007DD

    /// <summary>'{0}' is not a valid source file.</summary>
    InvalidSourceFile = 2014, // 0x000007DE

    /// <summary>Invalid warning level specified with option '{0}'.</summary>
    InvalidWarningLevel = 2015, // 0x000007DF

    /// <summary>Multiple output file names specified.</summary>
    MultipleOutputNames = 2016, // 0x000007E0

    /// <summary>Multiple targets specified.</summary>
    MultipleTargets = 2017, // 0x000007E1

    /// <summary>Missing identifier for '-define:' command-line option.</summary>
    MissingDefineArgument = 2018, // 0x000007E2

    /// <summary>Missing file extension for '{0}'.</summary>
    MissingExtension = 2019, // 0x000007E3

    /// <summary>Missing file specification for '-lib:' command-line option.</summary>
    MissingLibArgument = 2020, // 0x000007E4

    /// <summary>Missing version information in -assembly.version option.</summary>
    MissingVersionInfo = 2021, // 0x000007E5

    /// <summary>Managed resource '{0}' was not found.</summary>
    ManagedResourceNotFound = 2022, // 0x000007E6

    /// <summary>Cannot process nested response files.</summary>
    NestedResponseFiles = 2023, // 0x000007E7

    /// <summary>No code page is specified with option '{0}'.</summary>
    NoCodePage = 2024, // 0x000007E8

    /// <summary>No file name specified with option '{0}'.</summary>
    NoFileName = 2025, // 0x000007E9

    /// <summary>No input sources specified.</summary>
    NoInputSourcesSpecified = 2026, // 0x000007EA

    /// <summary>No Locale ID is specified with option '{0}'.</summary>
    NoLocaleID = 2027, // 0x000007EB

    /// <summary>No warning level specified with option '{0}'.</summary>
    NoWarningLevel = 2028, // 0x000007EC

    /// <summary>Win32 resource '{0}' was not found.</summary>
    ResourceNotFound = 2029, // 0x000007ED

    /// <summary>Unknown option '{0}'.</summary>
    UnknownOption = 2030, // 0x000007EE

    /// <summary>'{0}' is not a valid version string.</summary>
    InvalidVersion = 2031, // 0x000007EF

    /// <summary>Source file too big.</summary>
    SourceFileTooBig = 2032, // 0x000007F0

    /// <summary>Cannot specify more than one Win32 resource file.</summary>
    MultipleWin32Resources = 2033, // 0x000007F1

    /// <summary>Missing reference in argument list '{0}'.</summary>
    MissingReference = 2034, // 0x000007F2

    /// <summary>Could not find source file '{0}'.</summary>
    SourceNotFound = 2035, // 0x000007F3

    /// <summary>File name '{0}' contains invalid characters (wildcard characters are not supported).</summary>
    InvalidCharacters = 2036, // 0x000007F4

    /// <summary>'{0}' is not allowed in the CompilerOptions field.</summary>
    InvalidForCompilerOptions = 2037, // 0x000007F5

    /// <summary>The value of the <see cref="P:System.CodeDom.Compiler.CompilerParameters.GenerateExecutable" /> property is not compatible with target type '{0}'.</summary>
    IncompatibleTargets = 2038, // 0x000007F6

    /// <summary>Platform '{0}' is invalid. Specify 'x86', 'Itanium', 'x64', or 'anycpu'.</summary>
    InvalidPlatform = 2039, // 0x000007F7

    /// <summary>A placeholder enumeration member that contains the same value as the last (highest) assigned enumeration value.</summary>
    LAST = 2039, // 0x000007F7

    /// <summary>Unspecified error.</summary>
    Unspecified = 2999, // 0x00000BB7
}