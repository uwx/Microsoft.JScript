// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Globals
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Configuration.Assemblies;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Stores references to all globally available types and manages the stack. This class belongs to the compilation and run-time state category.</summary>
public sealed class Globals
{
    [ThreadStatic] private static TypeReferences _typeRefs;
    private Stack callContextStack;
    private Stack scopeStack;
    internal object caller;
    private SimpleHashtable regExpTable;
    internal GlobalObject globalObject;
    internal VsaEngine engine;
    internal bool assemblyDelaySign;
    internal CultureInfo assemblyCulture;

    internal AssemblyFlags assemblyFlags =
        AssemblyFlags.EnableJITcompileTracking | AssemblyFlags.DisableJITcompileOptimizer;

    internal System.Configuration.Assemblies.AssemblyHashAlgorithm assemblyHashAlgorithm =
        System.Configuration.Assemblies.AssemblyHashAlgorithm.SHA1;

    internal string assemblyKeyFileName;
    internal Context assemblyKeyFileNameContext;
    internal string assemblyKeyName;
    internal Context assemblyKeyNameContext;
    internal Version assemblyVersion;
    internal AssemblyVersionCompatibility assemblyVersionCompatibility;
    private static SimpleHashtable BuiltinFunctionTable;

    /// <summary>Stores a global reference to the single scripting engine that is running in the current context.</summary>
    [ContextStatic] public static VsaEngine contextEngine;

    internal static TypeReferences TypeRefs
    {
        get => Globals._typeRefs ?? (Globals._typeRefs = Runtime.TypeRefs);
        set => Globals._typeRefs = value;
    }

    internal Globals(bool fast, VsaEngine engine)
    {
        this.engine = engine;
        this.callContextStack = (Stack)null;
        this.scopeStack = (Stack)null;
        this.caller = (object)DBNull.Value;
        this.regExpTable = (SimpleHashtable)null;
        if (fast)
            this.globalObject = GlobalObject.commonInstance;
        else
            this.globalObject = (GlobalObject)new LenientGlobalObject(engine);
    }

    internal static BuiltinFunction BuiltinFunctionFor(object obj, MethodInfo meth)
    {
        if (Globals.BuiltinFunctionTable == null)
            Globals.BuiltinFunctionTable = new SimpleHashtable(64U /*0x40*/);
        BuiltinFunction builtinFunction1 = (BuiltinFunction)Globals.BuiltinFunctionTable[(object)meth];
        if (builtinFunction1 != null)
            return builtinFunction1;
        BuiltinFunction builtinFunction2 = new BuiltinFunction(obj, meth);
        lock (Globals.BuiltinFunctionTable)
            Globals.BuiltinFunctionTable[(object)meth] = (object)builtinFunction2;
        return builtinFunction2;
    }

    internal Stack CallContextStack
    {
        get
        {
            if (this.callContextStack == null)
                this.callContextStack = new Stack();
            return this.callContextStack;
        }
    }

    /// <summary>Provides global access to the <see cref="M:Microsoft.JScript.ArrayConstructor.CreateInstance(System.Object[])" /> method.</summary>
    /// <returns>A new instance of a JScript <see cref="T:Microsoft.JScript.ArrayObject" />.</returns>
    /// <param name="args">The values to populate the array with.</param>
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
    public static ArrayObject ConstructArray(params object[] args)
    {
        return (ArrayObject)ArrayConstructor.ob.Construct(args);
    }

    /// <summary>Provides global access to the <see cref="M:Microsoft.JScript.ArrayConstructor.ConstructArray(System.Object[])" /> method.</summary>
    /// <returns>A new instance of a JScript <see cref="T:Microsoft.JScript.ArrayObject" />.</returns>
    /// <param name="args">The values to populate the array with.</param>
    public static ArrayObject ConstructArrayLiteral(object[] args)
    {
        return ArrayConstructor.ob.ConstructArray(args);
    }

    internal SimpleHashtable RegExpTable
    {
        get
        {
            if (this.regExpTable == null)
                this.regExpTable = new SimpleHashtable(8U);
            return this.regExpTable;
        }
    }

    internal Stack ScopeStack
    {
        get
        {
            if (this.scopeStack == null)
            {
                this.scopeStack = new Stack();
                this.scopeStack.Push(this.engine.GetGlobalScope().GetObject());
            }

            return this.scopeStack;
        }
    }
}