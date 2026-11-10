// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Try
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Identifies blocks of code to run in error handling scopes. This class belongs to the abstract syntax tree category.</summary>
public sealed class Try : AST
{
    private AST body;
    private TypeExpression type;
    private AST handler;
    private AST finally_block;
    private BlockScope handler_scope;
    private FieldInfo field;
    private string fieldName;
    private bool finallyHasControlFlowOutOfIt;
    private Context tryEndContext;

    internal Try(
        Context context,
        AST body,
        AST identifier,
        TypeExpression type,
        AST handler,
        AST finally_block,
        bool finallyHasControlFlowOutOfIt,
        Context tryEndContext)
        : base(context)
    {
        this.body = body;
        this.type = type;
        this.handler = handler;
        this.finally_block = finally_block;
        ScriptObject parent = this.Globals.ScopeStack.Peek();
        while (parent is WithObject)
            parent = parent.GetParent();
        this.handler_scope = (BlockScope)null;
        this.field = (FieldInfo)null;
        if (identifier != null)
        {
            this.fieldName = identifier.ToString();
            this.field = parent.GetField(this.fieldName,
                BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
            if (this.field != (FieldInfo)null)
            {
                if (type == null && this.field is JSVariableField && this.field.IsStatic &&
                    ((JSVariableField)this.field).type == null && !this.field.IsLiteral && !this.field.IsInitOnly)
                    return;
                if (((IActivationObject)parent).GetLocalField(this.fieldName) != (FieldInfo)null)
                    identifier.context.HandleError(JSError.DuplicateName, false);
            }

            this.handler_scope = new BlockScope(parent);
            this.handler_scope.catchHanderScope = true;
            JSVariableField jsVariableField =
                this.handler_scope.AddNewField(identifier.ToString(), (object)Missing.Value, FieldAttributes.Public);
            this.field = (FieldInfo)jsVariableField;
            jsVariableField.originalContext = identifier.context;
            if (identifier.context.document.debugOn && this.field is JSLocalField)
                this.handler_scope.AddFieldForLocalScopeDebugInfo((JSLocalField)this.field);
        }

        this.finallyHasControlFlowOutOfIt = finallyHasControlFlowOutOfIt;
        this.tryEndContext = tryEndContext;
    }

    internal override object Evaluate()
    {
        int i1 = this.Globals.ScopeStack.Size();
        int i2 = this.Globals.CallContextStack.Size();
        Completion completion1 = (Completion)null;
        Completion completion2 = (Completion)null;
        try
        {
            object obj = (object)null;
            try
            {
                completion1 = (Completion)this.body.Evaluate();
            }
            catch (Exception ex)
            {
                if (this.handler == null)
                    throw;
                obj = (object)ex;
                if (this.type != null)
                {
                    Type type = this.type.ToType();
                    if (Typeob.Exception.IsAssignableFrom(type))
                    {
                        if (!type.IsInstanceOfType((object)ex))
                            throw;
                    }
                    else if (!type.IsInstanceOfType(obj = Try.JScriptExceptionValue((object)ex, this.Engine)))
                        throw;
                }
                else
                    obj = Try.JScriptExceptionValue((object)ex, this.Engine);
            }

            if (obj != null)
            {
                this.Globals.ScopeStack.TrimToSize(i1);
                this.Globals.CallContextStack.TrimToSize(i2);
                if (this.handler_scope != null)
                {
                    this.handler_scope.SetParent(this.Globals.ScopeStack.Peek());
                    this.Globals.ScopeStack.Push((object)this.handler_scope);
                }

                if (this.field != (FieldInfo)null)
                    this.field.SetValue((object)this.Globals.ScopeStack.Peek(), obj);
                completion1 = (Completion)this.handler.Evaluate();
            }
        }
        finally
        {
            this.Globals.ScopeStack.TrimToSize(i1);
            this.Globals.CallContextStack.TrimToSize(i2);
            if (this.finally_block != null)
                completion2 = (Completion)this.finally_block.Evaluate();
        }

        if (completion1 == null || completion2 != null &&
            (completion2.Exit > 0 || completion2.Continue > 0 || completion2.Return))
            completion1 = completion2;
        else if (completion2 != null && completion2.value is Missing)
            completion1.value = completion2.value;
        return (object)new Completion()
        {
            Continue = (completion1.Continue - 1),
            Exit = (completion1.Exit - 1),
            Return = completion1.Return,
            value = completion1.value
        };
    }

    internal override Context GetFirstExecutableContext() => this.body.GetFirstExecutableContext();

    /// <summary>Returns the specified object as an exception, converting it to a specific <see cref="T:Microsoft.JScript.JScriptException" /> if it can.</summary>
    /// <returns>The exception <paramref name="e" />, converted to a specific <see cref="T:Microsoft.JScript.JScriptException" /> if that is possible.</returns>
    /// <param name="e">The object that represents the exception.</param>
    /// <param name="engine">A reference to the scripting engine.</param>
    public static object JScriptExceptionValue(object e, VsaEngine engine)
    {
        if (engine == null)
        {
            engine = new VsaEngine(true);
            engine.InitVsaEngine("JS7://Microsoft.JScript.Vsa.VsaEngine", (IJSVsaSite)new DefaultVsaSite());
        }

        ErrorConstructor originalError = engine.Globals.globalObject.originalError;
        switch (e)
        {
            case JScriptException _:
                object obj = ((JScriptException)e).value;
                return obj is Exception || obj is Missing ||
                       (((JScriptException)e).Number & (int)ushort.MaxValue) != 5022
                    ? (object)originalError.Construct((object)(Exception)e)
                    : obj;
            case StackOverflowException _:
                return (object)originalError.Construct((object)new JScriptException(JSError.OutOfStack));
            case OutOfMemoryException _:
                return (object)originalError.Construct((object)new JScriptException(JSError.OutOfMemory));
            default:
                return (object)originalError.Construct(e);
        }
    }

    internal override AST PartiallyEvaluate()
    {
        if (this.type != null)
        {
            this.type.PartiallyEvaluate();
            ((JSVariableField)this.field).type = this.type;
        }
        else if (this.field is JSLocalField)
            ((JSLocalField)this.field).SetInferredType((IReflect)Typeob.Object, (AST)null);

        ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
        while (scriptObject is WithObject)
            scriptObject = scriptObject.GetParent();
        FunctionScope functionScope = (FunctionScope)null;
        BitArray bitArray = (BitArray)null;
        if (scriptObject is FunctionScope)
        {
            functionScope = (FunctionScope)scriptObject;
            bitArray = functionScope.DefinedFlags;
        }

        this.body = this.body.PartiallyEvaluate();
        if (this.handler != null)
        {
            if (this.handler_scope != null)
                this.Globals.ScopeStack.Push((object)this.handler_scope);
            if (this.field is JSLocalField)
                ((JSLocalField)this.field).isDefined = true;
            this.handler = this.handler.PartiallyEvaluate();
            if (this.handler_scope != null)
                this.Globals.ScopeStack.Pop();
        }

        if (this.finally_block != null)
            this.finally_block = this.finally_block.PartiallyEvaluate();
        if (functionScope != null)
            functionScope.DefinedFlags = bitArray;
        return (AST)this;
    }

    /// <summary>Creates a new <see cref="T:Microsoft.JScript.BlockScope" /> object for the try block and pushes it onto the global scope stack.</summary>
    /// <param name="engine">A reference to the scripting engine.</param>
    /// <param name="id">The name of the scope of the try block.</param>
    /// <param name="scopeId">The ID of the scope of the try block.</param>
    public static void PushHandlerScope(VsaEngine engine, string id, int scopeId)
    {
        engine.PushScriptObject((ScriptObject)new BlockScope(engine.ScriptObjectStackTop(), id, scopeId));
    }

    internal override void TranslateToIL(ILGenerator il, Type rtype)
    {
        bool insideProtectedRegion = this.compilerGlobals.InsideProtectedRegion;
        this.compilerGlobals.InsideProtectedRegion = true;
        this.compilerGlobals.BreakLabelStack.Push(this.compilerGlobals.BreakLabelStack.Peek(0));
        this.compilerGlobals.ContinueLabelStack.Push(this.compilerGlobals.ContinueLabelStack.Peek(0));
        il.BeginExceptionBlock();
        if (this.finally_block != null)
        {
            if (this.finallyHasControlFlowOutOfIt)
                il.BeginExceptionBlock();
            if (this.handler != null)
                il.BeginExceptionBlock();
        }

        this.body.TranslateToIL(il, Typeob.Void);
        if (this.tryEndContext != null)
            this.tryEndContext.EmitLineInfo(il);
        if (this.handler != null)
        {
            if (this.type == null)
            {
                il.BeginCatchBlock(Typeob.Exception);
                this.handler.context.EmitLineInfo(il);
                this.EmitILToLoadEngine(il);
                il.Emit(OpCodes.Call, CompilerGlobals.jScriptExceptionValueMethod);
            }
            else
            {
                Type type = this.type.ToType();
                if (Typeob.Exception.IsAssignableFrom(type))
                {
                    il.BeginCatchBlock(type);
                    this.handler.context.EmitLineInfo(il);
                }
                else
                {
                    il.BeginExceptFilterBlock();
                    this.handler.context.EmitLineInfo(il);
                    this.EmitILToLoadEngine(il);
                    il.Emit(OpCodes.Call, CompilerGlobals.jScriptExceptionValueMethod);
                    il.Emit(OpCodes.Isinst, type);
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Cgt_Un);
                    il.BeginCatchBlock((Type)null);
                    this.EmitILToLoadEngine(il);
                    il.Emit(OpCodes.Call, CompilerGlobals.jScriptExceptionValueMethod);
                    Convert.Emit((AST)this, il, Typeob.Object, type);
                }
            }

            object obj = this.field is JSVariableField ? ((JSField)this.field).GetMetaData() : (object)this.field;
            if (obj is LocalBuilder)
                il.Emit(OpCodes.Stloc, (LocalBuilder)obj);
            else if ((object)(obj as FieldInfo) != null)
                il.Emit(OpCodes.Stsfld, (FieldInfo)obj);
            else
                Convert.EmitLdarg(il, (short)obj);
            if (this.handler_scope != null)
            {
                if (!this.handler_scope.isKnownAtCompileTime)
                {
                    this.EmitILToLoadEngine(il);
                    il.Emit(OpCodes.Ldstr, this.fieldName);
                    ConstantWrapper.TranslateToILInt(il, this.handler_scope.scopeId);
                    il.Emit(OpCodes.Call, Typeob.Try.GetMethod("PushHandlerScope"));
                    this.Globals.ScopeStack.Push((object)this.handler_scope);
                    il.BeginExceptionBlock();
                }

                il.BeginScope();
                if (this.context.document.debugOn)
                    this.handler_scope.EmitLocalInfoForFields(il);
            }

            this.handler.TranslateToIL(il, Typeob.Void);
            if (this.handler_scope != null)
            {
                il.EndScope();
                if (!this.handler_scope.isKnownAtCompileTime)
                {
                    il.BeginFinallyBlock();
                    this.EmitILToLoadEngine(il);
                    il.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
                    il.Emit(OpCodes.Pop);
                    this.Globals.ScopeStack.Pop();
                    il.EndExceptionBlock();
                }
            }

            il.EndExceptionBlock();
        }

        if (this.finally_block != null)
        {
            bool insideFinally = this.compilerGlobals.InsideFinally;
            int finallyStackTop = this.compilerGlobals.FinallyStackTop;
            this.compilerGlobals.InsideFinally = true;
            this.compilerGlobals.FinallyStackTop = this.compilerGlobals.BreakLabelStack.Size();
            il.BeginFinallyBlock();
            this.finally_block.TranslateToIL(il, Typeob.Void);
            il.EndExceptionBlock();
            this.compilerGlobals.InsideFinally = insideFinally;
            this.compilerGlobals.FinallyStackTop = finallyStackTop;
            if (this.finallyHasControlFlowOutOfIt)
            {
                il.BeginCatchBlock(Typeob.BreakOutOfFinally);
                il.Emit(OpCodes.Ldfld, Typeob.BreakOutOfFinally.GetField("target"));
                int i1 = this.compilerGlobals.BreakLabelStack.Size() - 1;
                int num1 = i1;
                for (; i1 > 0; --i1)
                {
                    il.Emit(OpCodes.Dup);
                    ConstantWrapper.TranslateToILInt(il, i1);
                    Label label = il.DefineLabel();
                    il.Emit(OpCodes.Blt, label);
                    il.Emit(OpCodes.Pop);
                    if (insideFinally && i1 < finallyStackTop)
                        il.Emit(OpCodes.Rethrow);
                    else
                        il.Emit(OpCodes.Leave, (Label)this.compilerGlobals.BreakLabelStack.Peek(num1 - i1));
                    il.MarkLabel(label);
                }

                il.Emit(OpCodes.Pop);
                il.BeginCatchBlock(Typeob.ContinueOutOfFinally);
                il.Emit(OpCodes.Ldfld, Typeob.ContinueOutOfFinally.GetField("target"));
                int i2 = this.compilerGlobals.ContinueLabelStack.Size() - 1;
                int num2 = i2;
                for (; i2 > 0; --i2)
                {
                    il.Emit(OpCodes.Dup);
                    ConstantWrapper.TranslateToILInt(il, i2);
                    Label label = il.DefineLabel();
                    il.Emit(OpCodes.Blt, label);
                    il.Emit(OpCodes.Pop);
                    if (insideFinally && i2 < finallyStackTop)
                        il.Emit(OpCodes.Rethrow);
                    else
                        il.Emit(OpCodes.Leave, (Label)this.compilerGlobals.ContinueLabelStack.Peek(num2 - i2));
                    il.MarkLabel(label);
                }

                il.Emit(OpCodes.Pop);
                ScriptObject scriptObject = this.Globals.ScopeStack.Peek();
                while (true)
                {
                    switch (scriptObject)
                    {
                        case null:
                        case FunctionScope _:
                            goto label_44;
                        default:
                            scriptObject = scriptObject.GetParent();
                            continue;
                    }
                }

                label_44:
                if (scriptObject != null && !insideFinally)
                {
                    il.BeginCatchBlock(Typeob.ReturnOutOfFinally);
                    il.Emit(OpCodes.Pop);
                    il.Emit(OpCodes.Leave, ((FunctionScope)scriptObject).owner.returnLabel);
                }

                il.EndExceptionBlock();
            }
        }

        this.compilerGlobals.InsideProtectedRegion = insideProtectedRegion;
        this.compilerGlobals.BreakLabelStack.Pop();
        this.compilerGlobals.ContinueLabelStack.Pop();
    }

    internal override void TranslateToILInitializer(ILGenerator il)
    {
        this.body.TranslateToILInitializer(il);
        if (this.handler != null)
            this.handler.TranslateToILInitializer(il);
        if (this.finally_block == null)
            return;
        this.finally_block.TranslateToILInitializer(il);
    }
}