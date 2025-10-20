// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.SystemGestureProperties
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;

#nullable disable
namespace Microsoft.Surface.Core.RawInput
{
  internal abstract class SystemGestureProperties
  {
    private IntPtr hwndTarget;

    public SystemGestureProperties(IntPtr hwnd) => this.hwndTarget = hwnd;

    public abstract SystemGestureProperties Clone();

    public abstract int Id { get; }

    public abstract PointF Position { get; }

    public abstract SystemGestureType GestureType { get; }

    public virtual IntPtr HwndTarget
    {
      get => this.hwndTarget;
      set => this.hwndTarget = value;
    }
  }
}
