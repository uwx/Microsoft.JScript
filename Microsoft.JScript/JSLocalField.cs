// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSLocalField
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a local variable.</summary>
public sealed class JSLocalField : JSVariableField
{
    internal int slotNumber;
    internal IReflect inferred_type;
    private ArrayList dependents;
    internal bool debugOn;
    internal JSLocalField outerField;
    internal bool isDefined;
    internal bool isUsedBeforeDefinition;

    /// <summary>Initializes a new instance of the <see cref="T:Microsoft.JScript.JSLocalField" /> class, using the specified name, handle, and slot number.</summary>
    /// <param name="name">The name of this field.</param>
    /// <param name="handle">A handle to the internal metadata representation of this field.</param>
    /// <param name="slotNumber">The slot number of the variable.</param>
    public JSLocalField(string name, RuntimeTypeHandle handle, int slotNumber)
        : this(name, (FunctionScope)null, slotNumber, (object)Missing.Value)
    {
        this.type = new TypeExpression((AST)new ConstantWrapper((object)Type.GetTypeFromHandle(handle), (Context)null));
        this.isDefined = true;
    }

    internal JSLocalField(string name, FunctionScope scope, int slotNumber, object value)
        : base(name, (ScriptObject)scope, FieldAttributes.Public | FieldAttributes.Static)
    {
        this.slotNumber = slotNumber;
        this.inferred_type = (IReflect)null;
        this.dependents = (ArrayList)null;
        this.value = value;
        this.debugOn = false;
        this.outerField = (JSLocalField)null;
        this.isDefined = false;
        this.isUsedBeforeDefinition = false;
    }

    /// <summary>Gets the type of this field.</summary>
    /// <returns>The type of this field.</returns>
    public override Type FieldType
    {
        get { return this.type != null ? base.FieldType : Convert.ToType(this.GetInferredType((JSField)null)); }
    }

    internal override IReflect GetInferredType(JSField inference_target)
    {
        if ((FieldInfo)this.outerField != (FieldInfo)null)
            return this.outerField.GetInferredType(inference_target);
        if (this.type != null)
            return base.GetInferredType(inference_target);
        if (this.inferred_type == null || (object)this.inferred_type == (object)Typeob.Object)
            return (IReflect)Typeob.Object;
        if ((FieldInfo)inference_target != (FieldInfo)null && (FieldInfo)inference_target != (FieldInfo)this)
        {
            if (this.dependents == null)
                this.dependents = new ArrayList();
            this.dependents.Add((object)inference_target);
        }

        return this.inferred_type;
    }

    /// <summary>Gets the value of the field.</summary>
    /// <returns>The value of the field.</returns>
    /// <param name="obj">The object whose field value will be returned.</param>
    public override object GetValue(object obj)
    {
        if ((this.attributeFlags & FieldAttributes.Literal) != FieldAttributes.PrivateScope &&
            !(this.value is FunctionObject))
            return this.value;
        while (obj is BlockScope)
            obj = (object)((ScriptObject)obj).GetParent();
        StackFrame stackFrame = (StackFrame)obj;
        JSLocalField outerField = this.outerField;
        int slotNumber = this.slotNumber;
        for (; (FieldInfo)outerField != (FieldInfo)null; outerField = outerField.outerField)
        {
            slotNumber = outerField.slotNumber;
            stackFrame = (StackFrame)stackFrame.GetParent();
        }

        return stackFrame.localVars[slotNumber];
    }

    internal void SetInferredType(IReflect ir, AST expr)
    {
        this.isDefined = true;
        if (this.type != null)
            return;
        if ((FieldInfo)this.outerField != (FieldInfo)null)
        {
            this.outerField.SetInferredType(ir, expr);
        }
        else
        {
            if (Convert.IsPrimitiveNumericTypeFitForDouble(ir))
                ir = (IReflect)Typeob.Double;
            else if ((object)ir == (object)Typeob.Void)
                ir = (IReflect)Typeob.Object;
            if (this.inferred_type == null)
            {
                this.inferred_type = ir;
            }
            else
            {
                if (ir == this.inferred_type || Convert.IsPrimitiveNumericType(this.inferred_type) &&
                    Convert.IsPrimitiveNumericType(ir) && Convert.IsPromotableTo(ir, this.inferred_type))
                    return;
                this.inferred_type = (IReflect)Typeob.Object;
                if (this.dependents == null)
                    return;
                int index = 0;
                for (int count = this.dependents.Count; index < count; ++index)
                    ((JSLocalField)this.dependents[index]).SetInferredType((IReflect)Typeob.Object, (AST)null);
            }
        }
    }

    /// <summary>Sets the value of the field, using the specified value, culture, and binding information.</summary>
    /// <param name="obj">The object whose field value will be set.</param>
    /// <param name="value">The value to assign to the field.</param>
    /// <param name="invokeAttr">A bitwise combination of the enumeration values that control the binding.</param>
    /// <param name="binder">A set of properties that enables the binding, coercion of argument types, and invocation of members through reflection.</param>
    /// <param name="locale">Provides information about a specific culture or locale. Used to format numbers, dates, and strings correctly.</param>
    public override void SetValue(
        object obj,
        object value,
        BindingFlags invokeAttr,
        Binder binder,
        CultureInfo locale)
    {
        if (this.type != null)
            value = Convert.Coerce(value, (object)this.type);
        while (obj is BlockScope)
            obj = (object)((ScriptObject)obj).GetParent();
        StackFrame stackFrame = (StackFrame)obj;
        JSLocalField outerField = this.outerField;
        int slotNumber = this.slotNumber;
        for (; (FieldInfo)outerField != (FieldInfo)null; outerField = outerField.outerField)
        {
            slotNumber = outerField.slotNumber;
            stackFrame = (StackFrame)stackFrame.GetParent();
        }

        if (stackFrame.localVars == null)
            return;
        stackFrame.localVars[slotNumber] = value;
    }
}