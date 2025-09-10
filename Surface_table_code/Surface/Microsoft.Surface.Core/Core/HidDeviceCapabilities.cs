// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.HidDeviceCapabilities
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.HidSupport;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal class HidDeviceCapabilities : DeviceCapabilities
  {
    private float tiltAngle;
    private bool hasCachedTiltAngle;
    private readonly Rectangle displayBounds;
    private readonly SizeF physicalSize;
    private readonly HidDevice hidDevice;

    public HidDeviceCapabilities(Rectangle displayBounds, SizeF physicalSize, HidDevice hidDevice)
    {
      this.displayBounds = displayBounds;
      this.physicalSize = physicalSize;
      this.hidDevice = hidDevice;
    }

    public override Rectangle DisplayBounds => this.displayBounds;

    public override SizeF PhysicalSize => this.physicalSize;

    public override int MaximumTouchesSupported
    {
      get => this.hidDevice != null ? (int) this.hidDevice.MaxContacts : 0;
    }

    public override bool IsFingerRecognitionSupported
    {
      get => this.hidDevice != null && (this.hidDevice.SupportedContactTypes & 2) != 0;
    }

    public override bool IsTagRecognitionSupported
    {
      get => this.hidDevice != null && (this.hidDevice.SupportedContactTypes & 4) != 0;
    }

    public override bool IsTouchGeometrySupported
    {
      get => this.hidDevice != null && this.hidDevice.ContactGeometrySupported;
    }

    public override bool IsTouchOrientationSupported
    {
      get => this.hidDevice != null && this.hidDevice.OrientationSupported;
    }

    public override bool IsTouchBoundsSupported
    {
      get
      {
        return this.hidDevice != null && this.hidDevice.IsContactWidthSupported && this.hidDevice.IsContactHeightSupported;
      }
    }

    public override bool IsTiltSupported => this.hidDevice != null && this.hidDevice.TiltSupported;

    public override bool IsImageTypeSupported(ImageType imageType)
    {
      return this.hidDevice != null && imageType == ImageType.Normalized && (this.hidDevice.SupportedImageFormats & 2) != 0;
    }

    public override float TiltAngle
    {
      get
      {
        return this.hidDevice != null && this.hidDevice.TiltSupported && this.hasCachedTiltAngle ? this.tiltAngle : 1.57079637f;
      }
      protected set
      {
        this.tiltAngle = value;
        this.hasCachedTiltAngle = true;
      }
    }
  }
}
