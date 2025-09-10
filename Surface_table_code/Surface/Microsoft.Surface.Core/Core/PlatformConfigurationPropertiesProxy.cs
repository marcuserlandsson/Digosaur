// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.PlatformConfigurationPropertiesProxy
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal static class PlatformConfigurationPropertiesProxy
  {
    private static Type platformConfigurationPropertiesType;
    private static bool pcptInitialized;

    public static object TiltHorizontalThreshold
    {
      get
      {
        object obj;
        return ReflectionUtilities.TryGetField(PlatformConfigurationPropertiesProxy.PlatformConfigurationPropertiesType, nameof (TiltHorizontalThreshold), out obj) ? obj : (object) null;
      }
    }

    public static object TiltVerticalThreshold
    {
      get
      {
        object obj;
        return ReflectionUtilities.TryGetField(PlatformConfigurationPropertiesProxy.PlatformConfigurationPropertiesType, nameof (TiltVerticalThreshold), out obj) ? obj : (object) null;
      }
    }

    private static string PlatformConfigurationPropertiesName
    {
      get
      {
        return ReflectionUtilities.ConstructAssemblyQualifiedName("Microsoft.Surface.Configuration", "Microsoft.Surface.Configuration.PlatformConfigurationProperties");
      }
    }

    private static Type PlatformConfigurationPropertiesType
    {
      get
      {
        if (!PlatformConfigurationPropertiesProxy.pcptInitialized)
        {
          PlatformConfigurationPropertiesProxy.pcptInitialized = true;
          PlatformConfigurationPropertiesProxy.platformConfigurationPropertiesType = Type.GetType(PlatformConfigurationPropertiesProxy.PlatformConfigurationPropertiesName, false, false);
        }
        return PlatformConfigurationPropertiesProxy.platformConfigurationPropertiesType;
      }
    }
  }
}
