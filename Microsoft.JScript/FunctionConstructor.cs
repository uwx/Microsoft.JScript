// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.FunctionConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.Text;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. This class belongs to the built-in object model category.</summary>
public sealed class FunctionConstructor : ScriptFunction
{
    internal static readonly FunctionConstructor ob = new FunctionConstructor();
    internal FunctionPrototype originalPrototype;

    internal FunctionConstructor()
        : base((ScriptObject)FunctionPrototype.ob, "Function", 1)
    {
        this.originalPrototype = FunctionPrototype.ob;
        FunctionPrototype._constructor = this;
        this.proto = (object)FunctionPrototype.ob;
    }

    internal FunctionConstructor(LenientFunctionPrototype prototypeProp)
        : base((ScriptObject)prototypeProp, "Function", 1)
    {
        this.originalPrototype = (FunctionPrototype)prototypeProp;
        prototypeProp.constructor = (object)this;
        this.proto = (object)prototypeProp;
        this.noExpando = false;
    }

    internal override object Call(object[] args, object thisob)
    {
        return (object)this.Construct(args, this.engine);
    }

    internal override object Construct(object[] args) => (object)this.Construct(args, this.engine);

    internal ScriptFunction Construct(object[] args, VsaEngine engine)
    {
        StringBuilder stringBuilder = new StringBuilder("function anonymous(");
        int index1 = 0;
        for (int index2 = args.Length - 2; index1 < index2; ++index1)
        {
            stringBuilder.Append(Convert.ToString(args[index1]));
            stringBuilder.Append(", ");
        }

        if (args.Length > 1)
            stringBuilder.Append(Convert.ToString(args[args.Length - 2]));
        stringBuilder.Append(") {\n");
        if (args.Length != 0)
            stringBuilder.Append(Convert.ToString(args[args.Length - 1]));
        stringBuilder.Append("\n}");
        JSParser jsParser =
            new JSParser(new Context(new DocumentContext("anonymous", engine), stringBuilder.ToString()));
        engine.PushScriptObject((ScriptObject)((IActivationObject)engine.ScriptObjectStackTop()).GetGlobalScope());
        try
        {
            return (ScriptFunction)jsParser.ParseFunctionExpression().PartiallyEvaluate().Evaluate();
        }
        finally
        {
            engine.PopScriptObject();
        }
    }

    /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
    /// <returns>The instance of the object.</returns>
    /// <param name="args">The arguments to pass to the constructor.</param>
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
    public ScriptFunction CreateInstance(params object[] args) => this.Construct(args, this.engine);

    /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
    /// <returns>The instance of the object.</returns>
    /// <param name="args">The arguments to pass to the constructor.</param>
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
    public ScriptFunction Invoke(params object[] args) => this.Construct(args, this.engine);
}