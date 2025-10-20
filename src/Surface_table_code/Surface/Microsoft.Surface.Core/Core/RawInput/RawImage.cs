// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.RawImage
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace Microsoft.Surface.Core.RawInput
{
  internal class RawImage
  {
    private readonly ImageType type;
    private readonly int width;
    private readonly int height;
    private readonly int bitsPerPixel;
    private readonly int stride;
    private byte[] imageData;
    private readonly IArrayFactory<byte> imageDataFactory;

    public unsafe RawImage(
      ImageType t,
      int w,
      int h,
      int bpp,
      byte* data,
      IArrayFactory<byte> imageDataFactory)
    {
      this.type = t;
      this.width = w;
      this.height = h;
      this.bitsPerPixel = bpp;
      this.stride = 0;
      this.imageDataFactory = imageDataFactory;
      switch (bpp)
      {
        case 1:
          this.stride = (this.width + 7) / 8;
          break;
        case 8:
          this.stride = this.width;
          break;
        default:
          this.stride = this.width;
          break;
      }
      this.imageData = imageDataFactory.Alloc(this.stride * this.height);
      Marshal.Copy(new IntPtr((void*) data), this.imageData, 0, this.stride * this.height);
    }

    public ImageType ImageType => this.type;

    public int Width => this.width;

    public int Height => this.height;

    public int BitsPerPixel => this.bitsPerPixel;

    public int Stride => this.stride;

    public byte[] ImageData => this.imageData;

    public void DetachImageData()
    {
      if (this.imageData == null || !this.imageDataFactory.Release(this.imageData))
        return;
      this.imageData = (byte[]) null;
    }

    public bool IsImageDataAttached => this.imageData != null;
  }
}
