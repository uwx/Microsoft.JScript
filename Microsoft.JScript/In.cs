// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.In
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices.Expando;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Determines whether an object is contained in another object. This class belongs to the abstract syntax tree category.</summary>
public sealed class In : BinaryOp
{
    internal In(Context context, AST operand1, AST operand2)
        : base(context, operand1, operand2)
    {
    }

    internal override object Evaluate()
    {
        object v1 = this.operand1.Evaluate();
        object v2 = this.operand2.Evaluate();
        try
        {
            return (object)In.JScriptIn(v1, v2);
        }
        catch (JScriptException ex)
        {
            if (ex.context == null)
                ex.context = this.operand2.context;
            throw ex;
        }
    }

    internal override IReflect InferType(JSField inference_target) => (IReflect)Typeob.Boolean;

    /// <summary>Determines whether the first specified object is contained in the second specified object.</summary>
    /// <returns>true if <paramref name="v1" /> is contained in <paramref name="v2" />; otherwise, false.</returns>
    /// <param name="v1">The object to search for.</param>
    /// <param name="v2">The object to search in.</param>
    public static bool JScriptIn(object v1, object v2)
    {
        bool flag = false;
        switch (v2)
        {
            case ScriptObject _:
                return !(((ScriptObject)v2).GetMemberValue(Convert.ToString(v1)) is Missing);
            case Array _:
                Array array = (Array)v2;
                double number = Convert.ToNumber(v1);
                int num = (int)number;
                return number == (double)num && array.GetLowerBound(0) <= num && num <= array.GetUpperBound(0);
            case IEnumerable _:
                if (v1 == null)
                    return false;
                switch (v2)
                {
                    case IDictionary _:
                        return ((IDictionary)v2).Contains(v1);
                    case IExpando _:
                        return ((IReflect)v2).GetMember(Convert.ToString(v1),
                            BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).Length != 0;
                    default:
                        IEnumerator enumerator1 = ((IEnumerable)v2).GetEnumerator();
                        while (!flag && enumerator1.MoveNext())
                        {
                            if (v1.Equals(enumerator1.Current))
                                return true;
                        }

                        break;
                }

                break;
            case IEnumerator _:
                if (v1 == null)
                    return false;
                IEnumerator enumerator2 = (IEnumerator)v2;
                while (!flag && enumerator2.MoveNext())
                {
                    if (v1.Equals(enumerator2.Current))
                        return true;
                }

                break;
            case IDebuggerObject _:
                return ((IDebuggerObject)v2).HasEnumerableMember(Convert.ToString(v1));
        }

        throw new JScriptException(JSError.ObjectExpected);
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        this.operand1.TranslateToIL(il, Typeob.Object);
        this.operand2.TranslateToIL(il, Typeob.Object);
        il.Emit(OpCodes.Call, CompilerGlobals.jScriptInMethod);
        Convert.Emit((AST)this, il, Typeob.Boolean, rtype);
    }
}