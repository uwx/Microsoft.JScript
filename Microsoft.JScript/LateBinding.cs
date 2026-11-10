// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.LateBinding
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
using System.Runtime.InteropServices.Expando;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a reference to a class member, callable expression, or variable name. This class attempts to resolve the reference to the correct value at run time. </summary>
public sealed class LateBinding
{
    private object last_ir;
    internal MemberInfo last_member;
    internal MemberInfo[] last_members;
    internal object last_object;
    private string name;

    /// <summary>Stores a reference to the object that the binding reference is defined on.</summary>
    public object obj;

    private bool checkForDebugger;

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.LateBinding" /> class, using the specified identifier.</summary>
    /// <param name="name">A string that identifies the binding reference.</param>
    public LateBinding(string name)
        : this(name, (object)null, false)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.LateBinding" /> class, using the specified identifier and object.</summary>
    /// <param name="name">A string that identifies the binding reference.</param>
    /// <param name="obj">The object that the binding reference is defined on.</param>
    public LateBinding(string name, object obj)
        : this(name, obj, false)
    {
    }

    internal LateBinding(string name, object obj, bool checkForDebugger)
    {
        this.last_ir = (object)null;
        this.last_member = (MemberInfo)null;
        this.last_members = (MemberInfo[])null;
        this.last_object = (object)null;
        this.name = name;
        this.obj = obj;
        this.checkForDebugger = checkForDebugger;
    }

    internal MemberInfo BindToMember()
    {
        if (this.obj == this.last_object && this.last_member != (MemberInfo)null)
            return this.last_member;
        BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public;
        object obj = this.obj;
        TypeReflector typeReflectorFor = TypeReflector.GetTypeReflectorFor(obj.GetType());
        IReflect reflect;
        if (typeReflectorFor.Is__ComObject())
        {
            if (!this.checkForDebugger)
                return (MemberInfo)null;
            if (!(obj is IDebuggerObject debuggerObject))
                return (MemberInfo)null;
            if (debuggerObject.IsCOMObject())
                return (MemberInfo)null;
            reflect = (IReflect)obj;
        }
        else if (typeReflectorFor.ImplementsIReflect())
        {
            reflect = (IReflect)(obj as ScriptObject);
            if (reflect != null)
            {
                if (obj is ClassScope)
                    bindingAttr = BindingFlags.Static | BindingFlags.Public;
            }
            else
            {
                reflect = (IReflect)(obj as Type);
                if (reflect != null)
                    bindingAttr = BindingFlags.Static | BindingFlags.Public;
                else
                    reflect = (IReflect)obj;
            }
        }
        else
            reflect = (IReflect)typeReflectorFor;

        this.last_object = this.obj;
        this.last_ir = (object)reflect;
        MemberInfo[] memberInfoArray = this.last_members = reflect.GetMember(this.name, bindingAttr);
        this.last_member = LateBinding.SelectMember(memberInfoArray);
        if ((object)(this.obj as Type) != null)
        {
            MemberInfo[] member = typeof(Type).GetMember(this.name, BindingFlags.Instance | BindingFlags.Public);
            int length1;
            if (member != null && (length1 = member.Length) > 0)
            {
                int length2;
                if (memberInfoArray == null || (length2 = memberInfoArray.Length) == 0)
                {
                    this.last_member = LateBinding.SelectMember(this.last_members = member);
                }
                else
                {
                    MemberInfo[] target = new MemberInfo[length1 + length2];
                    ArrayObject.Copy((object[])memberInfoArray, 0, (object[])target, 0, length2);
                    ArrayObject.Copy((object[])member, 0, (object[])target, length2, length1);
                    this.last_member = LateBinding.SelectMember(this.last_members = target);
                }
            }
        }

        return this.last_member;
    }

    /// <summary>Resolves the binding reference by calling a member.</summary>
    /// <returns>The return value of the call to the member.</returns>
    /// <param name="arguments">The arguments to pass to the member.</param>
    /// <param name="construct">true to use the construct method; otherwise, false.</param>
    /// <param name="brackets">true if the binding reference contains brackets; otherwise, false.</param>
    /// <param name="engine">A reference to the scripting engine.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public object Call(object[] arguments, bool construct, bool brackets, VsaEngine engine)
    {
        try
        {
            return this.name == null
                ? LateBinding.CallValue(this.obj, arguments, construct, brackets, engine,
                    ((IActivationObject)engine.ScriptObjectStackTop()).GetDefaultThisObject(), (Binder)JSBinder.ob,
                    (CultureInfo)null, (string[])null)
                : this.Call((Binder)JSBinder.ob, arguments, (ParameterModifier[])null, (CultureInfo)null,
                    (string[])null, construct, brackets, engine);
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal object Call(
        Binder binder,
        object[] arguments,
        ParameterModifier[] modifiers,
        CultureInfo culture,
        string[] namedParameters,
        bool construct,
        bool brackets,
        VsaEngine engine)
    {
        MemberInfo member = this.BindToMember();
        if (this.obj is ScriptObject || this.obj is GlobalObject)
        {
            if (this.obj is WithObject)
            {
                object containedObject = ((WithObject)this.obj).contained_object;
                if (!(containedObject is ScriptObject))
                {
                    IReflect requiresInvokeMember =
                        LateBinding.GetIRForObjectThatRequiresInvokeMember(containedObject, VsaEngine.executeForJSEE);
                    if (requiresInvokeMember != null)
                        return LateBinding.CallCOMObject(requiresInvokeMember, this.name, containedObject, binder,
                            arguments, modifiers, culture, namedParameters, construct, brackets, engine);
                }
            }

            if ((object)(member as FieldInfo) != null)
                return LateBinding.CallValue(((FieldInfo)member).GetValue(this.obj), arguments, construct, brackets,
                    engine, this.obj, (Binder)JSBinder.ob, (CultureInfo)null, (string[])null);
            if ((object)(member as PropertyInfo) != null && !(member is JSProperty))
            {
                if (!brackets)
                {
                    JSWrappedPropertyAndMethod propertyAndMethod = member as JSWrappedPropertyAndMethod;
                    if ((PropertyInfo)propertyAndMethod != (PropertyInfo)null)
                    {
                        BindingFlags options = arguments == null || arguments.Length == 0
                            ? BindingFlags.InvokeMethod
                            : BindingFlags.InvokeMethod | BindingFlags.GetProperty;
                        return propertyAndMethod.Invoke(this.obj, options, (Binder)JSBinder.ob, arguments,
                            (CultureInfo)null);
                    }
                }

                return LateBinding.CallValue(JSProperty.GetValue((PropertyInfo)member, this.obj, (object[])null),
                    arguments, construct, brackets, engine, this.obj, (Binder)JSBinder.ob, (CultureInfo)null,
                    (string[])null);
            }

            if ((object)(member as MethodInfo) != null)
            {
                if (member is JSMethod)
                    return construct
                        ? ((JSMethod)member).Construct(arguments)
                        : ((JSMethod)member).Invoke(this.obj, this.obj, BindingFlags.Default, (Binder)JSBinder.ob,
                            arguments, (CultureInfo)null);
                Type declaringType = member.DeclaringType;
                if (declaringType == typeof(object))
                    return LateBinding.CallMethod((MethodInfo)member, arguments, this.obj, binder, culture,
                        namedParameters);
                if (declaringType == typeof(string))
                    return LateBinding.CallMethod((MethodInfo)member, arguments, (object)Convert.ToString(this.obj),
                        binder, culture, namedParameters);
                if (Convert.IsPrimitiveNumericType((IReflect)declaringType))
                    return LateBinding.CallMethod((MethodInfo)member, arguments,
                        Convert.CoerceT(this.obj, declaringType), binder, culture, namedParameters);
                if (declaringType == typeof(bool))
                    return LateBinding.CallMethod((MethodInfo)member, arguments, (object)Convert.ToBoolean(this.obj),
                        binder, culture, namedParameters);
                if (((declaringType == typeof(StringObject) || declaringType == typeof(BooleanObject)
                        ? 1
                        : (declaringType == typeof(NumberObject) ? 1 : 0)) | (brackets ? 1 : 0)) != 0)
                    return LateBinding.CallMethod((MethodInfo)member, arguments, Convert.ToObject(this.obj, engine),
                        binder, culture, namedParameters);
                if (declaringType == typeof(GlobalObject) && ((MethodBase)member).IsSpecialName)
                    return LateBinding.CallValue(((MethodBase)member).Invoke(this.obj, (object[])null), arguments,
                        construct, false, engine, this.obj, (Binder)JSBinder.ob, (CultureInfo)null, (string[])null);
                if (!(this.obj is ClassScope))
                {
                    if (CustomAttribute.IsDefined(member, typeof(JSFunctionAttribute), false))
                    {
                        FieldInfo fieldInfo = LateBinding.SelectMember(this.last_members) as FieldInfo;
                        if (fieldInfo != (FieldInfo)null)
                        {
                            object val = this.obj;
                            if (!(val is Closure))
                                val = fieldInfo.GetValue(this.obj);
                            return LateBinding.CallValue(val, arguments, construct, brackets, engine, this.obj,
                                (Binder)JSBinder.ob, (CultureInfo)null, (string[])null);
                        }
                    }

                    return LateBinding.CallValue((object)new BuiltinFunction(this.obj, (MethodInfo)member), arguments,
                        construct, false, engine, this.obj, (Binder)JSBinder.ob, (CultureInfo)null, (string[])null);
                }
            }
        }

        MethodInfo method = member as MethodInfo;
        if (method != (MethodInfo)null)
            return LateBinding.CallMethod(method, arguments, this.obj, binder, culture, namedParameters);
        JSConstructor jsConstructor = member as JSConstructor;
        if ((ConstructorInfo)jsConstructor != (ConstructorInfo)null)
            return LateBinding.CallValue((object)jsConstructor.cons, arguments, construct, brackets, engine, this.obj,
                (Binder)JSBinder.ob, (CultureInfo)null, (string[])null);
        if ((object)(member as Type) != null)
            return LateBinding.CallValue((object)member, arguments, construct, brackets, engine, this.obj,
                (Binder)JSBinder.ob, (CultureInfo)null, (string[])null);
        if ((object)(member as ConstructorInfo) != null)
            return LateBinding.CallOneOfTheMembers(new MemberInfo[1]
            {
                this.last_member
            }, arguments, true, this.obj, binder, culture, namedParameters, engine);
        if (!construct && (object)(member as PropertyInfo) != null)
        {
            if (member is COMPropertyInfo)
                return ((PropertyInfo)member).GetValue(this.obj,
                    BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                    BindingFlags.OptionalParamBinding, binder, arguments, culture);
            if (((PropertyInfo)member).GetIndexParameters().Length == 0)
            {
                Type propertyType = ((PropertyInfo)member).PropertyType;
                if (propertyType == typeof(object))
                {
                    MethodInfo getMethod = JSProperty.GetGetMethod((PropertyInfo)member, false);
                    if (getMethod != (MethodInfo)null)
                        return LateBinding.CallValue(getMethod.Invoke(this.obj, (object[])null), arguments, construct,
                            brackets, engine, this.obj, (Binder)JSBinder.ob, (CultureInfo)null, (string[])null);
                }

                MemberInfo[] defaultMembers = TypeReflector.GetTypeReflectorFor(propertyType).GetDefaultMembers();
                if (defaultMembers != null && defaultMembers.Length != 0)
                {
                    MethodInfo getMethod = JSProperty.GetGetMethod((PropertyInfo)member, false);
                    if (getMethod != (MethodInfo)null)
                    {
                        object thisob = getMethod.Invoke(this.obj, (object[])null);
                        return LateBinding.CallOneOfTheMembers(defaultMembers, arguments, false, thisob, binder,
                            culture, namedParameters, engine);
                    }
                }
            }
        }

        if (this.last_members != null && this.last_members.Length != 0)
        {
            bool memberCalled;
            object obj = LateBinding.CallOneOfTheMembers(this.last_members, arguments, construct, this.obj, binder,
                culture, namedParameters, engine, out memberCalled);
            if (memberCalled)
                return obj;
        }

        IReflect requiresInvokeMember1 =
            LateBinding.GetIRForObjectThatRequiresInvokeMember(this.obj, VsaEngine.executeForJSEE);
        if (requiresInvokeMember1 != null)
            return LateBinding.CallCOMObject(requiresInvokeMember1, this.name, this.obj, binder, arguments, modifiers,
                culture, namedParameters, construct, brackets, engine);
        object memberValue = LateBinding.GetMemberValue(this.obj, this.name, this.last_member, this.last_members);
        if (!(memberValue is Missing))
            return LateBinding.CallValue(memberValue, arguments, construct, brackets, engine, this.obj,
                (Binder)JSBinder.ob, (CultureInfo)null, (string[])null);
        if (!brackets)
            throw new JScriptException(JSError.FunctionExpected);
        if (this.obj is IActivationObject)
            throw new JScriptException(JSError.ObjectExpected);
        throw new JScriptException(JSError.OLENoPropOrMethod);
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    private static object CallCOMObject(
        IReflect ir,
        string name,
        object ob,
        Binder binder,
        object[] arguments,
        ParameterModifier[] modifiers,
        CultureInfo culture,
        string[] namedParameters,
        bool construct,
        bool brackets,
        VsaEngine engine)
    {
        try
        {
            try
            {
                LateBinding.Change64bitIntegersToDouble(arguments);
                BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                            BindingFlags.OptionalParamBinding;
                if (construct)
                    return ir.InvokeMember(name, bindingFlags | BindingFlags.CreateInstance, binder, ob, arguments,
                        modifiers, culture, namedParameters);
                if (brackets)
                {
                    try
                    {
                        return ir.InvokeMember(name, bindingFlags | BindingFlags.GetProperty | BindingFlags.GetField,
                            binder, ob, arguments, modifiers, culture, namedParameters);
                    }
                    catch (TargetInvocationException ex)
                    {
                        object obj = ir.InvokeMember(name,
                            bindingFlags | BindingFlags.GetProperty | BindingFlags.GetField, binder, ob, new object[0],
                            modifiers, culture, new string[0]);
                        return LateBinding.CallValue(obj, arguments, construct, brackets, engine, obj, binder, culture,
                            namedParameters);
                    }
                }
                else
                {
                    int length = arguments == null ? 0 : arguments.Length;
                    if (namedParameters != null && namedParameters.Length != 0 &&
                        (namedParameters[0].Equals("[DISPID=-613]") || namedParameters[0].Equals("this")))
                        --length;
                    BindingFlags invokeAttr = bindingFlags | (length > 0
                        ? BindingFlags.InvokeMethod | BindingFlags.GetProperty
                        : BindingFlags.InvokeMethod);
                    return ir.InvokeMember(name, invokeAttr, binder, ob, arguments, modifiers, culture,
                        namedParameters);
                }
            }
            catch (MissingMemberException ex)
            {
                if (brackets)
                    return (object)null;
                throw new JScriptException(JSError.FunctionExpected);
            }
            catch (COMException ex)
            {
                int errorCode = ex.ErrorCode;
                switch (errorCode)
                {
                    case -2147352573 /*0x80020003*/:
                    case -2147352570 /*0x80020006*/:
                        if (brackets)
                            return (object)null;
                        throw new JScriptException(JSError.FunctionExpected);
                    default:
                        if (((long)errorCode & 4294901760L) == 2148139008L /*0x800A0000*/)
                        {
                            string source = ex.Source;
                            if (source != null && source.IndexOf("JScript") != -1)
                                throw new JScriptException((Exception)ex, (Context)null);
                        }

                        throw ex;
                }
            }
        }
        catch (JScriptException ex)
        {
            if ((ex.Number & (int)ushort.MaxValue) == 5002)
            {
                MemberInfo[] member = typeof(object).GetMember(name, BindingFlags.Instance | BindingFlags.Public);
                if (member != null && member.Length != 0)
                    return LateBinding.CallOneOfTheMembers(member, arguments, construct, ob, binder, culture,
                        namedParameters, engine);
            }

            throw ex;
        }
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    private static object CallMethod(
        MethodInfo method,
        object[] arguments,
        object thisob,
        Binder binder,
        CultureInfo culture,
        string[] namedParameters)
    {
        if (namedParameters != null && namedParameters.Length != 0)
        {
            if (arguments.Length < namedParameters.Length)
                throw new JScriptException(JSError.MoreNamedParametersThanArguments);
            arguments = JSBinder.ArrangeNamedArguments((MethodBase)method, arguments, namedParameters);
        }

        object[] objArray = LateBinding.LickArgumentsIntoShape(method.GetParameters(), arguments, binder, culture);
        try
        {
            object obj = method.Invoke(thisob, BindingFlags.SuppressChangeType, (Binder)null, objArray,
                (CultureInfo)null);
            if (objArray != arguments && objArray != null && arguments != null)
            {
                int num = arguments.Length;
                int length = objArray.Length;
                if (length < num)
                    num = length;
                for (int index = 0; index < num; ++index)
                    arguments[index] = objArray[index];
            }

            return obj;
        }
        catch (TargetException ex)
        {
            if (thisob is ClassScope classScope)
                return classScope.FakeCallToTypeMethod(method, objArray, (Exception)ex);
            throw;
        }
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal static object CallOneOfTheMembers(
        MemberInfo[] members,
        object[] arguments,
        bool construct,
        object thisob,
        Binder binder,
        CultureInfo culture,
        string[] namedParameters,
        VsaEngine engine)
    {
        bool memberCalled;
        object obj = LateBinding.CallOneOfTheMembers(members, arguments, construct, thisob, binder, culture,
            namedParameters, engine, out memberCalled);
        if (!memberCalled)
            throw new MissingMemberException();
        return obj;
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal static object CallOneOfTheMembers(
        MemberInfo[] members,
        object[] arguments,
        bool construct,
        object thisob,
        Binder binder,
        CultureInfo culture,
        string[] namedParameters,
        VsaEngine engine,
        out bool memberCalled)
    {
        memberCalled = true;
        if (construct)
        {
            ConstructorInfo target1 = JSBinder.SelectConstructor(Microsoft.JScript.Runtime.TypeRefs, members,
                ref arguments, namedParameters);
            if (target1 != (ConstructorInfo)null)
            {
                if (CustomAttribute.IsDefined((MemberInfo)target1, typeof(JSFunctionAttribute), false))
                {
                    if (thisob is StackFrame)
                        thisob = ((StackFrame)thisob).closureInstance;
                    int length = arguments.Length;
                    object[] target2 = new object[length + 1];
                    ArrayObject.Copy(arguments, 0, target2, 0, length);
                    target2[length] = thisob;
                    arguments = target2;
                }

                JSConstructor jsConstructor = target1 as JSConstructor;
                object obj = !((ConstructorInfo)jsConstructor != (ConstructorInfo)null)
                    ? target1.Invoke(BindingFlags.SuppressChangeType, (Binder)null,
                        LateBinding.LickArgumentsIntoShape(target1.GetParameters(), arguments, (Binder)JSBinder.ob,
                            culture), (CultureInfo)null)
                    : jsConstructor.Construct(thisob,
                        LateBinding.LickArgumentsIntoShape(target1.GetParameters(), arguments, (Binder)JSBinder.ob,
                            culture));
                if (obj is INeedEngine)
                    ((INeedEngine)obj).SetEngine(engine);
                return obj;
            }
        }
        else
        {
            object[] objArray = arguments;
            MethodInfo methodInfo = JSBinder.SelectMethod(Microsoft.JScript.Runtime.TypeRefs, members, ref arguments,
                namedParameters);
            if (methodInfo != (MethodInfo)null)
            {
                if (methodInfo is JSMethod)
                    return ((JSMethod)methodInfo).Invoke(thisob, thisob, BindingFlags.Default, (Binder)JSBinder.ob,
                        arguments, (CultureInfo)null);
                if (CustomAttribute.IsDefined((MemberInfo)methodInfo, typeof(JSFunctionAttribute), false))
                {
                    if (!construct)
                    {
                        JSBuiltin builtinFunction =
                            ((JSFunctionAttribute)CustomAttribute.GetCustomAttributes((MemberInfo)methodInfo,
                                typeof(JSFunctionAttribute), false)[0]).builtinFunction;
                        if (builtinFunction != JSBuiltin.None)
                        {
                            if (thisob is IActivationObject activationObject)
                                thisob = activationObject.GetDefaultThisObject();
                            return BuiltinFunction.QuickCall(arguments, thisob, builtinFunction, (MethodInfo)null,
                                engine);
                        }
                    }

                    return LateBinding.CallValue((object)new BuiltinFunction(thisob, methodInfo), arguments, construct,
                        false, engine, thisob, (Binder)JSBinder.ob, (CultureInfo)null, (string[])null);
                }

                object[] parameters = LateBinding.LickArgumentsIntoShape(methodInfo.GetParameters(), arguments,
                    (Binder)JSBinder.ob, culture);
                if (thisob != null && !methodInfo.DeclaringType.IsAssignableFrom(thisob.GetType()))
                {
                    switch (thisob)
                    {
                        case StringObject _:
                            return methodInfo.Invoke((object)((StringObject)thisob).value,
                                BindingFlags.SuppressChangeType, (Binder)null, parameters, (CultureInfo)null);
                        case NumberObject _:
                            return methodInfo.Invoke(((NumberObject)thisob).value, BindingFlags.SuppressChangeType,
                                (Binder)null, parameters, (CultureInfo)null);
                        case BooleanObject _:
                            return methodInfo.Invoke((object)((BooleanObject)thisob).value,
                                BindingFlags.SuppressChangeType, (Binder)null, parameters, (CultureInfo)null);
                        case ArrayWrapper _:
                            return methodInfo.Invoke((object)((ArrayWrapper)thisob).value,
                                BindingFlags.SuppressChangeType, (Binder)null, parameters, (CultureInfo)null);
                    }
                }

                object obj = methodInfo.Invoke(thisob, BindingFlags.SuppressChangeType, (Binder)null, parameters,
                    (CultureInfo)null);
                if (parameters != objArray && arguments == objArray && parameters != null && arguments != null)
                {
                    int num = arguments.Length;
                    int length = parameters.Length;
                    if (length < num)
                        num = length;
                    for (int index = 0; index < num; ++index)
                        arguments[index] = parameters[index];
                }

                return obj;
            }
        }

        memberCalled = false;
        return (object)null;
    }

    /// <summary>Resolves the binding reference by calling a member.</summary>
    /// <returns>The return value of the call to the member.</returns>
    /// <param name="thisob">The object that the binding reference is defined on.</param>
    /// <param name="val">Information about the member to call.</param>
    /// <param name="arguments">The arguments to pass to the member.</param>
    /// <param name="construct">true to use the construct method; otherwise, false.</param>
    /// <param name="brackets">true if the binding reference contains brackets; otherwise, false.</param>
    /// <param name="engine">A reference to the scripting engine.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public static object CallValue(
        object thisob,
        object val,
        object[] arguments,
        bool construct,
        bool brackets,
        VsaEngine engine)
    {
        try
        {
            return LateBinding.CallValue(val, arguments, construct, brackets, engine, thisob, (Binder)JSBinder.ob,
                (CultureInfo)null, (string[])null);
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
    }

    /// <summary>Resolves the binding reference by calling a member.</summary>
    /// <returns>The return value of the call to the member.</returns>
    /// <param name="val">Information about the member to call.</param>
    /// <param name="thisob">The object that the binding reference is defined on.</param>
    /// <param name="arguments">The arguments to pass to the member.</param>
    /// <param name="construct">true to use the construct method; otherwise, false.</param>
    /// <param name="brackets">true if the binding reference contains brackets; otherwise, false.</param>
    /// <param name="engine">A reference to the scripting engine.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public static object CallValue2(
        object val,
        object thisob,
        object[] arguments,
        bool construct,
        bool brackets,
        VsaEngine engine)
    {
        try
        {
            return LateBinding.CallValue(val, arguments, construct, brackets, engine, thisob, (Binder)JSBinder.ob,
                (CultureInfo)null, (string[])null);
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal static object CallValue(
        object val,
        object[] arguments,
        bool construct,
        bool brackets,
        VsaEngine engine,
        object thisob,
        Binder binder,
        CultureInfo culture,
        string[] namedParameters)
    {
        if (construct)
        {
            if (val is ScriptFunction)
            {
                ScriptFunction scriptFunction = (ScriptFunction)val;
                if (brackets)
                {
                    object obj;
                    object val1 = obj = scriptFunction[arguments];
                    if (val1 != null)
                        return LateBinding.CallValue(val1, new object[0], true, false, engine, thisob, binder, culture,
                            namedParameters);
                    Type predefinedType = Microsoft.JScript.Runtime.TypeRefs.GetPredefinedType(scriptFunction.name);
                    if (predefinedType != (Type)null)
                    {
                        int[] numArray = new int[arguments.Length];
                        int num1 = 0;
                        foreach (object ob in arguments)
                        {
                            if (ob is int num4)
                            {
                                numArray[num1++] = num4;
                            }
                            else
                            {
                                IConvertible iconvertible = Convert.GetIConvertible(ob);
                                if (iconvertible != null &&
                                    Convert.IsPrimitiveNumericTypeCode(iconvertible.GetTypeCode()))
                                {
                                    double num2 = iconvertible.ToDouble((IFormatProvider)null);
                                    int num3 = (int)num2;
                                    if (num2 == (double)num3)
                                        numArray[num1++] = num3;
                                    else
                                        goto label_15;
                                }
                                else
                                    goto label_15;
                            }
                        }

                        return (object)Array.CreateInstance(predefinedType, numArray);
                    }
                }

                label_15:
                if (scriptFunction is FunctionObject functionObject)
                {
                    JSObject thisob1 = thisob as JSObject;
                    object[] args = arguments == null ? new object[0] : arguments;
                    return functionObject.Construct(thisob1, args);
                }

                object obj1 = scriptFunction.Construct(arguments == null ? new object[0] : arguments);
                if (obj1 is JSObject jsObject)
                    jsObject.outer_class_instance = thisob as JSObject;
                return obj1;
            }

            if (val is ClassScope)
            {
                if (brackets)
                    return (object)Array.CreateInstance(typeof(object), LateBinding.ToIndices(arguments));
                JSObject jsObject = (JSObject)LateBinding.CallOneOfTheMembers(
                    (MemberInfo[])((ClassScope)val).constructors, arguments, construct, thisob, binder, culture,
                    namedParameters, engine);
                jsObject.noExpando = ((ClassScope)val).noExpando;
                return (object)jsObject;
            }

            if ((object)(val as Type) != null)
            {
                Type type = (Type)val;
                if (type.IsInterface && type.IsImport)
                    type = JSBinder.HandleCoClassAttribute(type);
                if (brackets)
                    return (object)Array.CreateInstance(type, LateBinding.ToIndices(arguments));
                ConstructorInfo[] constructors = type.GetConstructors();
                object obj = constructors == null || constructors.Length == 0
                    ? Activator.CreateInstance(type, BindingFlags.Default, (Binder)JSBinder.ob, arguments,
                        (CultureInfo)null)
                    : LateBinding.CallOneOfTheMembers((MemberInfo[])constructors, arguments, construct, thisob, binder,
                        culture, namedParameters, engine);
                if (obj is INeedEngine)
                    ((INeedEngine)obj).SetEngine(engine);
                return obj;
            }

            if (val is TypedArray & brackets)
                return (object)Array.CreateInstance(typeof(object), LateBinding.ToIndices(arguments));
            if (VsaEngine.executeForJSEE && val is IDebuggerObject)
            {
                if (!(val is IReflect reflect))
                    throw new JScriptException(JSError.FunctionExpected);
                return reflect.InvokeMember(string.Empty,
                    BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.CreateInstance |
                    BindingFlags.OptionalParamBinding, binder, thisob, arguments, (ParameterModifier[])null, culture,
                    namedParameters);
            }
        }

        if (brackets)
        {
            if (val is ScriptObject scriptObject1)
            {
                object val2 = scriptObject1[arguments];
                return construct ? LateBinding.CallValue(thisob, val2, new object[0], true, false, engine) : val2;
            }
        }
        else
        {
            if (val is ScriptFunction)
            {
                if (thisob is IActivationObject)
                    thisob = ((IActivationObject)thisob).GetDefaultThisObject();
                return ((ScriptFunction)val).Call(arguments == null ? new object[0] : arguments, thisob, binder,
                    culture);
            }

            if ((object)(val as Delegate) != null)
                return LateBinding.CallMethod(((Delegate)val).Method, arguments, thisob, binder, culture,
                    namedParameters);
            if ((object)(val as MethodInfo) != null)
                return LateBinding.CallMethod((MethodInfo)val, arguments, thisob, binder, culture, namedParameters);
            if ((object)(val as Type) != null && arguments.Length == 1)
                return Convert.CoerceT(arguments[0], (Type)val, true);
            if (VsaEngine.executeForJSEE && val is IDebuggerObject)
            {
                if (!(val is IReflect ir))
                    throw new JScriptException(JSError.FunctionExpected);
                object[] objArray = new object[(arguments != null ? arguments.Length : 0) + 1];
                objArray[0] = thisob;
                if (arguments != null)
                    ArrayObject.Copy(arguments, 0, objArray, 1, arguments.Length);
                string[] strArray = new string[(namedParameters != null ? namedParameters.Length : 0) + 1];
                strArray[0] = "this";
                if (namedParameters != null)
                    ArrayObject.Copy((object[])namedParameters, 0, (object[])strArray, 1, namedParameters.Length);
                return LateBinding.CallCOMObject(ir, string.Empty, val, binder, objArray, (ParameterModifier[])null,
                    culture, strArray, false, false, engine);
            }

            if (val is ClassScope)
            {
                if (arguments == null || arguments.Length != 1)
                    throw new JScriptException(JSError.FunctionExpected);
                return ((ClassScope)val).HasInstance(arguments[0])
                    ? arguments[0]
                    : throw new InvalidCastException((string)null);
            }

            if (val is TypedArray && arguments.Length == 1)
                return Convert.Coerce(arguments[0], val, true);
            if (val is ScriptObject)
                throw new JScriptException(JSError.FunctionExpected);
            if (val is MemberInfo[])
                return LateBinding.CallOneOfTheMembers((MemberInfo[])val, arguments, construct, thisob, binder, culture,
                    namedParameters, engine);
        }

        if (val != null)
        {
            if (val is Array array)
            {
                if (arguments.Length != array.Rank)
                    throw new JScriptException(JSError.IncorrectNumberOfIndices);
                return array.GetValue(LateBinding.ToIndices(arguments));
            }

            val = Convert.ToObject(val, engine);
            if (val is ScriptObject scriptObject2)
            {
                if (brackets)
                    return scriptObject2[arguments];
                if (!(scriptObject2 is ScriptFunction scriptFunction))
                    throw new JScriptException(JSError.InvalidCall);
                if (thisob is IActivationObject activationObject)
                    thisob = activationObject.GetDefaultThisObject();
                object[] args = arguments == null ? new object[0] : arguments;
                object thisob2 = thisob;
                Binder binder1 = binder;
                CultureInfo culture1 = culture;
                return scriptFunction.Call(args, thisob2, binder1, culture1);
            }

            IReflect requiresInvokeMember =
                LateBinding.GetIRForObjectThatRequiresInvokeMember(val, VsaEngine.executeForJSEE);
            if (requiresInvokeMember != null)
            {
                if (brackets)
                {
                    string empty = string.Empty;
                    int length = arguments.Length;
                    if (length > 0)
                        empty = Convert.ToString(arguments[length - 1]);
                    return LateBinding.CallCOMObject(requiresInvokeMember, empty, val, binder, (object[])null,
                        (ParameterModifier[])null, culture, namedParameters, false, true, engine);
                }

                if (!(val is IReflect))
                    return LateBinding.CallCOMObject(requiresInvokeMember, string.Empty, val, binder, arguments,
                        (ParameterModifier[])null, culture, namedParameters, false, brackets, engine);
                object[] objArray = new object[(arguments != null ? arguments.Length : 0) + 1];
                objArray[0] = thisob;
                if (arguments != null)
                    ArrayObject.Copy(arguments, 0, objArray, 1, arguments.Length);
                string[] strArray = new string[(namedParameters != null ? namedParameters.Length : 0) + 1];
                strArray[0] = "[DISPID=-613]";
                if (namedParameters != null)
                    ArrayObject.Copy((object[])namedParameters, 0, (object[])strArray, 1, namedParameters.Length);
                return LateBinding.CallCOMObject(requiresInvokeMember, "[DISPID=0]", val, binder, objArray,
                    (ParameterModifier[])null, culture, strArray, false, brackets, engine);
            }

            if (VsaEngine.executeForJSEE && val is IDebuggerObject && val is IReflect)
                return LateBinding.CallCOMObject((IReflect)val, string.Empty, val, binder, arguments,
                    (ParameterModifier[])null, culture, namedParameters, false, brackets, engine);
            MemberInfo[] defaultMembers = TypeReflector.GetTypeReflectorFor(val.GetType()).GetDefaultMembers();
            if (defaultMembers != null && defaultMembers.Length != 0)
            {
                MethodInfo method = JSBinder.SelectMethod(Microsoft.JScript.Runtime.TypeRefs, defaultMembers,
                    ref arguments, namedParameters);
                if (method != (MethodInfo)null)
                    return LateBinding.CallMethod(method, arguments, val, binder, culture, namedParameters);
            }
        }

        throw new JScriptException(JSError.FunctionExpected);
    }

    private static void Change64bitIntegersToDouble(object[] arguments)
    {
        if (arguments == null)
            return;
        int index = 0;
        for (int length = arguments.Length; index < length; ++index)
        {
            object ob = arguments[index];
            IConvertible iconvertible = Convert.GetIConvertible(ob);
            switch (Convert.GetTypeCode(ob, iconvertible))
            {
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    arguments[index] = (object)iconvertible.ToDouble((IFormatProvider)null);
                    break;
            }
        }
    }

    /// <summary>Deletes the binding reference member from the object that the binding reference is defined on.</summary>
    /// <returns>true if the member was successfully deleted; otherwise, false.</returns>
    public bool Delete() => LateBinding.DeleteMember(this.obj, this.name);

    /// <summary>Deletes the specified member from the specified object.</summary>
    /// <returns>true if the member was successfully deleted; otherwise, false.</returns>
    /// <param name="obj">The object to delete from.</param>
    /// <param name="name">The member to delete.</param>
    public static bool DeleteMember(object obj, string name)
    {
        if (name != null)
        {
            switch (obj)
            {
                case null:
                    break;
                case ScriptObject _:
                    return ((ScriptObject)obj).DeleteMember(name);
                case IExpando _:
                    try
                    {
                        IExpando expando = (IExpando)obj;
                        MemberInfo m =
                            LateBinding.SelectMember(expando.GetMember(name,
                                BindingFlags.Instance | BindingFlags.Public));
                        if (!(m != (MemberInfo)null))
                            return false;
                        expando.RemoveMember(m);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                case IDictionary _:
                    IDictionary dictionary = (IDictionary)obj;
                    if (!dictionary.Contains((object)name))
                        return false;
                    dictionary.Remove((object)name);
                    return true;
                default:
                    Type type = obj.GetType();
                    MethodInfo method = TypeReflector.GetTypeReflectorFor(type).GetMethod("op_Delete",
                        BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, (Binder)null, new Type[2]
                        {
                            type,
                            typeof(object[])
                        }, (ParameterModifier[])null);
                    if (method == (MethodInfo)null ||
                        (method.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope ||
                        method.ReturnType != typeof(bool))
                        return false;
                    return (bool)method.Invoke((object)null, new object[2]
                    {
                        obj,
                        (object)new object[1] { (object)name }
                    });
            }
        }

        return false;
    }

    internal static bool DeleteValueAtIndex(object obj, ulong index)
    {
        return obj is ArrayObject && index < (ulong)uint.MaxValue
            ? ((ArrayObject)obj).DeleteValueAtIndex((uint)index)
            : LateBinding.DeleteMember(obj, index.ToString((IFormatProvider)CultureInfo.InvariantCulture));
    }

    private static IReflect GetIRForObjectThatRequiresInvokeMember(object obj, bool checkForDebugger)
    {
        Type type = obj.GetType();
        if (!TypeReflector.GetTypeReflectorFor(type).Is__ComObject())
            return (IReflect)null;
        if (!checkForDebugger || !(obj is IDebuggerObject debuggerObject))
            return (IReflect)type;
        return !debuggerObject.IsCOMObject() ? (IReflect)null : (IReflect)obj;
    }

    private static IReflect GetIRForObjectThatRequiresInvokeMember(
        object obj,
        bool checkForDebugger,
        TypeCode tcode)
    {
        return tcode == TypeCode.Object
            ? LateBinding.GetIRForObjectThatRequiresInvokeMember(obj, checkForDebugger)
            : (IReflect)null;
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal static object GetMemberValue(object obj, string name)
    {
        return obj is ScriptObject
            ? ((ScriptObject)obj).GetMemberValue(name)
            : new LateBinding(name, obj).GetNonMissingValue();
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal static object GetMemberValue2(object obj, string name)
    {
        return obj is ScriptObject ? ((ScriptObject)obj).GetMemberValue(name) : new LateBinding(name, obj).GetValue();
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal static object GetMemberValue(
        object obj,
        string name,
        MemberInfo member,
        MemberInfo[] members)
    {
        if (member != (MemberInfo)null)
        {
            try
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Event:
                        return (object)null;
                    case MemberTypes.Field:
                        object memberValue = ((FieldInfo)member).GetValue(obj);
                        Type enumType = obj as Type;
                        if (enumType != (Type)null)
                        {
                            if (enumType.IsEnum)
                            {
                                try
                                {
                                    memberValue = Enum.ToObject(enumType,
                                        ((IConvertible)memberValue).ToUInt64((IFormatProvider)null));
                                }
                                catch
                                {
                                }
                            }
                        }

                        return memberValue;
                    case MemberTypes.Property:
                        PropertyInfo prop = (PropertyInfo)member;
                        if (prop.DeclaringType == typeof(ArrayObject))
                        {
                            if (obj is ArrayObject arrayObject)
                                return arrayObject.length;
                        }
                        else if (prop.DeclaringType == typeof(StringObject) && obj is StringObject stringObject)
                            return (object)stringObject.length;

                        return JSProperty.GetValue(prop, obj, (object[])null);
                    case MemberTypes.NestedType:
                        return (object)member;
                }
            }
            catch
            {
                switch (obj)
                {
                    case StringObject _:
                        return LateBinding.GetMemberValue((object)((StringObject)obj).value, name, member, members);
                    case NumberObject _:
                        return LateBinding.GetMemberValue(((NumberObject)obj).value, name, member, members);
                    case BooleanObject _:
                        return LateBinding.GetMemberValue((object)((BooleanObject)obj).value, name, member, members);
                    case ArrayWrapper _:
                        return LateBinding.GetMemberValue((object)((ArrayWrapper)obj).value, name, member, members);
                    default:
                        throw;
                }
            }
        }

        if (members != null && members.Length != 0)
        {
            if (members.Length == 1 && members[0].MemberType == MemberTypes.Method)
            {
                MethodInfo member1 = (MethodInfo)members[0];
                Type declaringType = member1.DeclaringType;
                if (declaringType == typeof(GlobalObject) || declaringType != (Type)null &&
                    declaringType != typeof(StringObject) && declaringType != typeof(NumberObject) &&
                    declaringType != typeof(BooleanObject) && declaringType.IsSubclassOf(typeof(JSObject)))
                    return (object)Globals.BuiltinFunctionFor(obj, member1);
            }

            return (object)new FunctionWrapper(name, obj, members);
        }

        switch (obj)
        {
            case ScriptObject _:
                return ((ScriptObject)obj).GetMemberValue(name);
            case Namespace _:
                Namespace @namespace = (Namespace)obj;
                string str = $"{@namespace.Name}.{name}";
                Type type = @namespace.GetType(str);
                return type != (Type)null ? (object)type : (object)Namespace.GetNamespace(str, @namespace.engine);
            default:
                IReflect requiresInvokeMember = LateBinding.GetIRForObjectThatRequiresInvokeMember(obj, true);
                if (requiresInvokeMember != null)
                {
                    try
                    {
                        return requiresInvokeMember.InvokeMember(name,
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField |
                            BindingFlags.GetProperty | BindingFlags.OptionalParamBinding, (Binder)JSBinder.ob, obj,
                            (object[])null, (ParameterModifier[])null, (CultureInfo)null, (string[])null);
                    }
                    catch (MissingMemberException ex)
                    {
                    }
                    catch (COMException ex)
                    {
                        switch (ex.ErrorCode)
                        {
                            case -2147352573 /*0x80020003*/:
                            case -2147352570 /*0x80020006*/:
                                break;
                            default:
                                throw ex;
                        }
                    }
                }

                return (object)Missing.Value;
        }
    }

    /// <summary>Gets the value of the binding reference, and converts a <see cref="T:Microsoft.JScript.Missing" /> value to null.</summary>
    /// <returns>The value of the binding reference; otherwise, null.</returns>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public object GetNonMissingValue()
    {
        object obj = this.GetValue();
        return obj is Missing ? (object)null : obj;
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal object GetValue()
    {
        this.BindToMember();
        return LateBinding.GetMemberValue(this.obj, this.name, this.last_member, this.last_members);
    }

    /// <summary>Gets the value of the binding reference, and throws an exception if it cannot be resolved.</summary>
    /// <returns>The value of the binding reference.</returns>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public object GetValue2()
    {
        object obj = this.GetValue();
        return obj != Missing.Value
            ? obj
            : throw new JScriptException(JSError.UndefinedIdentifier,
                new Context(new DocumentContext("", (VsaEngine)null), this.name));
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal static object GetValueAtIndex(object obj, ulong index)
    {
        if (obj is ScriptObject)
            return index < (ulong)uint.MaxValue
                ? ((ScriptObject)obj).GetValueAtIndex((uint)index)
                : ((ScriptObject)obj).GetMemberValue(index.ToString((IFormatProvider)CultureInfo.InvariantCulture));
        MethodInfo propertyForArrayIndex;
        for (;
             !(obj is IList);
             obj = propertyForArrayIndex.Invoke(obj, BindingFlags.SuppressChangeType, (Binder)null, (object[])null,
                 (CultureInfo)null))
        {
            if (obj is Array)
                return ((Array)obj).GetValue(checked((int)index));
            Type type = obj.GetType();
            if (type.IsCOMObject || obj is IReflect || index > (ulong)int.MaxValue)
                return LateBinding.GetMemberValue(obj, index.ToString((IFormatProvider)CultureInfo.InvariantCulture));
            propertyForArrayIndex = JSBinder.GetDefaultPropertyForArrayIndex(type, (int)index, (Type)null, false);
            if (!(propertyForArrayIndex != (MethodInfo)null))
                return (object)Missing.Value;
            ParameterInfo[] parameters = propertyForArrayIndex.GetParameters();
            if (parameters != null && parameters.Length != 0)
                return propertyForArrayIndex.Invoke(obj, BindingFlags.Default, (Binder)JSBinder.ob, new object[1]
                {
                    (object)(int)index
                }, (CultureInfo)null);
        }

        return ((IList)obj)[checked((int)index)];
    }

    private static object[] LickArgumentsIntoShape(
        ParameterInfo[] pars,
        object[] arguments,
        Binder binder,
        CultureInfo culture)
    {
        if (arguments == null)
            return (object[])null;
        int length1 = pars.Length;
        if (length1 == 0)
            return (object[])null;
        object[] objArray = arguments;
        int length2 = arguments.Length;
        if (length2 != length1)
            objArray = new object[length1];
        int index1 = length1 - 1;
        int num = length2 < index1 ? length2 : index1;
        for (int index2 = 0; index2 < num; ++index2)
            objArray[index2] = !(arguments[index2] is DBNull)
                ? binder.ChangeType(arguments[index2], pars[index2].ParameterType, culture)
                : (object)null;
        for (int index3 = num; index3 < index1; ++index3)
        {
            object obj = TypeReferences.GetDefaultParameterValue(pars[index3]);
            if (obj == System.Convert.DBNull)
                obj = binder.ChangeType((object)null, pars[index3].ParameterType, culture);
            objArray[index3] = obj;
        }

        if (CustomAttribute.IsDefined(pars[index1], typeof(ParamArrayAttribute), false))
        {
            int length3 = length2 - index1;
            if (length3 < 0)
                length3 = 0;
            Type elementType = pars[index1].ParameterType.GetElementType();
            Array instance = Array.CreateInstance(elementType, length3);
            for (int index4 = 0; index4 < length3; ++index4)
                instance.SetValue(binder.ChangeType(arguments[index4 + index1], elementType, culture), index4);
            objArray[index1] = (object)instance;
        }
        else if (length2 < length1)
        {
            object obj = TypeReferences.GetDefaultParameterValue(pars[index1]);
            if (obj == System.Convert.DBNull)
                obj = binder.ChangeType((object)null, pars[index1].ParameterType, culture);
            objArray[index1] = obj;
        }
        else
            objArray[index1] = binder.ChangeType(arguments[index1], pars[index1].ParameterType, culture);

        return objArray;
    }

    internal static MemberInfo SelectMember(MemberInfo[] mems)
    {
        if (mems == null)
            return (MemberInfo)null;
        MemberInfo memberInfo = (MemberInfo)null;
        foreach (MemberInfo mem in mems)
        {
            switch (mem.MemberType)
            {
                case MemberTypes.Field:
                    if (memberInfo == (MemberInfo)null || memberInfo.MemberType != MemberTypes.Field)
                    {
                        memberInfo = mem;
                        break;
                    }

                    break;
                case MemberTypes.Property:
                    if (memberInfo == (MemberInfo)null || memberInfo.MemberType != MemberTypes.Field &&
                        memberInfo.MemberType != MemberTypes.Property)
                    {
                        ParameterInfo[] indexParameters = ((PropertyInfo)mem).GetIndexParameters();
                        if (indexParameters != null && indexParameters.Length == 0)
                        {
                            memberInfo = mem;
                            break;
                        }

                        break;
                    }

                    break;
                case MemberTypes.TypeInfo:
                case MemberTypes.NestedType:
                    if (memberInfo == (MemberInfo)null)
                    {
                        memberInfo = mem;
                        break;
                    }

                    break;
            }
        }

        return memberInfo;
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal void SetIndexedDefaultPropertyValue(object ob, object[] arguments, object value)
    {
        switch (ob)
        {
            case ScriptObject scriptObject:
                scriptObject[arguments] = value;
                break;
            case Array array:
                if (arguments.Length != array.Rank)
                    throw new JScriptException(JSError.IncorrectNumberOfIndices);
                array.SetValue(value, LateBinding.ToIndices(arguments));
                break;
            default:
                TypeCode typeCode = Convert.GetTypeCode(ob);
                if (Convert.NeedsWrapper(typeCode))
                    break;
                IReflect reflect =
                    LateBinding.GetIRForObjectThatRequiresInvokeMember(ob, this.checkForDebugger, typeCode);
                if (reflect == null && this.checkForDebugger && ob is IDebuggerObject && ob is IReflect)
                    reflect = (IReflect)ob;
                if (reflect != null)
                {
                    try
                    {
                        int length = arguments.Length + 1;
                        object[] objArray = new object[length];
                        ArrayObject.Copy(arguments, 0, objArray, 0, length - 1);
                        objArray[length - 1] = value;
                        reflect.InvokeMember(string.Empty,
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField |
                            BindingFlags.SetProperty | BindingFlags.OptionalParamBinding, (Binder)JSBinder.ob, ob,
                            objArray, (ParameterModifier[])null, (CultureInfo)null, (string[])null);
                        break;
                    }
                    catch (MissingMemberException ex)
                    {
                        throw new JScriptException(JSError.OLENoPropOrMethod);
                    }
                }
                else
                {
                    MemberInfo[] defaultMembers = TypeReflector.GetTypeReflectorFor(ob.GetType()).GetDefaultMembers();
                    if (defaultMembers != null && defaultMembers.Length != 0)
                    {
                        PropertyInfo prop = JSBinder.SelectProperty(Microsoft.JScript.Runtime.TypeRefs, defaultMembers,
                            arguments);
                        if (prop != (PropertyInfo)null)
                        {
                            MethodInfo setMethod = JSProperty.GetSetMethod(prop, false);
                            if (setMethod != (MethodInfo)null)
                            {
                                arguments = LateBinding.LickArgumentsIntoShape(prop.GetIndexParameters(), arguments,
                                    (Binder)JSBinder.ob, (CultureInfo)null);
                                value = Convert.CoerceT(value, prop.PropertyType);
                                int length = arguments.Length + 1;
                                object[] objArray = new object[length];
                                ArrayObject.Copy(arguments, 0, objArray, 0, length - 1);
                                objArray[length - 1] = value;
                                setMethod.Invoke(ob, objArray);
                                break;
                            }
                        }
                    }

                    throw new JScriptException(JSError.OLENoPropOrMethod);
                }
        }
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal void SetIndexedPropertyValue(object[] arguments, object value)
    {
        if (this.obj == null)
            throw new JScriptException(JSError.ObjectExpected);
        if (this.name == null)
        {
            this.SetIndexedDefaultPropertyValue(this.obj, arguments, value);
        }
        else
        {
            this.BindToMember();
            if (this.last_members != null && this.last_members.Length != 0)
            {
                PropertyInfo prop =
                    JSBinder.SelectProperty(Microsoft.JScript.Runtime.TypeRefs, this.last_members, arguments);
                if (prop != (PropertyInfo)null)
                {
                    if (arguments.Length != 0 && prop.GetIndexParameters().Length == 0 && !(prop is COMPropertyInfo))
                    {
                        MethodInfo getMethod = JSProperty.GetGetMethod(prop, false);
                        if (getMethod != (MethodInfo)null)
                        {
                            LateBinding.SetIndexedPropertyValueStatic(getMethod.Invoke(this.obj, (object[])null),
                                arguments, value);
                            return;
                        }
                    }

                    arguments = LateBinding.LickArgumentsIntoShape(prop.GetIndexParameters(), arguments,
                        (Binder)JSBinder.ob, (CultureInfo)null);
                    value = Convert.CoerceT(value, prop.PropertyType);
                    JSProperty.SetValue(prop, this.obj, value, arguments);
                    return;
                }
            }

            TypeCode typeCode = Convert.GetTypeCode(this.obj);
            if (Convert.NeedsWrapper(typeCode))
                return;
            IReflect requiresInvokeMember =
                LateBinding.GetIRForObjectThatRequiresInvokeMember(this.obj, this.checkForDebugger, typeCode);
            if (requiresInvokeMember != null)
            {
                int length = arguments.Length + 1;
                object[] objArray = new object[length];
                ArrayObject.Copy(arguments, 0, objArray, 0, length - 1);
                objArray[length - 1] = value;
                requiresInvokeMember.InvokeMember(this.name,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty |
                    BindingFlags.OptionalParamBinding, (Binder)JSBinder.ob, this.obj, objArray,
                    (ParameterModifier[])null, (CultureInfo)null, (string[])null);
            }
            else
            {
                object ob = this.GetValue();
                switch (ob)
                {
                    case null:
                    case Missing _:
                        throw new JScriptException(JSError.OLENoPropOrMethod);
                    default:
                        this.SetIndexedDefaultPropertyValue(ob, arguments, value);
                        break;
                }
            }
        }
    }

    /// <summary>Sets an indexed property.</summary>
    /// <param name="obj">The object that the binding reference is defined on.</param>
    /// <param name="arguments">The arguments to pass to the member.</param>
    /// <param name="value">The new value of the property.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public static void SetIndexedPropertyValueStatic(object obj, object[] arguments, object value)
    {
        switch (obj)
        {
            case null:
                throw new JScriptException(JSError.ObjectExpected);
            case ScriptObject scriptObject:
                scriptObject[arguments] = value;
                break;
            case Array array:
                if (arguments.Length != array.Rank)
                    throw new JScriptException(JSError.IncorrectNumberOfIndices);
                array.SetValue(value, LateBinding.ToIndices(arguments));
                break;
            default:
                TypeCode typeCode = Convert.GetTypeCode(obj);
                if (Convert.NeedsWrapper(typeCode))
                    break;
                IReflect requiresInvokeMember = LateBinding.GetIRForObjectThatRequiresInvokeMember(obj, true, typeCode);
                if (requiresInvokeMember != null)
                {
                    string empty = string.Empty;
                    int length = arguments.Length;
                    if (length > 0)
                        empty = Convert.ToString(arguments[length - 1]);
                    requiresInvokeMember.InvokeMember(empty,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty |
                        BindingFlags.OptionalParamBinding, (Binder)JSBinder.ob, obj, new object[1]
                        {
                            value
                        }, (ParameterModifier[])null, (CultureInfo)null, (string[])null);
                    break;
                }

                MemberInfo[] defaultMembers = TypeReflector.GetTypeReflectorFor(obj.GetType()).GetDefaultMembers();
                if (defaultMembers != null && defaultMembers.Length != 0)
                {
                    PropertyInfo prop =
                        JSBinder.SelectProperty(Microsoft.JScript.Runtime.TypeRefs, defaultMembers, arguments);
                    if (prop != (PropertyInfo)null)
                    {
                        MethodInfo setMethod = JSProperty.GetSetMethod(prop, false);
                        if (setMethod != (MethodInfo)null)
                        {
                            arguments = LateBinding.LickArgumentsIntoShape(prop.GetIndexParameters(), arguments,
                                (Binder)JSBinder.ob, (CultureInfo)null);
                            value = Convert.CoerceT(value, prop.PropertyType);
                            int length = arguments.Length + 1;
                            object[] objArray = new object[length];
                            ArrayObject.Copy(arguments, 0, objArray, 0, length - 1);
                            objArray[length - 1] = value;
                            setMethod.Invoke(obj, objArray);
                            break;
                        }
                    }
                }

                throw new JScriptException(JSError.OLENoPropOrMethod);
        }
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    private static void SetMember(object obj, object value, MemberInfo member)
    {
        switch (member.MemberType)
        {
            case MemberTypes.Field:
                FieldInfo fieldInfo = (FieldInfo)member;
                if (fieldInfo.IsLiteral || fieldInfo.IsInitOnly)
                    break;
                if (fieldInfo is JSField)
                {
                    fieldInfo.SetValue(obj, value);
                    break;
                }

                fieldInfo.SetValue(obj, Convert.CoerceT(value, fieldInfo.FieldType), BindingFlags.SuppressChangeType,
                    (Binder)null, (CultureInfo)null);
                break;
            case MemberTypes.Property:
                PropertyInfo prop = (PropertyInfo)member;
                switch (prop)
                {
                    case JSProperty _:
                    case JSWrappedProperty _:
                        prop.SetValue(obj, value, (object[])null);
                        return;
                    default:
                        MethodInfo setMethod = JSProperty.GetSetMethod(prop, false);
                        if (!(setMethod != (MethodInfo)null))
                            return;
                        try
                        {
                            setMethod.Invoke(obj, BindingFlags.SuppressChangeType, (Binder)null, new object[1]
                            {
                                Convert.CoerceT(value, prop.PropertyType)
                            }, (CultureInfo)null);
                            return;
                        }
                        catch (TargetInvocationException ex)
                        {
                            throw ex.InnerException;
                        }
                }
        }
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal static void SetMemberValue(object obj, string name, object value)
    {
        if (obj is ScriptObject)
            ((ScriptObject)obj).SetMemberValue(name, value);
        else
            new LateBinding(name, obj).SetValue(value);
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal static void SetMemberValue(
        object obj,
        string name,
        object value,
        MemberInfo member,
        MemberInfo[] members)
    {
        if (member != (MemberInfo)null)
            LateBinding.SetMember(obj, value, member);
        else if (obj is ScriptObject)
        {
            ((ScriptObject)obj).SetMemberValue(name, value);
        }
        else
        {
            TypeCode typeCode = Convert.GetTypeCode(obj);
            if (Convert.NeedsWrapper(typeCode))
                return;
            IReflect requiresInvokeMember = LateBinding.GetIRForObjectThatRequiresInvokeMember(obj, true, typeCode);
            if (requiresInvokeMember != null)
            {
                try
                {
                    object[] args = new object[1] { value };
                    IConvertible convertible = value as IConvertible;
                    BindingFlags invokeAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField |
                                              BindingFlags.SetProperty | BindingFlags.OptionalParamBinding;
                    requiresInvokeMember.InvokeMember(name, invokeAttr, (Binder)JSBinder.ob, obj, args,
                        (ParameterModifier[])null, (CultureInfo)null, (string[])null);
                    return;
                }
                catch (MissingMemberException ex)
                {
                }
                catch (COMException ex)
                {
                    switch (ex.ErrorCode)
                    {
                        case -2147352573 /*0x80020003*/:
                        case -2147352570 /*0x80020006*/:
                            break;
                        default:
                            throw ex;
                    }
                }
            }

            if (!(obj is IExpando))
                return;
            PropertyInfo propertyInfo = ((IExpando)obj).AddProperty(name);
            if (propertyInfo != (PropertyInfo)null)
            {
                propertyInfo.SetValue(obj, value, (object[])null);
            }
            else
            {
                FieldInfo fieldInfo = ((IExpando)obj).AddField(name);
                if (!(fieldInfo != (FieldInfo)null))
                    return;
                fieldInfo.SetValue(obj, value);
            }
        }
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal static void SetValueAtIndex(object obj, ulong index, object value)
    {
        if (obj is ScriptObject)
        {
            if (index < (ulong)uint.MaxValue)
                ((ScriptObject)obj).SetValueAtIndex((uint)index, value);
            else
                ((ScriptObject)obj).SetMemberValue(index.ToString((IFormatProvider)CultureInfo.InvariantCulture),
                    value);
        }
        else
        {
            MethodInfo propertyForArrayIndex;
            for (;
                 !(obj is IList);
                 obj = propertyForArrayIndex.Invoke(obj, BindingFlags.SuppressChangeType, (Binder)null, (object[])null,
                     (CultureInfo)null))
            {
                if (obj is Array)
                {
                    ((Array)obj).SetValue(Convert.CoerceT(value, obj.GetType().GetElementType()), checked((int)index));
                    return;
                }

                Type type = obj.GetType();
                if (type.IsCOMObject || obj is IReflect || index > (ulong)int.MaxValue)
                {
                    LateBinding.SetMemberValue(obj, index.ToString((IFormatProvider)CultureInfo.InvariantCulture),
                        value);
                    return;
                }

                propertyForArrayIndex = JSBinder.GetDefaultPropertyForArrayIndex(type, (int)index, (Type)null, true);
                if (!(propertyForArrayIndex != (MethodInfo)null))
                    return;
                ParameterInfo[] parameters = propertyForArrayIndex.GetParameters();
                if (parameters != null && parameters.Length != 0)
                {
                    propertyForArrayIndex.Invoke(obj, BindingFlags.Default, (Binder)JSBinder.ob, new object[2]
                    {
                        (object)(int)index,
                        value
                    }, (CultureInfo)null);
                    return;
                }
            }

            IList list = (IList)obj;
            if (index < (ulong)list.Count)
                list[(int)index] = value;
            else
                list.Insert((int)index, value);
        }
    }

    /// <summary>Sets a property.</summary>
    /// <param name="value">The new value of the property.</param>
    [DebuggerStepThrough]
    [DebuggerHidden]
    public void SetValue(object value)
    {
        this.BindToMember();
        LateBinding.SetMemberValue(this.obj, this.name, value, this.last_member, this.last_members);
    }

    internal static void SwapValues(object obj, uint left, uint right)
    {
        switch (obj)
        {
            case JSObject _:
                ((JSObject)obj).SwapValues(left, right);
                break;
            case Array _:
                Array array = (Array)obj;
                object obj2 = array.GetValue((int)left);
                array.SetValue(array.GetValue((int)right), (int)left);
                array.SetValue(obj2, (int)right);
                break;
            case IList _:
                IList list = (IList)obj;
                object obj1 = list[(int)left];
                list[(int)left] = list[(int)right];
                list[(int)right] = obj1;
                break;
            case IExpando _:
                string name1 = System.Convert.ToString(left, (IFormatProvider)CultureInfo.InvariantCulture);
                string name2 = System.Convert.ToString(right, (IFormatProvider)CultureInfo.InvariantCulture);
                IExpando expando = (IExpando)obj;
                FieldInfo m1 = expando.GetField(name1, BindingFlags.Instance | BindingFlags.Public);
                FieldInfo m2 = expando.GetField(name2, BindingFlags.Instance | BindingFlags.Public);
                if (m1 == (FieldInfo)null)
                {
                    if (m2 == (FieldInfo)null)
                        break;
                    try
                    {
                        m1 = expando.AddField(name1);
                        m1.SetValue(obj, m2.GetValue(obj));
                        expando.RemoveMember((MemberInfo)m2);
                    }
                    catch
                    {
                        throw new JScriptException(JSError.ActionNotSupported);
                    }
                }
                else if (m2 == (FieldInfo)null)
                {
                    try
                    {
                        m2 = expando.AddField(name2);
                        m2.SetValue(obj, m1.GetValue(obj));
                        expando.RemoveMember((MemberInfo)m1);
                    }
                    catch
                    {
                        throw new JScriptException(JSError.ActionNotSupported);
                    }
                }

                object obj3 = m1.GetValue(obj);
                m1.SetValue(obj, m2.GetValue(obj));
                m2.SetValue(obj, obj3);
                break;
            default:
                object valueAtIndex1 = LateBinding.GetValueAtIndex(obj, (ulong)left);
                object valueAtIndex2 = LateBinding.GetValueAtIndex(obj, (ulong)right);
                if (valueAtIndex1 is Missing)
                    LateBinding.DeleteValueAtIndex(obj, (ulong)right);
                else
                    LateBinding.SetValueAtIndex(obj, (ulong)right, valueAtIndex1);
                if (valueAtIndex2 is Missing)
                {
                    LateBinding.DeleteValueAtIndex(obj, (ulong)left);
                    break;
                }

                LateBinding.SetValueAtIndex(obj, (ulong)left, valueAtIndex2);
                break;
        }
    }

    private static int[] ToIndices(object[] arguments)
    {
        int length = arguments.Length;
        int[] indices = new int[length];
        for (int index = 0; index < length; ++index)
            indices[index] = Convert.ToInt32(arguments[index]);
        return indices;
    }
}