// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.InteractiveSurface
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

#nullable disable
namespace Microsoft.Surface.Core
{
  public static class InteractiveSurface
  {
    private static InteractiveSurfaceDevice primarySurfaceDevice = InteractiveSurface.GetIsInputSurfaceAvailable() ? new InteractiveSurfaceDevice() : (InteractiveSurfaceDevice) null;

    private static bool GetIsInputSurfaceAvailable()
    {
      return ContextMap.Instance.AreDeviceCapabilitiesAvailable();
    }

    public static InteractiveSurfaceDevice PrimarySurfaceDevice
    {
      get => InteractiveSurface.primarySurfaceDevice;
      internal set => InteractiveSurface.primarySurfaceDevice = value;
    }
  }
}
