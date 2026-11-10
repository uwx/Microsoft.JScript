// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.CustomAttribute
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System.CodeDom.Compiler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class CustomAttribute : AST
{
  private AST ctor;
  private ASTList args;
  private AST target;
  internal object type;
  private ArrayList positionalArgValues;
  private ArrayList namedArgFields;
  private ArrayList namedArgFieldValues;
  private ArrayList namedArgProperties;
  private ArrayList namedArgPropertyValues;
  internal bool raiseToPropertyLevel;

  internal CustomAttribute(Context context, AST func, ASTList args)
    : base(context)
  {
    this.ctor = func;
    this.args = args;
    this.target = (AST) null;
    this.type = (object) null;
    this.positionalArgValues = new ArrayList();
    this.namedArgFields = new ArrayList();
    this.namedArgFieldValues = new ArrayList();
    this.namedArgProperties = new ArrayList();
    this.namedArgPropertyValues = new ArrayList();
    this.raiseToPropertyLevel = false;
  }

  private bool CheckIfTargetOK(object caType)
  {
    if (caType == null)
      return false;
    Type target1 = caType as Type;
    AttributeTargets attributeTargets = !(target1 != (Type) null) ? ((ClassScope) caType).owner.validOn : ((AttributeUsageAttribute) CustomAttribute.GetCustomAttributes((MemberInfo) target1, typeof (AttributeUsageAttribute), true)[0]).ValidOn;
    object target2 = (object) this.target;
    switch (target2)
    {
      case Class @class:
        if (@class.isInterface)
        {
          if ((attributeTargets & AttributeTargets.Interface) != (AttributeTargets) 0)
            return true;
        }
        else if (@class is EnumDeclaration)
        {
          if ((attributeTargets & AttributeTargets.Enum) != (AttributeTargets) 0)
            return true;
        }
        else
        {
          if ((attributeTargets & AttributeTargets.Class) != (AttributeTargets) 0)
          {
            if (target1 == typeof (AttributeUsageAttribute))
            {
              if (this.positionalArgValues.Count > 0 && this.positionalArgValues[0] is AttributeTargets positionalArgValue)
                @class.validOn = positionalArgValue;
              int index = 0;
              for (int count = this.namedArgProperties.Count; index < count; ++index)
              {
                if ((this.namedArgProperties[index] as PropertyInfo).Name == "AllowMultiple")
                  @class.allowMultiple = (bool) this.namedArgPropertyValues[index];
              }
            }
            return true;
          }
          if (target1.FullName == "System.NonSerializedAttribute")
          {
            @class.attributes &= ~TypeAttributes.Serializable;
            return false;
          }
        }
        this.context.HandleError(JSError.InvalidCustomAttributeTarget, CustomAttribute.GetTypeName(caType));
        return false;
      case FunctionDeclaration functionDeclaration:
        if ((attributeTargets & AttributeTargets.Property) != (AttributeTargets) 0 && (PropertyInfo) functionDeclaration.enclosingProperty != (PropertyInfo) null)
        {
          if ((MethodInfo) functionDeclaration.enclosingProperty.getter == (MethodInfo) null || ((JSFieldMethod) functionDeclaration.enclosingProperty.getter).func == functionDeclaration.func)
          {
            this.raiseToPropertyLevel = true;
            return true;
          }
          this.context.HandleError(JSError.PropertyLevelAttributesMustBeOnGetter);
          return false;
        }
        if ((attributeTargets & AttributeTargets.Method) != (AttributeTargets) 0 && functionDeclaration.isMethod || (attributeTargets & AttributeTargets.Constructor) != (AttributeTargets) 0 && functionDeclaration.func.isConstructor)
          return true;
        this.context.HandleError(JSError.InvalidCustomAttributeTarget, CustomAttribute.GetTypeName(caType));
        return false;
      case VariableDeclaration _:
      case Constant _:
        if ((attributeTargets & AttributeTargets.Field) != (AttributeTargets) 0)
          return true;
        this.context.HandleError(JSError.InvalidCustomAttributeTarget, CustomAttribute.GetTypeName(caType));
        return false;
      default:
        if (target2 is AssemblyCustomAttributeList && (attributeTargets & AttributeTargets.Assembly) != (AttributeTargets) 0 || target2 == null && (attributeTargets & AttributeTargets.Parameter) != (AttributeTargets) 0)
          return true;
        this.context.HandleError(JSError.InvalidCustomAttributeTarget, CustomAttribute.GetTypeName(caType));
        return false;
    }
  }

  private static ushort DaysSince2000() => (ushort) (DateTime.Now - new DateTime(2000, 1, 1)).Days;

  internal override object Evaluate()
  {
    ConstructorInfo member = (ConstructorInfo) ((Binding) this.ctor).member;
    ParameterInfo[] parameters1 = member.GetParameters();
    int length = parameters1.Length;
    for (int count = this.positionalArgValues.Count; count < length; ++count)
      this.positionalArgValues.Add(Convert.CoerceT((object) null, parameters1[count].ParameterType));
    object[] parameters2 = new object[length];
    this.positionalArgValues.CopyTo(0, (Array) parameters2, 0, length);
    object obj = member.Invoke(BindingFlags.ExactBinding, (Binder) null, parameters2, (CultureInfo) null);
    int index1 = 0;
    for (int count = this.namedArgProperties.Count; index1 < count; ++index1)
    {
      JSProperty namedArgProperty = this.namedArgProperties[index1] as JSProperty;
      if ((PropertyInfo) namedArgProperty != (PropertyInfo) null)
        namedArgProperty.SetValue(obj, Convert.Coerce(this.namedArgPropertyValues[index1], (object) namedArgProperty.PropertyIR()), (object[]) null);
      else
        ((PropertyInfo) this.namedArgProperties[index1]).SetValue(obj, this.namedArgPropertyValues[index1], (object[]) null);
    }
    int index2 = 0;
    for (int count = this.namedArgFields.Count; index2 < count; ++index2)
    {
      JSVariableField namedArgField = this.namedArgFields[index2] as JSVariableField;
      if ((FieldInfo) namedArgField != (FieldInfo) null)
        namedArgField.SetValue(obj, Convert.Coerce(this.namedArgFieldValues[index2], (object) namedArgField.GetInferredType((JSField) null)));
      else
        ((FieldInfo) this.namedArgFields[index2]).SetValue(obj, this.namedArgFieldValues[index2]);
    }
    return obj;
  }

  internal CLSComplianceSpec GetCLSComplianceValue()
  {
    return !(bool) this.positionalArgValues[0] ? CLSComplianceSpec.NonCLSCompliant : CLSComplianceSpec.CLSCompliant;
  }

  private void ConvertClassScopesAndEnumWrappers(ArrayList vals)
  {
    int index = 0;
    for (int count = vals.Count; index < count; ++index)
    {
      if (vals[index] is ClassScope val2)
        vals[index] = (object) val2.GetTypeBuilder();
      else if (vals[index] is EnumWrapper val1)
        vals[index] = val1.ToNumericValue();
    }
  }

  private void ConvertFieldAndPropertyInfos(ArrayList vals)
  {
    int index = 0;
    for (int count = vals.Count; index < count; ++index)
    {
      JSField val1 = vals[index] as JSField;
      if ((FieldInfo) val1 != (FieldInfo) null)
      {
        vals[index] = val1.GetMetaData();
      }
      else
      {
        JSProperty val2 = vals[index] as JSProperty;
        if ((PropertyInfo) val2 != (PropertyInfo) null)
          vals[index] = (object) val2.metaData;
      }
    }
  }

  internal CustomAttributeBuilder GetCustomAttribute()
  {
    ConstructorInfo con = (ConstructorInfo) ((Binding) this.ctor).member;
    ParameterInfo[] parameters = con.GetParameters();
    int length = parameters.Length;
    if (con is JSConstructor)
      con = ((JSConstructor) con).GetConstructorInfo(this.compilerGlobals);
    this.ConvertClassScopesAndEnumWrappers(this.positionalArgValues);
    this.ConvertClassScopesAndEnumWrappers(this.namedArgPropertyValues);
    this.ConvertClassScopesAndEnumWrappers(this.namedArgFieldValues);
    this.ConvertFieldAndPropertyInfos(this.namedArgProperties);
    this.ConvertFieldAndPropertyInfos(this.namedArgFields);
    for (int count = this.positionalArgValues.Count; count < length; ++count)
      this.positionalArgValues.Add(Convert.CoerceT((object) null, parameters[count].ParameterType));
    object[] constructorArgs = new object[length];
    this.positionalArgValues.CopyTo(0, (Array) constructorArgs, 0, length);
    PropertyInfo[] namedProperties = new PropertyInfo[this.namedArgProperties.Count];
    this.namedArgProperties.CopyTo((Array) namedProperties);
    object[] propertyValues = new object[this.namedArgPropertyValues.Count];
    this.namedArgPropertyValues.CopyTo((Array) propertyValues);
    FieldInfo[] namedFields = new FieldInfo[this.namedArgFields.Count];
    this.namedArgFields.CopyTo((Array) namedFields);
    object[] fieldValues = new object[this.namedArgFieldValues.Count];
    this.namedArgFieldValues.CopyTo((Array) fieldValues);
    return new CustomAttributeBuilder(con, constructorArgs, namedProperties, propertyValues, namedFields, fieldValues);
  }

  internal object GetTypeIfAttributeHasToBeUnique()
  {
    Type type = this.type as Type;
    if (type != (Type) null)
    {
      object[] customAttributes = CustomAttribute.GetCustomAttributes((MemberInfo) type, typeof (AttributeUsageAttribute), false);
      return customAttributes.Length != 0 && !((AttributeUsageAttribute) customAttributes[0]).AllowMultiple ? (object) type : (object) null;
    }
    return !((ClassScope) this.type).owner.allowMultiple ? this.type : (object) null;
  }

  private static string GetTypeName(object t)
  {
    Type type = t as Type;
    return type != (Type) null ? type.FullName : ((ClassScope) t).GetFullName();
  }

  internal bool IsExpandoAttribute() => this.ctor is Lookup ctor && ctor.Name == "expando";

  internal override AST PartiallyEvaluate()
  {
    this.ctor = this.ctor.PartiallyEvaluateAsCallable();
    ASTList args = new ASTList(this.args.context);
    ASTList astList = new ASTList(this.args.context);
    int i1 = 0;
    for (int count = this.args.count; i1 < count; ++i1)
    {
      AST ast = this.args[i1];
      if (ast is Assign elem)
      {
        elem.rhside = elem.rhside.PartiallyEvaluate();
        astList.Append((AST) elem);
      }
      else
        args.Append(ast.PartiallyEvaluate());
    }
    int count1 = args.count;
    IReflect[] argIRs = new IReflect[count1];
    for (int i2 = 0; i2 < count1; ++i2)
    {
      AST ast = args[i2];
      switch (ast)
      {
        case ConstantWrapper _:
          object obj = ast.Evaluate();
          if ((argIRs[i2] = CustomAttribute.TypeOfArgument(obj)) != null)
          {
            this.positionalArgValues.Add(obj);
            break;
          }
          goto default;
        case ArrayLiteral _:
          if (((ArrayLiteral) ast).IsOkToUseInCustomAttribute())
          {
            argIRs[i2] = (IReflect) Typeob.ArrayObject;
            this.positionalArgValues.Add(ast.Evaluate());
            break;
          }
          goto default;
        default:
          ast.context.HandleError(JSError.InvalidCustomAttributeArgument);
          return (AST) null;
      }
    }
    this.type = this.ctor.ResolveCustomAttribute(args, argIRs, this.target);
    if (this.type == null)
      return (AST) null;
    if (Convert.IsPromotableTo((IReflect) this.type, (IReflect) Typeob.CodeAccessSecurityAttribute))
    {
      this.context.HandleError(JSError.CannotUseStaticSecurityAttribute);
      return (AST) null;
    }
    ParameterInfo[] parameters1 = ((MethodBase) ((Binding) this.ctor).member).GetParameters();
    int index = 0;
    int count2 = this.positionalArgValues.Count;
    foreach (ParameterInfo parameter in parameters1)
    {
      IReflect type = parameter is ParameterDeclaration ? ((ParameterDeclaration) parameter).ParameterIReflect : (IReflect) parameter.ParameterType;
      if (index < count2)
      {
        object positionalArgValue = this.positionalArgValues[index];
        this.positionalArgValues[index] = Convert.Coerce(positionalArgValue, (object) type, positionalArgValue is ArrayObject);
        ++index;
      }
      else
        this.positionalArgValues.Add(TypeReferences.GetDefaultParameterValue(parameter) != System.Convert.DBNull ? TypeReferences.GetDefaultParameterValue(parameter) : Convert.Coerce((object) null, (object) type));
    }
    int i3 = 0;
    for (int count3 = astList.count; i3 < count3; ++i3)
    {
      Assign assign = (Assign) astList[i3];
      if (assign.lhside is Lookup && (assign.rhside is ConstantWrapper || assign.rhside is ArrayLiteral && ((ArrayLiteral) assign.rhside).IsOkToUseInCustomAttribute()))
      {
        object obj1 = assign.rhside.Evaluate();
        IReflect reflect;
        if (obj1 is ArrayObject || (reflect = CustomAttribute.TypeOfArgument(obj1)) != null && (object) reflect != (object) Typeob.Object)
        {
          MemberInfo[] member = ((IReflect) this.type).GetMember(((Lookup) assign.lhside).Name, BindingFlags.Instance | BindingFlags.Public);
          if (member == null || member.Length == 0)
          {
            assign.context.HandleError(JSError.NoSuchMember);
            return (AST) null;
          }
          if (member.Length == 1)
          {
            MemberInfo prop = member[0];
            if ((object) (prop as FieldInfo) != null)
            {
              FieldInfo fieldInfo = (FieldInfo) prop;
              if (!fieldInfo.IsLiteral)
              {
                if (!fieldInfo.IsInitOnly)
                {
                  try
                  {
                    IReflect type = fieldInfo is JSVariableField ? ((JSVariableField) fieldInfo).GetInferredType((JSField) null) : (IReflect) fieldInfo.FieldType;
                    object obj2 = Convert.Coerce(obj1, (object) type, obj1 is ArrayObject);
                    this.namedArgFields.Add((object) prop);
                    this.namedArgFieldValues.Add(obj2);
                    continue;
                  }
                  catch (JScriptException ex)
                  {
                    assign.rhside.context.HandleError(JSError.TypeMismatch);
                    return (AST) null;
                  }
                }
              }
            }
            else if ((object) (prop as PropertyInfo) != null)
            {
              MethodInfo setMethod = JSProperty.GetSetMethod((PropertyInfo) prop, false);
              if (setMethod != (MethodInfo) null)
              {
                ParameterInfo[] parameters2 = setMethod.GetParameters();
                if (parameters2 != null)
                {
                  if (parameters2.Length == 1)
                  {
                    try
                    {
                      IReflect type = parameters2[0] is ParameterDeclaration ? ((ParameterDeclaration) parameters2[0]).ParameterIReflect : (IReflect) parameters2[0].ParameterType;
                      object obj3 = Convert.Coerce(obj1, (object) type, obj1 is ArrayObject);
                      this.namedArgProperties.Add((object) prop);
                      this.namedArgPropertyValues.Add(obj3);
                      continue;
                    }
                    catch (JScriptException ex)
                    {
                      assign.rhside.context.HandleError(JSError.TypeMismatch);
                      return (AST) null;
                    }
                  }
                }
              }
            }
          }
        }
      }
      assign.context.HandleError(JSError.InvalidCustomAttributeArgument);
      return (AST) null;
    }
    if (!this.CheckIfTargetOK(this.type))
      return (AST) null;
    try
    {
      Type type = this.type as Type;
      if (type != (Type) null)
      {
        if (this.target is AssemblyCustomAttributeList)
        {
          if (type.FullName == "System.Reflection.AssemblyAlgorithmIdAttribute")
          {
            if (this.positionalArgValues.Count > 0)
              this.Engine.Globals.assemblyHashAlgorithm = (System.Configuration.Assemblies.AssemblyHashAlgorithm) Convert.CoerceT(this.positionalArgValues[0], typeof (System.Configuration.Assemblies.AssemblyHashAlgorithm));
            return (AST) null;
          }
          if (type.FullName == "System.Reflection.AssemblyCultureAttribute")
          {
            if (this.positionalArgValues.Count > 0)
            {
              string name = Convert.ToString(this.positionalArgValues[0]);
              if (this.Engine.PEFileKind != PEFileKinds.Dll && name.Length > 0)
              {
                this.context.HandleError(JSError.ExecutablesCannotBeLocalized);
                return (AST) null;
              }
              this.Engine.Globals.assemblyCulture = new CultureInfo(name);
            }
            return (AST) null;
          }
          if (type.FullName == "System.Reflection.AssemblyDelaySignAttribute")
          {
            if (this.positionalArgValues.Count > 0)
              this.Engine.Globals.assemblyDelaySign = Convert.ToBoolean(this.positionalArgValues[0], false);
            return (AST) null;
          }
          if (type.FullName == "System.Reflection.AssemblyFlagsAttribute")
          {
            if (this.positionalArgValues.Count > 0)
              this.Engine.Globals.assemblyFlags = (AssemblyFlags) (uint) Convert.CoerceT(this.positionalArgValues[0], typeof (uint));
            return (AST) null;
          }
          if (type.FullName == "System.Reflection.AssemblyKeyFileAttribute")
          {
            if (this.positionalArgValues.Count > 0)
            {
              this.Engine.Globals.assemblyKeyFileName = Convert.ToString(this.positionalArgValues[0]);
              this.Engine.Globals.assemblyKeyFileNameContext = this.context;
              if (this.Engine.Globals.assemblyKeyFileName != null && this.Engine.Globals.assemblyKeyFileName.Length == 0)
              {
                this.Engine.Globals.assemblyKeyFileName = (string) null;
                this.Engine.Globals.assemblyKeyFileNameContext = (Context) null;
              }
            }
            return (AST) null;
          }
          if (type.FullName == "System.Reflection.AssemblyKeyNameAttribute")
          {
            if (this.positionalArgValues.Count > 0)
            {
              this.Engine.Globals.assemblyKeyName = Convert.ToString(this.positionalArgValues[0]);
              this.Engine.Globals.assemblyKeyNameContext = this.context;
              if (this.Engine.Globals.assemblyKeyName != null && this.Engine.Globals.assemblyKeyName.Length == 0)
              {
                this.Engine.Globals.assemblyKeyName = (string) null;
                this.Engine.Globals.assemblyKeyNameContext = (Context) null;
              }
            }
            return (AST) null;
          }
          if (type.FullName == "System.Reflection.AssemblyVersionAttribute")
          {
            if (this.positionalArgValues.Count > 0)
              this.Engine.Globals.assemblyVersion = this.ParseVersion(Convert.ToString(this.positionalArgValues[0]));
            return (AST) null;
          }
          if (type.FullName == "System.CLSCompliantAttribute")
          {
            this.Engine.isCLSCompliant = this.args == null || this.args.count == 0 || Convert.ToBoolean(this.positionalArgValues[0], false);
            return (AST) this;
          }
        }
      }
    }
    catch (ArgumentException ex)
    {
      this.context.HandleError(JSError.InvalidCall);
    }
    return (AST) this;
  }

  private Version ParseVersion(string vString)
  {
    ushort major = 1;
    ushort minor = 0;
    ushort build = 0;
    ushort revision = 0;
    try
    {
      int length1 = vString.Length;
      int length2 = vString.IndexOf('.', 0);
      if (length2 < 0)
        throw new Exception();
      major = ushort.Parse(vString.Substring(0, length2), (IFormatProvider) CultureInfo.InvariantCulture);
      int num1 = vString.IndexOf('.', length2 + 1);
      if (num1 < length2 + 1)
      {
        minor = ushort.Parse(vString.Substring(length2 + 1, length1 - length2 - 1), (IFormatProvider) CultureInfo.InvariantCulture);
      }
      else
      {
        minor = ushort.Parse(vString.Substring(length2 + 1, num1 - length2 - 1), (IFormatProvider) CultureInfo.InvariantCulture);
        if (vString[num1 + 1] == '*')
        {
          build = CustomAttribute.DaysSince2000();
          revision = CustomAttribute.SecondsSinceMidnight();
        }
        else
        {
          int num2 = vString.IndexOf('.', num1 + 1);
          if (num2 < num1 + 1)
          {
            build = ushort.Parse(vString.Substring(num1 + 1, length1 - num1 - 1), (IFormatProvider) CultureInfo.InvariantCulture);
          }
          else
          {
            build = ushort.Parse(vString.Substring(num1 + 1, num2 - num1 - 1), (IFormatProvider) CultureInfo.InvariantCulture);
            revision = vString[num2 + 1] != '*' ? ushort.Parse(vString.Substring(num2 + 1, length1 - num2 - 1), (IFormatProvider) CultureInfo.InvariantCulture) : CustomAttribute.SecondsSinceMidnight();
          }
        }
      }
    }
    catch
    {
      this.args[0].context.HandleError(JSError.NotValidVersionString);
    }
    return new Version((int) major, (int) minor, (int) build, (int) revision);
  }

  private static ushort SecondsSinceMidnight()
  {
    TimeSpan timeSpan = DateTime.Now - DateTime.Today;
    return (ushort) ((timeSpan.Hours * 60 * 60 + timeSpan.Minutes * 60 + timeSpan.Seconds) / 2);
  }

  internal void SetTarget(AST target) => this.target = target;

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
  }

  internal static IReflect TypeOfArgument(object argument)
  {
    if (argument is Enum)
      return (IReflect) argument.GetType();
    if (argument is EnumWrapper)
      return ((EnumWrapper) argument).classScopeOrType;
    switch (Convert.GetTypeCode(argument))
    {
      case TypeCode.Empty:
      case TypeCode.DBNull:
        return (IReflect) Typeob.Object;
      case TypeCode.Object:
        if ((object) (argument as Type) != null)
          return (IReflect) Typeob.Type;
        if (argument is ClassScope)
          return (IReflect) Typeob.Type;
        break;
      case TypeCode.Boolean:
        return (IReflect) Typeob.Boolean;
      case TypeCode.Char:
        return (IReflect) Typeob.Char;
      case TypeCode.SByte:
        return (IReflect) Typeob.SByte;
      case TypeCode.Byte:
        return (IReflect) Typeob.Byte;
      case TypeCode.Int16:
        return (IReflect) Typeob.Int16;
      case TypeCode.UInt16:
        return (IReflect) Typeob.UInt16;
      case TypeCode.Int32:
        return (IReflect) Typeob.Int32;
      case TypeCode.UInt32:
        return (IReflect) Typeob.UInt32;
      case TypeCode.Int64:
        return (IReflect) Typeob.Int64;
      case TypeCode.UInt64:
        return (IReflect) Typeob.UInt64;
      case TypeCode.Single:
        return (IReflect) Typeob.Single;
      case TypeCode.Double:
        return (IReflect) Typeob.Double;
      case TypeCode.String:
        return (IReflect) Typeob.String;
    }
    return (IReflect) null;
  }

  private static object GetCustomAttributeValue(CustomAttributeTypedArgument arg)
  {
    Type argumentType = arg.ArgumentType;
    return argumentType.IsEnum ? Enum.ToObject(Type.GetType(argumentType.FullName), arg.Value) : arg.Value;
  }

  internal static object[] GetCustomAttributes(Assembly target, Type caType, bool inherit)
  {
    return !target.ReflectionOnly ? target.GetCustomAttributes(caType, inherit) : CustomAttribute.ExtractCustomAttribute(CustomAttributeData.GetCustomAttributes(target), caType);
  }

  internal static object[] GetCustomAttributes(Module target, Type caType, bool inherit)
  {
    return !target.Assembly.ReflectionOnly ? target.GetCustomAttributes(caType, inherit) : CustomAttribute.ExtractCustomAttribute(CustomAttributeData.GetCustomAttributes(target), caType);
  }

  internal static object[] GetCustomAttributes(MemberInfo target, Type caType, bool inherit)
  {
    return target.GetType().Assembly == typeof (CustomAttribute).Assembly || !target.Module.Assembly.ReflectionOnly ? target.GetCustomAttributes(caType, inherit) : CustomAttribute.ExtractCustomAttribute(CustomAttributeData.GetCustomAttributes(target), caType);
  }

  internal static object[] GetCustomAttributes(ParameterInfo target, Type caType, bool inherit)
  {
    return target.GetType().Assembly == typeof (CustomAttribute).Assembly || !target.Member.Module.Assembly.ReflectionOnly ? target.GetCustomAttributes(caType, inherit) : CustomAttribute.ExtractCustomAttribute(CustomAttributeData.GetCustomAttributes(target), caType);
  }

  private static object[] ExtractCustomAttribute(IList<CustomAttributeData> attributes, Type caType)
  {
    Type referenceContext = Globals.TypeRefs.ToReferenceContext(caType);
    foreach (CustomAttributeData attribute in (IEnumerable<CustomAttributeData>) attributes)
    {
      if (attribute.Constructor.DeclaringType == referenceContext)
      {
        ArrayList arrayList = new ArrayList();
        foreach (CustomAttributeTypedArgument constructorArgument in (IEnumerable<CustomAttributeTypedArgument>) attribute.ConstructorArguments)
          arrayList.Add(CustomAttribute.GetCustomAttributeValue(constructorArgument));
        object instance = Activator.CreateInstance(caType, arrayList.ToArray());
        foreach (CustomAttributeNamedArgument namedArgument in (IEnumerable<CustomAttributeNamedArgument>) attribute.NamedArguments)
          caType.InvokeMember(namedArgument.MemberInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField | BindingFlags.SetProperty, (Binder) null, instance, new object[1]
          {
            CustomAttribute.GetCustomAttributeValue(namedArgument.TypedValue)
          }, (ParameterModifier[]) null, CultureInfo.InvariantCulture, (string[]) null);
        return new object[1]{ instance };
      }
    }
    return new object[0];
  }

  internal static bool IsDefined(MemberInfo target, Type caType, bool inherit)
  {
    return target.GetType().Assembly == typeof (CustomAttribute).Assembly || !target.Module.Assembly.ReflectionOnly ? target.IsDefined(caType, inherit) : CustomAttribute.CheckForCustomAttribute(CustomAttributeData.GetCustomAttributes(target), caType);
  }

  internal static bool IsDefined(ParameterInfo target, Type caType, bool inherit)
  {
    return target.GetType().Assembly == typeof (CustomAttribute).Assembly || !target.Member.Module.Assembly.ReflectionOnly ? target.IsDefined(caType, inherit) : CustomAttribute.CheckForCustomAttribute(CustomAttributeData.GetCustomAttributes(target), caType);
  }

  private static bool CheckForCustomAttribute(IList<CustomAttributeData> attributes, Type caType)
  {
    Type referenceContext = Globals.TypeRefs.ToReferenceContext(caType);
    foreach (CustomAttributeData attribute in (IEnumerable<CustomAttributeData>) attributes)
    {
      if (attribute.Constructor.DeclaringType == referenceContext)
        return true;
    }
    return false;
  }
}
