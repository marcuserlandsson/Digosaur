// Decompiled with JetBrains decompiler
// Type: Microsoft.Surface.Core.RawInput.HidInput.ContactRecord
// Assembly: Microsoft.Surface.Core, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: DD6306C5-AF9D-470C-9C01-56F631E1B11F
// Assembly location: C:\Users\lilja\Downloads\Microsoft.Surface.Core.dll

using Microsoft.Win32;
using System;
using System.Globalization;

#nullable disable
namespace Microsoft.Surface.Core.RawInput.HidInput
{
  internal class ContactRecord
  {
    private static readonly SmoothingConfiguration fingerSmoothingConfiguration = new SmoothingConfiguration(9, 1f, 9, ContactRecord.ConvertDegreesToRadians(8f), 9, 1f, 9, 2f, 0);
    private static readonly SmoothingConfiguration tagSmoothingConfiguration = new SmoothingConfiguration(6, 2f, 6, ContactRecord.ConvertDegreesToRadians(3f), 1, 1f, 1, 1f, 10);
    private static readonly SmoothingConfiguration blobSmoothingConfiguration = new SmoothingConfiguration(3, 1f, 1, ContactRecord.ConvertDegreesToRadians(1f), 6, 1f, 6, 2f, 0);
    private static readonly SmoothingConfiguration nonVipSmoothingConfiguration = new SmoothingConfiguration(1, 0.5f, 1, ContactRecord.ConvertDegreesToRadians(2f), 1, 0.5f, 1, 0.5f, 0);
    private ISmoothingStrategy<float> positionXSmoothingStrategy;
    private ISmoothingStrategy<float> positionYSmoothingStrategy;
    private ISmoothingStrategy<float> centerPositionXSmoothingStrategy;
    private ISmoothingStrategy<float> centerPositionYSmoothingStrategy;
    private readonly ISmoothingStrategy<float> orientationSmoothingStrategy;
    private readonly ISmoothingStrategy<float> ellipseMajorAxisSmoothingStrategy;
    private readonly ISmoothingStrategy<float> ellipseMinorAxisSmoothingStrategy;
    private readonly ISmoothingStrategy<float> widthSmoothingStrategy;
    private readonly ISmoothingStrategy<float> heightSmoothingStrategy;
    private readonly ISmoothingStrategy<TagData> tagDataSmoothingStrategy;
    private IntPtr capturedHwnd;
    private readonly SmoothingConfiguration smoothingConfiguration;

    public uint ContactId { get; private set; }

    public IntPtr CapturedHwnd
    {
      get => this.capturedHwnd;
      set
      {
        if (this.capturedHwnd != IntPtr.Zero)
        {
          this.InitialPosition = ContactRecord.TransformPointToNewWindowCoordinateSpace(this.InitialPosition, this.CapturedHwnd, value);
          this.LastTouchProperties.Position = ContactRecord.TransformPointToNewWindowCoordinateSpace(this.SmoothedPosition, this.CapturedHwnd, value);
          this.ResetPositionSmoothingStrategies();
          this.positionXSmoothingStrategy.AddSample(this.LastTouchProperties.Position.X);
          this.positionYSmoothingStrategy.AddSample(this.LastTouchProperties.Position.Y);
          this.LastTouchProperties.CenterPosition = ContactRecord.TransformPointToNewWindowCoordinateSpace(this.SmoothedCenter, this.CapturedHwnd, value);
          this.ResetCenterPositionSmoothingStrategies();
          this.centerPositionXSmoothingStrategy.AddSample(this.LastTouchProperties.CenterPosition.X);
          this.centerPositionYSmoothingStrategy.AddSample(this.LastTouchProperties.CenterPosition.Y);
          SizeF size = new SizeF(this.LastTouchProperties.Bounds.Width, this.LastTouchProperties.Bounds.Height);
          this.LastTouchProperties.Bounds = new TouchBounds(ContactRecord.TransformPointToNewWindowCoordinateSpace(new PointF(this.LastTouchProperties.Bounds.Left, this.LastTouchProperties.Bounds.Top), this.CapturedHwnd, value), size);
        }
        this.capturedHwnd = value;
        if (this.LastTouchProperties == null)
          return;
        this.LastTouchProperties.HwndTarget = value;
      }
    }

    public PointF InitialPosition { get; set; }

    public DateTime FirstSeen { get; set; }

    public DateTime LastSeen { get; set; }

    public bool LastTipSwitch { get; set; }

    public bool LastInRange { get; set; }

    public TouchProperties LastTouchProperties { get; set; }

    public bool SuppressEvents { get; set; }

    public PointF SmoothedPosition
    {
      get
      {
        return new PointF(this.positionXSmoothingStrategy.SmoothedValue, this.positionYSmoothingStrategy.SmoothedValue);
      }
    }

    public PointF SmoothedCenter
    {
      get
      {
        return new PointF(this.centerPositionXSmoothingStrategy.SmoothedValue, this.centerPositionYSmoothingStrategy.SmoothedValue);
      }
    }

    public float SmoothedEllipseMajorAxis => this.ellipseMajorAxisSmoothingStrategy.SmoothedValue;

    public float SmoothedEllipseMinorAxis => this.ellipseMinorAxisSmoothingStrategy.SmoothedValue;

    public SizeF SmoothedSize
    {
      get
      {
        return new SizeF(this.widthSmoothingStrategy.SmoothedValue, this.heightSmoothingStrategy.SmoothedValue);
      }
    }

    public float SmoothedOrientation => this.orientationSmoothingStrategy.SmoothedValue;

    public TagData SmoothedTagData => this.tagDataSmoothingStrategy.SmoothedValue;

    public ContactRecord(
      uint contactId,
      IntPtr capturedHwnd,
      TouchTypes touchTypes,
      bool isFromVip)
    {
      this.ContactId = contactId;
      this.CapturedHwnd = capturedHwnd;
      this.smoothingConfiguration = isFromVip ? ContactRecord.GetSmoothingConfiguration(touchTypes) : ContactRecord.nonVipSmoothingConfiguration;
      this.ResetPositionSmoothingStrategies();
      this.ResetCenterPositionSmoothingStrategies();
      this.orientationSmoothingStrategy = (ISmoothingStrategy<float>) new OrientationRunningAverageWithThresholdSmoothingStrategy(this.smoothingConfiguration.OrientationNumSamplesInAverage, this.smoothingConfiguration.OrientationThresholdPlusOrMinus);
      this.ellipseMajorAxisSmoothingStrategy = (ISmoothingStrategy<float>) new RunningAverageWithThresholdSmoothingStrategy(this.smoothingConfiguration.EllipseAxisNumSamplesInAverage, this.smoothingConfiguration.EllipseAxisThresholdPlusOrMinus);
      this.ellipseMinorAxisSmoothingStrategy = (ISmoothingStrategy<float>) new RunningAverageWithThresholdSmoothingStrategy(this.smoothingConfiguration.EllipseAxisNumSamplesInAverage, this.smoothingConfiguration.EllipseAxisThresholdPlusOrMinus);
      this.widthSmoothingStrategy = (ISmoothingStrategy<float>) new RunningAverageWithThresholdSmoothingStrategy(this.smoothingConfiguration.SizeNumSamplesInAverage, this.smoothingConfiguration.SizeThresholdPlusOrMinus);
      this.heightSmoothingStrategy = (ISmoothingStrategy<float>) new RunningAverageWithThresholdSmoothingStrategy(this.smoothingConfiguration.SizeNumSamplesInAverage, this.smoothingConfiguration.SizeThresholdPlusOrMinus);
      this.tagDataSmoothingStrategy = (ISmoothingStrategy<TagData>) new MostFrequentValueSmoothingStrategy<TagData>(this.smoothingConfiguration.TagDataNumSamplesRequired, TagData.None);
    }

    private void ResetPositionSmoothingStrategies()
    {
      this.positionXSmoothingStrategy = (ISmoothingStrategy<float>) new RunningAverageWithThresholdSmoothingStrategy(this.smoothingConfiguration.PositionNumSamplesInAverage, this.smoothingConfiguration.PositionThresholdPlusOrMinus);
      this.positionYSmoothingStrategy = (ISmoothingStrategy<float>) new RunningAverageWithThresholdSmoothingStrategy(this.smoothingConfiguration.PositionNumSamplesInAverage, this.smoothingConfiguration.PositionThresholdPlusOrMinus);
    }

    private void ResetCenterPositionSmoothingStrategies()
    {
      this.centerPositionXSmoothingStrategy = (ISmoothingStrategy<float>) new RunningAverageWithThresholdSmoothingStrategy(this.smoothingConfiguration.PositionNumSamplesInAverage, this.smoothingConfiguration.PositionThresholdPlusOrMinus);
      this.centerPositionYSmoothingStrategy = (ISmoothingStrategy<float>) new RunningAverageWithThresholdSmoothingStrategy(this.smoothingConfiguration.PositionNumSamplesInAverage, this.smoothingConfiguration.PositionThresholdPlusOrMinus);
    }

    public void AddDigitizerPositionSample(
      HidInputProvider.DeviceRecord deviceRecord,
      int x,
      int y)
    {
      PointF clientCoordinates = HidInputProvider.DigitizerToClientCoordinates(deviceRecord, this.CapturedHwnd, x, y);
      this.positionXSmoothingStrategy.AddSample(clientCoordinates.X);
      this.positionYSmoothingStrategy.AddSample(clientCoordinates.Y);
    }

    public void AddDigitizerCenterPositionSample(
      HidInputProvider.DeviceRecord deviceRecord,
      int x,
      int y)
    {
      PointF clientCoordinates = HidInputProvider.DigitizerToClientCoordinates(deviceRecord, this.CapturedHwnd, x, y);
      this.centerPositionXSmoothingStrategy.AddSample(clientCoordinates.X);
      this.centerPositionYSmoothingStrategy.AddSample(clientCoordinates.Y);
    }

    public void AddDigitizerOrientationSample(
      HidInputProvider.DeviceRecord deviceRecord,
      float digitizerOrientation)
    {
      switch ((int) deviceRecord.Orientation)
      {
        case 0:
          this.orientationSmoothingStrategy.AddSample(digitizerOrientation);
          break;
        case 1:
          this.orientationSmoothingStrategy.AddSample(digitizerOrientation - 1.57079637f);
          break;
        case 2:
          this.orientationSmoothingStrategy.AddSample(digitizerOrientation - 3.14159274f);
          break;
        case 3:
          this.orientationSmoothingStrategy.AddSample(digitizerOrientation - 4.712389f);
          break;
      }
    }

    public void AddDigitizerEllipseSample(
      HidInputProvider.DeviceRecord deviceRecord,
      int digitizerMajorAxis,
      int digitizerMinorAxis,
      float orientation)
    {
      double num1 = Math.Cos((double) orientation);
      double num2 = Math.Sin((double) orientation);
      float pixelAspectRatio = HidInputProvider.GetPixelAspectRatio(deviceRecord);
      float num3 = (float) digitizerMajorAxis * (float) num1;
      float num4 = (float) digitizerMajorAxis * (float) num2;
      this.ellipseMajorAxisSmoothingStrategy.AddSample(((VectorF) HidInputProvider.DigitizerToScreenSpace(deviceRecord, Math.Abs(num3), Math.Abs(num4 * pixelAspectRatio))).Length);
      float num5 = (float) digitizerMinorAxis * (float) -num2;
      float num6 = (float) digitizerMinorAxis * (float) num1;
      this.ellipseMinorAxisSmoothingStrategy.AddSample(((VectorF) HidInputProvider.DigitizerToScreenSpace(deviceRecord, Math.Abs(num5), Math.Abs(num6 * pixelAspectRatio))).Length);
    }

    public void AddDigitizerSizeSample(
      HidInputProvider.DeviceRecord deviceRecord,
      int digitizerWidth,
      int digitizerHeight)
    {
      SizeF screenSpace = HidInputProvider.DigitizerToScreenSpace(deviceRecord, digitizerWidth, digitizerHeight);
      this.widthSmoothingStrategy.AddSample(screenSpace.Width);
      this.heightSmoothingStrategy.AddSample(screenSpace.Height);
    }

    public void AddDigitizerTagDataSample(TagData tagData)
    {
      this.tagDataSmoothingStrategy.AddSample(tagData);
    }

    static ContactRecord() => ContactRecord.InitializeSmoothingConfigurations();

    private static SmoothingConfiguration GetSmoothingConfiguration(TouchTypes touchTypes)
    {
      if ((touchTypes & TouchTypes.Tag) != (TouchTypes) 0)
        return ContactRecord.tagSmoothingConfiguration;
      return (touchTypes & TouchTypes.Finger) != (TouchTypes) 0 ? ContactRecord.fingerSmoothingConfiguration : ContactRecord.blobSmoothingConfiguration;
    }

    private static void InitializeSmoothingConfigurations()
    {
      ContactRecord.InitializeSmoothingConfiguration(ContactRecord.GetSmoothingConfiguration(TouchTypes.Tag), "Tag");
      ContactRecord.InitializeSmoothingConfiguration(ContactRecord.GetSmoothingConfiguration(TouchTypes.Finger), "Finger");
      ContactRecord.InitializeSmoothingConfiguration(ContactRecord.GetSmoothingConfiguration(TouchTypes.Blob), "Blob");
    }

    private static void InitializeSmoothingConfiguration(
      SmoothingConfiguration smoothingConfiguration,
      string subkeyName)
    {
      using (RegistryKey registryKey1 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
      {
        if (registryKey1 == null)
          return;
        using (RegistryKey registryKey2 = registryKey1.OpenSubKey("SOFTWARE\\Microsoft\\Surface\\v2.0\\Input\\" + subkeyName))
        {
          if (registryKey2 == null)
            return;
          object obj1 = registryKey2.GetValue("PositionThresholdPlusOrMinus");
          if (obj1 != null)
            smoothingConfiguration.PositionThresholdPlusOrMinus = Convert.ToSingle(obj1, (IFormatProvider) CultureInfo.InvariantCulture);
          object obj2 = registryKey2.GetValue("PositionNumSamplesInAverage");
          if (obj2 != null)
            smoothingConfiguration.PositionNumSamplesInAverage = Convert.ToInt32(obj2, (IFormatProvider) CultureInfo.InvariantCulture);
          object obj3 = registryKey2.GetValue("OrientationThresholdPlusOrMinus");
          if (obj3 != null)
            smoothingConfiguration.OrientationThresholdPlusOrMinus = ContactRecord.ConvertDegreesToRadians(Convert.ToSingle(obj3, (IFormatProvider) CultureInfo.InvariantCulture));
          object obj4 = registryKey2.GetValue("OrientationNumSamplesInAverage");
          if (obj4 != null)
            smoothingConfiguration.OrientationNumSamplesInAverage = Convert.ToInt32(obj4, (IFormatProvider) CultureInfo.InvariantCulture);
          object obj5 = registryKey2.GetValue("EllipseAxisThresholdPlusOrMinus");
          if (obj5 != null)
            smoothingConfiguration.EllipseAxisThresholdPlusOrMinus = (float) Convert.ToInt32(obj5, (IFormatProvider) CultureInfo.InvariantCulture);
          object obj6 = registryKey2.GetValue("EllipseAxisNumSamplesInAverage");
          if (obj6 != null)
            smoothingConfiguration.EllipseAxisNumSamplesInAverage = Convert.ToInt32(obj6, (IFormatProvider) CultureInfo.InvariantCulture);
          object obj7 = registryKey2.GetValue("SizeThresholdPlusOrMinus");
          if (obj7 != null)
            smoothingConfiguration.SizeThresholdPlusOrMinus = Convert.ToSingle(obj7, (IFormatProvider) CultureInfo.InvariantCulture);
          object obj8 = registryKey2.GetValue("SizeNumSamplesInAverage");
          if (obj8 != null)
            smoothingConfiguration.SizeNumSamplesInAverage = Convert.ToInt32(obj8, (IFormatProvider) CultureInfo.InvariantCulture);
          object obj9 = registryKey2.GetValue("TagDataNumSamplesRequired");
          if (obj9 == null)
            return;
          smoothingConfiguration.TagDataNumSamplesRequired = Convert.ToInt32(obj9, (IFormatProvider) CultureInfo.InvariantCulture);
        }
      }
    }

    private static float ConvertDegreesToRadians(float degrees)
    {
      return (float) ((double) degrees * 3.1415927410125732 / 180.0);
    }

    private static PointF TransformPointToNewWindowCoordinateSpace(
      PointF originalPoint,
      IntPtr fromWindowHandle,
      IntPtr toWindowHandle)
    {
      try
      {
        float x = originalPoint.X;
        float y = originalPoint.Y;
        Microsoft.Surface.NativeMethods.ClientToScreen(fromWindowHandle, ref x, ref y);
        Microsoft.Surface.NativeMethods.ScreenToClient(toWindowHandle, ref x, ref y);
        return new PointF(x, y);
      }
      catch (InvalidOperationException ex)
      {
        return originalPoint;
      }
    }
  }
}
