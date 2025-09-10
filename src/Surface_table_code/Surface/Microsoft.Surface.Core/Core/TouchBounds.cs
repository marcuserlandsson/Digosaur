// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.TouchBounds
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.Properties;
using System;
using System.Globalization;

#nullable disable
namespace Microsoft.Surface.Core
{
  public struct TouchBounds
  {
    private PointF topLeft;
    private SizeF size;

    internal TouchBounds(PointF topLeft, SizeF size)
    {
      this.topLeft = topLeft;
      this.size = size;
    }

    public float Height => this.size.Height;

    public float Width => this.size.Width;

    public float Top => this.topLeft.Y;

    public float Bottom => this.topLeft.Y + this.size.Height;

    public float Left => this.topLeft.X;

    public float Right => this.topLeft.X + this.size.Width;

    public static bool operator ==(TouchBounds b1, TouchBounds b2)
    {
      return b1.topLeft == b2.topLeft && b1.size == b2.size;
    }

    public static bool operator !=(TouchBounds b1, TouchBounds b2) => !(b1 == b2);

    public override bool Equals(object obj)
    {
      return obj is TouchBounds touchBounds && touchBounds == this;
    }

    public override int GetHashCode() => base.GetHashCode();

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TouchBoundsToStringFormat, (object) this.Left, (object) this.Top, (object) this.Right, (object) this.Bottom, (object) this.Width, (object) this.Height);
    }
  }
}
