// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Vsa.BaseVsaEngine
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.Win32;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;

#nullable disable
namespace Microsoft.JScript.Vsa;

/// <summary>Implements <see cref="T:Microsoft.JScript.Vsa.IJSVsaEngine" /> interface.</summary>
[Obsolete(
    "Use of this type is not recommended because it is being deprecated in Visual Studio 2005; there will be no replacement for this feature. Please see the ICodeCompiler documentation for additional help.")]
[Guid("F8932A50-9127-48B6-B115-2BFDC627CEE3")]
[ComVisible(true)]
public abstract class BaseVsaEngine : IJSVsaEngine
{
    /// <summary>Gets or sets the application path.</summary>
    protected string applicationPath;

    /// <summary>Gets or sets the loaded assembly.</summary>
    protected Assembly loadedAssembly;

    /// <summary>Gets or sets the compiled root namespace.</summary>
    protected string compiledRootNamespace;

    /// <summary>Gets or sets the engine site</summary>
    protected IJSVsaSite engineSite;

    /// <summary>Gets or sets a value that indicates whether debug information is generated.</summary>
    protected bool genDebugInfo;

    /// <summary>Gets or sets a value that indicates whether there is a compiled state.</summary>
    protected bool haveCompiledState;

    /// <summary>Gets or sets a value that indicates whether the compilation failed.</summary>
    protected bool failedCompilation;

    /// <summary>Gets or sets a value that indicates whether the engine is closed.</summary>
    protected bool isClosed;

    /// <summary>Gets or sets a value that indicates whether the engine is compiled.</summary>
    protected bool isEngineCompiled;

    /// <summary>Gets or sets a value that indicates whether debug information is supported.</summary>
    protected bool isDebugInfoSupported;

    /// <summary>Gets or sets a value that indicates whether the current in-memory representation of the item differs from the persisted representation.</summary>
    protected bool isEngineDirty;

    /// <summary>Gets or sets a value that indicates whether the engine is initialized.</summary>
    protected bool isEngineInitialized;

    /// <summary>Gets or sets a value that indicates whether the engine is running.</summary>
    protected bool isEngineRunning;

    /// <summary>Gets or sets the collection of IVsaItem objects.</summary>
    protected IJSVsaItems vsaItems;

    /// <summary>Gets or sets the script language.</summary>
    protected string scriptLanguage;

    /// <summary>Gets or sets the error locale.</summary>
    protected int errorLocale;

    /// <summary>Gets or sets the name table.</summary>
    protected static Hashtable nameTable = new Hashtable(10);

    /// <summary>Gets or sets the engine name.</summary>
    protected string engineName;

    /// <summary>Gets or sets the engine moniker.</summary>
    protected string engineMoniker;

    /// <summary>Gets or sets the root namespace.</summary>
    protected string rootNamespace;

    /// <summary>Gets or sets the startup class.</summary>
    protected Type startupClass;

    /// <summary>Gets or sets the startup instance.</summary>
    protected BaseVsaStartup startupInstance;

    /// <summary>Gets or sets the assembly version.</summary>
    protected string assemblyVersion;

    /// <summary>Gets or sets the execution evidence.</summary>
    protected Evidence executionEvidence;

    internal BaseVsaEngine(string language, string version, bool supportDebug)
    {
        this.applicationPath = "";
        this.compiledRootNamespace = (string)null;
        this.genDebugInfo = false;
        this.haveCompiledState = false;
        this.failedCompilation = false;
        this.isClosed = false;
        this.isEngineCompiled = false;
        this.isEngineDirty = false;
        this.isEngineInitialized = false;
        this.isEngineRunning = false;
        this.vsaItems = (IJSVsaItems)null;
        this.engineSite = (IJSVsaSite)null;
        this.errorLocale = CultureInfo.CurrentUICulture.LCID;
        this.engineName = "";
        this.rootNamespace = "";
        this.engineMoniker = "";
        this.scriptLanguage = language;
        this.assemblyVersion = version;
        this.isDebugInfoSupported = supportDebug;
        this.executionEvidence = (Evidence)null;
    }

    /// <summary>Returns an error.</summary>
    /// <returns>An exception with the specified error number.</returns>
    /// <param name="vsaErrorNumber">The error number to be returned.</param>
    protected JSVsaException Error(JSVsaError vsaErrorNumber) => new JSVsaException(vsaErrorNumber);

    internal void TryObtainLock()
    {
        if (!Monitor.TryEnter((object)this))
            throw new JSVsaException(JSVsaError.EngineBusy);
    }

    internal void ReleaseLock() => Monitor.Exit((object)this);

    private bool IsCondition(BaseVsaEngine.Pre flag, BaseVsaEngine.Pre test) => (flag & test) != 0;

    /// <summary>Sets the precondition flags.</summary>
    /// <param name="flags">The flags to set.</param>
    protected void Preconditions(BaseVsaEngine.Pre flags)
    {
        if (this.isClosed)
            throw this.Error(JSVsaError.EngineClosed);
        if (flags == BaseVsaEngine.Pre.EngineNotClosed)
            return;
        if (this.IsCondition(flags, BaseVsaEngine.Pre.SupportForDebug) && !this.isDebugInfoSupported)
            throw this.Error(JSVsaError.DebugInfoNotSupported);
        if (this.IsCondition(flags, BaseVsaEngine.Pre.EngineCompiled) && !this.haveCompiledState)
            throw this.Error(JSVsaError.EngineNotCompiled);
        if (this.IsCondition(flags, BaseVsaEngine.Pre.EngineRunning) && !this.isEngineRunning)
            throw this.Error(JSVsaError.EngineNotRunning);
        if (this.IsCondition(flags, BaseVsaEngine.Pre.EngineNotRunning) && this.isEngineRunning)
            throw this.Error(JSVsaError.EngineRunning);
        if (this.IsCondition(flags, BaseVsaEngine.Pre.RootMonikerSet) && this.engineMoniker == "")
            throw this.Error(JSVsaError.RootMonikerNotSet);
        if (this.IsCondition(flags, BaseVsaEngine.Pre.RootMonikerNotSet) && this.engineMoniker != "")
            throw this.Error(JSVsaError.RootMonikerAlreadySet);
        if (this.IsCondition(flags, BaseVsaEngine.Pre.RootNamespaceSet) && this.rootNamespace == "")
            throw this.Error(JSVsaError.RootNamespaceNotSet);
        if (this.IsCondition(flags, BaseVsaEngine.Pre.SiteSet) && this.engineSite == null)
            throw this.Error(JSVsaError.SiteNotSet);
        if (this.IsCondition(flags, BaseVsaEngine.Pre.SiteNotSet) && this.engineSite != null)
            throw this.Error(JSVsaError.SiteAlreadySet);
        if (this.IsCondition(flags, BaseVsaEngine.Pre.EngineInitialised) && !this.isEngineInitialized)
            throw this.Error(JSVsaError.EngineNotInitialized);
        if (this.IsCondition(flags, BaseVsaEngine.Pre.EngineNotInitialised) && this.isEngineInitialized)
            throw this.Error(JSVsaError.EngineInitialized);
    }

    /// <summary>Gets or sets the application domain.</summary>
    /// <returns>The application domain.</returns>
    public AppDomain AppDomain
    {
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
            throw new NotSupportedException();
        }
        set
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
            throw new JSVsaException(JSVsaError.AppDomainCannotBeSet);
        }
    }

    /// <summary>Provides evidence for the purpose of validating the current object's identity.</summary>
    /// <returns>Returns a reference to an Evidence object.</returns>
    public Evidence Evidence
    {
        [SecurityPermission(SecurityAction.Demand, ControlEvidence = true),
         PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
            return this.executionEvidence;
        }
        [SecurityPermission(SecurityAction.Demand, ControlEvidence = true),
         PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning |
                               BaseVsaEngine.Pre.EngineInitialised);
            this.executionEvidence = value;
        }
    }

    /// <summary>Gets or sets the application base.</summary>
    /// <returns>The application base.</returns>
    public string ApplicationBase
    {
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
            throw new NotSupportedException();
        }
        set
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
            throw new JSVsaException(JSVsaError.ApplicationBaseCannotBeSet);
        }
    }

    /// <summary>Gets a reference to the running assembly generated by Run method.</summary>
    /// <returns>Reference to the currently running assembly.</returns>
    public Assembly Assembly
    {
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineRunning);
            return this.loadedAssembly;
        }
    }

    /// <summary>Sets or gets a Boolean value that signifies whether the script engine produces debug information when the Compile method is called.</summary>
    /// <returns>true to produce debug information; otherwise, false.</returns>
    public bool GenerateDebugInfo
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
            return this.genDebugInfo;
        }
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.SupportForDebug |
                                   BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.EngineInitialised);
                if (this.genDebugInfo == value)
                    return;
                this.genDebugInfo = value;
                this.isEngineDirty = true;
                this.isEngineCompiled = false;
            }
            finally
            {
                this.ReleaseLock();
            }
        }
    }

    /// <summary>Gets or sets a value that indicates whether the source is compiled.</summary>
    /// <returns>true if the source is compiled; otherwise, false.</returns>
    public bool IsCompiled
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
            return this.isEngineCompiled;
        }
    }

    /// <summary>Gets or sets a value that indicates whether the current in-memory representation of the item differs from the persisted representation.</summary>
    /// <returns>true if the in-memory representation differs from the persisted representation; otherwise, false.</returns>
    public bool IsDirty
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
            return this.isEngineDirty;
        }
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
                this.isEngineDirty = value;
                if (!this.isEngineDirty)
                    return;
                this.isEngineCompiled = false;
            }
            finally
            {
                this.ReleaseLock();
            }
        }
    }

    /// <summary>Gets a Boolean value that reports whether the engine is currently in run mode.</summary>
    /// <returns>true if the engine is in run mode; otherwise, false.</returns>
    public bool IsRunning
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
            return this.isEngineRunning;
        }
    }

    /// <summary>Gets the collection of IJSVsaItem objects, which represent all items added to the engine.</summary>
    /// <returns>The collection of items.</returns>
    public IJSVsaItems Items
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
            return this.vsaItems;
        }
    }

    /// <summary>Gets the name of the programming language supported by the engine.</summary>
    /// <returns>The name of the programming language.</returns>
    public string Language
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
            return this.scriptLanguage;
        }
    }

    /// <summary>Gets or sets the geographical locale and language in which to report exception messages.</summary>
    /// <returns>An integer value representing the locale in which exception messages are to be reported.</returns>
    public int LCID
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
            return this.errorLocale;
        }
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning |
                                   BaseVsaEngine.Pre.EngineInitialised);
                try
                {
                    CultureInfo cultureInfo = new CultureInfo(value);
                }
                catch (ArgumentException ex)
                {
                    throw this.Error(JSVsaError.LCIDNotSupported);
                }

                this.errorLocale = value;
                this.isEngineDirty = true;
            }
            finally
            {
                this.ReleaseLock();
            }
        }
    }

    /// <summary>Sets or gets the display name of the engine.</summary>
    /// <returns>The name of the engine.</returns>
    public string Name
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
            return this.engineName;
        }
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning |
                                   BaseVsaEngine.Pre.EngineInitialised);
                if (this.engineName == value)
                    return;
                lock (BaseVsaEngine.nameTable)
                {
                    if (BaseVsaEngine.nameTable[(object)value] != null)
                        throw this.Error(JSVsaError.EngineNameInUse);
                    BaseVsaEngine.nameTable[(object)value] = new object();
                    if (this.engineName != null)
                    {
                        if (this.engineName.Length > 0)
                            BaseVsaEngine.nameTable[(object)this.engineName] = (object)null;
                    }
                }

                this.engineName = value;
                this.isEngineDirty = true;
                this.isEngineCompiled = false;
            }
            finally
            {
                this.ReleaseLock();
            }
        }
    }

    /// <summary>Sets or gets a script engine's root moniker.</summary>
    /// <returns>The root moniker of the engine.</returns>
    public string RootMoniker
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
            return this.engineMoniker;
        }
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.RootMonikerNotSet);
                this.ValidateRootMoniker(value);
                this.engineMoniker = value;
            }
            finally
            {
                this.ReleaseLock();
            }
        }
    }

    /// <summary>Sets or gets the root namespace used by the engine.</summary>
    /// <returns>The namespace of the engine.</returns>
    public string RootNamespace
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
            return this.rootNamespace;
        }
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning |
                                   BaseVsaEngine.Pre.EngineInitialised);
                this.rootNamespace = this.IsValidNamespaceName(value)
                    ? value
                    : throw this.Error(JSVsaError.RootNamespaceInvalid);
                this.isEngineDirty = true;
                this.isEngineCompiled = false;
            }
            finally
            {
                this.ReleaseLock();
            }
        }
    }

    /// <summary>Sets or gets the host-implemented IJSVsaSite object that is used by the engine to communicate with the host.</summary>
    /// <returns>The host-implemented object.</returns>
    public IJSVsaSite Site
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.RootMonikerSet);
            return this.engineSite;
        }
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        set
        {
            this.TryObtainLock();
            try
            {
                this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.RootMonikerSet |
                                   BaseVsaEngine.Pre.SiteNotSet);
                this.engineSite = value != null ? value : throw this.Error(JSVsaError.SiteInvalid);
            }
            finally
            {
                this.ReleaseLock();
            }
        }
    }

    /// <summary>Gets the current version of the language compiler supported by the engine, in the form Major.Minor.Revision.Build.</summary>
    /// <returns>The current version.</returns>
    public string Version
    {
        [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
        get
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
            return this.assemblyVersion;
        }
    }

    /// <summary>Closes the engine.</summary>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual void Close()
    {
        this.TryObtainLock();
        try
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed);
            if (this.isEngineRunning)
                this.Reset();
            lock (BaseVsaEngine.nameTable)
            {
                if (this.engineName != null)
                {
                    if (this.engineName.Length > 0)
                        BaseVsaEngine.nameTable[(object)this.engineName] = (object)null;
                }
            }

            this.DoClose();
            this.isClosed = true;
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Causes the engine to compile the existing source state.</summary>
    /// <returns>true on successful compilation; otherwise, false.</returns>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual bool Compile()
    {
        this.TryObtainLock();
        try
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning |
                               BaseVsaEngine.Pre.RootNamespaceSet | BaseVsaEngine.Pre.EngineInitialised);
            bool flag = false;
            int index = 0;
            for (int count = this.vsaItems.Count; !flag && index < count; ++index)
            {
                IJSVsaItem vsaItem = this.vsaItems[index];
                flag = this.vsaItems[index].ItemType == JSVsaItemType.Code;
            }

            if (!flag)
                throw this.Error(JSVsaError.EngineClosed);
            try
            {
                this.ResetCompiledState();
                this.isEngineCompiled = this.DoCompile();
            }
            catch (JSVsaException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new JSVsaException(JSVsaError.InternalCompilerError, ex.ToString(), ex);
            }

            if (this.isEngineCompiled)
            {
                this.haveCompiledState = true;
                this.failedCompilation = false;
                this.compiledRootNamespace = this.rootNamespace;
            }

            return this.isEngineCompiled;
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Gets implementation-specific options for the engine.</summary>
    /// <returns>The value of the option.</returns>
    /// <param name="name">The name of the option to get.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual object GetOption(string name)
    {
        this.TryObtainLock();
        try
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineInitialised);
            return this.GetCustomOption(name);
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Provides a notification that the initialization phase is complete.</summary>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual void InitNew()
    {
        this.TryObtainLock();
        try
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.RootMonikerSet |
                               BaseVsaEngine.Pre.SiteSet | BaseVsaEngine.Pre.EngineNotInitialised);
            this.isEngineInitialized = true;
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Loads the compiled state.</summary>
    /// <returns>The compiled state.</returns>
    protected virtual Assembly LoadCompiledState()
    {
        byte[] pe;
        byte[] debugInfo;
        this.DoSaveCompiledState(out pe, out debugInfo);
        return Assembly.Load(pe, debugInfo);
    }

    /// <summary>Directs the engine to load source items from their point of persistence, as specified by the host-provided object.</summary>
    /// <param name="site">The site from which the specified source items is loaded.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual void LoadSourceState(IJSVsaPersistSite site)
    {
        this.TryObtainLock();
        try
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.RootMonikerSet |
                               BaseVsaEngine.Pre.SiteSet | BaseVsaEngine.Pre.EngineNotInitialised);
            this.isEngineInitialized = true;
            try
            {
                this.DoLoadSourceState(site);
            }
            catch
            {
                this.isEngineInitialized = false;
                throw;
            }

            this.isEngineDirty = false;
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Removes the engine from the running state and disconnects automatically bound event handlers.</summary>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual void Reset()
    {
        this.TryObtainLock();
        try
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineRunning);
            try
            {
                this.startupInstance.Shutdown();
            }
            catch (Exception ex)
            {
                throw new JSVsaException(JSVsaError.EngineCannotReset, ex.ToString(), ex);
            }

            this.isEngineRunning = false;
            this.loadedAssembly = (Assembly)null;
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Invalidates the cached assembly for the engine, as specified by its root moniker.</summary>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual void RevokeCache()
    {
        this.TryObtainLock();
        try
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning |
                               BaseVsaEngine.Pre.RootMonikerSet);
            try
            {
                System.AppDomain.CurrentDomain.SetData(this.engineMoniker, (object)null);
            }
            catch (Exception ex)
            {
                throw new JSVsaException(JSVsaError.RevokeFailed, ex.ToString(), ex);
            }
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Initiates execution of compiled code in the engine and binds all event handlers</summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual void Run()
    {
        this.TryObtainLock();
        try
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning |
                               BaseVsaEngine.Pre.RootMonikerSet | BaseVsaEngine.Pre.RootNamespaceSet |
                               BaseVsaEngine.Pre.SiteSet);
            System.AppDomain currentDomain = System.AppDomain.CurrentDomain;
            if (this.haveCompiledState)
            {
                if (this.rootNamespace != this.compiledRootNamespace)
                    throw new JSVsaException(JSVsaError.RootNamespaceInvalid);
                this.loadedAssembly = this.LoadCompiledState();
                currentDomain.SetData(this.engineMoniker, (object)this.loadedAssembly);
            }
            else
            {
                if (this.failedCompilation)
                    throw new JSVsaException(JSVsaError.EngineNotCompiled);
                this.startupClass = (Type)null;
                this.loadedAssembly = currentDomain.GetData(this.engineMoniker) as Assembly;
                if (this.loadedAssembly == (Assembly)null)
                {
                    Mutex mutex = new Mutex(false,
                        $"{this.engineMoniker}/{currentDomain.GetHashCode().ToString((IFormatProvider)CultureInfo.InvariantCulture)}");
                    if (mutex.WaitOne())
                    {
                        try
                        {
                            this.loadedAssembly = currentDomain.GetData(this.engineMoniker) as Assembly;
                            if (this.loadedAssembly == (Assembly)null)
                            {
                                byte[] pe;
                                byte[] debugInfo;
                                this.engineSite.GetCompiledState(out pe, out debugInfo);
                                if (pe == null)
                                    throw new JSVsaException(JSVsaError.GetCompiledStateFailed);
                                this.loadedAssembly = Assembly.Load(pe, debugInfo);
                                currentDomain.SetData(this.engineMoniker, (object)this.loadedAssembly);
                            }
                        }
                        finally
                        {
                            mutex.ReleaseMutex();
                            mutex.Close();
                        }
                    }
                }
            }

            try
            {
                if (this.startupClass == (Type)null)
                    this.startupClass = this.loadedAssembly.GetType(this.rootNamespace + "._Startup", true);
            }
            catch (Exception ex)
            {
                throw new JSVsaException(JSVsaError.BadAssembly, ex.ToString(), ex);
            }

            try
            {
                this.startupInstance = (BaseVsaStartup)Activator.CreateInstance(this.startupClass);
                this.isEngineRunning = true;
                this.startupInstance.SetSite(this.engineSite);
                this.startupInstance.Startup();
            }
            catch (Exception ex)
            {
                throw new JSVsaException(JSVsaError.UnknownError, ex.ToString(), ex);
            }
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Sets implementation-specific options for the engine.</summary>
    /// <param name="name">The name of the option to set.</param>
    /// <param name="value">The value for the option being set.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual void SetOption(string name, object value)
    {
        this.TryObtainLock();
        try
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning |
                               BaseVsaEngine.Pre.EngineInitialised);
            this.SetCustomOption(name, value);
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Saves the compiled state of the engine; optionally, it also saves debugging information.</summary>
    /// <param name="pe">The compiled state of the script engine.</param>
    /// <param name="debugInfo">Specifies debugging information contained in the .PDB file corresponding to the PE (portable executable).</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual void SaveCompiledState(out byte[] pe, out byte[] debugInfo)
    {
        this.TryObtainLock();
        try
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineCompiled |
                               BaseVsaEngine.Pre.EngineNotRunning | BaseVsaEngine.Pre.EngineInitialised);
            this.DoSaveCompiledState(out pe, out debugInfo);
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Directs the engine to persist its source state.</summary>
    /// <param name="site">The site to which source state is saved. </param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public virtual void SaveSourceState(IJSVsaPersistSite site)
    {
        this.TryObtainLock();
        try
        {
            this.Preconditions(BaseVsaEngine.Pre.EngineNotClosed | BaseVsaEngine.Pre.EngineNotRunning |
                               BaseVsaEngine.Pre.EngineInitialised);
            if (site == null)
                throw this.Error(JSVsaError.SiteInvalid);
            try
            {
                this.DoSaveSourceState(site);
            }
            catch (Exception ex)
            {
                throw new JSVsaException(JSVsaError.SaveElementFailed, ex.ToString(), ex);
            }
        }
        finally
        {
            this.ReleaseLock();
        }
    }

    /// <summary>Validates the root moniker.</summary>
    /// <param name="rootMoniker">The root moniker to validate.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    protected virtual void ValidateRootMoniker(string rootMoniker)
    {
        if (rootMoniker == null)
            throw new JSVsaException(JSVsaError.RootMonikerInvalid);
        Uri uri;
        try
        {
            uri = new Uri(rootMoniker);
        }
        catch (UriFormatException ex)
        {
            throw new JSVsaException(JSVsaError.RootMonikerInvalid);
        }

        string scheme = uri.Scheme;
        if (scheme.Length == 0)
            throw new JSVsaException(JSVsaError.RootMonikerProtocolInvalid);
        string[] strArray = new string[18]
        {
            "file",
            "ftp",
            "gopher",
            "http",
            "https",
            "javascript",
            "mailto",
            "microsoft",
            "news",
            "res",
            "smtp",
            "socks",
            "vbscript",
            "xlang",
            "xml",
            "xpath",
            "xsd",
            "xsl"
        };
        try
        {
            new RegistryPermission(RegistryPermissionAccess.Read,
                "HKEY_LOCAL_MACHINE\\SOFTWARE\\Classes\\PROTOCOLS\\Handler").Assert();
            strArray = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Classes\\PROTOCOLS\\Handler").GetSubKeyNames();
        }
        catch
        {
        }

        foreach (string strA in strArray)
        {
            if (string.Compare(strA, scheme, StringComparison.OrdinalIgnoreCase) == 0)
                throw new JSVsaException(JSVsaError.RootMonikerProtocolInvalid);
        }
    }

    /// <summary>Closes the engine.</summary>
    protected abstract void DoClose();

    /// <summary>Causes the script engine to compile the existing source state.</summary>
    /// <returns>true on successful compilation; otherwise, false.</returns>
    protected abstract bool DoCompile();

    /// <summary>Directs the engine to load source items.</summary>
    /// <param name="site">The site from which the specified source items are loaded.</param>
    protected abstract void DoLoadSourceState(IJSVsaPersistSite site);

    /// <summary>Saves the compiled state of the script engine; optionally, it also saves debugging information.</summary>
    /// <param name="pe"> The compiled state of the engine.</param>
    /// <param name="debugInfo"> Specifies debugging information contained in the .PDB file.</param>
    protected abstract void DoSaveCompiledState(out byte[] pe, out byte[] debugInfo);

    /// <summary>Directs the script engine to persist its source state to the specified object.</summary>
    /// <param name="site"> The site to which source state is saved.</param>
    protected abstract void DoSaveSourceState(IJSVsaPersistSite site);

    /// <summary>Gets implementation-specific options for the engine.</summary>
    /// <returns>The value of the option.</returns>
    /// <param name="name">The name of the option to get.</param>
    protected abstract object GetCustomOption(string name);

    /// <summary>Checks whether the supplied namespace is valid for the engine.</summary>
    /// <returns>true if the namespace is valid; otherwise, it returns false.</returns>
    /// <param name="name">A string value provided as identifier.</param>
    protected abstract bool IsValidNamespaceName(string name);

    /// <summary>Checks whether the supplied identifier is valid for the engine.</summary>
    /// <returns>true if the identifier is valid; otherwise, it returns false.</returns>
    /// <param name="ident">A string value provided as identifier.</param>
    [PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
    public abstract bool IsValidIdentifier(string ident);

    /// <summary>Resets the compiled state.</summary>
    protected abstract void ResetCompiledState();

    /// <summary>Sets implementation-specific options for the engine.</summary>
    /// <param name="name">The name of the option to set.</param>
    /// <param name="value">The value for the option being set.</param>
    protected abstract void SetCustomOption(string name, object value);

    /// <summary>Provides enumeration for the <see cref="T:Microsoft.JScript.Vsa.BaseVsaEngine" /> class.</summary>
    [Flags]
    protected enum Pre
    {
        /// <summary>No engine is set.</summary>
        None = 0,

        /// <summary>The engine is not closed.</summary>
        EngineNotClosed = 1,

        /// <summary>The debug features are switched on.</summary>
        SupportForDebug = 2,

        /// <summary>The engine is compiled.</summary>
        EngineCompiled = 4,

        /// <summary>The engine is running.</summary>
        EngineRunning = 8,

        /// <summary>The engine is not running.</summary>
        EngineNotRunning = 16, // 0x00000010

        /// <summary>The root moniker is set.</summary>
        RootMonikerSet = 32, // 0x00000020

        /// <summary>The root moniker is not set.</summary>
        RootMonikerNotSet = 64, // 0x00000040

        /// <summary>The root namespace is set.</summary>
        RootNamespaceSet = 128, // 0x00000080

        /// <summary>The site is set.</summary>
        SiteSet = 256, // 0x00000100

        /// <summary>The site is not set.</summary>
        SiteNotSet = 512, // 0x00000200

        /// <summary>The engine is initialized.</summary>
        EngineInitialised = 1024, // 0x00000400

        /// <summary>The engine is not initialized.</summary>
        EngineNotInitialised = 2048, // 0x00000800
    }
}