// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.PointF
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.Properties;
using System;
using System.Globalization;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal struct PointF
  {
    private float x;
    private float y;

    public PointF(float x, float y)
    {
      this.x = x;
      this.y = y;
    }

    public static explicit operator SizeF(PointF point) => new SizeF(point.x, point.y);

    public static explicit operator VectorF(PointF point) => new VectorF(point.x, point.y);

    public static bool operator !=(PointF left, PointF right)
    {
      return (double) left.X != (double) right.X || (double) left.Y != (double) right.Y;
    }

    public static bool operator ==(PointF left, PointF right)
    {
      return (double) left.X == (double) right.X && (double) left.Y == (double) right.Y;
    }

    public static PointF operator +(PointF pt, VectorF offset)
    {
      return new PointF(pt.X + offset.X, pt.Y + offset.Y);
    }

    public static VectorF operator -(PointF point1, PointF point2)
    {
      return new VectorF(point1.x - point2.x, point1.y - point2.y);
    }

    public static PointF operator -(PointF point, VectorF vector)
    {
      return new PointF(point.x - vector.X, point.y - vector.Y);
    }

    public float X
    {
      get => this.x;
      set => this.x = value;
    }

    public float Y
    {
      get => this.y;
      set => this.y = value;
    }

    public override bool Equals(object obj) => obj is PointF pointF && pointF == this;

    public override int GetHashCode() => this.x.GetHashCode() ^ this.y.GetHashCode();

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.PointFToStringFormat, new object[2]
      {
        (object) this.X,
        (object) this.Y
      });
    }
  }
}
