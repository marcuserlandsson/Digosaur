// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.ArrayFactory`1
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;
using System.Collections.Generic;

#nullable disable
namespace Microsoft.Surface.Core.RawInput
{
  internal class ArrayFactory<T> : IArrayFactory<T>
  {
    private readonly IList<WeakReference> weakRefCacheCollection = (IList<WeakReference>) new List<WeakReference>();
    private readonly object syncObject = new object();

    public T[] Alloc(int length)
    {
      T[] cacheData;
      if (!this.TryDequeueImageDataCache(length, out cacheData))
        cacheData = new T[length];
      return cacheData;
    }

    public bool Release(T[] cacheData)
    {
      lock (this.syncObject)
        this.weakRefCacheCollection.Add(new WeakReference((object) cacheData));
      return true;
    }

    private bool TryDequeueImageDataCache(int length, out T[] cacheData)
    {
      cacheData = (T[]) null;
      lock (this.syncObject)
      {
        int index = 0;
        while (index < this.weakRefCacheCollection.Count)
        {
          WeakReference weakRefCache = this.weakRefCacheCollection[index];
          if ((weakRefCache.IsAlive ? weakRefCache.Target : (object) null) is T[] target)
          {
            if (target.Length == length)
            {
              cacheData = target;
              this.weakRefCacheCollection.RemoveAt(index);
              break;
            }
            ++index;
          }
          else
            this.weakRefCacheCollection.RemoveAt(index);
        }
      }
      return cacheData != null;
    }
  }
}
