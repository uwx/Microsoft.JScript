// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.BuiltinFunction
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.Globalization;
using System.Reflection;

#nullable disable
namespace Microsoft.JScript;

internal sealed class BuiltinFunction : ScriptFunction
{
    internal MethodInfo method;
    private JSBuiltin biFunc;

    internal BuiltinFunction(object obj, MethodInfo method)
        : this(method.Name, obj, method, (ScriptFunction)FunctionPrototype.ob)
    {
    }

    internal BuiltinFunction(string name, object obj, MethodInfo method, ScriptFunction parent)
        : base((ScriptObject)parent, name)
    {
        this.noExpando = false;
        int num = this.ilength = method.GetParameters().Length;
        object[] customAttributes =
            CustomAttribute.GetCustomAttributes((MemberInfo)method, typeof(JSFunctionAttribute), false);
        JSFunctionAttribute functionAttribute = customAttributes.Length != 0
            ? (JSFunctionAttribute)customAttributes[0]
            : new JSFunctionAttribute(JSFunctionAttributeEnum.None);
        JSFunctionAttributeEnum attributeValue = functionAttribute.attributeValue;
        if ((attributeValue & JSFunctionAttributeEnum.HasThisObject) != JSFunctionAttributeEnum.None)
            --this.ilength;
        if ((attributeValue & JSFunctionAttributeEnum.HasEngine) != JSFunctionAttributeEnum.None)
            --this.ilength;
        if ((attributeValue & JSFunctionAttributeEnum.HasVarArgs) != JSFunctionAttributeEnum.None)
            --this.ilength;
        this.biFunc = functionAttribute.builtinFunction;
        if (this.biFunc == JSBuiltin.None)
            this.method = (MethodInfo)new JSNativeMethod(method, obj, this.engine);
        else
            this.method = (MethodInfo)null;
    }

    internal override object Call(object[] args, object thisob)
    {
        return BuiltinFunction.QuickCall(args, thisob, this.biFunc, this.method, this.engine);
    }

    internal static object QuickCall(
        object[] args,
        object thisob,
        JSBuiltin biFunc,
        MethodInfo method,
        VsaEngine engine)
    {
        int length = args.Length;
        switch (biFunc)
        {
            case JSBuiltin.Array_concat:
                return (object)ArrayPrototype.concat(thisob, engine, args);
            case JSBuiltin.Array_join:
                return (object)ArrayPrototype.join(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Array_pop:
                return ArrayPrototype.pop(thisob);
            case JSBuiltin.Array_push:
                return (object)ArrayPrototype.push(thisob, args);
            case JSBuiltin.Array_reverse:
                return ArrayPrototype.reverse(thisob);
            case JSBuiltin.Array_shift:
                return ArrayPrototype.shift(thisob);
            case JSBuiltin.Array_slice:
                return (object)ArrayPrototype.slice(thisob, engine,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)), BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.Array_sort:
                return ArrayPrototype.sort(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Array_splice:
                return (object)ArrayPrototype.splice(thisob, engine,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)),
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 1, length)),
                    BuiltinFunction.VarArgs(args, 2, length));
            case JSBuiltin.Array_toLocaleString:
                return (object)ArrayPrototype.toLocaleString(thisob);
            case JSBuiltin.Array_toString:
                return (object)ArrayPrototype.toString(thisob);
            case JSBuiltin.Array_unshift:
                return ArrayPrototype.unshift(thisob, args);
            case JSBuiltin.Boolean_toString:
                return (object)BooleanPrototype.toString(thisob);
            case JSBuiltin.Boolean_valueOf:
                return BooleanPrototype.valueOf(thisob);
            case JSBuiltin.Date_getDate:
                return (object)DatePrototype.getDate(thisob);
            case JSBuiltin.Date_getDay:
                return (object)DatePrototype.getDay(thisob);
            case JSBuiltin.Date_getFullYear:
                return (object)DatePrototype.getFullYear(thisob);
            case JSBuiltin.Date_getHours:
                return (object)DatePrototype.getHours(thisob);
            case JSBuiltin.Date_getMilliseconds:
                return (object)DatePrototype.getMilliseconds(thisob);
            case JSBuiltin.Date_getMinutes:
                return (object)DatePrototype.getMinutes(thisob);
            case JSBuiltin.Date_getMonth:
                return (object)DatePrototype.getMonth(thisob);
            case JSBuiltin.Date_getSeconds:
                return (object)DatePrototype.getSeconds(thisob);
            case JSBuiltin.Date_getTime:
                return (object)DatePrototype.getTime(thisob);
            case JSBuiltin.Date_getTimezoneOffset:
                return (object)DatePrototype.getTimezoneOffset(thisob);
            case JSBuiltin.Date_getUTCDate:
                return (object)DatePrototype.getUTCDate(thisob);
            case JSBuiltin.Date_getUTCDay:
                return (object)DatePrototype.getUTCDay(thisob);
            case JSBuiltin.Date_getUTCFullYear:
                return (object)DatePrototype.getUTCFullYear(thisob);
            case JSBuiltin.Date_getUTCHours:
                return (object)DatePrototype.getUTCHours(thisob);
            case JSBuiltin.Date_getUTCMilliseconds:
                return (object)DatePrototype.getUTCMilliseconds(thisob);
            case JSBuiltin.Date_getUTCMinutes:
                return (object)DatePrototype.getUTCMinutes(thisob);
            case JSBuiltin.Date_getUTCMonth:
                return (object)DatePrototype.getUTCMonth(thisob);
            case JSBuiltin.Date_getUTCSeconds:
                return (object)DatePrototype.getUTCSeconds(thisob);
            case JSBuiltin.Date_getVarDate:
                return DatePrototype.getVarDate(thisob);
            case JSBuiltin.Date_getYear:
                return (object)DatePrototype.getYear(thisob);
            case JSBuiltin.Date_parse:
                return (object)DateConstructor.parse(Convert.ToString(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Date_setDate:
                return (object)DatePrototype.setDate(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Date_setFullYear:
                return (object)DatePrototype.setFullYear(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)), BuiltinFunction.GetArg(args, 1, length),
                    BuiltinFunction.GetArg(args, 2, length));
            case JSBuiltin.Date_setHours:
                return (object)DatePrototype.setHours(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)),
                    BuiltinFunction.GetArg(args, 1, length), BuiltinFunction.GetArg(args, 2, length),
                    BuiltinFunction.GetArg(args, 3, length));
            case JSBuiltin.Date_setMinutes:
                return (object)DatePrototype.setMinutes(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)), BuiltinFunction.GetArg(args, 1, length),
                    BuiltinFunction.GetArg(args, 2, length));
            case JSBuiltin.Date_setMilliseconds:
                return (object)DatePrototype.setMilliseconds(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Date_setMonth:
                return (object)DatePrototype.setMonth(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)),
                    BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.Date_setSeconds:
                return (object)DatePrototype.setSeconds(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)), BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.Date_setTime:
                return (object)DatePrototype.setTime(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Date_setUTCDate:
                return (object)DatePrototype.setUTCDate(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Date_setUTCFullYear:
                return (object)DatePrototype.setUTCFullYear(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)), BuiltinFunction.GetArg(args, 1, length),
                    BuiltinFunction.GetArg(args, 2, length));
            case JSBuiltin.Date_setUTCHours:
                return (object)DatePrototype.setUTCHours(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)), BuiltinFunction.GetArg(args, 1, length),
                    BuiltinFunction.GetArg(args, 2, length), BuiltinFunction.GetArg(args, 3, length));
            case JSBuiltin.Date_setUTCMinutes:
                return (object)DatePrototype.setUTCMinutes(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)), BuiltinFunction.GetArg(args, 1, length),
                    BuiltinFunction.GetArg(args, 2, length));
            case JSBuiltin.Date_setUTCMilliseconds:
                return (object)DatePrototype.setUTCMilliseconds(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Date_setUTCMonth:
                return (object)DatePrototype.setUTCMonth(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)), BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.Date_setUTCSeconds:
                return (object)DatePrototype.setUTCSeconds(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)), BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.Date_setYear:
                return (object)DatePrototype.setYear(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Date_toDateString:
                return (object)DatePrototype.toDateString(thisob);
            case JSBuiltin.Date_toGMTString:
                return (object)DatePrototype.toGMTString(thisob);
            case JSBuiltin.Date_toLocaleDateString:
                return (object)DatePrototype.toLocaleDateString(thisob);
            case JSBuiltin.Date_toLocaleString:
                return (object)DatePrototype.toLocaleString(thisob);
            case JSBuiltin.Date_toLocaleTimeString:
                return (object)DatePrototype.toLocaleTimeString(thisob);
            case JSBuiltin.Date_toString:
                return (object)DatePrototype.toString(thisob);
            case JSBuiltin.Date_toTimeString:
                return (object)DatePrototype.toTimeString(thisob);
            case JSBuiltin.Date_toUTCString:
                return (object)DatePrototype.toUTCString(thisob);
            case JSBuiltin.Date_UTC:
                return (object)DateConstructor.UTC(BuiltinFunction.GetArg(args, 0, length),
                    BuiltinFunction.GetArg(args, 1, length), BuiltinFunction.GetArg(args, 2, length),
                    BuiltinFunction.GetArg(args, 3, length), BuiltinFunction.GetArg(args, 4, length),
                    BuiltinFunction.GetArg(args, 5, length), BuiltinFunction.GetArg(args, 6, length));
            case JSBuiltin.Date_valueOf:
                return (object)DatePrototype.valueOf(thisob);
            case JSBuiltin.Enumerator_atEnd:
                return (object)EnumeratorPrototype.atEnd(thisob);
            case JSBuiltin.Enumerator_item:
                return EnumeratorPrototype.item(thisob);
            case JSBuiltin.Enumerator_moveFirst:
                EnumeratorPrototype.moveFirst(thisob);
                return (object)null;
            case JSBuiltin.Enumerator_moveNext:
                EnumeratorPrototype.moveNext(thisob);
                return (object)null;
            case JSBuiltin.Error_toString:
                return (object)ErrorPrototype.toString(thisob);
            case JSBuiltin.Function_apply:
                return FunctionPrototype.apply(thisob, BuiltinFunction.GetArg(args, 0, length),
                    BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.Function_call:
                return FunctionPrototype.call(thisob, BuiltinFunction.GetArg(args, 0, length),
                    BuiltinFunction.VarArgs(args, 1, length));
            case JSBuiltin.Function_toString:
                return (object)FunctionPrototype.toString(thisob);
            case JSBuiltin.Global_CollectGarbage:
                GlobalObject.CollectGarbage();
                return (object)null;
            case JSBuiltin.Global_decodeURI:
                return (object)GlobalObject.decodeURI(BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Global_decodeURIComponent:
                return (object)GlobalObject.decodeURIComponent(BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Global_encodeURI:
                return (object)GlobalObject.encodeURI(BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Global_encodeURIComponent:
                return (object)GlobalObject.encodeURIComponent(BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Global_escape:
                return (object)GlobalObject.escape(BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Global_eval:
                return GlobalObject.eval(BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Global_GetObject:
                return GlobalObject.GetObject(BuiltinFunction.GetArg(args, 0, length),
                    BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.Global_isNaN:
                return (object)GlobalObject.isNaN(BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Global_isFinite:
                return (object)GlobalObject.isFinite(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Global_parseFloat:
                return (object)GlobalObject.parseFloat(BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Global_parseInt:
                return (object)GlobalObject.parseInt(BuiltinFunction.GetArg(args, 0, length),
                    BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.Global_ScriptEngine:
                return (object)GlobalObject.ScriptEngine();
            case JSBuiltin.Global_ScriptEngineBuildVersion:
                return (object)GlobalObject.ScriptEngineBuildVersion();
            case JSBuiltin.Global_ScriptEngineMajorVersion:
                return (object)GlobalObject.ScriptEngineMajorVersion();
            case JSBuiltin.Global_ScriptEngineMinorVersion:
                return (object)GlobalObject.ScriptEngineMinorVersion();
            case JSBuiltin.Global_unescape:
                return (object)GlobalObject.unescape(BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Math_abs:
                return (object)MathObject.abs(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Math_acos:
                return (object)MathObject.acos(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Math_asin:
                return (object)MathObject.asin(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Math_atan:
                return (object)MathObject.atan(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Math_atan2:
                return (object)MathObject.atan2(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)),
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 1, length)));
            case JSBuiltin.Math_ceil:
                return (object)MathObject.ceil(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Math_cos:
                return (object)MathObject.cos(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Math_exp:
                return (object)MathObject.exp(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Math_floor:
                return (object)MathObject.floor(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Math_log:
                return (object)MathObject.log(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Math_max:
                return (object)MathObject.max(BuiltinFunction.GetArg(args, 0, length),
                    BuiltinFunction.GetArg(args, 1, length), BuiltinFunction.VarArgs(args, 2, length));
            case JSBuiltin.Math_min:
                return (object)MathObject.min(BuiltinFunction.GetArg(args, 0, length),
                    BuiltinFunction.GetArg(args, 1, length), BuiltinFunction.VarArgs(args, 2, length));
            case JSBuiltin.Math_pow:
                return (object)MathObject.pow(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)),
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 1, length)));
            case JSBuiltin.Math_random:
                return (object)MathObject.random();
            case JSBuiltin.Math_round:
                return (object)MathObject.round(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Math_sin:
                return (object)MathObject.sin(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Math_sqrt:
                return (object)MathObject.sqrt(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Math_tan:
                return (object)MathObject.tan(Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Number_toExponential:
                return (object)NumberPrototype.toExponential(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Number_toFixed:
                return (object)NumberPrototype.toFixed(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.Number_toLocaleString:
                return (object)NumberPrototype.toLocaleString(thisob);
            case JSBuiltin.Number_toPrecision:
                return (object)NumberPrototype.toPrecision(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Number_toString:
                return (object)NumberPrototype.toString(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Number_valueOf:
                return NumberPrototype.valueOf(thisob);
            case JSBuiltin.Object_hasOwnProperty:
                return (object)ObjectPrototype.hasOwnProperty(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Object_isPrototypeOf:
                return (object)ObjectPrototype.isPrototypeOf(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Object_propertyIsEnumerable:
                return (object)ObjectPrototype.propertyIsEnumerable(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.Object_toLocaleString:
                return (object)ObjectPrototype.toLocaleString(thisob);
            case JSBuiltin.Object_toString:
                return (object)ObjectPrototype.toString(thisob);
            case JSBuiltin.Object_valueOf:
                return ObjectPrototype.valueOf(thisob);
            case JSBuiltin.RegExp_compile:
                return (object)RegExpPrototype.compile(thisob, BuiltinFunction.GetArg(args, 0, length),
                    BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.RegExp_exec:
                return RegExpPrototype.exec(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.RegExp_test:
                return (object)RegExpPrototype.test(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.RegExp_toString:
                return (object)RegExpPrototype.toString(thisob);
            case JSBuiltin.String_anchor:
                return (object)StringPrototype.anchor(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.String_big:
                return (object)StringPrototype.big(thisob);
            case JSBuiltin.String_blink:
                return (object)StringPrototype.blink(thisob);
            case JSBuiltin.String_bold:
                return (object)StringPrototype.bold(thisob);
            case JSBuiltin.String_charAt:
                return (object)StringPrototype.charAt(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.String_charCodeAt:
                return StringPrototype.charCodeAt(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)));
            case JSBuiltin.String_concat:
                return (object)StringPrototype.concat(thisob, args);
            case JSBuiltin.String_fixed:
                return (object)StringPrototype.@fixed(thisob);
            case JSBuiltin.String_fontcolor:
                return (object)StringPrototype.fontcolor(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.String_fontsize:
                return (object)StringPrototype.fontsize(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.String_fromCharCode:
                return (object)StringConstructor.fromCharCode(args);
            case JSBuiltin.String_indexOf:
                return (object)StringPrototype.indexOf(thisob, BuiltinFunction.GetArg(args, 0, length),
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 1, length)));
            case JSBuiltin.String_italics:
                return (object)StringPrototype.italics(thisob);
            case JSBuiltin.String_lastIndexOf:
                return (object)StringPrototype.lastIndexOf(thisob, BuiltinFunction.GetArg(args, 0, length),
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 1, length)));
            case JSBuiltin.String_link:
                return (object)StringPrototype.link(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.String_localeCompare:
                return (object)StringPrototype.localeCompare(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.String_match:
                return StringPrototype.match(thisob, engine, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.String_replace:
                return (object)StringPrototype.replace(thisob, BuiltinFunction.GetArg(args, 0, length),
                    BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.String_search:
                return (object)StringPrototype.search(thisob, engine, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.String_slice:
                return (object)StringPrototype.slice(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)),
                    BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.String_small:
                return (object)StringPrototype.small(thisob);
            case JSBuiltin.String_split:
                return (object)StringPrototype.split(thisob, engine, BuiltinFunction.GetArg(args, 0, length),
                    BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.String_strike:
                return (object)StringPrototype.strike(thisob);
            case JSBuiltin.String_sub:
                return (object)StringPrototype.sub(thisob);
            case JSBuiltin.String_substr:
                return (object)StringPrototype.substr(thisob, Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)),
                    BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.String_substring:
                return (object)StringPrototype.substring(thisob,
                    Convert.ToNumber(BuiltinFunction.GetArg(args, 0, length)), BuiltinFunction.GetArg(args, 1, length));
            case JSBuiltin.String_sup:
                return (object)StringPrototype.sup(thisob);
            case JSBuiltin.String_toLocaleLowerCase:
                return (object)StringPrototype.toLocaleLowerCase(thisob);
            case JSBuiltin.String_toLocaleUpperCase:
                return (object)StringPrototype.toLocaleUpperCase(thisob);
            case JSBuiltin.String_toLowerCase:
                return (object)StringPrototype.toLowerCase(thisob);
            case JSBuiltin.String_toString:
                return (object)StringPrototype.toString(thisob);
            case JSBuiltin.String_toUpperCase:
                return (object)StringPrototype.toUpperCase(thisob);
            case JSBuiltin.String_valueOf:
                return StringPrototype.valueOf(thisob);
            case JSBuiltin.VBArray_dimensions:
                return (object)VBArrayPrototype.dimensions(thisob);
            case JSBuiltin.VBArray_getItem:
                return VBArrayPrototype.getItem(thisob, args);
            case JSBuiltin.VBArray_lbound:
                return (object)VBArrayPrototype.lbound(thisob, BuiltinFunction.GetArg(args, 0, length));
            case JSBuiltin.VBArray_toArray:
                return (object)VBArrayPrototype.toArray(thisob, engine);
            case JSBuiltin.VBArray_ubound:
                return (object)VBArrayPrototype.ubound(thisob, BuiltinFunction.GetArg(args, 0, length));
            default:
                return method.Invoke(thisob, BindingFlags.Default, (Binder)JSBinder.ob, args, (CultureInfo)null);
        }
    }

    private static object GetArg(object[] args, int i, int n)
    {
        return i >= n ? (object)Missing.Value : args[i];
    }

    private static object[] VarArgs(object[] args, int offset, int n)
    {
        object[] objArray = new object[n >= offset ? n - offset : 0];
        for (int index = offset; index < n; ++index)
            objArray[index - offset] = args[index];
        return objArray;
    }

    public override string ToString() => $"function {this.name}() {{\n    [native code]\n}}";
}