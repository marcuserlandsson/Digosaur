// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.FrameReceivedEventArgs
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.RawInput;
using System;
using System.Diagnostics;

#nullable disable
namespace Microsoft.Surface.Core
{
  public class FrameReceivedEventArgs : EventArgs
  {
    private readonly FrameProperties properties;
    private readonly TouchTarget target;

    internal FrameReceivedEventArgs(TouchTarget TouchTarget, FrameProperties frameProperties)
    {
      this.properties = frameProperties;
      this.target = TouchTarget;
    }

    public long FrameTimestamp => this.properties.Timestamp;

    private bool IsImageAvailable(ImageType type)
    {
      ImageTypeValidation.ValidateImageType(type);
      return this.target.IsImageTypeEnabled(type) && InteractiveSurface.PrimarySurfaceDevice != null && this.properties.IsImageTypeAvailable(type);
    }

    public bool TryGetRawImage(
      ImageType type,
      int left,
      int top,
      int width,
      int height,
      out byte[] buffer,
      out ImageMetrics metrics)
    {
      if (width < 0)
        throw SurfaceCoreExceptions.ArgumentOutOfRangeException(nameof (width));
      if (height < 0)
        throw SurfaceCoreExceptions.ArgumentOutOfRangeException(nameof (height));
      if (!this.IsImageAvailable(type))
      {
        buffer = (byte[]) null;
        metrics = (ImageMetrics) null;
        return false;
      }
      int paddingLeft = 0;
      int paddingRight = 0;
      RawImage image;
      Rectangle intersectRect;
      Rectangle source;
      Rectangle dest;
      bool doBitBlt;
      this.PrepareForImageCopy(type, left, top, width, height, out int _, out int _, out int _, out paddingLeft, out paddingRight, out metrics, out image, out intersectRect, out source, out dest, out doBitBlt);
      if (!image.IsImageDataAttached)
      {
        buffer = (byte[]) null;
        return false;
      }
      int num = 8 / image.BitsPerPixel;
      int stride = dest.Width / num;
      metrics = new ImageMetrics(metrics.BitsPerPixel, metrics.DpiX, metrics.DpiY, stride, dest.Width, dest.Height);
      buffer = new byte[metrics.Stride * dest.Height];
      if (doBitBlt)
      {
        FrameReceivedEventArgs.BitBlt(source, image, metrics.Stride, intersectRect, buffer, dest.Top, dest.Left);
        image.DetachImageData();
      }
      return true;
    }

    public bool TryGetRawImage(
      ImageType type,
      TouchPoint touchPoint,
      out byte[] buffer,
      out ImageMetrics metrics)
    {
      if (touchPoint == null)
        throw SurfaceCoreExceptions.ArgumentNullException(nameof (touchPoint));
      return this.TryGetRawImage(type, (int) touchPoint.Bounds.Left, (int) touchPoint.Bounds.Top, (int) touchPoint.Bounds.Width, (int) touchPoint.Bounds.Height, out buffer, out metrics);
    }

    public bool TryGetRawImage(
      ImageType type,
      int left,
      int top,
      int width,
      int height,
      out byte[] buffer,
      out ImageMetrics metrics,
      out int paddingLeft,
      out int paddingRight)
    {
      if (width < 0)
        throw SurfaceCoreExceptions.ArgumentOutOfRangeException(nameof (width));
      if (height < 0)
        throw SurfaceCoreExceptions.ArgumentOutOfRangeException(nameof (height));
      if (!this.IsImageAvailable(type))
      {
        buffer = (byte[]) null;
        metrics = (ImageMetrics) null;
        paddingLeft = 0;
        paddingRight = 0;
        return false;
      }
      RawImage image;
      Rectangle intersectRect;
      Rectangle source;
      Rectangle dest;
      bool doBitBlt;
      this.PrepareForImageCopy(type, left, top, width, height, out int _, out int _, out int _, out paddingLeft, out paddingRight, out metrics, out image, out intersectRect, out source, out dest, out doBitBlt);
      if (!image.IsImageDataAttached)
      {
        buffer = (byte[]) null;
        return false;
      }
      metrics = new ImageMetrics(metrics.BitsPerPixel, metrics.DpiX, metrics.DpiY, metrics.Stride, dest.Width, dest.Height);
      buffer = new byte[metrics.Stride * dest.Height];
      if (doBitBlt)
      {
        FrameReceivedEventArgs.BitBlt(source, image, metrics.Stride, intersectRect, buffer, dest.Top, dest.Left);
        image.DetachImageData();
      }
      return true;
    }

    public bool TryGetRawImage(
      ImageType type,
      TouchPoint touchPoint,
      out byte[] buffer,
      out ImageMetrics metrics,
      out int paddingLeft,
      out int paddingRight)
    {
      if (touchPoint == null)
        throw SurfaceCoreExceptions.ArgumentNullException(nameof (touchPoint));
      return this.TryGetRawImage(type, (int) touchPoint.Bounds.Left, (int) touchPoint.Bounds.Top, (int) touchPoint.Bounds.Width, (int) touchPoint.Bounds.Height, out buffer, out metrics, out paddingLeft, out paddingRight);
    }

    public bool UpdateRawImage(
      ImageType type,
      byte[] buffer,
      int left,
      int top,
      int width,
      int height)
    {
      if (buffer == null)
        throw SurfaceCoreExceptions.ArgumentNullException(nameof (buffer));
      if (width < 0)
        throw SurfaceCoreExceptions.ArgumentOutOfRangeException(nameof (width));
      if (height < 0)
        throw SurfaceCoreExceptions.ArgumentOutOfRangeException(nameof (height));
      if (!this.IsImageAvailable(type))
        return false;
      ImageMetrics metrics;
      RawImage image;
      Rectangle intersectRect;
      Rectangle source;
      Rectangle dest;
      bool doBitBlt;
      this.PrepareForImageCopy(type, left, top, width, height, out int _, out int _, out int _, out int _, out int _, out metrics, out image, out intersectRect, out source, out dest, out doBitBlt);
      if (!image.IsImageDataAttached)
      {
        buffer = (byte[]) null;
        return false;
      }
      int num = 8 / image.BitsPerPixel;
      int stride = dest.Width / num;
      if (stride * dest.Height != buffer.Length)
        stride = metrics.Stride;
      if (buffer.Length != stride * dest.Height)
        throw SurfaceCoreExceptions.InvalidBufferLengthForUpdateRawImageException();
      ImageMetrics imageMetrics = new ImageMetrics(metrics.BitsPerPixel, metrics.DpiX, metrics.DpiY, stride, dest.Width, dest.Height);
      if (!source.Contains(dest))
        Array.Clear((Array) buffer, 0, buffer.Length);
      if (doBitBlt)
      {
        FrameReceivedEventArgs.BitBlt(source, image, imageMetrics.Stride, intersectRect, buffer, dest.Top, dest.Left);
        image.DetachImageData();
      }
      return true;
    }

    private static int FourByteAlign(int align) => (align + 3) / 4 * 4;

    private void PrepareForImageCopy(
      ImageType type,
      int clientLeft,
      int clientTop,
      int width,
      int height,
      out int imageLeftByteOffset,
      out int imageTop,
      out int copyWidthInBytes,
      out int paddingLeft,
      out int paddingRight,
      out ImageMetrics metrics,
      out RawImage image,
      out Rectangle intersectRect,
      out Rectangle source,
      out Rectangle dest,
      out bool doBitBlt)
    {
      imageLeftByteOffset = 0;
      imageTop = 0;
      copyWidthInBytes = 0;
      paddingLeft = 0;
      paddingRight = 0;
      metrics = (ImageMetrics) null;
      image = (RawImage) null;
      doBitBlt = true;
      source = Rectangle.Empty;
      intersectRect = Rectangle.Empty;
      dest = Rectangle.Empty;
      ImageTypeValidation.ValidateImageType(type);
      if (!this.target.IsImageTypeEnabled(type) || !this.properties.TryGetImage(type, out image))
        throw SurfaceCoreExceptions.ImageTypeNotEnabledException();
      NativeMethods.POINT pt1 = new NativeMethods.POINT(clientLeft, clientTop);
      NativeMethods.POINT pt2 = new NativeMethods.POINT(clientLeft + width, clientTop + height);
      if (this.target.Hwnd != IntPtr.Zero)
      {
        NativeMethods.ClientToScreen(this.target.Hwnd, ref pt1);
        NativeMethods.ClientToScreen(this.target.Hwnd, ref pt2);
      }
      InteractiveSurfaceDevice primarySurfaceDevice = InteractiveSurface.PrimarySurfaceDevice;
      source = new Rectangle(0, 0, primarySurfaceDevice.Height, primarySurfaceDevice.Width);
      dest = new Rectangle(pt1.Y - primarySurfaceDevice.Top, pt1.X - primarySurfaceDevice.Left, pt2.Y - primarySurfaceDevice.Top, pt2.X - primarySurfaceDevice.Left);
      intersectRect = source.Intersect(dest);
      if (intersectRect == Rectangle.Empty)
        doBitBlt = false;
      float amountInX = (float) image.Width / (float) primarySurfaceDevice.Width;
      float amountInY = (float) image.Height / (float) primarySurfaceDevice.Height;
      intersectRect.Resize(amountInX, amountInY);
      source.Resize(amountInX, amountInY);
      dest.Resize(amountInX, amountInY);
      int num = 8 / image.BitsPerPixel;
      int left = intersectRect.Left;
      intersectRect.Left = intersectRect.Left / num * num;
      intersectRect.Right = (intersectRect.Right + num - 1) / num * num;
      source.Left = source.Left / num * num;
      source.Right = (source.Right + num - 1) / num * num;
      dest.Left = dest.Left / num * num;
      dest.Right = (dest.Right + num - 1) / num * num;
      paddingLeft = left - intersectRect.Left;
      int stride = FrameReceivedEventArgs.FourByteAlign(image.BitsPerPixel % 8 != 0 ? (dest.Width + paddingLeft + num - 1) / 8 : dest.Width * (image.BitsPerPixel / 8));
      paddingRight = stride * num - (dest.Width + paddingLeft);
      ImageMetrics rawImageMetrics = this.target.GetRawImageMetrics(image.ImageType);
      metrics = new ImageMetrics(image.BitsPerPixel, rawImageMetrics.DpiX, rawImageMetrics.DpiY, stride, dest.Width, dest.Height);
      imageLeftByteOffset = intersectRect.Left / num;
      copyWidthInBytes = intersectRect.Width / num;
      imageTop = intersectRect.Top;
    }

    private static void BitBlt(
      Rectangle surfaceRect,
      RawImage surfaceImage,
      int imageStride,
      Rectangle destRect,
      byte[] dest,
      int imageTop,
      int imageLeft)
    {
      byte[] imageData = surfaceImage.ImageData;
      if (imageData.Length == dest.Length && surfaceRect.Left == destRect.Left && surfaceRect.Top == destRect.Top)
      {
        Array.Copy((Array) imageData, (Array) dest, dest.Length);
      }
      else
      {
        int num1 = 8 / surfaceImage.BitsPerPixel;
        int num2 = (destRect.Left - imageLeft) / num1;
        int num3 = (destRect.Left - surfaceRect.Left) / num1;
        int num4 = surfaceRect.Width / num1;
        int length = destRect.Width / num1;
        int sourceIndex = num4 * (destRect.Top - surfaceRect.Top) + num3;
        int destinationIndex = imageStride * (destRect.Top - imageTop) + num2;
        for (int index = 0; index < destRect.Height; ++index)
        {
          Array.Copy((Array) imageData, sourceIndex, (Array) dest, destinationIndex, length);
          destinationIndex += imageStride;
          sourceIndex += num4;
        }
      }
    }

    [Conditional("DEBUG")]
    private void ValidateImageMetrics(RawImage image)
    {
      this.target.GetRawImageMetrics(image.ImageType);
    }
  }
}
