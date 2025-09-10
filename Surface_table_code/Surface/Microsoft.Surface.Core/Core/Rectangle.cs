// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.Rectangle
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.Properties;
using System;
using System.Globalization;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal struct Rectangle
  {
    private static Rectangle empty = new Rectangle(0, 0, 0, 0);
    private int top;
    private int left;
    private int bottom;
    private int right;

    public Rectangle(int top, int left, int bottom, int right)
    {
      this.top = top;
      this.left = left;
      this.right = right;
      this.bottom = bottom;
    }

    public static Rectangle Empty => Rectangle.empty;

    public void Resize(float amountInX, float amountInY)
    {
      this = Rectangle.Resize(this, amountInX, amountInY);
    }

    public static Rectangle Resize(Rectangle rectangle, float amountInX, float amountInY)
    {
      rectangle.Left = (int) ((double) rectangle.Left * (double) amountInX);
      rectangle.Right = (int) ((double) rectangle.Right * (double) amountInX);
      rectangle.Top = (int) ((double) rectangle.Top * (double) amountInY);
      rectangle.Bottom = (int) ((double) rectangle.Bottom * (double) amountInY);
      return rectangle;
    }

    public bool Contains(Rectangle rectangle)
    {
      return this.Left <= rectangle.Left && this.Right >= rectangle.Right && this.Top <= rectangle.Top && this.Bottom >= rectangle.Bottom;
    }

    public int Top
    {
      get => this.top;
      set => this.top = value;
    }

    public int Left
    {
      get => this.left;
      set => this.left = value;
    }

    public int Bottom
    {
      get => this.bottom;
      set => this.bottom = value;
    }

    public int Right
    {
      get => this.right;
      set => this.right = value;
    }

    public int Width => this.right - this.left;

    public int Height => this.bottom - this.top;

    public static Rectangle Intersect(Rectangle a, Rectangle b)
    {
      Rectangle rectangle = new Rectangle(Math.Max(a.top, b.top), Math.Max(a.left, b.left), Math.Min(a.bottom, b.bottom), Math.Min(a.right, b.right));
      if (rectangle.left >= rectangle.right || rectangle.top >= rectangle.bottom)
        rectangle = Rectangle.Empty;
      return rectangle;
    }

    public Rectangle Intersect(Rectangle rectangle) => Rectangle.Intersect(this, rectangle);

    public static bool operator ==(Rectangle r1, Rectangle r2)
    {
      return r1.left == r2.left && r1.right == r2.right && r1.top == r2.top && r1.bottom == r2.bottom;
    }

    public static bool operator !=(Rectangle r1, Rectangle r2) => !(r1 == r2);

    public override bool Equals(object obj) => obj is Rectangle rectangle && this == rectangle;

    public override int GetHashCode()
    {
      return this.left.GetHashCode() ^ this.right.GetHashCode() ^ this.top.GetHashCode() ^ this.bottom.GetHashCode();
    }

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.RectangleToStringFormat, (object) this.Left, (object) this.Top, (object) this.Right, (object) this.Bottom, (object) this.Width, (object) this.Height);
    }
  }
}
