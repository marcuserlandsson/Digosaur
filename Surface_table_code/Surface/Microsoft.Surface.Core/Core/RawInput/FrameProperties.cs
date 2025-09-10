// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.FrameProperties
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System.Collections.Generic;

#nullable disable
namespace Microsoft.Surface.Core.RawInput
{
  internal class FrameProperties
  {
    private Dictionary<ImageType, RawImage> frameImages;
    private readonly long timestamp;

    public FrameProperties(Dictionary<ImageType, RawImage> images, long timestamp)
    {
      this.frameImages = images;
      this.timestamp = timestamp;
    }

    public bool TryGetImage(ImageType type, out RawImage image)
    {
      image = (RawImage) null;
      return this.frameImages != null && this.frameImages.TryGetValue(type, out image);
    }

    public bool IsImageTypeAvailable(ImageType type) => this.TryGetImage(type, out RawImage _);

    internal long Timestamp => this.timestamp;
  }
}
