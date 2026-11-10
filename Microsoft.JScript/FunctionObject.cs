// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.FunctionObject
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.CodeDom.Compiler;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a function object. </summary>
public sealed class FunctionObject : ScriptFunction
{
    internal ParameterDeclaration[] parameter_declarations;
    internal string[] formal_parameters;
    internal TypeExpression return_type_expr;
    private Block body;
    private MethodInfo method;
    private ParameterInfo[] parameterInfos;
    internal Context funcContext;
    private int argumentsSlotNumber;
    internal JSLocalField[] fields;
    internal FunctionScope own_scope;
    internal ScriptObject enclosing_scope;
    internal bool must_save_stack_locals;
    internal bool hasArgumentsObject;
    internal IReflect implementedIface;
    internal MethodInfo implementedIfaceMethod;
    internal bool isMethod;
    internal bool isExpandoMethod;
    internal bool isConstructor;
    internal bool isImplicitCtor;
    internal bool isStatic;
    internal bool noVersionSafeAttributeSpecified;
    internal bool suppressIL;
    internal string text;
    private MethodBuilder mb;
    private ConstructorBuilder cb;
    internal TypeBuilder classwriter;
    internal MethodAttributes attributes;
    internal Globals globals;
    private ConstructorInfo superConstructor;
    internal ConstructorCall superConstructorCall;
    internal CustomAttributeList customAttributes;
    private CLSComplianceSpec clsCompliance;
    private LocalBuilder engineLocal;
    private bool partiallyEvaluated;
    internal Label returnLabel;

    internal FunctionObject(
        string name,
        ParameterDeclaration[] parameter_declarations,
        TypeExpression return_type_expr,
        Block body,
        FunctionScope own_scope,
        ScriptObject enclosing_scope,
        Context funcContext,
        MethodAttributes attributes)
        : this(name, parameter_declarations, return_type_expr, body, own_scope, enclosing_scope, funcContext,
            attributes, (CustomAttributeList)null, false)
    {
    }

    internal FunctionObject(
        string name,
        ParameterDeclaration[] parameter_declarations,
        TypeExpression return_type_expr,
        Block body,
        FunctionScope own_scope,
        ScriptObject enclosing_scope,
        Context funcContext,
        MethodAttributes attributes,
        CustomAttributeList customAttributes,
        bool isMethod)
        : base((ScriptObject)body.Globals.globalObject.originalFunction.originalPrototype, name,
            parameter_declarations.Length)
    {
        this.parameter_declarations = parameter_declarations;
        int length = parameter_declarations.Length;
        this.formal_parameters = new string[length];
        for (int index = 0; index < length; ++index)
            this.formal_parameters[index] = parameter_declarations[index].identifier;
        this.argumentsSlotNumber = 0;
        this.return_type_expr = return_type_expr;
        if (this.return_type_expr != null)
            own_scope.AddReturnValueField();
        this.body = body;
        this.method = (MethodInfo)null;
        this.parameterInfos = (ParameterInfo[])null;
        this.funcContext = funcContext;
        this.own_scope = own_scope;
        this.own_scope.owner = this;
        if ((!(enclosing_scope is ActivationObject) || !((ActivationObject)enclosing_scope).fast) && !isMethod)
        {
            this.argumentsSlotNumber = this.own_scope.GetNextSlotNumber();
            JSLocalField jsLocalField =
                (JSLocalField)this.own_scope.AddNewField("arguments", (object)null, FieldAttributes.PrivateScope);
            jsLocalField.type = new TypeExpression((AST)new ConstantWrapper((object)Typeob.Object, funcContext));
            jsLocalField.isDefined = true;
            this.hasArgumentsObject = true;
        }
        else
            this.hasArgumentsObject = false;

        this.implementedIface = (IReflect)null;
        this.implementedIfaceMethod = (MethodInfo)null;
        this.isMethod = isMethod;
        this.isExpandoMethod = customAttributes != null && customAttributes.ContainsExpandoAttribute();
        this.isStatic = this.own_scope.isStatic = (attributes & MethodAttributes.Static) != 0;
        this.suppressIL = false;
        this.noVersionSafeAttributeSpecified = true;
        this.fields = this.own_scope.GetLocalFields();
        this.enclosing_scope = enclosing_scope;
        this.must_save_stack_locals = false;
        this.text = (string)null;
        this.mb = (MethodBuilder)null;
        this.cb = (ConstructorBuilder)null;
        this.attributes = attributes;
        if (!this.isStatic)
            this.attributes |= MethodAttributes.HideBySig;
        this.globals = body.Globals;
        this.superConstructor = (ConstructorInfo)null;
        this.superConstructorCall = (ConstructorCall)null;
        this.customAttributes = customAttributes;
        this.noExpando = false;
        this.clsCompliance = CLSComplianceSpec.NotAttributed;
        this.engineLocal = (LocalBuilder)null;
        this.partiallyEvaluated = false;
    }

    internal FunctionObject(
        Type t,
        string name,
        string method_name,
        string[] formal_parameters,
        JSLocalField[] fields,
        bool must_save_stack_locals,
        bool hasArgumentsObject,
        string text,
        VsaEngine engine)
        : base((ScriptObject)engine.Globals.globalObject.originalFunction.originalPrototype, name,
            formal_parameters.Length)
    {
        this.engine = engine;
        this.formal_parameters = formal_parameters;
        this.argumentsSlotNumber = 0;
        this.body = (Block)null;
        this.method = TypeReflector.GetTypeReflectorFor(Globals.TypeRefs.ToReferenceContext(t))
            .GetMethod(method_name, BindingFlags.Static | BindingFlags.Public);
        this.parameterInfos = this.method.GetParameters();
        if (!CustomAttribute.IsDefined((MemberInfo)this.method, typeof(JSFunctionAttribute), false))
            this.isMethod = true;
        else
            this.isExpandoMethod =
                (((JSFunctionAttribute)CustomAttribute.GetCustomAttributes((MemberInfo)this.method,
                    typeof(JSFunctionAttribute), false)[0]).attributeValue & JSFunctionAttributeEnum.IsExpandoMethod) !=
                0;
        this.funcContext = (Context)null;
        this.own_scope = (FunctionScope)null;
        this.fields = fields;
        this.must_save_stack_locals = must_save_stack_locals;
        this.hasArgumentsObject = hasArgumentsObject;
        this.text = text;
        this.attributes = MethodAttributes.Public;
        this.globals = engine.Globals;
        this.superConstructor = (ConstructorInfo)null;
        this.superConstructorCall = (ConstructorCall)null;
        this.enclosing_scope = this.globals.ScopeStack.Peek();
        this.noExpando = false;
        this.clsCompliance = CLSComplianceSpec.NotAttributed;
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal override object Call(object[] args, object thisob)
    {
        return this.Call(args, thisob, (Binder)null, (CultureInfo)null);
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal override object Call(object[] args, object thisob, Binder binder, CultureInfo culture)
    {
        if (this.body == null)
            return this.Call(args, thisob, this.enclosing_scope, new Closure(this), binder, culture);
        StackFrame sf = new StackFrame(thisob is JSObject ? (ScriptObject)thisob : this.enclosing_scope, this.fields,
            new object[this.fields.Length], thisob);
        if (this.isConstructor)
        {
            sf.closureInstance = thisob;
            if (this.superConstructor != (ConstructorInfo)null)
            {
                if (this.superConstructorCall == null)
                {
                    if (this.superConstructor is JSConstructor)
                        this.superConstructor.Invoke(thisob, new object[0]);
                }
                else
                {
                    ASTList arguments = this.superConstructorCall.arguments;
                    int count = arguments.count;
                    object[] parameters = new object[count];
                    for (int i = 0; i < count; ++i)
                        parameters[i] = arguments[i].Evaluate();
                    this.superConstructor.Invoke(thisob, BindingFlags.Default, binder, parameters, culture);
                }
            }

            this.globals.ScopeStack.GuardedPush(thisob is JSObject
                ? (object)(ScriptObject)thisob
                : (object)this.enclosing_scope);
            try
            {
                ((ClassScope)this.enclosing_scope).owner.body.EvaluateInstanceVariableInitializers();
            }
            finally
            {
                this.globals.ScopeStack.Pop();
            }
        }
        else if (this.isMethod && !this.isStatic)
            sf.closureInstance = ((ClassScope)this.enclosing_scope).HasInstance(thisob)
                ? thisob
                : throw new JScriptException(JSError.TypeMismatch);

        this.globals.ScopeStack.GuardedPush((object)sf);
        try
        {
            this.own_scope.CloseNestedFunctions(sf);
            this.ConvertArguments(args, sf.localVars, 0, args.Length, this.formal_parameters.Length, binder, culture);
            Completion completion = (Completion)this.body.Evaluate();
            return completion.Return ? completion.value : (object)null;
        }
        finally
        {
            this.globals.ScopeStack.Pop();
        }
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal override object Call(
        object[] args,
        object thisob,
        ScriptObject enclosing_scope,
        Closure calleeClosure,
        Binder binder,
        CultureInfo culture)
    {
        if (this.body != null)
            return this.CallASTFunc(args, thisob, enclosing_scope, calleeClosure, binder, culture);
        object caller = calleeClosure.caller;
        calleeClosure.caller = this.globals.caller;
        this.globals.caller = (object)calleeClosure;
        object arguments = calleeClosure.arguments;
        ScriptObject scriptObject = this.globals.ScopeStack.Peek();
        ArgumentsObject callerArguments = scriptObject is StackFrame
            ? ((StackFrame)scriptObject).caller_arguments
            : (ArgumentsObject)null;
        StackFrame scope = new StackFrame(enclosing_scope, this.fields,
            this.must_save_stack_locals ? new object[this.fields.Length] : (object[])null, thisob);
        this.globals.ScopeStack.GuardedPush((object)scope);
        ArgumentsObject argumentsObject =
            new ArgumentsObject((ScriptObject)this.globals.globalObject.originalObjectPrototype, args, this,
                calleeClosure, (ScriptObject)scope, callerArguments);
        scope.caller_arguments = argumentsObject;
        calleeClosure.arguments = (object)argumentsObject;
        try
        {
            int length1 = this.formal_parameters.Length;
            int length2 = args.Length;
            if (this.hasArgumentsObject)
            {
                object[] objArray = new object[length1 + 3];
                objArray[0] = thisob;
                objArray[1] = (object)this.engine;
                objArray[2] = (object)argumentsObject;
                this.ConvertArguments(args, objArray, 3, length2, length1, binder, culture);
                return this.method.Invoke(thisob, BindingFlags.SuppressChangeType, (Binder)null, objArray,
                    (CultureInfo)null);
            }

            if (!this.isMethod)
            {
                object[] objArray = new object[length1 + 2];
                objArray[0] = thisob;
                objArray[1] = (object)this.engine;
                this.ConvertArguments(args, objArray, 2, length2, length1, binder, culture);
                return this.method.Invoke(thisob, BindingFlags.SuppressChangeType, (Binder)null, objArray,
                    (CultureInfo)null);
            }

            if (length1 == length2)
            {
                this.ConvertArguments(args, args, 0, length2, length1, binder, culture);
                return this.method.Invoke(thisob, BindingFlags.SuppressChangeType, (Binder)null, args,
                    (CultureInfo)null);
            }

            object[] objArray1 = new object[length1];
            this.ConvertArguments(args, objArray1, 0, length2, length1, binder, culture);
            return this.method.Invoke(thisob, BindingFlags.SuppressChangeType, (Binder)null, objArray1,
                (CultureInfo)null);
        }
        catch (TargetInvocationException ex)
        {
            throw ex.InnerException;
        }
        finally
        {
            this.globals.ScopeStack.Pop();
            calleeClosure.arguments = arguments;
            this.globals.caller = calleeClosure.caller;
            calleeClosure.caller = caller;
        }
    }

    private object CallASTFunc(
        object[] args,
        object thisob,
        ScriptObject enclosing_scope,
        Closure calleeClosure,
        Binder binder,
        CultureInfo culture)
    {
        object caller = calleeClosure.caller;
        calleeClosure.caller = this.globals.caller;
        this.globals.caller = (object)calleeClosure;
        object arguments = calleeClosure.arguments;
        ScriptObject scriptObject = this.globals.ScopeStack.Peek();
        ArgumentsObject callerArguments = scriptObject is StackFrame
            ? ((StackFrame)scriptObject).caller_arguments
            : (ArgumentsObject)null;
        StackFrame stackFrame = new StackFrame(enclosing_scope, this.fields, new object[this.fields.Length], thisob);
        if (this.isMethod && !this.isStatic)
            stackFrame.closureInstance = thisob;
        this.globals.ScopeStack.GuardedPush((object)stackFrame);
        try
        {
            this.own_scope.CloseNestedFunctions(stackFrame);
            ArgumentsObject argumentsObject = (ArgumentsObject)null;
            if (this.hasArgumentsObject)
            {
                argumentsObject = new ArgumentsObject((ScriptObject)this.globals.globalObject.originalObjectPrototype,
                    args, this, calleeClosure, (ScriptObject)stackFrame, callerArguments);
                stackFrame.localVars[this.argumentsSlotNumber] = (object)argumentsObject;
            }

            stackFrame.caller_arguments = argumentsObject;
            calleeClosure.arguments = (object)argumentsObject;
            this.ConvertArguments(args, stackFrame.localVars, 0, args.Length, this.formal_parameters.Length, binder,
                culture);
            Completion completion = (Completion)this.body.Evaluate();
            return completion.Return ? completion.value : (object)null;
        }
        finally
        {
            this.globals.ScopeStack.Pop();
            calleeClosure.arguments = arguments;
            this.globals.caller = calleeClosure.caller;
            calleeClosure.caller = caller;
        }
    }

    internal void CheckCLSCompliance(bool classIsCLSCompliant)
    {
        if (classIsCLSCompliant)
        {
            if (this.clsCompliance == CLSComplianceSpec.NonCLSCompliant)
                return;
            int index = 0;
            for (int length = this.parameter_declarations.Length; index < length; ++index)
            {
                IReflect parameterIreflect = this.parameter_declarations[index].ParameterIReflect;
                if (parameterIreflect != null && !TypeExpression.TypeIsCLSCompliant((object)parameterIreflect))
                {
                    this.clsCompliance = CLSComplianceSpec.NonCLSCompliant;
                    this.funcContext.HandleError(JSError.NonCLSCompliantMember);
                    return;
                }
            }

            if (this.return_type_expr == null || this.return_type_expr.IsCLSCompliant())
                return;
            this.clsCompliance = CLSComplianceSpec.NonCLSCompliant;
            this.funcContext.HandleError(JSError.NonCLSCompliantMember);
        }
        else
        {
            if (this.clsCompliance != CLSComplianceSpec.CLSCompliant)
                return;
            this.funcContext.HandleError(JSError.MemberTypeCLSCompliantMismatch);
        }
    }

    [DebuggerStepThrough]
    [DebuggerHidden]
    internal object Construct(JSObject thisob, object[] args)
    {
        JSObject thisob1 = new JSObject((ScriptObject)null, false);
        thisob1.SetParent(this.GetPrototypeForConstructedObject());
        thisob1.outer_class_instance = thisob;
        object obj = this.Call(args, (object)thisob1);
        return obj is ScriptObject ? obj : (object)thisob1;
    }

    private void ConvertArguments(
        object[] args,
        object[] newargs,
        int offset,
        int length,
        int n,
        Binder binder,
        CultureInfo culture)
    {
        ParameterInfo[] parameterInfos = this.parameterInfos;
        if (parameterInfos != null)
        {
            int offset1 = 0;
            int index = offset;
            while (offset1 < n)
            {
                Type parameterType = parameterInfos[index].ParameterType;
                if (offset1 == n - 1 &&
                    CustomAttribute.IsDefined(parameterInfos[index], typeof(ParamArrayAttribute), false))
                {
                    int n1 = length - offset1;
                    if (n1 < 0)
                        n1 = 0;
                    newargs[index] = (object)FunctionObject.CopyToNewParamArray(parameterType.GetElementType(), n1,
                        args, offset1, binder, culture);
                    break;
                }

                object obj = offset1 < length ? args[offset1] : (object)null;
                newargs[index] = !(parameterType == Typeob.Object)
                    ? (binder == null
                        ? Convert.CoerceT(obj, parameterType)
                        : binder.ChangeType(obj, parameterType, culture))
                    : obj;
                ++offset1;
                ++index;
            }
        }
        else
        {
            ParameterDeclaration[] parameterDeclarations = this.parameter_declarations;
            int offset2 = 0;
            int index = offset;
            while (offset2 < n)
            {
                IReflect parameterIreflect = parameterDeclarations[offset2].ParameterIReflect;
                if (offset2 == n - 1 && CustomAttribute.IsDefined((ParameterInfo)parameterDeclarations[index],
                        typeof(ParamArrayAttribute), false))
                {
                    int n2 = length - offset2;
                    if (n2 < 0)
                        n2 = 0;
                    newargs[index] =
                        (object)FunctionObject.CopyToNewParamArray(((TypedArray)parameterIreflect).elementType, n2,
                            args, offset2);
                    break;
                }

                object obj = offset2 < length ? args[offset2] : (object)null;
                newargs[index] = (object)parameterIreflect != (object)Typeob.Object
                    ? (!(parameterIreflect is ClassScope)
                        ? (binder == null
                            ? Convert.CoerceT(obj, Convert.ToType(parameterIreflect))
                            : binder.ChangeType(obj, Convert.ToType(parameterIreflect), culture))
                        : Convert.Coerce(obj, (object)parameterIreflect))
                    : obj;
                ++offset2;
                ++index;
            }
        }
    }

    private static object[] CopyToNewParamArray(IReflect ir, int n, object[] args, int offset)
    {
        object[] newParamArray = new object[n];
        for (int index = 0; index < n; ++index)
            newParamArray[index] = Convert.Coerce(args[index + offset], (object)ir);
        return newParamArray;
    }

    private static Array CopyToNewParamArray(
        Type t,
        int n,
        object[] args,
        int offset,
        Binder binder,
        CultureInfo culture)
    {
        Array instance = Array.CreateInstance(t, n);
        for (int index = 0; index < n; ++index)
            instance.SetValue(binder.ChangeType(args[index + offset], t, culture), index);
        return instance;
    }

    internal void EmitLastLineInfo(ILGenerator il)
    {
        if (this.isImplicitCtor)
            return;
        int endLine = this.body.context.EndLine;
        int endColumn = this.body.context.EndColumn;
        this.body.context.document.EmitLineInfo(il, endLine, endColumn, endLine, endColumn + 1);
    }

    internal string GetName() => this.name;

    internal override int GetNumberOfFormalParameters() => this.formal_parameters.Length;

    internal ConstructorInfo GetConstructorInfo(CompilerGlobals compilerGlobals)
    {
        return (ConstructorInfo)this.GetMethodBase(compilerGlobals);
    }

    internal MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
    {
        return (MethodInfo)this.GetMethodBase(compilerGlobals);
    }

    internal MethodBase GetMethodBase(CompilerGlobals compilerGlobals)
    {
        if ((MethodInfo)this.mb != (MethodInfo)null)
            return (MethodBase)this.mb;
        if ((ConstructorInfo)this.cb != (ConstructorInfo)null)
            return (MethodBase)this.cb;
        JSFunctionAttributeEnum functionAttributeEnum = JSFunctionAttributeEnum.None;
        int num = 3;
        if (this.isMethod)
        {
            if (this.isConstructor && ((ClassScope)this.enclosing_scope).outerClassField != (FieldInfo)null)
            {
                num = 1;
                functionAttributeEnum |= JSFunctionAttributeEnum.IsInstanceNestedClassConstructor;
            }
            else
                num = 0;
        }
        else if (!this.hasArgumentsObject)
            num = 2;

        int iSequence = this.formal_parameters.Length + num;
        Type[] parameterTypes = new Type[iSequence];
        Type type = Convert.ToType(this.ReturnType((JSField)null));
        if (num > 0)
        {
            if (this.isConstructor)
                parameterTypes[iSequence - 1] = ((ClassScope)this.enclosing_scope).outerClassField.FieldType;
            else
                parameterTypes[0] = Typeob.Object;
            functionAttributeEnum |= JSFunctionAttributeEnum.HasThisObject;
        }

        if (num > 1)
        {
            parameterTypes[1] = Typeob.VsaEngine;
            functionAttributeEnum |= JSFunctionAttributeEnum.HasEngine;
        }

        if (num > 2)
        {
            parameterTypes[2] = Typeob.Object;
            functionAttributeEnum |= JSFunctionAttributeEnum.HasArguments;
        }

        if (this.must_save_stack_locals)
            functionAttributeEnum |= JSFunctionAttributeEnum.HasStackFrame;
        if (this.isExpandoMethod)
            functionAttributeEnum |= JSFunctionAttributeEnum.IsExpandoMethod;
        if (this.isConstructor)
        {
            for (int index = 0; index < iSequence - num; ++index)
                parameterTypes[index] = this.parameter_declarations[index].ParameterType;
        }
        else
        {
            for (int index = num; index < iSequence; ++index)
                parameterTypes[index] = this.parameter_declarations[index - num].ParameterType;
        }

        if (this.enclosing_scope is ClassScope)
        {
            if (this.isConstructor)
            {
                this.cb = ((ClassScope)this.enclosing_scope).GetTypeBuilder().DefineConstructor(
                    this.attributes & MethodAttributes.MemberAccessMask, CallingConventions.Standard, parameterTypes);
            }
            else
            {
                string name = this.name;
                if (this.implementedIfaceMethod != (MethodInfo)null)
                {
                    JSMethod implementedIfaceMethod = this.implementedIfaceMethod as JSMethod;
                    if ((MethodInfo)implementedIfaceMethod != (MethodInfo)null)
                        this.implementedIfaceMethod = implementedIfaceMethod.GetMethodInfo(compilerGlobals);
                    name = $"{this.implementedIfaceMethod.DeclaringType.FullName}.{name}";
                }

                TypeBuilder typeBuilder = ((ClassScope)this.enclosing_scope).GetTypeBuilder();
                if ((MethodInfo)this.mb != (MethodInfo)null)
                    return (MethodBase)this.mb;
                this.mb = typeBuilder.DefineMethod(name, this.attributes, type, parameterTypes);
                if (this.implementedIfaceMethod != (MethodInfo)null)
                    ((ClassScope)this.enclosing_scope).GetTypeBuilder()
                        .DefineMethodOverride((MethodInfo)this.mb, this.implementedIfaceMethod);
            }
        }
        else
        {
            if (this.enclosing_scope is FunctionScope)
            {
                if (((FunctionScope)this.enclosing_scope).owner != null)
                {
                    this.name = $"{((FunctionScope)this.enclosing_scope).owner.name}.{this.name}";
                    functionAttributeEnum |= JSFunctionAttributeEnum.IsNested;
                }
                else
                {
                    for (ScriptObject scriptObject = this.enclosing_scope;
                         scriptObject != null;
                         scriptObject = scriptObject.GetParent())
                    {
                        if (scriptObject is FunctionScope && ((FunctionScope)scriptObject).owner != null)
                        {
                            this.name = $"{((FunctionScope)scriptObject).owner.name}.{this.name}";
                            functionAttributeEnum |= JSFunctionAttributeEnum.IsNested;
                            break;
                        }
                    }
                }
            }

            if (compilerGlobals.usedNames[(object)this.name] != null)
                this.name =
                    $"{this.name}:{compilerGlobals.usedNames.count.ToString((IFormatProvider)CultureInfo.InvariantCulture)}";
            compilerGlobals.usedNames[(object)this.name] = (object)this;
            ScriptObject scriptObject1 = this.enclosing_scope;
            while (true)
            {
                switch (scriptObject1)
                {
                    case null:
                    case ClassScope _:
                        goto label_54;
                    default:
                        scriptObject1 = scriptObject1.GetParent();
                        continue;
                }
            }

            label_54:
            this.classwriter = scriptObject1 == null
                ? compilerGlobals.globalScopeClassWriter
                : compilerGlobals.classwriter;
            this.mb = this.classwriter.DefineMethod(this.name, this.attributes, type, parameterTypes);
        }

        if (num > 0)
        {
            if ((MethodInfo)this.mb != (MethodInfo)null)
            {
                this.mb.DefineParameter(1, ParameterAttributes.None, "this");
            }
            else
            {
                this.cb.DefineParameter(iSequence, ParameterAttributes.None, "this").SetConstant((object)null);
                num = 0;
                --iSequence;
            }
        }

        if (num > 1)
            this.mb.DefineParameter(2, ParameterAttributes.None, "vsa Engine");
        if (num > 2)
            this.mb.DefineParameter(3, ParameterAttributes.None, "arguments");
        for (int index = num; index < iSequence; ++index)
        {
            ParameterBuilder parameterBuilder = (MethodInfo)this.mb != (MethodInfo)null
                ? this.mb.DefineParameter(index + 1, ParameterAttributes.None,
                    this.parameter_declarations[index - num].identifier)
                : this.cb.DefineParameter(index + 1, ParameterAttributes.None,
                    this.parameter_declarations[index - num].identifier);
            CustomAttributeList customAttributes = this.parameter_declarations[index - num].customAttributes;
            if (customAttributes != null)
            {
                foreach (CustomAttributeBuilder attributeBuilder in customAttributes.GetCustomAttributeBuilders(false))
                    parameterBuilder.SetCustomAttribute(attributeBuilder);
            }
        }

        if (functionAttributeEnum > JSFunctionAttributeEnum.None)
        {
            CustomAttributeBuilder customBuilder = new CustomAttributeBuilder(
                CompilerGlobals.jsFunctionAttributeConstructor, new object[1]
                {
                    (object)functionAttributeEnum
                });
            if ((MethodInfo)this.mb != (MethodInfo)null)
                this.mb.SetCustomAttribute(customBuilder);
            else
                this.cb.SetCustomAttribute(customBuilder);
        }

        if (this.customAttributes != null)
        {
            CustomAttributeBuilder[] attributeBuilders = this.customAttributes.GetCustomAttributeBuilders(false);
            for (int index = 0; index < attributeBuilders.Length; ++index)
            {
                if ((MethodInfo)this.mb != (MethodInfo)null)
                    this.mb.SetCustomAttribute(attributeBuilders[index]);
                else
                    this.cb.SetCustomAttribute(attributeBuilders[index]);
            }
        }

        if (this.clsCompliance == CLSComplianceSpec.CLSCompliant)
        {
            if ((MethodInfo)this.mb != (MethodInfo)null)
                this.mb.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor,
                    new object[1]
                    {
                        (object)true
                    }));
            else
                this.cb.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor,
                    new object[1]
                    {
                        (object)true
                    }));
        }
        else if (this.clsCompliance == CLSComplianceSpec.NonCLSCompliant)
        {
            if ((MethodInfo)this.mb != (MethodInfo)null)
                this.mb.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor,
                    new object[1]
                    {
                        (object)false
                    }));
            else
                this.cb.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor,
                    new object[1]
                    {
                        (object)false
                    }));
        }

        if ((MethodInfo)this.mb != (MethodInfo)null)
        {
            this.mb.InitLocals = true;
            return (MethodBase)this.mb;
        }

        this.cb.InitLocals = true;
        return (MethodBase)this.cb;
    }

    private static bool IsPresentIn(FieldInfo field, FieldInfo[] fields)
    {
        int index = 0;
        for (int length = fields.Length; index < length; ++index)
        {
            if (field == fields[index])
                return true;
        }

        return false;
    }

    internal bool Must_save_stack_locals
    {
        get
        {
            if (!this.partiallyEvaluated)
                this.PartiallyEvaluate();
            return this.must_save_stack_locals;
        }
    }

    internal void PartiallyEvaluate()
    {
        if (this.partiallyEvaluated)
            return;
        if (this.enclosing_scope is ClassScope enclosingScope1)
            enclosingScope1.owner.PartiallyEvaluate();
        if (this.partiallyEvaluated)
            return;
        this.partiallyEvaluated = true;
        this.clsCompliance = CLSComplianceSpec.NotAttributed;
        if (this.customAttributes != null)
        {
            this.customAttributes.PartiallyEvaluate();
            CustomAttribute attribute1 = this.customAttributes.GetAttribute(Typeob.CLSCompliantAttribute);
            if (attribute1 != null)
            {
                this.clsCompliance = attribute1.GetCLSComplianceValue();
                this.customAttributes.Remove(attribute1);
            }

            CustomAttribute attribute2 = this.customAttributes.GetAttribute(Typeob.Override);
            if (attribute2 != null)
            {
                if (this.isStatic)
                    attribute2.context.HandleError(JSError.StaticMethodsCannotOverride);
                else
                    this.attributes &= ~MethodAttributes.VtableLayoutMask;
                this.noVersionSafeAttributeSpecified = false;
                this.customAttributes.Remove(attribute2);
            }

            CustomAttribute attribute3 = this.customAttributes.GetAttribute(Typeob.Hide);
            if (attribute3 != null)
            {
                if (!this.noVersionSafeAttributeSpecified)
                {
                    attribute3.context.HandleError(JSError.OverrideAndHideUsedTogether);
                    this.attributes |= MethodAttributes.VtableLayoutMask;
                    this.noVersionSafeAttributeSpecified = true;
                }
                else
                {
                    if (this.isStatic)
                        attribute3.context.HandleError(JSError.StaticMethodsCannotHide);
                    this.noVersionSafeAttributeSpecified = false;
                }

                this.customAttributes.Remove(attribute3);
            }

            CustomAttribute attribute4 = this.customAttributes.GetAttribute(Typeob.Expando);
            if (attribute4 != null)
            {
                if (!this.noVersionSafeAttributeSpecified && (this.attributes & MethodAttributes.VtableLayoutMask) ==
                    MethodAttributes.PrivateScope)
                {
                    attribute4.context.HandleError(JSError.ExpandoPrecludesOverride);
                    this.attributes |= MethodAttributes.VtableLayoutMask;
                    this.noVersionSafeAttributeSpecified = true;
                }

                if (this.isConstructor)
                    attribute4.context.HandleError(JSError.NotValidForConstructor);
                else if ((this.attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
                    attribute4.context.HandleError(JSError.ExpandoPrecludesAbstract);
                else if ((this.attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
                    attribute4.context.HandleError(JSError.ExpandoPrecludesStatic);
                else if ((this.attributes & MethodAttributes.MemberAccessMask) != MethodAttributes.Public)
                {
                    attribute4.context.HandleError(JSError.ExpandoMustBePublic);
                }
                else
                {
                    this.own_scope.isMethod = false;
                    this.isMethod = false;
                    this.isExpandoMethod = true;
                    this.isStatic = true;
                    this.attributes &= ~MethodAttributes.Virtual;
                    this.attributes &= ~MethodAttributes.VtableLayoutMask;
                    this.attributes |= MethodAttributes.Static;
                }
            }
        }

        int index1 = 0;
        for (int length = this.parameter_declarations.Length; index1 < length; ++index1)
        {
            this.parameter_declarations[index1].PartiallyEvaluate();
            JSLocalField jsLocalField = (JSLocalField)this.own_scope.name_table[(object)this.formal_parameters[index1]];
            jsLocalField.type = this.parameter_declarations[index1].type;
            if (jsLocalField.type == null)
                jsLocalField.type = new TypeExpression((AST)new ConstantWrapper((object)Typeob.Object,
                    this.parameter_declarations[index1].context));
            jsLocalField.isDefined = true;
        }

        if (this.return_type_expr != null)
        {
            this.return_type_expr.PartiallyEvaluate();
            this.own_scope.returnVar.type = this.return_type_expr;
            if ((object)this.own_scope.returnVar.type.ToIReflect() == (object)Typeob.Void)
            {
                this.own_scope.returnVar.type = (TypeExpression)null;
                this.own_scope.returnVar = (JSLocalField)null;
            }
        }

        this.globals.ScopeStack.Push((object)this.own_scope);
        if (!this.own_scope.isKnownAtCompileTime)
        {
            int index2 = 0;
            for (int length = this.fields.Length; index2 < length; ++index2)
                this.fields[index2].SetInferredType((IReflect)Typeob.Object, (AST)null);
        }

        if (!this.isConstructor)
        {
            this.body.PartiallyEvaluate();
        }
        else
        {
            this.body.MarkSuperOKIfIsFirstStatement();
            this.body.PartiallyEvaluate();
            ClassScope enclosingScope2 = (ClassScope)this.enclosing_scope;
            int count = this.superConstructorCall == null ? 0 : this.superConstructorCall.arguments.count;
            Type[] typeArray = count == 0 ? Type.EmptyTypes : new Type[count];
            IReflect[] argIRs = new IReflect[count];
            for (int i = 0; i < count; ++i)
                argIRs[i] = this.superConstructorCall.arguments[i].InferType((JSField)null);
            Context context = this.superConstructorCall == null ? this.funcContext : this.superConstructorCall.context;
            try
            {
                this.superConstructor =
                    this.superConstructorCall == null || this.superConstructorCall.isSuperConstructorCall
                        ? enclosingScope2.owner.GetSuperConstructor(argIRs)
                        : JSBinder.SelectConstructor((MemberInfo[])enclosingScope2.constructors, argIRs);
                if (this.superConstructor == (ConstructorInfo)null)
                {
                    context.HandleError(JSError.SuperClassConstructorNotAccessible);
                }
                else
                {
                    ConstructorInfo superConstructor = this.superConstructor;
                    if (!superConstructor.IsPublic && !superConstructor.IsFamily &&
                        !superConstructor.IsFamilyOrAssembly && (!(this.superConstructor is JSConstructor) ||
                                                                 !((JSConstructor)this.superConstructor)
                                                                     .IsAccessibleFrom(this.enclosing_scope)))
                    {
                        context.HandleError(JSError.SuperClassConstructorNotAccessible);
                        this.superConstructor = (ConstructorInfo)null;
                    }
                    else if (count > 0)
                    {
                        if (!Binding.CheckParameters(superConstructor.GetParameters(), argIRs,
                                this.superConstructorCall.arguments, this.superConstructorCall.context))
                            this.superConstructor = (ConstructorInfo)null;
                    }
                }
            }
            catch (AmbiguousMatchException ex)
            {
                context.HandleError(JSError.AmbiguousConstructorCall);
            }
        }

        this.own_scope.HandleUnitializedVariables();
        this.globals.ScopeStack.Pop();
        this.must_save_stack_locals = this.own_scope.mustSaveStackLocals;
        this.fields = this.own_scope.GetLocalFields();
    }

    internal IReflect ReturnType(JSField inference_target)
    {
        if (!this.partiallyEvaluated)
            this.PartiallyEvaluate();
        if ((FieldInfo)this.own_scope.returnVar == (FieldInfo)null)
            return (IReflect)Typeob.Void;
        return this.return_type_expr != null
            ? this.return_type_expr.ToIReflect()
            : this.own_scope.returnVar.GetInferredType(inference_target);
    }

    /// <summary>Returns a string representation of this function object.</summary>
    /// <returns>A string representation of this function object that contains the text of the function.</returns>
    public override string ToString() => this.text != null ? this.text : this.funcContext.GetCode();

    internal void TranslateBodyToIL(ILGenerator il, CompilerGlobals compilerGlobals)
    {
        this.returnLabel = il.DefineLabel();
        if (this.body.Engine.GenerateDebugInfo)
        {
            ScriptObject parent = this.enclosing_scope.GetParent();
            while (true)
            {
                switch (parent)
                {
                    case null:
                        goto label_7;
                    case PackageScope _:
                        il.UsingNamespace(((PackageScope)parent).name);
                        break;
                    case WrappedNamespace _:
                        if (!((WrappedNamespace)parent).name.Equals(""))
                        {
                            il.UsingNamespace(((WrappedNamespace)parent).name);
                            break;
                        }

                        break;
                }

                parent = parent.GetParent();
            }
        }

        label_7:
        int startLine = this.body.context.StartLine;
        int startColumn = this.body.context.StartColumn;
        this.body.context.document.EmitLineInfo(il, startLine, startColumn, startLine, startColumn + 1);
        if (this.body.context.document.debugOn)
            il.Emit(OpCodes.Nop);
        int length = this.fields.Length;
        for (int index = 0; index < length; ++index)
        {
            if (!this.fields[index].IsLiteral || this.fields[index].value is FunctionObject)
            {
                Type fieldType = this.fields[index].FieldType;
                LocalBuilder localBuilder = il.DeclareLocal(fieldType);
                if (this.fields[index].debugOn)
                    localBuilder.SetLocalSymInfo(this.fields[index].debuggerName);
                this.fields[index].metaData = (object)localBuilder;
            }
        }

        this.globals.ScopeStack.Push((object)this.own_scope);
        try
        {
            if (this.must_save_stack_locals)
            {
                this.TranslateToMethodWithStackFrame(il, compilerGlobals, true);
            }
            else
            {
                this.body.TranslateToILInitializer(il);
                this.body.TranslateToIL(il, Typeob.Void);
                il.MarkLabel(this.returnLabel);
            }
        }
        finally
        {
            this.globals.ScopeStack.Pop();
        }
    }

    internal void TranslateToIL(CompilerGlobals compilerGlobals)
    {
        if (this.suppressIL)
            return;
        this.globals.ScopeStack.Push((object)this.own_scope);
        try
        {
            if ((MethodInfo)this.mb == (MethodInfo)null && (ConstructorInfo)this.cb == (ConstructorInfo)null)
                this.GetMethodBase(compilerGlobals);
            int num1 = (this.attributes & MethodAttributes.Static) == MethodAttributes.Static ? 0 : 1;
            int num2 = 3;
            if (this.isMethod)
                num2 = 0;
            else if (!this.hasArgumentsObject)
                num2 = 2;
            ILGenerator ilGenerator = (MethodInfo)this.mb != (MethodInfo)null
                ? this.mb.GetILGenerator()
                : this.cb.GetILGenerator();
            this.returnLabel = ilGenerator.DefineLabel();
            if (this.body.Engine.GenerateDebugInfo)
            {
                ScriptObject parent = this.enclosing_scope.GetParent();
                while (true)
                {
                    switch (parent)
                    {
                        case null:
                            goto label_15;
                        case PackageScope _:
                            ilGenerator.UsingNamespace(((PackageScope)parent).name);
                            break;
                        case WrappedNamespace _:
                            if (!((WrappedNamespace)parent).name.Equals(""))
                            {
                                ilGenerator.UsingNamespace(((WrappedNamespace)parent).name);
                                break;
                            }

                            break;
                    }

                    parent = parent.GetParent();
                }
            }

            label_15:
            if (!this.isImplicitCtor && this.body != null)
            {
                int startLine = this.body.context.StartLine;
                int startColumn = this.body.context.StartColumn;
                this.body.context.document.EmitLineInfo(ilGenerator, startLine, startColumn, startLine,
                    startColumn + 1);
                if (this.body.context.document.debugOn)
                    ilGenerator.Emit(OpCodes.Nop);
            }

            int length = this.fields.Length;
            for (int index = 0; index < length; ++index)
            {
                int num3 = this.IsNestedFunctionField(this.fields[index])
                    ? -1
                    : Array.IndexOf<string>(this.formal_parameters, this.fields[index].Name);
                if (num3 >= 0)
                    this.fields[index].metaData = (object)(short)(num3 + num2 + num1);
                else if (this.hasArgumentsObject && this.fields[index].Name.Equals("arguments"))
                    this.fields[index].metaData = (object)(short)(2 + num1);
                else if (!this.fields[index].IsLiteral || this.fields[index].value is FunctionObject)
                {
                    Type fieldType = this.fields[index].FieldType;
                    LocalBuilder localBuilder = ilGenerator.DeclareLocal(fieldType);
                    if (this.fields[index].debugOn)
                        localBuilder.SetLocalSymInfo(this.fields[index].debuggerName);
                    this.fields[index].metaData = (object)localBuilder;
                }
                else if (this.own_scope.mustSaveStackLocals)
                {
                    LocalBuilder localBuilder = ilGenerator.DeclareLocal(this.fields[index].FieldType);
                    this.fields[index].metaData = (object)localBuilder;
                }
            }

            if (this.isConstructor)
            {
                int callerParameterCount = this.formal_parameters.Length + 1;
                ClassScope enclosingScope = (ClassScope)this.enclosing_scope;
                if (this.superConstructor == (ConstructorInfo)null)
                {
                    enclosingScope.owner.EmitInitialCalls(ilGenerator, (MethodBase)null, (ParameterInfo[])null,
                        (ASTList)null, 0);
                }
                else
                {
                    ParameterInfo[] parameters = this.superConstructor.GetParameters();
                    if (this.superConstructorCall != null)
                        enclosingScope.owner.EmitInitialCalls(ilGenerator, (MethodBase)this.superConstructor,
                            parameters, this.superConstructorCall.arguments, callerParameterCount);
                    else
                        enclosingScope.owner.EmitInitialCalls(ilGenerator, (MethodBase)this.superConstructor,
                            parameters, (ASTList)null, callerParameterCount);
                }
            }

            if ((this.isMethod || this.isConstructor) && this.must_save_stack_locals)
            {
                this.TranslateToMethodWithStackFrame(ilGenerator, compilerGlobals, false);
            }
            else
            {
                this.TranslateToILToCopyOuterScopeLocals(ilGenerator, true, (JSLocalField[])null);
                bool insideProtectedRegion = compilerGlobals.InsideProtectedRegion;
                compilerGlobals.InsideProtectedRegion = false;
                bool insideFinally = compilerGlobals.InsideFinally;
                int finallyStackTop = compilerGlobals.FinallyStackTop;
                compilerGlobals.InsideFinally = false;
                this.body.TranslateToILInitializer(ilGenerator);
                this.body.TranslateToIL(ilGenerator, Typeob.Void);
                compilerGlobals.InsideProtectedRegion = insideProtectedRegion;
                compilerGlobals.InsideFinally = insideFinally;
                compilerGlobals.FinallyStackTop = finallyStackTop;
                ilGenerator.MarkLabel(this.returnLabel);
                if (this.body.context.document.debugOn)
                {
                    this.EmitLastLineInfo(ilGenerator);
                    ilGenerator.Emit(OpCodes.Nop);
                }

                this.TranslateToILToSaveLocals(ilGenerator);
                if ((FieldInfo)this.own_scope.returnVar != (FieldInfo)null)
                    ilGenerator.Emit(OpCodes.Ldloc, (LocalBuilder)this.own_scope.returnVar.GetMetaData());
                ilGenerator.Emit(OpCodes.Ret);
            }
        }
        finally
        {
            this.globals.ScopeStack.Pop();
        }
    }

    private bool IsNestedFunctionField(JSLocalField field)
    {
        return field.value != null && field.value is FunctionObject;
    }

    internal void TranslateToILToLoadEngine(ILGenerator il)
    {
        this.TranslateToILToLoadEngine(il, false);
    }

    private void TranslateToILToLoadEngine(ILGenerator il, bool allocateLocal)
    {
        if (this.isMethod)
        {
            if (this.isStatic)
            {
                if (this.body.Engine.doCRS)
                {
                    il.Emit(OpCodes.Ldsfld, CompilerGlobals.contextEngineField);
                }
                else
                {
                    if (this.engineLocal == null)
                    {
                        if (allocateLocal)
                            this.engineLocal = il.DeclareLocal(Typeob.VsaEngine);
                        if (this.body.Engine.PEFileKind == PEFileKinds.Dll)
                        {
                            il.Emit(OpCodes.Ldtoken, (Type)((ClassScope)this.own_scope.GetParent()).GetTypeBuilder());
                            il.Emit(OpCodes.Call, CompilerGlobals.createVsaEngineWithType);
                        }
                        else
                            il.Emit(OpCodes.Call, CompilerGlobals.createVsaEngine);

                        if (!allocateLocal)
                            return;
                        il.Emit(OpCodes.Stloc, this.engineLocal);
                    }

                    il.Emit(OpCodes.Ldloc, this.engineLocal);
                }
            }
            else
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Callvirt, CompilerGlobals.getEngineMethod);
            }
        }
        else
            il.Emit(OpCodes.Ldarg_1);
    }

    private void TranslateToMethodWithStackFrame(
        ILGenerator il,
        CompilerGlobals compilerGlobals,
        bool staticInitializer)
    {
        if (this.isStatic)
            il.Emit(OpCodes.Ldtoken, (Type)((ClassScope)this.own_scope.GetParent()).GetTypeBuilder());
        else
            il.Emit(OpCodes.Ldarg_0);
        int length = this.fields.Length;
        ConstantWrapper.TranslateToILInt(il, length);
        il.Emit(OpCodes.Newarr, Typeob.JSLocalField);
        for (int i = 0; i < length; ++i)
        {
            JSLocalField field = this.fields[i];
            il.Emit(OpCodes.Dup);
            ConstantWrapper.TranslateToILInt(il, i);
            il.Emit(OpCodes.Ldstr, field.Name);
            il.Emit(OpCodes.Ldtoken, field.FieldType);
            ConstantWrapper.TranslateToILInt(il, field.slotNumber);
            il.Emit(OpCodes.Newobj, CompilerGlobals.jsLocalFieldConstructor);
            il.Emit(OpCodes.Stelem_Ref);
        }

        this.TranslateToILToLoadEngine(il, true);
        if (this.isStatic)
            il.Emit(OpCodes.Call, CompilerGlobals.pushStackFrameForStaticMethod);
        else
            il.Emit(OpCodes.Call, CompilerGlobals.pushStackFrameForMethod);
        bool insideProtectedRegion = compilerGlobals.InsideProtectedRegion;
        compilerGlobals.InsideProtectedRegion = true;
        il.BeginExceptionBlock();
        this.body.TranslateToILInitializer(il);
        this.body.TranslateToIL(il, Typeob.Void);
        il.MarkLabel(this.returnLabel);
        this.TranslateToILToSaveLocals(il);
        Label label = il.DefineLabel();
        il.Emit(OpCodes.Leave, label);
        il.BeginFinallyBlock();
        this.TranslateToILToLoadEngine(il);
        il.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
        il.Emit(OpCodes.Pop);
        il.EndExceptionBlock();
        il.MarkLabel(label);
        if (!staticInitializer)
        {
            if (this.body.context.document.debugOn)
            {
                this.EmitLastLineInfo(il);
                il.Emit(OpCodes.Nop);
            }

            if ((FieldInfo)this.own_scope.returnVar != (FieldInfo)null)
                il.Emit(OpCodes.Ldloc, (LocalBuilder)this.own_scope.returnVar.GetMetaData());
            il.Emit(OpCodes.Ret);
        }

        compilerGlobals.InsideProtectedRegion = insideProtectedRegion;
    }

    internal void TranslateToILToRestoreLocals(ILGenerator il)
    {
        this.TranslateToILToRestoreLocals(il, (JSLocalField[])null);
    }

    internal void TranslateToILToRestoreLocals(ILGenerator il, JSLocalField[] notToBeRestored)
    {
        this.TranslateToILToCopyOuterScopeLocals(il, true, notToBeRestored);
        if (!this.must_save_stack_locals)
            return;
        int num1 = (this.attributes & MethodAttributes.Static) == MethodAttributes.Static ? 0 : 1;
        int num2 = 3;
        if (this.isMethod)
            num2 = 0;
        else if (!this.hasArgumentsObject)
            num2 = 2;
        int length = this.fields.Length;
        this.TranslateToILToLoadEngine(il);
        il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
        ScriptObject scriptObject = this.globals.ScopeStack.Peek();
        while (true)
        {
            switch (scriptObject)
            {
                case WithObject _:
                case BlockScope _:
                    il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
                    scriptObject = scriptObject.GetParent();
                    continue;
                default:
                    goto label_8;
            }
        }

        label_8:
        il.Emit(OpCodes.Castclass, Typeob.StackFrame);
        il.Emit(OpCodes.Ldfld, CompilerGlobals.localVarsField);
        for (int index = 0; index < length; ++index)
        {
            if ((notToBeRestored == null ||
                 !FunctionObject.IsPresentIn((FieldInfo)this.fields[index], (FieldInfo[])notToBeRestored)) &&
                !this.fields[index].IsLiteral)
            {
                il.Emit(OpCodes.Dup);
                int num3 = Array.IndexOf<string>(this.formal_parameters, this.fields[index].Name);
                ConstantWrapper.TranslateToILInt(il, this.fields[index].slotNumber);
                il.Emit(OpCodes.Ldelem_Ref);
                Convert.Emit((AST)this.body, il, Typeob.Object, this.fields[index].FieldType);
                if (num3 >= 0 || this.fields[index].Name.Equals("arguments") && this.hasArgumentsObject)
                    il.Emit(OpCodes.Starg, (short)(num3 + num2 + num1));
                else
                    il.Emit(OpCodes.Stloc, (LocalBuilder)this.fields[index].metaData);
            }
        }

        il.Emit(OpCodes.Pop);
    }

    internal void TranslateToILToSaveLocals(ILGenerator il)
    {
        this.TranslateToILToCopyOuterScopeLocals(il, false, (JSLocalField[])null);
        if (!this.must_save_stack_locals)
            return;
        int num1 = (this.attributes & MethodAttributes.Static) == MethodAttributes.Static ? 0 : 1;
        int num2 = 3;
        if (this.isMethod)
            num2 = 0;
        else if (!this.hasArgumentsObject)
            num2 = 2;
        int length = this.fields.Length;
        this.TranslateToILToLoadEngine(il);
        il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
        ScriptObject scriptObject = this.globals.ScopeStack.Peek();
        while (true)
        {
            switch (scriptObject)
            {
                case WithObject _:
                case BlockScope _:
                    il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
                    scriptObject = scriptObject.GetParent();
                    continue;
                default:
                    goto label_8;
            }
        }

        label_8:
        il.Emit(OpCodes.Castclass, Typeob.StackFrame);
        il.Emit(OpCodes.Ldfld, CompilerGlobals.localVarsField);
        for (int index = 0; index < length; ++index)
        {
            JSLocalField field = this.fields[index];
            if (!field.IsLiteral || field.value is FunctionObject)
            {
                il.Emit(OpCodes.Dup);
                ConstantWrapper.TranslateToILInt(il, field.slotNumber);
                int num3 = Array.IndexOf<string>(this.formal_parameters, field.Name);
                if (num3 >= 0 || field.Name.Equals("arguments") && this.hasArgumentsObject)
                    Convert.EmitLdarg(il, (short)(num3 + num2 + num1));
                else
                    il.Emit(OpCodes.Ldloc, (LocalBuilder)field.metaData);
                Convert.Emit((AST)this.body, il, field.FieldType, Typeob.Object);
                il.Emit(OpCodes.Stelem_Ref);
            }
        }

        il.Emit(OpCodes.Pop);
    }

    private void TranslateToILToCopyOuterScopeLocals(
        ILGenerator il,
        bool copyToNested,
        JSLocalField[] notToBeRestored)
    {
        if (this.own_scope.ProvidesOuterScopeLocals == null || this.own_scope.ProvidesOuterScopeLocals.count == 0)
            return;
        this.TranslateToILToLoadEngine(il);
        il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
        ScriptObject scriptObject = this.globals.ScopeStack.Peek();
        while (true)
        {
            switch (scriptObject)
            {
                case WithObject _:
                case BlockScope _:
                    il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
                    scriptObject = scriptObject.GetParent();
                    continue;
                default:
                    goto label_4;
            }
        }

        label_4:
        for (ScriptObject key = this.enclosing_scope; key != null; key = key.GetParent())
        {
            il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
            if (key is FunctionScope && ((FunctionScope)key).owner != null &&
                this.own_scope.ProvidesOuterScopeLocals[(object)key] != null)
            {
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Castclass, Typeob.StackFrame);
                il.Emit(OpCodes.Ldfld, CompilerGlobals.localVarsField);
                if (copyToNested)
                    ((FunctionScope)key).owner.TranslateToILToCopyLocalsToNestedScope(il, this.own_scope,
                        notToBeRestored);
                else
                    ((FunctionScope)key).owner.TranslateToILToCopyLocalsFromNestedScope(il, this.own_scope);
            }
            else if (key is GlobalScope || key is ClassScope)
                break;
        }

        il.Emit(OpCodes.Pop);
    }

    private void TranslateToILToCopyLocalsToNestedScope(
        ILGenerator il,
        FunctionScope nestedScope,
        JSLocalField[] notToBeRestored)
    {
        int num = (this.attributes & MethodAttributes.Static) == MethodAttributes.Static ? 0 : 1;
        int length = this.fields.Length;
        for (int index = 0; index < length; ++index)
        {
            JSLocalField outerLocalField = nestedScope.GetOuterLocalField(this.fields[index].Name);
            if (!((FieldInfo)outerLocalField == (FieldInfo)null) &&
                !((FieldInfo)outerLocalField.outerField != (FieldInfo)this.fields[index]) && (notToBeRestored == null ||
                    !FunctionObject.IsPresentIn((FieldInfo)outerLocalField, (FieldInfo[])notToBeRestored)))
            {
                il.Emit(OpCodes.Dup);
                ConstantWrapper.TranslateToILInt(il, this.fields[index].slotNumber);
                il.Emit(OpCodes.Ldelem_Ref);
                Convert.Emit((AST)this.body, il, Typeob.Object, this.fields[index].FieldType);
                il.Emit(OpCodes.Stloc, (LocalBuilder)outerLocalField.metaData);
            }
        }

        il.Emit(OpCodes.Pop);
    }

    private void TranslateToILToCopyLocalsFromNestedScope(ILGenerator il, FunctionScope nestedScope)
    {
        int num = (this.attributes & MethodAttributes.Static) == MethodAttributes.Static ? 0 : 1;
        int length = this.fields.Length;
        for (int index = 0; index < length; ++index)
        {
            JSLocalField outerLocalField = nestedScope.GetOuterLocalField(this.fields[index].Name);
            if (!((FieldInfo)outerLocalField == (FieldInfo)null) &&
                !((FieldInfo)outerLocalField.outerField != (FieldInfo)this.fields[index]))
            {
                il.Emit(OpCodes.Dup);
                ConstantWrapper.TranslateToILInt(il, this.fields[index].slotNumber);
                il.Emit(OpCodes.Ldloc, (LocalBuilder)outerLocalField.metaData);
                Convert.Emit((AST)this.body, il, this.fields[index].FieldType, Typeob.Object);
                il.Emit(OpCodes.Stelem_Ref);
            }
        }

        il.Emit(OpCodes.Pop);
    }
}