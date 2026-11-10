// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Binding
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a reference to a class member, callable expression, or variable name. This class attempts to resolve the reference to the correct value at compile time. </summary>
public abstract class Binding : AST
{
    private IReflect[] argIRs;

    /// <summary>Stores a reference to a default indexed property.</summary>
    protected MemberInfo defaultMember;

    private IReflect defaultMemberReturnIR;
    private bool isArrayElementAccess;
    private bool isArrayConstructor;

    /// <summary>Stores a value that indicates whether the binding reference is to a default indexed property.</summary>
    protected bool isAssignmentToDefaultIndexedProperty;

    /// <summary>Stores a value that indicates whether the binding reference is fully resolved.</summary>
    protected bool isFullyResolved;

    /// <summary>Stores a value that indicates whether the binding reference is to a member that overrides a member in a base class.</summary>
    protected bool isNonVirtual;

    internal MemberInfo[] members;
    internal MemberInfo member;

    /// <summary>Stores a string that identifies the binding reference.</summary>
    protected string name;

    private bool giveErrors;

    internal static ConstantWrapper ReflectionMissingCW =
        new ConstantWrapper((object)System.Reflection.Missing.Value, (Context)null);

    private static ConstantWrapper JScriptMissingCW = new ConstantWrapper((object)Missing.Value, (Context)null);

    internal Binding(Context context, string name)
        : base(context)
    {
        this.argIRs = (IReflect[])null;
        this.defaultMember = (MemberInfo)null;
        this.defaultMemberReturnIR = (IReflect)null;
        this.isArrayElementAccess = false;
        this.isArrayConstructor = false;
        this.isAssignmentToDefaultIndexedProperty = false;
        this.isFullyResolved = true;
        this.isNonVirtual = false;
        this.members = (MemberInfo[])null;
        this.member = (MemberInfo)null;
        this.name = name;
        this.giveErrors = true;
    }

    private bool Accessible(bool checkSetter)
    {
        if (this.member == (MemberInfo)null)
            return false;
        switch (this.member.MemberType)
        {
            case MemberTypes.Constructor:
                return this.AccessibleConstructor();
            case MemberTypes.Event:
                return false;
            case MemberTypes.Field:
                return this.AccessibleField(checkSetter);
            case MemberTypes.Method:
                return this.AccessibleMethod();
            case MemberTypes.Property:
                return this.AccessibleProperty(checkSetter);
            case MemberTypes.TypeInfo:
                if (!((Type)this.member).IsPublic)
                {
                    if (this.giveErrors)
                        this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                    return false;
                }

                return !checkSetter;
            case MemberTypes.NestedType:
                if (!((Type)this.member).IsNestedPublic)
                {
                    if (this.giveErrors)
                        this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                    return false;
                }

                return !checkSetter;
            default:
                return false;
        }
    }

    private bool AccessibleConstructor()
    {
        ConstructorInfo member = (ConstructorInfo)this.member;
        if (member is JSConstructor && ((JSConstructor)this.member).GetClassScope().owner.isAbstract ||
            !(member is JSConstructor) && member.DeclaringType.IsAbstract)
        {
            this.context.HandleError(JSError.CannotInstantiateAbstractClass);
            return false;
        }

        if (member.IsPublic || member is JSConstructor &&
            ((JSConstructor)member).IsAccessibleFrom(this.Globals.ScopeStack.Peek()))
            return true;
        if (this.giveErrors)
            this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
        return false;
    }

    private bool AccessibleField(bool checkWritable)
    {
        FieldInfo fieldInfo = (FieldInfo)this.member;
        if (checkWritable && (fieldInfo.IsInitOnly || fieldInfo.IsLiteral))
            return false;
        if (!fieldInfo.IsPublic)
        {
            JSWrappedField jsWrappedField = fieldInfo as JSWrappedField;
            if ((FieldInfo)jsWrappedField != (FieldInfo)null)
                this.member = (MemberInfo)(fieldInfo = jsWrappedField.wrappedField);
            JSClosureField jsClosureField = fieldInfo as JSClosureField;
            JSMemberField jsMemberField =
                ((FieldInfo)jsClosureField != (FieldInfo)null ? jsClosureField.field : fieldInfo) as JSMemberField;
            if ((FieldInfo)jsMemberField == (FieldInfo)null)
            {
                if (!fieldInfo.IsFamily && !fieldInfo.IsFamilyOrAssembly ||
                    !Binding.InsideClassThatExtends(this.Globals.ScopeStack.Peek(), fieldInfo.ReflectedType))
                {
                    if (this.giveErrors)
                        this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                    return false;
                }
            }
            else if (!jsMemberField.IsAccessibleFrom(this.Globals.ScopeStack.Peek()))
            {
                if (this.giveErrors)
                    this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                return false;
            }
        }

        if (fieldInfo.IsLiteral && fieldInfo is JSVariableField &&
            ((JSVariableField)fieldInfo).value is ClassScope classScope && !classScope.owner.IsStatic)
        {
            if (!(this is Lookup lookup) || !lookup.InStaticCode() || lookup.InFunctionNestedInsideInstanceMethod() ||
                !this.giveErrors)
                return true;
            this.context.HandleError(JSError.InstanceNotAccessibleFromStatic, this.isFullyResolved);
            return true;
        }

        if (fieldInfo.IsStatic || fieldInfo.IsLiteral || this.defaultMember != (MemberInfo)null || !(this is Lookup) ||
            !((Lookup)this).InStaticCode() ||
            fieldInfo is JSWrappedField && fieldInfo.DeclaringType == Typeob.LenientGlobalObject)
            return true;
        if (this.giveErrors)
        {
            if (!fieldInfo.IsStatic && this is Lookup && ((Lookup)this).InStaticCode())
                this.context.HandleError(JSError.InstanceNotAccessibleFromStatic, this.isFullyResolved);
            else
                this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
        }

        return false;
    }

    private bool AccessibleMethod() => this.AccessibleMethod((MethodInfo)this.member);

    private bool AccessibleMethod(MethodInfo meth)
    {
        if (meth == (MethodInfo)null)
            return false;
        if (this.isNonVirtual && meth.IsAbstract)
        {
            this.context.HandleError(JSError.InvalidCall);
            return false;
        }

        if (!meth.IsPublic)
        {
            JSWrappedMethod jsWrappedMethod = meth as JSWrappedMethod;
            if ((MethodInfo)jsWrappedMethod != (MethodInfo)null)
                meth = jsWrappedMethod.method;
            JSClosureMethod jsClosureMethod = meth as JSClosureMethod;
            JSFieldMethod jsFieldMethod =
                ((MethodInfo)jsClosureMethod != (MethodInfo)null ? jsClosureMethod.method : meth) as JSFieldMethod;
            if ((MethodInfo)jsFieldMethod == (MethodInfo)null)
            {
                if ((meth.IsFamily || meth.IsFamilyOrAssembly) &&
                    Binding.InsideClassThatExtends(this.Globals.ScopeStack.Peek(), meth.ReflectedType))
                    return true;
                if (this.giveErrors)
                    this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                return false;
            }

            if (!jsFieldMethod.IsAccessibleFrom(this.Globals.ScopeStack.Peek()))
            {
                if (this.giveErrors)
                    this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
                return false;
            }
        }

        if (meth.IsStatic || this.defaultMember != (MemberInfo)null || !(this is Lookup) ||
            !((Lookup)this).InStaticCode() || meth is JSWrappedMethod &&
            ((Lookup)this).CanPlaceAppropriateObjectOnStack(((JSMethod)meth).obj))
            return true;
        if (this.giveErrors)
        {
            if (!meth.IsStatic && this is Lookup && ((Lookup)this).InStaticCode())
                this.context.HandleError(JSError.InstanceNotAccessibleFromStatic, this.isFullyResolved);
            else
                this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
        }

        return false;
    }

    private bool AccessibleProperty(bool checkSetter)
    {
        PropertyInfo member = (PropertyInfo)this.member;
        if (this.AccessibleMethod(checkSetter
                ? JSProperty.GetSetMethod(member, true)
                : JSProperty.GetGetMethod(member, true)))
            return true;
        if (this.giveErrors && !checkSetter)
            this.context.HandleError(JSError.WriteOnlyProperty);
        return false;
    }

    internal static bool AssignmentCompatible(
        IReflect lhir,
        AST rhexpr,
        IReflect rhir,
        bool reportError)
    {
        if (rhexpr is ConstantWrapper)
        {
            object obj = rhexpr.Evaluate();
            if (obj is ClassScope)
            {
                if ((object)lhir == (object)Typeob.Type || (object)lhir == (object)Typeob.Object ||
                    (object)lhir == (object)Typeob.String)
                    return true;
                if (reportError)
                    rhexpr.context.HandleError(JSError.TypeMismatch);
                return false;
            }

            if (lhir is ClassScope classScope)
            {
                if (classScope.owner is EnumDeclaration owner)
                {
                    if (rhexpr is ConstantWrapper constantWrapper && constantWrapper.value is string)
                    {
                        FieldInfo field = classScope.GetField((string)constantWrapper.value,
                            BindingFlags.Static | BindingFlags.Public);
                        if (field == (FieldInfo)null)
                            return false;
                        owner.PartiallyEvaluate();
                        constantWrapper.value =
                            (object)new DeclaredEnumValue(((JSVariableField)field).value, field.Name, classScope);
                    }

                    if ((object)rhir == (object)Typeob.String)
                        return true;
                    lhir = (IReflect)owner.baseType.ToType();
                }
            }
            else if ((object)(lhir as Type) != null)
            {
                Type enumType = lhir as Type;
                if (enumType.IsEnum)
                {
                    if (rhexpr is ConstantWrapper constantWrapper && constantWrapper.value is string)
                    {
                        FieldInfo field = enumType.GetField((string)constantWrapper.value,
                            BindingFlags.Static | BindingFlags.Public);
                        if (field == (FieldInfo)null)
                            return false;
                        constantWrapper.value =
                            MetadataEnumValue.GetEnumValue(field.FieldType, field.GetRawConstantValue());
                    }

                    if ((object)rhir == (object)Typeob.String)
                        return true;
                    lhir = (IReflect)Enum.GetUnderlyingType(enumType);
                }
            }

            if ((object)(lhir as Type) != null)
            {
                try
                {
                    Convert.CoerceT(obj, (Type)lhir);
                    return true;
                }
                catch
                {
                    if ((object)lhir == (object)Typeob.Single && obj is double)
                    {
                        if (((ConstantWrapper)rhexpr).isNumericLiteral)
                            return true;
                        double num1 = (double)obj;
                        float num2 = (float)num1;
                        if (num1.ToString((IFormatProvider)CultureInfo.InvariantCulture)
                            .Equals(num2.ToString((IFormatProvider)CultureInfo.InvariantCulture)))
                        {
                            ((ConstantWrapper)rhexpr).value = (object)num2;
                            return true;
                        }
                    }

                    if ((object)lhir == (object)Typeob.Decimal && rhexpr is ConstantWrapper constantWrapper)
                    {
                        if (constantWrapper.isNumericLiteral)
                        {
                            try
                            {
                                Convert.CoerceT((object)constantWrapper.context.GetCode(), Typeob.Decimal);
                                return true;
                            }
                            catch
                            {
                            }
                        }
                    }

                    if (reportError)
                        rhexpr.context.HandleError(JSError.TypeMismatch);
                }

                return false;
            }
        }
        else if (rhexpr is ArrayLiteral)
            return ((ArrayLiteral)rhexpr).AssignmentCompatible(lhir, reportError);

        if ((object)rhir == (object)Typeob.Object ||
            (object)rhir == (object)Typeob.Double && Convert.IsPrimitiveNumericType(lhir) ||
            (object)(lhir as Type) != null && Typeob.Delegate.IsAssignableFrom((Type)lhir) &&
            (object)rhir == (object)Typeob.ScriptFunction && rhexpr is Binding &&
            ((Binding)rhexpr).IsCompatibleWithDelegate((Type)lhir) || Convert.IsPromotableTo(rhir, lhir) ||
            Convert.IsJScriptArray(rhir) && Binding.ArrayAssignmentCompatible(rhexpr, lhir) ||
            (object)lhir == (object)Typeob.String)
            return true;
        if ((object)rhir == (object)Typeob.String &&
            ((object)lhir == (object)Typeob.Boolean || Convert.IsPrimitiveNumericType(lhir)))
        {
            if (reportError)
                rhexpr.context.HandleError(JSError.PossibleBadConversionFromString);
            return true;
        }

        if ((object)lhir == (object)Typeob.Char && (object)rhir == (object)Typeob.String ||
            Convert.IsPromotableTo(lhir, rhir) ||
            Convert.IsPrimitiveNumericType(lhir) && Convert.IsPrimitiveNumericType(rhir))
        {
            if (reportError)
                rhexpr.context.HandleError(JSError.PossibleBadConversion);
            return true;
        }

        if (reportError)
            rhexpr.context.HandleError(JSError.TypeMismatch);
        return false;
    }

    private static bool ArrayAssignmentCompatible(AST ast, IReflect lhir)
    {
        if (!Convert.IsArray(lhir))
            return false;
        if ((object)lhir == (object)Typeob.Array)
        {
            ast.context.HandleError(JSError.ArrayMayBeCopied);
            return true;
        }

        if (Convert.GetArrayRank(lhir) != 1)
            return false;
        ast.context.HandleError(JSError.ArrayMayBeCopied);
        return true;
    }

    internal void CheckIfDeletable()
    {
        if (this.member != (MemberInfo)null || this.defaultMember != (MemberInfo)null)
            this.context.HandleError(JSError.NotDeletable);
        this.member = (MemberInfo)null;
        this.defaultMember = (MemberInfo)null;
    }

    internal void CheckIfUseless()
    {
        if (this.members == null || this.members.Length == 0)
            return;
        this.context.HandleError(JSError.UselessExpression);
    }

    internal static bool CheckParameters(
        ParameterInfo[] pars,
        IReflect[] argIRs,
        ASTList argAST,
        Context ctx)
    {
        return Binding.CheckParameters(pars, argIRs, argAST, ctx, 0, false, true);
    }

    internal static bool CheckParameters(
        ParameterInfo[] pars,
        IReflect[] argIRs,
        ASTList argAST,
        Context ctx,
        int offset,
        bool defaultIsUndefined,
        bool reportError)
    {
        int num = argIRs.Length;
        int length1 = pars.Length;
        bool flag = false;
        if (num > length1 - offset)
        {
            num = length1 - offset;
            flag = true;
        }

        for (int i1 = 0; i1 < num; ++i1)
        {
            IReflect reflect = pars[i1 + offset] is ParameterDeclaration
                ? ((ParameterDeclaration)pars[i1 + offset]).ParameterIReflect
                : (IReflect)pars[i1 + offset].ParameterType;
            IReflect argIr = argIRs[i1];
            if (i1 == num - 1 &&
                ((object)(reflect as Type) != null && Typeob.Array.IsAssignableFrom((Type)reflect) ||
                 reflect is TypedArray) &&
                CustomAttribute.IsDefined(pars[i1 + offset], typeof(ParamArrayAttribute), false))
            {
                int length2 = argIRs.Length;
                if (i1 == length2 - 1 && Binding.AssignmentCompatible(reflect, argAST[i1], argIRs[i1], false))
                    return true;
                IReflect lhir = (object)(reflect as Type) != null
                    ? (IReflect)((Type)reflect).GetElementType()
                    : ((TypedArray)reflect).elementType;
                for (int i2 = i1; i2 < length2; ++i2)
                {
                    if (!Binding.AssignmentCompatible(lhir, argAST[i2], argIRs[i2], reportError))
                        return false;
                }

                return true;
            }

            if (!Binding.AssignmentCompatible(reflect, argAST[i1], argIr, reportError))
                return false;
        }

        if (flag & reportError)
            ctx.HandleError(JSError.TooManyParameters);
        if (offset == 0 && num < length1 && !defaultIsUndefined)
        {
            for (int index = num; index < length1; ++index)
            {
                if (TypeReferences.GetDefaultParameterValue(pars[index]) == System.Convert.DBNull)
                {
                    if (pars[index] is ParameterDeclaration par)
                        par.PartiallyEvaluate();
                    if (index < length1 - 1 ||
                        !CustomAttribute.IsDefined(pars[index], typeof(ParamArrayAttribute), false))
                    {
                        if (reportError)
                            ctx.HandleError(JSError.TooFewParameters);
                        Type type = (pars[index + offset] is ParameterDeclaration
                            ? ((ParameterDeclaration)pars[index + offset]).ParameterIReflect
                            : (IReflect)pars[index + offset].ParameterType) as Type;
                        if (type != (Type)null && type.IsValueType && !type.IsPrimitive && !type.IsEnum)
                            return false;
                    }
                }
            }
        }

        return true;
    }

    internal override bool Delete() => this.EvaluateAsLateBinding().Delete();

    internal override object Evaluate()
    {
        object thisob = this.GetObject();
        MemberInfo member1 = this.member;
        if (member1 != (MemberInfo)null)
        {
            switch (member1.MemberType)
            {
                case MemberTypes.Event:
                    return (object)null;
                case MemberTypes.Field:
                    return ((FieldInfo)member1).GetValue(thisob);
                case MemberTypes.Property:
                    return LateBinding.CallOneOfTheMembers(new MemberInfo[1]
                    {
                        (MemberInfo)JSProperty.GetGetMethod((PropertyInfo)member1, false)
                    }, new object[0], false, thisob, (Binder)null, (CultureInfo)null, (string[])null, this.Engine);
                case MemberTypes.NestedType:
                    return (object)member1;
            }
        }

        if (this.members == null || this.members.Length == 0)
            return this.EvaluateAsLateBinding().GetValue();
        if (this.members.Length == 1 && this.members[0].MemberType == MemberTypes.Method)
        {
            MethodInfo member2 = (MethodInfo)this.members[0];
            Type declaringType = member2 is JSMethod ? (Type)null : member2.DeclaringType;
            if (declaringType == Typeob.GlobalObject || declaringType != (Type)null &&
                declaringType != Typeob.StringObject && declaringType != Typeob.NumberObject &&
                declaringType != Typeob.BooleanObject && declaringType.IsSubclassOf(Typeob.JSObject))
                return (object)Globals.BuiltinFunctionFor(thisob, TypeReferences.ToExecutionContext(member2));
        }

        return (object)new FunctionWrapper(this.name, thisob, this.members);
    }

    private MemberInfoList GetAllKnownInstanceBindingsForThisName()
    {
        IReflect[] allEligibleClasses = this.GetAllEligibleClasses();
        MemberInfoList bindingsForThisName = new MemberInfoList();
        foreach (IReflect reflect in allEligibleClasses)
        {
            if (reflect is ClassScope)
            {
                if (((ClassScope)reflect).ParentIsInSamePackage())
                    bindingsForThisName.AddRange(reflect.GetMember(this.name,
                        BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public |
                        BindingFlags.NonPublic));
                else
                    bindingsForThisName.AddRange(reflect.GetMember(this.name,
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
            }
            else
                bindingsForThisName.AddRange(reflect.GetMember(this.name, BindingFlags.Instance | BindingFlags.Public));
        }

        return bindingsForThisName;
    }

    private IReflect[] GetAllEligibleClasses()
    {
        ArrayList result = new ArrayList(16 /*0x10*/);
        ClassScope excludedClass = (ClassScope)null;
        PackageScope packageScope = (PackageScope)null;
        ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
        while (scriptObject is WithObject || scriptObject is BlockScope)
            scriptObject = scriptObject.GetParent();
        if (scriptObject is FunctionScope)
            scriptObject = ((FunctionScope)scriptObject).owner.enclosing_scope;
        if (scriptObject is ClassScope)
        {
            excludedClass = (ClassScope)scriptObject;
            packageScope = excludedClass.package;
        }

        excludedClass?.AddClassesFromInheritanceChain(this.name, result);
        if (packageScope != null)
            packageScope.AddClassesExcluding(excludedClass, this.name, result);
        else
            ((IActivationObject)scriptObject).GetGlobalScope().AddClassesExcluding(excludedClass, this.name, result);
        IReflect[] allEligibleClasses = new IReflect[result.Count];
        result.CopyTo((Array)allEligibleClasses);
        return allEligibleClasses;
    }

    /// <summary>When overridden in a derived class, gets the object that the binding reference is defined on.</summary>
    /// <returns>The object that the binding reference is defined on.</returns>
    protected abstract object GetObject();

    /// <summary>When overridden in a derived class, handles situations in which the binding reference is to a member that cannot be found.</summary>
    protected abstract void HandleNoSuchMemberError();

    internal override IReflect InferType(JSField inference_target)
    {
        if (this.isArrayElementAccess)
        {
            IReflect defaultMemberReturnIr = this.defaultMemberReturnIR;
            return !(defaultMemberReturnIr is TypedArray)
                ? (IReflect)((Type)defaultMemberReturnIr).GetElementType()
                : ((TypedArray)defaultMemberReturnIr).elementType;
        }

        if (this.isAssignmentToDefaultIndexedProperty)
            return (object)(this.member as PropertyInfo) != null
                ? (IReflect)((PropertyInfo)this.member).PropertyType
                : (IReflect)Typeob.Object;
        MemberInfo memberInfo = this.member;
        if ((object)(memberInfo as FieldInfo) != null)
        {
            JSWrappedField jsWrappedField = memberInfo as JSWrappedField;
            if ((FieldInfo)jsWrappedField != (FieldInfo)null)
                memberInfo = (MemberInfo)jsWrappedField.wrappedField;
            return memberInfo is JSVariableField
                ? ((JSVariableField)memberInfo).GetInferredType(inference_target)
                : (IReflect)((FieldInfo)memberInfo).FieldType;
        }

        if ((object)(memberInfo as PropertyInfo) != null)
        {
            JSWrappedProperty jsWrappedProperty = memberInfo as JSWrappedProperty;
            if ((PropertyInfo)jsWrappedProperty != (PropertyInfo)null)
                memberInfo = (MemberInfo)jsWrappedProperty.property;
            if (memberInfo is JSProperty)
                return ((JSProperty)memberInfo).PropertyIR();
            PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
            return propertyInfo.DeclaringType == Typeob.GlobalObject
                ? (IReflect)propertyInfo.GetValue((object)this.Globals.globalObject, (object[])null)
                : (IReflect)propertyInfo.PropertyType;
        }

        if ((object)(memberInfo as Type) != null)
            return (IReflect)Typeob.Type;
        if ((object)(memberInfo as EventInfo) != null)
            return (IReflect)Typeob.EventInfo;
        return this.members.Length != 0 && this.Engine.doFast
            ? (IReflect)Typeob.ScriptFunction
            : (IReflect)Typeob.Object;
    }

    internal virtual IReflect InferTypeOfCall(JSField inference_target, bool isConstructor)
    {
        if (!this.isFullyResolved)
            return (IReflect)Typeob.Object;
        if (this.isArrayConstructor)
            return this.defaultMemberReturnIR;
        if (this.isArrayElementAccess)
        {
            IReflect defaultMemberReturnIr = this.defaultMemberReturnIR;
            return !(defaultMemberReturnIr is TypedArray)
                ? (IReflect)((Type)defaultMemberReturnIr).GetElementType()
                : ((TypedArray)defaultMemberReturnIr).elementType;
        }

        MemberInfo member = this.member;
        if (member is JSFieldMethod)
            return !isConstructor ? ((JSFieldMethod)member).ReturnIR() : (IReflect)Typeob.Object;
        if ((object)(member as MethodInfo) != null)
            return (IReflect)((MethodInfo)member).ReturnType;
        if (member is JSConstructor)
            return (IReflect)((JSConstructor)member).GetClassScope();
        if ((object)(member as ConstructorInfo) != null)
            return (IReflect)member.DeclaringType;
        if ((object)(member as Type) != null)
            return (IReflect)(Type)member;
        if ((object)(member as FieldInfo) != null && ((FieldInfo)member).IsLiteral)
        {
            object obj = member is JSVariableField
                ? ((JSVariableField)member).value
                : TypeReferences.GetConstantValue((FieldInfo)member);
            switch (obj)
            {
                case ClassScope _:
                case TypedArray _:
                    return (IReflect)obj;
            }
        }

        return (IReflect)Typeob.Object;
    }

    private static bool InsideClassThatExtends(ScriptObject scope, Type type)
    {
        while (true)
        {
            switch (scope)
            {
                case WithObject _:
                case BlockScope _:
                    scope = scope.GetParent();
                    continue;
                case ClassScope _:
                    goto label_3;
                case FunctionScope _:
                    goto label_4;
                default:
                    goto label_5;
            }
        }

        label_3:
        return type.IsAssignableFrom(((ClassScope)scope).GetBakedSuperType());
        label_4:
        return Binding.InsideClassThatExtends(((FunctionScope)scope).owner.enclosing_scope, type);
        label_5:
        return false;
    }

    internal void InvalidateBinding()
    {
        this.isAssignmentToDefaultIndexedProperty = false;
        this.isArrayConstructor = false;
        this.isArrayElementAccess = false;
        this.defaultMember = (MemberInfo)null;
        this.member = (MemberInfo)null;
        this.members = new MemberInfo[0];
    }

    internal bool IsCompatibleWithDelegate(Type delegateType)
    {
        MethodInfo method = delegateType.GetMethod("Invoke");
        ParameterInfo[] parameters = method.GetParameters();
        Type returnType = method.ReturnType;
        foreach (MemberInfo member in this.members)
        {
            if ((object)(member as MethodInfo) != null)
            {
                MethodInfo methodInfo = (MethodInfo)member;
                Type type;
                if (methodInfo is JSFieldMethod)
                {
                    IReflect ir = ((JSFieldMethod)methodInfo).ReturnIR();
                    type = !(ir is ClassScope)
                        ? ((object)(ir as Type) == null ? Convert.ToType(ir) : (Type)ir)
                        : ((ClassScope)ir).GetBakedSuperType();
                    if (((JSFieldMethod)methodInfo).func.isExpandoMethod)
                        return false;
                }
                else
                    type = methodInfo.ReturnType;

                if (type == returnType && Class.ParametersMatch(parameters, methodInfo.GetParameters()))
                {
                    this.member = (MemberInfo)methodInfo;
                    this.isFullyResolved = true;
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>Determines whether the specified object is equivalent to <see cref="T:Microsoft.JScript.Missing" />.</summary>
    /// <returns>true if <paramref name="value" /> is equivalent to <see cref="T:Microsoft.JScript.Missing" />; otherwise, false.</returns>
    /// <param name="value">The object to compare.</param>
    public static bool IsMissing(object value) => value is Missing;

    private MethodInfo LookForParameterlessPropertyGetter()
    {
        int index = 0;
        for (int length = this.members.Length; index < length; ++index)
        {
            PropertyInfo member = this.members[index] as PropertyInfo;
            if (member != (PropertyInfo)null)
            {
                MethodInfo getMethod = member.GetGetMethod(true);
                if (!(getMethod == (MethodInfo)null))
                {
                    ParameterInfo[] parameters = getMethod.GetParameters();
                    if (parameters == null || parameters.Length == 0)
                        continue;
                }
                else
                    continue;
            }

            return (MethodInfo)null;
        }

        try
        {
            MethodInfo methodInfo = JSBinder.SelectMethod(this.members, new IReflect[0]);
            if (methodInfo != (MethodInfo)null)
            {
                if (methodInfo.IsSpecialName)
                    return methodInfo;
            }
        }
        catch (AmbiguousMatchException ex)
        {
        }

        return (MethodInfo)null;
    }

    internal override bool OkToUseAsType()
    {
        MemberInfo member = this.member;
        if ((object)(member as Type) != null)
            return this.isFullyResolved = true;
        if ((object)(member as FieldInfo) != null)
        {
            FieldInfo fieldInfo = (FieldInfo)member;
            if (fieldInfo.IsLiteral)
                return (!(fieldInfo is JSMemberField) || !(((JSVariableField)fieldInfo).value is ClassScope) ||
                        fieldInfo.IsStatic) && (this.isFullyResolved = true);
            if (!(member is JSField) && fieldInfo.IsStatic &&
                (object)(fieldInfo.GetValue((object)null) as Type) != null)
                return this.isFullyResolved = true;
        }

        return false;
    }

    private int PlaceValuesForHiddenParametersOnStack(
        ILGenerator il,
        MethodInfo meth,
        ParameterInfo[] pars)
    {
        int num = 0;
        if (meth is JSFieldMethod)
        {
            FunctionObject func = ((JSFieldMethod)meth).func;
            if (func != null && func.isMethod)
                return 0;
            if (this is Lookup)
                ((Lookup)this).TranslateToILDefaultThisObject(il);
            else
                this.TranslateToILObject(il, Typeob.Object, false);
            this.EmitILToLoadEngine(il);
            return 0;
        }

        object[] customAttributes =
            CustomAttribute.GetCustomAttributes((MemberInfo)meth, typeof(JSFunctionAttribute), false);
        if (customAttributes == null || customAttributes.Length == 0)
            return 0;
        JSFunctionAttributeEnum attributeValue = ((JSFunctionAttribute)customAttributes[0]).attributeValue;
        if ((attributeValue & JSFunctionAttributeEnum.HasThisObject) != JSFunctionAttributeEnum.None)
        {
            num = 1;
            Type parameterType = pars[0].ParameterType;
            if (this is Lookup && parameterType == Typeob.Object)
                ((Lookup)this).TranslateToILDefaultThisObject(il);
            else if (Typeob.ArrayObject.IsAssignableFrom(this.member.DeclaringType))
                this.TranslateToILObject(il, Typeob.ArrayObject, false);
            else
                this.TranslateToILObject(il, parameterType, false);
        }

        if ((attributeValue & JSFunctionAttributeEnum.HasEngine) != JSFunctionAttributeEnum.None)
        {
            ++num;
            this.EmitILToLoadEngine(il);
        }

        return num;
    }

    private bool ParameterlessPropertyValueIsCallable(
        MethodInfo meth,
        ASTList args,
        IReflect[] argIRs,
        bool constructor,
        bool brackets)
    {
        ParameterInfo[] parameters = meth.GetParameters();
        if (parameters == null || parameters.Length == 0)
        {
            if (meth is JSWrappedMethod && ((JSWrappedMethod)meth).GetWrappedObject() is GlobalObject ||
                argIRs.Length != 0 || !(meth is JSMethod) && Typeob.ScriptFunction.IsAssignableFrom(meth.ReturnType))
            {
                this.member = this.ResolveOtherKindOfCall(args, argIRs, constructor, brackets);
                return true;
            }

            IReflect reflect = meth is JSFieldMethod ? ((JSFieldMethod)meth).ReturnIR() : (IReflect)meth.ReturnType;
            if ((object)reflect != (object)Typeob.Object && (object)reflect != (object)Typeob.ScriptFunction)
            {
                this.context.HandleError(JSError.InvalidCall);
            }
            else
            {
                this.member = this.ResolveOtherKindOfCall(args, argIRs, constructor, brackets);
                return true;
            }
        }

        return false;
    }

    internal static void PlaceArgumentsOnStack(
        ILGenerator il,
        ParameterInfo[] pars,
        ASTList args,
        int offset,
        int rhoffset,
        AST missing)
    {
        int count = args.count;
        int num1 = count + offset;
        int num2 = pars.Length - rhoffset;
        bool flag1 = num2 > 0 && CustomAttribute.IsDefined(pars[num2 - 1], typeof(ParamArrayAttribute), false) &&
                     (count != num2 || !Convert.IsArrayType(args[count - 1].InferType((JSField)null)));
        Type elementType = flag1 ? pars[--num2].ParameterType.GetElementType() : (Type)null;
        if (num1 > num2)
            num1 = num2;
        for (int index = offset; index < num1; ++index)
        {
            Type parameterType = pars[index].ParameterType;
            AST ast = args[index - offset];
            if (ast is ConstantWrapper && ((ConstantWrapper)ast).value == System.Reflection.Missing.Value)
            {
                object defaultParameterValue = TypeReferences.GetDefaultParameterValue(pars[index]);
                ((ConstantWrapper)ast).value = defaultParameterValue != System.Convert.DBNull
                    ? defaultParameterValue
                    : (object)null;
            }

            if (parameterType.IsByRef)
                ast.TranslateToILReference(il, parameterType.GetElementType());
            else
                ast.TranslateToIL(il, parameterType);
        }

        if (num1 < num2)
        {
            for (int index = num1; index < num2; ++index)
            {
                Type parameterType = pars[index].ParameterType;
                if (TypeReferences.GetDefaultParameterValue(pars[index]) == System.Convert.DBNull)
                {
                    if (parameterType.IsByRef)
                        missing.TranslateToILReference(il, parameterType.GetElementType());
                    else
                        missing.TranslateToIL(il, parameterType);
                }
                else if (parameterType.IsByRef)
                    new ConstantWrapper(TypeReferences.GetDefaultParameterValue(pars[index]), (Context)null)
                        .TranslateToILReference(il, parameterType.GetElementType());
                else
                    new ConstantWrapper(TypeReferences.GetDefaultParameterValue(pars[index]), (Context)null)
                        .TranslateToIL(il, parameterType);
            }
        }

        if (!flag1)
            return;
        int num3 = num1 - offset;
        int i1 = count > num3 ? count - num3 : 0;
        ConstantWrapper.TranslateToILInt(il, i1);
        il.Emit(OpCodes.Newarr, elementType);
        bool flag2 = elementType.IsValueType && !elementType.IsPrimitive;
        for (int i2 = 0; i2 < i1; ++i2)
        {
            il.Emit(OpCodes.Dup);
            ConstantWrapper.TranslateToILInt(il, i2);
            if (flag2)
                il.Emit(OpCodes.Ldelema, elementType);
            args[i2 + num3].TranslateToIL(il, elementType);
            Binding.TranslateToStelem(il, elementType);
        }
    }

    internal bool RefersToMemoryLocation()
    {
        if (!this.isFullyResolved)
            return false;
        return this.isArrayElementAccess || this.member is FieldInfo;
    }

    internal override void ResolveCall(
        ASTList args,
        IReflect[] argIRs,
        bool constructor,
        bool brackets)
    {
        this.argIRs = argIRs;
        if (this.members == null || this.members.Length == 0)
        {
            if (constructor && this.isFullyResolved && this.Engine.doFast)
            {
                if (this.member != (MemberInfo)null && ((object)(this.member as Type) != null ||
                                                        (object)(this.member as FieldInfo) != null &&
                                                        ((FieldInfo)this.member).IsLiteral))
                    this.context.HandleError(JSError.NoConstructor);
                else
                    this.HandleNoSuchMemberError();
            }
            else
                this.HandleNoSuchMemberError();
        }
        else
        {
            MemberInfo target = (MemberInfo)null;
            if (!(this is CallableExpression))
            {
                if (!(constructor & brackets))
                {
                    try
                    {
                        if (constructor)
                        {
                            this.member = target = (MemberInfo)JSBinder.SelectConstructor(this.members, argIRs);
                        }
                        else
                        {
                            MethodInfo methodInfo;
                            MethodInfo meth = methodInfo = JSBinder.SelectMethod(this.members, argIRs);
                            target = (MemberInfo)methodInfo;
                            this.member = (MemberInfo)methodInfo;
                            if (meth != (MethodInfo)null)
                            {
                                if (meth.IsSpecialName)
                                {
                                    if (this.name == meth.Name)
                                    {
                                        if (!this.name.StartsWith("get_", StringComparison.Ordinal))
                                        {
                                            if (!this.name.StartsWith("set_", StringComparison.Ordinal))
                                                goto label_25;
                                        }

                                        this.context.HandleError(JSError.NotMeantToBeCalledDirectly);
                                        this.member = (MemberInfo)null;
                                        return;
                                    }

                                    if (this.ParameterlessPropertyValueIsCallable(meth, args, argIRs, constructor,
                                            brackets))
                                        return;
                                }
                            }
                        }
                    }
                    catch (AmbiguousMatchException ex)
                    {
                        if (constructor)
                        {
                            this.context.HandleError(JSError.AmbiguousConstructorCall, this.isFullyResolved);
                            return;
                        }

                        MethodInfo meth = this.LookForParameterlessPropertyGetter();
                        if (meth != (MethodInfo)null &&
                            this.ParameterlessPropertyValueIsCallable(meth, args, argIRs, constructor, brackets))
                            return;
                        this.context.HandleError(JSError.AmbiguousMatch, this.isFullyResolved);
                        this.member = (MemberInfo)null;
                        return;
                    }
                    catch (JScriptException ex)
                    {
                        this.context.HandleError((JSError)(ex.ErrorNumber & (int)ushort.MaxValue), ex.Message, true);
                        return;
                    }
                }
            }

            label_25:
            if (target == (MemberInfo)null)
                target = this.ResolveOtherKindOfCall(args, argIRs, constructor, brackets);
            if (target == (MemberInfo)null)
                return;
            if (!this.Accessible(false))
            {
                this.member = (MemberInfo)null;
            }
            else
            {
                this.WarnIfObsolete();
                if ((object)(target as MethodBase) == null)
                    return;
                if (CustomAttribute.IsDefined(target, typeof(JSFunctionAttribute), false) &&
                    (object)(this.defaultMember as PropertyInfo) == null)
                {
                    int offset = 0;
                    JSFunctionAttributeEnum attributeValue =
                        ((JSFunctionAttribute)CustomAttribute.GetCustomAttributes(target, typeof(JSFunctionAttribute),
                            false)[0]).attributeValue;
                    if (constructor && (object)(target as ConstructorInfo) == null ||
                        (attributeValue & JSFunctionAttributeEnum.HasArguments) != JSFunctionAttributeEnum.None)
                    {
                        this.member = LateBinding.SelectMember(this.members);
                        this.defaultMember = (MemberInfo)null;
                    }
                    else
                    {
                        if ((attributeValue & JSFunctionAttributeEnum.HasThisObject) != JSFunctionAttributeEnum.None)
                            offset = 1;
                        if ((attributeValue & JSFunctionAttributeEnum.HasEngine) != JSFunctionAttributeEnum.None)
                            ++offset;
                        if (Binding.CheckParameters(((MethodBase)target).GetParameters(), argIRs, args, this.context,
                                offset, true, this.isFullyResolved))
                            return;
                        this.member = (MemberInfo)null;
                    }
                }
                else if (constructor && target is JSFieldMethod)
                    this.member = LateBinding.SelectMember(this.members);
                else if (constructor && (object)(target as ConstructorInfo) != null && !(target is JSConstructor) &&
                         Typeob.Delegate.IsAssignableFrom(target.DeclaringType))
                {
                    this.context.HandleError(JSError.DelegatesShouldNotBeExplicitlyConstructed);
                    this.member = (MemberInfo)null;
                }
                else
                {
                    if (Binding.CheckParameters(((MethodBase)target).GetParameters(), argIRs, args, this.context, 0,
                            false, this.isFullyResolved))
                        return;
                    this.member = (MemberInfo)null;
                }
            }
        }
    }

    internal override object ResolveCustomAttribute(ASTList args, IReflect[] argIRs, AST target)
    {
        try
        {
            this.ResolveCall(args, argIRs, true, false);
        }
        catch (AmbiguousMatchException ex)
        {
            this.context.HandleError(JSError.AmbiguousConstructorCall);
            return (object)null;
        }

        JSConstructor member1 = this.member as JSConstructor;
        if ((ConstructorInfo)member1 != (ConstructorInfo)null)
        {
            ClassScope classScope = member1.GetClassScope();
            if (classScope.owner.IsCustomAttribute())
                return (object)classScope;
        }
        else
        {
            ConstructorInfo member2 = this.member as ConstructorInfo;
            if (member2 != (ConstructorInfo)null)
            {
                Type declaringType = member2.DeclaringType;
                if (Typeob.Attribute.IsAssignableFrom(declaringType) && CustomAttribute
                        .GetCustomAttributes((MemberInfo)declaringType, typeof(AttributeUsageAttribute), false)
                        .Length != 0)
                    return (object)declaringType;
            }
        }

        this.context.HandleError(JSError.InvalidCustomAttributeClassOrCtor);
        return (object)null;
    }

    internal void ResolveLHValue()
    {
        MemberInfo memberInfo = this.member = LateBinding.SelectMember(this.members);
        if (memberInfo != (MemberInfo)null && !this.Accessible(true) ||
            this.member == (MemberInfo)null && this.members.Length != 0)
        {
            this.context.HandleError(JSError.AssignmentToReadOnly, this.isFullyResolved);
            this.member = (MemberInfo)null;
            this.members = new MemberInfo[0];
        }
        else if (memberInfo is JSPrototypeField)
        {
            this.member = (MemberInfo)null;
            this.members = new MemberInfo[0];
        }
        else
        {
            this.WarnIfNotFullyResolved();
            this.WarnIfObsolete();
        }
    }

    private MemberInfo ResolveOtherKindOfCall(
        ASTList argList,
        IReflect[] argIRs,
        bool constructor,
        bool brackets)
    {
        MemberInfo memberInfo = this.member = LateBinding.SelectMember(this.members);
        if ((object)(memberInfo as PropertyInfo) != null && !(memberInfo is JSProperty) &&
            memberInfo.DeclaringType == Typeob.GlobalObject)
        {
            PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
            Type propertyType = propertyInfo.PropertyType;
            if (propertyType == Typeob.Type)
                memberInfo = (MemberInfo)(Type)propertyInfo.GetValue((object)null, (object[])null);
            else if (constructor & brackets)
            {
                MethodInfo method = propertyType.GetMethod("CreateInstance",
                    BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                if (method != (MethodInfo)null)
                {
                    Type returnType = method.ReturnType;
                    memberInfo = !(returnType == Typeob.BooleanObject)
                        ? (!(returnType == Typeob.StringObject) ? (MemberInfo)returnType : (MemberInfo)Typeob.String)
                        : (MemberInfo)Typeob.Boolean;
                }
            }
        }

        if (this is CallableExpression callableExpression &&
            callableExpression.expression is ConstantWrapper expression &&
            (object)(expression.InferType((JSField)null) as Type) != null)
            memberInfo = (MemberInfo)new JSGlobalField((ScriptObject)null, (string)null, expression.value,
                FieldAttributes.Public | FieldAttributes.Literal);
        if ((object)(memberInfo as Type) != null)
        {
            if (constructor)
            {
                if (brackets)
                {
                    this.isArrayConstructor = true;
                    this.defaultMember = memberInfo;
                    this.defaultMemberReturnIR = (IReflect)new TypedArray((IReflect)(Type)memberInfo, argIRs.Length);
                    int i = 0;
                    for (int length = argIRs.Length; i < length; ++i)
                    {
                        if ((object)argIRs[i] != (object)Typeob.Object && !Convert.IsPrimitiveNumericType(argIRs[i]))
                        {
                            argList[i].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
                            break;
                        }
                    }

                    return this.member = memberInfo;
                }

                ConstructorInfo[] constructors =
                    ((Type)memberInfo).GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                if (constructors == null || constructors.Length == 0)
                {
                    this.context.HandleError(JSError.NoConstructor);
                    this.member = (MemberInfo)null;
                    return (MemberInfo)null;
                }

                this.members = (MemberInfo[])constructors;
                this.ResolveCall(argList, argIRs, true, brackets);
                return this.member;
            }

            if (!brackets && argIRs.Length == 1)
                return memberInfo;
            this.context.HandleError(JSError.InvalidCall);
            return this.member = (MemberInfo)null;
        }

        if (memberInfo is JSPrototypeField)
            return this.member = (MemberInfo)null;
        if ((object)(memberInfo as FieldInfo) != null && ((FieldInfo)memberInfo).IsLiteral)
        {
            if (!this.AccessibleField(false))
                return this.member = (MemberInfo)null;
            object elementType = memberInfo is JSVariableField
                ? ((JSVariableField)memberInfo).value
                : TypeReferences.GetConstantValue((FieldInfo)memberInfo);
            if (elementType is ClassScope || (object)(elementType as Type) != null)
            {
                if (constructor)
                {
                    if (brackets)
                    {
                        this.isArrayConstructor = true;
                        this.defaultMember = memberInfo;
                        this.defaultMemberReturnIR = (IReflect)new TypedArray(
                            elementType is ClassScope ? (IReflect)elementType : (IReflect)elementType, argIRs.Length);
                        int i = 0;
                        for (int length = argIRs.Length; i < length; ++i)
                        {
                            if ((object)argIRs[i] != (object)Typeob.Object &&
                                !Convert.IsPrimitiveNumericType(argIRs[i]))
                            {
                                argList[i].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
                                break;
                            }
                        }

                        return this.member = memberInfo;
                    }

                    if (elementType is ClassScope && !((ClassScope)elementType).owner.isStatic)
                    {
                        if (this is Member && ((Member)this).rootObject is ConstantWrapper rootObject &&
                            !(rootObject.Evaluate() is Namespace))
                        {
                            ((Member)this).rootObject.context.HandleError(JSError.NeedInstance);
                            return (MemberInfo)null;
                        }
                    }

                    this.members = elementType is ClassScope
                        ? (MemberInfo[])((ClassScope)elementType).constructors
                        : (MemberInfo[])((Type)elementType).GetConstructors(BindingFlags.Instance |
                                                                            BindingFlags.Public);
                    if (this.members == null || this.members.Length == 0)
                    {
                        this.context.HandleError(JSError.NoConstructor);
                        this.member = (MemberInfo)null;
                        return (MemberInfo)null;
                    }

                    this.ResolveCall(argList, argIRs, true, brackets);
                    return this.member;
                }

                if (!brackets && argIRs.Length == 1)
                {
                    Type type = elementType as Type;
                    return this.member = type != (Type)null ? (MemberInfo)type : memberInfo;
                }

                this.context.HandleError(JSError.InvalidCall);
                return this.member = (MemberInfo)null;
            }

            switch (elementType)
            {
                case TypedArray _:
                    if (!constructor)
                    {
                        if (!(argIRs.Length != 1 | brackets))
                            return this.member = memberInfo;
                        goto label_94;
                    }

                    if (brackets && argIRs.Length != 0)
                    {
                        this.isArrayConstructor = true;
                        this.defaultMember = memberInfo;
                        this.defaultMemberReturnIR = (IReflect)new TypedArray((IReflect)elementType, argIRs.Length);
                        int i = 0;
                        for (int length = argIRs.Length; i < length; ++i)
                        {
                            if ((object)argIRs[i] != (object)Typeob.Object &&
                                !Convert.IsPrimitiveNumericType(argIRs[i]))
                            {
                                argList[i].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
                                break;
                            }
                        }

                        return this.member = memberInfo;
                    }

                    goto label_94;
                case FunctionObject _:
                    FunctionObject functionObject = (FunctionObject)elementType;
                    return !functionObject.isExpandoMethod && !functionObject.Must_save_stack_locals &&
                           (functionObject.own_scope.ProvidesOuterScopeLocals == null ||
                            functionObject.own_scope.ProvidesOuterScopeLocals.count == 0)
                        ? (this.member =
                            (MemberInfo)((JSVariableField)this.member).GetAsMethod((object)functionObject.own_scope))
                        : this.member;
            }
        }

        IReflect reflect1 = this.InferType((JSField)null);
        Type c = reflect1 as Type;
        if (!brackets && (c != (Type)null && Typeob.ScriptFunction.IsAssignableFrom(c) || reflect1 is ScriptFunction))
        {
            this.defaultMember = memberInfo;
            if (c == (Type)null)
            {
                this.defaultMemberReturnIR = (IReflect)Globals.TypeRefs.ToReferenceContext(reflect1.GetType());
                this.member = (MemberInfo)this.defaultMemberReturnIR.GetMethod(
                    constructor ? "CreateInstance" : "Invoke",
                    BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                if (this.member == (MemberInfo)null)
                {
                    this.defaultMemberReturnIR = (IReflect)Typeob.ScriptFunction;
                    this.member = (MemberInfo)this.defaultMemberReturnIR.GetMethod(
                        constructor ? "CreateInstance" : "Invoke",
                        BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
                }

                return this.member;
            }

            if (constructor && this.members.Length != 0 && this.members[0] is JSFieldMethod)
            {
                JSFieldMethod member = (JSFieldMethod)this.members[0];
                member.func.PartiallyEvaluate();
                if (!member.func.isExpandoMethod)
                    this.context.HandleError(JSError.NotAnExpandoFunction, this.isFullyResolved);
            }

            this.defaultMemberReturnIR = (IReflect)c;
            return this.member = (MemberInfo)c.GetMethod(constructor ? "CreateInstance" : "Invoke",
                BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
        }

        if ((object)reflect1 == (object)Typeob.Type)
        {
            this.member = (MemberInfo)null;
            return (MemberInfo)null;
        }

        if ((object)reflect1 == (object)Typeob.Object ||
            reflect1 is ScriptObject & brackets && !(reflect1 is ClassScope))
            return memberInfo;
        if (reflect1 is TypedArray || (object)(reflect1 as Type) != null && ((Type)reflect1).IsArray)
        {
            int length = argIRs.Length;
            int num = reflect1 is TypedArray ? ((TypedArray)reflect1).rank : ((Type)reflect1).GetArrayRank();
            if (length != num)
            {
                this.context.HandleError(JSError.IncorrectNumberOfIndices, this.isFullyResolved);
            }
            else
            {
                for (int i = 0; i < num; ++i)
                {
                    if ((object)argIRs[i] != (object)Typeob.Object &&
                        (!Convert.IsPrimitiveNumericType(argIRs[i]) || Convert.IsBadIndex(argList[i])))
                    {
                        argList[i].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
                        break;
                    }
                }
            }

            if (constructor)
            {
                if (brackets)
                {
                    IReflect reflect2 = reflect1 is TypedArray
                        ? ((TypedArray)reflect1).elementType
                        : (IReflect)((Type)reflect1).GetElementType();
                    if ((object)reflect1 != (object)Typeob.Object && !(reflect1 is ClassScope) &&
                        ((object)(reflect1 as Type) == null || Typeob.Type.IsAssignableFrom((Type)reflect1) ||
                         Typeob.ScriptFunction.IsAssignableFrom((Type)reflect1)))
                        goto label_94;
                }
                else
                    goto label_94;
            }

            this.isArrayElementAccess = true;
            this.defaultMember = memberInfo;
            this.defaultMemberReturnIR = reflect1;
            return (MemberInfo)null;
        }

        if (!constructor)
        {
            if (brackets && (object)reflect1 == (object)Typeob.String &&
                (this.argIRs.Length != 1 || !Convert.IsPrimitiveNumericType(argIRs[0])))
                reflect1 = (IReflect)Typeob.StringObject;
            MemberInfo[] defaultMembers = brackets || !(reflect1 is ScriptObject)
                ? JSBinder.GetDefaultMembers(reflect1)
                : (MemberInfo[])null;
            if (defaultMembers != null)
            {
                if (defaultMembers.Length != 0)
                {
                    try
                    {
                        this.defaultMember = memberInfo;
                        this.defaultMemberReturnIR = reflect1;
                        return this.member = (MemberInfo)JSBinder.SelectMethod(this.members = defaultMembers, argIRs);
                    }
                    catch (AmbiguousMatchException ex)
                    {
                        this.context.HandleError(JSError.AmbiguousMatch, this.isFullyResolved);
                        return this.member = (MemberInfo)null;
                    }
                }
            }

            if (!brackets && (object)(reflect1 as Type) != null && Typeob.Delegate.IsAssignableFrom((Type)reflect1))
            {
                this.defaultMember = memberInfo;
                this.defaultMemberReturnIR = reflect1;
                return this.member = (MemberInfo)((Type)reflect1).GetMethod("Invoke");
            }
        }

        label_94:
        if (constructor)
            this.context.HandleError(JSError.NeedType, this.isFullyResolved);
        else if (brackets)
            this.context.HandleError(JSError.NotIndexable, this.isFullyResolved);
        else
            this.context.HandleError(JSError.FunctionExpected, this.isFullyResolved);
        return this.member = (MemberInfo)null;
    }

    /// <summary>Resolves the right-hand side of the binding reference.</summary>
    protected void ResolveRHValue()
    {
        MemberInfo memberInfo = this.member = LateBinding.SelectMember(this.members);
        JSLocalField member = this.member as JSLocalField;
        if ((FieldInfo)member != (FieldInfo)null && member.value is FunctionObject functionObject &&
            functionObject.enclosing_scope is FunctionScope enclosingScope)
            enclosingScope.closuresMightEscape = true;
        if (memberInfo is JSPrototypeField)
            this.member = (MemberInfo)null;
        else if (!this.Accessible(false))
        {
            this.member = (MemberInfo)null;
        }
        else
        {
            this.WarnIfObsolete();
            this.WarnIfNotFullyResolved();
        }
    }

    internal override void SetPartialValue(AST partial_value)
    {
        Binding.AssignmentCompatible(this.InferType((JSField)null), partial_value,
            partial_value.InferType((JSField)null), this.isFullyResolved);
    }

    internal void SetPartialValue(
        ASTList argList,
        IReflect[] argIRs,
        AST partial_value,
        bool inBrackets)
    {
        if (this.members == null || this.members.Length == 0)
        {
            this.HandleNoSuchMemberError();
            this.isAssignmentToDefaultIndexedProperty = true;
        }
        else
        {
            this.PartiallyEvaluate();
            IReflect ir = this.InferType((JSField)null);
            this.isAssignmentToDefaultIndexedProperty = true;
            if ((object)ir == (object)Typeob.Object)
            {
                JSVariableField member = this.member as JSVariableField;
                if ((FieldInfo)member == (FieldInfo)null || !member.IsLiteral || !(member.value is ClassScope))
                    return;
                ir = (IReflect)Typeob.Type;
            }
            else
            {
                if (ir is TypedArray || (object)(ir as Type) != null && ((Type)ir).IsArray)
                {
                    bool flag = false;
                    int length = argIRs.Length;
                    int num = ir is TypedArray ? ((TypedArray)ir).rank : ((Type)ir).GetArrayRank();
                    if (length != num)
                    {
                        this.context.HandleError(JSError.IncorrectNumberOfIndices, this.isFullyResolved);
                        flag = true;
                    }

                    for (int i = 0; i < num; ++i)
                    {
                        if (!flag && i < length && (object)argIRs[i] != (object)Typeob.Object &&
                            (!Convert.IsPrimitiveNumericType(argIRs[i]) || Convert.IsBadIndex(argList[i])))
                        {
                            argList[i].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
                            flag = true;
                        }
                    }

                    this.isArrayElementAccess = true;
                    this.isAssignmentToDefaultIndexedProperty = false;
                    this.defaultMember = this.member;
                    this.defaultMemberReturnIR = ir;
                    Binding.AssignmentCompatible(
                        ir is TypedArray ? ((TypedArray)ir).elementType : (IReflect)((Type)ir).GetElementType(),
                        partial_value, partial_value.InferType((JSField)null), this.isFullyResolved);
                    return;
                }

                MemberInfo[] defaultMembers = JSBinder.GetDefaultMembers(ir);
                if (defaultMembers != null && defaultMembers.Length != 0)
                {
                    if (this.member != (MemberInfo)null)
                    {
                        try
                        {
                            PropertyInfo prop = JSBinder.SelectProperty(defaultMembers, argIRs);
                            if (prop == (PropertyInfo)null)
                            {
                                this.context.HandleError(JSError.NotIndexable, Convert.ToTypeName(ir));
                                return;
                            }

                            if (JSProperty.GetSetMethod(prop, true) == (MethodInfo)null)
                            {
                                if ((object)ir == (object)Typeob.String)
                                {
                                    this.context.HandleError(JSError.UselessAssignment);
                                    return;
                                }

                                this.context.HandleError(JSError.AssignmentToReadOnly,
                                    this.isFullyResolved && this.Engine.doFast);
                                return;
                            }

                            if (!Binding.CheckParameters(prop.GetIndexParameters(), argIRs, argList, this.context, 0,
                                    false, true))
                                return;
                            this.defaultMember = this.member;
                            this.defaultMemberReturnIR = ir;
                            this.members = defaultMembers;
                            this.member = (MemberInfo)prop;
                            return;
                        }
                        catch (AmbiguousMatchException ex)
                        {
                            this.context.HandleError(JSError.AmbiguousMatch, this.isFullyResolved);
                            this.member = (MemberInfo)null;
                            return;
                        }
                    }
                }
            }

            this.member = (MemberInfo)null;
            if (!inBrackets)
                this.context.HandleError(JSError.IllegalAssignment);
            else
                this.context.HandleError(JSError.NotIndexable, Convert.ToTypeName(ir));
        }
    }

    internal override void SetValue(object value)
    {
        MemberInfo member = this.member;
        object obj = this.GetObject();
        if ((object)(member as FieldInfo) != null)
        {
            FieldInfo fieldInfo = (FieldInfo)member;
            if (fieldInfo.IsLiteral || fieldInfo.IsInitOnly)
                return;
            if (!(fieldInfo is JSField) || fieldInfo is JSWrappedField)
                value = Convert.CoerceT(value, fieldInfo.FieldType, false);
            fieldInfo.SetValue(obj, value, BindingFlags.SuppressChangeType, (Binder)null, (CultureInfo)null);
        }
        else if ((object)(member as PropertyInfo) != null)
        {
            PropertyInfo prop = (PropertyInfo)member;
            if (obj is ClassScope && !(prop is JSProperty))
            {
                JSProperty.SetValue(prop, ((WithObject)((ScriptObject)obj).GetParent()).contained_object, value,
                    (object[])null);
            }
            else
            {
                if (!(prop is JSProperty))
                    value = Convert.CoerceT(value, prop.PropertyType, false);
                JSProperty.SetValue(prop, obj, value, (object[])null);
            }
        }
        else
        {
            if (this.members != null && this.members.Length != 0)
                throw new JScriptException(JSError.IllegalAssignment);
            this.EvaluateAsLateBinding().SetValue(value);
        }
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        this.TranslateToIL(il, rtype, false, false);
    }

    internal void TranslateToIL(ILGenerator il, Type rtype, bool calledFromDelete)
    {
        this.TranslateToIL(il, rtype, false, false, calledFromDelete);
    }

    private void TranslateToIL(ILGenerator il, Type rtype, bool preSet, bool preSetPlusGet)
    {
        this.TranslateToIL(il, rtype, preSet, preSetPlusGet, false);
    }

    private void TranslateToIL(
        ILGenerator il,
        Type rtype,
        bool preSet,
        bool preSetPlusGet,
        bool calledFromDelete)
    {
        if ((object)(this.member as FieldInfo) != null)
        {
            FieldInfo member = (FieldInfo)this.member;
            bool flag = member.IsStatic || member.IsLiteral;
            if (member.IsLiteral && member is JSMemberField)
                flag = !(((JSVariableField)member).value is FunctionObject functionObject) ||
                       !functionObject.isExpandoMethod;
            if (!flag || member is JSClosureField)
            {
                this.TranslateToILObject(il, member.DeclaringType, true);
                if (preSetPlusGet)
                    il.Emit(OpCodes.Dup);
                flag = false;
            }

            if (preSet)
                return;
            object obj1;
            switch (member)
            {
                case JSField _:
                    obj1 = ((JSField)member).GetMetaData();
                    break;
                case JSFieldInfo _:
                    obj1 = (object)((JSFieldInfo)member).field;
                    break;
                default:
                    obj1 = (object)member;
                    break;
            }

            object obj2 = obj1;
            if ((object)(obj2 as FieldInfo) != null && !((FieldInfo)obj2).IsLiteral)
                il.Emit(flag ? OpCodes.Ldsfld : OpCodes.Ldfld, (FieldInfo)obj2);
            else if (obj2 is LocalBuilder)
            {
                il.Emit(OpCodes.Ldloc, (LocalBuilder)obj2);
            }
            else
            {
                if (member.IsLiteral)
                {
                    new ConstantWrapper(TypeReferences.GetConstantValue(member), this.context).TranslateToIL(il, rtype);
                    return;
                }

                Convert.EmitLdarg(il, (short)obj2);
            }

            Convert.Emit((AST)this, il, member.FieldType, rtype);
        }
        else if ((object)(this.member as PropertyInfo) != null)
        {
            PropertyInfo member = (PropertyInfo)this.member;
            MethodInfo method = preSet ? JSProperty.GetSetMethod(member, true) : JSProperty.GetGetMethod(member, true);
            if (method == (MethodInfo)null)
            {
                if (preSet)
                    return;
                if (this is Lookup)
                {
                    il.Emit(OpCodes.Ldc_I4, 5041);
                    il.Emit(OpCodes.Newobj, CompilerGlobals.scriptExceptionConstructor);
                    il.Emit(OpCodes.Throw);
                }
                else
                    il.Emit(OpCodes.Ldnull);
            }
            else
            {
                bool flag = method.IsStatic && !(method is JSClosureMethod);
                if (!flag)
                {
                    Type declaringType = method.DeclaringType;
                    if (declaringType == Typeob.StringObject && method.Name.Equals("get_length"))
                    {
                        this.TranslateToILObject(il, Typeob.String, false);
                        method = CompilerGlobals.stringLengthMethod;
                    }
                    else
                        this.TranslateToILObject(il, declaringType, true);
                }

                if (preSet)
                    return;
                MethodInfo methodInfoMetadata = this.GetMethodInfoMetadata(method);
                if (flag)
                {
                    il.Emit(OpCodes.Call, methodInfoMetadata);
                }
                else
                {
                    if (preSetPlusGet)
                        il.Emit(OpCodes.Dup);
                    if (!this.isNonVirtual && methodInfoMetadata.IsVirtual && !methodInfoMetadata.IsFinal &&
                        (!methodInfoMetadata.ReflectedType.IsSealed || !methodInfoMetadata.ReflectedType.IsValueType))
                        il.Emit(OpCodes.Callvirt, methodInfoMetadata);
                    else
                        il.Emit(OpCodes.Call, methodInfoMetadata);
                }

                Convert.Emit((AST)this, il, methodInfoMetadata.ReturnType, rtype);
            }
        }
        else if ((object)(this.member as MethodInfo) != null)
        {
            MethodInfo methodInfoMetadata = this.GetMethodInfoMetadata((MethodInfo)this.member);
            if (Typeob.Delegate.IsAssignableFrom(rtype))
            {
                if (!methodInfoMetadata.IsStatic)
                {
                    Type declaringType = methodInfoMetadata.DeclaringType;
                    this.TranslateToILObject(il, declaringType, false);
                    if (declaringType.IsValueType)
                        il.Emit(OpCodes.Box, declaringType);
                }
                else
                    il.Emit(OpCodes.Ldnull);

                if (methodInfoMetadata.IsVirtual && !methodInfoMetadata.IsFinal &&
                    (!methodInfoMetadata.ReflectedType.IsSealed || !methodInfoMetadata.ReflectedType.IsValueType))
                {
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Ldvirtftn, methodInfoMetadata);
                }
                else
                    il.Emit(OpCodes.Ldftn, methodInfoMetadata);

                ConstructorInfo constructor = rtype.GetConstructor(new Type[2]
                {
                    Typeob.Object,
                    Typeob.UIntPtr
                });
                if (constructor == (ConstructorInfo)null)
                    constructor = rtype.GetConstructor(new Type[2]
                    {
                        Typeob.Object,
                        Typeob.IntPtr
                    });
                il.Emit(OpCodes.Newobj, constructor);
            }
            else if (this.member is JSExpandoIndexerMethod)
            {
                MemberInfo member = this.member;
                this.member = this.defaultMember;
                this.TranslateToIL(il, Typeob.Object);
                this.member = member;
            }
            else
            {
                il.Emit(OpCodes.Ldnull);
                Convert.Emit((AST)this, il, Typeob.Object, rtype);
            }
        }
        else
        {
            object loc = (object)null;
            if (this is Lookup)
            {
                ((Lookup)this).TranslateToLateBinding(il);
            }
            else
            {
                if (!this.isFullyResolved && !preSet && !preSetPlusGet)
                    loc = this.TranslateToSpeculativeEarlyBindings(il, rtype, false);
                ((Member)this).TranslateToLateBinding(il, loc != null);
                if (!this.isFullyResolved & preSetPlusGet)
                    loc = this.TranslateToSpeculativeEarlyBindings(il, rtype, true);
            }

            if (preSetPlusGet)
                il.Emit(OpCodes.Dup);
            if (preSet)
                return;
            if (this is Lookup && !calledFromDelete)
                il.Emit(OpCodes.Call, CompilerGlobals.getValue2Method);
            else
                il.Emit(OpCodes.Call, CompilerGlobals.getNonMissingValueMethod);
            Convert.Emit((AST)this, il, Typeob.Object, rtype);
            if (loc == null)
                return;
            il.MarkLabel((Label)loc);
        }
    }

    internal override void TranslateToILCall(
        ILGenerator il,
        Type rtype,
        ASTList argList,
        bool construct,
        bool brackets)
    {
        MemberInfo member = this.member;
        if (this.defaultMember != (MemberInfo)null)
        {
            if (this.isArrayConstructor)
            {
                TypedArray defaultMemberReturnIr = (TypedArray)this.defaultMemberReturnIR;
                Type type1 = Convert.ToType(defaultMemberReturnIr.elementType);
                int rank = defaultMemberReturnIr.rank;
                if (rank == 1)
                {
                    argList[0].TranslateToIL(il, Typeob.Int32);
                    il.Emit(OpCodes.Newarr, type1);
                }
                else
                {
                    Type type2 = defaultMemberReturnIr.ToType();
                    Type[] typeArray = new Type[rank];
                    for (int index = 0; index < rank; ++index)
                        typeArray[index] = Typeob.Int32;
                    int i = 0;
                    for (int count = argList.count; i < count; ++i)
                        argList[i].TranslateToIL(il, Typeob.Int32);
                    if ((Type)(type1 as TypeBuilder) != (Type)null)
                    {
                        MethodInfo arrayMethod = ((ModuleBuilder)type2.Module).GetArrayMethod(type2, ".ctor",
                            CallingConventions.HasThis, Typeob.Void, typeArray);
                        il.Emit(OpCodes.Newobj, arrayMethod);
                    }
                    else
                    {
                        ConstructorInfo constructor = type2.GetConstructor(typeArray);
                        il.Emit(OpCodes.Newobj, constructor);
                    }
                }

                Convert.Emit((AST)this, il, defaultMemberReturnIr.ToType(), rtype);
                return;
            }

            this.member = this.defaultMember;
            IReflect defaultMemberReturnIr1 = this.defaultMemberReturnIR;
            Type type = (object)(defaultMemberReturnIr1 as Type) != null
                ? (Type)defaultMemberReturnIr1
                : Convert.ToType(defaultMemberReturnIr1);
            this.TranslateToIL(il, type);
            if (this.isArrayElementAccess)
            {
                int i = 0;
                for (int count = argList.count; i < count; ++i)
                    argList[i].TranslateToIL(il, Typeob.Int32);
                Type elementType = type.GetElementType();
                int arrayRank = type.GetArrayRank();
                if (arrayRank == 1)
                {
                    Binding.TranslateToLdelem(il, elementType);
                }
                else
                {
                    Type[] parameterTypes = new Type[arrayRank];
                    for (int index = 0; index < arrayRank; ++index)
                        parameterTypes[index] = Typeob.Int32;
                    MethodInfo arrayMethod = this.compilerGlobals.module.GetArrayMethod(type, "Get",
                        CallingConventions.HasThis, elementType, parameterTypes);
                    il.Emit(OpCodes.Call, arrayMethod);
                }

                Convert.Emit((AST)this, il, elementType, rtype);
                return;
            }

            this.member = member;
        }

        if ((object)(member as MethodInfo) != null)
        {
            MethodInfo methodInfo = (MethodInfo)member;
            Type declaringType = methodInfo.DeclaringType;
            Type reflectedType = methodInfo.ReflectedType;
            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (!methodInfo.IsStatic && this.defaultMember == (MemberInfo)null)
                this.TranslateToILObject(il, declaringType, true);
            if (methodInfo is JSClosureMethod)
                this.TranslateToILObject(il, declaringType, false);
            int offset = 0;
            ConstantWrapper missing;
            if (methodInfo is JSFieldMethod ||
                CustomAttribute.IsDefined((MemberInfo)methodInfo, typeof(JSFunctionAttribute), false))
            {
                offset = this.PlaceValuesForHiddenParametersOnStack(il, methodInfo, parameters);
                missing = Binding.JScriptMissingCW;
            }
            else
                missing = Binding.ReflectionMissingCW;

            if (argList.count == 1 && missing == Binding.JScriptMissingCW &&
                (object)(this.defaultMember as PropertyInfo) != null)
            {
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Newarr, Typeob.Object);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Ldc_I4_0);
                argList[0].TranslateToIL(il, Typeob.Object);
                il.Emit(OpCodes.Stelem_Ref);
            }
            else
                Binding.PlaceArgumentsOnStack(il, parameters, argList, offset, 0, (AST)missing);

            MethodInfo methodInfoMetadata = this.GetMethodInfoMetadata(methodInfo);
            if (!this.isNonVirtual && methodInfoMetadata.IsVirtual && !methodInfoMetadata.IsFinal &&
                (!reflectedType.IsSealed || !reflectedType.IsValueType))
                il.Emit(OpCodes.Callvirt, methodInfoMetadata);
            else
                il.Emit(OpCodes.Call, methodInfoMetadata);
            Convert.Emit((AST)this, il, methodInfoMetadata.ReturnType, rtype);
        }
        else if ((object)(member as ConstructorInfo) != null)
        {
            ConstructorInfo constructorInfo = (ConstructorInfo)member;
            ParameterInfo[] parameters = constructorInfo.GetParameters();
            bool flag1 = false;
            if (CustomAttribute.IsDefined((MemberInfo)constructorInfo, typeof(JSFunctionAttribute), false))
                flag1 = (((JSFunctionAttribute)CustomAttribute.GetCustomAttributes((MemberInfo)constructorInfo,
                             typeof(JSFunctionAttribute), false)[0]).attributeValue &
                         JSFunctionAttributeEnum.IsInstanceNestedClassConstructor) != 0;
            if (flag1)
            {
                Binding.PlaceArgumentsOnStack(il, parameters, argList, 0, 1, (AST)Binding.ReflectionMissingCW);
                this.TranslateToILObject(il, parameters[parameters.Length - 1].ParameterType, false);
            }
            else
                Binding.PlaceArgumentsOnStack(il, parameters, argList, 0, 0, (AST)Binding.ReflectionMissingCW);

            Type obtype;
            if (member is JSConstructor && (obtype = ((JSConstructor)member).OuterClassType()) != (Type)null)
                this.TranslateToILObject(il, obtype, false);
            Type declaringType = constructorInfo.DeclaringType;
            bool flag2;
            if (constructorInfo is JSConstructor)
            {
                constructorInfo = ((JSConstructor)constructorInfo).GetConstructorInfo(this.compilerGlobals);
                flag2 = true;
            }
            else
                flag2 = Typeob.INeedEngine.IsAssignableFrom(declaringType);

            il.Emit(OpCodes.Newobj, constructorInfo);
            if (flag2)
            {
                il.Emit(OpCodes.Dup);
                this.EmitILToLoadEngine(il);
                il.Emit(OpCodes.Callvirt, CompilerGlobals.setEngineMethod);
            }

            Convert.Emit((AST)this, il, declaringType, rtype);
        }
        else
        {
            Type type3 = member as Type;
            if (type3 != (Type)null)
            {
                AST ast = argList[0];
                if (ast is NullLiteral)
                {
                    ast.TranslateToIL(il, type3);
                    Convert.Emit((AST)this, il, type3, rtype);
                }
                else
                {
                    IReflect ir = ast.InferType((JSField)null);
                    if ((object)ir == (object)Typeob.ScriptFunction && Typeob.Delegate.IsAssignableFrom(type3))
                    {
                        ast.TranslateToIL(il, type3);
                    }
                    else
                    {
                        Type type4 = Convert.ToType(ir);
                        ast.TranslateToIL(il, type4);
                        Convert.Emit((AST)this, il, type4, type3, true);
                    }

                    Convert.Emit((AST)this, il, type3, rtype);
                }
            }
            else
            {
                if ((object)(member as FieldInfo) != null && ((FieldInfo)member).IsLiteral)
                {
                    object ir = member is JSVariableField
                        ? ((JSVariableField)member).value
                        : TypeReferences.GetConstantValue((FieldInfo)member);
                    if ((object)(ir as Type) == null)
                    {
                        switch (ir)
                        {
                            case ClassScope _:
                            case TypedArray _:
                                break;
                            default:
                                goto label_72;
                        }
                    }

                    AST ast = argList[0];
                    if (ast is NullLiteral)
                    {
                        il.Emit(OpCodes.Ldnull);
                        return;
                    }

                    if (ir is ClassScope classScope && classScope.owner is EnumDeclaration owner)
                        ir = (object)owner.baseType.ToType();
                    Type type5 = Convert.ToType(ast.InferType((JSField)null));
                    ast.TranslateToIL(il, type5);
                    Type type6 = (object)(ir as Type) != null
                        ? (Type)ir
                        : (ir is ClassScope ? Convert.ToType((IReflect)ir) : ((TypedArray)ir).ToType());
                    Convert.Emit((AST)this, il, type5, type6, true);
                    if (rtype.IsEnum)
                        return;
                    Convert.Emit((AST)this, il, type6, rtype);
                    return;
                }

                label_72:
                LocalBuilder local = (LocalBuilder)null;
                int i1 = 0;
                for (int count = argList.count; i1 < count; ++i1)
                {
                    if (argList[i1] is AddressOf)
                    {
                        local = il.DeclareLocal(Typeob.ArrayOfObject);
                        break;
                    }
                }

                object loc = (object)null;
                if (member == (MemberInfo)null && (this.members == null || this.members.Length == 0))
                {
                    if (this is Lookup)
                    {
                        ((Lookup)this).TranslateToLateBinding(il);
                    }
                    else
                    {
                        loc = this.TranslateToSpeculativeEarlyBoundCalls(il, rtype, argList, construct, brackets);
                        ((Member)this).TranslateToLateBinding(il, loc != null);
                    }

                    argList.TranslateToIL(il, Typeob.ArrayOfObject);
                    if (local != null)
                    {
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Stloc, local);
                    }

                    if (construct)
                        il.Emit(OpCodes.Ldc_I4_1);
                    else
                        il.Emit(OpCodes.Ldc_I4_0);
                    if (brackets)
                        il.Emit(OpCodes.Ldc_I4_1);
                    else
                        il.Emit(OpCodes.Ldc_I4_0);
                    this.EmitILToLoadEngine(il);
                    il.Emit(OpCodes.Call, CompilerGlobals.callMethod);
                    Convert.Emit((AST)this, il, Typeob.Object, rtype);
                    if (local != null)
                    {
                        int i2 = 0;
                        for (int count = argList.count; i2 < count; ++i2)
                        {
                            if (argList[i2] is AddressOf addressOf)
                            {
                                addressOf.TranslateToILPreSet(il);
                                il.Emit(OpCodes.Ldloc, local);
                                ConstantWrapper.TranslateToILInt(il, i2);
                                il.Emit(OpCodes.Ldelem_Ref);
                                Convert.Emit((AST)this, il, Typeob.Object,
                                    Convert.ToType(addressOf.InferType((JSField)null)));
                                addressOf.TranslateToILSet(il, (AST)null);
                            }
                        }
                    }

                    if (loc == null)
                        return;
                    il.MarkLabel((Label)loc);
                }
                else
                {
                    this.TranslateToILWithDupOfThisOb(il);
                    argList.TranslateToIL(il, Typeob.ArrayOfObject);
                    if (local != null)
                    {
                        il.Emit(OpCodes.Dup);
                        il.Emit(OpCodes.Stloc, local);
                    }

                    if (construct)
                        il.Emit(OpCodes.Ldc_I4_1);
                    else
                        il.Emit(OpCodes.Ldc_I4_0);
                    if (brackets)
                        il.Emit(OpCodes.Ldc_I4_1);
                    else
                        il.Emit(OpCodes.Ldc_I4_0);
                    this.EmitILToLoadEngine(il);
                    il.Emit(OpCodes.Call, CompilerGlobals.callValueMethod);
                    Convert.Emit((AST)this, il, Typeob.Object, rtype);
                    if (local == null)
                        return;
                    int i3 = 0;
                    for (int count = argList.count; i3 < count; ++i3)
                    {
                        if (argList[i3] is AddressOf addressOf)
                        {
                            addressOf.TranslateToILPreSet(il);
                            il.Emit(OpCodes.Ldloc, local);
                            ConstantWrapper.TranslateToILInt(il, i3);
                            il.Emit(OpCodes.Ldelem_Ref);
                            Convert.Emit((AST)this, il, Typeob.Object,
                                Convert.ToType(addressOf.InferType((JSField)null)));
                            addressOf.TranslateToILSet(il, (AST)null);
                        }
                    }
                }
            }
        }
    }

    internal override void TranslateToILDelete(ILGenerator il, Type rtype)
    {
        if (this is Lookup)
            ((Lookup)this).TranslateToLateBinding(il);
        else
            ((Member)this).TranslateToLateBinding(il, false);
        il.Emit(OpCodes.Call, CompilerGlobals.deleteMethod);
        Convert.Emit((AST)this, il, Typeob.Boolean, rtype);
    }

    /// <summary>When overridden in a derived class, generates Microsoft intermediate language (MSIL) code for the object that the binding reference is defined on.</summary>
    /// <param name="il">An object that generates Microsoft intermediate language (MSIL) instructions.</param>
    /// <param name="obtype">The type of the object.</param>
    /// <param name="noValue">true if the object does not have a value; otherwise, false.</param>
    protected abstract void TranslateToILObject(ILGenerator il, Type obtype, bool noValue);

    internal override void TranslateToILPreSet(ILGenerator il)
    {
        this.TranslateToIL(il, (Type)null, true, false);
    }

    internal override void TranslateToILPreSet(ILGenerator il, ASTList argList)
    {
        if (this.isArrayElementAccess)
        {
            this.member = this.defaultMember;
            IReflect defaultMemberReturnIr = this.defaultMemberReturnIR;
            Type rtype = (object)(defaultMemberReturnIr as Type) != null
                ? (Type)defaultMemberReturnIr
                : Convert.ToType(defaultMemberReturnIr);
            this.TranslateToIL(il, rtype);
            int i = 0;
            for (int count = argList.count; i < count; ++i)
                argList[i].TranslateToIL(il, Typeob.Int32);
            if (rtype.GetArrayRank() != 1)
                return;
            Type elementType = rtype.GetElementType();
            if (!elementType.IsValueType || elementType.IsPrimitive || elementType.IsEnum)
                return;
            il.Emit(OpCodes.Ldelema, elementType);
        }
        else if ((object)(this.member as PropertyInfo) != null && this.defaultMember != (MemberInfo)null)
        {
            PropertyInfo member = (PropertyInfo)this.member;
            this.member = this.defaultMember;
            this.TranslateToIL(il, Convert.ToType(this.defaultMemberReturnIR));
            this.member = (MemberInfo)member;
            Binding.PlaceArgumentsOnStack(il, member.GetIndexParameters(), argList, 0, 0,
                (AST)Binding.ReflectionMissingCW);
        }
        else
            base.TranslateToILPreSet(il, argList);
    }

    internal override void TranslateToILPreSetPlusGet(ILGenerator il)
    {
        this.TranslateToIL(il, Convert.ToType(this.InferType((JSField)null)), false, true);
    }

    internal override void TranslateToILPreSetPlusGet(
        ILGenerator il,
        ASTList argList,
        bool inBrackets)
    {
        if (this.isArrayElementAccess)
        {
            this.member = this.defaultMember;
            IReflect defaultMemberReturnIr = this.defaultMemberReturnIR;
            Type rtype = (object)(defaultMemberReturnIr as Type) != null
                ? (Type)defaultMemberReturnIr
                : Convert.ToType(defaultMemberReturnIr);
            this.TranslateToIL(il, rtype);
            il.Emit(OpCodes.Dup);
            int arrayRank = rtype.GetArrayRank();
            LocalBuilder[] localBuilderArray = new LocalBuilder[arrayRank];
            int i = 0;
            for (int count = argList.count; i < count; ++i)
            {
                argList[i].TranslateToIL(il, Typeob.Int32);
                localBuilderArray[i] = il.DeclareLocal(Typeob.Int32);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc, localBuilderArray[i]);
            }

            Type elementType = rtype.GetElementType();
            if (arrayRank == 1)
            {
                Binding.TranslateToLdelem(il, elementType);
            }
            else
            {
                Type[] types = new Type[arrayRank];
                for (int index = 0; index < arrayRank; ++index)
                    types[index] = Typeob.Int32;
                MethodInfo method = rtype.GetMethod("Get", types);
                il.Emit(OpCodes.Call, method);
            }

            LocalBuilder local = il.DeclareLocal(elementType);
            il.Emit(OpCodes.Stloc, local);
            for (int index = 0; index < arrayRank; ++index)
                il.Emit(OpCodes.Ldloc, localBuilderArray[index]);
            if (arrayRank == 1 && elementType.IsValueType && !elementType.IsPrimitive)
                il.Emit(OpCodes.Ldelema, elementType);
            il.Emit(OpCodes.Ldloc, local);
        }
        else
        {
            if (this.member != (MemberInfo)null && this.defaultMember != (MemberInfo)null)
            {
                this.member = this.defaultMember;
                this.defaultMember = (MemberInfo)null;
            }

            base.TranslateToILPreSetPlusGet(il, argList, inBrackets);
        }
    }

    internal override object TranslateToILReference(ILGenerator il, Type rtype)
    {
        if ((object)(this.member as FieldInfo) != null)
        {
            FieldInfo member = (FieldInfo)this.member;
            Type fieldType = member.FieldType;
            if (rtype == fieldType)
            {
                bool isStatic = member.IsStatic;
                if (!isStatic)
                    this.TranslateToILObject(il, member.DeclaringType, true);
                object obj1;
                switch (member)
                {
                    case JSField _:
                        obj1 = ((JSField)member).GetMetaData();
                        break;
                    case JSFieldInfo _:
                        obj1 = (object)((JSFieldInfo)member).field;
                        break;
                    default:
                        obj1 = (object)member;
                        break;
                }

                object obj2 = obj1;
                if ((object)(obj2 as FieldInfo) != null)
                {
                    if (member.IsInitOnly)
                    {
                        LocalBuilder local = il.DeclareLocal(fieldType);
                        il.Emit(isStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, (FieldInfo)obj2);
                        il.Emit(OpCodes.Stloc, local);
                        il.Emit(OpCodes.Ldloca, local);
                    }
                    else
                        il.Emit(isStatic ? OpCodes.Ldsflda : OpCodes.Ldflda, (FieldInfo)obj2);
                }
                else if (obj2 is LocalBuilder)
                    il.Emit(OpCodes.Ldloca, (LocalBuilder)obj2);
                else
                    il.Emit(OpCodes.Ldarga, (short)obj2);

                return (object)null;
            }
        }

        return base.TranslateToILReference(il, rtype);
    }

    internal override void TranslateToILSet(ILGenerator il, AST rhvalue)
    {
        if (this.isArrayElementAccess)
        {
            IReflect defaultMemberReturnIr = this.defaultMemberReturnIR;
            Type arrayClass = (object)(defaultMemberReturnIr as Type) != null
                ? (Type)defaultMemberReturnIr
                : Convert.ToType(defaultMemberReturnIr);
            int arrayRank = arrayClass.GetArrayRank();
            Type elementType = arrayClass.GetElementType();
            rhvalue?.TranslateToIL(il, elementType);
            if (arrayRank == 1)
            {
                Binding.TranslateToStelem(il, elementType);
            }
            else
            {
                Type[] parameterTypes = new Type[arrayRank + 1];
                for (int index = 0; index < arrayRank; ++index)
                    parameterTypes[index] = Typeob.Int32;
                parameterTypes[arrayRank] = elementType;
                MethodInfo arrayMethod = this.compilerGlobals.module.GetArrayMethod(arrayClass, "Set",
                    CallingConventions.HasThis, Typeob.Void, parameterTypes);
                il.Emit(OpCodes.Call, arrayMethod);
            }
        }
        else if (this.isAssignmentToDefaultIndexedProperty)
        {
            if ((object)(this.member as PropertyInfo) != null && this.defaultMember != (MemberInfo)null)
            {
                PropertyInfo member = (PropertyInfo)this.member;
                MethodInfo setMethod = JSProperty.GetSetMethod(member, false);
                JSWrappedMethod jsWrappedMethod = setMethod as JSWrappedMethod;
                if ((MethodInfo)jsWrappedMethod == (MethodInfo)null ||
                    !(jsWrappedMethod.GetWrappedObject() is GlobalObject))
                {
                    MethodInfo methodInfoMetadata = this.GetMethodInfoMetadata(setMethod);
                    rhvalue?.TranslateToIL(il, member.PropertyType);
                    if (methodInfoMetadata.IsVirtual && !methodInfoMetadata.IsFinal &&
                        (!methodInfoMetadata.ReflectedType.IsSealed || !methodInfoMetadata.ReflectedType.IsValueType))
                    {
                        il.Emit(OpCodes.Callvirt, methodInfoMetadata);
                        return;
                    }

                    il.Emit(OpCodes.Call, methodInfoMetadata);
                    return;
                }
            }

            base.TranslateToILSet(il, rhvalue);
        }
        else if ((object)(this.member as FieldInfo) != null)
        {
            FieldInfo member = (FieldInfo)this.member;
            rhvalue?.TranslateToIL(il, member.FieldType);
            if (member.IsLiteral || member.IsInitOnly)
            {
                il.Emit(OpCodes.Pop);
            }
            else
            {
                object obj;
                switch (member)
                {
                    case JSField _:
                        obj = ((JSField)member).GetMetaData();
                        break;
                    case JSFieldInfo _:
                        obj = (object)((JSFieldInfo)member).field;
                        break;
                    default:
                        obj = (object)member;
                        break;
                }

                object local = obj;
                FieldInfo field = local as FieldInfo;
                if (field != (FieldInfo)null)
                    il.Emit(field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, field);
                else if (local is LocalBuilder)
                    il.Emit(OpCodes.Stloc, (LocalBuilder)local);
                else
                    il.Emit(OpCodes.Starg, (short)local);
            }
        }
        else if ((object)(this.member as PropertyInfo) != null)
        {
            PropertyInfo member = (PropertyInfo)this.member;
            rhvalue?.TranslateToIL(il, member.PropertyType);
            MethodInfo setMethod = JSProperty.GetSetMethod(member, true);
            if (setMethod == (MethodInfo)null)
            {
                il.Emit(OpCodes.Pop);
            }
            else
            {
                MethodInfo methodInfoMetadata = this.GetMethodInfoMetadata(setMethod);
                if (methodInfoMetadata.IsStatic && !(methodInfoMetadata is JSClosureMethod))
                    il.Emit(OpCodes.Call, methodInfoMetadata);
                else if (!this.isNonVirtual && methodInfoMetadata.IsVirtual && !methodInfoMetadata.IsFinal &&
                         (!methodInfoMetadata.ReflectedType.IsSealed || !methodInfoMetadata.ReflectedType.IsValueType))
                    il.Emit(OpCodes.Callvirt, methodInfoMetadata);
                else
                    il.Emit(OpCodes.Call, methodInfoMetadata);
            }
        }
        else
        {
            object speculativeEarlyBoundSet = this.TranslateToSpeculativeEarlyBoundSet(il, rhvalue);
            rhvalue?.TranslateToIL(il, Typeob.Object);
            il.Emit(OpCodes.Call, CompilerGlobals.setValueMethod);
            if (speculativeEarlyBoundSet == null)
                return;
            il.MarkLabel((Label)speculativeEarlyBoundSet);
        }
    }

    /// <summary>When overridden in a derived class, generates Microsoft intermediate language (MSIL) code for the binding reference. If necessary, copies the current topmost value on the evaluation stack and then pushes the copy onto the evaluation stack.</summary>
    /// <param name="il">An object that generates Microsoft intermediate language (MSIL) instructions.</param>
    protected abstract void TranslateToILWithDupOfThisOb(ILGenerator il);

    private static void TranslateToLdelem(ILGenerator il, Type etype)
    {
        switch (Type.GetTypeCode(etype))
        {
            case TypeCode.Object:
            case TypeCode.Decimal:
            case TypeCode.DateTime:
            case TypeCode.String:
                if (etype.IsValueType)
                {
                    il.Emit(OpCodes.Ldelema, etype);
                    il.Emit(OpCodes.Ldobj, etype);
                    break;
                }

                il.Emit(OpCodes.Ldelem_Ref);
                break;
            case TypeCode.Boolean:
            case TypeCode.Byte:
                il.Emit(OpCodes.Ldelem_U1);
                break;
            case TypeCode.Char:
            case TypeCode.UInt16:
                il.Emit(OpCodes.Ldelem_U2);
                break;
            case TypeCode.SByte:
                il.Emit(OpCodes.Ldelem_I1);
                break;
            case TypeCode.Int16:
                il.Emit(OpCodes.Ldelem_I2);
                break;
            case TypeCode.Int32:
                il.Emit(OpCodes.Ldelem_I4);
                break;
            case TypeCode.UInt32:
                il.Emit(OpCodes.Ldelem_U4);
                break;
            case TypeCode.Int64:
            case TypeCode.UInt64:
                il.Emit(OpCodes.Ldelem_I8);
                break;
            case TypeCode.Single:
                il.Emit(OpCodes.Ldelem_R4);
                break;
            case TypeCode.Double:
                il.Emit(OpCodes.Ldelem_R8);
                break;
        }
    }

    private object TranslateToSpeculativeEarlyBoundSet(ILGenerator il, AST rhvalue)
    {
        this.giveErrors = false;
        object speculativeEarlyBoundSet = (object)null;
        bool flag = true;
        LocalBuilder local1 = (LocalBuilder)null;
        LocalBuilder local2 = (LocalBuilder)null;
        Label label = il.DefineLabel();
        MemberInfoList bindingsForThisName = this.GetAllKnownInstanceBindingsForThisName();
        int i = 0;
        for (int count = bindingsForThisName.count; i < count; ++i)
        {
            MemberInfo memberInfo = bindingsForThisName[i];
            FieldInfo field = (FieldInfo)null;
            MethodInfo method = (MethodInfo)null;
            PropertyInfo prop = (PropertyInfo)null;
            if ((object)(memberInfo as FieldInfo) != null)
            {
                field = (FieldInfo)memberInfo;
                if (field.IsLiteral || field.IsInitOnly)
                    continue;
            }
            else if ((object)(memberInfo as PropertyInfo) != null)
            {
                prop = (PropertyInfo)memberInfo;
                if (prop.GetIndexParameters().Length != 0 ||
                    (method = JSProperty.GetSetMethod(prop, true)) == (MethodInfo)null)
                    continue;
            }
            else
                continue;

            this.member = memberInfo;
            if (this.Accessible(true))
            {
                if (flag)
                {
                    flag = false;
                    if (rhvalue == null)
                    {
                        local2 = il.DeclareLocal(Typeob.Object);
                        il.Emit(OpCodes.Stloc, local2);
                    }

                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Ldfld, CompilerGlobals.objectField);
                    local1 = il.DeclareLocal(Typeob.Object);
                    il.Emit(OpCodes.Stloc, local1);
                    speculativeEarlyBoundSet = (object)il.DefineLabel();
                }

                Type declaringType = memberInfo.DeclaringType;
                il.Emit(OpCodes.Ldloc, local1);
                il.Emit(OpCodes.Isinst, declaringType);
                LocalBuilder local3 = il.DeclareLocal(declaringType);
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc, local3);
                il.Emit(OpCodes.Brfalse, label);
                il.Emit(OpCodes.Ldloc, local3);
                if (rhvalue == null)
                    il.Emit(OpCodes.Ldloc, local2);
                if (field != (FieldInfo)null)
                {
                    if (rhvalue == null)
                        Convert.Emit((AST)this, il, Typeob.Object, field.FieldType);
                    else
                        rhvalue.TranslateToIL(il, field.FieldType);
                    switch (field)
                    {
                        case JSField _:
                            il.Emit(OpCodes.Stfld, (FieldInfo)((JSField)field).GetMetaData());
                            break;
                        case JSFieldInfo _:
                            il.Emit(OpCodes.Stfld, ((JSFieldInfo)field).field);
                            break;
                        default:
                            il.Emit(OpCodes.Stfld, field);
                            break;
                    }
                }
                else
                {
                    if (rhvalue == null)
                        Convert.Emit((AST)this, il, Typeob.Object, prop.PropertyType);
                    else
                        rhvalue.TranslateToIL(il, prop.PropertyType);
                    MethodInfo methodInfoMetadata = this.GetMethodInfoMetadata(method);
                    if (methodInfoMetadata.IsVirtual && !methodInfoMetadata.IsFinal &&
                        (!declaringType.IsSealed || !declaringType.IsValueType))
                        il.Emit(OpCodes.Callvirt, methodInfoMetadata);
                    else
                        il.Emit(OpCodes.Call, methodInfoMetadata);
                }

                il.Emit(OpCodes.Pop);
                il.Emit(OpCodes.Br, (Label)speculativeEarlyBoundSet);
                il.MarkLabel(label);
                label = il.DefineLabel();
            }
        }

        if (local2 != null)
            il.Emit(OpCodes.Ldloc, local2);
        this.member = (MemberInfo)null;
        return speculativeEarlyBoundSet;
    }

    private object TranslateToSpeculativeEarlyBindings(
        ILGenerator il,
        Type rtype,
        bool getObjectFromLateBindingInstance)
    {
        this.giveErrors = false;
        object speculativeEarlyBindings = (object)null;
        bool flag = true;
        LocalBuilder local1 = (LocalBuilder)null;
        Label label = il.DefineLabel();
        MemberInfoList bindingsForThisName = this.GetAllKnownInstanceBindingsForThisName();
        int i = 0;
        for (int count = bindingsForThisName.count; i < count; ++i)
        {
            MemberInfo prop = bindingsForThisName[i];
            if ((object)(prop as FieldInfo) != null || (object)(prop as PropertyInfo) != null &&
                ((PropertyInfo)prop).GetIndexParameters().Length == 0 &&
                !(JSProperty.GetGetMethod((PropertyInfo)prop, true) == (MethodInfo)null))
            {
                this.member = prop;
                if (this.Accessible(false))
                {
                    if (flag)
                    {
                        flag = false;
                        if (getObjectFromLateBindingInstance)
                        {
                            il.Emit(OpCodes.Dup);
                            il.Emit(OpCodes.Ldfld, CompilerGlobals.objectField);
                        }
                        else
                            this.TranslateToILObject(il, Typeob.Object, false);

                        local1 = il.DeclareLocal(Typeob.Object);
                        il.Emit(OpCodes.Stloc, local1);
                        speculativeEarlyBindings = (object)il.DefineLabel();
                    }

                    Type declaringType = prop.DeclaringType;
                    il.Emit(OpCodes.Ldloc, local1);
                    il.Emit(OpCodes.Isinst, declaringType);
                    LocalBuilder local2 = il.DeclareLocal(declaringType);
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Stloc, local2);
                    il.Emit(OpCodes.Brfalse, label);
                    il.Emit(OpCodes.Ldloc, local2);
                    if ((object)(prop as FieldInfo) != null)
                    {
                        FieldInfo field = (FieldInfo)prop;
                        if (field.IsLiteral)
                        {
                            il.Emit(OpCodes.Pop);
                            continue;
                        }

                        switch (field)
                        {
                            case JSField _:
                                il.Emit(OpCodes.Ldfld, (FieldInfo)((JSField)field).GetMetaData());
                                break;
                            case JSFieldInfo _:
                                il.Emit(OpCodes.Ldfld, ((JSFieldInfo)field).field);
                                break;
                            default:
                                il.Emit(OpCodes.Ldfld, field);
                                break;
                        }

                        Convert.Emit((AST)this, il, field.FieldType, rtype);
                    }
                    else if ((object)(prop as PropertyInfo) != null)
                    {
                        MethodInfo methodInfoMetadata =
                            this.GetMethodInfoMetadata(JSProperty.GetGetMethod((PropertyInfo)prop, true));
                        if (methodInfoMetadata.IsVirtual && !methodInfoMetadata.IsFinal &&
                            (!declaringType.IsSealed || declaringType.IsValueType))
                            il.Emit(OpCodes.Callvirt, methodInfoMetadata);
                        else
                            il.Emit(OpCodes.Call, methodInfoMetadata);
                        Convert.Emit((AST)this, il, methodInfoMetadata.ReturnType, rtype);
                    }

                    il.Emit(OpCodes.Br, (Label)speculativeEarlyBindings);
                    il.MarkLabel(label);
                    label = il.DefineLabel();
                }
            }
        }

        il.MarkLabel(label);
        if (!flag && !getObjectFromLateBindingInstance)
            il.Emit(OpCodes.Ldloc, local1);
        this.member = (MemberInfo)null;
        return speculativeEarlyBindings;
    }

    private object TranslateToSpeculativeEarlyBoundCalls(
        ILGenerator il,
        Type rtype,
        ASTList argList,
        bool construct,
        bool brackets)
    {
        this.giveErrors = false;
        object speculativeEarlyBoundCalls = (object)null;
        bool flag = true;
        LocalBuilder local1 = (LocalBuilder)null;
        Label label = il.DefineLabel();
        IReflect[] allEligibleClasses = this.GetAllEligibleClasses();
        if (construct)
            return speculativeEarlyBoundCalls;
        foreach (IReflect reflect in allEligibleClasses)
        {
            MemberInfo[] member = reflect.GetMember(this.name,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            try
            {
                MemberInfo memberInfo = JSBinder.SelectCallableMember(member, this.argIRs);
                MethodInfo method;
                if (memberInfo != (MemberInfo)null && memberInfo.MemberType == MemberTypes.Property)
                {
                    method = ((PropertyInfo)memberInfo).GetGetMethod(true);
                    if (!(method == (MethodInfo)null))
                    {
                        ParameterInfo[] parameters;
                        if ((parameters = method.GetParameters()) != null)
                        {
                            if (parameters.Length == 0)
                                continue;
                        }
                        else
                            continue;
                    }
                    else
                        continue;
                }
                else
                    method = memberInfo as MethodInfo;

                if (method != (MethodInfo)null)
                {
                    if (Binding.CheckParameters(method.GetParameters(), this.argIRs, argList, this.context, 0, true,
                            false))
                    {
                        switch (method)
                        {
                            case JSFieldMethod _:
                                FunctionObject func = ((JSFieldMethod)method).func;
                                if (func != null && (func.attributes & MethodAttributes.VtableLayoutMask) ==
                                    MethodAttributes.PrivateScope)
                                {
                                    if (!((ClassScope)reflect).ParentIsInSamePackage())
                                        break;
                                    continue;
                                }

                                break;
                            case JSWrappedMethod _:
                                if (((JSMethod)method).obj is ClassScope)
                                {
                                    if (((JSMethod)method).GetPackage() != ((ClassScope)reflect).package)
                                        break;
                                    continue;
                                }

                                break;
                        }

                        this.member = (MemberInfo)method;
                        if (this.Accessible(false))
                        {
                            if (flag)
                            {
                                flag = false;
                                this.TranslateToILObject(il, Typeob.Object, false);
                                local1 = il.DeclareLocal(Typeob.Object);
                                il.Emit(OpCodes.Stloc, local1);
                                speculativeEarlyBoundCalls = (object)il.DefineLabel();
                            }

                            Type declaringType = method.DeclaringType;
                            il.Emit(OpCodes.Ldloc, local1);
                            il.Emit(OpCodes.Isinst, declaringType);
                            LocalBuilder local2 = il.DeclareLocal(declaringType);
                            il.Emit(OpCodes.Dup);
                            il.Emit(OpCodes.Stloc, local2);
                            il.Emit(OpCodes.Brfalse, label);
                            il.Emit(OpCodes.Ldloc, local2);
                            Binding.PlaceArgumentsOnStack(il, method.GetParameters(), argList, 0, 0,
                                (AST)Binding.ReflectionMissingCW);
                            MethodInfo methodInfoMetadata = this.GetMethodInfoMetadata(method);
                            if (methodInfoMetadata.IsVirtual && !methodInfoMetadata.IsFinal &&
                                (!declaringType.IsSealed || declaringType.IsValueType))
                                il.Emit(OpCodes.Callvirt, methodInfoMetadata);
                            else
                                il.Emit(OpCodes.Call, methodInfoMetadata);
                            Convert.Emit((AST)this, il, methodInfoMetadata.ReturnType, rtype);
                            il.Emit(OpCodes.Br, (Label)speculativeEarlyBoundCalls);
                            il.MarkLabel(label);
                            label = il.DefineLabel();
                        }
                    }
                }
            }
            catch (AmbiguousMatchException ex)
            {
            }
        }

        il.MarkLabel(label);
        if (!flag)
            il.Emit(OpCodes.Ldloc, local1);
        this.member = (MemberInfo)null;
        return speculativeEarlyBoundCalls;
    }

    internal static void TranslateToStelem(ILGenerator il, Type etype)
    {
        switch (Type.GetTypeCode(etype))
        {
            case TypeCode.Object:
            case TypeCode.Decimal:
            case TypeCode.DateTime:
            case TypeCode.String:
                if (etype.IsValueType)
                {
                    il.Emit(OpCodes.Stobj, etype);
                    break;
                }

                il.Emit(OpCodes.Stelem_Ref);
                break;
            case TypeCode.Boolean:
            case TypeCode.SByte:
            case TypeCode.Byte:
                il.Emit(OpCodes.Stelem_I1);
                break;
            case TypeCode.Char:
            case TypeCode.Int16:
            case TypeCode.UInt16:
                il.Emit(OpCodes.Stelem_I2);
                break;
            case TypeCode.Int32:
            case TypeCode.UInt32:
                il.Emit(OpCodes.Stelem_I4);
                break;
            case TypeCode.Int64:
            case TypeCode.UInt64:
                il.Emit(OpCodes.Stelem_I8);
                break;
            case TypeCode.Single:
                il.Emit(OpCodes.Stelem_R4);
                break;
            case TypeCode.Double:
                il.Emit(OpCodes.Stelem_R8);
                break;
        }
    }

    private void WarnIfNotFullyResolved()
    {
        if (this.isFullyResolved || this.member == (MemberInfo)null ||
            this.member is JSVariableField && ((JSVariableField)this.member).type == null ||
            !this.Engine.doFast && this.member is IWrappedMember)
            return;
        ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
        while (true)
        {
            switch (scriptObject)
            {
                case null:
                    goto label_1;
                case WithObject _ when !((WithObject)scriptObject).isKnownAtCompileTime:
                    goto label_3;
                case ActivationObject _ when !((ActivationObject)scriptObject).isKnownAtCompileTime:
                    goto label_4;
                default:
                    scriptObject = scriptObject.GetParent();
                    continue;
            }
        }

        label_3:
        this.context.HandleError(JSError.AmbiguousBindingBecauseOfWith);
        return;
        label_4:
        this.context.HandleError(JSError.AmbiguousBindingBecauseOfEval);
        return;
        label_1: ;
    }

    private void WarnIfObsolete() => Binding.WarnIfObsolete(this.member, this.context);

    internal static void WarnIfObsolete(MemberInfo member, Context context)
    {
        if (member == (MemberInfo)null)
            return;
        object[] customAttributes1 = CustomAttribute.GetCustomAttributes(member, typeof(ObsoleteAttribute), false);
        string message;
        bool treatAsError;
        if (customAttributes1 != null && customAttributes1.Length != 0)
        {
            ObsoleteAttribute obsoleteAttribute = (ObsoleteAttribute)customAttributes1[0];
            message = obsoleteAttribute.Message;
            treatAsError = obsoleteAttribute.IsError;
        }
        else
        {
            object[] customAttributes2 = CustomAttribute.GetCustomAttributes(member, typeof(NotRecommended), false);
            if (customAttributes2 == null || customAttributes2.Length == 0)
                return;
            message = ": " + ((NotRecommended)customAttributes2[0]).Message;
            treatAsError = false;
        }

        context.HandleError(JSError.Deprecated, message, treatAsError);
    }

    private MethodInfo GetMethodInfoMetadata(MethodInfo method)
    {
        switch (method)
        {
            case JSMethod _:
                return ((JSMethod)method).GetMethodInfo(this.compilerGlobals);
            case JSMethodInfo _:
                return ((JSMethodInfo)method).method;
            default:
                return method;
        }
    }
}