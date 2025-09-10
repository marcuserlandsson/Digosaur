// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.TouchPointCollection
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

#nullable disable
namespace Microsoft.Surface.Core
{
  internal class TouchPointCollection : KeyedCollection<int, TouchPoint>
  {
    internal TouchPointCollection()
      : base((IEqualityComparer<int>) null, 7)
    {
    }

    internal TouchPointCollection Clone()
    {
      TouchPointCollection touchPointCollection = new TouchPointCollection();
      foreach (TouchPoint touchPoint in (Collection<TouchPoint>) this)
        touchPointCollection.Add(touchPoint.Clone());
      return touchPointCollection;
    }

    internal IList<TouchPoint> WrappedItems => this.Items;

    protected override int GetKeyForItem(TouchPoint item) => item.Id;

    public override string ToString()
    {
      return string.Format((IFormatProvider) CultureInfo.CurrentCulture, Resources.TouchPointCollectionToStringFormat, new object[1]
      {
        (object) this.Count
      });
    }
  }
}
