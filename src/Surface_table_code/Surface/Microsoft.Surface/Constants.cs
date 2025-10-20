// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Constants
// Assembly: Microsoft.Surface, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 3075CDD2-EF12-4C05-803F-ABBE1D5E74B0
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.dll

using Microsoft.Win32;

#nullable disable
namespace Microsoft.Surface
{
  internal static class Constants
  {
    internal static class Registry
    {
      internal const string LocalMachineKeyPath = "HKEY_LOCAL_MACHINE\\";
      internal const RegistryView SurfaceRegistryView = RegistryView.Registry32;
      internal const string SurfaceRootKeyPath = "SOFTWARE\\Microsoft\\Surface\\v2.0";
    }
  }
}
