// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.TouchPoint
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.Properties;
using Microsoft.Surface.Core.RawInput;
using System;
using System.Globalization;

#nullable disable
namespace Microsoft.Surface.Core
{
  public class TouchPoint
  {
    private TouchProperties properties;

    internal TouchProperties TouchProperties => this.properties;

    internal TouchPoint(TouchProperties properties)
    {
      this.properties = properties != null ? properties : throw SurfaceCoreExceptions.ArgumentNullException(nameof (properties));
    }

    internal TouchPoint Clone() => new TouchPoint(this.properties);

    internal void SetPropertiesFromTouchPoint(TouchPoint touchPoint)
    {
      this.properties = touchPoint.properties;
    }

    public int Id => this.properties.Id;

    public float X => this.properties.Position.X;

    public float Y => this.properties.Position.Y;

    public float CenterX => this.properties.CenterPosition.X;

    public float CenterY => this.properties.CenterPosition.Y;

    public bool IsFingerRecognized
    {
      get => (this.properties.RecognizedTypes & TouchTypes.Finger) != (TouchTypes) 0;
    }

    public TagData Tag => this.properties.Tag;

    public bool IsTagRecognized => this.Tag != TagData.None;

    public float Orientation => this.properties.Orientation;

    public TouchBounds Bounds => this.properties.Bounds;

    public float MajorAxis => this.properties.MajorAxis;

    public float MinorAxis => this.properties.MinorAxis;

    public float PhysicalArea => this.properties.PhysicalArea;

    public long FrameTimestamp => this.properties.Timestamp;

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TouchPointToStringFormat, (object) this.Id, (object) this.X, (object) this.Y, (object) this.CenterX, (object) this.CenterY, (object) (this.IsTagRecognized ? this.Tag.ToString() : (this.IsFingerRecognized ? "finger" : "blob")));
    }
  }
}
