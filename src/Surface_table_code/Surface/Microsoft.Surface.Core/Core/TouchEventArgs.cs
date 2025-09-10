// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.TouchEventArgs
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;

#nullable disable
namespace Microsoft.Surface.Core
{
  public class TouchEventArgs : EventArgs
  {
    private readonly TouchPoint touchPoint;

    internal static TouchEventArgs Create(TouchPoint touchPoint) => new TouchEventArgs(touchPoint);

    protected TouchEventArgs(TouchPoint touchPoint)
    {
      this.touchPoint = touchPoint != null ? touchPoint.Clone() : throw SurfaceCoreExceptions.ArgumentNullException(nameof (touchPoint));
    }

    public TouchPoint TouchPoint => this.touchPoint;

    public override string ToString() => this.touchPoint.ToString();
  }
}
