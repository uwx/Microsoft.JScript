// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.WrappedNamespace
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class WrappedNamespace : ActivationObject
{
    internal string name;

    internal WrappedNamespace(string name, VsaEngine engine)
        : this(name, engine, true)
    {
    }

    internal WrappedNamespace(string name, VsaEngine engine, bool AddReferences)
        : base((ScriptObject)null)
    {
        this.name = name;
        this.engine = engine;
        this.isKnownAtCompileTime = true;
        if (!(name.Length > 0 & AddReferences))
            return;
        engine.TryToAddImplicitAssemblyReference(name);
    }

    public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
    {
        FieldInfo fieldInfo = (FieldInfo)this.name_table[(object)name];
        if (fieldInfo != (FieldInfo)null)
            return new MemberInfo[1] { (MemberInfo)fieldInfo };
        FieldAttributes attributeFlags = FieldAttributes.Literal;
        string str = this.name == null || this.name.Length == 0 ? name : $"{this.name}.{name}";
        object obj = (object)null;
        if (this.name != null && this.name.Length > 0)
            obj = (object)this.engine.GetClass(str);
        if (obj == null)
        {
            obj = (object)this.engine.GetType(str);
            if (obj != null && !((Type)obj).IsPublic)
            {
                if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
                    obj = (object)null;
                else
                    attributeFlags |= FieldAttributes.Private;
            }
        }
        else if ((((ClassScope)obj).owner.attributes & TypeAttributes.Public) == TypeAttributes.NotPublic)
        {
            if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
                obj = (object)null;
            else
                attributeFlags |= FieldAttributes.Private;
        }

        if (obj == null)
            return this.parent != null && (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default
                ? this.parent.GetMember(name, bindingAttr)
                : new MemberInfo[0];
        JSGlobalField field = (JSGlobalField)this.CreateField(name, attributeFlags, obj);
        if (this.engine.doFast)
            field.type = new TypeExpression((AST)new ConstantWrapper((object)Typeob.Type, (Context)null));
        this.name_table[(object)name] = (object)field;
        this.field_table.Add((object)field);
        return new MemberInfo[1] { (MemberInfo)field };
    }

    public override string ToString() => this.name;
}