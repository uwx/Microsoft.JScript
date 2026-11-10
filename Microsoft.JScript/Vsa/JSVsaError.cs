// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.JSVsaError
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Defines the set of exceptions that can be thrown by a .NET script engine.</summary>
[Guid("7607D148-3AA5-4CC6-AED5-D4DB0ECBD992")]
[Obsolete(
    "Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[ComVisible(true)]
public enum JSVsaError
{
    /// <summary>Value: 0x80133000 </summary>
    AppDomainCannotBeSet = -2146226176, // 0x80133000

    /// <summary>Value: 0x80133001 </summary>
    AppDomainInvalid = -2146226175, // 0x80133001

    /// <summary>Value: 0x80133002 </summary>
    ApplicationBaseCannotBeSet = -2146226174, // 0x80133002

    /// <summary>Value: 0x80133003 </summary>
    ApplicationBaseInvalid = -2146226173, // 0x80133003

    /// <summary>Value: 0x80133004 </summary>
    AssemblyExpected = -2146226172, // 0x80133004

    /// <summary>Value: 0x80133005 </summary>
    AssemblyNameInvalid = -2146226171, // 0x80133005

    /// <summary>Value: 0x80133006 </summary>
    BadAssembly = -2146226170, // 0x80133006

    /// <summary>Value: 0x80133007 </summary>
    CachedAssemblyInvalid = -2146226169, // 0x80133007

    /// <summary>Value: 0x80133008 </summary>
    CallbackUnexpected = -2146226168, // 0x80133008

    /// <summary>Value: 0x80133009 </summary>
    CodeDOMNotAvailable = -2146226167, // 0x80133009

    /// <summary>Value: 0x8013300A </summary>
    CompiledStateNotFound = -2146226166, // 0x8013300A

    /// <summary>Value: 0x8013300B </summary>
    DebugInfoNotSupported = -2146226165, // 0x8013300B

    /// <summary>Value: 0x8013300C </summary>
    ElementNameInvalid = -2146226164, // 0x8013300C

    /// <summary>Value: 0x8013300D </summary>
    ElementNotFound = -2146226163, // 0x8013300D

    /// <summary>Value: 0x8013300E </summary>
    EngineBusy = -2146226162, // 0x8013300E

    /// <summary>Value: 0x8013300F </summary>
    EngineCannotClose = -2146226161, // 0x8013300F

    /// <summary>Value: 0x80133010 </summary>
    EngineCannotReset = -2146226160, // 0x80133010

    /// <summary>Value: 0x80133011 </summary>
    EngineClosed = -2146226159, // 0x80133011

    /// <summary>Value: 0x80133012 </summary>
    EngineEmpty = -2146226159, // 0x80133011

    /// <summary>Value: 0x80133013 </summary>
    EngineInitialized = -2146226157, // 0x80133013

    /// <summary>Value: 0x80133014 </summary>
    EngineNameInUse = -2146226156, // 0x80133014

    /// <summary>Value: 0x80133015 </summary>
    EngineNotCompiled = -2146226155, // 0x80133015

    /// <summary>Value: 0x80133016 </summary>
    EngineNotInitialized = -2146226154, // 0x80133016

    /// <summary>Value: 0x80133017 </summary>
    EngineNotRunning = -2146226153, // 0x80133017

    /// <summary>Value: 0x80133018 </summary>
    EngineRunning = -2146226152, // 0x80133018

    /// <summary>Value: 0x80133019 </summary>
    EventSourceInvalid = -2146226151, // 0x80133019

    /// <summary>Value: 0x8013301A </summary>
    EventSourceNameInUse = -2146226150, // 0x8013301A

    /// <summary>Value: 0x8013301B </summary>
    EventSourceNameInvalid = -2146226149, // 0x8013301B

    /// <summary>Value: 0x8013301C </summary>
    EventSourceNotFound = -2146226148, // 0x8013301C

    /// <summary>Value: 0x8013301D </summary>
    EventSourceTypeInvalid = -2146226147, // 0x8013301D

    /// <summary>Value: 0x8013301E </summary>
    GetCompiledStateFailed = -2146226146, // 0x8013301E

    /// <summary>Value: 0x8013301F </summary>
    GlobalInstanceInvalid = -2146226145, // 0x8013301F

    /// <summary>Value: 0x80133020 </summary>
    GlobalInstanceTypeInvalid = -2146226144, // 0x80133020

    /// <summary>Value: 0x80133021 </summary>
    InternalCompilerError = -2146226143, // 0x80133021

    /// <summary>Value: 0x80133022 </summary>
    ItemCannotBeRemoved = -2146226142, // 0x80133022

    /// <summary>Value: 0x80133023 </summary>
    ItemFlagNotSupported = -2146226141, // 0x80133023

    /// <summary>Value: 0x80133024 </summary>
    ItemNameInUse = -2146226140, // 0x80133024

    /// <summary>Value: 0x80133025 </summary>
    ItemNameInvalid = -2146226139, // 0x80133025

    /// <summary>Value: 0x80133026 </summary>
    ItemNotFound = -2146226138, // 0x80133026

    /// <summary>Value: 0x80133027 </summary>
    ItemTypeNotSupported = -2146226137, // 0x80133027

    /// <summary>Value: 0x80133028 </summary>
    LCIDNotSupported = -2146226136, // 0x80133028

    /// <summary>Value: 0x80133029 </summary>
    LoadElementFailed = -2146226135, // 0x80133029

    /// <summary>Value: 0x8013302A </summary>
    NotificationInvalid = -2146226134, // 0x8013302A

    /// <summary>Value: 0x8013302B </summary>
    OptionInvalid = -2146226133, // 0x8013302B

    /// <summary>Value: 0x8013302C </summary>
    OptionNotSupported = -2146226132, // 0x8013302C

    /// <summary>Value: 0x8013302D </summary>
    RevokeFailed = -2146226131, // 0x8013302D

    /// <summary>Value: 0x8013302E </summary>
    RootMonikerAlreadySet = -2146226130, // 0x8013302E

    /// <summary>Value: 0x8013302F </summary>
    RootMonikerInUse = -2146226129, // 0x8013302F

    /// <summary>Value: 0x80133030 </summary>
    RootMonikerInvalid = -2146226128, // 0x80133030

    /// <summary>Value: 0x80133031 </summary>
    RootMonikerNotSet = -2146226127, // 0x80133031

    /// <summary>Value: 0x80133032 </summary>
    RootMonikerProtocolInvalid = -2146226126, // 0x80133032

    /// <summary>Value: 0x80133033 </summary>
    RootNamespaceInvalid = -2146226125, // 0x80133033

    /// <summary>Value: 0x80133034 </summary>
    RootNamespaceNotSet = -2146226124, // 0x80133034

    /// <summary>Value: 0x80133035 </summary>
    SaveCompiledStateFailed = -2146226123, // 0x80133035

    /// <summary>Value: 0x80133036 </summary>
    SaveElementFailed = -2146226122, // 0x80133036

    /// <summary>Value: 0x80133037 </summary>
    SiteAlreadySet = -2146226121, // 0x80133037

    /// <summary>Value: 0x80133038 </summary>
    SiteInvalid = -2146226120, // 0x80133038

    /// <summary>Value: 0x80133039 </summary>
    SiteNotSet = -2146226119, // 0x80133039

    /// <summary>Value: 0x8013303A </summary>
    SourceItemNotAvailable = -2146226118, // 0x8013303A

    /// <summary>Value: 0x8013303B </summary>
    SourceMonikerNotAvailable = -2146226117, // 0x8013303B

    /// <summary>Value: 0x8013303C </summary>
    URLInvalid = -2146226116, // 0x8013303C

    /// <summary>Value: 0x8013303D </summary>
    BrowserNotExist = -2146226115, // 0x8013303D

    /// <summary>Value: 0x8013303E </summary>
    DebuggeeNotStarted = -2146226114, // 0x8013303E

    /// <summary>Value: 0x8013303F </summary>
    EngineNameInvalid = -2146226113, // 0x8013303F

    /// <summary>Value: 0x80133040 </summary>
    EngineNotExist = -2146226112, // 0x80133040

    /// <summary>Value: 0x80133041 </summary>
    FileFormatUnsupported = -2146226111, // 0x80133041

    /// <summary>Value: 0x80133042 </summary>
    FileTypeUnknown = -2146226110, // 0x80133042

    /// <summary>Value: 0x80133043 </summary>
    ItemCannotBeRenamed = -2146226109, // 0x80133043

    /// <summary>Value: 0x80133044 </summary>
    MissingSource = -2146226108, // 0x80133044

    /// <summary>Value: 0x80133045 </summary>
    NotInitCompleted = -2146226107, // 0x80133045

    /// <summary>Value: 0x80133046 </summary>
    NameTooLong = -2146226106, // 0x80133046

    /// <summary>Value: 0x80133047 </summary>
    ProcNameInUse = -2146226105, // 0x80133047

    /// <summary>Value: 0x80133048 </summary>
    ProcNameInvalid = -2146226104, // 0x80133048

    /// <summary>Value: 0x80133049 </summary>
    VsaServerDown = -2146226103, // 0x80133049

    /// <summary>Value: 0x8013304A </summary>
    MissingPdb = -2146226102, // 0x8013304A

    /// <summary>Value: 0x8013304B </summary>
    NotClientSideAndNoUrl = -2146226101, // 0x8013304B

    /// <summary>Value: 0x8013304C </summary>
    CannotAttachToWebServer = -2146226100, // 0x8013304C

    /// <summary>Value: 0x8013303F </summary>
    EngineNameNotSet = -2146226099, // 0x8013304D

    /// <summary>Value: 0x801330FF </summary>
    UnknownError = -2146225921, // 0x801330FF
}