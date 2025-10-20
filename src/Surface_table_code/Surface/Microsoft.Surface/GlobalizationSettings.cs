// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.GlobalizationSettings
// Assembly: Microsoft.Surface, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 3075CDD2-EF12-4C05-803F-ABBE1D5E74B0
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.dll

using System;
using System.Globalization;
using System.Threading;

#nullable disable
namespace Microsoft.Surface
{
  public static class GlobalizationSettings
  {
    private static Type internalGlobalizationSettingsType;
    private static bool initialized;

    public static CultureInfo CurrentCulture
    {
      get
      {
        object obj;
        return ReflectionUtilities.TryGetProperty(GlobalizationSettings.InternalGlobalizationSettingsType, nameof (CurrentCulture), out obj) ? (CultureInfo) obj : Thread.CurrentThread.CurrentCulture;
      }
    }

    public static CultureInfo CurrentUICulture
    {
      get
      {
        object obj;
        return ReflectionUtilities.TryGetProperty(GlobalizationSettings.InternalGlobalizationSettingsType, nameof (CurrentUICulture), out obj) ? (CultureInfo) obj : Thread.CurrentThread.CurrentUICulture;
      }
    }

    public static void ApplyToCurrentThread()
    {
      GlobalizationSettings.ApplyToThread(Thread.CurrentThread);
    }

    public static void ApplyToThread(Thread thread)
    {
      if (thread == null)
        throw new ArgumentNullException(nameof (thread));
      thread.CurrentUICulture = GlobalizationSettings.CurrentUICulture;
      thread.CurrentCulture = GlobalizationSettings.CurrentCulture;
    }

    private static string GlobalizationSettingsName
    {
      get
      {
        return ReflectionUtilities.ConstructAssemblyQualifiedName("Microsoft.Surface.Shell.ShellApi", "Microsoft.Surface.Shell.InternalGlobalizationSettings");
      }
    }

    private static Type InternalGlobalizationSettingsType
    {
      get
      {
        if (!GlobalizationSettings.initialized)
        {
          GlobalizationSettings.initialized = true;
          GlobalizationSettings.internalGlobalizationSettingsType = Type.GetType(GlobalizationSettings.GlobalizationSettingsName, false, false);
        }
        return GlobalizationSettings.internalGlobalizationSettingsType;
      }
    }
  }
}
