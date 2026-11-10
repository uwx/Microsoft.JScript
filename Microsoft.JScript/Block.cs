// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Block
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a block in JScript. This class is used mainly by the <see cref="T:Microsoft.JScript.JSParser" /> to create <see cref="T:Microsoft.JScript.Block" /> objects when it parses code. This class belongs to the abstract syntax tree category.</summary>
public sealed class Block : AST
{
    private Completion completion;
    private ArrayList list;

    internal Block(Context context)
        : base(context)
    {
        this.completion = new Completion();
        this.list = new ArrayList();
    }

    internal void Append(AST elem) => this.list.Add((object)elem);

    internal void ComplainAboutAnythingOtherThanClassOrPackage()
    {
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
        {
            object obj = this.list[index];
            if (!(obj is Class) && !(obj is Package) && !(obj is Import) &&
                (!(obj is Block block) || block.list.Count != 0) && (!(obj is Expression expression) ||
                                                                     !(expression
                                                                         .operand is AssemblyCustomAttributeList)))
            {
                ((AST)obj).context.HandleError(JSError.OnlyClassesAndPackagesAllowed);
                break;
            }
        }
    }

    internal override object Evaluate()
    {
        this.completion.Continue = 0;
        this.completion.Exit = 0;
        this.completion.value = (object)null;
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
        {
            AST ast = (AST)this.list[index];
            object obj;
            try
            {
                obj = ast.Evaluate();
            }
            catch (JScriptException ex)
            {
                if (ex.context == null)
                    ex.context = ast.context;
                throw ex;
            }

            Completion completion = (Completion)obj;
            if (completion.value != null)
                this.completion.value = completion.value;
            if (completion.Continue > 1)
            {
                this.completion.Continue = completion.Continue - 1;
                break;
            }

            if (completion.Exit > 0)
            {
                this.completion.Exit = completion.Exit - 1;
                break;
            }

            if (completion.Return)
                return (object)completion;
        }

        return (object)this.completion;
    }

    internal void EvaluateStaticVariableInitializers()
    {
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
        {
            switch (this.list[index])
            {
                case VariableDeclaration variableDeclaration
                    when variableDeclaration.field.IsStatic && !variableDeclaration.field.IsLiteral:
                    variableDeclaration.Evaluate();
                    break;
                case StaticInitializer staticInitializer:
                    staticInitializer.Evaluate();
                    break;
                case Class @class:
                    @class.Evaluate();
                    break;
                case Constant constant when constant.field.IsStatic:
                    constant.Evaluate();
                    break;
                case Block block:
                    block.EvaluateStaticVariableInitializers();
                    break;
            }
        }
    }

    internal void EvaluateInstanceVariableInitializers()
    {
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
        {
            switch (this.list[index])
            {
                case VariableDeclaration variableDeclaration
                    when !variableDeclaration.field.IsStatic && !variableDeclaration.field.IsLiteral:
                    variableDeclaration.Evaluate();
                    break;
                case Block block:
                    block.EvaluateInstanceVariableInitializers();
                    break;
            }
        }
    }

    internal override bool HasReturn()
    {
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
        {
            if (((AST)this.list[index]).HasReturn())
                return true;
        }

        return false;
    }

    internal void ProcessAssemblyAttributeLists()
    {
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
        {
            if (this.list[index] is Expression expression && expression.operand is AssemblyCustomAttributeList operand)
                operand.Process();
        }
    }

    internal void MarkSuperOKIfIsFirstStatement()
    {
        if (this.list.Count <= 0 || !(this.list[0] is ConstructorCall))
            return;
        ((ConstructorCall)this.list[0]).isOK = true;
    }

    internal override AST PartiallyEvaluate()
    {
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
        {
            AST ast = (AST)this.list[index];
            this.list[index] = (object)ast.PartiallyEvaluate();
        }

        return (AST)this;
    }

    internal Expression ToExpression()
    {
        return this.list.Count == 1 && this.list[0] is Expression ? (Expression)this.list[0] : (Expression)null;
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        Label loc = il.DefineLabel();
        this.compilerGlobals.BreakLabelStack.Push((object)loc);
        this.compilerGlobals.ContinueLabelStack.Push((object)loc);
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
            ((AST)this.list[index]).TranslateToIL(il, Typeob.Void);
        il.MarkLabel(loc);
        this.compilerGlobals.BreakLabelStack.Pop();
        this.compilerGlobals.ContinueLabelStack.Pop();
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
            ((AST)this.list[index]).TranslateToILInitializer(il);
    }

    internal void TranslateToILInitOnlyInitializers(ILGenerator il)
    {
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
        {
            if (this.list[index] is Constant constant)
                constant.TranslateToILInitOnlyInitializers(il);
        }
    }

    internal void TranslateToILInstanceInitializers(ILGenerator il)
    {
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
        {
            AST ast = (AST)this.list[index];
            switch (ast)
            {
                case VariableDeclaration _ when !((VariableDeclaration)ast).field.IsStatic &&
                                                !((VariableDeclaration)ast).field.IsLiteral:
                    ast.TranslateToILInitializer(il);
                    ast.TranslateToIL(il, Typeob.Void);
                    break;
                case FunctionDeclaration _ when !((FunctionDeclaration)ast).func.isStatic:
                    ast.TranslateToILInitializer(il);
                    break;
                case Constant _ when !((Constant)ast).field.IsStatic:
                    ast.TranslateToIL(il, Typeob.Void);
                    break;
                case Block _:
                    ((Block)ast).TranslateToILInstanceInitializers(il);
                    break;
            }
        }
    }

    internal void TranslateToILStaticInitializers(ILGenerator il)
    {
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
        {
            AST ast = (AST)this.list[index];
            switch (ast)
            {
                case VariableDeclaration _ when ((VariableDeclaration)ast).field.IsStatic:
                case Constant _ when ((Constant)ast).field.IsStatic:
                    ast.TranslateToILInitializer(il);
                    ast.TranslateToIL(il, Typeob.Void);
                    break;
                case StaticInitializer _:
                    ast.TranslateToIL(il, Typeob.Void);
                    break;
                case FunctionDeclaration _ when ((FunctionDeclaration)ast).func.isStatic:
                    ast.TranslateToILInitializer(il);
                    break;
                case Class _:
                    ast.TranslateToIL(il, Typeob.Void);
                    break;
                case Block _:
                    ((Block)ast).TranslateToILStaticInitializers(il);
                    break;
            }
        }
    }

    internal override Context GetFirstExecutableContext()
    {
        int index = 0;
        for (int count = this.list.Count; index < count; ++index)
        {
            Context executableContext;
            if ((executableContext = ((AST)this.list[index]).GetFirstExecutableContext()) != null)
                return executableContext;
        }

        return (Context)null;
    }
}