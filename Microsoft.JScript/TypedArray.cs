// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.TypedArray
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a reflectable reference to the <see cref="T:System.Array" /> class. This class stores the element type of a typed array, but this class does not store data. </summary>
public sealed class TypedArray : IReflect
{
    internal IReflect elementType;
    internal int rank;

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.TypedArray" /> class.</summary>
    /// <param name="elementType">The element type of a typed array.</param>
    /// <param name="rank">The rank (number of dimensions) of the array.</param>
    public TypedArray(IReflect elementType, int rank)
    {
        this.elementType = elementType;
        this.rank = rank;
    }

    /// <summary>Determines whether the specified typed array has the same element type and rank as the current typed array.</summary>
    /// <returns>true if <paramref name="obj" /> has the same element type and rank as the current typed array; otherwise, false.</returns>
    /// <param name="obj">The typed array to compare with the current typed array.</param>
    public override bool Equals(object obj)
    {
        if (obj is TypedArray)
            return this.ToString().Equals(obj.ToString());
        Type type = obj as Type;
        return !(type == (Type)null) && type.IsArray && type.GetArrayRank() == this.rank &&
               this.elementType.Equals((object)type.GetElementType());
    }

    /// <summary>Gets a field of the <see cref="T:System.Array" /> class type that has the specified name.</summary>
    /// <returns>A field that has the specified name, if one is found; otherwise, null.</returns>
    /// <param name="name">The name of the field to get.</param>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public FieldInfo GetField(string name, BindingFlags bindingAttr)
    {
        return Typeob.Array.GetField(name, bindingAttr);
    }

    /// <summary>Gets an array of <see cref="T:System.Reflection.FieldInfo" /> objects that correspond to all fields of the <see cref="T:System.Array" /> class type.</summary>
    /// <returns>An array of objects that contains all fields of the <see cref="T:System.Array" /> class type.</returns>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public FieldInfo[] GetFields(BindingFlags bindingAttr) => Typeob.Array.GetFields(bindingAttr);

    /// <summary>Serves as a hash function for the element type of the typed array.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode() => this.ToString().GetHashCode();

    /// <summary>Gets an array of <see cref="T:System.Reflection.MemberInfo" /> objects that correspond to all members of the <see cref="T:System.Array" /> class type that match a specified name.</summary>
    /// <returns>An array of objects that match the <paramref name="name" /> argument.</returns>
    /// <param name="name">The name of the member to search for.</param>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
    {
        return Typeob.Array.GetMember(name, bindingAttr);
    }

    /// <summary>Gets an array of <see cref="T:System.Reflection.MemberInfo" /> objects that correspond to all members of the <see cref="T:System.Array" /> class type.</summary>
    /// <returns>An array of objects that contains all members of the <see cref="T:System.Array" /> class type.</returns>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public MemberInfo[] GetMembers(BindingFlags bindingAttr) => Typeob.Array.GetMembers(bindingAttr);

    /// <summary>Gets a method of the <see cref="T:System.Array" /> class type that has the specified name.</summary>
    /// <returns>A method that has the specified name, if one is found; otherwise, null.</returns>
    /// <param name="name">The name of the method to get.</param>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
    {
        return Typeob.Array.GetMethod(name, bindingAttr);
    }

    /// <summary>Gets a method of the <see cref="T:System.Array" /> class type that has the specified name by using the specified selection criteria.</summary>
    /// <returns>A method that has the specified name, if one is found; otherwise, null.</returns>
    /// <param name="name">The name of the method to get.</param>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    /// <param name="binder">An object that performs type conversion from actual argument type to formal argument type.</param>
    /// <param name="types">The parameter types that are used to locate a matching method.</param>
    /// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified.</param>
    public MethodInfo GetMethod(
        string name,
        BindingFlags bindingAttr,
        Binder binder,
        Type[] types,
        ParameterModifier[] modifiers)
    {
        return Typeob.Array.GetMethod(name, bindingAttr, binder, types, modifiers);
    }

    /// <summary>Gets an array of <see cref="T:System.Reflection.MethodInfo" /> objects that correspond to all methods of the <see cref="T:System.Array" /> class type.</summary>
    /// <returns>An array of objects that contains all methods of the <see cref="T:System.Array" /> class type.</returns>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public MethodInfo[] GetMethods(BindingFlags bindingAttr) => Typeob.Array.GetMethods(bindingAttr);

    /// <summary>Gets a property of the <see cref="T:System.Array" /> class type that has the specified name.</summary>
    /// <returns>A property that has the specified name, if one is found; otherwise, null.</returns>
    /// <param name="name">The name of the property to get.</param>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
    {
        return Typeob.Array.GetProperty(name, bindingAttr);
    }

    /// <summary>Gets a property of the <see cref="T:System.Array" /> class type that has the specified name by using the specified selection criteria.</summary>
    /// <returns>A property that has the specified name, if one is found; otherwise, null.</returns>
    /// <param name="name">The name of the property to get.</param>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    /// <param name="binder">An object that performs type conversion from actual argument type to formal argument type.</param>
    /// <param name="returnType">The type of the return value that the property must have.</param>
    /// <param name="types">The index types of the property being searched for. Used for index properties, such as the indexer for a class.</param>
    /// <param name="modifiers">An array of parameter modifiers that enable binding to work with parameter signatures in which the types have been modified.</param>
    public PropertyInfo GetProperty(
        string name,
        BindingFlags bindingAttr,
        Binder binder,
        Type returnType,
        Type[] types,
        ParameterModifier[] modifiers)
    {
        return Typeob.Array.GetProperty(name, bindingAttr, binder, returnType, types, modifiers);
    }

    /// <summary>Gets an array of <see cref="T:System.Reflection.PropertyInfo" /> objects that correspond to all properties of the <see cref="T:System.Array" /> class type.</summary>
    /// <returns>An array of objects that contains all properties of the <see cref="T:System.Array" /> class type.</returns>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public PropertyInfo[] GetProperties(BindingFlags bindingAttr)
    {
        return Typeob.Array.GetProperties(bindingAttr);
    }

    /// <summary>Invokes the specified member of the element type or the <see cref="T:System.Array" /> class type.</summary>
    /// <returns>The return value of the call to the member.</returns>
    /// <param name="name">The name of the member to invoke.</param>
    /// <param name="flags">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    /// <param name="binder">An object that performs type conversion from actual argument type to formal argument type.</param>
    /// <param name="target">The object on which to invoke the member. This must be the current script object.</param>
    /// <param name="args">An array of objects that contains the number, order, and type of the parameters of the member to be invoked. This is an empty array if there are no parameters.</param>
    /// <param name="modifiers">An array that has the same length as the <paramref name="args" /> parameter. It represents the invoked member's argument attributes in the metadata.</param>
    /// <param name="locale">Provides information about a specific culture or locale. Used to format numbers, dates, and strings correctly.</param>
    /// <param name="namedParameters">An array of parameters.</param>
    public object InvokeMember(
        string name,
        BindingFlags flags,
        Binder binder,
        object target,
        object[] args,
        ParameterModifier[] modifiers,
        CultureInfo locale,
        string[] namedParameters)
    {
        return (flags & BindingFlags.CreateInstance) == BindingFlags.Default
            ? LateBinding.CallValue((object)this.elementType, args, true, true, (VsaEngine)null, (object)null, binder,
                locale, namedParameters)
            : Typeob.Array.InvokeMember(name, flags, binder, target, args, modifiers, locale, namedParameters);
    }

    internal static string ToRankString(int rank)
    {
        switch (rank)
        {
            case 1:
                return "[]";
            case 2:
                return "[,]";
            case 3:
                return "[,,]";
            default:
                StringBuilder stringBuilder = new StringBuilder(rank + 1);
                stringBuilder.Append('[');
                for (int index = 1; index < rank; ++index)
                    stringBuilder.Append(',');
                stringBuilder.Append(']');
                return stringBuilder.ToString();
        }
    }

    /// <summary>Returns a string that contains the element type of the typed array and the rank information.</summary>
    /// <returns>A string that contains the element type of the typed array and the rank information.</returns>
    public override string ToString()
    {
        Type elementType1 = this.elementType as Type;
        if (elementType1 != (Type)null)
            return elementType1.FullName + TypedArray.ToRankString(this.rank);
        if (this.elementType is ClassScope elementType2)
            return elementType2.GetFullName() + TypedArray.ToRankString(this.rank);
        return this.elementType is TypedArray elementType3
            ? elementType3.ToString() + TypedArray.ToRankString(this.rank)
            : Convert.ToType(this.elementType).FullName + TypedArray.ToRankString(this.rank);
    }

    internal Type ToType()
    {
        Type type = Convert.ToType(this.elementType);
        return Convert.ToType(TypedArray.ToRankString(this.rank), type);
    }

    /// <summary>Gets the type of the <see cref="T:Microsoft.JScript.TypedArray" /> object.</summary>
    /// <returns>The type of the <see cref="T:Microsoft.JScript.TypedArray" /> object.</returns>
    public Type UnderlyingSystemType => this.GetType();
}