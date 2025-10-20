// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.TouchProperties
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;

#nullable disable
namespace Microsoft.Surface.Core.RawInput
{
  internal class TouchProperties
  {
    private IntPtr hwndTarget;

    public TouchProperties(IntPtr hwnd) => this.hwndTarget = hwnd;

    public virtual TouchProperties Clone()
    {
      return new TouchProperties(this.hwndTarget)
      {
        Id = this.Id,
        Position = this.Position,
        CenterPosition = this.CenterPosition,
        RecognizedTypes = this.RecognizedTypes,
        Orientation = this.Orientation,
        Tag = this.Tag,
        Bounds = this.Bounds,
        MajorAxis = this.MajorAxis,
        MinorAxis = this.MinorAxis,
        PhysicalArea = this.PhysicalArea,
        Timestamp = this.Timestamp
      };
    }

    public virtual int Id { get; set; }

    public virtual PointF Position { get; set; }

    public virtual PointF CenterPosition { get; set; }

    public virtual TouchTypes RecognizedTypes { get; set; }

    public virtual float Orientation { get; set; }

    public virtual TagData Tag { get; set; }

    public virtual TouchBounds Bounds { get; set; }

    public virtual float MajorAxis { get; set; }

    public virtual float MinorAxis { get; set; }

    public virtual float PhysicalArea { get; set; }

    public virtual long Timestamp { get; set; }

    public virtual IntPtr HwndTarget
    {
      get => this.hwndTarget;
      set => this.hwndTarget = value;
    }
  }
}
