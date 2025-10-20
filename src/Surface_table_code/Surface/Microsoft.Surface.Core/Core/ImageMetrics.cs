// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.ImageMetrics
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

#nullable disable
namespace Microsoft.Surface.Core
{
  public class ImageMetrics
  {
    private readonly double dpiX;
    private readonly double dpiY;
    private readonly int stride;
    private readonly int width;
    private readonly int height;
    private readonly int bpp;

    internal ImageMetrics(int bpp, double dpiX, double dpiY, int stride, int width, int height)
    {
      this.bpp = bpp;
      this.dpiX = dpiX;
      this.dpiY = dpiY;
      this.stride = stride;
      this.width = width;
      this.height = height;
    }

    public int BitsPerPixel => this.bpp;

    public double DpiX => this.dpiX;

    public double DpiY => this.dpiY;

    public int Stride => this.stride;

    public int Width => this.width;

    public int Height => this.height;
  }
}
