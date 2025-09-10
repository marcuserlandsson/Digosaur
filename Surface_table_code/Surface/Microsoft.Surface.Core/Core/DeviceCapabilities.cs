// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.DeviceCapabilities
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal abstract class DeviceCapabilities
  {
    public abstract Rectangle DisplayBounds { get; }

    public abstract SizeF PhysicalSize { get; }

    public abstract int MaximumTouchesSupported { get; }

    public abstract bool IsFingerRecognitionSupported { get; }

    public abstract bool IsTagRecognitionSupported { get; }

    public abstract bool IsTouchGeometrySupported { get; }

    public abstract bool IsTouchOrientationSupported { get; }

    public abstract bool IsTouchBoundsSupported { get; }

    public abstract bool IsTiltSupported { get; }

    public abstract bool IsImageTypeSupported(ImageType imageType);

    public abstract float TiltAngle { get; protected set; }

    public event EventHandler DeviceInvalidated;

    public event EventHandler<TiltChangedEventArgs> TiltChanged;

    protected internal void RaiseDeviceInvalidatedEvent(object source, EventArgs e)
    {
      EventHandler deviceInvalidated = this.DeviceInvalidated;
      if (deviceInvalidated == null)
        return;
      deviceInvalidated(source, e);
    }

    protected internal void RaiseTiltChangedEvent(object source, TiltChangedEventArgs e)
    {
      this.TiltAngle = e.TiltAngle;
      EventHandler<TiltChangedEventArgs> tiltChanged = this.TiltChanged;
      if (tiltChanged == null)
        return;
      tiltChanged(source, e);
    }
  }
}
