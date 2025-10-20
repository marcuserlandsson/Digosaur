// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.HidInput.ThresholdSmoothingStrategy
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;

#nullable disable
namespace Microsoft.Surface.Core.RawInput.HidInput
{
  internal class ThresholdSmoothingStrategy : ISmoothingStrategy<float>
  {
    private readonly float thresholdPlusOrMinus;

    protected bool HaveAnySamplesBeenAdded { get; private set; }

    public ThresholdSmoothingStrategy(float thresholdPlusOrMinus)
    {
      this.thresholdPlusOrMinus = thresholdPlusOrMinus;
    }

    public virtual void AddSample(float sample)
    {
      if ((double) Math.Abs(this.SmoothedValue - sample) < (double) this.thresholdPlusOrMinus && this.HaveAnySamplesBeenAdded)
        return;
      this.SmoothedValue = sample;
      this.HaveAnySamplesBeenAdded = true;
    }

    public float SmoothedValue { get; private set; }
  }
}
