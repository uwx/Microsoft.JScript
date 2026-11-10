// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.ArrayConstructor
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;

#nullable disable
namespace Microsoft.JScript;

/// <summary>Represents a prototype-based object that is defined by a constructor function. This class belongs to the built-in object model category.</summary>
public sealed class ArrayConstructor : ScriptFunction
{
    internal static readonly ArrayConstructor ob = new ArrayConstructor();
    private ArrayPrototype originalPrototype;

    internal ArrayConstructor()
        : base((ScriptObject)FunctionPrototype.ob, "Array", 1)
    {
        this.originalPrototype = ArrayPrototype.ob;
        ArrayPrototype._constructor = this;
        this.proto = (object)ArrayPrototype.ob;
    }

    internal ArrayConstructor(LenientFunctionPrototype parent, LenientArrayPrototype prototypeProp)
        : base((ScriptObject)parent, "Array", 1)
    {
        this.originalPrototype = (ArrayPrototype)prototypeProp;
        prototypeProp.constructor = (object)this;
        this.proto = (object)prototypeProp;
        this.noExpando = false;
    }

    internal override object Call(object[] args, object thisob) => this.Construct(args);

    internal ArrayObject Construct()
    {
        return new ArrayObject((ScriptObject)this.originalPrototype, typeof(ArrayObject));
    }

    internal override object Construct(object[] args) => (object)this.CreateInstance(args);

    /// <summary>Helper method that creates a new array and populates it with the specified values.</summary>
    /// <returns>The new array.</returns>
    /// <param name="args">The values to populate the array with.</param>
    public ArrayObject ConstructArray(object[] args)
    {
        ArrayObject arrayObject = new ArrayObject((ScriptObject)this.originalPrototype, typeof(ArrayObject));
        arrayObject.length = (object)args.Length;
        for (int index = 0; index < args.Length; ++index)
            arrayObject.SetValueAtIndex((uint)index, args[index]);
        return arrayObject;
    }

    internal ArrayObject ConstructWrapper()
    {
        return (ArrayObject)new ArrayWrapper((ScriptObject)this.originalPrototype, (Array)null, false);
    }

    internal ArrayObject ConstructWrapper(Array arr)
    {
        return (ArrayObject)new ArrayWrapper((ScriptObject)this.originalPrototype, arr, false);
    }

    internal ArrayObject ConstructImplicitWrapper(Array arr)
    {
        return (ArrayObject)new ArrayWrapper((ScriptObject)this.originalPrototype, arr, true);
    }

    /// <summary>Creates an instance of the prototype-based object, passing in the specified arguments.</summary>
    /// <returns>The instance of the object.</returns>
    /// <param name="args">The arguments to pass to the constructor.</param>
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
    public ArrayObject CreateInstance(params object[] args)
    {
        ArrayObject instance = new ArrayObject((ScriptObject)this.originalPrototype, typeof(ArrayObject));
        if (args.Length != 0)
        {
            if (args.Length == 1)
            {
                object ob = args[0];
                IConvertible iconvertible = Convert.GetIConvertible(ob);
                if ((uint)(Convert.GetTypeCode(ob, iconvertible) - 4) <= 11U)
                {
                    double number = Convert.ToNumber(ob, iconvertible);
                    uint uint32 = Convert.ToUint32(ob, iconvertible);
                    instance.length = number == (double)uint32
                        ? (object)uint32
                        : throw new JScriptException(JSError.ArrayLengthConstructIncorrect);
                    return instance;
                }
            }

            if (args.Length == 1 && args[0] is Array)
            {
                Array array = (Array)args[0];
                instance.length = array.Rank == 1
                    ? (object)array.Length
                    : throw new JScriptException(JSError.TypeMismatch);
                for (int index = 0; index < array.Length; ++index)
                    instance.SetValueAtIndex((uint)index, array.GetValue(index));
            }
            else
            {
                instance.length = (object)args.Length;
                for (int index = 0; index < args.Length; ++index)
                    instance.SetValueAtIndex((uint)index, args[index]);
            }
        }

        return instance;
    }

    /// <summary>Invokes a method of the prototype-based object, passing in the specified arguments.</summary>
    /// <returns>The result of the call to the method.</returns>
    /// <param name="args">The arguments to pass to the method.</param>
    [JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
    public ArrayObject Invoke(params object[] args)
    {
        return args.Length == 1 && args[0] is Array ? this.ConstructWrapper((Array)args[0]) : this.CreateInstance(args);
    }
}