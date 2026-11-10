// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.JSBinder
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.JScript;

[Serializable]
internal sealed class JSBinder : Binder
{
  internal static readonly JSBinder ob = new JSBinder();

  internal static object[] ArrangeNamedArguments(
    MethodBase method,
    object[] args,
    string[] namedParameters)
  {
    ParameterInfo[] parameters = method.GetParameters();
    int length1 = parameters.Length;
    object[] target = length1 != 0 ? new object[length1] : throw new JScriptException(JSError.MissingNameParameter);
    int length2 = args.Length;
    int length3 = namedParameters.Length;
    int n = length2 - length3;
    ArrayObject.Copy(args, length3, target, 0, n);
    for (int index1 = 0; index1 < length3; ++index1)
    {
      string namedParameter = namedParameters[index1];
      if (namedParameter == null || namedParameter.Equals(""))
        throw new JScriptException(JSError.MustProvideNameForNamedParameter);
      int index2;
      for (index2 = n; index2 < length1; ++index2)
      {
        if (namedParameter.Equals(parameters[index2].Name))
        {
          if (target[index2] is Empty)
            throw new JScriptException(JSError.DuplicateNamedParameter);
          target[index2] = args[index1];
          break;
        }
      }
      if (index2 == length1)
        throw new JScriptException(JSError.MissingNameParameter);
    }
    if (method is JSMethod)
      return target;
    for (int index = 0; index < length1; ++index)
    {
      if (target[index] == null || target[index] == Missing.Value)
      {
        object defaultParameterValue = TypeReferences.GetDefaultParameterValue(parameters[index]);
        target[index] = defaultParameterValue != System.Convert.DBNull ? defaultParameterValue : throw new ArgumentException(parameters[index].Name);
      }
    }
    return target;
  }

  public override FieldInfo BindToField(
    BindingFlags bindAttr,
    FieldInfo[] match,
    object value,
    CultureInfo locale)
  {
    if (value == null)
      value = (object) DBNull.Value;
    int num1 = int.MaxValue;
    int num2 = 0;
    FieldInfo field = (FieldInfo) null;
    Type type = value.GetType();
    int index = 0;
    for (int length = match.Length; index < length; ++index)
    {
      FieldInfo fieldInfo = match[index];
      int num3 = JSBinder.TypeDistance(Microsoft.JScript.Runtime.TypeRefs, fieldInfo.FieldType, type);
      if (num3 < num1)
      {
        num1 = num3;
        field = fieldInfo;
        num2 = 0;
      }
      else if (num3 == num1)
        ++num2;
    }
    if (num2 > 0)
      throw new AmbiguousMatchException();
    return field;
  }

  public override MethodBase BindToMethod(
    BindingFlags bindingAttr,
    MethodBase[] match,
    ref object[] args,
    ParameterModifier[] modifiers,
    CultureInfo locale,
    string[] namedParameters,
    out object state)
  {
    state = (object) null;
    return JSBinder.SelectMethodBase(Microsoft.JScript.Runtime.TypeRefs, match, ref args, modifiers, namedParameters);
  }

  public override object ChangeType(object value, Type target_type, CultureInfo locale)
  {
    return Convert.CoerceT(value, target_type);
  }

  internal static MemberInfo[] GetDefaultMembers(IReflect ir)
  {
    return JSBinder.GetDefaultMembers(Globals.TypeRefs, ir);
  }

  internal static MemberInfo[] GetDefaultMembers(TypeReferences typeRefs, IReflect ir)
  {
    while (ir is ClassScope)
    {
      ClassScope classScope = (ClassScope) ir;
      classScope.owner.IsExpando();
      if ((PropertyInfo) classScope.itemProp != (PropertyInfo) null)
        return new MemberInfo[1]
        {
          (MemberInfo) classScope.itemProp
        };
      ir = (IReflect) classScope.GetParent();
      if (ir is WithObject)
        ir = (IReflect) ((WithObject) ir).contained_object;
    }
    if ((object) (ir as Type) != null)
      return JSBinder.GetDefaultMembers((Type) ir);
    return ir is JSObject ? typeRefs.ScriptObject.GetDefaultMembers() : (MemberInfo[]) null;
  }

  internal static MemberInfo[] GetDefaultMembers(Type t)
  {
    for (; t != typeof (object) && t != (Type) null; t = t.BaseType)
    {
      MemberInfo[] defaultMembers = t.GetDefaultMembers();
      if (defaultMembers != null && defaultMembers.Length != 0)
        return defaultMembers;
    }
    return (MemberInfo[]) null;
  }

  internal static MethodInfo GetDefaultPropertyForArrayIndex(
    Type t,
    int index,
    Type elementType,
    bool getSetter)
  {
    try
    {
      MemberInfo[] defaultMembers = JSBinder.GetDefaultMembers(Microsoft.JScript.Runtime.TypeRefs, (IReflect) t);
      int length;
      if (defaultMembers == null || (length = defaultMembers.Length) == 0)
        return (MethodInfo) null;
      for (int index1 = 0; index1 < length; ++index1)
      {
        MemberInfo memberInfo = defaultMembers[index1];
        MemberTypes memberType = memberInfo.MemberType;
        MethodInfo propertyForArrayIndex;
        switch (memberType)
        {
          case MemberTypes.Method:
            propertyForArrayIndex = (MethodInfo) memberInfo;
            break;
          case MemberTypes.Property:
            propertyForArrayIndex = ((PropertyInfo) memberInfo).GetGetMethod();
            break;
          default:
            continue;
        }
        if (propertyForArrayIndex != (MethodInfo) null)
        {
          ParameterInfo[] parameters = propertyForArrayIndex.GetParameters();
          if (parameters == null || parameters.Length == 0)
          {
            Type returnType = propertyForArrayIndex.ReturnType;
            if (typeof (Array).IsAssignableFrom(returnType) || typeof (IList).IsAssignableFrom(returnType))
              return propertyForArrayIndex;
          }
          else if (parameters.Length == 1 && memberType == MemberTypes.Property)
          {
            PropertyInfo propertyInfo = (PropertyInfo) memberInfo;
            if (elementType == (Type) null || propertyInfo.PropertyType.IsAssignableFrom(elementType))
            {
              try
              {
                Convert.CoerceT((object) index, parameters[0].ParameterType);
                return getSetter ? propertyInfo.GetSetMethod() : propertyForArrayIndex;
              }
              catch (JScriptException ex)
              {
              }
            }
          }
        }
      }
    }
    catch (InvalidOperationException ex)
    {
    }
    return (MethodInfo) null;
  }

  internal static MemberInfo[] GetInterfaceMembers(string name, Type t)
  {
    BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
    MemberInfo[] member1 = t.GetMember(name, bindingAttr);
    Type[] interfaces = t.GetInterfaces();
    if (interfaces == null || interfaces.Length == 0)
      return member1;
    ArrayList arrayList = new ArrayList((ICollection) interfaces);
    MemberInfoList memberInfoList = new MemberInfoList();
    memberInfoList.AddRange(member1);
    for (int index = 0; index < arrayList.Count; ++index)
    {
      Type type1 = (Type) arrayList[index];
      MemberInfo[] member2 = type1.GetMember(name, bindingAttr);
      if (member2 != null)
        memberInfoList.AddRange(member2);
      foreach (Type type2 in type1.GetInterfaces())
      {
        if (arrayList.IndexOf((object) type2) == -1)
          arrayList.Add((object) type2);
      }
    }
    return memberInfoList.ToArray();
  }

  private static bool FormalParamTypeIsObject(ParameterInfo par)
  {
    return par is ParameterDeclaration parameterDeclaration ? (object) parameterDeclaration.ParameterIReflect == (object) Typeob.Object : par.ParameterType == Typeob.Object;
  }

  public override void ReorderArgumentArray(ref object[] args, object state)
  {
  }

  internal static MemberInfo Select(
    TypeReferences typeRefs,
    MemberInfo[] match,
    int matches,
    IReflect[] argIRs,
    MemberTypes memberType)
  {
    int candidates = 0;
    ParameterInfo[][] fparams = new ParameterInfo[matches][];
    bool flag = memberType == MemberTypes.Method;
    for (int index = 0; index < matches; ++index)
    {
      MemberInfo getMethod = match[index];
      if (getMethod is PropertyInfo & flag)
        getMethod = (MemberInfo) ((PropertyInfo) getMethod).GetGetMethod(true);
      if (!(getMethod == (MemberInfo) null) && getMethod.MemberType == memberType)
      {
        fparams[index] = (object) (getMethod as PropertyInfo) == null ? ((MethodBase) getMethod).GetParameters() : ((PropertyInfo) getMethod).GetIndexParameters();
        ++candidates;
      }
    }
    int index1 = JSBinder.SelectBest(typeRefs, match, matches, argIRs, fparams, (object[][]) null, candidates, argIRs.Length);
    return index1 < 0 ? (MemberInfo) null : match[index1];
  }

  internal static MemberInfo Select(
    TypeReferences typeRefs,
    MemberInfo[] match,
    int matches,
    ref object[] args,
    string[] namedParameters,
    MemberTypes memberType)
  {
    bool flag1 = false;
    if (namedParameters != null && namedParameters.Length != 0)
    {
      if (args.Length < namedParameters.Length)
        throw new JScriptException(JSError.MoreNamedParametersThanArguments);
      flag1 = true;
    }
    int candidates = 0;
    ParameterInfo[][] fparams = new ParameterInfo[matches][];
    object[][] aparams = new object[matches][];
    bool flag2 = memberType == MemberTypes.Method;
    for (int index = 0; index < matches; ++index)
    {
      MemberInfo getMethod = match[index];
      if (flag2 && getMethod.MemberType == MemberTypes.Property)
        getMethod = (MemberInfo) ((PropertyInfo) getMethod).GetGetMethod(true);
      if (getMethod.MemberType == memberType)
      {
        fparams[index] = memberType != MemberTypes.Property ? ((MethodBase) getMethod).GetParameters() : ((PropertyInfo) getMethod).GetIndexParameters();
        aparams[index] = !flag1 ? args : JSBinder.ArrangeNamedArguments((MethodBase) getMethod, args, namedParameters);
        ++candidates;
      }
    }
    int index1 = JSBinder.SelectBest(typeRefs, match, matches, (IReflect[]) null, fparams, aparams, candidates, args.Length);
    if (index1 < 0)
      return (MemberInfo) null;
    args = aparams[index1];
    MemberInfo getMethod1 = match[index1];
    if (flag2 && getMethod1.MemberType == MemberTypes.Property)
      getMethod1 = (MemberInfo) ((PropertyInfo) getMethod1).GetGetMethod(true);
    return getMethod1;
  }

  private static int SelectBest(
    TypeReferences typeRefs,
    MemberInfo[] match,
    int matches,
    IReflect[] argIRs,
    ParameterInfo[][] fparams,
    object[][] aparams,
    int candidates,
    int parameters)
  {
    switch (candidates)
    {
      case 0:
        return -1;
      case 1:
        for (int index = 0; index < matches; ++index)
        {
          if (fparams[index] != null)
            return index;
        }
        break;
    }
    bool[] flagArray = new bool[matches];
    int[] numArray = new int[matches];
    for (int index1 = 0; index1 < matches; ++index1)
    {
      ParameterInfo[] fparam = fparams[index1];
      if (fparam != null)
      {
        int length = fparam.Length;
        if ((argIRs == null ? aparams[index1].Length : argIRs.Length) > length && (length == 0 || !CustomAttribute.IsDefined(fparam[length - 1], typeof (ParamArrayAttribute), false)))
        {
          fparams[index1] = (ParameterInfo[]) null;
          --candidates;
        }
        else
        {
          for (int index2 = parameters; index2 < length; ++index2)
          {
            ParameterInfo parameterInfo = fparam[index2];
            if (index2 != length - 1 || !CustomAttribute.IsDefined(parameterInfo, typeof (ParamArrayAttribute), false))
            {
              if (TypeReferences.GetDefaultParameterValue(parameterInfo) is DBNull)
                numArray[index1] = 50;
            }
            else
              break;
          }
        }
      }
    }
    int index3 = 0;
    while (candidates > 1)
    {
      int num1 = 0;
      int num2 = int.MaxValue;
      bool flag1 = false;
      for (int index4 = 0; index4 < matches; ++index4)
      {
        int num3 = 0;
        ParameterInfo[] fparam = fparams[index4];
        if (fparam != null)
        {
          IReflect actual = (IReflect) typeRefs.Missing;
          if (argIRs == null)
          {
            if (aparams[index4].Length > index3)
            {
              object obj = aparams[index4][index3] ?? (object) DBNull.Value;
              actual = (IReflect) typeRefs.ToReferenceContext(obj.GetType());
            }
          }
          else if (index3 < parameters)
            actual = argIRs[index3];
          int length = fparam.Length;
          if (length - 1 > index3)
            ++num1;
          IReflect formal = (IReflect) typeRefs.Missing;
          if (length > 0 && index3 >= length - 1 && CustomAttribute.IsDefined(fparam[length - 1], typeof (ParamArrayAttribute), false) && !(actual is TypedArray) && (object) actual != (object) typeRefs.ArrayObject && ((object) (actual as Type) == null || !((Type) actual).IsArray))
          {
            ParameterInfo parameterInfo = fparam[length - 1];
            formal = !(parameterInfo is ParameterDeclaration) ? (IReflect) parameterInfo.ParameterType.GetElementType() : ((TypedArray) ((ParameterDeclaration) parameterInfo).ParameterIReflect).elementType;
            if (index3 == length - 1)
              ++numArray[index4];
          }
          else if (index3 < length)
          {
            ParameterInfo parameter = fparam[index3];
            formal = parameter is ParameterDeclaration ? ((ParameterDeclaration) parameter).ParameterIReflect : (IReflect) parameter.ParameterType;
            if ((object) actual == (object) typeRefs.Missing && !(TypeReferences.GetDefaultParameterValue(parameter) is DBNull))
            {
              actual = formal;
              num3 = 1;
            }
          }
          int num4 = JSBinder.TypeDistance(typeRefs, formal, actual) + numArray[index4] + num3;
          if (num4 == num2)
          {
            if (index3 == length - 1 && flagArray[index4])
            {
              --candidates;
              fparams[index4] = (ParameterInfo[]) null;
            }
            flag1 = flag1 && flagArray[index4];
          }
          else if (num4 > num2)
          {
            if (flag1 && index3 < length && JSBinder.FormalParamTypeIsObject(fparams[index4][index3]))
              num2 = num4;
            else if (index3 <= length - 1 || (object) actual != (object) typeRefs.Missing || !CustomAttribute.IsDefined(fparam[length - 1], typeof (ParamArrayAttribute), false))
              flagArray[index4] = true;
          }
          else
          {
            if (candidates == 1 && !flagArray[index4])
              return index4;
            flag1 = flagArray[index4];
            for (int index5 = 0; index5 < index4; ++index5)
            {
              if (fparams[index5] != null && !flagArray[index5])
              {
                bool flag2 = fparams[index5].Length <= index3;
                if ((!flag2 || parameters > index3) && (flag2 || !flag1 || !JSBinder.FormalParamTypeIsObject(fparams[index5][index3])))
                  flagArray[index5] = true;
              }
            }
            num2 = num4;
          }
        }
      }
      if (index3 < parameters - 1 || num1 >= 1)
        ++index3;
      else
        break;
    }
    int index6 = -1;
    for (int index7 = 0; index7 < matches && candidates > 0; ++index7)
    {
      ParameterInfo[] fparam = fparams[index7];
      if (fparam != null)
      {
        if (flagArray[index7])
        {
          --candidates;
          fparams[index7] = (ParameterInfo[]) null;
        }
        else
        {
          int length = fparam.Length;
          if (index6 == -1)
            index6 = index7;
          else if (Class.ParametersMatch(fparam, fparams[index6]))
          {
            MemberInfo method = match[index6];
            JSWrappedMethod jsWrappedMethod = match[index6] as JSWrappedMethod;
            if ((MethodInfo) jsWrappedMethod != (MethodInfo) null)
              method = (MemberInfo) jsWrappedMethod.method;
            switch (method)
            {
              case JSFieldMethod _:
              case JSConstructor _:
              case JSProperty _:
                --candidates;
                fparams[index7] = (ParameterInfo[]) null;
                continue;
              default:
                Type declaringType1 = match[index6].DeclaringType;
                Type declaringType2 = match[index7].DeclaringType;
                if (declaringType1 != declaringType2)
                {
                  if (declaringType2.IsAssignableFrom(declaringType1))
                  {
                    --candidates;
                    fparams[index7] = (ParameterInfo[]) null;
                    continue;
                  }
                  if (declaringType1.IsAssignableFrom(declaringType2))
                  {
                    fparams[index6] = (ParameterInfo[]) null;
                    index6 = index7;
                    --candidates;
                    continue;
                  }
                  continue;
                }
                continue;
            }
          }
        }
      }
    }
    if (candidates != 1)
      throw new AmbiguousMatchException();
    return index6;
  }

  internal static Type HandleCoClassAttribute(Type t)
  {
    object[] customAttributes = CustomAttribute.GetCustomAttributes((MemberInfo) t, typeof (CoClassAttribute), false);
    if (customAttributes != null && customAttributes.Length == 1)
    {
      t = ((CoClassAttribute) customAttributes[0]).CoClass;
      if (!t.IsPublic)
        throw new JScriptException(JSError.NotAccessible, new Context(new DocumentContext("", (VsaEngine) null), t.ToString()));
    }
    return t;
  }

  internal static ConstructorInfo SelectConstructor(
    MemberInfo[] match,
    ref object[] args,
    string[] namedParameters)
  {
    return JSBinder.SelectConstructor(Globals.TypeRefs, match, ref args, namedParameters);
  }

  internal static ConstructorInfo SelectConstructor(
    TypeReferences typeRefs,
    MemberInfo[] match,
    ref object[] args,
    string[] namedParameters)
  {
    if (match == null)
      return (ConstructorInfo) null;
    int length = match.Length;
    if (length == 0)
      return (ConstructorInfo) null;
    if (length == 1)
    {
      Type t = match[0] as Type;
      if (t != (Type) null)
      {
        if (t.IsInterface && t.IsImport)
          t = JSBinder.HandleCoClassAttribute(t);
        match = (MemberInfo[]) t.GetConstructors();
        length = match.Length;
      }
    }
    return length == 1 ? match[0] as ConstructorInfo : (ConstructorInfo) JSBinder.Select(typeRefs, match, length, ref args, namedParameters, MemberTypes.Constructor);
  }

  internal static ConstructorInfo SelectConstructor(MemberInfo[] match, IReflect[] argIRs)
  {
    return JSBinder.SelectConstructor(Globals.TypeRefs, match, argIRs);
  }

  internal static ConstructorInfo SelectConstructor(
    TypeReferences typeRefs,
    MemberInfo[] match,
    IReflect[] argIRs)
  {
    if (match == null)
      return (ConstructorInfo) null;
    int length = match.Length;
    if (length == 1)
    {
      object obj = (object) match[0];
      if (obj is JSGlobalField)
        obj = ((FieldInfo) obj).GetValue((object) null);
      Type t = obj as Type;
      if (t != (Type) null)
      {
        if (t.IsInterface && t.IsImport)
          t = JSBinder.HandleCoClassAttribute(t);
        match = (MemberInfo[]) t.GetConstructors();
      }
      length = match.Length;
    }
    if (length == 0)
      return (ConstructorInfo) null;
    return length == 1 ? match[0] as ConstructorInfo : (ConstructorInfo) JSBinder.Select(typeRefs, match, length, argIRs, MemberTypes.Constructor);
  }

  internal static MemberInfo SelectCallableMember(MemberInfo[] match, IReflect[] argIRs)
  {
    if (match == null)
      return (MemberInfo) null;
    int length = match.Length;
    MemberInfo memberInfo;
    switch (length)
    {
      case 0:
        return (MemberInfo) null;
      case 1:
        memberInfo = match[0];
        break;
      default:
        memberInfo = JSBinder.Select(Globals.TypeRefs, match, length, argIRs, MemberTypes.Method);
        break;
    }
    return memberInfo;
  }

  internal static MethodInfo SelectMethod(
    MemberInfo[] match,
    ref object[] args,
    string[] namedParameters)
  {
    return JSBinder.SelectMethod(Globals.TypeRefs, match, ref args, namedParameters);
  }

  internal static MethodInfo SelectMethod(
    TypeReferences typeRefs,
    MemberInfo[] match,
    ref object[] args,
    string[] namedParameters)
  {
    if (match == null)
      return (MethodInfo) null;
    int length = match.Length;
    MemberInfo memberInfo1;
    switch (length)
    {
      case 0:
        return (MethodInfo) null;
      case 1:
        memberInfo1 = match[0];
        break;
      default:
        memberInfo1 = JSBinder.Select(typeRefs, match, length, ref args, namedParameters, MemberTypes.Method);
        break;
    }
    MemberInfo memberInfo2 = memberInfo1;
    if (memberInfo2 != (MemberInfo) null && memberInfo2.MemberType == MemberTypes.Property)
      memberInfo2 = (MemberInfo) ((PropertyInfo) memberInfo2).GetGetMethod(true);
    return memberInfo2 as MethodInfo;
  }

  internal static MethodInfo SelectMethod(MemberInfo[] match, IReflect[] argIRs)
  {
    return JSBinder.SelectMethod(Globals.TypeRefs, match, argIRs);
  }

  internal static MethodInfo SelectMethod(
    TypeReferences typeRefs,
    MemberInfo[] match,
    IReflect[] argIRs)
  {
    if (match == null)
      return (MethodInfo) null;
    int length = match.Length;
    MemberInfo memberInfo1;
    switch (length)
    {
      case 0:
        return (MethodInfo) null;
      case 1:
        memberInfo1 = match[0];
        break;
      default:
        memberInfo1 = JSBinder.Select(typeRefs, match, length, argIRs, MemberTypes.Method);
        break;
    }
    MemberInfo memberInfo2 = memberInfo1;
    return memberInfo2 != (MemberInfo) null && memberInfo2.MemberType == MemberTypes.Property ? ((PropertyInfo) memberInfo2).GetGetMethod(true) : memberInfo2 as MethodInfo;
  }

  public override MethodBase SelectMethod(
    BindingFlags bindingAttr,
    MethodBase[] match,
    Type[] types,
    ParameterModifier[] modifiers)
  {
    if (match == null)
      return (MethodBase) null;
    int length = match.Length;
    switch (length)
    {
      case 0:
        return (MethodBase) null;
      case 1:
        return match[0];
      default:
        return match[0].MemberType == MemberTypes.Constructor ? (MethodBase) JSBinder.Select(Microsoft.JScript.Runtime.TypeRefs, (MemberInfo[]) match, length, (IReflect[]) types, MemberTypes.Constructor) : (MethodBase) JSBinder.Select(Microsoft.JScript.Runtime.TypeRefs, (MemberInfo[]) match, length, (IReflect[]) types, MemberTypes.Method);
    }
  }

  private static MethodBase SelectMethodBase(
    TypeReferences typeRefs,
    MethodBase[] match,
    ref object[] args,
    ParameterModifier[] modifiers,
    string[] namedParameters)
  {
    if (match == null)
      return (MethodBase) null;
    int length = match.Length;
    switch (length)
    {
      case 0:
        return (MethodBase) null;
      case 1:
        return match[0];
      default:
        MethodBase methodBase = (MethodBase) JSBinder.Select(typeRefs, (MemberInfo[]) match, length, ref args, namedParameters, MemberTypes.Method);
        if (methodBase == (MethodBase) null)
          methodBase = (MethodBase) JSBinder.Select(typeRefs, (MemberInfo[]) match, length, ref args, namedParameters, MemberTypes.Constructor);
        return methodBase;
    }
  }

  internal static MethodInfo SelectOperator(MethodInfo op1, MethodInfo op2, Type t1, Type t2)
  {
    ParameterInfo[] parameterInfoArray1 = (ParameterInfo[]) null;
    if (op1 == (MethodInfo) null || (op1.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || (parameterInfoArray1 = op1.GetParameters()).Length != 2)
      op1 = (MethodInfo) null;
    ParameterInfo[] parameterInfoArray2 = (ParameterInfo[]) null;
    if (op2 == (MethodInfo) null || (op2.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || (parameterInfoArray2 = op2.GetParameters()).Length != 2)
      op2 = (MethodInfo) null;
    return op1 == (MethodInfo) null || !(op2 == (MethodInfo) null) && JSBinder.TypeDistance(Globals.TypeRefs, parameterInfoArray1[0].ParameterType, t1) + JSBinder.TypeDistance(Globals.TypeRefs, parameterInfoArray1[1].ParameterType, t2) > JSBinder.TypeDistance(Globals.TypeRefs, parameterInfoArray2[0].ParameterType, t1) + JSBinder.TypeDistance(Globals.TypeRefs, parameterInfoArray2[1].ParameterType, t2) ? op2 : op1;
  }

  internal static PropertyInfo SelectProperty(MemberInfo[] match, object[] args)
  {
    return JSBinder.SelectProperty(Globals.TypeRefs, match, args);
  }

  internal static PropertyInfo SelectProperty(
    TypeReferences typeRefs,
    MemberInfo[] match,
    object[] args)
  {
    if (match == null)
      return (PropertyInfo) null;
    int length = match.Length;
    switch (length)
    {
      case 0:
        return (PropertyInfo) null;
      case 1:
        return match[0] as PropertyInfo;
      default:
        int candidates = 0;
        PropertyInfo propertyInfo = (PropertyInfo) null;
        ParameterInfo[][] fparams = new ParameterInfo[length][];
        object[][] aparams = new object[length][];
        for (int index = 0; index < length; ++index)
        {
          MemberInfo memberInfo = match[index];
          if (memberInfo.MemberType == MemberTypes.Property)
          {
            MethodInfo getMethod = (propertyInfo = (PropertyInfo) memberInfo).GetGetMethod(true);
            fparams[index] = !(getMethod == (MethodInfo) null) ? getMethod.GetParameters() : propertyInfo.GetIndexParameters();
            aparams[index] = args;
            ++candidates;
          }
        }
        if (candidates <= 1)
          return propertyInfo;
        int index1 = JSBinder.SelectBest(typeRefs, match, length, (IReflect[]) null, fparams, aparams, candidates, args.Length);
        return index1 < 0 ? (PropertyInfo) null : (PropertyInfo) match[index1];
    }
  }

  public override PropertyInfo SelectProperty(
    BindingFlags bindingAttr,
    PropertyInfo[] match,
    Type rtype,
    Type[] types,
    ParameterModifier[] modifiers)
  {
    if (match == null)
      return (PropertyInfo) null;
    int length = match.Length;
    switch (length)
    {
      case 0:
        return (PropertyInfo) null;
      case 1:
        return match[0];
      default:
        int candidates = 0;
        PropertyInfo propertyInfo = (PropertyInfo) null;
        int maxValue = int.MaxValue;
        ParameterInfo[][] fparams = new ParameterInfo[length][];
        for (int index1 = 0; index1 < length; ++index1)
        {
          propertyInfo = match[index1];
          if (rtype != (Type) null)
          {
            int num = JSBinder.TypeDistance(Globals.TypeRefs, propertyInfo.PropertyType, rtype);
            if (num <= maxValue)
            {
              if (num < maxValue)
              {
                for (int index2 = 0; index2 < index1; ++index2)
                {
                  if (fparams[index2] != null)
                  {
                    fparams[index2] = (ParameterInfo[]) null;
                    --candidates;
                  }
                }
              }
            }
            else
              continue;
          }
          fparams[index1] = propertyInfo.GetIndexParameters();
          ++candidates;
        }
        if (candidates <= 1)
          return propertyInfo;
        int index = JSBinder.SelectBest(Globals.TypeRefs, (MemberInfo[]) match, length, (IReflect[]) types, fparams, (object[][]) null, candidates, types.Length);
        return index < 0 ? (PropertyInfo) null : match[index];
    }
  }

  internal static PropertyInfo SelectProperty(MemberInfo[] match, IReflect[] argIRs)
  {
    return JSBinder.SelectProperty(Globals.TypeRefs, match, argIRs);
  }

  internal static PropertyInfo SelectProperty(
    TypeReferences typeRefs,
    MemberInfo[] match,
    IReflect[] argIRs)
  {
    if (match == null)
      return (PropertyInfo) null;
    int length = match.Length;
    switch (length)
    {
      case 0:
        return (PropertyInfo) null;
      case 1:
        return match[0] as PropertyInfo;
      default:
        return (PropertyInfo) JSBinder.Select(typeRefs, match, length, argIRs, MemberTypes.Property);
    }
  }

  private static int TypeDistance(TypeReferences typeRefs, IReflect formal, IReflect actual)
  {
    if (formal is TypedArray)
    {
      if (actual is TypedArray)
      {
        TypedArray typedArray1 = (TypedArray) formal;
        TypedArray typedArray2 = (TypedArray) actual;
        if (typedArray1.rank == typedArray2.rank && JSBinder.TypeDistance(typeRefs, typedArray1.elementType, typedArray2.elementType) == 0)
          return 0;
      }
      else if ((object) (actual as Type) != null)
      {
        TypedArray typedArray = (TypedArray) formal;
        Type type = (Type) actual;
        if (type.IsArray && typedArray.rank == type.GetArrayRank())
          return JSBinder.TypeDistance(typeRefs, typedArray.elementType, (IReflect) type.GetElementType()) != 0 ? 100 : 0;
        if (type == typeRefs.Array || type == typeRefs.ArrayObject)
          return 30;
      }
      return 100;
    }
    if (actual is TypedArray)
    {
      if ((object) (formal as Type) != null)
      {
        Type type = (Type) formal;
        TypedArray typedArray = (TypedArray) actual;
        if (type.IsArray && type.GetArrayRank() == typedArray.rank)
          return JSBinder.TypeDistance(typeRefs, (IReflect) type.GetElementType(), typedArray.elementType) != 0 ? 100 : 0;
        if (type == typeRefs.Array)
          return 30;
        if (type == typeRefs.Object)
          return 50;
      }
      return 100;
    }
    if (formal is ClassScope)
      return actual is ClassScope && ((ClassScope) actual).IsSameOrDerivedFrom((ClassScope) formal) ? 0 : 100;
    if (!(actual is ClassScope))
      return JSBinder.TypeDistance(typeRefs, Convert.ToType(typeRefs, formal), Convert.ToType(typeRefs, actual));
    return (object) (formal as Type) != null && ((ClassScope) actual).IsPromotableTo((Type) formal) ? 0 : 100;
  }

  private static int TypeDistance(TypeReferences typeRefs, Type formal, Type actual)
  {
    TypeCode typeCode1 = Type.GetTypeCode(actual);
    TypeCode typeCode2 = Type.GetTypeCode(formal);
    if (actual.IsEnum)
      typeCode1 = TypeCode.Object;
    if (formal.IsEnum)
      typeCode2 = TypeCode.Object;
    switch (typeCode1)
    {
      case TypeCode.Object:
        if (formal == actual)
          return 0;
        if (formal == typeRefs.Missing)
          return 200;
        if (!formal.IsAssignableFrom(actual))
        {
          if (typeRefs.Array.IsAssignableFrom(formal) && (actual == typeRefs.Array || typeRefs.ArrayObject.IsAssignableFrom(actual)))
            return 10;
          if (typeCode2 == TypeCode.String)
            return 20;
          return actual == typeRefs.ScriptFunction && typeRefs.Delegate.IsAssignableFrom(formal) ? 19 : 100;
        }
        Type[] interfaces = actual.GetInterfaces();
        int length = interfaces.Length;
        for (int index = 0; index < length; ++index)
        {
          if (formal == interfaces[index])
            return index + 1;
        }
        int num = 0;
        while (actual != typeRefs.Object && actual != (Type) null && !(formal == actual))
        {
          actual = actual.BaseType;
          ++num;
        }
        return num + length + 1;
      case TypeCode.DBNull:
        return !(formal == typeRefs.Object) ? 1 : 0;
      case TypeCode.Boolean:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return JSBinder.TypeDistance(typeRefs, formal, actual, 12);
          case TypeCode.Boolean:
            return 0;
          case TypeCode.SByte:
            return 5;
          case TypeCode.Byte:
            return 1;
          case TypeCode.Int16:
            return 6;
          case TypeCode.UInt16:
            return 2;
          case TypeCode.Int32:
            return 7;
          case TypeCode.UInt32:
            return 3;
          case TypeCode.Int64:
            return 8;
          case TypeCode.UInt64:
            return 4;
          case TypeCode.Single:
            return 9;
          case TypeCode.Double:
            return 10;
          case TypeCode.Decimal:
            return 11;
          case TypeCode.String:
            return 13;
          default:
            return 100;
        }
      case TypeCode.Char:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return JSBinder.TypeDistance(typeRefs, formal, actual, 9);
          case TypeCode.Boolean:
            return 14;
          case TypeCode.Char:
            return 0;
          case TypeCode.SByte:
            return 13;
          case TypeCode.Byte:
            return 12;
          case TypeCode.Int16:
            return 11;
          case TypeCode.UInt16:
            return 1;
          case TypeCode.Int32:
            return 3;
          case TypeCode.UInt32:
            return 2;
          case TypeCode.Int64:
            return 5;
          case TypeCode.UInt64:
            return 4;
          case TypeCode.Single:
            return 6;
          case TypeCode.Double:
            return 7;
          case TypeCode.Decimal:
            return 8;
          case TypeCode.String:
            return 10;
          default:
            return 100;
        }
      case TypeCode.SByte:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return JSBinder.TypeDistance(typeRefs, formal, actual, 7);
          case TypeCode.Boolean:
            return 14;
          case TypeCode.SByte:
            return 0;
          case TypeCode.Byte:
            return 9;
          case TypeCode.Int16:
            return 1;
          case TypeCode.UInt16:
            return 10;
          case TypeCode.Int32:
            return 2;
          case TypeCode.UInt32:
            return 12;
          case TypeCode.Int64:
            return 3;
          case TypeCode.UInt64:
            return 13;
          case TypeCode.Single:
            return 4;
          case TypeCode.Double:
            return 5;
          case TypeCode.Decimal:
            return 6;
          case TypeCode.String:
            return 8;
          default:
            return 100;
        }
      case TypeCode.Byte:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return JSBinder.TypeDistance(typeRefs, formal, actual, 11);
          case TypeCode.Boolean:
            return 14;
          case TypeCode.SByte:
            return 13;
          case TypeCode.Byte:
            return 0;
          case TypeCode.Int16:
            return 3;
          case TypeCode.UInt16:
            return 1;
          case TypeCode.Int32:
            return 5;
          case TypeCode.UInt32:
            return 4;
          case TypeCode.Int64:
            return 7;
          case TypeCode.UInt64:
            return 6;
          case TypeCode.Single:
            return 8;
          case TypeCode.Double:
            return 9;
          case TypeCode.Decimal:
            return 10;
          case TypeCode.String:
            return 12;
          default:
            return 100;
        }
      case TypeCode.Int16:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return JSBinder.TypeDistance(typeRefs, formal, actual, 6);
          case TypeCode.Boolean:
            return 14;
          case TypeCode.SByte:
            return 12;
          case TypeCode.Byte:
            return 13;
          case TypeCode.Int16:
            return 0;
          case TypeCode.UInt16:
            return 8;
          case TypeCode.Int32:
            return 1;
          case TypeCode.UInt32:
            return 10;
          case TypeCode.Int64:
            return 2;
          case TypeCode.UInt64:
            return 11;
          case TypeCode.Single:
            return 3;
          case TypeCode.Double:
            return 4;
          case TypeCode.Decimal:
            return 5;
          case TypeCode.String:
            return 7;
          default:
            return 100;
        }
      case TypeCode.UInt16:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return JSBinder.TypeDistance(typeRefs, formal, actual, 9);
          case TypeCode.Boolean:
            return 14;
          case TypeCode.SByte:
            return 13;
          case TypeCode.Byte:
            return 12;
          case TypeCode.Int16:
            return 11;
          case TypeCode.UInt16:
            return 0;
          case TypeCode.Int32:
            return 4;
          case TypeCode.UInt32:
            return 1;
          case TypeCode.Int64:
            return 5;
          case TypeCode.UInt64:
            return 2;
          case TypeCode.Single:
            return 6;
          case TypeCode.Double:
            return 7;
          case TypeCode.Decimal:
            return 8;
          case TypeCode.String:
            return 10;
          default:
            return 100;
        }
      case TypeCode.Int32:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return JSBinder.TypeDistance(typeRefs, formal, actual, 4);
          case TypeCode.Boolean:
            return 14;
          case TypeCode.SByte:
            return 12;
          case TypeCode.Byte:
            return 13;
          case TypeCode.Int16:
            return 9;
          case TypeCode.UInt16:
            return 10;
          case TypeCode.Int32:
            return 0;
          case TypeCode.UInt32:
            return 7;
          case TypeCode.Int64:
            return 1;
          case TypeCode.UInt64:
            return 6;
          case TypeCode.Single:
            return 8;
          case TypeCode.Double:
            return 2;
          case TypeCode.Decimal:
            return 3;
          case TypeCode.String:
            return 5;
          default:
            return 100;
        }
      case TypeCode.UInt32:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return JSBinder.TypeDistance(typeRefs, formal, actual, 5);
          case TypeCode.Boolean:
            return 14;
          case TypeCode.SByte:
            return 13;
          case TypeCode.Byte:
            return 12;
          case TypeCode.Int16:
            return 11;
          case TypeCode.UInt16:
            return 9;
          case TypeCode.Int32:
            return 7;
          case TypeCode.UInt32:
            return 0;
          case TypeCode.Int64:
            return 2;
          case TypeCode.UInt64:
            return 1;
          case TypeCode.Single:
            return 8;
          case TypeCode.Double:
            return 3;
          case TypeCode.Decimal:
            return 4;
          case TypeCode.String:
            return 6;
          default:
            return 100;
        }
      case TypeCode.Int64:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return JSBinder.TypeDistance(typeRefs, formal, actual, 2);
          case TypeCode.Boolean:
            return 14;
          case TypeCode.SByte:
            return 8;
          case TypeCode.Byte:
            return 13;
          case TypeCode.Int16:
            return 7;
          case TypeCode.UInt16:
            return 11;
          case TypeCode.Int32:
            return 6;
          case TypeCode.UInt32:
            return 10;
          case TypeCode.Int64:
            return 0;
          case TypeCode.UInt64:
            return 9;
          case TypeCode.Single:
            return 5;
          case TypeCode.Double:
            return 4;
          case TypeCode.Decimal:
            return 1;
          case TypeCode.String:
            return 3;
          default:
            return 100;
        }
      case TypeCode.UInt64:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return JSBinder.TypeDistance(typeRefs, formal, actual, 2);
          case TypeCode.Boolean:
            return 14;
          case TypeCode.SByte:
            return 13;
          case TypeCode.Byte:
            return 10;
          case TypeCode.Int16:
            return 12;
          case TypeCode.UInt16:
            return 8;
          case TypeCode.Int32:
            return 11;
          case TypeCode.UInt32:
            return 7;
          case TypeCode.Int64:
            return 4;
          case TypeCode.UInt64:
            return 0;
          case TypeCode.Single:
            return 6;
          case TypeCode.Double:
            return 5;
          case TypeCode.Decimal:
            return 1;
          case TypeCode.String:
            return 3;
          default:
            return 100;
        }
      case TypeCode.Single:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return JSBinder.TypeDistance(typeRefs, formal, actual, 12);
          case TypeCode.Boolean:
            return 14;
          case TypeCode.SByte:
            return 10;
          case TypeCode.Byte:
            return 11;
          case TypeCode.Int16:
            return 7;
          case TypeCode.UInt16:
            return 8;
          case TypeCode.Int32:
            return 5;
          case TypeCode.UInt32:
            return 6;
          case TypeCode.Int64:
            return 3;
          case TypeCode.UInt64:
            return 4;
          case TypeCode.Single:
            return 0;
          case TypeCode.Double:
            return 1;
          case TypeCode.Decimal:
            return 2;
          case TypeCode.String:
            return 13;
          default:
            return 100;
        }
      case TypeCode.Double:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return JSBinder.TypeDistance(typeRefs, formal, actual, 12);
          case TypeCode.Boolean:
            return 14;
          case TypeCode.SByte:
            return 10;
          case TypeCode.Byte:
            return 11;
          case TypeCode.Int16:
            return 7;
          case TypeCode.UInt16:
            return 8;
          case TypeCode.Int32:
            return 5;
          case TypeCode.UInt32:
            return 6;
          case TypeCode.Int64:
            return 3;
          case TypeCode.UInt64:
            return 4;
          case TypeCode.Single:
            return 2;
          case TypeCode.Double:
            return 0;
          case TypeCode.Decimal:
            return 1;
          case TypeCode.String:
            return 13;
          default:
            return 100;
        }
      case TypeCode.Decimal:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return !(formal == typeRefs.Object) ? 100 : 12;
          case TypeCode.Boolean:
            return 14;
          case TypeCode.SByte:
            return 10;
          case TypeCode.Byte:
            return 11;
          case TypeCode.Int16:
            return 7;
          case TypeCode.UInt16:
            return 8;
          case TypeCode.Int32:
            return 5;
          case TypeCode.UInt32:
            return 6;
          case TypeCode.Int64:
            return 3;
          case TypeCode.UInt64:
            return 4;
          case TypeCode.Single:
            return 2;
          case TypeCode.Double:
            return 1;
          case TypeCode.Decimal:
            return 0;
          case TypeCode.String:
            return 13;
          default:
            return 100;
        }
      case TypeCode.DateTime:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return !(formal == typeRefs.Object) ? 100 : 1;
          case TypeCode.Int32:
            return 9;
          case TypeCode.UInt32:
            return 8;
          case TypeCode.Int64:
            return 7;
          case TypeCode.UInt64:
            return 6;
          case TypeCode.Double:
            return 4;
          case TypeCode.Decimal:
            return 5;
          case TypeCode.DateTime:
            return 0;
          case TypeCode.String:
            return 3;
          default:
            return 100;
        }
      case TypeCode.String:
        switch (typeCode2)
        {
          case TypeCode.Object:
            return !(formal == typeRefs.Object) ? 100 : 1;
          case TypeCode.Char:
            return 2;
          case TypeCode.String:
            return 0;
          default:
            return 100;
        }
      default:
        return 0;
    }
  }

  private static int TypeDistance(
    TypeReferences typeRefs,
    Type formal,
    Type actual,
    int distFromObject)
  {
    if (formal == typeRefs.Object)
      return distFromObject;
    return formal.IsEnum ? JSBinder.TypeDistance(typeRefs, Enum.GetUnderlyingType(formal), actual) + 10 : 100;
  }
}
