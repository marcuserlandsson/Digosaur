// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.CorePerformanceCounters
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Surface.Core;
using System;

#nullable disable
namespace Microsoft.Surface
{
  internal sealed class CorePerformanceCounters : PerformanceCounters
  {
    private static object staticSyncRoot = new object();
    private static CorePerformanceCounters singleton;
    public PerformanceCounters.ElapsedTimeCounter VisionProcessingTime = PerformanceCounters.ElapsedTimeCounter.None;
    public PerformanceCounters.ElapsedTimeCounter CoreTouchProcessingTime = PerformanceCounters.ElapsedTimeCounter.None;
    public PerformanceCounters.Counter CoreTotalTouchCount = PerformanceCounters.Counter.None;
    public PerformanceCounters.Counter CoreNumberOfTouches = PerformanceCounters.Counter.None;
    public PerformanceCounters.Counter CoreFrameEventRate = PerformanceCounters.Counter.None;

    public override string CategoryName => "Microsoft Surface Core";

    public override void Initialize(string instanceName)
    {
      this.VisionProcessingTime = new PerformanceCounters.ElapsedTimeCounter((PerformanceCounters) this, this.CategoryName, "Vision System - processing time", instanceName);
      this.CoreTouchProcessingTime = new PerformanceCounters.ElapsedTimeCounter((PerformanceCounters) this, this.CategoryName, "Core - touch processing time", instanceName);
      this.CoreTotalTouchCount = new PerformanceCounters.Counter((PerformanceCounters) this, this.CategoryName, "Core - total touch count", instanceName);
      this.CoreNumberOfTouches = new PerformanceCounters.Counter((PerformanceCounters) this, this.CategoryName, "Core - number of touches", instanceName);
      this.CoreFrameEventRate = new PerformanceCounters.Counter((PerformanceCounters) this, this.CategoryName, "Core - frame rate", instanceName);
    }

    public override void Dispose()
    {
      PerformanceCounters.SafeDispose((PerformanceCounters.CounterBase) this.VisionProcessingTime);
      PerformanceCounters.SafeDispose((PerformanceCounters.CounterBase) this.CoreTouchProcessingTime);
      PerformanceCounters.SafeDispose((PerformanceCounters.CounterBase) this.CoreTotalTouchCount);
      PerformanceCounters.SafeDispose((PerformanceCounters.CounterBase) this.CoreNumberOfTouches);
      PerformanceCounters.SafeDispose((PerformanceCounters.CounterBase) this.CoreFrameEventRate);
    }

    protected override void PerformanceCountersAreNotInstalled()
    {
      EventsCore.LogPerformanceCountersAreNotInstalled();
    }

    public override void CannotInstantiatePerformanceCounter(string counterName, Exception e)
    {
      EventsCore.LogCannotInstantiatePerformanceCounter(counterName, e.ToString());
    }

    public static CorePerformanceCounters Instance
    {
      get
      {
        if (CorePerformanceCounters.singleton == null)
        {
          lock (CorePerformanceCounters.staticSyncRoot)
          {
            if (CorePerformanceCounters.singleton == null)
              CorePerformanceCounters.singleton = new CorePerformanceCounters();
          }
        }
        return CorePerformanceCounters.singleton;
      }
    }
  }
}
