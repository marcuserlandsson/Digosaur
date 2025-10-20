// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Constants
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

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

    internal static class Vision
    {
      internal const int HiResTagDataSize = 18;
      internal const string InputConnectionServerGuid = "E99BEB34-B06F-4a0f-B9E8-9D6B74936A0E";
      internal const string InputConnectionServerName = "Microsoft.Surface.Input.InputServer.InputConnection.InputConnectionServer";
    }

    internal static class Shell
    {
      public const string SurfaceApplicationEventWaitHandlePrefix = "Global\\Microsoft.Surface.Core.IsSurfaceApp:";

      public static int OutOfOrderUserDismissedExitCode => 100;
    }
  }
}
