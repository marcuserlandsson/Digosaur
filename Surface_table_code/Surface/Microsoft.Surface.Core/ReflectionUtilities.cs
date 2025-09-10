// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.ReflectionUtilities
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Surface
{
  internal static class ReflectionUtilities
  {
    internal static string ConstructAssemblyQualifiedName(string assemblyName, string typeFullName)
    {
      AssemblyName name = Assembly.GetExecutingAssembly().GetName();
      return string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}, {1}, Version={2}, Culture=neutral, PublicKeyToken={3}", (object) typeFullName, (object) assemblyName, (object) name.Version, (object) BitConverter.ToString(name.GetPublicKeyToken()).Replace("-", "").ToUpperInvariant());
    }

    internal static void InvokeMethod(
      Type type,
      string methodName,
      Type[] parameterTypes,
      object[] parameters)
    {
      if (!(type != (Type) null))
        return;
      MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, (System.Reflection.Binder) null, parameterTypes, (ParameterModifier[]) null);
      if (!(method != (MethodInfo) null))
        return;
      try
      {
        method.Invoke((object) null, parameters);
      }
      catch (TargetInvocationException ex)
      {
        if (ex.InnerException != null)
          throw ex.InnerException;
        throw;
      }
    }

    internal static bool TryGetField(Type type, string fieldName, out object value)
    {
      value = (object) null;
      if (type != (Type) null)
      {
        FieldInfo field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (field != (FieldInfo) null)
        {
          try
          {
            value = field.GetValue((object) null);
          }
          catch (TargetInvocationException ex)
          {
            if (ex.InnerException != null)
              throw ex.InnerException;
            throw;
          }
          return true;
        }
      }
      return false;
    }

    internal static bool TrySetField(Type type, string fieldName, object value)
    {
      if (type != (Type) null)
      {
        FieldInfo field = type.GetField(fieldName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (field != (FieldInfo) null)
        {
          try
          {
            field.SetValue((object) null, value);
          }
          catch (TargetInvocationException ex)
          {
            if (ex.InnerException != null)
              throw ex.InnerException;
            throw;
          }
          return true;
        }
      }
      return false;
    }

    internal static bool TryGetProperty(Type type, string propertyName, out object value)
    {
      value = (object) null;
      if (type != (Type) null)
      {
        PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (property != (PropertyInfo) null)
        {
          try
          {
            value = property.GetValue((object) null, (object[]) null);
          }
          catch (TargetInvocationException ex)
          {
            if (ex.InnerException != null)
              throw ex.InnerException;
            throw;
          }
          return true;
        }
      }
      return false;
    }

    internal static bool TrySetProperty(Type type, string propertyName, object value)
    {
      if (type != (Type) null)
      {
        PropertyInfo property = type.GetProperty(propertyName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (property != (PropertyInfo) null)
        {
          try
          {
            property.SetValue((object) null, value, (object[]) null);
          }
          catch (TargetInvocationException ex)
          {
            if (ex.InnerException != null)
              throw ex.InnerException;
            throw;
          }
          return true;
        }
      }
      return false;
    }

    internal static void AddEventHandler(Type type, string eventName, Delegate value)
    {
      if (!(type != (Type) null))
        return;
      object obj1 = type.GetField("eventManager", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue((object) null);
      object obj2 = type.GetField(eventName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue((object) null);
      // ISSUE: reference to a compiler-generated field
      if (ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ReflectionUtilities), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> pSite1 = ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site1;
      // ISSUE: reference to a compiler-generated field
      if (ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site2 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (ReflectionUtilities), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site2.Target((CallSite) ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site2, obj1, (object) null);
      // ISSUE: reference to a compiler-generated field
      if (ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site3 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site3 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof (ReflectionUtilities), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      object obj4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site3.Target((CallSite) ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site3, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site4 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site4 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof (ReflectionUtilities), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target2 = ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site4.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> pSite4 = ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site4;
        object obj5 = obj3;
        // ISSUE: reference to a compiler-generated field
        if (ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site5 == null)
        {
          // ISSUE: reference to a compiler-generated field
          ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site5 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (ReflectionUtilities), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site5.Target((CallSite) ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site5, obj2, (object) null);
        obj4 = target2((CallSite) pSite4, obj5, obj6);
      }
      else
        obj4 = obj3;
      if (!target1((CallSite) pSite1, obj4))
        return;
      // ISSUE: reference to a compiler-generated field
      if (ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site6 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site6 = CallSite<Action<CallSite, object, object, Delegate>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (AddEventHandler), (IEnumerable<Type>) null, typeof (ReflectionUtilities), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site6.Target((CallSite) ReflectionUtilities.\u003CAddEventHandler\u003Eo__SiteContainer0.\u003C\u003Ep__Site6, obj1, obj2, value);
    }

    internal static void RemoveEventHandler(Type type, string eventName, Delegate value)
    {
      if (!(type != (Type) null))
        return;
      object obj1 = type.GetField("eventManager", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue((object) null);
      object obj2 = type.GetField(eventName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).GetValue((object) null);
      // ISSUE: reference to a compiler-generated field
      if (ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Site8 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Site8 = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ReflectionUtilities), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Site8.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> pSite8 = ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Site8;
      // ISSUE: reference to a compiler-generated field
      if (ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Site9 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Site9 = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (ReflectionUtilities), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj3 = ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Site9.Target((CallSite) ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Site9, obj1, (object) null);
      // ISSUE: reference to a compiler-generated field
      if (ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Sitea == null)
      {
        // ISSUE: reference to a compiler-generated field
        ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Sitea = CallSite<Func<CallSite, object, bool>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsFalse, typeof (ReflectionUtilities), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      object obj4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Sitea.Target((CallSite) ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Sitea, obj3))
      {
        // ISSUE: reference to a compiler-generated field
        if (ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Siteb == null)
        {
          // ISSUE: reference to a compiler-generated field
          ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Siteb = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.BinaryOperationLogical, ExpressionType.And, typeof (ReflectionUtilities), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, object, object> target2 = ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Siteb.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, object, object>> pSiteb = ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Siteb;
        object obj5 = obj3;
        // ISSUE: reference to a compiler-generated field
        if (ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Sitec == null)
        {
          // ISSUE: reference to a compiler-generated field
          ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Sitec = CallSite<Func<CallSite, object, object, object>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (ReflectionUtilities), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj6 = ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Sitec.Target((CallSite) ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Sitec, obj2, (object) null);
        obj4 = target2((CallSite) pSiteb, obj5, obj6);
      }
      else
        obj4 = obj3;
      if (!target1((CallSite) pSite8, obj4))
        return;
      // ISSUE: reference to a compiler-generated field
      if (ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Sited == null)
      {
        // ISSUE: reference to a compiler-generated field
        ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Sited = CallSite<Action<CallSite, object, object, Delegate>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, nameof (RemoveEventHandler), (IEnumerable<Type>) null, typeof (ReflectionUtilities), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[3]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Sited.Target((CallSite) ReflectionUtilities.\u003CRemoveEventHandler\u003Eo__SiteContainer7.\u003C\u003Ep__Sited, obj1, obj2, value);
    }
  }
}
