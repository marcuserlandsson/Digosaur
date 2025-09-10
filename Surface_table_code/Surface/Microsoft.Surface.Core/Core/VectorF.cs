// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.VectorF
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.Properties;
using System;
using System.Globalization;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal struct VectorF
  {
    private float x;
    private float y;

    public VectorF(float x, float y)
    {
      this.x = x;
      this.y = y;
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

    public static explicit operator PointF(VectorF vector) => new PointF(vector.x, vector.y);

    public static explicit operator SizeF(VectorF vector) => new SizeF(vector.x, vector.y);

    public static VectorF operator -(VectorF vector) => new VectorF(-vector.x, -vector.y);

    public void Negate()
    {
      this.x = -this.x;
      this.y = -this.y;
    }

    public static bool operator !=(VectorF vector1, VectorF vector2)
    {
      return (double) vector1.x != (double) vector2.x || (double) vector1.y != (double) vector2.y;
    }

    public static bool operator ==(VectorF vector1, VectorF vector2)
    {
      return (double) vector1.x == (double) vector2.x && (double) vector1.y == (double) vector2.y;
    }

    public static bool Equals(VectorF vector1, VectorF vector2) => vector1 == vector2;

    public override bool Equals(object o) => o is VectorF vectorF && vectorF == this;

    public bool Equals(VectorF value) => value == this;

    public static VectorF operator +(VectorF vector1, VectorF vector2)
    {
      return new VectorF(vector1.x + vector2.x, vector1.y + vector2.y);
    }

    public static VectorF Add(VectorF vector1, VectorF vector2) => vector1 + vector2;

    public static PointF operator +(VectorF vector, PointF point)
    {
      return new PointF(point.X + vector.x, point.Y + vector.y);
    }

    public static PointF Add(VectorF vector, PointF point) => vector + point;

    public static VectorF operator -(VectorF vector1, VectorF vector2)
    {
      return new VectorF(vector1.x - vector2.x, vector1.y - vector2.y);
    }

    public static VectorF Subtract(VectorF vector1, VectorF vector2) => vector1 - vector2;

    public static VectorF operator *(float scalar, VectorF vector)
    {
      return new VectorF(vector.x * scalar, vector.y * scalar);
    }

    public static VectorF operator *(VectorF vector, float scalar)
    {
      return new VectorF(vector.x * scalar, vector.y * scalar);
    }

    public static VectorF operator /(VectorF vector, float scalar)
    {
      return new VectorF(vector.x / scalar, vector.y / scalar);
    }

    public static VectorF Multiply(float scalar, VectorF vector) => scalar * vector;

    public static VectorF Multiply(VectorF vector, float scalar) => vector * scalar;

    public static VectorF Divide(VectorF vector, float scalar) => vector / scalar;

    public static float operator *(VectorF vector1, VectorF vector2)
    {
      return (float) ((double) vector1.x * (double) vector2.x + (double) vector1.y * (double) vector2.y);
    }

    public static float Multiply(VectorF vector1, VectorF vector2) => vector1 * vector2;

    public float Length => (float) Math.Sqrt((double) this.LengthSquared);

    public float LengthSquared => this * this;

    public void Normalize()
    {
      float length = this.Length;
      this.x /= length;
      this.y /= length;
    }

    public static float CrossProduct(VectorF vector1, VectorF vector2)
    {
      return (float) ((double) vector1.x * (double) vector2.y - (double) vector1.y * (double) vector2.x);
    }

    public static float Determinant(VectorF vector1, VectorF vector2)
    {
      return VectorF.CrossProduct(vector1, vector2);
    }

    public static float AngleBetween(VectorF vector1, VectorF vector2)
    {
      vector1.Normalize();
      vector2.Normalize();
      double num = Math.Atan2((double) vector2.y, (double) vector2.x) - Math.Atan2((double) vector1.y, (double) vector1.x);
      if (num > Math.PI)
        num -= 2.0 * Math.PI;
      else if (num < -1.0 * Math.PI)
        num += 2.0 * Math.PI;
      return (float) num;
    }

    public override int GetHashCode() => this.x.GetHashCode() ^ this.y.GetHashCode();

    public override string ToString()
    {
      return this.ToString((IFormatProvider) CultureInfo.CurrentCulture);
    }

    public string ToString(IFormatProvider provider)
    {
      return string.Format(provider, Resources.VectorToStringFormat, new object[2]
      {
        (object) this.x,
        (object) this.y
      });
    }
  }
}
