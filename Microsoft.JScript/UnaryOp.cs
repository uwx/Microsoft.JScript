// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.UnaryOp
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents the base class for classes that perform operations on single operands, such as <see cref="T:Microsoft.JScript.Typeof" /> and <see cref="T:Microsoft.JScript.PostOrPrefixOperator" />. This class belongs to the abstract syntax tree category.</summary>
public abstract class UnaryOp : AST
{
    /// <summary>Represents the operand.</summary>
    protected AST operand;

    internal UnaryOp(Context context, AST operand)
        : base(context)
    {
        this.operand = operand;
    }

    internal override void CheckIfOKToUseInSuperConstructorCall()
    {
        this.operand.CheckIfOKToUseInSuperConstructorCall();
    }

    internal override AST PartiallyEvaluate()
    {
        this.operand = this.operand.PartiallyEvaluate();
        if (this.operand is ConstantWrapper)
        {
            try
            {
                return (AST)new ConstantWrapper(this.Evaluate(), this.context);
            }
            catch (JScriptException ex)
            {
                this.context.HandleError((JSError)(ex.ErrorNumber & (int)ushort.MaxValue));
            }
            catch
            {
                this.context.HandleError(JSError.TypeMismatch);
            }
        }

        return (AST)this;
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        this.operand.TranslateToILInitializer(il);
    }
}