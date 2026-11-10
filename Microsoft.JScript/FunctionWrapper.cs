// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.FunctionWrapper
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Binds the members of a function to an object. </summary>
public sealed class FunctionWrapper : ScriptFunction
{
    private object obj;
    private MemberInfo[] members;

    internal FunctionWrapper(string name, object obj, MemberInfo[] members)
        : base((ScriptObject)FunctionPrototype.ob, name, 0)
    {
        this.obj = obj;
        this.members = members;
        foreach (MemberInfo member in members)
        {
            if ((object)(member as MethodInfo) != null)
            {
                this.ilength = ((MethodBase)member).GetParameters().Length;
                break;
            }
        }
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
        MethodInfo member1 = this.members[0] as MethodInfo;
        if (thisob is GlobalScope || thisob == null || member1 != (MethodInfo)null &&
            (member1.Attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
            thisob = this.obj;
        else if (!this.obj.GetType().IsInstanceOfType(thisob) && !(this.obj is ClassScope))
        {
            if (this.members.Length == 1)
            {
                JSWrappedMethod member2 = this.members[0] as JSWrappedMethod;
                if ((MethodInfo)member2 != (MethodInfo)null && member2.DeclaringType == Typeob.Object)
                    return LateBinding.CallOneOfTheMembers(new MemberInfo[1]
                    {
                        (MemberInfo)member2.method
                    }, args, false, thisob, binder, culture, (string[])null, this.engine);
            }

            throw new JScriptException(JSError.TypeMismatch);
        }

        return LateBinding.CallOneOfTheMembers(this.members, args, false, thisob, binder, culture, (string[])null,
            this.engine);
    }

    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    internal Delegate ConvertToDelegate(Type delegateType)
    {
        return Delegate.CreateDelegate(delegateType, this.obj, this.name);
    }

    /// <summary>Creates a string representation of the function.</summary>
    /// <returns>A string representation of the function that contains the function source, the function members, and the function name.</returns>
    public override string ToString()
    {
        Type declaringType = this.members[0].DeclaringType;
        MethodInfo method = declaringType == (Type)null
            ? (MethodInfo)null
            : declaringType.GetMethod(this.name + " source");
        if (!(method == (MethodInfo)null))
            return (string)method.Invoke((object)null, (object[])null);
        StringBuilder stringBuilder = new StringBuilder();
        bool flag = true;
        foreach (MemberInfo member in this.members)
        {
            if ((object)(member as MethodInfo) != null || (object)(member as PropertyInfo) != null &&
                JSProperty.GetGetMethod((PropertyInfo)member, false) != (MethodInfo)null)
            {
                if (!flag)
                    stringBuilder.Append("\n");
                else
                    flag = false;
                stringBuilder.Append(member.ToString());
            }
        }

        return stringBuilder.Length > 0
            ? stringBuilder.ToString()
            : $"function {this.name}() {{\n    [native code]\n}}";
    }
}