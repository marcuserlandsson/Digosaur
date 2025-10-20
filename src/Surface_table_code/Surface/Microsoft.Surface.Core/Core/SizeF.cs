// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.SizeF
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.Properties;
using System;
using System.Globalization;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal struct SizeF
  {
    private float width;
    private float height;

    public SizeF(float width, float height)
    {
      this.width = width;
      this.height = height;
    }

    public static explicit operator PointF(SizeF size) => new PointF(size.width, size.height);

    public static explicit operator VectorF(SizeF size) => new VectorF(size.width, size.height);

    public float Width
    {
      get => this.width;
      set => this.width = value;
    }

    public float Height
    {
      get => this.height;
      set => this.height = value;
    }

    public static bool operator ==(SizeF s1, SizeF s2)
    {
      return (double) s1.Width == (double) s2.Width && (double) s1.Height == (double) s2.Height;
    }

    public static bool operator !=(SizeF s1, SizeF s2) => !(s1 == s2);

    public override bool Equals(object obj) => obj is SizeF sizeF && this == sizeF;

    public override int GetHashCode() => this.width.GetHashCode() ^ this.height.GetHashCode();

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.SizeFToStringFormat, new object[2]
      {
        (object) this.Width,
        (object) this.Height
      });
    }
  }
}
