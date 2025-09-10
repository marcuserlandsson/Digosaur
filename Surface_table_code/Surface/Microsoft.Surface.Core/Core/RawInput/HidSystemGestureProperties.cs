// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.HidSystemGestureProperties
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.RawInput.HidInput;

#nullable disable
namespace Microsoft.Surface.Core.RawInput
{
  internal class HidSystemGestureProperties : SystemGestureProperties
  {
    private int contactId;
    private SystemGestureType gestureType;
    private PointF position;

    public HidSystemGestureProperties(SystemGestureType gestureType, ContactRecord contactRecord)
      : base(contactRecord.CapturedHwnd)
    {
      this.contactId = contactRecord.LastTouchProperties.Id;
      this.position = contactRecord.LastTouchProperties.Position;
      this.gestureType = gestureType;
    }

    private HidSystemGestureProperties(HidSystemGestureProperties copyMe)
      : base(copyMe.HwndTarget)
    {
      this.contactId = copyMe.contactId;
      this.position = copyMe.position;
      this.gestureType = copyMe.gestureType;
    }

    public override SystemGestureProperties Clone()
    {
      return (SystemGestureProperties) new HidSystemGestureProperties(this);
    }

    public override int Id => this.contactId;

    public override PointF Position => this.position;

    public override SystemGestureType GestureType => this.gestureType;
  }
}
