// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.Class
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using Microsoft.JScript.Vsa;
using System.CodeDom.Compiler;
using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

#nullable disable
namespace Microsoft.JScript;

internal class Class : AST
{
  internal string name;
  private TypeExpression superTypeExpression;
  private TypeExpression[] interfaces;
  internal Block body;
  internal ScriptObject enclosingScope;
  internal TypeAttributes attributes;
  private bool hasAlreadyBeenAskedAboutExpando;
  internal bool isAbstract;
  private bool isAlreadyPartiallyEvaluated;
  private bool isCooked;
  private Type cookedType;
  private bool isExpando;
  internal bool isInterface;
  internal bool isStatic;
  protected bool needsEngine;
  internal AttributeTargets validOn;
  internal bool allowMultiple;
  protected ClassScope classob;
  private FunctionObject implicitDefaultConstructor;
  private JSVariableField ownField;
  protected JSMemberField[] fields;
  private Class superClass;
  private IReflect superIR;
  private object[] superMembers;
  private SimpleHashtable firstIndex;
  private MethodInfo fieldInitializer;
  internal CustomAttributeList customAttributes;
  internal CLSComplianceSpec clsCompliance;
  private bool generateCodeForExpando;
  private PropertyBuilder expandoItemProp;
  private MethodBuilder getHashTableMethod;
  private MethodBuilder getItem;
  private MethodBuilder setItem;
  internal MethodBuilder deleteOpMethod;
  private static int badTypeNameCount;

  internal Class(
    Context context,
    AST id,
    TypeExpression superTypeExpression,
    TypeExpression[] interfaces,
    Block body,
    FieldAttributes attributes,
    bool isAbstract,
    bool isFinal,
    bool isStatic,
    bool isInterface,
    CustomAttributeList customAttributes)
    : base(context)
  {
    this.name = id.ToString();
    this.superTypeExpression = superTypeExpression;
    this.interfaces = interfaces;
    this.body = body;
    this.enclosingScope = (ScriptObject) this.Globals.ScopeStack.Peek(1);
    this.attributes = TypeAttributes.Serializable;
    this.SetAccessibility(attributes);
    if (isAbstract)
      this.attributes |= TypeAttributes.Abstract;
    this.isAbstract = isAbstract | isInterface;
    this.isAlreadyPartiallyEvaluated = false;
    if (isFinal)
      this.attributes |= TypeAttributes.Sealed;
    if (isInterface)
      this.attributes |= TypeAttributes.ClassSemanticsMask | TypeAttributes.Abstract;
    this.isCooked = false;
    this.cookedType = (Type) null;
    this.isExpando = false;
    this.isInterface = isInterface;
    this.isStatic = isStatic;
    this.needsEngine = !isInterface;
    this.validOn = (AttributeTargets) 0;
    this.allowMultiple = true;
    this.classob = (ClassScope) this.Globals.ScopeStack.Peek();
    this.classob.name = this.name;
    this.classob.owner = this;
    this.implicitDefaultConstructor = (FunctionObject) null;
    if (!isInterface && !(this is EnumDeclaration))
      this.SetupConstructors();
    this.EnterNameIntoEnclosingScopeAndGetOwnField(id, isStatic);
    this.fields = this.classob.GetMemberFields();
    this.superClass = (Class) null;
    this.superIR = (IReflect) null;
    this.superMembers = (object[]) null;
    this.firstIndex = (SimpleHashtable) null;
    this.fieldInitializer = (MethodInfo) null;
    this.customAttributes = customAttributes;
    this.clsCompliance = CLSComplianceSpec.NotAttributed;
    this.generateCodeForExpando = false;
    this.expandoItemProp = (PropertyBuilder) null;
    this.getHashTableMethod = (MethodBuilder) null;
    this.getItem = (MethodBuilder) null;
    this.setItem = (MethodBuilder) null;
  }

  private void AddImplicitInterfaces(
    IReflect iface,
    IReflect[] explicitInterfaces,
    ArrayList implicitInterfaces)
  {
    Type type1 = iface as Type;
    if (type1 != (Type) null)
    {
      foreach (Type type2 in type1.GetInterfaces())
      {
        if (Array.IndexOf<IReflect>(explicitInterfaces, (IReflect) type2, 0) >= 0 || implicitInterfaces.IndexOf((object) type2, 0) >= 0)
          break;
        implicitInterfaces.Add((object) type2);
      }
    }
    else
    {
      foreach (TypeExpression typeExpression in ((ClassScope) iface).owner.interfaces)
      {
        IReflect ireflect = typeExpression.ToIReflect();
        if (Array.IndexOf<IReflect>(explicitInterfaces, ireflect, 0) >= 0 || implicitInterfaces.IndexOf((object) ireflect, 0) >= 0)
          break;
        implicitInterfaces.Add((object) ireflect);
      }
    }
  }

  private void AllocateImplicitDefaultConstructor()
  {
    this.implicitDefaultConstructor = new FunctionObject(".ctor", new ParameterDeclaration[0], (TypeExpression) null, new Block(this.context), new FunctionScope((ScriptObject) this.classob, true), (ScriptObject) this.classob, this.context, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask, (CustomAttributeList) null, true);
    this.implicitDefaultConstructor.isImplicitCtor = true;
    this.implicitDefaultConstructor.isConstructor = true;
    this.implicitDefaultConstructor.proto = (object) this.classob;
  }

  private bool CanSee(MemberInfo member)
  {
    switch (member.MemberType)
    {
      case MemberTypes.Event:
        MethodBase addMethod = (MethodBase) ((EventInfo) member).GetAddMethod();
        if (addMethod == (MethodBase) null)
          return false;
        switch (addMethod.Attributes & MethodAttributes.MemberAccessMask)
        {
          case MethodAttributes.PrivateScope:
          case MethodAttributes.Private:
          case MethodAttributes.FamANDAssem:
            return false;
          case MethodAttributes.Assembly:
            return this.IsInTheSamePackage(member);
          default:
            return true;
        }
      case MemberTypes.Field:
        switch (((FieldInfo) member).Attributes & FieldAttributes.FieldAccessMask)
        {
          case FieldAttributes.PrivateScope:
          case FieldAttributes.Private:
          case FieldAttributes.FamANDAssem:
            return false;
          case FieldAttributes.Assembly:
            return this.IsInTheSamePackage(member);
          default:
            return true;
        }
      case MemberTypes.Method:
        switch (((MethodBase) member).Attributes & MethodAttributes.MemberAccessMask)
        {
          case MethodAttributes.PrivateScope:
          case MethodAttributes.Private:
          case MethodAttributes.FamANDAssem:
            return false;
          case MethodAttributes.Assembly:
            return this.IsInTheSamePackage(member);
          default:
            return true;
        }
      case MemberTypes.Property:
        MethodBase methodBase = (MethodBase) JSProperty.GetGetMethod((PropertyInfo) member, true);
        if (methodBase == (MethodBase) null)
          methodBase = (MethodBase) JSProperty.GetSetMethod((PropertyInfo) member, true);
        if (methodBase == (MethodBase) null)
          return false;
        switch (methodBase.Attributes & MethodAttributes.MemberAccessMask)
        {
          case MethodAttributes.PrivateScope:
          case MethodAttributes.Private:
          case MethodAttributes.FamANDAssem:
            return false;
          case MethodAttributes.Assembly:
            return this.IsInTheSamePackage(member);
          default:
            return true;
        }
      case MemberTypes.TypeInfo:
      case MemberTypes.NestedType:
        switch (((Type) member).Attributes & TypeAttributes.VisibilityMask)
        {
          case TypeAttributes.NestedPrivate:
          case TypeAttributes.NestedFamANDAssem:
            return false;
          case TypeAttributes.NestedAssembly:
            return this.IsInTheSamePackage(member);
          default:
            return true;
        }
      default:
        return true;
    }
  }

  private void CheckFieldDeclarationConsistency(JSMemberField field)
  {
    object obj = this.firstIndex[(object) field.Name];
    if (obj == null)
      return;
    int index = (int) obj;
    for (int length = this.superMembers.Length; index < length; ++index)
    {
      object superMember = this.superMembers[index];
      if ((object) (superMember as MemberInfo) == null)
        break;
      MemberInfo memberInfo = (MemberInfo) superMember;
      if (!memberInfo.Name.Equals(field.Name))
        break;
      if (this.CanSee(memberInfo))
      {
        string fullNameFor = this.GetFullNameFor(memberInfo);
        field.originalContext.HandleError(JSError.HidesParentMember, fullNameFor, this.IsInTheSameCompilationUnit(memberInfo));
        break;
      }
    }
  }

  private void CheckIfOKToGenerateCodeForExpando(bool superClassIsExpando)
  {
    if (superClassIsExpando)
    {
      this.context.HandleError(JSError.BaseClassIsExpandoAlready);
      this.generateCodeForExpando = false;
    }
    else if (this.classob.GetMember("Item", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length != 0)
    {
      this.context.HandleError(JSError.ItemNotAllowedOnExpandoClass);
      this.generateCodeForExpando = false;
    }
    else if (this.classob.GetMember("get_Item", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length != 0 || this.classob.GetMember("set_Item", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length != 0)
    {
      this.context.HandleError(JSError.MethodNotAllowedOnExpandoClass);
      this.generateCodeForExpando = false;
    }
    else if (this.ImplementsInterface((IReflect) Typeob.IEnumerable))
    {
      this.context.HandleError(JSError.ExpandoClassShouldNotImpleEnumerable);
      this.generateCodeForExpando = false;
    }
    else if (this.superIR.GetMember("Item", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length != 0 || this.superIR.GetMember("get_Item", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length != 0 || this.superIR.GetMember("set_Item", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length != 0)
    {
      this.context.HandleError(JSError.MethodClashOnExpandoSuperClass);
      this.generateCodeForExpando = false;
    }
    else
    {
      JSProperty jsProperty = this.classob.itemProp = new JSProperty("Item");
      jsProperty.getter = (JSMethod) new JSExpandoIndexerMethod(this.classob, true);
      jsProperty.setter = (JSMethod) new JSExpandoIndexerMethod(this.classob, false);
      this.classob.AddNewField("Item", (object) jsProperty, FieldAttributes.Literal);
    }
  }

  private string GetFullName()
  {
    string str = ((ActivationObject) this.enclosingScope).GetName();
    if (str == null)
    {
      VsaEngine engine = this.context.document.engine;
      if (engine != null && engine.genStartupClass)
        str = engine.RootNamespace;
    }
    return str != null ? $"{str}.{this.name}" : this.name;
  }

  protected void CheckMemberNamesForCLSCompliance()
  {
    if (!(this.enclosingScope is ClassScope))
      this.Engine.CheckTypeNameForCLSCompliance(this.name, this.GetFullName(), this.context);
    Hashtable hashtable = new Hashtable((IEqualityComparer) StringComparer.OrdinalIgnoreCase);
    int index = 0;
    for (int length = this.fields.Length; index < length; ++index)
    {
      JSMemberField field = this.fields[index];
      if (!field.IsPrivate)
      {
        if (!VsaEngine.CheckIdentifierForCLSCompliance(field.Name))
          field.originalContext.HandleError(JSError.NonCLSCompliantMember);
        else if ((FieldInfo) hashtable[(object) field.Name] == (FieldInfo) null)
          hashtable.Add((object) field.Name, (object) field);
        else
          field.originalContext.HandleError(JSError.NonCLSCompliantMember);
      }
    }
  }

  private void CheckIfValidExtensionOfSuperType()
  {
    this.GetIRForSuperType();
    if (this.superIR is ClassScope superIr)
    {
      if (this.IsStatic)
      {
        if (!superIr.owner.IsStatic)
        {
          this.superTypeExpression.context.HandleError(JSError.NestedInstanceTypeCannotBeExtendedByStatic);
          this.superIR = (IReflect) Typeob.Object;
          this.superTypeExpression = (TypeExpression) null;
        }
      }
      else if (!superIr.owner.IsStatic && this.enclosingScope != superIr.owner.enclosingScope)
      {
        this.superTypeExpression.context.HandleError(JSError.NestedInstanceTypeCannotBeExtendedByStatic);
        this.superIR = (IReflect) Typeob.Object;
        this.superTypeExpression = (TypeExpression) null;
      }
    }
    this.GetSuperTypeMembers();
    this.GetStartIndexForEachName();
    bool forClsCompliance = this.NeedsToBeCheckedForCLSCompliance();
    if (forClsCompliance)
      this.CheckMemberNamesForCLSCompliance();
    int index1 = 0;
    for (int length = this.fields.Length; index1 < length; ++index1)
    {
      JSMemberField jsMemberField = this.fields[index1];
      if (jsMemberField.IsLiteral)
      {
        object obj = jsMemberField.value;
        if (obj is FunctionObject)
        {
          while (true)
          {
            FunctionObject func = (FunctionObject) obj;
            if (func.implementedIface != null)
            {
              this.CheckMethodDeclarationConsistency(func);
              if (func.implementedIfaceMethod == (MethodInfo) null)
                func.funcContext.HandleError(JSError.NoMethodInBaseToOverride);
              if (jsMemberField.IsPublic || jsMemberField.IsFamily || jsMemberField.IsFamilyOrAssembly)
                func.CheckCLSCompliance(forClsCompliance);
              jsMemberField = jsMemberField.nextOverload;
              if (!((FieldInfo) jsMemberField == (FieldInfo) null))
                obj = jsMemberField.value;
              else
                goto label_21;
            }
            else
              goto label_21;
          }
        }
        else
        {
          JSProperty jsProperty = obj as JSProperty;
          continue;
        }
      }
      else
        continue;
label_21:;
    }
    int index2 = 0;
    for (int length = this.fields.Length; index2 < length; ++index2)
    {
      JSMemberField field = this.fields[index2];
      if (field.IsLiteral)
      {
        object obj = field.value;
        switch (obj)
        {
          case FunctionObject _:
            while (true)
            {
              FunctionObject func = (FunctionObject) obj;
              if (func.implementedIface == null)
              {
                this.CheckMethodDeclarationConsistency(func);
                if (field.IsPublic || field.IsFamily || field.IsFamilyOrAssembly)
                  func.CheckCLSCompliance(forClsCompliance);
                field = field.nextOverload;
                if (!((FieldInfo) field == (FieldInfo) null))
                  obj = field.value;
                else
                  goto label_34;
              }
              else
                goto label_34;
            }
          case JSProperty _:
            continue;
        }
      }
      this.CheckFieldDeclarationConsistency(field);
      if (field.IsPublic || field.IsFamily || field.IsFamilyOrAssembly)
      {
        field.CheckCLSCompliance(forClsCompliance);
        continue;
      }
      continue;
label_34:;
    }
  }

  private void CheckMethodDeclarationConsistency(FunctionObject func)
  {
    if (func.isStatic && !func.isExpandoMethod || func.isConstructor)
      return;
    object obj = this.firstIndex[(object) func.name];
    if (obj == null)
    {
      this.CheckThatMethodIsNotMarkedWithOverrideOrHide(func);
      if ((func.attributes & MethodAttributes.Final) == MethodAttributes.PrivateScope)
        return;
      func.attributes &= ~(MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask);
    }
    else
    {
      MemberInfo memberInfo = (MemberInfo) null;
      int i = (int) obj;
      for (int length = this.superMembers.Length; i < length; ++i)
      {
        MemberInfo superMember = this.superMembers[i] as MemberInfo;
        if (!(superMember == (MemberInfo) null))
        {
          if (superMember.Name.Equals(func.name))
          {
            if (this.CanSee(superMember))
            {
              if (superMember.MemberType != MemberTypes.Method)
              {
                memberInfo = superMember;
              }
              else
              {
                if (func.isExpandoMethod)
                {
                  memberInfo = superMember;
                  break;
                }
                MethodInfo matchingMethod = (MethodInfo) superMember;
                if (func.implementedIface != null)
                {
                  if (matchingMethod is JSFieldMethod)
                  {
                    if (((JSFieldMethod) matchingMethod).EnclosingScope() != func.implementedIface)
                      continue;
                  }
                  else if ((object) matchingMethod.DeclaringType != (object) func.implementedIface)
                    continue;
                }
                if (Class.ParametersMatch(matchingMethod.GetParameters(), (ParameterInfo[]) func.parameter_declarations))
                {
                  if (matchingMethod is JSWrappedMethod)
                    matchingMethod = ((JSWrappedMethod) matchingMethod).method;
                  if (!func.noVersionSafeAttributeSpecified && (func.attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.VtableLayoutMask)
                    return;
                  this.CheckMatchingMethodForConsistency(matchingMethod, func, i, length);
                  return;
                }
              }
            }
          }
          else
            break;
        }
      }
      if (memberInfo != (MemberInfo) null)
      {
        if (!func.noVersionSafeAttributeSpecified && ((func.attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.VtableLayoutMask || func.isExpandoMethod))
          return;
        string fullNameFor = this.GetFullNameFor(memberInfo);
        func.funcContext.HandleError(JSError.HidesParentMember, fullNameFor, this.IsInTheSameCompilationUnit(memberInfo));
      }
      else
      {
        this.CheckThatMethodIsNotMarkedWithOverrideOrHide(func);
        if ((func.attributes & MethodAttributes.Final) == MethodAttributes.PrivateScope)
          return;
        func.attributes &= ~(MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask);
      }
    }
  }

  private void CheckMatchingMethodForConsistency(
    MethodInfo matchingMethod,
    FunctionObject func,
    int i,
    int n)
  {
    if (!func.ReturnType((JSField) null).Equals(matchingMethod is JSFieldMethod ? (object) ((JSFieldMethod) matchingMethod).func.ReturnType((JSField) null) : (object) matchingMethod.ReturnType))
      func.funcContext.HandleError(JSError.DifferentReturnTypeFromBase, func.name, true);
    else if (func.implementedIface != null)
    {
      func.implementedIfaceMethod = matchingMethod;
      this.superMembers[i] = (object) func.name;
    }
    else
    {
      MethodAttributes methodAttributes = func.attributes & MethodAttributes.MemberAccessMask;
      if ((matchingMethod.Attributes & MethodAttributes.MemberAccessMask) != methodAttributes && ((matchingMethod.Attributes & MethodAttributes.MemberAccessMask) != MethodAttributes.FamORAssem || methodAttributes != MethodAttributes.Family))
        func.funcContext.HandleError(JSError.CannotChangeVisibility);
      if (func.noVersionSafeAttributeSpecified)
      {
        if (this.Engine.versionSafe)
        {
          if ((matchingMethod.Attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
          {
            func.funcContext.HandleError(JSError.HidesAbstractInBase, $"{this.name}.{func.name}");
            func.attributes &= ~MethodAttributes.VtableLayoutMask;
          }
          else
          {
            func.funcContext.HandleError(JSError.NewNotSpecifiedInMethodDeclaration, this.IsInTheSameCompilationUnit((MemberInfo) matchingMethod));
            i = -1;
          }
        }
        else if ((matchingMethod.Attributes & MethodAttributes.Virtual) == MethodAttributes.PrivateScope || (matchingMethod.Attributes & MethodAttributes.Final) != MethodAttributes.PrivateScope)
        {
          i = -1;
        }
        else
        {
          func.attributes &= ~MethodAttributes.VtableLayoutMask;
          if ((matchingMethod.Attributes & MethodAttributes.Abstract) == MethodAttributes.PrivateScope)
            i = -1;
        }
      }
      else if ((func.attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.PrivateScope)
      {
        if ((matchingMethod.Attributes & MethodAttributes.Virtual) == MethodAttributes.PrivateScope || (matchingMethod.Attributes & MethodAttributes.Final) != MethodAttributes.PrivateScope)
        {
          func.funcContext.HandleError(JSError.MethodInBaseIsNotVirtual);
          i = -1;
        }
        else
        {
          func.attributes &= ~MethodAttributes.VtableLayoutMask;
          if ((matchingMethod.Attributes & MethodAttributes.Abstract) == MethodAttributes.PrivateScope)
            i = -1;
        }
      }
      else if ((matchingMethod.Attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
      {
        func.funcContext.HandleError(JSError.HidesAbstractInBase, $"{this.name}.{func.name}");
        func.attributes &= ~MethodAttributes.VtableLayoutMask;
      }
      else
        i = -1;
      if (i < 0)
        return;
      this.superMembers[i] = (object) func.name;
      for (int index = i + 1; index < n; ++index)
      {
        MemberInfo superMember = this.superMembers[index] as MemberInfo;
        if (!(superMember == (MemberInfo) null))
        {
          if (superMember.Name != matchingMethod.Name)
            break;
          MethodInfo methodInfo = superMember as MethodInfo;
          if (!(methodInfo == (MethodInfo) null) && methodInfo.IsAbstract && Class.ParametersMatch(methodInfo.GetParameters(), matchingMethod.GetParameters()) && (matchingMethod is JSFieldMethod ? ((JSFieldMethod) matchingMethod).ReturnIR() : (IReflect) matchingMethod.ReturnType) == (methodInfo is JSFieldMethod ? ((JSFieldMethod) methodInfo).ReturnIR() : (IReflect) methodInfo.ReturnType))
            this.superMembers[index] = (object) func.name;
        }
      }
    }
  }

  private void CheckThatAllAbstractSuperClassMethodsAreImplemented()
  {
    int index1 = 0;
    for (int length1 = this.superMembers.Length; index1 < length1; ++index1)
    {
      MethodInfo superMember1 = this.superMembers[index1] as MethodInfo;
      if (superMember1 != (MethodInfo) null && superMember1.IsAbstract)
      {
        for (int index2 = index1 - 1; index2 >= 0; --index2)
        {
          object superMember2 = this.superMembers[index2];
          if ((object) (superMember2 as MethodInfo) != null)
          {
            MethodInfo methodInfo = (MethodInfo) superMember2;
            if (!(methodInfo.Name != superMember1.Name))
            {
              if (!methodInfo.IsAbstract && Class.ParametersMatch(methodInfo.GetParameters(), superMember1.GetParameters()) && (superMember1 is JSFieldMethod ? ((JSFieldMethod) superMember1).ReturnIR() : (IReflect) superMember1.ReturnType) == (methodInfo is JSFieldMethod ? ((JSFieldMethod) methodInfo).ReturnIR() : (IReflect) methodInfo.ReturnType))
              {
                this.superMembers[index1] = (object) superMember1.Name;
                goto label_19;
              }
            }
            else
              break;
          }
        }
        if (!this.isAbstract || !this.isInterface && Class.DefinedOnInterface(superMember1))
        {
          StringBuilder stringBuilder = new StringBuilder(superMember1.DeclaringType.ToString());
          stringBuilder.Append('.');
          stringBuilder.Append(superMember1.Name);
          stringBuilder.Append('(');
          ParameterInfo[] parameters = superMember1.GetParameters();
          int index3 = 0;
          for (int length2 = parameters.Length; index3 < length2; ++index3)
          {
            stringBuilder.Append(parameters[index3].ParameterType.FullName);
            if (index3 < length2 - 1)
              stringBuilder.Append(", ");
          }
          stringBuilder.Append(")");
          if (superMember1.ReturnType != Typeob.Void)
          {
            stringBuilder.Append(" : ");
            stringBuilder.Append(superMember1.ReturnType.FullName);
          }
          this.context.HandleError(JSError.MustImplementMethod, stringBuilder.ToString());
          this.attributes |= TypeAttributes.Abstract;
          continue;
        }
        continue;
      }
      continue;
label_19:;
    }
  }

  private void CheckThatMethodIsNotMarkedWithOverrideOrHide(FunctionObject func)
  {
    if (func.noVersionSafeAttributeSpecified)
      return;
    if ((func.attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.PrivateScope)
      func.funcContext.HandleError(JSError.NoMethodInBaseToOverride);
    else
      func.funcContext.HandleError(JSError.NoMethodInBaseToNew);
  }

  private static bool DefinedOnInterface(MethodInfo meth)
  {
    JSFieldMethod jsFieldMethod = meth as JSFieldMethod;
    return (MethodInfo) jsFieldMethod != (MethodInfo) null ? ((ClassScope) jsFieldMethod.func.enclosing_scope).owner.isInterface : meth.DeclaringType.IsInterface;
  }

  private void EmitILForINeedEngineMethods()
  {
    if (!this.needsEngine)
      return;
    TypeBuilder classwriter = (TypeBuilder) this.classob.classwriter;
    FieldBuilder field = classwriter.DefineField("vsa Engine", Typeob.VsaEngine, FieldAttributes.Private | FieldAttributes.NotSerialized);
    MethodBuilder methodInfoBody1 = classwriter.DefineMethod("GetEngine", MethodAttributes.Private | MethodAttributes.Virtual, Typeob.VsaEngine, (Type[]) null);
    ILGenerator ilGenerator1 = methodInfoBody1.GetILGenerator();
    ilGenerator1.Emit(OpCodes.Ldarg_0);
    ilGenerator1.Emit(OpCodes.Ldfld, (FieldInfo) field);
    ilGenerator1.Emit(OpCodes.Ldnull);
    Label label = ilGenerator1.DefineLabel();
    ilGenerator1.Emit(OpCodes.Bne_Un, label);
    ilGenerator1.Emit(OpCodes.Ldarg_0);
    if (this.body.Engine.doCRS)
      ilGenerator1.Emit(OpCodes.Ldsfld, CompilerGlobals.contextEngineField);
    else if (this.context.document.engine.PEFileKind == PEFileKinds.Dll)
    {
      ilGenerator1.Emit(OpCodes.Ldtoken, (Type) classwriter);
      ilGenerator1.Emit(OpCodes.Call, CompilerGlobals.createVsaEngineWithType);
    }
    else
      ilGenerator1.Emit(OpCodes.Call, CompilerGlobals.createVsaEngine);
    ilGenerator1.Emit(OpCodes.Stfld, (FieldInfo) field);
    ilGenerator1.MarkLabel(label);
    ilGenerator1.Emit(OpCodes.Ldarg_0);
    ilGenerator1.Emit(OpCodes.Ldfld, (FieldInfo) field);
    ilGenerator1.Emit(OpCodes.Ret);
    classwriter.DefineMethodOverride((MethodInfo) methodInfoBody1, CompilerGlobals.getEngineMethod);
    MethodBuilder methodInfoBody2 = classwriter.DefineMethod("SetEngine", MethodAttributes.Private | MethodAttributes.Virtual, Typeob.Void, new Type[1]
    {
      Typeob.VsaEngine
    });
    ILGenerator ilGenerator2 = methodInfoBody2.GetILGenerator();
    ilGenerator2.Emit(OpCodes.Ldarg_0);
    ilGenerator2.Emit(OpCodes.Ldarg_1);
    ilGenerator2.Emit(OpCodes.Stfld, (FieldInfo) field);
    ilGenerator2.Emit(OpCodes.Ret);
    classwriter.DefineMethodOverride((MethodInfo) methodInfoBody2, CompilerGlobals.setEngineMethod);
  }

  internal void EmitInitialCalls(
    ILGenerator il,
    MethodBase supcons,
    ParameterInfo[] pars,
    ASTList argAST,
    int callerParameterCount)
  {
    bool flag = true;
    if (supcons != (MethodBase) null)
    {
      il.Emit(OpCodes.Ldarg_0);
      int length = pars.Length;
      int count = argAST == null ? 0 : argAST.count;
      object[] objArray = new object[length];
      for (int i = 0; i < length; ++i)
      {
        AST ast = i < count ? argAST[i] : (AST) new ConstantWrapper((object) null, (Context) null);
        if (pars[i].ParameterType.IsByRef)
        {
          objArray[i] = ast.TranslateToILReference(il, pars[i].ParameterType.GetElementType());
        }
        else
        {
          ast.TranslateToIL(il, pars[i].ParameterType);
          objArray[i] = (object) null;
        }
      }
      if (supcons is JSConstructor)
      {
        JSConstructor jsConstructor = (JSConstructor) supcons;
        flag = jsConstructor.GetClassScope() != this.classob;
        supcons = (MethodBase) jsConstructor.GetConstructorInfo(this.compilerGlobals);
        if (jsConstructor.GetClassScope().outerClassField != (FieldInfo) null)
          Convert.EmitLdarg(il, (short) callerParameterCount);
      }
      il.Emit(OpCodes.Call, (ConstructorInfo) supcons);
      for (int i = 0; i < count; ++i)
      {
        AST ast = argAST[i];
        if (ast is AddressOf && objArray[i] != null)
        {
          Type type = Convert.ToType(ast.InferType((JSField) null));
          ast.TranslateToILPreSet(il);
          il.Emit(OpCodes.Ldloc, (LocalBuilder) objArray[i]);
          Convert.Emit((AST) this, il, pars[i].ParameterType, type);
          ast.TranslateToILSet(il);
        }
      }
    }
    if (this.classob.outerClassField != (FieldInfo) null)
    {
      il.Emit(OpCodes.Ldarg_0);
      Convert.EmitLdarg(il, (short) callerParameterCount);
      il.Emit(OpCodes.Stfld, this.classob.outerClassField);
    }
    if (!flag)
      return;
    il.Emit(OpCodes.Ldarg_0);
    il.Emit(OpCodes.Call, this.fieldInitializer);
    this.body.TranslateToILInitOnlyInitializers(il);
  }

  private void EnterNameIntoEnclosingScopeAndGetOwnField(AST id, bool isStatic)
  {
    if (((IActivationObject) this.enclosingScope).GetLocalField(this.name) != (FieldInfo) null)
    {
      id.context.HandleError(JSError.DuplicateName, true);
      this.name += " class";
    }
    FieldAttributes fieldAttributes = FieldAttributes.Literal;
    FieldAttributes attributeFlags;
    switch (this.attributes & TypeAttributes.VisibilityMask)
    {
      case TypeAttributes.NestedPrivate:
        attributeFlags = fieldAttributes | FieldAttributes.Private;
        break;
      case TypeAttributes.NestedFamily:
        attributeFlags = fieldAttributes | FieldAttributes.Family;
        break;
      case TypeAttributes.NestedAssembly:
        attributeFlags = fieldAttributes | FieldAttributes.Assembly;
        break;
      case TypeAttributes.NestedFamANDAssem:
        attributeFlags = fieldAttributes | FieldAttributes.FamANDAssem;
        break;
      case TypeAttributes.VisibilityMask:
        attributeFlags = fieldAttributes | FieldAttributes.FamORAssem;
        break;
      default:
        attributeFlags = fieldAttributes | FieldAttributes.Public;
        break;
    }
    ScriptObject scriptObject = this.enclosingScope;
    while (true)
    {
      switch (scriptObject)
      {
        case BlockScope _:
          scriptObject = scriptObject.GetParent();
          continue;
        case GlobalScope _:
        case PackageScope _:
        case ClassScope _:
          goto label_15;
        default:
          goto label_12;
      }
    }
label_12:
    isStatic = false;
    if (this is EnumDeclaration)
      this.context.HandleError(JSError.EnumNotAllowed);
    else
      this.context.HandleError(JSError.ClassNotAllowed);
label_15:
    if (isStatic)
      attributeFlags |= FieldAttributes.Static;
    if (this.enclosingScope is ActivationObject)
    {
      if (this.enclosingScope is ClassScope && this.name == ((ClassScope) this.enclosingScope).name)
      {
        this.context.HandleError(JSError.CannotUseNameOfClass);
        this.name += " nested class";
      }
      this.ownField = ((ActivationObject) this.enclosingScope).AddNewField(this.name, (object) this.classob, attributeFlags);
      if (this.ownField is JSLocalField)
        ((JSLocalField) this.ownField).isDefined = true;
    }
    else
      this.ownField = ((StackFrame) this.enclosingScope).AddNewField(this.name, (object) this.classob, attributeFlags);
    this.ownField.originalContext = id.context;
  }

  internal override object Evaluate()
  {
    this.Globals.ScopeStack.GuardedPush((object) this.classob);
    try
    {
      this.body.EvaluateStaticVariableInitializers();
    }
    finally
    {
      this.Globals.ScopeStack.Pop();
    }
    return (object) new Completion();
  }

  private void GenerateGetEnumerator()
  {
    TypeBuilder typeBuilder = this.classob.GetTypeBuilder();
    MethodBuilder methodInfoBody = typeBuilder.DefineMethod("get enumerator", MethodAttributes.Private | MethodAttributes.Virtual, Typeob.IEnumerator, (Type[]) null);
    ILGenerator ilGenerator = methodInfoBody.GetILGenerator();
    ilGenerator.Emit(OpCodes.Ldarg_0);
    ilGenerator.Emit(OpCodes.Call, (MethodInfo) this.getHashTableMethod);
    ilGenerator.Emit(OpCodes.Call, CompilerGlobals.hashTableGetEnumerator);
    ilGenerator.Emit(OpCodes.Ret);
    typeBuilder.DefineMethodOverride((MethodInfo) methodInfoBody, CompilerGlobals.getEnumeratorMethod);
  }

  private void GetExpandoFieldGetter(TypeBuilder classwriter)
  {
    if (!((PropertyInfo) this.expandoItemProp == (PropertyInfo) null))
      return;
    this.expandoItemProp = classwriter.DefineProperty("Item", PropertyAttributes.None, Typeob.Object, new Type[1]
    {
      Typeob.String
    });
    FieldInfo field = (FieldInfo) classwriter.DefineField("expando table", Typeob.SimpleHashtable, FieldAttributes.Private);
    this.getHashTableMethod = classwriter.DefineMethod("get expando table", MethodAttributes.Private, Typeob.SimpleHashtable, (Type[]) null);
    ILGenerator ilGenerator = this.getHashTableMethod.GetILGenerator();
    ilGenerator.Emit(OpCodes.Ldarg_0);
    ilGenerator.Emit(OpCodes.Ldfld, field);
    ilGenerator.Emit(OpCodes.Ldnull);
    Label label = ilGenerator.DefineLabel();
    ilGenerator.Emit(OpCodes.Bne_Un, label);
    ilGenerator.Emit(OpCodes.Ldarg_0);
    ilGenerator.Emit(OpCodes.Ldc_I4_8);
    ilGenerator.Emit(OpCodes.Newobj, CompilerGlobals.hashtableCtor);
    ilGenerator.Emit(OpCodes.Stfld, field);
    ilGenerator.MarkLabel(label);
    ilGenerator.Emit(OpCodes.Ldarg_0);
    ilGenerator.Emit(OpCodes.Ldfld, field);
    ilGenerator.Emit(OpCodes.Ret);
  }

  internal MethodInfo GetExpandoIndexerGetter()
  {
    if ((MethodInfo) this.getItem == (MethodInfo) null)
    {
      TypeBuilder typeBuilder = this.classob.GetTypeBuilder();
      this.GetExpandoFieldGetter(typeBuilder);
      this.getItem = typeBuilder.DefineMethod("get_Item", MethodAttributes.Public | MethodAttributes.SpecialName, Typeob.Object, new Type[1]
      {
        Typeob.String
      });
      ILGenerator ilGenerator = this.getItem.GetILGenerator();
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Call, (MethodInfo) this.getHashTableMethod);
      ilGenerator.Emit(OpCodes.Ldarg_1);
      ilGenerator.Emit(OpCodes.Call, CompilerGlobals.hashtableGetItem);
      ilGenerator.Emit(OpCodes.Dup);
      Label label = ilGenerator.DefineLabel();
      ilGenerator.Emit(OpCodes.Brtrue, label);
      ilGenerator.Emit(OpCodes.Pop);
      ilGenerator.Emit(OpCodes.Ldsfld, CompilerGlobals.missingField);
      ilGenerator.MarkLabel(label);
      ilGenerator.Emit(OpCodes.Ret);
      this.expandoItemProp.SetGetMethod(this.getItem);
    }
    return (MethodInfo) this.getItem;
  }

  internal MethodInfo GetExpandoIndexerSetter()
  {
    if ((MethodInfo) this.setItem == (MethodInfo) null)
    {
      TypeBuilder typeBuilder = this.classob.GetTypeBuilder();
      this.GetExpandoFieldGetter(typeBuilder);
      this.setItem = typeBuilder.DefineMethod("set_Item", MethodAttributes.Public | MethodAttributes.SpecialName, Typeob.Void, new Type[2]
      {
        Typeob.String,
        Typeob.Object
      });
      ILGenerator ilGenerator = this.setItem.GetILGenerator();
      ilGenerator.Emit(OpCodes.Ldarg_0);
      ilGenerator.Emit(OpCodes.Call, (MethodInfo) this.getHashTableMethod);
      ilGenerator.Emit(OpCodes.Ldarg_2);
      ilGenerator.Emit(OpCodes.Ldsfld, CompilerGlobals.missingField);
      Label label = ilGenerator.DefineLabel();
      ilGenerator.Emit(OpCodes.Beq, label);
      ilGenerator.Emit(OpCodes.Ldarg_1);
      ilGenerator.Emit(OpCodes.Ldarg_2);
      ilGenerator.Emit(OpCodes.Call, CompilerGlobals.hashtableSetItem);
      ilGenerator.Emit(OpCodes.Ret);
      ilGenerator.MarkLabel(label);
      ilGenerator.Emit(OpCodes.Ldarg_1);
      ilGenerator.Emit(OpCodes.Call, CompilerGlobals.hashtableRemove);
      ilGenerator.Emit(OpCodes.Ret);
      this.expandoItemProp.SetSetMethod(this.setItem);
    }
    return (MethodInfo) this.setItem;
  }

  private void GetExpandoDeleteMethod()
  {
    TypeBuilder typeBuilder = this.classob.GetTypeBuilder();
    MethodBuilder methodBuilder = this.deleteOpMethod = typeBuilder.DefineMethod("op_Delete", MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, Typeob.Boolean, new Type[2]
    {
      (Type) typeBuilder,
      Typeob.ArrayOfObject
    });
    methodBuilder.DefineParameter(2, ParameterAttributes.None, (string) null).SetCustomAttribute(new CustomAttributeBuilder(Typeob.ParamArrayAttribute.GetConstructor(Type.EmptyTypes), new object[0]));
    ILGenerator ilGenerator = methodBuilder.GetILGenerator();
    ilGenerator.Emit(OpCodes.Ldarg_0);
    ilGenerator.Emit(OpCodes.Call, (MethodInfo) this.getHashTableMethod);
    ilGenerator.Emit(OpCodes.Ldarg_1);
    ilGenerator.Emit(OpCodes.Dup);
    ilGenerator.Emit(OpCodes.Ldlen);
    ilGenerator.Emit(OpCodes.Ldc_I4_1);
    ilGenerator.Emit(OpCodes.Sub);
    ilGenerator.Emit(OpCodes.Ldelem_Ref);
    ilGenerator.Emit(OpCodes.Call, CompilerGlobals.hashtableRemove);
    ilGenerator.Emit(OpCodes.Ldc_I4_1);
    ilGenerator.Emit(OpCodes.Ret);
  }

  private string GetFullNameFor(MemberInfo supMem)
  {
    string str;
    switch (supMem)
    {
      case JSField _:
        str = ((JSField) supMem).GetClassFullName();
        break;
      case JSConstructor _:
        str = ((JSConstructor) supMem).GetClassFullName();
        break;
      case JSMethod _:
        str = ((JSMethod) supMem).GetClassFullName();
        break;
      case JSProperty _:
        str = ((JSProperty) supMem).GetClassFullName();
        break;
      case JSWrappedProperty _:
        str = ((JSWrappedProperty) supMem).GetClassFullName();
        break;
      default:
        str = supMem.DeclaringType.FullName;
        break;
    }
    return $"{str}.{supMem.Name}";
  }

  internal MemberInfo[] GetInterfaceMember(string name)
  {
    this.PartiallyEvaluate();
    if (this.isInterface)
    {
      MemberInfo[] member = this.classob.GetMember(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
      if (member != null && member.Length != 0)
        return member;
    }
    foreach (TypeExpression typeExpression in this.interfaces)
    {
      MemberInfo[] member = typeExpression.ToIReflect().GetMember(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
      if (member != null && member.Length != 0)
        return member;
    }
    return new MemberInfo[0];
  }

  private void GetIRForSuperType()
  {
    IReflect reflect = this.superIR = (IReflect) Typeob.Object;
    if (this.superTypeExpression != null)
    {
      this.superTypeExpression.PartiallyEvaluate();
      reflect = this.superIR = this.superTypeExpression.ToIReflect();
    }
    Type c = reflect as Type;
    if (c != (Type) null)
    {
      if (c.IsSealed || c.IsInterface || c == Typeob.ValueType || c == Typeob.ArrayObject)
      {
        if (this.superTypeExpression.Evaluate() is Namespace)
          this.superTypeExpression.context.HandleError(JSError.NeedType);
        else
          this.superTypeExpression.context.HandleError(JSError.TypeCannotBeExtended, c.FullName);
        this.superTypeExpression = (TypeExpression) null;
        this.superIR = (IReflect) Typeob.Object;
      }
      else
      {
        if (!Typeob.INeedEngine.IsAssignableFrom(c))
          return;
        this.needsEngine = false;
      }
    }
    else if (reflect is ClassScope)
    {
      if (((ClassScope) reflect).owner.IsASubClassOf(this))
      {
        this.superTypeExpression.context.HandleError(JSError.CircularDefinition);
        this.superTypeExpression = (TypeExpression) null;
        this.superIR = (IReflect) Typeob.Object;
      }
      else
      {
        this.needsEngine = false;
        this.superClass = ((ClassScope) reflect).owner;
        if ((this.superClass.attributes & TypeAttributes.Sealed) != TypeAttributes.NotPublic)
        {
          this.superTypeExpression.context.HandleError(JSError.TypeCannotBeExtended, this.superClass.name);
          this.superClass.attributes &= ~TypeAttributes.Sealed;
          this.superTypeExpression = (TypeExpression) null;
        }
        else
        {
          if (!this.superClass.isInterface)
            return;
          this.superTypeExpression.context.HandleError(JSError.TypeCannotBeExtended, this.superClass.name);
          this.superIR = (IReflect) Typeob.Object;
          this.superTypeExpression = (TypeExpression) null;
        }
      }
    }
    else
    {
      this.superTypeExpression.context.HandleError(JSError.TypeCannotBeExtended);
      this.superIR = (IReflect) Typeob.Object;
      this.superTypeExpression = (TypeExpression) null;
    }
  }

  private void GetStartIndexForEachName()
  {
    SimpleHashtable simpleHashtable = new SimpleHashtable(32U /*0x20*/);
    string str = (string) null;
    int index = 0;
    for (int length = this.superMembers.Length; index < length; ++index)
    {
      string name = ((MemberInfo) this.superMembers[index]).Name;
      if (name != str)
        simpleHashtable[(object) (str = name)] = (object) index;
    }
    this.firstIndex = simpleHashtable;
  }

  internal ConstructorInfo GetSuperConstructor(IReflect[] argIRs)
  {
    object obj = this.superTypeExpression == null ? (object) Typeob.Object : this.superTypeExpression.Evaluate();
    return obj is ClassScope ? JSBinder.SelectConstructor((MemberInfo[]) ((ClassScope) obj).constructors, argIRs) : JSBinder.SelectConstructor((MemberInfo[]) ((Type) obj).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), argIRs);
  }

  private void GetSuperTypeMembers()
  {
    SuperTypeMembersSorter sorter = new SuperTypeMembersSorter();
    for (IReflect reflect = this.superIR; reflect != null; reflect = (object) (reflect as Type) == null ? ((ClassScope) reflect).GetSuperType() : (IReflect) ((Type) reflect).BaseType)
      sorter.Add(reflect.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
    ArrayList implicitInterfaces = new ArrayList();
    int length = this.interfaces.Length;
    IReflect[] explicitInterfaces = new IReflect[length];
    for (int index = 0; index < length; ++index)
    {
      IReflect reflect = explicitInterfaces[index] = this.interfaces[index].ToIReflect();
      Type type = reflect as Type;
      if (!(!(type != (Type) null) ? ((ClassScope) reflect).owner.isInterface : type.IsInterface))
        this.interfaces[index].context.HandleError(JSError.NeedInterface);
    }
    foreach (IReflect iface in explicitInterfaces)
      this.AddImplicitInterfaces(iface, explicitInterfaces, implicitInterfaces);
    for (int index = 0; index < implicitInterfaces.Count; ++index)
      this.AddImplicitInterfaces((IReflect) implicitInterfaces[index], explicitInterfaces, implicitInterfaces);
    int count = implicitInterfaces.Count;
    if (count > 0)
    {
      TypeExpression[] typeExpressionArray = new TypeExpression[length + count];
      for (int index = 0; index < length; ++index)
        typeExpressionArray[index] = this.interfaces[index];
      for (int index = 0; index < count; ++index)
        typeExpressionArray[index + length] = new TypeExpression((AST) new ConstantWrapper(implicitInterfaces[index], (Context) null));
      this.interfaces = typeExpressionArray;
    }
    foreach (TypeExpression typeExpression in this.interfaces)
    {
      if (typeExpression.ToIReflect() is ClassScope ireflect && ireflect.owner.ImplementsInterface((IReflect) this.classob))
      {
        this.context.HandleError(JSError.CircularDefinition);
        this.interfaces = new TypeExpression[0];
        break;
      }
      sorter.Add(typeExpression.ToIReflect().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
    }
    IReflect reflect1 = this.superIR;
    while (reflect1 != null)
    {
      Type type = reflect1 as Type;
      if (type != (Type) null)
      {
        if (type.IsAbstract)
        {
          Class.GetUnimplementedInferfaceMembersFor(type, sorter);
          reflect1 = (IReflect) type.BaseType;
        }
        else
          break;
      }
      else
      {
        ClassScope classScope = (ClassScope) reflect1;
        if (classScope.owner.isAbstract)
        {
          classScope.owner.GetUnimplementedInferfaceMembers(sorter);
          reflect1 = (IReflect) null;
        }
        else
          break;
      }
    }
    this.superMembers = sorter.GetMembers();
  }

  internal TypeBuilder GetTypeBuilder() => (TypeBuilder) this.GetTypeBuilderOrEnumBuilder();

  internal virtual Type GetTypeBuilderOrEnumBuilder()
  {
    if (this.classob.classwriter != (Type) null)
      return this.classob.classwriter;
    if (!this.isAlreadyPartiallyEvaluated)
      this.PartiallyEvaluate();
    Type parent = this.superTypeExpression == null ? (this.isInterface ? (Type) null : Typeob.Object) : this.superTypeExpression.ToType();
    int num1 = (this.needsEngine ? 1 : 0) + (this.generateCodeForExpando ? 1 : 0);
    int length1 = this.interfaces.Length + num1;
    Type[] interfaces = new Type[length1];
    for (int index = num1; index < length1; ++index)
      interfaces[index] = this.interfaces[index - num1].ToType();
    if (this.needsEngine)
      interfaces[--num1] = Typeob.INeedEngine;
    if (this.generateCodeForExpando)
    {
      int num2;
      interfaces[num2 = num1 - 1] = Typeob.IEnumerable;
    }
    TypeBuilder builderOrEnumBuilder;
    if (this.enclosingScope is ClassScope)
    {
      if ((Type) (builderOrEnumBuilder = (TypeBuilder) this.classob.classwriter) == (Type) null)
      {
        TypeBuilder typeBuilder = ((ClassScope) this.enclosingScope).owner.GetTypeBuilder();
        if (this.classob.classwriter != (Type) null)
          return this.classob.classwriter;
        builderOrEnumBuilder = typeBuilder.DefineNestedType(this.name, this.attributes, parent, interfaces);
        this.classob.classwriter = (Type) builderOrEnumBuilder;
        if (!this.isStatic && !this.isInterface)
          this.classob.outerClassField = (FieldInfo) builderOrEnumBuilder.DefineField("outer class instance", (Type) typeBuilder, FieldAttributes.Private);
      }
    }
    else
    {
      string str1 = ((ActivationObject) this.enclosingScope).GetName();
      if (str1 == null)
      {
        VsaEngine engine = this.context.document.engine;
        if (engine != null && engine.genStartupClass)
          str1 = engine.RootNamespace;
      }
      if ((Type) (builderOrEnumBuilder = (TypeBuilder) this.classob.classwriter) == (Type) null)
      {
        string str2 = this.name;
        if (str1 != null)
          str2 = $"{str1}.{str2}";
        if (str2.Length >= 1024 /*0x0400*/)
        {
          this.context.HandleError(JSError.TypeNameTooLong, str2);
          str2 = "bad type name " + Class.badTypeNameCount.ToString((IFormatProvider) CultureInfo.InvariantCulture);
          ++Class.badTypeNameCount;
        }
        builderOrEnumBuilder = this.compilerGlobals.module.DefineType(str2, this.attributes, parent, interfaces);
        this.classob.classwriter = (Type) builderOrEnumBuilder;
      }
    }
    if (this.customAttributes != null)
    {
      foreach (CustomAttributeBuilder attributeBuilder in this.customAttributes.GetCustomAttributeBuilders(false))
        builderOrEnumBuilder.SetCustomAttribute(attributeBuilder);
    }
    if (this.clsCompliance == CLSComplianceSpec.CLSCompliant)
      builderOrEnumBuilder.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor, new object[1]
      {
        (object) true
      }));
    else if (this.clsCompliance == CLSComplianceSpec.NonCLSCompliant)
      builderOrEnumBuilder.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor, new object[1]
      {
        (object) false
      }));
    if (this.generateCodeForExpando)
      builderOrEnumBuilder.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.defaultMemberAttributeCtor, new object[1]
      {
        (object) "Item"
      }));
    int index1 = 0;
    for (int length2 = this.fields.Length; index1 < length2; ++index1)
    {
      JSMemberField jsMemberField = this.fields[index1];
      if (jsMemberField.IsLiteral)
      {
        object ob = jsMemberField.value;
        if (ob is JSProperty)
        {
          JSProperty jsProperty = (JSProperty) ob;
          ParameterInfo[] indexParameters = jsProperty.GetIndexParameters();
          int length3 = indexParameters.Length;
          Type[] parameterTypes = new Type[length3];
          for (int index2 = 0; index2 < length3; ++index2)
            parameterTypes[index2] = indexParameters[index2].ParameterType;
          PropertyBuilder propertyBuilder = jsProperty.metaData = builderOrEnumBuilder.DefineProperty(jsMemberField.Name, jsProperty.Attributes, jsProperty.PropertyType, parameterTypes);
          if ((MethodInfo) jsProperty.getter != (MethodInfo) null)
          {
            CustomAttributeList customAttributes = ((JSFieldMethod) jsProperty.getter).func.customAttributes;
            if (customAttributes != null)
            {
              foreach (CustomAttributeBuilder attributeBuilder in customAttributes.GetCustomAttributeBuilders(true))
                propertyBuilder.SetCustomAttribute(attributeBuilder);
            }
            propertyBuilder.SetGetMethod((MethodBuilder) jsProperty.getter.GetMethodInfo(this.compilerGlobals));
          }
          if ((MethodInfo) jsProperty.setter != (MethodInfo) null)
          {
            CustomAttributeList customAttributes = ((JSFieldMethod) jsProperty.setter).func.customAttributes;
            if (customAttributes != null)
            {
              foreach (CustomAttributeBuilder attributeBuilder in customAttributes.GetCustomAttributeBuilders(true))
                propertyBuilder.SetCustomAttribute(attributeBuilder);
            }
            propertyBuilder.SetSetMethod((MethodBuilder) jsProperty.setter.GetMethodInfo(this.compilerGlobals));
            continue;
          }
          continue;
        }
        if (ob is ClassScope)
        {
          ((ClassScope) ob).GetTypeBuilderOrEnumBuilder();
          continue;
        }
        if (Convert.GetTypeCode(ob) != TypeCode.Object)
        {
          FieldBuilder fieldBuilder = builderOrEnumBuilder.DefineField(jsMemberField.Name, jsMemberField.FieldType, jsMemberField.Attributes);
          fieldBuilder.SetConstant(jsMemberField.value);
          jsMemberField.metaData = (object) fieldBuilder;
          jsMemberField.WriteCustomAttribute(this.Engine.doCRS);
          continue;
        }
        if (ob is FunctionObject)
        {
          FunctionObject functionObject = (FunctionObject) ob;
          if (functionObject.isExpandoMethod)
          {
            jsMemberField.metaData = (object) builderOrEnumBuilder.DefineField(jsMemberField.Name, Typeob.ScriptFunction, jsMemberField.Attributes & ~(FieldAttributes.Static | FieldAttributes.Literal));
            functionObject.isStatic = false;
          }
          if (this.isInterface)
          {
            while (true)
            {
              functionObject.GetMethodInfo(this.compilerGlobals);
              jsMemberField = jsMemberField.nextOverload;
              if (!((FieldInfo) jsMemberField == (FieldInfo) null))
                functionObject = (FunctionObject) jsMemberField.value;
              else
                goto label_66;
            }
          }
          else
            continue;
        }
        else
          continue;
      }
      else
      {
        jsMemberField.metaData = (object) builderOrEnumBuilder.DefineField(jsMemberField.Name, jsMemberField.FieldType, jsMemberField.Attributes);
        jsMemberField.WriteCustomAttribute(this.Engine.doCRS);
        continue;
      }
label_66:;
    }
    return (Type) builderOrEnumBuilder;
  }

  private void GetUnimplementedInferfaceMembers(SuperTypeMembersSorter sorter)
  {
    int index = 0;
    for (int length = this.superMembers.Length; index < length; ++index)
    {
      MethodInfo superMember = this.superMembers[index] as MethodInfo;
      if (superMember != (MethodInfo) null && superMember.DeclaringType.IsInterface)
        sorter.Add((MemberInfo) superMember);
    }
  }

  private static void GetUnimplementedInferfaceMembersFor(Type type, SuperTypeMembersSorter sorter)
  {
    foreach (Type interfaceType in type.GetInterfaces())
    {
      InterfaceMapping interfaceMap = type.GetInterfaceMap(interfaceType);
      MethodInfo[] interfaceMethods = interfaceMap.InterfaceMethods;
      MethodInfo[] targetMethods = interfaceMap.TargetMethods;
      int index = 0;
      for (int length = interfaceMethods.Length; index < length; ++index)
      {
        if (targetMethods[index] == (MethodInfo) null || targetMethods[index].IsAbstract)
          sorter.Add((MemberInfo) interfaceMethods[index]);
      }
    }
  }

  internal bool ImplementsInterface(IReflect iface)
  {
    foreach (TypeExpression typeExpression in this.interfaces)
    {
      IReflect ireflect = typeExpression.ToIReflect();
      if (ireflect == iface || ireflect is ClassScope && ((ClassScope) ireflect).ImplementsInterface(iface) || (object) (ireflect as Type) != null && (object) (iface as Type) != null && ((Type) iface).IsAssignableFrom((Type) ireflect))
        return true;
    }
    return false;
  }

  private bool IsASubClassOf(Class cl)
  {
    if (this.superTypeExpression != null)
    {
      this.superTypeExpression.PartiallyEvaluate();
      IReflect ireflect = this.superTypeExpression.ToIReflect();
      if (ireflect is ClassScope)
      {
        Class owner = ((ClassScope) ireflect).owner;
        return owner == cl || owner.IsASubClassOf(cl);
      }
    }
    return false;
  }

  internal bool IsCustomAttribute()
  {
    this.GetIRForSuperType();
    if ((object) this.superIR != (object) Typeob.Attribute || this.customAttributes == null)
      return false;
    this.customAttributes.PartiallyEvaluate();
    return this.validOn != (AttributeTargets) 0;
  }

  internal bool IsExpando()
  {
    if (this.hasAlreadyBeenAskedAboutExpando)
      return this.isExpando;
    if (this.customAttributes != null)
    {
      this.customAttributes.PartiallyEvaluate();
      if (this.customAttributes.GetAttribute(Typeob.Expando) != null)
        this.generateCodeForExpando = this.isExpando = true;
    }
    bool superClassIsExpando = false;
    this.GetIRForSuperType();
    if (this.superIR is ClassScope superIr)
    {
      superIr.owner.PartiallyEvaluate();
      if (superIr.owner.IsExpando())
        this.isExpando = superClassIsExpando = true;
    }
    else if (CustomAttribute.IsDefined((MemberInfo) (Type) this.superIR, typeof (Expando), true))
      this.isExpando = superClassIsExpando = true;
    this.hasAlreadyBeenAskedAboutExpando = true;
    if (this.generateCodeForExpando)
      this.CheckIfOKToGenerateCodeForExpando(superClassIsExpando);
    if (!this.isExpando)
      return false;
    this.classob.noExpando = false;
    return true;
  }

  private bool IsInTheSameCompilationUnit(MemberInfo member)
  {
    return member is JSField || member is JSMethod;
  }

  private bool IsInTheSamePackage(MemberInfo member)
  {
    switch (member)
    {
      case JSMethod _:
      case JSField _:
        PackageScope packageScope = !(member is JSMethod) ? (!(member is JSConstructor) ? ((JSField) member).GetPackage() : ((JSConstructor) member).GetPackage()) : ((JSMethod) member).GetPackage();
        return this.classob.GetPackage() == packageScope;
      default:
        return false;
    }
  }

  internal bool IsStatic => this.isStatic || !(this.enclosingScope is ClassScope);

  protected bool NeedsToBeCheckedForCLSCompliance()
  {
    bool forClsCompliance = false;
    this.clsCompliance = CLSComplianceSpec.NotAttributed;
    if (this.customAttributes != null)
    {
      CustomAttribute attribute = this.customAttributes.GetAttribute(Typeob.CLSCompliantAttribute);
      if (attribute != null)
      {
        this.clsCompliance = attribute.GetCLSComplianceValue();
        forClsCompliance = this.clsCompliance == CLSComplianceSpec.CLSCompliant;
        this.customAttributes.Remove(attribute);
      }
    }
    if (this.clsCompliance == CLSComplianceSpec.CLSCompliant && !this.Engine.isCLSCompliant)
      this.context.HandleError(JSError.TypeAssemblyCLSCompliantMismatch);
    if (this.clsCompliance == CLSComplianceSpec.NotAttributed && (this.attributes & TypeAttributes.Public) != TypeAttributes.NotPublic)
      forClsCompliance = this.Engine.isCLSCompliant;
    return forClsCompliance;
  }

  internal static bool ParametersMatch(ParameterInfo[] suppars, ParameterInfo[] pars)
  {
    if (suppars.Length != pars.Length)
      return false;
    int index = 0;
    for (int length = pars.Length; index < length; ++index)
    {
      IReflect reflect = suppars[index] is ParameterDeclaration ? ((ParameterDeclaration) suppars[index]).ParameterIReflect : (IReflect) suppars[index].ParameterType;
      if (!(pars[index] is ParameterDeclaration ? (object) ((ParameterDeclaration) pars[index]).ParameterIReflect : (object) pars[index].ParameterType).Equals((object) reflect))
        return false;
    }
    return true;
  }

  internal override AST PartiallyEvaluate()
  {
    if (this.isAlreadyPartiallyEvaluated)
      return (AST) this;
    this.isAlreadyPartiallyEvaluated = true;
    this.IsExpando();
    this.classob.SetParent((ScriptObject) new WithObject(this.enclosingScope, (object) this.superIR, true));
    this.Globals.ScopeStack.Push((object) this.classob);
    try
    {
      this.body.PartiallyEvaluate();
      if (this.implicitDefaultConstructor != null)
        this.implicitDefaultConstructor.PartiallyEvaluate();
    }
    finally
    {
      this.Globals.ScopeStack.Pop();
    }
    foreach (JSMemberField field in this.fields)
      field.CheckOverloadsForDuplicates();
    this.CheckIfValidExtensionOfSuperType();
    this.CheckThatAllAbstractSuperClassMethodsAreImplemented();
    return (AST) this;
  }

  private void SetAccessibility(FieldAttributes attributes)
  {
    FieldAttributes fieldAttributes = attributes & FieldAttributes.FieldAccessMask;
    if (this.enclosingScope is ClassScope)
    {
      switch (fieldAttributes)
      {
        case FieldAttributes.Private:
          this.attributes |= TypeAttributes.NestedPrivate;
          break;
        case FieldAttributes.Assembly:
          this.attributes |= TypeAttributes.NestedAssembly;
          break;
        case FieldAttributes.Family:
          this.attributes |= TypeAttributes.NestedFamily;
          break;
        case FieldAttributes.FamORAssem:
          this.attributes |= TypeAttributes.VisibilityMask;
          break;
        case FieldAttributes.Public:
          this.attributes |= TypeAttributes.NestedPublic;
          break;
        default:
          this.attributes |= TypeAttributes.NestedPublic;
          break;
      }
    }
    else
    {
      if (fieldAttributes != FieldAttributes.Public && fieldAttributes != FieldAttributes.PrivateScope)
        return;
      this.attributes |= TypeAttributes.Public;
    }
  }

  private void SetupConstructors()
  {
    MemberInfo[] member = this.classob.GetMember(this.name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    if (member == null)
    {
      this.AllocateImplicitDefaultConstructor();
      FieldInfo fieldInfo = (FieldInfo) this.classob.AddNewField(this.name, (object) this.implicitDefaultConstructor, FieldAttributes.Literal);
      this.classob.constructors = new ConstructorInfo[1]
      {
        (ConstructorInfo) new JSConstructor(this.implicitDefaultConstructor)
      };
    }
    else
    {
      MemberInfo memberInfo1 = (MemberInfo) null;
      foreach (MemberInfo memberInfo2 in member)
      {
        if (memberInfo2 is JSFieldMethod)
        {
          FunctionObject func = ((JSFieldMethod) memberInfo2).func;
          if (memberInfo1 == (MemberInfo) null)
            memberInfo1 = memberInfo2;
          if (func.return_type_expr != null)
            func.return_type_expr.context.HandleError(JSError.ConstructorMayNotHaveReturnType);
          if ((func.attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope || (func.attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
          {
            func.isStatic = false;
            JSVariableField field = (JSVariableField) ((JSFieldMethod) memberInfo2).field;
            field.attributeFlags &= ~FieldAttributes.Static;
            field.originalContext.HandleError(JSError.NotValidForConstructor);
          }
          func.return_type_expr = new TypeExpression((AST) new ConstantWrapper((object) Typeob.Void, this.context));
          func.own_scope.AddReturnValueField();
        }
      }
      if (memberInfo1 != (MemberInfo) null)
      {
        this.classob.constructors = ((JSMemberField) ((JSFieldMethod) memberInfo1).field).GetAsConstructors((object) this.classob);
      }
      else
      {
        this.AllocateImplicitDefaultConstructor();
        this.classob.constructors = new ConstructorInfo[1]
        {
          (ConstructorInfo) new JSConstructor(this.implicitDefaultConstructor)
        };
      }
    }
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
    this.GetTypeBuilderOrEnumBuilder();
    this.TranslateToCOMPlusClass();
    object metaData = this.ownField.GetMetaData();
    if (metaData == null)
      return;
    il.Emit(OpCodes.Ldtoken, this.classob.classwriter);
    il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
    if (metaData is LocalBuilder)
      il.Emit(OpCodes.Stloc, (LocalBuilder) metaData);
    else
      il.Emit(OpCodes.Stsfld, (FieldInfo) metaData);
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
  }

  private void EmitUsingNamespaces(ILGenerator il)
  {
    if (!this.body.Engine.GenerateDebugInfo)
      return;
    ScriptObject scriptObject = this.enclosingScope;
    while (true)
    {
      switch (scriptObject)
      {
        case null:
          goto label_8;
        case PackageScope _:
          il.UsingNamespace(((PackageScope) scriptObject).name);
          break;
        case WrappedNamespace _:
          if (!((WrappedNamespace) scriptObject).name.Equals(""))
          {
            il.UsingNamespace(((WrappedNamespace) scriptObject).name);
            break;
          }
          break;
      }
      scriptObject = scriptObject.GetParent();
    }
label_8:;
  }

  private void TranslateToCOMPlusClass()
  {
    if (this.isCooked)
      return;
    this.isCooked = true;
    if (this is EnumDeclaration)
    {
      if (this.enclosingScope is ClassScope)
        return;
      this.TranslateToCreateTypeCall();
    }
    else
    {
      if (this.superClass != null)
        this.superClass.TranslateToCOMPlusClass();
      int index = 0;
      for (int length = this.interfaces.Length; index < length; ++index)
      {
        IReflect ireflect = this.interfaces[index].ToIReflect();
        if (ireflect is ClassScope)
          ((ClassScope) ireflect).owner.TranslateToCOMPlusClass();
      }
      this.Globals.ScopeStack.Push((object) this.classob);
      TypeBuilder classwriter = this.compilerGlobals.classwriter;
      this.compilerGlobals.classwriter = (TypeBuilder) this.classob.classwriter;
      if (!this.isInterface)
      {
        ILGenerator ilGenerator1 = this.compilerGlobals.classwriter.DefineTypeInitializer().GetILGenerator();
        LocalBuilder local = (LocalBuilder) null;
        if (this.classob.staticInitializerUsesEval)
        {
          local = ilGenerator1.DeclareLocal(Typeob.VsaEngine);
          ilGenerator1.Emit(OpCodes.Ldtoken, (Type) this.classob.GetTypeBuilder());
          ConstantWrapper.TranslateToILInt(ilGenerator1, 0);
          ilGenerator1.Emit(OpCodes.Newarr, Typeob.JSLocalField);
          if (this.Engine.PEFileKind == PEFileKinds.Dll)
          {
            ilGenerator1.Emit(OpCodes.Ldtoken, (Type) this.classob.GetTypeBuilder());
            ilGenerator1.Emit(OpCodes.Call, CompilerGlobals.createVsaEngineWithType);
          }
          else
            ilGenerator1.Emit(OpCodes.Call, CompilerGlobals.createVsaEngine);
          ilGenerator1.Emit(OpCodes.Dup);
          ilGenerator1.Emit(OpCodes.Stloc, local);
          ilGenerator1.Emit(OpCodes.Call, CompilerGlobals.pushStackFrameForStaticMethod);
          ilGenerator1.BeginExceptionBlock();
        }
        this.body.TranslateToILStaticInitializers(ilGenerator1);
        if (this.classob.staticInitializerUsesEval)
        {
          ilGenerator1.BeginFinallyBlock();
          ilGenerator1.Emit(OpCodes.Ldloc, local);
          ilGenerator1.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
          ilGenerator1.Emit(OpCodes.Pop);
          ilGenerator1.EndExceptionBlock();
        }
        ilGenerator1.Emit(OpCodes.Ret);
        this.EmitUsingNamespaces(ilGenerator1);
        MethodBuilder methodBuilder = this.compilerGlobals.classwriter.DefineMethod(".init", MethodAttributes.Private, Typeob.Void, new Type[0]);
        this.fieldInitializer = (MethodInfo) methodBuilder;
        ILGenerator ilGenerator2 = methodBuilder.GetILGenerator();
        if (this.classob.instanceInitializerUsesEval)
        {
          ilGenerator2.Emit(OpCodes.Ldarg_0);
          ConstantWrapper.TranslateToILInt(ilGenerator2, 0);
          ilGenerator2.Emit(OpCodes.Newarr, Typeob.JSLocalField);
          ilGenerator2.Emit(OpCodes.Ldarg_0);
          ilGenerator2.Emit(OpCodes.Callvirt, CompilerGlobals.getEngineMethod);
          ilGenerator2.Emit(OpCodes.Call, CompilerGlobals.pushStackFrameForMethod);
          ilGenerator2.BeginExceptionBlock();
        }
        this.body.TranslateToILInstanceInitializers(ilGenerator2);
        if (this.classob.instanceInitializerUsesEval)
        {
          ilGenerator2.BeginFinallyBlock();
          ilGenerator2.Emit(OpCodes.Ldarg_0);
          ilGenerator2.Emit(OpCodes.Callvirt, CompilerGlobals.getEngineMethod);
          ilGenerator2.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
          ilGenerator2.Emit(OpCodes.Pop);
          ilGenerator2.EndExceptionBlock();
        }
        ilGenerator2.Emit(OpCodes.Ret);
        this.EmitUsingNamespaces(ilGenerator2);
        if (this.implicitDefaultConstructor != null)
          this.implicitDefaultConstructor.TranslateToIL(this.compilerGlobals);
        if (this.generateCodeForExpando)
        {
          this.GetExpandoIndexerGetter();
          this.GetExpandoIndexerSetter();
          this.GetExpandoDeleteMethod();
          this.GenerateGetEnumerator();
        }
        this.EmitILForINeedEngineMethods();
      }
      if (!(this.enclosingScope is ClassScope))
        this.TranslateToCreateTypeCall();
      this.compilerGlobals.classwriter = classwriter;
      this.Globals.ScopeStack.Pop();
    }
  }

  private void TranslateToCreateTypeCall()
  {
    if (this.cookedType != (Type) null)
      return;
    if (this is EnumDeclaration)
    {
      EnumBuilder classwriter = this.classob.classwriter as EnumBuilder;
      if ((Type) classwriter != (Type) null)
        this.cookedType = classwriter.CreateType();
      else
        this.cookedType = ((TypeBuilder) this.classob.classwriter).CreateType();
    }
    else
    {
      if (this.superClass != null)
        this.superClass.TranslateToCreateTypeCall();
      AppDomain domain = Thread.GetDomain();
      ResolveEventHandler resolveEventHandler = new ResolveEventHandler(this.ResolveEnum);
      domain.TypeResolve += resolveEventHandler;
      this.cookedType = ((TypeBuilder) this.classob.classwriter).CreateType();
      domain.TypeResolve -= resolveEventHandler;
      foreach (JSVariableField field in this.fields)
      {
        if (field.value is ClassScope classScope)
          classScope.owner.TranslateToCreateTypeCall();
      }
    }
  }

  private Assembly ResolveEnum(object sender, ResolveEventArgs args)
  {
    FieldInfo field = this.classob.GetField(args.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
    if (field != (FieldInfo) null && field.IsLiteral && TypeReferences.GetConstantValue(field) is ClassScope constantValue)
      constantValue.owner.TranslateToCreateTypeCall();
    return (Assembly) this.compilerGlobals.assemblyBuilder;
  }

  internal override Context GetFirstExecutableContext() => (Context) null;
}
