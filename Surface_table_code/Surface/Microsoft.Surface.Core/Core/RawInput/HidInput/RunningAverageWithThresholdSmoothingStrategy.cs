// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.HidInput.RunningAverageWithThresholdSmoothingStrategy
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace Microsoft.Surface.Core.RawInput.HidInput
{
  internal class RunningAverageWithThresholdSmoothingStrategy : ThresholdSmoothingStrategy
  {
    private readonly int numSamplesInAverage;
    private readonly LinkedList<float> samples = new LinkedList<float>();

    protected float LastSampleAdded => this.samples.Last.Value;

    public RunningAverageWithThresholdSmoothingStrategy(
      int numSamplesInAverage,
      float thresholdPlusOrMinus)
      : base(thresholdPlusOrMinus)
    {
      this.numSamplesInAverage = numSamplesInAverage;
    }

    public override void AddSample(float sample)
    {
      this.samples.AddLast(sample);
      if (this.samples.Count > this.numSamplesInAverage)
        this.samples.RemoveFirst();
      base.AddSample(this.samples.Average());
    }
  }
}
