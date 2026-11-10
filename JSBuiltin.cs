// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSBuiltin
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

#nullable disable
namespace Microsoft.JScript;

/// <summary>Specifies the built-in JScript functions. These functions correspond to the static methods of the intrinsic JScript objects. The members of this enumeration are used to create <see cref="T:Microsoft.JScript.JSFunctionAttribute" /> objects.</summary>
public enum JSBuiltin
{
  /// <summary>No method. This is the default value.</summary>
  None,
  /// <summary>The static <see cref="M:Microsoft.JScript.ArrayPrototype.concat(System.Object,Microsoft.JScript.Vsa.VsaEngine,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
  Array_concat,
  /// <summary>The static <see cref="M:Microsoft.JScript.ArrayPrototype.join(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
  Array_join,
  /// <summary>The static <see cref="M:Microsoft.JScript.ArrayPrototype.pop(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
  Array_pop,
  /// <summary>The static <see cref="M:Microsoft.JScript.ArrayPrototype.push(System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
  Array_push,
  /// <summary>The static <see cref="M:Microsoft.JScript.ArrayPrototype.reverse(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
  Array_reverse,
  /// <summary>The static <see cref="M:Microsoft.JScript.ArrayPrototype.shift(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
  Array_shift,
  /// <summary>The static <see cref="M:Microsoft.JScript.ArrayPrototype.slice(System.Object,Microsoft.JScript.Vsa.VsaEngine,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
  Array_slice,
  /// <summary>The static <see cref="M:Microsoft.JScript.ArrayPrototype.sort(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
  Array_sort,
  /// <summary>The static <see cref="M:Microsoft.JScript.ArrayPrototype.splice(System.Object,Microsoft.JScript.Vsa.VsaEngine,System.Double,System.Double,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
  Array_splice,
  /// <summary>The static <see cref="M:Microsoft.JScript.ArrayPrototype.toLocaleString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
  Array_toLocaleString,
  /// <summary>The static <see cref="M:Microsoft.JScript.ArrayPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
  Array_toString,
  /// <summary>The static <see cref="M:Microsoft.JScript.ArrayPrototype.unshift(System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.ArrayPrototype" /> class.</summary>
  Array_unshift,
  /// <summary>The static <see cref="M:Microsoft.JScript.BooleanPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.BooleanPrototype" /> class.</summary>
  Boolean_toString,
  /// <summary>The static <see cref="M:Microsoft.JScript.BooleanPrototype.valueOf(System.Object)" /> method of the <see cref="T:Microsoft.JScript.BooleanPrototype" /> class.</summary>
  Boolean_valueOf,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getDate(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getDate,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getDay(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getDay,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getFullYear(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getFullYear,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getHours(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getHours,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getMilliseconds(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getMilliseconds,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getMinutes(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getMinutes,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getMonth(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getMonth,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getSeconds(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getSeconds,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getTime(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getTime,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getTimezoneOffset(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getTimezoneOffset,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getUTCDate(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getUTCDate,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getUTCDay(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getUTCDay,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getUTCFullYear(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getUTCFullYear,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getUTCHours(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getUTCHours,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getUTCMilliseconds(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getUTCMilliseconds,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getUTCMinutes(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getUTCMinutes,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getUTCMonth(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getUTCMonth,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getUTCSeconds(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getUTCSeconds,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getVarDate(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getVarDate,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.getYear(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_getYear,
  /// <summary>The static <see cref="M:Microsoft.JScript.DateConstructor.parse(System.String)" /> method of the <see cref="T:Microsoft.JScript.DateConstructor" /> class.</summary>
  Date_parse,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setDate(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setDate,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setFullYear(System.Object,System.Double,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setFullYear,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setHours(System.Object,System.Double,System.Object,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setHours,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setMinutes(System.Object,System.Double,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setMinutes,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setMilliseconds(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setMilliseconds,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setMonth(System.Object,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setMonth,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setSeconds(System.Object,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setSeconds,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setTime(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setTime,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setUTCDate(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setUTCDate,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setUTCFullYear(System.Object,System.Double,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setUTCFullYear,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setUTCHours(System.Object,System.Double,System.Object,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setUTCHours,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setUTCMinutes(System.Object,System.Double,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setUTCMinutes,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setUTCMilliseconds(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setUTCMilliseconds,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setUTCMonth(System.Object,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setUTCMonth,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setUTCSeconds(System.Object,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setUTCSeconds,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.setYear(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_setYear,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.toDateString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_toDateString,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.toGMTString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_toGMTString,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.toLocaleDateString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_toLocaleDateString,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.toLocaleString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_toLocaleString,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.toLocaleTimeString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_toLocaleTimeString,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_toString,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.toTimeString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_toTimeString,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.toUTCString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_toUTCString,
  /// <summary>The static <see cref="M:Microsoft.JScript.DateConstructor.UTC(System.Object,System.Object,System.Object,System.Object,System.Object,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.DateConstructor" /> class.</summary>
  Date_UTC,
  /// <summary>The static <see cref="M:Microsoft.JScript.DatePrototype.valueOf(System.Object)" /> method of the <see cref="T:Microsoft.JScript.DatePrototype" /> class.</summary>
  Date_valueOf,
  /// <summary>The static <see cref="M:Microsoft.JScript.EnumeratorPrototype.atEnd(System.Object)" /> method of the <see cref="T:Microsoft.JScript.EnumeratorPrototype" /> class.</summary>
  Enumerator_atEnd,
  /// <summary>The static <see cref="M:Microsoft.JScript.EnumeratorPrototype.item(System.Object)" /> method of the <see cref="T:Microsoft.JScript.EnumeratorPrototype" /> class.</summary>
  Enumerator_item,
  /// <summary>The static <see cref="M:Microsoft.JScript.EnumeratorPrototype.moveFirst(System.Object)" /> method of the <see cref="T:Microsoft.JScript.EnumeratorPrototype" /> class.</summary>
  Enumerator_moveFirst,
  /// <summary>The static <see cref="M:Microsoft.JScript.EnumeratorPrototype.moveNext(System.Object)" /> method of the <see cref="T:Microsoft.JScript.EnumeratorPrototype" /> class.</summary>
  Enumerator_moveNext,
  /// <summary>The static <see cref="M:Microsoft.JScript.ErrorPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ErrorPrototype" /> class.</summary>
  Error_toString,
  /// <summary>The static <see cref="M:Microsoft.JScript.FunctionPrototype.apply(System.Object,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.FunctionPrototype" /> class.</summary>
  Function_apply,
  /// <summary>The static <see cref="M:Microsoft.JScript.FunctionPrototype.call(System.Object,System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.FunctionPrototype" /> class.</summary>
  Function_call,
  /// <summary>The static <see cref="M:Microsoft.JScript.FunctionPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.FunctionPrototype" /> class.</summary>
  Function_toString,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.CollectGarbage" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_CollectGarbage,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.decodeURI(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_decodeURI,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.decodeURIComponent(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_decodeURIComponent,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.encodeURI(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_encodeURI,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.encodeURIComponent(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_encodeURIComponent,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.escape(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_escape,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.eval(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_eval,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.GetObject(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_GetObject,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.isNaN(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_isNaN,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.isFinite(System.Double)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_isFinite,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.parseFloat(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_parseFloat,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.parseInt(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_parseInt,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.ScriptEngine" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_ScriptEngine,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.ScriptEngineBuildVersion" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_ScriptEngineBuildVersion,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.ScriptEngineMajorVersion" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_ScriptEngineMajorVersion,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.ScriptEngineMinorVersion" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_ScriptEngineMinorVersion,
  /// <summary>The static <see cref="M:Microsoft.JScript.GlobalObject.unescape(System.Object)" /> method of the <see cref="T:Microsoft.JScript.GlobalObject" /> class.</summary>
  Global_unescape,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.abs(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_abs,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.acos(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_acos,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.asin(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_asin,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.atan(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_atan,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.atan2(System.Double,System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_atan2,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.ceil(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_ceil,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.cos(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_cos,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.exp(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_exp,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.floor(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_floor,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.log(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_log,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.max(System.Object,System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_max,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.min(System.Object,System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_min,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.pow(System.Double,System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_pow,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.random" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_random,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.round(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_round,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.sin(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_sin,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.sqrt(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_sqrt,
  /// <summary>The static <see cref="M:Microsoft.JScript.MathObject.tan(System.Double)" /> method of the <see cref="T:Microsoft.JScript.MathObject" /> class.</summary>
  Math_tan,
  /// <summary>The static <see cref="M:Microsoft.JScript.NumberPrototype.toExponential(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class.</summary>
  Number_toExponential,
  /// <summary>The static <see cref="M:Microsoft.JScript.NumberPrototype.toFixed(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class.</summary>
  Number_toFixed,
  /// <summary>The static <see cref="M:Microsoft.JScript.NumberPrototype.toLocaleString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class.</summary>
  Number_toLocaleString,
  /// <summary>The static <see cref="M:Microsoft.JScript.NumberPrototype.toPrecision(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class.</summary>
  Number_toPrecision,
  /// <summary>The static <see cref="M:Microsoft.JScript.NumberPrototype.toString(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class.</summary>
  Number_toString,
  /// <summary>The static <see cref="M:Microsoft.JScript.NumberPrototype.valueOf(System.Object)" /> method of the <see cref="T:Microsoft.JScript.NumberPrototype" /> class.</summary>
  Number_valueOf,
  /// <summary>The static <see cref="M:Microsoft.JScript.ObjectPrototype.hasOwnProperty(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class.</summary>
  Object_hasOwnProperty,
  /// <summary>The static <see cref="M:Microsoft.JScript.ObjectPrototype.isPrototypeOf(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class.</summary>
  Object_isPrototypeOf,
  /// <summary>The static <see cref="M:Microsoft.JScript.ObjectPrototype.propertyIsEnumerable(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class.</summary>
  Object_propertyIsEnumerable,
  /// <summary>The static <see cref="M:Microsoft.JScript.ObjectPrototype.toLocaleString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class.</summary>
  Object_toLocaleString,
  /// <summary>The static <see cref="M:Microsoft.JScript.ObjectPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class.</summary>
  Object_toString,
  /// <summary>The static <see cref="M:Microsoft.JScript.ObjectPrototype.valueOf(System.Object)" /> method of the <see cref="T:Microsoft.JScript.ObjectPrototype" /> class.</summary>
  Object_valueOf,
  /// <summary>The static <see cref="M:Microsoft.JScript.RegExpPrototype.compile(System.Object,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.RegExpPrototype" /> class.</summary>
  RegExp_compile,
  /// <summary>The static <see cref="M:Microsoft.JScript.RegExpPrototype.exec(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.RegExpPrototype" /> class.</summary>
  RegExp_exec,
  /// <summary>The static <see cref="M:Microsoft.JScript.RegExpPrototype.test(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.RegExpPrototype" /> class.</summary>
  RegExp_test,
  /// <summary>The static <see cref="M:Microsoft.JScript.RegExpPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.RegExpPrototype" /> class.</summary>
  RegExp_toString,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.anchor(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_anchor,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.big(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_big,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.blink(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_blink,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.bold(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_bold,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.charAt(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_charAt,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.charCodeAt(System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_charCodeAt,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.concat(System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_concat,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.fixed(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_fixed,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.fontcolor(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_fontcolor,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.fontsize(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_fontsize,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringConstructor.fromCharCode(System.Object[])" /> method of the <see cref="T:Microsoft.JScript.StringConstructor" /> class.</summary>
  String_fromCharCode,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.indexOf(System.Object,System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_indexOf,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.italics(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_italics,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.lastIndexOf(System.Object,System.Object,System.Double)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_lastIndexOf,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.link(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_link,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.localeCompare(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_localeCompare,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.match(System.Object,Microsoft.JScript.Vsa.VsaEngine,System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_match,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.replace(System.Object,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_replace,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.search(System.Object,Microsoft.JScript.Vsa.VsaEngine,System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_search,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.slice(System.Object,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_slice,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.small(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_small,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.split(System.Object,Microsoft.JScript.Vsa.VsaEngine,System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_split,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.strike(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_strike,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.sub(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_sub,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.substr(System.Object,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_substr,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.substring(System.Object,System.Double,System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_substring,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.sup(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_sup,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.toLocaleLowerCase(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_toLocaleLowerCase,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.toLocaleUpperCase(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_toLocaleUpperCase,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.toLowerCase(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_toLowerCase,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.toString(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_toString,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.toUpperCase(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_toUpperCase,
  /// <summary>The static <see cref="M:Microsoft.JScript.StringPrototype.valueOf(System.Object)" /> method of the <see cref="T:Microsoft.JScript.StringPrototype" /> class.</summary>
  String_valueOf,
  /// <summary>The static <see cref="M:Microsoft.JScript.VBArrayPrototype.dimensions(System.Object)" /> method of the <see cref="T:Microsoft.JScript.VBArrayPrototype" /> class.</summary>
  VBArray_dimensions,
  /// <summary>The static <see cref="M:Microsoft.JScript.VBArrayPrototype.getItem(System.Object,System.Object[])" /> method of the <see cref="T:Microsoft.JScript.VBArrayPrototype" /> class.</summary>
  VBArray_getItem,
  /// <summary>The static <see cref="M:Microsoft.JScript.VBArrayPrototype.lbound(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.VBArrayPrototype" /> class.</summary>
  VBArray_lbound,
  /// <summary>The static <see cref="M:Microsoft.JScript.VBArrayPrototype.toArray(System.Object,Microsoft.JScript.Vsa.VsaEngine)" /> method of the <see cref="T:Microsoft.JScript.VBArrayPrototype" /> class.</summary>
  VBArray_toArray,
  /// <summary>The static <see cref="M:Microsoft.JScript.VBArrayPrototype.ubound(System.Object,System.Object)" /> method of the <see cref="T:Microsoft.JScript.VBArrayPrototype" /> class.</summary>
  VBArray_ubound,
}
