// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ParameterDeclaration
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class ParameterDeclaration : ParameterInfo
{
    internal string identifier;
    internal TypeExpression type;
    internal Context context;
    internal CustomAttributeList customAttributes;

    internal ParameterDeclaration(
        Context context,
        string identifier,
        TypeExpression type,
        CustomAttributeList customAttributes)
    {
        this.identifier = identifier;
        this.type = type == null ? new TypeExpression((AST)new ConstantWrapper((object)Typeob.Object, context)) : type;
        this.context = context;
        ActivationObject activationObject = (ActivationObject)context.document.engine.Globals.ScopeStack.Peek();
        if (activationObject.name_table[(object)this.identifier] != null)
            context.HandleError(JSError.DuplicateName, this.identifier,
                activationObject is ClassScope || activationObject.fast || type != null);
        else
            activationObject.AddNewField(this.identifier, (object)null, FieldAttributes.PrivateScope).originalContext =
                context;
        this.customAttributes = customAttributes;
    }

    internal ParameterDeclaration(Type type, string identifier)
    {
        this.identifier = identifier;
        this.type = new TypeExpression((AST)new ConstantWrapper((object)type, (Context)null));
        this.customAttributes = (CustomAttributeList)null;
    }

    public override object DefaultValue => System.Convert.DBNull;

    public override object[] GetCustomAttributes(bool inherit) => (object[])new FieldInfo[0];

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        return (object[])new FieldInfo[0];
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
        return this.customAttributes != null && this.customAttributes.GetAttribute(attributeType) != null;
    }

    public override string Name => this.identifier;

    internal IReflect ParameterIReflect => this.type.ToIReflect();

    public override Type ParameterType
    {
        get
        {
            Type type = this.type.ToType();
            if (type == Typeob.Void)
                type = Typeob.Object;
            return type;
        }
    }

    internal void PartiallyEvaluate()
    {
        if (this.type != null)
            this.type.PartiallyEvaluate();
        if (this.customAttributes == null)
            return;
        this.customAttributes.PartiallyEvaluate();
        if (!CustomAttribute.IsDefined((ParameterInfo)this, typeof(ParamArrayAttribute), false))
            return;
        if (this.type != null)
        {
            IReflect ireflect = this.type.ToIReflect();
            if ((object)(ireflect as Type) != null && ((Type)ireflect).IsArray || ireflect is TypedArray)
                return;
        }

        this.customAttributes.context.HandleError(JSError.IllegalParamArrayAttribute);
    }
}