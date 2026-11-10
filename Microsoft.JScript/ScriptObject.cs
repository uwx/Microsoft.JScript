// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ScriptObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Supports all JScript classes. This is the ultimate base class of all JScript classes. This class belongs to the objects and functions category.</summary>
[ComVisible(true)]
public abstract class ScriptObject : IReflect
{
    /// <summary>Stores a reference to the parent object.</summary>
    protected ScriptObject parent;

    internal SimpleHashtable wrappedMemberCache;

    /// <summary>Stores a reference to the scripting engine.</summary>
    public VsaEngine engine;

    internal ScriptObject(ScriptObject parent)
    {
        this.parent = parent;
        this.wrappedMemberCache = (SimpleHashtable)null;
        if (this.parent != null)
            this.engine = parent.engine;
        else
            this.engine = (VsaEngine)null;
    }

    internal virtual bool DeleteMember(string name) => false;

    internal virtual object GetDefaultValue(PreferredType preferred_type)
    {
        throw new JScriptException(JSError.InternalError);
    }

    /// <summary>Gets a field that has the specified name.</summary>
    /// <returns>A field that has the specified <paramref name="name" />, or null if the script object does not contain the field.</returns>
    /// <param name="name">The name of the field to get.</param>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public FieldInfo GetField(string name, BindingFlags bindingAttr)
    {
        foreach (MemberInfo field in this.GetMember(name, bindingAttr))
        {
            if (field.MemberType == MemberTypes.Field)
                return (FieldInfo)field;
        }

        return (FieldInfo)null;
    }

    /// <summary>Gets an array of <see cref="T:System.Reflection.FieldInfo" /> objects that correspond to all fields of the current script object.</summary>
    /// <returns>An array of objects that contains all fields of the current script object.</returns>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public virtual FieldInfo[] GetFields(BindingFlags bindingAttr)
    {
        if (this is ArrayObject arrayObject && arrayObject.denseArrayLength > 0U)
        {
            uint num = arrayObject.denseArrayLength;
            if (num > arrayObject.len)
                num = arrayObject.len;
            for (uint index = 0; index < num; ++index)
            {
                object dense = arrayObject.denseArray[(int)index];
                if (dense != Missing.Value)
                    arrayObject.SetMemberValue2(index.ToString((IFormatProvider)CultureInfo.InvariantCulture), dense);
            }

            arrayObject.denseArrayLength = 0U;
            arrayObject.denseArray = (object[])null;
        }

        MemberInfo[] members = this.GetMembers(bindingAttr);
        if (members == null)
            return new FieldInfo[0];
        int length = 0;
        foreach (MemberInfo memberInfo in members)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
                ++length;
        }

        FieldInfo[] fields = new FieldInfo[length];
        int num1 = 0;
        foreach (MemberInfo memberInfo in members)
        {
            if (memberInfo.MemberType == MemberTypes.Field)
                fields[num1++] = (FieldInfo)memberInfo;
        }

        return fields;
    }

    /// <summary>When overridden in a derived class, gets an array of <see cref="T:System.Reflection.MemberInfo" /> objects that correspond to all members that match a specified name.</summary>
    /// <returns>An array of objects that match the <paramref name="name" /> argument.</returns>
    /// <param name="name">The name of the member to search for.</param>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public abstract MemberInfo[] GetMember(string name, BindingFlags bindingAttr);

    internal virtual object GetMemberValue(string name)
    {
        MemberInfo[] member = this.GetMember(name, BindingFlags.Instance | BindingFlags.Public);
        return member.Length == 0
            ? (object)Missing.Value
            : LateBinding.GetMemberValue((object)this, name, LateBinding.SelectMember(member), member);
    }

    /// <summary>When overridden in a derived class, gets an array of <see cref="T:System.Reflection.MemberInfo" /> objects that correspond to all members of the current script object.</summary>
    /// <returns>An array of objects that contains all members of the current script object.</returns>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public abstract MemberInfo[] GetMembers(BindingFlags bindingAttr);

    /// <summary>Gets a method that has the specified name.</summary>
    /// <returns>A method that has the specified <paramref name="name" />, or null if the script object does not contain the method.</returns>
    /// <param name="name">The name of the method to get.</param>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public MethodInfo GetMethod(string name, BindingFlags bindingAttr)
    {
        return this.GetMethod(name, bindingAttr, (Binder)JSBinder.ob, Type.EmptyTypes, (ParameterModifier[])null);
    }

    /// <summary>Gets a method that has the specified name by using the specified selection criteria.</summary>
    /// <returns>A method that has the specified <paramref name="name" />, or null if the script object does not contain the method.</returns>
    /// <param name="name">The name of the method to search for.</param>
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
        MemberInfo[] member = this.GetMember(name, bindingAttr);
        if (member.Length == 1)
            return member[0] as MethodInfo;
        int length = 0;
        foreach (MemberInfo memberInfo in member)
        {
            if (memberInfo.MemberType == MemberTypes.Method)
                ++length;
        }

        if (length == 0)
            return (MethodInfo)null;
        MethodInfo[] match = new MethodInfo[length];
        int num = 0;
        foreach (MemberInfo memberInfo in member)
        {
            if (memberInfo.MemberType == MemberTypes.Method)
                match[num++] = (MethodInfo)memberInfo;
        }

        if (binder == null)
            binder = (Binder)JSBinder.ob;
        return (MethodInfo)binder.SelectMethod(bindingAttr, (MethodBase[])match, types, modifiers);
    }

    /// <summary>Gets an array of <see cref="T:System.Reflection.MethodInfo" /> objects that correspond to all methods of the current script object.</summary>
    /// <returns>An array of objects that contains all methods of the current script object.</returns>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public virtual MethodInfo[] GetMethods(BindingFlags bindingAttr)
    {
        MemberInfo[] members = this.GetMembers(bindingAttr);
        if (members == null)
            return new MethodInfo[0];
        int length = 0;
        foreach (MemberInfo memberInfo in members)
        {
            if (memberInfo.MemberType == MemberTypes.Method)
                ++length;
        }

        MethodInfo[] methods = new MethodInfo[length];
        int num = 0;
        foreach (MemberInfo memberInfo in members)
        {
            if (memberInfo.MemberType == MemberTypes.Method)
                methods[num++] = (MethodInfo)memberInfo;
        }

        return methods;
    }

    /// <summary>Gets a reference to the parent object.</summary>
    /// <returns>A reference to the parent object.</returns>
    public ScriptObject GetParent() => this.parent;

    internal virtual void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
    {
        MemberInfo[] members = this.GetMembers(BindingFlags.Instance | BindingFlags.Public);
        if (members.Length != 0)
        {
            enums.Add((object)members.GetEnumerator());
            objects.Add((object)this);
        }

        this.GetParent()?.GetPropertyEnumerator(enums, objects);
    }

    /// <summary>Gets a property that has the specified name.</summary>
    /// <returns>A property that has the specified <paramref name="name" />, or null if the script object does not contain the property.</returns>
    /// <param name="name">The name of the property to get.</param>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public PropertyInfo GetProperty(string name, BindingFlags bindingAttr)
    {
        return this.GetProperty(name, bindingAttr, (Binder)JSBinder.ob, (Type)null, Type.EmptyTypes,
            (ParameterModifier[])null);
    }

    /// <summary>Gets a property that has the specified name by using the specified selection criteria.</summary>
    /// <returns>A property that has the specified <paramref name="name" />, or null if the script object does not contain the property.</returns>
    /// <param name="name">The name of the property to search for.</param>
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
        MemberInfo[] member = this.GetMember(name, bindingAttr);
        if (member.Length == 1)
            return member[0] as PropertyInfo;
        int length = 0;
        foreach (MemberInfo memberInfo in member)
        {
            if (memberInfo.MemberType == MemberTypes.Property)
                ++length;
        }

        if (length == 0)
            return (PropertyInfo)null;
        PropertyInfo[] match = new PropertyInfo[length];
        int num = 0;
        foreach (MemberInfo memberInfo in member)
        {
            if (memberInfo.MemberType == MemberTypes.Property)
                match[num++] = (PropertyInfo)memberInfo;
        }

        if (binder == null)
            binder = (Binder)JSBinder.ob;
        return binder.SelectProperty(bindingAttr, match, returnType, types, modifiers);
    }

    /// <summary>Gets an array of <see cref="T:System.Reflection.PropertyInfo" /> objects that correspond to all properties of the current script object.</summary>
    /// <returns>An array of objects that contains all properties of the current script object.</returns>
    /// <param name="bindingAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    public virtual PropertyInfo[] GetProperties(BindingFlags bindingAttr)
    {
        MemberInfo[] members = this.GetMembers(bindingAttr);
        if (members == null)
            return new PropertyInfo[0];
        int length = 0;
        foreach (MemberInfo memberInfo in members)
        {
            if (memberInfo.MemberType == MemberTypes.Property)
                ++length;
        }

        PropertyInfo[] properties = new PropertyInfo[length];
        int num = 0;
        foreach (MemberInfo memberInfo in members)
        {
            if (memberInfo.MemberType == MemberTypes.Property)
                properties[num++] = (PropertyInfo)memberInfo;
        }

        return properties;
    }

    internal virtual object GetValueAtIndex(uint index)
    {
        return this.GetMemberValue(index.ToString((IFormatProvider)CultureInfo.CurrentUICulture));
    }

    /// <summary>Invokes the specified member.</summary>
    /// <returns>The return value of the call to the member.</returns>
    /// <param name="name">The name of the member to invoke.</param>
    /// <param name="invokeAttr">A bitwise combination of the enumeration values that specifies how the search for members is conducted by reflection.</param>
    /// <param name="binder">An object that performs type conversion from actual argument type to formal argument type.</param>
    /// <param name="target">The object on which to invoke the member. This must be the current script object.</param>
    /// <param name="args">An array of objects that contains the number, order, and type of the parameters of the member to be invoked. This is an empty array if there are no parameters.</param>
    /// <param name="modifiers">An array that has the same length as the args parameter. It represents the invoked member's argument attributes in the metadata.</param>
    /// <param name="locale">Provides information about a specific culture or locale. Used to format numbers, dates, and strings correctly.</param>
    /// <param name="namedParameters">An array of parameters.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public virtual object InvokeMember(
        string name,
        BindingFlags invokeAttr,
        Binder binder,
        object target,
        object[] args,
        ParameterModifier[] modifiers,
        CultureInfo locale,
        string[] namedParameters)
    {
        if (target != this)
            throw new TargetException();
        bool flag1 = name.StartsWith("< JScript-", StringComparison.Ordinal);
        bool flag2 =
            ((name == null || name == string.Empty ? 1 : (name.Equals("[DISPID=0]") ? 1 : 0)) | (flag1 ? 1 : 0)) != 0;
        if ((invokeAttr & BindingFlags.CreateInstance) != BindingFlags.Default)
        {
            if ((invokeAttr & (BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.SetField |
                               BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty)) !=
                BindingFlags.Default)
                throw new ArgumentException(JScriptException.Localize("Bad binding flags", locale));
            if (flag2)
                throw new MissingMethodException();
            return new LateBinding(name, (object)this).Call(binder, args, modifiers, locale, namedParameters, true,
                false, this.engine);
        }

        if (name == null)
            throw new ArgumentException(JScriptException.Localize("Bad name", locale));
        if ((invokeAttr & (BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.GetProperty)) !=
            BindingFlags.Default)
        {
            if ((invokeAttr & (BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.PutDispProperty)) !=
                BindingFlags.Default)
                throw new ArgumentException(JScriptException.Localize("Bad binding flags", locale));
            if (flag2)
            {
                if ((invokeAttr & (BindingFlags.GetField | BindingFlags.GetProperty)) == BindingFlags.Default)
                    throw new MissingMethodException();
                if (args == null || args.Length == 0)
                {
                    switch (this)
                    {
                        case JSObject _:
                        case GlobalScope _:
                        case ClassScope _:
                            PreferredType preferred_type = PreferredType.Either;
                            if (flag1)
                            {
                                if (name.StartsWith("< JScript-Number", StringComparison.Ordinal))
                                    preferred_type = PreferredType.Number;
                                else if (name.StartsWith("< JScript-String", StringComparison.Ordinal))
                                    preferred_type = PreferredType.String;
                                else if (name.StartsWith("< JScript-LocaleString", StringComparison.Ordinal))
                                    preferred_type = PreferredType.LocaleString;
                            }

                            return this.GetDefaultValue(preferred_type);
                        default:
                            throw new MissingFieldException();
                    }
                }
                else
                {
                    object ob = args.Length <= 1
                        ? args[0]
                        : throw new ArgumentException(JScriptException.Localize("Too many arguments", locale));
                    if (ob is int index)
                        return this[index];
                    IConvertible iconvertible = Convert.GetIConvertible(ob);
                    if (iconvertible != null && Convert.IsPrimitiveNumericTypeCode(iconvertible.GetTypeCode()))
                    {
                        double num = iconvertible.ToDouble((IFormatProvider)null);
                        if (num >= 0.0 && num <= (double)int.MaxValue && num == Math.Round(num))
                            return this[(int)num];
                    }

                    return this[Convert.ToString(ob)];
                }
            }
            else
            {
                if ((args == null || args.Length == 0) &&
                    (invokeAttr & (BindingFlags.GetField | BindingFlags.GetProperty)) != BindingFlags.Default)
                {
                    object memberValue = this.GetMemberValue(name);
                    if (memberValue != Missing.Value)
                        return memberValue;
                    if ((invokeAttr & BindingFlags.InvokeMethod) == BindingFlags.Default)
                        throw new MissingFieldException();
                }

                return new LateBinding(name, (object)this).Call(binder, args, modifiers, locale, namedParameters, false,
                    false, this.engine);
            }
        }
        else
        {
            if ((invokeAttr & (BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.PutDispProperty)) ==
                BindingFlags.Default)
                throw new ArgumentException(JScriptException.Localize("Bad binding flags", locale));
            if (flag2)
            {
                if (args == null || args.Length < 2)
                    throw new ArgumentException(JScriptException.Localize("Too few arguments", locale));
                object ob = args.Length <= 2
                    ? args[0]
                    : throw new ArgumentException(JScriptException.Localize("Too many arguments", locale));
                if (ob is int index)
                {
                    this[index] = args[1];
                    return (object)null;
                }

                IConvertible iconvertible = Convert.GetIConvertible(ob);
                if (iconvertible != null && Convert.IsPrimitiveNumericTypeCode(iconvertible.GetTypeCode()))
                {
                    double num = iconvertible.ToDouble((IFormatProvider)null);
                    if (num >= 0.0 && num <= (double)int.MaxValue && num == Math.Round(num))
                    {
                        this[(int)num] = args[1];
                        return (object)null;
                    }
                }

                this[Convert.ToString(ob)] = args[1];
                return (object)null;
            }

            if (args == null || args.Length < 1)
                throw new ArgumentException(JScriptException.Localize("Too few arguments", locale));
            if (args.Length > 1)
                throw new ArgumentException(JScriptException.Localize("Too many arguments", locale));
            this.SetMemberValue(name, args[0]);
            return (object)null;
        }
    }

    internal virtual void SetMemberValue(string name, object value)
    {
        MemberInfo[] member = this.GetMember(name, BindingFlags.Instance | BindingFlags.Public);
        LateBinding.SetMemberValue((object)this, name, value, LateBinding.SelectMember(member), member);
    }

    internal void SetParent(ScriptObject parent)
    {
        this.parent = parent;
        if (parent == null)
            return;
        this.engine = parent.engine;
    }

    internal virtual void SetValueAtIndex(uint index, object value)
    {
        this.SetMemberValue(index.ToString((IFormatProvider)CultureInfo.InvariantCulture), value);
    }

    /// <summary>Gets or sets a member of the script object that is designated by the specified <see cref="T:System.Double" /> index value.</summary>
    /// <returns>The value of the member that is at <paramref name="index" />.</returns>
    /// <param name="index">The index of the member.</param>
    public object this[double index]
    {
        get
        {
            if (this == null)
                throw new JScriptException(JSError.ObjectExpected);
            object obj = index < 0.0 || index > (double)uint.MaxValue || index != Math.Round(index)
                ? this.GetMemberValue(Convert.ToString(index))
                : this.GetValueAtIndex((uint)index);
            return !(obj is Missing) ? obj : (object)null;
        }
        set
        {
            if (index >= 0.0 && index <= (double)uint.MaxValue && index == Math.Round(index))
                this.SetValueAtIndex((uint)index, value);
            else
                this.SetMemberValue(Convert.ToString(index), value);
        }
    }

    /// <summary>Gets or sets a member of the script object that is designated by the specified <see cref="T:System.Int32" /> index value.</summary>
    /// <returns>The value of the member that is at <paramref name="index" />.</returns>
    /// <param name="index">The index of the member.</param>
    public object this[int index]
    {
        get
        {
            if (this == null)
                throw new JScriptException(JSError.ObjectExpected);
            object obj = index < 0
                ? this.GetMemberValue(Convert.ToString((double)index))
                : this.GetValueAtIndex((uint)index);
            return !(obj is Missing) ? obj : (object)null;
        }
        set
        {
            if (this == null)
                throw new JScriptException(JSError.ObjectExpected);
            if (index >= 0)
                this.SetValueAtIndex((uint)index, value);
            else
                this.SetMemberValue(Convert.ToString((double)index), value);
        }
    }

    /// <summary>Gets or sets a member of the script object that is designated by the specified name.</summary>
    /// <returns>The value of the member that is named <paramref name="name" />.</returns>
    /// <param name="name">The name of the member.</param>
    public object this[string name]
    {
        get
        {
            if (this == null)
                throw new JScriptException(JSError.ObjectExpected);
            object memberValue = this.GetMemberValue(name);
            return !(memberValue is Missing) ? memberValue : (object)null;
        }
        set
        {
            if (this == null)
                throw new JScriptException(JSError.ObjectExpected);
            this.SetMemberValue(name, value);
        }
    }

    /// <summary>Gets or sets a member of the script object that is designated by the specified name, <see cref="T:System.Double" /> index value, or <see cref="T:System.Int32" /> index value.</summary>
    /// <returns>The value of the member that is specified in <paramref name="pars" />.</returns>
    /// <param name="pars">The name or index of the member.</param>
    public object this[params object[] pars]
    {
        get
        {
            int length = pars.Length;
            if (length == 0)
            {
                switch (this)
                {
                    case ScriptFunction _:
                    case null:
                        throw new JScriptException(JSError.FunctionExpected);
                    default:
                        throw new JScriptException(JSError.TooFewParameters);
                }
            }
            else
            {
                if (this == null)
                    throw new JScriptException(JSError.ObjectExpected);
                object par = pars[length - 1];
                if (par is int index)
                    return this[index];
                IConvertible iconvertible = Convert.GetIConvertible(par);
                if (iconvertible != null && Convert.IsPrimitiveNumericTypeCode(iconvertible.GetTypeCode()))
                {
                    double num = iconvertible.ToDouble((IFormatProvider)null);
                    if (num >= 0.0 && num <= (double)int.MaxValue && num == Math.Round(num))
                        return this[(int)num];
                }

                return this[Convert.ToString(par)];
            }
        }
        set
        {
            int length = pars.Length;
            if (length == 0)
            {
                if (this == null)
                    throw new JScriptException(JSError.FunctionExpected);
                if (this is ScriptFunction)
                    throw new JScriptException(JSError.CannotAssignToFunctionResult);
                throw new JScriptException(JSError.TooFewParameters);
            }

            if (this == null)
                throw new JScriptException(JSError.ObjectExpected);
            object par = pars[length - 1];
            if (par is int index)
            {
                this[index] = value;
            }
            else
            {
                IConvertible iconvertible = Convert.GetIConvertible(par);
                if (iconvertible != null && Convert.IsPrimitiveNumericTypeCode(iconvertible.GetTypeCode()))
                {
                    double num = iconvertible.ToDouble((IFormatProvider)null);
                    if (num >= 0.0 && num <= (double)int.MaxValue && num == Math.Round(num))
                    {
                        this[(int)num] = value;
                        return;
                    }
                }

                this[Convert.ToString(par)] = value;
            }
        }
    }

    /// <summary>Gets the type of the script object.</summary>
    /// <returns>The type of the script object.</returns>
    public virtual Type UnderlyingSystemType => this.GetType();

    /// <summary>Creates an array of wrapped objects that contain the specified members together with the specified object.</summary>
    /// <returns>An array of wrapped objects, each of which contains member information and a copy of <paramref name="obj" />.</returns>
    /// <param name="members">The members to wrap.</param>
    /// <param name="obj">The object that <paramref name="members" /> are defined on.</param>
    protected static MemberInfo[] WrapMembers(MemberInfo[] members, object obj)
    {
        if (members == null)
            return (MemberInfo[])null;
        int length = members.Length;
        if (length == 0)
            return members;
        MemberInfo[] memberInfoArray = new MemberInfo[length];
        for (int index = 0; index < length; ++index)
            memberInfoArray[index] = ScriptObject.WrapMember(members[index], obj);
        return memberInfoArray;
    }

    /// <summary>Creates a wrapped object that contains the specified member together with the specified object.</summary>
    /// <returns>A one-element array that has a wrapped object that contains the member information and <paramref name="obj" />.</returns>
    /// <param name="member">The member to wrap.</param>
    /// <param name="obj">The object that <paramref name="member" /> is defined on.</param>
    protected static MemberInfo[] WrapMembers(MemberInfo member, object obj)
    {
        return new MemberInfo[1]
        {
            ScriptObject.WrapMember(member, obj)
        };
    }

    /// <summary>Creates an array of wrapped objects that contain the specified members together with the specified object, caching the results in the specified data storage.</summary>
    /// <returns>An array of wrapped objects, each of which contains member information and a copy of <paramref name="obj" />.</returns>
    /// <param name="members">The members to wrap.</param>
    /// <param name="obj">The object that <paramref name="members" /> are defined on.</param>
    /// <param name="cache">Temporary data storage for the wrapped objects.</param>
    protected static MemberInfo[] WrapMembers(
        MemberInfo[] members,
        object obj,
        SimpleHashtable cache)
    {
        if (members == null)
            return (MemberInfo[])null;
        int length = members.Length;
        if (length == 0)
            return members;
        MemberInfo[] memberInfoArray = new MemberInfo[length];
        for (int index = 0; index < length; ++index)
        {
            MemberInfo memberInfo = (MemberInfo)cache[(object)members[index]];
            if ((MemberInfo)null == memberInfo)
            {
                memberInfo = ScriptObject.WrapMember(members[index], obj);
                cache[(object)members[index]] = (object)memberInfo;
            }

            memberInfoArray[index] = memberInfo;
        }

        return memberInfoArray;
    }

    internal static MemberInfo WrapMember(MemberInfo member, object obj)
    {
        switch (member.MemberType)
        {
            case MemberTypes.Field:
                FieldInfo field = (FieldInfo)member;
                if (field.IsStatic || field.IsLiteral)
                    return (MemberInfo)field;
                return !(field is JSWrappedField) ? (MemberInfo)new JSWrappedField(field, obj) : (MemberInfo)field;
            case MemberTypes.Method:
                MethodInfo method = (MethodInfo)member;
                if (method.IsStatic)
                    return (MemberInfo)method;
                return !(method is JSWrappedMethod) ? (MemberInfo)new JSWrappedMethod(method, obj) : (MemberInfo)method;
            case MemberTypes.Property:
                PropertyInfo propertyInfo = (PropertyInfo)member;
                if (propertyInfo is JSWrappedProperty)
                    return (MemberInfo)propertyInfo;
                MethodInfo getMethod = JSProperty.GetGetMethod(propertyInfo, true);
                MethodInfo setMethod = JSProperty.GetSetMethod(propertyInfo, true);
                return (getMethod == (MethodInfo)null || getMethod.IsStatic) &&
                       (setMethod == (MethodInfo)null || setMethod.IsStatic)
                    ? (MemberInfo)propertyInfo
                    : (MemberInfo)new JSWrappedProperty(propertyInfo, obj);
            default:
                return member;
        }
    }
}