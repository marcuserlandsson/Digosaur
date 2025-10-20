// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.HidInput.UnsmoothedSmoothingStrategy`1
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

#nullable disable
namespace Microsoft.Surface.Core.RawInput.HidInput
{
  internal class UnsmoothedSmoothingStrategy<T> : ISmoothingStrategy<T>
  {
    public void AddSample(T sample) => this.SmoothedValue = sample;

    public T SmoothedValue { get; private set; }
  }
}
