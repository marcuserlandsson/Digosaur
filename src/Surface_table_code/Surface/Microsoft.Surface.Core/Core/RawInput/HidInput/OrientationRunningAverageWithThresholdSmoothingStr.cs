// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.HidInput.OrientationRunningAverageWithThresholdSmoothingStrategy
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System;

#nullable disable
namespace Microsoft.Surface.Core.RawInput.HidInput
{
  internal class OrientationRunningAverageWithThresholdSmoothingStrategy : 
    RunningAverageWithThresholdSmoothingStrategy
  {
    private const float RadianEquivalentOf180Degrees = 3.14159274f;
    private const float RadianEquivalentOf360Degrees = 6.28318548f;

    public OrientationRunningAverageWithThresholdSmoothingStrategy(
      int numSamplesInAverage,
      float thresholdPlusOrMinus)
      : base(numSamplesInAverage, thresholdPlusOrMinus)
    {
    }

    public override void AddSample(float sample)
    {
      sample = OrientationRunningAverageWithThresholdSmoothingStrategy.ClampRadiansBetweenZeroAndTwoPi(sample);
      if (this.HaveAnySamplesBeenAdded && (double) Math.Abs(sample - this.LastSampleAdded) > 3.1415927410125732)
      {
        if ((double) sample > (double) this.LastSampleAdded)
          sample -= 6.28318548f;
        else
          sample += 6.28318548f;
      }
      base.AddSample(sample);
    }

    private static float ClampRadiansBetweenZeroAndTwoPi(float value)
    {
      value %= 6.28318548f;
      if ((double) value < 0.0)
        value += 6.28318548f;
      return value;
    }
  }
}
