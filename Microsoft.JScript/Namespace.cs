// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Namespace
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a namespace.</summary>
public sealed class Namespace
{
    private string name;
    internal VsaEngine engine;

    private Namespace(string name, VsaEngine engine)
    {
        this.name = name;
        this.engine = engine;
    }

    /// <summary>Creates and returns a new instance of the <see cref="T:Microsoft.JScript.Namespace" /> class.</summary>
    /// <returns>A new instance of the <see cref="T:Microsoft.JScript.Namespace" /> class.</returns>
    /// <param name="name">The name of the namespace.</param>
    /// <param name="engine">A reference to the scripting engine.</param>
    public static Namespace GetNamespace(string name, VsaEngine engine)
    {
        return new Namespace(name, engine);
    }

    internal Type GetType(string typeName) => this.engine.GetType(typeName);

    internal string Name => this.name;
}