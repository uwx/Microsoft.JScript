// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.EnumDeclaration
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class EnumDeclaration : Class
{
    internal TypeExpression baseType;

    internal EnumDeclaration(
        Context context,
        IdentifierLiteral id,
        TypeExpression baseType,
        Block body,
        FieldAttributes attributes,
        CustomAttributeList customAttributes)
        : base(context, (AST)id, new TypeExpression((AST)new ConstantWrapper((object)Typeob.Enum, (Context)null)),
            new TypeExpression[0], body, attributes, false, false, true, false, customAttributes)
    {
        this.baseType = baseType != null
            ? baseType
            : new TypeExpression((AST)new ConstantWrapper((object)Typeob.Int32, (Context)null));
        this.needsEngine = false;
        this.attributes &= TypeAttributes.VisibilityMask;
        TypeExpression typeExpression =
            new TypeExpression((AST)new ConstantWrapper((object)this.classob, this.context));
        AST operand1 = (AST)new ConstantWrapper((object)-1, (Context)null);
        AST operand2 = (AST)new ConstantWrapper((object)1, (Context)null);
        foreach (JSVariableField field in this.fields)
        {
            field.attributeFlags = FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal;
            field.type = typeExpression;
            if (field.value == null)
                field.value = (object)(Plus)(operand1 = (AST)new Plus(operand1.context, operand1, operand2));
            else
                operand1 = (AST)field.value;
            field.value = (object)new DeclaredEnumValue(field.value, field.Name, this.classob);
        }
    }

    internal override AST PartiallyEvaluate()
    {
        if (!(this.classob.GetParent() is GlobalScope))
            return (AST)this;
        this.baseType.PartiallyEvaluate();
        IReflect ireflect = this.baseType.ToIReflect();
        Type bt;
        if ((object)(ireflect as Type) == null || !Convert.IsPrimitiveIntegerType(bt = (Type)ireflect))
        {
            this.baseType.context.HandleError(JSError.InvalidBaseTypeForEnum);
            this.baseType = new TypeExpression((AST)new ConstantWrapper((object)Typeob.Int32, (Context)null));
            bt = Typeob.Int32;
        }

        if (this.customAttributes != null)
            this.customAttributes.PartiallyEvaluate();
        if (this.NeedsToBeCheckedForCLSCompliance())
        {
            if (!TypeExpression.TypeIsCLSCompliant((object)ireflect))
                this.baseType.context.HandleError(JSError.NonCLSCompliantType);
            this.CheckMemberNamesForCLSCompliance();
        }

        ScriptObject parent = this.enclosingScope;
        while (true)
        {
            switch (parent)
            {
                case GlobalScope _:
                case PackageScope _:
                    goto label_13;
                default:
                    parent = parent.GetParent();
                    continue;
            }
        }

        label_13:
        this.classob.SetParent((ScriptObject)new WithObject(parent, (object)Typeob.Enum, true));
        this.Globals.ScopeStack.Push((object)this.classob);
        try
        {
            foreach (JSMemberField field in this.fields)
                ((DeclaredEnumValue)field.value).CoerceToBaseType(bt, field.originalContext);
        }
        finally
        {
            this.Globals.ScopeStack.Pop();
        }

        return (AST)this;
    }

    internal override Type GetTypeBuilderOrEnumBuilder()
    {
        if (this.classob.classwriter != (Type)null)
            return this.classob.classwriter;
        this.PartiallyEvaluate();
        if (this.enclosingScope is ClassScope enclosingScope)
        {
            TypeBuilder builderOrEnumBuilder = ((TypeBuilder)enclosingScope.classwriter).DefineNestedType(this.name,
                this.attributes | TypeAttributes.Sealed, Typeob.Enum, (Type[])null);
            this.classob.classwriter = (Type)builderOrEnumBuilder;
            Type type = this.baseType.ToType();
            builderOrEnumBuilder.DefineField("value__", type, FieldAttributes.Private | FieldAttributes.SpecialName);
            if (this.customAttributes != null)
            {
                foreach (CustomAttributeBuilder attributeBuilder in this.customAttributes.GetCustomAttributeBuilders(
                             false))
                    builderOrEnumBuilder.SetCustomAttribute(attributeBuilder);
            }

            foreach (FieldInfo field in this.fields)
                ((FieldBuilder)(((JSVariableField)field).metaData = (object)builderOrEnumBuilder.DefineField(field.Name,
                        (Type)builderOrEnumBuilder,
                        FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal)))
                    .SetConstant(((EnumWrapper)field.GetValue((object)null)).ToNumericValue());
            return (Type)builderOrEnumBuilder;
        }

        EnumBuilder builderOrEnumBuilder1 =
            this.compilerGlobals.module.DefineEnum(this.name, this.attributes, this.baseType.ToType());
        this.classob.classwriter = (Type)builderOrEnumBuilder1;
        if (this.customAttributes != null)
        {
            foreach (CustomAttributeBuilder attributeBuilder in this.customAttributes.GetCustomAttributeBuilders(false))
                builderOrEnumBuilder1.SetCustomAttribute(attributeBuilder);
        }

        foreach (FieldInfo field in this.fields)
            ((JSVariableField)field).metaData = (object)builderOrEnumBuilder1.DefineLiteral(field.Name,
                ((EnumWrapper)field.GetValue((object)null)).ToNumericValue());
        return (Type)builderOrEnumBuilder1;
    }
}