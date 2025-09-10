// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.HidInput.SmoothingConfiguration
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

#nullable disable
namespace Microsoft.Surface.Core.RawInput.HidInput
{
  internal class SmoothingConfiguration
  {
    public int PositionNumSamplesInAverage { get; set; }

    public float PositionThresholdPlusOrMinus { get; set; }

    public int OrientationNumSamplesInAverage { get; set; }

    public float OrientationThresholdPlusOrMinus { get; set; }

    public int EllipseAxisNumSamplesInAverage { get; set; }

    public float EllipseAxisThresholdPlusOrMinus { get; set; }

    public int SizeNumSamplesInAverage { get; set; }

    public float SizeThresholdPlusOrMinus { get; set; }

    public int TagDataNumSamplesRequired { get; set; }

    public SmoothingConfiguration(
      int positionNumSamplesInAverage,
      float positionThresholdPlusOrMinus,
      int orientationNumSamplesInAverage,
      float orientationThresholdPlusOrMinus,
      int ellipseAxisNumSamplesInAverage,
      float ellipseAxisThresholdPlusOrMinus,
      int sizeNumSamplesInAverage,
      float sizeThresholdPlusOrMinus,
      int tagDataNumSamplesRequired)
    {
      this.PositionNumSamplesInAverage = positionNumSamplesInAverage;
      this.PositionThresholdPlusOrMinus = positionThresholdPlusOrMinus;
      this.OrientationNumSamplesInAverage = orientationNumSamplesInAverage;
      this.OrientationThresholdPlusOrMinus = orientationThresholdPlusOrMinus;
      this.EllipseAxisNumSamplesInAverage = ellipseAxisNumSamplesInAverage;
      this.EllipseAxisThresholdPlusOrMinus = ellipseAxisThresholdPlusOrMinus;
      this.SizeNumSamplesInAverage = sizeNumSamplesInAverage;
      this.SizeThresholdPlusOrMinus = sizeThresholdPlusOrMinus;
      this.TagDataNumSamplesRequired = tagDataNumSamplesRequired;
    }
  }
}
