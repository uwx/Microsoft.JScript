// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.IActivationObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Defines methods for resolving scope. This interface is implemented by all objects that take part in a scope hierarchy.</summary>
public interface IActivationObject
{
    /// <summary>When implemented in a class, gets the current default scope. Either the contained object of a with statement scope, or the global scope.</summary>
    /// <returns>The current default scope.</returns>
    object GetDefaultThisObject();

    /// <summary>When implemented in a class, gets the global scope by searching the scope hierarchy until it finds a scope that includes static variables, for example, a module scope.</summary>
    /// <returns>The global scope.</returns>
    GlobalScope GetGlobalScope();

    /// <summary>When implemented in a class, gets the local declaration of a field that has the specified name. This is used at compile time for error checking.</summary>
    /// <returns>The local declaration of the field.</returns>
    /// <param name="name">The name of the field.</param>
    FieldInfo GetLocalField(string name);

    /// <summary>When implemented in a class, gets the value of the specified member by searching the scope hierarchy. This method searches only the scope that is specified by the lexical level.</summary>
    /// <returns>The value of the member <paramref name="name" />.</returns>
    /// <param name="name">The name of the member to search for.</param>
    /// <param name="lexlevel">The lexical level to search.</param>
    object GetMemberValue(string name, int lexlevel);

    /// <summary>When implemented in a class, returns a field that has the specified name by searching the scope hierarchy. This method searches only the scope that is specified by the lexical level.</summary>
    /// <returns>A <see cref="T:System.Reflection.FieldInfo" /> object that has the specified name.</returns>
    /// <param name="name">The name of the field to search for.</param>
    /// <param name="lexLevel">The lexical level to search.</param>
    FieldInfo GetField(string name, int lexLevel);
}