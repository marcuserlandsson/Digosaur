// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.RawInputEvent
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;

#nullable disable
namespace Microsoft.Surface.Core.RawInput
{
  internal class RawInputEvent
  {
    private SurfaceEventId eventId;
    private IntPtr targetHwnd;
    private readonly object properties;

    public RawInputEvent(SurfaceEventId eventId, object properties)
    {
      this.eventId = eventId;
      this.targetHwnd = IntPtr.Zero;
      this.properties = properties;
    }

    public RawInputEvent(SurfaceEventId eventId, IntPtr target, object properties)
    {
      if (properties == null)
        throw SurfaceCoreExceptions.ArgumentNullException(nameof (properties));
      this.eventId = eventId;
      this.targetHwnd = target;
      this.properties = properties;
    }

    public SurfaceEventId EventId
    {
      get => this.eventId;
      set => this.eventId = value;
    }

    public IntPtr TargetHwnd
    {
      get => this.targetHwnd;
      set => this.targetHwnd = value;
    }

    public object Properties => this.properties;
  }
}
