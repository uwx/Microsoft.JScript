// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Constant
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class Constant : AST
{
    internal FieldAttributes attributes;
    private Completion completion;
    internal CustomAttributeList customAttributes;
    internal JSVariableField field;
    private FieldBuilder valueField;
    private Lookup identifier;
    internal string name;
    internal AST value;

    internal Constant(
        Context context,
        Lookup identifier,
        TypeExpression type,
        AST value,
        FieldAttributes attributes,
        CustomAttributeList customAttributes)
        : base(context)
    {
        this.attributes = attributes | FieldAttributes.InitOnly;
        this.customAttributes = customAttributes;
        this.completion = new Completion();
        this.identifier = identifier;
        this.name = identifier.ToString();
        this.value = value;
        ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
        while (scriptObject is WithObject)
            scriptObject = scriptObject.GetParent();
        if (scriptObject is ClassScope)
        {
            if (this.name == ((ClassScope)scriptObject).name)
            {
                identifier.context.HandleError(JSError.CannotUseNameOfClass);
                this.name += " const";
            }

            if (attributes == FieldAttributes.PrivateScope)
                attributes = FieldAttributes.Public;
        }
        else
        {
            if (attributes != FieldAttributes.PrivateScope)
                this.context.HandleError(JSError.NotInsideClass);
            attributes = FieldAttributes.Public;
        }

        if (((IActivationObject)scriptObject).GetLocalField(this.name) != (FieldInfo)null)
        {
            identifier.context.HandleError(JSError.DuplicateName, true);
            this.name += " const";
        }

        this.field = !(scriptObject is ActivationObject)
            ? ((StackFrame)scriptObject).AddNewField(this.identifier.ToString(), (object)value,
                attributes | FieldAttributes.Static)
            : ((ActivationObject)scriptObject).AddNewField(this.identifier.ToString(), (object)value, attributes);
        this.field.type = type;
        this.field.customAttributes = customAttributes;
        this.field.originalContext = context;
        if (!(this.field is JSLocalField))
            return;
        ((JSLocalField)this.field).debugOn = this.identifier.context.document.debugOn;
    }

    internal override object Evaluate()
    {
        this.completion.value = this.value != null ? this.value.Evaluate() : this.field.value;
        return (object)this.completion;
    }

    internal override AST PartiallyEvaluate()
    {
        this.field.attributeFlags &= ~FieldAttributes.InitOnly;
        this.identifier.PartiallyEvaluateAsReference();
        if (this.field.type != null)
            this.field.type.PartiallyEvaluate();
        this.Globals.ScopeStack.Peek();
        if (this.value != null)
        {
            this.value = this.value.PartiallyEvaluate();
            this.identifier.SetPartialValue(this.value);
            if (this.value is ConstantWrapper)
            {
                object ob = this.field.value = this.value.Evaluate();
                if (this.field.type != null)
                    this.field.value = Convert.Coerce(ob, (object)this.field.type, true);
                if (this.field.IsStatic && ((object)(ob as Type) != null || ob is ClassScope || ob is TypedArray ||
                                            Convert.GetTypeCode(ob) != TypeCode.Object))
                {
                    this.field.attributeFlags |= FieldAttributes.Literal;
                    goto label_9;
                }
            }

            this.field.attributeFlags |= FieldAttributes.InitOnly;
            label_9:
            if (this.field.type == null)
                this.field.type =
                    new TypeExpression((AST)new ConstantWrapper((object)this.value.InferType((JSField)null),
                        (Context)null));
        }
        else
        {
            this.value = (AST)new ConstantWrapper((object)null, this.context);
            this.field.attributeFlags |= FieldAttributes.InitOnly;
        }

        if ((FieldInfo)this.field != (FieldInfo)null && this.field.customAttributes != null)
            this.field.customAttributes.PartiallyEvaluate();
        return (AST)this;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        if ((this.field.attributeFlags & FieldAttributes.Literal) != FieldAttributes.PrivateScope)
        {
            object obj = this.field.value;
            if ((object)(obj as Type) == null)
            {
                switch (obj)
                {
                    case ClassScope _:
                    case TypedArray _:
                        break;
                    default:
                        return;
                }
            }

            this.field.attributeFlags &= ~FieldAttributes.Literal;
            this.identifier.TranslateToILPreSet(il);
            this.identifier.TranslateToILSet(il, (AST)new ConstantWrapper(obj, (Context)null));
            this.field.attributeFlags |= FieldAttributes.Literal;
        }
        else
        {
            if (!this.field.IsStatic)
            {
                FieldBuilder fieldBuilder = this.valueField = this.field.metaData as FieldBuilder;
                if ((FieldInfo)fieldBuilder != (FieldInfo)null)
                    this.field.metaData =
                        (object)((TypeBuilder)fieldBuilder.DeclaringType).DefineField(this.name + " value",
                            this.field.type.ToType(), FieldAttributes.Private);
            }

            this.field.attributeFlags &= ~FieldAttributes.InitOnly;
            this.identifier.TranslateToILPreSet(il);
            this.identifier.TranslateToILSet(il, this.value);
            this.field.attributeFlags |= FieldAttributes.InitOnly;
        }
    }

    internal void TranslateToILInitOnlyInitializers(ILGenerator il)
    {
        FieldBuilder valueField = this.valueField;
        if (!((FieldInfo)valueField != (FieldInfo)null))
            return;
        il.Emit(OpCodes.Ldarg_0);
        il.Emit(OpCodes.Dup);
        il.Emit(OpCodes.Ldfld, (FieldInfo)this.field.metaData);
        il.Emit(OpCodes.Stfld, (FieldInfo)valueField);
        this.valueField = (FieldBuilder)this.field.metaData;
        this.field.metaData = (object)valueField;
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        if (this.value == null)
            return;
        this.value.TranslateToILInitializer(il);
    }
}