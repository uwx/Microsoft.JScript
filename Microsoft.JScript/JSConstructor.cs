// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Wraps a <see cref="T:Microsoft.JScript.FunctionObject" />. </summary>
public sealed class JSConstructor : ConstructorInfo
{
    internal FunctionObject cons;

    internal JSConstructor(FunctionObject cons) => this.cons = cons;

    /// <summary>Gets the attributes that are associated with this constructor.</summary>
    /// <returns>A bitwise combination of the enumeration values.</returns>
    public override MethodAttributes Attributes => this.cons.attributes;

    internal object Construct(object thisob, object[] args)
    {
        return LateBinding.CallValue((object)this.cons, args, true, false, this.cons.engine, thisob,
            (Binder)JSBinder.ob, (CultureInfo)null, (string[])null);
    }

    /// <summary>Gets the name of this constructor.</summary>
    /// <returns>The name of this constructor.</returns>
    public override string Name => this.cons.name;

    /// <summary>Gets the type that declares this constructor.</summary>
    /// <returns>The Type object for the class that declares this member.</returns>
    public override Type DeclaringType => Convert.ToType((IReflect)this.cons.enclosing_scope);

    internal string GetClassFullName() => ((ClassScope)this.cons.enclosing_scope).GetFullName();

    internal ClassScope GetClassScope() => (ClassScope)this.cons.enclosing_scope;

    internal ConstructorInfo GetConstructorInfo(CompilerGlobals compilerGlobals)
    {
        return this.cons.GetConstructorInfo(compilerGlobals);
    }

    /// <summary>Returns an array that contains the custom attributes that are attached to this constructor, searching only for attributes of the specified type.</summary>
    /// <returns>An array of <see cref="T:System.Object" /> objects that has zero elements.</returns>
    /// <param name="t">The type of attribute to search for.</param>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public override object[] GetCustomAttributes(Type t, bool inherit) => new object[0];

    /// <summary>Returns an array that contains the custom attributes that are attached to this constructor.</summary>
    /// <returns>An array of custom attributes that are attached to this method, or, if there are none, an array that has zero elements.</returns>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public override object[] GetCustomAttributes(bool inherit)
    {
        if (this.cons != null)
        {
            CustomAttributeList customAttributes = this.cons.customAttributes;
            if (customAttributes != null)
                return (object[])customAttributes.Evaluate(false);
        }

        return new object[0];
    }

    /// <summary>Returns information about the implementation of this method.</summary>
    /// <returns>
    /// <see cref="F:System.Reflection.MethodImplAttributes.Managed" /> and <see cref="F:System.Reflection.MethodImplAttributes.IL" />.</returns>
    public override MethodImplAttributes GetMethodImplementationFlags() => MethodImplAttributes.IL;

    internal PackageScope GetPackage() => ((ClassScope)this.cons.enclosing_scope).GetPackage();

    /// <summary>Gets the parameters of this constructor.</summary>
    /// <returns>An array that contains information that matches the signature of this constructor.</returns>
    public override ParameterInfo[] GetParameters()
    {
        return (ParameterInfo[])this.cons.parameter_declarations;
    }

    /// <summary>Invokes the constructor, using the specified culture and binding information.</summary>
    /// <returns>An instance of the class associated with the constructor.</returns>
    /// <param name="options">A bitwise combination of the enumeration values that control the binding.</param>
    /// <param name="binder">An object that converts the type from actual argument type to formal argument type.</param>
    /// <param name="parameters">The argument list to pass to the constructor.</param>
    /// <param name="culture">Provides information about a specific culture or locale. Used to format numbers, dates, and strings correctly.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public override object Invoke(
        BindingFlags options,
        Binder binder,
        object[] parameters,
        CultureInfo culture)
    {
        return LateBinding.CallValue((object)this.cons, parameters, true, false, this.cons.engine, (object)null, binder,
            culture, (string[])null);
    }

    /// <summary>Invokes the constructor on the specified object, using the specified culture and binding information.</summary>
    /// <returns>An instance of the class associated with the constructor.</returns>
    /// <param name="obj">The object to call the constructor on.</param>
    /// <param name="options">A bitwise combination of the enumeration values that control the binding.</param>
    /// <param name="binder">An object that converts the type from actual argument type to formal argument type.</param>
    /// <param name="parameters">The argument list to pass to the constructor.</param>
    /// <param name="culture">Provides information about a specific culture or locale. Used to format numbers, dates, and strings correctly.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public override object Invoke(
        object obj,
        BindingFlags options,
        Binder binder,
        object[] parameters,
        CultureInfo culture)
    {
        return this.cons.Call(parameters, obj, binder, culture);
    }

    internal bool IsAccessibleFrom(ScriptObject scope)
    {
        while (true)
        {
            switch (scope)
            {
                case null:
                case ClassScope _:
                    goto label_3;
                default:
                    scope = scope.GetParent();
                    continue;
            }
        }

        label_3:
        ClassScope enclosingScope = (ClassScope)this.cons.enclosing_scope;
        if (this.IsPrivate)
        {
            if (scope == null)
                return false;
            return scope == enclosingScope || ((ClassScope)scope).IsNestedIn(enclosingScope, false);
        }

        if (this.IsFamily)
        {
            if (scope == null)
                return false;
            return ((ClassScope)scope).IsSameOrDerivedFrom(enclosingScope) ||
                   ((ClassScope)scope).IsNestedIn(enclosingScope, false);
        }

        if (this.IsFamilyOrAssembly && scope != null && (((ClassScope)scope).IsSameOrDerivedFrom(enclosingScope) ||
                                                         ((ClassScope)scope).IsNestedIn(enclosingScope, false)))
            return true;
        return scope == null
            ? enclosingScope.GetPackage() == null
            : enclosingScope.GetPackage() == ((ClassScope)scope).GetPackage();
    }

    /// <summary>Determines whether the specified attribute type is attached to this constructor.</summary>
    /// <returns>false in all cases.</returns>
    /// <param name="type">The attribute type to search for.</param>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public override bool IsDefined(Type type, bool inherit) => false;

    /// <summary>Gets the type of member that this method is. Identifies the type of class derived from <see cref="T:System.Reflection.MemberInfo" /> that this class derives from.</summary>
    /// <returns>
    /// <see cref="F:System.Reflection.MemberTypes.Constructor" />, because this class derives from <see cref="T:System.Reflection.ConstructorInfo" />.</returns>
    public override MemberTypes MemberType => MemberTypes.Constructor;

    /// <summary>Gets a handle to the internal metadata representation of this constructor.</summary>
    /// <returns>A handle to the internal metadata representation of this constructor.</returns>
    public override RuntimeMethodHandle MethodHandle
    {
        get => this.GetConstructorInfo((CompilerGlobals)null).MethodHandle;
    }

    internal Type OuterClassType()
    {
        FieldInfo outerClassField = ((ClassScope)this.cons.enclosing_scope).outerClassField;
        return !(outerClassField == (FieldInfo)null) ? outerClassField.FieldType : (Type)null;
    }

    /// <summary>Gets the class object that was used to obtain this instance through reflection.</summary>
    /// <returns>The Type object for the class that declares this member.</returns>
    public override Type ReflectedType => this.DeclaringType;
}