// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.ApplicationServices
// Assembly: Microsoft.Surface, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 3075CDD2-EF12-4C05-803F-ABBE1D5E74B0
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.dll

using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

#nullable disable
namespace Microsoft.Surface
{
  public static class ApplicationServices
  {
    private static Type internalApplicationServicesType;
    private static bool initialized;

    public static UserOrientation InitialOrientation
    {
      get
      {
        if (ApplicationServices.\u003Cget_InitialOrientation\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 == null)
          ApplicationServices.\u003Cget_InitialOrientation\u003Eo__SiteContainer0.\u003C\u003Ep__Site1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Func<CallSite, object, bool> target = ApplicationServices.\u003Cget_InitialOrientation\u003Eo__SiteContainer0.\u003C\u003Ep__Site1.Target;
        CallSite<Func<CallSite, object, bool>> pSite1 = ApplicationServices.\u003Cget_InitialOrientation\u003Eo__SiteContainer0.\u003C\u003Ep__Site1;
        if (ApplicationServices.\u003Cget_InitialOrientation\u003Eo__SiteContainer0.\u003C\u003Ep__Site3 == null)
          ApplicationServices.\u003Cget_InitialOrientation\u003Eo__SiteContainer0.\u003C\u003Ep__Site3 = CallSite<ApplicationServices.\u003Cget_InitialOrientation\u003Eo__SiteContainer0.\u003C\u003Eq__SiteDelegate2>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "TryGetProperty", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
          }));
        object obj1;
        object obj2 = ApplicationServices.\u003Cget_InitialOrientation\u003Eo__SiteContainer0.\u003C\u003Ep__Site3.Target((CallSite) ApplicationServices.\u003Cget_InitialOrientation\u003Eo__SiteContainer0.\u003C\u003Ep__Site3, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, nameof (InitialOrientation), out obj1);
        return target((CallSite) pSite1, obj2) ? (UserOrientation) obj1 : UserOrientation.Bottom;
      }
    }

    public static long SessionId
    {
      get
      {
        if (ApplicationServices.\u003Cget_SessionId\u003Eo__SiteContainer4.\u003C\u003Ep__Site5 == null)
          ApplicationServices.\u003Cget_SessionId\u003Eo__SiteContainer4.\u003C\u003Ep__Site5 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Func<CallSite, object, bool> target = ApplicationServices.\u003Cget_SessionId\u003Eo__SiteContainer4.\u003C\u003Ep__Site5.Target;
        CallSite<Func<CallSite, object, bool>> pSite5 = ApplicationServices.\u003Cget_SessionId\u003Eo__SiteContainer4.\u003C\u003Ep__Site5;
        if (ApplicationServices.\u003Cget_SessionId\u003Eo__SiteContainer4.\u003C\u003Ep__Site7 == null)
          ApplicationServices.\u003Cget_SessionId\u003Eo__SiteContainer4.\u003C\u003Ep__Site7 = CallSite<ApplicationServices.\u003Cget_SessionId\u003Eo__SiteContainer4.\u003C\u003Eq__SiteDelegate6>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "TryGetProperty", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
          }));
        object obj1;
        object obj2 = ApplicationServices.\u003Cget_SessionId\u003Eo__SiteContainer4.\u003C\u003Ep__Site7.Target((CallSite) ApplicationServices.\u003Cget_SessionId\u003Eo__SiteContainer4.\u003C\u003Ep__Site7, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, nameof (SessionId), out obj1);
        return target((CallSite) pSite5, obj2) ? (long) obj1 : 0L;
      }
    }

    public static ShutdownReason ShutdownReason
    {
      get
      {
        if (ApplicationServices.\u003Cget_ShutdownReason\u003Eo__SiteContainer8.\u003C\u003Ep__Site9 == null)
          ApplicationServices.\u003Cget_ShutdownReason\u003Eo__SiteContainer8.\u003C\u003Ep__Site9 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Func<CallSite, object, bool> target = ApplicationServices.\u003Cget_ShutdownReason\u003Eo__SiteContainer8.\u003C\u003Ep__Site9.Target;
        CallSite<Func<CallSite, object, bool>> pSite9 = ApplicationServices.\u003Cget_ShutdownReason\u003Eo__SiteContainer8.\u003C\u003Ep__Site9;
        if (ApplicationServices.\u003Cget_ShutdownReason\u003Eo__SiteContainer8.\u003C\u003Ep__Siteb == null)
          ApplicationServices.\u003Cget_ShutdownReason\u003Eo__SiteContainer8.\u003C\u003Ep__Siteb = CallSite<ApplicationServices.\u003Cget_ShutdownReason\u003Eo__SiteContainer8.\u003C\u003Eq__SiteDelegatea>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "TryGetProperty", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
          }));
        object obj1;
        object obj2 = ApplicationServices.\u003Cget_ShutdownReason\u003Eo__SiteContainer8.\u003C\u003Ep__Siteb.Target((CallSite) ApplicationServices.\u003Cget_ShutdownReason\u003Eo__SiteContainer8.\u003C\u003Ep__Siteb, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, nameof (ShutdownReason), out obj1);
        return target((CallSite) pSite9, obj2) ? (ShutdownReason) obj1 : ShutdownReason.None;
      }
    }

    public static WindowAvailability WindowAvailability
    {
      get
      {
        if (ApplicationServices.\u003Cget_WindowAvailability\u003Eo__SiteContainerc.\u003C\u003Ep__Sited == null)
          ApplicationServices.\u003Cget_WindowAvailability\u003Eo__SiteContainerc.\u003C\u003Ep__Sited = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        Func<CallSite, object, bool> target = ApplicationServices.\u003Cget_WindowAvailability\u003Eo__SiteContainerc.\u003C\u003Ep__Sited.Target;
        CallSite<Func<CallSite, object, bool>> pSited = ApplicationServices.\u003Cget_WindowAvailability\u003Eo__SiteContainerc.\u003C\u003Ep__Sited;
        if (ApplicationServices.\u003Cget_WindowAvailability\u003Eo__SiteContainerc.\u003C\u003Ep__Sitef == null)
          ApplicationServices.\u003Cget_WindowAvailability\u003Eo__SiteContainerc.\u003C\u003Ep__Sitef = CallSite<ApplicationServices.\u003Cget_WindowAvailability\u003Eo__SiteContainerc.\u003C\u003Eq__SiteDelegatee>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "TryGetProperty", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsOut, (string) null)
          }));
        object obj1;
        object obj2 = ApplicationServices.\u003Cget_WindowAvailability\u003Eo__SiteContainerc.\u003C\u003Ep__Sitef.Target((CallSite) ApplicationServices.\u003Cget_WindowAvailability\u003Eo__SiteContainerc.\u003C\u003Ep__Sitef, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, nameof (WindowAvailability), out obj1);
        return target((CallSite) pSited, obj2) ? (WindowAvailability) obj1 : WindowAvailability.Interactive;
      }
    }

    public static event EventHandler<CancelEventArgs> InactivityTimeoutOccurring
    {
      add
      {
        if (ApplicationServices.\u003Cadd_InactivityTimeoutOccurring\u003Eo__SiteContainer10.\u003C\u003Ep__Site11 == null)
          ApplicationServices.\u003Cadd_InactivityTimeoutOccurring\u003Eo__SiteContainer10.\u003C\u003Ep__Site11 = CallSite<Action<CallSite, Type, object, string, EventHandler<CancelEventArgs>>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "AddEventHandler", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        ApplicationServices.\u003Cadd_InactivityTimeoutOccurring\u003Eo__SiteContainer10.\u003C\u003Ep__Site11.Target((CallSite) ApplicationServices.\u003Cadd_InactivityTimeoutOccurring\u003Eo__SiteContainer10.\u003C\u003Ep__Site11, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, "InactivityTimeoutOccurringEvent", value);
      }
      remove
      {
        if (ApplicationServices.\u003Cremove_InactivityTimeoutOccurring\u003Eo__SiteContainer12.\u003C\u003Ep__Site13 == null)
          ApplicationServices.\u003Cremove_InactivityTimeoutOccurring\u003Eo__SiteContainer12.\u003C\u003Ep__Site13 = CallSite<Action<CallSite, Type, object, string, EventHandler<CancelEventArgs>>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "RemoveEventHandler", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        ApplicationServices.\u003Cremove_InactivityTimeoutOccurring\u003Eo__SiteContainer12.\u003C\u003Ep__Site13.Target((CallSite) ApplicationServices.\u003Cremove_InactivityTimeoutOccurring\u003Eo__SiteContainer12.\u003C\u003Ep__Site13, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, "InactivityTimeoutOccurringEvent", value);
      }
    }

    public static event EventHandler<SessionEndedEventArgs> SessionEnded
    {
      add
      {
        if (ApplicationServices.\u003Cadd_SessionEnded\u003Eo__SiteContainer14.\u003C\u003Ep__Site15 == null)
          ApplicationServices.\u003Cadd_SessionEnded\u003Eo__SiteContainer14.\u003C\u003Ep__Site15 = CallSite<Action<CallSite, Type, object, string, EventHandler<SessionEndedEventArgs>>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "AddEventHandler", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        ApplicationServices.\u003Cadd_SessionEnded\u003Eo__SiteContainer14.\u003C\u003Ep__Site15.Target((CallSite) ApplicationServices.\u003Cadd_SessionEnded\u003Eo__SiteContainer14.\u003C\u003Ep__Site15, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, "SessionEndedEvent", value);
      }
      remove
      {
        if (ApplicationServices.\u003Cremove_SessionEnded\u003Eo__SiteContainer16.\u003C\u003Ep__Site17 == null)
          ApplicationServices.\u003Cremove_SessionEnded\u003Eo__SiteContainer16.\u003C\u003Ep__Site17 = CallSite<Action<CallSite, Type, object, string, EventHandler<SessionEndedEventArgs>>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "RemoveEventHandler", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        ApplicationServices.\u003Cremove_SessionEnded\u003Eo__SiteContainer16.\u003C\u003Ep__Site17.Target((CallSite) ApplicationServices.\u003Cremove_SessionEnded\u003Eo__SiteContainer16.\u003C\u003Ep__Site17, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, "SessionEndedEvent", value);
      }
    }

    public static event EventHandler<WindowAvailabilityChangedEventArgs> WindowInteractive
    {
      add
      {
        if (ApplicationServices.\u003Cadd_WindowInteractive\u003Eo__SiteContainer18.\u003C\u003Ep__Site19 == null)
          ApplicationServices.\u003Cadd_WindowInteractive\u003Eo__SiteContainer18.\u003C\u003Ep__Site19 = CallSite<Action<CallSite, Type, object, string, EventHandler<WindowAvailabilityChangedEventArgs>>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "AddEventHandler", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        ApplicationServices.\u003Cadd_WindowInteractive\u003Eo__SiteContainer18.\u003C\u003Ep__Site19.Target((CallSite) ApplicationServices.\u003Cadd_WindowInteractive\u003Eo__SiteContainer18.\u003C\u003Ep__Site19, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, "WindowInteractiveEvent", value);
      }
      remove
      {
        if (ApplicationServices.\u003Cremove_WindowInteractive\u003Eo__SiteContainer1a.\u003C\u003Ep__Site1b == null)
          ApplicationServices.\u003Cremove_WindowInteractive\u003Eo__SiteContainer1a.\u003C\u003Ep__Site1b = CallSite<Action<CallSite, Type, object, string, EventHandler<WindowAvailabilityChangedEventArgs>>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "RemoveEventHandler", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        ApplicationServices.\u003Cremove_WindowInteractive\u003Eo__SiteContainer1a.\u003C\u003Ep__Site1b.Target((CallSite) ApplicationServices.\u003Cremove_WindowInteractive\u003Eo__SiteContainer1a.\u003C\u003Ep__Site1b, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, "WindowInteractiveEvent", value);
      }
    }

    public static event EventHandler<WindowAvailabilityChangedEventArgs> WindowNoninteractive
    {
      add
      {
        if (ApplicationServices.\u003Cadd_WindowNoninteractive\u003Eo__SiteContainer1c.\u003C\u003Ep__Site1d == null)
          ApplicationServices.\u003Cadd_WindowNoninteractive\u003Eo__SiteContainer1c.\u003C\u003Ep__Site1d = CallSite<Action<CallSite, Type, object, string, EventHandler<WindowAvailabilityChangedEventArgs>>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "AddEventHandler", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        ApplicationServices.\u003Cadd_WindowNoninteractive\u003Eo__SiteContainer1c.\u003C\u003Ep__Site1d.Target((CallSite) ApplicationServices.\u003Cadd_WindowNoninteractive\u003Eo__SiteContainer1c.\u003C\u003Ep__Site1d, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, "WindowNoninteractiveEvent", value);
      }
      remove
      {
        if (ApplicationServices.\u003Cremove_WindowNoninteractive\u003Eo__SiteContainer1e.\u003C\u003Ep__Site1f == null)
          ApplicationServices.\u003Cremove_WindowNoninteractive\u003Eo__SiteContainer1e.\u003C\u003Ep__Site1f = CallSite<Action<CallSite, Type, object, string, EventHandler<WindowAvailabilityChangedEventArgs>>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "RemoveEventHandler", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        ApplicationServices.\u003Cremove_WindowNoninteractive\u003Eo__SiteContainer1e.\u003C\u003Ep__Site1f.Target((CallSite) ApplicationServices.\u003Cremove_WindowNoninteractive\u003Eo__SiteContainer1e.\u003C\u003Ep__Site1f, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, "WindowNoninteractiveEvent", value);
      }
    }

    public static event EventHandler<WindowAvailabilityChangedEventArgs> WindowUnavailable
    {
      add
      {
        if (ApplicationServices.\u003Cadd_WindowUnavailable\u003Eo__SiteContainer20.\u003C\u003Ep__Site21 == null)
          ApplicationServices.\u003Cadd_WindowUnavailable\u003Eo__SiteContainer20.\u003C\u003Ep__Site21 = CallSite<Action<CallSite, Type, object, string, EventHandler<WindowAvailabilityChangedEventArgs>>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "AddEventHandler", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        ApplicationServices.\u003Cadd_WindowUnavailable\u003Eo__SiteContainer20.\u003C\u003Ep__Site21.Target((CallSite) ApplicationServices.\u003Cadd_WindowUnavailable\u003Eo__SiteContainer20.\u003C\u003Ep__Site21, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, "WindowUnavailableEvent", value);
      }
      remove
      {
        if (ApplicationServices.\u003Cremove_WindowUnavailable\u003Eo__SiteContainer22.\u003C\u003Ep__Site23 == null)
          ApplicationServices.\u003Cremove_WindowUnavailable\u003Eo__SiteContainer22.\u003C\u003Ep__Site23 = CallSite<Action<CallSite, Type, object, string, EventHandler<WindowAvailabilityChangedEventArgs>>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "RemoveEventHandler", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[4]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
          }));
        ApplicationServices.\u003Cremove_WindowUnavailable\u003Eo__SiteContainer22.\u003C\u003Ep__Site23.Target((CallSite) ApplicationServices.\u003Cremove_WindowUnavailable\u003Eo__SiteContainer22.\u003C\u003Ep__Site23, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, "WindowUnavailableEvent", value);
      }
    }

    public static void SignalApplicationLoadComplete()
    {
      // ISSUE: reference to a compiler-generated field
      if (ApplicationServices.\u003CSignalApplicationLoadComplete\u003Eo__SiteContainer24.\u003C\u003Ep__Site25 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ApplicationServices.\u003CSignalApplicationLoadComplete\u003Eo__SiteContainer24.\u003C\u003Ep__Site25 = CallSite<Action<CallSite, Type, object, string, Type[], object[]>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InvokeMethod", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ApplicationServices.\u003CSignalApplicationLoadComplete\u003Eo__SiteContainer24.\u003C\u003Ep__Site25.Target((CallSite) ApplicationServices.\u003CSignalApplicationLoadComplete\u003Eo__SiteContainer24.\u003C\u003Ep__Site25, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, nameof (SignalApplicationLoadComplete), new Type[0], new object[0]);
    }

    public static void ActivateApplication(string uniqueName)
    {
      // ISSUE: reference to a compiler-generated field
      if (ApplicationServices.\u003CActivateApplication\u003Eo__SiteContainer26.\u003C\u003Ep__Site27 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ApplicationServices.\u003CActivateApplication\u003Eo__SiteContainer26.\u003C\u003Ep__Site27 = CallSite<Action<CallSite, Type, object, string, Type[], object[]>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InvokeMethod", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ApplicationServices.\u003CActivateApplication\u003Eo__SiteContainer26.\u003C\u003Ep__Site27.Target((CallSite) ApplicationServices.\u003CActivateApplication\u003Eo__SiteContainer26.\u003C\u003Ep__Site27, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, nameof (ActivateApplication), new Type[1]
      {
        typeof (string)
      }, new object[1]{ (object) uniqueName });
    }

    public static void ActivateApplication(
      string uniqueName,
      UserOrientation newSuggestedOrientation)
    {
      // ISSUE: reference to a compiler-generated field
      if (ApplicationServices.\u003CActivateApplication\u003Eo__SiteContainer28.\u003C\u003Ep__Site29 == null)
      {
        // ISSUE: reference to a compiler-generated field
        ApplicationServices.\u003CActivateApplication\u003Eo__SiteContainer28.\u003C\u003Ep__Site29 = CallSite<Action<CallSite, Type, object, string, Type[], object[]>>.Create(Binder.InvokeMember(CSharpBinderFlags.ResultDiscarded, "InvokeMethod", (IEnumerable<Type>) null, typeof (ApplicationServices), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[5]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.IsStaticType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType | CSharpArgumentInfoFlags.Constant, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ApplicationServices.\u003CActivateApplication\u003Eo__SiteContainer28.\u003C\u003Ep__Site29.Target((CallSite) ApplicationServices.\u003CActivateApplication\u003Eo__SiteContainer28.\u003C\u003Ep__Site29, typeof (ReflectionUtilities), ApplicationServices.InternalApplicationServicesType, nameof (ActivateApplication), new Type[2]
      {
        typeof (string),
        typeof (int)
      }, new object[2]
      {
        (object) uniqueName,
        (object) (int) newSuggestedOrientation
      });
    }

    private static string ApplicationServicesName
    {
      get
      {
        return ReflectionUtilities.ConstructAssemblyQualifiedName("Microsoft.Surface.Shell.ShellApi", "Microsoft.Surface.Shell.InternalApplicationServices");
      }
    }

    private static object InternalApplicationServicesType
    {
      get
      {
        if (!ApplicationServices.initialized)
        {
          ApplicationServices.initialized = true;
          ApplicationServices.internalApplicationServicesType = Type.GetType(ApplicationServices.ApplicationServicesName, false, false);
        }
        return (object) ApplicationServices.internalApplicationServicesType;
      }
    }
  }
}
