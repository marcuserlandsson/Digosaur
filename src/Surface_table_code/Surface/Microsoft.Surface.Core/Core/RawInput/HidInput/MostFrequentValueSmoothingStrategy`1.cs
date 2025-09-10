// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.HidInput.MostFrequentValueSmoothingStrategy`1
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using System.Collections.Generic;

#nullable disable
namespace Microsoft.Surface.Core.RawInput.HidInput
{
  internal class MostFrequentValueSmoothingStrategy<T> : ISmoothingStrategy<T>
  {
    private readonly int minRequiredSamples;
    private readonly T defaultValue;
    private readonly Dictionary<T, int> scoreCard = new Dictionary<T, int>();

    public MostFrequentValueSmoothingStrategy(int minRequiredSamples, T defaultValue)
    {
      this.minRequiredSamples = minRequiredSamples;
      this.defaultValue = defaultValue;
      this.SmoothedValue = defaultValue;
    }

    public virtual void AddSample(T sample)
    {
      int num1 = 1;
      if (this.scoreCard.ContainsKey(sample))
      {
        num1 = this.scoreCard[sample];
        if (num1 < int.MaxValue)
          ++num1;
      }
      this.scoreCard[sample] = num1;
      if (sample.Equals((object) this.SmoothedValue))
        return;
      T obj = this.defaultValue;
      int num2 = int.MinValue;
      foreach (T key in this.scoreCard.Keys)
      {
        int num3 = this.scoreCard[key];
        if (num3 >= num2)
        {
          num2 = num3;
          obj = key;
        }
      }
      if (num2 < this.minRequiredSamples)
        return;
      this.SmoothedValue = obj;
    }

    public T SmoothedValue { get; private set; }
  }
}
