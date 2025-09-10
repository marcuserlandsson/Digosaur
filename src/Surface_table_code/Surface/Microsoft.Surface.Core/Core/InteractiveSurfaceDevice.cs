// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.InteractiveSurfaceDevice
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Surface.Core.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

#nullable disable
namespace Microsoft.Surface.Core
{
  public class InteractiveSurfaceDevice
  {
    private const int TiltThresholdMin = 0;
    private const int TiltThresholdMax = 45;
    private const int TiltThresholdDefault = 20;
    private SettingsProvider<int> horizontalSettingsProvider;
    private SettingsProvider<int> verticalSettingsProvider;
    private PointF physicalScale = new PointF(float.NaN, float.NaN);

    private static SettingsProvider<T> InitializeConfigurationProperty<T>(object property)
    {
      // ISSUE: reference to a compiler-generated field
      if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site1 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site1 = CallSite<Func<CallSite, object, bool>>.Create(Binder.UnaryOperation(CSharpBinderFlags.None, ExpressionType.IsTrue, typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, bool> target1 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site1.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, bool>> pSite1 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site1;
      // ISSUE: reference to a compiler-generated field
      if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site2 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site2 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.NotEqual, typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.Constant, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site2.Target((CallSite) InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site2, property, (object) null);
      return target1((CallSite) pSite1, obj1) ? new SettingsProvider<T>((Action<EventHandler>) (e =>
      {
        // ISSUE: reference to a compiler-generated field
        if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site3 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site3 = CallSite<Func<CallSite, object, bool>>.Create(Binder.IsEvent(CSharpBinderFlags.None, "ValueChanged", typeof (InteractiveSurfaceDevice)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site3.Target((CallSite) InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site3, property))
        {
          // ISSUE: reference to a compiler-generated field
          if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site6 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site6 = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.ValueFromCompoundAssignment, "ValueChanged", typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object> target2 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site6.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object>> pSite6 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site6;
          object obj2 = property;
          // ISSUE: reference to a compiler-generated field
          if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site5 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site5 = CallSite<Func<CallSite, object, EventHandler, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.AddAssign, typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, EventHandler, object> target3 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site5.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, EventHandler, object>> pSite5 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site5;
          // ISSUE: reference to a compiler-generated field
          if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site7 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site7 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ValueChanged", typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj3 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site7.Target((CallSite) InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site7, property);
          EventHandler eventHandler = e;
          object obj4 = target3((CallSite) pSite5, obj3, eventHandler);
          object obj5 = target2((CallSite) pSite6, obj2, obj4);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site4 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site4 = CallSite<Func<CallSite, object, EventHandler, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSpecialName | CSharpBinderFlags.ResultDiscarded, "add_ValueChanged", (IEnumerable<Type>) null, typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj6 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site4.Target((CallSite) InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site4, property, e);
        }
      }), (Action<EventHandler>) (e =>
      {
        // ISSUE: reference to a compiler-generated field
        if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site8 == null)
        {
          // ISSUE: reference to a compiler-generated field
          InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site8 = CallSite<Func<CallSite, object, bool>>.Create(Binder.IsEvent(CSharpBinderFlags.None, "ValueChanged", typeof (InteractiveSurfaceDevice)));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site8.Target((CallSite) InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site8, property))
        {
          // ISSUE: reference to a compiler-generated field
          if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Siteb == null)
          {
            // ISSUE: reference to a compiler-generated field
            InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Siteb = CallSite<Func<CallSite, object, object, object>>.Create(Binder.SetMember(CSharpBinderFlags.ValueFromCompoundAssignment, "ValueChanged", typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, object, object> target4 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Siteb.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, object, object>> pSiteb = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Siteb;
          object obj7 = property;
          // ISSUE: reference to a compiler-generated field
          if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sitea == null)
          {
            // ISSUE: reference to a compiler-generated field
            InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sitea = CallSite<Func<CallSite, object, EventHandler, object>>.Create(Binder.BinaryOperation(CSharpBinderFlags.None, ExpressionType.SubtractAssign, typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          Func<CallSite, object, EventHandler, object> target5 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sitea.Target;
          // ISSUE: reference to a compiler-generated field
          CallSite<Func<CallSite, object, EventHandler, object>> pSitea = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sitea;
          // ISSUE: reference to a compiler-generated field
          if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sitec == null)
          {
            // ISSUE: reference to a compiler-generated field
            InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sitec = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "ValueChanged", typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj8 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sitec.Target((CallSite) InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sitec, property);
          EventHandler eventHandler = e;
          object obj9 = target5((CallSite) pSitea, obj8, eventHandler);
          object obj10 = target4((CallSite) pSiteb, obj7, obj9);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site9 == null)
          {
            // ISSUE: reference to a compiler-generated field
            InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site9 = CallSite<Func<CallSite, object, EventHandler, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSpecialName | CSharpBinderFlags.ResultDiscarded, "remove_ValueChanged", (IEnumerable<Type>) null, typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
            {
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
              CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
            }));
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          object obj11 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site9.Target((CallSite) InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Site9, property, e);
        }
      }), (Func<T>) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sited == null)
        {
          // ISSUE: reference to a compiler-generated field
          InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sited = CallSite<Func<CallSite, object, T>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (T), typeof (InteractiveSurfaceDevice)));
        }
        // ISSUE: reference to a compiler-generated field
        Func<CallSite, object, T> target6 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sited.Target;
        // ISSUE: reference to a compiler-generated field
        CallSite<Func<CallSite, object, T>> pSited = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sited;
        // ISSUE: reference to a compiler-generated field
        if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sitee == null)
        {
          // ISSUE: reference to a compiler-generated field
          InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sitee = CallSite<Func<CallSite, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.None, "GetValue", (IEnumerable<Type>) null, typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[1]
          {
            CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
          }));
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        object obj12 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sitee.Target((CallSite) InteractiveSurfaceDevice.\u003CInitializeConfigurationProperty\u003Eo__SiteContainer0<T>.\u003C\u003Ep__Sitee, property);
        return target6((CallSite) pSited, obj12);
      })) : (SettingsProvider<T>) null;
    }

    private static int GetTiltThreshold(SettingsProvider<int> settingsProvider)
    {
      int tiltThreshold = 20;
      if (settingsProvider != null && settingsProvider.Value >= 0 && settingsProvider.Value <= 45)
        tiltThreshold = settingsProvider.Value;
      return tiltThreshold;
    }

    private int TiltHorizontalThreshold
    {
      get => InteractiveSurfaceDevice.GetTiltThreshold(this.horizontalSettingsProvider);
    }

    private int TiltVerticalThreshold
    {
      get => InteractiveSurfaceDevice.GetTiltThreshold(this.verticalSettingsProvider);
    }

    private static DeviceCapabilities DeviceCapabilities
    {
      get => ContextMap.Instance.InputProvider.PrimaryDeviceCapabilities;
    }

    internal InteractiveSurfaceDevice()
    {
      this.HandleDeviceEvents();
      this.InitializeConfigurationProperties();
    }

    private void InitializeConfigurationProperties()
    {
      // ISSUE: reference to a compiler-generated field
      if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site18 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site18 = CallSite<Func<CallSite, object, SettingsProvider<int>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (SettingsProvider<int>), typeof (InteractiveSurfaceDevice)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, SettingsProvider<int>> target1 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site18.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, SettingsProvider<int>>> pSite18 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site18;
      // ISSUE: reference to a compiler-generated field
      if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site19 == null)
      {
        // ISSUE: reference to a compiler-generated field
        InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site19 = CallSite<Func<CallSite, InteractiveSurfaceDevice, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName, "InitializeConfigurationProperty", (IEnumerable<Type>) new Type[1]
        {
          typeof (int)
        }, typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj1 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site19.Target((CallSite) InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site19, this, PlatformConfigurationPropertiesProxy.TiltHorizontalThreshold);
      this.horizontalSettingsProvider = target1((CallSite) pSite18, obj1);
      if (this.horizontalSettingsProvider != null)
        this.horizontalSettingsProvider.Changed += new EventHandler(this.OnTiltConfigChanged);
      // ISSUE: reference to a compiler-generated field
      if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site1a == null)
      {
        // ISSUE: reference to a compiler-generated field
        InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site1a = CallSite<Func<CallSite, object, SettingsProvider<int>>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof (SettingsProvider<int>), typeof (InteractiveSurfaceDevice)));
      }
      // ISSUE: reference to a compiler-generated field
      Func<CallSite, object, SettingsProvider<int>> target2 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site1a.Target;
      // ISSUE: reference to a compiler-generated field
      CallSite<Func<CallSite, object, SettingsProvider<int>>> pSite1a = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site1a;
      // ISSUE: reference to a compiler-generated field
      if (InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site1b == null)
      {
        // ISSUE: reference to a compiler-generated field
        InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site1b = CallSite<Func<CallSite, InteractiveSurfaceDevice, object, object>>.Create(Binder.InvokeMember(CSharpBinderFlags.InvokeSimpleName, "InitializeConfigurationProperty", (IEnumerable<Type>) new Type[1]
        {
          typeof (int)
        }, typeof (InteractiveSurfaceDevice), (IEnumerable<CSharpArgumentInfo>) new CSharpArgumentInfo[2]
        {
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
          CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null)
        }));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      object obj2 = InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site1b.Target((CallSite) InteractiveSurfaceDevice.\u003CInitializeConfigurationProperties\u003Eo__SiteContainer17.\u003C\u003Ep__Site1b, this, PlatformConfigurationPropertiesProxy.TiltVerticalThreshold);
      this.verticalSettingsProvider = target2((CallSite) pSite1a, obj2);
      if (this.verticalSettingsProvider == null)
        return;
      this.verticalSettingsProvider.Changed += new EventHandler(this.OnTiltConfigChanged);
    }

    private void HandleDeviceEvents()
    {
      InteractiveSurfaceDevice.DeviceCapabilities.DeviceInvalidated += new EventHandler(this.OnDeviceInvalidated);
      InteractiveSurfaceDevice.DeviceCapabilities.TiltChanged += new EventHandler<TiltChangedEventArgs>(this.OnTiltChanged);
    }

    private void OnTiltConfigChanged(object sender, EventArgs e)
    {
      this.RaiseTiltChangedEvent((object) this, EventArgs.Empty);
    }

    private void OnTiltChanged(object sender, EventArgs e)
    {
      this.RaiseTiltChangedEvent((object) this, EventArgs.Empty);
    }

    private void OnDeviceInvalidated(object sender, EventArgs e)
    {
      if (sender is DeviceCapabilities deviceCapabilities)
      {
        deviceCapabilities.DeviceInvalidated -= new EventHandler(this.OnDeviceInvalidated);
        deviceCapabilities.TiltChanged -= new EventHandler<TiltChangedEventArgs>(this.OnTiltChanged);
      }
      this.HandleDeviceEvents();
      this.RaiseDeviceChangedEvent((object) this, EventArgs.Empty);
      this.RaiseTiltChangedEvent((object) this, EventArgs.Empty);
    }

    private Rectangle GetApplicationWorkingArea()
    {
      System.Drawing.Rectangle rect = new System.Drawing.Rectangle()
      {
        X = InteractiveSurfaceDevice.DeviceCapabilities.DisplayBounds.Left,
        Y = InteractiveSurfaceDevice.DeviceCapabilities.DisplayBounds.Top,
        Width = InteractiveSurfaceDevice.DeviceCapabilities.DisplayBounds.Width,
        Height = InteractiveSurfaceDevice.DeviceCapabilities.DisplayBounds.Height
      };
      System.Drawing.Rectangle workingArea = Screen.GetWorkingArea(rect);
      workingArea.Intersect(rect);
      return new Rectangle(workingArea.Top, workingArea.Left, workingArea.Bottom, workingArea.Right);
    }

    internal virtual int GetLeft()
    {
      return InteractiveSurfaceDevice.DeviceCapabilities.DisplayBounds.Left;
    }

    internal virtual int GetWorkingAreaLeft() => this.GetApplicationWorkingArea().Left;

    public int Left => this.GetLeft();

    public int WorkingAreaLeft => this.GetWorkingAreaLeft();

    internal virtual int GetRight()
    {
      return InteractiveSurfaceDevice.DeviceCapabilities.DisplayBounds.Right;
    }

    internal virtual int GetWorkingAreaRight() => this.GetApplicationWorkingArea().Right;

    public int Right => this.GetRight();

    public int WorkingAreaRight => this.GetWorkingAreaRight();

    internal virtual int GetTop() => InteractiveSurfaceDevice.DeviceCapabilities.DisplayBounds.Top;

    internal virtual int GetWorkingAreaTop() => this.GetApplicationWorkingArea().Top;

    public int Top => this.GetTop();

    public int WorkingAreaTop => this.GetWorkingAreaTop();

    internal virtual int GetBottom()
    {
      return InteractiveSurfaceDevice.DeviceCapabilities.DisplayBounds.Bottom;
    }

    internal virtual int GetWorkingAreaBottom() => this.GetApplicationWorkingArea().Bottom;

    public int Bottom => this.GetBottom();

    public int WorkingAreaBottom => this.GetWorkingAreaBottom();

    internal virtual int GetWidth()
    {
      return InteractiveSurfaceDevice.DeviceCapabilities.DisplayBounds.Width;
    }

    internal virtual int GetWorkingAreaWidth() => this.GetApplicationWorkingArea().Width;

    public int Width => this.GetWidth();

    public int WorkingAreaWidth => this.GetWorkingAreaWidth();

    internal virtual int GetHeight()
    {
      return InteractiveSurfaceDevice.DeviceCapabilities.DisplayBounds.Height;
    }

    internal virtual int GetWorkingAreaHeight() => this.GetApplicationWorkingArea().Height;

    public int Height => this.GetHeight();

    public int WorkingAreaHeight => this.GetWorkingAreaHeight();

    public float PhysicalDpiX
    {
      get
      {
        if (!float.IsNaN(this.DisplaySizeInInches.Width))
          return (float) this.Width / this.DisplaySizeInInches.Width;
        float physicalDpiX = 96f;
        IntPtr hDC = IntPtr.Zero;
        try
        {
          hDC = NativeMethods.CreateDC("DISPLAY");
          if (hDC != IntPtr.Zero)
            physicalDpiX = (float) NativeMethods.GetDeviceCaps(hDC, 88);
        }
        finally
        {
          if (hDC != IntPtr.Zero)
            NativeMethods.DeleteDC(hDC);
        }
        return physicalDpiX;
      }
    }

    public float PhysicalDpiY
    {
      get
      {
        if (!float.IsNaN(this.DisplaySizeInInches.Width))
          return (float) this.Height / this.DisplaySizeInInches.Height;
        float physicalDpiY = 96f;
        IntPtr hDC = IntPtr.Zero;
        try
        {
          hDC = NativeMethods.CreateDC("DISPLAY");
          if (hDC != IntPtr.Zero)
            physicalDpiY = (float) NativeMethods.GetDeviceCaps(hDC, 90);
        }
        finally
        {
          if (hDC != IntPtr.Zero)
            NativeMethods.DeleteDC(hDC);
        }
        return physicalDpiY;
      }
    }

    public float PhysicalScaleX
    {
      get
      {
        if (float.IsNaN(this.physicalScale.X))
          this.CalculatePhysicalScale();
        return this.physicalScale.X;
      }
    }

    public float PhysicalScaleY
    {
      get
      {
        if (float.IsNaN(this.physicalScale.Y))
          this.CalculatePhysicalScale();
        return this.physicalScale.Y;
      }
    }

    private void CalculatePhysicalScale()
    {
      float num1 = 96f;
      float num2 = 96f;
      IntPtr hDC = IntPtr.Zero;
      try
      {
        hDC = NativeMethods.CreateDC("DISPLAY");
        if (hDC != IntPtr.Zero)
        {
          num1 = (float) NativeMethods.GetDeviceCaps(hDC, 88);
          num2 = (float) NativeMethods.GetDeviceCaps(hDC, 90);
        }
      }
      finally
      {
        if (hDC != IntPtr.Zero)
          NativeMethods.DeleteDC(hDC);
      }
      this.physicalScale.X = this.PhysicalDpiX / num1;
      this.physicalScale.Y = this.PhysicalDpiY / num2;
    }

    internal SizeF DisplaySizeInInches => InteractiveSurfaceDevice.DeviceCapabilities.PhysicalSize;

    public Tilt Tilt
    {
      get
      {
        float num1 = (float) ((double) this.TiltAngle * 180.0 / Math.PI) % 360f;
        if ((double) num1 > 180.0)
          num1 -= 360f;
        if ((double) num1 <= -180.0)
          num1 += 360f;
        float num2 = (float) Math.Round((double) num1, MidpointRounding.AwayFromZero);
        float num3 = (float) -this.TiltHorizontalThreshold;
        float horizontalThreshold = (float) this.TiltHorizontalThreshold;
        if ((double) num2 >= (double) num3 && (double) num2 <= (double) horizontalThreshold)
          return Tilt.Horizontal;
        float num4 = 90f - (float) this.TiltVerticalThreshold;
        float num5 = 90f + (float) this.TiltVerticalThreshold;
        return (double) num2 >= (double) num4 && (double) num2 <= (double) num5 ? Tilt.Vertical : Tilt.Tilted;
      }
    }

    public float TiltAngle => InteractiveSurfaceDevice.DeviceCapabilities.TiltAngle;

    public bool IsFingerRecognitionSupported
    {
      get => InteractiveSurfaceDevice.DeviceCapabilities.IsFingerRecognitionSupported;
    }

    public bool IsTagRecognitionSupported
    {
      get => InteractiveSurfaceDevice.DeviceCapabilities.IsTagRecognitionSupported;
    }

    public bool IsTouchOrientationSupported
    {
      get => InteractiveSurfaceDevice.DeviceCapabilities.IsTouchOrientationSupported;
    }

    public bool IsTouchBoundsSupported
    {
      get => InteractiveSurfaceDevice.DeviceCapabilities.IsTouchBoundsSupported;
    }

    public bool IsTiltSupported => InteractiveSurfaceDevice.DeviceCapabilities.IsTiltSupported;

    public bool IsImageTypeSupported(ImageType imageType)
    {
      return InteractiveSurfaceDevice.DeviceCapabilities.IsImageTypeSupported(imageType);
    }

    public int MaximumTouchesSupported
    {
      get => InteractiveSurfaceDevice.DeviceCapabilities.MaximumTouchesSupported;
    }

    public event EventHandler DeviceChanged;

    public event EventHandler TiltChanged;

    private void RaiseTiltChangedEvent(object source, EventArgs e)
    {
      EventHandler tiltChanged = this.TiltChanged;
      if (tiltChanged == null)
        return;
      tiltChanged(source, e);
    }

    private void RaiseDeviceChangedEvent(object source, EventArgs e)
    {
      EventHandler deviceChanged = this.DeviceChanged;
      if (deviceChanged == null)
        return;
      deviceChanged(source, e);
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.InteractiveSurfaceToStringFormat, (object) this.Left, (object) this.Top, (object) this.Right, (object) this.Bottom, (object) this.Width, (object) this.Height);
    }
  }
}
