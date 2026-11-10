// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.FunctionDeclaration
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a function declaration. This class belongs to the abstract syntax tree category.</summary>
public sealed class FunctionDeclaration : AST
{
    internal FunctionObject func;
    private Member declaringObject;
    private TypeExpression ifaceId;
    private string name;
    internal bool isMethod;
    private bool inFastScope;
    private JSVariableField field;
    internal JSProperty enclosingProperty;
    private Completion completion = new Completion();

    internal FunctionDeclaration(
        Context context,
        AST ifaceId,
        IdentifierLiteral id,
        ParameterDeclaration[] formal_parameters,
        TypeExpression return_type,
        Block body,
        FunctionScope own_scope,
        FieldAttributes attributes,
        bool isMethod,
        bool isGetter,
        bool isSetter,
        bool isAbstract,
        bool isFinal,
        CustomAttributeList customAttributes)
        : base(context)
    {
        MethodAttributes methodAttributes1 = (attributes & FieldAttributes.FieldAccessMask) != FieldAttributes.Public
            ? ((attributes & FieldAttributes.FieldAccessMask) != FieldAttributes.Private
                ? ((attributes & FieldAttributes.FieldAccessMask) != FieldAttributes.Assembly
                    ? ((attributes & FieldAttributes.FieldAccessMask) != FieldAttributes.Family
                        ? ((attributes & FieldAttributes.FieldAccessMask) != FieldAttributes.FamORAssem
                            ? MethodAttributes.Public
                            : MethodAttributes.FamORAssem)
                        : MethodAttributes.Family)
                    : MethodAttributes.Assembly)
                : MethodAttributes.Private)
            : MethodAttributes.Public;
        MethodAttributes attributes1 =
            (attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope || !isMethod
                ? methodAttributes1 | MethodAttributes.Static
                : methodAttributes1 | MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask;
        if (isAbstract)
            attributes1 |= MethodAttributes.Abstract;
        if (isFinal)
            attributes1 |= MethodAttributes.Final;
        this.name = id.ToString();
        this.isMethod = isMethod;
        if (ifaceId != null)
        {
            if (isMethod)
            {
                this.ifaceId = new TypeExpression(ifaceId);
                attributes1 = attributes1 & ~MethodAttributes.MemberAccessMask | MethodAttributes.Private |
                              MethodAttributes.Final;
            }
            else
            {
                this.declaringObject = new Member(ifaceId.context, ifaceId, (AST)id);
                this.name = this.declaringObject.ToString();
            }
        }

        ScriptObject enclosing_scope = this.Globals.ScopeStack.Peek();
        if (attributes == FieldAttributes.PrivateScope && !isAbstract && !isFinal)
        {
            if (enclosing_scope is ClassScope)
                attributes |= FieldAttributes.Public;
        }
        else if (!(enclosing_scope is ClassScope))
        {
            this.context.HandleError(JSError.NotInsideClass);
            attributes = FieldAttributes.PrivateScope;
            attributes1 = MethodAttributes.Public;
        }

        if (enclosing_scope is ActivationObject)
        {
            this.inFastScope = ((ActivationObject)enclosing_scope).fast;
            string name = this.name;
            if (isGetter)
            {
                attributes1 |= MethodAttributes.SpecialName;
                this.name = "get_" + this.name;
                if (return_type == null)
                    return_type = new TypeExpression((AST)new ConstantWrapper((object)Typeob.Object, context));
            }
            else if (isSetter)
            {
                attributes1 |= MethodAttributes.SpecialName;
                this.name = "set_" + this.name;
                return_type = new TypeExpression((AST)new ConstantWrapper((object)Typeob.Void, context));
            }

            attributes &= FieldAttributes.FieldAccessMask;
            MethodAttributes methodAttributes2 = attributes1 & MethodAttributes.MemberAccessMask;
            if ((attributes1 & MethodAttributes.Virtual) != MethodAttributes.PrivateScope &&
                (attributes1 & MethodAttributes.Final) == MethodAttributes.PrivateScope &&
                (methodAttributes2 == MethodAttributes.Private || methodAttributes2 == MethodAttributes.Assembly ||
                 methodAttributes2 == MethodAttributes.FamANDAssem))
                attributes1 |= MethodAttributes.CheckAccessOnOverride;
            this.func = new FunctionObject(this.name, formal_parameters, return_type, body, own_scope, enclosing_scope,
                this.context, attributes1, customAttributes, this.isMethod);
            if (this.declaringObject != null)
                return;
            string str1 = this.name;
            if (this.ifaceId != null)
                str1 = $"{ifaceId.ToString()}.{str1}";
            JSVariableField jsVariableField =
                (JSVariableField)((ActivationObject)enclosing_scope).name_table[(object)str1];
            if ((FieldInfo)jsVariableField != (FieldInfo)null && (!(jsVariableField is JSMemberField) ||
                                                                  !(jsVariableField.value is FunctionObject) ||
                                                                  this.func.isExpandoMethod))
            {
                if (name != this.name)
                {
                    jsVariableField.originalContext.HandleError(JSError.ClashWithProperty);
                }
                else
                {
                    id.context.HandleError(JSError.DuplicateName, this.func.isExpandoMethod);
                    if (jsVariableField.value is FunctionObject)
                        ((FunctionObject)jsVariableField.value).suppressIL = true;
                }
            }

            if (this.isMethod)
            {
                if (!(jsVariableField is JSMemberField) || !(jsVariableField.value is FunctionObject) ||
                    name != this.name)
                {
                    this.field = ((ActivationObject)enclosing_scope).AddNewField(str1, (object)this.func,
                        attributes | FieldAttributes.Literal);
                    if (name == this.name)
                        this.field.type =
                            new TypeExpression((AST)new ConstantWrapper((object)Typeob.FunctionWrapper, this.context));
                }
                else
                    this.field =
                        (JSVariableField)((JSMemberField)jsVariableField).AddOverload(this.func,
                            attributes | FieldAttributes.Literal);
            }
            else if (enclosing_scope is FunctionScope)
            {
                if (this.inFastScope)
                    attributes |= FieldAttributes.Literal;
                this.field = ((FunctionScope)enclosing_scope).AddNewField(this.name, attributes, this.func);
                if (this.field is JSLocalField)
                {
                    JSLocalField field = (JSLocalField)this.field;
                    if (this.inFastScope)
                    {
                        field.type =
                            new TypeExpression((AST)new ConstantWrapper((object)Typeob.ScriptFunction, this.context));
                        field.attributeFlags |= FieldAttributes.Literal;
                    }

                    field.debugOn = this.context.document.debugOn;
                    field.isDefined = true;
                }
            }
            else if (this.inFastScope)
            {
                this.field = ((ActivationObject)enclosing_scope).AddNewField(this.name, (object)this.func,
                    attributes | FieldAttributes.Literal);
                this.field.type =
                    new TypeExpression((AST)new ConstantWrapper((object)Typeob.ScriptFunction, this.context));
            }
            else
                this.field = ((ActivationObject)enclosing_scope).AddNewField(this.name, (object)this.func,
                    attributes | FieldAttributes.Static);

            this.field.originalContext = context;
            if (!(name != this.name))
                return;
            string str2 = name;
            if (this.ifaceId != null)
                str2 = $"{ifaceId.ToString()}.{name}";
            FieldInfo fieldInfo = (FieldInfo)((ActivationObject)enclosing_scope).name_table[(object)str2];
            if (fieldInfo != (FieldInfo)null)
            {
                if (fieldInfo.IsLiteral)
                {
                    object obj = ((JSVariableField)fieldInfo).value;
                    if (obj is JSProperty)
                        this.enclosingProperty = (JSProperty)obj;
                }

                if ((PropertyInfo)this.enclosingProperty == (PropertyInfo)null)
                    id.context.HandleError(JSError.DuplicateName, true);
            }

            if ((PropertyInfo)this.enclosingProperty == (PropertyInfo)null)
            {
                this.enclosingProperty = new JSProperty(name);
                ((ActivationObject)enclosing_scope)
                    .AddNewField(str2, (object)this.enclosingProperty, attributes | FieldAttributes.Literal)
                    .originalContext = this.context;
            }
            else if (isGetter && (MethodInfo)this.enclosingProperty.getter != (MethodInfo)null ||
                     isSetter && (MethodInfo)this.enclosingProperty.setter != (MethodInfo)null)
                id.context.HandleError(JSError.DuplicateName, true);

            if (isGetter)
                this.enclosingProperty.getter =
                    (JSMethod)new JSFieldMethod((FieldInfo)this.field, (object)enclosing_scope);
            else
                this.enclosingProperty.setter =
                    (JSMethod)new JSFieldMethod((FieldInfo)this.field, (object)enclosing_scope);
        }
        else
        {
            this.inFastScope = false;
            this.func = new FunctionObject(this.name, formal_parameters, return_type, body, own_scope, enclosing_scope,
                this.context, MethodAttributes.Public, (CustomAttributeList)null, false);
            this.field = ((StackFrame)enclosing_scope).AddNewField(this.name, (object)new Closure(this.func),
                attributes | FieldAttributes.Static);
        }
    }

    internal override object Evaluate()
    {
        if (this.declaringObject != null)
            this.declaringObject.SetValue((object)this.func);
        return (object)this.completion;
    }

    /// <summary>Creates a new closure function, using the specified information.</summary>
    /// <returns>A new closure function that represents the function declaration.</returns>
    /// <param name="handle">A handle to the type of the function.</param>
    /// <param name="name">The name of the function.</param>
    /// <param name="method_name">The name of the function.</param>
    /// <param name="formal_parameters">The parameters of the function.</param>
    /// <param name="fields">The fields of the function.</param>
    /// <param name="must_save_stack_locals">true to save local stack variables. This is important for a nested function that accesses one of its local variables or for an eval statement.</param>
    /// <param name="hasArgumentsObject">true if the function has a parameter array.</param>
    /// <param name="text">The text of the function declaration.</param>
    /// <param name="declaringObject">Gets the type that declares this method.</param>
    /// <param name="engine">A reference to the scripting engine.</param>
    public static Closure JScriptFunctionDeclaration(
        RuntimeTypeHandle handle,
        string name,
        string method_name,
        string[] formal_parameters,
        JSLocalField[] fields,
        bool must_save_stack_locals,
        bool hasArgumentsObject,
        string text,
        object declaringObject,
        VsaEngine engine)
    {
        return new Closure(
            new FunctionObject(Type.GetTypeFromHandle(handle), name, method_name, formal_parameters, fields,
                must_save_stack_locals, hasArgumentsObject, text, engine), declaringObject);
    }

    internal override Context GetFirstExecutableContext() => (Context)null;

    internal override AST PartiallyEvaluate()
    {
        if (this.ifaceId != null)
        {
            this.ifaceId.PartiallyEvaluate();
            this.func.implementedIface = this.ifaceId.ToIReflect();
            Type implementedIface1 = this.func.implementedIface as Type;
            ClassScope implementedIface2 = this.func.implementedIface as ClassScope;
            if (implementedIface1 != (Type)null && !implementedIface1.IsInterface ||
                implementedIface2 != null && !implementedIface2.owner.isInterface)
            {
                this.ifaceId.context.HandleError(JSError.NeedInterface);
                this.func.implementedIface = (IReflect)null;
            }

            if ((this.func.attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
                this.func.funcContext.HandleError(JSError.AbstractCannotBePrivate);
        }
        else if (this.declaringObject != null)
            this.declaringObject.PartiallyEvaluateAsCallable();

        this.func.PartiallyEvaluate();
        if (this.inFastScope && this.func.isExpandoMethod && (FieldInfo)this.field != (FieldInfo)null &&
            this.field.type != null)
            this.field.type.expression = (AST)new ConstantWrapper((object)Typeob.ScriptFunction, (Context)null);
        if ((this.func.attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope &&
            !((ClassScope)this.func.enclosing_scope).owner.isAbstract)
        {
            ((ClassScope)this.func.enclosing_scope).owner.attributes |= TypeAttributes.Abstract;
            ((ClassScope)this.func.enclosing_scope).owner.context.HandleError(JSError.CannotBeAbstract, this.name);
        }

        if ((PropertyInfo)this.enclosingProperty != (PropertyInfo)null &&
            !this.enclosingProperty.GetterAndSetterAreConsistent())
            this.context.HandleError(JSError.GetAndSetAreInconsistent);
        return (AST)this;
    }

    private void TranslateToILClosure(ILGenerator il)
    {
        if (!this.func.isStatic)
            il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Ldtoken,
            (Type)this.func.classwriter != (Type)null
                ? (Type)this.func.classwriter
                : (Type)this.compilerGlobals.classwriter);
        il.Emit(OpCodes.Ldstr, this.name);
        il.Emit(OpCodes.Ldstr, this.func.GetName());
        int length1 = this.func.formal_parameters.Length;
        ConstantWrapper.TranslateToILInt(il, length1);
        il.Emit(OpCodes.Newarr, Typeob.String);
        for (int i = 0; i < length1; ++i)
        {
            il.Emit(OpCodes.Dup);
            ConstantWrapper.TranslateToILInt(il, i);
            il.Emit(OpCodes.Ldstr, this.func.formal_parameters[i]);
            il.Emit(OpCodes.Stelem_Ref);
        }

        int length2 = this.func.fields.Length;
        ConstantWrapper.TranslateToILInt(il, length2);
        il.Emit(OpCodes.Newarr, Typeob.JSLocalField);
        for (int i = 0; i < length2; ++i)
        {
            JSLocalField field = this.func.fields[i];
            il.Emit(OpCodes.Dup);
            ConstantWrapper.TranslateToILInt(il, i);
            il.Emit(OpCodes.Ldstr, field.Name);
            il.Emit(OpCodes.Ldtoken, field.FieldType);
            ConstantWrapper.TranslateToILInt(il, field.slotNumber);
            il.Emit(OpCodes.Newobj, CompilerGlobals.jsLocalFieldConstructor);
            il.Emit(OpCodes.Stelem_Ref);
        }

        if (this.func.must_save_stack_locals)
            il.Emit(OpCodes.Ldc_I4_1);
        else
            il.Emit(OpCodes.Ldc_I4_0);
        if (this.func.hasArgumentsObject)
            il.Emit(OpCodes.Ldc_I4_1);
        else
            il.Emit(OpCodes.Ldc_I4_0);
        il.Emit(OpCodes.Ldstr, this.func.ToString());
        if (!this.func.isStatic)
            il.Emit(OpCodes.Ldarg_0);
        else
            il.Emit(OpCodes.Ldnull);
        this.EmitILToLoadEngine(il);
        il.Emit(OpCodes.Call, CompilerGlobals.jScriptFunctionDeclarationMethod);
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        if (this.func.suppressIL)
            return;
        this.func.TranslateToIL(this.compilerGlobals);
        if (this.declaringObject != null)
        {
            this.declaringObject.TranslateToILInitializer(il);
            this.declaringObject.TranslateToILPreSet(il);
            this.TranslateToILClosure(il);
            this.declaringObject.TranslateToILSet(il);
        }
        else
        {
            object metaData = this.field.metaData;
            if (this.func.isMethod)
            {
                if (metaData is FunctionDeclaration)
                    this.field.metaData = (object)null;
                else
                    this.TranslateToILSourceTextProvider();
            }
            else
            {
                if (metaData == null)
                    return;
                this.TranslateToILClosure(il);
                if (metaData is LocalBuilder)
                    il.Emit(OpCodes.Stloc, (LocalBuilder)metaData);
                else if (this.func.isStatic)
                    il.Emit(OpCodes.Stsfld, (FieldInfo)metaData);
                else
                    il.Emit(OpCodes.Stfld, (FieldInfo)metaData);
            }
        }
    }

    private void TranslateToILSourceTextProvider()
    {
        if (this.Engine.doFast || string.Compare(this.name, this.field.Name, StringComparison.Ordinal) != 0)
            return;
        StringBuilder stringBuilder = new StringBuilder(this.func.ToString());
        for (JSMemberField nextOverload = ((JSMemberField)this.field).nextOverload;
             (FieldInfo)nextOverload != (FieldInfo)null;
             nextOverload = nextOverload.nextOverload)
        {
            nextOverload.metaData = (object)this;
            stringBuilder.Append('\n');
            stringBuilder.Append(nextOverload.value.ToString());
        }

        MethodAttributes attributes = MethodAttributes.Public | MethodAttributes.Static;
        ILGenerator ilGenerator = ((ClassScope)this.func.enclosing_scope).GetTypeBuilder()
            .DefineMethod(this.name + " source", attributes, Typeob.String, new Type[0]).GetILGenerator();
        ilGenerator.Emit(OpCodes.Ldstr, stringBuilder.ToString());
        ilGenerator.Emit(OpCodes.Ret);
    }
}