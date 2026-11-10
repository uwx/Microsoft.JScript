// Decompiled with JetBrains decompiler
// Type: Microsoft.JScript.CustomAttributeList
// Assembly: Microsoft.JScript, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 946AD632-7F67-424B-97EC-EF867744749C
// Assembly location: C:\Windows\Microsoft.NET\Framework64\v4.0.30319\Microsoft.JScript.dll
// XML documentation location: C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\Microsoft.JScript.xml

using System;
using System.Collections;
using System.Reflection.Emit;

#nullable disable
namespace Microsoft.JScript;

internal sealed class CustomAttributeList : AST
{
  private ArrayList list;
  private ArrayList customAttributes;
  private bool alreadyPartiallyEvaluated;

  internal CustomAttributeList(Context context)
    : base(context)
  {
    this.list = new ArrayList();
    this.customAttributes = (ArrayList) null;
    this.alreadyPartiallyEvaluated = false;
  }

  internal void Append(CustomAttribute elem)
  {
    this.list.Add((object) elem);
    this.context.UpdateWith(elem.context);
  }

  internal bool ContainsExpandoAttribute()
  {
    int index = 0;
    for (int count = this.list.Count; index < count; ++index)
    {
      CustomAttribute customAttribute = (CustomAttribute) this.list[index];
      if (customAttribute != null && customAttribute.IsExpandoAttribute())
        return true;
    }
    return false;
  }

  internal CustomAttribute GetAttribute(Type attributeClass)
  {
    int index = 0;
    for (int count = this.list.Count; index < count; ++index)
    {
      CustomAttribute customAttribute = (CustomAttribute) this.list[index];
      if (customAttribute != null)
      {
        object type = customAttribute.type;
        if ((object) (type as Type) != null && (Type) type == attributeClass)
          return (CustomAttribute) this.list[index];
      }
    }
    return (CustomAttribute) null;
  }

  internal override object Evaluate() => this.Evaluate(false);

  internal object Evaluate(bool getForProperty)
  {
    int count = this.list.Count;
    ArrayList arrayList = new ArrayList(count);
    for (int index = 0; index < count; ++index)
    {
      CustomAttribute customAttribute = (CustomAttribute) this.list[index];
      if (customAttribute != null)
      {
        if (customAttribute.raiseToPropertyLevel)
        {
          if (!getForProperty)
            continue;
        }
        else if (getForProperty)
          continue;
        arrayList.Add(customAttribute.Evaluate());
      }
    }
    object[] objArray = new object[arrayList.Count];
    arrayList.CopyTo((Array) objArray);
    return (object) objArray;
  }

  internal CustomAttributeBuilder[] GetCustomAttributeBuilders(bool getForProperty)
  {
    this.customAttributes = new ArrayList(this.list.Count);
    int index = 0;
    for (int count = this.list.Count; index < count; ++index)
    {
      CustomAttribute customAttribute1 = (CustomAttribute) this.list[index];
      if (customAttribute1 != null)
      {
        if (customAttribute1.raiseToPropertyLevel)
        {
          if (!getForProperty)
            continue;
        }
        else if (getForProperty)
          continue;
        CustomAttributeBuilder customAttribute2 = customAttribute1.GetCustomAttribute();
        if (customAttribute2 != null)
          this.customAttributes.Add((object) customAttribute2);
      }
    }
    CustomAttributeBuilder[] attributeBuilders = new CustomAttributeBuilder[this.customAttributes.Count];
    this.customAttributes.CopyTo((Array) attributeBuilders);
    return attributeBuilders;
  }

  internal override AST PartiallyEvaluate()
  {
    if (this.alreadyPartiallyEvaluated)
      return (AST) this;
    this.alreadyPartiallyEvaluated = true;
    int index1 = 0;
    for (int count = this.list.Count; index1 < count; ++index1)
      this.list[index1] = (object) ((AST) this.list[index1]).PartiallyEvaluate();
    int index2 = 0;
    for (int count = this.list.Count; index2 < count; ++index2)
    {
      CustomAttribute customAttribute1 = (CustomAttribute) this.list[index2];
      if (customAttribute1 != null)
      {
        object attributeHasToBeUnique = customAttribute1.GetTypeIfAttributeHasToBeUnique();
        if (attributeHasToBeUnique != null)
        {
          for (int index3 = index2 + 1; index3 < count; ++index3)
          {
            CustomAttribute customAttribute2 = (CustomAttribute) this.list[index3];
            if (customAttribute2 != null && attributeHasToBeUnique == customAttribute2.type)
            {
              customAttribute2.context.HandleError(JSError.CustomAttributeUsedMoreThanOnce);
              this.list[index3] = (object) null;
            }
          }
        }
      }
    }
    return (AST) this;
  }

  internal void Remove(CustomAttribute elem) => this.list.Remove((object) elem);

  internal void SetTarget(AST target)
  {
    int index = 0;
    for (int count = this.list.Count; index < count; ++index)
      ((CustomAttribute) this.list[index]).SetTarget(target);
  }

  internal override void TranslateToIL(ILGenerator il, Type rtype)
  {
  }

  internal override void TranslateToILInitializer(ILGenerator il)
  {
  }
}
