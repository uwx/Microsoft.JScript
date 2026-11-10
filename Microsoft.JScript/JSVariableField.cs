// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSVariableField
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Supports variable fields in JScript. This is the base class for parameters, local variables, global variables, and class members. </summary>
public abstract class JSVariableField : JSField
{
    internal ScriptObject obj;
    private string name;
    internal string debuggerName;
    internal object metaData;
    internal TypeExpression type;
    internal FieldAttributes attributeFlags;
    private MethodInfo method;
    internal object value;
    internal CustomAttributeList customAttributes;
    internal Context originalContext;
    internal CLSComplianceSpec clsCompliance;

    internal JSVariableField(string name, ScriptObject obj, FieldAttributes attributeFlags)
    {
        this.obj = obj;
        this.name = name;
        this.debuggerName = name;
        this.metaData = (object)null;
        if ((attributeFlags & FieldAttributes.FieldAccessMask) == FieldAttributes.PrivateScope)
            attributeFlags |= FieldAttributes.Public;
        this.attributeFlags = attributeFlags;
        this.type = (TypeExpression)null;
        this.method = (MethodInfo)null;
        this.value = (object)null;
        this.originalContext = (Context)null;
        this.clsCompliance = CLSComplianceSpec.NotAttributed;
    }

    /// <summary>Gets the attributes that are associated with this field.</summary>
    /// <returns>A bitwise combination of the enumeration values.</returns>
    public override FieldAttributes Attributes => this.attributeFlags;

    internal void CheckCLSCompliance(bool classIsCLSCompliant)
    {
        if (this.customAttributes != null)
        {
            CustomAttribute attribute = this.customAttributes.GetAttribute(Typeob.CLSCompliantAttribute);
            if (attribute != null)
            {
                this.clsCompliance = attribute.GetCLSComplianceValue();
                this.customAttributes.Remove(attribute);
            }
        }

        if (classIsCLSCompliant)
        {
            if (this.clsCompliance == CLSComplianceSpec.NonCLSCompliant || this.type == null ||
                this.type.IsCLSCompliant())
                return;
            this.clsCompliance = CLSComplianceSpec.NonCLSCompliant;
            if (this.originalContext == null)
                return;
            this.originalContext.HandleError(JSError.NonCLSCompliantMember);
        }
        else
        {
            if (this.clsCompliance != CLSComplianceSpec.CLSCompliant)
                return;
            this.originalContext.HandleError(JSError.MemberTypeCLSCompliantMismatch);
        }
    }

    /// <summary>Gets the type that declares this field.</summary>
    /// <returns>The Type object for the class that declares this member.</returns>
    public override Type DeclaringType
    {
        get { return this.obj is ClassScope ? ((ClassScope)this.obj).GetTypeBuilderOrEnumBuilder() : (Type)null; }
    }

    /// <summary>Gets the type of this field.</summary>
    /// <returns>The type of this field.</returns>
    public override Type FieldType
    {
        get
        {
            Type type = Typeob.Object;
            if (this.type != null)
            {
                type = this.type.ToType();
                if (type == Typeob.Void)
                    type = Typeob.Object;
            }

            return type;
        }
    }

    internal MethodInfo GetAsMethod(object obj)
    {
        if (this.method == (MethodInfo)null)
            this.method = (MethodInfo)new JSFieldMethod((FieldInfo)this, obj);
        return this.method;
    }

    internal override string GetClassFullName()
    {
        return this.obj is ClassScope
            ? ((ClassScope)this.obj).GetFullName()
            : throw new JScriptException(JSError.InternalError);
    }

    /// <summary>Returns an array that contains the custom attributes that are attached to this field.</summary>
    /// <returns>An array that contains the custom attributes that are attached to this field, or, if there are none, an array that has zero elements.</returns>
    /// <param name="inherit">true to search this member's inheritance hierarchy to find the attributes.</param>
    public override object[] GetCustomAttributes(bool inherit)
    {
        return this.customAttributes != null ? (object[])this.customAttributes.Evaluate() : new object[0];
    }

    internal virtual IReflect GetInferredType(JSField inference_target)
    {
        return this.type != null ? this.type.ToIReflect() : (IReflect)Typeob.Object;
    }

    internal override object GetMetaData() => this.metaData;

    internal override PackageScope GetPackage()
    {
        return this.obj is ClassScope
            ? ((ClassScope)this.obj).GetPackage()
            : throw new JScriptException(JSError.InternalError);
    }

    /// <summary>Gets the name of this field.</summary>
    /// <returns>The name of this field.</returns>
    public override string Name => this.name;

    internal void WriteCustomAttribute(bool doCRS)
    {
        if (!(this.metaData is FieldBuilder))
            return;
        FieldBuilder metaData = (FieldBuilder)this.metaData;
        if (this.customAttributes != null)
        {
            CustomAttributeBuilder[] attributeBuilders = this.customAttributes.GetCustomAttributeBuilders(false);
            int index = 0;
            for (int length = attributeBuilders.Length; index < length; ++index)
                metaData.SetCustomAttribute(attributeBuilders[index]);
        }

        if (this.clsCompliance == CLSComplianceSpec.CLSCompliant)
            metaData.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor,
                new object[1]
                {
                    (object)true
                }));
        else if (this.clsCompliance == CLSComplianceSpec.NonCLSCompliant)
            metaData.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor,
                new object[1]
                {
                    (object)false
                }));
        if (!doCRS || !this.IsStatic)
            return;
        metaData.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.contextStaticAttributeCtor,
            new object[0]));
    }
}