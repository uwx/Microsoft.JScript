// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ActivationObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents an object that is taking part in a scope hierarchy. This class contains methods for resolving scope and for getting member information. It belongs to the compilation and run-time state category.</summary>
[ComVisible(true)]
public abstract class ActivationObject : ScriptObject, IActivationObject
{
    internal bool isKnownAtCompileTime;
    internal bool fast;
    internal SimpleHashtable name_table;

    /// <summary>Stores an array of variables that are in scope.</summary>
    protected ArrayList field_table;

    internal ActivationObject(ScriptObject parent)
        : base(parent)
    {
        this.name_table = new SimpleHashtable(32U /*0x20*/);
        this.field_table = new ArrayList();
    }

    internal virtual JSVariableField AddFieldOrUseExistingField(
        string name,
        object value,
        FieldAttributes attributeFlags)
    {
        FieldInfo fieldInfo = (FieldInfo)this.name_table[(object)name];
        if (fieldInfo is JSVariableField)
        {
            if (!(value is Missing))
                ((JSVariableField)fieldInfo).value = value;
            return (JSVariableField)fieldInfo;
        }

        if (value is Missing)
            value = (object)null;
        return this.AddNewField(name, value, attributeFlags);
    }

    internal void AddClassesExcluding(ClassScope excludedClass, string name, ArrayList result)
    {
        ArrayList arrayList = new ArrayList();
        foreach (MemberInfo member in this.GetMembers(
                     BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
        {
            if (member is JSVariableField && ((FieldInfo)member).IsLiteral)
            {
                object obj = ((JSVariableField)member).value;
                if (obj is ClassScope)
                {
                    ClassScope other = (ClassScope)obj;
                    if (!(other.name != member.Name) &&
                        (excludedClass == null || !excludedClass.IsSameOrDerivedFrom(other)) && other.GetMember(name,
                            BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public |
                            BindingFlags.NonPublic).Length != 0)
                        arrayList.Add((object)other);
                }
            }
        }

        if (arrayList.Count == 0)
            return;
        ClassScope[] classScopeArray = new ClassScope[arrayList.Count];
        arrayList.CopyTo((Array)classScopeArray);
        Array.Sort<ClassScope>(classScopeArray);
        result.AddRange((ICollection)classScopeArray);
    }

    internal virtual JSVariableField AddNewField(
        string name,
        object value,
        FieldAttributes attributeFlags)
    {
        JSVariableField field = this.CreateField(name, attributeFlags, value);
        this.name_table[(object)name] = (object)field;
        this.field_table.Add((object)field);
        return field;
    }

    /// <summary>Creates a new global variable, using the specified name, value, and attributes.</summary>
    /// <returns>A global variable that has the specified name, value, and attributes.</returns>
    /// <param name="name">The name of the field.</param>
    /// <param name="attributeFlags">A bitwise combination of the enumeration values that describe the attributes of a field. <see cref="F:System.Reflection.FieldAttributes.Static" /> is added automatically.</param>
    /// <param name="value">The value of the field.</param>
    protected virtual JSVariableField CreateField(
        string name,
        FieldAttributes attributeFlags,
        object value)
    {
        return (JSVariableField)new JSGlobalField((ScriptObject)this, name, value,
            attributeFlags | FieldAttributes.Static);
    }

    /// <summary>Returns a field that has the specified name by searching the scope hierarchy. This method searches only the scope that is specified by the lexical level.</summary>
    /// <returns>A <see cref="T:System.Reflection.FieldInfo" /> object that has the specified name.</returns>
    /// <param name="name">The name of the field to search for.</param>
    /// <param name="lexLevel">The lexical level to search.</param>
    public virtual FieldInfo GetField(string name, int lexLevel)
    {
        throw new JScriptException(JSError.InternalError);
    }

    internal virtual string GetName() => (string)null;

    /// <summary>Gets the current default scope, which is either the contained object of a with statement scope, or the global scope.</summary>
    /// <returns>The current default scope.</returns>
    public virtual object GetDefaultThisObject()
    {
        return ((IActivationObject)this.GetParent()).GetDefaultThisObject();
    }

    /// <summary>Gets the global scope by searching the scope hierarchy until it finds a scope that includes static variables, for example, a module scope.</summary>
    /// <returns>The global scope.</returns>
    public virtual GlobalScope GetGlobalScope()
    {
        return ((IActivationObject)this.GetParent()).GetGlobalScope();
    }

    /// <summary>Gets the local declaration of a field that has the specified name. This is used at compile time for error checking.</summary>
    /// <returns>The local declaration of the field.</returns>
    /// <param name="name">The name of the field.</param>
    public virtual FieldInfo GetLocalField(string name) => (FieldInfo)this.name_table[(object)name];

    /// <summary>Retrieves an array of <see cref="T:System.Reflection.MemberInfo" /> objects that correspond to all members that match a specified name.</summary>
    /// <returns>An array of objects that match the <paramref name="name" /> argument.</returns>
    /// <param name="name">The name of the member to search for.</param>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
    {
        FieldInfo fieldInfo = (FieldInfo)this.name_table[(object)name];
        return fieldInfo != (FieldInfo)null
            ? new MemberInfo[1]
            {
                (MemberInfo)fieldInfo
            }
            : (this.parent != null && (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default
                ? ScriptObject.WrapMembers(this.parent.GetMember(name, bindingAttr), (object)this.parent)
                : new MemberInfo[0]);
    }

    /// <summary>Retrieves an array of <see cref="T:System.Reflection.MemberInfo" /> objects that corresponds to all members of the current object.</summary>
    /// <returns>An array of objects that contains all members of the current object.</returns>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
    {
        int count = this.field_table.Count;
        MemberInfo[] members = new MemberInfo[count];
        for (int index = 0; index < count; ++index)
            members[index] = (MemberInfo)this.field_table[index];
        return members;
    }

    /// <summary>Gets the value of the specified member by searching the scope hierarchy. This method searches only the scope that is specified by the lexical level.</summary>
    /// <returns>The value of the member <paramref name="name" />.</returns>
    /// <param name="name">The name of the member to search for.</param>
    /// <param name="lexlevel">The lexical level to search.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public object GetMemberValue(string name, int lexlevel)
    {
        if (lexlevel <= 0)
            return (object)Missing.Value;
        FieldInfo fieldInfo = (FieldInfo)this.name_table[(object)name];
        if (fieldInfo != (FieldInfo)null)
            return fieldInfo.GetValue((object)this);
        return this.parent != null
            ? ((IActivationObject)this.parent).GetMemberValue(name, lexlevel - 1)
            : (object)Missing.Value;
    }
}