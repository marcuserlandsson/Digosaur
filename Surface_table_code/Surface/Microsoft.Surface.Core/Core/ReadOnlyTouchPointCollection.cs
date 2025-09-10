// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.ReadOnlyTouchPointCollection
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;

#nullable disable
namespace Microsoft.Surface.Core
{
  public class ReadOnlyTouchPointCollection : ReadOnlyCollection<TouchPoint>
  {
    private readonly TouchPointCollection touchPointCollection;

    internal ReadOnlyTouchPointCollection(TouchPointCollection touchPointCollection)
      : base(touchPointCollection.WrappedItems)
    {
      this.touchPointCollection = touchPointCollection;
    }

    public ReadOnlyTouchPointCollection(ReadOnlyCollection<TouchPoint> touchPoints)
      : base((IList<TouchPoint>) touchPoints)
    {
      TouchPointCollection touchPointCollection = new TouchPointCollection();
      foreach (TouchPoint touchPoint in touchPoints)
        touchPointCollection.Add(touchPoint);
      this.touchPointCollection = touchPointCollection;
    }

    public TouchPoint GetTouchPointFromId(int touchPointId)
    {
      return this.touchPointCollection[touchPointId];
    }

    public bool TryGetTouchPointFromId(int touchPointId, out TouchPoint touchPoint)
    {
      if (this.touchPointCollection.Contains(touchPointId))
      {
        touchPoint = this.touchPointCollection[touchPointId];
        return true;
      }
      touchPoint = (TouchPoint) null;
      return false;
    }

    public bool Contains(int touchPointId) => this.touchPointCollection.Contains(touchPointId);
  }
}
